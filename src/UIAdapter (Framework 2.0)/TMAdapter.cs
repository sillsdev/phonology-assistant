using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using SilUtils;

namespace SIL.FieldWorks.Common.UIAdapters
{
	#region CommandInfo Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	internal class CommandInfo
	{
		internal string Message;
		internal string TextId;
		internal string Text = "?Unknown?";
		internal string TextAltId;
		internal string TextAlt;
		internal string ContextMenuTextId;
		internal string ContextMenuText;
		internal string ToolTipId;
		internal string ToolTip;
		internal string CategoryId;
		internal string Category;
		internal string StatusMsgId;
		internal string StatusMsg;
		internal Keys ShortcutKey = Keys.None;
		internal Image Image;
	}

	#endregion

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Main TM adapter class.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TMAdapter : ITMAdapter
	{
		private const string kMainMenuName = "~MainMenu~";
		private const string kToolbarItemSuffix = "~ToolbarItem~";
		private const string kDateTimeRegEntry = "TMDefinitionFileDateTime";
		private const string kAdapterVerEntry = "AdapterAssemblyVersion";
		private const string kRufMenuItemNamePrefix = "~rufItem~";

		protected ToolStripPanel m_tspTop;
		protected ToolStripPanel m_tspBottom;
		protected ToolStripPanel m_tspLeft;
		protected ToolStripPanel m_tspRight;

		protected MenuStrip m_menuBar;
		protected Dictionary<string, ToolStrip> m_bars = new Dictionary<string, ToolStrip>();
		protected Dictionary<string, ToolStripItem> m_items = new Dictionary<string, ToolStripItem>();
		protected Dictionary<string, ContextMenuStrip> m_cmenus = new Dictionary<string, ContextMenuStrip>();
		protected SortedDictionary<uint, ToolStrip> m_barLocationOrder = new SortedDictionary<uint, ToolStrip>();
		protected ToolStripMenuItem m_rufMarkerItem;
		protected ToolStripSeparator m_rufMarkerSeparator;

		// Stores all the images until we're done reading all the command ids.
		protected Dictionary<string, Image> m_images = new Dictionary<string, Image>();

		// Stores all the commands (and related information). The keys for this collection
		// are the command id strings from the XML definition file.
		private Dictionary<string, CommandInfo> m_commands = new Dictionary<string, CommandInfo>();

		// This is true while we are reading the XML block of context menus.
		protected bool m_readingContextMenuDef;

		protected int m_currentToolbarParentItemType = -1;
		protected bool m_allowUndocking = true;
		protected string m_settingsFilePrefix;
		protected ArrayList m_menusWithShortcuts = new ArrayList();
		protected bool m_allowuUpdates = true;
		protected DateTime m_itemUpdateTime = DateTime.Now;
		protected XmlTextReader m_xmlReader;
		protected Control m_parentControl;
		protected Mediator m_msgMediator;
		protected string m_appsRegKeyPath = @"Software\SIL\FieldWorks";

		// Stores the item on the View menu that's the parent for the list of
		// toolbars.
		protected ToolStripMenuItem m_toolbarListItem;

		// Resource Manager for localized toolbar and menu strings.
		protected ArrayList m_rmlocalStrings = new ArrayList();

		// Stores the TMItemProperties tag field for items that have one.
		protected Hashtable m_htItemTags = new Hashtable();
		
		#region IToolBarAdapter Events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Event fired when adding menu/toolbar items to a menu/toolbar. This allows
		/// delegates of this event to initialize properties of the menu/toolbar item such
		/// as its text, etc.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public event InitializeItemHandler InitializeItem;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Event fired when adding a menubar or toolbar to the toolbar container.
		/// This allows delegates of this event to initialize properties of the toolbar such
		/// as its text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public event InitializeBarHandler InitializeBar;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Event fired when adding a combobox toolbar item. This gives applications a chance
		/// to initialize a combobox item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public event InitializeComboItemHandler InitializeComboItem;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Event fired when a control container item requests the control to contain.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public event LoadControlContainerItemHandler LoadControlContainerItem;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Event fired when a user picks one of the recently used files in the recently used
		/// files list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public event RecentlyUsedItemChosenHandler RecentlyUsedItemChosen;
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Event fired when the adapter offers the toolbar/menu item to the application for
		/// localization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public event LocalizeItemHandler LocalizeItem;
	
		#endregion

		#region Adapter initialization
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parentControl"></param>
		/// <param name="msgMediator"></param>
		/// <param name="definitions"></param>
		/// <param name="appsRegKeyPath">Registry key path (under HKCU) where application's
		/// settings are stored (default is "Software\SIL\FieldWorks").</param>
		/// <param name="definitions"></param>
		/// ------------------------------------------------------------------------------------
		public void Initialize(Control parentControl, Mediator msgMediator,
			string appsRegKeyPath, string[] definitions)
		{
			m_appsRegKeyPath = appsRegKeyPath;
			Initialize(parentControl, msgMediator, definitions);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parentControl"></param>
		/// <param name="msgMediator"></param>
		/// <param name="definitions"></param>
		/// ------------------------------------------------------------------------------------
		public void Initialize(Control parentControl, Mediator msgMediator, string[] definitions)
		{
			if (m_parentControl != null && m_parentControl == parentControl)
				return;

			m_parentControl = parentControl;
			m_msgMediator = msgMediator;

			SetupToolStripPanels();

			// Read images, localized strings and command Ids.
			ReadResourcesAndCommands(definitions);

			m_parentControl.SuspendLayout();

			ReadMenuDefinitions(definitions);
		
			if (m_toolbarListItem != null && m_toolbarListItem.OwnerItem != null &&
				m_toolbarListItem.OwnerItem is ToolStripMenuItem)
			{
				ToolStripMenuItem item = m_toolbarListItem.OwnerItem as ToolStripMenuItem;
				if (item != null)
					item.DropDownClosed += HandleToolBarListMenuClosing;
			}

			GetSettingFilesPrefix(definitions);
			CheckDefinitionDates(definitions);
			CheckAssemblyVersion();
			ReadToolbarDefinitions(definitions);
			PositionToolbars();
			
			AllowAppsToIntializeComboItems();

			m_images = null;
			m_rmlocalStrings = null;

			m_parentControl.ResumeLayout();
			Application.Idle += HandleItemUpdates;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets the combobox items and fires the InitializeComboItem event in
		/// case the application wants to initialize the combo box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AllowAppsToIntializeComboItems()
		{
			if (InitializeComboItem == null || m_bars == null)
				return;

			foreach (ToolStrip bar in m_bars.Values)
			{
				if (bar.Items == null)
					continue;

				foreach (ToolStripItem item in bar.Items)
				{
					ToolStripComboBox cboItem = item as ToolStripComboBox;
					if (cboItem != null)
					{
						cboItem.ComboBox.Items.Clear();
						InitializeComboItem(cboItem.Name, cboItem.ComboBox);
					}
				}
			}
		}

		#endregion

		#region Misc. Setup
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates toolstrip panels.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupToolStripPanels()
		{
			m_tspTop = new ToolStripPanel();
			m_tspTop.Dock = DockStyle.Top;
			m_tspTop.Name = "tspTop";
			m_tspTop.Orientation = Orientation.Horizontal;
			m_tspTop.RowMargin = new Padding(3, 0, 0, 0);

			m_tspBottom = new ToolStripPanel();
			m_tspBottom.Dock = DockStyle.Bottom;
			m_tspBottom.Name = "tspBottom";
			m_tspBottom.Orientation = Orientation.Horizontal;
			m_tspBottom.RowMargin = new Padding(3, 0, 0, 0);

			m_tspLeft = new ToolStripPanel();
			m_tspLeft.Dock = DockStyle.Left;
			m_tspLeft.Name = "tspLeft";
			m_tspLeft.Orientation = Orientation.Vertical;
			m_tspLeft.RowMargin = new Padding(0, 3, 0, 0);

			m_tspRight = new ToolStripPanel();
			m_tspRight.Dock = DockStyle.Right;
			m_tspRight.Name = "tspRight";
			m_tspRight.Orientation = Orientation.Vertical;
			m_tspRight.RowMargin = new Padding(0, 3, 0, 0);

			m_parentControl.Controls.Add(m_tspLeft);
			m_parentControl.Controls.Add(m_tspRight);
			m_parentControl.Controls.Add(m_tspTop);
			m_parentControl.Controls.Add(m_tspBottom);
		}

		#endregion

		#region Misc. methods for loading toolbar defs. from settings file as well as saving settings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Causes the adapter to save toolbar settings (e.g. user placement of toolbars).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveBarSettings()
		{
			RegistryKey regKey = BarPropKey;

			foreach (ToolStrip bar in m_bars.Values)
			{
				regKey.SetValue(bar.Name + "Top", bar.Top);
				regKey.SetValue(bar.Name + "Left", bar.Left);
				regKey.SetValue(bar.Name + "Visible", bar.Visible);

				// Save which side of the form the toolbar is docked.
				if (bar.Parent != null)
					regKey.SetValue(bar.Name + "Side", bar.Parent.Name);
			}
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Examines the XML definition files to find which one (if any) have the prefix
		/// used for storing toolbar settings.
		/// </summary>
		/// <param name="definitions"></param>
		/// ------------------------------------------------------------------------------------
		private void GetSettingFilesPrefix(string[] definitions)
		{
			XmlDocument xmlDef = new XmlDocument();
			xmlDef.PreserveWhitespace = false;

			foreach (string def in definitions)
			{
				if (def == null)
					continue;

				xmlDef.Load(def);
				XmlNode node = xmlDef.SelectSingleNode("TMDef/toolbars");
				if (node != null)
				{
					m_settingsFilePrefix = GetAttributeValue(node, "settingFilesPrefix");
					m_allowUndocking = GetBoolFromAttribute(node, "onmodalform", true);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method checks the date and time of the TM definition files to see if they
		/// are newer than they were the last time the application was run. If one or more
		/// of them are newer, then all the toolbar settings files written by DotNetBar are
		/// deleted so the toolbars will be completely reconstructed from the TM definition
		/// files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CheckDefinitionDates(IEnumerable<string> definitions)
		{
			DateTime newestDefDateTime = new DateTime(0);

			// Read the saved date/time from the registry.
			string dateString = (string)TMDefinitionDateKey.GetValue(kDateTimeRegEntry, "{1/1/1970}");
			DateTime savedDateTime = new DateTime(0);
			try
			{
				savedDateTime = DateTime.Parse(dateString);
			}
			catch {}
					 
			foreach (string def in definitions)
			{
				if (def == null || !File.Exists(def))
					continue;

				// Get the date/time of this definition file. If it is the newest one so
				// far, then save the date/time. By getting the file's date/time in the
				// following way, we strip off the milliseconds, which is what we want.
				DateTime fileDateTime = DateTime.Parse(File.GetLastWriteTime(def).ToString("G"));
				if (fileDateTime > newestDefDateTime)
					newestDefDateTime = fileDateTime;

				// If the def. file is newer than it was the last time we ran the app. then
				// delete all the saved toolbar properties and start from scratch.
				if (fileDateTime > savedDateTime)
				{
					// REVIEW: Is this really necessary if the user can only customize
					// the visibility and location of toolbars?
					//Registry.CurrentUser.CreateSubKey(m_appsRegKeyPath).DeleteSubKey("ToolBarProperties", false);
					//break;
				}
			}

			// Save the date/time of the newest TM definition file.
			TMDefinitionDateKey.SetValue(kDateTimeRegEntry, newestDefDateTime.ToString("G"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if the current adapter assembly version has changed since the last time
		/// the application was run.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CheckAssemblyVersion()
		{
			// Read the saved version of the adapter assembly from the registry.
			string savedVersion = (string)TMDefinitionDateKey.GetValue(kAdapterVerEntry, "0");
			string currentVersion = GetType().Assembly.GetName().Version.ToString();

			// If the assembly versions do not match then delete the saved files.
			if (savedVersion != currentVersion)
			{
				// REVIEW: Do we need to blow away toolbar settings for this adapter
				// since the user is only allowed to change the location and visibility
				// of a toolbar?
			}

			// Save the current adapter assembly version
			TMDefinitionDateKey.SetValue(kAdapterVerEntry, currentVersion);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Positions the toolbars to the positions specified in the xml definition file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PositionToolbars()
		{
			if (m_menuBar != null)
			{
				m_menuBar.Dock = DockStyle.Top;
				m_parentControl.Controls.Add(m_menuBar);
			}

			int lastRow = 0;
			int x = 0;
			RegistryKey regKey = BarPropKey;

			foreach (KeyValuePair<uint, ToolStrip> bar in m_barLocationOrder)
			{
				// Dock the toolbar to the proper side of the form.
				string docSide = (string)regKey.GetValue(bar.Value.Name + "Side", "tspTop");

				// See if the bar's location was saved in the registry.
				int left = (int)regKey.GetValue(bar.Value.Name + "Left", -1);
				int top = (int)regKey.GetValue(bar.Value.Name + "Top", -1);

				// If the bar's location wasn't found in the registry, then figure out what
				// it should be based on the position and row read from the definition file.
				if (left == -1 || top == -1)
				{
					// Get the row from the high 16 bits.
					int row = (int)((bar.Key & 0xFFFF0000) >> 16);

					if (row != lastRow)
					{
						lastRow = row;
						x = 0;
					}
					
					top = row * bar.Value.Height;
					left = x;
				}
			
				bar.Value.Location = new Point(left, top);
				bar.Value.Visible = bool.Parse((string)regKey.GetValue(bar.Value.Name + "Visible", "true"));

				switch (docSide)
				{
					case "tspTop": m_tspTop.Controls.Add(bar.Value); break;
					case "tspBottom": m_tspBottom.Controls.Add(bar.Value); break;
					case "tspLeft": m_tspLeft.Controls.Add(bar.Value); break;
					case "tspRight": m_tspRight.Controls.Add(bar.Value); break;
				}
	
				x += (bar.Value.Left + bar.Value.Width);
			}

			m_barLocationOrder = null;
		}

		#endregion
		
		#region Internal Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the registry key where the adapter stores the date/time of the newest TM
		/// definition file passed from an application.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private RegistryKey TMDefinitionDateKey
		{
			get 
			{
				m_appsRegKeyPath = m_appsRegKeyPath.TrimEnd(new[] {'\\'});
				return Registry.CurrentUser.CreateSubKey(
					m_appsRegKeyPath + @"\ToolBarAdapterVersions\" + m_settingsFilePrefix);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the registry key where the adapter stores the tool bar positions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private RegistryKey BarPropKey
		{
			get	{return Registry.CurrentUser.CreateSubKey(m_appsRegKeyPath + @"\ToolBarProperties");}
		}

		#endregion

		#region ITMInterface Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the message mediator used by the TM adapter for message dispatch.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Mediator MessageMediator
		{
			get {return m_msgMediator;}
			set {m_msgMediator = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not updates to toolbar items should
		/// take place during the application's idle cycles.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AllowUpdates
		{
			get {return m_allowuUpdates;}
			set
			{
				if (m_allowuUpdates != value)
				{
					m_allowuUpdates = value;
					Application.Idle -= HandleItemUpdates;
					
					if (value)
						Application.Idle += HandleItemUpdates;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an array of toolbar property objects for display on a view menu.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TMBarProperties[] BarInfoForViewMenu
		{
			get
			{
				if (m_bars.Count == 0)
					return null;

				TMBarProperties[] barProps = new TMBarProperties[m_bars.Count];
				int i = 0;

				foreach (KeyValuePair<string, ToolStrip> bar in m_bars)
					barProps[i++] = GetBarProperties(bar.Key);

				Array.Sort(barProps, new ToolBarNameSorter());
				return barProps;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private class ToolBarNameSorter : IComparer  
		{
			int IComparer.Compare(object x, object y)  
			{
				string item1 = ((TMBarProperties)x).Text;
				string item2 = ((TMBarProperties)y).Text;
				return((new CaseInsensitiveComparer()).Compare(item1, item2));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of files to show on the recent files list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] RecentFilesList
		{
			get
			{
				int rufIndex;
				ToolStripMenuItem parentItem = GetRecentlyUsedMarkerItemsParent(out rufIndex);
				if (parentItem == null || rufIndex < 0)
					return null;

				List<string> recentItems = new List<string>();
				for (int i = rufIndex + 1; i < parentItem.DropDownItems.Count; i++)
				{
					ToolStripMenuItem item = parentItem.DropDownItems[i] as ToolStripMenuItem;
					if (item == null)
						continue;

					// If the item is a recently used file, then strip off the ampersand/digit
					// mnumonic and the space following it and save it in the list.
					if (!item.Name.StartsWith(kRufMenuItemNamePrefix))
						break;

					recentItems.Add(item.Text.Substring(3));
				}

				return (recentItems.Count == 0 ? null : recentItems.ToArray());
			}
			set
			{
				int rufIndex;
				ToolStripMenuItem parentItem = GetRecentlyUsedMarkerItemsParent(out rufIndex);
				if (parentItem == null || rufIndex < 0)
					return;

				// First, clear any old items from the recently used list.
				int indexToRemove = rufIndex + 1;
				while (indexToRemove < parentItem.DropDownItems.Count)
				{
					ToolStripItem item = parentItem.DropDownItems[indexToRemove];
					if (!item.Name.StartsWith(kRufMenuItemNamePrefix))
						break;

					item.Click -= HandleRecentlyUsedItemClick;
					parentItem.DropDownItems.Remove(item);
					item.Dispose();
				}

				// Now add the list of new items.
				if (value == null || value.Length == 0)
					return;

				for (int i = 1; i <= value.Length; i++)
				{
					// Only add the accelerator for files 1 - 9.
					string fmt = (i > 9 ? string.Empty : "&") + "{0} {1}";
					string text = string.Format(fmt, i, value[i - 1]);
					ToolStripMenuItem item = new ToolStripMenuItem(text);
					item.Name = kRufMenuItemNamePrefix + i;
					item.Click += HandleRecentlyUsedItemClick;
					parentItem.DropDownItems.Insert(rufIndex + i, item);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle a recently used item being clicked on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleRecentlyUsedItemClick(object sender, EventArgs e)
		{
			ToolStripMenuItem item = sender as ToolStripMenuItem;

			if (item != null && RecentlyUsedItemChosen != null)
				RecentlyUsedItemChosen(item.Text.Substring(3));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the parent item on whose drop-down list is the recently used list marker menu
		/// item (it's a marker item because the menu item is only used to mark the item right
		/// before the first item in the recently used list of menu items). The index of the
		/// marker menu item is returned also.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private ToolStripMenuItem GetRecentlyUsedMarkerItemsParent(out int rufIndex)
		{
			rufIndex = -1;

			if (m_rufMarkerItem == null || m_rufMarkerItem.OwnerItem == null ||
				!(m_rufMarkerItem.OwnerItem is ToolStripMenuItem))
			{
				return null;
			}

			ToolStripMenuItem parentItem = m_rufMarkerItem.OwnerItem as ToolStripMenuItem;
			if (parentItem != null)
				rufIndex = parentItem.DropDownItems.IndexOf(m_rufMarkerItem);
			
			return parentItem;
		}

		#endregion

		#region Methods for reading resources and command section of definition
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="definitions"></param>
		/// ------------------------------------------------------------------------------------
		private void ReadResourcesAndCommands(string[] definitions)
		{
			XmlDocument xmlDef = new XmlDocument();
			
			foreach (string def in definitions)
			{
				if (def == null)
					continue;

				xmlDef.PreserveWhitespace = false;
				xmlDef.Load(def);

				XmlNode node = xmlDef.SelectSingleNode("TMDef/resources");
				if (node == null || !node.HasChildNodes)
					return;

				node = node.FirstChild;
				while (node != null)
				{
					switch (node.Name)
					{
						case "imageList":
							// Get the images from the specified resource files.
							ReadImagesResources(node);	
							break;

						case "localizedstrings":
							// Get the resource files containing the localized strings.
							ResourceManager rm = GetResourceMngr(node);
							if (rm != null)
								m_rmlocalStrings.Add(rm);
							break;
					}
					
					node = ReadOverJunk(node.NextSibling);
				}

				node = xmlDef.SelectSingleNode("TMDef/commands");
				ReadCommands(node);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the images from the resource specified in the XML definition.
		/// </summary>
		/// <param name="node"></param>
		/// ------------------------------------------------------------------------------------
		private void ReadImagesResources(XmlNode node)
		{
			string assemblyPath = GetAttributeValue(node, "assemblyPath");
			string className = GetAttributeValue(node, "class");
			string field = GetAttributeValue(node, "field");
			string labels = GetAttributeValue(node, "labels");

			if (assemblyPath == null || className == null || field == null || labels == null)
				return;

			ImageList images = GetImageListFromResourceAssembly(assemblyPath, className, field);
			string[] imageLabels = labels.Split(new[] {',', '\r', '\n', '\t'});
			int i = 0;
			foreach (string label in imageLabels)
			{
				string trimmedLabel = label.Trim();
				if (trimmedLabel != string.Empty && i >= 0 && i < images.Images.Count)
					m_images[trimmedLabel] = images.Images[i++];
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <param name="className"></param>
		/// <param name="field"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private static ImageList GetImageListFromResourceAssembly(string assemblyName,
			string className, string field)
		{
			Assembly assembly = GetAssembly(assemblyName);
			
			// Instantiate an object of the class containing the image list we're after.
			object classIntance = assembly.CreateInstance(className);
			if (classIntance == null)
			{
				throw new Exception("TM Adapter could not create the class: " +
					className + ".");
			}

			//Get the named ImageList
			FieldInfo fldInfo = classIntance.GetType().GetField(field,
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

			if (fldInfo == null)
			{
				throw new Exception("TM Adapter could not find the field '" + field +
					"' in the class: " + className + ".");
			}

			return (ImageList)fldInfo.GetValue(classIntance);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected static ResourceManager GetResourceMngr(XmlNode node)
		{
			string assemblyPath = GetAttributeValue(node, "assemblyPath");
			string className = GetAttributeValue(node, "class");
			if (assemblyPath == null || className == null)
				return null;

			Assembly assembly = GetAssembly(assemblyPath);
			return new ResourceManager(className, assembly);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads a named assembly.
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected static Assembly GetAssembly(string assemblyName)
		{
			string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
			// Note: CodeBase prepends "file:/", which must be removed.
			string assemblyPath = Path.Combine(baseDir.Substring(6), assemblyName);

			Assembly assembly;
			
			try
			{
				assembly = Assembly.LoadFrom(assemblyPath);
				if (assembly == null)
					throw new ApplicationException(); //will be caught and described in the catch
			}
			catch (Exception error)
			{
				throw new Exception("TM Adapter could not load the DLL at: " +
					assemblyPath, error);
			}

			return assembly;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reads the commands section of a definition file.
		/// </summary>
		/// <param name="node"></param>
		/// ------------------------------------------------------------------------------------
		protected void ReadCommands(XmlNode node)
		{
			XmlNode commandNode = node.FirstChild;

			while (commandNode != null)
			{
				string cmd = GetAttributeValue(commandNode, "id");
				if (cmd == null)
					continue;

                CommandInfo cmdInfo = new CommandInfo();
				cmdInfo.TextId = GetAttributeValue(commandNode, "text");
				cmdInfo.TextAltId = GetAttributeValue(commandNode, "textalt");
				cmdInfo.ContextMenuTextId = GetAttributeValue(commandNode, "contextmenutext");
				cmdInfo.CategoryId = GetAttributeValue(commandNode, "category");
				cmdInfo.ToolTipId = GetAttributeValue(commandNode, "tooltip");
				cmdInfo.StatusMsgId = GetAttributeValue(commandNode, "statusmsg");
				cmdInfo.Message = GetAttributeValue(commandNode, "message");
				string shortcut = GetAttributeValue(commandNode, "shortcutkey");
				string imageLabel =	GetAttributeValue(commandNode, "image");

				// If the kstid for the text wasn't specified, build a kstid using
				// the command id.
				if (cmdInfo.TextId == null)
				{
					cmdInfo.TextId = cmd;
					if (cmdInfo.TextId.StartsWith("Cmd"))
						cmdInfo.TextId = cmdInfo.TextId.Remove(0, 3);
					cmdInfo.TextId = "kstid" + cmdInfo.TextId + "Text";
				}

				// If the kstid for the tooltip wasn't specified, build a kstid using
				// the command id.
				if (cmdInfo.ToolTipId == null)
				{
					cmdInfo.ToolTipId = cmd;
					if (cmdInfo.ToolTipId.StartsWith("Cmd"))
						cmdInfo.ToolTipId = cmdInfo.ToolTipId.Remove(0, 3);
					cmdInfo.ToolTipId = "kstid" + cmdInfo.ToolTipId + "ToolTip";
				}

				if (cmdInfo.TextId != null)
					cmdInfo.Text = GetStringFromResource(cmdInfo.TextId);

				if (cmdInfo.TextAltId != null)
					cmdInfo.TextAlt = GetStringFromResource(cmdInfo.TextAltId);

				if (cmdInfo.ContextMenuTextId != null)
					cmdInfo.ContextMenuText = GetStringFromResource(cmdInfo.ContextMenuTextId);

				if (cmdInfo.ToolTipId != null)
					cmdInfo.ToolTip = GetStringFromResource(cmdInfo.ToolTipId, null);

				if (cmdInfo.StatusMsgId != null)
					cmdInfo.StatusMsg = GetStringFromResource(cmdInfo.ToolTipId);

				if (string.IsNullOrEmpty(cmdInfo.StatusMsg))
					cmdInfo.StatusMsg = GetStringFromResource("kstidDefaultStatusBarMsg");

				cmdInfo.ShortcutKey = ParseShortcutKeyString(shortcut);

				// If the command doesn't have an explicit image label, then use
				// the command id as the image label.
				if (imageLabel == null)
					imageLabel = cmd;
				if (m_images.ContainsKey(imageLabel))
					cmdInfo.Image = m_images[imageLabel];

                m_commands[cmd] = cmdInfo;

				commandNode = ReadOverJunk(commandNode.NextSibling);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="shortcut"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private static Keys ParseShortcutKeyString(string shortcut)
		{
			Keys sckeys = Keys.None;

			if (shortcut == null)
				return sckeys;

			try
			{
				shortcut = shortcut.ToLower();
				int i;

				if ((i = shortcut.IndexOf("shift")) > -1)
				{
					sckeys = (sckeys == Keys.None ? Keys.Shift : sckeys | Keys.Shift);
					shortcut = shortcut.Remove(i, 5);
				}
				if ((i = shortcut.IndexOf("ctrl")) > -1)
				{
					sckeys = (sckeys == Keys.None ? Keys.Control : sckeys | Keys.Control);
					shortcut = shortcut.Remove(i, 4);
				}
				if ((i = shortcut.IndexOf("alt")) > -1)
				{
					sckeys = (sckeys == Keys.None ? Keys.Alt : sckeys | Keys.Alt);
					shortcut = shortcut.Remove(i, 3);
				}

				// If the remaining portion of the short cut is a number between 0 and 9, then a
				// 'D' must be placed in front of it since that's how .Net represents number keys.
				if (shortcut.Length == 1 && shortcut[0] >= '0' && shortcut[0] <= '9')
					shortcut = "D" + shortcut;

				if (sckeys == Keys.None)
					sckeys = (Keys)Enum.Parse(typeof(Keys), shortcut, true);
				else
					sckeys |= (Keys)Enum.Parse(typeof(Keys), shortcut, true);

			}
			catch {}

			return sckeys;
		}

		#endregion

		#region Methods for reading menu definitions and building menu items
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="definitions"></param>
		/// ------------------------------------------------------------------------------------
		private void ReadMenuDefinitions(string[] definitions)
		{
			XmlDocument xmlDef = new XmlDocument();
			
			foreach (string def in definitions)
			{
				if (def == null)
					continue;

				xmlDef.PreserveWhitespace = false;
				xmlDef.Load(def);
				ReadContextMenus(xmlDef.SelectSingleNode("TMDef/contextmenus/contextmenu"));
				XmlNode node = xmlDef.SelectSingleNode("TMDef/menus/item");
				if (node == null)
					continue;

				if (m_menuBar == null)
				{
					m_menuBar = new MenuStrip();
					m_menuBar.Name = kMainMenuName;
					m_menuBar.Text = kMainMenuName;
					m_menuBar.AllowItemReorder = false;
					m_menuBar.Dock = DockStyle.Top;
					m_menuBar.GripStyle = ToolStripGripStyle.Hidden;
					m_menuBar.AccessibleRole = AccessibleRole.MenuBar;
					m_menuBar.AccessibleName = m_menuBar.Text;
					m_menuBar.ShowItemToolTips = true;
				}

				ReadMenuItems(node, m_menuBar, false);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds all context menus, adding them to the DNB manager's list of context menus.
		/// </summary>
		/// <param name="node"></param>
		/// ------------------------------------------------------------------------------------
		private void ReadContextMenus(XmlNode node)
		{
			m_cmenus.Clear();

			while (node != null)
			{
				string name = GetAttributeValue(node, "name");

				if (name != null)
				{
					ContextMenuStrip cmnu = new ContextMenuStrip();
					cmnu.Name = name;
					cmnu.ShowImageMargin = GetBoolFromAttribute(node, "showimagemargin", true);
					cmnu.ShowCheckMargin = GetBoolFromAttribute(node, "showcheckmargin", !cmnu.ShowImageMargin);
					cmnu.ShowItemToolTips = GetBoolFromAttribute(node, "showitemtooltips", false);
					cmnu.Opened += HandleContextMenuOpened;
					
					ReadMenuItems(node.FirstChild, cmnu, true);
					
					if (cmnu.Items.Count > 0)
					{
						// Make sure that if a command has different text for
						// context menus then we account for it.
						foreach (ToolStripItem item in cmnu.Items)
						{
							if (item is ToolStripMenuItem)
							{
								CommandInfo cmndInfo = GetCommandInfo(item);
								if (cmndInfo != null && cmndInfo.ContextMenuText != null)
									item.Text = cmndInfo.ContextMenuText;
							}
						}

						m_cmenus.Add(name, cmnu);
					}
				}

				node = ReadOverJunk(node.NextSibling);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Recursively builds menus.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="parentItem"></param>
		/// <param name="readingContextMenus"></param>
		/// ------------------------------------------------------------------------------------
		private void ReadMenuItems(XmlNode node, object parentItem, bool readingContextMenus)
		{
			if (parentItem == null)
				return;

			while (node != null)
			{
				if (node.NodeType == XmlNodeType.Whitespace || node.NodeType == XmlNodeType.Comment)
				{
					node = ReadOverJunk(node.NextSibling);
					continue;
				}

				bool wndListItem = GetBoolFromAttribute(node, "windowlist");
				bool moreWndItem = GetBoolFromAttribute(node, "morewindowsitem");

				// Ignore the "morewindowsitem" and tell the menu bar who should hold
				// the window list. The "morewindowsitem" and window list features are
				// free with the MenuStrip class.
				if (wndListItem || moreWndItem)
				{
					if (wndListItem)
						m_menuBar.MdiWindowListItem = (parentItem as ToolStripMenuItem);
										
					node = ReadOverJunk(node.NextSibling);
					continue;
				}
				
				// When we're reading the menu section of the XML, we don't need to send
				// the parent item to ReadSingleItem. Therefore, send null.
				ToolStripMenuItem item = (ToolStripMenuItem)ReadSingleItem(node, null, true);

				// Check if this item, after which, the recently used list will appear.
				if (GetBoolFromAttribute(node, "recentlyusedlist"))
				{
					item.Visible = false;
					m_rufMarkerItem = item;
				}

				// If the item has a shortcut then we need to add it to the list of
				// menu items to get updated during the application's idle cycles.
				if (item.ShortcutKeys != Keys.None && !m_menusWithShortcuts.Contains(item))
					m_menusWithShortcuts.Add(item);

				string insertBefore = GetAttributeValue(node, "insertbefore");
				string addTo = GetAttributeValue(node, "addto");
				bool beginGroup = GetBoolFromAttribute(node, "begingroup");
				bool cancelBeginGroupOnFollowing = GetBoolFromAttribute(node, "cancelbegingrouponfollowingitem");

				if (insertBefore != null)
					InsertMenuItem(item, insertBefore, beginGroup, cancelBeginGroupOnFollowing);
				else if (addTo != null)
					AddMenuItem(item, addTo, beginGroup);
				else
				{
					if (parentItem is ContextMenuStrip)
					{
						if (beginGroup)
							((ContextMenuStrip)parentItem).Items.Add(new ToolStripSeparator());

						((ContextMenuStrip)parentItem).Items.Add(item);
					}
					else if (parentItem is MenuStrip)
						((MenuStrip)parentItem).Items.Add(item);
					else if (parentItem is ToolStripMenuItem)
					{
						if (beginGroup)
							((ToolStripMenuItem)parentItem).DropDownItems.Add(new ToolStripSeparator());

						((ToolStripMenuItem)parentItem).DropDownItems.Add(item);
					}
				}

				// If the item we just added is the recently used file list marker and it
				// begins a group, save its separater item so we can hide it or make it
				// visible, depending upon whether or not there are any recently used items,
				// when the menu pops up.
				if (item == m_rufMarkerItem && beginGroup && parentItem is ToolStripMenuItem)
				{
					int i;
					GetRecentlyUsedMarkerItemsParent(out i);

					ToolStripItemCollection dropDownItems =
						((ToolStripMenuItem)parentItem).DropDownItems;
					
					if (i >= 0 && dropDownItems.Count > 0)
						m_rufMarkerSeparator = dropDownItems[i - 1] as ToolStripSeparator;
				}

				// Now read any subitems of the one just created.
				if (node.ChildNodes.Count > 0)
					ReadMenuItems(node.FirstChild, item, readingContextMenus);

				node = ReadOverJunk(node.NextSibling);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Finds the item specified by parentItem and inserts the specified item before
		/// it in the collection to which parentItem belongs.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="refItemName"></param>
		/// <param name="beginGroup">True if item should begin a new group.</param>
		/// <param name="cancelBeginGroupOnFollowing">True if parentItem should no longer
		/// begin a group.</param>
		/// ------------------------------------------------------------------------------------
		private void InsertMenuItem(ToolStripMenuItem item, string refItemName, bool beginGroup,
			bool cancelBeginGroupOnFollowing)
		{
			if (m_menuBar == null || item == null || refItemName == null)
				return;

			ToolStripMenuItem refItem = m_items[refItemName] as ToolStripMenuItem;
			if (refItem == null)
				return;

			if (refItem.OwnerItem is ToolStripMenuItem)
			{
				// Get the parent item of the item we're inserting before. Then
				// get the reference item's index in the parent's subitem collection.
				ToolStripMenuItem parentItem = refItem.OwnerItem as ToolStripMenuItem;
				int i = parentItem.DropDownItems.IndexOf(refItem);
				
				// If the reference item should no longer begin a group
				// then get rid of the separator before it.
				if (cancelBeginGroupOnFollowing && i > 0 && parentItem.DropDownItems[i - 1] is ToolStripSeparator)
				{
					parentItem.DropDownItems.RemoveAt(i - 1);
					i--;
				}

				parentItem.DropDownItems.Insert(i, item);
				
				// If the inserted item should begin a group then add a separator
				// if there isn't already one before it.
				if (beginGroup && i > 0 && !(parentItem.DropDownItems[i - 1] is ToolStripSeparator))
					parentItem.DropDownItems.Insert(i, item);
			}
			else
			{
				// The reference item's parent is not another non menu bar menu item so
				// the ref. item must be on the main menu bar. Therefore, add the inserted
				// item to the main menu.
				int i = m_menuBar.Items.IndexOf(refItem);
				m_menuBar.Items.Insert(i, item);
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Finds the parentItem and Adds the specified item to its collection of sub items.
		/// If the parent item cannot be found and there is a menu bar, then the item is
		/// added to the menu bar.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="parentItem"></param>
		/// <param name="beginGroup"></param>
		/// ------------------------------------------------------------------------------------
		private void AddMenuItem(ToolStripMenuItem item, string parentItem, bool beginGroup)
		{
			if (item == null)
				return;

			ToolStripDropDownItem tsddiParent = null;
			
			if (m_items.ContainsKey(parentItem))
				tsddiParent = m_items[parentItem] as ToolStripDropDownItem;

			if (tsddiParent != null)
			{
				if (beginGroup)
					tsddiParent.DropDownItems.Add(new ToolStripSeparator());

				tsddiParent.DropDownItems.Add(item);
			}
			else if (m_menuBar != null)
				m_menuBar.Items.Add(item);
		}

		#endregion		

		#region Methods for reading toolbar definitions and building toolbars and their items
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Iterates through the toolbar definitions to read the XML.
		/// </summary>
		/// <param name="definitions">Array of XML strings toolbar definitions.</param>
		/// ------------------------------------------------------------------------------------
		private void ReadToolbarDefinitions(string[] definitions)
		{
			XmlDocument xmlDef = new XmlDocument();
			xmlDef.PreserveWhitespace = false;

			foreach (string def in definitions)
			{
				if (def == null || !File.Exists(def))
					continue;
				
				xmlDef.Load(def);
				XmlNode node = xmlDef.SelectSingleNode("TMDef/toolbars/toolbar");
				
				while (node != null)
				{
					ReadSingleToolbarDef(node);
					node = ReadOverJunk(node.NextSibling);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// ------------------------------------------------------------------------------------
		private void ReadSingleToolbarDef(XmlNode node)
		{
			string barName = GetAttributeValue(node, "name");
			
			ToolStrip bar = MakeNewBar(node, barName);
			
			if (node.ChildNodes.Count > 0)
				ReadToolbarItems(node.FirstChild, bar);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ReadToolbarItems(XmlNode node, object parentItem)
		{
			while (node != null)
			{
				bool beginGroup = GetBoolFromAttribute(node, "begingroup");
				ToolStripItem item = ReadSingleItem(node, parentItem, false);

				if (parentItem is ToolStrip)
				{
					if (beginGroup)
						((ToolStrip)parentItem).Items.Add(new ToolStripSeparator());

					((ToolStrip)parentItem).Items.Add(item);
				}
				else if (parentItem is ToolStripDropDownItem)
				{
					// This will enforce that items on a drop-down don't have tooltips.
					item.ToolTipText = null;

					ToolStripDropDownItem pitem = (ToolStripDropDownItem)parentItem;

					if (beginGroup)
						pitem.DropDownItems.Add(new ToolStripSeparator());

					// If the parent item's drop-down type is a menu, then make sure the text shows.
					if (pitem.DropDown is ToolStripDropDownMenu)
						item.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;

					if (!(item is ToolStripControlHost))
						pitem.DropDownItems.Add(item);
					else
					{
						// When we're a control host then put ourselves inside a CustomDropDown.
						CustomDropDown dropDown = new CustomDropDown();
						dropDown.AddHost(item as ToolStripControlHost);
						dropDown.AutoCloseWhenMouseLeaves =
							GetBoolFromAttribute(node, "autoclose", true);
						
						pitem.DropDown = dropDown;
					}
				}

				if (node.ChildNodes.Count > 0)
					ReadToolbarItems(node.FirstChild, item);

				node = ReadOverJunk(node.NextSibling);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a toolbar item to the specified toolbar.
		/// </summary>
		/// <param name="toolBarName">Name of the toolbar in which to add the item.</param>
		/// <param name="xml">XML string in the "item" format used in TM Definition xml files.
		/// </param>
		/// <param name="insertBeforeItem">name of item before which the item will be
		/// inserted.</param>
		/// <param name="beginGroupAfter">A flag indicating whether or not to begin a group
		/// after the item being added.</param>
		/// ------------------------------------------------------------------------------------
		public void AddToolBarItem(string toolBarName, string xml, string insertBeforeItem,
			bool beginGroupAfter)
		{
			ToolStrip parentItem;
			ToolStripItem beforeItem;
			int insertIndex = -1;
			
			if (!m_bars.TryGetValue(toolBarName, out parentItem))
				return;

			if (insertBeforeItem != null)
			{
				if (!m_items.TryGetValue(insertBeforeItem, out beforeItem))
					return;

				if (beforeItem.Owner != parentItem)
					beforeItem = null;
			
				if (beforeItem != null)
					insertIndex = parentItem.Items.IndexOf(beforeItem);
			}

			// Make sure the string is valid XML.
			if (!xml.StartsWith("<item "))
				xml = "<item " + xml;

			if (!xml.EndsWith("/>"))
				xml += "/>";

			XmlDocument doc = new XmlDocument();
			doc.InnerXml = xml;
			ToolStripItem item = ReadSingleItem(doc.FirstChild, parentItem, false);

			// Check if we need to add a begin group item.
			if (xml.Contains("begingroup=\"true\"") || xml.Contains("begingroup=\"True\"") ||
				xml.Contains("begingroup=\"TRUE\""))
			{
				if (insertIndex >= 0)
					parentItem.Items.Insert(insertIndex++, new ToolStripSeparator());
				else
					parentItem.Items.Add(new ToolStripSeparator());
			}

			if (insertIndex >= 0)
				parentItem.Items.Insert(insertIndex++, item);
			else
				parentItem.Items.Add(item);

			// Check if we need to add a following begin group item.
			if (beginGroupAfter)
			{
				if (insertIndex >= 0)
					parentItem.Items.Insert(insertIndex, new ToolStripSeparator());
				else
					parentItem.Items.Add(new ToolStripSeparator());
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make a new toolbar and initialize it based on what's in the XML
		/// definition.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="barName"></param>
		/// ------------------------------------------------------------------------------------
		private ToolStrip MakeNewBar(XmlNode node, string barName)
		{
			// Every bar must have a unique name so if we received a null or empty barName
			// then name it with a new guid.
			if (barName == null || barName == string.Empty)
				barName = Guid.NewGuid().ToString();

			ToolStrip bar = new ToolStrip();
			bar.Name = barName;
			bar.CanOverflow = true;
			bar.AccessibleName = barName;
			bar.Stretch = GetBoolFromAttribute(node, "stretch", false);

			bool allowCustomizing = GetBoolFromAttribute(node, "allowcustomizing", true);
			bar.AllowItemReorder = allowCustomizing;
			if (!allowCustomizing)
				bar.GripStyle = ToolStripGripStyle.Hidden;

			string barText = GetAttributeValue(node, "text");
			barText = GetStringFromResource(barText);
			TMBarProperties barProps = new TMBarProperties(barName, barText, true,
				GetBoolFromAttribute(node, "visible", true), m_parentControl);

			if (InitializeBar != null)
				InitializeBar(ref barProps);

			barProps.Update = true;
			SetBarProperties(bar, barProps);

			// Add this bar in the ordered list of bar locations. Combine the position on the row,
			// with the row to give a single integer that will put the bar in the list sorted from
			// left to right, top to bottom.
			uint pos = (uint)GetIntFromAttribute(node, "position", 0);
			uint row = (uint)GetIntFromAttribute(node, "row", m_bars.Count - 1);
			
			// Force the numbers to be unsigned shorts.
			pos &= 0xFFFF;
			row &= 0xFFFF;

			// The key is a 32-bit number with the position in the
			// high 16 bits and the row in the low 16 bits.
			m_barLocationOrder[(row << 16) | pos] = bar;

			m_bars[barName] = bar;

			return bar;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a toolbar item with the specified name. 
		/// </summary>
		/// <param name="node"></param>
		/// <param name="parentItem">The parent item of the one about to be created.</param>
		/// <param name="name">The name to be given the item about to be created.</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected ToolStripItem GetToolbarItem(XmlNode node, object parentItem, string name)
		{
			ToolStripItem item = null;
			
			int type = GetIntFromAttribute(node, "type", 0);

			// Get nasty if the type in the XML definition is bad.
			if (type < 0 || type > 5)
				throw new Exception(type + " is an invalid toolbar item type.");

			switch (type)
			{
				case 0:
					item = CreateNormalToolBarItem(parentItem);
					if (!(item is ToolStripMenuItem))
						item.DisplayStyle = GetToolBarItemDisplayStyle(node);
					break;

				case 1:
				case 2:
				case 3:
					item = CreateDropDownToolBarItem(node, type);
					item.DisplayStyle = GetToolBarItemDisplayStyle(node);
					break;
				
				case 4:
					// Create a combo box item.
					item = new ToolStripComboBox();
					ToolStripComboBox cboItem = item as ToolStripComboBox;
					cboItem.Width = GetIntFromAttribute(node, "width", 25);
					
					// Setting the height to 1 will will force height to minimum (but it won't be 1).
					cboItem.ComboBox.ItemHeight = 1;
					item.DisplayStyle = ToolStripItemDisplayStyle.None;
					break;

				case 5:
					// Create a host item for a custom control.
					item = GetCustomControlHost(name);
					break;
			}

			item.Name = name;
			return item;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a custom control and return it in a returned ToolStripControlHost.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private ToolStripControlHost GetCustomControlHost(string name)
		{
			Control ctrl = (LoadControlContainerItem != null ? LoadControlContainerItem(name) : null);
			if (ctrl == null)
			{
				ctrl = new Label();
				ctrl.Text = "Missing Control: " + name;
			}
			
			ToolStripControlHost host = new ToolStripControlHost(ctrl);
			host.AutoSize = false;
			host.Size = ctrl.Size;
			ctrl.Dock = DockStyle.Fill;
			host.Dock = DockStyle.Fill;
			host.Padding = Padding.Empty;
			host.Margin = Padding.Empty;
			return host;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a normal (in the sense that it's not a combo box, custom control, drop-down
		/// item, etc.) toolbar item.
		/// </summary>
		/// <param name="parentItem"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private static ToolStripItem CreateNormalToolBarItem(object parentItem)
		{
			ToolStripItem item;
			
			if (parentItem is ToolStripDropDownItem &&
				((ToolStripDropDownItem)parentItem).DropDown is ToolStripDropDownMenu)
			{
				// Create a toolbar button that is on a menu
				// dropped-down from a toolbar item.
				item = new ToolStripMenuItem();
				item.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
			}
			else
			{
				// Create a plain vanilla toolbar button.
				item = new ToolStripButton();
				item.DisplayStyle = ToolStripItemDisplayStyle.Image;
			}
			
			return item;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates one of three different types of drop-down toolbar items.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static ToolStripItem CreateDropDownToolBarItem(XmlNode node, int type)
		{
			ToolStripItem item;

			// True if the drop-down button is split into two segments, one for the arrow and
			// one for the icon. False if there is no behavioral distinction between the arrow
			// and icon portions of the button.
			bool split = GetBoolFromAttribute(node, "split", true);

			// Types 1 and 2 are drop-down buttons.
			if (split)
				item = new ToolStripSplitButton();
			else
				item = new ToolStripDropDownButton();

			switch (type)
			{
				case 1:
					// Create a drop-down that will act like a drop-down toolbar.
					ToolStripDropDown dropDown = new ToolStripDropDown();
					dropDown.LayoutStyle = ToolStripLayoutStyle.StackWithOverflow;
					((ToolStripDropDownItem)item).DropDown = dropDown;
					break;

				case 2:
					// Create a drop-down that will act like a drop-down menu.
					ToolStripDropDownMenu dropDownMenu = new ToolStripDropDownMenu();
					dropDownMenu.ShowImageMargin = GetBoolFromAttribute(node, "showimagemargin", true);
					dropDownMenu.ShowCheckMargin = GetBoolFromAttribute(node, "showcheckmargin", false);
					((ToolStripDropDownItem)item).DropDown = dropDownMenu;
					break;

				case 3:
					// Create a drop-down for a custom control.
					((ToolStripDropDownItem)item).DropDown = new CustomDropDown();
					break;
			}

			item.DisplayStyle = ToolStripItemDisplayStyle.Image;
			return item;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a display style read from a toolbar item node.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static ToolStripItemDisplayStyle GetToolBarItemDisplayStyle(XmlNode node)
		{
			switch (GetAttributeValue(node, "style"))
			{
				case "textonly": return ToolStripItemDisplayStyle.Text;
				case "both": return ToolStripItemDisplayStyle.ImageAndText;
				default: return ToolStripItemDisplayStyle.Image;
			}
		}

		#endregion

		#region Methods for reading XML definition for single toolbar/menu item
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read the information for a single menu item from the specified node.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="parentItem">The parent item to the one that is about to be created.</param>
		/// <param name="isMenuItem">True if the menu section of the XML definition is
		/// being read. Otherwise, false.</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected ToolStripItem ReadSingleItem(XmlNode node, object parentItem, bool isMenuItem)
		{
			string name = GetAttributeValue(node, "name");
			int displayType = GetIntFromAttribute(node, "displaytype", (isMenuItem ? 2 : 0));

			// Give the item a guid for the name if one wasn't found in the XML def.
			if (name == null || name == string.Empty)
				name = Guid.NewGuid().ToString();
			
			// If the item is for a menu just make a menu item since that's all we allow
			// on menus. Otherwise it must be a toolbar item, so go make the appropriate
			// type of toolbar item.
			
			ToolStripItem item;

			if (!isMenuItem)
				item = GetToolbarItem(node, parentItem, name);
			else
			{
				item = new ToolStripMenuItem();

				if (GetBoolFromAttribute(node, "windowlist"))
					((MenuStrip)parentItem).MdiWindowListItem = item as ToolStripMenuItem;

				if (GetIntFromAttribute(node, "type", -1) == 3)
				{
					ToolStripControlHost host = GetCustomControlHost(name);
					if (host != null)
					{
						((ToolStripMenuItem)item).DropDown = new CustomDropDown();
						((ToolStripMenuItem)item).DropDown.Items.Add(host);
					}
				}
			}

			switch (displayType)
			{
				case 0: item.DisplayStyle = ToolStripItemDisplayStyle.Image; break;
				case 1: item.DisplayStyle = ToolStripItemDisplayStyle.Text; break;
				case 2: item.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText; break;
			}

			bool rightAligned = GetBoolFromAttribute(node, "rightaligned", false);
			item.Alignment = (rightAligned ? ToolStripItemAlignment.Right : ToolStripItemAlignment.Left);
			
			int leftMargin = GetIntFromAttribute(node, "leftmargin", item.Margin.Left);
			int topMargin = GetIntFromAttribute(node, "topmargin", item.Margin.Top);
			int rightMargin = GetIntFromAttribute(node, "rightmargin", item.Margin.Right);
			int bottomMargin = GetIntFromAttribute(node, "bottommargin", item.Margin.Bottom);
			item.Margin = new Padding(leftMargin, topMargin, rightMargin, bottomMargin);
			item.AutoSize = GetBoolFromAttribute(node, "autosize", item.AutoSize);

			item.Name = name;
			InitItem(node, item, name);

			if (isMenuItem && item.DisplayStyle != ToolStripItemDisplayStyle.Image)
				item.ToolTipText = null;

			return item;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method initializes a toolbar or menu item's properties.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="item">Item to be initialized.</param>
		/// <param name="name"></param>
		/// ------------------------------------------------------------------------------------
		private void InitItem(XmlNode node, ToolStripItem item, string name)
		{
			string commandid = GetAttributeValue(node, "commandid");
			bool visible = GetBoolFromAttribute(node, "visible", true);

			item.Tag = commandid;
			item.Name = name;
			item.AccessibleName = name;
			
			TMItemProperties itemProps = new TMItemProperties();
			itemProps.ParentControl = m_parentControl;
			itemProps.Name = name;
			itemProps.CommandId = commandid;
			itemProps.Enabled = true;
			itemProps.Visible = visible;
			itemProps.Size = item.Size;

			CommandInfo cmdInfo = m_commands[commandid];
			if (cmdInfo != null)
			{
				itemProps.Text = cmdInfo.Text;
				itemProps.Category = cmdInfo.Category;
				itemProps.Tooltip = cmdInfo.ToolTip;
				itemProps.Image = cmdInfo.Image;

				if (cmdInfo.ShortcutKey != Keys.None && item is ToolStripMenuItem)
				{
					itemProps.ShortcutKey = cmdInfo.ShortcutKey;
					((ToolStripMenuItem)item).ShortcutKeys = cmdInfo.ShortcutKey;
				}
			}

			if (GetBoolFromAttribute(node, "toolbarlist"))
				m_toolbarListItem = (item as ToolStripMenuItem);

			if (item is ToolStripComboBox)
			{
				item.AccessibleRole = AccessibleRole.ComboBox;
				itemProps.Control = ((ToolStripComboBox)item).ComboBox;
			}
			else if (item is ToolStripDropDownButton)
			{
				((ToolStripDropDownButton)item).DropDownOpening += HandleItemsPopup;
				((ToolStripDropDownButton)item).DropDownClosed += HandleDropDownClosed;
			}
			else if (item is ToolStripSplitButton)
			{
				((ToolStripSplitButton)item).ButtonClick += HandleItemClicks;
				((ToolStripSplitButton)item).DropDownOpening += HandleItemsPopup;
				((ToolStripSplitButton)item).DropDownClosed += HandleDropDownClosed;
			}
			else if (!(item is ToolStripTextBox))
			{
				item.Click += HandleItemClicks;

				if (item is ToolStripMenuItem)
				{
					((ToolStripMenuItem)item).DropDownOpening += HandleMenuPopups;
					((ToolStripMenuItem)item).DropDownClosed += HandleDropDownClosed;
				}
			}
			
			// Let the application have a stab at initializing the item.
			if (InitializeItem != null)
				InitializeItem(ref itemProps);

			// Save all initializatons by updating the item.
			itemProps.Update = true;
			SetItemProps(item, itemProps);

			if (LocalizeItem != null)
			{
				string id = commandid;
				if (id.StartsWith("Cmd"))
					id = id.Substring(3);

				if (item.DisplayStyle == ToolStripItemDisplayStyle.Image ||
					item.DisplayStyle == ToolStripItemDisplayStyle.None)
				{
					item.Text = string.Empty;
				}

				LocalizeItem(item, BuildLocalizationId(item, id), itemProps);
			}

			m_items[name] = item;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds the localization id for the specified item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string BuildLocalizationId(object item, string id)
		{
			if (id == "ShowRecordPane")
				System.Diagnostics.Debug.WriteLine("here");


			if (item is ToolStripMenuItem || item is ToolStripDropDownItem ||
				item is ToolStripDropDownMenu)
			{
				return "MenuItems." + id;
			}

			if (item is ToolStripButton || item is ToolStripSplitButton ||
				item is ToolStripDropDownButton)
			{
				return "ToolbarButtons." + id;
			}

			if (item is ToolStripComboBox)
				return "ToolbarComboBoxes." + id;

			return id;
		}

		#endregion

		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method handles clicks on toolbar/menu items.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void HandleItemClicks(object sender, EventArgs e)
		{
			ToolStripItem item = sender as ToolStripItem;
			string message = GetItemsCommandMessage(item);

			// Ignore the click on the toolbar list item because it has sub menu items. It has
			// a command ID that's used by it's sub items and for that reason, message isn't
			// null. But when the item is the toolbar list item, we have to ignore the click
			// and that's why we test it explicitly here. Confusing?
			if (item == null || item == m_toolbarListItem || message == string.Empty || m_msgMediator == null)
				return;

			TMItemProperties itemProps = GetItemProps(item);

			// If the user clicked on one of the toolbar items in the toolbar menu list
			// then save the toolbar's name in the tag property.
			if (item.Name.EndsWith(kToolbarItemSuffix))
				itemProps.Tag = item.Name.Replace(kToolbarItemSuffix, string.Empty);

			if (m_msgMediator.SendMessage(message, itemProps) && itemProps.Update)
				SetItemProperties(item.Name, itemProps);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// While the system is idle (or about to become so), this method will go through all
		/// the toolbar items and make sure they are enabled properly for the current view.
		/// Then it will go through the menu items with shortcuts to make sure they're enabled
		/// properly. (This is only done every 0.75 seconds.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleItemUpdates(object sender, EventArgs e)
		{
			if (m_msgMediator == null || !AllowUpdates || DateTime.Now < m_itemUpdateTime.AddSeconds(0.75))
				return;

			m_itemUpdateTime = DateTime.Now;

			// Loop through all the toolbars and items on toolbars.
			foreach (ToolStrip bar in m_bars.Values)
			{
				if (bar.Items != null && bar.Visible)
				{
					foreach (ToolStripItem item in bar.Items)
						CallItemUpdateHandler(item);
				}
			}

			// Update menus with shortcut keys.
			for (int i = 0; i < m_menusWithShortcuts.Count; i++)
				CallItemUpdateHandler(m_menusWithShortcuts[i] as ToolStripMenuItem);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Trap this event so we can clear the toolbar list item when it closes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void HandleToolBarListMenuClosing(object sender, EventArgs e)
		{
			m_toolbarListItem.DropDownItems.Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles clicks on the drop-down portion of a split button (i.e. a click on the
		/// arrow portion of a two-segmented toolbar button) or clicks on drop-down buttons.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void HandleItemsPopup(object sender, EventArgs e)
		{
			if (m_msgMediator == null)
				return;

			// Handle control hosts specially.
			if (sender is ToolStripControlHost)
			{
				HandleControlHostPopup(sender as ToolStripControlHost);
				return;
			}

			ToolStripDropDownItem item = sender as ToolStripDropDownItem;
			if (item == null)
				return;

			string message = GetItemsCommandMessage(item);

			//// This deals with drop-down toolbar items whose drop-down is a custom control.
			//if (item.DropDown is CustomDropDown && item.DropDown.Items.Count > 0 &&
			//    item.DropDown.Items[0] is ToolStripControlHost)
			//{
			//    HandleControlHostPopup(item.DropDown.Items[0] as ToolStripControlHost, message);
			//    return;
			//}

			if (!string.IsNullOrEmpty(message))
			{
				ToolBarPopupInfo popupInfo = new ToolBarPopupInfo(item.Name);
				if (m_msgMediator.SendMessage("DropDown" + message, popupInfo))
				{
					if (popupInfo.Control != null && item.DropDown is CustomDropDown)
						((CustomDropDown)item.DropDown).AddControl(popupInfo.Control);
				}
			}

			if (item.DropDownItems.Count == 0)
				return;

			foreach (ToolStripItem subItem in item.DropDownItems)
			{
				if (!(subItem is ToolStripSeparator))
					CallItemUpdateHandler(subItem);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles a control host about to be popped-up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleControlHostPopup(ToolStripControlHost host)
		{
			HandleControlHostPopup(host, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles a control host about to be popped-up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleControlHostPopup(ToolStripControlHost host, string message)
		{
			if (string.IsNullOrEmpty(message))
				message = GetItemsCommandMessage(host);

			if (!string.IsNullOrEmpty(message))
			{
				TMItemProperties itemProps = GetItemProps(host);
				if (m_msgMediator.SendMessage("DropDown" + message, itemProps))
				{
					// Save the item properties for reference in the VisibleChange event.
					host.Owner.Tag = itemProps;
					host.Owner.VisibleChanged += new EventHandler(ControlHostOwnerVisibleChanged);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Monitor when a custom popup closes and sends a message informing anyone who cares.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ControlHostOwnerVisibleChanged(object sender, EventArgs e)
		{
			CustomDropDown dropDown = sender as CustomDropDown;
			
			// We only care about when the drop-down closes.
			if (dropDown == null || dropDown.Visible)
				return;

			if (dropDown.Tag is TMItemProperties)
			{
				//if (dropDown.OwnerItem.Owner != null)
				//    dropDown.OwnerItem.Owner.Hide();
				
				TMItemProperties itemProps = dropDown.Tag as TMItemProperties;
				if (itemProps != null)
					m_msgMediator.SendMessage("DropDownClosed" +  itemProps.Message, itemProps);
			}

			dropDown.VisibleChanged -= ControlHostOwnerVisibleChanged;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Send a message to anyone who cares that the drop down is closing.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//void HandleDropDownClosed(object sender, EventArgs e)
		//{
		//    ToolStripItem item = sender as ToolStripItem;
		//    string message = GetItemsCommandMessage(item);

		//    if (item != null && message != string.Empty && m_msgMediator != null)
		//    {
		//        TMItemProperties itemProps = GetItemProps(item);

		//        if (m_msgMediator.SendMessage("DropDownClosed" + message, itemProps) && itemProps.Update)
		//            SetItemProperties(item.Name, itemProps);
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When a menu item is popped-up, then cycle through the subitems and call update
		/// handlers.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void HandleMenuPopups(object sender, EventArgs e)
		{
			ToolStripMenuItem item = sender as ToolStripMenuItem;

			if (item == null || m_msgMediator == null)
				return;

			// If we're popping-up the toolbar list, then build it.
			if (m_toolbarListItem != null && item == m_toolbarListItem.OwnerItem)
				BuildToolbarList();

			foreach (ToolStripItem subItem in item.DropDownItems)
			{
				// Don't bother sending a message for the recently used marker item.
				if (subItem == m_rufMarkerItem)
					continue;

				// If the item is the separator before the recently used file list and
				// there are no items in the recenlty used files list, hide the item.
				if (subItem == m_rufMarkerSeparator)
				{
					subItem.Visible = (RecentFilesList != null);
					continue;
				}

				if ((subItem is ToolStripControlHost && item.DropDown is CustomDropDown) ||
					(subItem is ToolStripMenuItem &&
					((ToolStripMenuItem)subItem).DropDown is CustomDropDown))
				{
					HandleItemsPopup(subItem, EventArgs.Empty);
				}
				else
				{
					if (!(subItem is ToolStripSeparator))
						CallItemUpdateHandler(subItem);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Call the update handlers for each menu item on the context menu. I chose to do
		/// this in the opened event rather than the opening event so applications would have
		/// a chance to be notified of the menu being opened before the update handlers are
		/// called.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleContextMenuOpened(object sender, EventArgs e)
		{
			ContextMenuStrip cmnu = sender as ContextMenuStrip;

			if (cmnu == null || m_msgMediator == null)
				return;

			foreach (ToolStripItem subItem in cmnu.Items)
			{
				if (subItem is ToolStripMenuItem && ((ToolStripMenuItem)subItem).DropDown is CustomDropDown)
					HandleItemsPopup(subItem, EventArgs.Empty);
				else
				{
					if (!(subItem is ToolStripSeparator))
						CallItemUpdateHandler(subItem);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildToolbarList()
		{
			TMBarProperties[] barProps = BarInfoForViewMenu;

			for (int i = 0; i < barProps.Length; i++)
			{
				ToolStripMenuItem item = new ToolStripMenuItem(barProps[i].Text);
			    item.Name = barProps[i].Name + kToolbarItemSuffix;
			    item.Checked = barProps[i].Visible;
			    item.Tag = m_toolbarListItem.Tag;
				item.Click += HandleItemClicks;
				m_toolbarListItem.DropDownItems.Add(item);
			}
		}

		#endregion

		#region Method for calling update command handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is a helper method for the HandleMenuPopups and HandleItemUpdates methods. It
		/// accepts a single DNB item and calls it's update handler, if there is one.
		/// </summary>
		/// <param name="item"></param>
		/// ------------------------------------------------------------------------------------
		protected void CallItemUpdateHandler(ToolStripItem item)
		{
			string message = GetItemsCommandMessage(item);
			if (message == string.Empty || m_msgMediator == null)
				return;
			
			TMItemProperties itemProps = GetItemProps(item);

			// If the item being updated is one of the toolbar items in the toolbar menu
			// list then save the toolbar's name in the tag property.
			if (item.Name.EndsWith(kToolbarItemSuffix))
				itemProps.Tag = item.Name.Replace(kToolbarItemSuffix, string.Empty);

			// Call update method (e.g. OnUpdateEditCopy). If that method doesn't exist or
			// if all update methods return false, we check for the existence of the
			// command handler.
			if (m_msgMediator.SendMessage("Update" + message, itemProps))
				SetItemProps(item, itemProps);
			else
			{
				ToolStripMenuItem menuItem = item as ToolStripMenuItem;

				// If the item is a menu item on the main menu or has sub items, then don't disable
				// it. Menu items with sub items often don't have receivers so we shouldn't
				// assume it should be disabled just because it doesn't have a receiver.
				if (menuItem == null || menuItem.OwnerItem == null || menuItem.DropDownItems.Count > 0)
					return;

				// If the item is not a menu and there's no receiver then automatically disable it.
				item.Enabled = m_msgMediator.HasReceiver(message);
			}
		}

		#endregion

		#region Methods for Getting/Setting menu/toolbar item properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads a TMItemProperties object with several properties from a toolbar item.
		/// </summary>
		/// <param name="name">The Name of the item whose properties are being stored.</param>
		/// <returns>The properties of a menu or toolbar item.</returns>
		/// ------------------------------------------------------------------------------------
		public TMItemProperties GetItemProperties(string name)
		{
			return (m_items.ContainsKey(name) ? GetItemProps(m_items[name]) : null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads a TMItemProperties object with several properties from a toolbar item.
		/// </summary>
		/// <param name="item">The item whose properties are being stored.</param>
		/// <returns>The properties of a menu or toolbar item.</returns>
		/// ------------------------------------------------------------------------------------
		private TMItemProperties GetItemProps(ToolStripItem item)
		{
			TMItemProperties itemProps = new TMItemProperties();

			// Set default values.
			itemProps.Update = false;
			itemProps.Name = string.Empty;
			itemProps.Text = string.Empty;
			itemProps.OriginalText = string.Empty;
			itemProps.Category = string.Empty;
			itemProps.Tooltip = string.Empty;
			itemProps.Enabled = false;
			itemProps.Visible = false;
			itemProps.Checked = false;
			itemProps.Image = null;
			itemProps.CommandId = null;
			itemProps.Control = null;
			itemProps.List = null;
			itemProps.ParentControl = m_parentControl;
			itemProps.Size = Size.Empty;

			if (item == null)
				return itemProps;

			itemProps.Name = item.Name;
			itemProps.Text = item.Text;
			itemProps.CommandId = item.Tag as string;
			itemProps.Tooltip = item.ToolTipText;
			itemProps.Enabled = item.Enabled;
			itemProps.Visible = item.Visible;
			itemProps.Image = item.Image;
			itemProps.Tag = m_htItemTags[item];
			itemProps.Size = item.Size;

			//TODO:	itemProps.BeginGroup = item.BeginGroup;

			CommandInfo cmdInfo = GetCommandInfo(item);
			if (cmdInfo != null)
			{
				if (cmdInfo.Message != null)
					itemProps.Message = cmdInfo.Message;
				if (cmdInfo.Text != null)
					itemProps.OriginalText = cmdInfo.Text;
			}

			if (item is ToolStripButton)
				itemProps.Checked = ((ToolStripButton)item).Checked;
			else if (item is ToolStripMenuItem)
			{
				itemProps.Checked = ((ToolStripMenuItem)item).Checked;
				itemProps.ShortcutKey = ((ToolStripMenuItem)item).ShortcutKeys;
			}
			else if (item is ToolStripComboBox)
			{
				ToolStripComboBox cboItem = item as ToolStripComboBox;
				itemProps.Control = cboItem.ComboBox;
				if (cboItem.Items.Count > 0)
				{
					// Get all the combo items and save in the List property.
					itemProps.List = new ArrayList();
					for (int i = 0; i < cboItem.Items.Count; i++)
						itemProps.List.Add(cboItem.Items[i]);
				}
			}
			else if (item is ToolStripControlHost)
				itemProps.Control = ((ToolStripControlHost)item).Control;

			//REVIEW: should this return null if item is null?
			return itemProps;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets some of a toolbar item's properties. Note: if the Update property in the
		/// TMItemProperties object hasn't been set to true, no updating will occur.
		/// </summary>
		/// <param name="name">Name of item whose properties are being updated.</param>
		/// <param name="itemProps">The TMItemProperties containing the new property
		/// values for the toolbar item.</param>
		/// ------------------------------------------------------------------------------------
		public void SetItemProperties(string name, TMItemProperties itemProps)
		{
			SetItemProps(m_items[name], itemProps);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets some of a menu/toolbar item's properties. Note: if the Update property in the
		/// TMItemProperties object hasn't been set to true, no updating will occur. Items that
		/// can be set are: Text, Category, Tooltip, Enabled, Visible, BeginGroup, Checked,
		/// Image, List, Tag, and CommandId;
		/// </summary>
		/// <param name="item">The item whose properties are being updated.</param>
		/// <param name="itemProps">The TMItemProperties containing the new property
		/// values for the toolbar item.</param>
		/// ------------------------------------------------------------------------------------
		private void SetItemProps(ToolStripItem item, TMItemProperties itemProps)
		{
			if (item == null || !itemProps.Update)
				return;

			if (item.Tag as string != itemProps.CommandId)
			{
				item.Tag = itemProps.CommandId;

				// Since we just changed the command ID, we should change the item's
				// image if the image isn't already being specified in the item properties.
				CommandInfo cmdInfo = GetCommandInfo(item);
				if (cmdInfo != null && itemProps.Image == null)
					itemProps.Image = cmdInfo.Image;
			}
			
			m_htItemTags[item] = itemProps.Tag;

			// Update all the changed fields only if necessary.

			if (item.Image != itemProps.Image)
			{
				item.Image = itemProps.Image;

				// For some reason, setting the image scaling to none when there
				// isn't an image causes a crash. Sounds like a bug in .Net to me.
				item.ImageScaling = (item.Image != null ?
					ToolStripItemImageScaling.None : ToolStripItemImageScaling.SizeToFit);
			}

			if (item.Text != itemProps.Text)
				item.Text = itemProps.Text;

			if (item.Size != itemProps.Size)
				item.Size = itemProps.Size;

			// Don't show tooltips for menu items (either on a menu or a toolbar
			// item's drop-down) unless the item's text is not being shown.
			if ((item is ToolStripMenuItem && item.DisplayStyle != ToolStripItemDisplayStyle.Image &&
				item.DisplayStyle != ToolStripItemDisplayStyle.ImageAndText) ||
				item is ToolStripControlHost || item.IsOnDropDown)
			{
				item.ToolTipText = null;
			}
			else if (item.ToolTipText != itemProps.Tooltip)
				item.ToolTipText = itemProps.Tooltip;

			if (item is ToolStripButton && ((ToolStripButton)item).Checked != itemProps.Checked)
				((ToolStripButton)item).Checked = itemProps.Checked;
			else if (item is ToolStripMenuItem)
			{
				ToolStripMenuItem menuItem = item as ToolStripMenuItem;
				
				if (menuItem.Checked != itemProps.Checked)
					menuItem.Checked = itemProps.Checked;
		
				if (menuItem.ShortcutKeys != itemProps.ShortcutKey)
					menuItem.ShortcutKeys = itemProps.ShortcutKey;
			}

			if (item.Enabled != itemProps.Enabled)
				item.Enabled = itemProps.Enabled;

			if (item.Visible != itemProps.Visible || !item.Visible)
				item.Visible = itemProps.Visible;

			//TODO: if (item.BeginGroup != itemProps.BeginGroup)
			//    item.BeginGroup = itemProps.BeginGroup;

			if (item is ToolStripComboBox)
				SetComboItemSpecificProperties(item as ToolStripComboBox, itemProps);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the items in the Items collection for a ComboBoxItem type toolbar item.
		/// </summary>
		/// <param name="item">The combo box item whose Items collection is be updated.</param>
		/// <param name="itemProps">The TMItemProperties containing the new property
		/// values for the toolbar item.</param>
		/// ------------------------------------------------------------------------------------
		private void SetComboItemSpecificProperties(ToolStripComboBox item,
			TMItemProperties itemProps)
		{
			if (item == null)
				return;

			// First check if the lists are the same. If they are we don't want to
			// go to the trouble of rebuilding the list, especially since that will
			// cause unnecessary flicker.
			if (itemProps.List != null && itemProps.List.Count == item.Items.Count)
			{
				bool areSame = true;
				for (int i = 0; i < item.Items.Count || !areSame; i++)
					areSame = (item.Items[i] == itemProps.List[i]);

				if (areSame)
					return;
			}
			
			item.Items.Clear();

			// If there are item's in the list then upate the combobox item's
			// collection of items.
			if (itemProps.List != null && itemProps.List.Count > 0)
			{
				for (int i = 0; i < itemProps.List.Count; i++)
					item.Items.Add(itemProps.List[i]);
			}
		}

		#endregion

		#region Misc. Helper Methods and Property
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		internal CommandInfo GetCommandInfo(ToolStripItem item)
		{
			if (item != null)
			{
				string commandId = item.Tag as string;
				if (commandId != null)
					return m_commands[commandId];
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an item's command handling message from the appropriate hash table entry.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected string GetItemsCommandMessage(ToolStripItem item)
		{
			string message = string.Empty;

			CommandInfo cmdInfo = GetCommandInfo(item);
			if (cmdInfo != null && cmdInfo.Message != null)
				message = cmdInfo.Message;

			return message;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected string GetStringFromResource(string kstid)
		{
			return GetStringFromResource(kstid, kstid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected string GetStringFromResource(string kstid, string defaultValue)
		{
			if (kstid == null || kstid.Trim() == string.Empty)
				return null;

			string localizedStr = kstid;

			for (int i = 0; i < m_rmlocalStrings.Count; i++)
			{
				localizedStr = ((ResourceManager)m_rmlocalStrings[i]).GetString(kstid);
				if (localizedStr != null)
					break;
			}
				
			if (string.IsNullOrEmpty(localizedStr))
				localizedStr = defaultValue;
			
			return (localizedStr != null ? localizedStr.Trim() : null);
		}

		#endregion

		#region ITMAdapter AddCommandItem, AddMenuItem and RemoveSubitems implementation
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new command item to the adapter.
		/// </summary>
		/// <param name="cmdId">Command ID or name (usually starts with "Cmd")</param>
		/// <param name="message">item's command message</param>
		/// <param name="text">text of item (whether menu or toolbar item</param>
		/// <param name="textAlt">alternate text (trumps text)</param>
		/// <param name="contextMenuText">context menu text</param>
		/// <param name="toolTipText">tooltip for item</param>
		/// <param name="category">category of item</param>
		/// <param name="statusMsg">status bar message of item</param>
		/// <param name="shortcutKey">shortcut key for item</param>
		/// <param name="imageLabel"></param>
		/// <param name="image">image of item</param>
		/// ------------------------------------------------------------------------------------
		public void AddCommandItem(string cmdId, string message, string text, string textAlt,
			string contextMenuText, string toolTipText, string category, string statusMsg,
			Keys shortcutKey, string imageLabel, Image image)
		{
			if (m_commands.ContainsKey(cmdId))
				return;

			CommandInfo cmdInfo = new CommandInfo();
			cmdInfo.Message = message;
			cmdInfo.Text = text;
			cmdInfo.TextAlt = textAlt;
			cmdInfo.ContextMenuText = contextMenuText;
			cmdInfo.Category = category;
			cmdInfo.ToolTip = toolTipText;
			cmdInfo.ShortcutKey = shortcutKey;
			cmdInfo.StatusMsg = statusMsg;
			cmdInfo.Image = image;

			if (image != null)
				cmdInfo.Image = image;
			else if (imageLabel != null)
			{
				CommandInfo ci;
				if (m_commands.TryGetValue(imageLabel, out ci))
					cmdInfo.Image = ci.Image;
			}

			m_commands[cmdId] = cmdInfo;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new submenu item to the menu specified by parentItemName and inserts it
		/// before the item specified by insertBeforeItem. If insertBeforeItem is null, then
		/// the new submenu item is added to the end of parentItemName's menu collection. 
		/// </summary>
		/// <param name="itemProps">Properties of the new menu item.</param>
		/// <param name="parentItemName">Name of the menu item that will be added to.</param>
		/// <param name="insertBeforeItem">Name of the submenu item before which the new
		/// menu item will be added.</param>
		/// ------------------------------------------------------------------------------------
		public void AddMenuItem(TMItemProperties itemProps, string parentItemName, string insertBeforeItem)
		{
			ToolStripMenuItem item = new ToolStripMenuItem();
			item.Click += HandleItemClicks;
			item.DropDownOpening += HandleMenuPopups;
			item.DropDownClosed += HandleDropDownClosed;
			item.Name = itemProps.Name;

			CommandInfo ci;

			if (itemProps.Text == null)
			{
				if (m_commands.TryGetValue(itemProps.CommandId, out ci))
					itemProps.Text = (ci.TextAlt ?? ci.Text);
			}

			if (itemProps.ShortcutKey != Keys.None)
				item.ShortcutKeys = itemProps.ShortcutKey;
			else
			{
				if (m_commands.TryGetValue(itemProps.CommandId, out ci))
					itemProps.ShortcutKey = ci.ShortcutKey;
			}

			itemProps.Update = true;
			SetItemProps(item, itemProps);

			// If an item to insert before isn't specified, then add the item to the item to the
			// parent item specified. Otherwise, insert before "insertBeforeItem".
			if (insertBeforeItem == null || insertBeforeItem.Trim() == string.Empty)
				AddMenuItem(item, parentItemName, itemProps.BeginGroup);
			else
				InsertMenuItem(item, insertBeforeItem, itemProps.BeginGroup, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all the subitems of the specified menu.
		/// </summary>
		/// <param name="parentItemName">The name of the item whose subitems will be removed.
		/// </param>
		/// ------------------------------------------------------------------------------------
		public void RemoveMenuSubItems(string parentItemName)
		{
			if (!m_items.ContainsKey(parentItemName))
				return;

			ToolStripDropDownItem item = m_items[parentItemName] as ToolStripDropDownItem;

			if (item != null)
			{
				for (int i = 0; i < item.DropDownItems.Count; i++)
				{
					item.DropDownItems[i].Click -= HandleItemClicks;

					if (item.DropDownItems[i] is ToolStripDropDownItem)
					{
						((ToolStripDropDownItem)item.DropDownItems[i]).DropDownOpened -= HandleMenuPopups;
						((ToolStripDropDownItem)item.DropDownItems[i]).DropDownClosed -= HandleDropDownClosed;
					}
				}

				item.DropDownItems.Clear();
			}
		}

		#endregion
		
		#region ITMAdpater popup and context menu implementation
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the context menu for a specified control.
		/// </summary>
		/// <param name="ctrl">Control which is being assigned a context menu.</param>
		/// <param name="name">The name of the context menu to assign to the control.</param>
		/// ------------------------------------------------------------------------------------
		public void SetContextMenuForControl(Control ctrl, string name)
		{
			ContextMenuStrip cmnu;
			if (m_cmenus.TryGetValue(name, out cmnu))
				ctrl.ContextMenuStrip = cmnu;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Pops-up a menu so it shows like a context menu. If the item doesn't have any
		/// sub items, then this command is ignored.
		/// </summary>
		/// <param name="name">The name of the item to popup. The name could be the name of
		/// a menu off the application's menu bar, or one of the context menu's added to the
		/// menu adapter.</param>
		/// <param name="x">The X location (on the screen) where the menu is popped-up.</param>
		/// <param name="y">The Y location (on the screen) where the menu is popped-up.</param>
		/// ------------------------------------------------------------------------------------
		public void PopupMenu(string name, int x, int y)
		{
			// First see if it's a context menu.
			if (m_cmenus.ContainsKey(name))
				m_cmenus[name].Show(x, y);
			else if (m_items.ContainsKey(name))
			{
				// It's not a context menu so check if we can
				// drop down another item with the same name.
				ToolStripDropDownItem item = m_items[name] as ToolStripDropDownItem;
				if (item != null)
				{
					HandleItemsPopup(item, EventArgs.Empty);
					item.DropDown.Show(x, y);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Send a message to anyone who cares that the drop down is closing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleDropDownClosed(object sender, EventArgs e)
		{
			ToolStripItem item = sender as ToolStripItem;
			string message = GetItemsCommandMessage(item);

			if (item != null && message != string.Empty && m_msgMediator != null)
			{
				TMItemProperties itemProps = GetItemProps(item);

				if (m_msgMediator.SendMessage("DropDownClosed" + message, itemProps) && itemProps.Update)
					SetItemProperties(item.Name, itemProps);
			}
		}

		#endregion

		#region ITMAdpater methods for Getting/Setting toolbar properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the properties of a toolbar.
		/// </summary>
		/// <param name="name">Name of the toolbar whose properties are being requested.
		/// </param>
		/// <returns>The properties of the toolbar.</returns>
		/// ------------------------------------------------------------------------------------
		public TMBarProperties GetBarProperties(string name)
		{
			try
			{
				ToolStrip bar = m_bars[name];
				return new TMBarProperties(name, bar.Text, bar.Enabled, bar.Visible, m_parentControl);
			}
			catch{}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the properties of the main menu bar.
		/// </summary>
		/// <param name="barProps">Properties used to modfy the toolbar item.</param>
		/// ------------------------------------------------------------------------------------
		public void SetBarProperties(TMBarProperties barProps)
		{
			foreach (ToolStrip bar in m_bars.Values)
				SetBarProperties(bar, barProps);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets properties for a menu bar or toolbar.
		/// </summary>
		/// <param name="name">Name of bar to update.</param>
		/// <param name="barProps">New properties of bar.</param>
		/// ------------------------------------------------------------------------------------
		public void SetBarProperties(string name, TMBarProperties barProps)
		{
			try
			{
				SetBarProperties(m_bars[name], barProps);
			}
			catch {}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets properties for a menu bar or toolbar.
		/// </summary>
		/// <param name="bar">Bar to update.</param>
		/// <param name="barProps">New properties of bar.</param>
		/// ------------------------------------------------------------------------------------
		private void SetBarProperties(ToolStrip bar, TMBarProperties barProps)
		{
			if (bar == null || barProps == null || !barProps.Update)
				return;

			if (barProps.Text != bar.Text)
				bar.Text = barProps.Text;

			if (barProps.Enabled != bar.Enabled)
				bar.Enabled = barProps.Enabled;

			if (barProps.Visible != bar.Visible)
				bar.Visible = barProps.Visible;
		}

		#endregion

		#region Misc. ITMAdapter methods for tool bars
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the control contained within a control container toolbar item.
		/// </summary>
		/// <param name="name">Name of the control container item.</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public Control GetBarItemControl(string name)
		{
			ToolStripControlHost item = m_items[name] as ToolStripControlHost;
			return (item == null ? null : item.Control);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Allows an application to force the hiding of a toolbar item's popup control.
		/// This can also be used to hide a context menu.
		/// </summary>
		/// <param name="name">Name of item whose popup should be hidden.</param>
		/// ------------------------------------------------------------------------------------
		public void HideBarItemsPopup(string name)
		{
			try
			{
				if (m_cmenus.ContainsKey(name))
					m_cmenus[name].Hide();

				ToolStripDropDownItem tsddi = m_items[name] as ToolStripDropDownItem;
				if (tsddi != null)
					tsddi.HideDropDown();
				else
				{
					ToolStripMenuItem tsmi = m_items[name] as ToolStripMenuItem;
					if (tsmi != null)
						tsmi.HideDropDown();
				}
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Causes the adapter to show it's dialog for customizing toolbars.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ShowCustomizeDialog()
		{
			// Not supported.
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Allows an application to hide of a toolbar.
		/// </summary>
		/// <param name="name">Name of toolbar to hide.</param>
		/// ------------------------------------------------------------------------------------
		public void HideToolBar(string name)
		{
			try
			{
				m_bars[name].Visible = false;
			}
			catch {}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Allows an application to show of a toolbar.
		/// </summary>
		/// <param name="name">Name of toolbar to show.</param>
		/// ------------------------------------------------------------------------------------
		public void ShowToolBar(string name)
		{
			try
			{
				m_bars[name].Visible = true;
			}
			catch {}
		}

		#endregion

		#region XML Helper Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Skips over white space and any other junk that's not considered an element or
		/// end element.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected static XmlNode ReadOverJunk(XmlNode node)
		{
			while (node != null &&
				(node.NodeType == XmlNodeType.Whitespace || node.NodeType == XmlNodeType.Comment))
			{
				node = node.NextSibling;
			}

			return node;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an attribute's value from the specified node.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="attribute"></param>
		/// <returns>String value of the attribute or null if it cannot be found.</returns>
		/// ------------------------------------------------------------------------------------
		protected static string GetAttributeValue(XmlNode node, string attribute)
		{
			if (node == null || node.Attributes[attribute] == null)
				return null;

			return node.Attributes.GetNamedItem(attribute).Value.Trim();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <param name="attribute"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected static bool GetBoolFromAttribute(XmlNode node, string attribute)
		{
			return GetBoolFromAttribute(node, attribute, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <param name="attribute"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected static bool GetBoolFromAttribute(XmlNode node, string attribute, bool defaultValue)
		{
			string val = GetAttributeValue(node, attribute);
			return (val == null ? defaultValue : val.ToLower() == "true");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected static int GetIntFromAttribute(XmlNode node, string attribute, int defaultValue)
		{
			string val = GetAttributeValue(node, attribute);
			return (val == null ? defaultValue : int.Parse(val));
		}

		#endregion

		#region Dispose Method
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			Application.Idle -= HandleItemUpdates;

			if (m_parentControl != null)
			{
				m_parentControl.Controls.Remove(m_menuBar);
				m_parentControl.Controls.Remove(m_tspLeft);
				m_parentControl.Controls.Remove(m_tspRight);
				m_parentControl.Controls.Remove(m_tspTop);
				m_parentControl.Controls.Remove(m_tspBottom);
			}

			if (m_menuBar != null)
			{
				m_menuBar.Items.Clear();
				m_menuBar.Dispose();
			}

			if (m_items != null)
			{
				foreach (ToolStripItem item in m_items.Values)
					item.Dispose();

				m_items.Clear();
			}

			if (m_bars != null)
			{
				foreach (ToolStrip bar in m_bars.Values)
					bar.Dispose();

				m_bars.Clear();
			}

			if (m_cmenus != null)
			{
				foreach (ContextMenuStrip cmnu in m_cmenus.Values)
					cmnu.Dispose();

				m_cmenus.Clear();
			}

			if (m_barLocationOrder != null)
			{
				foreach (ToolStrip bar in m_barLocationOrder.Values)
					bar.Dispose();

				m_barLocationOrder.Clear();
			}

			if (m_images != null)
			{
				foreach (Image img in m_images.Values)
					img.Dispose();

				m_images.Clear();
			}

			if (m_commands != null)
				m_commands.Clear();

			if (m_menusWithShortcuts != null)
				m_menusWithShortcuts.Clear();

			if (m_rmlocalStrings != null)
				m_rmlocalStrings.Clear();

			if (m_htItemTags != null)
				m_htItemTags.Clear();

			if (m_tspTop != null && !m_tspTop.IsDisposed)
				m_tspTop.Dispose();

			if (m_tspBottom != null && !m_tspBottom.IsDisposed)
				m_tspBottom.Dispose();

			if (m_tspLeft != null && !m_tspLeft.IsDisposed)
				m_tspLeft.Dispose();

			if (m_tspRight != null && !m_tspRight.IsDisposed)
				m_tspRight.Dispose();

			m_commands = null;
			m_rmlocalStrings = null;
			m_menusWithShortcuts = null;
			m_menuBar = null;
			m_rufMarkerItem = null;
			m_rufMarkerSeparator = null;
			m_images = null;
			m_barLocationOrder = null;
			m_bars = null;
			m_cmenus = null;
			m_toolbarListItem = null;
			m_tspTop = null;
			m_tspBottom = null;
			m_tspLeft = null;
			m_tspRight = null;
			m_htItemTags = null;
			m_parentControl = null;
		}

		#endregion
	}
}
