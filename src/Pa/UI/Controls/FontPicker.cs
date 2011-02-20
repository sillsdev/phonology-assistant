using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using SilTools;
using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
	public partial class FontPicker : UserControl
	{
		public delegate void FontPickerClosedHandler(FontPicker sender, DialogResult result);
		public event FontPickerClosedHandler Closed;
		protected CustomDropDown m_dropDown;
		private bool m_updatingFontSettingsInProgress;

		/// ------------------------------------------------------------------------------------
		public DialogResult DialogResult { get; private set; }

		/// ------------------------------------------------------------------------------------
		public FontPicker()
		{
			if (m_dropDown == null)
			{
				m_dropDown = new CustomDropDown();
				m_dropDown.AutoCloseWhenMouseLeaves = false;
				m_dropDown.AddControl(this);
				m_dropDown.Closing += HandleDropDownClosing;
				m_dropDown.Closed += delegate { OnClose(DialogResult); };
			}

			InitializeComponent();

			cboFontFamily.Font = FontHelper.UIFont;
			cboSize.Font = FontHelper.UIFont;
			chkBold.Font = FontHelper.UIFont;
			chkItalic.Font = FontHelper.UIFont;
			btnOK.Font = FontHelper.UIFont;
			btnCancel.Font = FontHelper.UIFont;

			DoubleBuffered = true;

			pnlSample.TextFormatFlags |= TextFormatFlags.HorizontalCenter;
			cboFontFamily.Items.AddRange(new InstalledFontCollection().Families.Select(f => f.Name).ToArray());
			cboSize.Items.AddRange(new object[] {8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 22, 24, 26, 28, 30, 36, 48, 72});

			DialogResult = DialogResult.Cancel;
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public override Font Font
		{
			get { return base.Font; }
			set { SetFont(value, true); }
		}

		/// ------------------------------------------------------------------------------------
		public void SetFont(Font fnt, bool setBaseFont)
		{
			if (setBaseFont)
				base.Font = fnt;
	
			pnlSample.Font = fnt;

			m_updatingFontSettingsInProgress = true;
			cboFontFamily.SelectedItem = fnt.FontFamily.Name;
			cboSize.Text = ((int)fnt.SizeInPoints).ToString();
			chkBold.Checked = fnt.Bold;
			chkItalic.Checked = fnt.Italic;
			m_updatingFontSettingsInProgress = false;
		}

		/// ------------------------------------------------------------------------------------
		public void ShowForGridCell(Font fnt, DataGridViewCell cell)
		{
			ShowForGridCell(fnt, cell, false);
		}

		/// ------------------------------------------------------------------------------------
		public void ShowForGridCell(Font fnt, DataGridViewCell cell, bool rightAlign)
		{
			int col = cell.ColumnIndex;
			int row = cell.RowIndex;
			var rc = cell.DataGridView.GetCellDisplayRectangle(col, row, false);
			rc.Y += rc.Height;

			if (rightAlign)
				rc.X = rc.Right - pnlOuter.Width;

			Show(fnt, cell.DataGridView.PointToScreen(rc.Location));
		}

		/// ------------------------------------------------------------------------------------
		public void Show(Font fnt, Point location)
		{
			// When the font for the buttons that's specified in the designer is different
			// from the system font for buttons, for some reason, somewhere along the lines,
			// the buttons font gets set to the system font for buttons. Check here and
			// put it back to the 8pt. specified in the designer.
			if (btnOK.Font.SizeInPoints >= 9f)
			{
				btnOK.Font = new Font(btnOK.Font.FontFamily, 8f, GraphicsUnit.Point);
				btnCancel.Font = btnOK.Font;
				btnOK.Size = btnCancel.Size = btnOK.MinimumSize;
			}

			Size = pnlOuter.Size;
			DialogResult = DialogResult.Cancel;
			SetFont(fnt, true);
			m_dropDown.Show(location);
			cboFontFamily.Focus();
		}

		/// ------------------------------------------------------------------------------------
		private Font MakeFontFromSettings()
		{		
			var style = FontStyle.Regular;
			if (chkBold.Checked)
				style = FontStyle.Bold;
			if (chkItalic.Checked)
				style |= FontStyle.Italic;

			// This method is supposed to make the fonts from the current settings so it may
			// seem odd to call SetFont (which synchronizes the controls to the specified
			// font) after creating the font. The reason for this is that sometimes the
			// style specified is not supported by the font family, so the created font may
			// not always reflect the check states for bold and italic. Calling SetFont
			// right after creating the font will update those check boxes to reflect the
			// the style of the font that was really made.
			var fnt = FontHelper.MakeFont(cboFontFamily.Text, int.Parse(cboSize.Text), style);
			SetFont(fnt, false);
			return fnt;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFontSettingChanged(object sender, EventArgs e)
		{
			if (!m_updatingFontSettingsInProgress)
				pnlSample.Font = MakeFontFromSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Enter)
				HandleOKButtonClick(null, null);

			return base.ProcessDialogKey(keyData);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDropDownClosing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (e.CloseReason != ToolStripDropDownCloseReason.AppFocusChange &&
				e.CloseReason != ToolStripDropDownCloseReason.AppClicked)
			{
				return;
			}

			if (cboFontFamily.DroppedDown || cboSize.DroppedDown)
				e.Cancel = true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleSizeComboValidating(object sender, CancelEventArgs e)
		{
			int size;
			if (!int.TryParse(cboSize.Text, out size))
				cboSize.Text = "10";
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCancelButtonClick(object sender, EventArgs e)
		{
			if (m_dropDown == null)
				OnClose(DialogResult.Cancel);
			else
			{
				DialogResult = DialogResult.Cancel;
				m_dropDown.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleOKButtonClick(object sender, EventArgs e)
		{
			if (m_dropDown == null)
				OnClose(DialogResult.OK);
			else
			{
				DialogResult = DialogResult.OK;
				m_dropDown.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnClose(DialogResult result)
		{
			if (result == DialogResult.OK)
				Font = MakeFontFromSettings();

			DialogResult = result;
			if (Closed != null)
				Closed(this, result);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var clr1 = Color.White;
			var clr2 = ColorHelper.CalculateColor(Color.White, SystemColors.GradientActiveCaption, 100);

			using (var br = new LinearGradientBrush(ClientRectangle, clr1, clr2, 45f))
				e.Graphics.FillRectangle(br, ClientRectangle);
		}
	}
}
