// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2003, SIL International. All Rights Reserved.   
// <copyright from='2003' to='2003' company='SIL International'>
//		Copyright (c) 2003, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: MenuInterface.cs
// Responsibility: David Olson
// Last reviewed: 
// 
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------
using System.Windows.Forms;
using XCore;

namespace SIL.FieldWorks.Common.UIAdapters
{
	#region ITMAdapter Interface
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface ITMAdapter
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a Toolbar/Menu adapter object.
		/// </summary>
		/// <param name="parentCtrl">The form or control owning the toolbars.</param>
		/// <param name="messageMediator">An XCore message mediator used for message routing.
		/// </param>
		/// <param name="definitions">XML strings defining all the menus/toolbars in an
		/// application.</param>
		/// ------------------------------------------------------------------------------------
		void Initialize(Control parentCtrl, Mediator messageMediator, string[] definitions);
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a Toolbar/Menu adapter object.
		/// </summary>
		/// <param name="parentCtrl">The form or control owning the toolbars.</param>
		/// <param name="messageMediator">An XCore message mediator used for message routing.
		/// </param>
		/// <param name="appsRegKeyPath">The registry path where the application's settings
		/// are stored. (e.g. "Software\SIL\FieldWorks")</param>
		/// <param name="definitions">XML strings defining all the menus/toolbars in an
		/// application.</param>
		/// ------------------------------------------------------------------------------------
		void Initialize(Control parentCtrl, Mediator messageMediator, string appsRegKeyPath, string[] definitions);

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Informs the adapter to dispose of all it's menu and toolbar items.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void Dispose();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the properties of a toolbar/menu item.
		/// </summary>
		/// <param name="name">Name of the toolbar/menu item whose properties are being
		/// requested.
		/// </param>
		/// <returns>The properties of the toolbar/menu item.</returns>
		/// ------------------------------------------------------------------------------------
		TMItemProperties GetItemProperties(string name);

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the properties of a toolbar/menu item to those stored in a TMItemProperties
		/// object.
		/// </summary>
		/// <param name="name">Name of the toolbar/menu item to modify.</param>
		/// <param name="itemProps">Properties used to modfy the toolbar/menu item.</param>
		/// ------------------------------------------------------------------------------------
		void SetItemProperties(string name, TMItemProperties itemProps);

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
		void AddMenuItem(TMItemProperties itemProps, string parentItemName, string insertBeforeItem);

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
		void AddToolBarItem(string toolBarName, string xml,	string insertBeforeItem, bool beginGroupAfter);

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
		/// <param name="imageLabel">label of image from TM def. file (if this is
		/// specified, then the image isn't necessary)</param>
		/// <param name="image">image of item (if this is specified, then the image isn't
		/// necessary)</param>
		/// ------------------------------------------------------------------------------------
		void AddCommandItem(string cmdId, string message, string text, string textAlt,
			string contextMenuText, string toolTipText, string category, string statusMsg,
			Keys shortcutKey, string imageLabel, System.Drawing.Image image);

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all the subitems of the specified menu.
		/// </summary>
		/// <param name="parentItemName">The name of the item whose subitems will be removed.
		/// </param>
		/// ------------------------------------------------------------------------------------
		void RemoveMenuSubItems(string parentItemName);

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
		void PopupMenu(string name, int x, int y);
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the context menu for a specified control.
		/// </summary>
		/// <param name="ctrl">Control which is being assigned a context menu.</param>
		/// <param name="name">The name of the context menu to assign to the control.</param>
		/// ------------------------------------------------------------------------------------
		void SetContextMenuForControl(Control ctrl, string name);

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Allows an application to force the hiding of a toolbar item's popup control.
		/// </summary>
		/// <param name="name">Name of item whose popup should be hidden.</param>
		/// ------------------------------------------------------------------------------------
		void HideBarItemsPopup(string name);

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Allows an application to hide of a toolbar.
		/// </summary>
		/// <param name="name">Name of toolbar to hide.</param>
		/// ------------------------------------------------------------------------------------
		void HideToolBar(string name);

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Allows an application to show of a toolbar.
		/// </summary>
		/// <param name="name">Name of toolbar to show.</param>
		/// ------------------------------------------------------------------------------------
		void ShowToolBar(string name);

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the properties of a toolbar.
		/// </summary>
		/// <param name="name">Name of the toolbar whose properties are being requested.
		/// </param>
		/// <returns>The properties of the toolbar.</returns>
		/// ------------------------------------------------------------------------------------
		TMBarProperties GetBarProperties(string name);

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the properties of a toolbar/menu to those stored in a TMBarProperties
		/// object.
		/// </summary>
		/// <param name="name">Name of the toolbar/menu to modify.</param>
		/// <param name="barProps">Properties used to modfy the toolbar/menu item.</param>
		/// ------------------------------------------------------------------------------------
		void SetBarProperties(string name, TMBarProperties barProps);
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the control contained within a control container toolbar item.
		/// </summary>
		/// <param name="name">Name of the control container item.</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		Control GetBarItemControl(string name);

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Causes the adapter to save toolbar settings (e.g. user placement of toolbars).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void SaveBarSettings();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Causes the adapter to show it's dialog for customizing toolbars.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void ShowCustomizeDialog();

		#region Events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Event fired when adding toolbar/menu items to a menu, menu bar or toolbar. This
		/// allows delegates of this event to initialize properties of the menu item such as
		/// its text, etc.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		event InitializeItemHandler InitializeItem;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Event fired when adding a toolbar to the toolbar container. This allows
		/// delegates of this event to initialize properties of the toolbar such as its text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		event InitializeBarHandler InitializeBar;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Event fired when adding a combobox toolbar item. This gives applications a chance
		/// to initialize a combobox item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		event InitializeComboItemHandler InitializeComboItem;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Event fired when a control container item requests the control to contain.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		event LoadControlContainerItemHandler LoadControlContainerItem;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Event fired when a user picks one of the recently used files in the recently used
		/// files list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		event RecentlyUsedItemChosenHandler RecentlyUsedItemChosen;
		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the message mediator used by the menu adapter for message dispatch.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		Mediator MessageMediator
		{
			get;
			set;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not updates to toolbar/menu items should
		/// take place during the application's idle cycles.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool AllowUpdates
		{
			get;
			set;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an array of toolbar property objects an application can use to send to a
		/// menu extender to use for display on a view menu allowing users to toggle the
		/// visibility of each toolbar.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		TMBarProperties[] BarInfoForViewMenu
		{
			get;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of files to show on the recent files list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string[] RecentFilesList
		{
			get;
			set;
		}

		#endregion
	}

	#endregion
}
