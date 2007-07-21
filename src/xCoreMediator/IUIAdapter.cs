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
// File: Choice.cs
// Authorship History: John Hatton 
// Last reviewed: 
// --------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using SIL.Utils;
//for ImageList

namespace XCore
{
	public interface IUIAdapter
	{
		Control  Init (Form window, ImageCollection smallImages, ImageCollection largeImages, Mediator mediator);
		void CreateUIForChoiceGroupCollection(ChoiceGroupCollection groupCollection);
		void CreateUIForChoiceGroup(ChoiceGroup group);
		void OnIdle();
		void FinishInit();
		void PersistLayout();
	}

	public class AdapterAssemblyFactory
	{
		public static Assembly GetAdapterAssembly(string preferredLibrary)
		{
			Assembly adaptorAssembly = null;
			// load an adapter library from the same directory as the .dll we're running
			// We strip file:/ because that's not accepted by LoadFrom()
			string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Substring(6);
			try
			{
				adaptorAssembly = Assembly.LoadFrom(Path.Combine(baseDir, preferredLibrary));		
			}
			catch (Exception)
			{
			}
			try
			{
				if (adaptorAssembly == null)
					adaptorAssembly = Assembly.LoadFrom(
						Path.Combine(baseDir, "xCoreOpenSourceAdapter.dll"));
			}
			catch (Exception)
			{
			}
			if (adaptorAssembly == null)
				throw new ApplicationException("XCore Could not find the adapter library DLL");
			return adaptorAssembly;
		}
	}

	public interface IUIMenuAdapter
	{
		bool HandleAltKey(KeyEventArgs e, bool wasDown); //for showing, for example, menus

		// This method supports various scenarios under which the context menu is expected to operate.
		// The first two parameters required, but the last two are optional.
		// The last two are special in that the implementor is expected to do preliminary work with them, before the menu opens,
		// and follow up work when the menu closes.
		void ShowContextMenu(ChoiceGroup group, Point location,
			TemporaryColleagueParameter temporaryColleagueParam,
			MessageSequencer sequencer);
	}

	/// <summary>
	/// This class is a 'Parameter Object', as described in Fowler's "Refactoring" book.
	/// It serves here to bundle the Mediator and an XCore colleague together for use
	/// by a client (in this case the implementor of IUIMenuAdapter. The motivation to
	/// introduce this came out of the need for code being done after a popup menu had closed.
	/// The post-closing activity expected here is the close event handler will remove
	/// the temporary colleague from the Mediator.
	/// The expected pre-opening activty is to add the colleague to the Mediator
	/// Both the Mediator and the colleague are required in order to meet both expectations,
	/// so an exception is thrown if either Constructor parameter is null.
	/// </summary>
	public class TemporaryColleagueParameter
	{
		private Mediator m_mediator;
		private IxCoreColleague m_temporaryColleague;
		private bool m_shouldDispose;

		/// <summary>
		/// Constructor with paramters being required.
		/// </summary>
		/// <param name="mediator">Mediator that will handle the collegue during its temporary liketime.</param>
		/// <param name="temporaryColleague"></param>
		public TemporaryColleagueParameter(Mediator mediator, IxCoreColleague temporaryColleague, bool shouldDispose)
		{
			if (mediator == null)
				throw new ArgumentNullException("'mediator' parameter cannot be null.");
			if (temporaryColleague == null)
				throw new ArgumentNullException("'temporaryColleague' parameter cannot be null.");

			m_mediator = mediator;
			m_temporaryColleague = temporaryColleague;
			m_shouldDispose = shouldDispose;
		}

		public Mediator Mediator
		{
			get { return m_mediator; }
		}

		public bool ShouldDispose
		{
			get { return m_shouldDispose; }
		}

		public IxCoreColleague TemporaryColleague
		{
			get { return m_temporaryColleague; }
		}
	}

	public interface ITestableUIAdapter
	{
		/// <summary>
		/// works for sub-groups too.
		/// </summary>
		/// <param name="groupId"></param>
		/// <returns></returns>
		int GetItemCountOfGroup (string groupId);
		void ClickItem (string groupId, string itemId);
		bool IsItemEnabled(string groupId, string itemId);
		bool HasItem(string groupId, string itemId);
		void ClickOnEverything();
	}

	//--------------------------------------------------------------------
	/// <summary>
	/// UIItemDisplayProperties contains the details of how an item should be displayed
	/// e.g its label, whether it is enabled, or whether it has a check mark.
	/// </summary>
	//--------------------------------------------------------------------
	public class UIItemDisplayProperties
	{
		#region Fields
		protected string m_text;
		protected bool m_enabled;
		protected bool m_checked;
		protected bool m_visible;
		//protected bool m_radio;
		protected string m_imageLabel;

		#endregion
		#region Properties
		public string Text  { get {return m_text;} set{m_text =value;} } // get/set text
		public bool Enabled { get {return m_enabled;} set {m_enabled = value ;} } // enable object
		public bool Visible 
		{ 
			get 
			{
				//review: I'm not sure really what to do for this...
				//somehow we want to incorporate the effect of the defaultVisible attribute
				//what I have here is not quite right... it will hide the item just because
//				//it is not currently enabled
				// See the comment on Choice.cs: describes needing set enabled and visible to 
				// the same value to keep from showing a bool choice that was deafulted to 
				// not visible.
				if (m_enabled || m_visible)
					return true;
				else
					return m_visible;
			} 
			set 
			{
				m_visible = value ;
			} 
		} 

		public bool Checked { get {return m_checked;} set{m_checked = value;} } // check it
		public string ImageLabel { get {return m_imageLabel;} }
		//public bool Radio   { get; set; } // use radio button, not checkbox
		#endregion

		public UIItemDisplayProperties(string text, bool enabled, string imageLabel,bool defaultVisible)
		{
			m_text = text;
			m_enabled = enabled;
			m_checked = false;
			m_visible = defaultVisible;
			m_imageLabel=imageLabel;
		}
	}

	//--------------------------------------------------------------------
	/// <summary>
	/// UIItemDisplayProperties contains the details of how an item should be displayed
	/// e.g its label, whether it is enabled, or whether it has a check mark.
	/// </summary>
	//--------------------------------------------------------------------
	public class UIListDisplayProperties
	{
		#region Fields
		List m_list;
		string m_propertyName;
		#endregion

		#region Properties
		public List List { get {return m_list;} }

		/// <summary>
		/// this is used for lists which are keyed to change the value of a property.
		/// having this here allows the interested colleague to actually change the property
		/// of the list at run-time, whenever it is redisplayed.
		/// </summary>
		public string PropertyName
		{
			get
			{
				return m_propertyName;
			}
			set
			{
				m_propertyName = value;
			}
		}
		#endregion

		public UIListDisplayProperties(List list)
		{
			m_list= list;
		}
	}

}