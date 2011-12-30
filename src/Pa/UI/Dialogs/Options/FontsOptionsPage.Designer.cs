namespace SIL.Pa.UI.Dialogs
{
	partial class FontsOptionsPage
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._panelFonts = new SilTools.Controls.SilPanel();
			this.SuspendLayout();
			// 
			// _panelFonts
			// 
			this._panelFonts.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelFonts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelFonts.ClipTextForChildControls = true;
			this._panelFonts.ControlReceivingFocusOnMnemonic = null;
			this._panelFonts.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelFonts.DoubleBuffered = true;
			this._panelFonts.DrawOnlyBottomBorder = false;
			this._panelFonts.DrawOnlyTopBorder = false;
			this._panelFonts.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelFonts.ForeColor = System.Drawing.SystemColors.ControlText;
			this._panelFonts.Location = new System.Drawing.Point(0, 0);
			this._panelFonts.MnemonicGeneratesClick = false;
			this._panelFonts.Name = "_panelFonts";
			this._panelFonts.PaintExplorerBarBackground = false;
			this._panelFonts.Size = new System.Drawing.Size(386, 256);
			this._panelFonts.TabIndex = 1;
			// 
			// FontsOptionsPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this._panelFonts);
			this.Name = "FontsOptionsPage";
			this.Size = new System.Drawing.Size(386, 256);
			this.ResumeLayout(false);

		}

		#endregion

		private SilTools.Controls.SilPanel _panelFonts;
	}
}
