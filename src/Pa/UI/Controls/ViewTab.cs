using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public class ViewTab : Label
	{
		private static bool s_viewSelectionInProgress;
		private bool _ignoreTabSelection;
		private Point _mouseDownLocation = Point.Empty;
		private bool _mouseOver;
		private bool _selected;
		private UndockedViewWnd _viewsForm;
		private bool _viewDocked;
		private bool _undockingInProgress;

		/// <summary>
		/// This flag gets set when a view is undocking. Suppose view A is being undocked.
		/// That means  another view in the main application window has to become active within
		/// the main window. Suppose that view is B. If view B was not previously opened
		/// (opening a view for the first time also triggers a docking of that view) then it
		/// will be and this flag will prevent the opening and docking of view B from causing
		/// its parent window (i.e. the main application window) to become activated, which
		/// is the normal behavior when a view is docked.
		/// </summary>
		private static bool s_undockingInProgress;

		public Func<string> HelpToolTipProvider { get; private set; }
		private readonly Func<Control> _viewTabProvider;

		/// ------------------------------------------------------------------------------------
		public ViewTab(ViewTabGroup owningTabControl, string text, Image img,
			Type viewType, Func<Control> viewTabProvider,
			string helptopicid, Func<string> helpToolTipProvider)
		{
			base.DoubleBuffered = true;
			base.AutoSize = false;
			base.AllowDrop = true;
			base.Font = ViewTabGroup.s_tabFont;
			Dock = DockStyle.Left;

			OwningTabGroup = owningTabControl;
			TabImage = img;
			Text = Utils.RemoveAcceleratorPrefix(text);
			ViewType = viewType;
			_viewTabProvider = viewTabProvider;
			HelpToolTipProvider = helpToolTipProvider;
			HelpTopicId = helptopicid;

			if (App.MainForm != null)
				App.MainForm.Activated += MainForm_Activated;
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (App.MainForm != null)
					App.MainForm.Activated -= MainForm_Activated;

				if (_viewsForm != null && !_viewsForm.IsDisposed)
				{
					_viewsForm.FormClosing -= m_viewsForm_FormClosing;
					_viewsForm.FormClosed -= m_viewsForm_FormClosed;
					_viewsForm.Activated -= m_viewsForm_Activated;
					_viewsForm.Dispose();
				}
			}
			
			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public Control OpenView()
		{
			App.StatusBarLabel.Text = string.Empty;

			// Check if the view is already loaded.
			if (View != null)
			{
				if (_viewsForm != null)
					_viewsForm.Activate();
				else if (View != null)
					View.Visible = true;

				return View;
			}

			View = _viewTabProvider();
			App.MsgMediator.SendMessage("BeginViewOpen", View);
			View.Dock = DockStyle.Fill;

			if (!(View is ITabView))
				Utils.MsgBox(string.Format("Error: {0} is not based on ITabView!", View.GetType()));

			try
			{
				if (View is IxCoreColleague)
					App.AddMediatorColleague(View as IxCoreColleague);
			}
			catch { }

			DockView();
			App.MsgMediator.SendMessage("ViewOpened", View);
			return View;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the tab's view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CloseView()
		{
			if (_undockingInProgress || View == null)
				return;

			App.MsgMediator.SendMessage("BeginViewClosing", View);
			Visible = true;

			if (!View.IsDisposed)
			{
				if (OwningTabGroup != null && OwningTabGroup.Controls.Contains(View))
					OwningTabGroup.Controls.Remove(View);

				if (View is IxCoreColleague)
					App.RemoveMediatorColleague(View as IxCoreColleague);

				if (View is ITabView)
					((ITabView)View).TMAdapter.Dispose();

				View.Dispose();
				View = null;
			}

			if (_viewsForm != null)
				_viewsForm.Close();

			App.MsgMediator.SendMessage("ViewClosed", ViewType);
			_viewDocked = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves the view's control from its form to the tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void DockView()
		{
			if (_undockingInProgress || _viewDocked)
				return;

			App.MsgMediator.SendMessage("BeginViewDocking", View);
			Utils.SetWindowRedraw(OwningTabGroup, false, false);
			Visible = true;
			
			OwningTabGroup.ViewWasDocked(this);
			View.Size = OwningTabGroup.ClientSize;
			OwningTabGroup.Controls.Add(View);
			View.PerformLayout();
			View.BringToFront();

			_viewDocked = true;
			_ignoreTabSelection = true;
			OwningTabGroup.SelectTab(this);
			_ignoreTabSelection = false;

			Utils.SetWindowRedraw(OwningTabGroup, true, true);
			View.Focus();
			OwningTabGroup.SetActiveView(View as ITabView, false);
			App.MsgMediator.SendMessage("ViewDocked", View);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves the view's control from its form to the tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UnDockView()
		{
			if (s_undockingInProgress || !_viewDocked)
				return;

			App.MsgMediator.SendMessage("BeginViewUnDocking", View);
			_undockingInProgress = true;
			s_undockingInProgress = true;

			if (OwningTabGroup.Controls.Contains(View))
				OwningTabGroup.Controls.Remove(View);

			// Prepare the undocked view's form to host the view and be displayed.
			_viewsForm = new UndockedViewWnd(View);
			_viewsForm.FormClosing += m_viewsForm_FormClosing;
			_viewsForm.FormClosed += m_viewsForm_FormClosed;
			_viewsForm.Activated += m_viewsForm_Activated;

			if (TabImage != null)
				_viewsForm.Icon = Icon.FromHandle(((Bitmap)TabImage).GetHicon());
			
			// Strip out accelerator key prefixes but keep ampersands that should be kept.
			var prjName = ((ITabView)View).Project.Name;
			var caption = Utils.RemoveAcceleratorPrefix(Text);
			var fmt = App.GetString("UndockedViewCaptionFormat", "{0} ({1}) - {2}",
				"Parameter one is the project name; parameter 2 is the view name; parameter 3 is the application name.");
			_viewsForm.Text = string.Format(fmt, prjName, caption, Application.ProductName);
			
			Visible = false;

			// Inform the tab group that one of it's views has been undocked.
			_ignoreTabSelection = true;
			OwningTabGroup.ViewWasUnDocked(this);
			_ignoreTabSelection = false;
			s_undockingInProgress = false;
			_viewDocked = false;
			_undockingInProgress = false;

			_viewsForm.Show();
			_viewsForm.Activate();
			App.MsgMediator.SendMessage("ViewUndocked", View);
		}

		/// ------------------------------------------------------------------------------------
		private void MainForm_Activated(object sender, EventArgs e)
		{
			if (_viewDocked && View != null && View.Visible)
				OwningTabGroup.SetActiveView(View as ITabView, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the global view and view type are set when an undocked view gets focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void m_viewsForm_Activated(object sender, EventArgs e)
		{
			OwningTabGroup.SetActiveView(View as ITabView, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When closing because the user closed the window, we just want to treat that as
		/// docking back into the tab. So dock the control and cancel the closing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_viewsForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_undockingInProgress)
			{
				e.Cancel = true;
				return;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure to clean up after a view is closed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_viewsForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			_viewsForm.FormClosing -= m_viewsForm_FormClosing;
			_viewsForm.FormClosed -= m_viewsForm_FormClosed;
			_viewsForm.Activated -= m_viewsForm_Activated;
			
			if (View != null)
				DockView();
			
			_viewsForm.Dispose();
			_viewsForm = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes and reopens the view's form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshView()
		{
			CloseView();
			OpenView();
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public Image TabImage { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the help topic ID for the view's overview topic.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string HelpTopicId { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab's view type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Type ViewType { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab's view form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Control View { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the tab's view is docked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsViewDocked
		{
			get { return _viewDocked; }
			set
			{
				if (_viewDocked != value)
				{
					if (value)
						DockView();
					else
						UnDockView();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool Selected
		{
			get { return _selected; }
			set
			{
				if (_selected == value || s_viewSelectionInProgress)
					return;

				s_viewSelectionInProgress = true;
				_selected = value;
				Invalidate();
				Utils.UpdateWindow(Handle);

				// Invalidate the tab to the left of this one in
				// case it needs to redraw its etched right border.
				ViewTab adjacentTab = OwningTabGroup.FindFirstVisibleTabToLeft(this);
				if (adjacentTab != null)
				{
					adjacentTab.Invalidate();
					Utils.UpdateWindow(adjacentTab.Handle);
				}

				if (!_ignoreTabSelection)
				{
					if (value)
						OpenView();
					else if (View != null && _viewDocked)
						View.Visible = false;
				}

				s_viewSelectionInProgress = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the tab's owning group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewTabGroup OwningTabGroup { get; set; }

		#endregion

		#region Overridden methods and event handlers
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Make sure the current tab is selected when its grid get's focus.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void HandleResultViewEnter(object sender, EventArgs e)
		//{
		//    if (!m_selected || !m_owningTabGroup.IsCurrent)
		//        m_owningTabGroup.SelectTab(this, true);
		//}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				_mouseDownLocation = e.Location;
			else
			{
				Form frm = FindForm();
				if (!App.IsFormActive(frm))
					frm.Focus();
			}

			base.OnMouseDown(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseUp(MouseEventArgs e)
		{
			_mouseDownLocation = Point.Empty; 
			base.OnMouseUp(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			// This will be empty when the mouse button is not down.
			if (_mouseDownLocation.IsEmpty)
				return;
			
			// Begin draging a tab when the mouse is held down
			// and has moved 4 or more pixels in any direction.
			int dx = Math.Abs(_mouseDownLocation.X - e.X);
			int dy = Math.Abs(_mouseDownLocation.Y - e.Y);
			if (dx >= 4 || dy >= 4)
			{
				_mouseDownLocation = Point.Empty;
				DoDragDrop(this, DragDropEffects.Move);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			_mouseOver = true;
			Invalidate();
			base.OnMouseEnter(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			_mouseOver = false;
			Invalidate();
			base.OnMouseLeave(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			DrawBackground(e.Graphics);
			DrawImage(e.Graphics);
			DrawText(e.Graphics);
			DrawHoverIndicator(e.Graphics);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the tab's background.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawBackground(Graphics g)
		{
			var rc = ClientRectangle;

			// First, fill the entire background with the control color.
			g.FillRectangle(SystemBrushes.Control, rc);

			var pts = new[] {new Point(0, rc.Bottom), new Point(0, rc.Top + 3),
				new Point(3, 0), new Point(rc.Right - 4, 0), new Point(rc.Right - 1, rc.Top + 3),
				new Point(rc.Right - 1, rc.Bottom)};

			if (_selected)
			{
				using (var br = new SolidBrush(Color.White))
					g.FillPolygon(br, pts);

				g.DrawLines(SystemPens.ControlDark, pts);
			}
			else
			{
				// Draw the etched line on the right edge to act as a separator. But
				// only draw it when the tab to the right of this one is not selected.
				if (!OwningTabGroup.IsRightAdjacentTabSelected(this))
				{
					g.DrawLine(SystemPens.ControlDark, rc.Width - 2, 1, rc.Width - 2, rc.Height - 5);
					g.DrawLine(SystemPens.ControlLight, rc.Width - 1, 1, rc.Width - 1, rc.Height - 5);
				}

				// The tab is not selected tab, so draw a
				// line across the bottom of the tab.
				g.DrawLine(SystemPens.ControlDark, 0, rc.Bottom - 1, rc.Right, rc.Bottom - 1);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw's the tab's image.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawImage(Graphics g)
		{
			if (TabImage == null)
				return;

			var rc = ClientRectangle;
			rc.X = 7;
			rc.Y = (rc.Height - TabImage.Height) / 2;
			rc.Size = TabImage.Size;

			if (_selected)
				rc.Y++;

			g.DrawImage(TabImage, rc);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the tab's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawText(IDeviceContext g)
		{
			const TextFormatFlags kFlags = TextFormatFlags.VerticalCenter |
				TextFormatFlags.HorizontalCenter | TextFormatFlags.WordEllipsis |
				TextFormatFlags.SingleLine | TextFormatFlags.NoPadding |
				TextFormatFlags.HidePrefix | TextFormatFlags.PreserveGraphicsClipping;

			var clrText = (_selected ? Color.Black :
				ColorHelper.CalculateColor(SystemColors.ControlText,
				SystemColors.Control, 145));
			
			var rc = ClientRectangle;

			// Account for the image if there is one.
			if (TabImage != null)
			{
				rc.X += (5 + TabImage.Width);
				rc.Width -= (5 + TabImage.Width);
			}

			// When the tab is selected, then bump the text down a couple of pixels.
			if (_selected)
			{
				rc.Y += 2;
				rc.Height -= 2;
			}

			TextRenderer.DrawText(g, Text, Font, rc, clrText, kFlags);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the mouse is over the tab, draw a line across the top to hightlight the tab
		/// the mouse is over.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawHoverIndicator(Graphics g)
		{
			if (!_mouseOver)
				return;

			var rc = ClientRectangle;

			var clr = (PaintingHelper.CanPaintVisualStyle() ?
				VisualStyleInformation.ControlHighlightHot : SystemColors.Highlight);

			// Draw the lines that only show when the mouse is over the tab.
			using (Pen pen = new Pen(clr))
			{
				if (_selected)
				{
					g.DrawLine(pen, 3, 1, rc.Right - 4, 1);
					g.DrawLine(pen, 2, 2, rc.Right - 3, 2);
				}
				else
				{
					g.DrawLine(pen, 0, 0, rc.Right - 3, 0);
					g.DrawLine(pen, 0, 1, rc.Right - 3, 1);
				}
			}
		}

		#endregion
	}
}
