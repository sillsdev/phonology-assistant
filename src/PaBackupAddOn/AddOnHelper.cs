using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using SIL.Pa;
using SIL.Pa.Data;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.SpeechTools.Utils;
using XCore;

namespace SIL.Pa.AddOn
{
	internal class AddOnHelper : IxCoreColleague
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static void Initialize()
		{
			new AddOnHelper();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Because there is a bug in the UI adapter code, I need to kludge a fix to add
		/// menu separators in the right places after adding menus in add-on code. This
		/// will add a menu separator before the menu with the specified name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static void AddSeparatorBeforeMenuItem(string menuItem)
		{
			try
			{
				// Get the adapter's dictionay that holds all the menu and toolbar items.
				Dictionary<string, ToolStripItem> menuItems =
					ReflectionHelper.GetField(PaApp.TMAdapter, "m_items") as
					Dictionary<string, ToolStripItem>;

				if (menuItems == null)
					return;

				// Get the menu item before which to add the separator.
				ToolStripItem refItem = menuItems[menuItem];
				if (refItem == null)
					return;

				// Get the parent menu item of the one before which to add the separator.
				ToolStripMenuItem parentItem = refItem.OwnerItem as ToolStripMenuItem;
				
				// Get the index of the menu item in the parent item's
				// drop-down list. Then add a separator at that index.
				int i = parentItem.DropDownItems.IndexOf(refItem);
				parentItem.DropDownItems.Insert(i, new ToolStripSeparator());
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal AddOnHelper()
		{
			try
			{
				PaApp.TMAdapter.AddCommandItem("CmdAboutAddOns", "AboutAddOns",
					"About Add-Ons...", null, null, null, null, null, Keys.None, null, null);

				TMItemProperties itemProps = new TMItemProperties();
				itemProps.CommandId = "CmdAboutAddOns";
				itemProps.Name = "mnuAboutAddOns";
				itemProps.Text = null;
				PaApp.TMAdapter.AddMenuItem(itemProps, "mnuFile", "mnuHelpAbout");

				PaApp.AddMediatorColleague(this);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAboutAddOns(object args)
		{
			foreach (Assembly assembly in PaApp.AddOnAssemblys)
			{
				System.Diagnostics.Debug.WriteLine(assembly.FullName);
				object[] obj = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
			
			//AssemblyInformationalVersionAttribute
				//AssemblyVersionAttribute
				//AssemblyName
			
			}

			return true;
		}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
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
