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
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace SilTools
{
	public enum VersionType
	{
		Alpha,
		Beta,
		Production
	}

	#region ISplashScreen interface
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Public interface (exported with COM wrapper) for the splash screen
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface ISplashScreen
	{
		/// ----------------------------------------------------------------------------------------
		void Show();

		/// ----------------------------------------------------------------------------------------
		void Show(bool showBuildDate, bool isBetaVersion);

		/// ----------------------------------------------------------------------------------------
		void ShowWithoutFade();

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Activates (brings back to the top) the splash screen (assuming it is already visible
		/// and the application showing it is the active application).
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		void Activate();

		/// ----------------------------------------------------------------------------------------
		void Close();

		/// ----------------------------------------------------------------------------------------
		void Refresh();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The product version which appears in the App Version label on the splash screen
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string ProdVersion {set;}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The copyright info to display on the splash screen
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string Copyright { set;}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The message to display to indicate startup activity on the splash screen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string Message { set;}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the ISplashScreen's underlying form is
		/// still available (i.e. non null).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool StillAlive { get;}
	}

	#endregion

	#region SplashScreen implementation
	/// ----------------------------------------------------------------------------------------
	public class SplashScreen : ISplashScreen
	{
		#region Data members
		private delegate void MethodWithStringDelegate(string value);

		private bool m_useFading = true;
		private Thread m_thread;
		private SplashScreenForm m_splashScreen;
		internal EventWaitHandle m_waitHandle;
		private bool m_showBuildNum;
		private VersionType m_versionType;
		#endregion

		#region Constructor
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Default Constructor for SplashScreen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SplashScreen()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SplashScreen(bool showBuildNum, VersionType versionType)
		{
			m_showBuildNum = showBuildNum;
			m_versionType = versionType;
		}
		
		#endregion

		#region Public Methods
		/// ------------------------------------------------------------------------------------
		void ISplashScreen.Show(bool showBuildDate, bool isBetaVersion)
		{
			m_showBuildNum = showBuildDate;
			m_versionType = (isBetaVersion ? VersionType.Beta : VersionType.Production);
			InternalShow();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the splash screen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void ISplashScreen.Show()
		{
			InternalShow();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Does the work of showing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InternalShow()
		{
			if (m_thread != null)
				return;

			m_waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

			if (Thread.CurrentThread.Name == null)
				Thread.CurrentThread.Name = "Main";

			Utils.s_splashScreen = this;

			// For some reason we have to specify a stack size, otherwise we get a stack overflow. 
			// The default stack size of 1MB works on WinXP. Needs to be 2MB on Win2K.
			// Don't know what value it's using if we don't specify it.
			m_thread = new Thread(StartSplashScreen, 0x200000);
			m_thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			m_thread.IsBackground = true;
			m_thread.SetApartmentState(ApartmentState.STA);
			m_thread.Name = "SplashScreen";
			m_thread.Start();
			m_waitHandle.WaitOne();
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the splash screen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void ISplashScreen.ShowWithoutFade()
		{
			m_useFading = false;
			StartSplashScreen();
			Utils.s_splashScreen = this;

			// Wait until the splash screen is actually up
			while (m_splashScreen == null || !m_splashScreen.Visible)
				Thread.Sleep(50);
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Activates (brings back to the top) the splash screen (assuming it is already visible
		/// and the application showing it is the active application).
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		void ISplashScreen.Activate()
		{
			if (!m_useFading)
			{
				m_splashScreen.Activate();
				Application.DoEvents();
				return;
			}

			Debug.Assert(m_splashScreen != null);
			lock (m_splashScreen)
			{
				m_splashScreen.Invoke(new MethodInvoker(m_splashScreen.Activate));
			}
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the splash screen
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		void ISplashScreen.Close()
		{
			if (m_splashScreen.Opacity < 1.0)
			{
				lock (m_splashScreen)
				{
					m_splashScreen.Invoke(new MethodInvoker(m_splashScreen.MakeFullyOpaque));
				}
			}
			
			Utils.s_splashScreen = null;

			if (m_splashScreen == null)
				return;

			if (!m_useFading)
			{
				m_splashScreen.Hide();
				m_splashScreen = null;
				return;
			}

			lock (m_splashScreen)
			{
				try
				{
					if (!m_splashScreen.IsDisposed)
						m_splashScreen.Invoke(new MethodInvoker(m_splashScreen.RealClose));
				}
				catch { }
			}

			try
			{
				m_thread.Join();
				lock (m_splashScreen)
				{
					m_splashScreen.Dispose();
				}
			}
			catch { }

			m_splashScreen = null;
			m_thread = null;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Refreshes the display of the splash screen
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		void ISplashScreen.Refresh()
		{
			if (!m_useFading)
			{
				m_splashScreen.Refresh();
				Application.DoEvents();
				return;
			}

			Debug.Assert(m_splashScreen != null);
			lock (m_splashScreen)
			{
				m_splashScreen.Invoke(new MethodInvoker(m_splashScreen.Refresh));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the ISplashScreen's underlying form is
		/// still available (i.e. non null).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool ISplashScreen.StillAlive
		{
			get {return m_splashScreen != null; }
		}

		#endregion

		#region Public Properties needed for all clients
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The message to display to indicate startup activity on the splash screen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string ISplashScreen.Message
		{
			set
			{
				Debug.Assert(m_splashScreen != null);
				lock (m_splashScreen)
				{
					m_splashScreen.Invoke(new MethodWithStringDelegate(m_splashScreen.SetMessage), value);
				}
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
		string ISplashScreen.ProdVersion
		{
			set
			{
				Debug.Assert(m_splashScreen != null);
				lock (m_splashScreen)
				{
					m_splashScreen.Invoke(new MethodWithStringDelegate(m_splashScreen.SetProdVersion), value);
				}
			}
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
		string ISplashScreen.Copyright
		{
			set
			{
				Debug.Assert(m_splashScreen != null);
				lock (m_splashScreen)
				{
					m_splashScreen.Invoke(new MethodWithStringDelegate(m_splashScreen.SetCopyright), value);
				}
			}
		}

		#endregion

		#region private/protected methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Starts the splash screen.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void StartSplashScreen()
		{
			m_splashScreen = GetSplashScreenForm();
			m_splashScreen.RealShow(m_waitHandle, m_useFading);
			if (m_useFading)
				m_splashScreen.ShowDialog();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual SplashScreenForm GetSplashScreenForm()
		{
			return new SplashScreenForm(m_showBuildNum, m_versionType);
		}
		
		#endregion
	}

	#endregion
}
