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
			get { return Properties.Settings.Default.DefineDescriptiveFeatureClassDlgUseCompactConsonantView; }
			set { Properties.Settings.Default.DefineDescriptiveFeatureClassDlgUseCompactConsonantView = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool UseCompactVowelView
		{
			get { return Properties.Settings.Default.DefineDescriptiveFeatureClassDlgUseCompactVowelView; }
			set { Properties.Settings.Default.DefineDescriptiveFeatureClassDlgUseCompactVowelView = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Properties.Settings.Default.DefineDescriptiveFeatureClassDlgSplit1Loc = _splitterCV.SplitterDistance;
			Properties.Settings.Default.DefineDescriptiveFeatureClassDlgSplit2Loc = _splitterOuter.SplitterDistance;
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
				if (Properties.Settings.Default.DefineDescriptiveFeatureClassDlgSplit2Loc > 0)
					_splitterOuter.SplitterDistance = Properties.Settings.Default.DefineDescriptiveFeatureClassDlgSplit2Loc;

				if (Properties.Settings.Default.DefineDescriptiveFeatureClassDlgSplit1Loc > 0)
					_splitterCV.SplitterDistance = Properties.Settings.Default.DefineDescriptiveFeatureClassDlgSplit1Loc;
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