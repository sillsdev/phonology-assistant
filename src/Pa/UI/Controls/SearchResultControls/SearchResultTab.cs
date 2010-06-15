// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: SearchResultTab.cs
// Responsibility: Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Localization;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SilUtils;
using SilUtils.Controls;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SearchResultTab : Panel, IxCoreColleague
	{
		// The combined left and right margins of the image. 
		private const int kleftImgMargin = 6;

		private readonly XButton m_btnCIEOptions;
		private Point m_mouseDownLocation = Point.Empty;
		private bool m_mouseOver;
		private bool m_selected;
		private SearchResultTabGroup m_owningTabGroup;
		private SearchResultView m_resultView;
		private SearchQuery m_query;
		private bool m_tabTextClipped;
		private Image m_image;
		private ToolTip m_CIEButtonToolTip;
		private CustomDropDown m_cieOptionsDropDownContainer;
		private CIEOptionsDropDown m_cieOptionsDropDown;
		private Color m_activeTabInactiveGroupBack1;
		private Color m_activeTabInactiveGroupBack2;
		private Color m_activeTabInactiveGroupFore;
		private Color m_activeTabFore;
		private Color m_activeTabBack;
		private Color m_inactiveTabFore;
		private Color m_inactiveTabBack;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultTab(SearchResultTabGroup owningTabControl)
		{
			base.DoubleBuffered = true;
			base.AutoSize = false;
			base.AllowDrop = true;
			base.Font = FontHelper.PhoneticFont;
			m_owningTabGroup = owningTabControl;
			m_query = new SearchQuery();
			App.AddMediatorColleague(this);
			SetContextMenus();

			// Prepare the tab's minimal pair options button.
			Image img = Properties.Resources.kimidMinimalPairsOptionsDropDown;
			m_btnCIEOptions = new XButton();
			m_btnCIEOptions.Image = img;
			m_btnCIEOptions.Size = new Size(img.Width + 4, img.Height + 4);
			m_btnCIEOptions.BackColor = Color.Transparent;
			m_btnCIEOptions.Visible = false;
			m_btnCIEOptions.Left = kleftImgMargin;
			m_btnCIEOptions.Click += m_btnCIEOptions_Click;
			m_btnCIEOptions.MouseEnter += m_btnCIEOptions_MouseEnter;
			m_btnCIEOptions.MouseLeave += m_btnCIEOptions_MouseLeave;
			Controls.Add(m_btnCIEOptions);
			GetTabColors();

			Text = EmptyTabText;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void SetContextMenus()
		{
			if (base.ContextMenuStrip != null)
				base.ContextMenuStrip.Opening -= ContextMenuStrip_Opening;

			if (m_owningTabGroup != null && m_owningTabGroup.TMAdapter != null)
			{
				m_owningTabGroup.TMAdapter.SetContextMenuForControl(this, "cmnuSearchResultTab");

				if (base.ContextMenuStrip != null)
					base.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

				if (m_resultView != null && m_resultView.Grid != null)
				{
					m_owningTabGroup.TMAdapter.SetContextMenuForControl(
						m_resultView.Grid, "cmnuSearchResultTab");
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetTabColors()
		{
			m_activeTabInactiveGroupBack1 = App.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "activeininactivegroup1", Color.White);

			m_activeTabInactiveGroupBack2 = App.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "activeininactivegroup1", 0xFFD7D1C4);

			m_activeTabInactiveGroupFore = App.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "activeininactivegroupfore", Color.Black);

			m_activeTabBack = App.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "activetabback", Color.White);

			m_activeTabFore = App.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "activetabfore", Color.Black);

			m_inactiveTabBack = App.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "inactivetabback", SystemColors.Control);

			m_inactiveTabFore = App.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "inactivetabfore", SystemColors.ControlText);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up a little.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				UnsubscribeToGridEvents();

				if (ContextMenuStrip != null && !ContextMenuStrip.IsDisposed)
					ContextMenuStrip.Opening -= ContextMenuStrip_Opening;

				App.RemoveMediatorColleague(this);

				if (!m_btnCIEOptions.IsDisposed)
					m_btnCIEOptions.Dispose();

				if (m_image != null)
				{
					m_image.Dispose();
					m_image = null;
				}

				if (m_resultView != null)
				{
					m_resultView.Dispose();
					m_resultView = null;
				}

				if (m_query != null)
					m_query = null;
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal XButton CIEOptionsButton
		{
			get { return m_btnCIEOptions; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			ContextMenuStrip cms = sender as ContextMenuStrip;

			if (cms == null)
				return;

			if (cms.SourceControl == this && Selected && m_owningTabGroup.IsCurrent ||
				cms.SourceControl == m_resultView || cms.SourceControl == Grid)
			{
				m_owningTabGroup.ContextMenuTab = this;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the tab's result view with a new result cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshResultView(WordListCache resultCache)
		{
			Text = (resultCache == null ||
				resultCache.SearchQuery == null ? string.Empty :
				resultCache.SearchQuery.ToString());

			if (resultCache != null)
			{
				UnsubscribeToGridEvents();
				m_resultView.Initialize(resultCache);
				UpdateRecordView();
				SubscribeToGridEvents();
				m_query = resultCache.SearchQuery;
			}

			if (m_btnCIEOptions.Visible)
			{
				FindInfo.CanFindAgain = false;
				m_btnCIEOptions.Visible = (m_resultView != null &&
					m_resultView.Grid != null && m_resultView.Grid.Cache != null &&
					m_resultView.Grid.Cache.IsCIEList);
			}

			AdjustWidth();
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string EmptyTabText
		{
			get
			{
				return LocalizationManager.LocalizeString("SearchResultTabs.EmptySearchResultTabText",
					"(empty tab)", App.kLocalizationGroupMisc);
			}
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get & Set the CieOptionsDropDownContainer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CustomDropDown CieOptionsDropDownContainer
		{
			get { return m_cieOptionsDropDownContainer; }
			set { m_cieOptionsDropDownContainer = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get & Set the CieOptionsDropDown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEOptionsDropDown CieOptionsDropDown
		{
			get { return m_cieOptionsDropDown; }
			set { m_cieOptionsDropDown = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Image Image
		{
			get { return m_image; }
			set
			{
				if (m_image != value)
				{
					m_image = value;
					Invalidate();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the tab contains any results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsEmpty
		{
			get { return m_resultView == null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Selected
		{
			get { return m_selected; }
			set
			{
				if (m_selected != value)
				{
					m_selected = value;
					Invalidate();
					Utils.UpdateWindow(Handle);

					if (m_resultView != null)
					{
						if (m_resultView.Grid != null)
							m_resultView.Grid.IsCurrentPlaybackGrid = value;

						m_resultView.Visible = value;
						if (value)
						{
							m_resultView.BringToFront();
							if (m_resultView.Grid != null)
							{
								m_resultView.Grid.Focus();
								FindInfo.Grid = m_resultView.Grid;
							}
						}
					}
				}
				else if (m_owningTabGroup.IsCurrent && m_resultView != null &&
					m_resultView.Grid != null && !m_resultView.Grid.Focused)
				{
					m_resultView.Grid.Focus();
				}

				UpdateRecordView();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the tab's result view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultView ResultView
		{
			get { return m_resultView; }
			set
			{
				if (m_resultView == value)
					return;

				if (value == null)
					Clear();

				m_resultView = value;
				if (m_resultView != null)
				{
					m_query = m_resultView.SearchQuery;
					m_resultView.Dock = DockStyle.Fill;
					SubscribeToGridEvents();
					UpdateRecordView();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab's grid control from it's result view control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaWordListGrid Grid
		{
			get { return (m_resultView == null ? null : m_resultView.Grid); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab's search query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery SearchQuery
		{
			get { return m_query; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the tab's owning group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultTabGroup OwningTabGroup
		{
			get { return m_owningTabGroup; }
			set { m_owningTabGroup = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the text in a tab was clipped (i.e. was
		/// too long so it is displayed with ellipses).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool TabTextClipped
		{
			get { return m_tabTextClipped; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Color ForeColor
		{
			get
			{
				if (!m_selected)
					return m_inactiveTabFore;

				return (m_owningTabGroup.IsCurrent ?
					m_activeTabFore : m_activeTabInactiveGroupFore);
			}
			set { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get
			{
				if (!m_selected)
					return m_inactiveTabBack;

				return (m_owningTabGroup.IsCurrent ? m_activeTabBack : SystemColors.Control);
			}
			set
			{
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjust the tab's width based on it's text and font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void AdjustWidth()
		{
			const TextFormatFlags kFlags = TextFormatFlags.VerticalCenter |
				TextFormatFlags.SingleLine | TextFormatFlags.LeftAndRightPadding;

			int width;

			// Get the text's width.
			using (Graphics g = CreateGraphics())
				width = TextRenderer.MeasureText(g, Text, Font, Size.Empty, kFlags).Width;

			// Add a little for good measure.
			width += 6;

			if (m_image != null)
				width += (kleftImgMargin + m_image.Width);

			if (m_btnCIEOptions.Visible)
				width += (kleftImgMargin + m_btnCIEOptions.Width);

			// Don't allow the width of a tab to be any
			// wider than 3/4 of it's owning group's width.
			Width = Math.Min(width, (int)(m_owningTabGroup.Width * 0.75));

			m_tabTextClipped = (Width < width);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears the search results on the tab and sets the tab to an empty tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Clear()
		{
			RemoveResultView();

			if (m_owningTabGroup.RecordView != null)
				m_owningTabGroup.RecordView.Clear();

			m_query = new SearchQuery();
			m_btnCIEOptions.Visible = false;
			Text = EmptyTabText;
			AdjustWidth();
			m_owningTabGroup.AdjustTabContainerWidth();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the tab's result view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveResultView()
		{
			if (m_resultView != null)
			{
				UnsubscribeToGridEvents();
				if (m_owningTabGroup != null && m_owningTabGroup.Controls.Contains(m_resultView))
					m_owningTabGroup.Controls.Remove(m_resultView);

				m_resultView.Dispose();
				m_resultView = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the search results as a result of the project's underlying data sources
		/// changing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDataSourcesModified(object args)
		{
			if (m_resultView != null)
			{
				UnsubscribeToGridEvents();

				// Update the fonts in case a custom field's name
				// has changed (since each field has it's own font).
				if (m_owningTabGroup != null && m_owningTabGroup.RecordView != null)
					m_owningTabGroup.RecordView.UpdateFonts();

				m_resultView.RefreshResults();
				SubscribeToGridEvents();
				UpdateRecordView();
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SubscribeToGridEvents()
		{
			if (m_resultView == null)
				return;

			if (m_owningTabGroup.TMAdapter != null)
			{
				m_owningTabGroup.TMAdapter.SetContextMenuForControl(
					m_resultView, "cmnuSearchResultTab");
			}

			if (m_resultView.Grid != null)
			{
				if (m_owningTabGroup.TMAdapter != null)
				{
					m_owningTabGroup.TMAdapter.SetContextMenuForControl(
						m_resultView.Grid, "cmnuSearchResultTab");
				}

				m_resultView.Grid.AllowDrop = true;
				m_resultView.Grid.DragOver += HandleResultViewDragOver;
				m_resultView.Grid.DragDrop += HandleResultViewDragDrop;
				m_resultView.Grid.DragLeave += HandleResultViewDragLeave;
				m_resultView.Grid.RowEnter += HandleResultViewRowEnter;
				m_resultView.Grid.Enter += HandleResultViewEnter;
				m_resultView.Grid.AllowDrop = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UnsubscribeToGridEvents()
		{
			if (m_resultView != null && m_resultView.Grid != null)
			{
				m_resultView.Grid.AllowDrop = false;
				m_resultView.Grid.DragOver -= HandleResultViewDragOver;
				m_resultView.Grid.DragDrop -= HandleResultViewDragDrop;
				m_resultView.Grid.DragLeave -= HandleResultViewDragLeave;
				m_resultView.Grid.RowEnter -= HandleResultViewRowEnter;
				m_resultView.Grid.Enter -= HandleResultViewEnter;
			}
		}

		#region Overridden methods and event handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the current tab is selected when its grid get's focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleResultViewEnter(object sender, EventArgs e)
		{
			if (!m_selected || !m_owningTabGroup.IsCurrent)
				m_owningTabGroup.SelectTab(this, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the record pane with the data source's record for the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleResultViewRowEnter(object sender, DataGridViewCellEventArgs e)
		{
			UpdateRecordView(e.RowIndex);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the record view after a sort has taken place since the grid's RowEnter
		/// event doesn't seem to take care of it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnWordListGridSorted(object args)
		{
			UpdateRecordView();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateRecordView()
		{
			UpdateRecordView(-1);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateRecordView(int rowIndex)
		{
			if (!m_selected || (m_owningTabGroup != null && !m_owningTabGroup.IsCurrent))
				return;

			if (m_owningTabGroup.RecordView == null || m_resultView == null ||
				!m_owningTabGroup.IsCurrent || m_resultView.Grid == null)
			{
				m_owningTabGroup.RecordView.UpdateRecord(null);
			}
			else
			{
				RecordCacheEntry entry = (rowIndex < 0 ? m_resultView.Grid.GetRecord() :
					m_resultView.Grid.GetRecord(rowIndex));

				m_owningTabGroup.RecordView.UpdateRecord(entry);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the CV patterns get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCVPatternsChanged(object args)
		{
			return OnRecordViewOptionsChanged(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the record view when the user changed the order or visibility of fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRecordViewOptionsChanged(object args)
		{
			if (m_selected && m_owningTabGroup.IsCurrent &&
				m_owningTabGroup.RecordView != null &&
				m_resultView != null && m_resultView.Grid != null)
			{
				m_owningTabGroup.RecordView.UpdateRecord(
					m_resultView.Grid.GetRecord(), true);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Treat dragging on a result view grid just like dragging on the tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleResultViewDragOver(object sender, DragEventArgs e)
		{
			m_owningTabGroup.InternalDragOver(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Treat dragging on a result view grid just like dragging on the tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleResultViewDragLeave(object sender, EventArgs e)
		{
			m_owningTabGroup.InternalDragLeave(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Treat dropping on a result view grid just like dropping on the tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleResultViewDragDrop(object sender, DragEventArgs e)
		{
			m_owningTabGroup.InternalDragDrop(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reflects drag over events to the tab's owning group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			m_owningTabGroup.InternalDragOver(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reflects drag leave events to the tab's owning group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);
			m_owningTabGroup.InternalDragLeave(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reflects drag drop events to the tab's owning group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);
			m_owningTabGroup.InternalDragDrop(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				m_mouseDownLocation = e.Location;
			else
			{
				Form frm = FindForm();
				if (!App.IsFormActive(frm))
					frm.Focus();
			}

			base.OnMouseDown(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseUp(MouseEventArgs e)
		{
			m_mouseDownLocation = Point.Empty;
			base.OnMouseUp(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			// This will be empty when the mouse button is not down.
			if (m_mouseDownLocation.IsEmpty)
				return;

			// Begin draging a tab when the mouse is held down
			// and has moved 4 or more pixels in any direction.
			int dx = Math.Abs(m_mouseDownLocation.X - e.X);
			int dy = Math.Abs(m_mouseDownLocation.Y - e.Y);
			if (dx >= 4 || dy >= 4)
			{
				m_mouseDownLocation = Point.Empty;
				DoDragDrop(this, DragDropEffects.Move);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			m_mouseOver = true;
			Invalidate();
			base.OnMouseEnter(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			m_mouseOver = false;
			Invalidate();
			base.OnMouseLeave(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			m_btnCIEOptions.Top = (Height - m_btnCIEOptions.Height) / 2 + 1;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rc = ClientRectangle;
			e.Graphics.FillRectangle(SystemBrushes.Control, rc);

			int topMargin = (m_selected ? 0 : 2);

			// Establish the points that outline the region for the tab outline (which
			// also marks off it's interior).
			Point[] pts = new[] {
				new Point(0, rc.Bottom), new Point(0, rc.Top + topMargin + 3),
				new Point(3, topMargin), new Point(rc.Right - 4, topMargin),
				new Point(rc.Right - 1, rc.Top + topMargin + 3),
				new Point(rc.Right - 1, rc.Bottom)};

			// First, clear the decks with an all white background.
			using (SolidBrush br = new SolidBrush(Color.White))
				e.Graphics.FillPolygon(br, pts);

			if (!m_selected || m_owningTabGroup.IsCurrent)
			{
				using (SolidBrush br = new SolidBrush(BackColor))
					e.Graphics.FillPolygon(br, pts);
			}
			else
			{
				// The tab is the current tab but is not in the current
				// tab group so paint with a gradient background.
				//Color clr1 = Color.FromArgb(120, SystemColors.ControlDark);
				//Color clr2 = Color.FromArgb(150, SystemColors.Control);
				using (LinearGradientBrush br = new LinearGradientBrush(rc,
					m_activeTabInactiveGroupBack1, m_activeTabInactiveGroupBack2, 70))
				{
					e.Graphics.FillPolygon(br, pts);
				}
			}

			e.Graphics.DrawLines(SystemPens.ControlDark, pts);

			if (!m_selected)
			{
				// The tab is not the selected tab, so draw a
				// line across the bottom of the tab.
				e.Graphics.DrawLine(SystemPens.ControlDark,
					0, rc.Bottom - 1, rc.Right, rc.Bottom - 1);
			}

			if (!m_btnCIEOptions.Visible)
				DrawImage(e.Graphics, ref rc);
			else
			{
				rc.X += (kleftImgMargin + m_btnCIEOptions.Width);
				rc.Width -= (kleftImgMargin + m_btnCIEOptions.Width);
			}

			if (!m_selected)
			{
				rc.Y += topMargin;
				rc.Height -= topMargin;
			}

			DrawText(e.Graphics, ref rc);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the tab's image.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawImage(Graphics g, ref Rectangle rc)
		{
			if (m_image != null)
			{
				Rectangle rcImage = new Rectangle();
				rcImage.Size = m_image.Size;
				rcImage.X = rc.Left + kleftImgMargin;
				rcImage.Y = rc.Top + (rc.Height - rcImage.Height) / 2;
				g.DrawImage(m_image, rcImage);
				rc.X += (kleftImgMargin + rcImage.Width);
				rc.Width -= (kleftImgMargin + rcImage.Width);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the tab's text
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawText(Graphics g, ref Rectangle rc)
		{
			TextFormatFlags flags = TextFormatFlags.VerticalCenter |
				TextFormatFlags.WordEllipsis | TextFormatFlags.SingleLine |
				TextFormatFlags.NoPadding | TextFormatFlags.LeftAndRightPadding |
				TextFormatFlags.PreserveGraphicsClipping;

			if (m_image == null)
				flags |= TextFormatFlags.HorizontalCenter;

			rc.Height -= 3;
			TextRenderer.DrawText(g, Text, Font, rc, ForeColor, flags);

			if (m_mouseOver)
			{
				// Draw the lines that only show when the mouse is over the tab.
				using (Pen pen = new Pen(Color.DarkOrange))
				{
					int topLine = (m_selected ? 1 : 3);
					g.DrawLine(pen, 3, topLine, rc.Right - 4, topLine);
					g.DrawLine(pen, 2, topLine + 1, rc.Right - 3, topLine + 1);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Text;
		}

		#endregion

		#region Minimal pair (i.e. CIE) options drop-down related methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ShowCIEOptions()
		{
			if (m_btnCIEOptions.Visible)
				m_owningTabGroup.ShowCIEOptions(m_btnCIEOptions);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_btnCIEOptions_Click(object sender, EventArgs e)
		{
			if (!m_selected || !m_owningTabGroup.IsCurrent)
				m_owningTabGroup.SelectTab(this, true);

			ShowCIEOptions();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_btnCIEOptions_MouseLeave(object sender, EventArgs e)
		{
			m_CIEButtonToolTip.Hide(this);
			m_CIEButtonToolTip.Dispose();
			m_CIEButtonToolTip = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_btnCIEOptions_MouseEnter(object sender, EventArgs e)
		{
			m_CIEButtonToolTip = new ToolTip();
			Point pt = PointToClient(MousePosition);
			pt.Y += (Cursor.Size.Height - (int)(Cursor.Size.Height * 0.3));

			var text = LocalizationManager.LocalizeString("SearchResultTabs.MinimalPairsButtonToolTipText",
				"Minimal Pairs Options (Ctrl+Alt+M)", App.kLocalizationGroupMisc);
			
			m_CIEButtonToolTip.Show(text, this, pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ToggleCIEView()
		{
			if (m_resultView != null && m_resultView.Grid != null && m_resultView.Grid.Cache != null)
			{
				if (m_resultView.Grid.Cache.IsCIEList)
					m_resultView.Grid.CIEViewOff();
				else
					m_resultView.Grid.CIEViewOn();

				// Force users to restart Find when toggling the CIEView
				FindInfo.CanFindAgain = false;

				m_btnCIEOptions.Visible = m_resultView.Grid.Cache.IsCIEList;
				AdjustWidth();
				m_owningTabGroup.AdjustTabContainerWidth();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void CIEViewRefresh()
		{
			if (m_resultView.Grid == null || !m_resultView.Grid.CIEViewRefresh())
			{
				m_btnCIEOptions.Visible = false;
				AdjustWidth();
				m_owningTabGroup.AdjustTabContainerWidth();
			}
		}

		#endregion

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
