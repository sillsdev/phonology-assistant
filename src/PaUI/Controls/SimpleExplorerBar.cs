using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a simple explorer bar-like control.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SimpleExplorerBar : PaPanel
	{
		private readonly List<ExplorerBarItem> m_items = new List<ExplorerBarItem>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a SimpleExplorerBar object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SimpleExplorerBar()
		{
			base.AutoScroll = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the collection of ExplorerBarItems contained in the explorer bar.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ExplorerBarItem[] Items
		{
			get {return m_items.ToArray();}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the control's background color.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get	{return base.BackColor;}
			set
			{
				base.BackColor = value;
				foreach (ExplorerBarItem item in m_items)
					item.BackColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an ExplorerBarItem with the specified text and hosting the specified control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ExplorerBarItem Add(string text, Control control)
		{
			ExplorerBarItem item = new ExplorerBarItem(text, control);
			Add(item);
			return item;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified item to the item collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Add(ExplorerBarItem item)
		{
			item.Dock = DockStyle.Top;
			item.BackColor = BackColor;
			m_items.Add(item);
			Controls.Add(item);
			item.BringToFront();

			item.Collapsed += item_SizeChanged;
			item.Expanded += item_SizeChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles a item expanding or collapsing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void item_SizeChanged(object sender, EventArgs e)
		{
			ScrollControlIntoView(sender as ExplorerBarItem);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the specified item from the collection of items.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Remove(ExplorerBarItem item)
		{
			if (item != null && m_items.Contains(item))
			{
				Controls.Remove(item);
				m_items.Remove(item);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes from the item collection the item at the specified index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Remove(int index)
		{
			if (index < m_items.Count)
				Remove(m_items[index]);
		}
	}
}
