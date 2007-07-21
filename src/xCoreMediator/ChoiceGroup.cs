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
// File: ControlGroup.cs
// Authorship History: John Hatton  
// Last reviewed: 
// 
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using SIL.Utils;

namespace XCore
{
	/// <summary>
	/// Summary description for ChoiceGroupCollection.
	/// </summary>
	public abstract class ChoiceRelatedClass : ArrayList
	{
		protected IUIAdapter m_adapter;
		protected XmlNode m_configurationNode;

		public XmlNode ConfigurationNode
		{
			get
			{
				return m_configurationNode;
			}
		}
		protected Mediator m_mediator;

		protected object m_referenceWidget;

		protected bool m_defaultVisible;

		public ChoiceRelatedClass(Mediator mediator,  IUIAdapter adapter, XmlNode configurationNode) 
		{
			m_adapter = adapter;
			m_mediator = mediator;
			m_configurationNode = configurationNode;
			m_defaultVisible= XmlUtils.GetOptionalBooleanAttributeValue(m_configurationNode, "defaultVisible", true);
		}

		protected abstract void Populate();

		abstract public UIItemDisplayProperties GetDisplayProperties();

		/// <summary>
		/// currently, this is used for unit testing.it may be used for more in the future.
		/// </summary>
		virtual public string Id
		{
			get
			{
				string id = XmlUtils.GetAttributeValue(m_configurationNode, "id", "");
				if (id == "")
				{	//default to the label
					id = this.Label.Replace("_","");//remove underscores
				}
				return id;
			}
		}

		virtual public string Label
		{
			get
			{
				StringTable tbl = null;
				if (m_mediator != null && m_mediator.HasStringTable)
					tbl = m_mediator.StringTbl;
				return XmlUtils.GetLocalizedAttributeValue(tbl, m_configurationNode, "label", null);
			}
		}

		/// <summary>
		/// the icon
		/// </summary>
		public virtual string ImageName
		{
			get
			{
				return XmlUtils.GetAttributeValue(m_configurationNode, "icon", "default");
			}
		}

		/// <summary>
		/// this is used by the IUIAdaptor to store whatever it needs to to link this to a real UI choice
		/// </summary>
		public object ReferenceWidget
		{
			set
			{
				m_referenceWidget = value;
			}
			get
			{
				return m_referenceWidget;
			}
		}
	}

	//menubar, sidebar, toolbars set
	public class ChoiceGroupCollection : ChoiceRelatedClass
	{
		/// -----------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ChoiceGroupCollection"/> class.
		/// </summary>
		/// -----------------------------------------------------------------------------------
		public ChoiceGroupCollection(Mediator mediator,IUIAdapter adapter, XmlNode configurationNode) 
			: base(mediator,  adapter, configurationNode)
		{
		}
		
		public void Init()
		{
			//there is no "OnDisplay" event for the main menu bar
			UpdateUI();
		}
		protected void UpdateUI()
		{
			Populate ();
			m_adapter.CreateUIForChoiceGroupCollection(this);
		}
		protected override void Populate()
		{
			XmlNodeList groups =m_configurationNode.SelectNodes(NodeSelector);
			foreach (XmlNode node in groups)
			{
				ChoiceGroup group = new ChoiceGroup(m_mediator, m_adapter, node, null);
				this.Add(group);
			} 
		}
		protected string NodeSelector
		{
			get {return "menu | toolbar | tab";}
		}

		override public UIItemDisplayProperties GetDisplayProperties()
		{
			return null;// not using this (yet)... might need it when we want to hide, say, a whole toolbar
		}
		/*
				public bool HandleKeydown(System.Windows.Forms.KeyEventArgs e)
				{
					foreach(ChoiceRelatedClass c in this)
					{
						if(c is ChoiceGroup)
						{
							if( ((ChoiceGroup)c).HandleKeydown(e))
								return true;
						}
					}
					return false;
				}
		*/
		/// <summary>
		/// look for the group(e.g. menu, tab, toolbar) with the supplied id
		/// </summary>
		/// <param name="id"> the id attribute (or the label attribute without any underscores, 
		/// if no id is specified in the configuration)</param>
		/// <returns></returns>
		public ChoiceGroup FindById(string id)
		{
			foreach(ChoiceGroup group in this)
			{
				if (group.Id== id)
					return group;
			}
			throw new ArgumentException("There is no item with the id '"+id+"'.");
		}
	}


