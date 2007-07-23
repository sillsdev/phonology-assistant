using System.Collections.Generic;
using System.Xml.Serialization;
using SIL.SpeechTools.Utils;

/// --------------------------------------------------------------------------------------------
/// Contains classes for handling ambiguous sequences. These classes replace what's in the file
/// AmbiguousItemInfo.cs.
/// --------------------------------------------------------------------------------------------
namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AmbiguousSequences : List<AmbiguousSeq>
	{
		public const string kDefaultSequenceFile = "DefaultAmbiguousSequences.xml";
		public const string kSequenceFile = "AmbiguousSequences.xml";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Construct the file name for the project-specific sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string BuildFileName(string projectFileName)
		{
			string filename = (projectFileName ?? string.Empty);
			filename += (filename.EndsWith(".") ? string.Empty : ".") + kSequenceFile;
			return filename;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the default and project-specific ambiguous sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static AmbiguousSequences Load(string projectFileName)
		{
			// Get the default list of sequences.
			AmbiguousSequences defaultList = STUtils.DeserializeData(kDefaultSequenceFile,
				typeof(AmbiguousSequences)) as AmbiguousSequences;

			// Make sure there is a default list.
			if (defaultList == null)
				defaultList = new AmbiguousSequences();

			// Mark the default sequences before adding the project specific ones.
			foreach (AmbiguousSeq seq in defaultList)
				seq.IsDefault = true;

			string filename = BuildFileName(projectFileName);

			// Get the project-specific sequences.
			AmbiguousSequences projectList = STUtils.DeserializeData(filename,
				typeof(AmbiguousSequences)) as AmbiguousSequences;

			if (projectList != null && projectList.Count > 0)
			{
				// If there any sequences are found in the project-specific list that are
				// also found in the default list, then remove them from the default list.
				foreach (AmbiguousSeq seq in projectList)
				{
					int i = defaultList.GetSequenceIndex(seq.Unit);
					if (i >= 0)
						defaultList.RemoveAt(i);
				}

				// Now Combine the project-specific sequences with the default ones.
				defaultList.AddRange(projectList);
			}

			return (defaultList.Count == 0 ? null : defaultList);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an ambiguous sequence to the collection, first making sure the sequence with
		/// the same Unit value is not already in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new void Add(AmbiguousSeq seq)
		{
			if (seq == null)
				return;

			if (!ContainsSeq(seq.Unit, false))
				base.Add(seq);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an ambiguous sequence to the collection with the specified sequence, first
		/// making sure the unit is not already found in one of the sequences in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Add(string unit)
		{
			if (!string.IsNullOrEmpty(unit) && !ContainsSeq(unit, false))
				Add(new AmbiguousSeq(unit));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the list of ambiguous sequences to a project-specific xml file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(string projectFileName)
		{
			// Before saving, make sure there are no empty or null units.
			for (int i = Count - 1; i >= 0; i--)
			{
				if (this[i].Unit == null || this[i].Unit.Trim().Length == 0)
					RemoveAt(i);
			}
			
			string filename = BuildFileName(projectFileName);
			STUtils.SerializeData(filename, this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the specified sequence is in the collection
		/// of ambiguous sequences. When "convert" is true, it means that a match will only be
		/// returned if the item is found and the AmbiguousSeq object's convert flag is true.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ContainsSeq(string seq, bool convert)
		{
			foreach (AmbiguousSeq ambigSeq in this)
			{
				if (ambigSeq.Unit == seq)
					return (convert ? ambigSeq.Convert : true);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the ambiguous sequence matching the specified phone. If the specified phone 
		/// is not an ambiguous sequence, then null is returned. When "convert" is true, it
		/// means that a match will only be returned if the item is found and the AmbiguousSeq
		/// object's Convert flag is true.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AmbiguousSeq GetAmbiguousSeq(string phone, bool convert)
		{
			foreach (AmbiguousSeq ambigSeq in this)
			{
				if (ambigSeq.Unit == phone)
					return (ambigSeq.Convert || !convert ? ambigSeq : null);
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the index of the sequence having the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int GetSequenceIndex(string phone)
		{
			for (int i = 0; i < Count; i++)
			{
				if (this[i].Unit == phone)
					return i;
			}

			return -1;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the number of sequences in the list that are not default sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int NonDefaultCount
		{
			get
			{
				int i = 0;
				foreach (AmbiguousSeq seq in this)
				{
					if (!seq.IsDefault)
						i++;
				}

				return i;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SortByUnitLength()
		{
			for (int i = 0; i < Count; i++)
				this[i].DisplayIndex = i;

			Sort(AmbiguousSeqComparer);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compare method for the length of the units of two ambiguous sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static int AmbiguousSeqComparer(AmbiguousSeq x, AmbiguousSeq y)
		{
			if (x == y || ((x == null || x.Unit == null) && (y == null || y.Unit == null)))
				return 0;

			if (x == null || x.Unit == null)
				return 1;

			if (y == null || y.Unit == null)
				return -1;

			// For items of the same length, this will preserve the order in
			// which the user entered the items in the Phone Inventory view.
			if (x.Unit.Length == y.Unit.Length)
				return x.DisplayIndex.CompareTo(y.DisplayIndex);

			return -(x.Unit.Length.CompareTo(y.Unit.Length));
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("AmbiguousSequence")]
	public class AmbiguousSeq
	{
		[XmlAttribute]
		public string Unit;
		[XmlAttribute]
		public bool Convert = true;
		[XmlAttribute]
		public string BaseChar;
		
		// This flag is only used for sequences that were added
		// from the PhoneticParser in the IPA character cache.
		[XmlAttribute]
		public bool IsProjectDefault = false;

		[XmlIgnore]
		public bool IsDefault = false;
		[XmlIgnore]
		internal int DisplayIndex;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AmbiguousSeq()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AmbiguousSeq(string unit)
		{
			Unit = unit;

			if (string.IsNullOrEmpty(unit))
				return;

			// Find the first base character starting from the end of
			// the string. Make that character the base character for the unit.
			for (int i = unit.Length - 1; i >= 0; i--)
			{
				IPACharInfo charInfo = DataUtils.IPACharCache[unit[i]];
				if (charInfo != null && charInfo.IsBaseChar)
				{
					BaseChar = charInfo.IPAChar;
					return;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Unit;
		}
	}
}
