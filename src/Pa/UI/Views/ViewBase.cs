using System.Windows.Forms;
using System.Xml;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Views
{
	public class ViewBase : UserControl, IxCoreColleague
	{
		public PaProject Project { get; private set; }

		private ToolStripButton _regExpSrchEngButton;
		private ToolStripButton _otherSrchEngButton;

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
			_regExpSrchEngButton.Checked = Settings.Default.UseRegExpressionSearching;
			_otherSrchEngButton.Checked = !Settings.Default.UseRegExpressionSearching;

			toolstrip.Items.Add(_regExpSrchEngButton);
			toolstrip.Items.Add(_otherSrchEngButton);
		}

		/// ------------------------------------------------------------------------------------
		void HandleSearchEngineItemClick(object sender, System.EventArgs e)
		{
			var item = sender as ToolStripButton;
			Settings.Default.UseRegExpressionSearching = (item == _regExpSrchEngButton);
			_regExpSrchEngButton.Checked = Settings.Default.UseRegExpressionSearching;
			_otherSrchEngButton.Checked = !Settings.Default.UseRegExpressionSearching;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnShowSearchSelectionButtonsChagned(object args)
		{
			_regExpSrchEngButton.Visible = Settings.Default.ShowSearchEngineChoiceButtons;
			_otherSrchEngButton.Visible = Settings.Default.ShowSearchEngineChoiceButtons;
			return true;
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