	//menus, sidebar bands, toolbars
	public class ChoiceGroup : ChoiceRelatedClass
	{
		protected ChoiceGroup m_parent;
		protected CommandChoice m_treeGroupCommandChoice;
		protected List<XmlNode> m_configurationNodes;
		
		/// <summary>
		/// this is the PropertyTable property which is changed when the user selects an item from the group
		/// </summary>
		protected string m_propertyName;

		/// -----------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ControlGroup"/> class.
		/// </summary>
		/// -----------------------------------------------------------------------------------
		public ChoiceGroup(Mediator mediator, IUIAdapter adapter, XmlNode configurationNode, ChoiceGroup parent)
			: base(mediator, adapter, configurationNode)
		{
			m_parent = parent;

			//allow for a command to be attached to a group (todo: should be for tree groups only) 
			//as it doesn't make sense for some menus or command bars to have an associated command.
			//for now, leave it to the schema to prevent anything but the right element from having the command attribute.
			if (XmlUtils.GetAttributeValue(m_configurationNode,"command") != null)
			{
				m_treeGroupCommandChoice = new CommandChoice(mediator, configurationNode, adapter, this);
			}

		}

		/// <summary>
		/// Group made of multiple menus
		/// </summary>
		/// <param name="mediator"></param>
		/// <param name="adapter"></param>
		/// <param name="configurationNodes"></param>
		/// <param name="parent"></param>
		public ChoiceGroup(Mediator mediator, IUIAdapter adapter, List<XmlNode> configurationNodes, ChoiceGroup parent) 
			: base(mediator, adapter, configurationNodes[0]) //hack; just give it the first one
		{
			m_parent = parent;
			m_configurationNodes = configurationNodes;
		}


		public CommandChoice CommandChoice
		{
			get
			{
				return m_treeGroupCommandChoice;
			}
		}

		/// <summary>
		/// Return whether the given choice in the ChoiceGroup has been selected.
		/// </summary>
		/// <param name="choiceValue"></param>
		/// <returns></returns>
		public bool IsValueSelected(string choiceValue)
		{
			switch (this.Behavior)
			{
			case "singlePropertyAtomicValue":
				return SinglePropertyValue == choiceValue;

			case "singlePropertySequenceValue":
				string[] rgsValues = SinglePropertyValue.Split(new char[] { ',' });
				for (int i = 0; i < rgsValues.Length; ++i)
				{
					if (rgsValues[i] == choiceValue)
						return true;
				}
				return false;

			default:
				Trace.Fail("The behavior '" + Behavior + "' is not supported or for some other reason was unexpected here(check capitalization).");
				return false;
			}
		}

		/// <summary>
		/// look for the choice(e.g. menu, tab, toolbar) with the supplied id
		/// </summary>
		/// <param name="id"> the id attribute (or the label attribute without any underscores, 
		/// if no id is specified in the configuration)</param>
		/// <returns></returns>
		public ChoiceRelatedClass FindById(string id)
		{
			//since we are lazy about Populating, we need to Populate before we can search
			this.Populate();
			foreach(ChoiceRelatedClass crc in this)
			{
				if (crc.Id== id)
					return crc;
			}
			return null;
		}

