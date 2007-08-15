using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Controls;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;
using XCore;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class PhoneInventoryWnd : Form, IxCoreColleague, ITabView
	{
		private SizableDropDownPanel m_sddpAFeatures;
		private CustomDropDown m_aFeatureDropdown;
		private FeatureListView m_lvAFeatures;
		private SizableDropDownPanel m_sddpBFeatures;
		private CustomDropDown m_bFeatureDropdown;
		private FeatureListView m_lvBFeatures;
		private ExperimentalTranscriptionControl m_experimentalTransCtrl;
		private ToolTip m_phoneToolTip;
		private ToolTip m_bFeatureToolTip;
		private readonly ITMAdapter m_mainMenuAdapter;
		private readonly ITMAdapter m_tmAdapter;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInventoryWnd()
		{
			if (!PaApp.DesignMode)
			{
				PaApp.InitializeProgressBarForLoadingView(
					Properties.Resources.kstidPhoneInventoryViewText, 9);
			}

			InitializeComponent();

			if (PaApp.DesignMode)
				return;

			m_phoneToolTip = new ToolTip();
			m_bFeatureToolTip = new ToolTip();

			PaApp.IncProgressBar();
			m_mainMenuAdapter = PaApp.LoadDefaultMenu(this);
			m_mainMenuAdapter.AllowUpdates = false;
			m_tmAdapter = AdapterHelper.CreateTMAdapter();
			PaApp.IncProgressBar();

			m_experimentalTransCtrl = new ExperimentalTranscriptionControl();
			m_experimentalTransCtrl.BorderStyle = BorderStyle.None;
			m_experimentalTransCtrl.Dock = DockStyle.Fill;
			m_experimentalTransCtrl.TabIndex = pgpExperimental.TabIndex;
			m_experimentalTransCtrl.Grid.ShowWaterMarkWhenDirty = true;
			m_experimentalTransCtrl.Grid.GetWaterMarkRect += HandleGetWaterMarkRect;
			pnlExperimental.Controls.Add(m_experimentalTransCtrl);
			m_experimentalTransCtrl.BringToFront();
			pgpExperimental.ControlReceivingFocusOnMnemonic = m_experimentalTransCtrl.Grid;
			pgpExperimental.BorderStyle = BorderStyle.None;
			pgpAmbiguous.BorderStyle = BorderStyle.None;
			pgpPhoneList.BorderStyle = BorderStyle.None;

			PaApp.IncProgressBar();
			BuildPhoneGrid();
			PaApp.IncProgressBar();
			LoadPhoneGrid();
			PaApp.IncProgressBar();
			BuildAmbiguousGrid();
			PaApp.IncProgressBar();
			LoadAmbiguousGrid();
			PaApp.IncProgressBar();
			OnPaFontsChanged(null);
			PaApp.IncProgressBar();
			LoadSettings();
			PaApp.IncProgressBar();
			InitializeFeatureDropDowns();
			PaApp.IncProgressBar();
			PaApp.UninitializeProgressBar();

			base.MinimumSize = PaApp.MinimumViewWindowSize;
			Application.Idle += Application_Idle;
			Disposed += PhoneInventoryWnd_Disposed;
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

			if (m_experimentalTransCtrl != null && !m_experimentalTransCtrl.IsDisposed)
			{
				m_experimentalTransCtrl.Dispose();
				m_experimentalTransCtrl = null;
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
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is quite kludgy but it works and all the straight-foward ways to force the
		/// phone list grid to get focus when the view is first built didn't work. No matter
		/// what I tried, the pnlMasterOuter seemed to always have the focus first. I think
		/// it may have something to do with the fact that a view is removed from it's form
		/// and docked in the main form right after its loaded the first time.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void Application_Idle(object sender, EventArgs e)
		{
			Application.Idle -= Application_Idle;
			gridPhones.Focus();
		}

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

			m_lvAFeatures = new FeatureListView(PaApp.FeatureType.Articulatory, m_aFeatureDropdown);
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

			m_lvBFeatures = new FeatureListView(PaApp.FeatureType.Binary, m_bFeatureDropdown);
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

			PaApp.SettingsHandler.LoadGridProperties(gridPhones);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadPhoneGrid()
		{
			gridPhones.Rows.Clear();

			SortedList<string, KeyValuePair<string, IPhoneInfo>> sortedPhones =
				new SortedList<string, KeyValuePair<string, IPhoneInfo>>();

			// Create a sorted list of phones by place of articulation.
			foreach (KeyValuePair<string, IPhoneInfo> phoneInfo in PaApp.PhoneCache)
			{
				if (phoneInfo.Value is PhoneInfo && ((PhoneInfo)phoneInfo.Value).IsUndefined)
					continue;

				KeyValuePair<string, IPhoneInfo> kvpPhoneInfo =
					   new KeyValuePair<string, IPhoneInfo>(phoneInfo.Key, phoneInfo.Value.Clone());

				if (phoneInfo.Key.Trim() != string.Empty)
					sortedPhones[phoneInfo.Value.POAKey] = kvpPhoneInfo;
			}

			if (sortedPhones.Count > 0)
			{
				// Now fill the grid with the sorted list.
				gridPhones.Rows.Add(sortedPhones.Count);

				int i = 0;
				foreach (KeyValuePair<string, IPhoneInfo> phoneInfo in sortedPhones.Values)
				{
					gridPhones.Rows[i].Cells["phone"].Value = phoneInfo.Value;
					gridPhones.Rows[i++].Cells["count"].Value = phoneInfo.Value.TotalCount;
					if (i == 0)
						gridPhones.Rows[0].Selected = true;
				}
			}

			gridPhones.IsDirty = false;
		}

		#endregion

		#region Build and load ambiguous experimentaTransList grid
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildAmbiguousGrid()
		{
			gridAmbiguous.Name = Name + "AmbigGrid";
			gridAmbiguous.AutoGenerateColumns = false;
			gridAmbiguous.AllowUserToAddRows = true;
			gridAmbiguous.AllowUserToDeleteRows = true;
			gridAmbiguous.AllowUserToOrderColumns = false;
			gridAmbiguous.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
			gridAmbiguous.Font = FontHelper.UIFont;

			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("seq");
			col.Width = 90;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			col.HeaderText = Properties.Resources.kstidAmbiguousSeqHdg;
			gridAmbiguous.Columns.Add(col);

			col = SilGrid.CreateCheckBoxColumn("convert");
			col.Width = 75;
			col.HeaderText = Properties.Resources.kstidAmbiguousConvertHdg;
			col.CellTemplate.ValueType = typeof(bool);
			gridAmbiguous.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("base");
			col.Width = 75;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			col.HeaderText = Properties.Resources.kstidAmbiguousBaseCharHdg;
			gridAmbiguous.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("cvpattern");
			col.ReadOnly = true;
			col.Width = 70;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			col.HeaderText = Properties.Resources.kstidAmbiguousCVPatternHdg;
			gridAmbiguous.Columns.Add(col);

			col = SilGrid.CreateCheckBoxColumn("default");
			col.Visible = false;
			gridAmbiguous.Columns.Add(col);

			col = SilGrid.CreateCheckBoxColumn("autodefault");
			col.Visible = false;
			gridAmbiguous.Columns.Add(col);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the ambiguous sequences grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadAmbiguousGrid()
		{
			bool showDefaults = chkShowDefaults.Checked ||
				PaApp.SettingsHandler.GetBoolSettingsValue(Name, "showdefault", true);
			
			int prevRow = gridAmbiguous.CurrentCellAddress.Y;

			gridAmbiguous.Rows.Clear();
			AmbiguousSequences ambigSeqList = DataUtils.IPACharCache.AmbiguousSequences;

			if (ambigSeqList == null || ambigSeqList.Count == 0)
			{
				gridAmbiguous.IsDirty = false;
				return;
			}

			bool hasDefaultSequences = false;
			gridAmbiguous.Rows.Add(ambigSeqList.Count);

			for (int i = 0; i < ambigSeqList.Count; i++)
			{
				gridAmbiguous["seq", i].Value = ambigSeqList[i].Unit;
				gridAmbiguous["convert", i].Value = ambigSeqList[i].Convert;
				gridAmbiguous["base", i].Value = ambigSeqList[i].BaseChar;
				gridAmbiguous["default", i].Value = ambigSeqList[i].IsDefault;
				gridAmbiguous["autodefault", i].Value = ambigSeqList[i].IsAutoGeneratedDefault;

				if (!string.IsNullOrEmpty(ambigSeqList[i].BaseChar))
				{
					gridAmbiguous["cvpattern", i].Value =
						PaApp.PhoneCache.GetCVPattern(ambigSeqList[i].BaseChar);
				}

				if (ambigSeqList[i].IsDefault || ambigSeqList[i].IsAutoGeneratedDefault)
				{
					gridAmbiguous.Rows[i].Cells[0].ReadOnly = true;
					hasDefaultSequences = true;
					if (!showDefaults)
						gridAmbiguous.Rows[i].Visible = false;
				}
			}

			// Select a row if there isn't one currently selected.
			if (gridAmbiguous.CurrentCellAddress.Y < 0 && gridAmbiguous.RowCountLessNewRow > 0 &&
				gridAmbiguous.Rows.GetRowCount(DataGridViewElementStates.Visible) > 0)
			{
				// Check if the previous row is a valid row.
				if (prevRow < 0 || prevRow >= gridAmbiguous.RowCountLessNewRow ||
					!gridAmbiguous.Rows[prevRow].Visible)
				{
					prevRow = gridAmbiguous.FirstDisplayedScrollingRowIndex;
				}

				gridAmbiguous.CurrentCell =	gridAmbiguous[0, prevRow];
			}

			PaApp.SettingsHandler.LoadGridProperties(gridAmbiguous);
			gridAmbiguous.IsDirty = false;
			chkShowDefaults.Enabled = hasDefaultSequences;
		}

		#endregion

		#region Loading/Saving settings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadSettings()
		{
			PaApp.SettingsHandler.LoadFormProperties(this);
			ViewDocked();

			PaApp.SettingsHandler.LoadGridProperties(gridAmbiguous);
			PaApp.SettingsHandler.LoadGridProperties(gridPhones);

			chkShowDefaults.Checked = (chkShowDefaults.Enabled &&
				PaApp.SettingsHandler.GetBoolSettingsValue(Name, "showdefault", true));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up a bit.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			splitOuter.Dispose();
			base.OnFormClosed(e);
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
			if (PhoneFeaturesChanged || AmbiguousSequencesChanged || m_experimentalTransCtrl.Grid.IsDirty)
			{
				string msg = Properties.Resources.kstidUnAppliedPhoneInventoryChangesMsg;
				DialogResult rslt = STUtils.STMsgBox(msg, MessageBoxButtons.YesNoCancel);
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
		public Control DockableContainer
		{
			get { return pnlMasterOuter; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves some misc. settings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveSettings()
		{
			// Save the value of the show default ambiguous sequences check box.
			if (chkShowDefaults.Enabled)
				PaApp.SettingsHandler.SaveSettingsValue(Name, "showdefault", chkShowDefaults.Checked);

			PaApp.SettingsHandler.SaveFormProperties(this);
			PaApp.SettingsHandler.SaveGridProperties(gridAmbiguous);
			PaApp.SettingsHandler.SaveGridProperties(gridPhones);
			
			float splitRatio = splitOuter.SplitterDistance / (float)splitOuter.Width;
			PaApp.SettingsHandler.SaveSettingsValue(Name, "splitratio1", splitRatio);

			splitRatio = splitChanges.SplitterDistance / (float)splitChanges.Height;
			PaApp.SettingsHandler.SaveSettingsValue(Name, "splitratio2", splitRatio);

			splitRatio = splitInventory.SplitterDistance / (float)splitInventory.Height;
			PaApp.SettingsHandler.SaveSettingsValue(Name, "splitratio3", splitRatio);

			splitRatio = splitFeatures.SplitterDistance / (float)splitFeatures.Height;
			PaApp.SettingsHandler.SaveSettingsValue(Name, "splitratio4", splitRatio);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ViewUndocking()
		{
			m_mainMenuAdapter.AllowUpdates = true;
			SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ViewUndocked()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ViewDocking()
		{
			SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ViewDocked()
		{
			try
			{
				// These are in a try/catch because sometimes they might throw an exception
				// in rare cases. The exception has to do with a condition in the underlying
				// .Net framework that I haven't been able to make sense of. Anyway, if an
				// exception is thrown, no big deal, the splitter distances will just be set
				// to their default values.

				// Splitter between left and right half of view.
				float splitRatio = PaApp.SettingsHandler.GetFloatSettingsValue(Name, "splitratio1", 0.25f);
				splitOuter.SplitterDistance = (int)(splitOuter.Width * splitRatio);

				// Splitter between experimental transcriptions and ambiguous sequences.
				splitRatio = PaApp.SettingsHandler.GetFloatSettingsValue(Name, "splitratio2", 0.5f);
				splitChanges.SplitterDistance = (int)(splitChanges.Height * splitRatio);

				// Splitter between phone inventory and features.
				splitRatio = PaApp.SettingsHandler.GetFloatSettingsValue(Name, "splitratio3", 0.4f);
				splitInventory.SplitterDistance = (int)(splitInventory.Height * splitRatio);

				// Splitter between articulatory and binary features.
				splitRatio = PaApp.SettingsHandler.GetFloatSettingsValue(Name, "splitratio4", 0.4f);
				splitFeatures.SplitterDistance = (int)(splitFeatures.Height * splitRatio);
			}
			catch { }

			m_mainMenuAdapter.AllowUpdates = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ViewActivatedWhileDocked()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the status bar.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public StatusStrip StatusBar
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the status bar label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolStripStatusLabel StatusBarLabel
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the progress bar.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolStripProgressBar ProgressBar
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the progress bar's label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolStripStatusLabel ProgressBarLabel
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the view's tooltip control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolTip ViewsToolTip
		{
			get { return m_tooltip; }
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
		/// Gets called any time any view is about to be opened, docked or undocked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewChangingStatus(object args)
		{
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets called any time any view is finished being opened, docked or undocked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnEndViewChangingStatus(object args)
		{
			return false;
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

			if (gridPhones.SelectedRows.Count > 0)
				UpdatePhonesFeatureText(gridPhones.SelectedRows[0].Index);
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
				UpdatePhonesFeatureText(gridPhones["phone", rowIndex].Value as PhoneInfo);
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

			string features = DataUtils.AFeatureCache.GetFeaturesText(phoneInfo.Masks);
			if (!string.IsNullOrEmpty(features))
				txtAFeatures.Text = features.Replace(", ", "\r\n");

			features = DataUtils.BFeatureCache.GetFeaturesText(phoneInfo.BinaryMask);
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

				BFeature feature = DataUtils.BFeatureCache[shortName];
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
			if (gridPhones.CurrentRow == null)
				return;

			PhoneInfo phoneInfo = gridPhones.CurrentRow.Cells["phone"].Value as PhoneInfo;
			if (phoneInfo != null)
			{
				m_lvAFeatures.CurrentMasks = phoneInfo.Masks;
				Rectangle rc = hlblAFeatures.DisplayRectangle;
				Point pt = new Point(rc.Left, rc.Bottom);
				pt = pnlAFeatures.PointToScreen(pt);
				m_aFeatureDropdown.Show(pt);
				m_lvAFeatures.Focus();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnBDropDownArrow_Click(object sender, EventArgs e)
		{
			if (gridPhones.CurrentRow == null)
				return;

			PhoneInfo phoneInfo = gridPhones.CurrentRow.Cells["phone"].Value as PhoneInfo;
			if (phoneInfo != null)
			{
				m_lvBFeatures.CurrentMasks = new ulong[] { phoneInfo.BinaryMask, 0 };
				Rectangle rc = hlblBFeatures.DisplayRectangle;
				Point pt = new Point(rc.Left, rc.Bottom);
				pt = pnlBFeatures.PointToScreen(pt);
				m_bFeatureDropdown.Show(pt);
				m_lvBFeatures.Focus();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update a phones feature mask(s) after one of the feature drop-down lists closes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_featureDropdown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (gridPhones.CurrentRow == null)
				return;

			FeatureListView lv = (sender == m_aFeatureDropdown ? m_lvAFeatures : m_lvBFeatures);

			PhoneInfo phoneInfo = gridPhones.CurrentRow.Cells["phone"].Value as PhoneInfo;
			if (phoneInfo == null)
				return;

			if (lv == m_lvAFeatures)
			{
				if (phoneInfo.Masks[0] == m_lvAFeatures.CurrentMasks[0] &&
					phoneInfo.Masks[1] == m_lvAFeatures.CurrentMasks[1])
				{
					return;
				}

				phoneInfo.Masks = m_lvAFeatures.CurrentMasks;
			}
			else
			{
				if (phoneInfo.BinaryMask == m_lvBFeatures.CurrentMasks[0])
					return;

				phoneInfo.BinaryMask = m_lvBFeatures.CurrentMasks[0];
			}

			UpdatePhonesFeatureText(phoneInfo);
			gridPhones.IsDirty = PhoneFeaturesChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure new rows get proper default values
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void gridAmbiguous_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			e.Row.Cells["seq"].Value = string.Empty;
			e.Row.Cells["convert"].Value = true;
			e.Row.Cells["default"].Value = false;
			e.Row.Cells["autodefault"].Value = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Validate the edited base character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void gridAmbiguous_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			if (e.RowIndex == gridAmbiguous.NewRowIndex)
				return;

			if (e.ColumnIndex == 0)
				e.Cancel = ValidateSequence(e.RowIndex, e.FormattedValue as string);
			else if (e.ColumnIndex == 2)
				e.Cancel = ValidateBaseCharacter(e.RowIndex, e.FormattedValue as string);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns whether or not the row edit should be cancelled due to a duplicate
		/// sequence.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ValidateSequence(int row, string newSeq)
		{
			// Make sure a unit was specified.
			//if (string.IsNullOrEmpty(newUnit))
			//    msg = Properties.Resources.kstidAmbiguousBaseCharMissingMsg;

			for (int i = 0; i < gridAmbiguous.NewRowIndex; i++)
			{
				if (i != row && gridAmbiguous[0, i].Value as string == newSeq)
				{
					bool isDefault = ((bool)gridAmbiguous["default", row].Value ||
						(bool)gridAmbiguous["autodefault", row].Value);

					string msg = (isDefault ?
						Properties.Resources.kstidAmbiguousSeqDuplicateMsg2 :
						Properties.Resources.kstidAmbiguousSeqDuplicateMsg1);

					STUtils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return true;
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns whether or not the row edit should be cancelled due to an invalid
		/// base character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ValidateBaseCharacter(int row, string newBaseChar)
		{
			string msg = null;

			// Make sure a base character was specified.
			if (string.IsNullOrEmpty(newBaseChar))
				msg = Properties.Resources.kstidAmbiguousBaseCharMissingMsg;

			string phone = gridAmbiguous["seq", row].Value as string;

			if (msg == null)
			{
				// Make sure there is an ambiguous sequence before specifying a base character.
				if (string.IsNullOrEmpty(phone))
					msg = Properties.Resources.kstidAmbiguousTransMissingMsg;
			}

			// Make sure the new base character is part of the ambiguous sequence.
			if (msg == null && phone != null && !phone.Contains(newBaseChar))
				msg = Properties.Resources.kstidAmbiguousBaseCharNotInTransMsg;

			if (msg != null)
			{
				STUtils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return true;
			}

			return false;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void gridAmbiguous_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			// When the base character was edited then automatically determine
			// the C or V type of the ambiguous sequence.
			if (e.ColumnIndex == 2)
			{
				string newBaseChar = gridAmbiguous["base", e.RowIndex].Value as string;
				gridAmbiguous["cvpattern", e.RowIndex].Value =
					PaApp.PhoneCache.GetCVPattern(newBaseChar);
			}
			else if (e.ColumnIndex == 0)
			{
				// Get the edited ambiguous sequence.
				string phone = gridAmbiguous["seq", e.RowIndex].Value as string;
				if (phone != null)
					phone = phone.Trim();

				if (string.IsNullOrEmpty(phone))
				{
					PaApp.MsgMediator.PostMessage("RemoveAmbiguousSeqRow", e.RowIndex);
					return;
				}

				PhoneInfo phoneInfo = new PhoneInfo(phone);

				string prevBaseChar = gridAmbiguous["base", e.RowIndex].Value as string;
				if (prevBaseChar == null || !phone.Contains(prevBaseChar))
				{
					string newBaseChar = phoneInfo.BaseCharacter.ToString();
					gridAmbiguous["base", e.RowIndex].Value = newBaseChar;
					gridAmbiguous["cvpattern", e.RowIndex].Value =
						PaApp.PhoneCache.GetCVPattern(newBaseChar);
				}
			}

			gridAmbiguous.IsDirty = AmbiguousSequencesChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message is called when a user edits away the ambiguous transcription. It is
		/// posted in the after cell edit event because rows cannot be removed in the after
		/// cell edit event handler.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveAmbiguousSeqRow(object args)
		{
			if (args.GetType() == typeof(int))
			{
				int rowIndex = (int)args;
				if (rowIndex >= 0 && rowIndex < gridAmbiguous.RowCountLessNewRow)
				{
					gridAmbiguous.Rows.RemoveAt(rowIndex--);
					if (rowIndex >= 0 && rowIndex < gridAmbiguous.RowCountLessNewRow)
						gridAmbiguous.Rows[rowIndex].Selected = true;
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Don't allow deleting default sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void gridAmbiguous_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			if (e.Row != null &&
				(e.Row.Cells["default"].Value != null && (bool)e.Row.Cells["default"].Value) ||
				(e.Row.Cells["autodefault"].Value != null && (bool)e.Row.Cells["autodefault"].Value))
			{	
				STUtils.STMsgBox(Properties.Resources.kstidAmbiguousSeqCantDeleteDefaultMsg,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				e.Cancel = true;
			}
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

				PhoneInfo phoneInfo = gridPhones[0, e.RowIndex].Value as PhoneInfo;
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

				tip = STUtils.ConvertLiteralNewLines(tip);

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
				foreach (DataGridViewRow row in gridPhones.Rows)
				{
					PhoneInfo phoneInfo = row.Cells["phone"].Value as PhoneInfo;
					if (phoneInfo == null)
						continue;

					IPhoneInfo cachePhoneInfo = PaApp.PhoneCache[phoneInfo.Phone];

					if (cachePhoneInfo == null ||
						phoneInfo.BinaryMask != cachePhoneInfo.BinaryMask ||
						phoneInfo.Masks[0] != cachePhoneInfo.Masks[0] ||
						phoneInfo.Masks[1] != cachePhoneInfo.Masks[1])
					{
						return true;
					}
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not there are any changes to the ambiguous
		/// sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool AmbiguousSequencesChanged
		{
			get
			{
				if (DataUtils.IPACharCache.AmbiguousSequences == null &&
					gridAmbiguous.RowCountLessNewRow > 0)
				{
					return true;
				}

				int i = 0;

				// Go through the ambiguous sequences in the grid and check them against
				// those found in the project's list of ambiguous sequences.
				foreach (DataGridViewRow row in gridAmbiguous.Rows)
				{
					if (row.Index == gridAmbiguous.NewRowIndex)
						continue;

					i++;
					string seq = row.Cells["seq"].Value as string;
					string baseChar = row.Cells["base"].Value as string;
					bool convert = (bool)row.Cells["convert"].Value;

					AmbiguousSeq ambigSeq =
						DataUtils.IPACharCache.AmbiguousSequences.GetAmbiguousSeq(seq, false);

					if (ambigSeq == null || ambigSeq.Convert != convert || ambigSeq.BaseChar != baseChar)
						return true;
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

			if (ApplyAmbiguousSequencesChanges())
				changed = true;

			if (m_experimentalTransCtrl.Grid.IsDirty)
			{
				m_experimentalTransCtrl.SaveChanges();
				changed = true;
			}

			if (reloadProject && changed)
				PaApp.Project.ReloadDataSources();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save any changes made to ambiguous sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ApplyAmbiguousSequencesChanges()
		{
			if (!AmbiguousSequencesChanged)
			{
				gridAmbiguous.IsDirty = false;
				return false;
			}

			AmbiguousSequences ambigSeqList = new AmbiguousSequences();

			foreach (DataGridViewRow row in gridAmbiguous.Rows)
			{
				if (row.Index != gridAmbiguous.NewRowIndex)
				{
					string phone = row.Cells["seq"].Value as string;
					string basechar = row.Cells["base"].Value as string;

					// Don't bother saving anything if there isn't
					// a phone (sequence) or base character.
					if (phone != null && phone.Trim().Length > 0 &&
						basechar != null && basechar.Trim().Length > 0)
					{
						AmbiguousSeq seq = new AmbiguousSeq(phone.Trim());
						seq.BaseChar = basechar.Trim(); ;
						seq.Convert = (row.Cells["convert"].Value == null ?
							false : (bool)row.Cells["convert"].Value);

						seq.IsDefault = (bool)row.Cells["default"].Value;
						seq.IsAutoGeneratedDefault = (bool)row.Cells["autodefault"].Value;
						ambigSeqList.Add(seq);
					}
				}
			}

			ambigSeqList.Save(PaApp.Project.ProjectPathFilePrefix);
			DataUtils.IPACharCache.AmbiguousSequences =
				AmbiguousSequences.Load(PaApp.Project.ProjectPathFilePrefix);

			LoadAmbiguousGrid();
			return true;
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

			PhoneFeatureOverrides featureOverrideList = new PhoneFeatureOverrides();

			foreach (DataGridViewRow row in gridPhones.Rows)
			{
				// Get the phone from the grid.
				PhoneInfo phoneInfo = row.Cells["phone"].Value as PhoneInfo;
				if (phoneInfo != null)
				{
					// Find the grid phone's entry in the application's phone cache. If the
					// features in the grid phone are different from those in the phone
					// cache entry, then add the phone from the grid to our temporary list
					// of phone features to override.
					IPhoneInfo phoneCacheEntry = PaApp.PhoneCache[phoneInfo.Phone];
					if (phoneCacheEntry != null &&
						(phoneCacheEntry.Masks[0] != phoneInfo.Masks[0] ||
						phoneCacheEntry.Masks[1] != phoneInfo.Masks[1] ||
						phoneCacheEntry.BinaryMask != phoneInfo.BinaryMask))
					{
						featureOverrideList[phoneInfo.Phone] = phoneInfo;
					}
				}
			}

			featureOverrideList.Save(PaApp.Project.ProjectPathFilePrefix);
			gridPhones.IsDirty = false;
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
		/// Change the visible state of the default ambiguous sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void chkShowDefaults_CheckedChanged(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in gridAmbiguous.Rows)
			{
				if (row.Index == gridAmbiguous.NewRowIndex)
					continue;

				if ((bool)row.Cells["default"].Value || (bool)row.Cells["autodefault"].Value)
					row.Visible = chkShowDefaults.Checked;
			}

			if (chkShowDefaults.Checked)
				return;

			int currRow = gridAmbiguous.CurrentCellAddress.Y;
			if (currRow < 0 || !gridAmbiguous.Rows[currRow].Visible)
			{
				foreach (DataGridViewRow row in gridAmbiguous.Rows)
				{
					if (row.Visible)
					{
						row.Selected = true;
						return;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For some reason the anchoring of the appply button doesn't work when the master
		/// panel is docked and undocked. Therefore, this code will make sure the button
		/// gets placed in it's correct place when the size of the master panel changes and
		/// when the apply button isn't already in its correct location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlMasterOuter_SizeChanged(object sender, EventArgs e)
		{
			// Get the X screen coordinate where the button should be.
			int x = pnlMasterOuter.PointToScreen(new Point(pnlMasterOuter.Width -
				pnlMasterOuter.Padding.Right - btnApply.Width, 0)).X;

			// Get the Y screen coordinates for the top and bottom of the gap into which
			// the apply button should be located. This gap is vertically larger than the
			// height of the apply button and the apply button will be vertically centered
			// within the gap.
			int y1 = pnlAmbiguous.PointToScreen(new Point(0, pnlAmbiguous.Height)).Y;
			int y2 = pnlMasterOuter.PointToScreen(new Point(0, pnlMasterOuter.Height)).Y;

			// Calculate the y screen coordinate where the button should be.
			int y = y1 + ((y2 - y1 - btnApply.Height) / 2);

			// Convert the screen coordinate to client coordinates and compare it with
			// the button's current location.
			Point pt = pnlMasterOuter.PointToClient(new Point(x, y));

			if (pt != btnApply.Location)
				btnApply.Location = pt;
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
			LoadAmbiguousGrid();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the font(s) get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPaFontsChanged(object args)
		{
			hlblAFeatures.Font = FontHelper.UIFont;
			hlblBFeatures.Font = FontHelper.UIFont;
			txtAFeatures.Font = FontHelper.UIFont;
			txtBFeatures.Font = FontHelper.UIFont;
			pnlAmbiguous.Font = FontHelper.UIFont;
			chkShowDefaults.Font = FontHelper.UIFont;
			pgpAmbiguous.Font = FontHelper.UIFont;
			pgpExperimental.Font = FontHelper.UIFont;
			pgpPhoneList.Font = FontHelper.UIFont;

			int y = (pgpAmbiguous.Height - chkShowDefaults.Height) / 2 + 1;
			chkShowDefaults.Location = new Point(pgpAmbiguous.Width - chkShowDefaults.Width - 3, y);

			gridPhones.Columns["phone"].DefaultCellStyle.Font = FontHelper.PhoneticFont;
			gridPhones.Columns["phone"].CellTemplate.Style.Font = FontHelper.PhoneticFont;
			gridAmbiguous.Columns["seq"].DefaultCellStyle.Font = FontHelper.PhoneticFont;
			gridAmbiguous.Columns["seq"].CellTemplate.Style.Font = FontHelper.PhoneticFont;
			gridAmbiguous.Columns["base"].DefaultCellStyle.Font = FontHelper.PhoneticFont;
			gridAmbiguous.Columns["base"].CellTemplate.Style.Font = FontHelper.PhoneticFont;
			gridAmbiguous.Columns["cvpattern"].DefaultCellStyle.Font = FontHelper.PhoneticFont;
			gridAmbiguous.Columns["cvpattern"].CellTemplate.Style.Font = FontHelper.PhoneticFont;

			// Return false to allow other windows to update their fonts.
			return false;
		}

		#region Update handlers for menus that shouldn't be enabled when this view is current
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlayback(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlaybackRepeatedly(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateStopPlayback(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditSourceRecord(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowCIEResults(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGroupBySortedField(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExpandAllGroups(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateCollapseAllGroups(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowRecordPane(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
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