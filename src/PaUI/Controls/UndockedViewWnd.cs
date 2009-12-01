using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class UndockedViewWnd : Form, IUndockedViewWnd
	{
		private ITMAdapter m_mainMenuAdapter;
		private Control m_view;
		private bool m_checkForModifiedDataSources = true;
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UndockedViewWnd()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UndockedViewWnd(Control view) : this()
		{
			if (view != null)
				Name = view.GetType().Name;

			m_mainMenuAdapter = PaApp.LoadDefaultMenu(this);
			m_mainMenuAdapter.AllowUpdates = false;
			Controls.Add(view);
			view.BringToFront();
			m_view = view;
			PaApp.SettingsHandler.LoadFormProperties(this);
			Opacity = 0;

			sbProgress.Visible = false;
			sblblMain.Text = sblblProgress.Text = string.Empty;
			MinimumSize = PaApp.MinimumViewWindowSize;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			// Fade-in the undocked form because it looks cool.
			while (Opacity < 1.0)
			{
				try
				{
					System.Threading.Thread.Sleep(10);
					Opacity += 0.05f;
				}
				catch
				{
					try
					{
						Opacity = 1;
					}
					catch { }
				}
				finally
				{
					SilUtils.Utils.UpdateWindow(Handle);
				}
			}

			m_checkForModifiedDataSources = false;
			Activate();
			m_checkForModifiedDataSources = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);

			if (m_mainMenuAdapter != null)
				m_mainMenuAdapter.AllowUpdates = true;
		
			SilUtils.Utils.UpdateWindow(Handle);

			bool reloadProjectsOnActivate =
				PaApp.SettingsHandler.GetBoolSettingsValue(PaApp.kAppSettingsName,
				"reloadprojectsonactivate", true);

			if (PaApp.Project != null && m_checkForModifiedDataSources && reloadProjectsOnActivate)
				PaApp.Project.CheckForModifiedDataSources();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);

			if (m_mainMenuAdapter != null)
				m_mainMenuAdapter.AllowUpdates = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			PaApp.SettingsHandler.SaveFormProperties(this);
			Visible = false;
			PaApp.UnloadDefaultMenu(m_mainMenuAdapter);
			m_mainMenuAdapter.Dispose();
			m_mainMenuAdapter = null;
			Controls.Remove(m_view);
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the status bar.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public StatusStrip StatusBar
		{
			get { return statusStrip; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the status bar label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolStripStatusLabel StatusBarLabel
		{
			get { return sblblMain; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the progress bar.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolStripProgressBar ProgressBar
		{
			get { return sbProgress; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the progress bar's label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolStripStatusLabel ProgressBarLabel
		{
			get { return sblblProgress; }
		}
	}
}