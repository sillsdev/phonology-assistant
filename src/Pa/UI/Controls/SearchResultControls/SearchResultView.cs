using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public partial class SearchResultView : UserControl
	{
		private SearchQuery _query;
		private PaWordListGrid _grid;
		private ITMAdapter _tmAdapter;
		private readonly Type _owningViewType;
		private SearchQueryValidationErrorControl _errorControl;

		/// ------------------------------------------------------------------------------------
		public SearchResultView(Type owningViewType, ITMAdapter tmAdapter)
		{
			InitializeComponent();
			base.DoubleBuffered = true;
			base.Dock = DockStyle.Fill;
			_owningViewType = owningViewType;
			_tmAdapter = tmAdapter;
			Disposed += SearchResultView_Disposed;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a wordEntry list grid with the specified cache and adds it to the form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Initialize(WordListCache cache)
		{
			App.MsgMediator.SendMessage("BeforeSearchResultViewInitialized", this);
			Controls.Clear();

			_query = (cache != null ? cache.SearchQuery : null);

			if (cache == null || cache.Count == 0)
			{
				if (_grid != null)
				{
					_grid.Dispose();
					_grid = null;
				}

				if (_query != null && _query.Errors.Count > 0)
					SetupErrorDisplay(_query);
				
				return;
			}

			// Save the grid we're replacing.
			var tmpgrid = _grid;

			_grid = new PaWordListGrid(cache, _owningViewType);
			_grid.OwningViewType = _owningViewType;
			_grid.TMAdapter = _tmAdapter;

			// Even though the grid is docked, setting it's size here prevents the user from
			// seeing it during that split second during which the grid goes from it's small,
			// default size to its docked size.
			_grid.Size = Size;

			_grid.Name = Name + "Grid";
			_grid.LoadSettings();
			_grid.Visible = false;
			Controls.Add(_grid);
			_grid.Visible = true;

			// I wait until the new grid is all done building and loading before
			// removing the old so the user cannot see the painting of the new one.
			if (tmpgrid != null)
			{
				Controls.Remove(tmpgrid);
				tmpgrid.Dispose();
			}

			_grid.UseWaitCursor = false;
			_grid.Cursor = Cursors.Default;

			App.MsgMediator.SendMessage("AfterSearchResultViewInitialized", this);
		}

		/// ------------------------------------------------------------------------------------
		private void SetupErrorDisplay(SearchQuery query)
		{
			_errorControl = new SearchQueryValidationErrorControl(query.Pattern, query.Errors, false)
			{
				Dock = DockStyle.Fill,
			};

			Controls.Add(_errorControl);
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
			
			if (_grid != null)
			{
				_grid.Dispose();
				_grid = null;
			}

			if (_errorControl != null)
			{
				_errorControl.Dispose();
				_errorControl = null;
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
			CIEOptions savCIEOptions = null;

			if (_errorControl != null)
			{
				Controls.Remove(_errorControl);
				_errorControl.Dispose();
				_errorControl = null;
			}

			if (_grid != null)
			{
				// Save the index of the row and column that's current, the
				// index of the first visible row, and the current sort options.
				savCurrRowIndex = (_grid.CurrentRow != null ? _grid.CurrentRow.Index : 0);
				savCurrColIndex = (_grid.CurrentCell != null ? _grid.CurrentCell.ColumnIndex : 0);
				savFirstRowIndex = _grid.FirstDisplayedScrollingRowIndex;
				savSortOptions = _grid.SortOptions;
				savCIEOptions = _grid.CIEOptions;
			}

			App.InitializeProgressBar(App.kstidQuerySearchingMsg);
			var resultCache = App.Search(_query);
			if (resultCache != null)
			{
				resultCache.SearchQuery = _query;
				Initialize(resultCache);
			}
			
			// Restore the current row to what it was before
			// rebuilding. Then make sure the row is visible.
			if (_grid != null)
			{
				_grid.PostDataSourceModifiedRestore(savCurrRowIndex,
					savCurrColIndex, savFirstRowIndex, savSortOptions, savCIEOptions);
			}

			App.UninitializeProgressBar();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			App.MsgMediator.SendMessage("SearchResultViewDestroying", this);

			base.OnHandleDestroyed(e);

			if (_grid != null)
				_grid.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Redraw the watermark image so it's in the bottom right corner.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			if (_grid == null)
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

			if (_grid != null || (_query != null && _query.Errors.Count > 0))
				return;

			const TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
				TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix |
				TextFormatFlags.WordBreak | TextFormatFlags.VerticalCenter |
				TextFormatFlags.PreserveGraphicsClipping;

			using (var fnt = FontHelper.MakeFont(SystemInformation.MenuFont, 10, FontStyle.Bold))
			{
				var msg = App.GetString("SearchResultView.NoSearchResultsFoundMsg", "No Results Found.",
					"Displayed in the search results area when no matches were found.");
				
				TextRenderer.DrawText(e.Graphics, msg, fnt, ClientRectangle, ForeColor, flags);
			}

			App.DrawWatermarkImage("kimidSearchWatermark", e.Graphics, ClientRectangle);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public ITMAdapter TMAdapter
		{
			get { return _tmAdapter; }
			set
			{
				_tmAdapter = value;
				if (_grid != null)
					_grid.TMAdapter = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public WordListCache Cache
		{
			get { return (_grid != null ? _grid.Cache : null); }
		}

		/// ------------------------------------------------------------------------------------
		public PaWordListGrid Grid
		{
			get { return _grid; }
		}

		/// ------------------------------------------------------------------------------------
		public SearchQuery SearchQuery
		{
			get { return _query; }
		}
		
		#endregion
	}
}
