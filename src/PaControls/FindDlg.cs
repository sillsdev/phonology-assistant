using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	public partial class FindDlg : Form
	{
		#region Member Variables
		// Declare member variables
		private List<char> m_reservedRegexChars =
			new List<char>(new char[] { '\\', '[', '^', '$', '.', '|', '?', '*', '+', '(', ')' });
		private PaWordListGrid m_grid;
		private static ArrayList m_findWhatItems = new ArrayList();
		private string m_cellValue = string.Empty;
		private int m_maxSavedFindPatterns = 20;
		private static List<string> s_colsToFindIn = new List<string>();
		private string m_findPattern = string.Empty;
		private bool m_cancel = false;
		private int m_dyHeightClientHeight = 0;
		private int m_optionsPanelHeight;
		private bool m_prevMatchCaseValue = false;
		# endregion

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
			SetUiFonts();

			// Select previous selected columns
			fldSelGridSrchCols.Load(false, true, s_colsToFindIn);

			// Load the cbFindWhat comboBox with past searches
			foreach (string searchPattern in m_findWhatItems)
				cboFindWhat.Items.Add(searchPattern);

			LoadSettings();
			btnFind1.Enabled = (fldSelGridSrchCols.CheckedFields.Count > 0);
			chkSrchCollapsedGrps.Enabled = grid.IsGroupedByField || grid.Cache.IsCIEList;

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
		/// Load saved settings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadSettings()
		{
			// Load saved window settings
			cboFindWhat.Text = PaApp.SettingsHandler.GetStringSettingsValue(Name, "findwhat", string.Empty);
			chkMatchCase.Checked = PaApp.SettingsHandler.GetBoolSettingsValue(Name, "matchcase", false);
			chkMatchEntireWord.Checked =
				PaApp.SettingsHandler.GetBoolSettingsValue(Name, "matchentireword", false);
			
			chkStartsWith.Checked = PaApp.SettingsHandler.GetBoolSettingsValue(Name, "startswith", false);
			chkRegEx.Checked = PaApp.SettingsHandler.GetBoolSettingsValue(Name, "regex", false);
			chkReverseSearch.Checked = PaApp.SettingsHandler.GetBoolSettingsValue(Name, "reverse", false);
			chkSrchCollapsedGrps.Checked = PaApp.SettingsHandler.GetBoolSettingsValue(Name, "srchcollapsedgrps", true);

			// Save the height of the form minus the caption bar.
			m_optionsPanelHeight = pnlColumnOptions.Height;
			m_dyHeightClientHeight = Height - ClientSize.Height;
			int saveOriginalHeight = Height;
			PaApp.SettingsHandler.LoadFormProperties(this);
			Height = saveOriginalHeight;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// FindDlg Closing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			PaApp.SettingsHandler.SaveFormProperties(this);

			// Save the FieldNames of the search columns for initial selection when
			// the FindDlg is reopened
			s_colsToFindIn.Clear();
			foreach (PaFieldInfo fieldInfo in fldSelGridSrchCols.CheckedFields)
				s_colsToFindIn.Add(fieldInfo.FieldName);
			
			// Save window settings if not canceled
			if (!m_cancel)
				SaveSettings();

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
			PaApp.SettingsHandler.SaveSettingsValue(Name, "findwhat", cboFindWhat.Text);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "matchcase", chkMatchCase.Checked);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "matchentireword", chkMatchEntireWord.Checked);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "startswith", chkStartsWith.Checked);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "regex", chkRegEx.Checked);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "reverse", chkReverseSearch.Checked);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "srchcollapsedgrps", chkSrchCollapsedGrps.Checked);
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
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void btnFind(object sender, EventArgs e)
		{
			if (!m_findWhatItems.Contains(cboFindWhat.Text))
				m_findWhatItems.Add(cboFindWhat.Text);
			if (m_findWhatItems.Count > m_maxSavedFindPatterns)
				m_findWhatItems.RemoveAt(0);

			FindInfo.Grid = m_grid;
			FindInfo.FindPattern = formatFindPattern(cboFindWhat.Text);
			FindInfo.FindText = cboFindWhat.Text;
			FindInfo.SearchCollapsedGroups = chkSrchCollapsedGrps.Checked;

			List<FindDlgColItem> columnsToSearch = new List<FindDlgColItem>();

			foreach (PaFieldInfo fieldInfo in fldSelGridSrchCols.CheckedFields)
			{
				FindDlgColItem item = new FindDlgColItem(
					m_grid.Columns[fieldInfo.FieldName].Index,
					fieldInfo.DisplayIndexInGrid,
					fieldInfo.DisplayText, fieldInfo.FieldName);

				if (item != null)
					columnsToSearch.Add(item);
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
		/// <param name="e"></param>
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
			btnFind1.Enabled = (fldSelGridSrchCols.CheckedFields.Count > 0); 
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
			PaApp.ShowHelpTopic(this);
		}

		#endregion

		#region Overrides
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjust the form's height based on the panel's heights.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			cboFindWhat.Font = FontHelper.PhoneticFont;
			Height = pnlFindWhat.Height + m_optionsPanelHeight +
				pnlButtons.Height + m_dyHeightClientHeight;
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