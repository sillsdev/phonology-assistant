using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CharPicker : ToolStrip
	{
		private struct PickerItemInfo
		{
			internal readonly string tooltip;
			internal readonly string character;

			internal PickerItemInfo(string chr, string tip)
			{
				character = chr;
				tooltip = tip;
			}
		}

		private const int kDefaultItemMargin = 1;

		public delegate bool ShouldLoadCharHandler(CharPicker picker, IPASymbol charInfo);
		public delegate void CharPickedHandler(CharPicker picker, ToolStripButton item);

		public event CharPickedHandler CharPicked;
		public event ShouldLoadCharHandler ShouldLoadChar;
		public event ItemDragEventHandler ItemDrag;
		private Size m_itemSize = new Size(30, 32);
		private bool m_checkItemsOnClick = true;
		private bool m_autoSizeItems;
		private SortedDictionary<int, PickerItemInfo> m_charsToLoad;
		private Point m_itemMouseDownPoint;
		private float m_fontSize = 14;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharPicker()
		{
			Padding = new Padding(0);
			BackColor = Color.Transparent;
			base.AutoSize = false;
			LayoutStyle = ToolStripLayoutStyle.Flow;
			base.Dock = DockStyle.None;
			RenderMode = ToolStripRenderMode.ManagerRenderMode;
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
			LoadCharacterType(typeInfo);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Refreshes the font face for the picker.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshFont()
		{
			if (!App.DesignMode)
				Font = FontHelper.MakeEticRegFontDerivative(m_fontSize);
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int GetPreferredWidth(int numberOfColumns)
		{
			// Add two for the borders.
			return (m_itemSize.Width + (kDefaultItemMargin * 2)) * numberOfColumns + 2;
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
				m_fontSize = value.SizeInPoints;
				base.Font = value;
				foreach (ToolStripItem item in Items)
					item.Font = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not item's check state should change
		/// when clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CheckItemsOnClick
		{
			get { return m_checkItemsOnClick; }
			set { m_checkItemsOnClick = value; }
		}

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

					foreach (ToolStripButton item in Items)
					{
						if (item.Bounds.Left == left)
							count++;
					}
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
				List<ToolStripButton> checkedItems = new List<ToolStripButton>();
				foreach (ToolStripButton item in Items)
				{
					if (item.Checked)
						checkedItems.Add(item);
				}

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

					foreach (ToolStripButton item in Items)
					{
						if (item.Bounds.Left == left)
							height = item.Bounds.Bottom + 3 + item.Margin.Bottom;
					}
				}

				return height;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint over (i.e. hide) a border with a rounded corner on the right and bottom
		/// of the toolstrip.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Color bkColor = SystemColors.Window;
			bool isOnTab = false;

			if (Parent != null)
				bkColor = GetParentBackColor(out isOnTab);

			if (isOnTab)
			{
				VisualStyleElement element = VisualStyleElement.Tab.Body.Normal;
				if (PaintingHelper.CanPaintVisualStyle(element))
				{
					PaintWithTabPageBackground(e.Graphics, element);
					return;
				}
			}

			using (Pen pen = new Pen(bkColor))
			{
				Rectangle rc = ClientRectangle;
				e.Graphics.DrawLine(pen, 0, rc.Height - 1, rc.Right - 1, rc.Bottom - 1);
				e.Graphics.DrawLine(pen, rc.Right - 1, 0, rc.Right - 1, rc.Bottom - 1);
				e.Graphics.DrawLine(pen, rc.Right - 2, rc.Bottom - 2, rc.Right - 1, rc.Bottom - 1);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the back color of the first parent in the parent chain that doesn't have
		/// a background color of transparent.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Color GetParentBackColor(out bool isOnTab)
		{
			Control ctrlParent = Parent;

			while (ctrlParent.Parent != null && ctrlParent.BackColor == Color.Transparent)
				ctrlParent = ctrlParent.Parent;

			isOnTab = ctrlParent is TabPage || ctrlParent is TabControl;
			return ctrlParent.BackColor;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint over (i.e. hide) a border with a rounded corner on the right and bottom
		/// of the toolstrip when the tool strip is on a tab page or tab control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PaintWithTabPageBackground(IDeviceContext g, VisualStyleElement element)
		{
			VisualStyleRenderer renderer = new VisualStyleRenderer(element);
			Rectangle crc = ClientRectangle;
			
			// Paint over the bottom border.
			Rectangle rc = new Rectangle(0, crc.Height - 1, crc.Width, 2);
			renderer.DrawBackground(g, rc);

			// Paint over the right border.
			rc = new Rectangle(crc.Width - 1, 0, 2, crc.Height);
			renderer.DrawBackground(g, rc);

			// Paint over the little rounded corner border in the bottom, right.
			rc = new Rectangle(crc.Width - 2, crc.Height - 2, 2, 2);
			renderer.DrawBackground(g, rc);
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
				ToolStripButton item = sender as ToolStripButton;

				// Only fire the ItemDrag event when the mouse cursor has moved 4 or more
				// pixels from where the mouse button went down.
				int dx = Math.Abs(m_itemMouseDownPoint.X - e.X);
				int dy = Math.Abs(m_itemMouseDownPoint.Y - e.Y);
				
				if (item != null && (dx >= 4 || dy >= 4))
				{
					ItemDragEventArgs args = new ItemDragEventArgs(e.Button, item.Text);
					ItemDrag(item, args);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
		{
			ToolStripButton item = e.ClickedItem as ToolStripButton;
			if (item == null)
				return;

			if (m_checkItemsOnClick)
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
		public void LoadCharacterType(IPASymbolTypeInfo typeInfo)
		{
			m_charsToLoad = new SortedDictionary<int, PickerItemInfo>();

			foreach (IPASymbol charInfo in App.IPASymbolCache.Values)
			{
				if (charInfo.Type == typeInfo.Type && (charInfo.SubType == typeInfo.SubType ||
					typeInfo.SubType == IPASymbolSubType.Unknown))
				{
					if (ShouldLoadChar != null && !ShouldLoadChar(this, charInfo))
						continue;

					string chr = (charInfo.DisplayWithDottedCircle ?
						App.kDottedCircle : string.Empty) + charInfo.Literal;

					// Characters will be sorted by place of articulation.
					m_charsToLoad[charInfo.POArticulation] = 
						new PickerItemInfo(chr, GetCharsToolTipText(charInfo));
				}
			}

			InternalLoad();
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with the specified ignore character type.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacterType(IPASymbolIgnoreType type)
		{
			m_charsToLoad = new SortedDictionary<int, PickerItemInfo>();

			foreach (IPASymbol charInfo in App.IPASymbolCache.Values)
			{
				if (charInfo.IgnoreType != type)
					continue;

				if (ShouldLoadChar != null && !ShouldLoadChar(this, charInfo))
					continue;

				string chr = (charInfo.DisplayWithDottedCircle ?
					App.kDottedCircle : string.Empty) + charInfo.Literal;

				// Characters will be sorted by place of articulation.
				m_charsToLoad[charInfo.POArticulation] =
					new PickerItemInfo(chr, GetCharsToolTipText(charInfo));
			}

			InternalLoad();
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with characters checked by a loading delegate.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacters()
		{
			if (ShouldLoadChar == null)
				return;

			m_charsToLoad = new SortedDictionary<int, PickerItemInfo>();

			foreach (IPASymbol charInfo in App.IPASymbolCache.Values)
			{
				if (ShouldLoadChar(this, charInfo))
				{
					string chr = (charInfo.DisplayWithDottedCircle ?
						App.kDottedCircle : string.Empty) + charInfo.Literal;

					// Characters will be sorted by place of articulation.
					m_charsToLoad[charInfo.POArticulation] =
						new PickerItemInfo(chr, GetCharsToolTipText(charInfo));
				}
			}

			InternalLoad();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a tooltip for the specified character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetCharsToolTipText(IPASymbol charInfo)
		{
			string tooltip = string.Format(string.IsNullOrEmpty(charInfo.Description) ?
				Properties.Resources.kstidCharPickerTooltipShort :
				Properties.Resources.kstidCharPickerTooltipLong,
				charInfo.Name, charInfo.Description);

			return Utils.ConvertLiteralNewLines(tooltip);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the picker with information from a collection of PickerItemInfo objects
		/// created from one of the public loading methods.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InternalLoad()
		{
			if (m_charsToLoad != null && m_charsToLoad.Count > 0)
			{
				foreach (PickerItemInfo pickerInfo in m_charsToLoad.Values)
				{
					ToolStripButton item = new ToolStripButton(pickerInfo.character);
					item.ToolTipText = pickerInfo.tooltip;
					Items.Add(item);
				}
			}
		}

		#endregion
	}
}
