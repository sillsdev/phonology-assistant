// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005, SIL International. All Rights Reserved.   
// <copyright from='2005' to='2005' company='SIL International'>
//		Copyright (c) 2005, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: ThinBorderPanel.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for ThinBorderPanel.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ThinBorderPanel : System.Windows.Forms.UserControl
	{
		private Color m_borderColor = SystemColors.Highlight;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ThinBorderPanel"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ThinBorderPanel()
		{
			InitializeComponent();
			this.DockPadding.All = 3;
		}

		/// -----------------------------------------------------------------------------------
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged 
		/// resources; <c>false</c> to release only unmanaged resources. 
		/// </param>
		/// -----------------------------------------------------------------------------------
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// -----------------------------------------------------------------------------------
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		/// -----------------------------------------------------------------------------------
		private void InitializeComponent()
		{
			// 
			// ThinBorderPanel
			// 
			this.Name = "ThinBorderPanel";
			this.Size = new System.Drawing.Size(168, 152);

		}
		#endregion

		#region Properties
		/// -----------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the control's border color.
		/// </summary>
		/// -----------------------------------------------------------------------------------
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Category("Appearance")]
		public Color BorderColor
		{
			get {return m_borderColor;}
			set {m_borderColor = value;}
		}

		#endregion

		/// -----------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// -----------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
	
			Rectangle rc = ClientRectangle;
			e.Graphics.FillRectangle(new SolidBrush(BackColor), rc);
			e.Graphics.DrawRectangle(new Pen(m_borderColor, 1), 0, 0, rc.Right - 1, rc.Bottom - 1);
		}
	}
}
