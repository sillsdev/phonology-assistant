using System;
using System.Windows.Forms;
using System.Data;
using SIL.SpeechTools.Database;

namespace SIL.Pa
{
	public enum PaTreeNodeType
	{
		/// <summary>Tree node is a category.</summary>
		Category,
		/// <summary>Tree node is a folder.</summary>
		Folder,
		/// <summary>Tree node is a document.</summary>
		Document,
		/// <summary>Tree node is undefined.</summary>
		Undefined
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Defines a class for nodes in the content tree of the content window.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaTreeNode : TreeNode
	{
		private int m_id = 0;
		private int m_docLinkId = 0;
		private string m_idField;
		private PaTreeNodeType m_nodeType = PaTreeNodeType.Undefined;
		private bool m_isAudioDoc = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs and returns a new category node.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static PaTreeNode CreateCategoryNode(string title, int id)
		{
			return new PaTreeNode(title, id, PaTreeNodeType.Category, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs and returns a new category node.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static PaTreeNode CreateFolderNode(string title, int id)
		{
			return new PaTreeNode(title, id, PaTreeNodeType.Folder, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs and returns a new document node.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="id"></param>
		/// <param name="isAudioDoc"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static PaTreeNode CreateDocumentNode(string title, int docLinkId, int id, bool isAudioDoc)
		{
			PaTreeNode node = new PaTreeNode(title, id, PaTreeNodeType.Document, isAudioDoc);
			node.m_docLinkId = docLinkId;
			node.Name = DBFields.DocLinkId + docLinkId;
			return node;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="id"></param>
		/// <param name="type"></param>
		/// <param name="isAudioDoc"></param>
		/// ------------------------------------------------------------------------------------
		public PaTreeNode(string text, int id, PaTreeNodeType type, bool isAudioDoc) : base(text)
		{
			m_id = id;
			m_nodeType = type;
			m_isAudioDoc = isAudioDoc;

			switch (type)
			{
				case PaTreeNodeType.Category:
					m_idField = DBFields.CatId;
					ImageIndex = 3;
					break;

				case PaTreeNodeType.Folder:
					m_idField = DBFields.FolderId;
					ImageIndex = 2;
					break;

				case PaTreeNodeType.Document:
					m_idField = DBFields.DocId;
					ImageIndex = (isAudioDoc ? 0 : 1);
					break;
			}

			Name = m_idField + id;
			SelectedImageIndex = ImageIndex;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the ID of the tree node. This is the ID in the database. That is,
		/// the CatID, FolderID or DocID.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Id
		{
			get {return m_id;}
			//internal set {m_id = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the document link Id when the PaTreeNodeType is Document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int DocLinkId
		{
			get {return m_docLinkId;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the id field for the database table from which the tree node gets its text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DBField
		{
			get {return m_idField;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the id of the category that is the parent for the node, if the node is 
		/// document or folder node. If the node is a category, or the node has no parent or
		/// its of the wrong type, then 0 is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int NodesParentCategoryId
		{
			get
			{
				try
				{
					return (NodeType == PaTreeNodeType.Document ?
						((Parent as PaTreeNode).Parent as PaTreeNode).Id : (Parent as PaTreeNode).Id);
				}
				catch
				{
					return 0;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the category that is the parent for the node, if the node is 
		/// document or folder node. If the node is a category, or the node has no parent or
		/// its of the wrong type, then null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NodesParentCategory
		{
			get
			{
				try
				{
					return (NodeType == PaTreeNodeType.Document ?
						((Parent as PaTreeNode).Parent as PaTreeNode).Text : (Parent as PaTreeNode).Text);
				}
				catch
				{
					return null;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the id of the folder that is the parent for the node, if the node is 
		/// document node. If the node is a folder or category, or the node has no parent or
		/// its of the wrong type, then 0 is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int NodesParentFolderId
		{
			get
			{
				if (NodeType != PaTreeNodeType.Document || Parent == null || !(Parent is PaTreeNode))
					return 0;
				
				return (Parent as PaTreeNode).Id;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the folder that is the parent for the node, if the node is 
		/// document node. If the node is a folder or category, or the node has no parent or
		/// its of the wrong type, then null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NodesParentFolder
		{
			get
			{
				if (NodeType != PaTreeNodeType.Document || Parent == null || !(Parent is PaTreeNode))
					return null;
				
				return (Parent as PaTreeNode).Text;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the type of node this is.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaTreeNodeType NodeType
		{
			get {return m_nodeType;}
			set {m_nodeType = value;}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not this node represents a document that
		/// has an audio file associated with it. This property is on relevant to nodes whose
		/// type is PaTreeNodeType.Category.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsAudioDocument
		{
			get {return m_isAudioDoc;}
			set {m_isAudioDoc = value;}
		}

		#endregion

		#region Renaming Nodes Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Renames the title associated with this node to the specified new title. This title
		/// is also changed in the database.
		/// </summary>
		/// <param name="newTitle"></param>
		/// <returns><c>true</c> if the renaming succeeded. Otherwise, <c>false</c>.</returns>
		/// ------------------------------------------------------------------------------------
		public bool RenameTitle(string newTitle)
		{
			string titleField = null;

			switch (m_nodeType)
			{
				case PaTreeNodeType.Category:
					if (DBUtils.CategoryExists(newTitle))
						return false;
					titleField = DBFields.CatTitle;
					break;
			
				case PaTreeNodeType.Folder:
					if (DBUtils.FolderExists(newTitle))
						return false;
					titleField = DBFields.FolderTitle;
					break;

				case PaTreeNodeType.Document:
					if (DBUtils.DocumentExists(newTitle))
						return false;
					titleField = DBFields.DocTitle;
					break;
			}

			return DBUtils.RenameTitle(Id, titleField, newTitle);
		}

		#endregion
	}
}
