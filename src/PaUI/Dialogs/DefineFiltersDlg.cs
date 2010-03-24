using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SIL.Localization;
using SIL.Pa.Filters;
using SIL.Pa.UI.Controls;
using SilUtils;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.FFSearchEngine;
using System.Windows.Forms.VisualStyles;
using System.Drawing.Drawing2D;

namespace SIL.Pa.UI.Dialogs
{
	#region DefineFiltersDlg class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class DefineFiltersDlg : OKCancelDlgBase
	{
		private const string kFieldCol = "Field";
		private const string kOpCol = "Operator";
		private const string kValueCol = "Value";
		private const string kTypeCol = "Type";

		private readonly DropDownFiltersListBox m_filterDropDown;
		private readonly ImageList m_images;
		private CustomDropDown m_queryDropDown;
		private Dictionary<Filter.Operator, string> m_operatorToText;
		private Dictionary<string, Filter.Operator> m_textToOperator;
		private Dictionary<Filter.ExpressionType, string> m_expTypeToText;
		private Dictionary<string, Filter.ExpressionType> m_textToExpType;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DefineFiltersDlg()
		{
			InitializeComponent();

			splitFilters.Panel2MinSize = splitFilters.Panel2.Bounds.Width;

			InitializeExpressionTypetrings();
			InitializeOperatorStrings();

			hlblFilters.Font = FontHelper.UIFont;
			lvFilters.Font = FontHelper.UIFont;
			m_grid.Font = FontHelper.UIFont;
			PaApp.SettingsHandler.LoadFormProperties(this);

			// Get rid of these three lines when there is a help topic for this dialog box.
			btnHelp.Visible = false;
			btnOK.Left = btnCancel.Left;
			btnCancel.Left = btnHelp.Left;

			int buttonGap = btnCancel.Left - btnOK.Right;
			btnApplyNow.Left = btnOK.Left - btnApplyNow.Width - (buttonGap * 3);

			splitFilter_SplitterMoved(null, null);

			// Create an image list that is used by the filters list view.
			m_images = new ImageList();
			m_images.Images.Add(Properties.Resources.kimidFilter);
			m_images.Images.Add(Properties.Resources.kimidGrayFilter);
			m_images.ColorDepth = ColorDepth.Depth32Bit;
			m_images.ImageSize = new Size(16, 16);
			lvFilters.SmallImageList = m_images;

			string tip = Utils.ConvertLiteralNewLines(Properties.Resources.kstidShowFilterToolTipText);
			m_tooltip.SetToolTip(chkShowHide, tip);
			m_filterDropDown = new DropDownFiltersListBox();
			BuildGrid();
			LoadFilters();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create lists that map the ExpressionType enumeration to it's string equivalent
		/// and back.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeExpressionTypetrings()
		{
			const string msg = "Displayed in the filters dialog.";
			const string category = "Dialog Boxes";

			m_expTypeToText = new Dictionary<Filter.ExpressionType, string>();
			m_textToExpType = new Dictionary<string, Filter.ExpressionType>();

			var text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionTypes.Normal", "Normal",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_expTypeToText[Filter.ExpressionType.Normal] = text;
			m_textToExpType[text] = Filter.ExpressionType.Normal;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionTypes.PhoneticSearchPattern", "Phonetic Search Pattern",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_expTypeToText[Filter.ExpressionType.PhoneticSrchPtrn] = text;
			m_textToExpType[text] = Filter.ExpressionType.PhoneticSrchPtrn;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionTypes.RegularExpression", "Regular Expression",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_expTypeToText[Filter.ExpressionType.RegExp] = text;
			m_textToExpType[text] = Filter.ExpressionType.RegExp;


			//kstidOtherFilterFieldName	(OTHER FILTER)	
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create lists that map the Filter.Operator enumeration to it's string equivalent
		/// and back.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeOperatorStrings()
		{
			const string msg = "Displayed in the filters dialog.";
			const string category = "Dialog Boxes";

			// Create lists that map the FilterOperator enumeration to it's string equivalent and back
			m_operatorToText = new Dictionary<Filter.Operator, string>();
			m_textToOperator = new Dictionary<string, Filter.Operator>();

			var text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.BeginsWith", "Begins with",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.BeginsWith] = text;
			m_textToOperator[text] = Filter.Operator.BeginsWith;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.Contains", "Contains",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.Contains] = text;
			m_textToOperator[text] = Filter.Operator.Contains;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.DoesNotBeginWith", "Does not begin with",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.DoesNotBeginsWith] = text;
			m_textToOperator[text] = Filter.Operator.DoesNotBeginsWith;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.DoesNotContain", "Does not contain",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.DoesNotContain] = text;
			m_textToOperator[text] = Filter.Operator.DoesNotContain;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.DoesNotEndWith", "Does not end with",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.DoesNotEndsWith] = text;
			m_textToOperator[text] = Filter.Operator.DoesNotEndsWith;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.EndsWith", "Ends with",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.EndsWith] = text;
			m_textToOperator[text] = Filter.Operator.EndsWith;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.Equals", "Equals",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.Equals] = text;
			m_textToOperator[text] = Filter.Operator.Equals;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.GreaterThan", "Greater than",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.GreaterThan] = text;
			m_textToOperator[text] = Filter.Operator.GreaterThan;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.GreaterThanOrEqual", "Greater than or equal",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.GreaterThanOrEqual] = text;
			m_textToOperator[text] = Filter.Operator.GreaterThanOrEqual;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.LessThan", "Less than",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.LessThan] = text;
			m_textToOperator[text] = Filter.Operator.LessThan;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.LessThanOrEqual", "Less than or equal",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.LessThanOrEqual] = text;
			m_textToOperator[text] = Filter.Operator.LessThanOrEqual;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.Matches", "Matches",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.Matches] = text;
			m_textToOperator[text] = Filter.Operator.Matches;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.NoteEqualTo", "Not equal to",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.NotEquals] = text;
			m_textToOperator[text] = Filter.Operator.NotEquals;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.PathDoesNotExist", "Path does not exist",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.PathDoesNotExist] = text;
			m_textToOperator[text] = Filter.Operator.PathDoesNotExist;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.PathExists", "Path exists",
				msg, category, LocalizationCategory.Unspecified, LocalizationPriority.High);

			m_operatorToText[Filter.Operator.PathExists] = text;
			m_textToOperator[text] = Filter.Operator.PathExists;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			try
			{
				// These are in a try/catch because sometimes they might throw an exception
				// in rare cases. The exception has to do with a condition in the underlying
				// .Net framework that I haven't been able to make sense of. Anyway, if an
				// exception is thrown, no big deal, the splitter distances will just be set
				// to their default values.
				int splitDistance = PaApp.SettingsHandler.GetIntSettingsValue(Name, "splitter", 0);
				if (splitDistance > 0)
					splitFilters.SplitterDistance = splitDistance;
			}
			catch { }

			lvFilters.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadFilters()
		{
			ListViewItem currItem = null;

			foreach (var filter in FilterHelper.Filters)
			{
				ListViewItem item = new ListViewItem(filter.Name);
				item.ImageIndex = (filter.ShowInToolbarList ? 0 : 1);
				item.Tag = filter;
				lvFilters.Items.Add(item);
				if (FilterHelper.CurrentFilter != null && filter.Name == FilterHelper.CurrentFilter.Name)
					currItem = item;
			}

			lvFilters.ItemSelectionChanged += lvFilters_ItemSelectionChanged;

			if (currItem != null)
			{
				lvFilters.FocusedItem = currItem;
				currItem.Selected = true;
			}
			else if (lvFilters.Items.Count > 0)
			{
				lvFilters.FocusedItem = lvFilters.Items[0];
				lvFilters.Items[0].Selected = true;
			}

			btnCopy.Enabled = btnRemove.Enabled = (lvFilters.Items.Count > 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildGrid()
		{
			m_grid.AllowUserToAddRows = true;
			m_grid.AllowUserToResizeRows = false;
			m_grid.AllowUserToDeleteRows = true;
			m_grid.AllowUserToOrderColumns = false;
			m_grid.AllowUserToResizeColumns = true;
			m_grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

			var fieldNames = (from x in PaApp.FieldInfo
							  orderby x.DisplayText
							  select x.DisplayText).ToList();

			// Add the "field" that represents another filter, rather than a field in the data.
			fieldNames.Add(FilterExpression.OtherFilterField);

			DataGridViewColumn col = SilGrid.CreateDropDownListComboBoxColumn(kFieldCol, fieldNames);
			col.HeaderText = Properties.Resources.kstidFilterFieldColHdgText;
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			((DataGridViewComboBoxColumn)col).DropDownWidth = 135;
			((DataGridViewComboBoxColumn)col).MaxDropDownItems = 15;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateDropDownListComboBoxColumn(kOpCol, new List<string>(m_operatorToText.Values));
			col.HeaderText = Properties.Resources.kstidFilterOperatorColHdgText;
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			((DataGridViewComboBoxColumn)col).DropDownWidth = 150;
			((DataGridViewComboBoxColumn)col).MaxDropDownItems = 15;
			m_grid.Columns.Add(col);

			col = new SilButtonColumn(kValueCol);
			((SilButtonColumn)col).UseComboButtonStyle = true;
			((SilButtonColumn)col).ButtonClicked += HandleValueColumnButtonClicked;
			((SilButtonColumn)col).DrawDefaultComboButtonWidth = false;
			col.HeaderText = Properties.Resources.kstidFilterValueColHdgText;
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateDropDownListComboBoxColumn(kTypeCol, new List<string>(m_expTypeToText.Values));
			col.HeaderText = Properties.Resources.kstidFilterTypeColHdgText;
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			((DataGridViewComboBoxColumn)col).DropDownWidth = 150;
			((DataGridViewComboBoxColumn)col).MaxDropDownItems = 15;
			m_grid.Columns.Add(col);

			m_grid.AutoResizeColumn(2, DataGridViewAutoSizeColumnMode.ColumnHeader);
			m_grid.AutoResizeColumn(3, DataGridViewAutoSizeColumnMode.ColumnHeader);
			m_grid.AutoResizeColumnHeadersHeight();
			m_grid.ColumnHeadersHeight += 4;
			PaApp.SettingsHandler.LoadGridProperties(m_grid);
			m_grid.IsDirty = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadExpressions(Filter filter)
		{
			bool wasDirty = m_grid.IsDirty;
			m_grid.RowValidated -= m_grid_RowValidated;
			m_grid.CurrentCellDirtyStateChanged -= m_grid_CurrentCellDirtyStateChanged;
			m_grid.Rows.Clear();

			if (filter != null && filter.Expressions.Count > 0)
			{
				foreach (FilterExpression expression in filter.Expressions)
				{
					string fieldName = expression.FieldName;
					if (fieldName != FilterExpression.OtherFilterField)
						fieldName = PaApp.FieldInfo[fieldName].DisplayText;

					m_grid.Rows.Add(fieldName, m_operatorToText[expression.Operator],
						expression.Pattern, m_expTypeToText[expression.ExpressionType]);
				}
			}

			m_grid.RowValidated += m_grid_RowValidated;
			m_grid.CurrentCellDirtyStateChanged += m_grid_CurrentCellDirtyStateChanged;
			m_grid.IsDirty = wasDirty;
		}

		#region Expression Grid control events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_ColumnHeadersHeightChanged(object sender, EventArgs e)
		{
			hlblFilters.Height = m_grid.ColumnHeadersHeight;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 2)
			{
				string fieldName = m_grid[kFieldCol, e.RowIndex].Value as string;
				PaFieldInfo fieldInfo = PaApp.FieldInfo[fieldName];
				e.CellStyle.Font = (fieldInfo != null ? fieldInfo.Font : FontHelper.UIFont);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			DataGridViewComboBoxColumn col = m_grid.Columns[kFieldCol] as DataGridViewComboBoxColumn;
			e.Row.Cells[kFieldCol].Value = col.Items[0];
			col = m_grid.Columns[kOpCol] as DataGridViewComboBoxColumn;
			e.Row.Cells[kOpCol].Value = col.Items[0];
			col = m_grid.Columns[kTypeCol] as DataGridViewComboBoxColumn;
			e.Row.Cells[kTypeCol].Value = col.Items[0];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_KeyDown(object sender, KeyEventArgs e)
		{
			DataGridViewCell cell = m_grid.CurrentCell;

			if (cell != null && cell.ColumnIndex == 2 && e.Alt && e.KeyCode == Keys.Down)
			{
				e.Handled = true;
				HandleValueColumnButtonClicked(m_grid,
					new DataGridViewCellMouseEventArgs(2, cell.RowIndex, 0, 0,
						new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0)));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the event when the user has clicked on the drop-down button in a value
		/// cell. The object dropped-down depends on the expression type. If the expression
		/// type is normal, then the drop-down contains a list of field values found in the
		/// cache for the field specified in the field column. If the expression type is a
		/// phonetic search, then the drop-down is a search query options drop-down for the
		/// query that better be stored in the tag property of the current row's expression
		/// type cell. If the expression type is a regular expression, then their is no
		/// drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleValueColumnButtonClicked(object sender, DataGridViewCellMouseEventArgs e)
		{
			string expType = m_grid[kTypeCol, e.RowIndex].Value as string;

			if (string.IsNullOrEmpty(expType))
				return;

			if ((m_grid[kFieldCol, e.RowIndex].Value as string) == FilterExpression.OtherFilterField)
			{
				if (FilterHelper.Filters.Count != 0)
					m_filterDropDown.Show(m_grid[kValueCol, e.RowIndex]);
			}
			else if (expType == m_expTypeToText[Filter.ExpressionType.PhoneticSrchPtrn])
			{
				SearchQuery query = m_grid[kTypeCol, e.RowIndex].Tag as SearchQuery;
				SearchOptionsDropDown sodd = new SearchOptionsDropDown(query);
				m_queryDropDown = new CustomDropDown();
				m_queryDropDown.Closed += m_queryDropDown_Closed;
				m_queryDropDown.AddControl(sodd);
				Rectangle rc = m_grid.GetCellDisplayRectangle(2, e.RowIndex, false);
				rc.Y += rc.Height;
				m_queryDropDown.Show(m_grid.PointToScreen(rc.Location));
			}
			else
			{
				m_filterDropDown.Show(m_grid[kValueCol, e.RowIndex],
					m_grid[kFieldCol, e.RowIndex].Value as string);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_queryDropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			m_queryDropDown.Closed -= m_queryDropDown_Closed;
			m_queryDropDown = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_RowValidated(object sender, DataGridViewCellEventArgs e)
		{
			Filter filter = CurrentFilter;

			if (filter != null)
			{
				// Save all the expressions for the current filter.
				filter.Expressions.Clear();
				foreach (DataGridViewRow row in m_grid.Rows)
				{
					if (row.Index != m_grid.NewRowIndex)
						filter.Expressions.Add(GetExpressionFromRow(row));
				}
			}

			m_grid.IsDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			if (m_grid.CurrentCell == null)
				return;

			int row = m_grid.CurrentCell.RowIndex;
			int col = m_grid.CurrentCell.ColumnIndex;

			// Commit the edit if the column is one of the combo box columns.
			if (col == 0 || col == 1 || col == 3)
			{
				m_grid.CurrentCellDirtyStateChanged -= m_grid_CurrentCellDirtyStateChanged;
				m_grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
				m_grid.CurrentCellDirtyStateChanged += m_grid_CurrentCellDirtyStateChanged;
			}

			// Get the expression type from the type column.
			string expType = m_grid[kTypeCol, row].Value as string;
			if (string.IsNullOrEmpty(expType) || col != 3)
				return;

			if (m_textToExpType[expType] == Filter.ExpressionType.PhoneticSrchPtrn)
			{
				// When the expression type is a phonetic search, create a search query
				// object in which the expression's search query options will be stored.
				// These are the options that will be displayed on the search query options
				// drop-down when the user clicks on this row's (i.e. e.RowIndex) value
				// column drop-down button.
				if (m_grid[kTypeCol, row].Tag == null)
					m_grid[kTypeCol, row].Tag = new SearchQuery();

				// Force the field to be phonetic and the operation to be a match, then
				// set those cells to readonly because those values are the only valid
				// ones for the phonetic search pattern expression type.
				m_grid[kFieldCol, row].Value = PaApp.FieldInfo.PhoneticField.DisplayText;
				m_grid[kOpCol, row].Value = m_operatorToText[Filter.Operator.Matches];
				m_grid[kFieldCol, row].ReadOnly = true;
				m_grid[kOpCol, row].ReadOnly = true;
			}
			else if (m_textToExpType[expType] == Filter.ExpressionType.RegExp)
			{
				// Force the operation to be match, since that's the only valid operation
				// for regular exp. expression types. Then make sure the field cell is
				// editable, but not the operation cell.
				m_grid[kOpCol, row].Value = m_operatorToText[Filter.Operator.Matches];
				m_grid[kFieldCol, row].ReadOnly = false;
				m_grid[kOpCol, row].ReadOnly = true;
			}
			else
			{
				// The expression type is normal, so make sure the field and operation
				// cells are editable.
				m_grid[kFieldCol, row].ReadOnly = false;
				m_grid[kOpCol, row].ReadOnly = false;
			}
		}

		#endregion

		#region Filter ListView control events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lvFilters_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			chkShowHide.CheckedChanged -= chkShowHide_CheckedChanged;
			rbOr.CheckedChanged -= HandleLogicalExpressionRelationshipChange;
			Filter filter = CurrentFilter;
			LoadExpressions(filter);

			if (filter != null)
			{
				chkShowHide.Checked = filter.ShowInToolbarList;
				rbAnd.Checked = !filter.OrExpressions;
				rbOr.Checked = filter.OrExpressions;
			}

			chkShowHide.Enabled = lblAndOr.Enabled = rbOr.Enabled = rbAnd.Enabled = (filter != null);
			chkShowHide.CheckedChanged += chkShowHide_CheckedChanged;
			rbOr.CheckedChanged += HandleLogicalExpressionRelationshipChange;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lvFilters_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			Filter filter = lvFilters.Items[e.Item].Tag as Filter;
			if (filter == null || e.Label == null || filter.Name == e.Label)
				return;

			if (FilterNameExists(e.Label))
			{
				string msg = string.Format(Properties.Resources.kstidFilterNameExistsMsg, e.Label);
				Utils.MsgBox(msg);
				e.CancelEdit = true;
				return;
			}

			m_dirty = true;
			filter.Name = e.Label;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lvFilters_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F2 && lvFilters.FocusedItem != null)
				lvFilters.FocusedItem.BeginEdit();
			else if (e.KeyCode == Keys.Delete)
				btnRemove_Click(null, null);
		}

		#endregion

		#region Add, Copy, Remove button events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnAdd_Click(object sender, EventArgs e)
		{
			lvFilters.SelectedItems.Clear();
			Filter newFilter = new Filter();
			newFilter.Name = GetNewFilterName(null);
			ListViewItem item = new ListViewItem();
			item.Text = newFilter.Name;
			item.Tag = newFilter;
			lvFilters.Items.Add(item);
			lvFilters.FocusedItem = item;
			item.Selected = true;
			lvFilters.Focus();
			item.BeginEdit();
			m_dirty = true;
			btnRemove.Enabled = btnCopy.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCopy_Click(object sender, EventArgs e)
		{
			if (CurrentFilter != null)
			{
				Filter newFilter = CurrentFilter.Clone();
				newFilter.Name = GetNewFilterName(newFilter.Name);

				ListViewItem item = new ListViewItem();
				item.Text = newFilter.Name;
				item.Tag = newFilter;
				lvFilters.Items.Add(item);
				lvFilters.FocusedItem = item;
				item.Selected = true;
				lvFilters.Focus();
				m_dirty = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (lvFilters.FocusedItem == null)
				return;

			int index = lvFilters.FocusedItem.Index;
			lvFilters.Items.RemoveAt(index);
			while (index >= lvFilters.Items.Count)
				index--;

			lvFilters.Focus();
			m_dirty = true;

			if (index < 0)
			{
				btnRemove.Enabled = btnCopy.Enabled = false;
				m_grid.Rows.Clear();
			}
			else
			{
				lvFilters.FocusedItem.Selected = true;
				lvFilters.FocusedItem = lvFilters.Items[index];
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnApplyNow_Click(object sender, EventArgs e)
		{
			if (CurrentFilter != null)
			{
				if (IsDirty)
				{
					if (!Verify() || !SaveChanges())
						return;
				}

				FilterHelper.ApplyFilter(CurrentFilter, true);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void chkShowHide_CheckedChanged(object sender, EventArgs e)
		{
			if (CurrentFilter != null)
			{
				CurrentFilter.ShowInToolbarList = chkShowHide.Checked;
				lvFilters.FocusedItem.ImageIndex = (chkShowHide.Checked ? 0 : 1);
				m_dirty = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleLogicalExpressionRelationshipChange(object sender, EventArgs e)
		{
			if (CurrentFilter != null)
			{
				CurrentFilter.OrExpressions = rbOr.Checked;
				m_dirty = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void splitFilter_SplitterMoved(object sender, SplitterEventArgs e)
		{
			hdrFilter.Width = lvFilters.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlFilterOptions_Paint(object sender, PaintEventArgs e)
		{
			Color clr1 = SystemColors.ControlLight;
			Color clr2 = SystemColors.ControlDark;
			Rectangle rc = pnlFilterOptions.ClientRectangle;
			using (LinearGradientBrush br = new LinearGradientBrush(rc, clr1, clr2, LinearGradientMode.Vertical))
				e.Graphics.FillRectangle(br, rc);

			// Draw a border around 3 sides: left, right and bottom.
			using (Pen pen = new Pen(VisualStyleInformation.TextControlBorder))
			{
				Point[] pts = new[] {new Point(0, 0), new Point(0, rc.Height - 1),
	                new Point(rc.Width - 1, rc.Height - 1), new	Point(rc.Width - 1, 0)};

				e.Graphics.DrawLines(pen, pts);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private FilterExpression GetExpressionFromRow(DataGridViewRow row)
		{
			FilterExpression expression = new FilterExpression();

			string fieldName = row.Cells[kFieldCol].Value as string;

			if (fieldName != FilterExpression.OtherFilterField)
			{
				PaFieldInfo fieldInfo =
					PaApp.FieldInfo.GetFieldFromDisplayText(fieldName);

				if (fieldInfo != null)
					fieldName = fieldInfo.FieldName;
			}

			expression.FieldName = fieldName;
			expression.Operator = m_textToOperator[row.Cells[kOpCol].Value as string];
			expression.Pattern = row.Cells[kValueCol].Value as string;
			expression.ExpressionType = m_textToExpType[row.Cells[kTypeCol].Value as string];

			if (expression.ExpressionType == Filter.ExpressionType.PhoneticSrchPtrn)
			{
				SearchQuery query = row.Cells[kTypeCol].Tag as SearchQuery;
				expression.SearchQuery = (query ?? new SearchQuery());
				expression.SearchQuery.Pattern = expression.Pattern;
			}

			return expression;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return m_dirty || m_grid.IsDirty; }
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
			PaApp.SettingsHandler.SaveSettingsValue(Name, "splitter", splitFilters.SplitterDistance);
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

			foreach (ListViewItem item in lvFilters.Items)
			{
				Filter filter = item.Tag as Filter;
				if (filter != null)
				{
					foreach (FilterExpression expression in filter.Expressions)
					{
						// Make sure expressions based on phonetic search patterns are valid.
						if (expression.ExpressionType == Filter.ExpressionType.PhoneticSrchPtrn &&
							FilterHelper.CheckSearchQuery(expression.SearchQuery, true) == null)
						{
							lvFilters.SelectedItems.Clear();
							lvFilters.FocusedItem = item;
							item.Selected = true;
							m_grid.Focus();
							return false;
						}
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
		protected override bool SaveChanges()
		{
			var filterList = new List<Filter>();
			foreach (ListViewItem item in lvFilters.Items)
			{
				Filter filter = item.Tag as Filter;
				if (filter != null)
					filterList.Add(filter);
			}

			// TODO: Validate expressions with search queries.

			FilterHelper.SaveFilters();
			m_dirty = false;
			m_grid.IsDirty = false;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetNewFilterName(string nameToCopy)
		{
			string fmt;

			if (nameToCopy != null)
			{
				fmt = Properties.Resources.kstidCopiedFilterNamePrefix;
				while (FilterNameExists(nameToCopy))
					nameToCopy = string.Format(fmt, nameToCopy);

				return nameToCopy;
			}

			fmt = Properties.Resources.kstidNewFilterName;
			string newName = string.Format(fmt, string.Empty).Trim();
			int i = 1;
			while (FilterNameExists(newName))
				newName = string.Format(fmt, i++);

			return newName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool FilterNameExists(string filterName)
		{
			foreach (ListViewItem item in lvFilters.Items)
			{
				Filter filter = item.Tag as Filter;
				if (filter != null && filter.Name == filterName)
					return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Filter CurrentFilter
		{
			get { return (lvFilters.FocusedItem == null ? null : lvFilters.FocusedItem.Tag as Filter); }
		}

		private void m_grid_Enter(object sender, EventArgs e)
		{

		}

		private void m_grid_Leave(object sender, EventArgs e)
		{
			//btnRemoveExp.Enabled = false;
		}

		private void m_grid_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			btnRemoveExp.Enabled = (e.RowIndex != m_grid.NewRowIndex);
		}

		private void btnRemoveExp_Click(object sender, EventArgs e)
		{
			if (m_grid.CurrentRow == null || m_grid.CurrentRow.Index < 0 ||
				m_grid.CurrentRow.Index == m_grid.NewRowIndex)
			{
				System.Media.SystemSounds.Beep.Play();
			}

			int i = m_grid.CurrentRow.Index;
			m_grid.Rows.RemoveAt(i);
			while (i > 0 && i >= m_grid.RowCount) i--;
			m_grid.CurrentCell = m_grid[0, i];
		}
	}

	#endregion

	#region DropDownFiltersListBox class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	internal class DropDownFiltersListBox : ListBox
	{
		private DataGridViewCell m_cell;
		private CustomDropDown m_dropDown;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal DropDownFiltersListBox()
		{
			m_dropDown = new CustomDropDown();
			m_dropDown.AutoCloseWhenMouseLeaves = false;
			m_dropDown.Closed += m_dropDown_Closed;
			m_dropDown.AddControl(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal bool IsDroppedDown
		{
			get { return m_cell != null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void Close()
		{
			m_dropDown.Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_dropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			m_cell = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void Show(DataGridViewCell cell)
		{
			Items.Clear();
			Font = FontHelper.UIFont;

			string cellValue = cell.Value as string;
			foreach (Filter filter in FilterHelper.Filters)
			{
				Items.Add(filter.Name);
				if (cellValue == filter.Name)
					SelectedIndex = Items.Count - 1;
			}

			if (SelectedIndex < 0)
				SelectedIndex = 0;

			m_cell = cell;
			int col = cell.ColumnIndex;
			int row = cell.RowIndex;
			Width = cell.DataGridView.Columns[col].Width;
			Height = (Math.Min(Items.Count, 15) * Font.Height) + 4;
			Rectangle rc = cell.DataGridView.GetCellDisplayRectangle(col, row, false);
			rc.Y += rc.Height;
			m_dropDown.Show(cell.DataGridView.PointToScreen(rc.Location));
			Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void Show(DataGridViewCell cell, string field)
		{
			PaFieldInfo fieldInfo = PaApp.FieldInfo[field];
			if (fieldInfo == null)
				return;

			Items.Clear();
			Font = fieldInfo.Font;

			var list = new List<string>();
	
			//SortedDictionary<string, bool> list = new SortedDictionary<string, bool>();
			foreach (WordCacheEntry entry in PaApp.WordCache)
			{
				string val = entry[field];
				if (!string.IsNullOrEmpty(val))
					list.Add(val);
			}

			// Make sure to include values that are filtered out.
			foreach (var entry in PaApp.RecordCache.WordsNotInCurrentFilter)
			{
				string val = entry[field];
				if (!string.IsNullOrEmpty(val))
					list.Add(val);
			}

			list.Sort();
			Items.AddRange(list.ToArray());
			SelectedItem = cell.Value as string;

			if (SelectedIndex < 0 && list.Count > 0)
				SelectedIndex = 0;

			m_cell = cell;
			int col = cell.ColumnIndex;
			int row = cell.RowIndex;
			IntegralHeight = (list.Count > 0);
			Width = Math.Max(150, cell.DataGridView.Columns[col].Width);
			Height = (list.Count == 0 ? 18 : (Math.Min(Items.Count, 15) * Font.Height) + 4);
			Rectangle rc = cell.DataGridView.GetCellDisplayRectangle(col, row, false);
			rc.Y += rc.Height;
			m_dropDown.Show(cell.DataGridView.PointToScreen(rc.Location));
			Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			int i = IndexFromPoint(e.Location);
			if (i >= 0 && i != SelectedIndex)
				SelectedIndex = i;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			int i = IndexFromPoint(e.Location);
			if (i >= 0)
				m_cell.Value = Items[i] as string;

			m_dropDown.Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.KeyCode == Keys.Escape)
				m_dropDown.Close();
			else if (e.KeyCode == Keys.Return && SelectedItem != null)
			{
				m_cell.Value = SelectedItem as string;
				m_dropDown.Close();
			}
		}
	}

	#endregion
}
