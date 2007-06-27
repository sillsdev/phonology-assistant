using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Resources;
using SIL.Pa.FFSearchEngine;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	public partial class SearchResultView : UserControl
	{
		private SearchQuery m_searchQuery;
		private PaWordListGrid m_grid;
		private ITMAdapter m_tmAdapter;
		private Type m_owningViewType;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultView(Type owningViewType, ITMAdapter tmAdapter)
		{
			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			m_owningViewType = owningViewType;
			m_tmAdapter = tmAdapter;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a wordEntry list grid with the specified cache and adds it to the form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Initialize(WordListCache cache)
		{
			m_searchQuery = cache.SearchQuery;

			if (cache == null || cache.Count == 0)
			{
				if (m_grid != null)
				{
					Controls.Remove(m_grid);
					m_grid.Dispose();
					m_grid = null;
				}

				return;
			}

			// Save the grid we're replacing.
			PaWordListGrid tmpgrid = m_grid;

			m_grid = new PaWordListGrid(cache, m_owningViewType);
			m_grid.OwningViewType = m_owningViewType;
			m_grid.TMAdapter = m_tmAdapter;

			// Even though the grid is docked, setting it's size here prevents the user from
			// seeing it during that split second during which the grid goes from it's small,
			// default size to its docked size.
			m_grid.Size = Size;

			m_grid.Name = Name + "Grid";
			m_grid.LoadSettings();
			m_grid.Visible = false;
			Controls.Add(m_grid);
			m_grid.Visible = true;

			// I wait until the new grid is all done building and loading before
			// removing the old so the user cannot see the painting of the new one.
			if (tmpgrid != null)
			{
				Controls.Remove(tmpgrid);
				tmpgrid.Dispose();
			}

			Disposed += new EventHandler(SearchResultView_Disposed);
			m_grid.UseWaitCursor = false;
			m_grid.Cursor = Cursors.Default;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For some reason, this is safer to do in a Disposed delegate than in an override
		/// of the Dispose method. Putting this in an override of Dispose sometimes throws
		/// a "Parameter is not valid" exception.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void SearchResultView_Disposed(object sender, EventArgs e)
		{
			Disposed -= SearchResultView_Disposed;
			
			if (m_grid != null)
			{
				m_grid.Dispose();
				m_grid = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Refreshes the search results by performing the search again and rebuilding the
		/// grid contents.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshResults()
		{
			int savCurrRowIndex = 0;
			int savCurrColIndex = 0;
			int savFirstRowIndex = 0;
			SortOptions savSortOptions = null;

			if (m_grid != null)
			{
				// Save the index of the row that's current and the index of the first visible row.
				savCurrRowIndex = (m_grid.CurrentRow != null ? m_grid.CurrentRow.Index : 0);
				savCurrColIndex = (m_grid.CurrentCell != null ? m_grid.CurrentCell.ColumnIndex : 0);
				savFirstRowIndex = m_grid.FirstDisplayedScrollingRowIndex;
				// Save the current sort options
				savSortOptions = m_grid.SortOptions;
			}

			PaApp.InitializeProgressBar(ResourceHelper.GetString("kstidQuerySearchingMsg"));
			WordListCache resultCache = PaApp.Search(m_searchQuery, 5);
			if (resultCache != null)
			{
				resultCache.SearchQuery = m_searchQuery;
				Initialize(resultCache);
			}
			
			// Restore the current row to what it was before rebuilding.
			// Then make sure the row is visible.
			if (m_grid != null)
				m_grid.PostDataSourceModifiedRestore(
					savCurrRowIndex, savCurrColIndex, savFirstRowIndex, savSortOptions);

			PaApp.UninitializeProgressBar();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);

			if (m_grid != null)
				m_grid.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Redraw the watermark image so it's in the bottom right corner.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			if (m_grid == null)
				Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When there's no grid, which means no matches were found for the query, then
		/// display a message in the middle of the view's client area.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (m_grid != null)
				return;

			TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
				TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix |
				TextFormatFlags.WordBreak | TextFormatFlags.VerticalCenter;

			using (Font fnt = FontHelper.MakeFont(SystemInformation.MenuFont, 10, FontStyle.Bold))
			{
				TextRenderer.DrawText(e.Graphics,
					Properties.Resources.kstidNoSearchResultsFoundMsg, fnt,	ClientRectangle,
					ForeColor, flags);
			}

			PaApp.DrawWatermarkImage("kimidSearchWatermark", e.Graphics, ClientRectangle);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ITMAdapter TMAdapter
		{
			get { return m_tmAdapter; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the result view's word list cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListCache Cache
		{
			get { return (m_grid != null ? m_grid.Cache : null); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the result view's grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaWordListGrid Grid
		{
			get { return m_grid; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the result view's search query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery SearchQuery
		{
			get { return m_searchQuery; }
		}
		
		#endregion
	}
}
