using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SIL.Pa.Model;
using SilTools;
using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public class FeatureListView : ListView
	{
		private const int kMaxColWidth = 210;

		public delegate void FeatureChangedHandler(object sender, FeatureMask newMask);
		public event FeatureChangedHandler FeatureChanged;

		public delegate void CustomDoubleClickHandler(object sender, string feature);
		public event CustomDoubleClickHandler CustomDoubleClick;

		private bool m_ignoreCheckChanges;
		private Size m_chkBoxSize = new Size(13, 13);
		private Color m_glyphColor = Color.Black;
		private FeatureMask m_currMask;
		private FeatureMask m_backupCurrMask;
		private readonly App.FeatureType m_featureType;
		private readonly Font m_checkedItemFont;
		private readonly CustomDropDown m_hostingDropDown;
		private readonly ToolTip m_tooltip;

		/// ------------------------------------------------------------------------------------
		public FeatureListView(App.FeatureType featureType, CustomDropDown hostingDropDown)
			: this(featureType)
		{
			m_hostingDropDown = hostingDropDown;
		}
		
		/// ------------------------------------------------------------------------------------
		public FeatureListView(App.FeatureType featureType)
		{
			AllowDoubleClickToChangeCheckState = true;
			EmphasizeCheckedItems = true;
			m_featureType = featureType;

			Name = "lvFeatures-" + (m_featureType == App.FeatureType.Binary ?
				"Binary" : "Articulatory");

			var colHdr = new ColumnHeader();
			colHdr.Width = kMaxColWidth;

			m_tooltip = new ToolTip();

			base.Font = FontHelper.UIFont;
			m_checkedItemFont = FontHelper.MakeFont(base.Font, FontStyle.Bold | FontStyle.Italic);
			CheckBoxes = true;
			Columns.Add(colHdr);
			HeaderStyle = ColumnHeaderStyle.None;
			base.DoubleBuffered = true;
			LabelEdit = false;
			MultiSelect = false;
			HideSelection = false;
			View = View.List;
			OwnerDraw = true;
			Visible = true;
		}

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && m_checkedItemFont != null)
				m_checkedItemFont.Dispose();

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			m_ignoreCheckChanges = true;
			base.OnHandleCreated(e);
			m_ignoreCheckChanges = false;

			if (m_featureType == App.FeatureType.Binary || !Application.RenderWithVisualStyles)
				return;

			var renderer = new VisualStyleRenderer(VisualStyleElement.Button.CheckBox.UncheckedNormal);
			m_glyphColor = renderer.GetColor(ColorProperty.BorderColor);

			using (var g = CreateGraphics())
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

			var htinfo = HitTest(e.Location);
			if (htinfo.Item != null && htinfo.Item.Tag != null)
			{
				var item = htinfo.Item.Tag as FeatureItemInfo;
				if (item != null && item.FullName != null &&
					item.Name.ToLower() != item.FullName.ToLower())
				{
					if (htinfo.Item != m_tooltip.Tag)
					{
						ErasePreviousUnderline();
						Point pt = PointToClient(MousePosition);
						pt.Y += (int)(Cursor.Current.Size.Height * 0.7);
						m_tooltip.Tag = htinfo.Item;
						m_tooltip.Show(item.FullName, this, pt);
						Invalidate(GetItemRect(htinfo.Item.Index, ItemBoundsPortion.Label));
					}

					return;
				}
			}

			m_tooltip.Hide(this);
			ErasePreviousUnderline();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			if (m_tooltip != null)
			{
				m_tooltip.Hide(this);
				ErasePreviousUnderline();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ErasePreviousUnderline()
		{
			if (m_tooltip == null)
				return;

			var item = m_tooltip.Tag as ListViewItem;
			m_tooltip.Tag = null;

			if (item != null)
			{
				// Erase the previous 
				var rc = GetItemRect(item.Index, ItemBoundsPortion.Label);
				Invalidate(rc);
			}
		}

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
				m_currMask = m_backupCurrMask.Clone();

			return base.ProcessCmdKey(ref msg, keyData);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);

			if (e.KeyChar == (char)Keys.Enter && m_hostingDropDown != null)
				m_hostingDropDown.Close();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnAfterLabelEdit(LabelEditEventArgs e)
		{
			string newName = (e.Label == null ? null : e.Label.Trim());

			if (newName != null)
			{
				if (e.Label.Trim() == string.Empty ||
					App.AFeatureCache.FeatureExits(newName, true))
				{
					e.CancelEdit = true;
				}
				else
				{
					var info = Items[e.Item].Tag as FeatureItemInfo;
					if (info == null)
						e.CancelEdit = true;
					else
					{
						info.Name = newName;
						((Feature)info.CacheEntry).Name = newName;
						IsDirty = true;
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
			if (m.HWnd == Handle && m.Msg == 0x203 && !AllowDoubleClickToChangeCheckState)
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

			var info = Items[e.Index].Tag as FeatureItemInfo;

			if (info != null)
			{
				if (m_featureType == App.FeatureType.Articulatory)
					SetCurrentArticulatoryFeatureMaskInfo(info);
				else
					SetCurrentBinaryFeatureMaskInfo(info);

				Invalidate(Items[e.Index].Bounds);

				if (FeatureChanged != null)
					FeatureChanged(this, m_currMask);
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
			info.Checked = !info.Checked;
			m_currMask[info.Bit] = info.Checked;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the current mask based on the state of the feature's list view item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetCurrentBinaryFeatureMaskInfo(FeatureItemInfo info)
		{
			switch (info.TriStateValue)
			{
				case BinaryFeatureValue.None:
					info.TriStateValue = BinaryFeatureValue.Plus;
					m_currMask[info.MinusBit] = false;
					m_currMask[info.PlusBit] = true;
					break;
				
				case BinaryFeatureValue.Plus:
					info.TriStateValue = BinaryFeatureValue.Minus;
					m_currMask[info.MinusBit] = true;
					m_currMask[info.PlusBit] = false;
					break;
				
				default:
					info.TriStateValue = BinaryFeatureValue.None;
					m_currMask[info.MinusBit] = false;
					m_currMask[info.PlusBit] = false;
					break;
			}
		}

		#endregion

		#region ListView Item Drawing Methods
		/// ------------------------------------------------------------------------------------
		protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
			e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
			var info = e.Item.Tag as FeatureItemInfo;
			DrawText(e, info);
			var rc = e.Item.GetBounds(ItemBoundsPortion.Entire);
			var rcChkBox = new Rectangle(new Point(rc.X + 3, rc.Y), m_chkBoxSize);
			rcChkBox.Y += ((rc.Height - rcChkBox.Height) / 2);

			if (CheckBoxes)
			{
				if (m_featureType == App.FeatureType.Articulatory)
					DrawFeatureState(e.Graphics, info, rcChkBox.Location);
				else
					DrawFeatureState(e.Graphics, info, rcChkBox);
			}

			if (m_tooltip == null || m_tooltip.Tag != e.Item)
				return;
			
			// Draw underline if the current item has a tooltip showing the feature's full name.
			int width = TextRenderer.MeasureText(e.Item.Text, FontHelper.UIFont).Width;
			rc = e.Item.GetBounds(ItemBoundsPortion.Label);
			using (Pen pen = (Pen)SystemPens.WindowText.Clone())
			{
				pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
				e.Graphics.DrawLine(pen, rc.X + 2, rc.Bottom - 2, rc.X + width, rc.Bottom - 2);
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
			var rc = e.Item.GetBounds(ItemBoundsPortion.Label);

			// Draw the text and its background.
			var fnt = Font;

			// Determine whether or not to use the emphasized font.
			if (EmphasizeCheckedItems)
			{
				if (m_featureType == App.FeatureType.Articulatory)
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
			var clrText = SystemColors.WindowText;

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
				TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw normal checked/unchecked check box (for articulatory features).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void DrawFeatureState(Graphics g, FeatureItemInfo info, Point pt)
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
				var ptCenter = new Point(rc.X + (rc.Width / 2), rc.Y + (rc.Height / 2));

				// Draw the minus
				g.DrawLine(pen, ptCenter.X - 3, ptCenter.Y, ptCenter.X + 3, ptCenter.Y);

				// Draw the vertical line to make a plus if the feature's value is such.
				if (info.TriStateValue == BinaryFeatureValue.Plus)
					g.DrawLine(pen, ptCenter.X, ptCenter.Y - 3, ptCenter.X, ptCenter.Y + 3);
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
				const string fmt = "[{0}]";
				var features = new List<string>();
				foreach (var info in (from ListViewItem item in Items select item.Tag).OfType<FeatureItemInfo>())
				{
					if (m_featureType == App.FeatureType.Articulatory && info.Checked)
						features.Add(string.Format(fmt, info.Name.ToLower()));
					else if (info.TriStateValue != BinaryFeatureValue.None)
					{
						features.Add(string.Format(fmt,
						    (info.TriStateValue == BinaryFeatureValue.Plus ? "+" : "-") +
						    info.Name));
					}
				}

				return (features.Count == 0 ? null : features.ToArray());
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a single string containing a comma delimited list of the currently
		/// checked features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string FormattedFeaturesString
		{
			get
			{
				var bldr = new StringBuilder();
	
				foreach (ListViewItem item in Items)
				{
					var info = item.Tag as FeatureItemInfo;
					if (info == null)
						continue;
					
					if (m_featureType == App.FeatureType.Articulatory && !info.Checked)
						continue;

					if (info.TriStateValue != BinaryFeatureValue.None)
						bldr.Append(info.TriStateValue == BinaryFeatureValue.Plus ? "+" : "-");

					bldr.Append(info.Name);
					bldr.Append(", ");
				}

				return bldr.ToString().TrimEnd(',', ' ');
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
					var item = SelectedItems[0];
					var info = item.Tag as FeatureItemInfo;
					if (info != null)
					{
						string feature = "[" + info.Name + "]";

						if (m_featureType == App.FeatureType.Articulatory)
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
		public FeatureMask CurrentMask
		{
			get {return m_currMask;}
			set
			{
				m_backupCurrMask = m_currMask;
				m_currMask = value;

				// Loop through items in the feature list and set their state according to
				// the specified mask.
				foreach (var info in (from ListViewItem item in Items select item.Tag).OfType<FeatureItemInfo>())
				{
					if (m_featureType == App.FeatureType.Articulatory)
						info.Checked = (m_currMask[info.Bit]);
					else
					{
						if (m_currMask[info.PlusBit])
							info.TriStateValue = BinaryFeatureValue.Plus;
						else if (m_currMask[info.MinusBit])
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
				var features = new StringBuilder();
				foreach (ListViewItem item in Items)
				{
					var info = item.Tag as FeatureItemInfo;
					if (info == null)
						continue;
					
					if (m_featureType == App.FeatureType.Articulatory && item.Checked)
						features.Append(info.Name);
					else if (info.TriStateValue != BinaryFeatureValue.None)
						features.Append((info.TriStateValue == BinaryFeatureValue.Plus ? "+" : "-") + info.Name);

					features.Append(", ");
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
		public bool AllowDoubleClickToChangeCheckState { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not checked/plus/minus items should
		/// appear emphasized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool EmphasizeCheckedItems { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not a feature name or a feature value in the
		/// list has changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public bool IsDirty { get; private set; }

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
			Load();
			IsDirty = false;
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

			if (m_featureType == App.FeatureType.Articulatory)
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
			foreach (var feature in App.AFeatureCache.Values.OrderBy(x => x.Name))
			{
				var info = new FeatureItemInfo();
				info.Name = feature.Name;
				info.FullName = feature.FullName;
				info.Bit = feature.Bit;
				info.CacheEntry = feature;
				var item = new ListViewItem(info.Name);
				item.Tag = info;
				Items.Add(item);
			}

			CurrentMask = App.AFeatureCache.GetEmptyMask();
			AdjustColumnWidth();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get all the feature information from the binary feature cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadBFeatures()
		{
			foreach (Feature feature in App.BFeatureCache.PlusFeatures.OrderBy(x => x.Name))
			{
				var info = new FeatureItemInfo();
				string name = feature.Name.Substring(1);
				string fullname = feature.Name.Substring(1);
				info.Name = name;
				info.FullName = fullname;
				info.PlusBit = feature.Bit;
				info.MinusBit = App.BFeatureCache.GetOppositeFeature(feature).Bit;
				info.IsBinary = true;
				info.CacheEntry = feature;
				var item = new ListViewItem(info.Name);
				item.Tag = info;
				Items.Add(item);
			}

			CurrentMask = App.BFeatureCache.GetEmptyMask();
			AdjustColumnWidth();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Figure out the widest text label and set the column width to that.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustColumnWidth()
		{
			var fnt = (EmphasizeCheckedItems ? m_checkedItemFont : FontHelper.UIFont);
			int width = 0;

			for (int i = 0; i < Items.Count; i++)
			{
				var sz = TextRenderer.MeasureText(Items[i].Text, fnt);
				width = Math.Max(width, sz.Width);
			}

			width = Math.Min(width, kMaxColWidth);

			if (CheckBoxes && Items.Count > 0)
			{
				width += (GetItemRect(0, ItemBoundsPortion.Entire).Width -
					GetItemRect(0, ItemBoundsPortion.ItemOnly).Width);
			}

			Columns[0].Width = width;
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
	internal class FeatureItemInfo
	{
		internal string Name;
		internal string FullName;
		internal int Bit;
		internal int PlusBit;
		internal int MinusBit;
		internal bool IsBinary;
		internal bool Checked;
		internal BinaryFeatureValue TriStateValue = BinaryFeatureValue.None;
		internal object CacheEntry;
	}

	#endregion
}
