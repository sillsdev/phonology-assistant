using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using SIL.Pa.UI.Controls.Controls;

namespace SIL.Pa.UI.Controls
{
	public partial class ColorPickerMatrix : UserControl
	{
		public event EventHandler ColorPicked;

		private const int kColorSquareSize = 18;
		private const int kNumberOfCols = 8;
		private const int kNumberOfColors = 40;

		private Dictionary<XButton, Color> m_clrButtons;
		private Color m_currColor = Color.Empty;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ColorPickerMatrix()
		{
			InitializeComponent();

			m_toolTip = new ToolTip();
			m_clrButtons = new Dictionary<XButton, Color>();

			ResourceManager resMngr = ColorPickerStrings.ResourceManager;
			string clrNameFmt = "kstidColor{0}";

			int row = 0;
			int col = 0;

			for (int i = 0; i < kNumberOfColors; i++)
			{
				// Get the entry from the resources that has the color name and RGB value.
				string resxEntry = resMngr.GetString(string.Format(clrNameFmt, i));
				if (resxEntry == null)
					continue;

				XButton btn = new XButton();
				btn.CanBeChecked = true;
				btn.DrawEmpty = true;
				btn.Size = new Size(kColorSquareSize, kColorSquareSize);
				btn.BackColor = BackColor;
				btn.Location = new Point(col * kColorSquareSize, row * kColorSquareSize);
				btn.Paint += new PaintEventHandler(btn_Paint);
				btn.Click += new EventHandler(btn_Click);
				Controls.Add(btn);

				// Parse the entry from the resx file.
				string[] entryParts = resxEntry.Split(",".ToCharArray());

				// Store the name in the tooltip and create a color from the RGB values.
				m_toolTip.SetToolTip(btn, entryParts[0]);
				m_clrButtons[btn] = Color.FromArgb(int.Parse(entryParts[1]),
					int.Parse(entryParts[2]), int.Parse(entryParts[3]));

				col++;
				if (col == kNumberOfCols)
				{
					col = 0;
					row++;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the control's current color.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Color CurrentColor
		{
			get { return m_currColor; }
			set
			{
				m_currColor = value;

				foreach (KeyValuePair<XButton, Color> square in m_clrButtons)
					square.Key.Checked = (value == square.Value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void btn_Click(object sender, EventArgs e)
		{
			XButton btn = sender as XButton;
			if (btn != null && m_clrButtons.ContainsKey(btn) && m_clrButtons[btn] != m_currColor)
			{
				CurrentColor = m_clrButtons[btn];
				if (ColorPicked != null)
					ColorPicked(this, EventArgs.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void btn_Paint(object sender, PaintEventArgs e)
		{
			XButton btn = sender as XButton;
			if (btn == null) // || btn.Parent == null)
				return;

			Rectangle rc = btn.ClientRectangle;

			using (SolidBrush br = new SolidBrush(btn.BackColor))
			{
				e.Graphics.FillRectangle(br, rc);

				br.Color = Color.Gray;
				rc.Inflate(-3, -3);
				e.Graphics.FillRectangle(br, rc);

				br.Color = m_clrButtons[btn];
				rc.Inflate(-1, -1);
				e.Graphics.FillRectangle(br, rc);
			}
		}
	}
}
