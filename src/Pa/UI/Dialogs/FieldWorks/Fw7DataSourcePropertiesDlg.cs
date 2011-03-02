using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Localization.UI;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class Fw7DataSourcePropertiesDlg : OKCancelDlgBase
	{
		private readonly PaDataSource m_datasource;
		private readonly IEnumerable<PaField> m_potentialFields;
		private Fw7FieldMappingGrid m_grid;

		#region Construction and initialization
		/// ------------------------------------------------------------------------------------
		public Fw7DataSourcePropertiesDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		public Fw7DataSourcePropertiesDlg(PaDataSource ds, IEnumerable<PaField> potentialFields) : this()
		{
			if (App.DesignMode)
				return;

			m_datasource = ds;
			m_potentialFields = potentialFields.Select(f => f.Copy());

			lblProjectValue.Text = ds.FwDataSourceInfo.ToString();
			lblProject.Font = FontHelper.UIFont;
			lblProjectValue.Font = new Font(FontHelper.UIFont, FontStyle.Bold);
			grpFields.Font = FontHelper.UIFont;
			grpPhoneticField.Font = FontHelper.UIFont;
			rbLexForm.Font = FontHelper.UIFont;
			rbPronunField.Font = FontHelper.UIFont;
			cboPhoneticWritingSystem.Font = FontHelper.UIFont;
			lblPronunciationOptions.Font = FontHelper.UIFont;
			cboPronunciationOptions.Font = FontHelper.UIFont;

			InitializeGrid(ds, m_potentialFields);
			InitializePhoneticFieldInfo(ds);

			m_dirty = false;
			LocalizeItemDlg.StringsLocalized += InitializePronunciationCombo;
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				LocalizeItemDlg.StringsLocalized -= InitializePronunciationCombo;
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load up the grid with PA field names and the FW writing systems assigned to them,
		/// if any.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeGrid(PaDataSource ds, IEnumerable<PaField> potentialFields)
		{
			m_grid = new Fw7FieldMappingGrid(ds, potentialFields);
			m_grid.Dock = DockStyle.Fill;
			pnlGrid.Controls.Add(m_grid);

			m_grid.AutoResizeColumn(0, DataGridViewAutoSizeColumnMode.DisplayedCells);

			if (Settings.Default.Fw7DataSourcePropertiesDlgFieldsGrid != null)
				Settings.Default.Fw7DataSourcePropertiesDlgFieldsGrid.InitializeGrid(m_grid);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializePhoneticFieldInfo(PaDataSource ds)
		{
			cboPhoneticWritingSystem.Items.AddRange(ds.FwDataSourceInfo.GetWritingSystems()
				.Where(ws => ws.Type == FwDBUtils.FwWritingSystemType.Vernacular)
				.Select(ws => ws.Name).ToArray());

			cboPhoneticWritingSystem.SelectedIndex = 0;

			if (ds.FieldMappings != null)
			{
				var mapping = ds.FieldMappings.SingleOrDefault(m =>
					m.Field != null && m.Field.Type == FieldType.Phonetic);

				if (mapping != null)
				{
					var fwws = ds.FwDataSourceInfo.GetWritingSystems().SingleOrDefault(ws => ws.Id == mapping.FwWsId);
					cboPhoneticWritingSystem.SelectedItem = (fwws != null ?
						fwws.Name : cboPhoneticWritingSystem.Items[0]);
				}
			}

			InitializePronunciationCombo();

			if (ds.FwDataSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.PronunciationField)
				cboPronunciationOptions.SelectedIndex = 0;
			else if (ds.FwDataSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.AllPronunciationFields)
				cboPronunciationOptions.SelectedIndex = 1;

			rbLexForm.Checked = 
				(ds.FwDataSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm);

			rbPronunField.Checked =
				(ds.FwDataSourceInfo.PhoneticStorageMethod != FwDBUtils.PhoneticStorageMethod.LexemeForm);
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<string> GetPronunciationFieldOptions()
		{
			yield return App.LocalizeString("Fw7DataSourcePropertiesDlg.PronunciationOptionFirst",
				"first pronunciation only", App.kLocalizationGroupDialogs);

			yield return App.LocalizeString("Fw7DataSourcePropertiesDlg.PronunciationOptionEach",
				"each pronunciation", App.kLocalizationGroupDialogs);
		}

		/// ------------------------------------------------------------------------------------
		void InitializePronunciationCombo()
		{
			int i = cboPronunciationOptions.SelectedIndex;
			cboPronunciationOptions.Items.Clear();
			cboPronunciationOptions.Items.AddRange(GetPronunciationFieldOptions().ToArray());
			cboPronunciationOptions.SelectedIndex = i;
		}

		#endregion

		#region Other overridden methods
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

		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		private void HandlePhoneticStorageTypeCheckedChanged(object sender, EventArgs e)
		{
			m_dirty = true;
			lblPronunciationOptions.Enabled = rbPronunField.Checked;
			cboPronunciationOptions.Enabled = rbPronunField.Checked;

			if (!cboPronunciationOptions.Enabled)
				cboPronunciationOptions.SelectedIndex = -1;
			else if (cboPronunciationOptions.SelectedIndex == -1)
				cboPronunciationOptions.SelectedIndex = 0;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTableLayoutPhoneticDataPaint(object sender, PaintEventArgs e)
		{
			var dy = cboPhoneticWritingSystem.Top -
				(cboPhoneticWritingSystem.Top - cboPronunciationOptions.Bottom) / 2;

			var pt1 = new Point(0, dy);
			var pt2 = new Point(tblLayoutPhoneticData.ClientSize.Width / 2, dy);

			using (var br = new LinearGradientBrush(pt1, pt2, grpPhoneticField.BackColor,
				VisualStyleInformation.TextControlBorder))
			{
				using (var pen = new Pen(br))
					e.Graphics.DrawLine(pen, pt1, pt2);
			}

			pt1.X = pt2.X - 1;
			pt2.X = tblLayoutPhoneticData.ClientSize.Width;

			using (var br = new LinearGradientBrush(pt1, pt2, VisualStyleInformation.TextControlBorder,
				grpPhoneticField.BackColor))
			{
				using (var pen = new Pen(br))
					e.Graphics.DrawLine(pen, pt1, pt2);
			}
		}

		#endregion

		#region Methods for verifying changes and saving them before closing
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.Fw7DataSourcePropertiesDlgFieldsGrid = GridSettings.Create(m_grid);
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			if (rbLexForm.Checked)
				m_datasource.FwDataSourceInfo.PhoneticStorageMethod = FwDBUtils.PhoneticStorageMethod.LexemeForm;
			else if (cboPronunciationOptions.SelectedIndex == 0)
				m_datasource.FwDataSourceInfo.PhoneticStorageMethod = FwDBUtils.PhoneticStorageMethod.PronunciationField;
			else
				m_datasource.FwDataSourceInfo.PhoneticStorageMethod = FwDBUtils.PhoneticStorageMethod.AllPronunciationFields;

			m_datasource.FieldMappings = m_grid.Mappings.ToList();

			// Find the phonetic field in the list of potential fields.
			var field = m_potentialFields.Single(f => f.Type == FieldType.Phonetic);

			// Find the phonetic writing system for the one selected by the user.
			var ws = m_datasource.FwDataSourceInfo.GetWritingSystems()
				.Single(w => w.Name == cboPhoneticWritingSystem.SelectedItem as string);

			// Add a phonetic field mapping to the mappings returned from the grid, making sure
			// to give the writing system specified by the user.
			m_datasource.FieldMappings.Add(new FieldMapping(field.Name, field, false) { FwWsId = ws.Id });

			return true;
		}

		#endregion
	}
}