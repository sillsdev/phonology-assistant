using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class FeaturesDlg : OKCancelDlgBase
	{
		private readonly PaProject _project;
		//private List<PhoneInfo> _phones;
		private ToolTip _phoneToolTip;
		private FeaturesDlgViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		public FeaturesDlg()
		{
			InitializeComponent();

			if (DesignMode)
				return;

			DoubleBuffered = true;
			_panelPhoneList.Font = FontHelper.UIFont;
			_phoneToolTip = new ToolTip();
			_panelPhoneList.BorderStyle = BorderStyle.None;
			_panelPhoneList.DrawOnlyBottomBorder = true;
		}

		/// ------------------------------------------------------------------------------------
		public FeaturesDlg(PaProject project, FeaturesDlgViewModel viewModel) : this()
		{
			_viewModel = viewModel;
			
			_project = project;

			BuildPhoneGrid();

			_buttonReset.Margin = new Padding(0, btnOK.Margin.Top, 0, btnOK.Margin.Bottom);
			tblLayoutButtons.Controls.Add(_buttonReset, 0, 0);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				if (_phoneToolTip != null)
				{
					_phoneToolTip.Dispose();
					_phoneToolTip = null;
				}

				components.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		private void BuildPhoneGrid()
		{
			_gridPhones.Name = Name + "PhoneGrid";
			_gridPhones.AutoGenerateColumns = false;
			_gridPhones.Font = FontHelper.UIFont;
			_gridPhones.VirtualMode = true;
			_gridPhones.CellValueNeeded += HandlePhoneGridCellValueNeeded;

			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("phone");
			col.ReadOnly = true;
			col.Width = 55;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = App.PhoneticFont;
			col.CellTemplate.Style.Font = App.PhoneticFont;
			col.HeaderText = App.GetString("FeaturesDlg.PhoneListPhoneHeadingText", "Phone");
			_gridPhones.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("count");
			col.ReadOnly = true;
			col.Width = 55;
			col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			col.HeaderText = App.GetString("FeaturesDlg.PhoneListCountHeadingText", "Count");
			_gridPhones.Columns.Add(col);

			if (Settings.Default.DescriptiveFeaturesDlgPhoneGrid != null)
				Settings.Default.DescriptiveFeaturesDlgPhoneGrid.InitializeGrid(_gridPhones);

			_gridPhones.Rows.Clear();
			_gridPhones.RowCount = _viewModel.PhoneCount;

			if (_gridPhones.RowCount > 0)
				_gridPhones.CurrentCell = _gridPhones[0, 0];

			_gridPhones.AdjustGridRows(Settings.Default.DescriptiveFeaturesDlgGridExtraRowHeight);
			_gridPhones.IsDirty = false;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			int savedLoc = Settings.Default.DescriptiveFeaturesDlgSplitLoc;

			if (savedLoc > 0 && savedLoc >= _splitFeatures.Panel1MinSize)
				_splitFeatures.SplitterDistance = savedLoc;

			UpdateDisplay();
		}

		#region Overridden methods of base class
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return base.IsDirty || _viewModel.GetDidAnyPhoneFeaturesChange(); }
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.DescriptiveFeaturesDlgPhoneGrid = GridSettings.Create(_gridPhones);
			Settings.Default.DescriptiveFeaturesDlgSplitLoc = _splitFeatures.SplitterDistance;
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			Hide();
			base.SaveChanges();
			_viewModel.SaveChanges();
			return true;
		}

		#endregion

		#region Grid event handlers
		/// ------------------------------------------------------------------------------------
		private void HandlePhoneGridCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex < 0 || e.RowIndex < 0)
				return;

			//if ((_featuresTab.IsAFeatureTabShowing && _phones[e.RowIndex].HasAFeatureOverrides) ||
			//    (_featuresTab.IsBFeatureTabShowing && _phones[e.RowIndex].HasBFeatureOverrides))
			//{
			//    e.CellStyle.ForeColor = Color.Black;
			//    e.CellStyle.SelectionForeColor = Color.White;
				
			//    e.CellStyle.BackColor = ColorHelper.CalculateColor(
			//        Color.White, Color.BlueViolet, 220);

			//    e.CellStyle.SelectionBackColor = Color.BlueViolet;
			//}
		}

		/// ------------------------------------------------------------------------------------
		void HandlePhoneGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			if (e.ColumnIndex == 0)
				e.Value = _viewModel.GetPhone(e.RowIndex);
			else
				e.Value = _viewModel.GetPhoneCount(e.RowIndex);
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePhoneGridCellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.ColumnIndex != 0)
					return;

				var phone = _viewModel.GetPhone(e.RowIndex);
				if (phone == null || phone.Trim().Length == 0)
					return;

				var bldr = new StringBuilder();
				foreach (char c in phone)
					bldr.AppendFormat("U+{0:X4}, ", (int)c);

				var fmt = App.GetString("DialogBoxes.FeaturesDlg.PhonesGridInfoFormat", "Unicode Values:\n{0}");
				var tip = bldr.ToString();
				tip = string.Format(fmt, tip.Substring(0, tip.Length - 2));
				tip = Utils.ConvertLiteralNewLines(tip);

				var rc = _gridPhones.GetCellDisplayRectangle(0, e.RowIndex, true);
				var pt = _gridPhones.PointToScreen(new Point(rc.Right - 5, rc.Bottom - 4));
				pt = PointToClient(pt);
				_phoneToolTip.Tag = rc;
				_phoneToolTip.Show(tip, this, pt, 5000);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePhoneGridCellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				// Sometimes this event is fired because the mouse is over the tooltip, even
				// though it's tip's point is still within the bounds of the cell. It is only
				// when the mouse location leaves the cell do we want to hide the tooltip.
				var rc = _gridPhones.GetCellDisplayRectangle(0, e.RowIndex, true);
				var pt = _gridPhones.PointToClient(MousePosition);
				if (!rc.Contains(pt))
					_phoneToolTip.Hide(this);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePhoneGridCurrentRowChanged(object sender, EventArgs e)
		{
			UpdateDisplay();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void HandleResetButtonClick(object sender, EventArgs e)
		{
			_featuresTab.Reset();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_featuresTab.SetCurrentInfo(_viewModel.GetPhoneInfo(_gridPhones.CurrentCellAddress.Y));
			
			//_buttonReset.Enabled = (_featuresTab.IsAFeatureTabShowing ?
			//    _phones[phoneIndex].HasAFeatureOverrides : _phones[phoneIndex].HasBFeatureOverrides);
		}
	}
}
