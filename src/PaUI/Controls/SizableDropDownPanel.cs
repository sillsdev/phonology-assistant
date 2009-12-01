using System;
using System.Drawing;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a panel that can be sized and hosts controls on drop-downs.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SizableDropDownPanel : UserControl
	{
		private bool m_leftMouseDown = false;
		private Point m_anchor;
		private string m_savedSettingsName;
		private Rectangle m_hotArea;
		private const int kHotDimension = 13;
		private bool m_resizeDoEvents = false;
		private readonly SettingsHandler m_settingsHndlr = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SizableDropDownPanel(string savedSettingName, Size defaultSize) :
			this(null, savedSettingName, defaultSize)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SizableDropDownPanel(SettingsHandler settingsHndlr, string savedSettingName,
			Size defaultSize)
		{
			m_settingsHndlr = settingsHndlr;

			Padding = new Padding(0, 0, 0, kHotDimension);
			base.DoubleBuffered = true;
			base.BackColor = Color.White;

			int width = SettingsHandler.GetIntSettingsValue(savedSettingName, "width", defaultSize.Width);
			int height = SettingsHandler.GetIntSettingsValue(savedSettingName, "height", defaultSize.Height);
			Size = new Size(width, height);
			m_savedSettingsName = savedSettingName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the settings handler the drop-down uses to get and retrieve the
		/// value of the drop-down's size.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private SettingsHandler SettingsHandler
		{
			get { return (m_settingsHndlr ?? PaApp.SettingsHandler); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the value of the name of the setting used for saving the drop-down's
		/// size in the settings file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SavedSettingsName
		{
			get { return m_savedSettingsName; }
			set { m_savedSettingsName = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			if (!Visible)
				return;

			// Make sure the bottom right corner (i.e. the resizing gripper thing) of
			// the drop-down is visible on the screen. It won't do for the drop-down
			// to be too large for the screen and the user unable to shrink it.
			Point pt = PointToScreen(new Point(m_hotArea.Right, m_hotArea.Bottom));
			Screen scn = Screen.FromControl(this);
			if (!scn.WorkingArea.Contains(pt))
			{
				// Shrink the drop-down so it's bottom, right corner is just inside the screen.
				int dx = (pt.X - scn.WorkingArea.Right);
				int dy = (pt.Y - scn.WorkingArea.Bottom);
				Width -= dx;
				Height -= dy;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			m_hotArea = new Rectangle(ClientRectangle.Right - kHotDimension - 1,
				ClientRectangle.Bottom - kHotDimension - 1, kHotDimension, kHotDimension);

			Invalidate(new Rectangle(0, ClientSize.Height - kHotDimension - 10,
				ClientSize.Width, kHotDimension + 10));

			m_resizeDoEvents = true;
			Application.DoEvents();
			m_resizeDoEvents = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Resizes the drop-down when the mouse is down and moving over the sizer control
		/// in the corner.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(MouseEventArgs e)
		{
			// This prevents the OnResize and OnMouseMove
			// events from getting into an infinite loop.
			if (m_resizeDoEvents)
				return;

			if (m_leftMouseDown)
			{
				Point pt = PointToScreen(e.Location);
				int dx = (pt.X - m_anchor.X);
				int dy = (pt.Y - m_anchor.Y);
				m_anchor = pt;

				int newWidth = Width + dx;
				int newHeight = Height + dy;

				if (newWidth < MinimumSize.Width)
					newWidth = MinimumSize.Width;

				if (newHeight < MinimumSize.Height)
					newHeight = MinimumSize.Height;

				Size = new Size(newWidth, newHeight);

				if (Cursor != Cursors.SizeNWSE)
					Cursor = Cursors.SizeNWSE;
			}
			else if (m_hotArea.Contains(e.Location))
			{
				if (Cursor != Cursors.SizeNWSE)
					Cursor = Cursors.SizeNWSE;
			}
			else if (Cursor != Cursors.Default)
				Cursor = Cursors.Default;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(MouseEventArgs e)
		{
			m_leftMouseDown = true;
			m_anchor = PointToScreen(e.Location);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			
			m_leftMouseDown = false;

			// Save the popup's size in case it was changed.
			SettingsHandler.SaveSettingsValue(m_savedSettingsName, "width", Width);
			SettingsHandler.SaveSettingsValue(m_savedSettingsName, "height", Height);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			if (!m_leftMouseDown)
				Cursor = Cursors.Default;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			Point pt1 = new Point(ClientSize.Width - kHotDimension,	ClientSize.Height - 1);
			Point pt2 = new Point(ClientSize.Width - 1, ClientSize.Height - kHotDimension);

			// Draw diagonal lines in the bottom right corner.
			for (int i = 0; i < 5; i++)
			{
				e.Graphics.DrawLine(SystemPens.MenuText, pt1, pt2);
				pt1.X += 3;
				pt2.X += 3;
			}
		}
	}
}
