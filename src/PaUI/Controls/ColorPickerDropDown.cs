using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ColorPickerDropDown : ToolStripDropDown
	{
		public event EventHandler ColorPicked;

		private readonly ToolStripButton m_autoItem;
		private readonly ToolStripMenuItem m_moreItem;
		private readonly ColorPickerMatrix m_colorMatrix;
		private Color m_currColor;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Encapsulates a color picker drop-down almost just like Word 2003's.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ColorPickerDropDown()
		{
			LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;

			// Add the "Automatic" button.
			m_autoItem = new ToolStripButton(/*ColorPickerStrings.kstidAutomaticText*/);
			m_autoItem.TextAlign = ContentAlignment.MiddleCenter;
			m_autoItem.Click += m_autoItem_Click;
			m_autoItem.Margin = new Padding(1, m_autoItem.Margin.Top,
				m_autoItem.Margin.Right, m_autoItem.Margin.Bottom);

			base.Items.Add(m_autoItem);

			// Add all the colored squares.
			m_colorMatrix = new ColorPickerMatrix();
			m_colorMatrix.ColorPicked += m_colorMatrix_ColorPicked;
			ToolStripControlHost host = new ToolStripControlHost(m_colorMatrix);
			host.AutoSize = false;
			host.Size = new Size(m_colorMatrix.Width + 6, m_colorMatrix.Height + 6);
			host.Padding = new Padding(3);
			base.Items.Add(host);

			// Add the "More Colors..." button.
			m_moreItem = new ToolStripMenuItem(/* ColorPickerStrings.kstidMoreColors */);
			m_moreItem.TextAlign = ContentAlignment.MiddleCenter;
			m_moreItem.Click += m_moreItem_Click;
			base.Items.Add(m_moreItem);

			CurrentColor = Color.Empty;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the drop-down's current color. Color.Empty is equivalent to the
		/// automatic value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Color CurrentColor
		{
			get {return m_currColor;}
			set
			{
				m_currColor = value;
				m_colorMatrix.CurrentColor = value;
				m_autoItem.Checked = (value == Color.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle a color change from clicking on one of the colored squares.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_colorMatrix_ColorPicked(object sender, EventArgs e)
		{
			m_currColor = m_colorMatrix.CurrentColor;

			Hide();

			if (ColorPicked != null)
				ColorPicked(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show the color dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_moreItem_Click(object sender, EventArgs e)
		{
			Hide();

			using (ColorDialog dlg = new ColorDialog())
			{
				dlg.FullOpen = true;

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					CurrentColor = dlg.Color;
					if (ColorPicked != null)
						ColorPicked(this, EventArgs.Empty);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_autoItem_Click(object sender, EventArgs e)
		{
			CurrentColor = Color.Empty;
			
			Hide();

			if (ColorPicked != null)
				ColorPicked(this, EventArgs.Empty);
		}
	}
}
