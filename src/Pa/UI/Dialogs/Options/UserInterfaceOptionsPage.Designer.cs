namespace SIL.Pa.UI.Dialogs
{
	partial class UserInterfaceOptionsPage
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
			this.cboUILanguage = new System.Windows.Forms.ComboBox();
			this.lblUILanguage = new System.Windows.Forms.Label();
			this._tableLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// cboUILanguage
			// 
			this.cboUILanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboUILanguage.DropDownHeight = 200;
			this.cboUILanguage.FormattingEnabled = true;
			this.cboUILanguage.IntegralHeight = false;
			this.cboUILanguage.Location = new System.Drawing.Point(0, 18);
			this.cboUILanguage.Margin = new System.Windows.Forms.Padding(0);
			this.cboUILanguage.Name = "cboUILanguage";
			this.cboUILanguage.Size = new System.Drawing.Size(200, 21);
			this.cboUILanguage.TabIndex = 3;
			// 
			// lblUILanguage
			// 
			this.lblUILanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblUILanguage.AutoSize = true;
			this.lblUILanguage.Location = new System.Drawing.Point(0, 0);
			this.lblUILanguage.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this.lblUILanguage.Name = "lblUILanguage";
			this.lblUILanguage.Size = new System.Drawing.Size(200, 13);
			this.lblUILanguage.TabIndex = 2;
			this.lblUILanguage.Text = "User Interface Language:";
			// 
			// _tableLayoutOuter
			// 
			this._tableLayoutOuter.BackColor = System.Drawing.Color.Transparent;
			this._tableLayoutOuter.ColumnCount = 1;
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.Controls.Add(this.lblUILanguage, 0, 0);
			this._tableLayoutOuter.Controls.Add(this.cboUILanguage, 0, 1);
			this._tableLayoutOuter.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutOuter.Name = "_tableLayoutOuter";
			this._tableLayoutOuter.RowCount = 2;
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.Size = new System.Drawing.Size(200, 68);
			this._tableLayoutOuter.TabIndex = 4;
			// 
			// UserInterfaceOptionsPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this._tableLayoutOuter);
			this.Name = "UserInterfaceOptionsPage";
			this.Size = new System.Drawing.Size(274, 164);
			this._tableLayoutOuter.ResumeLayout(false);
			this._tableLayoutOuter.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox cboUILanguage;
		private System.Windows.Forms.Label lblUILanguage;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutOuter;
	}
}
