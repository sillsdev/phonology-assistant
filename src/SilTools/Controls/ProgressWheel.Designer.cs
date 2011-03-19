namespace SilTools.Controls
{
	partial class ProgressWheel
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
			this.lblPercent = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.flowPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblPercent
			// 
			this.lblPercent.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPercent.BackColor = System.Drawing.Color.Transparent;
			this.lblPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPercent.Image = global::SilTools.Properties.Resources.ProgressWheel;
			this.lblPercent.Location = new System.Drawing.Point(3, 0);
			this.lblPercent.Name = "lblPercent";
			this.lblPercent.Size = new System.Drawing.Size(48, 48);
			this.lblPercent.TabIndex = 1;
			this.lblPercent.Text = "100%";
			this.lblPercent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblMessage
			// 
			this.lblMessage.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblMessage.AutoSize = true;
			this.lblMessage.Location = new System.Drawing.Point(57, 17);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(100, 13);
			this.lblMessage.TabIndex = 2;
			this.lblMessage.Text = "Message goes here";
			// 
			// flowPanel
			// 
			this.flowPanel.AutoSize = true;
			this.flowPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowPanel.BackColor = System.Drawing.Color.Transparent;
			this.flowPanel.Controls.Add(this.lblPercent);
			this.flowPanel.Controls.Add(this.lblMessage);
			this.flowPanel.Location = new System.Drawing.Point(0, 0);
			this.flowPanel.Margin = new System.Windows.Forms.Padding(0);
			this.flowPanel.Name = "flowPanel";
			this.flowPanel.Size = new System.Drawing.Size(160, 48);
			this.flowPanel.TabIndex = 3;
			// 
			// ProgressWheel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.flowPanel);
			this.Name = "ProgressWheel";
			this.Size = new System.Drawing.Size(160, 48);
			this.flowPanel.ResumeLayout(false);
			this.flowPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblPercent;
		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.FlowLayoutPanel flowPanel;

	}
}
