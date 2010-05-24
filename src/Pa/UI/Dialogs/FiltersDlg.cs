using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SIL.Localization;
using SIL.Pa.Filters;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilUtils;
using SIL.FieldWorks.Common.UIAdapters;
using System.Windows.Forms.VisualStyles;
using System.Drawing.Drawing2D;

namespace SIL.Pa.UI.Dialogs
{
	#region FiltersDlg class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class FiltersDlg : OKCancelDlgBase
	{
		private const string kFieldCol = "Field";
		private const string kOpCol = "Operator";
		private const string kValueCol = "Value";
		private const string kTypeCol = "Type";

		private readonly DropDownFiltersListBox m_filterDropDown;
		private readonly ImageList m_images;
		private readonly SearchOptionsDropDown m_queryOptionsDropDown;
		private CustomDropDown m_queryOptionsDropDownHost;
		private Dictionary<Filter.Operator, string> m_operatorToText;
		private Dictionary<string, Filter.Operator> m_textToOperator;
		private Dictionary<Filter.ExpressionType, string> m_expTypeToText;
		private Dictionary<string, Filter.ExpressionType> m_textToExpType;
		private bool m_applyFilterOnClose;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FiltersDlg()
		{
			InitializeComponent();

			if (DesignMode)
				return;

			btnApplyNow.Parent.Controls.Remove(btnApplyNow);
			btnApplyNow.Margin = new Padding(0, btnOK.Margin.Top, 20, btnOK.Margin.Bottom);
			tblLayoutButtons.Controls.Add(btnApplyNow, 0, 0);

			m_queryOptionsDropDown = new SearchOptionsDropDown();
			splitFilters.Panel2MinSize = splitFilters.Panel2.Bounds.Width;

			InitializeExpressionTypetrings();
			InitializeOperatorStrings();

			hlblFilters.Font = FontHelper.UIFont;
			lvFilters.Font = FontHelper.UIFont;
			m_grid.Font = FontHelper.UIFont;
			rbMatchAll.Font = FontHelper.UIFont;
			rbMatchAny.Font = FontHelper.UIFont;
			chkIncludeInList.Font = FontHelper.UIFont;
			hlblExpressions.Font = FontHelper.UIFont;

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
			const string category = "Dialog Boxes";

			m_expTypeToText = new Dictionary<Filter.ExpressionType, string>();
			m_textToExpType = new Dictionary<string, Filter.ExpressionType>();

			var text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionTypes.Normal", "Normal", category);

			m_expTypeToText[Filter.ExpressionType.Normal] = text;
			m_textToExpType[text] = Filter.ExpressionType.Normal;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionTypes.PhoneticSearchPattern",
				"Phonetic Search Pattern", category);

			m_expTypeToText[Filter.ExpressionType.PhoneticSrchPtrn] = text;
			m_textToExpType[text] = Filter.ExpressionType.PhoneticSrchPtrn;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionTypes.RegularExpression",
				"Regular Expression", category);

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
			const string category = "Dialog Boxes";

			// Create lists that map the FilterOperator enumeration to it's string equivalent and back
			m_operatorToText = new Dictionary<Filter.Operator, string>();
			m_textToOperator = new Dictionary<string, Filter.Operator>();

