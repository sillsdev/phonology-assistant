using System;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Filters;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class UndockedViewWnd : Form, IUndockedViewWnd, IxCoreColleague
	{
		private readonly Control m_view;
		private ITMAdapter m_mainMenuAdapter;
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

			try
			{
				Settings.Default[Name] = App.InitializeForm(this, Settings.Default[Name] as FormSettings);
			}
			catch
			{
				StartPosition = FormStartPosition.CenterScreen;
			}

			m_mainMenuAdapter = App.LoadDefaultMenu(this);
			m_mainMenuAdapter.AllowUpdates = false;
			Controls.Add(view);
			view.BringToFront();
			m_view = view;
			Opacity = 0;

			sbProgress.Visible = false;
			sblblMain.Text = sblblProgress.Text = string.Empty;
			MinimumSize = App.MinimumViewWindowSize;

			sblblFilter.Paint += FilterHelper.HandleFilterStatusStripLabelPaint;
			OnFilterChanged(FilterHelper.CurrentFilter);

			App.MsgMediator.AddColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
					Utils.UpdateWindow(Handle);
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

			Invalidate();  // Used to be: Utils.UpdateWindow(Handle); but I'm not sure why. I suspect there was a good reason though.

			if (App.Project != null && m_checkForModifiedDataSources &&
				Settings.Default.ReloadProjectsWhenAppBecomesActivate)
			{
				App.Project.CheckForModifiedDataSources();
			}
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
			App.MsgMediator.RemoveColleague(this);
			Visible = false;
			App.UnloadDefaultMenu(m_mainMenuAdapter);
			m_mainMenuAdapter.Dispose();
			m_mainMenuAdapter = null;
			Controls.Remove(m_view);
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When there is no project open, this forces the gradient background to be repainted
		/// on the application workspace.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if (App.Project == null)
				Invalidate();

			sblblFilter.Width = Math.Max(175, statusStrip.Width / 3);
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnFilterChanged(object args)
		{
			var filter = args as Filter;
			sblblFilter.Visible = (filter != null);
			if (filter != null)
				sblblFilter.Text = filter.Name;

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnFilterTurnedOff(object args)
		{
			sblblFilter.Visible = false;
			sblblFilter.Text = string.Empty;
			return false;
		}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] { this };
		}

		#endregion
	}
}