/*
Copyright (c) 2014, Los Alamos National Security, LLC
All rights reserved.
Copyright 2014. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, 
LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  
NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, 
OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, 
such modified software should be clearly marked, so as not to confuse it with the version available from LANL.

Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided 
that the following conditions are met:
•	Redistributions of source code must retain the above copyright notice, this list of conditions and the following 
disclaimer. 
•	Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
disclaimer in the documentation and/or other materials provided with the distribution. 
•	Neither the name of Los Alamos National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, 
nor the names of its contributors may be used to endorse or promote products derived from this software without specific 
prior written permission. 
THIS SOFTWARE IS PROVIDED BY LOS ALAMOS NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED 
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL LOS ALAMOS NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING 
IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NCCReporter;

namespace LMDAQServer
{
    /// Based on example from 
    /// http://msdn2.microsoft.com/en-us/library/system.net.sockets.socketasynceventargs.aspx
    /// Implements the connection logic for the socket server
    /// And the Client sockets send and receive functions.
    ///  [CriticalRecoveryPoint("Improve this entire class by detecting and recovering from socket disconnects and other failures");
    public sealed class Server
    {
        #region Fields

        /// Represents a large reusable set of buffers for all socket operations.       
        private BufferPool bufferManager;

        /// The socket used to listen for incoming connection requests.       
        private Socket listenerSocket;

        // arguments for listener socket
        private SocketAsyncEventArgs listenerEventArgs;

        /// Mutex to synchronize server execution.
        private static SemaphoreSlim onereader;

        /// The total number of clients connected to the server.        
        private Int32 numConnectedSockets;

        /// the maximum number of connections the sample is designed to handle simultaneously.       
        private Int32 maxNumConnections;

        /// Read,  (don't alloc buffer space for accepts)
        /// for this program only reading is high speed.  Writting does not need this space.
        private const Int32 buffersPerSocket = 1;

        /// Pool of reusable SocketAsyncEventArgs objects for write, read and accept socket operations.       
        private SocketAsyncEventArgsPool readWritePool;

        /// Controls the total number of clients connected to the server.       
        private SemaphoreSlim semaphoreAcceptedClients;

        /// Server port.      
        private Int32 port;
        private IPAddress subnet;
        private Byte[] subnetbytes;

        private LMLoggers.LognLM logger;

        public LMLoggers.LognLM Logger
        {
            get { return logger; }
            set { logger = value; }
        }
        #endregion

        #region Events
        /// <summary>
        /// Fired when a new connection is received.
        /// </summary>
        public event EventHandler<SocketAsyncEventArgs> clientConnected;
        public event EventHandler<SocketAsyncEventArgs> DataSent;
        public event EventHandler<SocketAsyncEventArgs> DataReceived;
        public event EventHandler<SocketAsyncEventArgs> ClientDisconnected;
        #endregion

        public struct ServerEventArgs// tdb:: can I use this???
        {
            public object ID;
            public Socket socket;
            public ServerEventArgs(object P1, Socket P2)
            {
                ID = P1;
                socket = P2;
            }
        }

        #region Constructors
        /// Create a server instance.  
        /// To start the server listening for connection requests
        /// call the Init method followed by Start method.
        /// <param name="numConnections">Maximum number of connections to be handled simultaneously.</param>
        /// <param name="receiveBufferSize">Buffer size to use for each socket I/O operation.</param>
        public Server(Int32 port, Int32 numConnections, Int32 receiveBufferSize, IPAddress subnet)
        {
            // Allocate buffers such that the maximum number of sockets can have one outstanding read and 
            // write posted to the socket simultaneously .
            //When the event is signaled, the application uses the SocketAsyncEventArgs object 
            //parameter to obtain the status of the completed asynchronous socket operation.
            this.port = port;
            this.numConnectedSockets = 0;
            this.maxNumConnections = numConnections;
            this.bufferManager = new BufferPool(receiveBufferSize * maxNumConnections * buffersPerSocket, receiveBufferSize);
            this.readWritePool = new SocketAsyncEventArgsPool(numConnections);
            this.semaphoreAcceptedClients = new SemaphoreSlim(numConnections, numConnections);
            listenerEventArgs = new SocketAsyncEventArgs();
            listenerEventArgs.UserToken = port; // TBD RR: use this !!!!
            listenerEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(FinishClientAccept);
            this.subnet = subnet;
            this.subnetbytes = subnet.GetAddressBytes();
        }
        #endregion

        #region Public Methods
        /// <summary>Starts the server such that it is listening for incoming connection requests.  </summary>          
        public void Start()
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("The LM DAQ Server is already running");
            }


            if (readWritePool.Count == 0)
            {
                // Initializes the manager by preallocating reusable buffers and 
                // context objects.  These objects do not need to be preallocated 
                // or reused, but it is done this way to illustrate how the API can 
                // easily be used to create reusable objects to increase server performance. 
                // Preallocate pool of SocketAsyncEventArgs objects.
                SocketAsyncEventArgs readWriteEventArg;
                // These big buffers are for the recieved data
                for (Int32 i = 0; i < this.maxNumConnections; i++)
                {
                    // Preallocate a set of reusable SocketAsyncEventArgs.
                    readWriteEventArg = new SocketAsyncEventArgs();
                    readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);

                    // Assign a Byte buffer from the buffer pool to the SocketAsyncEventArg object.
                    this.bufferManager.AssignBuffer(readWriteEventArg);

                    // Add SocketAsyncEventArg to the pool.
                    this.readWritePool.Push(readWriteEventArg);
                }
            }

            string baa = Connect();

            if (listenerSocket == null)
            {
                logger.TraceEvent(LogLevels.Warning, 204, "LM DAQ server cannot continue without a socket listener");
                return;
            }
            if (!listenerSocket.IsBound)
            {
                logger.TraceEvent(LogLevels.Warning, 205, "LM DAQ server cannot continue without a bound connection to " + baa);
                return;
            }
            // Start the server with a listen backlog of MaxNumConnections.
            listenerSocket.Listen(maxNumConnections);

            if (onereader == null)
                onereader = new SemaphoreSlim(1, 1);
            else
            {
                int count = onereader.CurrentCount;
                if (count == 0)
                {
                    onereader.Release();
                }
            }

            // Begin accepting on the listening socket.
            this.BeginClientAccept();

            onereader.Wait();
        }

        public string Connect()
        {
            // Get host related information.
            IPEndPoint localEndPoint;
            IPAddress[] al = Dns.GetHostEntry(Environment.MachineName).AddressList;
            byte[] lhb = new byte[] { 127, 0, 0, 1 };  // add localhost as a fall-back, prollly oughta check here
            IPAddress lh = new IPAddress(lhb);
            IPAddress[] addressList = new IPAddress[al.Length + 1];
            al.CopyTo(addressList, 0);
            addressList[al.Length] = lh;

            // Get endpoint for the listener.
            // Search through the IP list for the Server machine.
            // Find the entry that matches the IP domain of the Instrument network.
            // The NPOD is currently ???.???.x.y.  This machine must have an IP
            // address in this range.  Use that one.
            logger.TraceEvent(LogLevels.Verbose, 203, "Looking on " + subnet.ToString());
            string baa = "";

            foreach (IPAddress curAdd in addressList)
            {
                Byte[] bytes = curAdd.GetAddressBytes();
                if (bytes[0] == subnetbytes[0] && bytes[1] == subnetbytes[1]) // we found the proper address
                {
                    try
                    {
                        // Create the socket which listens for incoming connections.
                        baa = curAdd.ToString();
                        logger.TraceEvent(LogLevels.Verbose, 201, "Binding to " + baa);
                        localEndPoint = new IPEndPoint(curAdd, port);
                        if (listenerSocket != null)
                        {
                            listenerSocket.Close();
                        }
                        else
                        {
                            listenerSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                            // Associate the socket with the local endpoint.
                        }
                        listenerSocket.Bind(localEndPoint);
                        break;
                    }
                    catch (Exception e)
                    {
                        logger.TraceException(e, true);
                    }
                }
                else
                {
                    logger.TraceEvent(LogLevels.Verbose, 202, "Skipping " + curAdd.ToString());
                }
            }
            return baa;

        }

        /// Stop the server.       
        public void Stop()
        {
            if (listenerSocket != null)
                listenerSocket.Close();
            if (onereader != null && onereader.CurrentCount == 0)
                onereader.Release(); // tbd RR: exit linux and then disconnect, connect?? .....
        }

        /// Stop the server.       
        public void StopClient(SocketAsyncEventArgs e)
        {
            this.CloseClientSocket(e);
        }

        /// <summary>
        /// Is the class currently listening.
        /// </summary>
        public Boolean IsRunning
        {
            get { return listenerSocket != null && listenerSocket.IsBound; }
        }
        #endregion

        #region Private Methods
        /// Begins an operation to accept a connection request from the client.        
        /// <param name="acceptEventArg">The context object to use when issuing 
        /// the accept operation on the server's listening socket.</param>
        private void BeginClientAccept()
        {
            // AcceptSocket must be cleared since the context object is being reused.
            // the accept socket is the socket that the connection is accepted on.
            // It will be different from the listening socket.
            listenerEventArgs.AcceptSocket = null;

            this.semaphoreAcceptedClients.Wait();

            // BLOCK: here is where we wait for a connection.
            // if it returns syncronously(false), we have to call the finish here.
            if (!this.listenerSocket.AcceptAsync(listenerEventArgs))
            {
                this.FinishClientAccept(this, listenerEventArgs);
            }
        }

        /// Callback method associated with Socket.AcceptAsync 
        /// operations and is invoked when an accept operation is complete.        
        /// <param name="sender">Object who raised the event.</param>
        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>
        private void FinishClientAccept(object sender, SocketAsyncEventArgs e)
        {
            if (e.AcceptSocket.RemoteEndPoint == null)
                return;

            // Tell the UI layer that we have a new connection
            if (clientConnected != null)  // this might not be set in the calling class
            {
                // make a new event args for the UI to use for sending
                SocketAsyncEventArgs UIEventArgs = new SocketAsyncEventArgs();
                UIEventArgs.UserToken = e.AcceptSocket;
                UIEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);

                clientConnected(this, UIEventArgs);
            }

            // Get the socket for the accepted client connection and put it into the
            // ReadEventArg object user token.
            SocketAsyncEventArgs receiveEventArgs = this.readWritePool.Pop();
            receiveEventArgs.UserToken = e.AcceptSocket;

            // Now that the client is connected, post a receive to the connection.
            if (!e.AcceptSocket.ReceiveAsync(receiveEventArgs))
            {
                this.FinishReceive(receiveEventArgs);
            }

            // Accept the next client connection request.
            this.BeginClientAccept();
        }

        /// Close the socket associated with the client.        
        /// <param name="e">SocketAsyncEventArg associated with the client.</param>
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            Socket s = e.UserToken as Socket;

            try
            {
                s.Shutdown(SocketShutdown.Send);
            }
            catch (Exception ex)
            {
                // Throws if client process has already closed.
                logger.TraceException(ex);
            }

            s.Close();

            // Decrement the counter keeping track of the total number of clients connected to the server.
            this.semaphoreAcceptedClients.Release();

            Interlocked.Decrement(ref this.numConnectedSockets);

            if (ClientDisconnected != null)
            {
                ClientDisconnected(this, e);
            }

            // Free the SocketAsyncEventArg so they can be reused by another client.
            this.readWritePool.Push(e);
        }

        /// <summary>
        /// Sends data to a client who is the remote end of this socket.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="offset">The offset into the data.</param>
        /// <param name="count">The ammount of data to send.</param>
        public void SendData(string message, SocketAsyncEventArgs e)
        {
            // Format the data to send to the client.
            Byte[] sendBuffer = Encoding.ASCII.GetBytes(message);

            if (e == null) // state machine error, emit message and leave
            {
                logger.TraceEvent(LogLevels.Warning, 203, "Empty socket, cannot send LMMM command " + message);
                return;
            }

            e.SetBuffer(sendBuffer, 0, sendBuffer.Length);
            Socket s = e.UserToken as Socket;
            try
            {
                if (!s.SendAsync(e))
                {
                    this.FinishSend(e);
                }
            }
            catch (System.ObjectDisposedException)
            {
                throw;
            }
            catch (System.InvalidOperationException)
            {
                throw;
            }
        }

        /// Callback called whenever a receive or send operation is completed on a socket.        
        /// <param name="sender">Object who raised the event.</param>
        /// <param name="e">SocketAsyncEventArg associated with the completed send/receive operation.</param>
        private void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            // Determine which type of operation just completed and call the associated handler.
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    this.FinishReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    this.FinishSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation (" + e.LastOperation + ") completed on the socket was not a receive or send");
            }
        }

        /// This method is invoked when an asynchronous receive operation completes. 
        /// If the remote host closed the connection, then the socket is closed.  
        /// If data was received then the data is echoed back to the client.        
        /// <param name="e">SocketAsyncEventArg associated with the completed receive operation.</param>
        private void FinishReceive(SocketAsyncEventArgs e)
        {
            try
            {
                // Check if the remote host closed the connection.
                if (e.BytesTransferred > 0)
                {
                    if (e.SocketError == SocketError.Success)
                    {
                        // Return data to instrument layer
                        if (DataReceived != null)
                        {
                            DataReceived(this, e);
                        }

                        // Re-arm the read
                        Socket s = e.UserToken as Socket;
                        if (!s.ReceiveAsync(e))
                        {
                            this.FinishReceive(e);
                        }
                    }
                    else
                    {
                        this.CloseClientSocket(e);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex, true);
            }
        }

        private void FinishSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                // what to do?????
                // Return finished state to calling layer
                if (DataSent != null)
                {
                    DataSent(this, e);
                }
            }
            else
            {
                this.CloseClientSocket(e);
            }
        }
        #endregion
    }
}
