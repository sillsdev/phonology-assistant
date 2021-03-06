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
using SilTools;

namespace SIL.Pa.DataSource
{
	public partial class MissingDataSourceMsgBox : Form
	{
		private bool m_buttonClicked;
		private string m_filename;

		/// ------------------------------------------------------------------------------------
		public MissingDataSourceMsgBox()
		{
			InitializeComponent();
			base.Text = Application.ProductName;
			picIcon.Image = SystemIcons.Question.ToBitmap();
			lblMsg.Font = FontHelper.UIFont;
			lblFileName.Font = FontHelper.UIFont;
			lblFileName.Text = string.Empty;
		}

		/// ------------------------------------------------------------------------------------
		public static DialogResult ShowDialog(string dataSourceFileName)
		{
			using (var msgBox = new MissingDataSourceMsgBox())
			{
				msgBox.m_filename = dataSourceFileName;
				App.CloseSplashScreen();
				return msgBox.ShowDialog();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the only way out of the message box is to click one of the two buttons.
		/// Clicking on the windows 'X' button is ambiguous when determining whether or not
		/// the user wants to choose a different location or skip loading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
		
			if (!m_buttonClicked)
				e.Cancel = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleButtonClicked(object sender, EventArgs e)
		{
			m_buttonClicked = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnHelp_Click(object sender, EventArgs e)
		{
			App.ShowHelpTopic(this);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the file's path using the path ellipsis formatting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lblFileName_Paint(object sender, PaintEventArgs e)
		{
			TextFormatFlags flags = TextFormatFlags.PathEllipsis | TextFormatFlags.NoPadding;
			TextRenderer.DrawText(e.Graphics, m_filename, lblFileName.Font,
				lblFileName.ClientRectangle, lblFileName.ForeColor, flags);
		
			flags = TextFormatFlags.NoPadding;
			Size sz = TextRenderer.MeasureText(e.Graphics, m_filename, lblFileName.Font,
				Size.Empty, flags);

			if (lblFileName.ClientSize.Width < sz.Width)
				m_toolTip.SetToolTip(lblFileName, m_filename);
		}
	}
}