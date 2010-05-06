using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilUtils;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class FindDlg : Form
	{
		private const int kMaxSavedFindPatterns = 20;

		private bool m_cancel;
		private bool m_prevMatchCaseValue;
		private readonly PaWordListGrid m_grid;
		private static readonly List<string> s_findWhatItems = new List<string>();
		private static readonly List<string> s_colsToFindIn = new List<string>();
		private readonly List<char> m_reservedRegexChars =
			new List<char>(new[] { '\\', '[', '^', '$', '.', '|', '?', '*', '+', '(', ')' });

		#region Constructor & Closing
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// FindDlg constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FindDlg(PaWordListGrid grid)
		{
			Debug.Assert(grid != null);
			m_grid = grid;

			InitializeComponent();

			if (Settings.Default.FindDlgBounds.Height <= 0)
				StartPosition = FormStartPosition.CenterScreen;
			
			SetUiFonts();

			// Select previous selected columns
			fldSelGridSrchCols.Load(false, true, s_colsToFindIn);

			// Load the cbFindWhat comboBox with past searches
			foreach (string searchPattern in s_findWhatItems)
				cboFindWhat.Items.Add(searchPattern);

			LoadSettings();
			btnFind.Enabled = (fldSelGridSrchCols.CheckedFields.Count > 0);
			chkSrchCollapsedGrps.Enabled = (grid.IsGroupedByField || grid.Cache.IsCIEList);

			// Will prevent opening more than one FindDlg instance.
			FindInfo.FindDlgIsOpen = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set UI Fonts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetUiFonts()
		{
			chkRegEx.Font = FontHelper.UIFont;
			cboFindWhat.Font = FontHelper.PhoneticFont;
			chkMatchCase.Font = FontHelper.UIFont;
			chkMatchEntireWord.Font = FontHelper.UIFont;
			chkStartsWith.Font = FontHelper.UIFont;
			chkReverseSearch.Font = FontHelper.UIFont;
			lblSearchColumns.Font = FontHelper.UIFont;
			lblFindWhat.Font = FontHelper.UIFont;
			gbOptions.Font = FontHelper.UIFont;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load saved settings
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadSettings()
		{
			cboFindWhat.Text = (Settings.Default.FindDlgFindWhat ?? string.Empty);
			chkMatchCase.Checked = Settings.Default.FindDlgMatchCase;
			chkMatchEntireWord.Checked = Settings.Default.FindDlgMatchEntireWord;
			chkStartsWith.Checked = Settings.Default.FindDlgStartsWith;
			chkRegEx.Checked = Settings.Default.FindDlgRegEx;
			chkReverseSearch.Checked = Settings.Default.FindDlgReverse;
			chkSrchCollapsedGrps.Checked = Settings.Default.FindDlgSearchCollapsedGroups;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (Settings.Default.FindDlgBounds.Height > 0)
				Bounds = Settings.Default.FindDlgBounds;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// FindDlg Closing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			Settings.Default.FindDlgBounds = Bounds;

			// Save the FieldNames of the search columns for initial selection when
			// the FindDlg is reopened
			s_colsToFindIn.Clear();
			
			foreach (PaFieldInfo fieldInfo in fldSelGridSrchCols.CheckedFields)
				s_colsToFindIn.Add(fieldInfo.FieldName);
			
			// Save window settings if not canceled
			if (!m_cancel)
				SaveSettings();

			Settings.Default.Save();
			FindInfo.FindDlgIsOpen = false;
			base.OnClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save settings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveSettings()
		{
			if (cboFindWhat.Text.Trim() != string.Empty)
				Settings.Default.FindDlgFindWhat = cboFindWhat.Text.Trim();

			Settings.Default.FindDlgMatchCase = chkMatchCase.Checked;
			Settings.Default.FindDlgMatchEntireWord = chkMatchEntireWord.Checked;
			Settings.Default.FindDlgStartsWith = chkStartsWith.Checked;
			Settings.Default.FindDlgRegEx = chkRegEx.Checked;
			Settings.Default.FindDlgReverse = chkReverseSearch.Checked;
			Settings.Default.FindDlgSearchCollapsedGroups = chkSrchCollapsedGrps.Checked;
		}

		#endregion

		#region Searching
  	  	/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Formats the search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string formatFindPattern(string findWhatText)
		{
			string orginalFindPattern = FFNormalizer.Normalize(findWhatText);

			// Check if regular expression or simple search
			if (chkRegEx.Checked)
				return orginalFindPattern;

			StringBuilder findPattern = new StringBuilder();

			// Change all special characters to literals
			foreach (char c in orginalFindPattern)
			{
				if (m_reservedRegexChars.Contains(c))
					findPattern.Append('\\');
				findPattern.Append(c);
			}

			// 'Starts With' option
			if (chkStartsWith.Checked)
			{
				findPattern.Insert(0, "^");
				findPattern.Append(".*$");
			}

			// 'Match Entire Word' option
			if (chkMatchEntireWord.Checked)
			{
				//findPattern.Insert(0, @"\s[\[\{\(]?");
				//findPattern.Append(@"[\]\}\)\.]?\s");
				findPattern.Insert(0, "\\b");
				findPattern.Append("\\b");
			}

			// 'Match Case' option
			if (chkMatchCase.Checked)
			    findPattern.Insert(0, "(?-i)"); // Turn OFF case INsensitivity
			else
			    findPattern.Insert(0, "(?i)"); // Turn ON case INsensitivity

			return findPattern.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Call FindInfo to find matching cells based on the search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnFind_Click(object sender, EventArgs e)
		{
			if (!s_findWhatItems.Contains(cboFindWhat.Text))
				s_findWhatItems.Add(cboFindWhat.Text);
			if (s_findWhatItems.Count > kMaxSavedFindPatterns)
				s_findWhatItems.RemoveAt(0);

			FindInfo.Grid = m_grid;
			FindInfo.FindPattern = formatFindPattern(cboFindWhat.Text);
			FindInfo.FindText = cboFindWhat.Text;
			FindInfo.SearchCollapsedGroups = chkSrchCollapsedGrps.Checked;

			List<FindDlgColItem> columnsToSearch = new List<FindDlgColItem>();

			foreach (PaFieldInfo fieldInfo in fldSelGridSrchCols.CheckedFields)
			{
				columnsToSearch.Add(new FindDlgColItem(
					m_grid.Columns[fieldInfo.FieldName].Index,
					fieldInfo.DisplayIndexInGrid,
					fieldInfo.DisplayText, fieldInfo.FieldName));
			}

			FindInfo.ColumnsToSearch = columnsToSearch.ToArray();
			FindInfo.FindFirst(chkReverseSearch.Checked);
			Close();
		}

		#endregion

		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the Cancel button click.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCancel_Click(object sender, EventArgs e)
		{
			m_cancel = true;
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void fldSelGridSrchCols_AfterUserChangedValue(PaFieldInfo fieldInfo,
			bool selectAllValueChanged, bool newValue)
		{
			if (selectAllValueChanged || fieldInfo.IsPhonetic)
			{
				// Disable chkMatchCase if the "Select All" or phonetic field is checked.
				chkMatchCase.Enabled = !newValue;

				if (chkMatchCase.Enabled)
					chkMatchCase.Checked = m_prevMatchCaseValue;
				else
				{
					m_prevMatchCaseValue = chkMatchCase.Checked;
					chkMatchCase.Checked = false;
				}
			}

			// Enable the Find button if any columns are checked
			btnFind.Enabled = (fldSelGridSrchCols.CheckedFields.Count > 0); 
		}
		
		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Disable options when chkRegEx is checked.
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		private void cbRegEx_CheckedChanged(object sender, EventArgs e)
		{
			chkMatchCase.Enabled = !chkRegEx.Checked;
			chkMatchEntireWord.Enabled = !chkRegEx.Checked;
			chkStartsWith.Enabled = !chkRegEx.Checked;
			chkReverseSearch.Enabled = !chkRegEx.Checked;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Uncheck chkStartsWith when chkMatchEntireWord is checked.
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		private void chkMatchEntireWord_CheckedChanged(object sender, EventArgs e)
		{
			if (chkMatchEntireWord.Checked)
				chkStartsWith.Checked = false;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Uncheck chkMatchEntireWord when chkStartsWith is checked.
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		private void chkStartsWith_CheckedChanged(object sender, EventArgs e)
		{
			if (chkStartsWith.Checked)
				chkMatchEntireWord.Checked = false;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnHelp_Click(object sender, EventArgs e)
		{
			App.ShowHelpTopic(this);
		}

		#endregion

		#region Overrides
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			App.MsgMediator.SendMessage(Name + "HandleCreated", this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjust the form's height based on the panel's heights.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			cboFindWhat.Font = FontHelper.PhoneticFont;
			cboFindWhat.AutoCompleteSource = AutoCompleteSource.ListItems;
			base.OnShown(e);
		}

		#endregion

		#region Check the find options
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check or uncheck the MatchCase checkbox.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool MatchCase
		{
			get { return chkMatchCase.Checked; }
			set { chkMatchCase.Checked = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check or uncheck the MatchEntireWord checkbox.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool MatchEntireWord
		{
			get { return chkMatchEntireWord.Checked; }
			set { chkMatchEntireWord.Checked = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check or uncheck the StartsWith checkbox.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool StartsWith
		{
			get { return chkStartsWith.Checked; }
			set { chkStartsWith.Checked = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check or uncheck the Regular Expression checkbox.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsRegularExpression
		{
			get { return chkRegEx.Checked; }
			set { chkRegEx.Checked = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to search collapsed groups (only
		/// applies when the grid being searched is in grouped mode).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Search
		{
			get { return chkSrchCollapsedGrps.Checked; }
			set { chkSrchCollapsedGrps.Checked = value; }
		}

		#endregion
	}
}