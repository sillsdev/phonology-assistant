using System;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class CustomFieldsDlg : OKCancelDlgBase
	{
		private const string kNameCol = "name";
		private const string kTypeCol = "type";
		private const string kRTLCol = "rtl";
		private const string kILCol = "interlinear";
		private const string kParsedCol = "parsed";
		private const string kOrigCol = "origfield";

		private SilGrid m_grid;
		private readonly PaProject m_project;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CustomFieldsDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CustomFieldsDlg(PaProject project) : this()
		{
			lblInfo.Font = FontHelper.UIFont;
			BuildGrid();

			// Load grid
			foreach (PaFieldInfo fieldInfo in project.FieldInfo)
			{
				if (fieldInfo.IsCustom)
				{
					string dataType = Properties.Resources.kstidCustomFieldGridTypeText;
					if (fieldInfo.IsDate)
						dataType = Properties.Resources.kstidCustomFieldGridTypeDate;
					else if (fieldInfo.IsNumeric)
						dataType = Properties.Resources.kstidCustomFieldGridTypeNumeric;

					object[] row = new object[] { fieldInfo.DisplayText,
						dataType, fieldInfo.RightToLeft, fieldInfo.IsParsed,
						fieldInfo.CanBeInterlinear, fieldInfo };

					m_grid.Rows.Add(row);
				}
			}

			m_project = project;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildGrid()
		{
			m_grid = new SilGrid();
			m_grid.Name = Name + "Grid";
			m_grid.Dock = DockStyle.Fill;
			m_grid.AutoGenerateColumns = false;
			m_grid.Font = FontHelper.UIFont;
			m_grid.AllowUserToOrderColumns = false;
			m_grid.ColumnHeadersHeight *= 2;
			m_grid.RowHeadersWidth = 35;
			m_grid.TabIndex = 0;
			m_grid.AllowUserToAddRows = true;
			m_grid.AllowUserToDeleteRows = true;
			m_grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
			m_grid.CellPainting += m_grid_CellPainting;
			m_grid.CellBeginEdit += m_grid_CellBeginEdit;
			m_grid.CellEndEdit += m_grid_CellEndEdit;
			m_grid.RowsRemoved += m_grid_RowsRemoved;
			m_grid.DefaultValuesNeeded += m_grid_DefaultValuesNeeded;

			DataGridViewColumn col = SilGrid.CreateTextBoxColumn(kNameCol);
			col.HeaderText = Properties.Resources.kstidCustomFieldGridHdgName;
			col.Width = 135;
			m_grid.Columns.Add(col);

			// Create the data type column.
			string[] dataTypes = {Properties.Resources.kstidCustomFieldGridTypeText,
				Properties.Resources.kstidCustomFieldGridTypeNumeric,
				Properties.Resources.kstidCustomFieldGridTypeDate};

			col = SilGrid.CreateDropDownListComboBoxColumn(kTypeCol, dataTypes);
			col.HeaderText = Properties.Resources.kstidCustomFieldGridHdgType;
			col.Width = 80;
			m_grid.Columns.Add(col);

			// Create the column for the right-to-left check box.
			col = SilGrid.CreateCheckBoxColumn(kRTLCol);
			col.HeaderText = Properties.Resources.kstidCustomFieldGridHdgRTL;
			col.Width = 55;
			m_grid.Columns.Add(col);

			// Create the column for the interlinear check box.
			col = SilGrid.CreateCheckBoxColumn(kParsedCol);
			col.HeaderText = Properties.Resources.kstidCustomFieldGridHdgParsed;
			col.Width = 55;
			m_grid.Columns.Add(col);

			// Create the column for the interlinear check box.
			col = SilGrid.CreateCheckBoxColumn(kILCol);
			col.HeaderText = Properties.Resources.kstidCustomFieldGridHdgInterlinear;
			col.Width = 90;
			m_grid.Columns.Add(col);

			// Keep a column for the original field information object.
			col = SilGrid.CreateTextBoxColumn(kOrigCol);
			col.Visible = false;
			m_grid.Columns.Add(col);

			Controls.Add(m_grid);
			m_grid.BringToFront();
			
			// Do this for the sake of the first time the dialog is shown after pa.xml is
			// created. For all subsequent times, the following call to LoadGridProperties
			// will overwrite whatever size adjustments are made in these two calls.
			m_grid.AutoResizeColumnHeadersHeight();
			m_grid.AutoResizeColumns();

			PaApp.SettingsHandler.LoadFormProperties(this);
			PaApp.SettingsHandler.LoadGridProperties(m_grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return "Text" for the default value of the data type column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			e.Row.Cells[kTypeCol].Value = Properties.Resources.kstidCustomFieldGridTypeText;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For some reason, the grid doesn't automatically set the dirty flag when a row is
		/// deleted so I'm doing it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			m_grid.IsDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Disconnect the form's cancel button so pressing ESC while in edit mode in the grid
		/// doesn't close the form but just ends cell editing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			CancelButton = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Restore the cancel button for the form so pressing ESC is like clicking the cancel
		/// button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			CancelButton = btnCancel;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the values of the field's interlinear and parsed values don't contradict
		/// each other.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void VerifyCheckedValues(int row, int col)
		{
			bool parsedChecked = (m_grid[kParsedCol, row].EditedFormattedValue == null ? false :
				(bool)m_grid[kParsedCol, row].EditedFormattedValue);

			bool interlinearChecked = (m_grid[kILCol, row].EditedFormattedValue == null ? false :
				(bool)m_grid[kILCol, row].EditedFormattedValue);

			if (m_grid.Columns[col].Name == kParsedCol)
			{
				// If the field can be interlinear then it must be parsed.
				if (!parsedChecked && interlinearChecked)
				{
					m_grid[kILCol, row].Value = false;
					m_grid.InvalidateCell(m_grid[kILCol, row]);
				}
			}
			else if (m_grid.Columns[col].Name == kILCol)
			{
				// If the field is not parsed, then it cannot be interlinear.
				if (interlinearChecked && !parsedChecked)
				{
					m_grid[kParsedCol, row].Value = true;
					m_grid.InvalidateCell(m_grid[kParsedCol, row]);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			m_grid.AutoResizeRows();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex >= 0 || e.RowIndex == m_grid.NewRowIndex)
			{
				// When painting one of the two check box cells, make sure they don't
				// contradict each other. This is done here since all other cell editing
				// events where I can do this validation, only happen after the cell
				// leaves the edit mode, which is usually when the cell loses focus. For
				// me, that's too long to wait. I want the verification to happen right
				// after the user changes the checked value of one of the check boxes.
				if (e.RowIndex >= 0 && e.ColumnIndex > 0)
					VerifyCheckedValues(e.RowIndex, e.ColumnIndex);

				return;
			}

			// *******
			// Uncomment if it's desired to have row numbers painted in the row headings.
			// *******
			//Rectangle rc = e.CellBounds;
			//e.PaintBackground(rc, false);
			//using (StringFormat sf = STUtils.GetStringFormat(true))
			//{
			//    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
			//    e.Graphics.DrawString((e.RowIndex + 1).ToString(), FontHelper.UIFont,
			//        SystemBrushes.ControlText, rc, sf);
			//}

			//e.Handled = true;
		}

		#region Overridden methods of base class
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return m_grid.IsDirty; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			// Commit pending changes in the grid.
			m_grid.CommitEdit(DataGridViewDataErrorContexts.Commit);

			// Remove rows with no text in the field name.
			int i = 0;
			while (i < m_grid.NewRowIndex)
			{
				string fieldName = m_grid[kNameCol, i].Value as string;
				
				if (fieldName != null)
					fieldName = fieldName.Trim();

				if (string.IsNullOrEmpty(fieldName))
					m_grid.Rows.RemoveAt(i);
				else
					i++;
			}

			// Check for duplicate fields.
			foreach (DataGridViewRow row1 in m_grid.Rows)
			{
				if (row1.Index == m_grid.NewRowIndex)
					continue;

				PaFieldInfo origFieldInfo = row1.Cells[kOrigCol].Value as PaFieldInfo;
				string fieldName1 = row1.Cells[kNameCol].Value as string;

				// Check if the field already exists in the project's fields collection.
				// Make sure we don't test the field against itself.
				PaFieldInfo fieldInfo = m_project.FieldInfo[fieldName1];
				if (fieldInfo != null && fieldInfo != origFieldInfo)
				{
					STUtils.STMsgBox(string.Format(
						Properties.Resources.kstidCustomFieldExistsMsg, fieldName1),
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

					return false;
				}

				// Check if there are any other rows in the grid with the same field name.
				foreach (DataGridViewRow row2 in m_grid.Rows)
				{
					// Make sure we don't test the row against itself.
					if (row2 == row1 || row2.Index == m_grid.NewRowIndex)
						continue;

					string fieldName2 = row2.Cells[kNameCol].Value as string;
					if (fieldName2 == fieldName1)
					{
						STUtils.STMsgBox(string.Format(
							Properties.Resources.kstidCustomFieldDupMsg, fieldName2),
							MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

						return false;
					}
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			PaApp.SettingsHandler.SaveFormProperties(this);
			PaApp.SettingsHandler.SaveGridProperties(m_grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			m_project.FieldInfo.RemoveCustomFields();

			if (m_grid.Rows.Count == 0)
				return true;

			for (int i = 0; i < m_grid.Rows.Count; i++)
			{
				if (i == m_grid.NewRowIndex)
					continue;

				PaFieldInfo fieldInfo =
					(m_grid[kOrigCol, i].Value as PaFieldInfo ?? new PaFieldInfo());

				string origFieldName = fieldInfo.FieldName;

				fieldInfo.IsNumeric = ((m_grid[kTypeCol, i].Value as string) ==
					Properties.Resources.kstidCustomFieldGridTypeNumeric);

				fieldInfo.IsDate = ((m_grid[kTypeCol, i].Value as string) ==
					Properties.Resources.kstidCustomFieldGridTypeDate);

				fieldInfo.RightToLeft =
					(m_grid[kRTLCol, i].Value != null && (bool)m_grid[kRTLCol, i].Value);

				fieldInfo.IsParsed =
					(m_grid[kParsedCol, i].Value != null && (bool)m_grid[kParsedCol, i].Value);

				fieldInfo.CanBeInterlinear =
					(m_grid[kILCol, i].Value != null && (bool)m_grid[kILCol, i].Value);

				fieldInfo.DisplayText = m_grid[kNameCol, i].Value as string;
				fieldInfo.DisplayIndexInGrid = m_project.FieldInfo.Count - 1;
				fieldInfo.DisplayIndexInRecView = m_project.FieldInfo.Count - 1;
				fieldInfo.FieldName = fieldInfo.DisplayText;
				fieldInfo.IsCustom = true;
				m_project.FieldInfo.Add(fieldInfo);

				// Inform the project that a custom field has changed.
				if (origFieldName != null && origFieldName != fieldInfo.FieldName)
					m_project.ProcessRenamedCustomField(origFieldName, fieldInfo.FieldName);
			}

			return true;
		}

		#endregion
	}
}

