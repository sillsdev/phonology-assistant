using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Dialogs;
using SIL.SpeechTools.Utils;
using SIL.Pa.Controls;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.FFSearchEngine;

namespace SIL.Pa.FiltersAddOn
{
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

		private int m_gridRowHeight = 0;
		private string m_currFilterName;
		private DropDownFiltersListBox m_filterDropDown;
		private CustomDropDown m_queryDropDown;
		private Dictionary<FilterOperator, string> m_operatorToText;
		private Dictionary<string, FilterOperator> m_textToOperator;
		private Dictionary<ExpressionType, string> m_expTypeToText;
		private Dictionary<string, ExpressionType> m_textToExpType;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DefineFiltersDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DefineFiltersDlg(string currFilterName)
			: this()
		{
			m_operatorToText = new Dictionary<FilterOperator, string>();
			m_textToOperator = new Dictionary<string, FilterOperator>();
			foreach (FilterOperator op in Enum.GetValues(typeof(FilterOperator)))
			{
				string enumName = Enum.GetName(typeof(FilterOperator), op);
				string enumDisplayText = Properties.Resources.ResourceManager.GetString("kstid" + enumName);
				m_operatorToText[op] = enumDisplayText;
				m_textToOperator[enumDisplayText] = op;
			}

			m_expTypeToText = new Dictionary<ExpressionType, string>();
			m_textToExpType = new Dictionary<string, ExpressionType>();
			foreach (ExpressionType op in Enum.GetValues(typeof(ExpressionType)))
			{
				string enumName = Enum.GetName(typeof(ExpressionType), op);
				string enumDisplayText = Properties.Resources.ResourceManager.GetString("kstid" + enumName);
				m_expTypeToText[op] = enumDisplayText;
				m_textToExpType[enumDisplayText] = op;
			}

			m_currFilterName = currFilterName;
			hlblFilters.Font = FontHelper.UIFont;
			lvFilters.Font = FontHelper.UIFont;
			m_grid.Font = FontHelper.UIFont;
			lblAndOr.Font = FontHelper.UIFont;
			rbAnd.Font = FontHelper.UIFont;
			rbOr.Font = FontHelper.UIFont;
			PaApp.SettingsHandler.LoadFormProperties(this);

			// Get rid of these three lines when there is a help topic for this dialog box.
			btnHelp.Visible = false;
			btnOK.Left = btnCancel.Left;
			btnCancel.Left = btnHelp.Left;

			splitFilter_SplitterMoved(null, null);

			rbAnd.Top = rbOr.Top = (int)(((decimal)pnlFilterOptions.Height - rbOr.Height) / 2);
			lblAndOr.Top = (int)(((decimal)pnlFilterOptions.Height - lblAndOr.Height) / 2);
			rbOr.Left = pnlFilterOptions.Width - rbOr.Width - 5;
			rbAnd.Left = rbOr.Left - rbAnd.Width - 5;
			lblAndOr.Left = rbAnd.Left - lblAndOr.Width - 5;

			m_filterDropDown = new DropDownFiltersListBox();
			BuildGrid();
			LoadFilters();
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
			PaFiltersList filterList = PaFiltersList.Load();
			ListViewItem currItem = null;

			foreach (PaFilter filter in filterList)
			{
				ListViewItem item = new ListViewItem(filter.Name);
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

			List<string> fieldNames = new List<string>();
			foreach (PaFieldInfo fieldInfo in PaApp.FieldInfo)
			{
				fieldNames.Add(fieldInfo.DisplayText);
				m_gridRowHeight = Math.Max(fieldInfo.Font.Height, m_gridRowHeight);
			}

			// Add the "field" that represents another filter, rather than a field in the data.
			fieldNames.Add(FilterExpression.OtherFilterField);

			DataGridViewColumn col = SilGrid.CreateDropDownListComboBoxColumn(kFieldCol, fieldNames);
			col.HeaderText = Properties.Resources.kstidFieldColHdgText;
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			((DataGridViewComboBoxColumn)col).DropDownWidth = 135;
			((DataGridViewComboBoxColumn)col).MaxDropDownItems = 15;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateDropDownListComboBoxColumn(kOpCol, new List<string>(m_operatorToText.Values));
			col.HeaderText = Properties.Resources.kstidOperatorColHdgText;
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			((DataGridViewComboBoxColumn)col).DropDownWidth = 150;
			((DataGridViewComboBoxColumn)col).MaxDropDownItems = 15;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateSilButtonColumn(kValueCol);
			((SilButtonColumn)col).UseComboButtonStyle = true;
			((SilButtonColumn)col).ButtonClicked += HandleValueColumnButtonClicked;
			col.HeaderText = Properties.Resources.kstidValueColHdgText;
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateDropDownListComboBoxColumn(kTypeCol, new List<string>(m_expTypeToText.Values));
			col.HeaderText = Properties.Resources.kstidTypeColHdgText;
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
		private void LoadExpressions(PaFilter filter)
		{
			bool wasDirty = m_grid.IsDirty;
			m_grid.RowValidated -= m_grid_RowValidated;
			m_grid.CellValidated -= m_grid_CellValidated;
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
			m_grid.CellValidated += m_grid_CellValidated;
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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleValueColumnButtonClicked(object sender, DataGridViewCellMouseEventArgs e)
		{
			string expType = m_grid[kTypeCol, e.RowIndex].Value as string;

			if (string.IsNullOrEmpty(expType))
				return;

			if ((m_grid[kFieldCol, e.RowIndex].Value as string) == FilterExpression.OtherFilterField)
			{
				if (FilterHelper.FilterList.Count != 0)
					m_filterDropDown.Show(m_grid[kValueCol, e.RowIndex]);
			}
			else if (expType == m_expTypeToText[ExpressionType.PhoneticSrchPtrn])
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
			PaFilter filter = CurrentFilter;

			if (filter != null)
			{
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
		void m_grid_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
			string expType = m_grid[kTypeCol, e.RowIndex].Value as string;
			if (!string.IsNullOrEmpty(expType))
			{
				if (m_grid[kTypeCol, e.RowIndex].Tag == null)
					m_grid[kTypeCol, e.RowIndex].Tag = new SearchQuery();
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
			rbOr.CheckedChanged -= new EventHandler(HandleLogicalExpressionRelationshipChange);
			PaFilter filter = CurrentFilter;
			LoadExpressions(filter);
			if (filter != null)
			{
				rbAnd.Checked = !filter.OrExpressions;
				rbOr.Checked = filter.OrExpressions;
			}

			lblAndOr.Enabled = rbOr.Enabled = rbAnd.Enabled = (filter != null);
			rbOr.CheckedChanged += new EventHandler(HandleLogicalExpressionRelationshipChange);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lvFilters_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			PaFilter filter = lvFilters.Items[e.Item].Tag as PaFilter;
			if (filter == null || e.Label == null || filter.Name == e.Label)
				return;

			if (FilterNameExists(e.Label))
			{
				string msg = string.Format(Properties.Resources.kstidFilterNameExistsMsg, e.Label);
				STUtils.STMsgBox(msg);
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
			PaFilter newFilter = new PaFilter();
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
				PaFilter newFilter = CurrentFilter.Clone();
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
				btnRemove.Enabled = false;
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

			if (expression.ExpressionType == ExpressionType.PhoneticSrchPtrn)
			{
				SearchQuery query = row.Cells[kTypeCol].Tag as SearchQuery;
				expression.SearchQuery = (query == null ? new SearchQuery() : query);
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
			PaApp.SettingsHandler.SaveSettingsValue(Name, "split", splitFilters.SplitterDistance);
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
				PaFilter filter = item.Tag as PaFilter;
				if (filter != null)
				{
					foreach (FilterExpression expression in filter.Expressions)
					{
						if (expression.ExpressionType == ExpressionType.PhoneticSrchPtrn &&
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
			PaFiltersList filterList = new PaFiltersList();
			foreach (ListViewItem item in lvFilters.Items)
			{
				PaFilter filter = item.Tag as PaFilter;
				if (filter != null)
					filterList.Add(filter);
			}

			// TODO: Validate expressions with search queries.

			filterList.Save();
			PaApp.MsgMediator.SendMessage("FilterListUpdated", null);
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
				fmt = Properties.Resources.kstidCopiedNamePrefix;
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
				PaFilter filter = item.Tag as PaFilter;
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
		private PaFilter CurrentFilter
		{
			get { return (lvFilters.FocusedItem == null ? null : lvFilters.FocusedItem.Tag as PaFilter); }
		}
	}

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
			foreach (PaFilter filter in FilterHelper.FilterList)
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
			Height = (Math.Min(Items.Count, 10) * Font.Height) + 4;
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

			SortedDictionary<string, bool> list = new SortedDictionary<string, bool>();
			foreach (WordCacheEntry entry in PaApp.WordCache)
			{
				string val = entry[field];
				if (!string.IsNullOrEmpty(val))
					list[val] = true;
			}

			if (list.Count == 0)
				return;

			string cellValue = cell.Value as string;
			foreach (string val in list.Keys)
			{
				Items.Add(val);
				if (cellValue == val)
					SelectedIndex = Items.Count - 1;
			}

			if (SelectedIndex < 0)
				SelectedIndex = 0;

			m_cell = cell;
			int col = cell.ColumnIndex;
			int row = cell.RowIndex;
			Width = cell.DataGridView.Columns[col].Width;
			Height = (Math.Min(Items.Count, 10) * Font.Height) + 4;
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
