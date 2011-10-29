using System;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class DefineDistinctiveFeatureClassDlgBase : DefineFeatureClassDlgBase
	{
		/// ------------------------------------------------------------------------------------
		public DefineDistinctiveFeatureClassDlgBase(ClassListViewItem classInfo, ClassesDlg classDlg)
			: base(classInfo ?? new ClassListViewItem { ClassType = SearchClassType.Binary },
					classDlg, new DistinctiveFeatureListView(), App.BFeatureCache.GetEmptyMask())
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		protected override void SetLocalizedTexts()
		{
			_labelClassTypeValue.Text = App.GetString(
				"DialogBoxes.DefineClassesDialogs.DistinctiveFeatureClassDlg.ClassTypeLabel",
				"Distinctive features");
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ShowAllConsonantsInViewer
		{
			get { return Settings.Default.DefineDistinctiveFeatureClassDlgShowAllConsonantsInViewer; }
			set { Settings.Default.DefineDistinctiveFeatureClassDlgShowAllConsonantsInViewer = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ShowAllVowelsInViewer
		{
			get { return Settings.Default.DefineDistinctiveFeatureClassDlgShowAllVowelsInViewer; }
			set { Settings.Default.DefineDistinctiveFeatureClassDlgShowAllVowelsInViewer = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool UseCompactConsonantView
		{
			get { return Settings.Default.DefineDistinctiveFeatureClassDlgUseCompactConsonantView; }
			set { Settings.Default.DefineDistinctiveFeatureClassDlgUseCompactConsonantView = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool UseCompactVowelView
		{
			get { return Settings.Default.DefineDistinctiveFeatureClassDlgUseCompactVowelView; }
			set { Settings.Default.DefineDistinctiveFeatureClassDlgUseCompactVowelView = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.DefineDistinctiveFeatureClassDlgSplit1Loc = _splitterCV.SplitterDistance;
			Settings.Default.DefineDistinctiveFeatureClassDlgSplit2Loc = _splitterOuter.SplitterDistance;
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
				if (Settings.Default.DefineDistinctiveFeatureClassDlgSplit2Loc > 0)
					_splitterOuter.SplitterDistance = Settings.Default.DefineDistinctiveFeatureClassDlgSplit2Loc;

				if (Settings.Default.DefineDistinctiveFeatureClassDlgSplit1Loc > 0)
					_splitterCV.SplitterDistance = Settings.Default.DefineDistinctiveFeatureClassDlgSplit1Loc;
			}
			catch { }

			base.LoadSplitterSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			App.ShowHelpTopic("hidBinaryFeatureClassDlg");
		}

		/// ------------------------------------------------------------------------------------
		protected override void UpdateCharacterViewers()
		{
			_conViewer.SetBMask(m_classInfo.Mask, m_classInfo.ANDFeatures);
			_vowViewer.SetBMask(m_classInfo.Mask, m_classInfo.ANDFeatures);
		}
	}
}