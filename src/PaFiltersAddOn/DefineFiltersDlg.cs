using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Dialogs;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.FiltersAddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class DefineFiltersDlg : OKCancelDlgBase
	{
		private string m_currFilterName;
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
		public DefineFiltersDlg(string currFilterName) : this()
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

			//if (lvFilters.SelectedItems.Count > 0)
			//    lvFilters.FocusedItem = lvFilters.SelectedItems[0];

			//lvFilters_ItemSelectionChanged(null, null);
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
			m_grid.AllowUserToDeleteRows = true;
			m_grid.AllowUserToOrderColumns = false;
			m_grid.AllowUserToResizeColumns = true;
			m_grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

			List<string> fieldNames = new List<string>();
			foreach (PaFieldInfo fieldInfo in PaApp.FieldInfo)
				fieldNames.Add(fieldInfo.DisplayText);

			// Add the "field" that represents another filter, rather than a field in the data.
			fieldNames.Add(FilterExpression.OtherFilterField);

			DataGridViewColumn col = SilGrid.CreateDropDownListComboBoxColumn("Field", fieldNames);
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			((DataGridViewComboBoxColumn)col).DropDownWidth = 135;
			((DataGridViewComboBoxColumn)col).MaxDropDownItems = 15;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateDropDownListComboBoxColumn("Operator", new List<string>(m_operatorToText.Values));
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			((DataGridViewComboBoxColumn)col).DropDownWidth = 150;
			((DataGridViewComboBoxColumn)col).MaxDropDownItems = 15;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("Value");
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateDropDownListComboBoxColumn("Type", new List<string>(m_expTypeToText.Values));
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
		private void m_grid_ColumnHeadersHeightChanged(object sender, EventArgs e)
		{
			hlblFilters.Height = m_grid.ColumnHeadersHeight;
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
			m_grid.Rows.Clear();

			if (filter == null || filter.Expressions.Count == 0)
				return;

			foreach (FilterExpression expression in filter.Expressions)
			{
				string fieldName = expression.FieldName;
				if (fieldName != FilterExpression.OtherFilterField)
					fieldName = PaApp.FieldInfo[fieldName].DisplayText;

				m_grid.Rows.Add(fieldName, m_operatorToText[expression.Operator],
					expression.Pattern,	m_expTypeToText[expression.ExpressionType]);
			}

			m_grid.RowValidated += m_grid_RowValidated;
			m_grid.IsDirty = wasDirty;
		}

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
		private void m_grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 2 && e.CellStyle.Font != m_grid.Columns[2].DefaultCellStyle.Font)
				e.CellStyle.Font = m_grid.Columns[2].DefaultCellStyle.Font;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			DataGridViewComboBoxColumn col = m_grid.Columns[0] as DataGridViewComboBoxColumn;
			e.Row.Cells[0].Value = col.Items[0];
			col = m_grid.Columns[1] as DataGridViewComboBoxColumn;
			e.Row.Cells[1].Value = col.Items[0];
			col = m_grid.Columns[3] as DataGridViewComboBoxColumn;
			e.Row.Cells[3].Value = col.Items[0];
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
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_RowValidated(object sender, DataGridViewCellEventArgs e)
		{

		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private FilterExpression GetExpressionFromRow(DataGridViewRow row)
		{
			FilterExpression expression = new FilterExpression();

			string fieldName = row.Cells[0].Value as string;
			
			if (fieldName != FilterExpression.OtherFilterField)
			{
				PaFieldInfo fieldInfo =
					PaApp.FieldInfo.GetFieldFromDisplayText(fieldName);

				if (fieldInfo != null)
					fieldName = fieldInfo.FieldName;
			}

			expression.FieldName = fieldName;
			expression.Operator = m_textToOperator[row.Cells[1].Value as string];
			expression.Pattern = row.Cells[2].Value as string;
			expression.ExpressionType = m_textToExpType[row.Cells[3].Value as string];
			return expression;
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

			if (!IsDirty && !m_grid.IsDirty || DialogResult != DialogResult.OK)
				return;

			PaFiltersList filterList = new PaFiltersList();
			foreach (ListViewItem item in lvFilters.Items)
			{
				PaFilter filter = item.Tag as PaFilter;
				if (filter != null)
					filterList.Add(filter);
			}

			filterList.Save();
			PaApp.MsgMediator.SendMessage("FilterListUpdated", null);
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
			get {return (lvFilters.FocusedItem == null ? null : lvFilters.FocusedItem.Tag as PaFilter);}
		}
	}
}
