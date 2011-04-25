using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa
{
	public static class AppColor
	{
		/// ------------------------------------------------------------------------------------
		public static Color ViewBackground
		{
			get { return SystemColors.Control; }
			//Color.FromArgb(0xff, 0xff, 0xfa, 0xc0);
		}

		/// ------------------------------------------------------------------------------------
		public static Color ViewTabGroupBackground
		{
			get { return SystemColors.Control; }
			// Color.FromArgb(0xff, 0x9c, 0xaa, 0xc1);	
		}

		/// ------------------------------------------------------------------------------------
		public static Color ViewTabBackgroundActive
		{
			get { return Color.White; }
		}

		/// ------------------------------------------------------------------------------------
		public static Color ViewTabBackgroundActiveBorder
		{
			get { return SystemColors.ControlDark; }
		}

		/// ------------------------------------------------------------------------------------
		public static Color ViewTabBackgroundInactive
		{
			get { return SystemColors.Control; }
			//Color.FromArgb(0xff, 0x9c, 0xaa, 0xc1)
		}

		/// ------------------------------------------------------------------------------------
		public static Color ViewTabForegroundActive
		{
			get { return Color.Black; }
		}

		/// ------------------------------------------------------------------------------------
		public static Color ViewTabForegroundInactive
		{
			get
			{
				return ColorHelper.CalculateColor(SystemColors.ControlText,
					SystemColors.Control, 145);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color ViewTabMouseHoverLine
		{
			get
			{
				return (PaintingHelper.CanPaintVisualStyle() ?
					VisualStyleInformation.ControlHighlightHot : SystemColors.Highlight);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color PrimaryHeaderTop
		{
			get { return ColorHelper.CalculateColor(Color.White, SystemColors.ActiveCaption, 70); }
			//Color.FromArgb(0xff, 0x4c, 0x5f, 0x81);
		}

		/// ------------------------------------------------------------------------------------
		public static Color PrimaryHeaderBottom
		{
			get { return ColorHelper.CalculateColor(SystemColors.ActiveCaption, SystemColors.ActiveCaption, 0); }
			//Color.FromArgb(0xff, 0x2c, 0x3d, 0x5a);
		}

		/// ------------------------------------------------------------------------------------
		public static Color PrimaryHeaderForeground
		{
			get { return SystemColors.ActiveCaptionText; }
			//Color.White;
		}

		/// ------------------------------------------------------------------------------------
		public static Color SecondaryHeaderTop
		{
			get { return ColorHelper.CalculateColor(Color.White, SystemColors.GradientActiveCaption, 190); }
		}

		/// ------------------------------------------------------------------------------------
		public static Color SecondaryHeaderBottom
		{
			get { return ColorHelper.CalculateColor(SystemColors.ActiveCaption, SystemColors.GradientActiveCaption, 50); }
		}

		/// ------------------------------------------------------------------------------------
		public static Color SecondaryHeaderForeground
		{
			get { return SystemColors.ActiveCaptionText; }
		}

		#region Grid color methods and properties
		/// ------------------------------------------------------------------------------------
		public static void SetCellColors(DataGridView grid, DataGridViewCellFormattingEventArgs e)
		{
			if (grid.CurrentRow == null || grid.CurrentRow.Index != e.RowIndex)
				return;

			if (!grid.Focused)
			{
				e.CellStyle.SelectionBackColor = GridRowUnfocusedSelectionBackColor;
				e.CellStyle.SelectionForeColor = GridRowUnfocusedSelectionForeColor;
				return;
			}

			if (grid.CurrentCell != null && grid.CurrentCell.ColumnIndex == e.ColumnIndex)
			{
				// Set the selected cell's background color to be
				// distinct from the rest of the current row.
				e.CellStyle.SelectionBackColor = GridCellFocusedBackColor;
				e.CellStyle.SelectionForeColor = GridCellFocusedForeColor;
			}
			else
			{
				// Set the selected row's background color.
				e.CellStyle.SelectionBackColor = GridRowFocusedBackColor;
				e.CellStyle.SelectionForeColor = GridRowFocusedForeColor;
			}
		}

		/// ------------------------------------------------------------------------------------
		public static void SetGridSelectionColors(SilGrid grid, bool makeSelectedCellsDifferent)
		{
			grid.SelectedRowBackColor = GridRowFocusedBackColor;
			grid.SelectedRowForeColor = GridRowFocusedForeColor;

			grid.SelectedCellBackColor = (makeSelectedCellsDifferent ?
				GridCellFocusedBackColor : GridRowFocusedBackColor);

			grid.SelectedCellForeColor = (makeSelectedCellsDifferent ?
				GridCellFocusedForeColor : GridRowFocusedForeColor);
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridColor
		{
			get
			{
				var clr = Settings.Default.WordListGridColor;
				return (clr != Color.Transparent && clr != Color.Empty ? clr :
					ColorHelper.CalculateColor(SystemColors.WindowText, SystemColors.Window, 25));
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridRowFocusedForeColor
		{
			get
			{
				return (Settings.Default.UseSystemColors ? SystemColors.WindowText :
					Settings.Default.GridRowSelectionForeColor);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridRowFocusedBackColor
		{
			get
			{
				return (Settings.Default.UseSystemColors ? ColorHelper.LightHighlight :
					Settings.Default.GridRowSelectionBackColor);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridCellFocusedForeColor
		{
			get
			{
				return (Settings.Default.UseSystemColors ? SystemColors.WindowText :
					Settings.Default.GridCellSelectionForeColor);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridCellFocusedBackColor
		{
			get
			{
				return (Settings.Default.UseSystemColors ? ColorHelper.LightLightHighlight :
					Settings.Default.GridCellSelectionBackColor);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridRowUnfocusedSelectionBackColor
		{
			get
			{
				return (Settings.Default.UseSystemColors ? SystemColors.Control :
					Settings.Default.GridRowUnfocusedSelectionBackColor);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridRowUnfocusedSelectionForeColor
		{
			get
			{
				if (!Settings.Default.UseSystemColors)
					return Settings.Default.GridRowUnfocusedSelectionForeColor;

				// It turns out the control color for the silver Windows XP theme is very close
				// to the default color calculated for selected rows in PA word lists. Therefore,
				// when a word list grid looses focus and a selected row's background color gets
				// changed to the control color, it's very hard to tell the difference between a
				// selected row in a focused grid from that of a non focused grid. So, when the
				// theme is the silver (i.e. Metallic) then also make the text gray for selected
				// rows in non focused grid's.
				if (PaintingHelper.CanPaintVisualStyle() &&
					System.Windows.Forms.VisualStyles.VisualStyleInformation.DisplayName == "Windows XP style" &&
					System.Windows.Forms.VisualStyles.VisualStyleInformation.ColorScheme == "Metallic")
				{
					return SystemColors.GrayText;
				}

				return SystemColors.ControlText;
			}
		}

		#endregion
	}
}
