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
using System.Windows;
using System.Windows.Controls;
using AnalysisDefs;
using DetectorDefs;

namespace UI
{

    /// <summary>
    /// Interaction logic for MeasSetup.xaml
    /// </summary>
    public partial class MeasSetup : Window
    {

        public ShiftRegisterParameters sr;

        public MeasSetup()
        {
            InitializeComponent();
            Detector d = (Detector)detectors.SelectedItem;
            if (d == null)
                sr = new ShiftRegisterParameters();
            else
                sr = new ShiftRegisterParameters(d.SRParams);
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        public int InitialSelection
        {
            get; set;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void detectors_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Detector d = (Detector)detectors.SelectedItem;
            sr = new ShiftRegisterParameters(d.SRParams);
            int idx = -1, i = 0;
            foreach (object o in this.TypeCombo.Items)
            {               
                if (o.ToString().CompareTo( d.Id.SRType.ToString()) == 0)
                {
                    idx = i;
                    break;
                }
                i++;
            }
            //int idx = this.TypeCombo.Items( d.Id.SRType.ToString());
            this.TypeCombo.SelectedIndex = idx;

            this.predelay.Text = sr.predelayMS.ToString();
            this.dieaway.Text = sr.dieAwayTimeMS.ToString();
            this.MDTA.Text = sr.deadTimeCoefficientAinMicroSecs.ToString();
            this.MDTB.Text = sr.deadTimeCoefficientBinPicoSecs.ToString();
            this.MDTC.Text = sr.deadTimeCoefficientCinNanoSecs.ToString();
            this.mdeadtime.Text = sr.deadTimeCoefficientMultiplicityinNanoSecs.ToString();
            this.DoublesGateFrac.Text = sr.doublesGateFraction.ToString();
            this.TriplesGateFrac.Text = sr.triplesGateFraction.ToString();
            this.efficiency.Text = sr.efficiency.ToString();
            this.hv.Text = sr.highVoltage.ToString();
            this.gatelength.Text = sr.gateLengthMS.ToString();
            if (d.Id.SRType < InstrType.NPOD)
            {
                COMvNetLabel.Content = "Serial Port";
                string cur = "COM" + d.Id.SerialPort.ToString();
                idx = -1; i = 0;
                string[] ports = System.IO.Ports.SerialPort.GetPortNames();
                CommComboBox.Items.Clear();
                foreach (string p in ports)
                {
                    CommComboBox.Items.Add(p);
                    if (p.CompareTo(cur) == 0)
                    {
                        idx = i;
                    }
                    i++;
                }
                if (idx >= 0)
                    CommComboBox.SelectedIndex = idx;
                else
                {
                    CommComboBox.Items.Add(cur);
                    CommComboBox.SelectedIndex = 0;
                }

            }
            else if (d.Id.SRType < InstrType.MCNPX)
            {
                COMvNetLabel.Content = "TCP/IP Addr";
            }
            else
            {
                COMvNetLabel.Content = "Whah?";
            }
        }

        private void okButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (sr.modified)
            {
            }
            this.DialogResult = true;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            try
            {
                detectors.DataContext = NCC.CentralizedState.App;
                TypeCombo.DataContext = NCC.CentralizedState.App;
            }
            catch (Exception)
            {
            }

        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            this.detectors.SelectedIndex = InitialSelection;
        }

        private void predelay_LostFocus(object sender, RoutedEventArgs e)
        {
            double d = sr.predelayMS;
            if (NNCheck(predelay, ref d))
            {
                sr.predelayMS = d;
                sr.modified = true;
            }
        }

        private void gatelength_LostFocus(object sender, RoutedEventArgs e)
        {
            double d = sr.gateLengthMS;
            if (NNCheck(gatelength, ref d))
            {
                sr.gateLengthMS = d;
                sr.modified = true;
            }
        }
        private void hv_LostFocus(object sender, RoutedEventArgs e)
        {
            NNCheck(hv, ref sr.highVoltage);
        }

        private void dieaway_LostFocus(object sender, RoutedEventArgs e)
        {
            double d = sr.dieAwayTimeMS;
            if (NNCheck(dieaway, ref d))
            {
                sr.dieAwayTimeMS = d;
                sr.modified = true;
            }
        }

        private void efficiency_LostFocus(object sender, RoutedEventArgs e)
        {
            NNCheck(efficiency, ref sr.efficiency);
        }

        private void mdeadtime_LostFocus(object sender, RoutedEventArgs e)
        {
            DblCheck(mdeadtime, ref sr.deadTimeCoefficientTinNanoSecs);
        }

        private void MDTA_LostFocus(object sender, RoutedEventArgs e)
        {
            DblCheck(MDTA, ref sr.deadTimeCoefficientAinMicroSecs);
        }

        private void MDTB_LostFocus(object sender, RoutedEventArgs e)
        {
            DblCheck(MDTB, ref sr.deadTimeCoefficientBinPicoSecs);
        }

        private void MDTC_LostFocus(object sender, RoutedEventArgs e)
        {
            DblCheck(MDTC, ref sr.deadTimeCoefficientCinNanoSecs);
        }

        private void DoublesGateFrac_LostFocus(object sender, RoutedEventArgs e)
        {
            NZCheck(DoublesGateFrac, ref sr.doublesGateFraction);
        }

        private void TriplesGateFrac_LostFocus(object sender, RoutedEventArgs e)
        {
            NZCheck(TriplesGateFrac, ref sr.triplesGateFraction);
        }
    
        private void DblCheck(TextBox sender, ref double foo)
        {
            Double NewValue;
            if (Double.TryParse(sender.Text, out NewValue))
            {
                if (foo != NewValue)
                {
                    foo = NewValue;
                    sr.modified = true;
                }
            }
            sender.Text = foo.ToString();
        }

        private void NZCheck(TextBox sender, ref double foo)
        {
            sr.modified |= Format.ToPNZ(sender.Text, ref foo);
            sender.Text = foo.ToString();
        }
        private bool NNCheck(TextBox sender, ref double foo)
        {
            bool b = Format.ToNN(sender.Text, ref foo);
            sender.Text = foo.ToString();
            sr.modified |= b;
            return b;
        }
    }
}
