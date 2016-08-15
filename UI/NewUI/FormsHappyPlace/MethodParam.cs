/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
using AnalysisDefs;
namespace NewUI
{
    using Integ = NCC.IntegrationHelpers;
    using N = NCC.CentralizedState;

    public class MethodParamFormFields
    {

        public MethodParamFormFields(AnalysisMethod am)
        {
            this.am = am;
        }

        public void RefreshCollarModeComboBox (ComboBox cb)
        {
            cb.Items.Clear();
            //foreach ()
        }
        public void RefreshMatTypeComboBox(ComboBox cb)
        {
            cb.Items.Clear();
            foreach (INCCDB.Descriptor d in N.App.DB.Materials.GetList())
            {
                cb.Items.Add(d.Name);
            }
			if (N.App.DB.Materials.Has(acq.item_type))  // avoid case-insensitive mis-match by using the Name in the Materials list against that from the DB Acquire instance
			{
				INCCDB.Descriptor d = N.App.DB.Materials.Get(acq.item_type);
				cb.SelectedItem = d.Name;
			}
        }
        public void RefreshCurveEqComboBox(ComboBox cb)
        {
            RefreshCurveEqComboBox(cb, cev);
        }
        public void RefreshCurveEqComboBox(ComboBox cb, INCCAnalysisParams.CurveEquationVals v)
        {
            cb.Items.Clear();
            foreach (INCCAnalysisParams.CurveEquation cs in System.Enum.GetValues(typeof(INCCAnalysisParams.CurveEquation)))
            {
                //Per Martyn, use equation string, not named value.  7/17/2014 HN
                cb.Items.Add(cs.ToDisplayString());                
            }
            cb.Refresh();
            cb.SelectedIndex = cb.FindString (v.cal_curve_equation.ToDisplayString());
        }
        public bool HasMethod
        {
            get { return ams != null && ams.HasMethod(am); }
        }
        
        public void SelectMaterialType(ComboBox mtcb)
        {
            acq.item_type = String.Copy((string)(mtcb.SelectedItem));
            ams = Integ.GetMethodSelections(acq.detector_id, acq.item_type); // unfinished, test and firm up
        }

        public void SelectMaterialType(string mt)
        {
            acq.item_type = String.Copy(mt);
            ams = Integ.GetMethodSelections(acq.detector_id, acq.item_type); // unfinished, test and firm up
        }
        public void ATextBox_Leave(TextBox t)
        {
            Double d = cev.a;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { cev.a = d; imd.modified = true; }
            t.Text = cev.a.ToString("E6");
        }
        public void BTextBox_Leave(TextBox t)
        {
            Double d = cev.b;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { cev.b = d; imd.modified = true; }
            t.Text = cev.b.ToString("E6");
        }
        public void CTextBox_Leave(TextBox t)
        {
            Double d = cev.c;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { cev.c = d; imd.modified = true; }
            t.Text = cev.c.ToString("E6");
        }
        public void DTextBox_Leave(TextBox t)
        {
            Double d = cev.d;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { cev.d = d; imd.modified = true; }
            t.Text = cev.d.ToString("E6");
        }

        public void VarianceATextBox_Leave(TextBox t)
        {
            Double d = cev.var_a;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { cev.var_a = d; imd.modified = true; }
            t.Text = cev.var_a.ToString("E6");
        }
        public void DateTimePicker_Leave (DateTimePicker t, ref DateTime dt)
        {
            DateTime dt1 = t.Value;
            bool modified = dt1.CompareTo (dt) == 0;
            if (modified) { dt = dt1; imd.modified = true; }
            t.Value = dt1;
        }
        public void VarianceBTextBox_Leave(TextBox t)
        {
            Double d = cev.var_b;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { cev.var_b = d; imd.modified = true; }
            t.Text = cev.var_b.ToString("E6");
        }
        public void VarianceCTextBox_Leave(TextBox t)
        {
            Double d = cev.var_c;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { cev.var_c = d; imd.modified = true; }
            t.Text = cev.var_c.ToString("E6");
        }
        public void VarianceDTextBox_Leave(TextBox t)
        {
            Double d = cev.var_d;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { cev.var_d = d; imd.modified = true; }
            t.Text = cev.var_d.ToString("E6");
        }

