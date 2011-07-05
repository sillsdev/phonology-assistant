using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Filters;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public partial class UndockedViewWnd : Form, IUndockedViewWnd, IxCoreColleague
	{
		private readonly Control _view;
		private ITMAdapter _mainMenuAdapter;
		private bool _checkForModifiedDataSources = true;
		private readonly PaProject _project;

		/// ------------------------------------------------------------------------------------
		public UndockedViewWnd()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		public UndockedViewWnd(PaProject project, Control view) : this()
		{
			_project = project;

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

			_mainMenuAdapter = App.LoadDefaultMenu(this);
			_mainMenuAdapter.AllowUpdates = false;
			Controls.Add(view);
			view.BringToFront();
			_view = view;
			Opacity = 0;

			sblblMain.Text = sblblProgress.Text = string.Empty;
			sblblProgress.Font = FontHelper.MakeFont(FontHelper.UIFont, 9, FontStyle.Bold);
			sblblProgress.Visible = false;
			sbProgress.Visible = false;
			sblblPercent.Visible = false;
			MinimumSize = App.MinimumViewWindowSize;

			sblblFilter.Paint += HandleFilterStatusStripLabelPaint;
			
			if (project != null)
				OnFilterChanged(project.CurrentFilter);

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

			_checkForModifiedDataSources = false;
			Activate();
			_checkForModifiedDataSources = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);

			if (_mainMenuAdapter != null)
				_mainMenuAdapter.AllowUpdates = true;

			Invalidate();  // Used to be: Utils.UpdateWindow(Handle); but I'm not sure why. I suspect there was a good reason though.

			if (_project != null && _checkForModifiedDataSources &&
				Settings.Default.ReloadProjectsWhenAppBecomesActivate)
			{
				_project.CheckForModifiedDataSources();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);

			if (_mainMenuAdapter != null)
				_mainMenuAdapter.AllowUpdates = false;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			App.MsgMediator.RemoveColleague(this);
			Visible = false;
			App.UnloadDefaultMenu(_mainMenuAdapter);
			_mainMenuAdapter.Dispose();
			_mainMenuAdapter = null;
			Controls.Remove(_view);
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

			if (_project == null)
				Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleProgressLabelVisibleChanged(object sender, EventArgs e)
		{
			sblblMain.BorderSides = (sblblProgress.Visible ?
				ToolStripStatusLabelBorderSides.Right : ToolStripStatusLabelBorderSides.None);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFilterStatusStripLabelPaint(object sender, PaintEventArgs e)
		{
			if (_project != null && _project.CurrentFilter != null)
			{
				PaMainWnd.PaintFilterStatusStripLabel(sender as ToolStripStatusLabel,
					_project.CurrentFilter.Name, e);
			}
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
		/// Gets the status bar label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolStripStatusLabel ProgressPercentLabel
		{
			get { return sblblPercent; }
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnFilterChanged(object args)
		{
			var filter = args as Filter;
			sblblFilter.Visible = (filter != null);
			if (filter != null)
			{
				sblblFilter.Text = filter.Name;
				var constraint = new Size(statusStrip.Width / 3, 0);
				sblblFilter.Width = sblblFilter.GetPreferredSize(constraint).Width + 20;
			}

			return false;
		}

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