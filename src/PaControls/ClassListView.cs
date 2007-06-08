using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using SIL.Pa.Resources;
using SIL.SpeechTools.Utils;
using SIL.Pa.Data;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
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
			FindPhoneWindow,
			ClassesDialog,
			DefineClassesDialog
		}

		public static string kMemberSubitem = "Members";
		public static string kBasedOnSubitem = "BasedOn";

		// This boolean keeps track of whether they're classes to delete when the
		// SaveChanges() method goes to save all the changes made to the class list.
		private bool m_deletedClass = false;

		private ListApplicationType m_appliesTo = ListApplicationType.ClassesDialog;
		private bool m_showMembersAndClassTypeColumns = true;
		internal Font PhoneticFont = null;

		#region Construction and loading
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new list resultView for phonetic classes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ClassListView() : base()
		{
			DoubleBuffered = true;
			Name = "lvClasses";
			HeaderStyle = ColumnHeaderStyle.Nonclickable;
			MultiSelect = false;
			OwnerDraw = true;
			View = View.Details;
			FullRowSelect = true;
			HideSelection = false;

			// This will ensure the row height will accomodate the tallest font.
			Font = FontHelper.UIFont;

			AddColumns();
			//AddGroups();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the columns to the list
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddColumns()
		{
			SmallImageList = null;
			Columns.Clear();

			// Add the column for the class name.
			ColumnHeader hdr = new ColumnHeader();
			hdr.Name = "hdr" + ClassListViewItem.kClassNameSubitem;
			hdr.Text = Properties.Resources.kstidClassListViewNameText;
			hdr.Width = 180;
			Columns.Add(hdr);

			if (m_showMembersAndClassTypeColumns)
			{
				PhoneticFont = (FontHelper.PhoneticFont.SizeInPoints <= 10 ?
					FontHelper.PhoneticFont : FontHelper.MakeFont(FontHelper.PhoneticFont, 10));

				// This will force the height of items to fit the larger of the phonetic or
				// UI fonts. I realize this is sort of a kludge, but it's a workable one.
				int itemHeight = Math.Max(PhoneticFont.Height, FontHelper.UIFont.Height);
				SmallImageList = new ImageList();
				SmallImageList.ImageSize = new Size(1, itemHeight);

				// Add a column for the pattern of the class.
				hdr = new ColumnHeader();
				hdr.Name = "hdr" + kMemberSubitem;
				hdr.Text = Properties.Resources.kstidClassListViewMembersText;
				hdr.Width = 205;
				Columns.Add(hdr);

				// Add a column for the text showing what the class is based on.
				hdr = new ColumnHeader();
				hdr.Name = "hdr" + kBasedOnSubitem;
				hdr.Text = Properties.Resources.kstidClassListViewClassTypeText;
				hdr.Width = 175;
				Columns.Add(hdr);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list with classes and their members.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			if (PaApp.DesignMode)
				return;

			Items.Clear();

			foreach (SearchClass srchClass in PaApp.Project.SearchClasses)
				Items.Add(ClassListViewItem.Create(srchClass));

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
			foreach (ColumnHeader hdr in Columns)
				hdr.Width = PaApp.SettingsHandler.GetIntSettingsValue(parentFormName, "col" + hdr.Index, hdr.Width);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the list's column widths the query file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveSettings(string parentFormName)
		{
			foreach (ColumnHeader hdr in Columns)
				PaApp.SettingsHandler.SaveSettingsValue(parentFormName, "col" + hdr.Index, hdr.Width);
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
			PaApp.Project.SearchClasses.Clear();

			foreach (ClassListViewItem item in Items)
				PaApp.Project.SearchClasses.Add(item.SearchClass);

			PaApp.Project.SearchClasses.Save();		
		}		

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnBeforeLabelEdit(LabelEditEventArgs e)
		{
			ClassListViewItem item = Items[e.Item] as ClassListViewItem;
			if (item != null)
				item.InEditMode = true;

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

			ClassListViewItem item = Items[e.Item] as ClassListViewItem;
			if (item != null)
			{
				item.IsDirty = true;
				item.InEditMode = false;
			}
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
				m_showMembersAndClassTypeColumns = value;
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
				IntPtr hwnd = new IntPtr(STUtils.FindWindowEx(Handle, 0, "SysHeader32", null));
				STUtils.RECT rc = new STUtils.RECT();
				return (STUtils.GetWindowRect(hwnd, out rc) ? rc.bottom - rc.top + 1 : 0);
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
			get
			{
				if (m_deletedClass)
					return true;

				foreach (ClassListViewItem item in Items)
				{
					if (item.IsDirty)
						return true;
				}

				return false;
			}
		}

		#region Drawing methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Need to draw IPA character class members with the phonetic font.
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
			e.DrawDefault = !m_showMembersAndClassTypeColumns;
			base.OnDrawItem(e);
		
			if (!e.DrawDefault)
			{
				ClassListViewItem item = e.Item as ClassListViewItem;
				e.DrawDefault = (item == null || !item.Draw(e));
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Must have this when owner draw is turned on, even if this item isn't custom drawn.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
		{
			e.DrawDefault = true;
			base.OnDrawColumnHeader(e);
		}

		#endregion
	}
}
