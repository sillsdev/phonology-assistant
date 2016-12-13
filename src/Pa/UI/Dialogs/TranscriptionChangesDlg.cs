// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Windows.Forms;
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
			
			m_transChangeCtrl.Grid.AdjustGridRows(
				Properties.Settings.Default.TranscriptionChangesDlgGridExtraRowHeight);
	
			App.AddMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			m_transChangeCtrl.RefreshHeader();
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
			get { return m_transChangeCtrl.Grid.IsDirty; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
		}

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

        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.Escape:
                    {
                        this.Close();
                        return true;
                    }
                case Keys.Control | Keys.Tab:
                    {
                        return true;
                    }
                case Keys.Control | Keys.Shift | Keys.Tab:
                    {
                        return true;
                    }
            }
            return base.ProcessCmdKey(ref message, keys);
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
			m_transChangeCtrl.Grid.AdjustGridRows(
				Properties.Settings.Default.TranscriptionChangesDlgGridExtraRowHeight);
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