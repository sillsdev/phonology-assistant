using System;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class DefineDescriptiveFeatureClassDlg : DefineFeatureClassDlgBase
	{
		#region Construction and setup
		/// ------------------------------------------------------------------------------------
		public DefineDescriptiveFeatureClassDlg(ClassListViewItem classInfo, ClassesDlg classDlg)
			: base(classInfo ?? new ClassListViewItem { ClassType = SearchClassType.Articulatory },
					classDlg, new DescriptiveFeatureListView(), App.AFeatureCache.GetEmptyMask())
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		protected override void SetLocalizedTexts()
		{
			lblClassTypeValue.Text = App.GetString(
				"DefineClassDlg.ArticulatoryFeaturesClassTypeLabel", "Descriptive features",
				"Articulatory features class type label.");
		}

		#endregion

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			App.ShowHelpTopic("hidArticulatoryFeatureClassDlg");
		}

		/// ------------------------------------------------------------------------------------
		protected override void UpdateCharacterViewers()
		{
			_conViewer.SetAMasks(m_classInfo.Mask, m_classInfo.ANDFeatures);
			_vowViewer.SetAMasks(m_classInfo.Mask, m_classInfo.ANDFeatures);
			_otherPhonesViewer.SetAMasks(m_classInfo.Mask, m_classInfo.ANDFeatures);
		}

		#endregion
	}
}