namespace SIL.PaToFdoInterfaces
{
    partial class ChooseProject
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
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.Ok = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.AnotherLocation = new System.Windows.Forms.LinkLabel();
			this.Help = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Font = new System.Drawing.Font("Charis SIL", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 31;
			this.listBox1.Location = new System.Drawing.Point(15, 30);
			this.listBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(449, 283);
			this.listBox1.TabIndex = 0;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(11, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(169, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "Select FieldWorks Project";
			// 
			// Ok
			// 
			this.Ok.Location = new System.Drawing.Point(152, 380);
			this.Ok.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Ok.Name = "Ok";
			this.Ok.Size = new System.Drawing.Size(100, 32);
			this.Ok.TabIndex = 2;
			this.Ok.Text = "OK";
			this.Ok.UseVisualStyleBackColor = true;
			this.Ok.Click += new System.EventHandler(this.Ok_Click);
			// 
			// Cancel
			// 
			this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Cancel.Location = new System.Drawing.Point(258, 380);
			this.Cancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(100, 32);
			this.Cancel.TabIndex = 3;
			this.Cancel.Text = "Cancel";
			this.Cancel.UseVisualStyleBackColor = true;
			// 
			// AnotherLocation
			// 
			this.AnotherLocation.AutoSize = true;
			this.AnotherLocation.Location = new System.Drawing.Point(12, 388);
			this.AnotherLocation.Name = "AnotherLocation";
			this.AnotherLocation.Size = new System.Drawing.Size(116, 17);
			this.AnotherLocation.TabIndex = 4;
			this.AnotherLocation.TabStop = true;
			this.AnotherLocation.Text = "Another Location";
			this.AnotherLocation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AnotherLocation_LinkClicked);
			// 
			// Help
			// 
			this.Help.Location = new System.Drawing.Point(364, 380);
			this.Help.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Help.Name = "Help";
			this.Help.Size = new System.Drawing.Size(100, 32);
			this.Help.TabIndex = 6;
			this.Help.Text = "Help";
			this.Help.UseVisualStyleBackColor = true;
			this.Help.Click += new System.EventHandler(this.Help_Click);
			// 
			// ChooseProject
			// 
			this.AcceptButton = this.Ok;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.Cancel;
			this.ClientSize = new System.Drawing.Size(481, 432);
			this.Controls.Add(this.Help);
			this.Controls.Add(this.AnotherLocation);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.Ok);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChooseProject";
			this.Text = "Choose Project";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.ChooseProject_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.LinkLabel AnotherLocation;
		private System.Windows.Forms.Button Help;
	}
}

