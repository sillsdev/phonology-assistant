using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using SIL.Pa;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.AddOn
{
	internal class AddOnHelper
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Because there is a bug in the UI adapter code, I need to kludge a fix to add
		/// menu separators in the right places after adding menus in add-on code. This
		/// will add a menu separator before the menu with the specified name. This will
		/// only work if the menu adapter uses the MenuStrip class and if it references
		/// its MenuStrip using a member variable by the name of 'm_menuBar'. To be more
		/// generic, I could probably use reflection to iterate through the objects in the
		/// adapter to find one tha is of type MenuStrip, but I'm too lazy; YAGNI for now.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static void AddSeparatorBeforeMenuItem(string menuItem)
		{
			try
			{
				// Get the main menu from the adapter;
				MenuStrip mainMenu =
					ReflectionHelper.GetField(PaApp.TMAdapter, "m_menuBar") as MenuStrip;

				if (mainMenu == null)
					return;

				// Get the menu item before which to add the separator.
				ToolStripItem[] refItems = mainMenu.Items.Find(menuItem, true);
				if (refItems == null || refItems.Length == 0)
					return;

				// Get the parent menu item of the one before which to add the separator.
				// We assume there is only one item returned from 'Find', since one of the
				// rules of the toolbar/menu adapter is that all menu and toolbar names
				// must be unique.
				ToolStripMenuItem parentItem = refItems[0].OwnerItem as ToolStripMenuItem;

				// Get the index of the menu item in the parent item's
				// drop-down list. Then add a separator at that index.
				int i = parentItem.DropDownItems.IndexOf(refItems[0]);
				parentItem.DropDownItems.Insert(i, new ToolStripSeparator());
			}
			catch { }
		}
	}
}
