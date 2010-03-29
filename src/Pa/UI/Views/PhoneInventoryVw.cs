using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Model;
using SIL.Pa.UI.Controls;
using SilUtils;

namespace SIL.Pa.UI.Views
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class PhoneInventoryVw : UserControl, IxCoreColleague, ITabView
	{
		private bool m_activeView;
		private SizableDropDownPanel m_sddpAFeatures;
		private CustomDropDown m_aFeatureDropdown;
		private FeatureListView m_lvAFeatures;
		private SizableDropDownPanel m_sddpBFeatures;
		private CustomDropDown m_bFeatureDropdown;
		private FeatureListView m_lvBFeatures;
		private ToolTip m_phoneToolTip;
		private ToolTip m_bFeatureToolTip;
		private readonly ITMAdapter m_tmAdapter;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInventoryVw()
		{
			if (!App.DesignMode)
			{
				App.InitializeProgressBarForLoadingView(
					Properties.Resources.kstidPhoneInventoryViewText, 9);
			}

			InitializeComponent();
			Name = "PhoneInventoryVw";
			if (App.DesignMode)
				return;

			SetToolTips();

			App.IncProgressBar();
			m_tmAdapter = AdapterHelper.CreateTMAdapter();
			App.IncProgressBar();

			pgpPhoneList.BorderStyle = BorderStyle.None;

			pgpPhoneList.TextFormatFlags &= ~TextFormatFlags.HidePrefix;
			hlblAFeatures.TextFormatFlags &= ~TextFormatFlags.HidePrefix;
			hlblBFeatures.TextFormatFlags &= ~TextFormatFlags.HidePrefix;

			App.IncProgressBar();
			BuildPhoneGrid();
			App.IncProgressBar();
			LoadPhoneGrid();
			App.IncProgressBar();
			App.IncProgressBar();
			App.IncProgressBar();
			OnPaFontsChanged(null);
			App.IncProgressBar();
			LoadSettings();
			App.IncProgressBar();
			InitializeFeatureDropDowns();
			App.IncProgressBar();
			App.UninitializeProgressBar();

			Padding pdg = splitChanges.Panel2.Padding;
			splitChanges.Panel2.Padding = new Padding(pdg.Left, pdg.Top,
				pdg.Right, btnApply.Height + 5);

			base.DoubleBuffered = true;
			Disposed += PhoneInventoryWnd_Disposed;
			gridPhones.Focus();
		}

		#region Dispose method
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void PhoneInventoryWnd_Disposed(object sender, EventArgs e)
		{
			Disposed -= PhoneInventoryWnd_Disposed;
			
			if (m_bFeatureToolTip != null)
			{
				m_bFeatureToolTip.Dispose();
				m_bFeatureToolTip = null;
			}

			if (m_phoneToolTip != null)
			{
				m_phoneToolTip.Dispose();
				m_phoneToolTip = null;
			}

			if (m_sddpAFeatures != null && !m_sddpAFeatures.IsDisposed)
			{
				m_sddpAFeatures.Dispose();
				m_sddpAFeatures = null;
			}

			if (m_sddpBFeatures != null && !m_sddpBFeatures.IsDisposed)
			{
				m_sddpBFeatures.Dispose();
				m_sddpBFeatures = null;
			}

			if (m_lvAFeatures != null && !m_lvAFeatures.IsDisposed)
			{
				m_lvAFeatures.Dispose();
				m_lvAFeatures = null;
			}

			if (m_lvBFeatures != null && !m_lvBFeatures.IsDisposed)
			{
				m_lvBFeatures.Dispose();
				m_lvBFeatures = null;
			}

			if (m_aFeatureDropdown != null && !m_aFeatureDropdown.IsDisposed)
			{
				m_aFeatureDropdown.Dispose();
				m_aFeatureDropdown = null;
			}

			if (m_bFeatureDropdown != null && !m_bFeatureDropdown.IsDisposed)
			{
				m_bFeatureDropdown.Dispose();
				m_bFeatureDropdown = null;
			}
		
			if (splitOuter != null)
				splitOuter.Dispose();
		}

		#endregion

		#region Misc. setup
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeFeatureDropDowns()
		{
			// Build the articulatory features drop-down.
			m_sddpAFeatures = new SizableDropDownPanel(Name + "AFeatureDropDown",
				new Size((int)(splitFeatures.Panel1.Width * 2.5), 175));
			m_sddpAFeatures.MinimumSize = new Size(200, 100);
			m_sddpAFeatures.BorderStyle = BorderStyle.FixedSingle;
			m_sddpAFeatures.Padding = new Padding(7, 7, 7, m_sddpAFeatures.Padding.Bottom);

			m_aFeatureDropdown = new CustomDropDown();
			m_aFeatureDropdown.AutoCloseWhenMouseLeaves = false;
			m_aFeatureDropdown.AddControl(m_sddpAFeatures);
			m_aFeatureDropdown.Closing += m_featureDropdown_Closing;

			m_lvAFeatures = new FeatureListView(App.FeatureType.Articulatory, m_aFeatureDropdown);
			m_lvAFeatures.Dock = DockStyle.Fill;
			m_lvAFeatures.Load();
			m_sddpAFeatures.Controls.Add(m_lvAFeatures);

			// Build the binary features drop-down.
			m_sddpBFeatures = new SizableDropDownPanel(Name + "BFeatureDropDown",
				new Size((int)(splitFeatures.Panel1.Width * 2.5), 175));
			m_sddpBFeatures.MinimumSize = new Size(200, 100);
			m_sddpBFeatures.BorderStyle = BorderStyle.FixedSingle;
			m_sddpBFeatures.Padding = new Padding(7, 7, 7, m_sddpBFeatures.Padding.Bottom);

			m_bFeatureDropdown = new CustomDropDown();
			m_bFeatureDropdown.AutoCloseWhenMouseLeaves = false;
			m_bFeatureDropdown.AddControl(m_sddpBFeatures);
			m_bFeatureDropdown.Closing += m_featureDropdown_Closing;

			m_lvBFeatures = new FeatureListView(App.FeatureType.Binary, m_bFeatureDropdown);
			m_lvBFeatures.Dock = DockStyle.Fill;
			m_lvBFeatures.Load();
			m_sddpBFeatures.Controls.Add(m_lvBFeatures);

			if (!PaintingHelper.CanPaintVisualStyle())
			{
				m_lvAFeatures.BorderStyle = BorderStyle.FixedSingle;
				m_lvBFeatures.BorderStyle = BorderStyle.FixedSingle;
			}
		}

		#endregion

		#region Build and load phone grid
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildPhoneGrid()
		{
			gridPhones.Name = Name + "PhoneGrid";
			gridPhones.AutoGenerateColumns = false;
			gridPhones.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
			gridPhones.Font = FontHelper.UIFont;
			gridPhones.VirtualMode = true;
			gridPhones.CellValueNeeded += HandlePhoneGridCellValueNeeded;

			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("phone");
			col.ReadOnly = true;
			col.Width = 55;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			col.HeaderText = "Phone";
			gridPhones.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("count");
			col.ReadOnly = true;
			col.Width = 55;
			col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			col.HeaderText = "Count";
			gridPhones.Columns.Add(col);

			App.SettingsHandler.LoadGridProperties(gridPhones);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private List<IPhoneInfo> Phones { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadPhoneGrid()
		{
			gridPhones.Rows.Clear();

			Phones = (from x in App.PhoneCache.Values
						orderby x.POAKey
						select x.Clone()).ToList();

			gridPhones.RowCount = Phones.Count;

			if (Phones.Count > 0)
				gridPhones.CurrentCell = gridPhones[0, 0];

			AdjustGridRows(gridPhones, "phonegridextrarowheight", 3);
			gridPhones.IsDirty = false;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandlePhoneGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			int i = e.RowIndex;
			if (Phones == null || i < 0 || i >= Phones.Count)
			{
				e.Value = null;
				return;
			}

			if (e.ColumnIndex == 0)
				e.Value = Phones[i].Phone;
			else
				e.Value = Phones[i].TotalCount + Phones[i].CountAsPrimaryUncertainty;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjusts the rows in the specified grid by letting the grid calculate the row
		/// heights automatically, then adds an extra amount, found in the settings file,
		/// to each row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustGridRows(DataGridView grid, string settingsValue, int defaultAmount)
		{
			try
			{
				// Sometimes (or maybe always) this throws an exception when
				// the first row is the only row and is the NewRowIndex row.
				grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			}
			catch { }

			grid.AutoResizeRows();

			int extraRowHeight =
				App.SettingsHandler.GetIntSettingsValue(Name, settingsValue, defaultAmount);
			
			foreach (DataGridViewRow row in grid.Rows)
				row.Height += extraRowHeight;
		}

		#region Loading/Saving settings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadSettings()
		{
			OnViewDocked(this);
			App.SettingsHandler.LoadGridProperties(gridPhones);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure that if there are unpplied changes, the user has a chance to save them
		/// before PA shuts down. Returning true from this method tells PA's shutdown process
		/// to be cancelled.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPaShuttingDown(object args)
		{
			if (PhoneFeaturesChanged)
			{
				string msg = Properties.Resources.kstidUnAppliedPhoneInventoryChangesMsg;
				DialogResult rslt = Utils.MsgBox(msg, MessageBoxButtons.YesNoCancel);
				if (rslt == DialogResult.Cancel)
					return true;

				if (rslt == DialogResult.Yes)
					SaveChanges(false);
			}

			return false;
		}

		#endregion
		
		#region ITabView Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ActiveView
		{
			get { return m_activeView; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetViewActive(bool makeActive, bool isDocked)
		{
			m_activeView = makeActive;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Form OwningForm
		{
			get { return FindForm(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the view's toolbar/menu adapter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ITMAdapter TMAdapter
		{
			get { return m_tmAdapter; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves some misc. settings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveSettings()
		{
			App.SettingsHandler.SaveGridProperties(gridPhones);
			
			float splitRatio = splitOuter.SplitterDistance / (float)splitOuter.Width;
			App.SettingsHandler.SaveSettingsValue(Name, "splitratio1", splitRatio);

			splitRatio = splitChanges.SplitterDistance / (float)splitChanges.Height;
			App.SettingsHandler.SaveSettingsValue(Name, "splitratio2", splitRatio);

			splitRatio = splitInventory.SplitterDistance / (float)splitInventory.Height;
			App.SettingsHandler.SaveSettingsValue(Name, "splitratio3", splitRatio);

			splitRatio = splitFeatures.SplitterDistance / (float)splitFeatures.Height;
			App.SettingsHandler.SaveSettingsValue(Name, "splitratio4", splitRatio);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewClosing(object args)
		{
			if (args == this)
				SaveSettings();

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewUnDocking(object args)
		{
			if (args == this)
				SaveSettings();

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewDocking(object args)
		{
			if (args == this && IsHandleCreated)
				SaveSettings();

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewDocked(object args)
		{
			if (args == this)
			{
				SetToolTips();

				try
				{
					// These are in a try/catch because sometimes they might throw an exception
					// in rare cases. The exception has to do with a condition in the underlying
					// .Net framework that I haven't been able to make sense of. Anyway, if an
					// exception is thrown, no big deal, the splitter distances will just be set
					// to their default values.

					// Splitter between left and right half of view.
					float splitRatio = App.SettingsHandler.GetFloatSettingsValue(Name, "splitratio1", 0.25f);
					splitOuter.SplitterDistance = (int)(splitOuter.Width * splitRatio);

					// Splitter between experimental transcriptions and ambiguous sequences.
					splitRatio = App.SettingsHandler.GetFloatSettingsValue(Name, "splitratio2", 0.5f);
					splitChanges.SplitterDistance = (int)(splitChanges.Height * splitRatio);

					// Splitter between phone inventory and features.
					splitRatio = App.SettingsHandler.GetFloatSettingsValue(Name, "splitratio3", 0.4f);
					splitInventory.SplitterDistance = (int)(splitInventory.Height * splitRatio);

					// Splitter between articulatory and binary features.
					splitRatio = App.SettingsHandler.GetFloatSettingsValue(Name, "splitratio4", 0.4f);
					splitFeatures.SplitterDistance = (int)(splitFeatures.Height * splitRatio);
				}
				catch { }
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewUndocked(object args)
		{
			if (args == this)
				SetToolTips();

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetToolTips()
		{
			m_phoneToolTip = new ToolTip();
			m_bFeatureToolTip = new ToolTip();

			System.ComponentModel.ComponentResourceManager resources =
				new System.ComponentModel.ComponentResourceManager(GetType());
			
			m_tooltip = new ToolTip(components);
			m_tooltip.SetToolTip(btnADropDownArrow, resources.GetString("btnADropDownArrow.ToolTip"));
			m_tooltip.SetToolTip(btnBDropDownArrow, resources.GetString("btnBDropDownArrow.ToolTip"));
			m_tooltip.SetToolTip(btnApply, resources.GetString("btnApply.ToolTip"));
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the articulatory and binary feature text boxes with the features for the
		/// phone selected in the phone inventory grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void gridPhones_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			UpdatePhonesFeatureText(e.RowIndex);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the articulatory and binary feature text boxes with the features for the
		/// phone in the current grid row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdatePhonesFeatureText()
		{
			txtAFeatures.Text = txtBFeatures.Text = string.Empty;

			if (gridPhones.CurrentCellAddress.Y > 0)
				UpdatePhonesFeatureText(gridPhones.CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the articulatory and binary feature text boxes with the features for the
		/// phone in the specified grid row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdatePhonesFeatureText(int rowIndex)
		{
			txtAFeatures.Text = txtBFeatures.Text = string.Empty;

			if (rowIndex >= 0 && rowIndex < gridPhones.RowCount)
				UpdatePhonesFeatureText(Phones[rowIndex]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the articulatory and binary feature text boxes with the features for the
		/// specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdatePhonesFeatureText(IPhoneInfo phoneInfo)
		{
			txtAFeatures.Text = txtBFeatures.Text = string.Empty;

			if (phoneInfo == null)
				return;

			string features = App.AFeatureCache.GetFeaturesText(phoneInfo.AMask);
			if (!string.IsNullOrEmpty(features))
				txtAFeatures.Text = features.Replace(", ", "\r\n");

			features = App.BFeatureCache.GetFeaturesText(phoneInfo.BMask);
			if (!string.IsNullOrEmpty(features))
				txtBFeatures.Text = features.Replace(", ", "\r\n");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show binary feature's full name when the mouse hovers over it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtBFeatures_MouseMove(object sender, MouseEventArgs e)
		{
			if (txtBFeatures.Text.Length == 0 || txtBFeatures.Lines.Length == 0)
				return;

			int index = txtBFeatures.GetCharIndexFromPosition(e.Location);
			int line = txtBFeatures.GetLineFromCharIndex(index);

			if (line >= 0 && line < txtBFeatures.Lines.Length)
			{
				string shortName = txtBFeatures.Lines[line];

				Feature feature = App.BFeatureCache[shortName];
				if (feature != null && feature.Name.ToLower() != feature.FullName.ToLower())
				{
					if (feature != m_bFeatureToolTip.Tag)
					{
						txtBFeatures.Capture = true;
						Point pt = txtBFeatures.GetPositionFromCharIndex(index);
						m_bFeatureToolTip.Tag = feature;
						m_bFeatureToolTip.Show(shortName.Substring(0, 1) + feature.FullName,
							txtBFeatures, new Point(2, pt.Y));
					}

					return;
				}
			}

			txtBFeatures.Capture = false;
			m_bFeatureToolTip.Hide(txtBFeatures);
			m_bFeatureToolTip.Tag = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnADropDownArrow_Click(object sender, EventArgs e)
		{
			int i = gridPhones.CurrentCellAddress.Y;
			if (Phones == null || i < 0 || i >= Phones.Count)
				return;

			m_lvAFeatures.CurrentMask = Phones[i].AMask.Clone();
			Rectangle rc = hlblAFeatures.DisplayRectangle;
			Point pt = new Point(rc.Left, rc.Bottom);
			pt = pnlAFeatures.PointToScreen(pt);
			m_aFeatureDropdown.Show(pt);
			m_lvAFeatures.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnBDropDownArrow_Click(object sender, EventArgs e)
		{
			int i = gridPhones.CurrentCellAddress.Y;
			if (Phones == null || i < 0 || i >= Phones.Count)
				return;
			
			m_lvBFeatures.CurrentMask = Phones[i].BMask.Clone();
			Rectangle rc = hlblBFeatures.DisplayRectangle;
			Point pt = new Point(rc.Left, rc.Bottom);
			pt = pnlBFeatures.PointToScreen(pt);
			m_bFeatureDropdown.Show(pt);
			m_lvBFeatures.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update a phones feature mask(s) after one of the feature drop-down lists closes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_featureDropdown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			int i = gridPhones.CurrentCellAddress.Y;
			if (Phones == null || i < 0 || i >= Phones.Count)
				return;

			FeatureListView lv = (sender == m_aFeatureDropdown ? m_lvAFeatures : m_lvBFeatures);

			PhoneInfo phoneInfo = Phones[i] as PhoneInfo;
			if (phoneInfo == null)
				return;

			if (lv == m_lvAFeatures)
			{
				if (phoneInfo.AMask == m_lvAFeatures.CurrentMask)
					return;

				phoneInfo.AMask = m_lvAFeatures.CurrentMask.Clone();
			}
			else
			{
				if (phoneInfo.BMask == m_lvBFeatures.CurrentMask)
					return;

				phoneInfo.BMask = m_lvBFeatures.CurrentMask.Clone();
			}

			UpdatePhonesFeatureText(phoneInfo);
			gridPhones.IsDirty = PhoneFeaturesChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show unicode information about the phone being hovered over.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void gridPhones_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.ColumnIndex != 0)
					return;

				PhoneInfo phoneInfo = Phones[e.RowIndex] as PhoneInfo;
				if (phoneInfo == null || phoneInfo.Phone.Trim().Length == 0)
					return;

				Form frm = gridPhones.FindForm();
				if (frm == null)
					return;

				StringBuilder bldr = new StringBuilder();
				foreach (char c in phoneInfo.Phone)
					bldr.AppendFormat("U+{0:X4}, ", (int)c);

				string tip = bldr.ToString();
				tip = string.Format(Properties.Resources.kstidPhoneInventoryPhoneInfo,
					tip.Substring(0, tip.Length - 2));

				tip = Utils.ConvertLiteralNewLines(tip);

				Rectangle rc = gridPhones.GetCellDisplayRectangle(0, e.RowIndex, true);
				Point pt = gridPhones.PointToScreen(new Point(rc.Right - 5, rc.Bottom - 4));
				pt = frm.PointToClient(pt);
				m_phoneToolTip.Tag = rc;
				m_phoneToolTip.Show(tip, gridPhones.FindForm(), pt, 5000);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void gridPhones_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				// Sometimes this event is fired because the mouse is over the tooltip, even
				// though it's tip's point is still within the bounds of the cell. It is only
				// when the mouse location leaves the cell do we want to hide the tooltip.
				Rectangle rc = gridPhones.GetCellDisplayRectangle(0, e.RowIndex, true);
				Point pt = gridPhones.PointToClient(MousePosition);
				if (!rc.Contains(pt))
					m_phoneToolTip.Hide(gridPhones.FindForm());
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Treat the Ctrl+Shift+C as a special copy to the clipboard. When the phone grid
		/// has focus, Ctrl+Shift+C will copy to the clipboard just the phone in the current
		/// phone grid row, not the phone and it's count separated by a tab (which is normal
		/// copy behavior for a phone grid row).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (gridPhones.Focused && gridPhones.CurrentRow != null &&
				(keyData & Keys.C) == Keys.C && (keyData & Keys.Shift) == Keys.Shift &&
				(keyData & Keys.Control) == Keys.Control)
			{
				IPhoneInfo phoneInfo = gridPhones.CurrentRow.Cells[0].Value as IPhoneInfo;
				if (phoneInfo != null)
				{
					Clipboard.SetText(phoneInfo.ToString(), TextDataFormat.UnicodeText);
					return true;
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		#region Methods for saving changes
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the user has changed any of the features
		/// for the phones in the phone inventory list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool PhoneFeaturesChanged
		{
			get
			{
				foreach (var pi in Phones)
				{
					PhoneInfo phoneInfo = pi as PhoneInfo;
					if (phoneInfo == null)
						continue;

					IPhoneInfo origPhoneInfo = App.PhoneCache[phoneInfo.Phone];

					if (origPhoneInfo == null ||
						phoneInfo.AMask != origPhoneInfo.AMask ||
						phoneInfo.BMask != origPhoneInfo.BMask)
					{
						return true;
					}
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnApply_Click(object sender, EventArgs e)
		{
			SaveChanges(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Applies (i.e. saves) changes made in the phone inventory view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveChanges(bool reloadProject)
		{
			bool changed = ApplyPhoneFeatureChanges();

			if (reloadProject && changed)
				App.Project.ReloadDataSources();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves any changes made to phone features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ApplyPhoneFeatureChanges()
		{
			if (!PhoneFeaturesChanged)
			{
				gridPhones.IsDirty = false;
				return false;
			}

			FeatureOverrides featureOverrideList = new FeatureOverrides();

			foreach (var pi in Phones)
			{
				// Get the phone from the grid.
				PhoneInfo phoneInfo = pi as PhoneInfo;
				if (phoneInfo == null)
					continue;

				// Find the grid phone's entry in the application's phone cache. If the
				// features in the grid phone are different from those in the phone
				// cache entry, then add the phone from the grid to our temporary list
				// of phone features to override.
				IPhoneInfo origPhoneInfo = App.PhoneCache[phoneInfo.Phone];
				if (origPhoneInfo == null)
					continue;

				phoneInfo.AFeaturesAreOverridden = (origPhoneInfo.AMask != phoneInfo.AMask);
				phoneInfo.BFeaturesAreOverridden = (origPhoneInfo.BMask != phoneInfo.BMask);

				if (phoneInfo.AFeaturesAreOverridden || phoneInfo.BFeaturesAreOverridden)
					featureOverrideList[phoneInfo.Phone] = phoneInfo;
			}

			featureOverrideList.Save(App.Project.ProjectPathFilePrefix);
			gridPhones.IsDirty = false;
			App.MsgMediator.SendMessage("PhoneFeatureOverridesSaved", featureOverrideList);
			return true;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return the location where water marks should be displayed in the grids.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleGetWaterMarkRect(object sender,
			Rectangle adjClientRect, ref Rectangle rc)
		{
			// Return a rectangle in the bottom right corner of the grid.
			int dxy = (int)(Math.Min(adjClientRect.Width, adjClientRect.Height) / 3.5);
			rc = new Rectangle(adjClientRect.Right - (dxy + 5),
				adjClientRect.Bottom - (dxy + 5), dxy, dxy);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDataSourcesModified(object args)
		{
			LoadPhoneGrid();
			UpdatePhonesFeatureText();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the font(s) gets changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPaFontsChanged(object args)
		{
			hlblAFeatures.Font = FontHelper.UIFont;
			hlblBFeatures.Font = FontHelper.UIFont;
			txtAFeatures.Font = FontHelper.UIFont;
			txtBFeatures.Font = FontHelper.UIFont;
			pgpPhoneList.Font = FontHelper.UIFont;
			gridPhones.Font = FontHelper.UIFont;

			gridPhones.Columns["phone"].DefaultCellStyle.Font = FontHelper.PhoneticFont;
			gridPhones.Columns["phone"].CellTemplate.Style.Font = FontHelper.PhoneticFont;
			foreach (DataGridViewRow row in gridPhones.Rows)
				row.Cells["phone"].Style.Font = FontHelper.PhoneticFont;

			AdjustGridRows(gridPhones, "phonegridextrarowheight", 3);

			// Return false to allow other windows to update their fonts.
			return false;
		}

		#region Update handlers for menus that shouldn't be enabled when this view is current
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExportAsHTML(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExportAsRTF(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlayback(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlaybackRepeatedly(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateStopPlayback(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditSourceRecord(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowCIEResults(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGroupBySortedField(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExpandAllGroups(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateCollapseAllGroups(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowRecordPane(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, XmlNode configurationParameters)
		{
			// Not used in PA.
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] { this };
		}

		#endregion
	}
}