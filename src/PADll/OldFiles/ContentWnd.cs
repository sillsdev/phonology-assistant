using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms.VisualStyles;
using SIL.Pa.Resources;
using SIL.Pa.Dialogs;
using SIL.SpeechTools.Database;
using SIL.Pa.Controls;
using SIL.SpeechTools.Utils;
using SIL.FieldWorks.Common.UIAdapters;
using XCore;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for ContentWnd.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ContentWnd : System.Windows.Forms.Form, IxCoreColleague
	{
		private float m_trans1WndPortion;
		private float m_trans2WndPortion;
		private float m_trans3WndPortion;
		private PaDBGrid m_grid;
		private PaTreeNode m_prevTreeNode = null;
		private BoundSpeakerComboBoxHandler m_bchSpeaker;
		private BoundComboBoxHandler m_bchDialect;
		private BoundTextBoxHandler m_bthTranscriber;
		private BoundTextBoxHandler m_bthNoteBookRef;
		private Dictionary<int, int> m_documentOccurrences;
		private TreeNode m_currDragOverFolder = null;

		#region Construction, setup and loading of query.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ContentWnd()
		{
			InitializeComponent();

			//Text = string.Format(ResourceHelper.GetString("kstidContentWindowCaption"),
			//    DBUtils.LanguageName, DBUtils.DocumentCount);

			//SetControlFont();

			//lblLastUpdateVal.ToolTip = m_toolTip;
			//lblOrigDateVal.ToolTip = m_toolTip;
			//lblRecDateVal.ToolTip = m_toolTip;
			//lblFileNameVal.ToolTip = m_toolTip;
			//ltbFreeform.PaField = DBFields.Freeform;
			//ltbComments.PaField = DBFields.Comments;

			//cboSpeaker.DataSource = DBUtils.CachedSpeakerList;
			//cboSpeaker.DisplayMember = "Name";
			//m_bchSpeaker = new BoundSpeakerComboBoxHandler(cboSpeaker, rbMale, rbFemale);
			
			//cboDialect.DataSource = DBUtils.CachedDialectList;
			//m_bchDialect = new BoundComboBoxHandler(DBFields.Dialect, cboDialect);

			//m_bthTranscriber = new BoundTextBoxHandler(DBFields.Transcriber, txtTranscriber);
			//m_bthNoteBookRef = new BoundTextBoxHandler(DBFields.NoteBookRef, txtNBRef);

			//BuildGrid();
			//tvContents.ImageList = ResourceHelper.TreeImages;
			//LoadContentTree();
			//LoadSettings();

			PaApp.SettingsHandler.LoadFormProperties(this);
			
			// Create a new cache for this grid.
			WordListCache cache = new WordListCache();
			foreach (WordCacheEntry entry in DBUtils.WordCache)
				cache.Add(entry);

			m_grid2 = new PaWordListGrid(cache);			
			m_grid2.Name = Name + "Grid";
			m_grid2.LoadSettings();
			Controls.Add(m_grid2);

			Text = string.Format(ResourceHelper.GetString("kstidContentWindowCaption"),
				DBUtils.LanguageName, m_grid2.Rows.Count);
		}

		PaWordListGrid m_grid2;

		protected override void OnShown(EventArgs e)
		{
			m_grid2.AutoResizeRows();
			base.OnShown(e);

			//DBUtils.AFeatureCache.Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildGrid()
		{
			string[] fieldsInGrid = new string[] {DBFields.Phonetic, DBFields.Phonemic,
				DBFields.Tone, DBFields.Ortho, DBFields.Gloss, DBFields.POS,
				DBFields.Reference, DBFields.CVPattern, DBFields.AllWordIndexId};

			m_grid = PaDBGrid.Create(fieldsInGrid);
			m_grid.Dock = DockStyle.Fill;
			m_grid.Name = Name + "Grid";
			//PaApp.SettingsHandler.LoadGridProperties(m_grid);
			tpgColView.Controls.Add(m_grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Restores query from the query file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadSettings()
		{
			PaApp.SettingsHandler.LoadFormProperties(this);
			tabDocViews.SelectedIndex = PaApp.SettingsHandler.GetIntWindowValue(Name, "tab", 0);

			int splitterVal = PaApp.SettingsHandler.GetIntWindowValue(Name, "splitter0", -1);
			if (splitterVal > -1)
				scTrans.SplitterDistance = splitterVal;

			splitterVal = PaApp.SettingsHandler.GetIntWindowValue(Name, "splitter1", -1);
			if (splitterVal > -1)
				splitter1.SplitPosition = splitterVal;

			splitterVal = PaApp.SettingsHandler.GetIntWindowValue(Name, "splitter2", -1);
			if (splitterVal > -1)
				splitter2.SplitPosition = splitterVal;

			ltbTransWnd1.DBField =
				PaApp.SettingsHandler.GetStringWindowValue(Name, "transtype1", DBFields.Phonetic);

			ltbTransWnd2.DBField =
				PaApp.SettingsHandler.GetStringWindowValue(Name, "transtype2", DBFields.Phonemic);

			ltbTransWnd3.DBField =
				PaApp.SettingsHandler.GetStringWindowValue(Name, "transtype3", DBFields.Ortho);

			string nodeKey = PaApp.SettingsHandler.GetStringWindowValue(Name, "selectednode", null);
			PaTreeNode node = GetTreeNode(tvContents.Nodes, nodeKey);
			if (node != null)
				tvContents.SelectedNode = node;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the UI font for all the controls that aren't transcription controls.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetControlFont()
		{
			tabDocViews.Font = FontHelper.UIFont;
			tvContents.Font = FontHelper.UIFont;
			tpgTrans.Font = FontHelper.UIFont;
			tpgColView.Font = FontHelper.UIFont;
			tpgDocInfo.Font = FontHelper.UIFont;

			grpAudioFile.Font = FontHelper.UIFont;
			grpDates.Font = FontHelper.UIFont;
			grpSpeaker.Font = FontHelper.UIFont;
			cboDialect.Font = FontHelper.UIFont;
			cboSpeaker.Font = FontHelper.UIFont;

			txtNBRef.Font = FontHelper.UIFont;
			txtTranscriber.Font = FontHelper.UIFont;

			rbFemale.Font = FontHelper.UIFont;
			rbMale.Font = FontHelper.UIFont;

			Font smallUIFont = new Font(FontHelper.UIFont.Name, 8);
			lblOrigDateVal.Font = smallUIFont;
			lblLastUpdateVal.Font = smallUIFont;
			lblBitsVal.Font = smallUIFont;
			lblFileNameVal.Font = smallUIFont;
			lblLengthVal.Font = smallUIFont;
			lblOrigFmtVal.Font = smallUIFont;
			lblRateVal.Font = smallUIFont;
			lblRecDateVal.Font = smallUIFont;
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the type of the selected node in the tree view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaTreeNodeType CurrentTreeNodeType
		{
			get
			{
				PaTreeNode currNode = CurrentTreeNode;
				return (currNode == null ? PaTreeNodeType.Undefined : ((PaTreeNode)currNode).NodeType);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the selected node in the tree view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaTreeNode CurrentTreeNode
		{
			get
			{
				if (tvContents.Nodes == null || tvContents.Nodes.Count == 0)
					return null;

				if (tvContents.SelectedNode == null)
					tvContents.SelectedNode = tvContents.Nodes[0];

				return (tvContents.SelectedNode as PaTreeNode);
			}
		}
		
		public TextBox EticTextBox
		{
			get { return ltbTransWnd1.TextBox; }
		}

		public TextBox EmicTextBox
		{
			get { return ltbTransWnd2.TextBox; }
		}

		public TextBox OrthoTextBox
		{
			get { return ltbTransWnd3.TextBox; }
		}

		public TextBox FreeTextBox
		{
			get { return ltbFreeform.TextBox; }
		}

		public TextBox CommentTextBox
		{
			get { return ltbComments.TextBox; }
		}

		public TextBox NBRefTextBox
		{
			get { return txtNBRef; }
		}

		public TextBox TranscriberTextBox
		{
			get { return txtTranscriber; }
		}

		public ComboBox SpeakerComboBox
		{
			get { return cboSpeaker; }
		}

		#endregion

		#region Closing and saving query
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When this window goes away, the DB connection should be closed.
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosed(EventArgs e)
		{
			//DBUtils.CloseDatabase();
			//base.OnClosed(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Prompt user if they're only closing this window and not the application.
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			//// If we're not shutting down the application, then prompt the user.
			//if (MdiParent != null && !((PaMainWnd)MdiParent).IsShuttingDown)
			//{
			//    string msg = string.Format(ResourceHelper.GetString("kstidCloseDBPrompt"),
			//        DBUtils.LanguageName);

			//    if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo,
			//        MessageBoxIcon.Question) == DialogResult.No)
			//    {
			//        e.Cancel = true;
			//    }
			//}

			//// Make sure any changes to the current document are saved.
			//tvContents_BeforeSelect(null, null);

			//PaApp.SettingsHandler.SaveWindowValue(Name, "splitter0", scTrans.SplitterDistance);
			//PaApp.SettingsHandler.SaveWindowValue(Name, "splitter1", splitter1.SplitPosition);
			//PaApp.SettingsHandler.SaveWindowValue(Name, "splitter2", splitter2.SplitPosition);
			//PaApp.SettingsHandler.SaveWindowValue(Name, "transtype1", ltbTransWnd1.DBField);
			//PaApp.SettingsHandler.SaveWindowValue(Name, "transtype2", ltbTransWnd2.DBField);
			//PaApp.SettingsHandler.SaveWindowValue(Name, "transtype3", ltbTransWnd3.DBField);
			//PaApp.SettingsHandler.SaveWindowValue(Name, "tab", tabDocViews.SelectedIndex);
			//PaApp.SettingsHandler.LastDatabase = DBUtils.DatabaseFile;
			//PaApp.SettingsHandler.SaveFormProperties(this);
			////m_grid2.SaveSettings();
			////SaveTreeExpansionStates();

			//if (tvContents.SelectedNode != null)
			//    PaApp.SettingsHandler.SaveWindowValue(Name, "selectednode", tvContents.SelectedNode.Name);

			base.OnClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds lists of categories and folder ids and their expanded states and passes them
		/// to the db utilities to save to the DB.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveTreeExpansionStates()
		{
			Dictionary<int, bool> categoryStates = new Dictionary<int, bool>();
			Dictionary<int, bool> folderStates = new Dictionary<int, bool>();

			foreach (PaTreeNode catNode in tvContents.Nodes)
			{
				categoryStates[catNode.Id] = catNode.IsExpanded;

				foreach (PaTreeNode folderNode in catNode.Nodes)
					folderStates[folderNode.Id] = folderNode.IsExpanded;
			}

			DBUtils.SaveCategoryFolderExpansionStates(categoryStates, folderStates);
		}

		#endregion

		#region Resize handling
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the proportions of each transcription window.
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnResizeBegin(EventArgs e)
		{
			m_trans1WndPortion = (float)ltbTransWnd1.Height / tabDocViews.Height;
			m_trans2WndPortion = (float)ltbTransWnd2.Height / tabDocViews.Height;
			m_trans3WndPortion = (float)ltbTransWnd3.Height / tabDocViews.Height;

			base.OnResizeBegin(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Keep the same proportions for the transcription windows.
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if (!IsHandleCreated || m_trans1WndPortion == 0)
				return;

			try
			{
				ltbTransWnd1.Height = (int)((float)tabDocViews.Height * m_trans1WndPortion);
				ltbTransWnd2.Height = (int)((float)tabDocViews.Height * m_trans2WndPortion);
				ltbTransWnd3.Height = (int)((float)tabDocViews.Height * m_trans3WndPortion);
			}
			catch { }
		}

		#endregion

		#region Control Events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When visual styles are enabled, then paint the splitters on the tab pages to
		/// match the visual style for tab page backgrounds.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void splitter_Paint(object sender, PaintEventArgs e)
		{
			if (!Application.RenderWithVisualStyles)
				return;

			VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.Tab.Body.Normal);
			renderer.DrawBackground(e.Graphics, ClientRectangle);
		}

		#endregion

		#region Content Tree Setup, Events and methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the RefreshTree message sent by the message mediator.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnRefreshTree(object obj)
		{
			RefreshTreeInfo refreshInfo = obj as RefreshTreeInfo;

			string topNodeKey = (tvContents.TopNode == null ? null : tvContents.TopNode.Name);
			SaveTreeExpansionStates();
			LoadContentTree();

			// Make visible the node that was the top node before the refresh.
			// This will likely return it to the top (that's the hope, anyway).
			// Unfortunately the TopNode property cannot be set.
			PaTreeNode node = GetTreeNode(tvContents.Nodes, topNodeKey);
			if (node != null)
			    node.EnsureVisible();

			if (refreshInfo != null && refreshInfo.NodeKey != null)
			{
				node = GetTreeNode(tvContents.Nodes, refreshInfo.NodeKey);

				if (node != null)
				{
					tvContents.SelectedNode = node;
					if (refreshInfo.EditAfterRefresh)
						node.BeginEdit();

					return false;
				}
			}
			
			//tvContents.EndUpdate();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the node from the content window tree.
		/// </summary>
		/// <param name="xPath">path to the node</param>
		/// <returns>the node</returns>
		/// ------------------------------------------------------------------------------------
		private PaTreeNode GetTreeNode(TreeNodeCollection nodeCollection, string key)
		{
			if (key == null || key == string.Empty)
				return null;

			if (nodeCollection[key] != null)
				return nodeCollection[key] as PaTreeNode;

			PaTreeNode nodeSought;

			foreach (PaTreeNode node in nodeCollection)
			{
				nodeSought = node.Nodes[key] as PaTreeNode;

				if (nodeSought != null)
					return nodeSought;

				nodeSought = GetTreeNode(node.Nodes, key);
			
				if (nodeSought != null)
					return nodeSought;
			}

			return null;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Get the node from the content window tree.
		///// </summary>
		///// <param name="xPath">path to the node</param>
		///// <returns>the node</returns>
		///// ------------------------------------------------------------------------------------
		//private PaTreeNode GetTreeNode(string xPath)
		//{
		//    if (xPath == null || xPath == string.Empty)
		//        return null;

		//    string[] nodeNames = xPath.Split(tvContents.PathSeparator.ToCharArray(), 3);
		//    PaTreeNode returnNode = null;
			
		//    //Get the Category node
		//    if (nodeNames[0] != string.Empty)
		//    {
		//        foreach (PaTreeNode node in tvContents.Nodes)
		//        {
		//            if (node.Text == nodeNames[0])
		//                returnNode = node;
		//        }
		//    }
			
		//    bool existence = true;
		//    int i = 1;
		//    while ((i < nodeNames.Length) && (nodeNames[i] != string.Empty) && (returnNode != null) 
		//        && (existence != false))
		//    {
		//        existence = false;
		//        foreach (PaTreeNode node in returnNode.Nodes)
		//        {
		//            if (node.Text == nodeNames[i])
		//            {
		//                returnNode = node;
		//                existence = true;
		//            }
		//        }
		//        i++;
		//    }

		//    //Existence will be true even if the category node is not found, but returnNode will be Null.
		//    return (existence ? returnNode : null);
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the content tree from the DB
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadContentTree()
		{
			tvContents.BeginUpdate();
			tvContents.Nodes.Clear();

			m_documentOccurrences = DBUtils.GetDocumentLinkCounts();

			string sql = DBUtils.SelectSQL("Category") + " ORDER BY CatTitle";
			using (OleDbDataReader reader = DBUtils.GetSQLResultsFromDB(sql))
			{
				while (reader.Read())
				{
					PaTreeNode node = PaTreeNode.CreateCategoryNode(
						(string)reader[DBFields.CatTitle], (int)reader[DBFields.CatId]);

					tvContents.Nodes.Add(node);
					AddFolderNode(node);

					if ((bool)reader["CExp"])
						node.Expand();
					else
						node.Collapse();
				}

				reader.Close();
			}

			m_documentOccurrences = null;
			tvContents.EndUpdate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a Folder node subordinate to the specified category node.
		/// </summary>
		/// <param name="catNode"></param>
		/// ------------------------------------------------------------------------------------
		private void AddFolderNode(PaTreeNode catNode)
		{
			using (OleDbDataReader reader = DBUtils.GetQueryResultsFromDB("qryFoldersForCategory", catNode.Id))
			{
				while (reader.Read())
				{
					PaTreeNode node = PaTreeNode.CreateFolderNode(
						(string)reader[DBFields.FolderTitle], (int)reader[DBFields.FolderId]);

					catNode.Nodes.Add(node);
					AddDocumentNode(node);

					if ((bool)reader["FExp"])
						node.Expand();
					else
						node.Collapse();
				}

				reader.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create a document tree node subordinate to the specified folder node.
		/// </summary>
		/// <param name="fldrNode"></param>
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
						node.NodeFont = new Font(tvContents.Font, FontStyle.Bold);
					}
					fldrNode.Nodes.Add(node);
				}

				reader.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Commit document changes to the database before moving to the next document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tvContents_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (m_prevTreeNode != null && m_prevTreeNode.NodeType == PaTreeNodeType.Document)
				DBUtils.DocCache[m_prevTreeNode.Id].Commit();

			if (e != null)
				m_prevTreeNode = e.Node as PaTreeNode;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tvContents_AfterSelect(object sender, TreeViewEventArgs e)
		{
			PaTreeNode node = e.Node as PaTreeNode;
			Debug.Assert(node != null);

			// Set the id's in the app. class so other assemblies have access to the current ids.
			switch (node.NodeType)
			{
				case PaTreeNodeType.Category:
					PaApp.CurrentCategoryId = node.Id;
					PaApp.CurrentFolderId = 0;
					PaApp.CurrentDocumentId = 0;
					break;

				case PaTreeNodeType.Folder:
					PaApp.CurrentCategoryId = node.NodesParentCategoryId;
					PaApp.CurrentFolderId = node.Id;
					PaApp.CurrentDocumentId = 0;
					break;

				case PaTreeNodeType.Document:
					PaApp.CurrentCategoryId = node.NodesParentCategoryId;
					PaApp.CurrentFolderId = node.NodesParentFolderId;
					PaApp.CurrentDocumentId = node.Id;
					break;
			}

			// Update the various controls on this form.
			UpdateTranscriptionTab(PaApp.CurrentDocumentId);
			UpdateColumnViewTab(PaApp.CurrentDocumentId);
			UpdateDocumentInfoTab(PaApp.CurrentDocumentId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void tvContents_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			PaTreeNode node = e.Node as PaTreeNode;

			// When e.Label is null it probably means the user pressed ESC when editing.
			if (node == null || e.Label == null || e.Label == node.Text)
				return;

			if (!node.RenameTitle(e.Label))
			{
				string msgFmt = ResourceHelper.GetString("kstidTitleAlreadyExists");
				MessageBox.Show(this, string.Format(msgFmt, node.NodeType, e.Label));
				e.CancelEdit = true;
			}
			else if (node.NodeType == PaTreeNodeType.Document)
				RenameLinkedDocuments(tvContents.Nodes, node.Id, e.Label);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// After a document is renamed, this method will go through the tree and make sure
		/// all copies of the document have the same title.
		/// </summary>
		/// <param name="nodeCollection">Node collection to scan.</param>
		/// <param name="docId">Id of documents whose title changed.</param>
		/// <param name="newTitle">New document title (or name).</param>
		/// ------------------------------------------------------------------------------------
		private void RenameLinkedDocuments(TreeNodeCollection nodeCollection, int docId, string newTitle)
		{
			foreach (PaTreeNode node in nodeCollection)
			{
				if (node.NodeType != PaTreeNodeType.Document)
					RenameLinkedDocuments(node.Nodes, docId, newTitle);
				else
				{
					if (node.Id == docId)
						node.Text = newTitle;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Put a node in the edit mode when the user presses F2.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void tvContents_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (tvContents.SelectedNode == null)
				return;
			
			if (e.KeyCode == Keys.F2)
				tvContents.SelectedNode.BeginEdit();
			else if (e.KeyCode == Keys.Delete)
				OnEditDelete(null);
			else if (e.KeyCode == Keys.F5)
			{
				string nodeKey = (CurrentTreeNode != null ? CurrentTreeNode.Name : null);
				RefreshTreeInfo refreshInfo = new RefreshTreeInfo(nodeKey, false);
				OnRefreshTree(refreshInfo);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Begin the drag and drop process when the mouse is over a document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tvContents_ItemDrag(object sender, ItemDragEventArgs e)
		{
			PaTreeNode node = (PaTreeNode)e.Item;

			// For now, only support moving or copying documents.
			if (node.NodeType == PaTreeNodeType.Document)
			{
				// Right button will copy and the left will move.
				if (e.Button == MouseButtons.Right)
					DoDragDrop(e.Item, DragDropEffects.Copy);
				else if (e.Button == MouseButtons.Left)
					DoDragDrop(e.Item, DragDropEffects.Move | DragDropEffects.Copy);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Select the folder that will be the drop target if the drop happened at the
		/// current mouse location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tvContents_DragOver(object sender, DragEventArgs e)
		{
			// Retrieve the client coordinates of the mouse position and use
			// that to get the node the mouse is over.
			Point pt = tvContents.PointToClient(new Point(e.X, e.Y));
			PaTreeNode node = tvContents.GetNodeAt(pt) as PaTreeNode;

			// If the mouse is over a category, that's not an acceptable target.
			if (node == null || node.NodeType == PaTreeNodeType.Category)
			{
				InvalidateFolderAndChildren(m_currDragOverFolder);
				m_currDragOverFolder = null;
				e.Effect = DragDropEffects.None;
				return;
			}

			// If the mouse is over a document then set the node to the
			// document's owning folder.
			if (node.NodeType == PaTreeNodeType.Document)
				node = (PaTreeNode)node.Parent;

			// Select the folder that will be the target folder if drop
			// happens at the current point.
			if (m_currDragOverFolder != node)
			{
				InvalidateFolderAndChildren(m_currDragOverFolder);
				m_currDragOverFolder = node;
				HighlightFolderAndChildren(m_currDragOverFolder);
			}

			// First check if the right mouse button is down. When it is, that trumps any
			// change to ctrl or shift states. It may seem strange, but that's the way
			// Windows Explorer works. I'm not sure why they do it that way.
			if ((e.KeyState & 2) > 0)
				e.Effect = e.AllowedEffect;
			else
			{
				// Strip off the bit indicating the left mouse button is down.
				int ks = (e.KeyState ^ 1);
				e.Effect = ((ks & 8) == 8 ? DragDropEffects.Copy : DragDropEffects.Move);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void tvContents_DragLeave(object sender, EventArgs e)
		{
			InvalidateFolderAndChildren(m_currDragOverFolder);
			m_currDragOverFolder = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tvContents_DragDrop(object sender, DragEventArgs e)
		{
			InvalidateFolderAndChildren(m_currDragOverFolder);
			m_currDragOverFolder = null;

			// Retrieve the client coordinates of the drop location.
			// Then retrieve the drop location's folder node .
			Point pt = tvContents.PointToClient(new Point(e.X, e.Y));
			PaTreeNode targetNode = (PaTreeNode)tvContents.GetNodeAt(pt);
			if (targetNode.NodeType == PaTreeNodeType.Document)
				targetNode = (PaTreeNode)targetNode.Parent;

			PaTreeNode draggedNode = (PaTreeNode)e.Data.GetData(typeof(PaTreeNode));
			if (draggedNode != null)
			{
				ProcessTreeNodeDropped(e, targetNode, draggedNode);
				return;
			}

			// Check if the the user dropped a list of files on the tree. If so, assume
			// they're files to import and treat as such.
			string[] importFiles = e.Data.GetData("FileDrop") as string[];
			if (importFiles != null)
			{
				// Make the target node the current node so importing will put the
				// new documents there.
				if (targetNode != null)
					tvContents.SelectedNode = targetNode;

				// Make sure we're activated.
				MdiParent.Activate();
				OnFileImport(importFiles);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the processing after the user drops a tree node onto another tree node
		/// in order to copy or move the dragged node.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ProcessTreeNodeDropped(DragEventArgs e, PaTreeNode targetNode,
			PaTreeNode draggedNode)
		{
			// If the document is being dragged over it's owning folder,
			// there's no need to do anything.
			if (targetNode == draggedNode.Parent)
				return;

			RefreshTreeInfo refreshInfo = null;

			if (e.Effect == DragDropEffects.Copy)
			{
				int docLinkId = DBUtils.CopyDocument(targetNode.Id, draggedNode.Id);
				if (docLinkId > 0)
					refreshInfo = new RefreshTreeInfo(DBFields.DocLinkId + docLinkId, false);
			}
			else if (e.Effect == DragDropEffects.Move)
			{
				if (DBUtils.MoveDocument(targetNode.Id, draggedNode.DocLinkId))
					refreshInfo = new RefreshTreeInfo(draggedNode.Name, false);
			}

			if (refreshInfo != null)
				OnRefreshTree(refreshInfo);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Forces a folder and it's children to be repainted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InvalidateFolderAndChildren(TreeNode folderNode)
		{
			if (folderNode != null && folderNode.Bounds != Rectangle.Empty)
			{
				Rectangle rc = folderNode.Bounds;
				rc.Width = tvContents.Width - rc.Left;
				rc.Height += 2;

				// Expand the rectangle to include the children if there
				// are any and they're visible.
				if (folderNode.IsExpanded && folderNode.FirstNode != null)
					rc.Height = folderNode.LastNode.Bounds.Bottom - rc.Top + 2;

				tvContents.Invalidate(rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Highlights a folder and its children to indicate in what folder a dragged and
		/// dropped items will be copied/moved.
		/// </summary>
		/// <remarks>
		/// I chose to paint the highlight this way instead of owner-draw the nodes.
		/// .Net has a nice feature in that when you owner draw the nodes, in the draw event
		/// for the node you may return a value indicating that you want the system to draw
		/// the default node. This would have been really nice since most of the time I want
		/// default drawing of nodes. However, in a few situations (e.g. when the tree loses
		/// focus), the default drawing in owner-draw mode is not the same as the drawing
		/// when in normal drawing mode. Sigh! What I have done below actually works pretty
		/// well, though.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		private void HighlightFolderAndChildren(TreeNode folderNode)
		{
			if (folderNode == null)
				return;

			// Forces any pending paint events to finish before we go mucking with things.
			Application.DoEvents();

			Color clr = ColorHelper.CalculateColor(SystemColors.MenuHighlight, tvContents.BackColor, 70);

			// Prepare for drawing labels
			using (SolidBrush brBack = new SolidBrush(clr))
			using (Pen penBrdr = new Pen(SystemColors.MenuHighlight, 1))
			using (SolidBrush brFore = new SolidBrush(tvContents.ForeColor))
			using (StringFormat sf = STUtils.GetStringFormat(false))
			using (Graphics g = tvContents.CreateGraphics())
			{
				sf.Trimming = StringTrimming.None;

				// First highlight the folder node.
				Rectangle rc = folderNode.Bounds;
				rc.Width = tvContents.ClientRectangle.Width - rc.Left;
				g.FillRectangle(brBack, rc);
				rc.Y++;
				g.DrawString(folderNode.Text, tvContents.Font, brFore, rc, sf);

				// Draw border around folder node.
				rc.Y--;
				rc.Width--;
				g.DrawRectangle(penBrdr, rc);

				if (!folderNode.IsExpanded || folderNode.Nodes.Count == 0)
					return;

				// Now hightlight each child node.
				foreach (TreeNode node in folderNode.Nodes)
				{
					rc = node.Bounds;
					rc.Width = tvContents.ClientRectangle.Width - rc.Left;
					g.FillRectangle(brBack, rc);
					rc.Y++;
					g.DrawString(node.Text, tvContents.Font, brFore, rc, sf);
				}

				// Draw border around folder's child nodes.
				g.DrawLines(penBrdr, new Point[] {
					new Point(folderNode.FirstNode.Bounds.X, folderNode.FirstNode.Bounds.Y),
					new Point(folderNode.FirstNode.Bounds.X, folderNode.LastNode.Bounds.Bottom),
					new Point(tvContents.ClientRectangle.Right - 1, folderNode.LastNode.Bounds.Bottom),
					new Point(tvContents.ClientRectangle.Right - 1, folderNode.FirstNode.Bounds.Y)});
			}
		}

		#endregion

		#region Methods for updating tab pages when tree node changes
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the data displayed in the Transcription tab.
		/// </summary>
		/// <param name="docId">Id of the currently selected document, or 0 if a non-document
		/// node is selected.</param>
		/// ------------------------------------------------------------------------------------
		private void UpdateTranscriptionTab(int docId)
		{
			ltbTransWnd1.RefreshContent(docId);
			ltbTransWnd2.RefreshContent(docId);
			ltbTransWnd3.RefreshContent(docId);
			ltbTransWnd1.Enabled = (docId > 0);
			ltbTransWnd2.Enabled = (docId > 0);
			ltbTransWnd3.Enabled = (docId > 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the column view grid with data for the specified document id.
		/// </summary>
		/// <param name="docId"></param>
		/// ------------------------------------------------------------------------------------
		private void UpdateColumnViewTab(int docId)
		{
			m_grid.Enabled = (docId > 0);

			if (docId <= 0)
			{
				m_grid.DataSource = null;
				return;
			}

			m_grid.DataSource = DBUtils.DocCache[docId].Words;
			m_grid.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
			m_grid.IsDirty = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the Document Info tab if a document is selected.
		/// </summary>
		/// <param name="docID">ID of the document whose info will be displayed</param>
		/// ------------------------------------------------------------------------------------
		private void UpdateDocumentInfoTab(int docId)
		{
			bool isAudioDoc = (docId > 0 ? (bool)DBUtils.DocCache[docId][DBFields.Wave] : false);
			pnlDocInfo.Enabled = (docId > 0);

			UpdateDocumentDateTimeInfo(docId);

			ltbComments.RefreshContent(docId);
			ltbFreeform.RefreshContent(docId);
			m_bchSpeaker.RefreshContent(docId);
			m_bchDialect.RefreshContent(docId);
			m_bthTranscriber.RefreshContent(docId);
			m_bthNoteBookRef.RefreshContent(docId);
			
			if (isAudioDoc && docId > 0)
				UpdateAudioDocInfo(docId);
			else
			{
				lblBitsVal.Text = string.Empty;
				lblFileNameVal.Text = string.Empty;
				lblLengthVal.Text = string.Empty;
				lblOrigFmtVal.Text = string.Empty;
				lblRecDateVal.Text = string.Empty;
				lblRateVal.Text = string.Empty;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the original date and last update dates on the Document Info. tab.
		/// </summary>
		/// <param name="docId"></param>
		/// ------------------------------------------------------------------------------------
		private void UpdateDocumentDateTimeInfo(int docId)
		{
			if (docId <= 0)
			{
				lblOrigDateVal.Text = string.Empty;
				lblOrigDateVal.ToolTipText = null;
				lblLastUpdateVal.Text = string.Empty;
				lblLastUpdateVal.ToolTipText = null;
			}
			else
			{
				DateTime date = (DateTime)DBUtils.DocCache[docId][DBFields.OriginalDate];
				if (date == DateTime.MinValue)
				{
					// When the date is the minimum value, it means the value in the DB
					// is null. That should never happen, but just in case...
					lblOrigDateVal.Text = "Unknown";
					lblOrigDateVal.ToolTipText = null;
				}
				else
				{
					lblOrigDateVal.Text = date.ToString();
					lblOrigDateVal.ToolTipText = date.ToLongDateString() + "  " + date.ToLongTimeString();
				}

				date = (DateTime)DBUtils.DocCache[docId][DBFields.LastUpdate];
				if (date == DateTime.MinValue)
				{
					// When the date is the minimum value, it means the value in the DB
					// is null. That should never happen, but just in case...
					lblLastUpdateVal.Text = "Unknown";
					lblLastUpdateVal.ToolTipText = null;
				}
				else
				{
					lblLastUpdateVal.Text = date.ToString();
					lblLastUpdateVal.ToolTipText = date.ToLongDateString() + "  " + date.ToLongTimeString();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the audio information about the document.
		/// </summary>
		/// <param name="docId"></param>
		/// ------------------------------------------------------------------------------------
		private void UpdateAudioDocInfo(int docId)
		{
			string fullPath = DBUtils.DocCache[docId][DBFields.SAFileName] as string;
			lblFileNameVal.Text = System.IO.Path.GetFileName(fullPath);
			lblFileNameVal.ToolTipText = fullPath;

			lblBitsVal.Text = DBUtils.DocCache[docId][DBFields.BitsPerSample] + " " +
				((int)DBUtils.DocCache[docId][DBFields.Channels] == 1 ?
				ResourceHelper.GetString("kstidMono") :
				ResourceHelper.GetString("kstidStereo"));

			lblRateVal.Text = DBUtils.DocCache[docId][DBFields.SamplesPerSecond].ToString() +
				ResourceHelper.GetString("kstidHertzAbbrev");

			lblOrigFmtVal.Text = DBUtils.DocCache[docId].RecordingFileFormat;
			lblRecDateVal.Text = DBUtils.DocCache[docId].RecordingTimeStamp.ToString();
			lblRecDateVal.ToolTipText =
				DBUtils.DocCache[docId].RecordingTimeStamp.ToLongDateString() + "  " +
				DBUtils.DocCache[docId].RecordingTimeStamp.ToLongTimeString();

			// Build the recording length string.
			string recLength = DBUtils.DocCache[docId].RecordingLength + " ";
			int pieces = recLength.Split(":".ToCharArray(), 3).Length;
			if (pieces == 3)
				recLength += ResourceHelper.GetString("kstidHoursAbbrev");
			else if (pieces == 2)
				recLength += ResourceHelper.GetString("kstidMinutesAbbrev");
			else
				recLength += ResourceHelper.GetString("kstidSecondsAbbrev");

			lblLengthVal.Text = recLength;
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="mediator"></param>
		/// <param name="configurationParameters"></param>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
			// Do nothing for this application.
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] {this});
		}

		#endregion

		private void tabDocViews_ClientSizeChanged(object sender, EventArgs e)
		{
			tpgTrans.Invalidate();
			tpgColView.Invalidate();
			tpgDocInfo.Invalidate();
		}
	}
}
