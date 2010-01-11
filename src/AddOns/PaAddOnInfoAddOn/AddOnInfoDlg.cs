using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using SilUtils;

namespace SIL.Pa.AddOn
{
	public partial class AddOnInfoDlg : Form
	{
		private string m_settingName;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AddOnInfoDlg()
		{
			InitializeComponent();
			lblDisclaimer.Text = Properties.Resources.kstidDisclaimer;
			LoadAddOnGrid();
			m_settingName = Assembly.GetExecutingAssembly().CodeBase;
			m_settingName = Path.GetFileNameWithoutExtension(m_settingName);
			grid.Name = m_settingName + "Grid";

			try
			{
				PaApp.SettingsHandler.LoadFormProperties(this);
				PaApp.SettingsHandler.LoadGridProperties(grid);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadAddOnGrid()
		{
			int rowHeight = TextRenderer.MeasureText("X", grid.DefaultCellStyle.Font).Height * 3;

			foreach (Assembly assembly in PaApp.AddOnAssemblys)
			{
				if (assembly.CodeBase.ToLower().EndsWith("pafeedbackaddon.dll"))
					continue;

				string title = string.Empty;
				string desc = string.Empty;
				string filename = Path.GetFileName(assembly.CodeBase);
				string version = assembly.GetName().Version.ToString(3);

				// Get the Add-on's title.
				object[] obj = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (obj.Length > 0 && obj[0] is AssemblyTitleAttribute)
					title = ((AssemblyTitleAttribute)obj[0]).Title;

				// Get the Add-on's description.
				obj = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (obj.Length > 0 && obj[0] is AssemblyDescriptionAttribute)
					desc = ((AssemblyDescriptionAttribute)obj[0]).Description;

				// Get the enabled value for the add-on from the settings file.
				string settingName = Path.GetFileNameWithoutExtension(filename);
				bool enabled = PaApp.SettingsHandler.GetBoolSettingsValue(
					settingName, "Enabled", true);

				grid.Rows.Add(new object[] { title, desc, filename, version, enabled });
				grid.Rows[grid.RowCount - 1].Cells[4].ReadOnly = !CanBeDisabled(assembly);
				grid.Rows[grid.RowCount - 1].Height = rowHeight + 6;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not an add-on assembly can be disabled.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool CanBeDisabled(Assembly assembly)
		{
			if (assembly != Assembly.GetExecutingAssembly())
			{
				// Get the Add-on's description.
				object[] obj = assembly.GetCustomAttributes(typeof(AssemblyDefaultAliasAttribute), false);
				if (obj.Length > 0 && obj[0] is AssemblyDefaultAliasAttribute)
					return ((AssemblyDefaultAliasAttribute)obj[0]).DefaultAlias == "CanBeDisabled";
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save changes to the enabled states of add-ons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			try
			{
				PaApp.SettingsHandler.SaveFormProperties(this);
				PaApp.SettingsHandler.SaveGridProperties(grid);
			}
			catch { }


			if ((e.CloseReason != CloseReason.UserClosing && e.CloseReason != CloseReason.None) ||
				DialogResult != DialogResult.OK)
			{
				return;
			}

			try
			{
				// Save all the enabled values of the add-ons.
				foreach (DataGridViewRow row in grid.Rows)
				{
					if (row.Cells[4].ReadOnly)
						continue;

					string name = Path.GetFileNameWithoutExtension(row.Cells[2].Value as string);
					PaApp.SettingsHandler.SaveSettingsValue(name, "Enabled",
						(bool)row.Cells[4].Value);
				}
			}
			catch {}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw a nice gradient background for the selected row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex < 0)
				return;

			e.Handled = true;
			DataGridViewPaintParts paintParts = DataGridViewPaintParts.All;
			paintParts &= ~DataGridViewPaintParts.Focus;

			if (grid.Rows[e.RowIndex].Selected)
			{
				// Draw default everything but focus rectangle and background.
				paintParts &= ~DataGridViewPaintParts.Background;

				Color clr1 = ColorHelper.CalculateColor(Color.White,
					SystemColors.GradientActiveCaption, 250);
				
				Color clr2 = ColorHelper.CalculateColor(Color.White,
					SystemColors.GradientActiveCaption, 150);

				using (LinearGradientBrush br =
					new LinearGradientBrush(e.CellBounds, clr1, clr2, 90f))
				{
					e.Graphics.FillRectangle(br, e.CellBounds);
				}
			}

			e.Paint(e.ClipBounds, paintParts);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cause the space key to toggle the enabled state regardless of what cell is current.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void grid_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (grid.CurrentCellAddress.X != 4 && e.KeyChar == (char)Keys.Space &&
				grid.CurrentRow != null && grid.CurrentRow.Index >= 0 &&
				!grid.CurrentRow.Cells[4].ReadOnly)
			{
				grid.CurrentRow.Cells[4].Value = !(bool)(grid.CurrentRow.Cells[4].Value);
			}
		}
	}
}
