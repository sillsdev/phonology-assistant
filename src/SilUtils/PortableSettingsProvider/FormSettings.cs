using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SilUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("windowSettings")]
	public class FormSettings
	{
		[XmlElement("state")]
		public FormWindowState State { get; set; }
		[XmlElement("bounds")]
		public string SerializedBounds { get; set; }
		[XmlElement("dpi")]
		public float DPI { get; set; }
			
		private readonly float m_currDpi;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FormSettings()
		{
			Bounds = new Rectangle(0, 0, 0, -1);
			State = FormWindowState.Normal;
			using (Form frm = new Form())
			using (Graphics g = frm.CreateGraphics())
				m_currDpi = DPI = g.DpiX;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Rectangle Bounds
		{
			get
			{
				var bnds = PortableSettingsProvider.GetIntArrayFromString(SerializedBounds);
				var rc = new Rectangle(0, 0, 0, -1);

				if (bnds.Length >= 1)
					rc.X = bnds[0];
				if (bnds.Length >= 2)
					rc.Y = bnds[1];
				if (bnds.Length >= 3)
					rc.Width = bnds[2];
				if (bnds.Length >= 4)
					rc.Height = bnds[3];

				return rc;
			}
			set
			{
				SerializedBounds = string.Format("{0}, {1}, {2}, {3}",
					value.X, value.Y, value.Width, value.Height);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static FormSettings Create(Form frm)
		{
			var formSettings = new FormSettings();
			formSettings.InitializeForm(frm);
			return formSettings; 
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InitializeForm(Form frm)
		{
			frm.ResizeEnd += HandleFormResizeEnd;
			frm.LocationChanged += HandleFormLocationChanged;
			frm.HandleDestroyed += HandleFormHandleDestroyed;

			if (Bounds.Height <= 0 || DPI != m_currDpi)
			{
				frm.StartPosition = FormStartPosition.CenterScreen;
				DPI = m_currDpi;
			}
			else
			{
				frm.StartPosition = FormStartPosition.Manual;
				frm.Bounds = Bounds;

				if (frm.WindowState != State)
					frm.WindowState = State;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFormLocationChanged(object sender, EventArgs e)
		{
			HandleFormResizeEnd(sender, e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFormResizeEnd(object sender, EventArgs e)
		{
			var frm = sender as Form;

			if (frm.WindowState == FormWindowState.Normal)
				Bounds = frm.Bounds;
			
			if (frm.WindowState != FormWindowState.Minimized)
				State = frm.WindowState;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleFormHandleDestroyed(object sender, EventArgs e)
		{
			var frm = sender as Form;
			frm.ResizeEnd -= HandleFormResizeEnd;
			frm.LocationChanged -= HandleFormLocationChanged;
			frm.HandleDestroyed -= HandleFormHandleDestroyed;
		}
	}
}
