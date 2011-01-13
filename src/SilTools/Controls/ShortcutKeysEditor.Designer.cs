namespace SilTools.Controls
{
	partial class ShortcutKeysEditor
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
			this.components = new System.ComponentModel.Container();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.jjToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lblModifiers = new System.Windows.Forms.Label();
			this.chkCtrl = new System.Windows.Forms.CheckBox();
			this.chkAlt = new System.Windows.Forms.CheckBox();
			this.chkShift = new System.Windows.Forms.CheckBox();
			this.lblKey = new System.Windows.Forms.Label();
			this.cboKeys = new System.Windows.Forms.ComboBox();
			this.btnOK = new SilTools.Controls.XButton();
			this.btnReset = new SilTools.Controls.XButton();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jjToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(81, 26);
			// 
			// jjToolStripMenuItem
			// 
			this.jjToolStripMenuItem.Name = "jjToolStripMenuItem";
			this.jjToolStripMenuItem.Size = new System.Drawing.Size(80, 22);
			this.jjToolStripMenuItem.Text = "jj";
			// 
			// lblModifiers
			// 
			this.lblModifiers.AutoSize = true;
			this.lblModifiers.BackColor = System.Drawing.Color.Transparent;
			this.lblModifiers.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblModifiers.Location = new System.Drawing.Point(8, 8);
			this.lblModifiers.Name = "lblModifiers";
			this.lblModifiers.Size = new System.Drawing.Size(59, 13);
			this.lblModifiers.TabIndex = 0;
			this.lblModifiers.Text = "Modifiers:";
			// 
			// chkCtrl
			// 
			this.chkCtrl.AutoSize = true;
			this.chkCtrl.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkCtrl.Location = new System.Drawing.Point(11, 29);
			this.chkCtrl.Name = "chkCtrl";
			this.chkCtrl.Size = new System.Drawing.Size(44, 17);
			this.chkCtrl.TabIndex = 1;
			this.chkCtrl.Text = "Ctrl";
			this.chkCtrl.UseVisualStyleBackColor = true;
			// 
			// chkAlt
			// 
			this.chkAlt.AutoSize = true;
			this.chkAlt.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkAlt.Location = new System.Drawing.Point(66, 29);
			this.chkAlt.Name = "chkAlt";
			this.chkAlt.Size = new System.Drawing.Size(40, 17);
			this.chkAlt.TabIndex = 2;
			this.chkAlt.Text = "Alt";
			this.chkAlt.UseVisualStyleBackColor = true;
			// 
			// chkShift
			// 
			this.chkShift.AutoSize = true;
			this.chkShift.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkShift.Location = new System.Drawing.Point(117, 29);
			this.chkShift.Name = "chkShift";
			this.chkShift.Size = new System.Drawing.Size(50, 17);
			this.chkShift.TabIndex = 3;
			this.chkShift.Text = "Shift";
			this.chkShift.UseVisualStyleBackColor = true;
			// 
			// lblKey
			// 
			this.lblKey.AutoSize = true;
			this.lblKey.BackColor = System.Drawing.Color.Transparent;
			this.lblKey.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblKey.Location = new System.Drawing.Point(8, 63);
			this.lblKey.Name = "lblKey";
			this.lblKey.Size = new System.Drawing.Size(27, 13);
			this.lblKey.TabIndex = 4;
			this.lblKey.Text = "Key:";
			// 
			// cboKeys
			// 
			this.cboKeys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboKeys.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboKeys.FormattingEnabled = true;
			this.cboKeys.Location = new System.Drawing.Point(46, 60);
			this.cboKeys.Name = "cboKeys";
			this.cboKeys.Size = new System.Drawing.Size(118, 21);
			this.cboKeys.TabIndex = 5;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.BackColor = System.Drawing.SystemColors.Control;
			this.btnOK.CanBeChecked = false;
			this.btnOK.Checked = false;
			this.btnOK.DrawEmpty = false;
			this.btnOK.DrawLeftArrowButton = false;
			this.btnOK.DrawRightArrowButton = false;
			this.btnOK.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOK.Image = null;
			this.btnOK.Location = new System.Drawing.Point(122, 93);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(42, 18);
			this.btnOK.TabIndex = 8;
			this.btnOK.Text = "OK";
			this.btnOK.DrawBackground += new SilTools.Controls.XButton.DrawBackgroundHandler(this.HandleButtonDrawBackground);
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnReset
			// 
			this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnReset.BackColor = System.Drawing.SystemColors.Control;
			this.btnReset.CanBeChecked = false;
			this.btnReset.Checked = false;
			this.btnReset.DrawEmpty = false;
			this.btnReset.DrawLeftArrowButton = false;
			this.btnReset.DrawRightArrowButton = false;
			this.btnReset.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnReset.Image = null;
			this.btnReset.Location = new System.Drawing.Point(8, 93);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(42, 18);
			this.btnReset.TabIndex = 9;
			this.btnReset.Text = "Reset";
			this.btnReset.DrawBackground += new SilTools.Controls.XButton.DrawBackgroundHandler(this.HandleButtonDrawBackground);
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// ShortcutKeysEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.PaleGoldenrod;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.cboKeys);
			this.Controls.Add(this.lblKey);
			this.Controls.Add(this.chkShift);
			this.Controls.Add(this.chkAlt);
			this.Controls.Add(this.lblModifiers);
			this.Controls.Add(this.chkCtrl);
			this.MinimumSize = new System.Drawing.Size(175, 122);
			this.Name = "ShortcutKeysEditor";
			this.Size = new System.Drawing.Size(175, 120);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem jjToolStripMenuItem;
		private System.Windows.Forms.Label lblModifiers;
		private System.Windows.Forms.CheckBox chkCtrl;
		private System.Windows.Forms.CheckBox chkAlt;
		private System.Windows.Forms.CheckBox chkShift;
		private System.Windows.Forms.Label lblKey;
		private System.Windows.Forms.ComboBox cboKeys;
		private XButton btnOK;
		private XButton btnReset;
	}
}
