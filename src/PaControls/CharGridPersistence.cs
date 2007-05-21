using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.Windows.Forms;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	#region CharGridPersistence Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A single instance of one of these will hold all the information to persist the layout
	/// (including all heading/sub-heading changes made by a user and the location of the
	/// phones he may have dragged around) of a vowel or consonant chart.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlRoot("PhoneChartInfo")]
	public class CharGridPersistence
	{
		private int m_rowHeaderWidth;
		private int m_colHeaderHeight;
		private int m_rowSplitPosition;
		private int m_colSplitPosition;
		private List<CharGridCell> m_phones;
		private List<CharGridHeaderPersistenceInfo> m_colHeadings;
		private List<CharGridHeaderPersistenceInfo> m_rowHeadings;
		private bool m_showUncertainPhones = false;
		private string m_supraSegsToIgnore = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private CharGridPersistence()
		{
			m_phones = new List<CharGridCell>();
			m_colHeadings = new List<CharGridHeaderPersistenceInfo>();
			m_rowHeadings = new List<CharGridHeaderPersistenceInfo>();
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the row header's width.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public int RowHeaderWidth
		{
			get { return m_rowHeaderWidth; }
			set { m_rowHeaderWidth = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the column header's height.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public int ColumnHeaderHeight
		{
			get { return m_colHeaderHeight; }
			set { m_colHeaderHeight = value; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the position of the virtual splitter between row headers and their
		/// sub headers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public int RowSplitPosition
		{
			get { return m_rowSplitPosition; }
			set { m_rowSplitPosition = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the position of the virtual splitter between columns headers and their sub
		/// headers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public int ColumnSplitPosition
		{
			get { return m_colSplitPosition; }
			set { m_colSplitPosition = value; }
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the persisted value of whether or not to show uncertain phones in the chart.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool ShowUncertainPhones
		{
			get { return m_showUncertainPhones; }
			set { m_showUncertainPhones = value; }
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the persisted list of suprasegmentals to ignore.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		[XmlElement("SupraSegmentsToIgnore")]
		public string SupraSegsToIgnore
		{
			get { return m_supraSegsToIgnore; }
			set { m_supraSegsToIgnore = value; }
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the persisted columns headings information for a char grid.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public List<CharGridHeaderPersistenceInfo> ColHeadings
		{
			get { return m_colHeadings; }
			set { m_colHeadings = value; }
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the persisted row headings information for a char grid.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public List<CharGridHeaderPersistenceInfo> RowHeadings
		{
			get { return m_rowHeadings; }
			set { m_rowHeadings = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the persisted phone information for a char grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<CharGridCell> Phones
		{
			get { return m_phones; }
			set { m_phones = value; }
		}
		
		#endregion

		#region Methods for saving CharGrid to file.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gathers all the information about a character grid that needs to be persisted,
		/// then persists it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Save(CharGrid chrGrid, List<CharGridCell> phoneList, string filename)
		{
			CharGridPersistence cgp = new CharGridPersistence();
			cgp.RowHeaderWidth = chrGrid.RowHeaderWidth;
			cgp.ColumnHeaderHeight = chrGrid.ColumnHeaderHeight;
			cgp.RowSplitPosition = chrGrid.RowHeadersCollectionPanel.SplitPosition;
			cgp.ColumnSplitPosition = chrGrid.ColumnHeadersCollectionPanel.SplitPosition;
			cgp.ShowUncertainPhones = chrGrid.ShowUncertainPhones;
			cgp.SupraSegsToIgnore = chrGrid.SupraSegsToIgnore;
			cgp.StorePhones(chrGrid, phoneList);
			cgp.StoreHeaders(chrGrid, true);
			cgp.StoreHeaders(chrGrid, false);

			STUtils.SerializeData(filename, cgp);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Stores the header information for a set of headers from a CharGrid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void StoreHeaders(CharGrid chrGrid, bool isForColumns)
		{
			List<CharGridHeader> hdrList = (isForColumns ?
				chrGrid.ColumnHeaders : chrGrid.RowHeaders);

			// Collect the information about each row header.
			foreach (CharGridHeader hdr in hdrList)
			{
				CharGridHeaderPersistenceInfo hdrInfo = new CharGridHeaderPersistenceInfo();
				hdrInfo.SubHeadingsVisible = hdr.SubHeadingsVisible;
				hdrInfo.HeadingText = hdr.ToString();
				hdrInfo.Group = hdr.Group;
				foreach (Label lbl in hdr.SubHeaders)
					hdrInfo.SubHeadingTexts.Add(lbl.Text);

				if (isForColumns)
					m_colHeadings.Add(hdrInfo);
				else
					m_rowHeadings.Add(hdrInfo);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Stores the phone information from a CharGrid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void StorePhones(CharGrid chrGrid, List<CharGridCell> phoneList)
		{
			DataGridView grid = chrGrid.Grid;
			m_phones.Clear();

			// Go through each cell in CharGrid's grid control.
			for (int c = 0; c < grid.Columns.Count; c++)
			{
				for (int r = 0; r < grid.Rows.Count; r++)
				{
					CharGridCell cgc = grid[c, r].Value as CharGridCell;
					if (cgc != null)
					{
						cgc.Row = r;
						cgc.Column = c;
						cgc.Group = chrGrid.GetRowsGroup(r);
						m_phones.Add(cgc);
					}
				}
			}

			if (phoneList != null)
			{
				// Now that all the phones from the grid are stored, go through the rest of the
				// list and make sure invisible phones are also saved.
				foreach (CharGridCell cgc in phoneList)
				{
					if (!phoneList.Contains(cgc))
						m_phones.Add(cgc);
				}
			}
		}

		#endregion

		#region Method for loading a CharPickerRows control from persisted source
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializes vowel or consonant chart query returning false if the file doesn't
		/// exist or deserialization fails.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool Load(List<CharGridCell> phoneList, string filename,
			out string ssegsToIgnore)
		{
			ssegsToIgnore = null;

			if (!System.IO.File.Exists(filename))
				return false;

			CharGridPersistence cgp =
				STUtils.DeserializeData(filename, typeof(CharGridPersistence)) as CharGridPersistence;

			if (cgp == null || cgp.ColHeadings.Count == 0 || cgp.RowHeadings.Count == 0)
				return false;

			ssegsToIgnore = cgp.SupraSegsToIgnore;
			cgp.LoadPhones(phoneList);
			return true;
		}

		#endregion

		#region Methods for loading a CharGrid from persisted source
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializes data for a character grid and initializes the grid based on that
		/// information. Returns false if the file doesn't exist or deserialization fails.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool Load(CharGridBuilder chrGridBldr, string filename)
		{
			if (chrGridBldr == null || !System.IO.File.Exists(filename))
				return false;

			CharGridPersistence cgp =
				STUtils.DeserializeData(filename, typeof(CharGridPersistence)) as CharGridPersistence;

			if (cgp == null || cgp.ColHeadings.Count == 0 || cgp.RowHeadings.Count == 0)
				return false;

			List<CharGridCell> phoneList = chrGridBldr.InitializeFromPersistedSource(cgp);
			cgp.LoadPhones(phoneList);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadHeadings(CharGrid chrGrid, bool isForColumns)
		{
			List<CharGridHeaderPersistenceInfo> hdrInfoList = (isForColumns ?
				m_colHeadings : m_rowHeadings);

			foreach (CharGridHeaderPersistenceInfo hdrInfo in hdrInfoList)
			{
				// Determine the subheading text for the first column under the heading.
				string subheadtext = (hdrInfo.SubHeadingTexts.Count > 0 ?
					hdrInfo.SubHeadingTexts[0] : string.Empty);

				CharGridHeader hdr = (isForColumns ?
					chrGrid.AddColumnHeader(hdrInfo.HeadingText, subheadtext) :
					chrGrid.AddRowHeader(hdrInfo.HeadingText, subheadtext));

				hdr.Group = hdrInfo.Group;
				if (hdrInfo.SubHeadingTexts.Count <= 1)
					continue;

				// Add sub headings for the remaining columns or rows beyond the first.
				for (int i = 1; i < hdrInfo.SubHeadingTexts.Count; i++)
				{
					if (isForColumns)
						chrGrid.AddColumnToHeading(hdr, hdrInfo.SubHeadingTexts[i]);
					else
						chrGrid.AddRowToHeading(hdr, hdrInfo.SubHeadingTexts[i]);
				}

				hdr.SubHeadingsVisible = hdrInfo.SubHeadingsVisible;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load up the phones found in the persistence source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadPhones(List<CharGridCell> phoneList)
		{
			// Go through the phones deserialized from the XML file.
			foreach (CharGridCell cgc in m_phones)
			{
				bool found = false;
				
				// Look for the same phone in the list passed to us. When we find the phone
				// then use the persisted object to initialize the one in the list passed to us.
				for (int i = 0; i < phoneList.Count; i++)
				{
					if (cgc.Phone == phoneList[i].Phone)
					{
						phoneList[i].Group = cgc.Group;
						phoneList[i].Row = cgc.Row;
						phoneList[i].Column = cgc.Column;
						phoneList[i].Visible = cgc.Visible;
						found = true;
						break;
					}
				}

				// If the phone in the persisted object cannot be found in the list passed
				// to us, it's likely the phone no longer exists in the data source. In that
				// case, we'll still add the persisted phone to the list passed to us, but
				// it won't be visible.
				if (!found)
				{
					phoneList.Add(cgc);
					int i = phoneList.Count - 1;
					phoneList[i].Visible = false;
				}
			}
		}

		#endregion
	}

	#endregion

	#region CharGridHeaderPersistenceInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Contains information to persist information about a single CharGridHeader object.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("Heading")]
	public class CharGridHeaderPersistenceInfo
	{
		private bool m_subHeadingsVisible;
		private string m_headingText;
		private int m_group;
		private List<string> m_subHeadingTexts;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new CharGridHeaderPersistenceInfo object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeaderPersistenceInfo()
		{
			m_subHeadingTexts = new List<string>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the text of a CharGridHeader.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("text")]
		public string HeadingText
		{
			get { return m_headingText; }
			set { m_headingText = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the value of the CharGridHeader's show sub headings value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool SubHeadingsVisible
		{
			get { return m_subHeadingsVisible; }
			set { m_subHeadingsVisible = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the value of the CharGridHeader's group value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public int Group
		{
			get { return m_group; }
			set { m_group = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the collection of sub heading text associated with a CharGridHeader.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("SubHeading")]
		public List<string> SubHeadingTexts
		{
			get { return m_subHeadingTexts; }
			set { m_subHeadingTexts = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the number of sub headings associated with the heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int SubHeadingCount
		{
			get { return m_subHeadingTexts.Count; }
		}
	}

	#endregion
}
