using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LMSR
{
    public class UMSRLib
    {
        SerialPort port = null;
        Socket sock = null;
        enum ConnectionType { NONE, SERIAL, ETHERNET};
        ConnectionType which = ConnectionType.NONE;
        public string error { set; get; }

        public UMSRLib()
        {

        }
        ~UMSRLib()
        {
            if (port != null && port.IsOpen)
            {
                port.Close();
                port = null;
            }
            if (sock !=null &&sock.Connected)
            {
                sock.Disconnect(false);
                sock = null;
            }
        }
        public int Open(SerialPort p, IPEndPoint ip)
        {
            bool success = false;
            try
            {
                if (p != null)
                {
                    port = p;
                    port.Open();
                    success = port.IsOpen;
                    which = ConnectionType.SERIAL;
                    return !success ? sr_h.SR_NOTOPEN : sr_h.SR_SUCCESS;
                }
                else if (ip != null)
                {
                    sock = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    sock.Connect(ip.Address, ip.Port);
                    which = ConnectionType.ETHERNET;
                    return !sock.Connected ? sr_h.SR_NOTOPEN : sr_h.SR_SUCCESS;
                }
                else
                    return sr_h.SR_NOTOPEN;
            }
            catch (UnauthorizedAccessException uae)
            {
                error = uae.Message;
                return sr_h.SR_NOTOPEN;
            }
            catch (ArgumentNullException ane)
            {
                error = ane.Message;
                return sr_h.SR_NOTOPEN;
            }
            catch (ArgumentOutOfRangeException aore)
            {
                error = aore.Message;
                return sr_h.SR_NOTOPEN;
            }
            catch (NotSupportedException nse)
            {
                error = nse.Message;
                return sr_h.SR_NOTOPEN;
            }
            catch (ArgumentException ae)
            {
                error = ae.Message;
                return sr_h.SR_NOTOPEN;
            }
            catch (System.IO.IOException sioe)
            {
                error = sioe.Message;
                return sr_h.SR_NOTOPEN;
            }
            catch (InvalidOperationException ioe)
            {
                error = ioe.Message;
                return sr_h.SR_NOTOPEN;
            }
            catch (SocketException se)
            {
                error = se.Message;
                return sr_h.SR_NOTOPEN;
            }
        }
        public int Close()
        {
            bool success = false;
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                    port = null;
                    success = !port.IsOpen;
                    return success ? sr_h.SR_NOTOPEN : sr_h.SR_SUCCESS;
                }
                else if (sock.Connected)
                {
                    sock.Disconnect(false);
                    sock = null;
                    return sock.Connected ? sr_h.SR_SUCCESS : sr_h.SR_S_FAULT;
                }
                else
                    return sr_h.SR_NOTOPEN;
            }
            catch (InvalidOperationException ioe)
            {
                error = ioe.Message;
                return sr_h.SR_S_FAULT;
            }
            catch (PlatformNotSupportedException pnse)
            {
                error = pnse.Message;
                return sr_h.SR_S_FAULT;
            }
            catch (SocketException se)
            {
                error = se.Message;
                return sr_h.SR_NOTOPEN;
            }
        }
        public int Control(ushort wCommand, object pvArg)
        {
            /*
             
            */
            try
            {
                if (which == ConnectionType.SERIAL)
                    port.Write(wCommand.ToString());
                else
                    sock.Send(Encoding.ASCII.GetBytes(wCommand.ToString()), wCommand.ToString().ToCharArray().Length, SocketFlags.None);

                return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public bool SetParms(sr_h.sr_parms sr_parms)
        {
            /*
             Set HV = WW xx
             */ 
            bool success = false;
            try
            {
                if (which == ConnectionType.SERIAL)
                {
                    port.Write(String.Format("WW {0}", sr_parms.hv));
                }
                return success;
            }
            catch
            {
                return false;
            }

        }
        public int Control(int iPort, ushort wCommand, ref byte sr_status)
        {
            return 0;
        }
        public int Control(int iPort, ushort wCommand, ref sr_h.sr_rdout pvArg)
        {
            return 0;
        }
        public int Control(int iPort, ushort wCommand, ref sr_h.sr_mult_rdout pvArg)
        {
            return 0;
        }
        public int Control(int iPort, ushort wCommand, ref sr_h.sr_mult_rdout2 pvArg)
        {
            return 0;
        }
        public double Get_mult_time()
        {
            return 0;
        }
        public int Ioctl(int iPort, ushort wRequest, ref double pvArg)
        {
            return 0;
        }
        public int Is_timeout(ref Com_h.com_timeout pto)
        {
            return 0;
        }
        public void Set_mult_timeout(double dDuration)
        {
        }
        public void Set_timeout(ref Com_h.com_timeout pto, double duration)
        {
        }
    }
}
