using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class DescriptiveFeaturesDlg : FeaturesDlgBase
	{
		/// ------------------------------------------------------------------------------------
		public DescriptiveFeaturesDlg(FeaturesDlgViewModel viewModel)
			: base(viewModel, new DescriptiveFeatureListView())
		{
			InitializeComponent();
			_listView.EmphasizeCheckedItems = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(System.EventArgs e)
		{
			if (Settings.Default.DescriptiveFeaturesDlgPhoneGrid != null)
				Settings.Default.DescriptiveFeaturesDlgPhoneGrid.InitializeGrid(_gridPhones);

			_gridPhones.AdjustGridRows(Settings.Default.DescriptiveFeaturesDlgGridExtraRowHeight);
			
			int savedLoc = Settings.Default.DescriptiveFeaturesDlgSplitLoc;
			if (savedLoc > 0 && savedLoc >= _splitFeatures.Panel1MinSize)
				_splitFeatures.SplitterDistance = savedLoc;

			base.OnShown(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.DescriptiveFeaturesDlgPhoneGrid = GridSettings.Create(_gridPhones);
			Settings.Default.DescriptiveFeaturesDlgSplitLoc = _splitFeatures.SplitterDistance;
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override void Reset()
		{
			_viewModel.GetPhoneInfo(_gridPhones.CurrentCellAddress.Y).ResetAFeatures();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool GetDoesPhoneHaveOverrides()
		{
			return GetDoesPhoneHaveOverrides(_gridPhones.CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool GetDoesPhoneHaveOverrides(int rowIndex)
		{
			return _viewModel.GetPhoneInfo(rowIndex).HasAFeatureOverrides;
		}
	}
}
