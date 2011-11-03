namespace SIL.Pa.PhoneticSearching
{
	partial class BracketedTextErrorMsgBox
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BracketedTextErrorMsgBox));
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._labelMessage = new System.Windows.Forms.Label();
			this._pictureIcon = new System.Windows.Forms.PictureBox();
			this._buttonOK = new System.Windows.Forms.Button();
			this._linkHelp = new System.Windows.Forms.LinkLabel();
			this._tableLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._labelMessage, 1, 0);
			this._tableLayout.Controls.Add(this._pictureIcon, 0, 0);
			this._tableLayout.Controls.Add(this._buttonOK, 1, 2);
			this._tableLayout.Controls.Add(this._linkHelp, 1, 1);
			this._tableLayout.Location = new System.Drawing.Point(15, 15);
			this._tableLayout.MaximumSize = new System.Drawing.Size(394, 0);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 3;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(394, 124);
			this._tableLayout.TabIndex = 0;
			// 
			// _labelMessage
			// 
			this._labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelMessage.AutoSize = true;
			this._labelMessage.Location = new System.Drawing.Point(108, 0);
			this._labelMessage.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._labelMessage.Name = "_labelMessage";
			this._labelMessage.Size = new System.Drawing.Size(286, 65);
			this._labelMessage.TabIndex = 1;
			this._labelMessage.Text = resources.GetString("_labelMessage.Text");
			// 
			// _pictureIcon
			// 
			this._pictureIcon.Location = new System.Drawing.Point(0, 0);
			this._pictureIcon.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this._pictureIcon.Name = "_pictureIcon";
			this._pictureIcon.Size = new System.Drawing.Size(100, 50);
			this._pictureIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureIcon.TabIndex = 1;
			this._pictureIcon.TabStop = false;
			// 
			// _buttonOK
			// 
			this._buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonOK.AutoSize = true;
			this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._buttonOK.Location = new System.Drawing.Point(319, 98);
			this._buttonOK.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._buttonOK.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.Size = new System.Drawing.Size(75, 26);
			this._buttonOK.TabIndex = 2;
			this._buttonOK.Text = "OK";
			this._buttonOK.UseVisualStyleBackColor = true;
			// 
			// _linkHelp
			// 
			this._linkHelp.AutoSize = true;
			this._linkHelp.Location = new System.Drawing.Point(108, 75);
			this._linkHelp.Margin = new System.Windows.Forms.Padding(3, 10, 0, 0);
			this._linkHelp.Name = "_linkHelp";
			this._linkHelp.Size = new System.Drawing.Size(259, 13);
			this._linkHelp.TabIndex = 4;
			this._linkHelp.TabStop = true;
			this._linkHelp.Text = "See the help topics \'{0}\' and \'{1}\' for more information.";
			this._linkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleHelpLinkClicked);
			// 
			// BracketedTextErrorMsgBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(424, 142);
			this.Controls.Add(this._tableLayout);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BracketedTextErrorMsgBox";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Invalid Bracketed Text";
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.PictureBox _pictureIcon;
		private System.Windows.Forms.Button _buttonOK;
		private System.Windows.Forms.LinkLabel _linkHelp;
		private System.Windows.Forms.Label _labelMessage;
	}
}