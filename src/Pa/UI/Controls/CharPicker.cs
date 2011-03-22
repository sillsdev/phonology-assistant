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
		private Size m_itemSize = new Size(30, 32);
		private bool m_autoSizeItems;
		private Point m_itemMouseDownPoint;
		private float m_fontSize = 14;

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
		public CharPicker(IPASymbolTypeInfo typeInfo) : this()
		{
			LoadCharacterType(typeInfo, null);
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
			return (m_itemSize.Width + (kDefaultItemMargin * 2)) * numberOfColumns + 2;
		}

		/// ------------------------------------------------------------------------------------
		public int GetMaxNumberOfColumnsToFitWidth(int width)
		{
			// Whack off margins and borders.
			width -= ((kDefaultItemMargin * 2) + 2);
			return width / m_itemSize.Width;
		}

		/// ------------------------------------------------------------------------------------
		public float FontSize
		{
			get { return m_fontSize; }
			set
			{
				if (m_fontSize != value)
				{
					m_fontSize = value;
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
				m_fontSize = value.SizeInPoints;
				m_itemSize = new Size(base.Font.Height, base.Font.Height + 2);
				
				foreach (ToolStripItem item in Items)
				{
					item.Font = value;
					if (!item.AutoSize)
						item.Size = m_itemSize;
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
			get { return m_itemSize; }
			set
			{
				if (m_itemSize == value)
					return;

				m_itemSize = value;

				for (int i = 0; i < Items.Count; i++)
					Items[i].Size = m_itemSize;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not each item is auto sized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AutoSizeItems
		{
			get { return m_autoSizeItems; }
			set
			{
				if (m_autoSizeItems == value)
					return;

				m_autoSizeItems = value;

				for (int i = 0; i < Items.Count; i++)
					Items[i].AutoSize = m_autoSizeItems;
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
					count = Items.Cast<ToolStripButton>().Count(item => item.Bounds.Left == left);
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
				var checkedItems = Items.Cast<ToolStripButton>().Where(item => item.Checked).ToList();
				return (checkedItems.Count == 0 ? null : checkedItems.ToArray());
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

					foreach (var item in Items.Cast<ToolStripButton>().Where(item => item.Bounds.Left == left))
						height = item.Bounds.Bottom + 3 + item.Margin.Bottom;
				}

				return height;
			}
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

			e.Item.AutoSize = m_autoSizeItems;
			e.Item.Size = m_itemSize;
			e.Item.TextAlign = ContentAlignment.MiddleCenter;
			e.Item.DisplayStyle = ToolStripItemDisplayStyle.Text;
			e.Item.Margin = new Padding(kDefaultItemMargin);
			e.Item.MouseMove += Item_MouseMove;

			// Save the point at which the mouse went down over the item.
			e.Item.MouseDown += delegate(object sender, MouseEventArgs mea)
			{
				m_itemMouseDownPoint =
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
			if (e.Button == MouseButtons.Left && m_itemMouseDownPoint != Point.Empty &&
				ItemDrag != null)
			{
				var item = sender as ToolStripButton;

				// Only fire the ItemDrag event when the mouse cursor has moved 4 or more
				// pixels from where the mouse button went down.
				int dx = Math.Abs(m_itemMouseDownPoint.X - e.X);
				int dy = Math.Abs(m_itemMouseDownPoint.Y - e.Y);
				
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

		#region Methods for Loading Characters
		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with the specified character type.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacterType(IPASymbolTypeInfo typeInfo, Func<IPASymbol, bool> getShouldLoad)
		{
			var list = App.IPASymbolCache.Values.Where(ci =>
				ci.Type == typeInfo.Type && (ci.SubType == typeInfo.SubType || ci.SubType == IPASymbolSubType.Unknown));
			
			LoadCharacters(list, getShouldLoad);
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with the specified ignore character type.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacterType(IPASymbolIgnoreType type)
		{
			LoadCharacters(App.IPASymbolCache.Values.Where(ci => ci.IgnoreType == type));
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
		/// Load the chooser with characters checked by a loading delegate.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacters(IEnumerable<IPASymbol> symbols)
		{
			LoadCharacters(symbols, null);
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with characters checked by a loading delegate.
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
			InternalLoad(symbols.OrderBy(ci => ci.POArticulation)
				.Where(ci => (getShouldLoad == null ? true : getShouldLoad(ci))));
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
				fmt = App.GetString("CharacterPickerShortToolTip", "{0}",
					"Used to format the tooltip string for items in an IPA character picker control when the character has no description (argument is the character name).");
			}
			else
			{
				fmt = App.GetString("CharacterPickerLongToolTip", "{0},\n{1}",
					"Used to format the tooltip string for items in an IPA character picker control (1st argument is the character name, and the 2rd argument is for the description)");
			}
			
			string tooltip = string.Format(fmt, charInfo.Name, charInfo.Description);
			return Utils.ConvertLiteralNewLines(tooltip);
		}

		#endregion
	}
}
