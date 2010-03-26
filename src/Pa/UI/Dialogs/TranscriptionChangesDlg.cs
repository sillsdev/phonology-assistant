using System;
using System.Windows.Forms;
using SIL.Pa.UI.Controls;
using SilUtils;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Dialog for defining ambiguous sequences.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class TranscriptionChangesDlg : OKCancelDlgBase, IxCoreColleague
	{
		private readonly TranscriptionChangesControl m_TransChangeCtrl;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TranscriptionChangesDlg()
		{
			InitializeComponent();
			App.SettingsHandler.LoadFormProperties(this);

			if (!PaintingHelper.CanPaintVisualStyle())
				pnlGrid.BorderStyle = BorderStyle.Fixed3D;

			m_TransChangeCtrl = new TranscriptionChangesControl();
			m_TransChangeCtrl.BorderStyle = BorderStyle.None;
			m_TransChangeCtrl.Dock = DockStyle.Fill;
			m_TransChangeCtrl.TabIndex = 0;
			m_TransChangeCtrl.Grid.RowsAdded += HandleExperimentalTransCtrlRowsAdded;
			pnlGrid.Controls.Add(m_TransChangeCtrl);
			AdjustGridRows();
			
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
			m_TransChangeCtrl.RefreshHeader();
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjusts the rows in the specified grid by letting the grid calculate the row
		/// heights automatically, then adds an extra amount, found in the settings file,
		/// to each row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustGridRows()
		{
			try
			{
				// Sometimes (or maybe always) this throws an exception when
				// the first row is the only row and is the NewRowIndex row.
				m_TransChangeCtrl.Grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			}
			catch { }

			m_TransChangeCtrl.Grid.AutoResizeRows();

			int extraRowHeight =
				App.SettingsHandler.GetIntSettingsValue(Name, "exptransgridextrarowheight", 2);

			foreach (DataGridViewRow row in m_TransChangeCtrl.Grid.Rows)
				row.Height += extraRowHeight;
		}

		#region Overridden methods of base class
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return m_TransChangeCtrl.Grid.IsDirty; }
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
			if (m_TransChangeCtrl.Grid.IsDirty)
			{
				m_TransChangeCtrl.SaveChanges();
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
			AdjustGridRows();
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