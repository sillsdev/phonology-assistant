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
using L10NSharp;
using SIL.Pa.PhoneticSearching;
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
			_labelClassTypeValue.Text = LocalizationManager.GetString(
				"DialogBoxes.DefineClassesDialogs.DistinctiveFeatureClassDlg.ClassTypeLabel",
				"Distinctive features");
		}

		/// ------------------------------------------------------------------------------------
		protected override bool UseCompactConsonantView
		{
			get { return Properties.Settings.Default.DefineDistinctiveFeatureClassDlgUseCompactConsonantView; }
			set { Properties.Settings.Default.DefineDistinctiveFeatureClassDlgUseCompactConsonantView = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool UseCompactVowelView
		{
			get { return Properties.Settings.Default.DefineDistinctiveFeatureClassDlgUseCompactVowelView; }
			set { Properties.Settings.Default.DefineDistinctiveFeatureClassDlgUseCompactVowelView = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Properties.Settings.Default.DefineDistinctiveFeatureClassDlgSplit1Loc = _splitterCV.SplitterDistance;
			Properties.Settings.Default.DefineDistinctiveFeatureClassDlgSplit2Loc = _splitterOuter.SplitterDistance;
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
				if (Properties.Settings.Default.DefineDistinctiveFeatureClassDlgSplit2Loc > 0)
					_splitterOuter.SplitterDistance = Properties.Settings.Default.DefineDistinctiveFeatureClassDlgSplit2Loc;

				if (Properties.Settings.Default.DefineDistinctiveFeatureClassDlgSplit1Loc > 0)
					_splitterCV.SplitterDistance = Properties.Settings.Default.DefineDistinctiveFeatureClassDlgSplit1Loc;
			}
			catch { }

			base.LoadSplitterSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			App.ShowHelpTopic("hidDistinctiveFeatureClassDlg");
		}

		/// ------------------------------------------------------------------------------------
		protected override void UpdateCharacterViewers()
		{
			_conViewer.SetBMask(m_classInfo.Mask, m_classInfo.ANDFeatures);
			_vowViewer.SetBMask(m_classInfo.Mask, m_classInfo.ANDFeatures);
		}
	}
}