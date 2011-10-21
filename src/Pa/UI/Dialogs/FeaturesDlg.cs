using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
		private List<PhoneInfo> _phones;
		private ToolTip _phoneToolTip;

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
		public FeaturesDlg(PaProject project) : this()
		{
			_project = project;

			BuildPhoneGrid();
			LoadPhoneGrid();

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

			if (Settings.Default.FeaturesDlgPhoneGrid != null)
				Settings.Default.FeaturesDlgPhoneGrid.InitializeGrid(_gridPhones);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadPhoneGrid()
		{
			_gridPhones.Rows.Clear();

			_phones = (from p in _project.PhoneCache.Values
					   where p is PhoneInfo
					   orderby p.POAKey
					   select p.Clone() as PhoneInfo).ToList();

			_gridPhones.RowCount = _phones.Count;

			if (_phones.Count > 0)
				_gridPhones.CurrentCell = _gridPhones[0, 0];

			AdjustGridRows(_gridPhones, Settings.Default.FeaturesDlgGridExtraRowHeight);
			_gridPhones.IsDirty = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjusts the rows in the specified grid by letting the grid calculate the row
		/// heights automatically, then adds the extra amount specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void AdjustGridRows(DataGridView grid, int extraAmount)
		{
			try
			{
				// Sometimes (or maybe always) this throws an exception when
				// the first row is the only row and is the NewRowIndex row.
				grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			}
			catch { }

			grid.AutoResizeRows();

			if (extraAmount > 0)
			{
				foreach (DataGridViewRow row in grid.Rows)
					row.Height += extraAmount;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			int savedLoc = Settings.Default.FeaturesDlgSplitLoc;

			if (savedLoc > 0 && savedLoc >= _splitFeatures.Panel1MinSize)
				_splitFeatures.SplitterDistance = savedLoc;

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the user has changed any of the features
		/// for the phones in the phone inventory list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool GetDidAnyPhoneFeaturesChange()
		{
			return (from phone in _phones
					let origPhone = _project.PhoneCache[phone.Phone]
					where origPhone.AMask != phone.AMask || origPhone.BMask != phone.BMask
					select phone).Any();
		}

		#region Overridden methods of base class
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return base.IsDirty || GetDidAnyPhoneFeaturesChange(); }
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.FeaturesDlgPhoneGrid = GridSettings.Create(_gridPhones);
			Settings.Default.FeaturesDlgSplitLoc = _splitFeatures.SplitterDistance;
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			Hide();
			base.SaveChanges();
			_project.UpdateFeatureOverrides(_phones.Where(p => p.HasAFeatureOverrides || p.HasBFeatureOverrides));
			App.Project.ReloadDataSources();
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
			int i = e.RowIndex;
			if (_phones == null || i < 0 || i >= _phones.Count)
			{
				e.Value = null;
				return;
			}

			if (e.ColumnIndex == 0)
				e.Value = _phones[i].Phone;
			else
				e.Value = _phones[i].TotalCount + _phones[i].CountAsPrimaryUncertainty;
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePhoneGridCellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.ColumnIndex != 0)
					return;

				var phoneInfo = _phones[e.RowIndex];
				if (phoneInfo == null || phoneInfo.Phone.Trim().Length == 0)
					return;

				var bldr = new StringBuilder();
				foreach (char c in phoneInfo.Phone)
					bldr.AppendFormat("U+{0:X4}, ", (int)c);

				var fmt = App.GetString("FeaturesDlg.PhonesGridInfoFormat", "Unicode Values:\n{0}");
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
		private void HandlePhoneGridRowEnter(object sender, DataGridViewCellEventArgs e)
		{
			UpdateDisplay(e.RowIndex);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void HandleResetButtonClick(object sender, EventArgs e)
		{
			int i = _gridPhones.CurrentCellAddress.Y;
			if (i < 0 || i >= _phones.Count)
				return;

			_featuresTab.Reset();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			UpdateDisplay(_gridPhones.CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay(int phoneIndex)
		{
			_featuresTab.SetCurrentInfo(_phones[phoneIndex]);
			
			_buttonReset.Enabled = (_featuresTab.IsAFeatureTabShowing ?
				_phones[phoneIndex].HasAFeatureOverrides : _phones[phoneIndex].HasBFeatureOverrides);
		}
	}
}
