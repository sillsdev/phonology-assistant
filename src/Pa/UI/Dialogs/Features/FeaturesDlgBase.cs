using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Localization;
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
		protected readonly FeatureListViewBase _listView;

		private readonly Color _overrideHighlightBorderColor;
		private readonly Color _overrideHighlightBottomColor = Color.FromArgb(255, 255, 150);
		private readonly Color _overrideHighlightTopColor = ColorHelper.CalculateColor(Color.White, Color.FromArgb(255, 255, 150), 150);

		private readonly Color _selectedItemTopColor = Color.FromArgb(228, 237, 247);
		private readonly Color _selectedItemBottomColor = Color.FromArgb(185, 209, 234);
		private readonly Color _selectedItemBorderColor = Color.FromArgb(112, 127, 242);

		/// ------------------------------------------------------------------------------------
		public FeaturesDlgBase()
		{
			InitializeComponent();

			if (DesignMode)
				return;

			DoubleBuffered = true;

			_overrideHighlightBorderColor = _gridPhones.GridColor;
			
			_panelPhoneListHeading.Font = FontHelper.UIFont;
			_panelFeaturesHeading.Font = FontHelper.UIFont;
			_labelDistinctiveFeaturesSet.Font = FontHelper.UIFont;
			_labelDistinctiveFeaturesSetValue.Font = FontHelper.UIFont;
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
		public FeaturesDlgBase(FeaturesDlgViewModel viewModel, FeatureListViewBase listView) : this()
		{
			_viewModel = viewModel;
			_listView = listView;
			
			BuildPhoneGrid();

			_buttonReset.Margin = new Padding(0, btnOK.Margin.Top, 0, btnOK.Margin.Bottom);
			tblLayoutButtons.Controls.Add(_buttonReset, 0, 0);

			_labelDistinctiveFeaturesSetValue.Text = GetDistinctiveFeaturesSetName();

			_listView.BackColor = Color.White;
			_listView.ForeColor = Color.Black;
			_listView.Dock = DockStyle.Fill;
			_listView.Margin = new Padding(0);
			_listView.BorderStyle = BorderStyle.None;
			_tableLayout.Controls.Add(_listView, 0, 2);
			_panelPhoneListHeading.ControlReceivingFocusOnMnemonic = _listView;
			_listView.Load();
			_listView.FeatureChanged += delegate { UpdateDisplay(); };
			_listView.DrawItemBackgroundAndGetForeColor = ListViewItemBackgroundPainter;
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
			_gridPhones.BackgroundColor = Color.White;
			_gridPhones.ForeColor = Color.Black;

			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("phone");
			col.ReadOnly = true;
			col.Width = 55;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			col.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
			col.DefaultCellStyle.Font = App.PhoneticFont;
			col.CellTemplate.Style.Font = App.PhoneticFont;
			_gridPhones.Columns.Add(col);
			col.HeaderText = LocalizationManager.GetString(
				"DialogBoxes.FeaturesDlgBase.PhoneListGrid.ColumnHeadings.Phone", "Phone", null, col);

			col = SilGrid.CreateTextBoxColumn("count");
			col.ReadOnly = true;
			col.Width = 55;
			col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			col.DefaultCellStyle.Padding = new Padding(0, 0, 5, 0);
			_gridPhones.Columns.Add(col);
			col.HeaderText = LocalizationManager.GetString(
				"DialogBoxes.FeaturesDlgBase.PhoneListGrid.ColumnHeadings.Count", "Count", null, col);

			_gridPhones.Rows.Clear();
			_gridPhones.RowCount = _viewModel.PhoneCount;

			if (_gridPhones.RowCount > 0)
				_gridPhones.CurrentCell = _gridPhones[0, 0];

			_gridPhones.AutoResizeColumnHeadersHeight();
			_gridPhones.ColumnHeadersHeight += 8;
			
			_gridPhones.IsDirty = false;

			_gridPhones.DefaultCellStyle.SelectionForeColor = Color.Black;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
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

		/// ------------------------------------------------------------------------------------
		private Color ListViewItemBackgroundPainter(Graphics g, Rectangle rc, bool selected, bool itemNotInDefaultState)
		{
			if (selected || itemNotInDefaultState)
			{
				DrawHighlightedGridOrListViewItemBackground(g, rc, selected);

				var clrBorder = (selected ? _selectedItemBorderColor : _overrideHighlightBorderColor);
				using (var pen = new Pen(clrBorder))
				{
					rc.Width--;
					rc.Height--;
					g.DrawRectangle(pen, rc);
				}
			}

			return _listView.ForeColor;
		}

		/// ------------------------------------------------------------------------------------
		private void DrawHighlightedGridOrListViewItemBackground(Graphics g, Rectangle rc, bool selected)
		{
			var clrTop = (selected ? _selectedItemTopColor : _overrideHighlightTopColor);
			var clrBottom = (selected ? _selectedItemBottomColor : _overrideHighlightBottomColor);
			PaintingHelper.DrawGradientBackground(g, rc, clrTop, clrBottom, true);
		}

		#region Grid event handlers
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
				var fmt = LocalizationManager.GetString("DialogBoxes.FeaturesDlgBase.PhoneGridInfoToolTipFormat", "Unicode Values:\n{0}");
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
			var selected = (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected;
			if (e.ColumnIndex < 0 || e.RowIndex < 0 || (!selected && !GetDoesPhoneHaveOverrides(e.RowIndex)))
				return;

			var parts = e.PaintParts;
			parts &= ~DataGridViewPaintParts.Background;
			parts &= ~DataGridViewPaintParts.SelectionBackground;
			e.Paint(e.CellBounds, parts);
			e.Handled = true;

			var rc = e.CellBounds;
			rc.Width--;
			rc.Height--;
			DrawHighlightedGridOrListViewItemBackground(e.Graphics, rc, selected);

			e.PaintContent(e.CellBounds);
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

			int index = _gridPhones.CurrentCellAddress.Y;

			_labelPhoneDescription.Text = _viewModel.GetPhoneDescription(index);
			_listView.SetDefaultFeatures(_viewModel.GetListOfDefaultFeaturesForPhone((index)));
			_listView.SetMaskFromPhoneInfo(_viewModel.GetPhoneInfo(index));
			_buttonReset.Enabled = GetDoesPhoneHaveOverrides();
			_gridPhones.InvalidateRow(index);
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
