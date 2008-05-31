using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIL.Pa.AddOn
{
	public partial class RestoreProgressDlg : Form
	{
		public RestoreProgressDlg()
		{
			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Rectangle rc = ClientRectangle;
			rc.Width--;
			rc.Height--;

			using (Pen pen = new Pen(SystemColors.ActiveCaption))
				e.Graphics.DrawRectangle(pen, rc);
		}
	}
}
