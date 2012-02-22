using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Localization;
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

		protected Func<IPASymbol, bool> ShouldLoadCharacterDelegate { get; set; }
		protected readonly List<CharPicker> _pickers = new List<CharPicker>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the fonts for each picker.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshFont()
		{
			foreach (var item in Items)
			{
				var picker = item.Control as CharPicker;
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
				foreach (var picker in _pickers.Where(picker => !picker.IsDisposed))
				{
					picker.CharPicked -= HandleCharPicked;
					picker.ItemDrag -= HandleCharacterItemDrag;
					picker.Dispose();
				}

				_pickers.Clear();
			}
			
			base.Dispose(disposing);
		}

		#region Static methods to determine the types in a mask
		/// ------------------------------------------------------------------------------------
		protected static bool GetHasConsonantType(int types)
		{
			return ((types & (int)IPASymbolType.consonant) > 0);
		}

		/// ------------------------------------------------------------------------------------
		protected static bool GetHasVowelType(int types)
		{
			return ((types & (int)IPASymbolType.vowel) > 0);
		}

		/// ------------------------------------------------------------------------------------
		protected static bool GetHasDiacriticType(int types)
		{
			return ((types & (int)IPASymbolType.diacritic) > 0);
		}

		/// ------------------------------------------------------------------------------------
		protected static bool GetHasStressType(int types)
		{
			return ((types & (int)IPASymbolSubType.stress) > 0);
		}

		/// ------------------------------------------------------------------------------------
		protected static bool GetHasLengthType(int types)
		{
			return ((types & (int)IPASymbolSubType.length) > 0);
		}
		
		/// ------------------------------------------------------------------------------------
		protected static bool GetHasBoundaryType(int types)
		{
			return ((types & (int)IPASymbolSubType.boundary) > 0);
		}
		
		/// ------------------------------------------------------------------------------------
		protected static bool GetHasToneType(int types)
		{
			return ((types & (int)IPASymbolSubType.tone) > 0);
		}

		#endregion

		#region Methods for loading pickers
		/// ------------------------------------------------------------------------------------
		private IEnumerable<CharPicker> GetListOfPickersToShow(int typesToShow)
		{
			if (GetHasConsonantType(typesToShow))
				yield return CreatePicker((int)IPASymbolType.consonant, "chrPickerVowels", false);

			if (GetHasVowelType(typesToShow))
				yield return CreatePicker((int)IPASymbolType.vowel, "chrPickerVowels", false);

			if (GetHasDiacriticType(typesToShow))
				yield return CreatePicker((int)IPASymbolType.diacritic, "chrPickerDiacritics", true);

			if (GetHasStressType(typesToShow))
				yield return CreatePicker((int)IPASymbolSubType.stress, "chrPickerStress", true);

			if (GetHasLengthType(typesToShow))
				yield return CreatePicker((int)IPASymbolSubType.length, "chrPickerLength", true);

			if (GetHasBoundaryType(typesToShow))
				yield return CreatePicker((int)IPASymbolSubType.boundary, "chrPickerBoundaries", false);

			if (GetHasToneType(typesToShow))
				yield return CreatePicker((int)IPASymbolSubType.tone, "chrPickerTone", true);
		}

		/// ------------------------------------------------------------------------------------
		private CharPicker CreatePicker(int typeInfo, string name, bool makeBigFont)
		{
			var picker = new CharPicker();
			picker.Name = name;
			picker.CharPicked += HandleCharPicked;
			picker.ItemDrag += HandleCharacterItemDrag;
			picker.LoadCharacterType(typeInfo, ShouldLoadCharacterDelegate);
			picker.CheckItemsOnClick = false;
			picker.AutoSizeItems = true;

			if (makeBigFont)
			{
				picker.Font = FontHelper.MakeFont(picker.Font, kBigFontSize);
				picker.ItemSize = new Size(40, 46);
			}

			var item = Add(picker);
			LocalizePickerButton(typeInfo, item.Button);

			return picker;
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Load(int typesToLoad)
		{
			Load(typesToLoad, null);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Load(int typesToLoad, Func<IPASymbol, bool> shouldLoadCharDelegate)
		{
			if (App.DesignMode)
				return;

			ShouldLoadCharacterDelegate = shouldLoadCharDelegate;
			Utils.SetWindowRedraw(this, false, false);

			_pickers.Clear();
			_pickers.AddRange(GetListOfPickersToShow(typesToLoad));

			Dock = DockStyle.Fill;
			LayoutPickers(false);
			Utils.SetWindowRedraw(this, true, true);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void LocalizePickerButton(int typeInfo, Button button)
		{
			if (GetHasConsonantType(typeInfo))
			{
				LocalizationManager.GetString("CommonControls.CharacterPicker.ConsonantHeading",
					"Consonant", null, button);
			}
			if (GetHasVowelType(typeInfo))
			{
				LocalizationManager.GetString("CommonControls.CharacterPicker.VowelHeading",
					"Vowel", null, button);
			}
			if (GetHasDiacriticType(typeInfo))
			{
				LocalizationManager.GetString("CommonControls.CharacterPicker.DiacriticHeading",
					"Diacritic", null, button);
			}
			if (GetHasStressType(typeInfo))
			{
				LocalizationManager.GetString("CommonControls.CharacterPicker.StressHeading",
					"Stress", null, button);
			}
			if (GetHasLengthType(typeInfo))
			{
				LocalizationManager.GetString("CommonControls.CharacterPicker.LengthHeading",
					"Length", null, button);
			}
			if (GetHasBoundaryType(typeInfo))
			{
				LocalizationManager.GetString("CommonControls.CharacterPicker.BoundaryHeading",
					"Boundary", null, button);
			}
			if (GetHasToneType(typeInfo))
			{
				LocalizationManager.GetString("CommonControls.CharacterPicker.ToneHeading",
					"Tone", null, button);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected void LayoutPickers(bool suspendDraw)
		{
			if (suspendDraw)
				Utils.SetWindowRedraw(this, false, false);

			foreach (var item in Items)
			{
				var picker = item.Control as CharPicker;
				if (picker != null && picker == item.Control)
					item.SetHostedControlHeight(picker.PreferredHeight + 10);
			}

			if (suspendDraw)
				Utils.SetWindowRedraw(this, true, true);
		}

		#endregion

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Pass on item dragging events.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleCharacterItemDrag(object sender, ItemDragEventArgs e)
		{
			if (ItemDrag != null)
				ItemDrag(sender, e);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleCharPicked(CharPicker picker, ToolStripButton item)
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

		#endregion

		#region Loading/Restoring Settings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Restores the expanded states.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void LoadSettings(StringCollection settings)
		{
			for (int i = 0; i < Items.Length; i++)
				Items[i].IsExpanded = (!(settings != null && settings.Count == Items.Length) || bool.Parse(settings[i]));

			AutoScrollPosition = new Point(0, 0);
		}

		/// ------------------------------------------------------------------------------------
		public virtual StringCollection GetExpandedStates()
		{
			var settings = new StringCollection();

			foreach (var item in Items)
				settings.Add(item.IsExpanded.ToString(CultureInfo.InvariantCulture));

			return settings;
		}

		#endregion
	}
}
