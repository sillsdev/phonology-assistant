using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing.Drawing2D;
using SIL.SpeechTools.Utils;

namespace SIL.Localize.Localizer
{
	public partial class LabeledTextBox : UserControl
	{
		private bool m_useVisualStyles;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public LabeledTextBox()
		{
			InitializeComponent();

			m_useVisualStyles = 
				Application.VisualStyleState == VisualStyleState.ClientAndNonClientAreasEnabled;

			if (!m_useVisualStyles)
			{
				Padding = new Padding(0);
				BorderStyle = BorderStyle.Fixed3D;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TextBox TextBox
		{
			get { return txtText; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Label HeadingLabel
		{
			get { return lblHeading; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			if (!m_useVisualStyles)
				return;

			using (Pen pen = new Pen(VisualStyleInformation.TextControlBorder))
			{
				Rectangle rc = ClientRectangle;
				rc.Height--;
				rc.Width--;
				e.Graphics.DrawRectangle(pen, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lblHeading_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rc = lblHeading.ClientRectangle;

			Color clr2 = ColorHelper.CalculateColor(Color.Black, lblHeading.BackColor, 25);
			using (LinearGradientBrush br = new LinearGradientBrush(rc, lblHeading.BackColor, clr2, 91f))
				e.Graphics.FillRectangle(br, rc);
			
			rc.Y = rc.Height - 2;
			rc.Height = 1;
			rc.X += 2;
			using (LinearGradientBrush br = new LinearGradientBrush(rc,
				lblHeading.ForeColor, lblHeading.BackColor, LinearGradientMode.Horizontal))
			{
				e.Graphics.FillRectangle(br, rc);
			}

			rc = lblHeading.ClientRectangle;
			rc.Height -= 4;
			TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
			TextRenderer.DrawText(e.Graphics, lblHeading.Text,
				lblHeading.Font, rc, lblHeading.ForeColor, flags);
		}
	}
}
