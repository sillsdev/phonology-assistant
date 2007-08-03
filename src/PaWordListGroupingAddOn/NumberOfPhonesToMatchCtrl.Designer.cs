namespace SIL.Pa.AddOn
{
	partial class NumberOfPhonesToMatchCtrl
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
			this.lblMessage = new System.Windows.Forms.Label();
			this.updnPhones = new System.Windows.Forms.NumericUpDown();
			this.tblCtrls = new System.Windows.Forms.TableLayoutPanel();
			this.lnkApply = new System.Windows.Forms.LinkLabel();
			((System.ComponentModel.ISupportInitialize)(this.updnPhones)).BeginInit();
			this.tblCtrls.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblMessage
			// 
			this.lblMessage.AutoSize = true;
			this.lblMessage.Location = new System.Drawing.Point(3, 0);
			this.lblMessage.MaximumSize = new System.Drawing.Size(130, 0);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(123, 52);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Tag = "";
			this.lblMessage.Text = "Number of phones to match when grouping by environment {0}. Zero, matches all pho" +
				"nes.";
			// 
			// updnPhones
			// 
			this.updnPhones.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.updnPhones.Location = new System.Drawing.Point(8, 60);
			this.updnPhones.Margin = new System.Windows.Forms.Padding(8);
			this.updnPhones.Name = "updnPhones";
			this.updnPhones.Size = new System.Drawing.Size(114, 20);
			this.updnPhones.TabIndex = 1;
			this.updnPhones.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tblCtrls
			// 
			this.tblCtrls.ColumnCount = 1;
			this.tblCtrls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblCtrls.Controls.Add(this.lblMessage, 0, 0);
			this.tblCtrls.Controls.Add(this.updnPhones, 0, 1);
			this.tblCtrls.Controls.Add(this.lnkApply, 0, 2);
			this.tblCtrls.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tblCtrls.Location = new System.Drawing.Point(5, 5);
			this.tblCtrls.Name = "tblCtrls";
			this.tblCtrls.RowCount = 3;
			this.tblCtrls.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblCtrls.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblCtrls.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblCtrls.Size = new System.Drawing.Size(130, 112);
			this.tblCtrls.TabIndex = 2;
			// 
			// lnkApply
			// 
			this.lnkApply.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lnkApply.AutoSize = true;
			this.lnkApply.Location = new System.Drawing.Point(48, 93);
			this.lnkApply.Margin = new System.Windows.Forms.Padding(5);
			this.lnkApply.Name = "lnkApply";
			this.lnkApply.Size = new System.Drawing.Size(33, 13);
			this.lnkApply.TabIndex = 2;
			this.lnkApply.TabStop = true;
			this.lnkApply.Text = "Apply";
			// 
			// NumberOfPhonesToMatchCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.tblCtrls);
			this.DoubleBuffered = true;
			this.Name = "NumberOfPhonesToMatchCtrl";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Size = new System.Drawing.Size(140, 122);
			((System.ComponentModel.ISupportInitialize)(this.updnPhones)).EndInit();
			this.tblCtrls.ResumeLayout(false);
			this.tblCtrls.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NumericUpDown updnPhones;
		private System.Windows.Forms.TableLayoutPanel tblCtrls;
		internal System.Windows.Forms.LinkLabel lnkApply;
		private System.Windows.Forms.Label lblMessage;
	}
}
