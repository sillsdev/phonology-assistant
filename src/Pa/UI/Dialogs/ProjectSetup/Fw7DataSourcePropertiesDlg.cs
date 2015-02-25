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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Localization;
using Localization.UI;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.DataSourceClasses.FieldWorks;
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
        private readonly List<PaField> m_potentialVernacularFields;
        private Fw7FieldMappingGrid m_grid;
        private FieldMapping m_phoneticMapping;
        private FieldMapping m_audioFileMapping;
        private FieldMapping m_vernacularMapping;
        public static PaField m_selectedvernacularItem;

        #region Construction and initialization
        /// ------------------------------------------------------------------------------------
        public Fw7DataSourcePropertiesDlg()
        {
            InitializeComponent();
        }

        /// ------------------------------------------------------------------------------------
        public Fw7DataSourcePropertiesDlg(PaDataSource ds, IEnumerable<PaField> projectFields)
            : this()
        {
            if (App.DesignMode)
                return;

            m_datasource = ds;

            // Save the phonetic and audio file mappings because we need to remove them from the
            // mappings list so the user won't see them. They're mapped for free and the user
            // can't control that. These will get added back in when the dialog is closed.
            m_phoneticMapping = ds.FieldMappings.Single(m => m.Field.Type == FieldType.Phonetic && m.NameInDataSource == "Phonetic");
            m_vernacularMapping =
                ds.FieldMappings.FirstOrDefault(
                    m => m.Field.Name == ds.FwDataSourceInfo.PhoneticSourceField && m.NameInDataSource != "Phonetic");
            m_audioFileMapping = ds.FieldMappings.Single(m => m.Field.Type == FieldType.AudioFilePath);
            ds.FieldMappings.Remove(m_phoneticMapping);
            ds.FieldMappings.Remove(m_audioFileMapping);

            var potentialFieldNames = Settings.Default.DefaultFw7Fields.Cast<string>();

            var customFields = new Fw7CustomField(ds);
            var cuslist = potentialFieldNames.ToList();
            cuslist.AddRange(customFields.FieldNames());
            potentialFieldNames = cuslist; //(IEnumerable<string>)

            m_potentialFields = projectFields.Where(f => potentialFieldNames.Contains(f.Name));
            m_potentialVernacularFields = m_potentialFields.Where(f => f.FwWsType == FwDBUtils.FwWritingSystemType.Vernacular).ToList();
            foreach (var field in customFields.FieldNames().Where(fieldName => customFields.FwWritingSystemType(fieldName) == FwDBUtils.FwWritingSystemType.Vernacular).Select(fieldName => new PaField(fieldName) { FwWsType = FwDBUtils.FwWritingSystemType.Vernacular }))
            {
                if (m_potentialVernacularFields.FirstOrDefault(f => f.Name == field.Name) == null)
                {
                    m_potentialVernacularFields.Add(field);
                }
            }

            lblProjectValue.Text = ds.FwDataSourceInfo.ToString();
            lblProject.Font = FontHelper.UIFont;
            lblProjectValue.Font = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold);
            grpFields.Font = FontHelper.UIFont;
            grpPhoneticField.Font = FontHelper.UIFont;
            rbVernForm.Font = FontHelper.UIFont;
            rbPronunField.Font = FontHelper.UIFont;
            cboPhoneticWritingSystem.Font = FontHelper.UIFont;
            lblPronunciationOptions.Font = FontHelper.UIFont;
            cboPronunciationOptions.Font = FontHelper.UIFont;

            InitializeGrid();
            InitializePhoneticAndAudioFieldInfo();

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
        private void InitializeGrid()
        {
            m_grid = new Fw7FieldMappingGrid(m_datasource, m_potentialFields);
            m_grid.Dock = DockStyle.Fill;
            pnlGrid.Controls.Add(m_grid);

            m_grid.AutoResizeColumn(0, DataGridViewAutoSizeColumnMode.DisplayedCells);

            if (Settings.Default.Fw7DataSourcePropertiesDlgFieldsGrid != null)
                Settings.Default.Fw7DataSourcePropertiesDlgFieldsGrid.InitializeGrid(m_grid);
        }

        /// ------------------------------------------------------------------------------------
        private void InitializePhoneticAndAudioFieldInfo()
        {
            var vernOptions = m_potentialVernacularFields.Select(f => f.Name).ToArray();
            cboVernacularOptions.Items.AddRange(vernOptions);
            cboVernacularOptions.SelectedItem = null;
            if (m_vernacularMapping != null)
            {
                cboVernacularOptions.SelectedItem = vernOptions.FirstOrDefault(v => v == m_vernacularMapping.NameInDataSource);
                m_selectedvernacularItem = m_vernacularMapping.Field;
            }
            if (cboVernacularOptions.SelectedItem == null)
            {
                cboVernacularOptions.SelectedItem = vernOptions.Contains("LexemeForm") ? "LexemeForm" : cboVernacularOptions.Items[0];
                m_selectedvernacularItem = m_potentialVernacularFields.FirstOrDefault(v => v.Name == "LexemeForm");
            }
            cboPhoneticWritingSystem.Items.AddRange(m_datasource.FwDataSourceInfo.GetWritingSystems()
                .Where(ws => ws.Type == FwDBUtils.FwWritingSystemType.Vernacular)
                .Select(ws => ws.Name).ToArray());

            // Set the phonetic writing system combo's initial value.
            var writingSystems = m_datasource.FwDataSourceInfo.GetWritingSystems().ToArray();
            var fwws = writingSystems.SingleOrDefault(ws => ws.Id == m_phoneticMapping.FwWsId);
            cboPhoneticWritingSystem.SelectedItem = (fwws != null ? fwws.Name : cboPhoneticWritingSystem.Items[0]);

            InitializePronunciationCombo();

            if (m_datasource.FwDataSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.PronunciationField)
                cboPronunciationOptions.SelectedIndex = 0;
            else if (m_datasource.FwDataSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.AllPronunciationFields)
                cboPronunciationOptions.SelectedIndex = 1;

            rbVernForm.Checked =
                (m_datasource.FwDataSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm);

            rbPronunField.Checked =
                (m_datasource.FwDataSourceInfo.PhoneticStorageMethod != FwDBUtils.PhoneticStorageMethod.LexemeForm);

            // update audio WS in case it changed or has now been defined
            var audioWs = FwDBUtils.GetDefaultAudioWritingSystem(writingSystems);
            m_audioFileMapping.FwWsId = audioWs != null ? audioWs.Id : null;
        }

        /// ------------------------------------------------------------------------------------
        private IEnumerable<string> GetPronunciationFieldOptions()
        {
            yield return LocalizationManager.GetString("DialogBoxes.Fw7DataSourcePropertiesDlg.PronunciationOptionFirstOnly",
                "first pronunciation only");

            yield return LocalizationManager.GetString("DialogBoxes.Fw7DataSourcePropertiesDlg.PronunciationOptionEach",
                "each pronunciation");
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
            get { return base.IsDirty || m_grid.IsDirty; }
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
        private void HandleVernacularOptionsChanged(object sender, EventArgs e)
        {
            m_dirty = true;
            if (!cboPronunciationOptions.Enabled)
                m_selectedvernacularItem = m_potentialVernacularFields.First(f => f.Name == cboVernacularOptions.SelectedItem);

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

        #region Methods for verifying changes and saving them before closing
        /// ------------------------------------------------------------------------------------
        protected override void SaveSettings()
        {
            Settings.Default.Fw7DataSourcePropertiesDlgFieldsGrid = GridSettings.Create(m_grid);
            base.SaveSettings();
        }

        /// ------------------------------------------------------------------------------------
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Restore the phonetic and audio file mappings to the field mappings collection.

            m_datasource.FieldMappings.Add(m_phoneticMapping);
            m_datasource.FieldMappings.Add(m_audioFileMapping);
            if (rbVernForm.Checked)
            {
                var phoneticField = m_potentialVernacularFields.First(f => f.Name == cboVernacularOptions.SelectedItem);
                phoneticField.Type = FieldType.Phonetic;
                phoneticField.Note = "V";
                phoneticField.WidthInGrid = 0;
                phoneticField.VisibleInGrid = false;
                m_datasource.FwDataSourceInfo.PhoneticSourceField = phoneticField.Name;
                var newMapping = m_phoneticMapping.Copy();
                newMapping.Field = phoneticField;
                newMapping.NameInDataSource = phoneticField.Name;
                newMapping.Field.FwWsType = FwDBUtils.FwWritingSystemType.Vernacular;
                if (m_datasource.FieldMappings.Any(f => f.NameInDataSource == phoneticField.Name)) ;
                {
                    m_datasource.FieldMappings.Remove(m_datasource.FieldMappings.FirstOrDefault(f => f.NameInDataSource == phoneticField.Name));
                }
                m_datasource.FieldMappings.Add(newMapping);
            }

            base.OnFormClosed(e);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected override bool SaveChanges()
        {
            if (rbVernForm.Checked)
            {
                m_datasource.FwDataSourceInfo.PhoneticStorageMethod = FwDBUtils.PhoneticStorageMethod.LexemeForm;
                m_datasource.FwDataSourceInfo.PhoneticSourceField = m_selectedvernacularItem.Name;
            }
            else if (cboPronunciationOptions.SelectedIndex == 0)
            {
                m_datasource.FwDataSourceInfo.PhoneticStorageMethod = FwDBUtils.PhoneticStorageMethod.PronunciationField;
                m_datasource.FwDataSourceInfo.PhoneticSourceField = m_phoneticMapping.Field.Name;
            }
            else
            {
                m_datasource.FwDataSourceInfo.PhoneticStorageMethod = FwDBUtils.PhoneticStorageMethod.AllPronunciationFields;
                m_datasource.FwDataSourceInfo.PhoneticSourceField = m_phoneticMapping.Field.Name;
            }

            m_datasource.FieldMappings = m_grid.Mappings.ToList();

            // Find the phonetic writing system for the one selected by the user.
            var ws = m_datasource.FwDataSourceInfo.GetWritingSystems()
                .Single(w => w.Name == cboPhoneticWritingSystem.SelectedItem as string);

            m_phoneticMapping.FwWsId = ws.Id;

            return true;
        }

        #endregion

    }
}