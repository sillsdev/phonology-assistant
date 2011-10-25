using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class DefineFeatureClassDlgBase : DefineClassBaseDlg
	{
		protected readonly FeatureListViewBase _lvFeatures;
		protected PhonesInFeatureViewer _conViewer;
		protected PhonesInFeatureViewer _vowViewer;
		protected PhonesInFeatureViewer _otherPhonesViewer;
		
		private bool _splitterSettingsLoaded;
		private SplitContainer _splitterOuter;
		private SplitContainer _splitterCV;
		private SplitContainer _splitterPhoneViewers;

		#region Construction and setup
		/// ------------------------------------------------------------------------------------
		private DefineFeatureClassDlgBase()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		public DefineFeatureClassDlgBase(ClassListViewItem classInfo, ClassesDlg classDlg,
			FeatureListViewBase lvFeatures, FeatureMask emptyMask)
			: base(classInfo ?? new ClassListViewItem { ClassType = SearchClassType.Articulatory }, classDlg)
		{
			_lvFeatures = lvFeatures;
			_lvFeatures.Load();
			_lvFeatures.Dock = DockStyle.Fill;
			_lvFeatures.Visible = true;
			_lvFeatures.LabelEdit = false;
			_lvFeatures.FeatureChanged += HandleFeatureChanged;
			_lvFeatures.TabIndex = txtClassName.TabIndex + 1;
			_lvFeatures.CurrentMask = (m_classInfo.Mask ?? emptyMask);
			
			SetupPhoneViewers();
			SetupSplitters();
			UpdateCharacterViewers();

			rbMatchAll.Visible = true;
			rbMatchAny.Visible = true;

			rbMatchAll.Checked = m_classInfo.ANDFeatures;
			rbMatchAny.Checked = !rbMatchAll.Checked;
		}

		/// ------------------------------------------------------------------------------------
		private void SetupSplitters()
		{
			_splitterCV = GetSplitter(59, 0);
			_splitterCV.Panel1.Controls.Add(_conViewer);
			_splitterCV.Panel2.Controls.Add(_vowViewer);

			_splitterPhoneViewers = GetSplitter(121, 0);
			_splitterPhoneViewers.Panel1.Controls.Add(_splitterCV);
			_splitterPhoneViewers.Panel2.Controls.Add(_otherPhonesViewer);
			
			_splitterOuter = GetSplitter(89, 1);
			_splitterOuter.BackColor = SystemColors.Control;
			_splitterOuter.Orientation = Orientation.Horizontal;
			_splitterOuter.Panel2.Controls.Add(_lvFeatures);
			_splitterOuter.Panel1.Controls.Add(_splitterPhoneViewers);
			
			pnlMemberPickingContainer.Controls.Add(_splitterOuter);
		}

		/// ------------------------------------------------------------------------------------
		private SplitContainer GetSplitter(int splitDistance, int tabIndex)
		{
			var splitter = new SplitContainer();
			splitter.SplitterDistance = splitDistance;
			splitter.TabIndex = tabIndex;
			splitter.SplitterWidth = 6;
			splitter.Panel1.BackColor = SystemColors.Window;
			splitter.Panel2.BackColor = SystemColors.Window;
			splitter.Dock = DockStyle.Fill;
			return splitter;
		}

		/// ------------------------------------------------------------------------------------
		private void SetupPhoneViewers()
		{
			_conViewer = new PhonesInFeatureViewer(m_classesDlg.Project, IPASymbolType.Consonant,
				Settings.Default.DefineClassDlgShowAllConsonantsInViewer,
				Settings.Default.DefineClassDlgUseCompactConsonantView,
				showAll => Settings.Default.DefineClassDlgShowAllConsonantsInViewer = showAll,
				compactVw => Settings.Default.DefineClassDlgUseCompactConsonantView = compactVw);
			
			_conViewer.HeaderText = App.GetString("DefineClassDlg.ConsonantViewerHeaderText", "&Consonants");
			_conViewer.Dock = DockStyle.Fill;
			//_conViewer.BringToFront();

			_vowViewer = new PhonesInFeatureViewer(m_classesDlg.Project, IPASymbolType.Vowel,
				Settings.Default.DefineClassDlgShowAllVowelsInViewer,
				Settings.Default.DefineClassDlgUseCompactVowelView,
				showAll => Settings.Default.DefineClassDlgShowAllVowelsInViewer = showAll,
				compactVw => Settings.Default.DefineClassDlgUseCompactVowelView = compactVw);

			_vowViewer.HeaderText = App.GetString("DefineClassDlg.VowelViewerHeaderText", "&Vowels");
			_vowViewer.Dock = DockStyle.Fill;
			//_vowViewer.BringToFront();

			_otherPhonesViewer = new PhonesInFeatureViewer(m_classesDlg.Project, IPASymbolType.Unknown,
				Settings.Default.DefineClassDlgShowAllOthersInViewer,
				Settings.Default.DefineClassDlgUseCompactOtherView,
				showAll => Settings.Default.DefineClassDlgShowAllOthersInViewer = showAll,
				compactVw => Settings.Default.DefineClassDlgUseCompactOtherView = compactVw);

			_otherPhonesViewer.HeaderText = App.GetString("DefineClassDlg.OtherPhonesViewerHeaderText", "&Other Phones");
			_otherPhonesViewer.CanViewExpandAndCompact = false;
			_otherPhonesViewer.Dock = DockStyle.Fill;
			//_otherPhonesViewer.BringToFront();
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
					_splitterOuter.SplitterDistance = Settings.Default.DefineClassDlgSplit3Loc;

				if (Settings.Default.DefineClassDlgSplit2Loc > 0)
					_splitterPhoneViewers.SplitterDistance = Settings.Default.DefineClassDlgSplit2Loc;

				if (Settings.Default.DefineClassDlgSplit1Loc > 0)
					_splitterCV.SplitterDistance = Settings.Default.DefineClassDlgSplit1Loc;
			}
			catch { }

			_splitterSettingsLoaded = true;
		}

		#endregion

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			if (!_splitterSettingsLoaded)
				LoadSplitterSettings();

			UpdateCharacterViewers();

			base.OnShown(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.DefineClassDlgSplit1Loc = _splitterCV.SplitterDistance;
			Settings.Default.DefineClassDlgSplit2Loc = _splitterPhoneViewers.SplitterDistance;
			Settings.Default.DefineClassDlgSplit3Loc = _splitterOuter.SplitterDistance;
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the pattern that would be built from the contents of the members text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string CurrentPattern
		{
			get { return txtMembers.Text.Trim(); }
		}

		#endregion

		#region Event handlers
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
		/// Handle the user choosing one of the items in the tsbWhatToInclude drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleScopeClick(object sender, EventArgs e)
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
		protected virtual void UpdateCharacterViewers()
		{
			if (!DesignMode)
				throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For classes other than for IPA characters, delete all the text when the user
		/// presses the backspace or delete keys.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleMembersTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
			{
				txtMembers.Text = string.Empty;
				m_classInfo.Mask.Clear();
					
				// Fix for PA-555
				_lvFeatures.CurrentMask.Clear();
			}
		}

		#endregion
	}
}