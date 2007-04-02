using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Reflection;
using System.IO;
using SIL.Pa.Resources;
using SIL.Pa.Controls;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;
using SIL.SpeechTools.AudioUtils;
using SIL.FieldWorks.Common.UIAdapters;
using XCore;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaWordListGrid : DataGridView, IxCoreColleague
	{
		private const int kPopupHeadingVPadding = 15;
		private const int kPopupHeadingBodyGap = 5;
		private const int kPopupGapBetweenItems = 10;
		private const int kPopupBottomMargin = 10;
		private const int kPopupSidePadding = 30;

		private GridCellInfoPopup m_cellInfoPopup;
		private static bool s_showTopRightHotState = false;
		private static bool s_showBottomRightHotState = false;
		private WordListCache m_cache = null;
		private WordListCache m_backupCache = null;
		private SortOptions m_sortOptions = null;
		private CIEOptions m_cieOptions = null;
		private Type m_owningViewType = null;
		private string m_phoneticColName;
		private string m_audioFileFieldName;
		private string m_audioFileOffsetFieldName;
		private string m_audioFileLengthFieldName;
		private WordCacheEntry m_currPaintingCellEntry = null;
		private bool m_currPaintingCellSelected = false;
		internal bool m_suspendSavingColumnChanges = false;
		private int m_defaultRowHeight = 0;
		private Dictionary<int, int> m_customRowHeights;
		private ITMAdapter m_tmAdapter;
		private bool m_paintWaterMark = false;
		private bool m_playbackInProgress = false;
		private bool m_playbackAborted;
		private int m_currPlaybackRow = -2;
		private Bitmap m_spkrImage;
		private AudioPlayer m_audioPlayer;
		private int m_playbackSpeed;
		private bool m_isCurrentPlaybackGrid = false;
		private string m_dataSourcePathFieldName;
		private bool m_groupOnSortedField = false;

		#region Constructors
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new DataGridView using the specified query source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaWordListGrid(WordListCache cache) : this(cache, null)
		{
			Cursor = Cursors.Default;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new DataGridView using the specified cache as a data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaWordListGrid(WordListCache cache, Type owningViewType) :
			this(cache, owningViewType, true)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new DataGridView using the specified cache as a data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaWordListGrid(WordListCache cache, Type owningViewType, bool performInitialSort)
			: this()
		{
			Cache = cache;
			m_owningViewType = owningViewType;

			m_audioFileFieldName = PaApp.Project.FieldInfo.AudioFileField.FieldName;
			m_audioFileOffsetFieldName = PaApp.Project.FieldInfo.AudioFileOffsetField.FieldName;
			m_audioFileLengthFieldName = PaApp.Project.FieldInfo.AudioFileLengthField.FieldName;
			m_spkrImage = Properties.Resources.kimidPlaybackSpkr;
			OnSortingOptionsChanged(performInitialSort);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new DataGridView used for find phone results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaWordListGrid()	: base()
		{
			DoubleBuffered = true;
			ReadOnly = true;
			AllowUserToAddRows = false;
			AllowUserToDeleteRows = false;
			AllowUserToOrderColumns = true;
			AutoGenerateColumns = false;
			ShowCellToolTips = false;
			Dock = DockStyle.Fill;
			Font = FontHelper.UIFont;
			ForeColor = SystemColors.WindowText;
			BackgroundColor = SystemColors.Window;
			BorderStyle = BorderStyle.Fixed3D;
			SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			VirtualMode = true;
			RowHeadersWidth = 25;
			RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			GridColor = ColorHelper.CalculateColor(SystemColors.Window, SystemColors.GrayText, 100);
			RowsDefaultCellStyle.SelectionForeColor = SystemColors.WindowText;
			RowsDefaultCellStyle.SelectionBackColor = ColorHelper.LightHighlight;
			BuildColumns();
			OnWordListOptionsChanged(null);
			PaApp.AddMediatorColleague(this);

			// Create the popup for the uncertain phone possibilities
			// list and the experimental transcriptions list.
			m_cellInfoPopup = new GridCellInfoPopup();
			m_cellInfoPopup.AssociatedGrid = this;
			m_cellInfoPopup.HeadingPanel.Font = new Font(FontHelper.PhoneticFont, FontStyle.Bold);
			m_cellInfoPopup.Paint += new PaintEventHandler(m_cellInfoPopup_Paint);
			m_cellInfoPopup.CommandLink.Click += new EventHandler(PopupsCommandLink_Click);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the columns to the grid based on the collection of PA fields in the current
		/// project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildColumns()
		{
			m_suspendSavingColumnChanges = true;

			foreach (PaFieldInfo fieldInfo in PaApp.Project.FieldInfo)
			{
				if (fieldInfo.DisplayIndexInGrid >= 0)
					AddNewColumn(fieldInfo);
			}

			RefreshColumnFonts(false);
			m_suspendSavingColumnChanges = false;
			Disposed += new EventHandler(PaWordListGrid_Disposed);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new column to the grid using the information from the specified PaFieldInfo
		/// object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private DataGridViewColumn AddNewColumn(PaFieldInfo fieldInfo)
		{
			DataGridViewColumn col = SilGrid.CreateTextBoxColumn(fieldInfo.FieldName);
			col.HeaderText = fieldInfo.DisplayText;
			col.DataPropertyName = fieldInfo.FieldName;
			col.DefaultCellStyle.Font = fieldInfo.Font;
			col.SortMode = (fieldInfo.IsPhonetic ?
				DataGridViewColumnSortMode.Programmatic :
				DataGridViewColumnSortMode.Automatic);

			col.DisplayIndex = fieldInfo.DisplayIndexInGrid;
			col.Visible = fieldInfo.VisibleInGrid;
			if (fieldInfo.WidthInGrid > -1)
				col.Width = fieldInfo.WidthInGrid;

			Columns.Add(col);

			// Save this because we'll need it later.
			if (fieldInfo.IsPhonetic)
				m_phoneticColName = fieldInfo.FieldName;

			return col;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For some reason, this is safer to do in a Disposed delegate than in an override
		/// of the Dispose method. Putting this in an override of Dispose sometimes throws
		/// a "Parameter is not valid" exception.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PaWordListGrid_Disposed(object sender, EventArgs e)
		{
			Disposed -= PaWordListGrid_Disposed;

			if (m_cellInfoPopup != null)
				m_cellInfoPopup.Dispose();
		}

		#endregion

		#region Loading and saving settings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load settings for this grid from the settings file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadSettings()
		{
			m_suspendSavingColumnChanges = true;

			if (PaApp.Project.GridLayoutInfo.ColHeaderHeight < 10)
				AutoResizeColumnHeadersHeight();
			else
				ColumnHeadersHeight = PaApp.Project.GridLayoutInfo.ColHeaderHeight;

			m_suspendSavingColumnChanges = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save grid query to query file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveSettings()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveGridChange()
		{
			if (!m_suspendSavingColumnChanges)
			{
				PaApp.Project.GridLayoutInfo.Save(this);
				PaApp.Project.Save();
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current cache record in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RecordCacheEntry GetRecord()
		{
			return (CurrentRow == null ? null : GetRecord(CurrentRow.Index));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cache record for the specified grid row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RecordCacheEntry GetRecord(int rowIndex)
		{
			WordListCacheEntry entry = GetWordEntry(rowIndex);
			return (entry == null ? null : entry.WordCacheEntry.RecordEntry);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current cache word entry in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListCacheEntry GetWordEntry()
		{
			return (CurrentRow == null ? null : GetWordEntry(CurrentRow.Index));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cache word entry for the specified index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListCacheEntry GetWordEntry(int rowIndex)
		{
			if (m_cache != null && rowIndex >= 0 && rowIndex < RowCount)
			{
				PaCacheGridRow row = Rows[rowIndex] as PaCacheGridRow;
				if (row != null)
				{
					int i = (row.ParentRow == null ? row.CacheEntryIndex :
						row.ParentRow.GetCacheIndex(rowIndex));
					
					if (i >= 0 && i < m_cache.Count)
						return m_cache[i];
				}
			}

			return null;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of the cache entries that can play audio files, represented by
		/// selected grid rows.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortedList<int, WordCacheEntry> GetAudioEntriesInSelectedRows()
		{
			long offset;
			long length;
			SortedList<int, WordCacheEntry> entries = new SortedList<int, WordCacheEntry>();

			// Go through the selected rows collection.
			if (SelectedRows != null && SelectedRows.Count > 0)
			{
				foreach (DataGridViewRow row in SelectedRows)
				{
					if (CanRowPlayAudio(row.Index, out offset, out length))
						entries[row.Index] = GetWordEntry(row.Index).WordCacheEntry;
				}
			}

			// Now add the current row if it's not already in the collection.
			int currRow = CurrentCellAddress.Y;
			if (CanRowPlayAudio(currRow, out offset, out length) &&	!entries.ContainsKey(currRow))
				entries[currRow] = GetWordEntry(currRow).WordCacheEntry;

			return (entries.Count == 0 ? null : entries);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the row specified by rowIndex has data to playback an
		/// audio file (or a portion thereof).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool CanRowPlayAudio(int rowIndex, out long offset, out long length)
		{
			offset = -1;
			length = -1;

			WordListCacheEntry wlentry = GetWordEntry(rowIndex);
			if (wlentry == null)
				return false;

			string strVal = wlentry[m_audioFileOffsetFieldName];
			if (!long.TryParse(strVal, out offset))
				return false;

			strVal = wlentry[m_audioFileLengthFieldName];
			if (!long.TryParse(strVal, out length))
				return false;

			string audioFilePath = wlentry[m_audioFileFieldName];
			if (audioFilePath == null)
				return false;

			if (!File.Exists(audioFilePath))
				return AttemptToFindMissingAudioFile(wlentry.WordCacheEntry, audioFilePath);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will determine if the specified audio file path is relative or
		/// absolute. If it's relative, then it is combined with several different absolute
		/// paths in an attempt to find the audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool AttemptToFindMissingAudioFile(WordCacheEntry entry, string audioFilePath)
		{
			// Check if we've already determined an absolute path based on a relative path.
			string absolutePath = entry.AbsoluteAudioFilePath;
			if (!string.IsNullOrEmpty(absolutePath) && File.Exists(absolutePath))
				return true;

			entry.AbsoluteAudioFilePath = null;

			// Check if the path specified in the data source is an absolute path. Do this
			// by checking for a colon in the second character position.
			if (audioFilePath != null && audioFilePath.Length > 0 && audioFilePath[1] == ':')
				return false;

			// Get the name of the data source path field name, if it hasn't already been done.
			// The data source path is only relevant for non FW data sources.
			if (!entry.RecordEntry.DataSource.FwSourceDirectFromDB &&
				m_dataSourcePathFieldName == null)
			{
				PaFieldInfo dataSrcPathFld = PaApp.Project.FieldInfo.DataSourcePathField;
				if (PaApp.Project.FieldInfo.DataSourcePathField != null)
					m_dataSourcePathFieldName = PaApp.Project.FieldInfo.DataSourcePathField.FieldName;
			}

			string newAudioFileName;

			// Check a path relative to the data source file's path.
			if (m_dataSourcePathFieldName != null)
			{
				newAudioFileName = Path.Combine(entry[m_dataSourcePathFieldName], audioFilePath);
				if (File.Exists(newAudioFileName))
				{
					entry.AbsoluteAudioFilePath = newAudioFileName;
					return true;
				}
			}

			// Check a path relative to the project file's path
			newAudioFileName = Path.Combine(PaApp.Project.ProjectPath, audioFilePath);
			if (File.Exists(newAudioFileName))
			{
				entry.AbsoluteAudioFilePath = newAudioFileName;
				return true;
			}

			// Check a path relative to the application's startup path
			newAudioFileName = Path.Combine(Application.StartupPath, audioFilePath);
			if (File.Exists(newAudioFileName))
			{
				entry.AbsoluteAudioFilePath = newAudioFileName;
				return true;
			}

			return false;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the cache associated with the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListCache Cache
		{
			get { return m_cache; }
			set
			{
				m_cache = value;
				RefreshRows(false);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the adapter for the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ITMAdapter TMAdapter
		{
			get { return m_tmAdapter; }
			set { m_tmAdapter = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the type of the view that owns the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Type OwningViewType
		{
			get { return m_owningViewType; }
			set { m_owningViewType = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the grid's border style.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new DataGridViewCellBorderStyle CellBorderStyle
		{
			get { return base.CellBorderStyle; }
			set
			{
				base.CellBorderStyle = value;
				TMItemProperties itemProps = null;

				switch (value)
				{
					case DataGridViewCellBorderStyle.None:
						itemProps = PaApp.TMAdapter.GetItemProperties("tbbGridLinesNone");
						break;
					case DataGridViewCellBorderStyle.SingleHorizontal:
						itemProps = PaApp.TMAdapter.GetItemProperties("tbbGridLinesHorizontal");
						break;
					case DataGridViewCellBorderStyle.SingleVertical:
						itemProps = PaApp.TMAdapter.GetItemProperties("tbbGridLinesVertical");
						break;
					default:
						itemProps = PaApp.TMAdapter.GetItemProperties("tbbGridLinesBoth");
						break;
				}

				if (itemProps != null)
				{
					Image img = itemProps.Image;
					itemProps = PaApp.TMAdapter.GetItemProperties("tbbShowGridLines");
					if (itemProps != null)
					{
						itemProps.Image = img;
						itemProps.Update = true;
						PaApp.TMAdapter.SetItemProperties("tbbShowGridLines", itemProps);
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the PhoneticSortOptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptions SortOptions
		{
			get { return m_sortOptions; }
			set
			{
				if (value == null)
					return;

				m_sortOptions = value;

				// Turn off the glyph on all the columns.
				foreach (DataGridViewColumn col in Columns)
					col.HeaderCell.SortGlyphDirection = SortOrder.None;
				
				// Add the sortGlyph direction
				if (m_sortOptions.SortInformationList.Count > 0)
				{
					string colName = m_sortOptions.SortInformationList[0].FieldInfo.FieldName;

					Columns[colName].HeaderCell.SortGlyphDirection =
						(m_sortOptions.SortInformationList[0].ascending ?
						SortOrder.Ascending : SortOrder.Descending);
				}

				m_cache.Sort(m_sortOptions);
				WordListGroupingBuilder.Group(this);

				// Make the grid update its display
				Invalidate();
				PaApp.MsgMediator.SendMessage("WordListGridSorted", this);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the word list grid is grouped
		/// by the primary sort field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool GroupOnSortedField
		{
			get { return m_groupOnSortedField; }
			set
			{
				if (m_groupOnSortedField == value)
					return;

				// Make sure all groups are expanded before ungrouping. This will prevent
				// rows in collapsed groups from remaining invisible after the ungrouping
				// process.
				if (!value)
					ToggleGroupExpansion(true);
				
				m_groupOnSortedField = value;
				if (m_groupOnSortedField)
					WordListGroupingBuilder.Group(this);
				else
					WordListGroupingBuilder.UnGroup(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The minimal pairs options for this grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEOptions CIEOptions
		{
			get { return m_cieOptions; }
			set { m_cieOptions = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the grid should be considered
		/// the one whose entries will be played back (or stopped) when the user clicks
		/// on a playback button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsCurrentPlaybackGrid
		{
			get { return m_isCurrentPlaybackGrid; }
			set { m_isCurrentPlaybackGrid = value; }
		}

		#endregion

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// In order to achieve double buffering without the problem that arises from having
		/// double buffering on while sizing rows and columns or dragging columns around,
		/// monitor when the mouse goes down and turn off double buffering when it goes down
		/// on a column heading or over the dividers between rows or the dividers between
		/// columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex == -1 ||	(e.ColumnIndex == -1 && Cursor == Cursors.SizeNS))
				DoubleBuffered = false;

			base.OnCellMouseDown(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When double buffering is off, it means it was turned off in the cell mouse down
		/// event. Therefore, turn it back on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseUp(DataGridViewCellMouseEventArgs e)
		{
			if (!DoubleBuffered)
				DoubleBuffered = true;

			base.OnCellMouseUp(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the status bar accordingly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnLeave(EventArgs e)
		{
			PaApp.StatusBarLabel.Text = string.Empty;
			base.OnLeave(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the status bar accordingly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			SetStatusBarText();
			base.OnEnter(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnRowEnter(DataGridViewCellEventArgs e)
		{
			SetStatusBarText(e.RowIndex);
			base.OnRowEnter(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the status bar text with the row information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetStatusBarText()
		{
			SetStatusBarText(CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the status bar text with the row information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetStatusBarText(ToolStripStatusLabel sblbl)
		{
			SetStatusBarText(CurrentCellAddress.Y, sblbl);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the status bar text with the row information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetStatusBarText(int rowIndex)
		{
			ToolStripStatusLabel sblbl = PaApp.StatusBarLabel;

			Form frm = FindForm();
			if (frm is ITabView && ((ITabView)frm).StatusBar != null)
				sblbl = ((ITabView)frm).StatusBarLabel;

			SetStatusBarText(rowIndex, sblbl);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the status bar text with the row information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetStatusBarText(int rowIndex, ToolStripStatusLabel sblbl)
		{
			if (sblbl == null)
				return;

			if (rowIndex < 0 || rowIndex >= RowCount)
				sblbl.Text = string.Empty;
			else
			{
				PaCacheGridRow row = Rows[rowIndex] as PaCacheGridRow;
				sblbl.Text = (row == null ? string.Empty :
					string.Format(Properties.Resources.kstidWordListStatusBarText,
					row.CacheEntryIndex + 1, m_cache.Count));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the height of the row when it's requested.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnRowHeightInfoNeeded(DataGridViewRowHeightInfoNeededEventArgs e)
		{
			int rowHeight;

			e.Height = (m_customRowHeights != null &&
				m_customRowHeights.TryGetValue(e.RowIndex, out rowHeight) ?	rowHeight : m_defaultRowHeight);

			base.OnRowHeightInfoNeeded(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the custom height just set for the specified row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnRowHeightInfoPushed(DataGridViewRowHeightInfoPushedEventArgs e)
		{
			if (m_customRowHeights == null)
				m_customRowHeights = new Dictionary<int, int>();

			m_customRowHeights[e.RowIndex] = e.Height;
			base.OnRowHeightInfoPushed(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
		{
			base.OnCellValueNeeded(e);
			WordListCacheEntry entry = GetWordEntry(e.RowIndex);
			e.Value = (entry == null ? null : entry[Columns[e.ColumnIndex].DataPropertyName]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Play nice and clean up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			PaApp.RemoveMediatorColleague(this);
			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the cells get formatted with the proper font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
		{
			if (e.CellStyle.Font != Columns[e.ColumnIndex].DefaultCellStyle.Font)
				e.CellStyle.Font = Columns[e.ColumnIndex].DefaultCellStyle.Font;

			// Set the selected cell's background color to be distinct from
			if (CurrentCell != null && CurrentCell.RowIndex == e.RowIndex &&
				CurrentCell.ColumnIndex == e.ColumnIndex)
			{
				e.CellStyle.SelectionBackColor = ColorHelper.LightLightHighlight;
			}

			base.OnCellFormatting(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sort the cache based on the column clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
		{
			if (m_cache != null)
			{
				string colName = Columns[e.ColumnIndex].Name;

				if (e.Button == MouseButtons.Left)
					Sort(colName, true); // Sort using the SortOptions and the column clicked
				else if (colName == m_phoneticColName)
				{
					if (!Focused)
					{
						Focus();
						Application.DoEvents();
					}

					// Display the phonetic sort popup at the bottom left of the column header.
					Rectangle rc = GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
					Point pt = new Point(rc.X, rc.Bottom);
					pt = PointToScreen(pt);
					m_tmAdapter.PopupMenu("tbbPhoneticSort", pt.X, pt.Y);
				}
			}

			base.OnColumnHeaderMouseClick(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnColumnDisplayIndexChanged(DataGridViewColumnEventArgs e)
		{
			base.OnColumnDisplayIndexChanged(e);
			SaveGridChange();
			// Force users to restart Find when moving column positions
			FindInfo.CanFindAgain = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
		{
			base.OnColumnWidthChanged(e);
			SaveGridChange();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnColumnHeadersHeightChanged(EventArgs e)
		{
			base.OnColumnHeadersHeightChanged(e);
			SaveGridChange();
		}

		#endregion

		#region Methods for handling the uncertain phones and experimental transcription popup lists.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the user moves over a phonetic cell whose cache entry contains uncertain
		/// phones a list of words is popped-up to the right of of the cell. The words are
		/// all the possibilties given the specified uncertain phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
		{
			base.OnCellMouseEnter(e);

			if (PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) &&
				e.ColumnIndex >= 0 && e.RowIndex >= 0 && m_cache != null &&
				Columns[e.ColumnIndex].Name == m_phoneticColName)
			{
				WordListCacheEntry wlEntry = GetWordEntry(e.RowIndex);
				if (wlEntry == null)
					return;

				WordCacheEntry entry = wlEntry.WordCacheEntry;

				// Don't popup either of the popups in this method if the cell has
				// both indicators. In that case the popup is handled in OnCellMouseMove.
				if (entry.AppliedExperimentalTranscriptions != null && entry.ContiansUncertainties)
					return;

				if (entry.ContiansUncertainties)
					ShowUncertainDataPopup(e.RowIndex, e.ColumnIndex);
				else if (entry.AppliedExperimentalTranscriptions != null)
					ShowExperimentTranscriptionsPopup(e.RowIndex, e.ColumnIndex);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Hide the pop-up if it's showing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseLeave(DataGridViewCellEventArgs e)
		{
			base.OnCellMouseLeave(e);
			ClearIndicatorHotspot(e.RowIndex, e.ColumnIndex);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the cell the mouse is moving over contains an entry that has both uncertain
		/// phones and experimental transcriptions, then those popups will popup only when the
		/// mouse moves over certain portions of the cell. When the mouse is over the top right
		/// corner, then the list of all uncertain possibilities is shown. When the mosue is
		/// over the bottom right corner of the cell, the list of experimental transcription
		/// conversions is shown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseMove(DataGridViewCellMouseEventArgs e)
		{
			base.OnCellMouseMove(e);
			Rectangle rc = GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) ||
				e.ColumnIndex < 0 || e.RowIndex < 0 || m_cache == null ||
				Columns[e.ColumnIndex].Name != m_phoneticColName)
			{
				s_showTopRightHotState = false;
				s_showBottomRightHotState = false;
				return;
			}

			WordListCacheEntry wlentry = GetWordEntry(e.RowIndex);
			if (wlentry == null)
				return;
	
			WordCacheEntry entry = wlentry.WordCacheEntry;

			// If the entry doesn't have both indicators then don't show either
			// popup in this method. It's done in the OnCellMouseEnter method.
			if (!entry.ContiansUncertainties || entry.AppliedExperimentalTranscriptions == null)
			{
				s_showTopRightHotState = false;
				s_showBottomRightHotState = false;
				return;
			}

			if (IsMouseOverUncertainDataPopupHotSpot(e.RowIndex, e.ColumnIndex, e.Location))
				HandleMouseOverUncertainDataPopupHotSpot(e.RowIndex, e.ColumnIndex, entry);
			else if (IsMouseOverExperimentalTransPopupHotSpot(e.RowIndex, e.ColumnIndex, e.Location))
				HandleMouseOverExperimentalTransPopupHotSpot(e.RowIndex, e.ColumnIndex, entry);
			else
			{
				ClearIndicatorHotspot(e.RowIndex, e.ColumnIndex);

				// The mouse isn't over either corner so make sure the popups aren't showing.
				if (!m_cellInfoPopup.IsMouseOver)
					m_cellInfoPopup.Hide();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Processes the mouse moving over the hotspot area that triggers displaying the
		/// uncertain phone possibility list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleMouseOverUncertainDataPopupHotSpot(int row, int col, WordCacheEntry entry)
		{
			if (!s_showTopRightHotState || s_showBottomRightHotState)
			{
				s_showTopRightHotState = true;
				s_showBottomRightHotState = false;
				InvalidateCell(col, row);
			}

			// If the popup is active and it's for experimental transcriptions, then hide it.
			m_cellInfoPopup.Hide(GridCellInfoPopup.Purpose.ExperimentalTranscription);

			// The mouse is over the top right so, if it's not already shown,
			// show the uncertainty popup.
			if (!m_cellInfoPopup.Active(GridCellInfoPopup.Purpose.UncertainPossibilities) &&
				entry.ContiansUncertainties)
			{
				ShowUncertainDataPopup(row, col);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Processes the mouse moving over the hotspot area that triggers displaying the
		/// experimental transcriptions list .
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleMouseOverExperimentalTransPopupHotSpot(int row, int col, WordCacheEntry entry)
		{
			if (s_showTopRightHotState || !s_showBottomRightHotState)
			{
				s_showTopRightHotState = false;
				s_showBottomRightHotState = true;
				InvalidateCell(col, row);
			}

			// If the popup is active and it's for uncertain items, then hide it.
			m_cellInfoPopup.Hide(GridCellInfoPopup.Purpose.UncertainPossibilities);

			// The mouse is over the bottom right so, if it's not already shown,
			// show the experimental transcriptions popup.
			if (!m_cellInfoPopup.Active(GridCellInfoPopup.Purpose.ExperimentalTranscription)
				&& entry.AppliedExperimentalTranscriptions != null)
			{
				ShowExperimentTranscriptionsPopup(row, col);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the mouse is over the top right corner of the cell
		/// specified by row and col.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsMouseOverUncertainDataPopupHotSpot(int row, int col, Point mouseLocation)
		{
			Rectangle rc = GetCellDisplayRectangle(col, row, false);

			// Because the mouse's location is relative to the cell (not the entire
			// grid), we need to adjust the origin of the cell's rectangle. 
			rc.X = rc.Y = 0;

			int rightEdge = rc.Right - 1;
			rc.Height /= 2;
			rc.Width = (int)((float)rc.Height * 1.5);
			rc.X = rightEdge - rc.Width;
			return rc.Contains(mouseLocation);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the mouse is over the bottom right corner of the cell
		/// specified by row and col.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsMouseOverExperimentalTransPopupHotSpot(int row, int col, Point mouseLocation)
		{
			Rectangle rc = GetCellDisplayRectangle(col, row, false);

			// Because the mouse's location is relative to the cell (not the entire
			// grid), we need to adjust the origin of the cell's rectangle. 
			rc.X = rc.Y = 0;

			int rightEdge = rc.Right - 1;
			int bottomEdge = rc.Bottom - 1;
			rc.Height /= 2;
			rc.Width = (int)((float)rc.Height * 1.5);
			rc.X = rightEdge - rc.Width;
			rc.Y = bottomEdge = rc.Height;
			return rc.Contains(mouseLocation);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the specified popup at using a cell's row and column coordinates to
		/// determine where the popup should be shown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ShowUncertainDataPopup(int row, int col)
		{
			s_showTopRightHotState = true;
			s_showBottomRightHotState = false;
			InvalidateCell(col, row);

			WordListCacheEntry wlentry = GetWordEntry(row);
			if (wlentry == null)
				return;

			WordCacheEntry entry = wlentry.WordCacheEntry;
			if (entry == null)
				return;

			string[][] possibleWords = entry.GetAllPossibleUncertainWords(true);
			if (possibleWords == null)
				return;
			
			int hdgWidth;
			using (Font fnt = new Font(FontHelper.PhoneticFont, FontStyle.Bold))
				hdgWidth = m_cellInfoPopup.SetHeadingText(entry.PhoneticValue, fnt);

			m_cellInfoPopup.PurposeIndicator = GridCellInfoPopup.Purpose.UncertainPossibilities;
			m_cellInfoPopup.CacheEntry = entry;
			m_cellInfoPopup.MeasureBodyHeight(FontHelper.PhoneticFont, possibleWords.Length);
			m_cellInfoPopup.Width = hdgWidth + kPopupSidePadding;
			m_cellInfoPopup.AssociatedCell = this[col, row];

			// Calculate a point just to the right and even with the top of
			// the cell to which the popup belongs. Then show the popup there.
			Rectangle rc = GetCellDisplayRectangle(col, row, false);
			Point pt = new Point(rc.Right - 1, rc.Y);
			m_cellInfoPopup.Show(this, pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the specified popup at using a cell's row and column coordinates to
		/// determine where the popup should be shown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ShowExperimentTranscriptionsPopup(int row, int col)
		{
			s_showTopRightHotState = false;
			s_showBottomRightHotState = true;
			InvalidateCell(col, row);

			WordListCacheEntry wlentry = GetWordEntry(row);
			if (wlentry == null)
				return;

			WordCacheEntry entry = wlentry.WordCacheEntry;
			if (entry == null)
				return;

			Dictionary<string, string> experimentalTrans = entry.AppliedExperimentalTranscriptions;
			if (experimentalTrans == null)
				return;

			int widestExperimentalTrans = GetWidestExperimentalTrancription(experimentalTrans);

			int hdgWidth;
			string hdgText = string.Format(Properties.Resources.kstidCellInfoExperimentalTransHdgText, "\n");
			using (Font fnt = new Font(FontHelper.UIFont, FontStyle.Bold))
				hdgWidth = m_cellInfoPopup.SetHeadingText(hdgText, fnt);

			m_cellInfoPopup.CacheEntry = entry;
			m_cellInfoPopup.PurposeIndicator = GridCellInfoPopup.Purpose.ExperimentalTranscription;
			m_cellInfoPopup.MeasureBodyHeight(FontHelper.PhoneticFont, experimentalTrans.Count);
			m_cellInfoPopup.Width = Math.Max(hdgWidth, widestExperimentalTrans) + kPopupSidePadding;
			m_cellInfoPopup.AssociatedCell = this[col, row];
			m_cellInfoPopup.HeadingTextSidePadding = (kPopupSidePadding / 2 - 3);

			m_cellInfoPopup.HeadingTextFomatFlags = TextFormatFlags.NoPrefix |
				TextFormatFlags.VerticalCenter | TextFormatFlags.Left |
				TextFormatFlags.LeftAndRightPadding;

			// Calculate a point just to the right and even with the top of
			// the cell to which the popup belongs. Then show the popup there.
			Rectangle rc = GetCellDisplayRectangle(col, row, false);
			Point pt = new Point(rc.Right - 1, rc.Y);
			m_cellInfoPopup.Show(this, pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Measure how big the experimental transcription list must be.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int GetWidestExperimentalTrancription(Dictionary<string, string> experimentalTrans)
		{
			TextFormatFlags flags = TextFormatFlags.NoPrefix | TextFormatFlags.LeftAndRightPadding;

			// Determine the width of each experimental transcription and the width of the
			// string it was converted to. Determine the maximum of all the widths.
			int maxWidth = 0;
			foreach (KeyValuePair<string, string> item in experimentalTrans)
			{
				int itemWidth = TextRenderer.MeasureText(item.Key,
					FontHelper.PhoneticFont, Size.Empty, flags).Width;

				itemWidth += TextRenderer.MeasureText(item.Value,
					FontHelper.PhoneticFont, Size.Empty, flags).Width;

				maxWidth = Math.Max(itemWidth, maxWidth);
			}

			// Add some for the arrow glyph with some space on either side of it.
			return maxWidth + 15;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_cellInfoPopup_Paint(object sender, PaintEventArgs e)
		{
			if (m_cellInfoPopup.CacheEntry == null)
				return;

			if (m_cellInfoPopup.PurposeIndicator == GridCellInfoPopup.Purpose.UncertainPossibilities)
				DrawUncertaintyPopupList(e.Graphics);
			else
				DrawExperimentalTransPopupList(e.Graphics);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the list of possible words in the uncertainty popup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawUncertaintyPopupList(Graphics g)
		{
			string[][] possibleWords = m_cellInfoPopup.CacheEntry.GetAllPossibleUncertainWords(true);
			if (possibleWords == null)
				return;

			Rectangle rc = m_cellInfoPopup.FirstItemRectangle;

			TextFormatFlags flags = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter |
				TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.NoPadding;

			// Draw each word.
			foreach (string[] word in possibleWords)
			{
				rc.X = 25;

				// Draw each phone in the word
				foreach (string phone in word)
				{
					string ph = phone;
					Color textColor = Color.Black;

					if (ph.StartsWith("|"))
					{
						textColor = PaApp.UncertainPhoneForeColor;
						ph = ph.Substring(1);
					}

					TextRenderer.DrawText(g, ph, FontHelper.PhoneticFont,
						rc, textColor, flags);

					// Figure out where the next phone should be drawn.
					int phoneWidth = TextRenderer.MeasureText(g, ph,
						FontHelper.PhoneticFont, Size.Empty, flags).Width;

					rc.X += phoneWidth;
				}

				rc.Y += rc.Height;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the list of experimental transcription conversions performed on the
		/// cache entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawExperimentalTransPopupList(Graphics g)
		{
			Dictionary<string, string> experimentalTrans =
				m_cellInfoPopup.CacheEntry.AppliedExperimentalTranscriptions;

			if (experimentalTrans == null)
				return;

			TextFormatFlags flags = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter |
				TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.NoPadding |
				TextFormatFlags.LeftAndRightPadding;

			Rectangle rc = m_cellInfoPopup.FirstItemRectangle;
			rc.X += 25;

			foreach (KeyValuePair<string, string> item in experimentalTrans)
			{
				TextRenderer.DrawText(g, item.Key, FontHelper.PhoneticFont, rc,
					Color.Black, flags);

				Size sz = TextRenderer.MeasureText(g, item.Key, FontHelper.PhoneticFont);

				// Draw an arrow that points to what experimental transcription the
				// phone was converted to.
				using (SolidBrush br = new SolidBrush(Color.Black))
				{
					Point pt1 = new Point(rc.X + sz.Width + 12, rc.Y + (sz.Height / 2));
					Point pt2 = new Point(pt1.X - 5, pt1.Y - 5);
					Point pt3 = new Point(pt1.X - 5, pt1.Y + 5);
					g.FillPolygon(br, new Point[] { pt1, pt2, pt3 });
				}

				Rectangle rcTmp = rc;
				rcTmp.X += sz.Width + 15;
				TextRenderer.DrawText(g, item.Value, FontHelper.PhoneticFont, rcTmp,
					Color.Black, flags);

				rc.Y += rc.Height;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ClearIndicatorHotspot(int row, int col)
		{
			if (s_showTopRightHotState || s_showBottomRightHotState)
			{
				s_showTopRightHotState = false;
				s_showBottomRightHotState = false;
				InvalidateCell(col, row);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void PopupsCommandLink_Click(object sender, EventArgs e)
		{
			if (m_cellInfoPopup.PurposeIndicator == GridCellInfoPopup.Purpose.ExperimentalTranscription)
				PaApp.MsgMediator.SendMessage("ViewPhoneInventory", null);
			else if (m_cellInfoPopup.AssociatedCell != null)
				OnEditSourceRecord(m_cellInfoPopup.AssociatedCell.RowIndex);
		}

		#endregion

		#region Watermark handling methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the results in the grid are stale.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AreResultsStale
		{
			get { return m_paintWaterMark; }
			set
			{
				if (m_paintWaterMark != value)
				{
					m_paintWaterMark = value;
					Invalidate(WaterMarkRectangle);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the rectangle in which the watermark is drawn.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Rectangle WaterMarkRectangle
		{
			get
			{
				Rectangle rc = ClientRectangle;
				rc.Width = (int)((float)rc.Width * 0.5f);
				rc.Height = (int)((float)rc.Height * 0.5f);
				rc.X = (ClientRectangle.Width - rc.Width) / 2;
				rc.Y = (ClientRectangle.Height - rc.Height) / 2;
				return rc;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the water mark when the grid changes size.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			bool paintWaterMark = m_paintWaterMark;

			if (m_paintWaterMark)
			{
				// Clear previous water mark.
				m_paintWaterMark = false;
				Invalidate();
			}

			base.OnSizeChanged(e);

			m_paintWaterMark = paintWaterMark;
			if (m_paintWaterMark)
				Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the water mark when the grid scrolls.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnScroll(ScrollEventArgs e)
		{
			bool paintWaterMark = m_paintWaterMark;

			if (m_paintWaterMark)
			{
				// Clear previous water mark.
				m_paintWaterMark = false;
				Invalidate();
			}

			base.OnScroll(e);

			m_paintWaterMark = paintWaterMark;
			if (m_paintWaterMark)
				Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints a water mark when the results are stale (i.e. the query settings have been
		/// changed since the results were shown).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (!m_paintWaterMark)
				return;

			Rectangle rc = WaterMarkRectangle;
			GraphicsPath path = new GraphicsPath();
			FontFamily family = FontFamily.GenericSerif;

			// Find the first font size equal to or smaller than 256 that
			// fits in the water mark rectangle.
			for (int size = 256; size >= 0; size -= 2)
			{
				using (Font fnt = new Font(family, size, FontStyle.Bold))
				{
					int height = TextRenderer.MeasureText("!", fnt).Height;
					if (height < rc.Height)
					{
						using (StringFormat sf = STUtils.GetStringFormat(true))
							path.AddString("!", family, (int)FontStyle.Bold, size, rc, sf);

						break;
					}
				}
			}

			path.AddEllipse(rc);

			using (SolidBrush br = new SolidBrush(Color.FromArgb(35, DefaultCellStyle.ForeColor)))
				e.Graphics.FillRegion(br, new Region(path));
		}

		#endregion

		#region Drawing methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles special painting for the phonetic column when the grid is displaying
		/// find phone results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
		{
			if (e.ColumnIndex < 0 || e.RowIndex < 0 || m_cache == null)
			{
				if (e.ColumnIndex < 0 && e.RowIndex == m_currPlaybackRow)
					DrawRowHeaderForPlayingRow(e);
				else
					base.OnCellPainting(e);

				if (e.RowIndex == -1 && e.ColumnIndex == 0)
					DrawGapFiller(e);

				return;
			}

			WordListCacheEntry wlentry = GetWordEntry(e.RowIndex);
			if (wlentry == null)
			{
				base.OnCellPainting(e);
				return;
			}

			m_currPaintingCellEntry = wlentry.WordCacheEntry;
			m_currPaintingCellSelected =
				((e.State & DataGridViewElementStates.Selected) > 0);

			PaFieldInfo fieldInfo = PaApp.Project.FieldInfo.AudioFileField;
			if (fieldInfo != null && Columns[e.ColumnIndex].Name == fieldInfo.FieldName)
			{
				DrawFilePath(e);
				e.Handled = true;
				return;
			}

			fieldInfo = PaApp.Project.FieldInfo.DataSourcePathField;
			if (fieldInfo != null && Columns[e.ColumnIndex].Name == fieldInfo.FieldName)
			{
				DrawFilePath(e);
				e.Handled = true;
				return;
			}

			if (Columns[e.ColumnIndex].Name == m_phoneticColName)
			{
				if (m_cache.IsForFindPhoneResults)
				{
					DrawPhoneticFindPhoneResult(e);
					e.Handled = true;
					return;
				}

				if (m_currPaintingCellEntry.Phones != null)
				{
					if (m_currPaintingCellEntry.ContiansUncertainties &&
						m_currPaintingCellEntry.UncertainPhones != null)
					{
						DrawPhoneticUncertainty(e);
						e.Handled = true;
						return;
					}

					if (m_currPaintingCellEntry.AppliedExperimentalTranscriptions != null)
					{
						e.Paint(e.CellBounds, DataGridViewPaintParts.All);
						DrawIndicatorCornerGlyphs(e.Graphics, e.CellBounds,
							m_currPaintingCellEntry);
						
						e.Handled = true;
						return;
					}
				}
			}

			base.OnCellPainting(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method fills in the gap between the header of the last visible column and the
		/// right edge of the grid. That gap is filled with something that looks like one long
		/// empty column header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawGapFiller(DataGridViewCellPaintingEventArgs e)
		{
			int colWidths = Columns.GetColumnsWidth(DataGridViewElementStates.Visible);
			if (colWidths > ClientSize.Width)
				return;

			Rectangle rc = e.CellBounds;
			rc.Width = ClientSize.Width;

			VisualStyleElement element = VisualStyleElement.Header.Item.Normal;
			if (PaintingHelper.CanPaintVisualStyle(element))
			{
				if (ColumnHeadersBorderStyle == DataGridViewHeaderBorderStyle.Raised)
					rc.Height--;

				VisualStyleRenderer renderer = new VisualStyleRenderer(element);
				renderer.DrawBackground(e.Graphics, rc);
			}
			else
			{
				// Draw this when themes aren't supported.
				ControlPaint.DrawButton(e.Graphics, rc, ButtonState.Normal);
				ControlPaint.DrawBorder3D(e.Graphics, rc, Border3DStyle.Flat,
					Border3DSide.Bottom);
			}

			if (ColumnHeadersBorderStyle == DataGridViewHeaderBorderStyle.Raised)
			{
				ControlPaint.DrawBorder3D(e.Graphics, rc, Border3DStyle.Etched,
					Border3DSide.Top);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the row header cell for the row currently playing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawRowHeaderForPlayingRow(DataGridViewCellPaintingEventArgs e)
		{
			e.PaintBackground(e.CellBounds, false);

			// Figure out the size and location of the image so it's centered in the cell.
			Rectangle rc = e.CellBounds;
			rc.X += (rc.Width - m_spkrImage.Width) / 2;
			rc.Y += (rc.Height - m_spkrImage.Height) / 2;
			rc.Size = m_spkrImage.Size;
			e.Graphics.DrawImage(m_spkrImage, rc);
			e.Handled = true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints text in a cell using path with path ellipsis formatting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawFilePath(DataGridViewCellPaintingEventArgs e)
		{
			// Draw default everything but text.
			DataGridViewPaintParts parts = DataGridViewPaintParts.All;
			parts &= ~DataGridViewPaintParts.ContentForeground;
			e.Paint(e.CellBounds, parts);

			string soundFilePath = e.Value as string;
			if (string.IsNullOrEmpty(soundFilePath))
				return;

			TextFormatFlags flags = TextFormatFlags.PathEllipsis | TextFormatFlags.NoPrefix |
				TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;

			TextRenderer.DrawText(e.Graphics, soundFilePath, e.CellStyle.Font, e.CellBounds,
				(m_currPaintingCellSelected ? e.CellStyle.SelectionForeColor :
				e.CellStyle.ForeColor), flags);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints the phonetic column when the grid is displaying find phone results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawPhoneticFindPhoneResult(DataGridViewCellPaintingEventArgs e)
		{
			WordListCacheEntry wlentry = GetWordEntry(e.RowIndex);
			if (wlentry == null)
				return;

			Rectangle rc = e.CellBounds;

			// Draw default everything but text.
			DataGridViewPaintParts parts = DataGridViewPaintParts.All;
			parts &= ~DataGridViewPaintParts.ContentForeground;
			e.Paint(rc, parts);

			int srchItemOffset = wlentry.SearchItemPhoneOffset;
			int srchItemLength = wlentry.SearchItemPhoneLength;
			int envAfterOffset = srchItemOffset + srchItemLength;

			// Get the text that makes up the search item.
			// This is used only to measure it's width.
			string srchItem = string.Join(string.Empty, wlentry.Phones,
				srchItemOffset, srchItemLength);

			TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix |
				TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;

			// Calculate the width of the search item.
			int itemWidth = TextRenderer.MeasureText(e.Graphics, srchItem,
				FontHelper.PhoneticFont, Size.Empty, flags).Width;

			// Calculate the center of the cell less half the width of the search item.
			int itemLeft = rc.X + (rc.Width / 2) - (itemWidth / 2);

			// Draw the search item's background, taking into consideration when the
			// cell is selected. In that case, the highlighted background is made a
			// little transparent so the row's selection color shows through.
			Rectangle rcSearchItemBackground = new Rectangle(itemLeft, rc.Y, itemWidth, rc.Height - 1);
			Color backColor = (m_currPaintingCellSelected ?
				Color.FromArgb(90, PaApp.QuerySearchItemBackColor) : PaApp.QuerySearchItemBackColor);
			e.Graphics.FillRectangle(new SolidBrush(backColor), rcSearchItemBackground);

			Color textColor = (m_currPaintingCellSelected ?
				RowsDefaultCellStyle.SelectionForeColor : ForeColor);

			// Draw the phones in the search item.
			rc.X = itemLeft;
			rc.Width = e.CellBounds.Right - itemLeft;
			DrawPhones(e.Graphics, wlentry.Phones, srchItemOffset, envAfterOffset,
				rc, PaApp.QuerySearchItemForeColor, flags, true);

			// Draw the phones in the environment after.
			rc.X = itemLeft + itemWidth;
			rc.Width = e.CellBounds.Right - (itemLeft + itemWidth);
			DrawPhones(e.Graphics, wlentry.Phones, envAfterOffset, wlentry.Phones.Length,
				rc, textColor, flags, true);

			// Draw the phones in the environment before.
			rc.X = e.CellBounds.X;
			rc.Width = itemLeft - e.CellBounds.X;
			DrawPhones(e.Graphics, wlentry.Phones, srchItemOffset - 1, -1,
				rc, textColor, flags, false);

			DrawIndicatorCornerGlyphs(e.Graphics, e.CellBounds, wlentry.WordCacheEntry);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints the phonetic cell when the entry contains uncertain query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawPhoneticUncertainty(DataGridViewCellPaintingEventArgs e)
		{
			Rectangle rc = e.CellBounds;

			// Draw default everything but text.
			DataGridViewPaintParts parts = DataGridViewPaintParts.All;
			parts &= ~DataGridViewPaintParts.ContentForeground;
			e.Paint(rc, parts);

			TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix |
				TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.Left;

			Color textColor = (m_currPaintingCellSelected ?
				RowsDefaultCellStyle.SelectionForeColor : ForeColor);

			rc.X += 4;
			rc.Width -= 4;
			DrawPhones(e.Graphics, m_currPaintingCellEntry.Phones, 0,
				m_currPaintingCellEntry.Phones.Length, rc, textColor, flags, true);

			DrawIndicatorCornerGlyphs(e.Graphics, e.CellBounds, m_currPaintingCellEntry);
		}


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the specified number of phones (via begin and end) in the specified
		/// direction, using the specified color, in the specified rectangle.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawPhones(Graphics g, string[] phones, int begin, int end, Rectangle rc,
			Color textColor, TextFormatFlags flags, bool drawForward)
		{
			if (begin == end)
				return;

			if (!drawForward)
				flags |= TextFormatFlags.Right;

			int i = begin;
			while (i != end)
			{
				Color clr = (m_currPaintingCellEntry.ContiansUncertainties &&
					m_currPaintingCellEntry.UncertainPhones.ContainsKey(i) ?
					PaApp.UncertainPhoneForeColor : textColor);

				TextRenderer.DrawText(g, phones[i], FontHelper.PhoneticFont, rc, clr, flags);

				int phoneWidth = TextRenderer.MeasureText(g, phones[i],
					FontHelper.PhoneticFont, Size.Empty, flags).Width;

				// If the phones are being drawn from L to R (which should always be true unless
				// we're drawing the environment before in a find phone search result grid) then
				// move the left edge of the rectangle forward by the width of the phone just
				// drawn. Otherwise move the right edge back by the width of the phone just drawn.
				rc.Width -= phoneWidth;

				if (!drawForward)
					i--;
				else
				{
					rc.X += phoneWidth;
					i++;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When there are uncertain phones, draw a small red triangle in the top right corner
		/// of the phonetic cells. When there are experimental transcription conversions on an
		/// entry, draw a small green triangle in the bottom right corner of the phonetic cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void DrawIndicatorCornerGlyphs(Graphics g, Rectangle rc,
			WordCacheEntry entry)
		{
			if (entry.Phones == null)
				return;

			// If their are uncertain phones, draw the little red
			// triangle in the top right corner.
			if (entry.ContiansUncertainties)
				DrawUncertaintyCornerGlyph(g, rc, entry);
			
			// If their are experimental transcription conversions, draw the little green
			// triangle in the bottom right corner.
			if (entry.AppliedExperimentalTranscriptions != null)
				DrawExperimentalTransCornerGlyphs(g, rc, entry);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void DrawUncertaintyCornerGlyph(Graphics g, Rectangle rc,
			WordCacheEntry entry)
		{
			Point pt1 = new Point(rc.Right - 7, rc.Y);
			Point pt2 = new Point(rc.Right - 1, rc.Y + 6);
			Point ptCorner = new Point(rc.Right - 1, rc.Top);

			using (LinearGradientBrush br =
				new LinearGradientBrush(pt1, pt2, Color.Red, Color.DarkRed))
			{
				g.FillPolygon(br, new Point[] { pt1, pt2, ptCorner });
			}

			if (s_showTopRightHotState)
			{
				pt2.Y += 9;
				pt1.X -= 10;
				using (SolidBrush sbr = new SolidBrush(Color.FromArgb(100, Color.DarkRed)))
					g.FillPolygon(sbr, new Point[] { pt1, pt2, ptCorner });
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void DrawExperimentalTransCornerGlyphs(Graphics g, Rectangle rc,
			WordCacheEntry entry)
		{
			Point pt1 = new Point(rc.Right - 8, rc.Bottom - 1);
			Point pt2 = new Point(rc.Right - 1, rc.Bottom - 8);
			Point ptCorner = new Point(rc.Right - 1, rc.Bottom - 1);

			using (LinearGradientBrush br =
				new LinearGradientBrush(pt1, pt2, Color.LightGreen, Color.DarkGreen))
			{
				g.FillPolygon(br, new Point[] { pt1, pt2, ptCorner });
			}

			if (s_showBottomRightHotState)
			{
				pt2.Y -= 10;
				pt1.X -= 10;
				using (SolidBrush sbr = new SolidBrush(Color.FromArgb(100, Color.DarkGreen)))
					g.FillPolygon(sbr, new Point[] { pt1, pt2, ptCorner });
			}
		}

		#endregion

		#region Misc. Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When minimal pairs view is off, this will turn it on, otherwise, the existing
		/// minimal pairs cache is just refreshed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CIEViewRefresh()
		{
			if (m_cache.IsCIEList)
			{
				m_cache = m_backupCache;
				m_backupCache = null;

				if (!CIEViewOn())
				{
					m_cache.Sort(m_sortOptions);
					RefreshRows(true);
					return false;
				}
			}

			return true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Turns on showing minimal pairs from the grid's cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CIEViewOn()
		{
			if (m_cache.IsCIEList)
				return false;

			if (m_cieOptions == null)
				m_cieOptions = PaApp.Project.CIEOptions;

			CIEBuilder builder = new CIEBuilder(m_cache, m_sortOptions, m_cieOptions);
			WordListCache cieCache = builder.FindMinimalPairs();

			if (cieCache == null || cieCache.Count <= 1)
			{
				string msg = Properties.Resources.kstidNoMinimalPairsMsg;
				STUtils.STMsgBox(msg, MessageBoxButtons.OK);
				return false;
			}
			else
			{
				m_backupCache = m_cache;
				m_cache = cieCache;
				RefreshRows(true);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Turns off showing minimal pairs from the grid's cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CIEViewOff()
		{
			if (m_backupCache != null)
			{
				m_cache = m_backupCache;
				m_backupCache = null;
				m_cache.Sort(m_sortOptions);
				RefreshRows(true);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears all the grid's rows and rebuilds them from the grid's cache, grouping
		/// records if the grid is currently grouped on the primary sort field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshRows()
		{
			RefreshRows(m_groupOnSortedField);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears all the grid's rows and rebuilds them from the grid's cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshRows(bool groupRecords)
		{
			Rows.Clear();

			if (m_cache == null || m_cache.Count == 0)
				return;

			for (int i = 0; i < m_cache.Count; i++)
			{
				if (!m_cache.IsCIEList)
					m_cache[i].CIEGroupId = -1;

				Rows.Add(new PaCacheGridRow(i));
			}

			if (groupRecords)
				WordListGroupingBuilder.Group(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sort phonetically or based on the column clicked.
		/// </summary>
		/// <param name="colName">string</param>
		/// <param name="changeSortDirection">bool</param>
		/// ------------------------------------------------------------------------------------
		public void Sort(string colName, bool changeSortDirection)
		{
			// Remove the SortGlyph from the previous sort column header
			foreach (DataGridViewColumn col in Columns)
				col.HeaderCell.SortGlyphDirection = SortOrder.None;

			bool ascending = m_cache.Sort(SortOptions, colName, changeSortDirection);

			// Add the sortGlyphDirection to the current column
			Columns[colName].HeaderCell.SortGlyphDirection =
				(ascending ? SortOrder.Ascending : SortOrder.Descending);

			WordListGroupingBuilder.Group(this);
			
			// Make the grid update its display
			Invalidate();
			PaApp.MsgMediator.SendMessage("WordListGridSorted", this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides a way to expand or collapse all groups shown in the grid (when the
		/// primary sort field is not displayed in groups, this method will do nothing).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ToggleGroupExpansion(bool expand)
		{
			if (!m_groupOnSortedField && !m_cache.IsCIEList)
				return;

			Application.UseWaitCursor = true;
			PaApp.InitializeProgressBar(expand ? Properties.Resources.kstidExpandingGroups :
				Properties.Resources.kstidCollapsingGroups, RowCount);

			foreach (DataGridViewRow row in Rows)
			{
				if (row is SilHierarchicalGridRow)
				{
					((SilHierarchicalGridRow)row).SetExpandedState(expand, false);
					PaApp.IncProgressBar(((SilHierarchicalGridRow)row).ChildCount);
				}
			}

			// Force users to restart Find when collapsing all rows
			if (!expand)
				FindInfo.CanFindAgain = false;

			CurrentCell = this[0, 0];
			PaApp.IncProgressBar(RowCount);
			PaApp.UninitializeProgressBar();
			Application.UseWaitCursor = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the font used for the columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshColumnFonts(bool updateColumnFonts)
		{
			TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix |
				TextFormatFlags.SingleLine;

			using (Graphics g = CreateGraphics())
			{
				m_defaultRowHeight = 0;

				foreach (DataGridViewColumn col in Columns)
				{
					if (updateColumnFonts)
					{
						PaFieldInfo fieldInfo = PaApp.Project.FieldInfo[col.Name];
						if (fieldInfo != null)
							col.DefaultCellStyle.Font = fieldInfo.Font;
					}

					int fieldFontHeight = TextRenderer.MeasureText(g,
							"X", col.DefaultCellStyle.Font, Size.Empty, flags).Height;

					m_defaultRowHeight = Math.Max(m_defaultRowHeight, fieldFontHeight);
				}
			}

			m_defaultRowHeight += 4;
			AutoResizeRows(DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Restores the current row, sort options and first visible row to those specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void PostDataSourceModifiedRestore(int savedRowIndex, int savedFirstRowIndex,
			SortOptions savedSortOptions)
		{
			// Restore and apply the saved sort options
			if (savedSortOptions != null)
				SortOptions = savedSortOptions;

			// Restore the first visible row.
			if (Rows.Count > savedFirstRowIndex && savedFirstRowIndex >= 0)
				FirstDisplayedScrollingRowIndex = savedFirstRowIndex;
			
			// Restore the selected row.
			if (Rows.Count > savedRowIndex)
			{
				CurrentCell = this[0, savedRowIndex];
				CurrentRow.Selected = true;
				InvalidateRow(savedRowIndex);
			}
		}

		#endregion

		#region Message handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets called whenever the sort options change from modifications made on the
		/// sorting tab of the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSortingOptionsChanged(object args)
		{
			bool performSort = (args != null && args.GetType() == typeof(bool) && (bool)args);

			SortOptions sortOptions = null;

			// Load the sort options appropriate for the grid's view type. Can't compare
			// types because the m_owningViewType's are declared in PaDll which cannot
			// be referenced by PaControls since PaControls is referenced by PaDll.
			if (m_owningViewType.Name == "DataCorpusWnd")
				sortOptions = PaApp.Project.DataCorpusSortOptions;
			else if (m_owningViewType.Name == "FindPhoneWnd")
				sortOptions = PaApp.Project.FindPhoneSortOptions;
			else if (m_owningViewType.Name == "XYChartWnd")
				sortOptions = PaApp.Project.XYChartSortOptions;

			if (sortOptions == null)
				sortOptions = new SortOptions(true);

			// If the default sort options should not change as the user clicks headings or
			// changes phonetic sort options from the phonetic sort option drop-down, then
			// clone the sort options stored in the project so when the user does click
			// headings and change phonetic sort options, the changes won't get saved to
			// the sort options object in the project.
			if (!sortOptions.SaveManuallySetSortOptions)
				sortOptions = sortOptions.Clone();

			if (performSort)
				SortOptions = sortOptions;
			else
				m_sortOptions = sortOptions;

			FindInfo.ResetStartSearchCell();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDataSourcesModified(object args)
		{
			if (PaApp.Project == null || PaApp.Project.FieldInfo == null)
				return false;
			
			m_suspendSavingColumnChanges = true;

			foreach (PaFieldInfo fieldInfo in PaApp.Project.FieldInfo)
			{
				DataGridViewColumn col = Columns[fieldInfo.FieldName];
				if (col == null)
				{
					if (fieldInfo.DisplayIndexInGrid < 0)
						continue;

					col = AddNewColumn(fieldInfo);
				}

				if (fieldInfo.DisplayIndexInGrid < 0)
					col.Visible = false;
				else
				{
					col.DisplayIndex = (fieldInfo.DisplayIndexInGrid < Columns.Count ?
						fieldInfo.DisplayIndexInGrid : Columns.Count - 1);

					col.Visible = fieldInfo.VisibleInGrid;
				}
			}

			m_suspendSavingColumnChanges = false;

			if (CurrentRow != null)
				InvalidateRow(CurrentRow.Index);

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the CV syllables get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCVSyllablesChanged(object args)
		{
			PaFieldInfo fieldInfo = PaApp.Project.FieldInfo.CVPatternField;
			if (fieldInfo != null)
			{
				DataGridViewColumn col = Columns[fieldInfo.FieldName];
				if (col != null)
					InvalidateColumn(col.Index);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnEditSourceRecord(object args)
		{
			if (!Focused || !PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
				return false;

			int row = -1;
			if (args.GetType() == typeof(int))
				row = (int)args;
			else if (CurrentRow != null)
				row = CurrentRow.Index;

			if (GetWordEntry(row) != null)
				new DataSourceEditor(GetWordEntry(row).WordCacheEntry, FindForm().Text);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A hiddent feature is that when the user presses Ctrl+Shift+F2 on an entry that
		/// came from a FW database, a dialog with the jump url will be shown before jumping
		/// to Flex.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			
			if (e.Control && e.Shift && e.KeyCode == Keys.F2 && GetWordEntry() != null)
				new DataSourceEditor(GetWordEntry().WordCacheEntry, null, true);
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Uncomment if the desire is to have the menu item disabled when the user cannot
		///// edit the source record. As it is now, if a record cannot be edited, a dialog
		///// tells the user this.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnUpdateEditSourceRecord(object args)
		//{
		//    TMItemProperties itemProps = args as TMItemProperties;

		//    if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) ||
		//        itemProps == null || CurrentRow == null || CurrentRow.Index < 0 ||
		//        CurrentRow.Index >= m_cache.Count)
		//        return false;

		//    itemProps.Update = true;
		//    itemProps.Visible = true;
		//    itemProps.Enabled = PaApp.IsToolboxRunning &&
		//        m_cache[CurrentRow.Index].WordCacheEntry.RecordEntry.CanBeEditedInToolbox;

		//    return true;
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownShowGridColumns(object args)
		{
			if (!Focused || !PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
				return false;

			ToolBarPopupInfo itemProps = args as ToolBarPopupInfo;
			if (itemProps == null)
				return false;

			// Create a popup control with checkboxes used to turn column's on and off.
			GridColumnVisibilitySetter gcvs = new GridColumnVisibilitySetter(this);
			gcvs.Dock = DockStyle.Fill;

			SizableDropDownPanel gcvsHost =
				new SizableDropDownPanel("savedshowhidecolumndropdownsize", gcvs.DefltSize);
			gcvsHost.BackColor = SystemColors.Menu;
			gcvsHost.Padding = new Padding(0, 0, 0, gcvsHost.Padding.Bottom);
			gcvsHost.Controls.Add(gcvs);

			itemProps.Control = gcvsHost;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the settings after the user changes the visible columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownClosedShowGridColumns(object args)
		{
			SaveSettings();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowGridColumns(object args)
		{
			Form parentForm = FindForm();

			if (!Focused || !PaApp.IsViewOrFormActive(m_owningViewType, parentForm))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = parentForm.ContainsFocus;
			itemProps.Update = true;
			return itemProps.Enabled;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnWordListOptionsChanged(object args)
		{
			// Calling Load on the GridLayoutInfo will fire multiple calls to
			// OnColumnDisplayIndexChanged since the load updates all the column display
			// indexes. That should be prevented here since it will cause the field settings
			// to get saved each time. Therefore, we set a flag here to prevent the saving
			// of field settings each time the OnColumnDisplayIndexChanged event is fired.
			m_suspendSavingColumnChanges = true;
			PaApp.Project.GridLayoutInfo.Load(this);
			m_suspendSavingColumnChanges = false;
			FindInfo.ResetStartSearchCell();

			// Return false so everyone who cares gets a crack at handling the message.
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnGridLinesNone(object args)
		{
			if (!Focused || !PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
				return false;

			CellBorderStyle = DataGridViewCellBorderStyle.None;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnGridLinesBoth(object args)
		{
			if (!Focused || !PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
				return false;

			CellBorderStyle = DataGridViewCellBorderStyle.Single;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnGridLinesHorizontal(object args)
		{
			if (!Focused || !PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
				return false;

			CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnGridLinesVertical(object args)
		{
			if (!Focused || !PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
				return false;

			CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the font(s) get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPaFontsChanged(object args)
		{
			RefreshColumnFonts(true);

			// Return false to allow other windows to update their fonts.
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnPlayback(object args)
		{
			if ((!Focused && !m_isCurrentPlaybackGrid) ||
				!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
			{
				return false;
			}

			PlaybackAllSelectedRows(false);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPlaybackRepeatedly(object args)
		{
			if ((!Focused && !m_isCurrentPlaybackGrid) ||
				!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
			{
				return false;
			}

			PlaybackAllSelectedRows(true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlayback(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || (!Focused && !m_isCurrentPlaybackGrid) ||
				!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
			{
				return false;
			}

			bool enable = (GetAudioEntriesInSelectedRows() != null && !m_playbackInProgress);
			if (itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
			}
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlaybackRepeatedly(object args)
		{
			return OnUpdatePlayback(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAdjustPlaybackSpeed(object args)
		{
			return OnUpdatePlayback(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAdjustPlaybackSpeedParent(object args)
		{
			return OnUpdatePlayback(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnStopPlayback(object args)
		{
			if ((!Focused && !m_isCurrentPlaybackGrid) ||
				!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
			{
				return false;
			}

			m_playbackInProgress = false;
			m_playbackAborted = true;
			
			if (m_audioPlayer != null)
				m_audioPlayer.Stop();

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateStopPlayback(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || (!Focused && !m_isCurrentPlaybackGrid) ||
				!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
			{
				return false;
			}

			itemProps.Visible = true;
			itemProps.Enabled = AudioPlayer.IsPlaybackInProgress || m_playbackInProgress;
			itemProps.Update = true;
			return true;
		}
		#endregion

		#region Find Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the FindDlg form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void GroupExpandedChangedHandler(SilHierarchicalGridRow row)
		{
			if (!row.Expanded && row.IsRowOwned(FindInfo.FirstMatchedRow))
			{
				CurrentCell = this[0, row.Index];

				// Move the cell's row to the screen's top if it is not on the screen
				if (!CurrentCell.Displayed)
					FirstDisplayedScrollingRowIndex = (row.Index < 1) ? 0 : row.Index;

				FindInfo.ResetStartSearchCell();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the FindDlg form.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnEditFind(object args)
		{
			bool isGridCurrent =
				PaApp.MsgMediator.SendMessage("CompareGrid", this) || Focused;

			if (FindInfo.FindDlgIsOpen || !isGridCurrent ||
				!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
			{
				return false;
			}

			new FindDlg(this).Show(); // Not modal
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cycle through the cell rows that matched the Find search criteria.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnEditFindNext(object args)
		{
			bool isGridCurrent =
				PaApp.MsgMediator.SendMessage("CompareGrid", this) || Focused;

			if (!isGridCurrent || !PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
				return false;

			FindInfo.Grid = this;
			FindInfo.Find(false);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cycle backwards through the cell rows that matched the Find search criteria.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnEditFindPrevious(object args)
		{
			bool isGridCurrent =
				PaApp.MsgMediator.SendMessage("CompareGrid", this) || Focused;

			if (!isGridCurrent || !PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
				return false;

			FindInfo.Grid = this;
			FindInfo.Find(true);
			return true;
		}
		#endregion

		#region Playback methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PlaybackAllSelectedRows(bool repeatedly)
		{
			if (m_playbackInProgress)
				return;

			m_playbackAborted = false;
			m_playbackInProgress = true;
			PreparePlaybackControlButtonsForPlayback();

			m_audioPlayer = new AudioPlayer();

			do
			{
				// Loop through the selected items and playback their utterances.
				SortedList<int, WordCacheEntry> selectedWords = GetAudioEntriesInSelectedRows();
				if (selectedWords == null)
					break;
				foreach (KeyValuePair<int, WordCacheEntry> entry in selectedWords)
				{
					m_currPlaybackRow = entry.Key;
					InvalidateRow(entry.Key);
					Application.DoEvents();
					PlaybackSingleEntry(entry.Value);
					m_currPlaybackRow = -2;
					InvalidateRow(entry.Key);
					Application.DoEvents();

					if (m_playbackAborted)
						break;
				}
			}
			while (!m_playbackAborted && repeatedly);

			m_playbackInProgress = false;
			m_audioPlayer = null;
			PaApp.MsgMediator.SendMessage("PlaybackEnded", null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the playback control buttons to their appropriate state just before playback
		/// begins. Do this explicitly because these buttons won't get an update messages
		/// until after playback is complete. By that time, it's too late.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PreparePlaybackControlButtonsForPlayback()
		{
			// Update these buttons since they won't get an update
			// message until after playback is complete.
			TMItemProperties itemProps = m_tmAdapter.GetItemProperties("tbbStopPlayback");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = true;
				itemProps.Update = true;
				m_tmAdapter.SetItemProperties("tbbStopPlayback", itemProps);
			}

			itemProps = m_tmAdapter.GetItemProperties("tbbPlaybackOnMenu");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = false;
				itemProps.Update = true;
				m_tmAdapter.SetItemProperties("tbbPlaybackOnMenu", itemProps);
			}

			itemProps = m_tmAdapter.GetItemProperties("tbbPlaybackRepeatedly");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = false;
				itemProps.Update = true;
				m_tmAdapter.SetItemProperties("tbbPlaybackRepeatedly", itemProps);
			}

			itemProps = m_tmAdapter.GetItemProperties("tbbAdjustPlaybackSpeedParent");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = false;
				itemProps.Update = true;
				m_tmAdapter.SetItemProperties("tbbAdjustPlaybackSpeedParent", itemProps);
			}

			// Inform the main window what's happening.
			PaApp.MsgMediator.SendMessage("PlaybackBeginning", null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Plays back the audio for the specified word cache entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PlaybackSingleEntry(WordCacheEntry entry)
		{
			RecordCacheEntry recEntry = entry.RecordEntry;
			if (recEntry == null || m_audioPlayer == null)
				return;

			long offset = long.Parse(entry[m_audioFileOffsetFieldName]);
			long length = long.Parse(entry[m_audioFileLengthFieldName]);
			string audioFile = (string.IsNullOrEmpty(entry.AbsoluteAudioFilePath) ?
				entry[m_audioFileFieldName] : entry.AbsoluteAudioFilePath);

			// Get the playback speed for the Control grid
			m_playbackSpeed =
				PaApp.SettingsHandler.GetIntSettingsValue(m_owningViewType.Name, "playbackspeed", 100);
			
			// If the speed is not 100% then use Speech Analyzer to playback the utterance.
			if (m_playbackSpeed != 100f)
			{
				PlaybackEntryUsingSA(recEntry.DataSource.DataSourceType, audioFile, offset, length);
				return;
			}

			// For SA wave files, the offset and length are in bytes, not milliseconds.
			// Therefore, we need to calculate what those values are in milliseconds.
			if (recEntry.DataSource.DataSourceType == DataSourceType.SA)
			{
				offset = AudioPlayer.ByteValueToMilliseconds(offset, recEntry.Channels,
					recEntry.SamplesPerSecond, recEntry.BitsPerSample);

				length = AudioPlayer.ByteValueToMilliseconds(length, recEntry.Channels,
					recEntry.SamplesPerSecond, recEntry.BitsPerSample);
			}

			m_audioPlayer.Play(audioFile, offset, offset + length);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Uses Speech Analyzer to play back the specified audio file from the specified
		/// offset, for the specified length;
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PlaybackEntryUsingSA(DataSourceType dataSourceType, string audioFile,
			long offset, long length)
		{
			// If the entry is not from an SA audio file, the units for the offset
			// and length are assumed to be in milliseconds. Since SA will perform
			// the altered speed playback and it needs the values in bytes convert
			// the millisecond values to bytes.
			if (dataSourceType != DataSourceType.SA)
			{
				offset = AudioPlayer.MillisecondValueToBytes(offset, audioFile);
				length = AudioPlayer.MillisecondValueToBytes(length, audioFile);
			}

			// If AlteredSpeedPlayback returns false it means SA couldn't be found.
			// Therefore, abort trying to playback anymore utterances.
			if (!m_audioPlayer.AlteredSpeedPlayback(FindForm().Text,
				audioFile, offset, offset + length, m_playbackSpeed))
			{
				m_playbackAborted = true;
			}

			return;
		}

		#endregion

		#region Update handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGridLinesNone(object args)
		{
			if (!Focused || !PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Checked = (CellBorderStyle == DataGridViewCellBorderStyle.None);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGridLinesBoth(object args)
		{
			if (!Focused || !PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Checked = (CellBorderStyle == DataGridViewCellBorderStyle.Single);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGridLinesHorizontal(object args)
		{
			if (!Focused || !PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Checked = (CellBorderStyle == DataGridViewCellBorderStyle.SingleHorizontal);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGridLinesVertical(object args)
		{
			if (!Focused || !PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Checked = (CellBorderStyle == DataGridViewCellBorderStyle.SingleVertical);
			itemProps.Update = true;
			return true;
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used in PA.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] { this });
		}

		#endregion
	}

	#region PaCacheGridRow class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaCacheGridRow : DataGridViewRow
	{
		private int m_cacheEntryIndex = -1;
		private SilHierarchicalGridRow m_parentRow = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaCacheGridRow() : base()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaCacheGridRow(int cacheEntryIndex) : this()
		{
			m_cacheEntryIndex = cacheEntryIndex;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the index of the row's corresponding cache entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CacheEntryIndex
		{
			get { return m_cacheEntryIndex; }
			set { m_cacheEntryIndex = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the SilHierarchicalGridRow to which this row is subordinate.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilHierarchicalGridRow ParentRow
		{
			get { return m_parentRow; }
			set { m_parentRow = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates an exact copy of the row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override object Clone()
		{
			object clone = base.Clone();

			if (clone is PaCacheGridRow)
			{
				((PaCacheGridRow)clone).m_cacheEntryIndex = m_cacheEntryIndex;
				((PaCacheGridRow)clone).m_parentRow = m_parentRow;
			}

			return clone;
		}
	}

	#endregion
}