namespace SIL.Pa.AddOn
{
	partial class RatingSurveyCtrl
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
			this.lblItem = new System.Windows.Forms.Label();
			this.pnlRatings = new System.Windows.Forms.Panel();
			this.picInfo = new System.Windows.Forms.PictureBox();
			this.rb5 = new System.Windows.Forms.RadioButton();
			this.rb4 = new System.Windows.Forms.RadioButton();
			this.rb3 = new System.Windows.Forms.RadioButton();
			this.rb2 = new System.Windows.Forms.RadioButton();
			this.rb1 = new System.Windows.Forms.RadioButton();
			this.pnlRatings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picInfo)).BeginInit();
			this.SuspendLayout();
			// 
			// lblItem
			// 
			this.lblItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblItem.AutoEllipsis = true;
			this.lblItem.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblItem.Location = new System.Drawing.Point(0, 0);
			this.lblItem.Name = "lblItem";
			this.lblItem.Size = new System.Drawing.Size(208, 20);
			this.lblItem.TabIndex = 0;
			this.lblItem.Text = "#";
			this.lblItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pnlRatings
			// 
			this.pnlRatings.Controls.Add(this.picInfo);
			this.pnlRatings.Controls.Add(this.rb5);
			this.pnlRatings.Controls.Add(this.rb4);
			this.pnlRatings.Controls.Add(this.rb3);
			this.pnlRatings.Controls.Add(this.rb2);
			this.pnlRatings.Controls.Add(this.rb1);
			this.pnlRatings.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlRatings.Location = new System.Drawing.Point(214, 0);
			this.pnlRatings.Name = "pnlRatings";
			this.pnlRatings.Size = new System.Drawing.Size(120, 20);
			this.pnlRatings.TabIndex = 6;
			// 
			// picInfo
			// 
			this.picInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.picInfo.Location = new System.Drawing.Point(102, 2);
			this.picInfo.Name = "picInfo";
			this.picInfo.Size = new System.Drawing.Size(16, 16);
			this.picInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picInfo.TabIndex = 11;
			this.picInfo.TabStop = false;
			this.picInfo.Visible = false;
			this.picInfo.MouseLeave += new System.EventHandler(this.picInfo_MouseLeave);
			this.picInfo.MouseEnter += new System.EventHandler(this.picInfo_MouseEnter);
			// 
			// rb5
			// 
			this.rb5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.rb5.AutoSize = true;
			this.rb5.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rb5.Location = new System.Drawing.Point(84, 4);
			this.rb5.Name = "rb5";
			this.rb5.Size = new System.Drawing.Size(14, 13);
			this.rb5.TabIndex = 10;
			this.rb5.TabStop = true;
			this.rb5.UseVisualStyleBackColor = true;
			// 
			// rb4
			// 
			this.rb4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.rb4.AutoSize = true;
			this.rb4.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rb4.Location = new System.Drawing.Point(64, 4);
			this.rb4.Name = "rb4";
			this.rb4.Size = new System.Drawing.Size(14, 13);
			this.rb4.TabIndex = 9;
			this.rb4.TabStop = true;
			this.rb4.UseVisualStyleBackColor = true;
			// 
			// rb3
			// 
			this.rb3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.rb3.AutoSize = true;
			this.rb3.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rb3.Location = new System.Drawing.Point(44, 4);
			this.rb3.Name = "rb3";
			this.rb3.Size = new System.Drawing.Size(14, 13);
			this.rb3.TabIndex = 8;
			this.rb3.TabStop = true;
			this.rb3.UseVisualStyleBackColor = true;
			// 
			// rb2
			// 
			this.rb2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.rb2.AutoSize = true;
			this.rb2.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rb2.Location = new System.Drawing.Point(24, 4);
			this.rb2.Name = "rb2";
			this.rb2.Size = new System.Drawing.Size(14, 13);
			this.rb2.TabIndex = 7;
			this.rb2.TabStop = true;
			this.rb2.UseVisualStyleBackColor = true;
			// 
			// rb1
			// 
			this.rb1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.rb1.AutoSize = true;
			this.rb1.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rb1.Location = new System.Drawing.Point(4, 4);
			this.rb1.Name = "rb1";
			this.rb1.Size = new System.Drawing.Size(14, 13);
			this.rb1.TabIndex = 6;
			this.rb1.TabStop = true;
			this.rb1.UseVisualStyleBackColor = true;
			// 
			// RatingSurveyCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.pnlRatings);
			this.Controls.Add(this.lblItem);
			this.Name = "RatingSurveyCtrl";
			this.Size = new System.Drawing.Size(334, 20);
			this.pnlRatings.ResumeLayout(false);
			this.pnlRatings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picInfo)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblItem;
		private System.Windows.Forms.Panel pnlRatings;
		private System.Windows.Forms.RadioButton rb5;
		private System.Windows.Forms.RadioButton rb4;
		private System.Windows.Forms.RadioButton rb3;
		private System.Windows.Forms.RadioButton rb2;
		private System.Windows.Forms.RadioButton rb1;
		private System.Windows.Forms.PictureBox picInfo;
	}
}
