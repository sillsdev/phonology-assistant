// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public class FeatureListViewBase : ListView
	{
		private const int kMaxColWidth = 210;

		public delegate void FeatureChangedHandler(object sender, FeatureMask newMask);
		public event FeatureChangedHandler FeatureChanged;

		public delegate void CustomDoubleClickHandler(object sender, string feature);
		public event CustomDoubleClickHandler CustomDoubleClick;

		protected Size _chkBoxSize = new Size(13, 13);
		protected Color _glyphColor = Color.Black;
		protected HashSet<string> _defaultFeatures = new HashSet<string>();
		protected Font _emphasizedFont;

		private bool _ignoreCheckChanges;
		private FeatureMask m_currMask;
		private readonly FeatureMask _emptyMask;
		private readonly ToolTip _tooltip;

		/// ------------------------------------------------------------------------------------
		public FeatureListViewBase(FeatureMask emptyMask)
		{
			_emptyMask = emptyMask;
			AllowDoubleClickToChangeCheckState = true;

			var colHdr = new ColumnHeader();
			colHdr.Width = kMaxColWidth;

			_tooltip = new ToolTip();

			base.Font = FontHelper.UIFont;
			_emphasizedFont = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold);
			
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
			if (disposing && _emphasizedFont != null)
				_emphasizedFont.Dispose();

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			_ignoreCheckChanges = true;
			base.OnHandleCreated(e);
			_ignoreCheckChanges = false;
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

			if (_tooltip == null)
				return;

			var htinfo = HitTest(e.Location);
			if (htinfo.Item != null && htinfo.Item.Tag != null)
			{
				var item = htinfo.Item.Tag as FeatureItemInfo;
				if (item != null && item.FullName != null &&
					item.Name.ToLower() != item.FullName.ToLower())
				{
					if (htinfo.Item != _tooltip.Tag)
					{
						ErasePreviousUnderline();
						Point pt = PointToClient(MousePosition);
						pt.Y += (int)(Cursor.Current.Size.Height * 0.7);
						_tooltip.Tag = htinfo.Item;
						_tooltip.Show(item.FullName, this, pt);
						Invalidate(GetItemRect(htinfo.Item.Index, ItemBoundsPortion.Label));
					}

					return;
				}
			}

			_tooltip.Hide(this);
			ErasePreviousUnderline();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			if (_tooltip != null)
			{
				_tooltip.Hide(this);
				ErasePreviousUnderline();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ErasePreviousUnderline()
		{
			if (_tooltip == null)
				return;

			var item = _tooltip.Tag as ListViewItem;
			_tooltip.Tag = null;

			if (item != null)
			{
				// Erase the previous 
				var rc = GetItemRect(item.Index, ItemBoundsPortion.Label);
				Invalidate(rc);
			}
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

		#region Overridden methods for editing labels
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (LabelEdit && SelectedItems.Count > 0 && e.KeyCode == Keys.F2)
				SelectedItems[0].BeginEdit();
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

		#endregion

		#region Methods for setting feature states
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the feature masks when a feature's checked value changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnItemCheck(ItemCheckEventArgs e)
		{
			// If we're updating the check because the segment just changed
			// then take the default behavior.
			if (_ignoreCheckChanges)
				return;

			SelectedItems.Clear();
			Items[e.Index].Selected = true;

			var info = Items[e.Index].Tag as FeatureItemInfo;
			if (info == null)
				return;

			CycleFeatureStateValue(info, m_currMask);

			Invalidate(Items[e.Index].Bounds);

			if (FeatureChanged != null)
				FeatureChanged(this, m_currMask);
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
			var rcChkBox = new Rectangle(new Point(rc.X + 3, rc.Y), _chkBoxSize);
			rcChkBox.Y += ((rc.Height - rcChkBox.Height) / 2);

			if (CheckBoxes)
				DrawFeatureState(e.Graphics, info, rcChkBox);

			if (_tooltip == null || _tooltip.Tag != e.Item)
				return;
			
			// Draw underline if the current item has a tooltip showing the feature's full name.
			int width = TextRenderer.MeasureText(e.Item.Text, FontHelper.UIFont).Width;
			rc = e.Item.GetBounds(ItemBoundsPortion.Label);
			using (var pen = (Pen)SystemPens.WindowText.Clone())
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
			rc.X += 2;

			// Determine whether or not to use the emphasized font.
			var fnt = (EmphasizeCheckedItems && GetIsItemSet(info) ? _emphasizedFont : Font);

			// First, draw the text's background.
			rc.Width = TextRenderer.MeasureText(info.Name, fnt).Width + 3;
			var clrText = (selected ? SystemColors.HighlightText : ForeColor);

			if (selected && !Focused)
			{
				clrText = SystemColors.ControlText;
				e.Graphics.FillRectangle(SystemBrushes.Control, rc);
			}
			else if (DrawItemBackgroundAndGetForeColor != null)
			{
				clrText = DrawItemBackgroundAndGetForeColor(e.Graphics,
					rc, selected, GetIsItemNotInDefaultState(info));
			}
			else if (selected)
			{
				e.Graphics.FillRectangle(SystemBrushes.Highlight, rc);
				ControlPaint.DrawFocusRectangle(e.Graphics, rc);
			}

			// Now draw the text.
			rc.Y--;
			TextRenderer.DrawText(e.Graphics, info.Name, fnt, rc, clrText,
				TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void DrawFeatureState(Graphics g, FeatureItemInfo info, Rectangle rc)
		{
			throw new NotImplementedException();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		protected virtual bool GetIsItemNotInDefaultState(FeatureItemInfo info)
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string GetFormattedFeatureName(FeatureItemInfo itemInfo, bool includeBrackets)
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		protected IEnumerable<FeatureItemInfo> GetItems()
		{
			return Items.Cast<ListViewItem>().Select(i => i.Tag).OfType<FeatureItemInfo>();
		}

		/// ------------------------------------------------------------------------------------
		protected IEnumerable<FeatureItemInfo> GetItemsThatAreSet()
		{
			return GetItems().Where(i => GetIsItemSet(i));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool GetIsItemSet(FeatureItemInfo itemInfo)
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void SetFeatureInfoStateFromMask(FeatureItemInfo itemInfo, FeatureMask mask)
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void CycleFeatureStateValue(FeatureItemInfo itemInfo, FeatureMask mask)
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetMaskFromPhoneInfo(IPhoneInfo phoneInfo)
		{
			throw new NotImplementedException();
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public Func<Graphics, Rectangle, bool, bool, Color> DrawItemBackgroundAndGetForeColor;
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an array of formatted feature strings, one for each selected feature in the
		/// list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string[] FormattedFeatures
		{
			get { return GetItemsThatAreSet().Select(i => GetFormattedFeatureName(i, true)).ToArray(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a single string containing a comma delimited list of the currently
		/// checked features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string FormattedFeaturesString
		{
			get { throw new NotImplementedException(); }
		}

		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string CurrentFormattedFeature
		{
			get
			{
				if (SelectedItems.Count > 0)
				{
					var info = SelectedItems[0].Tag as FeatureItemInfo;
					if (info != null)
						return GetFormattedFeatureName(info, true);
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FeatureMask CurrentMask
		{
			get {return m_currMask;}
			set
			{
				m_currMask = value;

				// Loop through items in the feature list and set their state according to
				// the specified mask.
				foreach (var info in Items.Cast<ListViewItem>().Select(i => i.Tag).OfType<FeatureItemInfo>())
					SetFeatureInfoStateFromMask(info, m_currMask);

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
				foreach (var info in GetItemsThatAreSet())
					features.AppendFormat("{0}, ", GetFormattedFeatureName(info, false));

				return features.ToString().TrimEnd(',', ' ');
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

		/// ------------------------------------------------------------------------------------
		public void SetDefaultFeatures(IEnumerable<string> defaultFeatures)
		{
			if (defaultFeatures != null)
				_defaultFeatures = new HashSet<string>(defaultFeatures);
			else
				_defaultFeatures.Clear();
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
			_ignoreCheckChanges = true;
			BeginUpdate();
			Items.Clear();
			LoadFeatures();
			CurrentMask = _emptyMask;
			AdjustColumnWidth();

			if (Items.Count > 0)
				SelectedIndices.Add(0);

			EndUpdate();
			_ignoreCheckChanges = false;
		}

		/// ------------------------------------------------------------------------------------
		protected void LoadFeatures()
		{
			foreach (var feature in GetFeaturesToLoad())
			{
				var info = new FeatureItemInfo();
				info.Name = feature.Name;
				info.FullName = feature.Name;
				info.CacheEntry = feature;
				InitializeLoadedItem(feature, info);
				Items.Add(new ListViewItem(info.Name)).Tag = info;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual IEnumerable<Feature> GetFeaturesToLoad()
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void InitializeLoadedItem(Feature feature, FeatureItemInfo itemInfo)
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Figure out the widest text label and set the column width to that.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustColumnWidth()
		{
			var fnt = (EmphasizeCheckedItems ? _emphasizedFont : FontHelper.UIFont);
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
	public enum BinaryFeatureValue
	{
		Plus = (int)'+',
		Minus = (int)'-',
		None = 0
	}

	/// ------------------------------------------------------------------------------------
	public class FeatureItemInfo
	{
		public string Name;
		public string FullName;
		public int Bit;
		public int PlusBit;
		public int MinusBit;
		public bool IsBinary;
		public bool Checked;
		public BinaryFeatureValue TriStateValue = BinaryFeatureValue.None;
		public object CacheEntry;
	}

	#endregion
}
