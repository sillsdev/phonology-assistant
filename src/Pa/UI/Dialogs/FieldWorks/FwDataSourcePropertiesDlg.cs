using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class FwDataSourcePropertiesDlg : OKCancelDlgBase
	{
		private readonly PaDataSource m_dataSource;
		private readonly Fw6FieldMappingGrid m_grid;

		#region Construction and initialization
		/// ------------------------------------------------------------------------------------
		public FwDataSourcePropertiesDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		public FwDataSourcePropertiesDlg(PaProject project, PaDataSource ds) : this()
		{
			Utils.WaitCursors(true);
			m_dataSource = ds;

			lblProjectValue.Text = ds.FwDataSourceInfo.ToString();

			lblProject.Font = FontHelper.UIFont;
			lblProjectValue.Font = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold);
			grpWritingSystems.Font = FontHelper.UIFont;
			grpPhoneticDataStoreType.Font = FontHelper.UIFont;
			rbLexForm.Font = FontHelper.UIFont;
			rbPronunField.Font = FontHelper.UIFont;

			rbLexForm.Checked =
				(ds.FwDataSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm);

			rbPronunField.Checked =
				(ds.FwDataSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.PronunciationField);

			m_grid = new Fw6FieldMappingGrid(ds, project.Fields);
			m_grid.Dock = DockStyle.Fill;
			pnlGrid.Controls.Add(m_grid);

			m_dirty = false;
			m_grid.IsDirty = false;
			Utils.WaitCursors(false);
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePhoneticStorageTypeCheckedChanged(object sender, EventArgs e)
		{
			m_dirty = true;
		}

		#endregion

		#region Misc. Overridden methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not values on the dialog changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get	{return base.IsDirty || m_grid.IsDirty;}
		}

		#endregion

		#region Methods for verifying changes and saving them before closing
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.FwDataSourcePropertiesDlgGrid = GridSettings.Create(m_grid);
			base.SaveSettings();
		}
		
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Verifies that a writing system has been specified for at least one field.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected override bool Verify()
		//{
		//    if ((from DataGridViewRow row in m_grid.Rows select row.Tag as FwFieldWsMapping)
		//        .Any(ffwm => ffwm != null && IsWsNameSpecified(ffwm.WsName)))
		//    {
		//        return true;
		//    }

		//    var msg = App.LocalizeString("FwDataSourcePropertiesDlg.MissingWritingSystemMsg",
		//        "You must specify a writing system for at least one field.",
		//        "Message displayed in the FieldWorks data source properties dialog box when the user clicks OK without having assigned a writing system to any field.",
		//        App.kLocalizationGroupDialogs);

		//    Utils.MsgBox(msg);
		//    return false;
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			foreach (var newMapping in m_grid.Mappings)
			{
				var oldMapping = m_dataSource.FieldMappings
					.SingleOrDefault(m => m.PaFieldName == newMapping.PaFieldName);

				if (oldMapping != null)
					oldMapping.FwWsId = newMapping.FwWsId;
				else
					m_dataSource.FieldMappings.Add(newMapping);
			}

			m_dataSource.FwDataSourceInfo.PhoneticStorageMethod = (rbLexForm.Checked ?
				FwDBUtils.PhoneticStorageMethod.LexemeForm :
				FwDBUtils.PhoneticStorageMethod.PronunciationField);

			return true;
		}

		#endregion
	}
}