using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.PhoneticSearching;
using SilTools;
using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a tree resultView for find phone search categories and patterns.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SearchPatternTreeView : TreeView, IxCoreColleague
	{
		private bool m_allowModifications;
		private bool m_isForToolbarPopup;
		private bool m_copyCommand;
		private bool m_cutCommand;
		private bool m_pasteCommand;
		private SearchQuery m_patternClipboard;
		private ITMAdapter m_tmAdapter;
		private SlidingPanel m_slidingPanel;
		private XButton m_btnCopy;
		private XButton m_btnCut;
		private XButton m_btnPaste;
		private readonly ImageList m_images;
		private readonly int m_closedImage = 0;
		private readonly int m_openImage = 1;
		private readonly Label m_lblNoPatternsMsg;
		//private ToolTip m_tooltip;

		#region Construction and saving query and loading
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a search pattern tree resultView.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchPatternTreeView()
		{
			BorderStyle = BorderStyle.None;
			HideSelection = false;

			m_images = new ImageList();
			m_images.ColorDepth = ColorDepth.Depth32Bit;
			m_images.Images.Add(Properties.Resources.kimidSavedPatternCategoryClosed);
			m_images.Images.Add(Properties.Resources.kimidSavedPatternCategoryOpen);
			m_images.Images.Add(Properties.Resources.kimidPattern);
			ImageList = m_images;
			ImageKey = null;
			ImageIndex = -1;
			SelectedImageIndex = -1;

			m_lblNoPatternsMsg = new Label();
			m_lblNoPatternsMsg.Font = FontHelper.UIFont;
			m_lblNoPatternsMsg.AutoSize = false;
			m_lblNoPatternsMsg.Dock = DockStyle.Fill;
			m_lblNoPatternsMsg.TextAlign = ContentAlignment.MiddleCenter;
			m_lblNoPatternsMsg.Visible = false;

			m_lblNoPatternsMsg.Text = App.LocalizeString(
				"SearchVw.SavedSearchPatternsList.NoSavedSearchPatternsMsg",
				"No Saved Search Patterns",
				"Message shown in the saved search pattern list when there are on saved patterns.",
				App.kLocalizationGroupInfoMsg);

			Controls.Add(m_lblNoPatternsMsg);

			App.AddMediatorColleague(this);

			//m_tooltip = new ToolTip();
			//m_tooltip.OwnerDraw = true;
			//m_tooltip.Popup += new PopupEventHandler(m_tooltip_Popup);
			//m_tooltip.Draw += new DrawToolTipEventHandler(m_tooltip_Draw);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the expanded states of the categories.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveSettings()
		{
			int i = 0;

			foreach (TreeNode node in Nodes)
			{
				if (i < App.Project.SearchQueryGroups.Count)
				{

					if (m_isForToolbarPopup)
						App.Project.SearchQueryGroups[i++].ExpandedInPopup = node.IsExpanded;
					else
						App.Project.SearchQueryGroups[i++].Expanded = node.IsExpanded;
				}
			}

			App.Project.SearchQueryGroups.Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the tree resultView with Find Phone categories and search patterns from the
		/// database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			Nodes.Clear();

			foreach (SearchQueryGroup group in App.Project.SearchQueryGroups)
			{
				TreeNode categoryNode = new TreeNode(group.Name);
				categoryNode.Tag = group;
				Nodes.Add(categoryNode);

				if (group.Queries == null)
					continue;

				foreach (SearchQuery grpQuery in group.Queries)
				{
					// Create a query node.
					TreeNode node = new TreeNode();
					node.ImageIndex = node.SelectedImageIndex = 2;
					node.Text = grpQuery.ToString();
					node.Tag = grpQuery;
					node.Name = grpQuery.Id.ToString();
					categoryNode.Nodes.Add(node);
				}

				bool shouldExpand =
					(m_isForToolbarPopup ? group.ExpandedInPopup : group.Expanded);

				// Determine whether or not the category should be expanded.
				if (shouldExpand && !categoryNode.IsExpanded)
					categoryNode.Expand();
			}

			// Make the first node the current node.
			if (Nodes.Count == 0)
				m_lblNoPatternsMsg.Visible = true;
			else
			{
				UpdateEachPatternsCategory();
				SelectedNode = Nodes[0];
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tells the tree view what it's cut, copy and paste buttons are.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetCutCopyPasteButtons(XButton btnCut, XButton btnCopy, XButton btnPaste)
		{
			m_btnCut = btnCut;
			m_btnCopy = btnCopy;
			m_btnPaste = btnPaste;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the enabled state of the cut, copy and paste buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateButtons()
		{
			OnUpdateCutSavedPattern(null);
			OnUpdateCopySavedPattern(null);
			OnUpdatePasteSavedPattern(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes through each pattern node, updating each one's category to that of its
		/// parent node.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateEachPatternsCategory()
		{
			if (Nodes == null || Nodes.Count == 0)
				return;

			// Go through the category nodes.
			foreach (TreeNode categoryNode in Nodes)
			{
				if (categoryNode.Nodes != null)
				{
					// Go through the category's child nodes (i.e. pattern nodes).
					foreach (TreeNode patternNode in categoryNode.Nodes)
					{
						SearchQuery query = patternNode.Tag as SearchQuery;
						if (query != null)
							query.Category = categoryNode.Text;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified category exists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CategoryExists(string categoryName)
		{
			foreach (TreeNode node in Nodes)
			{
				if (node.Tag is SearchQueryGroup && node.Text == categoryName)
					return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks to see if the specified query already exists in the tree. The query's name
		/// and category are what's compared to those in the tree.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool PatternExists(SearchQuery query)
		{
			return (GetPatternsNode(query) != null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks to see if the specified query name exists in the specified category.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool PatternExists(string categoryName, string queryName)
		{
			return (GetPatternsNode(categoryName, queryName) != null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the node whose query namd and category matches that of the one specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TreeNode GetPatternsNode(string categoryName, string queryName)
		{
			if (string.IsNullOrEmpty(categoryName) || string.IsNullOrEmpty(queryName))
				return null;

			foreach (TreeNode categoryNode in Nodes)
			{
				// First find the category.
				if (categoryNode.Text == categoryName)
				{
					if (categoryNode.Nodes == null || categoryNode.Nodes.Count == 0)
						return null;

					// Next, find the query name.
					foreach (TreeNode patternNode in categoryNode.Nodes)
					{
						if (patternNode.Text == queryName)
							return patternNode;
					}
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the node whose query namd and category matches that of the one specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TreeNode GetPatternsNode(SearchQuery query)
		{
			if (query == null || Nodes == null || Nodes.Count == 0)
				return null;

			if (query.Id != 0)
			{
				TreeNode[] nodes = Nodes.Find(query.Id.ToString(), true);
				if (nodes.Length > 0)
					return nodes[0];
			}

			SearchQueryGroup group = App.Project.SearchQueryGroups.GetGroupFromQueryId(query.Id);
			return GetPatternsNode(group != null ? group.Name : query.Category, query.ToString());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Finds the node whose query name and category are the same as the name and category
		/// of the specified query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SavePattern(SearchQuery query)
		{
			TreeNode patternNode = GetPatternsNode(query);
			if (patternNode != null)
			{
				patternNode.Text = query.ToString();
				patternNode.Tag = query.Clone();
				App.Project.SearchQueryGroups.UpdateQuery(patternNode.Tag as SearchQuery);
			}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the query associated with the current node in the tree. If the current node
		/// is a category then null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery CurrentQuery
		{
			get { return (SelectedNode == null ? null : SelectedNode.Tag as SearchQuery); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not find phone categories and search
		/// patterns can be added or deleted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AllowDataModifications
		{
			get { return m_allowModifications; }
			set { m_allowModifications = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the control is being used in a
		/// toolbar popup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsForToolbarPopup
		{
			get { return m_isForToolbarPopup; }
			set { m_isForToolbarPopup = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the toolbar/menu adapter for the tree resultView.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ITMAdapter TMAdapter
		{
			get { return m_tmAdapter; }
			set { m_tmAdapter = value; }
		}

		#endregion

		#region Overridden methods (including Application Idle event)
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the font gets set appropriately;
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			Font = App.PhoneticFont;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Play nice by cleaning up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			App.RemoveMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Give the category node the correct image when closed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnAfterCollapse(TreeViewEventArgs e)
		{
			base.OnAfterCollapse(e);
			e.Node.ImageIndex = e.Node.SelectedImageIndex = m_closedImage;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Give the category node the correct image when opened.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnAfterExpand(TreeViewEventArgs e)
		{
			base.OnAfterExpand(e);
			e.Node.ImageIndex = e.Node.SelectedImageIndex = m_openImage;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Only allows label editing when on a category.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnAfterSelect(TreeViewEventArgs e)
		{
			base.OnAfterSelect(e);
			LabelEdit = (e.Node != null && m_allowModifications);
			UpdateButtons();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the new find phone category name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
		{
			// There's nothing to do if the node's text didn't change.
			if (e.Label != null && e.Node.Text != e.Label)
			{
				e.CancelEdit = !(e.Node.Level == 0 ?
					VerifyCategoryRename(e.Node, e.Label) :	VerifyQueryRename(e.Node, e.Label));

				if (!e.CancelEdit)
				{
					if (e.Node.Tag is SearchQueryGroup)
						AfterCategoryEdited(e.Node, e.Label);
					else if (e.Node.Tag is SearchQuery)
						AfterQueryNameEdited(e.Node.Tag as SearchQuery, e.Label);
				}
			}

			if (m_slidingPanel != null)
			{
				m_slidingPanel.Freeze = false;
				m_slidingPanel = null;
			}

			base.OnAfterLabelEdit(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not a renamed cateogory conflicts with an exisiting one.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool VerifyCategoryRename(TreeNode renamedNode, string newName)
		{
			if (newName == null || newName.Trim() == string.Empty)
			{
				SystemSounds.Beep.Play();
				return false;
			}

			foreach (TreeNode node in Nodes)
			{
				if (node.Tag is SearchQueryGroup && node != renamedNode && node.Text == newName)
				{
					var msg = App.LocalizeString(
						"SearchVw.SavedSearchPatternsList.DuplicateSearchCategoryMsg",
						"There is already a category named '{0}'.", App.kLocalizationGroupInfoMsg);

					Utils.MsgBox(string.Format(msg, newName), MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation);

					return false;
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not a renamed search query conflicts with an exisiting one
		/// in the same category.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool VerifyQueryRename(TreeNode renamedNode, string newName)
		{
			if (newName == null || newName.Trim() == string.Empty)
			{
				SystemSounds.Beep.Play();
				return false;
			}

			if (renamedNode.Parent == null)
				return true;

			foreach (TreeNode node in renamedNode.Parent.Nodes)
			{
				if (node.Tag is SearchQuery && node != renamedNode && node.Text == newName)
				{
					var msg = App.LocalizeString(
						"SearchVw.SavedSearchPatternsList.DuplicateSearchQueryMsg",
						"There is already a saved search pattern named '{0}' in the same category.",
						App.kLocalizationGroupInfoMsg);

					Utils.MsgBox(string.Format(msg, newName), MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation);

					return false;
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the processing after renaming of a search query category (i.e. group).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AfterCategoryEdited(TreeNode editedNode, string newName)
		{
			((SearchQueryGroup)editedNode.Tag).Name = newName;
			UpdateEachPatternsCategory();
			App.Project.SearchQueryGroups.Save();

			// Remove the node whose label just changed and give the system a moment to
			// process the message before moving on to re-add the node in alphabetical
			// order. Otherwise, I've seen some strange things happen.
			Nodes.Remove(editedNode);
			Application.DoEvents();

			int insertIndex = Nodes.Count;

			// Now reinsert the node in the proper alphabetic order.
			foreach (TreeNode node in Nodes)
			{
				if ( string.Compare(node.Text, newName) > 0)
				{
					insertIndex = node.Index;
					break;
				}
			}

			Nodes.Insert(insertIndex, editedNode);
			SelectedNode = editedNode;

			// See comments on Idle delegate method below.
			Application.Idle += Application_Idle;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the processing after a search query is renamed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AfterQueryNameEdited(SearchQuery query, string newName)
		{
			query.Name = newName;
			App.Project.SearchQueryGroups.Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is a frustrating kludge. This method gets called after a category has been
		/// renamed. It gets called in order to set the focus to this tree control. The
		/// reason it doesn't work in the OnAfterLabelEdit event seems to have something
		/// to do with the fact that in that method I remove the node whose label has just
		/// been edited in order to reinsert it in alphabetic order. However, the effect of
		/// doing that causes the tree to act like it doesn't get focus events though it
		/// does. I tried calling the Focus() method in the OnAfterLabelEdit but to no avail.
		/// Grrr!
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void Application_Idle(object sender, EventArgs e)
		{
			// Give the tree focus and unsubscribe to the idle event.
			Focus();
			Application.Idle -= Application_Idle;
			UpdateButtons();
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected override void OnNodeMouseHover(TreeNodeMouseHoverEventArgs e)
		//{
		//    base.OnNodeMouseHover(e);

		//    if (e.Node != null && e.Node.Tag is SearchQuery)
		//    {
		//        SearchQuery query = e.Node.Tag as SearchQuery;
		//        if (query.Name != null)
		//        {
		//            m_tooltip.Tag = e.Node;
		//            m_tooltip.Show(query.Pattern, this);
		//            System.Diagnostics.Debug.WriteLine("Showing: " + query.Pattern);

		//        }
		//    }
		//    else
		//    {
		//        m_tooltip.Hide(this);
		//        m_tooltip.Tag = null;
		//    }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected override void OnMouseMove(MouseEventArgs e)
		//{
		//    base.OnMouseMove(e);

		//    TreeViewHitTestInfo tvhti = HitTest(e.Location);
		//    if (tvhti.Node == null || tvhti.Node != m_tooltip.Tag)
		//    {
		//        m_tooltip.Tag = null;
		//        m_tooltip.Hide(this);
		//    }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//void m_tooltip_Popup(object sender, PopupEventArgs e)
		//{
		//    TreeNode node = m_tooltip.Tag as TreeNode;
			
		//    if (node == null || !(node.Tag is SearchQuery))
		//    {
		//        e.Cancel = true;
		//        return;
		//    }

		//    e.ToolTipSize =	TextRenderer.MeasureText(
		//        (node.Tag as SearchQuery).Pattern,	App.PhoneticFont);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//void m_tooltip_Draw(object sender, DrawToolTipEventArgs e)
		//{
		//    e.DrawBackground();
		//    e.DrawBorder();

		//    TextRenderer.DrawText(e.Graphics, e.ToolTipText, App.PhoneticFont,
		//        e.Bounds, SystemColors.InfoText);
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make a right-click select a node, just as does a left-click.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeViewHitTestInfo htInfo = HitTest(e.Location);
				if (htInfo.Node != null)
					SelectedNode = htInfo.Node;
			}

			base.OnMouseClick(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDoubleClick(EventArgs e)
		{
			if (m_isForToolbarPopup)
			{
				try
				{
					App.MsgMediator.SendMessage("ViewSearch", SelectedNode.Tag as SearchQuery);
					App.TMAdapter.HideBarItemsPopup("tbbFindPhones");
				}
				catch { }
			}

			if (e != null)
				base.OnDoubleClick(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This solution to copy/cut/paste is strange. I monitor those keystrokes in the
		/// KeyDown event and handle them here and here's why. I tried handling everything in
		/// the KeyDown event but no matter what I did, I couldn't seem to eliminate the
		/// bonk sound the system makes because the tree control thinks group+C, group+V and
		/// group+X are invalid keystrokes for the tree. Then I tried to handle those key
		/// strokes in this method. Of course this method doesn't get called when the group
		/// key is pressed but I thought I would just set a flag in the KeyDown event when
		/// the control key was pressed (clearing it in the KeyUp event) and check it here
		/// when one of the letter keys is pressed. But when the control key is held down and
		/// a letter key pressed, the value of this method's e.KeyChar property is not the
		/// letter. It is a value I don't know how to interpret, nor have I seen any
		/// documentation about it. Grrr!
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter && m_isForToolbarPopup)
			{
				e.Handled = true;
				base.OnKeyPress(e);
				OnDoubleClick(null);
			}

			if (m_pasteCommand && m_patternClipboard != null)
			{
				// Handle pasting a pattern from the clipboard
				m_pasteCommand = false;
				AddPattern(m_patternClipboard, true);
				e.Handled = true;
			}
			else if ((m_copyCommand || m_cutCommand) && CurrentQuery != null)
			{
				// Get the query to copy or cut
				m_patternClipboard = CurrentQuery;
				if (m_cutCommand)
					DeletePattern(SelectedNode, false);

				m_copyCommand = false;
				m_cutCommand = false;
				e.Handled = true;
			}

			base.OnKeyPress(e);
			UpdateButtons();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.Handled || SelectedNode == null)
				return;

			if (e.KeyCode == Keys.F2 && LabelEdit)
				SelectedNode.BeginEdit();
			else if (e.KeyCode == Keys.X && SelectedNode.Level > 0)
				m_cutCommand = true;
			else if (e.KeyCode == Keys.C && e.Control && SelectedNode.Level > 0)
				m_copyCommand = true;
			else if (e.KeyCode == Keys.V && e.Control)
				m_pasteCommand = true;
			else if (e.KeyCode == Keys.Delete && m_allowModifications && !m_isForToolbarPopup)
			{
				if (SelectedNode.Level == 0)
					DeleteCategory(SelectedNode);
				else
					DeletePattern(SelectedNode, true);

				UpdateButtons();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnItemDrag(ItemDragEventArgs e)
		{
			if (IsForToolbarPopup)
				return;

			TreeNode node = e.Item as TreeNode;
			if (node != null && node.Level > 0 && node.Tag is SearchQuery &&
				e.Button == MouseButtons.Left)
			{
				DoDragDrop(node.Tag as SearchQuery, DragDropEffects.Copy | DragDropEffects.Move);
			}

			base.OnItemDrag(e);
		}

		#endregion

		#region Methods for deleting Nodes
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the specified category node.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DeleteCategory(TreeNode node)
		{
			// Make sure the node is a pattern node.
			if (node == null || node.Level > 0 || !m_allowModifications)
				return;

			SearchQueryGroup group = node.Tag as SearchQueryGroup;
			if (group == null)
				return;

			var msg = App.LocalizeString(
				"SearchVw.SavedSearchPatternsList.DeleteSearchPatternCategoryConfirmationMsg",
				"Are you sure you want to remove the search category '{0}'?",
				App.kLocalizationGroupInfoMsg);

			if (Utils.MsgBox(string.Format(msg, node.Text), MessageBoxButtons.YesNo) == DialogResult.No)
				return;

			// Remove group from cache.
			App.Project.SearchQueryGroups.Remove(group);
			App.Project.SearchQueryGroups.Save();

			// Remove group from tree.
			TreeNode newNode = GetNewSelectedNodeIfDeleted(node);
			Nodes.Remove(node);
			SelectedNode = newNode;

			// Show the message that tells the user there are no saved patterns.
			if (Nodes.Count == 0)
				m_lblNoPatternsMsg.Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the specified pattern node.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DeletePattern(TreeNode node, bool showQuestion)
		{
			// Make sure the node is a category node.
			if (node == null || node.Level == 0 || !m_allowModifications)
				return;

			if (showQuestion)
			{
				var msg = App.LocalizeString(
					"SearchVw.SavedSearchPatternsList.DeleteSearchPatternConfirmationMsg",
					"Are you sure you want to remove the search pattern '{0}'?",
					App.kLocalizationGroupInfoMsg);

				if (Utils.MsgBox(string.Format(msg, node.Text), MessageBoxButtons.YesNo) == DialogResult.No)
					return;
			}

			App.Project.SearchQueryGroups[node.Parent.Index].Queries.RemoveAt(node.Index);
			App.Project.SearchQueryGroups.Save();
			TreeNode newNode = GetNewSelectedNodeIfDeleted(node);
			Nodes.Remove(node);
			SelectedNode = newNode;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Figures out what the new node should be if the specified node is deleted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private TreeNode GetNewSelectedNodeIfDeleted(TreeNode node)
		{
			if (node != null && node.NextNode != null)
				return node.NextNode;

			if (node != null && node.PrevNode != null)
				return node.PrevNode;

			if (node != null && node.Level > 0)
				return node.Parent;

			return null;
		}

		#endregion

		#region Methods for adding categories and patterns to the tree
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new category to the database and the tree. If the sliding panel is not
		/// null then it must be frozen until editing the category name is finished.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddCategory(SlidingPanel slidingPanel, bool beginEditAfterAdding)
		{
			var msg = App.LocalizeString(
				"SearchVw.SavedSearchPatternsList.NewSavedPatternCategoryName",
				"New Category",
				"This is the default name given to new categories in the saved search pattern tree in search view.",
				App.kLocalizationGroupInfoMsg);

			AddCategory(msg, slidingPanel, beginEditAfterAdding);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new category to the database and the tree. If the sliding panel is not
		/// null then it must be frozen until editing the category name is finished.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddCategory(string newCategoryName, SlidingPanel slidingPanel,
			bool beginEditAfterAdding)
		{
			// Make sure the category name is unique.
			int i = 0;
			string newName = newCategoryName;
			while (CategoryExists(newName))
				newName = string.Format("{0} ({1})", newCategoryName, ++i);

			newCategoryName = newName;
			
			SearchQueryGroup group = new SearchQueryGroup();
			group.Name = newCategoryName;
			App.Project.SearchQueryGroups.Add(group);
			App.Project.SearchQueryGroups.Save();

			// Hide the label that tells the user there are no saved patterns
			// because we know there is at least one category now.
			m_lblNoPatternsMsg.Visible = false;

			// Now add the category to the tree, select it and put user in edit mode.
			TreeNode node = Nodes.Add(newCategoryName);
			node.Tag = group;
			SelectedNode = node;

			if (!beginEditAfterAdding)
				return;

			node.BeginEdit();
			if (slidingPanel != null)
			{
				m_slidingPanel = slidingPanel;
				slidingPanel.Freeze = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a query in the specified pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AddPattern(SearchQuery query, string category)
		{
			bool categoryExists = false;

			foreach (TreeNode node in Nodes)
			{
				if (node.Text == category)
				{
					categoryExists = true;
					SelectedNode = node;
					break;
				}
			}

			if (!categoryExists)
				AddCategory(category, null, false);
			else if (PatternExists(category, query.ToString()))
			{
				// Pattern exisits so ask user if he wants to overwrite.
				var msg = App.LocalizeString("SearchVw.SavedSearchPatternsList.DuplicateSearchQueryQuestion",
					"There is already a saved search pattern named '{0}' in the same category. Would you like it overwritten?",
					App.kLocalizationGroupInfoMsg);

				if (Utils.MsgBox(string.Format(msg, query), MessageBoxButtons.YesNo) == DialogResult.No)
					return false;

				// User wants to overwrite so delete existing one first.
				DeletePattern(GetPatternsNode(category, query.ToString()), false);
			}

			AddPattern(query, false);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public void AddPattern(SearchQuery query, bool forceUniqueName)
		{
			if (query == null)
				return;

			if (Nodes == null || Nodes.Count == 0)
			{
				var msg = App.LocalizeString("SearchVw.SavedSearchPatternsList.AddSearchCategoryBeforeSaveMsg",
					"Before saving a search pattern, you must first add a category to the saved pattern list.",
					App.kLocalizationGroupInfoMsg);
				
				Utils.MsgBox(msg, MessageBoxButtons.OK);
				return;
			}

			// Create a new query object.
			SearchQuery newquery = query.Clone();

			// Make sure we're adding the new node to the correct parent.
			TreeNode categoryNode = (SelectedNode.Level == 0 ? SelectedNode : SelectedNode.Parent);
			newquery.Category = categoryNode.Text;

			if (forceUniqueName)
			{
				// Make sure the name of the query being added is unique within its group.
				int i = 0;
				string newName = newquery.ToString();

				while (PatternExists(newquery.Category, newName))
					newName = string.Format("{0} ({1})", newquery, ++i);

				if (newquery.ToString() != newName)
					newquery.Name = newName;
			}

			SearchQueryGroup group = App.Project.SearchQueryGroups[categoryNode.Index];

			// Make sure we have a list to add to.
			if (group.Queries == null)
				group.Queries = new List<SearchQuery>();

			newquery.Id = App.Project.SearchQueryGroups.NextAvailableId;
			group.Queries.Add(newquery);
			App.Project.SearchQueryGroups.Save();

			// Now create a new tree node for it.
			TreeNode node = new TreeNode();
			node.Text = newquery.ToString();
			node.Tag = newquery;
			node.Name = newquery.Id.ToString();
			categoryNode.Nodes.Add(node);
			node.ImageIndex = node.SelectedImageIndex = 2;

			// Make sure the category the pattern was added to is expanded.
			if (!categoryNode.IsExpanded)
				categoryNode.Expand();

			// Make the current node the one just added.
			SelectedNode = node;
		}

		#endregion

		#region Message handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the font(s) get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPaFontsChanged(object args)
		{
			Font = App.PhoneticFont;

			// Return false to allow other windows to update their fonts.
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the expanded states when the control popup closes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnDropDownClosedViewFindPhones(object args)
		{
			if (m_isForToolbarPopup)
			{
				SaveSettings();
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnCutSavedPattern(object args)
		{
			if (!App.IsFormActive(FindForm()))
				return false;

			m_cutCommand = true;
			OnKeyPress(new KeyPressEventArgs('X'));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnCopySavedPattern(object args)
		{
			if (!App.IsFormActive(FindForm()))
				return false;

			m_copyCommand = true;
			OnKeyPress(new KeyPressEventArgs('C'));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnPasteSavedPattern(object args)
		{
			if (!App.IsFormActive(FindForm()))
				return false;

			m_pasteCommand = true;
			OnKeyPress(new KeyPressEventArgs('V'));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnSearchUsingPattern(object args)
		{
			if (!App.IsFormActive(FindForm()))
				return false;

			App.MsgMediator.SendMessage("ViewSearch", CurrentQuery);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnRemovePattern(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !itemProps.Name.EndsWith("-FromSavedList") ||
				!App.IsFormActive(FindForm()))
			{
				return false;
			}

			if (CurrentQuery == null)
				DeleteCategory(SelectedNode);
			else
				DeletePattern(SelectedNode, false);

			UpdateButtons();
			return true;
		}

		#endregion

		#region Update handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnUpdateCutSavedPattern(object args)
		{
			if (!App.IsFormActive(FindForm()) || m_btnCut == null)
				return false;

			bool enable = (CurrentQuery != null);
			if (m_btnCut.Enabled != enable)
				m_btnCut.Enabled = enable;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps != null && itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnUpdateCopySavedPattern(object args)
		{
			if (!App.IsFormActive(FindForm()) || m_btnCopy == null)
				return false;

			bool enable = (CurrentQuery != null);
			if (m_btnCopy.Enabled != enable)
				m_btnCopy.Enabled = enable;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps != null && itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnUpdatePasteSavedPattern(object args)
		{
			if (!App.IsFormActive(FindForm()) || m_btnPaste == null)
				return false;

			bool enable = (SelectedNode != null && m_patternClipboard != null);
			if (m_btnPaste.Enabled != enable)
				m_btnPaste.Enabled = enable;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps != null && itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
			}

			return true;
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] { this });
		}

		#endregion
	}
}
