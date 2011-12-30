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
			this._checkBoxSearchEngineSelction = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// _checkBoxSearchEngineSelction
			// 
			this._checkBoxSearchEngineSelction.AutoSize = true;
			this._checkBoxSearchEngineSelction.Location = new System.Drawing.Point(18, 17);
			this._checkBoxSearchEngineSelction.Name = "_checkBoxSearchEngineSelction";
			this._checkBoxSearchEngineSelction.Size = new System.Drawing.Size(206, 17);
			this._checkBoxSearchEngineSelction.TabIndex = 5;
			this._checkBoxSearchEngineSelction.Text = "Show search engine selection buttons";
			this._checkBoxSearchEngineSelction.UseVisualStyleBackColor = true;
			// 
			// SearchingOptionsPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this._checkBoxSearchEngineSelction);
			this.Name = "SearchingOptionsPage";
			this.Size = new System.Drawing.Size(438, 238);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox _checkBoxSearchEngineSelction;

	}
}
