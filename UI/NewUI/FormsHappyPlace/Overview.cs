/*
This source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

This source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2016, International Atomic Energy Agency (IAEA), IAEA.org
   Authored by J. Longo

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS"AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AnalysisDefs;
using DetectorDefs;
namespace NewUI
{
	using N = NCC.CentralizedState;

	public partial class Overview : Form
	{
		void LazyChild(TreeNode tn, long count)
		{
			if (count > 0)
			{
				TreeNode n = new TreeNode(tn.Name + " " + count.ToString());
				n.Tag = true;  // signal that it needs to be evaluated and populated
				tn.Nodes.Add(n);
			}
		}

		void LoadDetectors(bool uneval = false)
		{
			TreeNode tn = treeView1.Nodes["Detectors"];
			tn.Tag = typeof(Detector);
			if (uneval)
				LazyChild(tn, N.App.DB.Detectors.Count);
			else
				foreach (Detector d in N.App.DB.Detectors)
				{
					TreeNode n = tn.Nodes.Add(d.Id.DetectorName, d.Id.DetectorName);
					if (d.Id.DetectorId == curdet.Id.DetectorId)
						n.NodeFont = new Font(treeView1.Font, FontStyle.Bold);
					n.Tag = d;
					LazyChild(n, 1);
				}
		}

		void LoadDetector(TreeNode tn)
		{
			Detector d = (Detector)tn.Tag;
			TreeNode n = tn.Nodes.Add("mult", GenDetMultLabelStr(d.MultiplicityParams));
			n.Tag = d.Item2;
			n = tn.Nodes.Add("AB", GenDetABLabelStr(d.AB));
			n.Tag = d.Item3;
			if (d.ListMode)
			{
				n = tn.Nodes.Add("counters", "TBD");
				n.Tag = d.Item3;
			}
			long count = N.App.DB.GetMeasurementCount(d.Id.DetectorId);
			// string s = string.Format("{0} measurements", count);
			n = tn.Nodes.Add("Measurements ", GenDetMeasLabelStr(count));
			n.Tag = true;
			LazyChild(n, count);
		}

		void LoadCollarItems(bool uneval = false)
		{
			TreeNode tn = treeView1.Nodes["Collar Items"];
			tn.Tag = typeof(CollarItemId);
			if (uneval)
				LazyChild(tn, N.App.DB.CollarItemIds.GetList().Count);
			else
				foreach (CollarItemId d in N.App.DB.CollarItemIds.GetList())
				{
					TreeNode n = tn.Nodes.Add(d.item_id, d.item_id);
					n.Tag = d;
				}
		}

		void LoadItems(bool uneval = false)
		{
			TreeNode tn = treeView1.Nodes["Items"];
			tn.Tag = typeof(ItemId);
			if (uneval)
				LazyChild(tn, N.App.DB.ItemIds.GetList().Count);
			else
				foreach (ItemId d in N.App.DB.ItemIds.GetList())
				{
					TreeNode n = tn.Nodes.Add(d.item, d.item);
					n.Tag = d;
				}
		}
		void LoadMaterials(bool uneval = false)
		{
			TreeNode tn = treeView1.Nodes[Mat];
			tn.Tag = typeof(INCCDB.Descriptor);
			if (uneval)
				LazyChild(tn, N.App.DB.Materials.GetList().Count);
			else
				foreach (INCCDB.Descriptor d in N.App.DB.Materials.GetList())
				{
					TreeNode n = tn.Nodes.Add(d.Name, d.Name);
					n.Tag = d;
				}
		}
		void LoadMBAs(bool uneval = false)
		{
			TreeNode tn = treeView1.Nodes[MBAs];
			tn.Tag = typeof(INCCDB.Descriptor);
			if (uneval)
				LazyChild(tn, N.App.DB.MBAs.GetList().Count);
			else
				foreach (INCCDB.Descriptor d in N.App.DB.MBAs.GetList())
				{
					TreeNode n = tn.Nodes.Add(d.Name, d.Name);
					n.Tag = d;
				}
		}
		void LoadFacilities(bool uneval = false)
		{
			TreeNode tn = treeView1.Nodes[Fac];
			tn.Tag = typeof(INCCDB.Descriptor);
			if (uneval)
				LazyChild(tn, N.App.DB.Facilities.GetList().Count);
			else
				foreach (INCCDB.Descriptor d in N.App.DB.Facilities.GetList())
				{
					TreeNode n = tn.Nodes.Add(d.Name, d.Name);
					n.Tag = d;
				}
		}

		void LoadIsotopics(bool uneval = false)
		{
			TreeNode tn = treeView1.Nodes["Isotopics"];
			tn.Tag = typeof(Isotopics);
			if (uneval)
				LazyChild(tn, N.App.DB.Isotopics.GetList().Count);
			else
				foreach (Isotopics d in N.App.DB.Isotopics.GetList())
				{
					TreeNode n = tn.Nodes.Add(d.id, d.id);
					n.Tag = d;
				}
		}
		void LoadStrata(bool uneval = false)
		{
			TreeNode tn = treeView1.Nodes["Strata"];
			tn.Tag = typeof(Stratum);
			if (uneval)
				LazyChild(tn, N.App.DB.Stratums.GetList().Count);
			else
				foreach (INCCDB.Descriptor d in N.App.DB.Stratums.GetList())
				{
					TreeNode n = tn.Nodes.Add(d.Name, d.Name);
					n.Tag = d;
				}
		}
		void LoadCompositeIsotopics(bool uneval = false)
		{
			TreeNode tn = treeView1.Nodes["Composite Isotopics"];
			tn.Tag = typeof(CompositeIsotopics);
			if (uneval)
				LazyChild(tn, N.App.DB.CompositeIsotopics.GetList().Count);
			else
				foreach (CompositeIsotopics d in N.App.DB.CompositeIsotopics.GetList())
				{
					TreeNode n = tn.Nodes.Add(d.id, d.id);
					n.Tag = d;
				}
		}

		void LoadMethods(bool uneval = false)
		{
			TreeNode tn = treeView1.Nodes["Methods"];
			tn.Tag = typeof(INCCSelector);
			if (uneval)
				LazyChild(tn, N.App.DB.DetectorMaterialAnalysisMethods.Count);
			else
				foreach (KeyValuePair<INCCSelector, AnalysisMethods> kv in N.App.DB.DetectorMaterialAnalysisMethods)
				{
					if (!kv.Value.AnySelected())
						continue;
					TreeNode n = tn.Nodes.Add(kv.Key.ToString(), kv.Key.ToString());
					n.Tag = kv.Value;
				}
		}
		void LoadAcquireState(bool uneval = false)
		{
			TreeNode tn = treeView1.Nodes["Acquisition State"];
			tn.Tag = typeof(AcquireParameters);
			if (uneval)
				LazyChild(tn, N.App.DB.AcquireParametersMap.Count);
			else
				foreach (KeyValuePair<INCCDB.AcquireSelector, AcquireParameters> kv in N.App.DB.AcquireParametersMap)
				{
					TreeNode n = tn.Nodes.Add(kv.Key.ToString(), kv.Key.ToString());
					n.Tag = kv.Value;
					if (kv.Value.MatchSelector(curacq))
						n.NodeFont = new Font(treeView1.Font, FontStyle.Bold);
				}
		}
		void StartQCAndTests()
		{
            TreeNode tn = treeView1.Nodes["QC and Tests"];
            tn.Tag = typeof(TestParameters);			
		}

		void LoadMeasurements(TreeNode tn)
		{
			Detector d = (Detector)tn.Parent.Tag;
			Dictionary<AssaySelector.MeasurementOption, long> l =  N.App.DB.GetMeasurementCounts(d.Id.DetectorId);
			Dictionary<AssaySelector.MeasurementOption, long>.Enumerator li = l.GetEnumerator();
			while (li.MoveNext())
			{
				string s = li.Current.Key.PrintName();
				TreeNode nn = tn.Nodes.Add(s, s + " " + li.Current.Value.ToString());
				nn.Tag = (int)li.Current.Key;
				LazyChild(nn, li.Current.Value);
			}
		}

		void LoadMeasurementDetails(TreeNode tn, string d, AssaySelector.MeasurementOption mo)
		{
			List<INCCDB.IndexedResults> ilist = N.App.DB.IndexedResultsFor(d, mo.ToString(), "All");
			foreach(INCCDB.IndexedResults ir in ilist)
			{
				ir.Rid = N.App.DB.GetCycleCount(ir.Mid);
                string l = ir.DateTime.ToString("yy.MM.dd HH:mm:ss");
				TreeNode nn = tn.Nodes.Add(l);
				nn.Tag = ir;
			}
		}

        AcquireParameters curacq = null;
        Detector curdet = null;
		const string Mat = @"Materials", MBAs = @"MBAs", Fac = @"Facilities";
					
		public Overview()
		{
			InitializeComponent();

            NCC.IntegrationHelpers.GetCurrentAcquireDetectorPair(ref curacq, ref curdet);
			LoadDetectors(true);
			LoadCollarItems(true);
			LoadItems(true);
			LoadCompositeIsotopics(true);
			LoadIsotopics(true);
			LoadStrata(true);
			LoadAcquireState(true);
			LoadMethods(true);
			StartQCAndTests();
			LoadFacilities(true);
			LoadMaterials(true);
			LoadMBAs(true);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            List<string> ls = null;
            if (e.Node.Parent == null)
            {
                if (e.Node.Tag == null)
                    return;
                else
                {
                    Type t = (Type)e.Node.Tag;
                    if (t == typeof(TestParameters))
                    {
                        TestParameters d = N.App.DB.TestParameters.Get();
                        ls = d.ToDBElementList(generate: true).AlignedNameValueList;
                    }
                }
            }
            else
            {
                object o = e.Node.Parent.Tag;
                if (o == null)
                    return;
                else  // display content in the right-hand pane
                {
                    Type t = e.Node.Tag.GetType();
                    if (t == typeof(ItemId))
                    {
                        ls = ((ParameterBase)e.Node.Tag).ToDBElementList(generate: true).AlignedNameValueList;
                    }
                    else if (t == typeof(Detector))
                    {
						ls = GenDetIdStr(((Detector)e.Node.Tag).Id);
                    }
                    else if (t == typeof(Isotopics))
                    {
                        ls = ((ParameterBase)e.Node.Tag).ToDBElementList(generate: true).AlignedNameValueList;
                    }
                    else if (t == typeof(CompositeIsotopics))
                    {
                        ls = ((ParameterBase)e.Node.Tag).ToDBElementList(generate: true).AlignedNameValueList;
                    }
                    else if (t == typeof(CollarItemId))
                    {
                        ls = ((ParameterBase)e.Node.Tag).ToDBElementList(generate: true).AlignedNameValueList;
                    }
                    else if (t == typeof(Stratum))
                    {
                        ls = ((ParameterBase)e.Node.Tag).ToDBElementList(generate: true).AlignedNameValueList;
                    }
                    else if (t == typeof(INCCDB.Descriptor))
                    {
                        INCCDB.Descriptor d = (INCCDB.Descriptor)e.Node.Tag;
                        ls = new List<string>(); ls.Add(d.Item1 + ": " + d.Item2);
                    }
                    else if (t == typeof(AnalysisMethods))
                    {
                        AnalysisMethods d = (AnalysisMethods)e.Node.Tag;
                        ls = d.ToDBElementList(generate: true).AlignedNameValueList;
                    }
                    else if (t == typeof(AcquireParameters))
                    {
                        AcquireParameters d = (AcquireParameters)e.Node.Tag;
                        ls = d.ToDBElementList(generate: true).AlignedNameValueList;
                    }
					else if (t == typeof(AlphaBeta))
					{
						AlphaBeta AB = (AlphaBeta)e.Node.Tag;
						ls = GenDetABStr(AB);
					}
					else if (t == typeof(Multiplicity))
					{
						Multiplicity m = (Multiplicity)e.Node.Tag;
						ls = GenDetMultStr((Detector)o, m);
					}
					else if (t == typeof(DataSourceIdentifier))
					{
						DataSourceIdentifier d = (DataSourceIdentifier)e.Node.Tag;
						ls = GenDetIdStr(d);
					}
                    else if(t == typeof(INCCDB.IndexedResults))
                    {
                        INCCDB.IndexedResults ir = (INCCDB.IndexedResults)e.Node.Tag;
                        List<string> _ls = new List<string>();
                        string l = ir.Rid.ToString() + " cycles";
                        if (!string.IsNullOrEmpty(ir.Campaign))
                            l = l + ", campaign: " + ir.Campaign;
                        _ls.Add(l);
                        ls = _ls;
                    }
                }

			}
            StringBuilder sb = new StringBuilder(100);
            if (ls != null)
            {
                foreach (string s in ls)
                {
                    sb.Append(s); sb.Append('\r');
                }
                richTextBox1.Text = sb.ToString();
            }
        }

		private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node.Nodes.Count == 1)
			{	
				TreeNode child = e.Node.Nodes[0];
				if (!(typeof(bool) == (child.Tag).GetType() && (bool)child.Tag)) // lazy expansion condition
					return;
				child.Tag = false;
				object o = e.Node.Tag;
				if (o.GetType() == typeof(Detector))  // expand this detector node
				{
					e.Node.Nodes.Clear();
					LoadDetector(e.Node);
					return;
				} else if (o.GetType() == typeof(bool) &&
						  child.Text.Contains("Measurement"))  // expand this Detector//Measurement node
				{
					e.Node.Nodes.Clear();
					LoadMeasurements(e.Node);
					e.Node.Tag = false;
					return;
				} else if (o.GetType() == typeof(int))  // expand this Detector//Measurement//Type node
				{
					e.Node.Nodes.Clear();
					object d = e.Node.Parent.Parent.Tag;
					e.Node.Tag = false;
					LoadMeasurementDetails(e.Node, ((Detector)d).Id.DetectorId, (AssaySelector.MeasurementOption)o);
					return;
				}				
				
				// else it is a 1st level tag with a type ensconced.
				
				Type t = (Type)o;
				if (t == typeof(Detector))
				{
					e.Node.Nodes.Clear();
					LoadDetectors();
				}
				if (t == typeof(CollarItemId))
				{
					e.Node.Nodes.Clear();
					LoadCollarItems();
				}
				if (t == typeof(ItemId))
				{
					e.Node.Nodes.Clear();
					LoadItems();
				}
				if (t == typeof(Isotopics))
				{
					e.Node.Nodes.Clear();
					LoadIsotopics();
				}
				if (t == typeof(CompositeIsotopics))
				{
					e.Node.Nodes.Clear();
					LoadCompositeIsotopics();
				}
				if (t == typeof(Stratum))
				{
					e.Node.Nodes.Clear();
					LoadStrata();
				}
				if (t == typeof(INCCSelector))
				{
					e.Node.Nodes.Clear();
					LoadMethods();
				}
				if (t == typeof(AcquireParameters))
				{
					e.Node.Nodes.Clear();
					LoadAcquireState();
				}
				if (t == typeof(INCCDB.Descriptor))
				{
					e.Node.Nodes.Clear();
					string name = e.Node.Name;
					if (name.Equals(Mat,StringComparison.InvariantCultureIgnoreCase))
						LoadMaterials();
					else if (name.Equals(Fac,StringComparison.InvariantCultureIgnoreCase))
						LoadFacilities();
					else if (name.Equals(MBAs,StringComparison.InvariantCultureIgnoreCase))
						LoadMBAs();
				}
			}
		}
		
		string GenDetIdLabelStr(DataSourceIdentifier dsid)
		{
			string s = dsid.ToString();
			return s;
		}
		string GenDetMultLabelStr(Multiplicity mult)
		{
			string s = mult.ToString();
			return s;
		}
		string GenDetMeasLabelStr(long n)
		{
			return string.Format("{0} measurements", n);
		}
		string GenDetABLabelStr(AlphaBeta AB)
		{
			if (AB.α == null || AB.β == null)
				return "AB nil";
			int αidx = 0;
			for (αidx = AB.α.Length - 1; αidx > 0; αidx--)
			{
				if (AB.α[αidx] != 0)
					break;
			}
			int βidx = 0;
			for (βidx = AB.β.Length - 1; βidx > 0; βidx--)
			{
				if (AB.β[βidx] != 0)
					break;
			}
			int mx = Math.Max(αidx, βidx);
			int minl = Math.Min(αidx, βidx);
			double[] potential = new double[4];
			potential[0] = αidx;
			potential[1] = AB.α[αidx];
			potential[2] = βidx;
			potential[3] = AB.β[βidx];
			return string.Format("AB {0,4}:{1,8}{2,4}:{3,8}", potential[0], potential[1], potential[2], potential[3]);
		}

		List<string> GenDetABStr(AlphaBeta AB)
		{
			List<string> ls = new List<string>();
			if (AB.α == null || AB.β == null)
				ls.Add("       A         B");
			int αidx = 0;
			for (αidx = AB.α.Length - 1; αidx > 0; αidx--)
			{
				if (AB.α[αidx] != 0)
					break;
			}
			int βidx = 0;
			for (βidx = AB.β.Length - 1; βidx > 0; βidx--)
			{
				if (AB.β[βidx] != 0)
					break;
			}
			int mx = Math.Max(αidx, βidx);
			int minl = Math.Min(αidx, βidx);
			double[] potential = new double[4];
			potential[0] = αidx;
			potential[1] = AB.α[αidx];
			potential[2] = βidx;
			potential[3] = AB.β[βidx];
 			ls.Add(string.Format("    {0,8} {1,8}", "A", "B"));
            for (int i = 0; i < minl; i++)
			{
				ls.Add(string.Format("{0,4}:{1,8} {2,8}", i, AB.α[i], AB.β[i]));
			}
			for (int i = minl; i < mx; i++)  // check for uneven column
            {
				if (i <= αidx && i <= βidx)
					ls.Add(string.Format("{0,4}:{1,8} {2,8}", i, AB.α[i], AB.β[i]));
				else if (i <= αidx && i > βidx)
					ls.Add(string.Format("{0,4}:{1,8} {2,8}", i, AB.α[i], string.Empty));
				else if (i > αidx && i <= βidx)
					ls.Add(string.Format("{0,4}:{1,8} {2,8}", i, string.Empty, AB.β[i]));
			}
			return ls;
		}

		List<string> GenDetMultStr(Detector det, Multiplicity m)
		{
			List<string> ls = new List<string>();
            ls.Add(string.Format("{0,10}: {1,7} {2}", "Predelay", m.SR.predelay.ToString(), "tics"));
            ls.Add(string.Format("{0,10}: {1,7} {2}", "Gate length", m.SR.gateLength.ToString(), "tics"));
            ls.Add(string.Format("{0,10}: {1,7}", "High voltage", m.SR.highVoltage.ToString()));
            ls.Add(string.Format("{0,10}: {1,7} {2}", "DieAway time", m.SR.dieAwayTime.ToString(), "tics"));
            ls.Add(string.Format("{0,10}: {1,7}", "Efficiency", m.SR.efficiency.ToString()));
            ls.Add(string.Format("{0,15}: {1,7} {2}", "Multiplicity deadtime (T)", m.SR.deadTimeCoefficientTinNanoSecs.ToString(), "pSec"));
            ls.Add(string.Format("{0,15}: {1,7} {2}", "Coefficient A deadtime", m.SR.deadTimeCoefficientAinMicroSecs.ToString(), "µSec"));
            ls.Add(string.Format("{0,15}: {1,7} {2}", "Coefficient B deadtime", m.SR.deadTimeCoefficientBinPicoSecs.ToString(), "pSec"));
            ls.Add(string.Format("{0,15}: {1,7} {2}", "Coefficient C deadtime", m.SR.deadTimeCoefficientCinNanoSecs.ToString(), "pSec"));
            ls.Add(string.Format("{0,15}: {1,7}", "Doubles gate fraction", m.SR.doublesGateFraction.ToString()));
            ls.Add(string.Format("{0,15}: {1,7}", "Triples gate fraction", m.SR.triplesGateFraction.ToString()));
            if (det.ListMode)
            {
                ls.Add(string.Format("{0,10}: {1,7}", "Triggering", m.FA == FAType.FAOff ? "Conventional" : "Fast accidentals"));
				if (m.FA == FAType.FAOn)
	                ls.Add(string.Format("{0,10}: {1,7} {2}", "Bkg gate", m.BackgroundGateTimeStepInTics, "tics"));
				else 
	                ls.Add(string.Format("{0,15}: {1,7} {2}", "Acc gate delay", m.AccidentalsGateDelayInTics, "tics"));
            }
			return ls;
		}

		List<string> GenDetIdStr(DataSourceIdentifier dsid)
		{
			List<string> ls = new List<string>();
			ls.Add(string.Format("{0,10}: {1}", "Name", dsid.DetectorId));
			if (dsid.SRType.IsListMode())
				ls.Add(string.Format("{0,10}: {1}", "LM type", dsid.SRType.ToString()));
			else
				ls.Add(string.Format("{0,10}: {1}", "SR type", dsid.SRType.INCC5ComboBoxString()));
			ls.Add(string.Format("{0,10}: {1}", "User type", dsid.Type));
			ls.Add(string.Format("{0,10}: {1}", "Elec. id", dsid.ElectronicsId));
			if (dsid.SRType.IsCOMPortBasedSR())
				ls.Add(string.Format("{0,10}: {1}", "Baud rate", dsid.BaudRate));
			ls.Add(string.Format("{0,10}: {1}", "Data src", dsid.source.HappyFunName()));
			if (dsid.SRType.IsListMode())
			{
				LMConnectionInfo lm = (LMConnectionInfo)dsid.FullConnInfo;
				//lm.NetComm and 
				//lm.DeviceConfig
				//ls.Add(string.Format("{0,10}: {1}", "Port", lm.NetComm.));
				//ls.Add(string.Format("{0,10}: {1}", "Port", dsid.FullConnInfo.Port));
			}
			if (!string.IsNullOrEmpty(dsid.FullConnInfo.Port))
				ls.Add(string.Format("{0,10}: {1}", "COM port", dsid.FullConnInfo.Port));
			if (dsid.FullConnInfo.Wait != 0)
				ls.Add(string.Format("{0,10}: {1}", "Wait", dsid.FullConnInfo.Wait) + " mSec");

			return ls;
		}

	}
}
