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
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SIL.Pa.PhoneticSearching;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	#region ClassListViewToolTip class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tooltip for showing the members of a class when the user hovers over a search class
	/// name.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ClassListViewToolTip : ToolTip
	{
		private readonly Font m_titleFont = new Font(FontHelper.UIFont, FontStyle.Bold);
		private readonly Control m_ctrl;
		private Point m_contentLocation = Point.Empty;
		private string m_tipText;
		private ClassListViewItem m_item;
		private ClassListViewItem m_prevItem;

		/// ------------------------------------------------------------------------------------
		public ClassListViewToolTip(Control ctrl)
		{
			m_ctrl = ctrl;
			OwnerDraw = true;
			Popup += HandlePopup;
			Draw += HandleDraw;
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && m_titleFont != null)
				m_titleFont.Dispose();

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public ClassListViewItem ClassListViewItem
		{
			get { return m_item; }
		}

		/// ------------------------------------------------------------------------------------
		public void Show(ClassListViewItem item)
		{
			ErasePrevItemDottedLine(item);

			if (item.ClassType == SearchClassType.Phones)
				m_tipText = item.FormattedMembersString;
			else
			{
				string[] features = item.FeatureNames;
				m_tipText = (features != null ? string.Join(Environment.NewLine, features) : string.Empty);
			}

			m_item = item;
			Point pt = m_ctrl.PointToClient(Control.MousePosition);

			if (!(m_ctrl is ListView))
				pt.Y += (int)(Cursor.Current.Size.Height * 0.7);
			else
			{
				Rectangle rc = (m_ctrl as ListView).GetItemRect(item.Index, ItemBoundsPortion.Label);
				pt.Y = rc.Bottom + 3;
			}

			Show(m_tipText, m_ctrl, pt);
		}

		/// ------------------------------------------------------------------------------------
		public void Hide()
		{
			ErasePrevItemDottedLine(null);
			
			if (m_ctrl != null)
				Hide(m_ctrl);

			m_item = null;
		}

		/// ------------------------------------------------------------------------------------
		private void ErasePrevItemDottedLine(ClassListViewItem item)
		{
			if (m_prevItem != null && m_prevItem.ListView != null)
			{
				Rectangle rc = m_prevItem.GetBounds(ItemBoundsPortion.Label);
				m_prevItem.ListView.Invalidate(rc);
			}

			m_prevItem = item;
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePopup(object sender, PopupEventArgs e)
		{
			if (m_item == null)
				return;

			ToolTipTitle = GetToolTipTitle();
			Size sz = TextRenderer.MeasureText(m_tipText, m_item.ClassMembersFont);
			Size szHdg = TextRenderer.MeasureText(ToolTipTitle, m_titleFont);
			
			// Add enough for 4 pixels of padding all around, and 5
			// pixels between the title and the feature members.
			e.ToolTipSize = new Size(Math.Max(sz.Width, szHdg.Width) + 8,
				sz.Height + szHdg.Height + 13);
			
			m_contentLocation = new Point(4, szHdg.Height + 9);
		}

		/// ------------------------------------------------------------------------------------
		private string GetToolTipTitle()
		{
			if (m_item.ClassType == SearchClassType.Phones)
			{
				return LocalizationManager.GetString("CommonControls.ClassesList.PhoneMembersToolTipHdg", "Members:",
					"Heading for the tooltip used to display the members of a class of phones in a class list view.");
			}

			if (m_tipText.IndexOf(Environment.NewLine, StringComparison.Ordinal) < 0)
			{
				return LocalizationManager.GetString("CommonControls.ClassesList.SingleMemberToolTipHdg", "Member:",
					"Heading for the tooltip used to display the member of a class containing a single feature.");
			}

			if (m_item.ANDFeatures)
			{
				return LocalizationManager.GetString("CommonControls.ClassesList.MembersToolTipMatchAllHdg", "Members (Match All):",
					"Heading for the tooltip used to display the members of a class of features in a class list view.");
			}

			return LocalizationManager.GetString("CommonControls.ClassesList.MembersToolTipOrHdg", "Members (Match Any):",
				"Heading for the tooltip used to display the members of a class of features in a class list view.");
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDraw(object sender, DrawToolTipEventArgs e)
		{
			e.DrawBackground();
			e.DrawBorder();

			TextRenderer.DrawText(e.Graphics, ToolTipTitle, m_titleFont,
				new Point(4, 4), SystemColors.InfoText);

			const TextFormatFlags kFlags = TextFormatFlags.LeftAndRightPadding | TextFormatFlags.WordBreak;
			TextRenderer.DrawText(e.Graphics, m_tipText, m_item.ClassMembersFont,
				m_contentLocation, SystemColors.InfoText, kFlags);
		}
	}

	#endregion

	/// ----------------------------------------------------------------------------------------
	public class ClassListView : ListView
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The type of window for which the class list applies.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public enum ListApplicationType
		{
			SearchViewWnd,
			ClassesDialog,
			DefineClassDialog
		}

		public static string kMemberSubitem = "Members";
		public static string kBasedOnSubitem = "BasedOn";

		// This boolean keeps track of whether they're classes to delete when the
		// SaveChanges() method goes to save all the changes made to the class list.
		private bool m_deletedClass;

		private ListApplicationType m_appliesTo = ListApplicationType.ClassesDialog;
		private bool m_showMembersAndClassTypeColumns = true;
		internal Font PhoneticFont;
		private SortOrder m_sortOrder = SortOrder.None;
		private int m_sortColumn = -1;
		private readonly ClassListViewToolTip m_tooltip;

		#region Construction and loading
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new list resultView for phonetic classes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ClassListView()
		{
			base.DoubleBuffered = true;
			Name = "lvClasses";
			HeaderStyle = ColumnHeaderStyle.Clickable;
			MultiSelect = false;
			OwnerDraw = true;
			View = View.Details;
			FullRowSelect = true;
			HideSelection = false;

			// This will ensure the row height will accomodate the tallest font.
			base.Font = FontHelper.UIFont;

			m_tooltip = new ClassListViewToolTip(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (m_tooltip != null)
					m_tooltip.Dispose();

				if (PhoneticFont != null && PhoneticFont != App.PhoneticFont)
				{
					PhoneticFont.Dispose();
					PhoneticFont = null;
				}
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the columns to the list
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddColumns()
		{
			if (App.DesignMode)
				return;

			SmallImageList = null;
			Columns.Clear();

			// Add the column for the class name.
			var hdr = new ColumnHeader();
			hdr.Name = "hdr" + ClassListViewItem.kClassNameSubitem;
			hdr.Width = 180;
			Columns.Add(hdr);
			hdr.Text = LocalizationManager.GetString("CommonControls.ClassListView.ColumnHeadings.Name", "Name", null, hdr);

			if (!m_showMembersAndClassTypeColumns)
				return;

			PhoneticFont = (App.PhoneticFont.SizeInPoints <= 10 ?
				App.PhoneticFont : FontHelper.MakeFont(App.PhoneticFont, 10));

			// This will force the height of items to fit the larger of the ponetic or
			// UI fonts. I realize this is sort of a kludge, but it's a workable one.
			int itemHeight = Math.Max(PhoneticFont.Height, FontHelper.UIFont.Height);
			SmallImageList = new ImageList();
			SmallImageList.ImageSize = new Size(1, itemHeight);

			// Add a column for the pattern of the class.
			hdr = new ColumnHeader();
			hdr.Name = "hdr" + kMemberSubitem;
			hdr.Width = 205;
			Columns.Add(hdr);
			hdr.Text = LocalizationManager.GetString("CommonControls.ClassListView.ColumnHeadings.Members", "Members", null, hdr);

			// Add a column for the text showing what the class is based on.
			hdr = new ColumnHeader();
			hdr.Name = "hdr" + kBasedOnSubitem;
			hdr.Width = 175;
			Columns.Add(hdr);
			hdr.Text = LocalizationManager.GetString("CommonControls.ClassListView.ColumnHeadings.Type", "Type", null, hdr);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list with classes and their members.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			if (App.DesignMode)
				return;

			Items.Clear();

			foreach (var srchClass in App.Project.SearchClasses)
				Items.Add(ClassListViewItem.Create(srchClass, m_showMembersAndClassTypeColumns));
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (App.DesignMode)
				return;

			AddColumns();

			if (m_sortColumn >= 0)
			{
				// Toggle the value now because it will be toggled
				// back in the OnColumnClick method.
				m_sortOrder = (m_sortOrder == SortOrder.Ascending ?
					SortOrder.Descending : SortOrder.Ascending);

				OnColumnClick(new ColumnClickEventArgs(m_sortColumn));
			}

			SelectedItems.Clear();
			if (Items.Count > 0)
				Items[0].Selected = true;
		}

		#endregion

		#region Loading/Restoring Settings and Saving changes to database
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Restores the list's column widths the query file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadSettings(string parentFormName)
		{
			// Review: I (Jason Naylor) am wondering if LoadSettings and SaveSettings ever do anything except throw and ignore exceptions
			foreach (ColumnHeader col in Columns)
			{
				try
				{
					var width = (int)Properties.Settings.Default[parentFormName + "ClassListViewColWidth" + col.Index];
					if (width > 0)
						col.Width = width;
				}
				catch { }
			}

			try
			{
				m_sortColumn = (int)Properties.Settings.Default[parentFormName + "ClassListViewSortedColumn"];
			}
			catch { }

			try
			{
				m_sortOrder = (SortOrder)Properties.Settings.Default[parentFormName + "ClassListViewSortOrder"];
			}
			catch
			{
				m_sortOrder = SortOrder.None;
			}
			
			SortList(-1);

			if (Items.Count > 0)
			{
				Items[0].Selected = true;
				Items[0].Focused = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the list's column widths the query file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveSettings(string parentFormName)
		{
			foreach (ColumnHeader col in Columns)
			{
				try
				{
					Properties.Settings.Default[parentFormName + "ClassListViewColWidth" + col.Index] = col.Width;
				}
				catch { }
			}

			try
			{
				Properties.Settings.Default[parentFormName + "ClassListViewSortedColumn"] = m_sortColumn;
			}
			catch { }

			try
			{
				Properties.Settings.Default[parentFormName + "ClassListViewSortOrder"] = m_sortOrder;
			}
			catch { }
			
			Properties.Settings.Default.Save();
		}

		#endregion

		#region Methods for saving changes to classes
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves to the database any changes made to the lists items.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveChanges()
		{
			m_deletedClass = false;
			var list = App.Project.SearchClasses;
			list.Clear();
			list.AddRange(from ClassListViewItem item in Items select item.SearchClass);
			list.Save();
		}		

		#endregion

		/// ------------------------------------------------------------------------------------
		protected override void OnBeforeLabelEdit(LabelEditEventArgs e)
		{
			var item = Items[e.Item] as ClassListViewItem;
			if (item != null)
			{
				item.InEditMode = true;
				item.Text = item.Text.Trim();
			}

			base.OnBeforeLabelEdit(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure to dirty the item after its class name changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnAfterLabelEdit(LabelEditEventArgs e)
		{
			base.OnAfterLabelEdit(e);

			var item = Items[e.Item] as ClassListViewItem;

			if (e.Label != null && item != null)
			{
				string newName = e.Label.Trim();
				if (newName != item.Text && !DoesClassNameExist(newName, item, true))
				{
					item.Text = newName;
					item.IsDirty = true;
				}
			}

			e.CancelEdit = true;
			item.InEditMode = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the specified item from the list and keeps track of its Id so the class
		/// will be deleted from the database when the SaveChanges method is called.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void DeleteItem(ClassListViewItem item)
		{
			int prevIndex = item.Index;
			m_deletedClass = true;
			Items.Remove(item);

			while (prevIndex >= Items.Count)
				prevIndex--;

			if (Items.Count > 0)
				Items[prevIndex].Selected = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the Members and class type columns
		/// should be shown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowMembersAndTypeColumns
		{
			get { return m_showMembersAndClassTypeColumns; }
			set
			{
				if (PhoneticFont != null && PhoneticFont != App.PhoneticFont)
				{
					PhoneticFont.Dispose();
					PhoneticFont = null;
				}
				
				m_showMembersAndClassTypeColumns = value;
				
				if (IsHandleCreated)
					AddColumns();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to show all the classes (i.e. for
		/// the find phone window) or not (i.e. for the class dialog).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ListApplicationType AppliesTo
		{
			get { return m_appliesTo; }
			set
			{
				m_appliesTo = value;
				Load();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the height of the columnn header portion of the listview.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ColumnHeaderHeight
		{
			get
			{
				IntPtr hwnd = new IntPtr(Utils.FindWindowEx(Handle, 0, "SysHeader32", null));
				Utils.RECT rc;
				return (Utils.GetWindowRect(hwnd, out rc) ? rc.bottom - rc.top + 1 : 0);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not any of the items in the list are dirty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsDirty
		{
			get { return (m_deletedClass || Items.Cast<ClassListViewItem>().Any(item => item.IsDirty)); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if the specified class name already exists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool DoesClassNameExist(string className, ClassListViewItem origClassInfo, bool showMsg)
		{
			if (className != null)
				className = className.Trim();

			// Ensure the new class doesn't have a duplicate class name
			if (Items.Cast<ClassListViewItem>()
				.Any(item => item.Text.Trim().ToLower() == className.ToLower() && item != origClassInfo))
			{
				if (showMsg)
				{
					var msg = LocalizationManager.GetString("CommonControls.ClassesList.DuplicateClassNameErrorMsg",
					    "Class '{0}' already exists. Choose a different name.",
					    "Error message when attempting to create class with duplicate name.");

					App.NotifyUserOfProblem(msg, className);
				}
					
				return true;
			}

			return false;
		}

		#region Member tooltip methods
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
			if (htinfo.Item != null)
			{
				var item = htinfo.Item as ClassListViewItem;
				if (item != null)
				{
					var rc = GetItemRect(item.Index, ItemBoundsPortion.Label);
					if (rc.Contains(e.Location))
					{
						if (item != m_tooltip.ClassListViewItem)
						{
							m_tooltip.Show(item);
							Invalidate(rc);
						}

						return;
					}
				}
			}

			m_tooltip.Hide();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			
			if (m_tooltip != null)
				m_tooltip.Hide();
		}

		#endregion

		#region Drawing methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Need to draw IPA character class members with the phonetic font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
			e.DrawDefault = true;
			base.OnDrawItem(e);
			
			// If we've returned from calling the base with the draw default flag set to false
			// we assume some derived class or delegate has already done the drawing.
			if (!e.DrawDefault)
				return;

			var item = e.Item as ClassListViewItem;
			if (item == null)
				return;
			
			e.DrawDefault = false;
			item.Draw(e);

			if (m_tooltip != null && m_tooltip.ClassListViewItem == item)
			{
				int width = TextRenderer.MeasureText(item.Text, FontHelper.UIFont).Width;
				var rc = item.GetBounds(ItemBoundsPortion.Label);
				using (var pen = (Pen)SystemPens.WindowText.Clone())
				{
					pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
					int dy = rc.Bottom - (m_showMembersAndClassTypeColumns ? 3 : 2);
					e.Graphics.DrawLine(pen, rc.X, dy, rc.X + Math.Min(rc.Width, width), dy);
				}
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Must have this when owner draw is turned on, even if this item isn't custom drawn.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
		{
			if (m_sortColumn != e.ColumnIndex)
			{
				e.DrawDefault = true;
				base.OnDrawColumnHeader(e);
				return;
			}

			e.DrawDefault = false;
			e.DrawBackground();

			// For some reason, the default formatting doesn't yield the same result you
			// get when letting the base class draw the text. This formatting is as close
			// as I could get to that. Maybe it's a Windows 7 thing.
			var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
				TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
			
			if (RightToLeft == RightToLeft.Yes)
				flags |= TextFormatFlags.RightToLeft;

			e.DrawText(flags);

			// Draw the arrow glyph, indicating the sort direction. The
			// arrow will be drawn from its wider end to the point.
			int inc = 1;
			int start = e.Bounds.Top + ((e.Bounds.Height - 5) / 2) - 1;
			if (m_sortOrder == SortOrder.Ascending)
			{
				// Draw fat end at the bottom and move up.
				start += 4;
				inc = -1;
			}

			var pt1 = new Point(e.Bounds.Right - 20, start);
			var pt2 = new Point(pt1.X + 8, start);

			// Draw four lines, each successive line being two pixels shorter
			// than the previous and one line up or down from the previous.
			for (int i = 0; i < 4; i++)
			{
				e.Graphics.DrawLine(SystemPens.ControlDark, pt1, pt2);
				pt1.X++;
				pt2.X--;
				pt1.Y += inc;
				pt2.Y += inc;
			}

			// Draw the point. Since a line cannot be 1 pixel long, draw a vertical
			// line that starts at the pixel that is the point and ends one pixel
			// into the body of the arrow.
			pt2.Y -= inc;
			e.Graphics.DrawLine(SystemPens.ControlDark, pt1, pt2);
		}

		#endregion

		#region Sorting methods and comparer class
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Allow sorting on the class name or the class type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnColumnClick(ColumnClickEventArgs e)
		{
			base.OnColumnClick(e);
			SortList(e.Column);
		}

		/// ------------------------------------------------------------------------------------
		public void SortList(int col)
		{
			if (col == 1)
				return;

			if (col != -1)
			{
				m_sortColumn = col;
				m_sortOrder = (m_sortOrder == SortOrder.Ascending ?
					SortOrder.Descending : SortOrder.Ascending);
			}

			var item = (SelectedItems.Count > 0 ? SelectedItems[0] : null);
			ListViewItemSorter = new ClassListComparer(m_sortOrder, m_sortColumn);
			ListViewItemSorter = null;
			Invalidate(new Rectangle(0, 0, Width, ColumnHeaderHeight), true);
			if (item != null)
				item.Focused = true;
		}

		/// ------------------------------------------------------------------------------------
		class ClassListComparer : System.Collections.IComparer
		{
			private readonly SortOrder m_sortOrder;
			private readonly int m_sortColumn;

			/// --------------------------------------------------------------------------------
			internal ClassListComparer(SortOrder order, int sortColumn)
			{
				m_sortOrder = order;
				m_sortColumn = sortColumn;
			}

			/// --------------------------------------------------------------------------------
			public int Compare(object ox, object oy)
			{
				if (m_sortOrder == SortOrder.None)
					return 0;

				var x = ox as ListViewItem;
				var y = oy as ListViewItem;

				int result = 0;

				if (m_sortColumn == 2)
					result = x.SubItems[2].Text.CompareTo(y.SubItems[2].Text);
				
				if (result == 0)
					result = x.Text.CompareTo(y.Text);

				return (result != 0 && m_sortOrder == SortOrder.Descending ? -result : result);
			}
		}

		#endregion
	}
}
