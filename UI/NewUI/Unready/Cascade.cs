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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnalysisDefs;

namespace NewUI
{
	using N = NCC.CentralizedState;

	public partial class Cascade : Form
	{
		public Cascade()
		{
			InitializeComponent();
			TreeNode tn = treeView1.Nodes["Detectors"];
			tn.Tag = typeof(Detector);
			foreach (Detector d in N.App.DB.Detectors)
			{
				TreeNode n = tn.Nodes.Add(d.Id.DetectorName, d.Id.DetectorName);
				n.Tag = d;
			}
			tn = treeView1.Nodes["Collar Items"];
			tn.Tag = typeof(CollarItemId);
			foreach (CollarItemId d in N.App.DB.CollarItemIds.GetList())
			{
				TreeNode n = tn.Nodes.Add(d.item_id, d.item_id);
				n.Tag = d;
			}
			tn = treeView1.Nodes["Items"];
			tn.Tag = typeof(ItemId);
			foreach (ItemId d in N.App.DB.ItemIds.GetList())
			{
				TreeNode n = tn.Nodes.Add(d.item, d.item);
				n.Tag = d;
			}
			tn = treeView1.Nodes["Materials"];
			tn.Tag = typeof(INCCDB.Descriptor);
			foreach (INCCDB.Descriptor d in N.App.DB.Materials.GetList())
			{
				TreeNode n = tn.Nodes.Add(d.Name, d.Name);
				n.Tag = d;
			}
			tn = treeView1.Nodes["MBAs"];
			tn.Tag = typeof(INCCDB.Descriptor);
			foreach (INCCDB.Descriptor d in N.App.DB.MBAs.GetList())
			{
				TreeNode n = tn.Nodes.Add(d.Name, d.Name);
				n.Tag = d;
			}
			tn = treeView1.Nodes["Facilities"];
			tn.Tag = typeof(INCCDB.Descriptor);
			foreach (INCCDB.Descriptor d in N.App.DB.Facilities.GetList())
			{
				TreeNode n = tn.Nodes.Add(d.Name, d.Name);
				n.Tag = d;
			}
			tn = treeView1.Nodes["Strata"];
			tn.Tag = typeof(Stratum);
			foreach (INCCDB.Descriptor d in N.App.DB.Stratums.GetList())
			{
				TreeNode n = tn.Nodes.Add(d.Name, d.Name);
				n.Tag = d;
			}
			tn = treeView1.Nodes["Isotopics"];
			tn.Tag = typeof(Isotopics);
			foreach (Isotopics d in N.App.DB.Isotopics.GetList())
			{
				TreeNode n = tn.Nodes.Add(d.id, d.id);
				n.Tag = d;
			}
			tn = treeView1.Nodes["Composite Isotopics"];
			tn.Tag = typeof(CompositeIsotopics);
			foreach (CompositeIsotopics d in N.App.DB.CompositeIsotopics.GetList())
			{
				TreeNode n = tn.Nodes.Add(d.id, d.id);
				n.Tag = d;
			}
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Parent == null)
				return;
			object o = e.Node.Parent.Tag;
			if (o == null)
				return;
			else
			{
				Type t = e.Node.Tag.GetType();
				List<string> ls = null;
				if (t == typeof(ItemId))
				{
					ls = ((ParameterBase)e.Node.Tag).ToDBElementList(generate:true).AlignedNameValueList;
				}
				else if (t == typeof(Detector))
				{
					ls = ((ParameterBase)e.Node.Tag).ToDBElementList(generate:true).AlignedNameValueList;
				}
				else if (t == typeof(Isotopics))
				{
					ls = ((ParameterBase)e.Node.Tag).ToDBElementList(generate:true).AlignedNameValueList;
				}
				else if  (t == typeof(CompositeIsotopics))
				{
					ls = ((ParameterBase)e.Node.Tag).ToDBElementList(generate:true).AlignedNameValueList;
				}
				else if  (t == typeof(CollarItemId))
				{
					ls = ((ParameterBase)e.Node.Tag).ToDBElementList(generate:true).AlignedNameValueList;
				}
				else if  (t == typeof(Stratum))
				{
					ls = ((ParameterBase)e.Node.Tag).ToDBElementList(generate:true).AlignedNameValueList;
				}
				else if  (t == typeof(INCCDB.Descriptor))
				{
					ls = new List<string>(); ls.Add(((INCCDB.Descriptor)e.Node.Tag).ToString());
				}
				StringBuilder sb = new StringBuilder(100);
				foreach(string s in ls)
                {
                    sb.Append(s); sb.Append('\r');
                }
				richTextBox1.Text = sb.ToString();
			}
		}
	}
}