		/*		public bool HandleKeydown(System.Windows.Forms.KeyEventArgs e)
				{
					foreach(ChoiceRelatedClass c in this)
					{
						if(c is ChoiceBase)
						{
							ChoiceBase choice = (ChoiceBase)c;
							if(choice.Shortcut == e.KeyData)
							{
								choice.OnClick(this, e);
								return true;
							}
						}
						else if (c is ChoiceGroup)
						{
							if(((ChoiceGroup)c).HandleKeydown(e))
								return true;
						}
					}
					return false;
				}
		*/
		/*		public ImageList GetImageList()
				{
					// This is obviously just a temporary hack.

					ImageList icons = new ImageList();
					icons.ImageSize = new Size(16, 16);
					//TODO: Remove this hard coded path (requires learning the resource system).
					string asmPathname = Assembly.GetExecutingAssembly().CodeBase;
					string asmPath = asmPathname.Substring(0, asmPathname.LastIndexOf("/"));
					string bitmapPath = System.IO.Path.Combine(asmPath, @"..\..\..\Src\XCore\xCoreTests\listitems.bmp");
					Bitmap b = (Bitmap)System.Drawing.Bitmap.FromFile(bitmapPath); 
					b.MakeTransparent();
					icons.Images.Add(b);
					return icons;
				}
		*/


		//for menus, this is wired to be "pop up" event
		public void OnDisplay(object sender, EventArgs args)
		{
			UpdateUI();
		}
		protected void UpdateUI()
		{
			Populate ();
			m_adapter.CreateUIForChoiceGroup(this);
		}

		/// <summary>
		/// called by the ui adaptor to get updated display parameters for a widget showing this group
		/// </summary>
		/// <returns></returns>
		override public UIItemDisplayProperties GetDisplayProperties()
		{
			//review: the enabled parameter is set to the same value as the defaultVisible
			// value so that only defaultVisible items are visible by default.  Previously
			// enabled items would be 'visible' and enabled was true by default.
			UIItemDisplayProperties display =new UIItemDisplayProperties(this.Label,
				this.m_defaultVisible, ImageName, this.m_defaultVisible);
			if (this.PropertyName != null && this.PropertyName != string.Empty)
				m_mediator.SendMessage("Display"+this.PropertyName, null, ref display);
			else
				m_mediator.SendMessage("Display"+this.Id, null, ref display);

			return display;
		}

		public string ListId
		{
			get
			{
				return XmlUtils.GetAttributeValue(m_configurationNode, "list");
			}
		}
		protected override void Populate()
		{
			Clear();
			if (IsAListGroup)
			{
				PopulateFromList();
			}
			else if (m_configurationNodes != null)
			{
				foreach (XmlNode n in m_configurationNodes)
				{
					Populate(n);
				}
			}
			else
			{
				Populate(m_configurationNode);
			}
		}

		/// <summary>
		/// force the group to populate, even if it wants to be lazy!
		/// </summary>
		/// <remarks> this is a hack because the interface says that Populate() is protected</remarks>
		public void PopulateNow()
		{
			Populate();
		}

		protected void PopulateFromList ()
		{
			/// Just before this group is displayed, allow the group's contents to be modified by colleagues
			//if this is a list-populated group.

			//first, we get the list as it is in the XML configuration file
			XmlNode listNode = m_configurationNode.OwnerDocument.SelectSingleNode("//list[@id='"+this.ListId+"']");

			StringTable stringTbl = null;
			if (m_mediator != null && m_mediator.HasStringTable)
				stringTbl = m_mediator.StringTbl;
			List list = new List(listNode, stringTbl);
			
			UIListDisplayProperties display =new UIListDisplayProperties(list);
			display.PropertyName= this.PropertyName;
			string wsSet = XmlUtils.GetOptionalAttributeValue(m_configurationNode, "wsSet");
			m_mediator.SendMessage("Display"+this.ListId, wsSet, ref display);
						
			PropertyName = display.PropertyName;

			foreach (ListItem item in list)
			{
				Add(new ListPropertyChoice(m_mediator, item,m_adapter , this));
			} 

			// select the first one if none is selected
			if(
				(m_mediator.PropertyTable.GetValue(this.PropertyName) == null)	// there isn't a value alread (from depersisting)
				&& (this.Count>0))
			{
				ListPropertyChoice first = (ListPropertyChoice)this[0];
				//				first.OnClick(this, null);
			}
		}


