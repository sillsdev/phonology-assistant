using System;
using System.Windows.Forms;
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
		private readonly TranscriptionChangesControl m_transChangeCtrl;

		/// ------------------------------------------------------------------------------------
		public TranscriptionChangesDlg()
		{
			InitializeComponent();

			if (!PaintingHelper.CanPaintVisualStyle())
				pnlGrid.BorderStyle = BorderStyle.Fixed3D;

			m_transChangeCtrl = new TranscriptionChangesControl();
			m_transChangeCtrl.BorderStyle = BorderStyle.None;
			m_transChangeCtrl.Dock = DockStyle.Fill;
			m_transChangeCtrl.TabIndex = 0;
			m_transChangeCtrl.Grid.RowsAdded += HandleExperimentalTransCtrlRowsAdded;
			pnlGrid.Controls.Add(m_transChangeCtrl);
			
			FeaturesDlg.AdjustGridRows(m_transChangeCtrl.Grid,
				Settings.Default.TranscriptionChangesDlgGridExtraRowHeight);
			
			App.AddMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			m_transChangeCtrl.RefreshHeader();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			App.RemoveMediatorColleague(this);
			base.OnFormClosed(e);
		}

		#region Overridden methods of base class
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return m_transChangeCtrl.Grid.IsDirty; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			if (m_transChangeCtrl.Grid.IsDirty)
			{
				m_transChangeCtrl.SaveChanges();
				App.Project.ReloadDataSources();
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
			FeaturesDlg.AdjustGridRows(m_transChangeCtrl.Grid,
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