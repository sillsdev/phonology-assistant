using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;
using SIL.Pa.Resources;
using SIL.SpeechTools.Database;
using SIL.Pa.Controls;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class FiltersDialog : Form
	{
		private Dictionary<int, int> m_documentOccurrences;
		private ArrayList m_filters;
		bool m_selectedFilter = false;
		bool m_isDirty = false;
		bool m_initiated_by_app = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FiltersDialog()
		{
			InitializeComponent();
			LoadFilterNamesView();
			tvCatAndFldr.ImageList = ResourceHelper.TreeImages;
			LoadFilterContentTree();
			clbDialects.DataSource = DBUtils.CachedDialectList;

			PaApp.SettingsHandler.LoadFormProperties(this);

			splitContainer1.SplitterDistance =
				PaApp.SettingsHandler.GetIntWindowValue(Name, "splitter1",
				splitContainer1.SplitterDistance);

			splitContainer2.SplitterDistance =
				PaApp.SettingsHandler.GetIntWindowValue(Name, "splitter2",
				splitContainer2.SplitterDistance);

			// Select the first filter if one exists
			if (lvNames.Items.Count > 0)
				lvNames.Items[0].Selected = true;

			lblFilters.Font = FontHelper.UIFont;
			lblCatAndFldrs.Font = FontHelper.UIFont;
			lblDialects.Font = FontHelper.UIFont;
			lvNames.Font = FontHelper.UIFont;
			tvCatAndFldr.Font = FontHelper.UIFont;
			clbDialects.Font = FontHelper.UIFont;
		}

		#region Overridden Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Ask user if he wants to save changes if user didn't click the cancel button.
		/// </summary>
		/// <param name="e">FormClosingEventArgs</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			if (e.Cancel)
				return;

			// Treat any close, except via the cancel button, the same as clicking the OK button.
			if (e.CloseReason == CloseReason.UserClosing)
				DialogResult = DialogResult.OK;

			if (m_isDirty && DialogResult == DialogResult.OK)
			{
				DialogResult result = DialogResult.Yes;

				result = MessageBox.Show(this,
					ResourceHelper.GetString("kstidSaveFeatureChangesMsg"),
					Application.ProductName, MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Question);

				if (result == DialogResult.Cancel)
					e.Cancel = true;
				else if (result == DialogResult.Yes)
				{
					// Delete all records in the Filter table
					Filter.DeleteAllFilters();

					// Save each filter
					foreach (Filter filter in m_filters)
						filter.Save();
				}
			}

			if (!e.Cancel)
			{
				PaApp.SettingsHandler.SaveFormProperties(this);
				PaApp.SettingsHandler.SaveWindowValue(Name, "splitter1", splitContainer1.SplitterDistance);
				PaApp.SettingsHandler.SaveWindowValue(Name, "splitter2", splitContainer2.SplitterDistance);
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create a document tree node subordinate to the specified folder node.
		/// </summary>
		/// <param name="fldrNode">PaTreeNode</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void AddDocumentNode(PaTreeNode fldrNode)
		{
			using (OleDbDataReader reader = DBUtils.GetQueryResultsFromDB("qryDocumentsForFolder", fldrNode.Id))
			{
				while (reader.Read())
				{
					PaTreeNode node = PaTreeNode.CreateDocumentNode(
						(string)reader[DBFields.DocTitle], (int)reader[DBFields.DocLinkId],
						(int)reader[DBFields.DocId], (bool)reader[DBFields.Wave]);

					if (m_documentOccurrences[node.Id] > 1)
					{
						// TODO: Get custom query for these. Also change the icons for copies;
						node.ForeColor = Color.DarkBlue;
						node.NodeFont = new Font(tvCatAndFldr.Font, FontStyle.Bold);
					}
					fldrNode.Nodes.Add(node);
				}

				reader.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a Folder node subordinate to the specified category node.
		/// </summary>
		/// <param name="catNode">PaTreeNode</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void AddFolderNode(PaTreeNode catNode)
		{
			bool folderExists = false;

			using (OleDbDataReader reader =
				DBUtils.GetQueryResultsFromDB("qryFoldersForCategory", catNode.Id))
			{
				while (reader.Read())
				{
					PaTreeNode node = PaTreeNode.CreateFolderNode(
						(string)reader[DBFields.FolderTitle], (int)reader[DBFields.FolderId]);

					folderExists = true;
					catNode.Nodes.Add(node);

					if ((bool)reader["FExp"])
						node.Expand();
					else
						node.Collapse();
				}

				reader.Close();
			}

			// Remove the Category if there are no Folders
			if (folderExists == false)
				tvCatAndFldr.Nodes.Remove(catNode);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the content tree from the DB
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void LoadFilterContentTree()
		{
			tvCatAndFldr.BeginUpdate();
			tvCatAndFldr.Nodes.Clear();

			m_documentOccurrences = DBUtils.GetDocumentLinkCounts();

			string sql = DBUtils.SelectSQL("Category") + " ORDER BY CatTitle";
			using (OleDbDataReader reader = DBUtils.GetSQLResultsFromDB(sql))
			{
				while (reader.Read())
				{
					PaTreeNode node = PaTreeNode.CreateCategoryNode(
						(string)reader[DBFields.CatTitle], (int)reader[DBFields.CatId]);

					tvCatAndFldr.Nodes.Add(node);
					AddFolderNode(node);

					if ((bool)reader["CExp"])
						node.Expand();
					else
						node.Collapse();
				}

				reader.Close();
			}

			m_documentOccurrences = null;
			tvCatAndFldr.EndUpdate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Keep track of which categories and/or folders are selected.
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void tvCatAndFldr_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (m_initiated_by_app)
				return;

			PaTreeNode currNode = e.Node as PaTreeNode;
			if (currNode == null)
				return;

			m_isDirty = true;

			Filter currentFilter = lvNames.SelectedItems[0].Tag as Filter;
			
			if (currNode.NodeType == PaTreeNodeType.Folder)
			{
				if (currNode.Checked)
					currentFilter.Folders.Add(currNode.Id);
				else
					currentFilter.Folders.Remove(currNode.Id);
			}
			else
			{
				foreach (PaTreeNode folder in currNode.Nodes)
				{
					if (currNode.Checked)
						currentFilter.Folders.Add(folder.Id);
					else
						currentFilter.Folders.Remove(folder.Id);
				}
			}

			SetCategoryFolderCheckBoxes(currentFilter);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the names ListView pane from the DB.
		/// </summary>
		/// <returns>ArrayList of loaded filters</returns>
		/// ------------------------------------------------------------------------------------
		private ArrayList LoadFilterNamesView()
		{
			m_filters = Filter.Load();

			foreach (Filter filter in m_filters)
			{
				ListViewItem lvi = new ListViewItem(filter.ToString());
				lvi.Tag = filter;
				lvNames.Items.Add(lvi);
			}

			if (lvNames.Items.Count > 0)
				lvNames.Items[0].Selected = true;

			return m_filters;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the categories/folders for the selected filter.
		/// </summary>
		/// <param name="selectedFilter">Filter</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void SetCategoryFolderCheckBoxes(Filter selectedFilter)
		{
			m_initiated_by_app = true;

			foreach (PaTreeNode category in tvCatAndFldr.Nodes)
			{
				if (category.Nodes == null)
					continue;

				// Check the category if all its folder nodes are checked
				int checkedCount = 0;
				foreach (PaTreeNode folder in category.Nodes)
				{
					folder.Checked = selectedFilter.Folders.Contains(folder.Id);

					if (folder.Checked)
						checkedCount++;
				}

				category.Checked = (checkedCount == category.Nodes.Count);
			}

			m_initiated_by_app = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check the checkBox if matches the selected filter's dialect(s).
		/// </summary>
		/// <param name="selectedFilter">Filter</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void SetDialectCheckBoxes(Filter selectedFilter)
		{
			m_initiated_by_app = true;
			Filter currentFilter = lvNames.SelectedItems[0].Tag as Filter;

			// Uncheck all checkBoxes
			for (int i = 0; i < clbDialects.Items.Count; i++)
				clbDialects.SetItemChecked(i, false);

			// Check the checkBox if matches the filter's dialect(s)
			ArrayList dialects = selectedFilter.Dialects;

			foreach (string dialect in dialects)
			{
				for (int i = 0; i < clbDialects.Items.Count; i++)
				{
					if (dialect == (string)clbDialects.Items[i])
						clbDialects.SetItemChecked(i, true);
				}
			}

			// Check or uncheck the "(none)" dialect based on if all the other
			// dialects are checked or not
			if (clbDialects.CheckedItems.Count == clbDialects.Items.Count)
			{
				clbDialects.SetItemChecked(0, true);
				if (!currentFilter.Dialects.Contains("(none)"))
					currentFilter.Dialects.Add("(none)");
			}
			else
			{
				clbDialects.SetItemChecked(0, false);
				currentFilter.Dialects.Remove("(none)");
			}

			m_initiated_by_app = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the categories/folders & dialects for the selected filter.
		/// </summary>
		/// <param name="selectedFilter">Filter</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void updateFoldersAndDialects(Filter selectedFilter)
		{
			m_selectedFilter = true;
			SetCategoryFolderCheckBoxes(selectedFilter);

			SetDialectCheckBoxes(selectedFilter);
			m_selectedFilter = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the categories/folders & dialects for the selected filter.
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void lvNames_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_selectedFilter)
				return;

			splitContainer2.Enabled = (lvNames.SelectedItems.Count > 0);
			if (lvNames.SelectedItems.Count == 0)
				return;

			updateFoldersAndDialects(lvNames.SelectedItems[0].Tag as Filter);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds or removes the dialect based on whether the user checked or unchecked the box.
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void clbDialects_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (m_initiated_by_app)
				return;

			m_isDirty = true;

			Filter currentFilter = lvNames.SelectedItems[0].Tag as Filter;
			string dialectName = (string)clbDialects.Items[e.Index];
			bool bChecked = false;
			int checkedItemsCount = clbDialects.CheckedItems.Count;

			if (e.NewValue == CheckState.Checked)
			{
				bChecked = true;
				checkedItemsCount++;
				if (!currentFilter.Dialects.Contains(dialectName))
					currentFilter.Dialects.Add(dialectName);
			}
			else
				currentFilter.Dialects.Remove(dialectName);

			// Check or uncheck all dialects based on whether "(none)" is checked
			if (dialectName == "(none)")
			{
				for (int i = 1; i < clbDialects.Items.Count; i++)
					clbDialects.SetItemChecked(i, bChecked);
				return;
			}
			
			// Check or uncheck the "(none)" dialect based on if all the other
			// dialects are checked or not
			m_initiated_by_app = true;
			if (checkedItemsCount == (clbDialects.Items.Count - 1))
			{
				clbDialects.SetItemChecked(0, true);
				if (!currentFilter.Dialects.Contains("(none)"))
					currentFilter.Dialects.Add("(none)");
			}
			else
			{
				clbDialects.SetItemChecked(0, false);
				currentFilter.Dialects.Remove("(none)");
			}

			m_initiated_by_app = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Change the filter's name.
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void lvNames_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			Filter filter = lvNames.Items[e.Item].Tag as Filter;
			if ((filter != null) && (e.Label != null))
				filter.Name = e.Label;

			m_isDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new filter with a unique name (e.g., "Filter 0", "Filter 1", "Filter 2"...)
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void btnAdd_Click(object sender, EventArgs e)
		{
			m_isDirty = true;

			string baseFilterName = "Filter ";
			string newFilterName = "Filter";
			ArrayList filterNames = new ArrayList();

			// Create an array of filter name strings
			foreach (Filter filter in m_filters)
				filterNames.Add(filter.Name);

			// Create a new, unique filter name
			for (int i = 0; i < m_filters.Count; i++)
			{
				newFilterName = baseFilterName + i.ToString();
				if (!filterNames.Contains(newFilterName))
					break;
			}

			Filter newFilter = new Filter();
			newFilter.Name = newFilterName;
			m_filters.Add(newFilter);

			ListViewItem lvi = new ListViewItem(newFilter.ToString());
			lvi.Tag = newFilter;
			lvNames.Items.Add(lvi);

			lvi.Selected = true;
			lvi.BeginEdit();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Delete the selected filter.
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (lvNames.SelectedItems.Count < 1)
				return;

			m_isDirty = true;

			int selectedIndex = lvNames.SelectedIndices[0];
			ListViewItem selectedItem = lvNames.SelectedItems[0];
			Filter selectedFilter = selectedItem.Tag as Filter;
			m_filters.Remove(selectedFilter);
			lvNames.Items.Remove(selectedItem);

			if (lvNames.Items.Count > 0)
			{
				if (lvNames.Items.Count == selectedIndex)
				{
					// If the deleted filter was the last one in the list
					// then uncheck all the category and folder nodes
					m_initiated_by_app = true;
					foreach (PaTreeNode category in tvCatAndFldr.Nodes)
					{
						category.Checked = false;

						if (category.Nodes == null)
							continue;

						foreach (PaTreeNode folder in category.Nodes)
							folder.Checked = false;
					}
					m_initiated_by_app = false;

					// Uncheck all dialect checkBoxes
					m_initiated_by_app = true;
					for (int i = 0; i < clbDialects.Items.Count; i++)
						clbDialects.SetItemChecked(i, false);
					m_initiated_by_app = false;
				}

				// Select the filter at the same location that
				// you just deleted the original filter at
				else
					lvNames.Items[selectedIndex].Selected = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Resize the filter names' column whenever the filters list view size changes
		/// when the splitter window is resized.
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void lvNames_Resize(object sender, EventArgs e)
		{
			this.columnHeader1.Width =
				lvNames.Width - SystemInformation.VerticalScrollBarWidth - 1;
		}
	}
}