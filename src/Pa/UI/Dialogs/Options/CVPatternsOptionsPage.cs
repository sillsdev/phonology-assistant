using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	public partial class CVPatternsOptionsPage : OptionsDlgPageBase
	{
		private bool m_handleTextChange = true;
		
		/// ------------------------------------------------------------------------------------
		public CVPatternsOptionsPage(PaProject project) : base(project)
		{
			InitializeComponent();

			var field = m_project.Fields.SingleOrDefault(f => f.Name == PaField.kCVPatternFieldName);
			var cvFont = (field == null ? App.PhoneticFont : field.Font);

			// Assign the fonts
			grpDisplayChars.Font = FontHelper.UIFont;
			lblInstruction.Font = FontHelper.UIFont;
			lblExampleDesc1.Font = FontHelper.UIFont;
			lblExampleDesc2.Font = FontHelper.UIFont;
			txtCustomChars.Font = FontHelper.MakeRegularFontDerivative(cvFont, txtCustomChars.Font.Size);
			txtExampleInput.Font = FontHelper.MakeRegularFontDerivative(cvFont, txtExampleInput.Font.Size);
			lblExampleCV.Font = FontHelper.MakeRegularFontDerivative(cvFont, lblExampleCV.Font.Size);
			lblExampleCVCV.Font = FontHelper.MakeRegularFontDerivative(cvFont, lblExampleCVCV.Font.Size);

			// Check the appropriate symbols in the symbol explorer.
			_cvPatternSymbolExplorer.SetCheckedItemsByText(m_project.CVPatternInfoList
				.Where(c => c.Type == CVPatternInfo.PatternType.Suprasegmental)
				.Select(c => c.Phone));
			
			LoadCustomType();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load custom type display characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadCustomType()
		{
			txtCustomChars.TextChanged += HandleCustomCharsTextChanged;
			txtCustomChars.KeyDown += HandleCustomCharsTextBoxKeyDown;

			foreach (var info in m_project.CVPatternInfoList.Where(i => i.Type == CVPatternInfo.PatternType.Custom))
			{
				if (txtCustomChars.TextLength == 0)
					txtCustomChars.Text = info.Phone;
				else
					txtCustomChars.Text += (" " + info.Phone);
			}
		}

		/// ------------------------------------------------------------------------------------
		public override string TabPageText
		{
			get { return App.GetString("DialogBoxes.OptionsDlg.CVPatternsTab.TabText", "CV Patterns"); }
		}

		/// ------------------------------------------------------------------------------------
		public override string HelpId
		{
			get { return "hidCVPatternsOptions"; }
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsDirty
		{
			get
			{
				var originalList = m_project.CVPatternInfoList.Select(c => c.Phone).OrderBy(c => c).ToArray();
				var newList = GetCurrentSymbolsToDisplayInPatterns().Select(c => c.Phone).OrderBy(c => c).ToArray();

				return (originalList.Length != newList.Length) ||
					newList.Where((t, i) => originalList[i] != t).Any();
			}
		}

		/// ------------------------------------------------------------------------------------
		public override void Save()
		{
			txtCustomChars.TextChanged -= HandleCustomCharsTextChanged;

			m_project.CVPatternInfoList = GetCurrentSymbolsToDisplayInPatterns().ToList();

			try
			{
				App.MsgMediator.SendMessage("CVPatternsChanged", null);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<CVPatternInfo> GetCurrentSymbolsToDisplayInPatterns()
		{
			// First build a list of the checked symbols
			var list = _cvPatternSymbolExplorer.GetTextsOfAllCheckedItems()
				.Select(symbol => CVPatternInfo.Create(symbol, CVPatternInfo.PatternType.Suprasegmental)).ToList();

			// Now add to the list of checked symbols, the list of custom segments entered.
			var split = txtCustomChars.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var seg in split.Select(p => p.Replace(App.kDottedCircle, string.Empty))
				.Where(s => s != string.Empty && !list.Any(c => c.Phone == s))
				.Distinct(StringComparer.Ordinal))
			{
				list.Add(CVPatternInfo.Create(seg, CVPatternInfo.PatternType.Custom));
			}

			return list;
		}

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		private void HandleCustomCharsTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (!e.Control || e.KeyCode != Keys.D0)
				return;

			// Ctrl-0 inserts a dotted circle.
			int selStart = txtCustomChars.SelectionStart;
			if (txtCustomChars.SelectionLength > 0)
			{
				// Remove any selected text.
				m_handleTextChange = false;
				txtCustomChars.Text =
				txtCustomChars.Text.Remove(selStart, txtCustomChars.SelectionLength);
				m_handleTextChange = true;
			}

			txtCustomChars.Text = txtCustomChars.Text.Insert(selStart, App.kDottedCircle);
			txtCustomChars.SelectionStart = selStart + 1;
			e.SuppressKeyPress = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the tbDisplayChars's text input.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCustomCharsTextChanged(object sender, EventArgs e)
		{
			// Return if already handling a text change with this method
			if (!m_handleTextChange)
				return;

			m_handleTextChange = false;

			var verifiedText = new StringBuilder();
			int selStart = txtCustomChars.SelectionStart;

			if (txtCustomChars.TextLength > 0)
			{
				// Verify the text character by character
				for (int i = 0; i < txtCustomChars.TextLength; i++)
				{
					// Continue if it's the dotted circle
					if (txtCustomChars.Text[i].ToString() == App.kDottedCircle)
					{
						verifiedText.Append(App.kDottedCircle);
						continue;
					}

					var charInfo = App.IPASymbolCache[txtCustomChars.Text[i]];

					// Eat the character if it is not in the IPACharCache
					if (charInfo == null)
						selStart--;
					else
					{
						// Insert the dotted circle if the character is at the beginning or after
						// a space and the character should be displayed with the dotted circle.
						if (charInfo.DisplayWithDottedCircle &&
							((i == 0 || txtCustomChars.Text[i - 1] == ' ')) &&
							(i + 1 < txtCustomChars.Text.Length && txtCustomChars.Text[i + 1] != App.kDottedCircleC))
						{
							verifiedText.Append(App.kDottedCircle);
							selStart++;
						}

						verifiedText.Append(txtCustomChars.Text[i]);
					}
				}

				txtCustomChars.Text = verifiedText.ToString();
				// Set the cursor at the end of the text
				txtCustomChars.SelectionStart = selStart;
			}

			m_handleTextChange = true;
		}

		#endregion
	}
}
