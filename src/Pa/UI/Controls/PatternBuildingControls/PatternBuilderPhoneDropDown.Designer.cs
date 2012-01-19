namespace SIL.Pa.UI.Controls
{
	partial class PatternBuilderPhoneDropDown
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
			this.m_grpConsonants = new System.Windows.Forms.GroupBox();
			this.m_grpVowels = new System.Windows.Forms.GroupBox();
			this.m_grpOther = new System.Windows.Forms.GroupBox();
			this.m_tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.m_otherPicker = new SIL.Pa.UI.Controls.CharPicker();
			this.m_conPicker = new SIL.Pa.UI.Controls.CharPicker();
			this.m_vowPicker = new SIL.Pa.UI.Controls.CharPicker();
			this.m_grpConsonants.SuspendLayout();
			this.m_grpVowels.SuspendLayout();
			this.m_grpOther.SuspendLayout();
			this.m_tableLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_grpConsonants
			// 
			this.m_grpConsonants.AutoSize = true;
			this.m_grpConsonants.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_grpConsonants.Controls.Add(this.m_conPicker);
			this.m_grpConsonants.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_grpConsonants.Location = new System.Drawing.Point(8, 5);
			this.m_grpConsonants.Margin = new System.Windows.Forms.Padding(8, 5, 4, 5);
			this.m_grpConsonants.Name = "m_grpConsonants";
			this.m_grpConsonants.Padding = new System.Windows.Forms.Padding(5, 3, 5, 0);
			this.m_grpConsonants.Size = new System.Drawing.Size(215, 65);
			this.m_grpConsonants.TabIndex = 0;
			this.m_grpConsonants.TabStop = false;
			this.m_grpConsonants.Text = "Consonants";
			// 
			// m_grpVowels
			// 
			this.m_grpVowels.AutoSize = true;
			this.m_grpVowels.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_grpVowels.Controls.Add(this.m_vowPicker);
			this.m_grpVowels.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_grpVowels.Location = new System.Drawing.Point(231, 5);
			this.m_grpVowels.Margin = new System.Windows.Forms.Padding(4, 5, 8, 5);
			this.m_grpVowels.Name = "m_grpVowels";
			this.m_grpVowels.Padding = new System.Windows.Forms.Padding(5, 3, 5, 0);
			this.m_grpVowels.Size = new System.Drawing.Size(110, 65);
			this.m_grpVowels.TabIndex = 1;
			this.m_grpVowels.TabStop = false;
			this.m_grpVowels.Text = "Vowels";
			// 
			// m_grpOther
			// 
			this.m_grpOther.AutoSize = true;
			this.m_grpOther.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_tableLayout.SetColumnSpan(this.m_grpOther, 2);
			this.m_grpOther.Controls.Add(this.m_otherPicker);
			this.m_grpOther.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_grpOther.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_grpOther.Location = new System.Drawing.Point(8, 78);
			this.m_grpOther.Margin = new System.Windows.Forms.Padding(8, 3, 8, 8);
			this.m_grpOther.Name = "m_grpOther";
			this.m_grpOther.Size = new System.Drawing.Size(333, 70);
			this.m_grpOther.TabIndex = 2;
			this.m_grpOther.TabStop = false;
			this.m_grpOther.Text = "Diacritics, Suprasegmentals, Tones, Accents, && Other Symbols";
			// 
			// m_tableLayout
			// 
			this.m_tableLayout.AutoSize = true;
			this.m_tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_tableLayout.BackColor = System.Drawing.Color.Transparent;
			this.m_tableLayout.ColumnCount = 2;
			this.m_tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.m_tableLayout.Controls.Add(this.m_grpOther, 0, 1);
			this.m_tableLayout.Controls.Add(this.m_grpConsonants, 0, 0);
			this.m_tableLayout.Controls.Add(this.m_grpVowels, 1, 0);
			this.m_tableLayout.Location = new System.Drawing.Point(0, 0);
			this.m_tableLayout.Name = "m_tableLayout";
			this.m_tableLayout.RowCount = 2;
			this.m_tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.m_tableLayout.Size = new System.Drawing.Size(349, 156);
			this.m_tableLayout.TabIndex = 2;
			this.m_tableLayout.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleTableLayoutPaint);
			// 
			// m_otherPicker
			// 
			this.m_otherPicker.AutoSize = false;
			this.m_otherPicker.AutoSizeItems = false;
			this.m_otherPicker.BackColor = System.Drawing.Color.Transparent;
			this.m_otherPicker.CheckItemsOnClick = false;
			this.m_otherPicker.Dock = System.Windows.Forms.DockStyle.None;
			this.m_otherPicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.m_otherPicker.ItemSize = new System.Drawing.Size(30, 32);
			this.m_otherPicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.m_otherPicker.Location = new System.Drawing.Point(3, 18);
			this.m_otherPicker.Name = "m_otherPicker";
			this.m_otherPicker.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.m_otherPicker.Size = new System.Drawing.Size(327, 34);
			this.m_otherPicker.TabIndex = 0;
			this.m_otherPicker.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.HandleItemClicked);
			// 
			// m_conPicker
			// 
			this.m_conPicker.AutoSize = false;
			this.m_conPicker.AutoSizeItems = false;
			this.m_conPicker.BackColor = System.Drawing.Color.Transparent;
			this.m_conPicker.CheckItemsOnClick = false;
			this.m_conPicker.Dock = System.Windows.Forms.DockStyle.None;
			this.m_conPicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.m_conPicker.ItemSize = new System.Drawing.Size(30, 32);
			this.m_conPicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.m_conPicker.Location = new System.Drawing.Point(5, 16);
			this.m_conPicker.Name = "m_conPicker";
			this.m_conPicker.Padding = new System.Windows.Forms.Padding(0);
			this.m_conPicker.ShowItemToolTips = false;
			this.m_conPicker.Size = new System.Drawing.Size(205, 34);
			this.m_conPicker.TabIndex = 0;
			this.m_conPicker.Text = "charPicker1";
			this.m_conPicker.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.HandleItemClicked);
			// 
			// m_vowPicker
			// 
			this.m_vowPicker.AutoSize = false;
			this.m_vowPicker.AutoSizeItems = false;
			this.m_vowPicker.BackColor = System.Drawing.Color.Transparent;
			this.m_vowPicker.CheckItemsOnClick = false;
			this.m_vowPicker.Dock = System.Windows.Forms.DockStyle.None;
			this.m_vowPicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.m_vowPicker.ItemSize = new System.Drawing.Size(30, 32);
			this.m_vowPicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.m_vowPicker.Location = new System.Drawing.Point(5, 16);
			this.m_vowPicker.Name = "m_vowPicker";
			this.m_vowPicker.Padding = new System.Windows.Forms.Padding(0);
			this.m_vowPicker.ShowItemToolTips = false;
			this.m_vowPicker.Size = new System.Drawing.Size(100, 34);
			this.m_vowPicker.TabIndex = 0;
			this.m_vowPicker.Text = "charPicker1";
			this.m_vowPicker.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.HandleItemClicked);
			// 
			// PatternBuilderPhoneDropDown
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.m_tableLayout);
			this.DoubleBuffered = true;
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "PatternBuilderPhoneDropDown";
			this.Size = new System.Drawing.Size(352, 159);
			this.m_grpConsonants.ResumeLayout(false);
			this.m_grpVowels.ResumeLayout(false);
			this.m_grpOther.ResumeLayout(false);
			this.m_tableLayout.ResumeLayout(false);
			this.m_tableLayout.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox m_grpConsonants;
		private System.Windows.Forms.GroupBox m_grpVowels;
		private System.Windows.Forms.GroupBox m_grpOther;
		private CharPicker m_conPicker;
		private System.Windows.Forms.TableLayoutPanel m_tableLayout;
		private CharPicker m_vowPicker;
		private CharPicker m_otherPicker;

	}
}