		/// <summary>
		/// this is used by the sidebar adapter to determine if we need a tree to represent this group.
		/// </summary>
		/// <param name="group"></param>
		/// <returns></returns>
		public bool HasSubGroups()
		{
			this.Clear();
			if (IsAListGroup)
				return false;//hierarchical lists are not currently supported
			else //enhance: all we really need here is in XPATH here to look for sub-group nodes.
			{
				Populate(m_configurationNode);
			
				foreach(ChoiceRelatedClass item in this )
				{
					if (item is ChoiceGroup)
						return true;
				}
				return false;
			}
		}

		protected  void Populate(XmlNode node)
		{
			Debug.Assert( node != null);			
			XmlNodeList items =node.SelectNodes("item | menu | group");
			foreach (XmlNode childNode in items)
			{
				switch (childNode.Name)
				{
					case "item":
						ChoiceBase choice = ChoiceBase.Make(m_mediator, childNode,m_adapter , this);
						this.Add(choice);
						break;
					case "menu":
						ChoiceGroup group = new ChoiceGroup(m_mediator, m_adapter, childNode, this);
						this.Add(group);
						break;
					case "group":	//for tree views in the sidebar
						group = new ChoiceGroup(m_mediator, m_adapter, childNode, this);
						this.Add(group);
						break;
					default:
						Debug.Fail("Didn't understand node type '"+childNode.Name+"' in this context."+node.OuterXml);
						break;
				}

			} 
		}
		
		public string Behavior
		{
			get
			{
				return XmlUtils.GetAttributeValue(m_configurationNode, "behavior", "pushOnly");
			}
		}

		//		public string Property
		//		{
		//			get
		//			{
		//				return XmlUtils.GetAttributeValue(m_configurationNode, "property", "");
		//			}
		//		}



		public string PropertyName
		{
			//			get
			//			{
			//				return XmlUtils.GetAttributeValue(m_configurationNode, "property", "");
			//			}
			get
			{
				if(m_propertyName == null)
					m_propertyName =  XmlUtils.GetAttributeValue(m_configurationNode, "property", "");
				
				return m_propertyName;
			}
			set
			{
				m_propertyName = value;
			}
		}
		
		public string DefaultSinglePropertyValue
		{
			get
			{	//I don't know what to do about the default here
				return XmlUtils.GetAttributeValue(m_configurationNode, "defaultPropertyValue", "????");
			}
		}

		public string SinglePropertyValue
		{
			get
			{
				return 	m_mediator.PropertyTable.GetStringProperty(PropertyName, DefaultSinglePropertyValue);
			}
		}

		public void HandleItemClick(ListPropertyChoice choice)
		{
			m_mediator.SendMessage("ProgressReset", this);
			switch (this.Behavior)
			{
			case "singlePropertyAtomicValue":
				HandleClickedWhenSinglePropertyAtomicValue(choice);
				break;
			case "singlePropertySequenceValue":
				HandleClickedWhenSinglePropertySequenceValue(choice);
				break;
			default:
				Trace.Fail("The behavior '" + Behavior + "' is not supported or for some other reason was unexpected here(check capitalization).");
				break;
			}
			m_mediator.SendMessage("ProgressReset", this);

		}

		protected bool IsAListGroup
		{
			get
			{
				return this.Behavior == "singlePropertyAtomicValue" || 
					this.Behavior == "singlePropertySequenceValue";
			}
		}
	
		protected void HandleClickedWhenSinglePropertyAtomicValue (ListPropertyChoice choice)
		{
			ChooseSinglePropertyAtomicValue(m_mediator,choice.Value, choice.ParameterNode,this.PropertyName);
		}
			
