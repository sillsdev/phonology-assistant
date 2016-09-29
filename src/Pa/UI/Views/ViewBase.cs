// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Windows.Forms;
using System.Xml;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Views
{
	public class ViewBase : UserControl, IxCoreColleague
	{
		public PaProject Project { get; private set; }

		private ToolStripButton _regExpSrchEngButton;
		private ToolStripButton _otherSrchEngButton;

		/// ------------------------------------------------------------------------------------
		public ViewBase()
		{
		}

		/// ------------------------------------------------------------------------------------
		public ViewBase(PaProject project)
		{
			Project = project;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool OnProjectLoaded(object args)
		{
			if (args is PaProject)
				Project = args as PaProject;

			return false;
		}
		
		/// ------------------------------------------------------------------------------------
		protected virtual bool OnDataSourcesModified(object args)
		{
			if (args is PaProject)
				Project = args as PaProject;

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool OnUserInterfaceLangaugeChanged(object args)
		{
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);

			if (!(this is SearchVw) && !(this is DistributionChartVw))
				return;

			var controls = Controls.Find(this is SearchVw ? "tbFFWnd" : "tbXYChartWnd", true);
			if (controls.Length == 0)
				return;

			var toolstrip = controls[0] as ToolStrip;

			_regExpSrchEngButton = new ToolStripButton("New", null, HandleSearchEngineItemClick);
			_regExpSrchEngButton.ToolTipText = "Use new search engine";
			_regExpSrchEngButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
			_regExpSrchEngButton.Margin = new Padding(10, _regExpSrchEngButton.Margin.Top,
				_regExpSrchEngButton.Margin.Right, _regExpSrchEngButton.Margin.Bottom);

			_otherSrchEngButton = new ToolStripButton("Old", null, HandleSearchEngineItemClick);
			_otherSrchEngButton.ToolTipText = "Use searching engine from version 3.3.2 and older";
			_otherSrchEngButton.DisplayStyle = ToolStripItemDisplayStyle.Text;

			OnShowSearchSelectionButtonsChagned(null);
			_regExpSrchEngButton.Checked = Properties.Settings.Default.UseNewSearchPatternParser;
			_otherSrchEngButton.Checked = !Properties.Settings.Default.UseNewSearchPatternParser;

			toolstrip.Items.Add(_regExpSrchEngButton);
			toolstrip.Items.Add(_otherSrchEngButton);
		}

		/// ------------------------------------------------------------------------------------
		void HandleSearchEngineItemClick(object sender, System.EventArgs e)
		{
			var item = sender as ToolStripButton;
			Properties.Settings.Default.UseNewSearchPatternParser = (item == _regExpSrchEngButton);
			_regExpSrchEngButton.Checked = Properties.Settings.Default.UseNewSearchPatternParser;
			_otherSrchEngButton.Checked = !Properties.Settings.Default.UseNewSearchPatternParser;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnShowSearchSelectionButtonsChagned(object args)
		{
			if (this is SearchVw || this is DistributionChartVw)
			{
				_regExpSrchEngButton.Visible = Properties.Settings.Default.ShowSearchEngineChoiceButtons;
				_otherSrchEngButton.Visible = Properties.Settings.Default.ShowSearchEngineChoiceButtons;
			}
			
			return false;
		}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] { this };
		}

		#endregion
	}
}
