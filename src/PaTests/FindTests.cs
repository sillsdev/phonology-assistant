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
using System.Linq;
using NUnit.Framework;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Model;
using SIL.Pa.Processing;
using SIL.Pa.TestUtils;
using SIL.Pa.UI.Controls;
using SIL.Pa.UI.Dialogs;
using SilTools;

namespace SIL.Pa.Tests
{
	[TestFixture]
	public class FindTests : TestBase
	{
		#region Declarations
		private RecordCache m_recCache;
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
		public override void FixtureSetup()
		{
			base.FixtureSetup();

			m_dataSource = new PaDataSource();
			m_dataSource.Type = DataSourceType.Toolbox;
			m_dataSource.ParseType = DataSourceParseType.Interlinear;
			m_dataSource.FieldMappings = new List<FieldMapping>();
			m_dataSource.FieldMappings.Add(new FieldMapping("\\ph", _prj.GetPhoneticField(), true));
			m_dataSource.FieldMappings.Add(new FieldMapping("\\cv", _prj.Fields.Single(f => f.Name == "CVPattern"), true));
			m_dataSource.FieldMappings.Add(new FieldMapping("\\gl", _prj.Fields.Single(f => f.Name == "Gloss"), true));
			_prj.DataSources = new List<PaDataSource>(new[] { m_dataSource });

			FindInfo.ShowMessages = false;
			FwDBAccessInfo.ShowMsgOnFileLoadFailure = false;
			FwQueries.ShowMsgOnFileLoadFailure = false;
		}

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
				"cvc cvc vcvc cvccvc ccvcc vccvcvcc",
				"GLFIB glbitter glebay glbitter drillbit glabitging");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Build the PaWordListGrid for searching.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Initialize(string eticWrds, string cvWrds, string glossWrds)
		{
			m_recCache = _prj.RecordCache;
			m_cache = new WordListCache();

			AddWords(eticWrds, cvWrds, glossWrds);

			// Create grid
			m_grid = new PaWordListGrid(m_cache, GetType(), false);
			SetField(m_grid, "m_suspendSavingColumnChanges", true);

			// Make all the grid's rows & columns visible and thus searchable
			for (int row = 0; row < m_grid.Rows.Count; row++)
			{
				m_grid.Rows[row].Visible = true;
				for (int col = 0; col < m_grid.Columns.Count; col++)
					m_grid.Columns[col].Visible = true;
			}

			// Get rid of columns we don't care about for the tests.
			for (int i = m_grid.ColumnCount - 1; i >= 0; i--)
			{
				if (!"Phonetic CVPattern Gloss".Contains(m_grid.Columns[i].Name))
					m_grid.Columns.Remove(m_grid.Columns[i]);
			}

			// Set the CVPattern & Gloss column Display Indexes to the correct order
			// based on the columnsToSearch below.
			m_grid.Columns["Phonetic"].DisplayIndex = 0;
			m_grid.Columns["CVPattern"].DisplayIndex = 1;
			m_grid.Columns["Gloss"].DisplayIndex = 2;

			SetField(typeof(FindInfo), "s_reverseFind", false);
			SetField(m_grid, "m_suspendSavingColumnChanges", false);
			FindInfo.Grid = m_grid;

			// Add columns to search
			var columnsToSearch = new List<FindDlgColItem>();
			columnsToSearch.Add(new FindDlgColItem(m_grid.Columns["Phonetic"].Index, 0, "Phonetic", "Phonetic"));
			columnsToSearch.Add(new FindDlgColItem(m_grid.Columns["CVPattern"].Index, 1, "CV Pattern", "CVPattern"));
			columnsToSearch.Add(new FindDlgColItem(m_grid.Columns["Gloss"].Index, 2, "Gloss", "Gloss"));
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
			var entry = new RecordCacheEntry(_prj);
			entry.SetValue(kPhonetic, words);
			entry.SetValue(kCVPattern, pattern);
			entry.SetValue(kGloss, gloss);
			entry.NeedsParsing = true;
			entry.DataSource = m_dataSource;
			m_recCache.Add(entry);
			m_recCache.BuildWordCache(null);

			m_cache.Clear();
			foreach (var wcEntry in m_recCache.WordCache)
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
			SetSearchString("ebay");
			Assert.AreEqual(true, FindInfo.FindFirst(false)); // Forward find
			Assert.AreEqual(true, IsCurrCellLocation(2, 0));

			SetSearchString("glbitter");
			Assert.AreEqual(true, FindInfo.FindFirst(true)); // Reverse find
			Assert.AreEqual(true, IsCurrCellLocation(1, 2));
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
			Assert.AreEqual(true, IsCurrCellLocation(2,2)); // finds "glebay"

			SetSearchString("glbitter");
			Assert.AreEqual(true, FindInfo.FindFirst(false)); // Reverse find
			Assert.AreEqual(true, IsCurrCellLocation(3,2));
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
			Assert.AreEqual(true, IsCurrCellLocation(4, 0));

			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(4, 2));
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
			Assert.AreEqual(true, IsCurrCellLocation(4, 2));

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
			Assert.IsTrue(IsCurrCellLocation(1, 2), "Curr. Cell Location is not 1,3");
			Assert.IsTrue(FindInfo.Find(false), "Did not find next forward"); // Forward find
			Assert.IsTrue(IsCurrCellLocation(3, 2), "Curr. Cell Location is not 3,3");
			Assert.IsTrue(FindInfo.Find(true), "Did not find backward"); // Backward find
			Assert.IsTrue(IsCurrCellLocation(1, 2), "Curr. Cell Location is not 1,3");
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
			Assert.AreEqual(true, IsCurrCellLocation(2, 2)); // glebay
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(2, 2)); // glebay
			Assert.AreEqual(true, FindInfo.Find(true));
			Assert.AreEqual(true, IsCurrCellLocation(2, 2)); // glebay
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
			Assert.AreEqual(true, IsCurrCellLocation(2, 2)); // glebay
			Assert.AreEqual(true, FindInfo.Find(true));
			Assert.AreEqual(true, IsCurrCellLocation(2, 2)); // glebay
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(2, 2)); // glebay
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
			Assert.IsTrue(IsCurrCellLocation(0, 2));

			m_findDlg.MatchCase = true;

			SetSearchString("glfib");
			Assert.IsFalse(FindInfo.FindFirst(false));

			SetSearchString("GLFIB");
			Assert.IsTrue(FindInfo.Find(false));
			Assert.IsTrue(IsCurrCellLocation(0, 2));
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
			Assert.AreEqual(true, IsCurrCellLocation(1, 2));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(3, 0));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(3, 2));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(4, 0));	// drillbit
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(4, 2));	// drillbit
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(5, 2));	// glabitging
			
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
			Assert.AreEqual(true, IsCurrCellLocation(0, 1));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 1));
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(3, 1));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(4, 1));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(5, 1));
			
			// Looping back to the top so use FindFirst()
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(0, 1));

			m_findDlg.StartsWith = true;

			ResetStartCell();
			SetSearchString("cvc");
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(0, 1));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 1));
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(3, 1));
			
			// Looping back to the top so use FindFirst()
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(0, 1));
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
			Assert.AreEqual(true, IsCurrCellLocation(0, 2));

			ResetStartCell();
			m_findDlg.IsRegularExpression = true;
			SetSearchString("(?-i)^GL.*$");
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(0, 2));
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
			Assert.AreEqual(true, IsCurrCellLocation(0, 1)); // cvc
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 1)); // cvc

			m_grid.Sort("Gloss", false);

			// Adds 5 new SilHierarchicalGridRow's & 1 new column
			m_grid.GroupByField = _prj.Fields.Single(f => f.Name == "Gloss");

			ResetStartCell();
			// WordListGroupingBuilder>GroupByField() inserted a hierarchical column for 
			// the + and - glpyhs, so we also have to update the column indexes by
			// passing 'true' to ResetStartSearchCell().
			FindInfo.ResetStartSearchCell(true);

			SetSearchString("cvc");
			m_findDlg.MatchEntireWord = true;
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 2)); // cvc
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(6, 2)); // cvc
			Assert.AreEqual(true, FindInfo.Find(false));

			// ------------------------------------------------------------------------------------
			// Testing finds in collapsed & expanded groups.
			// ------------------------------------------------------------------------------------

			m_grid.ToggleGroupExpansion(false); // collapse all groups
			FindInfo.SearchCollapsedGroups = false;

			ResetStartCell();
			Assert.AreEqual(false, FindInfo.FindFirst(false));
			Assert.AreEqual(false, FindInfo.Find(false));
			Assert.AreEqual(false, FindInfo.Find(false));

			m_grid.ToggleGroupExpansion(true); // expand all groups
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 2)); // cvc
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(6, 2)); // cvc
			Assert.AreEqual(true, FindInfo.Find(false));

			// ------------------------------------------------------------------------------------
			// Searching for "cvc" in 2 groups after collapsing one of them.
			// ------------------------------------------------------------------------------------

			// Collapse 'glbitter' Group
			((SilHierarchicalGridRow)m_grid.Rows[4]).SetExpandedState(false, false);

			ResetStartCell();
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 2));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 2));

			// ------------------------------------------------------------------------------------
			// Searching for "cvc" in 2 groups after collapsing one of them, but
			// searching collapsed groups this time.
			// ------------------------------------------------------------------------------------
			FindInfo.SearchCollapsedGroups = true;

			ResetStartCell();
			Assert.AreEqual(true, FindInfo.FindFirst(false));
			Assert.AreEqual(true, IsCurrCellLocation(1, 2));
			Assert.AreEqual(true, FindInfo.Find(false));
			Assert.AreEqual(true, IsCurrCellLocation(6, 2));
		}

		#endregion
	}
}
