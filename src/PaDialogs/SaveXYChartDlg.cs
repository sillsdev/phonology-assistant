using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Controls;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class SaveXYChartDlg : OKCancelDlgBase
	{
		private readonly XYGrid m_xyGrid;
		private readonly List<XYChartLayout> m_savedCharts;
		private XYChartLayout m_layoutToOverwrite = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SaveXYChartDlg()
		{
			InitializeComponent();
			lblName.Font = FontHelper.UIFont;
			txtName.Font = FontHelper.PhoneticFont;

			// Set the height of the dialog box.
			int dy = Height - ClientSize.Height;
			Height = tlpName.Bottom + 8 + pnlButtons.Height + dy;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SaveXYChartDlg(XYGrid xyGrid, List<XYChartLayout>savedCharts) : this()
		{
			m_xyGrid = xyGrid;
			m_savedCharts = savedCharts;
			txtName.Text = (string.IsNullOrEmpty(xyGrid.ChartName) ? xyGrid.DefaultName : xyGrid.ChartName);
			txtName.SelectAll();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the text box has focus when the dialog is first shown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			txtName.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			string text = txtName.Text.Trim();

			if (string.IsNullOrEmpty(text))
			{
				string msg = Properties.Resources.kstidNoSavedChartNameMsg;
				STUtils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtName.SelectAll();
				txtName.Focus();
				return false;
			}

			XYChartLayout existingLayout = GetExistingLayoutByName(m_xyGrid.ChartLayout, text);
			if (existingLayout != null)
			{
				string msg = Properties.Resources.kstidSavedChartNameAlreadyExistsOverwriteMsg;
				msg = string.Format(msg, text);
				if (STUtils.STMsgBox(msg, MessageBoxButtons.YesNo) == DialogResult.Yes)
					m_layoutToOverwrite = existingLayout;
				else
				{
					txtName.SelectAll();
					txtName.Focus();
					return false;
				}
			}

			m_xyGrid.ChartLayout.Name = text;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches the collection of saved chart layouts and returns the one whose name
		/// is that of the one specified. If the layout cannot be found, null is returned.
		/// </summary>
		/// <param name="chartToSkip">Chart Layout to skip as the collection of saved
		/// layouts is searched for the one having the specified name.</param>
		/// <param name="nameToCheck">The name of the saved layout to search for.</param>
		/// ------------------------------------------------------------------------------------
		private XYChartLayout GetExistingLayoutByName(XYChartLayout chartToSkip, string nameToCheck)
		{
			if (m_savedCharts != null)
			{
				// Check if chart name already exists. If it does,
				// tell the user and don't cancel the current edit.
				foreach (XYChartLayout savedChart in m_savedCharts)
				{
					if (savedChart != chartToSkip && savedChart.Name == nameToCheck)
						return savedChart;
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the user wants to overwrite a chart, then this will be the one he wants to
		/// overwrite.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public XYChartLayout LayoutToOverwrite
		{
			get { return m_layoutToOverwrite; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw an etched line between the buttons and what's above them.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Point pt1 = new Point(pnlButtons.Left, pnlButtons.Top - 2);
			Point pt2 = new Point(pnlButtons.Right - 1, pnlButtons.Top - 2);

			using (Pen pen = new Pen(SystemColors.ControlDark))
			{
				e.Graphics.DrawLine(pen, pt1, pt2);
				pt1.Y++;
				pt2.Y++;
				pen.Color = SystemColors.ControlLight;
				e.Graphics.DrawLine(pen, pt1, pt2);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleTextChanged(object sender, EventArgs e)
		{
			m_dirty = true;
		}
	}
}