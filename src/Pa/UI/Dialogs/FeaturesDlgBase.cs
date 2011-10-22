using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class FeaturesDlgBase : OKCancelDlgBase
	{
		protected ToolTip _phoneToolTip;
		private readonly Timer _phoneToolTipTimer;
		protected readonly FeaturesDlgViewModel _viewModel;
		protected readonly FeatureListView _listView;

		/// ------------------------------------------------------------------------------------
		public FeaturesDlgBase()
		{
			InitializeComponent();

			if (DesignMode)
				return;

			DoubleBuffered = true;
			
			_panelPhoneListHeading.Font = FontHelper.UIFont;
			_panelFeaturesHeading.Font = FontHelper.UIFont;
			_labelDistinctiveFeaturesSet.Font = FontHelper.UIFont;
			_labelPhoneDescription.Font = new Font(FontHelper.UIFont, FontStyle.Bold);

			_phoneToolTip = new ToolTip();
			_panelPhoneListHeading.BorderStyle = BorderStyle.None;
			_panelPhoneListHeading.DrawOnlyBottomBorder = true;
			_panelFeaturesHeading.BorderStyle = BorderStyle.None;
			_panelFeaturesHeading.DrawOnlyBottomBorder = true;

			_phoneToolTipTimer = new Timer();
			_phoneToolTipTimer.Interval = 250;
			_phoneToolTipTimer.Tick += delegate
			{
				_phoneToolTipTimer.Stop();
				var text = ((object[])_phoneToolTipTimer.Tag)[0] as string;
				var pt = (Point)((object[])_phoneToolTipTimer.Tag)[1];
				_phoneToolTip.Show(text, this, pt, 5000);
			};
		}

		/// ------------------------------------------------------------------------------------
		public FeaturesDlgBase(FeaturesDlgViewModel viewModel, FeatureListView listView) : this()
		{
			_viewModel = viewModel;
			_listView = listView;
			
			BuildPhoneGrid();

			_buttonReset.Margin = new Padding(0, btnOK.Margin.Top, 0, btnOK.Margin.Bottom);
			tblLayoutButtons.Controls.Add(_buttonReset, 0, 0);

			_labelDistinctiveFeaturesSet.Text = GetDistinctiveFeaturesSetName();

			_listView.Dock = DockStyle.Fill;
			_listView.Margin = new Padding(0);
			_listView.BorderStyle = BorderStyle.None;
			_tableLayout.Controls.Add(_listView, 0, 2);
			_panelPhoneListHeading.ControlReceivingFocusOnMnemonic = _listView;
			_listView.Load();
			_listView.FeatureChanged += delegate { UpdateDisplay(); };
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
		protected virtual void BuildPhoneGrid()
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

			_gridPhones.Rows.Clear();
			_gridPhones.RowCount = _viewModel.PhoneCount;

			if (_gridPhones.RowCount > 0)
				_gridPhones.CurrentCell = _gridPhones[0, 0];

			_gridPhones.AutoResizeColumnHeadersHeight();
			_gridPhones.ColumnHeadersHeight += 8;
			
			_gridPhones.IsDirty = false;
			App.SetGridSelectionColors(_gridPhones, false);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			_labelDistinctiveFeaturesSet.Left = _panelFeaturesHeading.ClientSize.Width -
				_labelDistinctiveFeaturesSet.Width - 8;

			base.OnShown(e);

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string GetDistinctiveFeaturesSetName()
		{
			return string.Empty;
		}

		#region Overridden methods of base class
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return base.IsDirty || _viewModel.GetDidAnyPhoneFeaturesChange(); }
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

		/// ------------------------------------------------------------------------------------
		private void HandlePaintLineUnderDescription(object sender, PaintEventArgs e)
		{
			var pt1 = new Point(3, _labelPhoneDescription.Bottom + 3);
			var pt2 = new Point(_tableLayout.Width - 6, _labelPhoneDescription.Bottom + 3);

			using (var br = new LinearGradientBrush(pt1, pt2, SystemColors.WindowText, SystemColors.Window))
			{
				var blend = new Blend();
				blend.Positions = new[] { 0.0f, 0.25f, 0.5f, 0.75f, 1.0f };
				blend.Factors = new[] { 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };
				br.Blend = blend;

				using (var pen = new Pen(br))
					e.Graphics.DrawLine(pen, pt1, pt2);
			}
		}

		#region Grid event handlers
		/// ------------------------------------------------------------------------------------
		private void HandlePhoneGridCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			//if (e.ColumnIndex < 0 || e.RowIndex < 0)
			//    return;

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

				bldr.Length -= 2;
				var fmt = App.GetString("DialogBoxes.FeaturesDlg.CommonStrings.PhoneGridInfoToolTipFormat", "Unicode Values:\n{0}");
				var tip = Utils.ConvertLiteralNewLines(string.Format(fmt, bldr));

				var rc = _gridPhones.GetCellDisplayRectangle(0, e.RowIndex, true);
				var pt = _gridPhones.PointToScreen(new Point(rc.Right - 5, rc.Bottom - 4));
				pt = PointToClient(pt);
				_phoneToolTip.Tag = rc;
				_phoneToolTipTimer.Tag = new object[] { tip, pt };
				_phoneToolTipTimer.Start();
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePhoneGridCellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				_phoneToolTipTimer.Stop();

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
		private void HandlePhoneGridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.ColumnIndex != 0 || e.RowIndex < 0 || !GetDoesPhoneHaveOverrides(e.RowIndex))
				return;

			e.Paint(e.CellBounds, e.PaintParts);
			e.Handled = true;

			var rc = e.CellBounds;

			Point pt1 = new Point(rc.Right - 7, rc.Y - 1);
			Point pt2 = new Point(rc.Right - 1, rc.Y + 6);
			Point ptCorner = new Point(rc.Right - 1, rc.Top);

			using (var br = new LinearGradientBrush(pt1, pt2, Color.LightBlue, Color.DarkBlue))
				e.Graphics.FillPolygon(br, new[] { pt1, pt2, ptCorner });
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePhoneGridCurrentRowChanged(object sender, EventArgs e)
		{
			UpdateDisplay();
		}

		#endregion

		#region Reset related methods
		/// ------------------------------------------------------------------------------------
		private void HandleResetButtonClick(object sender, EventArgs e)
		{
			Reset();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void Reset()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Update display methods
		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			if (DesignMode || !Visible)
				return;

			_labelPhoneDescription.Text = _viewModel.GetPhoneDescription(_gridPhones.CurrentCellAddress.Y);
			_listView.SetMaskFromPhoneInfo(_viewModel.GetPhoneInfo(_gridPhones.CurrentCellAddress.Y));
			_buttonReset.Enabled = GetDoesPhoneHaveOverrides();
			_gridPhones.InvalidateCell(0, _gridPhones.CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool GetDoesPhoneHaveOverrides()
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool GetDoesPhoneHaveOverrides(int rowIndex)
		{
			throw new NotImplementedException();
		}
	
		#endregion
	}
}