			var text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.BeginsWith",
				"Begins with", category);

			m_operatorToText[Filter.Operator.BeginsWith] = text;
			m_textToOperator[text] = Filter.Operator.BeginsWith;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.Contains",
				"Contains", category);

			m_operatorToText[Filter.Operator.Contains] = text;
			m_textToOperator[text] = Filter.Operator.Contains;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.DoesNotBeginWith",
				"Does not begin with", category);

			m_operatorToText[Filter.Operator.DoesNotBeginsWith] = text;
			m_textToOperator[text] = Filter.Operator.DoesNotBeginsWith;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.DoesNotContain",
				"Does not contain", category);

			m_operatorToText[Filter.Operator.DoesNotContain] = text;
			m_textToOperator[text] = Filter.Operator.DoesNotContain;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.DoesNotEndWith",
				"Does not end with", category);

			m_operatorToText[Filter.Operator.DoesNotEndsWith] = text;
			m_textToOperator[text] = Filter.Operator.DoesNotEndsWith;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.EndsWith",
				"Ends with", category);

			m_operatorToText[Filter.Operator.EndsWith] = text;
			m_textToOperator[text] = Filter.Operator.EndsWith;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.Equals",
				"Equals", category);

			m_operatorToText[Filter.Operator.Equals] = text;
			m_textToOperator[text] = Filter.Operator.Equals;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.GreaterThan",
				"Greater than", category);

			m_operatorToText[Filter.Operator.GreaterThan] = text;
			m_textToOperator[text] = Filter.Operator.GreaterThan;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.GreaterThanOrEqual",
				"Greater than or equal", category);

			m_operatorToText[Filter.Operator.GreaterThanOrEqual] = text;
			m_textToOperator[text] = Filter.Operator.GreaterThanOrEqual;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.LessThan",
				"Less than", category);

			m_operatorToText[Filter.Operator.LessThan] = text;
			m_textToOperator[text] = Filter.Operator.LessThan;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.LessThanOrEqual",
				"Less than or equal", category);

			m_operatorToText[Filter.Operator.LessThanOrEqual] = text;
			m_textToOperator[text] = Filter.Operator.LessThanOrEqual;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.Matches",
				"Matches", category);

			m_operatorToText[Filter.Operator.Matches] = text;
			m_textToOperator[text] = Filter.Operator.Matches;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.NoteEqualTo",
				"Not equal to", category);

			m_operatorToText[Filter.Operator.NotEquals] = text;
			m_textToOperator[text] = Filter.Operator.NotEquals;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.PathDoesNotExist",
				"Path does not exist", category);

			m_operatorToText[Filter.Operator.PathDoesNotExist] = text;
			m_textToOperator[text] = Filter.Operator.PathDoesNotExist;

			text = LocalizationManager.LocalizeString(
				"DefineFiltersDlg.FilterExpressionOperators.PathExists",
				"Path exists", category);

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
				if (Settings.Default.FiltersDlgSplitLoc > 0)
					splitFilters.SplitterDistance = Settings.Default.FiltersDlgSplitLoc;
			}
			catch { }

			lvFilters.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			if (FilterHelper.CurrentFilter != null && m_applyFilterOnClose)
			{
				Hide();

				if (FilterHelper.Filters.Contains(FilterHelper.CurrentFilter))
					FilterHelper.ApplyFilter(FilterHelper.CurrentFilter, true);
				else
					FilterHelper.TurnOffCurrentFilter();
			}
			
			base.OnFormClosed(e);
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

			UpdateView();
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

			var fieldNames = (from x in App.FieldInfo
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

			if (Settings.Default.FiltersDlgGrid != null)
				Settings.Default.FiltersDlgGrid.InitializeGrid(m_grid);
	
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
						fieldName = App.FieldInfo[fieldName].DisplayText;

					int i = m_grid.Rows.Add(fieldName, m_operatorToText[expression.Operator],
						expression.Pattern, m_expTypeToText[expression.ExpressionType]);

					m_grid[kTypeCol, i].Tag = expression.SearchQuery;
					m_grid.Rows[i].Tag = expression;
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
				PaFieldInfo fieldInfo = App.FieldInfo[fieldName];
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
				m_queryOptionsDropDown.SearchQuery = query ?? new SearchQuery();
				m_queryOptionsDropDownHost = new CustomDropDown();
				m_queryOptionsDropDownHost.Closed += m_queryDropDown_Closed;
				m_queryOptionsDropDownHost.AddControl(m_queryOptionsDropDown);
				Rectangle rc = m_grid.GetCellDisplayRectangle(2, e.RowIndex, false);
				rc.Y += rc.Height;
				m_queryOptionsDropDownHost.Show(m_grid.PointToScreen(rc.Location));
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
			m_grid[kTypeCol, m_grid.CurrentCellAddress.Y].Tag = m_queryOptionsDropDown.SearchQuery;
			m_queryOptionsDropDownHost.Closed -= m_queryDropDown_Closed;
			m_queryOptionsDropDownHost = null;
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
				m_grid[kFieldCol, row].Value = App.FieldInfo.PhoneticField.DisplayText;
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
			chkIncludeInList.CheckedChanged -= chkShowHide_CheckedChanged;
			rbMatchAny.CheckedChanged -= HandleLogicalExpressionRelationshipChange;
			var filter = CurrentFilter;
			LoadExpressions(filter);

			if (filter != null)
			{
				chkIncludeInList.Checked = filter.ShowInToolbarList;
				rbMatchAll.Checked = !filter.MatchAny;
				rbMatchAny.Checked = filter.MatchAny;
			}

			chkIncludeInList.CheckedChanged += chkShowHide_CheckedChanged;
			rbMatchAny.CheckedChanged += HandleLogicalExpressionRelationshipChange;
			UpdateView();
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
			UpdateView();
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
				m_grid.Rows.Clear();
			else
			{
				lvFilters.FocusedItem.Selected = true;
				lvFilters.FocusedItem = lvFilters.Items[index];
			}

			UpdateView();
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
				m_applyFilterOnClose = false;
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
				CurrentFilter.ShowInToolbarList = chkIncludeInList.Checked;
				lvFilters.FocusedItem.ImageIndex = (chkIncludeInList.Checked ? 0 : 1);
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
				CurrentFilter.MatchAny = rbMatchAny.Checked;
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
					App.FieldInfo.GetFieldFromDisplayText(fieldName);

				if (fieldInfo != null)
					fieldName = fieldInfo.FieldName;
			}

			expression.FieldName = fieldName;
			expression.Operator = m_textToOperator[row.Cells[kOpCol].Value as string];
			expression.Pattern = row.Cells[kValueCol].Value as string ?? string.Empty;
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
			Settings.Default.FiltersDlgGrid = GridSettings.Create(m_grid);
			Settings.Default.FiltersDlgSplitLoc = splitFilters.SplitterDistance;
			base.SaveSettings();
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
			FilterHelper.Filters.Clear();

			foreach (ListViewItem item in lvFilters.Items)
			{
				Filter filter = item.Tag as Filter;
				if (filter != null)
					FilterHelper.Filters.Add(filter);
			}

			// TODO: Validate expressions with search queries.
			
			FilterHelper.SaveFilters();
			m_dirty = false;
			m_grid.IsDirty = false;
			m_applyFilterOnClose = true;
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private FilterExpression CurrentExpression
		{
			get { return (m_grid.CurrentRow != null ? m_grid.CurrentRow.Tag as FilterExpression : null); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_Enter(object sender, EventArgs e)
		{
			UpdateView();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_Leave(object sender, EventArgs e)
		{
			UpdateView();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CurrentRowChanged(object sender, EventArgs e)
		{
			UpdateView();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateView()
		{
			var filter = CurrentFilter;
			m_grid.Enabled = (filter != null);
			chkIncludeInList.Enabled = (filter != null);
			rbMatchAny.Enabled = (filter != null);
			rbMatchAll.Enabled = (filter != null);
			btnDeleteFilter.Enabled = (filter != null);
			btnCopy.Enabled = (filter != null);
			btnApplyNow.Enabled = (filter != null);
			rbMatchAll.Checked = (filter != null && !filter.MatchAny);
			rbMatchAny.Checked = (filter != null && filter.MatchAny);
			btnRemoveExp.Enabled = (CurrentExpression != null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnRemoveExp_Click(object sender, EventArgs e)
		{
			if (m_grid.CurrentRow == null || m_grid.CurrentRow.Index < 0 ||
				m_grid.CurrentRow.Index == m_grid.NewRowIndex)
			{
				System.Media.SystemSounds.Beep.Play();
				return;
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
		private readonly CustomDropDown m_dropDown;

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
			PaFieldInfo fieldInfo = App.FieldInfo[field];
			if (fieldInfo == null)
				fieldInfo = App.FieldInfo.GetFieldFromDisplayText(field);
			
			if (fieldInfo == null)
				return;

			Items.Clear();
			Font = fieldInfo.Font;

			var list = new List<string>();
	
			//SortedDictionary<string, bool> list = new SortedDictionary<string, bool>();
			foreach (WordCacheEntry entry in App.WordCache)
			{
				string val = entry[fieldInfo.FieldName];
				if (!string.IsNullOrEmpty(val))
					list.Add(val);
			}

			// Make sure to include values that are filtered out.
			foreach (var entry in App.RecordCache.WordsNotInCurrentFilter)
			{
				string val = entry[fieldInfo.FieldName];
				if (!string.IsNullOrEmpty(val))
					list.Add(val);
			}

			list = list.Distinct().ToList();
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
