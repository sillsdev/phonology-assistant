using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;
using SIL.FieldWorks.Common.UIAdapters;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FeatureListView : ListView
	{
		public delegate void FeatureChangedHandler(object sender, ulong[] newMasks);
		public event FeatureChangedHandler FeatureChanged;

		public delegate void CustomDoubleClickHandler(object sender, string feature);
		public event CustomDoubleClickHandler CustomDoubleClick;

		private bool m_allowDoubleClickToChangeCheckState = true;
		private bool m_isDirty = false;
		private bool m_ignoreCheckChanges = false;
		private bool m_emphasizeCheckedItems = true;
		private Size m_chkBoxSize = new Size(13, 13);
		private Color m_glyphColor = Color.Black;
		private readonly ulong[] m_currMasks = new ulong[] { 0, 0 };
		private readonly ulong[] m_backupCurrMasks = new ulong[] { 0, 0 };
		private readonly PaApp.FeatureType m_featureType;
		private readonly Font m_checkedItemFont;
		private readonly CustomDropDown m_hostingDropDown;
		private readonly ToolTip m_tooltip;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureListView(PaApp.FeatureType featureType, CustomDropDown hostingDropDown)
			: this(featureType)
		{
			m_hostingDropDown = hostingDropDown;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureListView(PaApp.FeatureType featureType)
		{
			m_featureType = featureType;

			Name = "lvFeatures-" + (m_featureType == PaApp.FeatureType.Binary ?
				"Binary" : "Articulatory");

			ColumnHeader colHdr = new ColumnHeader();
			colHdr.Width = 210;

			if (m_featureType == PaApp.FeatureType.Binary)
				m_tooltip = new ToolTip();

			base.Font = FontHelper.UIFont;
			m_checkedItemFont = FontHelper.MakeFont(base.Font, FontStyle.Bold | FontStyle.Italic);
			CheckBoxes = true;
			Columns.Add(colHdr);
			HeaderStyle = ColumnHeaderStyle.None;
			base.DoubleBuffered = true;
			LabelEdit = true;
			MultiSelect = false;
			HideSelection = false;
			View = View.List;
			OwnerDraw = true;
			Visible = true;
		}

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && m_checkedItemFont != null)
				m_checkedItemFont.Dispose();

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			m_ignoreCheckChanges = true;
			base.OnHandleCreated(e);
			m_ignoreCheckChanges = false;

			if (m_featureType == PaApp.FeatureType.Binary || !Application.RenderWithVisualStyles)
				return;

			VisualStyleRenderer renderer =
				new VisualStyleRenderer(VisualStyleElement.Button.CheckBox.UncheckedNormal);

			m_glyphColor = renderer.GetColor(ColorProperty.BorderColor);

			using (Graphics g = CreateGraphics())
				m_chkBoxSize = renderer.GetPartSize(g, ThemeSizeType.Draw);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show the full names of binary features for those features where the name and full
		/// name are different.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (m_tooltip == null)
				return;

			ListViewHitTestInfo htinfo = HitTest(e.Location);
			if (htinfo.Item != null && htinfo.Item.Tag != null)
			{
				FeatureItemInfo item = htinfo.Item.Tag as FeatureItemInfo;
				if (item != null && item.Name != item.FullName && item.FullName != null)
				{
					if (item != m_tooltip.Tag)
					{
						Rectangle rc = GetItemRect(htinfo.Item.Index, ItemBoundsPortion.Label);
						rc.X += 3;
						m_tooltip.Tag = htinfo.Item.Font;
						m_tooltip.Show(item.FullName, this, rc.Location);
					}

					return;
				}
			}

			m_tooltip.Hide(this);
			m_tooltip.Tag = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (LabelEdit && SelectedItems.Count > 0 && e.KeyCode == Keys.F2)
				SelectedItems[0].BeginEdit();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Assume that when the list is hosted on a drop-down and the user presses Esc, that
		/// they want to abort any changes they made since the last time the CurrentMasks
		/// property was set.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape && m_hostingDropDown != null)
			{
				m_currMasks[0] = m_backupCurrMasks[0];
				m_currMasks[1] = m_backupCurrMasks[1];
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);

			if (e.KeyChar == (char)Keys.Enter && m_hostingDropDown != null)
				m_hostingDropDown.Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnAfterLabelEdit(LabelEditEventArgs e)
		{
			string newName = (e.Label == null ? null : e.Label.Trim());

			if (newName != null)
			{
				if (e.Label.Trim() == string.Empty ||
					DataUtils.AFeatureCache.FeatureExits(newName, true))
				{
					e.CancelEdit = true;
				}
				else
				{
					FeatureItemInfo info = Items[e.Item].Tag as FeatureItemInfo;
					if (info == null)
						e.CancelEdit = true;
					else
					{
						info.Name = newName;
						((AFeature)info.CacheEntry).Name = newName;
						m_isDirty = true;
					}
				}
			}

			base.OnAfterLabelEdit(e);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When it is not desired to have double-click cause the checked state of item's to
		/// change, we need to eat the double-click event here since handling this in the
		/// override to the double-click is too late. By that time, the list resultView has
		/// already done the dirty deed of changing the checked state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void WndProc(ref Message m)
		{
			if (m.HWnd == Handle && m.Msg == 0x203 && !m_allowDoubleClickToChangeCheckState)
			{
				m.Result = IntPtr.Zero;
				m.Msg = 0;

				// Creating a custom delegate for the double-click was easier than trying
				// to figure out how to get delegates to that event via the Events list.
				if (CustomDoubleClick != null && SelectedItems.Count > 0)
					CustomDoubleClick(this, CurrentFormattedFeature);
			}

			base.WndProc(ref m);
		}

		#endregion

		#region Methods for setting feature states
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the feature masks when a feature's checked value changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnItemCheck(ItemCheckEventArgs e)
		{
			// If we're updating the check because the IPA character just changed
			// then take the default behavior.
			if (m_ignoreCheckChanges)
				return;

			FeatureItemInfo info = Items[e.Index].Tag as FeatureItemInfo;

			if (info != null)
			{
				m_currMasks[0] = m_currMasks[1] = 0;

				if (m_featureType == PaApp.FeatureType.Articulatory)
					SetCurrentArticulatoryFeatureMaskInfo(info);
				else
					SetCurrentBinaryFeatureMaskInfo(info);

				Invalidate(Items[e.Index].Bounds);

				if (FeatureChanged != null)
					FeatureChanged(this, m_currMasks);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cycles through the articulatory features, building the masks for those that are
		/// checked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetCurrentArticulatoryFeatureMaskInfo(FeatureItemInfo info)
		{
			// First set the new value of the feature.
			info.Checked = !info.Checked;

			// Now rebuild the current articulatory feature masks.
			foreach (ListViewItem item in Items)
			{
				FeatureItemInfo fi = item.Tag as FeatureItemInfo;
				if (fi != null && fi.Name != null && fi.Checked)
					m_currMasks[fi.MaskNum] |= fi.Mask;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cycles through the binary features, building the masks for those that are plus'd
		/// or minus'd.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetCurrentBinaryFeatureMaskInfo(FeatureItemInfo info)
		{
			// First set the new value of the feature.
			if (info.TriStateValue == BinaryFeatureValue.None)
				info.TriStateValue = BinaryFeatureValue.Plus;
			else if (info.TriStateValue == BinaryFeatureValue.Plus)
				info.TriStateValue = BinaryFeatureValue.Minus;
			else
				info.TriStateValue = BinaryFeatureValue.None;

			// Now rebuild the current binary feature mask.
			foreach (ListViewItem item in Items)
			{
				FeatureItemInfo fi = item.Tag as FeatureItemInfo;
				if (fi != null && fi.Name != null &&
					fi.TriStateValue != BinaryFeatureValue.None)
				{
					m_currMasks[0] |= (fi.TriStateValue == BinaryFeatureValue.Plus ?
						fi.PlusMask : fi.MinusMask);
				}
			}
		}

		#endregion

		#region ListView Item Drawing Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
			e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
			FeatureItemInfo info = e.Item.Tag as FeatureItemInfo;
			DrawText(e, info);
			Rectangle rc = e.Item.GetBounds(ItemBoundsPortion.Entire);
			Rectangle rcChkBox = new Rectangle(new Point(rc.X + 3, rc.Y), m_chkBoxSize);
			rcChkBox.Y += ((rc.Height - rcChkBox.Height) / 2);

			if (CheckBoxes)
			{
				if (m_featureType == PaApp.FeatureType.Articulatory)
					DrawFeatureState(e.Graphics, info, rcChkBox.Location);
				else
					DrawFeatureState(e.Graphics, info, rcChkBox);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws just the text portion of a feature list resultView item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawText(DrawListViewItemEventArgs e, FeatureItemInfo info)
		{
			bool selected = (SelectedItems.Count > 0 && e.Item == SelectedItems[0]);
			Rectangle rc = e.Item.GetBounds(ItemBoundsPortion.Label);

			// Draw the text and its background.
			Font fnt = Font;

			// Determine whether or not to use the emphasized font.
			if (m_emphasizeCheckedItems)
			{
				if (m_featureType == PaApp.FeatureType.Articulatory)
				{
					if (info != null && info.Checked)
						fnt = m_checkedItemFont;
				}
				else
				{
					if (info != null && info.TriStateValue != BinaryFeatureValue.None)
						fnt = m_checkedItemFont;
				}
			}

			rc.Width = TextRenderer.MeasureText(e.Item.Text, fnt).Width + 3;
			Color clrText = SystemColors.WindowText;

			if (selected)
			{
				if (Focused)
				{
					clrText = SystemColors.HighlightText;
					e.Graphics.FillRectangle(SystemBrushes.Highlight, rc);
					ControlPaint.DrawFocusRectangle(e.Graphics, rc);
				}
				else
				{
					clrText = SystemColors.ControlText;
					e.Graphics.FillRectangle(SystemBrushes.Control, rc);
				}
			}

			TextRenderer.DrawText(e.Graphics, e.Item.Text, fnt, rc, clrText,
				TextFormatFlags.VerticalCenter);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw normal checked/unchecked check box (for articulatory features).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawFeatureState(Graphics g, FeatureItemInfo info, Point pt)
		{
			CheckBoxRenderer.DrawCheckBox(g, pt, (info != null && info.Checked ?
				CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the check box with a plus, minus or nothing (for binary features).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawFeatureState(Graphics g, FeatureItemInfo info, Rectangle rc)
		{
			// Draw an empty checkbox.
			CheckBoxRenderer.DrawCheckBox(g, rc.Location, CheckBoxState.UncheckedNormal);

			if (info == null || info.TriStateValue == BinaryFeatureValue.None)
				return;

			// Draw a plus or minus in the empty check box.
			using (Pen pen = new Pen(m_glyphColor, 1))
			{
				Point ptCenter = new Point(rc.X + (rc.Width / 2), rc.Y + (rc.Height / 2));

				// Draw the minus
				g.DrawLine(pen, ptCenter.X - 3, ptCenter.Y, ptCenter.X + 3, ptCenter.Y);

				// Draw the vertical line to make a plus if the feature's value is such.
				if (info.TriStateValue == BinaryFeatureValue.Plus)
					g.DrawLine(pen, ptCenter.X, ptCenter.Y - 3, ptCenter.X, ptCenter.Y + 3);
			}
		}

		#endregion

		#region Methods for adding and removing custom articulatory feature
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a custom articulatory feature to the list of articulatory features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddCustomArticulatoryFeature()
		{
			if (CanCustomFeaturesBeAdded)
			{
				int i = 1;
				string newName = Properties.Resources.kstidDefaultNewCustomFeatureName;
				while (DataUtils.AFeatureCache.FeatureExits(newName, false))
					newName = Properties.Resources.kstidDefaultNewCustomFeatureName + i++;

				// Feature should never come back null since
				// we ensured the new feature would be unique.
				AFeature feature = DataUtils.AFeatureCache.Add(newName, false);
				if (feature == null)
					return;

				FeatureItemInfo info = new FeatureItemInfo();
				info.Name = newName;
				info.Mask = feature.Mask;
				info.MaskNum = feature.MaskNumber;
				info.IsCustom = feature.IsCustomFeature;
				info.CacheEntry = feature;

				// Now add a list resultView item for the new feature and put the user in the edit mode.
				ListViewItem item = new ListViewItem(newName);
				item.Checked = false;
				Items.Add(item);
				item.EnsureVisible();
				item.Selected = true;
				item.Tag = info;
				Application.DoEvents();
				item.BeginEdit();
				m_isDirty = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a custom articulatory feature to the list of articulatory features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveCustomArticulatoryFeature()
		{
			if (!IsCurrentFeatureCustom)
				return;

			ListViewItem item = SelectedItems[0];
			FeatureItemInfo info = item.Tag as FeatureItemInfo;
			if (info == null)
				return;

			string msg = string.Format(Properties.Resources.kstidRemoveFeatureMsg, info.Name);

			// Make sure the user really wants to do this.
			if (STUtils.STMsgBox(msg, MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				DataUtils.AFeatureCache.Delete(info.Name, false);
				int newIndex = item.Index;

				// Remove the item from the list resultView and set the new selected item to
				// the most logical feature near the one deleted.
				Focus();
				item.Remove();
				SelectedItems.Clear();
				SelectedIndices.Add((newIndex < Items.Count ? newIndex : Items.Count - 1));
				m_isDirty = true;
			}
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an array of formatted feature strings, one for each selected feature in the
		/// list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string[] FormattedFeatures
		{
			get
			{
				string fmt = "[{0}]";
				List<string> features = new List<string>();
				foreach (ListViewItem item in Items)
				{
					FeatureItemInfo info = item.Tag as FeatureItemInfo;
					if (info != null)
					{
						if (m_featureType == PaApp.FeatureType.Articulatory && item.Checked)
							features.Add(string.Format(fmt, info.Name.ToLower()));
						else if (info.TriStateValue != BinaryFeatureValue.None)
						{
							features.Add(string.Format(fmt,
								(info.TriStateValue == BinaryFeatureValue.Plus ? "+" : "-") +
								info.Name));
						}
					}
				}

				return (features.Count == 0 ? null : features.ToArray());
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a feature name formatted properly. i.e. when the feature list are for binary
		/// features, then a + or - is tacked on to the front, depending upon the binary state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string CurrentFormattedFeature
		{
			get
			{
				if (SelectedItems.Count > 0)
				{
					ListViewItem item = SelectedItems[0];
					FeatureItemInfo info = item.Tag as FeatureItemInfo;
					if (info != null)
					{
						string feature = "[" + info.Name + "]";

						if (m_featureType == PaApp.FeatureType.Articulatory)
							return feature;

						if (info.TriStateValue != BinaryFeatureValue.None)
						{
							return feature.Insert(1,
								info.TriStateValue == BinaryFeatureValue.Plus ? "+" : "-");
						}
					}
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the current feature masks. When the list is for binary features,
		/// only the first mask of the two is relevant.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ulong[] CurrentMasks
		{
			get {return m_currMasks;}
			set
			{
				m_currMasks[0] = value[0];
				m_currMasks[1] = value[1];
				
				// These are used when the list view is hosted on a drop-down control and the
				// user presses Esc. The original mask values will be restored at that point.
				m_backupCurrMasks[0] = value[0];
				m_backupCurrMasks[1] = value[1];

				// Loop through items in the feature list and set their state according to
				// the specified mask.
				foreach (ListViewItem item in Items)
				{
					FeatureItemInfo info = item.Tag as FeatureItemInfo;
					if (info == null)
						continue;

					if (m_featureType == PaApp.FeatureType.Articulatory)
						info.Checked = ((m_currMasks[info.MaskNum] & info.Mask) != 0);
					else
					{
						if ((m_currMasks[0] & info.PlusMask) != 0)
							info.TriStateValue = BinaryFeatureValue.Plus;
						else if ((m_currMasks[0] & info.MinusMask) != 0)
							info.TriStateValue = BinaryFeatureValue.Minus;
						else
							info.TriStateValue = BinaryFeatureValue.None;
					}
				}

				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a string with all the feature names joined and delimited by a comma and space.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string FeaturesText
		{
			get
			{
				StringBuilder features = new StringBuilder();
				foreach (ListViewItem item in Items)
				{
					FeatureItemInfo info = item.Tag as FeatureItemInfo;
					if (info != null)
					{
						if (m_featureType == PaApp.FeatureType.Articulatory && item.Checked)
							features.Append(info.Name);
						else if (info.TriStateValue != BinaryFeatureValue.None)
						{
							features.Append(
								(info.TriStateValue == BinaryFeatureValue.Plus ? "+" : "-") +
								info.Name);
						}

						features.Append(", ");
					}
				}

				// Remove the last comma and space;
				features.Length -= 2;

				return (features.ToString());
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not double-clicking an item should
		/// change cause an item's value (i.e. check/plus/minus state) to change.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AllowDoubleClickToChangeCheckState
		{
			get { return m_allowDoubleClickToChangeCheckState; }
			set { m_allowDoubleClickToChangeCheckState = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not checked/plus/minus items should
		/// appear emphasized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool EmphasizeCheckedItems
		{
			get { return m_emphasizeCheckedItems; }
			set { m_emphasizeCheckedItems = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the currently selected item in the list is
		/// a custom feature. Note: this property only applies to articulatory features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsCurrentFeatureCustom
		{
			get
			{
				if (m_featureType == PaApp.FeatureType.Binary || SelectedItems.Count == 0)
					return false;

				FeatureItemInfo info = SelectedItems[0].Tag as FeatureItemInfo;
				return (info == null ? false : info.IsCustom);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the maximum number of custom articulatory
		/// features have been added. Note: this property only applies to articulatory
		/// features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CanCustomFeaturesBeAdded
		{
			get
			{
				return (m_featureType == PaApp.FeatureType.Articulatory &&
					DataUtils.AFeatureCache.CanAddCustomFeature);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not a feature name or a feature value in the
		/// list has changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsDirty
		{
			get {return m_isDirty;}
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets or sets the field on which to sort the features list.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		//[Browsable(false)]
		//public string SortField
		//{
		//    get {return m_currSortField;}
		//    set
		//    {
		//        if (value == kFeatureSortField)
		//            SortByFeatureName();
		//        else if (value == kTypeSortField)
		//            SortByType();
		//    }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets a value indicating whether or not the list is currently sorted by feature name.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		//[Browsable(false)]
		//public bool IsSortedByFeatureName
		//{
		//    get {return (m_currSortField == kFeatureSortField);}
		//}
		
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets a value indicating whether or not the list is currently sorted by type.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		//[Browsable(false)]
		//public bool IsSortedByType
		//{
		//    get {return (m_currSortField == kTypeSortField);}
		//}
		#endregion

		#region Misc. public methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears and reloads the list's internal cache of feature information as well as the
		/// list resultView contents.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Reset()
		{
			//m_featureInfo.Clear();
			//m_featureInfo = null;
			Load();
			m_isDirty = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SortByType()
		{
			//m_currSortField = kFeatureSortField;
			//Load();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SortByFeatureName()
		{
			//m_currSortField = kFeatureSortField;
			//Load();
		}

		#endregion

		#region Loading
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fill the feature grid (i.e. list) from the DB.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			m_ignoreCheckChanges = true;
			BeginUpdate();
			Items.Clear();

			if (m_featureType == PaApp.FeatureType.Articulatory)
				LoadAFeatures();
			else
				LoadBFeatures();

			if (Items.Count >= 0)
				SelectedIndices.Add(0);

			EndUpdate();
			m_ignoreCheckChanges = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get all the feature information from the articulatory feature cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadAFeatures()
		{
			foreach (KeyValuePair<string, AFeature> feature in DataUtils.AFeatureCache)
			{
				if (!feature.Value.IsBlank)
				{
					FeatureItemInfo info = new FeatureItemInfo();
					info.Name = feature.Value.Name;
					info.MaskNum = feature.Value.MaskNumber;
					info.Mask = feature.Value.Mask;
					info.IsCustom = feature.Value.IsCustomFeature;
					info.CacheEntry = feature.Value;
					ListViewItem item = new ListViewItem(info.Name);
					item.Tag = info;
					Items.Add(item);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get all the feature information from the binary feature cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadBFeatures()
		{
			foreach (KeyValuePair<string, BFeature> feature in DataUtils.BFeatureCache)
			{
				FeatureItemInfo info = new FeatureItemInfo();
				info.Name = feature.Value.Name;
				info.FullName = feature.Value.FullName;
				info.PlusMask = feature.Value.PlusMask;
				info.MinusMask = feature.Value.MinusMask;
				info.CacheEntry = feature.Value;
				ListViewItem item = new ListViewItem(info.Name);
				item.Tag = info;
				Items.Add(item);
			}
		}
		
		#endregion
	}

	#region Classes to encapsulate feature list resultView items
	/// ------------------------------------------------------------------------------------
	/// <summary>
	/// The values a single feature in the features list resultView may have. This is only
	/// relevant for the DefineBinaryFeatureDlg class.
	/// </summary>
	/// ------------------------------------------------------------------------------------
	internal enum BinaryFeatureValue
	{
		Plus,
		Minus,
		None
	}

	/// ------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ------------------------------------------------------------------------------------
	internal class FeatureItemInfo
	{
		internal string Name;
		internal string FullName;
		internal int MaskNum;
		internal ulong Mask = 0;
		internal ulong MinusMask = 0;
		internal ulong PlusMask = 0;
		internal bool IsCustom = false;
		internal bool Checked = false;
		internal BinaryFeatureValue TriStateValue = BinaryFeatureValue.None;
		internal object CacheEntry;
	}

	#endregion
}
