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
using System.Windows.Forms;

namespace NewUI
{
    public partial class IDDReviewBackground : Form
    {
        public IDDReviewBackground()
        {
            InitializeComponent();
        }


        private void OKBtn_Click(object sender, EventArgs e)
        {
            IDDMeasurementList measlist = new IDDMeasurementList("Background");
            measlist.ShowDialog();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, ".\\inccuser.chm"/*, HelpNavigator.Topic, "/WordDocuments/selectpu240ecoefficients.htm"*/);
        }

        private void DetectorParametersCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void IndividualCycleRawDataCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void IndividualCycleRateDataCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SummedRawCoincidenceDataCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SummedMultiplicityDistributionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void IndividualCycleMultiplicityDistributionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void PrintTextCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void DisplayResultsInTextRadioButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void PlotSinglesDoublesTriplesRadioButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void IDDReviewBackground_Load(object sender, EventArgs e)
        {
            ToolTip disclaimer = new ToolTip();
            disclaimer.AutoPopDelay = 5000;
            disclaimer.InitialDelay = 1000;
            disclaimer.ReshowDelay = 2000;
            disclaimer.ShowAlways = true;
            disclaimer.SetToolTip(this.OKBtn, "Current INCC cannot customize reports. \r\nYou will be shown a list of background measurements and \r\nthe report will be displayed as it was originally written.");
            disclaimer.SetToolTip(this.HelpBtn, "Current INCC cannot customize reports. \r\nYou will be shown a list of background measurements and \r\nthe report will be displayed as it was originally written.");
        }


    }
}
