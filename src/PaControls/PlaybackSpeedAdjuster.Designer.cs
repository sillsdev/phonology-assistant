namespace SIL.Pa.Controls
{
	partial class PlaybackSpeedAdjuster
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaybackSpeedAdjuster));
			this.trkSpeed = new System.Windows.Forms.TrackBar();
			this.lnkPlay = new System.Windows.Forms.LinkLabel();
			this.lblValue = new System.Windows.Forms.Label();
			this.lbl100 = new System.Windows.Forms.Label();
			this.lbl50 = new System.Windows.Forms.Label();
			this.lbl75 = new System.Windows.Forms.Label();
			this.lbl25 = new System.Windows.Forms.Label();
			this.lbl125 = new System.Windows.Forms.Label();
			this.lbl150 = new System.Windows.Forms.Label();
			this.lbl175 = new System.Windows.Forms.Label();
			this.lbl200 = new System.Windows.Forms.Label();
			this.lblZero = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.trkSpeed)).BeginInit();
			this.SuspendLayout();
			// 
			// trkSpeed
			// 
			resources.ApplyResources(this.trkSpeed, "trkSpeed");
			this.trkSpeed.BackColor = System.Drawing.Color.White;
			this.trkSpeed.Maximum = 200;
			this.trkSpeed.Name = "trkSpeed";
			this.trkSpeed.TickFrequency = 10;
			this.trkSpeed.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trkSpeed.Value = 100;
			this.trkSpeed.ValueChanged += new System.EventHandler(this.trkSpeed_ValueChanged);
			// 
			// lnkPlay
			// 
			this.lnkPlay.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lnkPlay, "lnkPlay");
			this.lnkPlay.Name = "lnkPlay";
			this.lnkPlay.TabStop = true;
			// 
			// lblValue
			// 
			this.lblValue.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblValue, "lblValue");
			this.lblValue.Name = "lblValue";
			// 
			// lbl100
			// 
			this.lbl100.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lbl100, "lbl100");
			this.lbl100.Name = "lbl100";
			this.lbl100.UseCompatibleTextRendering = true;
			// 
			// lbl50
			// 
			this.lbl50.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lbl50, "lbl50");
			this.lbl50.Name = "lbl50";
			this.lbl50.UseCompatibleTextRendering = true;
			// 
			// lbl75
			// 
			this.lbl75.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lbl75, "lbl75");
			this.lbl75.Name = "lbl75";
			this.lbl75.UseCompatibleTextRendering = true;
			// 
			// lbl25
			// 
			this.lbl25.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lbl25, "lbl25");
			this.lbl25.Name = "lbl25";
			this.lbl25.UseCompatibleTextRendering = true;
			// 
			// lbl125
			// 
			this.lbl125.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lbl125, "lbl125");
			this.lbl125.Name = "lbl125";
			this.lbl125.UseCompatibleTextRendering = true;
			// 
			// lbl150
			// 
			this.lbl150.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lbl150, "lbl150");
			this.lbl150.Name = "lbl150";
			this.lbl150.UseCompatibleTextRendering = true;
			// 
			// lbl175
			// 
			this.lbl175.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lbl175, "lbl175");
			this.lbl175.Name = "lbl175";
			this.lbl175.UseCompatibleTextRendering = true;
			// 
			// lbl200
			// 
			this.lbl200.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lbl200, "lbl200");
			this.lbl200.Name = "lbl200";
			this.lbl200.UseCompatibleTextRendering = true;
			// 
			// lblZero
			// 
			this.lblZero.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblZero, "lblZero");
			this.lblZero.Name = "lblZero";
			this.lblZero.UseCompatibleTextRendering = true;
			// 
			// PlaybackSpeedAdjuster
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.lblZero);
			this.Controls.Add(this.lbl200);
			this.Controls.Add(this.lbl175);
			this.Controls.Add(this.lbl150);
			this.Controls.Add(this.lbl125);
			this.Controls.Add(this.lbl25);
			this.Controls.Add(this.lbl75);
			this.Controls.Add(this.lbl50);
			this.Controls.Add(this.lbl100);
			this.Controls.Add(this.trkSpeed);
			this.Controls.Add(this.lnkPlay);
			this.Controls.Add(this.lblValue);
			this.DoubleBuffered = true;
			this.Name = "PlaybackSpeedAdjuster";
			resources.ApplyResources(this, "$this");
			((System.ComponentModel.ISupportInitialize)(this.trkSpeed)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TrackBar trkSpeed;
		private System.Windows.Forms.Label lblValue;
		private System.Windows.Forms.Label lbl100;
		private System.Windows.Forms.Label lbl50;
		private System.Windows.Forms.Label lbl75;
		private System.Windows.Forms.Label lbl25;
		private System.Windows.Forms.Label lbl125;
		private System.Windows.Forms.Label lbl150;
		private System.Windows.Forms.Label lbl175;
		private System.Windows.Forms.Label lbl200;
		private System.Windows.Forms.Label lblZero;
		public System.Windows.Forms.LinkLabel lnkPlay;
	}
}
