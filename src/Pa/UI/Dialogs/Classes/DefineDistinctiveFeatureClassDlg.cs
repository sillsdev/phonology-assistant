using System;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class DefineDistinctiveFeatureClassDlgBase : DefineFeatureClassDlgBase
	{
		#region Construction and setup
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
			lblClassTypeValue.Text = App.GetString("DefineClassDlg.BinaryFeaturesClassTypeLabel",
				"Binary features", "Binary features class type label.");
		}

		#endregion

		#region Event handlers
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
			_otherPhonesViewer.SetBMask(m_classInfo.Mask, m_classInfo.ANDFeatures);
		}

		#endregion
	}
}