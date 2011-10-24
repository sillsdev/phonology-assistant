using System.Collections.Generic;
using System.Linq;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class DistinctiveFeaturesDlg : FeaturesDlgBase
	{
		/// ------------------------------------------------------------------------------------
		public DistinctiveFeaturesDlg(FeaturesDlgViewModel viewModel)
			: base(viewModel, new DistinctiveFeatureListView())
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(System.EventArgs e)
		{
			if (Settings.Default.DistinctiveFeaturesDlgPhoneGrid != null)
				Settings.Default.DistinctiveFeaturesDlgPhoneGrid.InitializeGrid(_gridPhones);

			_gridPhones.AdjustGridRows(Settings.Default.DistinctiveFeaturesDlgGridExtraRowHeight);
			
			int savedLoc = Settings.Default.DistinctiveFeaturesDlgSplitLoc;

			if (savedLoc > 0 && savedLoc >= _splitFeatures.Panel1MinSize)
				_splitFeatures.SplitterDistance = savedLoc;

			base.OnShown(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.DistinctiveFeaturesDlgPhoneGrid = GridSettings.Create(_gridPhones);
			Settings.Default.DistinctiveFeaturesDlgSplitLoc = _splitFeatures.SplitterDistance;
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetDistinctiveFeaturesSetName()
		{
			return _viewModel.Project.DistinctiveFeatureSet;
		}

		/// ------------------------------------------------------------------------------------
		protected override void Reset()
		{
			_viewModel.GetPhoneInfo(_gridPhones.CurrentCellAddress.Y).ResetBFeatures();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool GetDoesPhoneHaveOverrides()
		{
			return GetDoesPhoneHaveOverrides(_gridPhones.CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool GetDoesPhoneHaveOverrides(int rowIndex)
		{
			return _viewModel.GetPhoneInfo(rowIndex).HasBFeatureOverrides;
		}
	}
}
