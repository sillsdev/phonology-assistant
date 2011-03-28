using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public class IPACharacterExplorer : SimpleExplorerBar
	{
		private const int kBigFontSize = 19;

		public event CharPicker.CharPickedHandler CharPicked;
		public event ItemDragEventHandler ItemDrag;

		private Func<IPASymbol, bool> ShouldLoadCharacterDelegate { get; set; }

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
			TypesToShow = typesToShow;
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
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (ConsonantChooser != null && !ConsonantChooser.IsDisposed)
				{
					ConsonantChooser.CharPicked -= HandleCharPicked;
					ConsonantChooser.ItemDrag -= HandleCharacterItemDrag;
					ConsonantChooser.Dispose();
				}

				if (NonPulmonicsConsonantsChooser != null && !NonPulmonicsConsonantsChooser.IsDisposed)
				{
					NonPulmonicsConsonantsChooser.CharPicked -= HandleCharPicked;
					NonPulmonicsConsonantsChooser.ItemDrag -= HandleCharacterItemDrag;
					NonPulmonicsConsonantsChooser.Dispose();
				}

				if (OtherConsonantsChooser != null && !OtherConsonantsChooser.IsDisposed)
				{
					OtherConsonantsChooser.CharPicked -= HandleCharPicked;
					OtherConsonantsChooser.ItemDrag -= HandleCharacterItemDrag;
					OtherConsonantsChooser.Dispose();
				}

				if (VowelChooser != null && !VowelChooser.IsDisposed)
				{
					VowelChooser.CharPicked -= HandleCharPicked;
					VowelChooser.ItemDrag -= HandleCharacterItemDrag;
					VowelChooser.Dispose();
				}

				if (DiacriticChooser != null && !DiacriticChooser.IsDisposed)
				{
					DiacriticChooser.CharPicked -= HandleCharPicked;
					DiacriticChooser.ItemDrag -= HandleCharacterItemDrag;
					DiacriticChooser.Dispose();
				}

				if (SuprasegmentalChooser != null && !SuprasegmentalChooser.IsDisposed)
				{
					SuprasegmentalChooser.CharPicked -= HandleCharPicked;
					SuprasegmentalChooser.ItemDrag -= HandleCharacterItemDrag;
					SuprasegmentalChooser.Dispose();
				}

				if (ToneChooser != null && !ToneChooser.IsDisposed)
				{
					ToneChooser.CharPicked -= HandleCharPicked;
					ToneChooser.ItemDrag -= HandleCharacterItemDrag;
					ToneChooser.Dispose();
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
			Load(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the explorer bar with all the necessary IPA character choosers for
		/// classes based on IPA characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Load(Func<IPASymbol, bool> shouldLoadCharDelegate)
		{
			if (App.DesignMode)
				return;

			ShouldLoadCharacterDelegate = shouldLoadCharDelegate;
			Utils.SetWindowRedraw(this, false, false);

			// Loop through the list of character types for which to build a chooser.
			foreach (IPASymbolTypeInfo typeInfo in TypesToShow)
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
		private void LayoutPickers(bool suspendDraw)
		{
			if (suspendDraw)
				Utils.SetWindowRedraw(this, false, false);

			foreach (var item in Items)
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
			ConsonantChooser = new CharPicker();
			ConsonantChooser.Name = "chrPickerConsonants";
			ConsonantChooser.CharPicked += HandleCharPicked;
			ConsonantChooser.ItemDrag += HandleCharacterItemDrag;
			ConsonantChooser.LoadCharacterType(typeInfo, ShouldLoadCharacterDelegate);
			ConsonantChooser.CheckItemsOnClick = false;
			ConsonantChooser.AutoSizeItems = true;

			var item = Add(ConsonantChooser);

			App.RegisterForLocalization(item.Button, 
				"IPACharacterExplorer.ConsonantChooserHeading", "Consonants", 
				"Text on heading above list of consonants from which to choose in side bar of search and XY chart views.");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the non pulmonic consonants character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadNonPulmonics(IPASymbolTypeInfo typeInfo)
		{
			NonPulmonicsConsonantsChooser = new CharPicker();
			NonPulmonicsConsonantsChooser.Name = "chrPickerNonPulmonics";
			NonPulmonicsConsonantsChooser.CharPicked += HandleCharPicked;
			NonPulmonicsConsonantsChooser.ItemDrag += HandleCharacterItemDrag;
			NonPulmonicsConsonantsChooser.LoadCharacterType(typeInfo, ShouldLoadCharacterDelegate);
			NonPulmonicsConsonantsChooser.CheckItemsOnClick = false;
			NonPulmonicsConsonantsChooser.AutoSizeItems = true;
			
			var item = Add(NonPulmonicsConsonantsChooser);

			App.RegisterForLocalization(item.Button,
				"IPACharacterExplorer.NonPulmonicChooserHeading", "Non Pulmonics", 
				"Text on heading above list of non pulmonic consonants from which to choose in side bar of search and XY chart views.");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the other consonant character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadOthers(IPASymbolTypeInfo typeInfo)
		{
			OtherConsonantsChooser = new CharPicker();
			OtherConsonantsChooser.Name = "chrPickerOthers";
			OtherConsonantsChooser.CharPicked += HandleCharPicked;
			OtherConsonantsChooser.ItemDrag += HandleCharacterItemDrag;
			OtherConsonantsChooser.LoadCharacterType(typeInfo, ShouldLoadCharacterDelegate);
			OtherConsonantsChooser.CheckItemsOnClick = false;
			OtherConsonantsChooser.AutoSizeItems = true;
			
			var item = Add(OtherConsonantsChooser);

			App.RegisterForLocalization(item.Button,
				"IPACharacterExplorer.OtherSymbolChooserHeading", "Other Symbols",
				"Text on heading above list of other symbols from which to choose in side bar of search and XY chart views.");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the vowel character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadVowels(IPASymbolTypeInfo typeInfo)
		{
			VowelChooser = new CharPicker();
			VowelChooser.Name = "chrPickerVowels";
			VowelChooser.CharPicked += HandleCharPicked;
			VowelChooser.ItemDrag += HandleCharacterItemDrag;
			VowelChooser.LoadCharacterType(typeInfo, ShouldLoadCharacterDelegate);
			VowelChooser.CheckItemsOnClick = false;
			VowelChooser.AutoSizeItems = true;

			var item = Add(VowelChooser);

			App.RegisterForLocalization(item.Button,
				"IPACharacterExplorer.VowelChooserHeading", "Vowels",
				"Text on heading above list of vowels from which to choose in side bar of search and XY chart views.");
		}
        
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the diacritics character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadDiacritics(IPASymbolTypeInfo typeInfo)
		{
			DiacriticChooser = new CharPicker();
			DiacriticChooser.Name = "chrPickerDiacritics";
			DiacriticChooser.CharPicked += HandleCharPicked;
			DiacriticChooser.ItemDrag += HandleCharacterItemDrag;
			DiacriticChooser.LoadCharacterType(typeInfo, ShouldLoadCharacterDelegate);
			
			DiacriticChooser.CheckItemsOnClick = false;
			DiacriticChooser.AutoSizeItems = true;

			var item = Add(DiacriticChooser);

			App.RegisterForLocalization(item.Button,
				"IPACharacterExplorer.DiacriticChooserHeading", "Diacritics",
				"Text on heading above list of diacritics from which to choose in side bar of search and XY chart views.");

			// Enlarge the font and cell size
			DiacriticChooser.Font = FontHelper.MakeFont(DiacriticChooser.Font, kBigFontSize);
			DiacriticChooser.ItemSize = new Size(40, 46);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the suprasegmental character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadSSegs(IPASymbolTypeInfo typeInfo)
		{
			SuprasegmentalChooser = new CharPicker();
			SuprasegmentalChooser.Name = "chrPickerSSegs";
			SuprasegmentalChooser.CharPicked += HandleCharPicked;
			SuprasegmentalChooser.ItemDrag += HandleCharacterItemDrag;
			SuprasegmentalChooser.LoadCharacterType(typeInfo, ShouldLoadCharacterDelegate);
			
			SuprasegmentalChooser.CheckItemsOnClick = false;
			SuprasegmentalChooser.AutoSizeItems = true;
	
			var item = Add(SuprasegmentalChooser);

			App.RegisterForLocalization(item.Button,
				"IPACharacterExplorer.SSegChooserHeading", "Stress and Length\\n(Suprasegmentals)",
				"Text on heading above list of suprasegmentals from which to choose in side bar of search and XY chart views.");

			// Enlarge the font and cell size
			SuprasegmentalChooser.Font = FontHelper.MakeFont(SuprasegmentalChooser.Font, kBigFontSize);
			SuprasegmentalChooser.ItemSize = new Size(40, 46);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the tone character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadTone(IPASymbolTypeInfo typeInfo)
		{
			ToneChooser = new CharPicker();
			ToneChooser.Name = "chrPickerTone";
			ToneChooser.CharPicked += HandleCharPicked;
			ToneChooser.ItemDrag += HandleCharacterItemDrag;
			ToneChooser.LoadCharacterType(typeInfo, ShouldLoadCharacterDelegate);

			ToneChooser.CheckItemsOnClick = false;
			ToneChooser.AutoSizeItems = true;
			
			var item = Add(ToneChooser);

			App.RegisterForLocalization(item.Button,
				"IPACharacterExplorer.ToneChooserHeading", "Tone and Accents",
				"Text on heading above list of tones and accents from which to choose in side bar of search and XY chart views.");

			// Enlarge the font and cell size
			ToneChooser.Font = FontHelper.MakeFont(ToneChooser.Font, kBigFontSize);
			ToneChooser.ItemSize = new Size(40, 46);
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
		public void LoadSettings(StringCollection settings)
		{
			for (int i = 0; i < Items.Length; i++)
			{
				Items[i].IsExpanded = (settings != null && settings.Count == Items.Length ?
					bool.Parse(settings[i]) : true);
			}

			AutoScrollPosition = new Point(0, 0);
		}

		/// ------------------------------------------------------------------------------------
		public StringCollection GetExpandedStates()
		{
			var settings = new StringCollection();

			foreach (var item in Items)
				settings.Add(item.IsExpanded.ToString());

			return settings;
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of character types/sub-types to display in the explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public List<IPASymbolTypeInfo> TypesToShow { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal consonant chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public CharPicker ConsonantChooser { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal non pulmonic consonants chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public CharPicker NonPulmonicsConsonantsChooser { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal other consonants chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public CharPicker OtherConsonantsChooser { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal vowel chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public CharPicker VowelChooser { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal suprasegmental chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public CharPicker SuprasegmentalChooser { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal diacritic chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public CharPicker DiacriticChooser { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal tone chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public CharPicker ToneChooser { get; private set; }

		#endregion
	}
}
