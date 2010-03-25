using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using SIL.Localization;
using SilUtils;

/// --------------------------------------------------------------------------------------------
/// Contains classes for handling ambiguous sequences. These classes replace what's in the file
/// AmbiguousItemInfo.cs.
/// --------------------------------------------------------------------------------------------
namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("ambiguousSequences")]
	public class AmbiguousSequences : List<AmbiguousSeq>
	{
		public const string kDefaultFileName = "DefaultAmbiguousSequences.xml";
		public const string kFileName = "AmbiguousSequences.xml";

		private static char s_unusedToken;
		
		// The parse tokens list is a hash table of character tokens unique to each ambiguous
		// sequence in the list. They are used when parsing phonetic text containing
		// ambiguous sequences. Before a phonetic transcription is parsed into phones, each
		// ambiguous sequence in the transcription is replaced by a single token. Then the
		// transcription is parsed into phones, the tokens are replaced by the ambiguous
		// sequences they represent.
		private Dictionary<char, string> m_parseTokens;

		#region Method to migrate previous versions of ambiguous sequences file to current.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void MigrateToLatestVersion(string filename)
		{
			var errMsg = LocalizationManager.LocalizeString("Misc.Strings.AmbiguousSeqMigrationErrMsg",
				"The following error occurred while attempting to update the your project’s ambiguous " +
				"sequences file:\n\n{0}\n\nIn order to continue working, your original ambiguous " +
				"sequence file  will be renamed to the following file: '{1}'.",
				"Message displayed when updating ambiguous sequences file to new version.",
				App.kLocalizationGroupMisc, LocalizationCategory.ErrorOrWarningMessage,
				LocalizationPriority.MediumHigh);

			App.MigrateToLatestVersion(filename, Assembly.GetExecutingAssembly(),
				"SIL.Pa.Model.UpdateFileTransforms.UpdateAmbiguousSequenceFile.xslt", errMsg);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the default and project-specific ambiguous sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static AmbiguousSequences Load(string pathPrefix)
		{
			// Get the default list of sequences.
			var defaultFile = Path.Combine(App.ConfigFolder, kDefaultFileName);

			AmbiguousSequences defaultList =
				XmlSerializationHelper.DeserializeFromFile<AmbiguousSequences>(
				defaultFile) ?? new AmbiguousSequences();

			// Make sure there is a default list.

			// Mark the default sequences before adding the project specific ones.
			foreach (AmbiguousSeq seq in defaultList)
				seq.IsDefault = true;

			string filename = pathPrefix + kFileName;
			MigrateToLatestVersion(filename);

			// Get the project-specific sequences.
			var projectList = XmlSerializationHelper.DeserializeFromFile<AmbiguousSequences>(filename);

			if (projectList != null && projectList.Count > 0)
			{
				// If any sequences are found in the project-specific list that are
				// also found in the default list, then remove them from the default list.
				for (int i = projectList.Count - 1; i >= 0; i--)
				{
					int index = defaultList.GetSequenceIndex(projectList[i].Literal);
					if (index >= 0)
						defaultList.RemoveAt(index);
					else if (projectList[i].IsDefault)
					{
						// At this point, we know we've found a sequence in the project list
						// that is marked as a default but it's not also in the default list.
						// Therefore, remove it from the project list since the default set
						// of ambiguous sequences must have changed. This should almost
						// never happen... and probably never happen.
						projectList.RemoveAt(i);
						projectList.Save(pathPrefix);
					}
				}

				// Now Combine the project-specific sequences with the default ones.
				defaultList.AddRange(projectList);
			}

			return (defaultList.Count == 0 ? null : defaultList);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AmbiguousSequences()
		{
			Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AmbiguousSequences(IEnumerable<AmbiguousSeq> list) : base(list)
		{
			m_parseTokens = null;
			s_unusedToken = '\uFFFF';
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new void Clear()
		{
			base.Clear();
			m_parseTokens = null;
			s_unusedToken = '\uFFFF';
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an ambiguous sequence to the collection, first making sure the sequence with
		/// the same Unit value is not already in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new void Add(AmbiguousSeq seq)
		{
			if (seq == null || string.IsNullOrEmpty(seq.BaseChar))
				return;

			if (!ContainsSeq(seq.Literal, false))
			{
				seq.ParseToken = s_unusedToken--;
				base.Add(seq);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new void AddRange(IEnumerable<AmbiguousSeq> collection)
		{
			if (collection != null)
				base.AddRange(collection);
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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new void Insert(int index, AmbiguousSeq seq)
		{
			if (seq == null || string.IsNullOrEmpty(seq.BaseChar))
				return;

			if (!ContainsSeq(seq.Literal, false))
			{
				seq.ParseToken = s_unusedToken--;
				base.Insert(index, seq);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the list of ambiguous sequences to a project-specific xml file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(string pathPrefix)
		{
			var tmpList = new AmbiguousSequences(this);

			// Before saving, make sure there are no empty or null units
			// and get rid of those sequences that were added automatically.
			for (int i = tmpList.Count - 1; i >= 0; i--)
			{
				string unit = tmpList[i].Literal;
				if (unit == null || unit.Trim().Length == 0)
					tmpList.RemoveAt(i);
			}

			XmlSerializationHelper.SerializeToFile(pathPrefix + kFileName, tmpList);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the specified sequence is in the collection
		/// of ambiguous sequences. When "convert" is true, it means that a match will only be
		/// returned if the item is found and the AmbiguousSeq object's convert flag is true.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ContainsSeq(string literal, bool convert)
		{
			var seq = this.FirstOrDefault(x => x.Literal == literal);
			if (seq == null)
				return false;

			return (convert ? seq.Convert : true);
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
				if (ambigSeq.Literal == phone)
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
				if (this[i].Literal == phone)
					return i;
			}

			return -1;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the unit of the ambiguous sequence for the specified parse token.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetAmbigSeqForToken(char token)
		{
			string ambigSeq = null;

			// Create the token list if it hasn't already been built.
			if (m_parseTokens == null)
			{
				m_parseTokens = new Dictionary<char, string>();
				foreach (AmbiguousSeq seq in this)
					m_parseTokens[seq.ParseToken] = seq.Literal;
			}

			m_parseTokens.TryGetValue(token, out ambigSeq);
			return ambigSeq;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// The parse tokens list is a hash table of character tokens unique to each ambiguous
		///// sequence in the list. They are used when parsing phonetic text containing
		///// ambiguous sequences. Before a phonetic transcription is parsed into phones, each
		///// ambiguous sequence in the transcription is replaced by a single token. Then the
		///// transcription is parsed into phones, the tokens are replaced by the ambiguous
		///// sequences they represent.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//internal Dictionary<char, string> ParseTokens
		//{
		//    get { return m_parseTokens; }
		//}

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
			if (x == y || ((x == null || x.Literal == null) && (y == null || y.Literal == null)))
				return 0;

			if (x == null || x.Literal == null)
				return 1;

			if (y == null || y.Literal == null)
				return -1;

			// For items of the same length, this will preserve the order in
			// which the user entered the items in the Phone Inventory view.
			if (x.Literal.Length == y.Literal.Length)
				return x.DisplayIndex.CompareTo(y.DisplayIndex);

			return -(x.Literal.Length.CompareTo(y.Literal.Length));
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("sequence")]
	public class AmbiguousSeq
	{
		[XmlAttribute("literal")]
		public string Literal;

		[XmlAttribute("unit")]
		public bool Convert = true;
	
		[XmlAttribute("primaryBase")]
		public string BaseChar;
		
		// This flag is only used for sequences that were added
		// from the PhoneticParser in the IPA character cache.
		[XmlAttribute("generated")]
		public bool IsGenerated;

		[XmlAttribute("default")]
		public bool IsDefault;

		[XmlIgnore]
		internal int DisplayIndex;
	
		[XmlIgnore]
		internal char ParseToken;

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
			Literal = unit;

			if (string.IsNullOrEmpty(unit))
				return;

			// Find the first base character starting from the end of
			// the string. Make that character the base character for the unit.
			for (int i = unit.Length - 1; i >= 0; i--)
			{
				IPASymbol charInfo = App.IPASymbolCache[unit[i]];
				if (charInfo != null && charInfo.IsBase)
				{
					BaseChar = charInfo.Literal;
					return;
				}
			}

			// If we got this far, then we didn't find a candidate for the base character.
			// In that case, see if any of the characters are not defined in the phonetic
			// character inventory. If so, then use the first one we encounter as the base.
			for (int i = unit.Length - 1; i >= 0; i--)
			{
				if (App.IPASymbolCache[unit[i]] == null)
				{
					BaseChar = unit[i].ToString();
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
			return Literal;
		}
	}
}
