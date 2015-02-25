// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIL.Pa.UI.Dialogs
{
	public partial class BRProgressDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public BRProgressDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For some reason, setting the progress dialog's start position property to center
		/// relative to its parent didn't work. Therefore, we'll do it ourselves.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CenterInParent(Form parent)
		{
			Top = parent.Top + Math.Max(0, (parent.Height - Height) / 2);

			if (Width < parent.Width)
				Left = parent.Left + Math.Max(0, Left = (parent.Width - Width) / 2);
			else
				Left = parent.Left - Math.Max(0, Left = (Width - parent.Width) / 2);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
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
