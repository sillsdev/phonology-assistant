using System;
using System.Windows.Forms;
using System.Data;
using SIL.SpeechTools.Database;

namespace SIL.Pa
{
	/// <summary>
	/// Basically stores info about what's to be found / what was last
	/// searched for. A DataView filters those rows in the DataTable which
	/// satisfy the search criterion specified by the user. We then access
	/// the array of rows from m_dataview[0] up to m_dataview[m_dataview.Count]
	/// Later, we may try to allow it to start from the currently selected
	/// item in ContentWnd without the user having conducted a prior search
	/// during a given session.
	/// 
	/// We shall use "ItemName LIKE '*" + m_SearchText + "*'" as the search criterion,
	/// unless we're searching for a Speaker, in which case we'll use the '=' operator
	/// refer to http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemdatadatacolumnclassexpressiontopic.asp
	/// for details on valid expressions for m_dataview.RowFilter
	/// (it's about DataColumn.Expression)
	/// </summary>
	public class FindClass
	{
		public const int TreeIndex = 0;
		public const int EticIndex = 1;
		public const int ToneIndex = 2;
		public const int EmicIndex = 3;
		public const int OrthoIndex = 4;
		public const int GlossIndex = 5;
		public const int POSIndex = 6;
		public const int RefIndex = 7;
		public const int FreeIndex = 8;
		public const int CmntIndex = 9;
		public const int ScribeIndex = 10;
		public const int NBRefIndex = 11;
		public const int SpkrIndex = 12;
		public const int GoneFullCircle = -2;
		public const int NoAnchor = -1;
		private const int RSTypeNonGrid = 0;
		private const int RSTypeGrid = 1;
		private const string NoMoreMatchesMsg = "No more matches found!";
		private const string NoMatchesMsg = "No matches found!";

