// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class FindDlg : Form
	{
		private const int kMaxSavedFindPatterns = 20;

		private static readonly List<string> s_findWhatItems = new List<string>();
		private static List<string> s_checkedFields;
		
		private bool m_cancel;
		private bool m_prevMatchCaseValue;
		private readonly PaWordListGrid m_grid;
		private readonly List<char> m_reservedRegexChars =
			new List<char>(new[] { '\\', '[', '^', '$', '.', '|', '?', '*', '+', '(', ')' });

		#region Constructor & Closing
		/// ------------------------------------------------------------------------------------
		public FindDlg(PaWordListGrid grid)
		{
			Debug.Assert(grid != null);
			m_grid = grid;

			InitializeComponent();
			
			Settings.Default.FindDlg = App.InitializeForm(this, Settings.Default.FindDlg);
			SetUiFonts();

			var fieldsInList = from field in App.Project.Fields
							   where field.VisibleInGrid && field.DisplayIndexInGrid >= 0 && m_grid.Columns[field.Name] != null
							   orderby field.DisplayIndexInGrid
							   select field;

			if (s_checkedFields == null)
				s_checkedFields = fieldsInList.Select(f => f.Name).ToList();

			fldSelGridSrchCols.Load(fieldsInList.Select(f =>
				new KeyValuePair<PaField, bool>(f,  s_checkedFields.Contains(f.Name))));

			// Load the cbFindWhat comboBox with past searches
			foreach (string searchPattern in s_findWhatItems)
				cboFindWhat.Items.Add(searchPattern);

			LoadSettings();
			btnFind.Enabled = (fldSelGridSrchCols.CheckedItemCount > 0);
            chkSrchCollapsedGrps.Enabled = (grid.IsGroupedByField || grid.Cache.IsMinimalPair || grid.Cache.IsSimilarEnvironment);

			// Will prevent opening more than one FindDlg instance.
			FindInfo.FindDlgIsOpen = true;
		}

		/// ------------------------------------------------------------------------------------
		public void SetUiFonts()
		{
			chkRegEx.Font = FontHelper.UIFont;
			cboFindWhat.Font = App.PhoneticFont;
			chkMatchCase.Font = FontHelper.UIFont;
			chkMatchEntireWord.Font = FontHelper.UIFont;
			chkStartsWith.Font = FontHelper.UIFont;
			chkReverseSearch.Font = FontHelper.UIFont;
			lblSearchColumns.Font = FontHelper.UIFont;
			lblFindWhat.Font = FontHelper.UIFont;
			gbOptions.Font = FontHelper.UIFont;

			LayoutEngine.Layout(this, new LayoutEventArgs(tblLayoutFindWhat, "Height"));
		}

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
		protected override void OnClosing(CancelEventArgs e)
		{
			// Save the checked fields for initial selection when the FindDlg is reopened
			s_checkedFields = fldSelGridSrchCols.GetCheckedFields().Select(f => f.Name).ToList();
			
			// Save window settings if not canceled
			if (!m_cancel)
				SaveSettings();

			Settings.Default.Save();
			FindInfo.FindDlgIsOpen = false;
			base.OnClosing(e);
		}

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

			var findPattern = new StringBuilder();

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

			FindInfo.ColumnsToSearch = fldSelGridSrchCols.GetCheckedFields().Select(field =>
				new FindDlgColItem(m_grid.Columns[field.Name].Index,
					field.DisplayIndexInGrid, field.DisplayName, field.Name)).ToArray();
			
			FindInfo.FindFirst(chkReverseSearch.Checked);
			Close();
		}

		#endregion

		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		private void btnCancel_Click(object sender, EventArgs e)
		{
			m_cancel = true;
			Close();
		}

		/// ------------------------------------------------------------------------------------
		private void fldSelGridSrchCols_AfterUserChangedValue(PaField field,
			bool selectAllValueChanged, bool newValue)
		{
			if (selectAllValueChanged || field.Type == FieldType.Phonetic)
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
			btnFind.Enabled = (fldSelGridSrchCols.CheckedItemCount > 0); 
		}
		
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
		private void btnHelp_Click(object sender, EventArgs e)
		{
			App.ShowHelpTopic(this);
		}

		#endregion

		#region Overrides
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
			cboFindWhat.AutoCompleteSource = AutoCompleteSource.ListItems;
			cboFindWhat.Height = cboFindWhat.PreferredHeight;
			base.OnShown(e);
		}

        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.Escape:
                    {
                        this.Close();
                        return true;
                    }
                case Keys.Control | Keys.Tab:
                    {
                        return true;
                    }
                case Keys.Control | Keys.Shift | Keys.Tab:
                    {
                        return true;
                    }
            }
            return base.ProcessCmdKey(ref message, keys);
        }
		#endregion

		#region Check the find options
		/// ------------------------------------------------------------------------------------
		public bool MatchCase
		{
			get { return chkMatchCase.Checked; }
			set { chkMatchCase.Checked = value; }
		}

		/// ------------------------------------------------------------------------------------
		public bool MatchEntireWord
		{
			get { return chkMatchEntireWord.Checked; }
			set { chkMatchEntireWord.Checked = value; }
		}

		/// ------------------------------------------------------------------------------------
		public bool StartsWith
		{
			get { return chkStartsWith.Checked; }
			set { chkStartsWith.Checked = value; }
		}

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