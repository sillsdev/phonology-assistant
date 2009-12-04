// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005, SIL International. All Rights Reserved.
// <copyright from='2005' to='2005' company='SIL International'>
//		Copyright (c) 2005, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: FindTests.cs
// Responsibility: DavidO & ToddJ
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using NUnit.Framework;
using SIL.Pa.Data;
using SIL.Pa.TestUtils;
using SIL.Pa.UI.Controls;
using SIL.Pa.UI.Dialogs;
using SilUtils;

namespace SIL.Pa
{
	[TestFixture]
	public class FindTests : TestBase
	{
		#region Declarations
		private RecordCache m_recCache;
		private WordCache m_wordCache;
		private WordListCache m_cache;
		private PaWordListGrid m_grid;
		private PaDataSource m_dataSource;
		private FindDlg m_findDlg;
		private const string kPhonetic = "Phonetic";
		private const string kGloss = "Gloss";
		private const string kCVPattern = "CV Pattern";
		#endregion

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create temporary test records.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			DataUtils.LoadIPACharCache(null);

			PaProject proj = new PaProject(true);
			proj.Language = "dummy";
			proj.ProjectName = "dummy";
			PaApp.Project = proj;

			m_dataSource = new PaDataSource();
			m_dataSource.DataSourceType = DataSourceType.Toolbox;
			m_dataSource.ParseType = DataSourceParseType.Interlinear;

			FindInfo.ShowMessages = false;
			FwDBAccessInfo.ShowMsgOnFileLoadFailure = false;
			FwQueries.ShowMsgOnFileLoadFailure = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			Initialize();
			m_findDlg.IsRegularExpression = false;
			m_findDlg.MatchCase = false;
			m_findDlg.MatchEntireWord = false;
			m_findDlg.StartsWith = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TestTearDown()
		{
			if (m_findDlg != null)
				m_findDlg.Dispose();
		}

		#endregion

