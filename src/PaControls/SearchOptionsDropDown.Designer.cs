namespace SIL.Pa.Controls
{
	partial class SearchOptionsDropDown
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchOptionsDropDown));
			this.chkIgnoreDiacritics = new System.Windows.Forms.CheckBox();
			this.chkShowAllWords = new System.Windows.Forms.CheckBox();
			this.chkStress = new System.Windows.Forms.CheckBox();
			this.grpStress = new System.Windows.Forms.GroupBox();
			this.stressPicker = new SIL.Pa.Controls.CharPicker();
			this.chkTone = new System.Windows.Forms.CheckBox();
			this.grpTone = new System.Windows.Forms.GroupBox();
			this.tonePicker = new SIL.Pa.Controls.CharPicker();
			this.chkLength = new System.Windows.Forms.CheckBox();
			this.grpLength = new System.Windows.Forms.GroupBox();
			this.lengthPicker = new SIL.Pa.Controls.CharPicker();
			this.chkIncludeUncertain = new System.Windows.Forms.CheckBox();
			this.lnkApplyToAll = new System.Windows.Forms.LinkLabel();
			this.lnkHelp = new System.Windows.Forms.LinkLabel();
			this.grpStress.SuspendLayout();
			this.grpTone.SuspendLayout();
			this.grpLength.SuspendLayout();
			this.SuspendLayout();
			// 
			// chkIgnoreDiacritics
			// 
			resources.ApplyResources(this.chkIgnoreDiacritics, "chkIgnoreDiacritics");
			this.chkIgnoreDiacritics.Name = "chkIgnoreDiacritics";
			this.chkIgnoreDiacritics.UseVisualStyleBackColor = true;
			// 
			// chkShowAllWords
			// 
			resources.ApplyResources(this.chkShowAllWords, "chkShowAllWords");
			this.chkShowAllWords.Name = "chkShowAllWords";
			this.chkShowAllWords.UseVisualStyleBackColor = true;
			// 
			// chkStress
			// 
			resources.ApplyResources(this.chkStress, "chkStress");
			this.chkStress.BackColor = System.Drawing.Color.Transparent;
			this.chkStress.Name = "chkStress";
			this.chkStress.ThreeState = true;
			this.chkStress.UseVisualStyleBackColor = false;
			this.chkStress.Click += new System.EventHandler(this.HandleIgnoreClick);
			// 
			// grpStress
			// 
			this.grpStress.Controls.Add(this.stressPicker);
			resources.ApplyResources(this.grpStress, "grpStress");
			this.grpStress.Name = "grpStress";
			this.grpStress.TabStop = false;
			// 
			// stressPicker
			// 
			resources.ApplyResources(this.stressPicker, "stressPicker");
			this.stressPicker.AutoSizeItems = false;
			this.stressPicker.BackColor = System.Drawing.Color.Transparent;
			this.stressPicker.CheckItemsOnClick = true;
			this.stressPicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.stressPicker.ItemSize = new System.Drawing.Size(30, 32);
			this.stressPicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.stressPicker.Name = "stressPicker";
			this.stressPicker.CharPicked += new SIL.Pa.Controls.CharPicker.CharPickedHandler(this.HandleCharChecked);
			// 
			// chkTone
			// 
			resources.ApplyResources(this.chkTone, "chkTone");
			this.chkTone.BackColor = System.Drawing.Color.Transparent;
			this.chkTone.Name = "chkTone";
			this.chkTone.ThreeState = true;
			this.chkTone.UseVisualStyleBackColor = false;
			this.chkTone.Click += new System.EventHandler(this.HandleIgnoreClick);
			// 
			// grpTone
			// 
			this.grpTone.Controls.Add(this.tonePicker);
			resources.ApplyResources(this.grpTone, "grpTone");
			this.grpTone.Name = "grpTone";
			this.grpTone.TabStop = false;
			// 
			// tonePicker
			// 
			resources.ApplyResources(this.tonePicker, "tonePicker");
			this.tonePicker.AutoSizeItems = false;
			this.tonePicker.BackColor = System.Drawing.Color.Transparent;
			this.tonePicker.CheckItemsOnClick = true;
			this.tonePicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tonePicker.ItemSize = new System.Drawing.Size(30, 32);
			this.tonePicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.tonePicker.Name = "tonePicker";
			this.tonePicker.CharPicked += new SIL.Pa.Controls.CharPicker.CharPickedHandler(this.HandleCharChecked);
			// 
			// chkLength
			// 
			resources.ApplyResources(this.chkLength, "chkLength");
			this.chkLength.BackColor = System.Drawing.Color.Transparent;
			this.chkLength.Name = "chkLength";
			this.chkLength.ThreeState = true;
			this.chkLength.UseVisualStyleBackColor = false;
			this.chkLength.Click += new System.EventHandler(this.HandleIgnoreClick);
			// 
			// grpLength
			// 
			this.grpLength.Controls.Add(this.lengthPicker);
			resources.ApplyResources(this.grpLength, "grpLength");
			this.grpLength.Name = "grpLength";
			this.grpLength.TabStop = false;
			// 
			// lengthPicker
			// 
			resources.ApplyResources(this.lengthPicker, "lengthPicker");
			this.lengthPicker.AutoSizeItems = false;
			this.lengthPicker.BackColor = System.Drawing.Color.Transparent;
			this.lengthPicker.CheckItemsOnClick = true;
			this.lengthPicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.lengthPicker.ItemSize = new System.Drawing.Size(30, 32);
			this.lengthPicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.lengthPicker.Name = "lengthPicker";
			this.lengthPicker.CharPicked += new SIL.Pa.Controls.CharPicker.CharPickedHandler(this.HandleCharChecked);
			// 
			// chkIncludeUncertain
			// 
			resources.ApplyResources(this.chkIncludeUncertain, "chkIncludeUncertain");
			this.chkIncludeUncertain.Name = "chkIncludeUncertain";
			this.chkIncludeUncertain.UseVisualStyleBackColor = true;
			// 
			// lnkApplyToAll
			// 
			resources.ApplyResources(this.lnkApplyToAll, "lnkApplyToAll");
			this.lnkApplyToAll.Name = "lnkApplyToAll";
			this.lnkApplyToAll.TabStop = true;
			// 
			// lnkHelp
			// 
			resources.ApplyResources(this.lnkHelp, "lnkHelp");
			this.lnkHelp.Name = "lnkHelp";
			this.lnkHelp.TabStop = true;
			this.lnkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleHelpClicked);
			// 
			// SearchOptionsDropDown
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.lnkHelp);
			this.Controls.Add(this.lnkApplyToAll);
			this.Controls.Add(this.chkIncludeUncertain);
			this.Controls.Add(this.chkLength);
			this.Controls.Add(this.grpLength);
			this.Controls.Add(this.chkTone);
			this.Controls.Add(this.grpTone);
			this.Controls.Add(this.chkShowAllWords);
			this.Controls.Add(this.chkStress);
			this.Controls.Add(this.chkIgnoreDiacritics);
			this.Controls.Add(this.grpStress);
			this.DoubleBuffered = true;
			this.MaximumSize = new System.Drawing.Size(268, 500);
			this.MinimumSize = new System.Drawing.Size(268, 458);
			this.Name = "SearchOptionsDropDown";
			this.grpStress.ResumeLayout(false);
			this.grpTone.ResumeLayout(false);
			this.grpLength.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private CharPicker tonePicker;
		private CharPicker stressPicker;
		private CharPicker lengthPicker;
		protected System.Windows.Forms.CheckBox chkShowAllWords;
		protected System.Windows.Forms.CheckBox chkIncludeUncertain;
		protected System.Windows.Forms.GroupBox grpStress;
		protected System.Windows.Forms.GroupBox grpTone;
		protected System.Windows.Forms.GroupBox grpLength;
		protected System.Windows.Forms.CheckBox chkIgnoreDiacritics;
		protected System.Windows.Forms.CheckBox chkStress;
		protected System.Windows.Forms.CheckBox chkTone;
		protected System.Windows.Forms.CheckBox chkLength;
		private System.Windows.Forms.LinkLabel lnkApplyToAll;
		public System.Windows.Forms.LinkLabel lnkHelp;
	}
}
