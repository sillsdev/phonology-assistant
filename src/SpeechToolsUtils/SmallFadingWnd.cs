using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SIL.Pa.Data
{
	public partial class SmallFadingWnd : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SmallFadingWnd() : this(null)
		{
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SmallFadingWnd(string msg)
		{
			InitializeComponent();

			if (!string.IsNullOrEmpty(msg))
				lblMsg.Text = msg;

			Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - Width - 20,
				Screen.PrimaryScreen.WorkingArea.Bottom - Height - 2);

			double maxOpacity = Opacity;
			Opacity = 0;
			Show();

			while (Opacity < maxOpacity)
			{
				Opacity += 0.05f;
				Application.DoEvents();
				Thread.Sleep(50);
			}

			Opacity = maxOpacity;
			Application.DoEvents();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Message
		{
			get { return lblMsg.Text; }
			set
			{
				lblMsg.Text = value;
				Application.DoEvents();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CloseFade()
		{
			while (Opacity > 0)
			{
				Opacity -= 0.05f;
				Application.DoEvents();
				Thread.Sleep(50);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ShowWithoutActivation
		{
			get {return true;}
		}
	}
}