// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005, SIL International. All Rights Reserved.   
// <copyright from='2005' to='2005' company='SIL International'>
//		Copyright (c) 2005, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: ArticulatoryFeaturesDlg.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Diagnostics;
using SIL.Pa.Data;
using SIL.Pa.Resources;
using SIL.SpeechTools.Utils;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Controls;
using XCore;

namespace SIL.Pa.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Dialog class to allowing modifying articulatory feature names and specify
	/// IPA character features.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class DefineFeaturesDlgBase : OKCancelDlgBase, IxCoreColleague
	{
		#region Class Variables
		protected IPACharCache.SortType m_currCharSortType = IPACharCache.SortType.Unicode;
		protected ITMAdapter m_tmAdapter;
		protected bool m_ignoreCheckChanges = true;
		protected Font m_checkedItemFont;
		protected FeatureListView m_lvFeatures;
		#endregion

		#region Construction/Setup
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DefineFeaturesDlgBase()
		{
			InitializeComponent();

			// I turned localization on for the derived classes, but for some
			// reason, the text never gets set via the expected means.
			ComponentResourceManager resources = new ComponentResourceManager(this.GetType());
			Text = resources.GetString("$this.Text");
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DefineFeaturesDlgBase(string name) : this()
		{
			Name = name;

			if (!PaApp.DesignMode)
			{
				LoadToolbarAndMenus();
				PaApp.AddMediatorColleague(this);
			}

			lstIPAChar.Font = new Font(FontHelper.PhoneticFont.FontFamily, 20,
				(FontHelper.PhoneticFont.Bold ? FontStyle.Bold : 0) |
				(FontHelper.PhoneticFont.Italic ? FontStyle.Italic : 0));

			sslCharDescription.Font = FontHelper.UIFont;
			lblIPA.Font = FontHelper.UIFont;
			lblFeatures.Font = FontHelper.UIFont;

			// There's something I'm not understanding going on in the designer that always
			// moves these labels down. Even after I move them up. Therefore, I'll let
			// designer do what it wills and just set the location of these in code.
			lblIPA.Location = new Point(13, 11);
			lblFeatures.Location = new Point(3, 11);
			lblIPA.SendToBack();
			lblFeatures.SendToBack();

			if (!PaApp.DesignMode)
				BuildFeatureList();

			// Do this so the toolbar doesn't cover the top of the panel.
			splitContainer1.BringToFront();
			PaApp.SettingsHandler.LoadFormProperties(this);

			try
			{
				splitContainer1.SplitterDistance =
					PaApp.SettingsHandler.GetIntSettingsValue(Name, "splitter",
					splitContainer1.SplitterDistance);
			}
			catch { }

			string sortType = PaApp.SettingsHandler.GetStringSettingsValue(Name, "ipacharsortorder", null);

			if (sortType != null)
			{
				try
				{
					m_currCharSortType =
						(IPACharCache.SortType)Enum.Parse(typeof(IPACharCache.SortType), sortType);
				}
				catch { }
			}

			if (!PaApp.DesignMode)
				LoadIPAList();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadToolbarAndMenus()
		{
			m_tmAdapter = AdapterHelper.CreateTMAdapter();

			if (m_tmAdapter != null)
			{
				string[] defs = new string[1];
				defs[0] = Path.Combine(Application.StartupPath, "DefineFeaturesTMDefinition.xml");
				m_tmAdapter.Initialize(this, PaApp.MsgMediator, PaApp.ApplicationRegKeyPath, defs);
				m_tmAdapter.AllowUpdates = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildFeatureList()
		{
			m_lvFeatures = new FeatureListView(FeatureType);
			m_lvFeatures.Load();
			m_lvFeatures.Dock = DockStyle.Fill;
			m_lvFeatures.FeatureChanged +=
				new FeatureListView.FeatureChangedHandler(HandleFeatureChanged);

			splitContainer1.Panel2.Controls.Add(m_lvFeatures);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the features list should display
		/// articulatory features or binary features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual PaApp.FeatureType FeatureType
		{
			get {return PaApp.FeatureType.Articulatory;}
		}
		
		#endregion

		#region Overridden Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Be nice and clean up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			PaApp.RemoveMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			base.SaveSettings();
			PaApp.SettingsHandler.SaveFormProperties(this);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "ipacharsortorder", m_currCharSortType);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "splitter", splitContainer1.SplitterDistance);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			if (m_dirty)
				PaApp.Project.SaveIPACharCache();
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void ThrowAwayChanges()
		{
			PaApp.Project.LoadIPACharCache();
		}

		#endregion

		#region Message and Update Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSortByCodepointOrder(object args)
		{
			m_currCharSortType = IPACharCache.SortType.Unicode;
			LoadIPAList();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateSortByCodepointOrder(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Checked = (m_currCharSortType == IPACharCache.SortType.Unicode);
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSortByMOAOrder(object args)
		{
			m_currCharSortType = IPACharCache.SortType.MOArticulation;
			LoadIPAList();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateSortByMOAOrder(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Checked = (m_currCharSortType == IPACharCache.SortType.MOArticulation);
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSortByPOAOrder(object args)
		{
			m_currCharSortType = IPACharCache.SortType.POArticulation;
			LoadIPAList();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateSortByPOAOrder(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Checked = (m_currCharSortType == IPACharCache.SortType.POArticulation);
				itemProps.Update = true;
			}

			return true;
		}

		#endregion

		#region Loading/Saving
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fills the IPA Listbox. Each list resultView item's tag holds the codepoint of the IPA
		/// character it displays. That is used to reference the appropriate IPA cache item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadIPAList()
		{
			lstIPAChar.BeginUpdate();
			lstIPAChar.Items.Clear();

			// Get a sorted list of references to IPA cache entries.
			SortedList<int, IPACharInfo> cacheRefs =
				DataUtils.IPACharCache.GetSortedReferenceList(m_currCharSortType);

			foreach (IPACharInfo charInfo in cacheRefs.Values)
			{
				if (charInfo.Codepoint > 32)
				{

					string ipaChar = (charInfo.DisplayWDottedCircle ?
						DataUtils.kDottedCircle : string.Empty) + charInfo.IPAChar;

					lstIPAChar.Items.Add(new CharListItem(ipaChar, charInfo));
				}
			}

			if (lstIPAChar.Items.Count >= 0)
				lstIPAChar.SelectedIndex = 0;

			lstIPAChar.EndUpdate();
		}

		#endregion

		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles a feature's state changing (i.e. from checked to unchecked or plus to
		/// minus to neither.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFeatureChanged(object sender, ulong[] newMasks)
		{
			if (lstIPAChar.SelectedItem is CharListItem)
			{
				IPACharInfo charInfo = ((CharListItem)lstIPAChar.SelectedItem).CharInfo;
				
				if (FeatureType == PaApp.FeatureType.Binary)
					charInfo.BinaryMask = newMasks[0];
				else
				{
					charInfo.Mask0 = newMasks[0];
					charInfo.Mask1 = newMasks[1];
				}

				m_dirty = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Override drawing so we can draw the characters centered within their cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lstIPAChar_DrawItem(object sender, DrawItemEventArgs e)
		{
			// This check is for design mode when opening derived classes in the designer.
			if (e.Index < 0 || lstIPAChar.Items.Count == 0)
				return;

			Color clrText = lstIPAChar.ForeColor;

			// Check if the item is selected.
			if ((e.State & DrawItemState.Selected) == 0)
				e.DrawBackground();
			else
			{
				// Draw the item with a selected appearance.
				e.Graphics.FillRectangle(SystemBrushes.MenuHighlight, e.Bounds);
				clrText = SystemColors.HighlightText;
				if (lstIPAChar.Focused)
					e.DrawFocusRectangle();
			}

			TextRenderer.DrawText(e.Graphics,
				lstIPAChar.Items[e.Index].ToString(), lstIPAChar.Font, e.Bounds, clrText,
				TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lstIPAChar_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstIPAChar.SelectedItem is CharListItem)
			{
				IPACharInfo charInfo = ((CharListItem)lstIPAChar.SelectedItem).CharInfo;
				sslCharDescription.Text = charInfo.Description;
				m_lvFeatures.CurrentMasks = (FeatureType == PaApp.FeatureType.Articulatory ?
					new ulong[] { charInfo.Mask0, charInfo.Mask1 } :
					new ulong[] { charInfo.BinaryMask, 0 });
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void btnRestoreDefaults_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(this, Properties.Resources.kstidRestoreDefaultFeatureMsg,
				Application.ProductName, MessageBoxButtons.YesNo,
				MessageBoxIcon.Question) == DialogResult.Yes)
			{
				//HandleRestoringDefaults();
				//m_lvFeatures.Reset();
				//LoadIPAList();
				//m_isDirty = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Let derived classes restore what's relevant to them.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleRestoringDefaults()
		{
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used in PA.
		/// </summary>
		/// <param name="mediator"></param>
		/// <param name="configurationParameters"></param>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public virtual IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] { this };
		}

		#endregion

		#region Classes to encapsulate feature list resultView items and ipa char list box items
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected struct CharListItem
		{
			internal string IPAChar;
			internal IPACharInfo CharInfo;

			internal CharListItem(string ipachar, IPACharInfo charinfo)
			{
				IPAChar = ipachar;
				CharInfo = charinfo;
			}

			public override string ToString()
			{
				return IPAChar;
			}
		}

		#endregion
	}
}
