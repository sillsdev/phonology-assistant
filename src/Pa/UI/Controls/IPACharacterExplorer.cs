using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using SIL.Pa.Model;
using SilUtils;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class IPACharacterExplorer : SimpleExplorerBar
	{
		private const int kBigFontSize = 19;

		public event CharPicker.CharPickedHandler CharPicked;
		public event CharPicker.ShouldLoadCharHandler ShouldLoadChar;
		public event ItemDragEventHandler ItemDrag;

		private CharPicker m_pickerConsonant;
		private CharPicker m_pickerNonPulmonics;
		private CharPicker m_pickerOther;
		private CharPicker m_pickerVowel;
		private CharPicker m_pickerSSeg;
		private CharPicker m_pickerDiacritics;
		private CharPicker m_pickerTone;
		private List<IPASymbolTypeInfo> m_typesToShow;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPACharacterExplorer()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructor allowing specific character types to be displayed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPACharacterExplorer(List<IPASymbolTypeInfo> typesToShow)
		{
			m_typesToShow = typesToShow;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the fonts for each picker.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshFont()
		{
			foreach (ExplorerBarItem item in Items)
			{
				CharPicker picker = item.Control as CharPicker;
				if (picker != null)
				{
					picker.RefreshFont();
					item.SetHostedControlHeight(picker.PreferredHeight + 10);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (m_pickerConsonant != null && !m_pickerConsonant.IsDisposed)
				{
					m_pickerConsonant.CharPicked -= HandleCharPicked;
					m_pickerConsonant.ShouldLoadChar -= HandleShouldLoadCharacter;
					m_pickerConsonant.ItemDrag -= HandleCharacterItemDrag;
					m_pickerConsonant.Dispose();
				}

				if (m_pickerNonPulmonics != null && !m_pickerNonPulmonics.IsDisposed)
				{
					m_pickerNonPulmonics.CharPicked -= HandleCharPicked;
					m_pickerNonPulmonics.ShouldLoadChar -= HandleShouldLoadCharacter;
					m_pickerNonPulmonics.ItemDrag -= HandleCharacterItemDrag;
					m_pickerNonPulmonics.Dispose();
				}

				if (m_pickerOther != null && !m_pickerOther.IsDisposed)
				{
					m_pickerOther.CharPicked -= HandleCharPicked;
					m_pickerOther.ShouldLoadChar -= HandleShouldLoadCharacter;
					m_pickerOther.ItemDrag -= HandleCharacterItemDrag;
					m_pickerOther.Dispose();
				}

				if (m_pickerVowel != null && !m_pickerVowel.IsDisposed)
				{
					m_pickerVowel.CharPicked -= HandleCharPicked;
					m_pickerVowel.ShouldLoadChar -= HandleShouldLoadCharacter;
					m_pickerVowel.ItemDrag -= HandleCharacterItemDrag;
					m_pickerVowel.Dispose();
				}

				if (m_pickerDiacritics != null && !m_pickerDiacritics.IsDisposed)
				{
					m_pickerDiacritics.CharPicked -= HandleCharPicked;
					m_pickerDiacritics.ShouldLoadChar -= HandleShouldLoadCharacter;
					m_pickerDiacritics.ItemDrag -= HandleCharacterItemDrag;
					m_pickerDiacritics.Dispose();
				}

				if (m_pickerSSeg != null && !m_pickerSSeg.IsDisposed)
				{
					m_pickerSSeg.CharPicked -= HandleCharPicked;
					m_pickerSSeg.ShouldLoadChar -= HandleShouldLoadCharacter;
					m_pickerSSeg.ItemDrag -= HandleCharacterItemDrag;
					m_pickerSSeg.Dispose();
				}

				if (m_pickerTone != null && !m_pickerTone.IsDisposed)
				{
					m_pickerTone.CharPicked -= HandleCharPicked;
					m_pickerTone.ShouldLoadChar -= HandleShouldLoadCharacter;
					m_pickerTone.ItemDrag -= HandleCharacterItemDrag;
					m_pickerTone.Dispose();
				}
			}
			
			base.Dispose(disposing);
		}

		#region Methods for loading IPA character choosers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the explorer bar with all the necessary IPA character choosers for
		/// classes based on IPA characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			if (App.DesignMode)
				return;

			Utils.SetWindowRedraw(this, false, false);

			// Loop through the list of character types for which to build a chooser.
			foreach (IPASymbolTypeInfo typeInfo in m_typesToShow)
			{
				switch (typeInfo.Type)
				{
					case IPASymbolType.Vowel:
						LoadVowels(typeInfo);
						break;

					case IPASymbolType.Diacritics:
						LoadDiacritics(typeInfo);
						break;

					case IPASymbolType.Consonant:
						if (typeInfo.SubType == IPASymbolSubType.NonPulmonic)
							LoadNonPulmonics(typeInfo);
						else if (typeInfo.SubType == IPASymbolSubType.OtherSymbols)
							LoadOthers(typeInfo);
						else
							LoadConsonants(typeInfo);

						break;

					case IPASymbolType.Suprasegmentals:
						if (typeInfo.SubType == IPASymbolSubType.ToneAndAccents)
							LoadTone(typeInfo);
						else
							LoadSSegs(typeInfo);

						break;
				}
			}

			Dock = DockStyle.Fill;
			LayoutPickers(false);
			Utils.SetWindowRedraw(this, true, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LayoutPickers(bool suspendDraw)
		{
			if (suspendDraw)
				Utils.SetWindowRedraw(this, false, false);

			foreach (ExplorerBarItem item in Items)
			{
				CharPicker picker = item.Control as CharPicker;
				if (picker != null && picker == item.Control)
					item.SetHostedControlHeight(picker.PreferredHeight + 10);
			}

			if (suspendDraw)
				Utils.SetWindowRedraw(this, true, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the consonants character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadConsonants(IPASymbolTypeInfo typeInfo)
		{
			m_pickerConsonant = new CharPicker();
			m_pickerConsonant.Name = "chrPickerConsonants";
			m_pickerConsonant.CharPicked += HandleCharPicked;
			m_pickerConsonant.ShouldLoadChar += HandleShouldLoadCharacter;
			m_pickerConsonant.ItemDrag += HandleCharacterItemDrag;
			m_pickerConsonant.LoadCharacterType(typeInfo);
			m_pickerConsonant.CheckItemsOnClick = false;
			m_pickerConsonant.AutoSizeItems = true;

			ExplorerBarItem item = Add(m_pickerConsonant);

			App.L10NMngr.LocalizeObject(item.Button, 
				"IPACharacterChooser.ConsonantsCharChooserHeading", "Consonants", null, null,
				"Text on heading above list of consonants from which to choose " +
				"in side bar of search and XY chart views.", App.kLocalizationGroupUICtrls,
				LocalizationPriority.High);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the non pulmonic consonants character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadNonPulmonics(IPASymbolTypeInfo typeInfo)
		{
			m_pickerNonPulmonics = new CharPicker();
			m_pickerNonPulmonics.Name = "chrPickerNonPulmonics";
			m_pickerNonPulmonics.CharPicked += HandleCharPicked;
			m_pickerNonPulmonics.ShouldLoadChar += HandleShouldLoadCharacter;
			m_pickerNonPulmonics.ItemDrag += HandleCharacterItemDrag;
			m_pickerNonPulmonics.LoadCharacterType(typeInfo);
			m_pickerNonPulmonics.CheckItemsOnClick = false;
			m_pickerNonPulmonics.AutoSizeItems = true;
			
			ExplorerBarItem item = Add(m_pickerNonPulmonics);

			App.L10NMngr.LocalizeObject(item.Button,
				"IPACharacterChooser.NonPulmonicsCharChooserHeading", "Non Pulmonics", null, null,
				"Text on heading above list of non pulmonic consonants from which to " +
				"choose in side bar of search and XY chart views.", App.kLocalizationGroupUICtrls, 
				LocalizationPriority.High);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the other consonant character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadOthers(IPASymbolTypeInfo typeInfo)
		{
			m_pickerOther = new CharPicker();
			m_pickerOther.Name = "chrPickerOthers";
			m_pickerOther.CharPicked += HandleCharPicked;
			m_pickerOther.ShouldLoadChar += HandleShouldLoadCharacter;
			m_pickerOther.ItemDrag += HandleCharacterItemDrag;
			m_pickerOther.LoadCharacterType(typeInfo);
			m_pickerOther.CheckItemsOnClick = false;
			m_pickerOther.AutoSizeItems = true;
			
			ExplorerBarItem item = Add(m_pickerOther);

			App.L10NMngr.LocalizeObject(item.Button,
				"IPACharacterChooser.OtherSymbolsCharChooserHeading", "Other Symbols", null, null,
				"Text on heading above list of other symbols from which to choose " +
				"in side bar of search and XY chart views.", App.kLocalizationGroupUICtrls,
				LocalizationPriority.High);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the vowel character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadVowels(IPASymbolTypeInfo typeInfo)
		{
			m_pickerVowel = new CharPicker();
			m_pickerVowel.Name = "chrPickerVowels";
			m_pickerVowel.CharPicked += HandleCharPicked;
			m_pickerVowel.ShouldLoadChar += HandleShouldLoadCharacter;
			m_pickerVowel.ItemDrag += HandleCharacterItemDrag;
			m_pickerVowel.LoadCharacterType(typeInfo);
			m_pickerVowel.CheckItemsOnClick = false;
			m_pickerVowel.AutoSizeItems = true;

			ExplorerBarItem item = Add(m_pickerVowel);

			App.L10NMngr.LocalizeObject(item.Button,
				"IPACharacterChooser.VowelsCharChooserHeading", "Vowels", null, null,
				"Text on heading above list of vowels from which to choose in side bar " +
				"of search and XY chart views.", App.kLocalizationGroupUICtrls,
				LocalizationPriority.High);
		}
        
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the diacritics character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadDiacritics(IPASymbolTypeInfo typeInfo)
		{
			m_pickerDiacritics = new CharPicker();
			m_pickerDiacritics.Name = "chrPickerDiacritics";
			m_pickerDiacritics.CharPicked += HandleCharPicked;
			m_pickerDiacritics.ShouldLoadChar += HandleShouldLoadCharacter;
			m_pickerDiacritics.ItemDrag += HandleCharacterItemDrag;
			m_pickerDiacritics.LoadCharacterType(typeInfo);
			
			m_pickerDiacritics.CheckItemsOnClick = false;
			m_pickerDiacritics.AutoSizeItems = true;

			ExplorerBarItem item = Add(m_pickerDiacritics);

			App.L10NMngr.LocalizeObject(item.Button,
				"IPACharacterChooser.DiacriticsCharChooserHeading", "Diacritics", null, null,
				"Text on heading above list of diacritics from which to choose in side bar " +
				"of search and XY chart views.", App.kLocalizationGroupUICtrls,
				LocalizationPriority.High);

			// Enlarge the font and cell size
			m_pickerDiacritics.Font = FontHelper.MakeFont(m_pickerDiacritics.Font, kBigFontSize);
			m_pickerDiacritics.ItemSize = new Size(40, 46);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the suprasegmental character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadSSegs(IPASymbolTypeInfo typeInfo)
		{
			m_pickerSSeg = new CharPicker();
			m_pickerSSeg.Name = "chrPickerSSegs";
			m_pickerSSeg.CharPicked += HandleCharPicked;
			m_pickerSSeg.ShouldLoadChar += HandleShouldLoadCharacter;
			m_pickerSSeg.ItemDrag += HandleCharacterItemDrag;
			m_pickerSSeg.LoadCharacterType(typeInfo);
			
			m_pickerSSeg.CheckItemsOnClick = false;
			m_pickerSSeg.AutoSizeItems = true;
	
			ExplorerBarItem item = Add(m_pickerSSeg);

			App.L10NMngr.LocalizeObject(item.Button,
				"IPACharacterChooser.SSegsCharChooserHeading", "Stress and Length\\n(Suprasegmentals)",
				null, null, "Text on heading above list of suprasegmentals from which to choose " +
				"in side bar of search and XY chart views.", App.kLocalizationGroupUICtrls,
				LocalizationPriority.High);

			// Enlarge the font and cell size
			m_pickerSSeg.Font = FontHelper.MakeFont(m_pickerSSeg.Font, kBigFontSize);
			m_pickerSSeg.ItemSize = new Size(40, 46);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the tone character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadTone(IPASymbolTypeInfo typeInfo)
		{
			m_pickerTone = new CharPicker();
			m_pickerTone.Name = "chrPickerTone";
			m_pickerTone.CharPicked += HandleCharPicked;
			m_pickerTone.ShouldLoadChar += HandleShouldLoadCharacter;
			m_pickerTone.ItemDrag += HandleCharacterItemDrag;
			m_pickerTone.LoadCharacterType(typeInfo);

			m_pickerTone.CheckItemsOnClick = false;
			m_pickerTone.AutoSizeItems = true;
			
			ExplorerBarItem item = Add(m_pickerTone);

			App.L10NMngr.LocalizeObject(item.Button,
				"IPACharacterChooser.ToneCharChooserHeading", "Tone and Accents", null, null,
				"Text on heading above list of tones and accents from which to choose " +
				"in side bar of search and XY chart views.", App.kLocalizationGroupUICtrls,
				LocalizationPriority.High);

			// Enlarge the font and cell size
			m_pickerTone.Font = FontHelper.MakeFont(m_pickerTone.Font, kBigFontSize);
			m_pickerTone.ItemSize = new Size(40, 46);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calls the delegate assigned to determining whether or not the specified IPA
		/// character should be loaded in the specified chooser.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool HandleShouldLoadCharacter(CharPicker picker, IPASymbol charInfo)
		{
			return (ShouldLoadChar == null ? true : ShouldLoadChar(picker, charInfo));
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Pass on item dragging events.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCharacterItemDrag(object sender, ItemDragEventArgs e)
		{
			if (ItemDrag != null)
				ItemDrag(sender, e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCharPicked(CharPicker picker, ToolStripButton item)
		{
			if (CharPicked != null)
				CharPicked(picker, item);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the IPACharChooser controls have their height and with adjusted as
		/// the explorer bar changes sizes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs eventargs)
		{
			base.OnResize(eventargs);
			LayoutPickers(true);
		}

		#region Loading/Restoring Settings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Restores the expanded states from the query file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadSettings(string parentFormName)
		{
			for (int i = 0; i < Items.Length; i++)
			{
				Items[i].IsExpanded =
					App.SettingsHandler.GetBoolSettingsValue(parentFormName, "chooser" + i, true);
			}

			AutoScrollPosition = new Point(0, 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the expanded states from the query file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveSettings(string parentFormName)
		{
			for (int i = 0; i < Items.Length; i++)
				App.SettingsHandler.SaveSettingsValue(parentFormName, "chooser" + i, Items[i].IsExpanded);
		}

		#endregion

		#region Properties
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of character types/sub-types to display in the explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<IPASymbolTypeInfo> TypesToShow
		{
			get { return m_typesToShow; }
			set { m_typesToShow = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal consonant chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public CharPicker ConsonantChooser
		{
			get { return m_pickerConsonant; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal non pulmonic consonants chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public CharPicker NonPulmonicsConsonantsChooser
		{
			get { return m_pickerNonPulmonics; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal other consonants chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public CharPicker OtherConsonantsChooser
		{
			get { return m_pickerOther; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal vowel chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public CharPicker VowelChooser
		{
			get { return m_pickerVowel; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal suprasegmental chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public CharPicker SuprasegmentalChooser
		{
			get { return m_pickerSSeg; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal diacritic chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public CharPicker DiacriticChooser
		{
			get { return m_pickerDiacritics; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal tone chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public CharPicker ToneChooser
		{
			get { return m_pickerTone; }
		}

		#endregion
	}
}
