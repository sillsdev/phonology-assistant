// --------------------------------------------------------------------------------------------
#region // Copyright © 2002-2004, SIL International. All Rights Reserved.
// <copyright from='2002' to='2004' company='SIL International'>
//		Copyright © 2002-2004, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: SplashScreen.cs
// Responsibility: TE Team
// Last reviewed: 
// 
// <remarks>
// Splash Screen
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Timer=System.Windows.Forms.Timer;

namespace SIL.SpeechTools.Utils
{
	#region SplashScreenForm class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	internal class SplashScreenForm : Form
	{
		#region Data members
		private delegate void UpdateOpacityDelegate();
		private delegate void MakeFullyOpaqueDelegate();
		// NOTE: we use a Forms.Timer here (compared to Threading.Timer in FW)
		// because we can't set the stack size of a thread in the thread pool which the 
		// Threading.Timer uses and so we'd get a stack overflow.
		private Timer m_timer;
		private EventWaitHandle m_waitHandle;
		private Panel m_panel;
		private PictureBox pictureBox1;
		private Label lblVersion;
		private Label lblMessage;
		private Label lblCopyright;
		private Label lblProductName;
		private bool m_useFading = true;
		private Label lblBuildNumber;
		private readonly bool m_showBuildNum = false;
		private readonly bool m_isBetaVersion = false;
		private readonly string m_versionFmt;
		private readonly string m_buildFmt;
		#endregion

		#region Constructor
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Default Constructor for SplashScreen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SplashScreenForm()
		{
			InitializeComponent();
			m_versionFmt = lblVersion.Text;
			m_buildFmt = lblBuildNumber.Text;
			lblCopyright.Font = SystemInformation.MenuFont;
			lblVersion.Font = SystemInformation.MenuFont;
			lblMessage.Font = SystemInformation.MenuFont;
			lblBuildNumber.Font = SystemInformation.MenuFont;
			Opacity = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SplashScreenForm(bool showBuildNum, bool isBetaVersion) : this()
		{
			m_showBuildNum = showBuildNum;
			m_isBetaVersion = isBetaVersion;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Disposes of the resources (other than memory) used by the 
		/// <see cref="T:System.Windows.Forms.Form"></see>.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false 
		/// to release only unmanaged resources.</param>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			STUtils.s_splashScreen = null;

			if (disposing)
			{
				if (m_timer != null)
					m_timer.Dispose();
			}

			m_timer = null;
			m_waitHandle = null;
			base.Dispose(disposing);
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreenForm));
			this.m_panel = new System.Windows.Forms.Panel();
			this.lblBuildNumber = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.lblCopyright = new System.Windows.Forms.Label();
			this.lblProductName = new System.Windows.Forms.Label();
			this.m_panel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// m_panel
			// 
			this.m_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.m_panel.Controls.Add(this.lblBuildNumber);
			this.m_panel.Controls.Add(this.pictureBox1);
			this.m_panel.Controls.Add(this.lblVersion);
			this.m_panel.Controls.Add(this.lblMessage);
			this.m_panel.Controls.Add(this.lblCopyright);
			this.m_panel.Controls.Add(this.lblProductName);
			resources.ApplyResources(this.m_panel, "m_panel");
			this.m_panel.Name = "m_panel";
			this.m_panel.Paint += new System.Windows.Forms.PaintEventHandler(this.m_panel_Paint);
			// 
			// lblBuildNumber
			// 
			resources.ApplyResources(this.lblBuildNumber, "lblBuildNumber");
			this.lblBuildNumber.BackColor = System.Drawing.Color.Transparent;
			this.lblBuildNumber.Name = "lblBuildNumber";
			// 
			// pictureBox1
			// 
			this.pictureBox1.ErrorImage = null;
			this.pictureBox1.Image = global::SIL.SpeechTools.Utils.Properties.Resources.kimidSilLogo1;
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.InitialImage = null;
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// lblVersion
			// 
			resources.ApplyResources(this.lblVersion, "lblVersion");
			this.lblVersion.BackColor = System.Drawing.Color.Transparent;
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.UseMnemonic = false;
			// 
			// lblMessage
			// 
			this.lblMessage.BackColor = System.Drawing.Color.Transparent;
			this.lblMessage.ForeColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.lblMessage, "lblMessage");
			this.lblMessage.Name = "lblMessage";
			// 
			// lblCopyright
			// 
			this.lblCopyright.BackColor = System.Drawing.Color.Transparent;
			this.lblCopyright.ForeColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.lblCopyright, "lblCopyright");
			this.lblCopyright.Name = "lblCopyright";
			// 
			// lblProductName
			// 
			resources.ApplyResources(this.lblProductName, "lblProductName");
			this.lblProductName.BackColor = System.Drawing.Color.Transparent;
			this.lblProductName.ForeColor = System.Drawing.Color.Black;
			this.lblProductName.Name = "lblProductName";
			this.lblProductName.UseMnemonic = false;
			// 
			// SplashScreenForm
			// 
			resources.ApplyResources(this, "$this");
			this.BackColor = System.Drawing.Color.White;
			this.ControlBox = false;
			this.Controls.Add(this.m_panel);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SplashScreenForm";
			this.Opacity = 0;
			this.ShowInTaskbar = false;
			this.m_panel.ResumeLayout(false);
			this.m_panel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Public Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the splash screen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RealShow(EventWaitHandle waitHandle, bool useFading)
		{
			m_waitHandle = waitHandle;
			InitControlLabels();
			m_useFading = useFading;

			if (!useFading)
			{
				Opacity = 1;
				Show();
				return;
			}
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Activates (brings back to the top) the splash screen (assuming it is already visible
		/// and the application showing it is the active application).
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public void RealActivate()
		{
			BringToFront();
			Refresh();
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the splash screen
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public void RealClose()
		{
			if (m_timer != null)
				m_timer.Stop();
			
			Close();
		}
		#endregion

		#region Public Properties needed for all clients
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The message to display to indicate startup activity on the splash screen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetMessage(string value)
		{
			// In some rare cases, setting the text causes an exception which should just
			// be ignored.
			try
			{
				lblMessage.Text = value;
			}
			catch { }
		}
		#endregion

		#region Public properties set automatically in constructor for .Net apps
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The product name which appears in the Name label on the splash screen
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored. They should set the
		/// AssemblyTitle attribute in AssemblyInfo.cs of the executable.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		public void SetProdName(string value)
		{
			lblProductName.Text = value;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The product version which appears in the App Version label on the splash screen
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored. They should set the
		/// AssemblyFileVersion attribute in AssemblyInfo.cs of the executable.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		public void SetProdVersion(string value)
		{
#if DEBUG
			lblVersion.Text = string.Format(m_versionFmt, value, "(Debug version)",
				(m_isBetaVersion ? "Beta" : string.Empty));
#else
			lblVersion.Text = string.Format(m_versionFmt, value, string.Empty,
				(m_isBetaVersion ? "Beta" : string.Empty));
#endif
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The copyright info which appears in the Copyright label on the splash screen
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored. They should set the
		/// AssemblyCopyrightAttribute attribute in AssemblyInfo.cs of the executable.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		public void SetCopyright(string value)
		{
			lblCopyright.Text = value.Replace("(C)", "©");
		}

		#endregion

		#region Non-public methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged"></see> event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		/// ------------------------------------------------------------------------------------
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (Visible && m_useFading)
				m_waitHandle.Set();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tasks needing to be done when Window is being opened: Set window position.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			Left = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
			Top = (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;

			// set build label visibility
			lblBuildNumber.Visible = m_showBuildNum;

			// start a timer to ramp up the opacity of the window.
			m_timer = new Timer();
			m_timer.Interval = 50;
			m_timer.Tick += OnUpdateOpacity;
			m_timer.Start();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initialize text of controls prior to display
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void InitControlLabels()
		{
			try
			{
				// Set the Application label to the name of the app
				object[] attributes;
				Assembly assembly = Assembly.GetEntryAssembly();

				if (assembly != null)
				{
					string productName = Application.ProductName;

					if (string.IsNullOrEmpty(productName))
					{
						attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
						productName = (attributes != null && attributes.Length > 0) ?
							((AssemblyTitleAttribute)attributes[0]).Title : "Unknown";
					}

					lblProductName.Text = productName;
					Text = productName;

					lblBuildNumber.Visible = m_showBuildNum;
					if (m_showBuildNum)
					{
						// The build number is just the number of days since 01/01/2000
						int bldNum = assembly.GetName().Version.Build;
						DateTime bldDate = new DateTime(2000, 1, 1).Add(new TimeSpan(bldNum, 0, 0, 0));
						lblBuildNumber.Text = string.Format(m_buildFmt, bldDate.ToString("dd-MMM-yyyy"));
					}

					// Set the application version text
					string appVersion = assembly.GetName().Version.ToString(2);

#if DEBUG
					lblVersion.Text = string.Format(m_versionFmt, appVersion, "(Debug version)",
						(m_isBetaVersion ? "Beta" : string.Empty));
#else
					lblVersion.Text = string.Format(m_versionFmt, appVersion, string.Empty,
						(m_isBetaVersion ? "Beta" : string.Empty));
#endif
				}
				// Get copyright information from assembly info. By doing this we don't have
				// to update the splash screen each year.
				string copyRight;
				attributes = Assembly.GetExecutingAssembly()
					.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes != null && attributes.Length > 0)
					copyRight = ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
				else
				{
					// if we can't find it in the assembly info, use generic one (which 
					// might be out of date)
					copyRight = "(C) 2002-2007 SIL International";
				}

				lblCopyright.Text = string.Format(lblCopyright.Text,
					copyRight.Replace("(C)", "©"), "\n");
			}
			catch
			{
				// ignore errors
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_panel_Paint(object sender, PaintEventArgs e)
		{
			Color clr1 = m_panel.BackColor;
			Color clr2 = ColorHelper.CalculateColor(Color.White, Color.DarkGray, 150);
			Rectangle rc = m_panel.ClientRectangle;
			using (LinearGradientBrush br = new LinearGradientBrush(rc, clr1, clr2, 45))
				e.Graphics.FillRectangle(br, rc);

			const int dypLineThickness = 2;
			int nTopOfGrayLine = lblCopyright.Bottom +
				(lblMessage.Top - lblCopyright.Bottom) / 2 -
				dypLineThickness;

			int x1 = 16;
			int x2 = m_panel.ClientSize.Width - (x1 + 1);

			using (Pen pen = new Pen(Color.FromArgb(128, 128, 128)))
			{
				e.Graphics.DrawLine(pen, x1, nTopOfGrayLine, x2, nTopOfGrayLine);
				e.Graphics.DrawLine(pen, x1, nTopOfGrayLine + 1, x2, nTopOfGrayLine + 1);
				pen.Color = Color.FromArgb(192, 192, 192);
				e.Graphics.DrawLine(pen, x1, nTopOfGrayLine + 2, x2, nTopOfGrayLine + 2);
				e.Graphics.DrawLine(pen, x1, nTopOfGrayLine + 3, x2, nTopOfGrayLine + 3);
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Timer event to increase the opacity of the splash screen over time. Since this
		/// event occurs in a different thread from the one in which the form exists, we
		/// cannot set the form's opacity property in this thread because it will generate
		/// a cross threading error. Calling the invoke method will invoke the method on
		/// the same thread in which the form was created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void OnUpdateOpacity(object sender, EventArgs e)
		{
			if (m_timer == null)
				return;

			// This callback might get called multiple times before the Invoke is finished,
			// which causes some problems. We just ignore any callbacks we get while we are
			// processing one, so we are using TryEnter/Exit(this) instead of lock(this).
			// We sync on "this" so that we're using the same flag as the FwSplashScreen class.
			if (Monitor.TryEnter(this))
			{
				try
				{
					// In some rare cases the splash screen is already disposed and the 
					// timer is still running. It happened to me (EberhardB) when I stopped 
					// debugging while starting up, but it might happen at other times too 
					// - so just be safe.
					if (!IsDisposed && IsHandleCreated)
						Invoke(new UpdateOpacityDelegate(UpdateOpacity));
				}
				catch (Exception ex)
				{
					// just ignore any exceptions
					Debug.WriteLine("Got exception in OnUpdateOpacity: " + ex.Message);
				}
				finally
				{
					Monitor.Exit(this);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateOpacity()
		{
			try
			{
				double currentOpacity = Opacity;

				if (currentOpacity == 0.0)
					Refresh();
				
				if (currentOpacity < 1.0)
					Opacity = currentOpacity + 0.05;
				else if (m_timer != null)
				{
					m_timer.Stop();
					m_timer.Dispose();
					m_timer = null;
				}
			}
			catch
			{
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void MakeFullyOpaque()
		{
			try
			{
				Refresh();
				Opacity = 1.0;
				Thread.Sleep(1200);
			}
			catch
			{
			}
		}
	}
	#endregion
}
