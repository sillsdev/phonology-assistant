using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	#region IPhoneListViewer interface
	public interface IPhoneListViewer
	{
		void LoadPhones(List<CharGridCell> phoneList);
		void Reset();
		string SupraSegsToIgnore { get;}
	}

	#endregion

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Provides a builder class for laying out phones in a chart (e.g. vowel or consonant) or
	/// any object derived from the IPhoneListViewer interface. When using for an
	/// IPhoneListViewer object, a collection of CharGridCell objects is provided to the
	/// object for it to do with what it pleases. The information in the CharGridCell
	/// collection indicates at what row and column a phone should be layed out in the viewer.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CharGridBuilder
	{
		private List<CharGridCell> m_phoneList;
		private string m_supraSegsToIgnore;
		private string m_persistedInfoFilename;
		private bool m_reloadError = false;
		private readonly CharGrid m_chrGrid;
		private readonly IPhoneListViewer m_phoneLstVwr;
		private readonly IPACharacterType m_chrType;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a character grid builder for loading a CharGrid control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridBuilder(CharGrid chrGrid, IPACharacterType chrType)
		{
			m_chrGrid = chrGrid;
			m_chrType = chrType;
			m_supraSegsToIgnore = m_chrGrid.SupraSegsToIgnore;
			m_persistedInfoFilename = PersistedInfoFilename;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a character grid builder for loading a CharPickerRows control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridBuilder(IPhoneListViewer phoneLstVwr, IPACharacterType chrType)
		{
			m_phoneLstVwr = phoneLstVwr;
			m_chrType = chrType;
			m_supraSegsToIgnore = m_phoneLstVwr.SupraSegsToIgnore;
			m_persistedInfoFilename = PersistedInfoFilename;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the filename used for serializing and deserializing chart information and
		/// layout.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string PersistedInfoFilename
		{
			get
			{
				return PaApp.Project.ProjectPathFilePrefix +
					(m_chrType == IPACharacterType.Consonant ? "Consonant" : "Vowel") +
					"Chart.xml";
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<CharGridCell> Build()
		{
		    if (PaApp.DesignMode || (m_chrGrid == null && m_phoneLstVwr == null))
		        return null;

			try
			{
				if (m_phoneLstVwr != null)
					BuildCharPickerRows();
				else
					BuildCharGrid();
			}
			catch (Exception e)
			{
				if (m_reloadError)
				{
					STUtils.STMsgBox("Error: " + e.Message);
					return null;
				}

				m_reloadError = true;
				AttemptReloadAfterError();
			}

			return m_phoneList;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildCharPickerRows()
		{
			if (!CharGridPersistence.Load(this, m_persistedInfoFilename))
			{
				// At this point, we know there wasn't chart information persisted for the
				// specified phone type. For building a picker we need to have first built and
				// persisted the contents of a CharGrid for the same phone type. Therefore,
				// force building a default CharGrid and persist its layout.
				CharGridBuilder bldr = new CharGridBuilder(new CharGrid(), m_chrType);
				bldr.Build();

				// Try again.
				if (!CharGridPersistence.Load(this, m_persistedInfoFilename))
				{
					m_phoneList = null;
					STUtils.STMsgBox(string.Format(
						Properties.Resources.kstidErrorLoadingCharPickerRowsMsg, m_chrType));
					return;
				}
			}

			m_phoneLstVwr.LoadPhones(m_phoneList);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildCharGrid()
		{
			bool defaultChartBuilt = false;

			if (!CharGridPersistence.Load(this, m_persistedInfoFilename))
			{
				BuildPhoneList();
				DefaultChartHeadings.Load(this, m_chrType);
				defaultChartBuilt = true;
			}

			PlacePhonesInChart();

			// Persist the chart right after building a default one.
			if (defaultChartBuilt)
				CharGridPersistence.Save(m_chrGrid, m_phoneList, m_persistedInfoFilename);

			// Select the first cell in the grid.
			if (m_chrGrid != null && m_chrGrid.Grid.Rows.Count > 0 && m_chrGrid.Grid.Columns.Count > 0)
				m_chrGrid.Grid.CurrentCell = m_chrGrid.Grid[0, 0];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to reload the character grid in a default state if there was an error
		/// trying to load once.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AttemptReloadAfterError()
		{
			if (m_phoneLstVwr != null)
				m_phoneLstVwr.Reset();
			else
				m_chrGrid.Reset();

			// Clear the persisted file name so Reset() will not succeed in deserializing
			// the grid info. That will force an attempt to build a default grid.
			m_persistedInfoFilename = null;

			Build();
			m_persistedInfoFilename = PersistedInfoFilename;
	
			if (m_chrGrid != null)
				m_chrGrid.ForceCurrentCellUpdate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<CharGridCell> InitializeFromPersistedSource(CharGridPersistence cgp)
		{
			if (cgp == null)
				return null;

			m_supraSegsToIgnore = cgp.SupraSegsToIgnore;
			BuildPhoneList();
			
			if (m_chrGrid != null)
			{
				cgp.LoadHeadings(m_chrGrid, true);
				cgp.LoadHeadings(m_chrGrid, false);
				
				if (cgp.RowHeaderWidth > 0)
					m_chrGrid.RowHeaderWidth = cgp.RowHeaderWidth;
				
				if (cgp.ColumnHeaderHeight > 0)
					m_chrGrid.ColumnHeaderHeight = cgp.ColumnHeaderHeight;
	
				if (cgp.RowSplitPosition > 0)
					m_chrGrid.RowHeadersCollectionPanel.SplitPosition = cgp.RowSplitPosition;
				
				if (cgp.ColumnSplitPosition > 0)
					m_chrGrid.ColumnHeadersCollectionPanel.SplitPosition = cgp.ColumnSplitPosition;
				
				m_chrGrid.ShowUncertainPhones = cgp.ShowUncertainPhones;
				m_chrGrid.SupraSegsToIgnore = cgp.SupraSegsToIgnore;
			}

			return m_phoneList;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets all the phones of the specified type that are found in the data source(s)
		/// (i.e. phone cache) and figures out where their default locations are in the chart.
		/// The return value is the list of phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildPhoneList()
		{
			m_phoneList = null;

			// This will keep track of the widest phone in the list (in pixels).
			int maxPhoneWidth = 0;

			// First build the list so phones are sorted by place of articulation.
			SortedList<string, CharGridCell> tmpPhoneList = new SortedList<string, CharGridCell>();

			// This should only happen in design mode.
			if (PaApp.PhoneCache == null)
				return;

			TextFormatFlags flags = TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix |
				TextFormatFlags.LeftAndRightPadding;

			Font fnt = (m_chrGrid != null ? m_chrGrid.ChartFont : FontHelper.PhoneticFont);

			// Get phones from the Phone cache.
			foreach (KeyValuePair<string, IPhoneInfo> phoneInfo in PaApp.PhoneCache)
			{
				string phone = QualifyingPhone(phoneInfo.Key, phoneInfo.Value, tmpPhoneList);
				if (phone == null)
					continue;

				// Find the Phone's base character in the IPA character cache
				// in order to find it's default placement in the chart.
				IPACharInfo info = GetBaseCharInfoForPhone(phoneInfo.Key);

				if (info != null)
				{
					CharGridCell cgc = new CharGridCell(phone);
					cgc.DefaultColumn = info.ChartColumn;
					cgc.DefaultGroup = info.ChartGroup;
					cgc.TotalCount = phoneInfo.Value.TotalCount;
					cgc.CountAsPrimaryUncertainty = phoneInfo.Value.CountAsPrimaryUncertainty;
					cgc.CountAsNonPrimaryUncertainty = phoneInfo.Value.CountAsNonPrimaryUncertainty;
					if (phoneInfo.Value.SiblingUncertainties != null &&
						phoneInfo.Value.SiblingUncertainties.Count > 0)
					{
						cgc.SiblingUncertainties =
							new List<string>(phoneInfo.Value.SiblingUncertainties);
					}
					
					string key = DataUtils.GetMOAKey(phone);
					tmpPhoneList[key] = cgc;

					maxPhoneWidth = Math.Max(maxPhoneWidth, TextRenderer.MeasureText(phone,
						fnt, Size.Empty, flags).Width);
				}
			}

			// Move all the phones from their sorted list into a more permanant list.
			m_phoneList = new List<CharGridCell>();
			foreach (CharGridCell cgc in tmpPhoneList.Values)
				m_phoneList.Add(cgc);

			// Set the grid's cell width to the widest phone, adding
			// some padding because the grid renders with padding.
			if (m_chrGrid != null)
			{
				m_chrGrid.CellWidth = maxPhoneWidth +
					PaApp.SettingsHandler.GetIntSettingsValue("cvcharts", "extracellwidth", 8);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not a phoneInfo contains a phone that qualifies to be shown
		/// in the chart. The criteria for determining whether or not a phone qualifies is
		/// its type and whether or not it's already in the list. If the phone qualifies, its
		/// phoneInfo is returned. When the phoneInfo contains some suprasegmentals that are
		/// found in the list of ones to ignore, then a modified version of the phoneInfo is
		/// returned. One without those suprasegmentals.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string QualifyingPhone(string phone, IPhoneInfo phoneInfo,
			SortedList<string, CharGridCell> phoneList)
		{
			// First, check if the phone's type is correct.
			if (phoneInfo.CharType != m_chrType)
				return null;

			// Now build a new phone without ignored suprasegmentals.
			StringBuilder bldr = new StringBuilder();
			for (int i = 0; i < phone.Length; i++)
			{
				if (m_supraSegsToIgnore == null || !m_supraSegsToIgnore.Contains(phone.Substring(i, 1)))
					bldr.Append(phone[i]);
			}

			// This should never happen, but return null if there's nothing left of the phone.
			string newPhone = FFNormalizer.Normalize(bldr.ToString());
			if (newPhone.Length == 0)
				return null;

			// Make sure the phone isn't already in the list of qualifying phones.
			return (PhoneExistsInList(newPhone, phoneInfo, phoneList) ? null : newPhone);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks to see if the specified phone already exists in the specified collection of
		/// CharGridCell objects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool PhoneExistsInList(string phone, IPhoneInfo phoneInfo,
			SortedList<string, CharGridCell> phoneList)
		{
			foreach (CharGridCell cgc in phoneList.Values)
			{
				// If the phone is already in the list, then increment the counts and
				// the list of sibling uncertainties stored in the grid cell object.
				// This should only happen when suprasegmentals modifying phones were
				// found and are also being ignored.
				if (cgc.Phone == phone)
				{
					cgc.TotalCount += phoneInfo.TotalCount;
					cgc.CountAsPrimaryUncertainty += phoneInfo.CountAsPrimaryUncertainty;
					cgc.CountAsNonPrimaryUncertainty += phoneInfo.CountAsNonPrimaryUncertainty;

					if (phoneInfo.SiblingUncertainties != null)
					{
						foreach (string siblingPhone in phoneInfo.SiblingUncertainties)
						{
							if (!cgc.SiblingUncertainties.Contains(siblingPhone))
								cgc.SiblingUncertainties.Add(siblingPhone);
						}
					}

					return true;
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return the IPACharInfo for the phones base character. With the exception of
		/// ambiguous sequences, the only time this should not be the first character is when
		/// there's a tie bar or linking suprasegmental involved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private IPACharInfo GetBaseCharInfoForPhone(string phone)
		{
			IPACharInfo charInfo = DataUtils.IPACharCache.ToneLetterInfo(phone);
			if (charInfo != null)
				return charInfo;
				
			IPhoneInfo phoneInfo = PaApp.PhoneCache[phone];
			return (phoneInfo == null ? null : DataUtils.IPACharCache[phoneInfo.BaseCharacter]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Place all the phones on the chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void PlacePhonesInChart()
		{
			foreach (CharGridCell cgc in m_phoneList)
			{
				if (!cgc.IsPlacedOnChart && cgc.Visible)
					PlaceSinglePhone(cgc);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Places a single Phone on the chart within the specified header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PlaceSinglePhone(CharGridCell cgc)
		{
			CharGridHeader hdr = GetHeaderForPhonesGroup(cgc);
			if (hdr == null || hdr.OwnedRows.Count == 0)
				return;

			int row = -1;
			int col = -1;

			// First, try the row and column contained in the cell object.
			if (IsCellEmtpy(cgc.Row, cgc.Column))
			{
				row = cgc.Row;
				col = cgc.Column;
			}
			else
			{
				// Find a row owned by the header that has an
				// empty cell in the Phone's desired column.
				foreach (DataGridViewRow ownedRow in hdr.OwnedRows)
				{
					if (IsCellEmtpy(ownedRow.Index, cgc.Column))
					{
						row = ownedRow.Index;
						col = cgc.Column;
						break;
					}
					else if (IsCellEmtpy(ownedRow.Index, cgc.DefaultColumn))
					{
						row = ownedRow.Index;
						col = cgc.DefaultColumn;
						break;
					}
				}
			}

			if (row > -1 && col > -1)
			{
				m_chrGrid.Grid[col, row].Value = cgc;
				cgc.Row = row;
				cgc.Column = col;
			}
			else
			{
				// If the header didn't have an empty cell at the desired column
				// in any of the rows belonging to the header, then it's necessary
				// to add a new row to the header to accomodate the phoneInfo.
				m_chrGrid.AddRowToHeading(hdr);
				int desiredCol = (cgc.Column > -1 ? cgc.Column : cgc.DefaultColumn);
				if (desiredCol >= hdr.LastRow.Cells.Count)
					desiredCol = hdr.LastRow.Cells.Count - 1;
				
				DataGridViewCell cell = hdr.LastRow.Cells[desiredCol];
				cell.Value = cgc;
				cgc.Row = hdr.LastRow.Index;
				cgc.Column = desiredCol;
			}

			cgc.Group = hdr.Group;
			cgc.IsPlacedOnChart = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the grid cell at the specified row and column is empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsCellEmtpy(int row, int col)
		{
			DataGridView grid = m_chrGrid.Grid;

			if (grid == null || row < 0 || col < 0 || row >= grid.Rows.Count || col >= grid.Columns.Count)
				return false;

			CharGridCell cgc = grid[col, row].Value as CharGridCell;
			return (cgc == null || !cgc.Visible);
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Finds the header whose group is the one specified in the specified CharGridCell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private CharGridHeader GetHeaderForPhonesGroup(CharGridCell cgc)
		{
			// If the CharGridCell's group is specified, then it means it was read from
			// a persisted XML source. Otherwise, use the default group.
			int groupToFind = (cgc.Group > -1 ? cgc.Group : cgc.DefaultGroup);

			// Find the group to which this Phone belongs.
			foreach (CharGridHeader hdr in m_chrGrid.RowHeaders)
			{
				if (groupToFind == hdr.Group)
					return hdr;
			}

			// If we've gotten here and weren't looking for the default group, then
			// we failed to find the desired group. So we might as well try finding
			// the default group.
			if (cgc.Group > -1 && cgc.Group != cgc.DefaultGroup)
			{
				foreach (CharGridHeader hdr in m_chrGrid.RowHeaders)
				{
					if (cgc.DefaultGroup == hdr.Group)
						return hdr;
				}
			}

			// We've failed to find a header so just create one at the end of the grid.
			return m_chrGrid.AddRowHeader();
		}
	}
}