		/// <summary>
		/// set the value of a property to a node of a list. Also sets the corresponding parameters property.
		/// </summary>
		/// <remarks> this is static so that it can be called by the XCore initializationcode and set from the contents of the XML configuration file</remarks>
		/// <param name="mediator"></param>
		/// <param name="choice"></param>
		/// <param name="propertyName"></param>
		static public void ChooseSinglePropertyAtomicValue (Mediator mediator, string choiceValue,
			XmlNode choiceParameters,string propertyName)
		{
			//a hack (that may be we could live with)
			//	if(choiceParameters !=null)
			//	{
			mediator.PropertyTable.SetProperty(propertyName+"Parameters", choiceParameters);
			//it is possible that we would like to persist these parameters 
			//however, as a practical matter, you cannot have XmlNodes witch do not belong to a document.
			//therefore, they could not be deserialize if we did save them.
			//unless, of course, we convert them to a string before serializing.
			//However, when de-serializing, what document would we attach the new xmlnode to?
			mediator.PropertyTable.SetPropertyPersistence(propertyName+"Parameters", false);
			//}

			
			//remember, each of these calls to SetProperty() generate a broadcast, so the order of these two calls
			//is relevant.
			mediator.PropertyTable.SetProperty(propertyName, choiceValue);

			if (choiceParameters != null)
			{
				//since we cannot persist the parameters, it's safer to not persist the choice either.
				mediator.PropertyTable.SetPropertyPersistence(propertyName, false);
			}

		}

		protected void HandleClickedWhenSinglePropertySequenceValue(ListPropertyChoice choice)
		{
			bool fEmptyAllowed = XmlUtils.GetOptionalBooleanAttributeValue(m_configurationNode,
				"emptyAllowed", false);
			ChooseSinglePropertySequenceValue(m_mediator, choice.Value, choice.ParameterNode,
				this.PropertyName, fEmptyAllowed);
		}

		static int IndexOf(string[] rgs, string s)
		{
			for (int i = 0; i < rgs.Length; ++i)
			{
				if (rgs[i] == s)
					return i;
			}
			return -1;
		}

		static public void ChooseSinglePropertySequenceValue(Mediator mediator, string choiceValue, 
			XmlNode choiceParameterNode, string propertyName, bool fEmptyAllowed)
		{
			mediator.PropertyTable.SetProperty(propertyName+"Parameters", choiceParameterNode);
			mediator.PropertyTable.SetPropertyPersistence(propertyName+"Parameters", false);
			string sValue = mediator.PropertyTable.GetStringProperty(propertyName, "");
			string[] rgsValues = sValue.Split(',');
			int idx = -1;
			if (sValue == choiceValue)
			{
				if (fEmptyAllowed)
					sValue = "";
			}
			else if ((idx = IndexOf(rgsValues, choiceValue)) != -1)
			{
				// remove the choiceValue from the string.
				Debug.Assert(rgsValues.Length > 1);
				StringBuilder sbValues = new StringBuilder(sValue.Length);
				for (int i = 0; i < rgsValues.Length; ++i)
				{
					if (idx != i)
					{
						if (sbValues.Length > 0)
							sbValues.Append(",");
						sbValues.Append(rgsValues[i]);
					}
				}
				sValue = sbValues.ToString();
			}
			else
			{
				if (sValue.Length == 0)
					sValue = choiceValue;
				else
					sValue = sValue + "," + choiceValue;
			}
			mediator.PropertyTable.SetProperty(propertyName, sValue);
			mediator.PropertyTable.SetPropertyPersistence(propertyName, false);
		}

		public bool IsTopLevelMenu
		{
			get
			{
				return m_parent == null;
			}
		}

		public bool IsSubmenu
		{
			get
			{
				return !IsTopLevelMenu && !IsInlineChoiceList;
			}
		}

		public bool IsInlineChoiceList
		{
			get
			{
				return  !IsTopLevelMenu && XmlUtils.GetBooleanAttributeValue(m_configurationNode, "inline") ;
			}
		}
	}

}
