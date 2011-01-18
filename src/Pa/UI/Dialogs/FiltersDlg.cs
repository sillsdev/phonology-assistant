using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Filters;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;
using SIL.FieldWorks.Common.UIAdapters;

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
		private const string kFilterNameCol = "FilterName";
		private const string kShowInListCol = "ShowInList";

		private const string kFieldCol = "Field";
		private const string kOpCol = "Operator";
		private const string kValueCol = "Value";
		private const string kTypeCol = "Type";
		private const string kDeleteCol = "Delete";

		private readonly DropDownFiltersListBox m_filterDropDown;
		private readonly SearchOptionsDropDown m_queryOptionsDropDown;
		private CustomDropDown m_queryOptionsDropDownHost;
		private Dictionary<Filter.Operator, string> m_operatorToText;
		private Dictionary<string, Filter.Operator> m_textToOperator;
		private Dictionary<Filter.ExpressionType, string> m_expTypeToText;
		private Dictionary<string, Filter.ExpressionType> m_textToExpType;
		private bool m_applyFilterOnClose;

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

			m_gridExpressions.Font = FontHelper.UIFont;
			m_gridFilters.Font = FontHelper.UIFont;
			hlblExpressions.Font = FontHelper.UIFont;
			lblExpressionMatchMsgPart1.Font = FontHelper.UIFont;
			lblExpressionMatchMsgPart2.Font = FontHelper.UIFont;
			cboExpressionMatch.Font = FontHelper.UIFont;

			// Get rid of these three lines when there is a help topic for this dialog box.
			btnHelp.Visible = false;
			btnOK.Left = btnCancel.Left;
			btnCancel.Left = btnHelp.Left;

			int buttonGap = btnCancel.Left - btnOK.Right;
			btnApplyNow.Left = btnOK.Left - btnApplyNow.Width - (buttonGap * 3);

			pnlExpressionMatch.ColorTop = Settings.Default.GradientPanelTopColor;
			pnlExpressionMatch.ColorBottom = Settings.Default.GradientPanelBottomColor;
			lblExpressionMatchMsgPart1.ForeColor = Settings.Default.GradientPanelTextColor;
			lblExpressionMatchMsgPart2.ForeColor = Settings.Default.GradientPanelTextColor;

			cboExpressionMatch.Items.Add(App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionMatchTypes.Any", "any",
				App.kLocalizationGroupDialogs));

			cboExpressionMatch.Items.Add(App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionMatchTypes.All", "all",
				App.kLocalizationGroupDialogs));

			lblExpressionMatchMsgPart1.Tag = lblExpressionMatchMsgPart1.Text;

			m_filterDropDown = new DropDownFiltersListBox();
			BuildFiltersGrid();
			BuildExpressionsGrid();
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
			m_expTypeToText = new Dictionary<Filter.ExpressionType, string>();
			m_textToExpType = new Dictionary<string, Filter.ExpressionType>();

			var text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionTypes.Normal", "Normal", App.kLocalizationGroupDialogs);

			m_expTypeToText[Filter.ExpressionType.Normal] = text;
			m_textToExpType[text] = Filter.ExpressionType.Normal;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionTypes.PhoneticSearchPattern",
				"Phonetic Search Pattern", App.kLocalizationGroupDialogs);

			m_expTypeToText[Filter.ExpressionType.PhoneticSrchPtrn] = text;
			m_textToExpType[text] = Filter.ExpressionType.PhoneticSrchPtrn;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionTypes.RegularExpression",
				"Regular Expression", App.kLocalizationGroupDialogs);

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
			// Create lists that map the FilterOperator enumeration to it's string equivalent and back
			m_operatorToText = new Dictionary<Filter.Operator, string>();
			m_textToOperator = new Dictionary<string, Filter.Operator>();

			var text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.BeginsWith",
				"Begins with", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.BeginsWith] = text;
			m_textToOperator[text] = Filter.Operator.BeginsWith;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.Contains",
				"Contains", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.Contains] = text;
			m_textToOperator[text] = Filter.Operator.Contains;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.DoesNotBeginWith",
				"Does not begin with", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.DoesNotBeginsWith] = text;
			m_textToOperator[text] = Filter.Operator.DoesNotBeginsWith;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.DoesNotContain",
				"Does not contain", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.DoesNotContain] = text;
			m_textToOperator[text] = Filter.Operator.DoesNotContain;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.DoesNotEndWith",
				"Does not end with", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.DoesNotEndsWith] = text;
			m_textToOperator[text] = Filter.Operator.DoesNotEndsWith;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.EndsWith",
				"Ends with", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.EndsWith] = text;
			m_textToOperator[text] = Filter.Operator.EndsWith;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.Equals",
				"Equals", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.Equals] = text;
			m_textToOperator[text] = Filter.Operator.Equals;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.GreaterThan",
				"Greater than", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.GreaterThan] = text;
			m_textToOperator[text] = Filter.Operator.GreaterThan;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.GreaterThanOrEqual",
				"Greater than or equal", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.GreaterThanOrEqual] = text;
			m_textToOperator[text] = Filter.Operator.GreaterThanOrEqual;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.LessThan",
				"Less than", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.LessThan] = text;
			m_textToOperator[text] = Filter.Operator.LessThan;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.LessThanOrEqual",
				"Less than or equal", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.LessThanOrEqual] = text;
			m_textToOperator[text] = Filter.Operator.LessThanOrEqual;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.Matches",
				"Matches", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.Matches] = text;
			m_textToOperator[text] = Filter.Operator.Matches;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.NoteEqualTo",
				"Not equal to", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.NotEquals] = text;
			m_textToOperator[text] = Filter.Operator.NotEquals;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.PathDoesNotExist",
				"Path does not exist", App.kLocalizationGroupDialogs);

			m_operatorToText[Filter.Operator.PathDoesNotExist] = text;
			m_textToOperator[text] = Filter.Operator.PathDoesNotExist;

			text = App.L10NMngr.LocalizeString(
				"FiltersDlg.FilterExpressionOperators.PathExists",
				"Path exists", App.kLocalizationGroupDialogs);

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

			// Size the expression match combo. to the width of its longest item.
			using (var g = CreateGraphics())
			{
				int width = 0;
				var font = cboExpressionMatch.Font;

				foreach (var item in cboExpressionMatch.Items)
				{
					var text = item as string;
					width = Math.Max(width, TextRenderer.MeasureText(g, text, font).Width);
				}

				cboExpressionMatch.Width = width + SystemInformation.VerticalScrollBarWidth;
			}

			m_gridFilters.Focus();
		}

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
		private void BuildFiltersGrid()
		{
			m_gridFilters.AllowUserToResizeRows = false;
			m_gridFilters.AllowUserToOrderColumns = false;
			m_gridFilters.AllowUserToResizeColumns = true;
			m_gridFilters.EditMode = DataGridViewEditMode.EditOnF2;
			m_gridFilters.RowHeadersVisible = false;
			m_gridFilters.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			m_gridFilters.CellBorderStyle = DataGridViewCellBorderStyle.None;
			m_gridFilters.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

			DataGridViewColumn col = SilGrid.CreateTextBoxColumn(kFilterNameCol);
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			col.Resizable = DataGridViewTriState.True;
			col.ReadOnly = false;
			col.Width = 200;
			m_gridFilters.Columns.Add(col);
			App.L10NMngr.LocalizeObject(m_gridFilters.Columns[kFilterNameCol],
				"FiltersDlg.FiltersGridFilterNameColumnHeadingText", "Available Filters",
				App.kLocalizationGroupDialogs);

			col = SilGrid.CreateCheckBoxColumn(kShowInListCol);
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			col.Resizable = DataGridViewTriState.False;
			m_gridFilters.Columns.Add(col);
			App.L10NMngr.LocalizeObject(m_gridFilters.Columns[kShowInListCol],
				"FiltersDlg.FiltersGridVisibleInFilterMenuNameColumnHeadingText",
				"Visible", App.kLocalizationGroupDialogs);

			m_gridFilters.AutoResizeColumn(1, DataGridViewAutoSizeColumnMode.ColumnHeader);
			m_gridFilters.AutoResizeColumnHeadersHeight();
			m_gridFilters.ColumnHeadersHeight += 4;

			if (Settings.Default.FiltersDlgFiltersGrid != null)
				Settings.Default.FiltersDlgFiltersGrid.InitializeGrid(m_gridFilters);

			m_gridFilters.IsDirty = false;
			m_gridFilters.CellFormatting += App.HandleFormattingSelectedGridCell;
		}

		/// ------------------------------------------------------------------------------------
		private void LoadFilters()
		{
			int index = 0;
	
			foreach (var filter in FilterHelper.Filters)
			{
				int i = m_gridFilters.Rows.Add(filter.Name, filter.ShowInToolbarList);
				m_gridFilters.Rows[i].Tag = filter;
				if (FilterHelper.CurrentFilter != null && filter.Name == FilterHelper.CurrentFilter.Name)
					index = i;
			}

			if (m_gridFilters.RowCount > 0)
				m_gridFilters.CurrentCell = m_gridFilters[kFilterNameCol, index];

			m_gridFilters.IsDirty = false;
			UpdateView();
		}

		/// ------------------------------------------------------------------------------------
		private void BuildExpressionsGrid()
		{
			m_gridExpressions.AllowUserToAddRows = true;
			m_gridExpressions.AllowUserToResizeRows = false;
			m_gridExpressions.AllowUserToDeleteRows = true;
			m_gridExpressions.AllowUserToOrderColumns = false;
			m_gridExpressions.AllowUserToResizeColumns = true;
			m_gridExpressions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

			var fieldNames = (from x in App.FieldInfo
							  orderby x.DisplayText
							  select x.DisplayText).ToList();
		
			// Add the "field" that represents another filter, rather than a field in the data.
			fieldNames.Add(FilterExpression.OtherFilterField);

			DataGridViewColumn col = SilGrid.CreateDropDownListComboBoxColumn(kFieldCol, fieldNames);
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			((DataGridViewComboBoxColumn)col).DropDownWidth = 135;
			((DataGridViewComboBoxColumn)col).MaxDropDownItems = 15;
			m_gridExpressions.Columns.Add(col);
			App.L10NMngr.LocalizeObject(m_gridExpressions.Columns[kFieldCol],
				"FiltersDlg.ExpressionsGridFieldColumnHeadingText",
				"Field", App.kLocalizationGroupDialogs);

			col = SilGrid.CreateDropDownListComboBoxColumn(kOpCol, new List<string>(m_operatorToText.Values));
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			((DataGridViewComboBoxColumn)col).DropDownWidth = 150;
			((DataGridViewComboBoxColumn)col).MaxDropDownItems = 15;
			m_gridExpressions.Columns.Add(col);
			App.L10NMngr.LocalizeObject(m_gridExpressions.Columns[kOpCol],
				"FiltersDlg.ExpressionsGridOperatorColumnHeadingText",
				"Operator", App.kLocalizationGroupDialogs);

			col = new SilButtonColumn(kValueCol);
			((SilButtonColumn)col).ButtonStyle = SilButtonColumn.ButtonType.MinimalistCombo;
			((SilButtonColumn)col).ButtonClicked += HandleExpressionsGridValueColumnButtonClicked;
			((SilButtonColumn)col).DrawDefaultComboButtonWidth = false;
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			m_gridExpressions.Columns.Add(col);
			App.L10NMngr.LocalizeObject(m_gridExpressions.Columns[kValueCol],
				"FiltersDlg.ExpressionsGridValueColumnHeadingText",
				"Value", App.kLocalizationGroupDialogs);

			col = SilGrid.CreateDropDownListComboBoxColumn(kTypeCol, new List<string>(m_expTypeToText.Values));
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			((DataGridViewComboBoxColumn)col).DropDownWidth = 150;
			((DataGridViewComboBoxColumn)col).MaxDropDownItems = 15;
			m_gridExpressions.Columns.Add(col);
			App.L10NMngr.LocalizeObject(m_gridExpressions.Columns[kTypeCol],
				"FiltersDlg.ExpressionsGridTypeColumnHeadingText",
				"Type", App.kLocalizationGroupDialogs);

			col = SilGrid.CreateImageColumn(kDeleteCol);
			col.HeaderText = string.Empty;
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			col.Resizable = DataGridViewTriState.False;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.Width = Properties.Resources.DeleteNormal.Width + 2;
			((DataGridViewImageColumn)col).Image = new Bitmap(Properties.Resources.DeleteNormal.Width,
				Properties.Resources.DeleteNormal.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			m_gridExpressions.Columns.Add(col);
			
			m_gridExpressions.AutoResizeColumn(2, DataGridViewAutoSizeColumnMode.ColumnHeader);
			m_gridExpressions.AutoResizeColumn(3, DataGridViewAutoSizeColumnMode.ColumnHeader);
			m_gridExpressions.AutoResizeColumnHeadersHeight();
			m_gridExpressions.ColumnHeadersHeight += 4;

			if (Settings.Default.FiltersDlgExpressionsGrid != null)
				Settings.Default.FiltersDlgExpressionsGrid.InitializeGrid(m_gridExpressions);
	
			m_gridExpressions.IsDirty = false;
		}

		/// ------------------------------------------------------------------------------------
		private void LoadExpressions(Filter filter)
		{
			bool wasDirty = m_gridExpressions.IsDirty;
			m_gridExpressions.RowValidated -= HandleExpressionsGridRowValidated;
			m_gridExpressions.CurrentCellDirtyStateChanged -= HandleExpressionsGridCurrentCellDirtyStateChanged;
			m_gridExpressions.Rows.Clear();

			if (filter != null && filter.Expressions.Count > 0)
			{
				foreach (FilterExpression expression in filter.Expressions)
				{
					string fieldName = expression.FieldName;
					if (fieldName != FilterExpression.OtherFilterField)
						fieldName = App.FieldInfo[fieldName].DisplayText;

					int i = m_gridExpressions.Rows.Add(fieldName, m_operatorToText[expression.Operator],
						expression.Pattern, m_expTypeToText[expression.ExpressionType],
						Properties.Resources.DeleteNormal);

					m_gridExpressions[kTypeCol, i].Tag = expression.SearchQuery;
					m_gridExpressions.Rows[i].Tag = expression;
				}
			}

			m_gridExpressions.RowValidated += HandleExpressionsGridRowValidated;
			m_gridExpressions.CurrentCellDirtyStateChanged += HandleExpressionsGridCurrentCellDirtyStateChanged;
			m_gridExpressions.IsDirty = wasDirty;
		}

		#region Expression Grid control events
		/// ------------------------------------------------------------------------------------
		private void HandleExpressionsGridCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			App.HandleFormattingSelectedGridCell(sender, e);

			if (m_gridExpressions.Columns[e.ColumnIndex].Name == kDeleteCol && e.RowIndex == m_gridExpressions.NewRowIndex)
			{
				e.Value = ((DataGridViewImageColumn)m_gridExpressions.Columns[kDeleteCol]).Image;
			}
			else if (m_gridExpressions.Columns[e.ColumnIndex].Name == kValueCol)
			{
				string fieldName = m_gridExpressions[kFieldCol, e.RowIndex].Value as string;
				PaFieldInfo fieldInfo = App.FieldInfo[fieldName];
				e.CellStyle.Font = (fieldInfo != null ? fieldInfo.Font : FontHelper.UIFont);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleExpressionsGridDefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			DataGridViewComboBoxColumn col = m_gridExpressions.Columns[kFieldCol] as DataGridViewComboBoxColumn;
			e.Row.Cells[kFieldCol].Value = col.Items[0];
			col = m_gridExpressions.Columns[kOpCol] as DataGridViewComboBoxColumn;
			e.Row.Cells[kOpCol].Value = col.Items[0];
			col = m_gridExpressions.Columns[kTypeCol] as DataGridViewComboBoxColumn;
			e.Row.Cells[kTypeCol].Value = col.Items[0];
			e.Row.Cells[kDeleteCol].Value = Properties.Resources.DeleteNormal;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleExpressionsGridCellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex == m_gridExpressions.NewRowIndex || e.RowIndex < 0 || e.ColumnIndex < 0 ||
				m_gridExpressions.Columns[e.ColumnIndex].Name != kDeleteCol)
			{
				return;
			}

			m_gridExpressions[e.ColumnIndex, e.RowIndex].Value = Properties.Resources.DeleteHot;

			var toolTip = App.L10NMngr.LocalizeString(
				"FiltersDlg.DeleteFilterExpressionToolTip",
				"Delete Expression", App.kLocalizationGroupDialogs);

			var pt = PointToClient(MousePosition);
			pt.Offset(0, Cursor.Size.Height + 20);
			m_tooltip.Show(toolTip, this, pt);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleExpressionsGridCellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			m_tooltip.Hide(this);
			
			if (e.RowIndex != m_gridExpressions.NewRowIndex && e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
				m_gridExpressions.Columns[e.ColumnIndex].Name == kDeleteCol)
			{
				m_gridExpressions[e.ColumnIndex, e.RowIndex].Value = Properties.Resources.DeleteNormal;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleExpressionsGridCellContentClicked(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != m_gridExpressions.NewRowIndex && e.RowIndex >= 0 &&
				m_gridExpressions.Columns[e.ColumnIndex].Name == kDeleteCol)
			{
				DeleteExpression(e.RowIndex);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void DeleteExpression(int rowIndex)
		{
			if (rowIndex < 0 || rowIndex == m_gridExpressions.NewRowIndex)
			{
				System.Media.SystemSounds.Beep.Play();
				return;
			}

			m_gridExpressions.Rows.RemoveAt(rowIndex);
			while (rowIndex > 0 && rowIndex >= m_gridExpressions.RowCount) rowIndex--;
			m_gridExpressions.CurrentCell = m_gridExpressions[0, rowIndex];
		}
	
		/// ------------------------------------------------------------------------------------
		private void HandleExpressionsGridKeyDown(object sender, KeyEventArgs e)
		{
			DataGridViewCell cell = m_gridExpressions.CurrentCell;

			if (cell != null && cell.ColumnIndex == 2 && e.Alt && e.KeyCode == Keys.Down)
			{
				e.Handled = true;
				HandleExpressionsGridValueColumnButtonClicked(m_gridExpressions,
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
		private void HandleExpressionsGridValueColumnButtonClicked(object sender, DataGridViewCellMouseEventArgs e)
		{
			string expType = m_gridExpressions[kTypeCol, e.RowIndex].Value as string;

			if (string.IsNullOrEmpty(expType))
				return;

			if ((m_gridExpressions[kFieldCol, e.RowIndex].Value as string) == FilterExpression.OtherFilterField)
			{
				if (FilterHelper.Filters.Count != 0)
					m_filterDropDown.Show(m_gridExpressions[kValueCol, e.RowIndex]);
			}
			else if (expType == m_expTypeToText[Filter.ExpressionType.PhoneticSrchPtrn])
			{
				SearchQuery query = m_gridExpressions[kTypeCol, e.RowIndex].Tag as SearchQuery;
				m_queryOptionsDropDown.SearchQuery = query ?? new SearchQuery();
				m_queryOptionsDropDownHost = new CustomDropDown();
				m_queryOptionsDropDownHost.Closed += m_queryDropDown_Closed;
				m_queryOptionsDropDownHost.AddControl(m_queryOptionsDropDown);
				Rectangle rc = m_gridExpressions.GetCellDisplayRectangle(2, e.RowIndex, false);
				rc.Y += rc.Height;
				m_queryOptionsDropDownHost.Show(m_gridExpressions.PointToScreen(rc.Location));
			}
			else
			{
				m_filterDropDown.Show(m_gridExpressions[kValueCol, e.RowIndex],
					m_gridExpressions[kFieldCol, e.RowIndex].Value as string);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void m_queryDropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			m_gridExpressions[kTypeCol, m_gridExpressions.CurrentCellAddress.Y].Tag = m_queryOptionsDropDown.SearchQuery;
			m_queryOptionsDropDownHost.Closed -= m_queryDropDown_Closed;
			m_queryOptionsDropDownHost = null;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleExpressionsGridRowValidated(object sender, DataGridViewCellEventArgs e)
		{
			Filter filter = CurrentFilter;

			if (filter != null)
			{
				// Save all the expressions for the current filter.
				filter.Expressions.Clear();
				foreach (DataGridViewRow row in m_gridExpressions.Rows)
				{
					if (row.Index != m_gridExpressions.NewRowIndex)
						filter.Expressions.Add(GetExpressionFromRow(row));
				}
			}

			m_gridExpressions.IsDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleExpressionsGridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && m_gridExpressions.Columns[e.ColumnIndex].Name == kDeleteCol)
			{
				e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
				e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
				e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
				e.CellStyle.SelectionBackColor = m_gridExpressions.BackgroundColor;
			}
		}

		/// ------------------------------------------------------------------------------------
		void HandleExpressionsGridCurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			if (m_gridExpressions.CurrentCell == null)
				return;

			int row = m_gridExpressions.CurrentCell.RowIndex;
			int col = m_gridExpressions.CurrentCell.ColumnIndex;

			// Commit the edit if the column is one of the combo box columns.
			if (col == 0 || col == 1 || col == 3)
			{
				m_gridExpressions.CurrentCellDirtyStateChanged -= HandleExpressionsGridCurrentCellDirtyStateChanged;
				m_gridExpressions.CommitEdit(DataGridViewDataErrorContexts.Commit);
				m_gridExpressions.CurrentCellDirtyStateChanged += HandleExpressionsGridCurrentCellDirtyStateChanged;
			}

			// Get the expression type from the type column.
			string expType = m_gridExpressions[kTypeCol, row].Value as string;
			if (string.IsNullOrEmpty(expType) || col != 3)
				return;

			if (m_textToExpType[expType] == Filter.ExpressionType.PhoneticSrchPtrn)
			{
				// When the expression type is a phonetic search, create a search query
				// object in which the expression's search query options will be stored.
				// These are the options that will be displayed on the search query options
				// drop-down when the user clicks on this row's (i.e. e.RowIndex) value
				// column drop-down button.
				if (m_gridExpressions[kTypeCol, row].Tag == null)
					m_gridExpressions[kTypeCol, row].Tag = new SearchQuery();

				// Force the field to be phonetic and the operation to be a match, then
				// set those cells to readonly because those values are the only valid
				// ones for the phonetic search pattern expression type.
				m_gridExpressions[kFieldCol, row].Value = App.FieldInfo.PhoneticField.DisplayText;
				m_gridExpressions[kOpCol, row].Value = m_operatorToText[Filter.Operator.Matches];
				m_gridExpressions[kFieldCol, row].ReadOnly = true;
				m_gridExpressions[kOpCol, row].ReadOnly = true;
			}
			else if (m_textToExpType[expType] == Filter.ExpressionType.RegExp)
			{
				// Force the operation to be match, since that's the only valid operation
				// for regular exp. expression types. Then make sure the field cell is
				// editable, but not the operation cell.
				m_gridExpressions[kOpCol, row].Value = m_operatorToText[Filter.Operator.Matches];
				m_gridExpressions[kFieldCol, row].ReadOnly = false;
				m_gridExpressions[kOpCol, row].ReadOnly = true;
			}
			else
			{
				// The expression type is normal, so make sure the field and operation
				// cells are editable.
				m_gridExpressions[kFieldCol, row].ReadOnly = false;
				m_gridExpressions[kOpCol, row].ReadOnly = false;
			}
		}

		#endregion

		#region Filter ListView control events

		/// ------------------------------------------------------------------------------------
		private void lvFilters_KeyDown(object sender, KeyEventArgs e)
		{
			//if (e.KeyCode == Keys.F2 && lvFilters.FocusedItem != null)
			//    lvFilters.FocusedItem.BeginEdit();
			//else if (e.KeyCode == Keys.Delete)
			//    HandleButtonRemoveClick(null, null);
		}

		#endregion

		#region Add, Copy, Remove button events
		/// ------------------------------------------------------------------------------------
		private void HandleButtonAddClick(object sender, EventArgs e)
		{
			AddNewFilter(new Filter());
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonCopyClick(object sender, EventArgs e)
		{
			if (CurrentFilter != null)
				AddNewFilter(CurrentFilter.Clone());
		}

		/// ------------------------------------------------------------------------------------
		private void AddNewFilter(Filter newFilter)
		{
			newFilter.Name = GetNewFilterName(newFilter.Name);
			newFilter.ShowInToolbarList = true;
			m_dirty = true;

			int i = m_gridFilters.Rows.Add(newFilter.Name, true);
			m_gridFilters.Rows[i].Tag = newFilter;
			m_gridFilters.CurrentCell = m_gridFilters[kFilterNameCol, i];
			m_gridFilters.BeginEdit(true);

			UpdateView();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonRemoveClick(object sender, EventArgs e)
		{
			if (m_gridFilters.CurrentRow == null)
				return;

			int index = m_gridFilters.CurrentRow.Index;
			m_gridFilters.Rows.RemoveAt(index);
			while (index >= m_gridFilters.RowCount)
				index--;

			m_gridFilters.Focus();
			m_dirty = true;

			if (index < 0)
				m_gridExpressions.Rows.Clear();
			else
				m_gridFilters.CurrentCell = m_gridFilters[kFilterNameCol, index];

			UpdateView();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void HandleButtonApplyNowClick(object sender, EventArgs e)
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
		protected override bool IsDirty
		{
			get { return m_dirty || m_gridExpressions.IsDirty || m_gridFilters.IsDirty; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.FiltersDlgFiltersGrid = GridSettings.Create(m_gridFilters);
			Settings.Default.FiltersDlgExpressionsGrid = GridSettings.Create(m_gridExpressions);
			Settings.Default.FiltersDlgSplitLoc = splitFilters.SplitterDistance;
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			// Commit pending changes in the grid.
			m_gridExpressions.CommitEdit(DataGridViewDataErrorContexts.Commit);

			foreach (DataGridViewRow row in m_gridFilters.Rows)
			{
				Filter filter = row.Tag as Filter;
				if (filter != null)
				{
					foreach (FilterExpression expression in filter.Expressions)
					{
						// Make sure expressions based on phonetic search patterns are valid.
						if (expression.ExpressionType == Filter.ExpressionType.PhoneticSrchPtrn &&
							FilterHelper.CheckSearchQuery(expression.SearchQuery, true) == null)
						{
							m_gridFilters.CurrentCell = m_gridFilters[kFilterNameCol, row.Index];
							m_gridExpressions.Focus();
							return false;
						}
					}
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			FilterHelper.Filters.Clear();

			foreach (DataGridViewRow row in m_gridFilters.Rows)
				FilterHelper.Filters.Add(row.Tag as Filter);

			// TODO: Validate expressions with search queries.
			
			FilterHelper.SaveFilters();
			m_dirty = false;
			m_gridExpressions.IsDirty = false;
			m_applyFilterOnClose = true;
			return true;
		}

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
		private bool FilterNameExists(string filterName)
		{
			foreach (DataGridViewRow row in m_gridFilters.Rows)
			{
				Filter filter = row.Tag as Filter;
				if (filter != null && filter.Name == filterName)
					return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		private Filter CurrentFilter
		{
			get { return (m_gridFilters.CurrentRow != null ? m_gridFilters.CurrentRow.Tag as Filter : null); }
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridEnter(object sender, EventArgs e)
		{
			UpdateView();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridLeave(object sender, EventArgs e)
		{
			UpdateView();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCurrentRowChanged(object sender, EventArgs e)
		{
			UpdateView();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateView()
		{
			UpdateView(true);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateView(bool updateExpressionMatchCombo)
		{
			Utils.SetWindowRedraw(this, false);

			var filter = CurrentFilter;
			m_gridExpressions.Enabled = (filter != null);
			btnDeleteFilter.Enabled = (filter != null);
			btnCopy.Enabled = (filter != null);
			btnApplyNow.Enabled = (filter != null);

			flowLayoutPanel.Visible = (filter != null);

			if (filter != null)
			{
				if (updateExpressionMatchCombo)
				{
					cboExpressionMatch.SelectedIndexChanged -= HandleExpressionMatchComboIndexChanged;
					cboExpressionMatch.SelectedIndex = (filter.MatchAny ? 0 : 1);
					cboExpressionMatch.SelectedIndexChanged += HandleExpressionMatchComboIndexChanged;
				}
				
				var text = lblExpressionMatchMsgPart1.Tag as string;
				lblExpressionMatchMsgPart1.Text = string.Format(text, filter.Name);
			}

			Utils.SetWindowRedraw(this, true);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleExpressionMatchComboIndexChanged(object sender, EventArgs e)
		{
			if (CurrentFilter != null)
				CurrentFilter.MatchAny = (cboExpressionMatch.SelectedIndex == 0);
	
			UpdateView(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the filter name to include the filter icon. I could have used an image column
		/// for this, but I wanted the filter name column heading to be aligned all the way
		/// left. So the image is drawn in the same column with its name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFilterGridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
				m_gridFilters.Columns[e.ColumnIndex].Name == kFilterNameCol)
			{
				var paintParts = e.PaintParts;
				var rc = e.CellBounds;
				paintParts &= ~DataGridViewPaintParts.ContentForeground;
				e.Paint(rc, paintParts);

				bool showInToolbarList = (bool)m_gridFilters[kShowInListCol, e.RowIndex].Value;
				var img = (showInToolbarList ? Properties.Resources.kimidFilter : Properties.Resources.kimidGrayFilter);
				int dy = (rc.Height - img.Height) / 2;
				e.Graphics.DrawImage(img, rc.X, rc.Y + dy, img.Width, img.Height);

				rc.Width -= (img.Width + 2);
				rc.X += (img.Width + 2);

				bool selected = (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected;
				var clrText = (selected ? e.CellStyle.SelectionForeColor : e.CellStyle.ForeColor);
				const TextFormatFlags flags = TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter |
					TextFormatFlags.SingleLine;
				
				TextRenderer.DrawText(e.Graphics, e.Value as string, m_gridFilters.Font, rc, clrText, flags);
				e.Handled = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFilterGridCellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex < 0 || e.RowIndex < 0)
				return;
			
			if (m_gridFilters.Columns[e.ColumnIndex].Name == kShowInListCol)
			{
				m_gridFilters.InvalidateRow(e.RowIndex);
				var filter = m_gridFilters.Rows[e.RowIndex].Tag as Filter;
				filter.ShowInToolbarList = (bool)m_gridFilters[kShowInListCol, e.RowIndex].Value;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFilterGridCurrentRowChanged(object sender, EventArgs e)
		{
			LoadExpressions(CurrentFilter);
			UpdateView();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFilterGridCellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			var filter = m_gridFilters.Rows[e.RowIndex].Tag as Filter;

			if (m_gridFilters.Columns[e.ColumnIndex].Name == kShowInListCol)
			{
				bool showInList = (bool)m_gridFilters[kShowInListCol, e.RowIndex].Value;
				if (showInList != filter.ShowInToolbarList)
					m_dirty = true;
				filter.ShowInToolbarList = showInList;
				return;
			}

			string newName = m_gridFilters[kFilterNameCol, e.RowIndex].Value as string;
			if (filter.Name != newName)
			{
				m_dirty = true;
				filter.Name = newName;
				UpdateView();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFilterGridCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			if (m_gridFilters.Columns[e.ColumnIndex].Name != kFilterNameCol)
				return;

			var filter = m_gridFilters.Rows[e.RowIndex].Tag as Filter;
			var newName = e.FormattedValue as string;
			if (filter.Name == newName)
				return;

			if (FilterNameExists(newName))
			{
				var msg = App.L10NMngr.LocalizeString("FiltersDlg.FilterNameExistsMsg",
					"The filter '{0}' already exists.",
					"Message displayed when adding a filter in the define filters dialog if the filter name already exists.",
					App.kLocalizationGroupDialogs);

				Utils.MsgBox(string.Format(msg, newName));
				e.Cancel = true;
			}
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
