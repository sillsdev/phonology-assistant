using System;
using Localization;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class DefineDescriptiveFeatureClassDlg : DefineFeatureClassDlgBase
	{
		/// ------------------------------------------------------------------------------------
		public DefineDescriptiveFeatureClassDlg(ClassListViewItem classInfo, ClassesDlg classDlg)
			: base(classInfo ?? new ClassListViewItem { ClassType = SearchClassType.Articulatory },
					classDlg, new DescriptiveFeatureListView(), App.AFeatureCache.GetEmptyMask())
		{
			InitializeComponent();
			_lvFeatures.EmphasizeCheckedItems = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void SetLocalizedTexts()
		{
			_labelClassTypeValue.Text = LocalizationManager.GetString(
				"DialogBoxes.DefineClassesDialogs.DescriptiveFeatureClassDlg.ClassTypeLabel",
				"Descriptive features");
		}

		/// ------------------------------------------------------------------------------------
		protected override bool UseCompactConsonantView
		{
			get { return Settings.Default.DefineDescriptiveFeatureClassDlgUseCompactConsonantView; }
			set { Settings.Default.DefineDescriptiveFeatureClassDlgUseCompactConsonantView = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool UseCompactVowelView
		{
			get { return Settings.Default.DefineDescriptiveFeatureClassDlgUseCompactVowelView; }
			set { Settings.Default.DefineDescriptiveFeatureClassDlgUseCompactVowelView = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.DefineDescriptiveFeatureClassDlgSplit1Loc = _splitterCV.SplitterDistance;
			Settings.Default.DefineDescriptiveFeatureClassDlgSplit2Loc = _splitterOuter.SplitterDistance;
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override void LoadSplitterSettings()
		{
			try
			{
				// These are in a try/catch because sometimes they might throw an exception
				// in rare cases. The exception has to do with a condition in the underlying
				// .Net framework that I haven't been able to make sense of. Anyway, if an
				// exception is thrown, no big deal, the splitter distances will just be set
				// to their default values.
				if (Settings.Default.DefineDescriptiveFeatureClassDlgSplit2Loc > 0)
					_splitterOuter.SplitterDistance = Settings.Default.DefineDescriptiveFeatureClassDlgSplit2Loc;

				if (Settings.Default.DefineDescriptiveFeatureClassDlgSplit1Loc > 0)
					_splitterCV.SplitterDistance = Settings.Default.DefineDescriptiveFeatureClassDlgSplit1Loc;
			}
			catch { }

			base.LoadSplitterSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			App.ShowHelpTopic("hidDescriptiveFeatureClassDlg");
		}

		/// ------------------------------------------------------------------------------------
		protected override void UpdateCharacterViewers()
		{
			_conViewer.SetAMasks(m_classInfo.Mask, m_classInfo.ANDFeatures);
			_vowViewer.SetAMasks(m_classInfo.Mask, m_classInfo.ANDFeatures);
		}
	}
}