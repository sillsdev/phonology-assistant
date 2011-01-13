using System;
using System.Drawing;
using System.Windows.Forms;

namespace SilTools.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ShortcutKeysEditor : SilPopup
	{
		private readonly Keys[] m_validShortcutKeys = new[] { Keys.None, Keys.A, Keys.B, Keys.C, Keys.D,
			Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N,
			Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X,
			Keys.Y, Keys.Z, Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7,
			Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12, Keys.D0, Keys.D1, Keys.D2, Keys.D3,
			Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.Insert, Keys.Delete,
			Keys.Home, Keys.End, Keys.PageUp, Keys.PageDown, Keys.Up, Keys.Down, Keys.Left,
			Keys.Right };

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ShortcutKeysEditor"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ShortcutKeysEditor()
		{
			InitializeComponent();

			foreach (Keys key in m_validShortcutKeys)
				cboKeys.Items.Add(GetStringFromNonModifierKeys(key));

			cboKeys.SelectedIndex = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the the selected keys as a string or sets the selected keys from a string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SelectedKeysAsString
		{
			get { return KeysToString(SelectedKeys); }
			set
			{
				if (string.IsNullOrEmpty(value) || value.Trim() == string.Empty)
					Reset();
				else
				{
					string keystr = value;
					if (keystr.Contains("Ctrl") || keystr.Contains("CTRL"))
						chkCtrl.Checked = true;
					if (keystr.Contains("Alt") || keystr.Contains("ALT"))
						chkAlt.Checked = true;
					if (keystr.Contains("Shift") || keystr.Contains("SHIFT"))
						chkShift.Checked = true;

					keystr = keystr.Replace("Ctrl", string.Empty);
					keystr = keystr.Replace("Alt", string.Empty);
					keystr = keystr.Replace("Shift", string.Empty);
					keystr = keystr.Replace("CTRL", string.Empty);
					keystr = keystr.Replace("ALT", string.Empty);
					keystr = keystr.Replace("SHIFT", string.Empty);
					keystr = keystr.Replace("+", string.Empty);
					keystr = keystr.Replace(" ", string.Empty);

					if (keystr.Length == 2 && keystr[0] == 'D')
						keystr = keystr.Substring(1);

					cboKeys.SelectedItem = keystr;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the the selected keys.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Keys SelectedKeys
		{
			get
			{
				Keys keys = Keys.None;

				if (chkCtrl.Checked)
					keys |= Keys.Control;

				if (chkAlt.Checked)
					keys |= Keys.Alt;

				if (chkShift.Checked)
					keys |= Keys.Shift;

				string keystr = cboKeys.SelectedItem as string;

				if (keystr == Keys.None.ToString() && keys != Keys.None)
				{
					Reset();
					return Keys.None;
				}

				return (keys | GetNonModifierKeyFromString(keystr));
			}
			set
			{
				chkCtrl.Checked = ((value & Keys.Control) == Keys.Control);
				chkAlt.Checked = ((value & Keys.Alt) == Keys.Alt);
				chkShift.Checked = ((value & Keys.Shift) == Keys.Shift);
				Keys keys = value;
				keys &= ~Keys.Control;
				keys &= ~Keys.Alt;
				keys &= ~Keys.Shift;

				string keystr = GetStringFromNonModifierKeys(keys);
				if (cboKeys.Items.Contains(keystr))
					cboKeys.SelectedItem = keystr;
				else
					cboKeys.SelectedIndex = 0;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the Click event of the btnOK control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnOK_Click(object sender, EventArgs e)
		{
			Hide();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the Click event of the btnReset control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnReset_Click(object sender, EventArgs e)
		{
			Reset();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Resets the contents of the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Reset()
		{
			cboKeys.SelectedIndex = 0;
			chkCtrl.Checked = false;
			chkAlt.Checked = false;
			chkShift.Checked = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Convert the specified Keys value to a string representation.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Keys KeysFromString(string keystr)
		{
			if (string.IsNullOrEmpty(keystr) || keystr.Trim() == string.Empty ||
				keystr == Keys.None.ToString())
			{
				return Keys.None;
			}

			Keys keys = Keys.None;

			if (keystr.Contains("Ctrl") || keystr.Contains("CTRL"))
				keys |= Keys.Control;
			if (keystr.Contains("Alt") || keystr.Contains("ALT"))
				keys |= Keys.Alt;
            if (keystr.Contains("Shift") || keystr.Contains("SHIFT"))
				keys |= Keys.Shift;

			return keys | GetNonModifierKeyFromString(keystr);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Convert the specified Keys value to a string representation.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string KeysToString(Keys keys)
		{
			string keystr = string.Empty;

			if ((keys & Keys.Control) == Keys.Control)
			{
				keystr += "Ctrl+";
				keys &= ~Keys.Control;
			}

			if ((keys & Keys.Alt) == Keys.Alt)
			{
				keystr += "Alt+";
				keys &= ~Keys.Alt;
			}

			if ((keys & Keys.Shift) == Keys.Shift)
			{
				keystr += "Shift+";
				keys &= ~Keys.Shift;
			}

			return keystr + GetStringFromNonModifierKeys(keys);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For the values in the Keys enumeration that represent the number keys (i.e. those
		/// across the top of the keyboard, Keyses the string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetStringFromNonModifierKeys(Keys key)
		{
			// First make sure the keys has no modifiers.
			key &= ~Keys.Control;
			key &= ~Keys.Alt;
			key &= ~Keys.Shift;

			string keystr = key.ToString();
			if (keystr.Length == 2 && keystr[0] == 'D' && keystr[1] >= '0' && keystr[1] <= '9')
				return keystr[1].ToString();

			return keystr;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static Keys GetNonModifierKeyFromString(string keystr)
		{
			keystr = keystr.Replace("Ctrl", string.Empty);
			keystr = keystr.Replace("Alt", string.Empty);
			keystr = keystr.Replace("Shift", string.Empty);
			keystr = keystr.Replace("CTRL", string.Empty);
			keystr = keystr.Replace("ALT", string.Empty);
			keystr = keystr.Replace("SHIFT", string.Empty);
			keystr = keystr.Replace("+", string.Empty);
			keystr = keystr.Replace(",", string.Empty);
			keystr = keystr.Replace(" ", string.Empty);
			keystr = keystr.Trim();

			if (keystr.Length == 1 && keystr[0] >= '0' && keystr[0] <= '9')
				keystr = "D" + keystr;

			try
			{
				return (Keys)Enum.Parse(typeof(Keys), keystr);
			}
			catch
			{
				return Keys.None;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// I found that when the combo box is dropped down and part of the drop-down window
		/// is below (i.e. outside) the bounds of this control, clicking the mouse anywhere
		/// in the combo's drop-down that is below the bounds of this control will make the
		/// this popup control get dismissed. Argh! Therefore, we'll cancel the closing when
		/// the mouse location is over the combo's drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDropDownClosing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (cboKeys.DroppedDown)
			{
				var rc = new Rectangle(0, 0, cboKeys.DropDownWidth, cboKeys.DropDownHeight);
				var pt = new Point(cboKeys.Left, cboKeys.Bottom);
				pt = PointToScreen(pt);
				rc.X = pt.X;
				rc.Y = pt.Y;
				if (rc.Contains(MousePosition))
					e.Cancel = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints the background of the OK and Reset buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool HandleButtonDrawBackground(XButton btn, PaintEventArgs e, PaintState state)
		{
			if (state == PaintState.Hot || state == PaintState.HotDown)
				return false;

			var rc = btn.ClientRectangle;
			Color clr = ColorHelper.CalculateColor(Color.White, BackColor, 100);

			using (SolidBrush br = new SolidBrush(clr))
				e.Graphics.FillRectangle(br, rc);

			rc.Width--;
			rc.Height--;
			clr = ColorHelper.CalculateColor(Color.Black, BackColor, 70);
			using (Pen pen = new Pen(clr))
				e.Graphics.DrawRectangle(pen, rc);

			//btn.DrawText(e);
			return true;
		}
	}
}
