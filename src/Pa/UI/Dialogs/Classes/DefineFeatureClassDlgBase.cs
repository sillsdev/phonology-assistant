using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class DefineFeatureClassDlgBase : DefineClassBaseDlg
	{
		protected readonly FeatureListViewBase _lvFeatures;
		protected PhonesInFeatureViewer _conViewer;
		protected PhonesInFeatureViewer _vowViewer;
		protected SplitContainer _splitterOuter;
		protected SplitContainer _splitterCV;
		
		private RadioButton _radioMatchAll;
		private RadioButton _radioMatchAny;
		private bool _splitterSettingsLoaded;

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
			_lvFeatures.TabIndex = _textBoxClassName.TabIndex + 1;
			_lvFeatures.CurrentMask = (m_classInfo.Mask ?? emptyMask);
			
			SetupPhoneViewers();
			SetupSplitters();
			SetupRadioButtons();
			UpdateCharacterViewers();
		}

		/// ------------------------------------------------------------------------------------
		private void SetupRadioButtons()
		{
			// TODO: Internationalize these radio buttons.
			
			_radioMatchAll = new RadioButton();
			_radioMatchAll.AutoSize = true;
			_radioMatchAll.Font = FontHelper.UIFont;
			_radioMatchAll.BackColor = Color.Transparent;
			_radioMatchAll.Margin = new Padding(3, 6, 8, 3);
			_radioMatchAll.TabIndex = _textBoxMembers.TabIndex + 1;
			_radioMatchAll.TabStop = true;
			_radioMatchAll.Text = "Match A&ll Selected Features";
			_radioMatchAll.CheckedChanged += HandleScopeClick;

			_radioMatchAny = new RadioButton();
			_radioMatchAny.AutoSize = true;
			_radioMatchAny.Font = FontHelper.UIFont;
			_radioMatchAny.BackColor = Color.Transparent;
			_radioMatchAny.Margin = new Padding(7, 6, 3, 3);
			_radioMatchAny.TabIndex = _radioMatchAll.TabIndex + 1;
			_radioMatchAny.TabStop = true;
			_radioMatchAny.Text = "Match A&ny Selected Features";
			_radioMatchAny.CheckedChanged += HandleScopeClick;

			_tableLayout.Controls.Add(_radioMatchAll, 1, 3);
			_tableLayout.Controls.Add(_radioMatchAny, 2, 3);

			_radioMatchAll.Checked = m_classInfo.ANDFeatures;
			_radioMatchAny.Checked = !_radioMatchAll.Checked;
		}

		/// ------------------------------------------------------------------------------------
		private void SetupSplitters()
		{
			_splitterCV = GetSplitter(59, 0);
			_splitterCV.Panel1.Controls.Add(_conViewer);
			_splitterCV.Panel2.Controls.Add(_vowViewer);

			_splitterOuter = GetSplitter(89, 1);
			_splitterOuter.BackColor = SystemColors.Control;
			_splitterOuter.Orientation = Orientation.Horizontal;
			_splitterOuter.Panel2.Controls.Add(_lvFeatures);
			_splitterOuter.Panel1.Controls.Add(_splitterCV);
			
			_panelMemberPickingContainer.Controls.Add(_splitterOuter);
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
			_conViewer = new PhonesInFeatureViewer(m_classesDlg.Project, IPASymbolType.consonant,
				ShowAllConsonantsInViewer, UseCompactConsonantView,
				showAll => ShowAllConsonantsInViewer = showAll,
				compactVw => UseCompactConsonantView = compactVw);
			
			_conViewer.HeaderText = App.GetString("DefineClassDlg.ConsonantViewerHeaderText", "&Consonants");
			_conViewer.Dock = DockStyle.Fill;

			_vowViewer = new PhonesInFeatureViewer(m_classesDlg.Project, IPASymbolType.vowel,
				ShowAllVowelsInViewer, UseCompactVowelView,
				showAll => ShowAllVowelsInViewer = showAll,
				compactVw => UseCompactVowelView = compactVw);

			_vowViewer.HeaderText = App.GetString("DefineClassDlg.VowelViewerHeaderText", "&Vowels");
			_vowViewer.Dock = DockStyle.Fill;
		}

		protected virtual bool ShowAllConsonantsInViewer { get; set; }
		protected virtual bool ShowAllVowelsInViewer { get; set; }
		protected virtual bool UseCompactConsonantView { get; set; }
		protected virtual bool UseCompactVowelView { get; set; }

		/// ------------------------------------------------------------------------------------
		protected virtual void LoadSplitterSettings()
		{
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
		/// <summary>
		/// Gets the pattern that would be built from the contents of the members text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string CurrentPattern
		{
			get { return _textBoxMembers.Text.Trim(); }
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
			_textBoxMembers.Text = m_classInfo.FormattedMembersString;
			m_classInfo.IsDirty = true;
			UpdateCharacterViewers();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user choosing one of the items in the tsbWhatToInclude drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleScopeClick(object sender, EventArgs e)
		{
			m_classInfo.ANDFeatures = _radioMatchAll.Checked;
			_textBoxMembers.Text = m_classInfo.FormattedMembersString;
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
				_textBoxMembers.Text = string.Empty;
				m_classInfo.Mask.Clear();
					
				// Fix for PA-555
				_lvFeatures.CurrentMask.Clear();
			}
		}

		#endregion
	}
}