        public void CovariancePlonk(TextBox t, Coeff x, Coeff y)
        {
            Double d = cev.covar(x, y);
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { cev.setcovar(x, y, d); imd.modified = true; }// a,b,c,d - 0,1,2,3
            t.Text = cev.covar(x, y).ToString("E6");
        }

        public void CovarianceABTextBox_Leave(TextBox t)
        {
            CovariancePlonk(t, Coeff.a, Coeff.b);
        }
        public void CovarianceACTextBox_Leave(TextBox t)
        {
            CovariancePlonk(t, Coeff.a, Coeff.c);
        }
        public void CovarianceADTextBox_Leave(TextBox t)
        {
            CovariancePlonk(t, Coeff.a, Coeff.d);
        }
        public void CovarianceBCTextBox_Leave(TextBox t)
        {
            CovariancePlonk(t, Coeff.b, Coeff.c);
        }
        public void CovarianceBDTextBox_Leave(TextBox t)
        {
            CovariancePlonk(t, Coeff.b, Coeff.d);
        }
        public void CovarianceCDTextBox_Leave(TextBox t)
        {
            CovariancePlonk(t, Coeff.c, Coeff.d);
        }

        public void SigmaXTextBox_Leave(TextBox t)
        {
            Double d = cev.sigma_x;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { cev.sigma_x = d; imd.modified = true; }
            t.Text = cev.sigma_x.ToString("E6");
        }

        public void LowerMassLimitTextBox_Leave(TextBox t)
        {
            Double d = cev.lower_mass_limit;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { cev.lower_mass_limit = d; imd.modified = true; }
            t.Text = cev.lower_mass_limit.ToString("N3");
        }

        public void UpperMassLimitTextBox_Leave(TextBox t)
        {
            Double d = cev.upper_mass_limit;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { cev.upper_mass_limit = d; imd.modified = true; }
            t.Text = cev.upper_mass_limit.ToString("N3");
        }

        public Detector det;
        public AcquireParameters acq = null;
        public INCCAnalysisParams.CurveEquationVals cev;
        public INCCAnalysisParams.INCCMethodDescriptor imd;
        public AnalysisMethods ams;
        public AnalysisMethod am;


        public void CheckBox_Leave(CheckBox c, ref bool selected)
        {
            bool select = c.Checked;
            bool modified = select == selected;
            if (modified) { selected = select; }
        }

        public void DblTextBox_Leave(TextBox t, ref double val)
        {
            Double d = val;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { val = d; imd.modified = true; }
            t.Text = val.ToString("E6");
        }

        public void UShortTextBox_Leave(TextBox t, ref ushort val)
        {
            uint us = val;
            bool modified = (Format.ToUInt(t.Text, ref us));
            if (modified) { val = (ushort) us; imd.modified = true; }
            t.Text = val.ToString("E6");
        }

        public void PctTextBox_Leave(TextBox t, ref double val)
        {
            Double d = val;
            bool modified = (Format.ToPct(t.Text, ref d));
            if (modified) { val = d; imd.modified = true; }
            t.Text = val.ToString("E3");
        }

        public void Persist()
        {
            if (imd.modified)
            {
                if (ams == null) // HN No analysis methods existed.
                {
                    ams = new AnalysisMethods();
                    ams.AddMethod(am, imd);
                }
                if (ams.HasMethod(am)) // if found, update the existing parameter values
                {
                    INCCAnalysisParams.INCCMethodDescriptor c = ams.GetMethodParameters(am);
                    imd.CopyTo(c);  // This a virtual so imd can be the primary type
                }
                else // add the new method params under the current method key
                {
                    ams.AddMethod(am, imd);
                }
                INCCSelector sel = new INCCSelector(acq.detector_id, acq.item_type);
                N.App.DB.UpdateAnalysisMethod(sel, ams);  // flush changes on internal map to the DB
            }
        }
    }
}
