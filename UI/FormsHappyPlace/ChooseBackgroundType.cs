using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AnalysisDefs;
using DetectorDefs;

namespace UI
{
    public partial class ChooseBackgroundType : Form
    {
        AcquireHandlers ah;
        
        public ChooseBackgroundType()
        { 
            InitializeComponent();
            // Generate an instance of the generic acquire dialog event handlers object (this now includes the AcquireParameters object used for change tracking)
            ah = new AcquireHandlers();
            ah.mo = AssaySelector.MeasurementOption.background;
            this.Text = "Detector Configuration for detector " + ah.det.Id.DetectorName;
            PassiveBackgroundButton.Checked = true;
            ah.ap.well_config = WellConfiguration.Passive;
        }

        private void ActiveBackgroundButton_CheckedChanged(object sender, EventArgs e)
        {
            ah.ap.well_config = ((RadioButton)sender).Checked ?WellConfiguration.Active:WellConfiguration.Passive;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
