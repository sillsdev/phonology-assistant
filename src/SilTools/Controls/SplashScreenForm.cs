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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Timer=System.Windows.Forms.Timer;

namespace SilTools
{
	#region SplashScreenForm class
	/// ----------------------------------------------------------------------------------------
	public class SplashScreenForm : Form
	{
		#region Data members
		private delegate void UpdateOpacityDelegate();
		
		// NOTE: we use a Forms.Timer here (compared to Threading.Timer in FW)
		// because we can't set the stack size of a thread in the thread pool which the 
		// Threading.Timer uses and so we'd get a stack overflow.
		private Timer m_timer;
		private EventWaitHandle m_waitHandle;
		protected Panel m_panel;
		protected PictureBox pictureBox1;
		protected Label lblVersion;
		protected Label lblMessage;
		protected Label lblCopyright;
		protected Label lblProductName;
		private bool m_useFading = true;
		private bool m_showStandardSILContent = true;
		protected Label lblBuildNumber;
		private readonly bool m_showBuildNum;
		private readonly VersionType m_versionType;
		private readonly string m_versionFmt;
		private PictureBox picLoadingWheel;
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
		public SplashScreenForm(bool showBuildNum, VersionType versionType) : this()
		{
			m_showBuildNum = showBuildNum;
			m_versionType = versionType;
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
			Utils.s_splashScreen = null;

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
			this.picLoadingWheel = new System.Windows.Forms.PictureBox();
			this.lblBuildNumber = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.lblCopyright = new System.Windows.Forms.Label();
			this.lblProductName = new System.Windows.Forms.Label();
			this.m_panel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picLoadingWheel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// m_panel
			// 
			this.m_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.m_panel.Controls.Add(this.picLoadingWheel);
			this.m_panel.Controls.Add(this.lblBuildNumber);
			this.m_panel.Controls.Add(this.pictureBox1);
			this.m_panel.Controls.Add(this.lblVersion);
			this.m_panel.Controls.Add(this.lblMessage);
			this.m_panel.Controls.Add(this.lblCopyright);
			this.m_panel.Controls.Add(this.lblProductName);
			resources.ApplyResources(this.m_panel, "m_panel");
			this.m_panel.Name = "m_panel";
			this.m_panel.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleBackgroundPanelPaint);
			// 
			// picLoadingWheel
			// 
			this.picLoadingWheel.BackColor = System.Drawing.Color.Transparent;
			this.picLoadingWheel.Image = global::SilTools.Properties.Resources.LoadingWheel;
			resources.ApplyResources(this.picLoadingWheel, "picLoadingWheel");
			this.picLoadingWheel.Name = "picLoadingWheel";
			this.picLoadingWheel.TabStop = false;
			// 
			// lblBuildNumber
			// 
			resources.ApplyResources(this.lblBuildNumber, "lblBuildNumber");
			this.lblBuildNumber.BackColor = System.Drawing.Color.Transparent;
			this.lblBuildNumber.Name = "lblBuildNumber";
			// 
			// pictureBox1
			// 
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Image = global::SilTools.Properties.Resources.kimidSilLogo;
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
			resources.ApplyResources(this.lblMessage, "lblMessage");
			this.lblMessage.AutoEllipsis = true;
			this.lblMessage.BackColor = System.Drawing.Color.Transparent;
			this.lblMessage.ForeColor = System.Drawing.Color.Black;
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
			this.DoubleBuffered = true;
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SplashScreenForm";
			this.Opacity = 0D;
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.m_panel.ResumeLayout(false);
			this.m_panel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picLoadingWheel)).EndInit();
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
		public virtual void RealShow(EventWaitHandle waitHandle, bool useFading)
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
		public virtual void RealActivate()
		{
			BringToFront();
			Refresh();
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the splash screen
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public virtual void RealClose()
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
		public virtual void SetMessage(string value)
		{
			// In some rare cases, setting the text causes an exception which should just
			// be ignored.
			try
			{
				lblMessage.Text = value;
				picLoadingWheel.Visible = !string.IsNullOrEmpty(value);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public bool ShowStandardSILContent
		{
			get { return m_showStandardSILContent; }
			set
			{
				m_showStandardSILContent = value;
				m_panel.Visible = value;
			}
		}

		#endregion

		#region Public properties set automatically in constructor for .Net apps
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The product version which appears in the App Version label on the splash screen
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored. They should set the
		/// AssemblyFileVersion attribute in AssemblyInfo.cs of the executable.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		public virtual void SetProdVersion(string value)
		{
			string version = value;
			if (string.IsNullOrEmpty(version))
			{
				var ver = new Version(Application.ProductVersion);
				version = ver.ToString(3);
			}

			var verType = string.Empty;
			if (m_versionType == VersionType.Alpha)
				verType = "Test Version";
			else if (m_versionType == VersionType.Beta)
				verType = "Beta";

#if DEBUG
			lblVersion.Text = string.Format(m_versionFmt, version, "(Debug version)", verType);
#else
			lblVersion.Text = string.Format(m_versionFmt, version, string.Empty, verType);
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
		public virtual void SetCopyright(string value)
		{
			lblCopyright.Text = value.Replace("(C)", "©");
		}

		#endregion

		#region Non-public methods
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			Utils.CenterFormInScreen(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged"></see> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (Visible && m_useFading && m_waitHandle != null)
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

			if (DesignMode)
				return;

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
		protected virtual void InitControlLabels()
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
						productName = (attributes.Length > 0) ? ((AssemblyTitleAttribute)attributes[0]).Title : "Unknown";
					}
					
					lblProductName.Text = productName;
					Text = productName;
				}

				lblBuildNumber.Visible = m_showBuildNum;
				
				// The build number is just the number of days since 01/01/2000
				var ver = new Version(Application.ProductVersion);
				//var bldDate = (ver.Build == 0 ?
				//    File.GetCreationTime(Application.ExecutablePath) :
				//    new DateTime(2000, 1, 1).Add(new TimeSpan(ver.Build, 0, 0, 0)));

				var bldDate = File.GetCreationTime(Application.ExecutablePath);
				lblBuildNumber.Text = string.Format(m_buildFmt, bldDate.ToString("dd-MMM-yyyy"));

				SetProdVersion(null);

				if (assembly == null)
					assembly = Assembly.GetExecutingAssembly();

				attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

				// Get copyright information from assembly info. By doing this we don't have to
				// update the splash screen each year. If we can't find the copyright in the
				// assembly info, use generic one (which might be out of date)
				var copyRight = attributes.Length > 0 ?
					((AssemblyCopyrightAttribute)attributes[0]).Copyright :
					string.Format("(C) 2002-{0} SIL International", DateTime.Now.Year);

				lblCopyright.Text = string.Format(lblCopyright.Text,
					copyRight.Replace("(C)", "©"), "\n");
			}
			catch
			{
				// ignore errors
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleBackgroundPanelPaint(object sender, PaintEventArgs e)
		{
			var clr1 = m_panel.BackColor;
			var clr2 = ColorHelper.CalculateColor(Color.White, Color.DarkGray, 150);
			var rc = m_panel.ClientRectangle;
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
		protected virtual void OnUpdateOpacity(object sender, EventArgs e)
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
		protected virtual void UpdateOpacity()
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
		public virtual void MakeFullyOpaque()
		{
			try
			{
				Refresh();
				Opacity = 1.0;
				Thread.Sleep(1200);
			}
			catch { }
		}
	}

	#endregion
}
