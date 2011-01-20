using System.Windows.Forms;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class DistributionChartCellInfoPopup
	{
		private TableLayoutPanel m_tblLayout;
		private Label m_lblPattern;
		private Label m_lblInfo;
		private Label m_lblMsg;

		private void InitializeComponent()
		{
			this.m_tblLayout = new System.Windows.Forms.TableLayoutPanel();
			this.m_lblInfo = new System.Windows.Forms.Label();
			this.m_lblPattern = new System.Windows.Forms.Label();
			this.m_lblMsg = new System.Windows.Forms.Label();
			this.m_tblLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_tblLayout
			// 
			this.m_tblLayout.AutoSize = true;
			this.m_tblLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_tblLayout.BackColor = System.Drawing.Color.Transparent;
			this.m_tblLayout.ColumnCount = 1;
			this.m_tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.m_tblLayout.Controls.Add(this.m_lblInfo, 0, 2);
			this.m_tblLayout.Controls.Add(this.m_lblPattern, 0, 0);
			this.m_tblLayout.Controls.Add(this.m_lblMsg, 0, 1);
			this.m_tblLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_tblLayout.Location = new System.Drawing.Point(8, 0);
			this.m_tblLayout.Name = "m_tblLayout";
			this.m_tblLayout.RowCount = 3;
			this.m_tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_tblLayout.Size = new System.Drawing.Size(221, 77);
			this.m_tblLayout.TabIndex = 0;
			// 
			// m_lblInfo
			// 
			this.m_lblInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.m_lblInfo.AutoSize = true;
			this.m_lblInfo.Location = new System.Drawing.Point(103, 56);
			this.m_lblInfo.Margin = new System.Windows.Forms.Padding(3, 5, 3, 8);
			this.m_lblInfo.Name = "m_lblInfo";
			this.m_lblInfo.Size = new System.Drawing.Size(14, 13);
			this.m_lblInfo.TabIndex = 1;
			this.m_lblInfo.Text = "#";
			// 
			// m_lblPattern
			// 
			this.m_lblPattern.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.m_lblPattern.AutoSize = true;
			this.m_lblPattern.Location = new System.Drawing.Point(103, 7);
			this.m_lblPattern.Margin = new System.Windows.Forms.Padding(3, 7, 3, 8);
			this.m_lblPattern.Name = "m_lblPattern";
			this.m_lblPattern.Size = new System.Drawing.Size(14, 13);
			this.m_lblPattern.TabIndex = 0;
			this.m_lblPattern.Text = "#";
			// 
			// m_lblMsg
			// 
			this.m_lblMsg.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.m_lblMsg.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.m_lblMsg.AutoSize = true;
			this.m_lblMsg.Location = new System.Drawing.Point(103, 33);
			this.m_lblMsg.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.m_lblMsg.Name = "m_lblMsg";
			this.m_lblMsg.Size = new System.Drawing.Size(14, 13);
			this.m_lblMsg.TabIndex = 1;
			this.m_lblMsg.Text = "#";
			// 
			// XYChartCellInfoPopup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.m_tblLayout);
			this.MinimumSize = new System.Drawing.Size(100, 0);
			this.Name = "XYChartCellInfoPopup";
			this.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.Size = new System.Drawing.Size(237, 158);
			this.m_tblLayout.ResumeLayout(false);
			this.m_tblLayout.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
	}
}
