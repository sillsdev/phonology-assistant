using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Controls;
using SIL.Pa.Data;
using SilUtils;
using XCore;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Dialog for defining ambiguous sequences.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ExperimentalTranscriptionsDlg : OKCancelDlgBase, IxCoreColleague
	{
		private ExperimentalTranscriptionControl m_experimentalTransCtrl;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ExperimentalTranscriptionsDlg()
		{
			InitializeComponent();
			PaApp.SettingsHandler.LoadFormProperties(this);

			if (!PaintingHelper.CanPaintVisualStyle())
				pnlGrid.BorderStyle = BorderStyle.Fixed3D;

			m_experimentalTransCtrl = new ExperimentalTranscriptionControl();
			m_experimentalTransCtrl.BorderStyle = BorderStyle.None;
			m_experimentalTransCtrl.Dock = DockStyle.Fill;
			m_experimentalTransCtrl.TabIndex = 0;
			m_experimentalTransCtrl.Grid.RowsAdded += HandleExperimentalTransCtrlRowsAdded;
			pnlGrid.Controls.Add(m_experimentalTransCtrl);
			AdjustGridRows();
			
			PaApp.AddMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			m_experimentalTransCtrl.RefreshHeader();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			PaApp.RemoveMediatorColleague(this);
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
				m_experimentalTransCtrl.Grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			}
			catch { }

			m_experimentalTransCtrl.Grid.AutoResizeRows();

			int extraRowHeight =
				PaApp.SettingsHandler.GetIntSettingsValue(Name, "exptransgridextrarowheight", 2);

			foreach (DataGridViewRow row in m_experimentalTransCtrl.Grid.Rows)
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
			get { return m_experimentalTransCtrl.Grid.IsDirty; }
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
			if (m_experimentalTransCtrl.Grid.IsDirty)
			{
				m_experimentalTransCtrl.SaveChanges();
				PaApp.Project.ReloadDataSources();
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