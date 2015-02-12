using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Localization;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class SaveDistributionChartDlg : OKCancelDlgBase
	{
		private readonly DistributionGrid m_xyGrid;
		private readonly List<DistributionChart> m_savedCharts;
		private DistributionChart m_layoutToOverwrite;

		/// ------------------------------------------------------------------------------------
		public SaveDistributionChartDlg()
		{
			InitializeComponent();
			lblName.Font = FontHelper.UIFont;
			txtName.Font = App.PhoneticFont;

			// Set the height of the dialog box.
			int dy = Height - ClientSize.Height;
			Height = tlpName.Bottom + 8 + tblLayoutButtons.Height + dy;
		}

		/// ------------------------------------------------------------------------------------
		public SaveDistributionChartDlg(DistributionGrid xyGrid, List<DistributionChart>savedCharts) : this()
		{
			m_xyGrid = xyGrid;
			m_savedCharts = savedCharts;
			txtName.Text = (string.IsNullOrEmpty(xyGrid.ChartName) ? xyGrid.DefaultName : xyGrid.ChartName);
			txtName.SelectAll();

			// This will force the user to put something in the name other than
			// spaces or an empty space, even when it was determined to make the
			// default name empty or all spaces. See PA-556.
			if (txtName.Text.Trim() == string.Empty)
				m_dirty = true;
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
		protected override bool Verify()
		{
			string text = txtName.Text.Trim();

			if (string.IsNullOrEmpty(text))
			{
				Utils.MsgBox(LocalizationManager.GetString(
					"DialogBoxes.SaveDistributionChartDlg.NoSavedChartNameMsg",
					"You must specify a name for your distribution chart."));
				
				txtName.SelectAll();
				txtName.Focus();
				return false;
			}

			var existingLayout = GetExistingLayoutByName(m_xyGrid.ChartLayout, text);
			if (existingLayout != null)
			{
				var msg = LocalizationManager.GetString(
					"DialogBoxes.SaveDistributionChartDlg.OverwriteSavedChartNameMsg",
					"There is already a saved chart with the name '{0}'.\nDo you want it overwritten?");
				
				msg = string.Format(msg, text);
				
				if (Utils.MsgBox(msg, MessageBoxButtons.YesNo) == DialogResult.Yes)
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
		private DistributionChart GetExistingLayoutByName(DistributionChart chartToSkip, string nameToCheck)
		{
			return (m_savedCharts != null ?
				m_savedCharts.FirstOrDefault(savedChart => savedChart != chartToSkip && savedChart.Name == nameToCheck) :
				null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the user wants to overwrite a chart, then this will be the one he wants to
		/// overwrite.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DistributionChart LayoutToOverwrite
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

			var pt1 = new Point(tblLayoutButtons.Left, tblLayoutButtons.Top - 2);
			var pt2 = new Point(tblLayoutButtons.Right - 1, tblLayoutButtons.Top - 2);

			using (var pen = new Pen(SystemColors.ControlDark))
			{
				e.Graphics.DrawLine(pen, pt1, pt2);
				pt1.Y++;
				pt2.Y++;
				pen.Color = SystemColors.ControlLight;
				e.Graphics.DrawLine(pen, pt1, pt2);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTextChanged(object sender, EventArgs e)
		{
			m_dirty = true;
		}

        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.Escape:
                    {
                        this.Close();
                        return true;
                    }
                case Keys.Control | Keys.Tab:
                    {
                        return true;
                    }
                case Keys.Control | Keys.Shift | Keys.Tab:
                    {
                        return true;
                    }
            }
            return base.ProcessCmdKey(ref message, keys);
        }
	}
}