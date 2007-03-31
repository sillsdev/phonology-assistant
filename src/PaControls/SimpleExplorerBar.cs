using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SIL.Pa.Resources;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a simple explorer bar-like control.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SimpleExplorerBar : PaPanel
	{
		private List<ExplorerBarItem> m_items = new List<ExplorerBarItem>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a SimpleExplorerBar object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SimpleExplorerBar()
		{
			AutoScroll = true;
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
		public void Add(string text, Control control)
		{
			Add(new ExplorerBarItem(text, control));
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

			item.Collapsed += new EventHandler(item_SizeChanged);
			item.Expanded += new EventHandler(item_SizeChanged);
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