		private bool m_TreeChangeFromFindClass;
		private bool m_GridSearch;
		private bool m_FindDlgCanceled;
		private bool m_MatchCase;
		private bool m_FindInCurrRecOnly;
		private int m_LastRSType;
		private int m_Gender;
		private int m_Field;
		private int m_PrevField;
		private int m_AnchorID;
		private int m_AnchorCursor;
		private int m_LastCursor;
		private string m_DBField;
		private string m_SearchText;
		private Control m_ReturnTo;
		private ContentWnd m_cwnd;
		private PaDataTable m_dtable;
		/// <summary>
		/// Contains the rows which satisfy the given search criterion.
		/// </summary>
		private DataView m_dview;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FindClass()
		{
			m_MatchCase = false;
			m_GridSearch = false;
			m_FindDlgCanceled = false;
			m_TreeChangeFromFindClass = false;
			m_LastRSType = -1;
			m_PrevField = -1;
			m_AnchorID = NoAnchor;
			m_AnchorCursor = NoAnchor;
			m_LastCursor = 0;
			m_SearchText = string.Empty;
			m_dtable = null;
			m_ReturnTo = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ExecuteFind()
		{
			try
			{
				if (m_AnchorID == GoneFullCircle)
				{
					MessageBox.Show(NoMoreMatchesMsg, Application.ProductName,
						MessageBoxButtons.OK, MessageBoxIcon.Information);
					ResetAnchor();
					return;
				}

				if (m_Field == TreeIndex)
				{
					//FindInTree();
				}
				else if (m_GridSearch)
				{
					if (m_FindInCurrRecOnly)
					{
						//FindInGridCurrRecOnly();
					}
					else
					{
						//FindInGrid;
					}
				}
				else if (m_Field == SpkrIndex)
				{
					//FindSpeaker();
				}
				else
				{
					switch (m_Field)
					{
						case EticIndex:
						case EmicIndex:
						case OrthoIndex:
						case FreeIndex:
						case CmntIndex:
						case NBRefIndex:
						case ScribeIndex:
							if (m_FindInCurrRecOnly)
							{
								//FindInFieldCurrRecOnly();
							}
							else
								FindInField();
							break;
					}
				}

				m_ReturnTo.Focus();
			}
			catch {}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ResetAnchor()
		{
			try
			{
				if (!m_TreeChangeFromFindClass)
				{
					m_AnchorID = NoAnchor;
					m_AnchorCursor = NoAnchor;
					m_LastCursor = 0;
					// rsFind.Close; rsFind = null;
				}
			}
			catch {}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fld"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public Control FindFieldTextBox(int fld)
		{
			switch (fld)
			{
				case EticIndex:	return m_cwnd.EticTextBox;
				case EmicIndex:	return m_cwnd.EmicTextBox;
				case OrthoIndex: return m_cwnd.OrthoTextBox;
				case FreeIndex:	return m_cwnd.FreeTextBox;
				case CmntIndex:	return m_cwnd.CommentTextBox;
				case NBRefIndex: return m_cwnd.NBRefTextBox;
				case ScribeIndex: return m_cwnd.TranscriberTextBox;
				case SpkrIndex:	return m_cwnd.SpeakerComboBox;
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void FindInField()
		{
			try
			{
				if (m_dtable == null)
				{
					MessageBox.Show(NoMatchesMsg, "Phonology Assistant",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
					ResetAnchor();
					return;
				}
			}

			catch
			{
				// LogError(this.ToString(), "FindInField", true)
			}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool FindDlgCanceled
		{
			get {return m_FindDlgCanceled;}
			set {m_FindDlgCanceled = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int FindField
		{
			get {return m_Field;}
			set
			{
				m_Field = value;

				switch (value)
				{
					case EticIndex:
						m_DBField = (m_GridSearch ? "Phonetic" : "IPAFile");
						m_MatchCase = true;
						break;
					case EmicIndex:
						m_DBField = (m_GridSearch ? "Phonemic" : "FullPhonemic");
						m_MatchCase = true;
						break;
					case OrthoIndex:
						m_DBField = (m_GridSearch ? "Ortho" : "FullOrtho");
						break;
					case SpkrIndex:
						m_DBField = "SpeakerName";
						break;
					case FreeIndex:
						m_DBField = "Freeform";
						break;
					case CmntIndex:
						m_DBField = "Comments";
						break;
					case NBRefIndex:
						m_DBField = "Reference";
						break;
					case ScribeIndex:
						m_DBField = "Transcriber";
						break;
					case POSIndex:
						m_DBField = "POS";
						m_GridSearch = true;
						break;
					case ToneIndex:
						m_DBField = "Tone";
						m_GridSearch = true;
						m_MatchCase = true;
						break;
					case GlossIndex:
						m_DBField = "Gloss";
						m_GridSearch = true;
						break;
					case RefIndex:
						m_DBField = "WordRef";
						m_GridSearch = true;
						break;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool FindInCurrRecOnly
		{
			get {return m_FindInCurrRecOnly;}
			set {m_FindInCurrRecOnly = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Gender
		{
			get {return m_Gender;}
			set {m_Gender = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool MatchCase
		{
			get {return m_MatchCase;}
			set {m_MatchCase = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int PrevFindField
		{
			get {return m_PrevField;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the ContentWnd to the currently open one.
		/// (There should only be one such window.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ContentWnd ContentWindow
		{
			get {return m_cwnd;}
			set {m_cwnd = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Control ReturnControl
		{
			get {return m_ReturnTo;}
			set
			{
				m_ReturnTo = value;

				if (value.Name == "tvContents")
					m_Field = TreeIndex;
				else if (value.Name == "ltbEtic")
					m_Field = EticIndex;
				else if (value.Name == "ltbEmic")
					m_Field = EmicIndex;
				else if (value.Name == "ltbOrtho")
					m_Field = OrthoIndex;
				//else if (value is FieldWorks.Common.Controls.FwGrid)
				//{
				//    m_Field = EticIndex;
				//    m_GridSearch = true;
				//}
				else if (value.Name == "ltbFreeForm")
					m_Field = FreeIndex;
				else if (value.Name == "ltbComments")
					m_Field = CmntIndex;
				else if (value.Name == "llbTranscriber")
					m_Field = ScribeIndex;
				else if (value.Name == "llbNotebookRef")
					m_Field = NBRefIndex;
				else if (value.Name == "cboSpeaker" || value is RadioButton)
					m_Field = SpkrIndex;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SearchGrid
		{
			set {m_GridSearch = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the search string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SearchText
		{
			get {return m_SearchText;}
			set {m_SearchText = value;}
		}
		
		#endregion
	}
}
