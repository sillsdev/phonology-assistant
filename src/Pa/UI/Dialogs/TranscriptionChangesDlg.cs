using System;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Dialog for defining ambiguous sequences.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class TranscriptionChangesDlg : OKCancelDlgBase, IxCoreColleague
	{
		private readonly TranscriptionChangesControl _transChangeCtrl;
		private readonly PaProject _project;

		/// ------------------------------------------------------------------------------------
		public TranscriptionChangesDlg()
		{
			InitializeComponent();
		}
	
		/// ------------------------------------------------------------------------------------
		public TranscriptionChangesDlg(PaProject project) : this()
		{
			_project = project;

			if (!PaintingHelper.CanPaintVisualStyle())
				pnlGrid.BorderStyle = BorderStyle.Fixed3D;

			_transChangeCtrl = new TranscriptionChangesControl();
			_transChangeCtrl.BorderStyle = BorderStyle.None;
			_transChangeCtrl.Dock = DockStyle.Fill;
			_transChangeCtrl.TabIndex = 0;
			_transChangeCtrl.Grid.RowsAdded += HandleExperimentalTransCtrlRowsAdded;
			pnlGrid.Controls.Add(_transChangeCtrl);
			
			FeaturesDlg.AdjustGridRows(_transChangeCtrl.Grid,
				Settings.Default.TranscriptionChangesDlgGridExtraRowHeight);
			
			App.AddMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			_transChangeCtrl.RefreshHeader();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			App.RemoveMediatorColleague(this);
			base.OnFormClosed(e);
		}

		#region Overridden methods of base class
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return _transChangeCtrl.Grid.IsDirty; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			if (_transChangeCtrl.Grid.IsDirty)
			{
				_transChangeCtrl.SaveChanges();
				_project.ReloadDataSources();
			}

			return true;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the new row has its height set correctly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleExperimentalTransCtrlRowsAdded(object sender,
			DataGridViewRowsAddedEventArgs e)
		{
			FeaturesDlg.AdjustGridRows(_transChangeCtrl.Grid,
				Settings.Default.TranscriptionChangesDlgGridExtraRowHeight);
		}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used in PA.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] {this};
		}

		#endregion
	}
}