using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for LeftLabelledBox.
	/// </summary>
	public class LeftLabelledBox : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label m_label;
		private System.Windows.Forms.TextBox m_text;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public LeftLabelledBox()
		{
			InitializeComponent();
			m_text.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
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
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_label = new System.Windows.Forms.Label();
			this.m_text = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// m_label
			// 
			this.m_label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.m_label.AutoSize = true;
			this.m_label.Location = new System.Drawing.Point(0, 3);
			this.m_label.Name = "m_label";
			this.m_label.Size = new System.Drawing.Size(29, 13);
			this.m_label.TabIndex = 0;
			this.m_label.Text = "label";
			this.m_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_text
			// 
			this.m_text.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_text.Location = new System.Drawing.Point(30, 0);
			this.m_text.Name = "m_text";
			this.m_text.Size = new System.Drawing.Size(130, 20);
			this.m_text.TabIndex = 1;
			// 
			// LeftLabelledBox
			// 
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.m_text);
			this.Controls.Add(this.m_label);
			this.Name = "LeftLabelledBox";
			this.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
			this.Size = new System.Drawing.Size(160, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region Properties
		
		/// ----------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets font of the label portion of the control.
		/// </summary>
		/// ----------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[Category("Appearance")]
		[Description("Gets or sets font of the label portion of the control.")]
		public Font LabelFont
		{
			get {return m_label.Font;}
			set {m_label.Font = value;}
		}

		/// ----------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets text of the label portion of the control.
		/// </summary>
		/// ----------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[Category("Appearance")]
		[Description("Gets or sets text of the label portion of the control.")]
		public string LabelText
		{
			get {return m_label.Text;}
			set {m_label.Text = value;}
		}

		/// ----------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets font of the textbox portion of the control.
		/// </summary>
		/// ----------------------------------------------------------------------------------
		public override Font Font
		{
			get {return m_text.Font;}
			set
			{
				m_text.Font = value;
				m_label.Font = value;

				//Height = m_text.Height;

				//Padding = new Padding(m_label.Width + 3, Padding.Top, Padding.Right, Padding.Bottom);
			}
		}

		/// ----------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets text of the textbox portion of the control.
		/// </summary>
		/// ----------------------------------------------------------------------------------
		public new string Text
		{
			get {return m_text.Text;}
			set {m_text.Text = value;}
		}

		/// ----------------------------------------------------------------------------------
		/// <summary>
		/// Gets the textbox portion of the control.
		/// </summary>
		/// ----------------------------------------------------------------------------------
		[Browsable(false)]
		public TextBox TextBox
		{
			get {return m_text;}
		}

		/// ----------------------------------------------------------------------------------
		/// <summary>
		/// Gets the label portion of the control.
		/// </summary>
		/// ----------------------------------------------------------------------------------
		[Browsable(false)]
		public Label Label
		{
			get {return m_label;}
		}

		[Browsable(false)]
		public override bool Focused
		{
			get
			{
				return m_text.Focused;
			}
		}
		#endregion

		protected override void OnGotFocus(EventArgs e)
		{
			m_text.Focus();
		}
		
		private void textBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			OnKeyPress(e);
		}
	}
}
