using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public class CharPicker : ToolStrip
	{
		private const int kDefaultItemMargin = 1;

		public delegate bool ShouldLoadCharHandler(CharPicker picker, IPASymbol charInfo);
		public delegate void CharPickedHandler(CharPicker picker, ToolStripButton item);

		public event CharPickedHandler CharPicked;
		public event ItemDragEventHandler ItemDrag;
		private Size _itemSize = new Size(30, 32);
		private bool _autoSizeItems;
		private Point _itemMouseDownPoint;
		private float _fontSize = 14;

		/// ------------------------------------------------------------------------------------
		public CharPicker()
		{
			CheckItemsOnClick = true;
			Padding = new Padding(0);
			BackColor = Color.Transparent;
			base.AutoSize = false;
			LayoutStyle = ToolStripLayoutStyle.Flow;
			base.Dock = DockStyle.None;
			RenderMode = ToolStripRenderMode.ManagerRenderMode;
			Renderer = new NoToolStripBorderRenderer();
			GripStyle = ToolStripGripStyle.Hidden;
			base.DoubleBuffered = true;
			SetStyle(ControlStyles.Selectable, true);
			RefreshFont();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs an instance of an IPACharChooser and loads the characters of the
		/// specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharPicker(int typeInfo) : this()
		{
			LoadCharacterType(typeInfo, null);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ToolStripButton> GetItems()
		{
			return Items.Cast<ToolStripButton>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Refreshes the font face for the picker.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshFont()
		{
			if (!App.DesignMode)
				Font = FontHelper.MakeRegularFontDerivative(App.PhoneticFont, FontSize);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the preferred height of a single char picker item based on the font. This
		/// does not include margins.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int PreferredItemHeight
		{
			get { return Font.Height; }
		}

		/// ------------------------------------------------------------------------------------
		public int GetPreferredWidth(int numberOfColumns)
		{
			// Add two for the borders.
			return (_itemSize.Width + (kDefaultItemMargin * 2)) * numberOfColumns + 2;
		}

		/// ------------------------------------------------------------------------------------
		public int GetPreferredHeight()
		{
			return GetPreferredHeight(NumberOfRows);
		}

		/// ------------------------------------------------------------------------------------
		public int GetPreferredHeight(int numberOfRows)
		{
			// Add two for the borders.
			return (_itemSize.Height + (kDefaultItemMargin * 2)) * numberOfRows + 2;
		}

		/// ------------------------------------------------------------------------------------
		public int GetMaxNumberOfColumnsToFitWidth(int width)
		{
			// Whack off margins and borders.
			width -= ((kDefaultItemMargin * 2) + 2);
			return width / _itemSize.Width;
		}

		/// ------------------------------------------------------------------------------------
		public float FontSize
		{
			get { return _fontSize; }
			set
			{
				if (_fontSize.Equals(value))
				{
					_fontSize = value;
					RefreshFont();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the font used for the IPA characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override Font Font
		{
			get { return base.Font; }
			set
			{
				if (FontHelper.AreFontsSame(base.Font, value))
					return;

				base.Font = value;
				_fontSize = value.SizeInPoints;
				_itemSize = new Size(base.Font.Height, base.Font.Height + 2);
				
				foreach (ToolStripItem item in Items)
				{
					item.Font = value;
					if (!item.AutoSize)
						item.Size = _itemSize;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not item's check state should change
		/// when clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CheckItemsOnClick { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the size of each item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Size ItemSize
		{
			get { return _itemSize; }
			set
			{
				if (_itemSize == value)
					return;

				_itemSize = value;

				for (int i = 0; i < Items.Count; i++)
					Items[i].Size = _itemSize;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not each item is auto sized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AutoSizeItems
		{
			get { return _autoSizeItems; }
			set
			{
				if (_autoSizeItems == value)
					return;

				_autoSizeItems = value;

				for (int i = 0; i < Items.Count; i++)
					Items[i].AutoSize = _autoSizeItems;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the number of rows currently in the picker.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int NumberOfRows
		{
			get
			{
				// Start with top and bottom borders.
				int count = 0;

				if (Items.Count > 0)
				{
					int left = Items[0].Bounds.Left;
					count = GetItems().Count(item => item.Bounds.Left == left);
				}

				return count;
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the checked items in the picker.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ToolStripButton[] CheckedItems
		{
			get
			{
				var checkedItems = GetItems().Where(item => item.Checked).ToArray();
				return (checkedItems.Length == 0 ? new ToolStripButton[0] : checkedItems);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the minimum height allowed (given the picker's current width) so all 
		/// characters are visible in the picker.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int PreferredHeight
		{
			get
			{
				int height = 0;

				if (Items.Count > 0)
				{
					int left = Items[0].Bounds.Left;

					foreach (var item in GetItems().Where(item => item.Bounds.Left == left))
						height = item.Bounds.Bottom + 3 + item.Margin.Bottom;
				}

				return height;
			}
		}

		/// ------------------------------------------------------------------------------------
		public CheckState GetRelevantCheckState()
		{
			var checkedItemCount = CheckedItems.Length;

			if (checkedItemCount == Items.Count)
				return CheckState.Checked;
			
			return  (checkedItemCount == 0 ? CheckState.Unchecked : CheckState.Indeterminate);
		}

		/// ------------------------------------------------------------------------------------
		public void Clear()
		{
			Items.Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the item just added.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnItemAdded(ToolStripItemEventArgs e)
		{
			base.OnItemAdded(e);

			e.Item.AutoSize = _autoSizeItems;
			e.Item.Size = _itemSize;
			e.Item.TextAlign = ContentAlignment.MiddleCenter;
			e.Item.DisplayStyle = ToolStripItemDisplayStyle.Text;
			e.Item.Margin = new Padding(kDefaultItemMargin);
			e.Item.MouseMove += Item_MouseMove;

			// Save the point at which the mouse went down over the item.
			e.Item.MouseDown += delegate(object sender, MouseEventArgs mea)
			{
				_itemMouseDownPoint =
					(mea.Button == MouseButtons.Left ? mea.Location : Point.Empty);
			};
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Track mouse movements to determine whether or not to go into the drag mode.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Item_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && _itemMouseDownPoint != Point.Empty &&
				ItemDrag != null)
			{
				var item = sender as ToolStripButton;

				// Only fire the ItemDrag event when the mouse cursor has moved 4 or more
				// pixels from where the mouse button went down.
				int dx = Math.Abs(_itemMouseDownPoint.X - e.X);
				int dy = Math.Abs(_itemMouseDownPoint.Y - e.Y);
				
				if (item != null && (dx >= 4 || dy >= 4))
				{
					var args = new ItemDragEventArgs(e.Button, item.Text);
					ItemDrag(item, args);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
		{
			var item = e.ClickedItem as ToolStripButton;
			if (item == null)
				return;

			if (CheckItemsOnClick)
				item.Checked = !item.Checked;
			
			base.OnItemClicked(e);

			if (CharPicked != null)
				CharPicked(this, item);
		}

		/// --------------------------------------------------------------------------------------------
		public void CheckAllItems()
		{
			foreach (var item in GetItems())
				item.Checked = true;
		}

		/// --------------------------------------------------------------------------------------------
		public void UncheckAllItems()
		{
			foreach (var item in GetItems())
				item.Checked = false;
		}

		/// --------------------------------------------------------------------------------------------
		public void SetCheckedItemsByText(IEnumerable<string> textsOfItemsToCheck)
		{
			foreach (var text in textsOfItemsToCheck)
			{
				var item = GetItems().FirstOrDefault(i => i.Text.Replace(App.kDottedCircle, string.Empty) == text);
				
				if (item != null)
				{
					item.Checked = true;
					item.Tag = true;
				}
			}
		}

		/// --------------------------------------------------------------------------------------------
		public IEnumerable<string> GetTextsOfCheckedItems()
		{
			return CheckedItems.Select(i => i.Text.Replace(App.kDottedCircle, string.Empty));
		}

		#region Methods for Loading Characters
		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with the specified character type.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacterType(int typeInfo, Func<IPASymbol, bool> getShouldLoad)
		{
			var list = App.IPASymbolCache.Values.Where(ci => (((int)ci.Type | (int)ci.SubType) & typeInfo) > 0);
			LoadCharacters(list, getShouldLoad);
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with the specified symbol sub type.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacterType(IPASymbolSubType type)
		{
			LoadCharacters(App.IPASymbolCache.Values.Where(ci => ci.SubType == type));
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with characters checked by a loading delegate.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacters(Func<IPASymbol, bool> getShouldLoad)
		{
			LoadCharacters(App.IPASymbolCache.Values, getShouldLoad);
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with the specified symbols.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacters(IEnumerable<IPASymbol> symbols)
		{
			LoadCharacters(symbols, null);
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with phones.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacters(IEnumerable<IPhoneInfo> phones)
		{
			Items.AddRange(phones.Select(p => new ToolStripButton(p.Phone)).ToArray());
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with characters checked by a loading delegate.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacters(IEnumerable<IPASymbol> symbols, Func<IPASymbol, bool> getShouldLoad)
		{
			InternalLoad(symbols.OrderBy(ci => ci.DisplayOrder)
				.Where(ci => (getShouldLoad == null || getShouldLoad(ci))));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the picker with information from a collection of PickerItemInfo objects
		/// created from one of the public loading methods.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InternalLoad(IEnumerable<IPASymbol> symbols)
		{
			Items.AddRange(symbols.Select(ci =>
				new ToolStripButton(GetDisplayableChar(ci)) { ToolTipText = GetCharsToolTipText(ci) }).ToArray());
		}

		/// ------------------------------------------------------------------------------------
		private static string GetDisplayableChar(IPASymbol ci)
		{
			return (ci.DisplayWithDottedCircle ? App.kDottedCircle : string.Empty) + ci.Literal;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a tooltip for the specified character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetCharsToolTipText(IPASymbol charInfo)
		{
			string fmt;

			if (string.IsNullOrEmpty(charInfo.Description))
			{
				fmt = App.GetString("CommonControls.CharacterPicker.ShortToolTip", "{0}",
					"Used to format the tooltip string for items in an IPA character picker control when the character has no description (argument is the character name).");
			}
			else
			{
				fmt = App.GetString("CommonControls.CharacterPicker.LongToolTip", "{0},\n{1}",
					"Used to format the tooltip string for items in an IPA character picker control (1st argument is the character name, and the 2rd argument is for the description)");
			}
			
			string tooltip = string.Format(fmt, charInfo.Name, charInfo.Description);
			return Utils.ConvertLiteralNewLines(tooltip);
		}

		#endregion
	}
}
