using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Localization;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class DefinePhoneClassDlg : DefineClassBaseDlg
	{
		private IPACharacterExplorer _charExplorer;

		#region Construction and setup
		/// ------------------------------------------------------------------------------------
		public DefinePhoneClassDlg(ClassListViewItem classInfo, ClassesDlg classDlg)
			: base(classInfo ?? new ClassListViewItem { ClassType = SearchClassType.Phones }, classDlg)
		{
			InitializeComponent();
			IinitializeCharExplorer();
			_textBoxMembers.ReadOnly = false;
			_textBoxMembers.Font = FontHelper.MakeRegularFontDerivative(App.PhoneticFont, 16);
		}

		/// ------------------------------------------------------------------------------------
		protected override void SetLocalizedTexts()
		{
			_labelClassTypeValue.Text = LocalizationManager.GetString(
				"DialogBoxes.DefineClassesDialogs.PhoneClassDlg.ClassTypeLabel",
				"Phones");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Iinitializes the IPA character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void IinitializeCharExplorer()
		{
			_charExplorer = new IPACharacterExplorer();
			_charExplorer.AutoScroll = true;
			_charExplorer.TabIndex = 2;
			_charExplorer.CharPicked += HandleIPACharPicked;
			_charExplorer.Dock = DockStyle.Fill;
			_charExplorer.BackColor = SystemColors.Window;
			_charExplorer.Load((int)IPASymbolType.All | (int)IPASymbolSubType.All);

			_panelMemberPickingContainer.Controls.Add(_charExplorer);
		}

		#endregion

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			_charExplorer.LoadSettings(Settings.Default.DefinePhoneClassDlgIPACharExplorerExpandedStates);
			base.OnShown(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.DefinePhoneClassDlgIPACharExplorerExpandedStates = _charExplorer.GetExpandedStates();
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			// Check if any of the characters entered are invalid.
			var undefinedChars = _textBoxMembers.Text.Trim().Replace(",", string.Empty)
				.Where(c => App.IPASymbolCache[c] == null || App.IPASymbolCache[c].IsUndefined).ToArray();

			if (undefinedChars.Length > 0)
			{
				using (var dlg = new UndefinedCharactersInClassDlg(undefinedChars))
					dlg.ShowDialog(this);
			}

			return base.SaveChanges();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the pattern that would be built from the contents of the members text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string CurrentPattern
		{
			get
			{
				string phones = _textBoxMembers.Text.Trim().Replace(",", string.Empty);
				phones = m_classesDlg.Project.PhoneticParser.PhoneticParser_CommaDelimited(phones, true, true);
				return "{" + (phones ?? string.Empty) + "}";
			}
		}

		#endregion

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			App.ShowHelpTopic("hidPhoneticCharacterClassDlg");
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user clicking on an IPA character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleIPACharPicked(CharPicker chooser, ToolStripButton item)
		{
			InsertText(item.Text.Replace(App.DottedCircle, string.Empty));
		}

		/// ------------------------------------------------------------------------------------
		private void InsertText(string itemText)
		{
			var charInfo = App.IPASymbolCache[itemText];
			bool isBase = (charInfo == null || charInfo.IsBase);

			int selStart = _textBoxMembers.SelectionStart;
			int selLen = _textBoxMembers.SelectionLength;

			// First, if there is a selection, get rid of the selected text.
			if (selLen > 0)
				_textBoxMembers.Text = _textBoxMembers.Text.Remove(selStart, selLen);

			// Check if what's being inserted needs to be preceded by a comma.
			if (selStart > 0 && _textBoxMembers.Text[selStart - 1] != ',' && isBase)
			{
				_textBoxMembers.Text = _textBoxMembers.Text.Insert(selStart, ",");
				selStart++;
			}

			_textBoxMembers.Text = _textBoxMembers.Text.Insert(selStart, itemText);
			_textBoxMembers.SelectionStart = selStart + itemText.Length;

			m_classInfo.IsDirty = true;
		}

		#endregion
	}
}