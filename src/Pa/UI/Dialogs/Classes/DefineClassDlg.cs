using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Localization.UI;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;
using Utils=SilTools.Utils;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class DefineClassDlg : OKCancelDlgBase
	{
		private readonly ClassListView lvClasses;
		private bool m_splitterSettingsLoaded;
		private PhonesInFeatureViewer m_conViewer;
		private PhonesInFeatureViewer m_vowViewer;
		private PhonesInFeatureViewer m_otherPhonesViewer;
		private readonly ClassesDlg m_classesDlg;
		private readonly ClassListViewItem m_classInfo;
		private readonly ClassListViewItem m_origClassInfo;
		private readonly FeatureListView m_lvArticulatoryFeatures;
		private readonly FeatureListView m_lvBinaryFeatures;
		private readonly Dictionary<SearchClassType, Control> m_ctrls = new Dictionary<SearchClassType, Control>();

		#region Construction and setup
		/// ------------------------------------------------------------------------------------
		private DefineClassDlg()
		{
			Utils.WaitCursors(true);
			lvClasses = new ClassListView();
			lvClasses.KeyPress += HandleClassesListViewKeyPress;
			lvClasses.DoubleClick += HandleClassesListViewDoubleClick;

			InitializeComponent();

			pnlMemberPickingContainer.Controls.Add(lvClasses);

			lblClassType.Font = FontHelper.UIFont;
			lblClassTypeValue.Font = FontHelper.UIFont;
			lblClassName.Font = FontHelper.UIFont;
			txtClassName.Font = FontHelper.UIFont;
			lblMembers.Font = FontHelper.UIFont;
			rbMatchAll.Font = FontHelper.UIFont;
			rbMatchAny.Font = FontHelper.UIFont;

			//lvClasses.Dock = DockStyle.Fill;
			//lvClasses.LoadSettings(Name);

			IinitializeCharExplorer();

			m_lvArticulatoryFeatures = InitializeFeatureList(App.FeatureType.Articulatory);
			m_lvBinaryFeatures = InitializeFeatureList(App.FeatureType.Binary);

			m_ctrls[SearchClassType.Phones] = charExplorer;
			m_ctrls[SearchClassType.Articulatory] = splitOuter;
			m_ctrls[SearchClassType.Binary] = splitOuter;
			//m_ctrls[SearchClassType.OtherClass] = lvClasses;

			splitOuter.Dock = DockStyle.Fill;

			LocalizeItemDlg.StringsLocalized += SetClassTypeTexts;
		}

		/// ------------------------------------------------------------------------------------
		private DefineClassDlg(ClassesDlg classDlg) : this()
		{
			m_classesDlg = classDlg;
			SetupPhoneViewers();
		}

		/// ------------------------------------------------------------------------------------
		public DefineClassDlg(SearchClassType type, ClassesDlg classDlg) : this(classDlg)
		{
			m_classInfo = new ClassListViewItem();
			m_classInfo.ClassType = type;
			Setup();
		}
		
		/// ------------------------------------------------------------------------------------
		public DefineClassDlg(ClassListViewItem classInfo, ClassesDlg classDlg) : this(classDlg)
		{
			Debug.Assert(classInfo != null);
			m_origClassInfo = classInfo;
			m_classInfo = new ClassListViewItem(classInfo);
			Setup();
		}

		/// ------------------------------------------------------------------------------------
		private void SetClassTypeTexts()
		{
			string classTypeText = string.Empty;
	
			switch (m_classInfo.ClassType)
			{
				case SearchClassType.Phones:
					classTypeText = App.GetString(
						"DefineClassDlg.PhoneClassDialogCaptionPrefix", "Phones", 
						"Part of the title for the dialog box when defining a phone class.");

					lblClassTypeValue.Text = App.GetString("DefineClassDlg.PhonesClassTypeLabel",
						"Phones", "Phone class type label.");

					break;

				case SearchClassType.Articulatory:
					classTypeText = App.GetString(
						"DefineClassDlg.ArticulatoryFeatureClassDialogCaptionPrefix", "Articulatory Features",
						"Part of the title for the dialog box when defining a articulatory features class.");

					lblClassTypeValue.Text = App.GetString(
						"DefineClassDlg.ArticulatoryFeaturesClassTypeLabel", "Articulatory features",
						"Articulatory features class type label.");

					break;

				case SearchClassType.Binary:
					classTypeText = App.GetString(
						"DefineClassDlg.BinaryFeatureClassDialogCaptionPrefix", "Binary Features",
						"Part of the title for the dialog box when defining a binary features class.");

					lblClassTypeValue.Text = App.GetString("DefineClassDlg.BinaryFeaturesClassTypeLabel",
						"Binary features", "Binary features class type label.");

					break;

				case SearchClassType.OtherClass:
					break;

				default:
					lblClassTypeValue.Text = string.Empty;
					break;
			}

			Text = string.Format(Text, classTypeText);
		}

		/// ------------------------------------------------------------------------------------
		private void Setup()
		{
			SetClassTypeTexts();

			if (m_classInfo.ClassType == SearchClassType.Articulatory)
				m_lvArticulatoryFeatures.CurrentMask = (m_classInfo.Mask ?? App.AFeatureCache.GetEmptyMask());
			else if (m_classInfo.ClassType == SearchClassType.Binary)
				m_lvBinaryFeatures.CurrentMask = (m_classInfo.Mask ?? App.BFeatureCache.GetEmptyMask());

			txtClassName.Text = m_classInfo.Text;
			SetupControlsForType();
			UpdateCharacterViewers();
			m_classInfo.IsDirty = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Change the resultView for the "Based on" class type that was chosen.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupControlsForType()
		{
			rbMatchAll.Checked = m_classInfo.ANDFeatures;
			rbMatchAny.Checked = !rbMatchAll.Checked;
			
			splitOuter.SuspendLayout();

			foreach (var ctrl in m_ctrls.Values)
				ctrl.Visible = false;

			m_ctrls[m_classInfo.ClassType].Visible = true;
			m_ctrls[m_classInfo.ClassType].BringToFront();

			m_lvArticulatoryFeatures.Visible = (m_classInfo.ClassType == SearchClassType.Articulatory);
			m_lvBinaryFeatures.Visible = (m_classInfo.ClassType == SearchClassType.Binary);

			// The scope button is irrelevant for IPA character classes.
			// So hide it when that's the case.
			rbMatchAll.Visible = rbMatchAny.Visible = (m_classInfo.ClassType != SearchClassType.Phones);

			UpdateCharacterViewers();

			// Adjust properties of the members text box accordingly.
			txtMembers.Font = (m_classInfo.ClassType == SearchClassType.Phones ?
				FontHelper.MakeRegularFontDerivative(App.PhoneticFont, 16) : FontHelper.UIFont);

			txtMembers.Text = m_classInfo.FormattedMembersString;
			txtMembers.ReadOnly = (m_classInfo.ClassType != SearchClassType.Phones);
			txtMembers.SelectionStart = txtMembers.Text.Length + 1;

			splitOuter.ResumeLayout();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Iinitializes the IPA character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void IinitializeCharExplorer()
		{
			var typesToShow = new List<IPASymbolTypeInfo>();
			typesToShow.Add(new IPASymbolTypeInfo(IPASymbolType.Consonant, IPASymbolSubType.Pulmonic));
			typesToShow.Add(new IPASymbolTypeInfo(IPASymbolType.Consonant, IPASymbolSubType.NonPulmonic));
			typesToShow.Add(new IPASymbolTypeInfo(IPASymbolType.Consonant, IPASymbolSubType.OtherSymbols));
			typesToShow.Add(new IPASymbolTypeInfo(IPASymbolType.Vowel));
			typesToShow.Add(new IPASymbolTypeInfo(IPASymbolType.Diacritics));
			typesToShow.Add(new IPASymbolTypeInfo(IPASymbolType.Suprasegmentals, IPASymbolSubType.StressAndLength));
			typesToShow.Add(new IPASymbolTypeInfo(IPASymbolType.Suprasegmentals, IPASymbolSubType.ToneAndAccents));
			charExplorer.TypesToShow = typesToShow;
			charExplorer.Dock = DockStyle.Fill;
			charExplorer.Load();
		}

		/// ------------------------------------------------------------------------------------
		private FeatureListView InitializeFeatureList(App.FeatureType featureType)
		{
			var flv = new FeatureListView(featureType);
			flv.Load();
			flv.Dock = DockStyle.Fill;
			flv.Visible = true;
			flv.LabelEdit = false;
			flv.FeatureChanged += HandleFeatureChanged;
			flv.TabIndex = txtClassName.TabIndex + 1;
			splitOuter.Panel2.Controls.Add(flv);
			return flv;
		}

		/// ------------------------------------------------------------------------------------
		private void SetupPhoneViewers()
		{
			m_conViewer = new PhonesInFeatureViewer(m_classesDlg.Project, IPASymbolType.Consonant,
				Settings.Default.DefineClassDlgShowAllConsonantsInViewer,
				Settings.Default.DefineClassDlgUseCompactConsonantView,
				showAll => Settings.Default.DefineClassDlgShowAllConsonantsInViewer = showAll,
				compactVw => Settings.Default.DefineClassDlgUseCompactConsonantView = compactVw);
			
			m_conViewer.HeaderText = App.GetString("DefineClassDlg.ConsonantViewerHeaderText", "&Consonants");
			m_conViewer.Dock = DockStyle.Fill;
			splitCV.Panel1.Controls.Add(m_conViewer);
			m_conViewer.BringToFront();

			m_vowViewer = new PhonesInFeatureViewer(m_classesDlg.Project, IPASymbolType.Vowel,
				Settings.Default.DefineClassDlgShowAllVowelsInViewer,
				Settings.Default.DefineClassDlgUseCompactVowelView,
				showAll => Settings.Default.DefineClassDlgShowAllVowelsInViewer = showAll,
				compactVw => Settings.Default.DefineClassDlgUseCompactVowelView = compactVw);

			m_vowViewer.HeaderText = App.GetString("DefineClassDlg.VowelViewerHeaderText", "&Vowels");
			m_vowViewer.Dock = DockStyle.Fill;
			splitCV.Panel2.Controls.Add(m_vowViewer);
			m_vowViewer.BringToFront();

			m_otherPhonesViewer = new PhonesInFeatureViewer(m_classesDlg.Project, IPASymbolType.Unknown,
				Settings.Default.DefineClassDlgShowAllOthersInViewer,
				Settings.Default.DefineClassDlgUseCompactOtherView,
				showAll => Settings.Default.DefineClassDlgShowAllOthersInViewer = showAll,
				compactVw => Settings.Default.DefineClassDlgUseCompactOtherView = compactVw);

			m_otherPhonesViewer.HeaderText = App.GetString("DefineClassDlg.OtherPhonesViewerHeaderText", "&Other Phones");
			m_otherPhonesViewer.CanViewExpandAndCompact = false;
			m_otherPhonesViewer.Dock = DockStyle.Fill;
			splitPhoneViewers.Panel2.Controls.Add(m_otherPhonesViewer);
			m_otherPhonesViewer.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the splitter locations from the settings file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadSplitterSettings()
		{
			try
			{
				// These are in a try/catch because sometimes they might throw an exception
				// in rare cases. The exception has to do with a condition in the underlying
				// .Net framework that I haven't been able to make sense of. Anyway, if an
				// exception is thrown, no big deal, the splitter distances will just be set
				// to their default values.
				if (Settings.Default.DefineClassDlgSplit3Loc > 0)
					splitOuter.SplitterDistance = Settings.Default.DefineClassDlgSplit3Loc;

				if (Settings.Default.DefineClassDlgSplit2Loc > 0)
					splitPhoneViewers.SplitterDistance = Settings.Default.DefineClassDlgSplit2Loc;

				if (Settings.Default.DefineClassDlgSplit1Loc > 0)
					splitCV.SplitterDistance = Settings.Default.DefineClassDlgSplit1Loc;
			}
			catch { }

			m_splitterSettingsLoaded = true;
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the information for the class being added or modified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ClassListViewItem ClassInfo
		{
			get {return m_classInfo;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets TxtClassName.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TextBox TxtClassName
		{
			get { return txtClassName; }
		}

		#endregion

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			Debug.Assert(m_classesDlg != null);
			Debug.Assert(m_classInfo != null);
			Debug.Assert(charExplorer != null);

			if (!m_splitterSettingsLoaded &&
				(m_classInfo.ClassType == SearchClassType.Articulatory ||
				m_classInfo.ClassType == SearchClassType.Binary))
			{
				LoadSplitterSettings();
			}

			charExplorer.LoadSettings(Name);
			UpdateCharacterViewers();
			Utils.WaitCursors(false);

			base.OnShown(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get	
			{
				if (m_origClassInfo == null)
					return true;

				return (CurrentPattern != m_origClassInfo.Pattern ||
					m_classInfo.Text != m_origClassInfo.Text);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			charExplorer.SaveSettings(Name);
			lvClasses.SaveSettings(Name);

			Settings.Default.DefineClassDlgSplit1Loc = splitCV.SplitterDistance;
			Settings.Default.DefineClassDlgSplit2Loc = splitPhoneViewers.SplitterDistance;
			Settings.Default.DefineClassDlgSplit3Loc = splitOuter.SplitterDistance;
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			if (m_classInfo.ClassType == SearchClassType.Phones)
			{
				// Check if any of the characters entered are invalid.
				var undefinedChars = new List<char>();
				foreach (char c in txtMembers.Text.Trim().Replace(",", string.Empty))
				{
					if (App.IPASymbolCache[c] == null || App.IPASymbolCache[c].IsUndefined)
						undefinedChars.Add(c);
				}

				if (undefinedChars.Count > 0)
				{
					using (UndefinedCharactersInClassDlg dlg =
						new UndefinedCharactersInClassDlg(undefinedChars.ToArray()))
					{
						dlg.ShowDialog(this);
					}
				}
			}

			m_classInfo.Pattern = CurrentPattern;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return true if data is OK.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			txtClassName.Text = txtClassName.Text.Trim();

			// Ensure the new class doesn't have an empty class name
			if (txtClassName.Text == string.Empty)
			{
				Utils.MsgBox(App.GetString("DefineClassDlg.EmptyClassNameMsg", "Class name must not be empty."));
				return false;
			}

			if (m_classesDlg == null)
				return true;

			bool exists = m_classesDlg.ClassListView.DoesClassNameExist(
				txtClassName.Text, m_origClassInfo, true);
			
			if (exists)
			{
				txtClassName.Focus();
				txtClassName.SelectAll();
			}

			return !exists;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the pattern that would be built from the contents of the members text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string CurrentPattern
		{
			get
			{
				if (m_classInfo.ClassType != SearchClassType.Phones)
					return txtMembers.Text.Trim();

				string phones = txtMembers.Text.Trim().Replace(",", string.Empty);
				phones = m_classesDlg.Project.PhoneticParser.PhoneticParser_CommaDelimited(phones, true, true);
				return "{" + (phones ?? string.Empty) + "}";
			}
		}

		#endregion

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			switch (m_classInfo.ClassType)
			{
				case SearchClassType.Phones: App.ShowHelpTopic("hidPhoneticCharacterClassDlg"); break;
				case SearchClassType.Articulatory: App.ShowHelpTopic("hidArticulatoryFeatureClassDlg"); break;
				case SearchClassType.Binary: App.ShowHelpTopic("hidBinaryFeatureClassDlg"); break;
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the class name changing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtClassName_TextChanged(object sender, EventArgs e)
		{
			m_classInfo.Text = txtClassName.Text.Trim();
			m_classInfo.IsDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user clicking on an IPA character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleIPACharPicked(CharPicker chooser, ToolStripButton item)
		{
			InsertText(item.Text.Replace(App.kDottedCircle, string.Empty));
		}

		/// ------------------------------------------------------------------------------------
		private void InsertText(string itemText)
		{
			var charInfo = App.IPASymbolCache[itemText];
			bool isBase = (charInfo == null || charInfo.IsBase);

			int selStart = txtMembers.SelectionStart;
			int selLen = txtMembers.SelectionLength;

			// First, if there is a selection, get rid of the selected text.
			if (selLen > 0)
				txtMembers.Text = txtMembers.Text.Remove(selStart, selLen);

			// Check if what's being inserted needs to be preceded by a comma.
			if (selStart > 0 && txtMembers.Text[selStart - 1] != ',' && isBase)
			{
				txtMembers.Text = txtMembers.Text.Insert(selStart, ",");
				selStart++;
			}

			txtMembers.Text = txtMembers.Text.Insert(selStart, itemText);
			txtMembers.SelectionStart = selStart + itemText.Length;

			m_classInfo.IsDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user choosing a feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleFeatureChanged(object sender, FeatureMask newMask)
		{
			m_classInfo.Mask = newMask;
			txtMembers.Text = m_classInfo.FormattedMembersString;
			m_classInfo.IsDirty = true;
			UpdateCharacterViewers();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user choosing a class (by double-clicking on a class in the class list)
		/// as a member of the class being defined.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleClassesListViewDoubleClick(object sender, EventArgs e)
		{
			if (lvClasses.SelectedItems.Count == 0)
				return;

			//ClassListViewItem item = lvClasses.SelectedItems[0] as ClassListViewItem;
			//m_classInfo.OtherClassIds += (string.IsNullOrEmpty(m_classInfo.OtherClassIds) ?
			//    "" : ",") + item.Id.ToString();

			//txtMembers.Text = m_classInfo.FormattedMembersString;
			//m_classInfo.IsDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user choosing a class (by pressing enter on a class in the class list)
		/// as a member of the class being defined.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleClassesListViewKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
				HandleClassesListViewDoubleClick(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user choosing one of the items in the tsbWhatToInclude drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleScopeClick(object sender, EventArgs e)
		{
			m_classInfo.ANDFeatures = rbMatchAll.Checked;
			txtMembers.Text = m_classInfo.FormattedMembersString;
			m_classInfo.IsDirty = true;
			UpdateCharacterViewers();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the appropriate character viewer is up to date.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateCharacterViewers()
		{
			if (m_classInfo.ClassType == SearchClassType.Binary)
			{
				m_conViewer.SetBMask(m_classInfo.Mask, m_classInfo.ANDFeatures);
				m_vowViewer.SetBMask(m_classInfo.Mask, m_classInfo.ANDFeatures);
				m_otherPhonesViewer.SetBMask(m_classInfo.Mask, m_classInfo.ANDFeatures);
			}
			else if (m_classInfo.ClassType == SearchClassType.Articulatory)
			{
				m_conViewer.SetAMasks(m_classInfo.Mask, m_classInfo.ANDFeatures);
				m_vowViewer.SetAMasks(m_classInfo.Mask, m_classInfo.ANDFeatures);
				m_otherPhonesViewer.SetAMasks(m_classInfo.Mask, m_classInfo.ANDFeatures);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For classes other than for IPA characters, delete all the text when the user
		/// presses the backspace or delete keys.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtMembers_KeyDown(object sender, KeyEventArgs e)
		{
			if (m_classInfo.ClassType == SearchClassType.Phones)
				return;

			if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
			{
				txtMembers.Text = string.Empty;
				//if (m_classInfo.ClassType == ClassType.OtherClasses)
				//    m_classInfo.OtherClassIds = null;
				//else
					m_classInfo.Mask.Clear();
					
				// Fix for PA-555
				m_lvArticulatoryFeatures.CurrentMask.Clear();
				m_lvBinaryFeatures.CurrentMask.Clear();
			}
		}

		#endregion
	}
}