		#region Create PaWordListGrid
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Build the PaWordListGrid for searching.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Initialize()
		{
			Initialize("fib bit ebay bitter drillbit abdiging",
				"cvc cvc vcvc cvccvc ccvccbit vcbitcvcvcc",
				"GLFIB glbitter glebay glbitter drillbit glabitging");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Build the PaWordListGrid for searching.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Initialize(string eticWrds, string cvWrds, string glossWrds)
		{
			m_recCache = new RecordCache();
			PaApp.RecordCache = m_recCache;
			m_cache = new WordListCache();

			AddWords(eticWrds, cvWrds, glossWrds);

			// Create grid
			m_grid = new PaWordListGrid(m_cache, GetType(), false);
			SetField(m_grid, "m_suspendSavingColumnChanges", true);

			// Make all the grid's rows & columns visible and thus searchable
			int m_iRow, m_iColumn;
			for (m_iRow = 0; m_iRow < m_grid.Rows.Count; m_iRow++)
			{
				m_grid.Rows[m_iRow].Visible = true;
				for (m_iColumn = 0; m_iColumn < m_grid.Columns.Count; m_iColumn++)
					m_grid.Columns[m_iColumn].Visible = true;
			}

			// Set the CVPattern & Gloss column Display Indexes to the correct order
			// based on the columnsToSearch below.
			m_grid.Columns[8].DisplayIndex = 1;
			m_grid.Columns[3].DisplayIndex = 2;
			m_grid.Columns[1].DisplayIndex = 3;
			m_grid.Columns[2].DisplayIndex = 4;
			m_grid.Columns[4].DisplayIndex = 5;
			m_grid.Columns[5].DisplayIndex = 6;
			m_grid.Columns[6].DisplayIndex = 7;
			m_grid.Columns[7].DisplayIndex = 8;

			SetField(typeof(FindInfo), "s_reverseFind", false);
			SetField(m_grid, "m_suspendSavingColumnChanges", false);
			FindInfo.Grid = m_grid;

			// Add columns to search
			List<FindDlgColItem> columnsToSearch = new List<FindDlgColItem>();
			columnsToSearch.Add(new FindDlgColItem(0, 0, "Phonetic", "Phonetic"));
			columnsToSearch.Add(new FindDlgColItem(8, 1, "CV Pattern", "CV Pattern"));
			columnsToSearch.Add(new FindDlgColItem(3, 2, "Gloss", "Gloss"));
			FindInfo.ColumnsToSearch = columnsToSearch.ToArray();

			m_findDlg = new FindDlg(m_grid);
			ResetStartCell();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add records to the m_cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddWords(string words, string pattern, string gloss)
		{
			m_recCache.Clear();
			RecordCacheEntry entry = new RecordCacheEntry(true);
			entry.SetValue(kPhonetic, words);
			entry.SetValue(kCVPattern, pattern);
			entry.SetValue(kGloss, gloss);
			entry.NeedsParsing = true;
			entry.DataSource = m_dataSource;
			m_recCache.Add(entry);
			m_wordCache = m_recCache.BuildWordCache(null);

			PaApp.BuildPhoneCache();

			m_cache.Clear();
			foreach (WordCacheEntry wcEntry in m_wordCache)
				m_cache.Add(wcEntry);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the search string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetSearchString(string searchString)
		{
			// Add search parameters
			FindInfo.FindPattern = m_findDlg.formatFindPattern(searchString);
			FindInfo.FindText = searchString; // For 'Not Found' msg
		}
		#endregion

		#region Debugging / Testing

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests Current Cell's Location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool IsCurrCellLocation(int rowIndex, int columnIndex)
		{
			return (FindInfo.Grid.CurrentCellAddress.X == columnIndex &&
				FindInfo.Grid.CurrentCellAddress.Y == rowIndex);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reset the starting grid cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void ResetStartCell()
		{
			FindInfo.Grid.CurrentCell = FindInfo.Grid[0, 0];
		}
		#endregion

		#region Find Tests
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests finding a whole word with a diacritic in a string. See PA-792.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[Ignore("Need fix for PA-792")]
		public void FindWholeWordWithDiacriticInData()
		{
			Initialize("foghorn aeo\u032Fu leghorn", "cvv vvc cvc", "one two three");
			m_findDlg.MatchEntireWord = true;

			SetSearchString("u");
			Assert.IsFalse(FindInfo.FindFirst(false)); // Forward find
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests forward & backward Find.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FindForBackTest()
		{
			//InspectGrid();
			SetSearchString("ebay");
			Assert.AreEqual(true, FindInfo.FindFirst(false)); // Forward find
			Assert.AreEqual(true, IsCurrCellLocation(2,0));

			SetSearchString("glbitter");
			Assert.AreEqual(true, FindInfo.FindFirst(true)); // Reverse find
			Assert.AreEqual(true, IsCurrCellLocation(1,3));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests backward & forward Find.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FindBackForTest()
		{
			SetSearchString("ebay");
			Assert.AreEqual(true, FindInfo.FindFirst(true)); // Forward find
			Assert.AreEqual(true, IsCurrCellLocation(2,3)); // finds "glebay"

			SetSearchString("glbitter");
			Assert.AreEqual(true, FindInfo.FindFirst(false)); // Reverse find
			Assert.AreEqual(true, IsCurrCellLocation(3,3));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests forward & backward Find of a non-existant search item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FindMissingTest()
		{
			SetSearchString("ebaywantsyou");

			Assert.AreEqual(false, FindInfo.FindFirst(false)); // Forward find
			ResetStartCell();
			Assert.AreEqual(false, FindInfo.FindFirst(true)); // Reverse find
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests a Find forward & Find Next.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FindNextNextTest()
		{
			SetSearchString("drillbit");

			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(4,0));

			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(4, 3));
		}


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests a Find backward & Find Prev.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FindPrevPrevTest()
		{
			SetSearchString("drillbit");

			Assert.AreEqual(true, FindInfo.FindFirst(true));
			Assert.AreEqual(true, IsCurrCellLocation(4, 3));

			Assert.AreEqual(true, FindInfo.Find(true));
			Assert.AreEqual(true, IsCurrCellLocation(4, 0));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Testing "when the user finds a match on the last Find column in the row
		/// with a 'Find Next' and then switches direction with a 'Find Prev'."
		/// See FindInfo.Find()
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FindForLastColBackTest()
		{
			SetSearchString("glbitter");

			Assert.IsTrue(FindInfo.FindFirst(false), "Did not find first forward"); // Forward find
			Assert.IsTrue(IsCurrCellLocation(1, 3), "Curr. Cell Location is not 1,3");
			Assert.IsTrue(FindInfo.Find(false), "Did not find next forward"); // Forward find
			Assert.IsTrue(IsCurrCellLocation(3, 3), "Curr. Cell Location is not 3,3");
			Assert.IsTrue(FindInfo.Find(true), "Did not find backward"); // Backward find
			Assert.IsTrue(IsCurrCellLocation(1, 3), "Curr. Cell Location is not 1,3");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Testing "when the user finds a match on the first Find column in the row
		/// with a 'Find Prev' and then switches direction with a 'Find Next'."
		/// See FindInfo.Find()
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FindBackFirstColForTest()
		{
			m_findDlg.StartsWith = true;
			SetSearchString("bit");

			Assert.AreEqual(true, FindInfo.FindFirst(true));
			Assert.AreEqual(true, IsCurrCellLocation(3,0)); // bitter
			Assert.AreEqual(true, FindInfo.Find(true));
			Assert.AreEqual(true, IsCurrCellLocation(1,0)); // bit
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(3,0)); // bitter
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Testing "when the user hits the end of the Find loop with a 'Find Next',
		/// gets the info popup, and then switches direction with a 'Find Prev'."
		/// See FindInfo.Find()
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FindForForBackTest()
		{
			SetSearchString("glebay");

			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(2,3)); // glebay
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(2, 3)); // glebay
			Assert.AreEqual(true, FindInfo.Find(true));
			Assert.AreEqual(true, IsCurrCellLocation(2,3)); // glebay
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Testing "when the user hits the end of the Find loop with a 'Find Prev',
		/// gets the info popup, and then switches direction with a 'Find Next'."
		/// See FindInfo.Find()
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FindBackBackForTest()
		{
			SetSearchString("glebay");

			Assert.AreEqual(true, FindInfo.FindFirst(true));
			Assert.AreEqual(true, IsCurrCellLocation(2, 3)); // glebay
			Assert.AreEqual(true, FindInfo.Find(true));
			Assert.AreEqual(true, IsCurrCellLocation(2, 3)); // glebay
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(2, 3)); // glebay
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Testing "Match Case"
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MatchCaseTest()
		{
			SetSearchString("glfib");
			m_findDlg.MatchCase = false;
			Assert.IsTrue(FindInfo.Find(false));
			Assert.IsTrue(IsCurrCellLocation(0, 3));

			m_findDlg.MatchCase = true;

			SetSearchString("glfib");
			Assert.IsFalse(FindInfo.FindFirst(false));

			SetSearchString("GLFIB");
			Assert.IsTrue(FindInfo.Find(false));
			Assert.IsTrue(IsCurrCellLocation(0, 3));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Testing "Match Entire Word"
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MatchEntireWordTest()
		{
			SetSearchString("bit");
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 0));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 3));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(3, 0));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(3, 3));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(4, 0));	// drillbit
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(4, 8));	// ccvccbit
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(4, 3));	// drillbit
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(5, 8));	// vcbitcvcvcc
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(5, 3));	// glabitging
			// Looping back to the top so use FindFirst()
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 0));

			ResetStartCell();

			m_findDlg.MatchEntireWord = true;
			SetSearchString("bit");
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 0));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 0));

			Assert.AreEqual(true, FindInfo.FindFirst(true));
			Assert.AreEqual(true, IsCurrCellLocation(1,0));
			Assert.AreEqual(true, FindInfo.Find(true));
			Assert.AreEqual(true, IsCurrCellLocation(1, 0));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Testing "Starts With"
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void StartsWithTest()
		{
			SetSearchString("cvc");
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(0,8));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 8));
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(2, 8));
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(3, 8));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(4, 8));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(5, 8));
			// Looping back to the top so use FindFirst()
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(0, 8));

			m_findDlg.StartsWith = true;

			ResetStartCell();
			SetSearchString("cvc");
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(0, 8));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 8));
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(3, 8));
			// Looping back to the top so use FindFirst()
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(0, 8));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Testing "Regular Expression"
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void RegularExpressionTest()
		{
			m_findDlg.MatchCase = true;
			m_findDlg.StartsWith = true;
			SetSearchString("GL");
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(0, 3));

			ResetStartCell();
			m_findDlg.IsRegularExpression = true;
			SetSearchString("(?-i)^GL.*$");
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(0, 3));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Testing finds in sorted field groups.
		/// IT IS PROBABLY BEST TO ALWAYS HAVE THIS TEST LAST.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GroupOnSortedFieldTest()
		{
			// Test data before grouping
			SetSearchString("cvc");
			m_findDlg.MatchEntireWord = true;
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(0, 8)); // cvc
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 8)); // cvc

			m_grid.Sort("Gloss", false);

			// Adds 5 new SilHierarchicalGridRow's & 1 new column
			m_grid.GroupByField = PaApp.FieldInfo["Gloss"];

			ResetStartCell();
			// WordListGroupingBuilder>GroupByField() inserted a hierarchical column for 
			// the + and - glpyhs, so we also have to update the column indexes by
			// passing 'true' to ResetStartSearchCell().
			FindInfo.ResetStartSearchCell(true);

			SetSearchString("cvc");
			m_findDlg.MatchEntireWord = true;
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 9)); // cvc
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(5, 9)); // cvc
			Assert.AreEqual(true, FindInfo.Find(false));

			/// ------------------------------------------------------------------------------------
			/// Testing finds in collapsed & expanded groups.
			/// ------------------------------------------------------------------------------------

			m_grid.ToggleGroupExpansion(false); // collapse all groups
			FindInfo.SearchCollapsedGroups = false;

			ResetStartCell();
			Assert.AreEqual(false, FindInfo.FindFirst(false));
			Assert.AreEqual(false, FindInfo.Find(false));
			Assert.AreEqual(false, FindInfo.Find(false));

			m_grid.ToggleGroupExpansion(true); // expand all groups
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 9)); // cvc
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(5, 9)); // cvc
			Assert.AreEqual(true, FindInfo.Find(false));

			/// ------------------------------------------------------------------------------------
			/// Searching for "cvc" in 2 groups after collapsing one of them.
			/// ------------------------------------------------------------------------------------

			// Collapse 'glbitter' Group
			((SilHierarchicalGridRow)m_grid.Rows[4]).SetExpandedState(false, false);

			ResetStartCell();
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 9));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 9));

			/// ------------------------------------------------------------------------------------
			/// Searching for "cvc" in 2 groups after collapsing one of them, but
			/// searching collapsed groups this time.
			/// ------------------------------------------------------------------------------------
			FindInfo.SearchCollapsedGroups = true;

			ResetStartCell();
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 9));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(5, 9));
		}
		#endregion
	}
}
