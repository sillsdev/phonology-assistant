using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Palaso.IO;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	#region DefaultChartHeadings class
	/// ----------------------------------------------------------------------------------------
	public class DefaultChartHeadings : CharGridPersistence
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the headings from the XML file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool Load(CharGridBuilder chrGridBldr, IPASymbolType chrType)
		{
			var filename = (chrType == IPASymbolType.Consonant ?
				FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, "DefaultConsonantChartHeadings.xml") :
				FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, "DefaultVowelChartHeadings.xml"));

			return Load(chrGridBldr, filename);
		}
	}

	#endregion

	#region CharGridPersistence Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A single instance of one of these will hold all the information to persist the layout
	/// (including all heading/sub-heading changes made by a user and the location of the
	/// phones he may have dragged around) of a vowel or consonant chart.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlRoot("PhoneChart")]
	public class CharGridPersistence
	{
		/// ------------------------------------------------------------------------------------
		internal CharGridPersistence()
		{
			Phones = new List<CharGridCell>();
			ColHeadings = new List<CharGridHeaderPersistenceInfo>();
			RowHeadings = new List<CharGridHeaderPersistenceInfo>();
		}

		#region Properties

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the row header's width.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public int RowHeaderWidth { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the column header's height.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public int ColumnHeaderHeight { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the position of the virtual splitter between row headers and their
		/// sub headers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public int RowSplitPosition { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the position of the virtual splitter between columns headers and their sub
		/// headers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public int ColumnSplitPosition { get; set; }

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the persisted value of whether or not to show uncertain phones in the chart.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool ShowUncertainPhones { get; set; }

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the persisted list of suprasegmentals to ignore.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		[XmlElement("SupraSegmentsToIgnore")]
		public string SupraSegsToIgnore { get; set; }

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the persisted columns headings information for a char grid.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public List<CharGridHeaderPersistenceInfo> ColHeadings { get; set; }

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the persisted row headings information for a char grid.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public List<CharGridHeaderPersistenceInfo> RowHeadings { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the persisted phone information for a char grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<CharGridCell> Phones { get; set; }

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
			var cgp = new CharGridPersistence();
			cgp.RowHeaderWidth = chrGrid.RowHeaderWidth;
			cgp.ColumnHeaderHeight = chrGrid.ColumnHeaderHeight;
			cgp.RowSplitPosition = chrGrid.RowHeadersCollectionPanel.SplitPosition;
			cgp.ColumnSplitPosition = chrGrid.ColumnHeadersCollectionPanel.SplitPosition;
			cgp.ShowUncertainPhones = chrGrid.ShowUncertainPhones;
			cgp.SupraSegsToIgnore = chrGrid.SupraSegsToIgnore;
			cgp.StorePhones(chrGrid, phoneList);
			cgp.StoreHeaders(chrGrid, true);
			cgp.StoreHeaders(chrGrid, false);

			XmlSerializationHelper.SerializeToFile(filename, cgp);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Stores the header information for a set of headers from a CharGrid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void StoreHeaders(CharGrid chrGrid, bool isForColumns)
		{
			var hdrList = (isForColumns ? chrGrid.ColumnHeaders : chrGrid.RowHeaders);

			// Collect the information about each row header.
			foreach (var hdr in hdrList)
			{
				var hdrInfo = new CharGridHeaderPersistenceInfo();
				hdrInfo.SubHeadingsVisible = hdr.SubHeadingsVisible;
				hdrInfo.HeadingText = hdr.ToString();
				hdrInfo.Group = hdr.Group;
				foreach (var lbl in hdr.SubHeaders)
					hdrInfo.SubHeadingTexts.Add(lbl.Text);

				if (isForColumns)
					ColHeadings.Add(hdrInfo);
				else
					RowHeadings.Add(hdrInfo);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Stores the phone information from a CharGrid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void StorePhones(CharGrid chrGrid, ICollection<CharGridCell> phoneList)
		{
			var grid = chrGrid.Grid;
			Phones.Clear();

			// Go through each cell in CharGrid's grid control.
			for (int c = 0; c < grid.Columns.Count; c++)
			{
				for (int r = 0; r < grid.Rows.Count; r++)
				{
					var cgc = grid[c, r].Value as CharGridCell;
					if (cgc != null)
					{
						cgc.Row = r;
						cgc.Column = c;
						cgc.Group = chrGrid.GetRowsGroup(r);
						Phones.Add(cgc);
					}
				}
			}

			if (phoneList != null)
			{
				// Now that all the phones from the grid are stored, go through the rest of the
				// list and make sure invisible phones are also saved.
				foreach (var cgc in phoneList.Where(cgc => !phoneList.Contains(cgc)))
					Phones.Add(cgc);
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

			if (!File.Exists(filename))
				return false;

			var cgp = XmlSerializationHelper.DeserializeFromFile<CharGridPersistence>(filename);
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
			if (chrGridBldr == null || !File.Exists(filename))
				return false;

			var cgp = XmlSerializationHelper.DeserializeFromFile<CharGridPersistence>(filename);
			if (cgp == null || cgp.ColHeadings.Count == 0 || cgp.RowHeadings.Count == 0)
				return false;

			var phoneList = chrGridBldr.InitializeFromPersistedSource(cgp);
			cgp.LoadPhones(phoneList);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public void LoadHeadings(CharGrid chrGrid, bool isForColumns)
		{
			var hdrInfoList = (isForColumns ? ColHeadings : RowHeadings);

			foreach (var hdrInfo in hdrInfoList)
			{
				// Determine the subheading text for the first column under the heading.
				string subheadtext = (hdrInfo.SubHeadingTexts.Count > 0 ?
					hdrInfo.SubHeadingTexts[0] : string.Empty);

				var hdr = (isForColumns ?
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
		private void LoadPhones(IList<CharGridCell> phoneList)
		{
			// Go through the phones deserialized from the XML file.
			foreach (var cgc in Phones)
			{
				bool found = false;
				
				// Look for the same phone in the list passed to us. When we find the phone
				// then use the persisted object to initialize the one in the list passed to us.
				foreach (var cell in phoneList.Where(cell => cgc.Phone == cell.Phone))
				{
					cell.Group = cgc.Group;
					cell.Row = cgc.Row;
					cell.Column = cgc.Column;
					cell.Visible = cgc.Visible;
					found = true;
					break;
				}

				// If the phone in the persisted object cannot be found in the list passed
				// to us, it's likely the phone no longer exists in the data source. In that
				// case, we'll still add the persisted phone to the list passed to us, but
				// it won't be visible.
				if (!found)
				{
					cgc.Visible = false;
					phoneList.Add(cgc);
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
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new CharGridHeaderPersistenceInfo object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeaderPersistenceInfo()
		{
			SubHeadingTexts = new List<string>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the text of a CharGridHeader.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("text")]
		public string HeadingText { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the value of the CharGridHeader's show sub headings value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool SubHeadingsVisible { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the value of the CharGridHeader's group value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public int Group { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the collection of sub heading text associated with a CharGridHeader.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("SubHeading")]
		public List<string> SubHeadingTexts { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the number of sub headings associated with the heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int SubHeadingCount
		{
			get { return SubHeadingTexts.Count; }
		}
	}

	#endregion
}
