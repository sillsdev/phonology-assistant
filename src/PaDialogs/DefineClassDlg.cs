using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using SIL.Pa.Controls;
using SIL.Pa.Data;
using SIL.Pa.Resources;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class DefineClassDlg : OKCancelDlgBase
	{
		private bool m_splitterSettingsLoaded = false;
		private PhonesInFeatureViewer m_conViewer;
		private PhonesInFeatureViewer m_vowViewer;
		private PhonesInFeatureViewer m_otherPhonesViewer;
		private readonly ClassesDlg m_classesDlg;
		private readonly ClassListViewItem m_classInfo;
		private readonly ClassListViewItem m_origClassInfo = null;
		private readonly FeatureListView m_lvArticulatoryFeatures;
		private readonly FeatureListView m_lvBinaryFeatures;
		private readonly Dictionary<SearchClassType, Control> m_ctrls =
			new Dictionary<SearchClassType, Control>();

		#region Construction and setup
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private DefineClassDlg()
		{
			Application.UseWaitCursor = true;
			Application.DoEvents();

			InitializeComponent();

			lblClassName.Font = FontHelper.UIFont;
			txtClassName.Font = FontHelper.UIFont;
			lblMembers.Font = FontHelper.UIFont;
			rdoAnd.Font = FontHelper.UIFont;
			rdoOr.Font = FontHelper.UIFont;
			lblClassType.Font = FontHelper.UIFont;
			lblClassTypeValue.Font = FontHelper.UIFont;

			lblMembers.Top = (pnlMembers.Height - txtMembers.Height) / 2;
			txtMembers.Left = lblMembers.Right + 6;

			//lvClasses.Dock = DockStyle.Fill;
			//lvClasses.LoadSettings(Name);

			IinitializeCharExplorer();

			m_lvArticulatoryFeatures = InitializeFeatureList(PaApp.FeatureType.Articulatory);
			m_lvBinaryFeatures = InitializeFeatureList(PaApp.FeatureType.Binary);

			m_ctrls[SearchClassType.Phones] = charExplorer;
			m_ctrls[SearchClassType.Articulatory] = splitOuter;
			m_ctrls[SearchClassType.Binary] = splitOuter;
			//m_ctrls[SearchClassType.OtherClass] = lvClasses;

			splitOuter.Dock = DockStyle.Fill;

			SetupPhoneViewers();

			rdoOr.Left = rdoAnd.Left;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private DefineClassDlg(ClassesDlg classDlg) : this()
		{
			m_classesDlg = classDlg;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DefineClassDlg(SearchClassType type, ClassesDlg classDlg) : this(classDlg)
		{
			m_classInfo = new ClassListViewItem();
			m_classInfo.ClassType = type;
			Setup();
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DefineClassDlg(ClassListViewItem classInfo, ClassesDlg classDlg) : this(classDlg)
		{
			Debug.Assert(classInfo != null);
			m_origClassInfo = classInfo;
			m_classInfo = new ClassListViewItem(classInfo);
			Setup();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Setup()
		{
			switch (m_classInfo.ClassType)
			{
				case SearchClassType.Phones:
					Text = string.Format(Text, Properties.Resources.kstidPhoneClassDlgHdg);
					lblClassTypeValue.Text = ResourceHelper.GetString("kstidClassTypePhones");
					break;
				case SearchClassType.Articulatory:
					Text = string.Format(Text, Properties.Resources.kstidArticulatoryFeatureClassDlgHdg);
					lblClassTypeValue.Text = ResourceHelper.GetString("kstidClassTypeArticulatoryFeatures");
					break;
				case SearchClassType.Binary:
					Text = string.Format(Text, Properties.Resources.kstidBinaryFeatureClassDlgHdg);
					lblClassTypeValue.Text = ResourceHelper.GetString("kstidClassTypeBinaryFeatures");
					break;
				case SearchClassType.OtherClass:
					break;
				default:
					Text = string.Format(Text, string.Empty);
					lblClassTypeValue.Text = string.Empty;
					break;
			}

			txtClassName.Text = m_classInfo.Text;
			m_lvArticulatoryFeatures.CurrentMasks = m_classInfo.Masks;
			m_lvBinaryFeatures.CurrentMasks = m_classInfo.Masks;

			SetupControlsForType();
			UpdateCharacterViewers();

			m_classInfo.IsDirty = false;
			PaApp.SettingsHandler.LoadFormProperties(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Change the resultView for the "Based on" class type that was chosen.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupControlsForType()
		{
			rdoAnd.Checked = m_classInfo.ANDFeatures;
			rdoOr.Checked = !rdoAnd.Checked;
			
			splitOuter.SuspendLayout();

			foreach (Control ctrl in m_ctrls.Values)
				ctrl.Visible = false;

			m_ctrls[m_classInfo.ClassType].Visible = true;
			m_ctrls[m_classInfo.ClassType].BringToFront();

			m_lvArticulatoryFeatures.Visible = (m_classInfo.ClassType == SearchClassType.Articulatory);
			m_lvBinaryFeatures.Visible = (m_classInfo.ClassType == SearchClassType.Binary);

			// The scope button is irrelevant for IPA character classes.
			// So hide it when that's the case.
			rdoAnd.Visible = rdoOr.Visible = (m_classInfo.ClassType != SearchClassType.Phones);

			UpdateCharacterViewers();

			// Adjust properties of the members text box accordingly.
			txtMembers.Font = (m_classInfo.ClassType == SearchClassType.Phones ?
				FontHelper.MakeEticRegFontDerivative(16) : FontHelper.UIFont);

			txtMembers.Top = (pnlMembers.Height - txtMembers.Height) / 2 - 1;
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
			List<IPACharacterTypeInfo> typesToShow = new List<IPACharacterTypeInfo>();
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Consonant,
				IPACharacterSubType.Pulmonic));
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Consonant,
				IPACharacterSubType.NonPulmonic));
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Consonant,
				IPACharacterSubType.OtherSymbols));
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Vowel));
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Diacritics));
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Suprasegmentals,
				IPACharacterSubType.StressAndLength));
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Suprasegmentals,
				IPACharacterSubType.ToneAndAccents));

			charExplorer.TypesToShow = typesToShow;
			charExplorer.Dock = DockStyle.Fill;
			charExplorer.Load();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private FeatureListView InitializeFeatureList(PaApp.FeatureType featureType)
		{
			FeatureListView flv = new FeatureListView(featureType);
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupPhoneViewers()
		{
			m_conViewer = new PhonesInFeatureViewer(IPACharacterType.Consonant, Name, "ConVwr");
			m_conViewer.HeaderText = Properties.Resources.kstidDefClassPhoneHdrCon;
			m_conViewer.Dock = DockStyle.Fill;
			splitCV.Panel1.Controls.Add(m_conViewer);
			m_conViewer.BringToFront();

			m_vowViewer = new PhonesInFeatureViewer(IPACharacterType.Vowel, Name, "VowVwr");
			m_vowViewer.HeaderText = Properties.Resources.kstidDefClassPhoneHdrVow;
			m_vowViewer.Dock = DockStyle.Fill;
			splitCV.Panel2.Controls.Add(m_vowViewer);
			m_vowViewer.BringToFront();

			m_otherPhonesViewer = new PhonesInFeatureViewer(IPACharacterType.Unknown, Name, "OtherVwr");
			m_otherPhonesViewer.HeaderText = Properties.Resources.kstidDefClassPhoneHdrOther;
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
				int splitDistance = PaApp.SettingsHandler.GetIntSettingsValue(Name, "split3", 0);
				if (splitDistance > 0)
					splitOuter.SplitterDistance = splitDistance;

				splitDistance = PaApp.SettingsHandler.GetIntSettingsValue(Name, "split2", 0);
				if (splitDistance > 0)
					splitPhoneViewers.SplitterDistance = splitDistance;

				splitDistance = PaApp.SettingsHandler.GetIntSettingsValue(Name, "split1", 0);
				if (splitDistance > 0)
					splitCV.SplitterDistance = splitDistance;
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
		/// <summary>
		/// 
		/// </summary>
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
			Application.UseWaitCursor = false;

			base.OnShown(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			base.SaveSettings();
			PaApp.SettingsHandler.SaveFormProperties(this);
			charExplorer.SaveSettings(Name);
			lvClasses.SaveSettings(Name);

			PaApp.SettingsHandler.SaveSettingsValue(Name, "split1", splitCV.SplitterDistance);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "split2", splitPhoneViewers.SplitterDistance);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "split3", splitOuter.SplitterDistance);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			if (m_classInfo.ClassType == SearchClassType.Phones)
			{
				// Check if any of the characters entered are invalid.
				List<char> undefinedChars = new List<char>();
				foreach (char c in txtMembers.Text.Trim().Replace(",", string.Empty))
				{
					if (DataUtils.IPACharCache[c] == null || DataUtils.IPACharCache[c].IsUndefined)
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
				STUtils.STMsgBox(Properties.Resources.kstidDefineClassEmptyClassName,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			return (m_classesDlg == null ? true :
				!m_classesDlg.ClassListView.DoesClassNameExist(txtClassName.Text, m_origClassInfo, true)); 
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
				phones = DataUtils.IPACharCache.PhoneticParser_CommaDelimited(phones, true, true);
				return "{" + (phones ?? string.Empty) + "}";
			}
		}

		#endregion

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			switch (m_classInfo.ClassType)
			{
				case SearchClassType.Phones:
					PaApp.ShowHelpTopic("hidPhoneticCharacterClassDlg");
					break;
				case SearchClassType.Articulatory:
					PaApp.ShowHelpTopic("hidArticulatoryFeatureClassDlg");
					break;
				case SearchClassType.Binary:
					PaApp.ShowHelpTopic("hidBinaryFeatureClassDlg");
					break;
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
			InsertText(item.Text.Replace(DataUtils.kDottedCircle, string.Empty));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InsertText(string itemText)
		{
			IPACharInfo charInfo = DataUtils.IPACharCache[itemText];
			bool isBase = (charInfo == null || charInfo.IsBaseChar);

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
		void HandleFeatureChanged(object sender, ulong[] newMasks)
		{
			m_classInfo.Masks = newMasks;
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
		private void lvClasses_DoubleClick(object sender, EventArgs e)
		{
			if (lvClasses.SelectedItems == null || lvClasses.SelectedItems.Count == 0)
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
		private void lvClasses_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
				lvClasses_DoubleClick(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user choosing one of the items in the tsbWhatToInclude drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleScopeClick(object sender, EventArgs e)
		{
			m_classInfo.ANDFeatures = rdoAnd.Checked;
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
				m_conViewer.SetBMask(m_classInfo.Masks[0], m_classInfo.ANDFeatures);
				m_vowViewer.SetBMask(m_classInfo.Masks[0], m_classInfo.ANDFeatures);
				m_otherPhonesViewer.SetBMask(m_classInfo.Masks[0], m_classInfo.ANDFeatures);
			}
			else if (m_classInfo.ClassType == SearchClassType.Articulatory)
			{
				m_conViewer.SetAMasks(m_classInfo.Masks, m_classInfo.ANDFeatures);
				m_vowViewer.SetAMasks(m_classInfo.Masks, m_classInfo.ANDFeatures);
				m_otherPhonesViewer.SetAMasks(m_classInfo.Masks, m_classInfo.ANDFeatures);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set a flag indicating whether or not the cancel button was pressed. That's because
		/// in the form's closing event, we don't know if a DialogResult of Cancel is due to
		/// the user clicking on the cancel button or closing the form in some other way
		/// beside clicking on the OK button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCancel_Click(object sender, EventArgs e)
		{
			m_cancelButtonPressed = true;
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
					m_classInfo.Masks = new ulong[] {0, 0};
					
				// Fix for PA-555
				m_lvArticulatoryFeatures.CurrentMasks = new ulong[] { 0, 0 };
				m_lvBinaryFeatures.CurrentMasks = new ulong[] { 0, 0 };
			}
		}

		#endregion
	}
}