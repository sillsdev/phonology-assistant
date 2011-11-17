namespace SIL.Pa.UI.Dialogs
{
	partial class SearchingOptionsPage
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
			this._tableLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this._radioUseRegExpSearching = new System.Windows.Forms.RadioButton();
			this._radioUseOldSearching = new System.Windows.Forms.RadioButton();
			this._tableLayoutOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tableLayoutOuter
			// 
			this._tableLayoutOuter.BackColor = System.Drawing.Color.Transparent;
			this._tableLayoutOuter.ColumnCount = 1;
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.Controls.Add(this._radioUseRegExpSearching, 0, 0);
			this._tableLayoutOuter.Controls.Add(this._radioUseOldSearching, 0, 1);
			this._tableLayoutOuter.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutOuter.Name = "_tableLayoutOuter";
			this._tableLayoutOuter.RowCount = 2;
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.Size = new System.Drawing.Size(307, 107);
			this._tableLayoutOuter.TabIndex = 4;
			// 
			// _radioUseRegExpSearching
			// 
			this._radioUseRegExpSearching.AutoSize = true;
			this._radioUseRegExpSearching.Location = new System.Drawing.Point(3, 3);
			this._radioUseRegExpSearching.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
			this._radioUseRegExpSearching.Name = "_radioUseRegExpSearching";
			this._radioUseRegExpSearching.Size = new System.Drawing.Size(216, 17);
			this._radioUseRegExpSearching.TabIndex = 5;
			this._radioUseRegExpSearching.TabStop = true;
			this._radioUseRegExpSearching.Text = "Use regular expression searching engine";
			this._radioUseRegExpSearching.UseVisualStyleBackColor = true;
			// 
			// _radioUseOldSearching
			// 
			this._radioUseOldSearching.AutoSize = true;
			this._radioUseOldSearching.Location = new System.Drawing.Point(3, 28);
			this._radioUseOldSearching.Name = "_radioUseOldSearching";
			this._radioUseOldSearching.Size = new System.Drawing.Size(262, 17);
			this._radioUseOldSearching.TabIndex = 6;
			this._radioUseOldSearching.TabStop = true;
			this._radioUseOldSearching.Text = "Use searching engine from version 3.3.2 and older";
			this._radioUseOldSearching.UseVisualStyleBackColor = true;
			// 
			// SearchingOptionsPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this._tableLayoutOuter);
			this.Name = "SearchingOptionsPage";
			this.Size = new System.Drawing.Size(310, 164);
			this._tableLayoutOuter.ResumeLayout(false);
			this._tableLayoutOuter.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutOuter;
		private System.Windows.Forms.RadioButton _radioUseRegExpSearching;
		private System.Windows.Forms.RadioButton _radioUseOldSearching;
	}
}
