using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIL.Pa.Data
{
	public partial class StartingSQLServerWnd : Form
	{
		public StartingSQLServerWnd()
		{
			InitializeComponent();

			Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - Width - 20,
				Screen.PrimaryScreen.WorkingArea.Bottom - Height - 2);

			double maxOpacity = Opacity;
			Opacity = 0;
			Show();

			while (Opacity < maxOpacity)
			{
				Opacity += 0.05f;
				Application.DoEvents();
				System.Threading.Thread.Sleep(50);
			}

			Opacity = maxOpacity;
			Application.DoEvents();
		}
	}
}