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
		private readonly PaProject m_project;
		private ToolTip m_phoneToolTip;
		private List<IPhoneInfo> m_phones;

		/// ------------------------------------------------------------------------------------
		public FeaturesDlg()
		{
			InitializeComponent();

			if (DesignMode)
				return;

			DoubleBuffered = true;
			pgpPhoneList.Font = FontHelper.UIFont;
			m_phoneToolTip = new ToolTip();
			pgpPhoneList.BorderStyle = BorderStyle.None;
			pgpPhoneList.DrawOnlyBottomBorder = true;
		}

		/// ------------------------------------------------------------------------------------
		public FeaturesDlg(PaProject project) : this()
		{
			m_project = project;

			BuildPhoneGrid();
			LoadPhoneGrid();

			btnReset.Margin = new Padding(0, btnOK.Margin.Top, 0, btnOK.Margin.Bottom);
			tblLayoutButtons.Controls.Add(btnReset, 0, 0);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				if (m_phoneToolTip != null)
				{
					m_phoneToolTip.Dispose();
					m_phoneToolTip = null;
				}

				components.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		private void BuildPhoneGrid()
		{
			gridPhones.Name = Name + "PhoneGrid";
			gridPhones.AutoGenerateColumns = false;
			gridPhones.Font = FontHelper.UIFont;
			gridPhones.VirtualMode = true;
			gridPhones.CellValueNeeded += HandlePhoneGridCellValueNeeded;

			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("phone");
			col.ReadOnly = true;
			col.Width = 55;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = App.PhoneticFont;
			col.CellTemplate.Style.Font = App.PhoneticFont;
			col.HeaderText = App.GetString("FeaturesDlg.PhoneListPhoneHeadingText", "Phone");
			
			gridPhones.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("count");
			col.ReadOnly = true;
			col.Width = 55;
			col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			col.HeaderText = App.GetString("FeaturesDlg.PhoneListCountHeadingText", "Count");

			gridPhones.Columns.Add(col);

			if (Settings.Default.FeaturesDlgPhoneGrid != null)
				Settings.Default.FeaturesDlgPhoneGrid.InitializeGrid(gridPhones);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadPhoneGrid()
		{
			gridPhones.Rows.Clear();

			m_phones = (from x in m_project.PhoneCache.Values
						orderby x.POAKey
						select x.Clone()).ToList();

			gridPhones.RowCount = m_phones.Count;

			if (m_phones.Count > 0)
				gridPhones.CurrentCell = gridPhones[0, 0];

			AdjustGridRows(gridPhones, Settings.Default.FeaturesDlgGridExtraRowHeight);
			gridPhones.IsDirty = false;
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

			if (savedLoc > 0 && savedLoc >= splitFeatures.Panel1MinSize)
				splitFeatures.SplitterDistance = savedLoc;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the user has changed any of the features
		/// for the phones in the phone inventory list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool DidPhoneFeaturesChanged
		{
			get
			{
				return m_phones.Cast<PhoneInfo>().Any(x =>
					x.AMask != x.DefaultAMask || x.BMask != x.DefaultBMask);
			}
		}

		#region Overridden methods of base class
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return base.IsDirty || DidPhoneFeaturesChanged; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.FeaturesDlgPhoneGrid = GridSettings.Create(gridPhones);
			Settings.Default.FeaturesDlgSplitLoc = splitFeatures.SplitterDistance;
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			base.SaveChanges();
			
			var featureOverrideList = new FeatureOverrides();

			foreach (var pi in m_phones)
			{
				// Get the phone from the grid.
				var phoneInfo = pi as PhoneInfo;
				if (phoneInfo == null)
					continue;

				phoneInfo.AFeaturesAreOverridden = (phoneInfo.DefaultAMask != phoneInfo.AMask);
				phoneInfo.BFeaturesAreOverridden = (phoneInfo.DefaultBMask != phoneInfo.BMask);

				if (phoneInfo.AFeaturesAreOverridden || phoneInfo.BFeaturesAreOverridden)
					featureOverrideList[phoneInfo.Phone] = phoneInfo;
			}

			App.MsgMediator.SendMessage("BeforePhoneFeatureOverridesSaved", featureOverrideList);
			featureOverrideList.Save(m_project.ProjectPathFilePrefix);
			App.MsgMediator.SendMessage("AfterPhoneFeatureOverridesSaved", featureOverrideList);
			m_project.ReloadDataSources();
			return true;
		}

		#endregion

		#region Grid event handlers
		/// ------------------------------------------------------------------------------------
		void HandlePhoneGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			int i = e.RowIndex;
			if (m_phones == null || i < 0 || i >= m_phones.Count)
			{
				e.Value = null;
				return;
			}

			if (e.ColumnIndex == 0)
				e.Value = m_phones[i].Phone;
			else
				e.Value = m_phones[i].TotalCount + m_phones[i].CountAsPrimaryUncertainty;
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePhoneGridCellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.ColumnIndex != 0)
					return;

				var phoneInfo = m_phones[e.RowIndex] as PhoneInfo;
				if (phoneInfo == null || phoneInfo.Phone.Trim().Length == 0)
					return;

				var bldr = new StringBuilder();
				foreach (char c in phoneInfo.Phone)
					bldr.AppendFormat("U+{0:X4}, ", (int)c);

				var fmt = App.GetString("FeaturesDlg.PhonesGridInfoFormat", "Unicode Values:\n{0}");
				var tip = bldr.ToString();
				tip = string.Format(fmt, tip.Substring(0, tip.Length - 2));
				tip = Utils.ConvertLiteralNewLines(tip);

				var rc = gridPhones.GetCellDisplayRectangle(0, e.RowIndex, true);
				var pt = gridPhones.PointToScreen(new Point(rc.Right - 5, rc.Bottom - 4));
				pt = PointToClient(pt);
				m_phoneToolTip.Tag = rc;
				m_phoneToolTip.Show(tip, this, pt, 5000);
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
				var rc = gridPhones.GetCellDisplayRectangle(0, e.RowIndex, true);
				var pt = gridPhones.PointToClient(MousePosition);
				if (!rc.Contains(pt))
					m_phoneToolTip.Hide(this);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePhoneGridRowEnter(object sender, DataGridViewCellEventArgs e)
		{
			_featuresTab.SetCurrentInfo(m_phones[e.RowIndex] as IFeatureBearer);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void btnReset_Click(object sender, EventArgs e)
		{
			int i = gridPhones.CurrentCellAddress.Y;
			if (i < 0 || i >= m_phones.Count)
				return;

			_featuresTab.Reset();
		}
	}
}
