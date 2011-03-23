using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using SilTools;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("transcriptionChanges")]
	public class TranscriptionChanges : List<TranscriptionChange>
	{
		public const string kFileName = "TranscriptionChanges.xml";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the project's transcription changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static TranscriptionChanges Load(string projectPathPrefix)
		{
			string filename = GetFileForProject(projectPathPrefix);

			var transChanges = XmlSerializationHelper.DeserializeFromFile<TranscriptionChanges>(filename);

			// This should never need to be done, but just in case there are entries in
			// the list whose source transcription (i.e. Item) is null, remove them from
			// the list since those entries will cause problems further down the line.
			if (transChanges != null)
			{
				for (int i = transChanges.Count - 1; i >= 0; i--)
				{
					if (transChanges[i].WhatToReplace == null)
						transChanges.RemoveAt(i);
				}
			}

			return (transChanges ?? new TranscriptionChanges());
		}

		/// ------------------------------------------------------------------------------------
		public static string GetFileForProject(string projectPathPrefix)
		{
			return projectPathPrefix + kFileName;
		}

		/// ------------------------------------------------------------------------------------
		public void Save(string pathPrefix)
		{
			XmlSerializationHelper.SerializeToFile(pathPrefix + kFileName, this);
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool AnyTranscriptionChanges
		{
			get { return (Count > 0); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of the items to convert. The returned list's key is what to replace
		/// and the key's value is what to replace it with.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Dictionary<string, string> ConversionList
		{
			get
			{
				if (TrueForAll(x => x.ReplaceWith == null))
					return null;

				// First, sort the list of items to convert on the length of the item
				// to convert to -- longest to shortest.
				List<TranscriptionChange> tmpList = new List<TranscriptionChange>();

				// Copy the TranscriptionChange references.
				for (int i = 0; i < Count; i++)
				{
					this[i].DisplayIndex = i;
					tmpList.Add(this[i]);
				}

				// Now order the items.
				tmpList.Sort(TransChangeComparer);

				// Now put the sorted items in a list whose keys are what
				// to convert from and whose values are what to convert to.
				Dictionary<string, string> list = new Dictionary<string, string>();
				foreach (TranscriptionChange item in tmpList)
				{
					string convertToItem = item.ReplaceWith;
					if (item.WhatToReplace != null && convertToItem != null)
						list[item.WhatToReplace] = convertToItem;
				}
				
				return (list.Count > 0 ? list : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compare method for the length of the item to convert from.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static int TransChangeComparer(TranscriptionChange x, TranscriptionChange y)
		{
			if (x == y || ((x == null || x.WhatToReplace == null) &&
				(y == null || y.WhatToReplace == null)))
			{
				return 0;
			}

			if (x == null || x.WhatToReplace == null)
				return 1;

			if (y == null || y.WhatToReplace == null)
				return -1;

			// For items of the same length, this will preserve the order in
			// which the user entered the items in the Phone Inventory view.
			if (x.WhatToReplace.Length == y.WhatToReplace.Length)
				return x.DisplayIndex.CompareTo(y.DisplayIndex);

			return -(x.WhatToReplace.Length.CompareTo(y.WhatToReplace.Length));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts all the specified text to contain the experimental transcriptions that
		/// are marked for converting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Convert(string text)
		{
			Dictionary<string, string> actualReplacements;
			return Convert(text, out actualReplacements);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches the specified text and replaces all the occurrances of the "WhatToReplace"
		/// strings (from each transcription change in the list) with the "ReplaceWith"
		/// strings (from each transcription change in the list).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Convert(string text, out Dictionary<string, string> actualReplacements)
		{
			if (ConversionList == null || ConversionList.Count == 0)
			{
				actualReplacements = null;
				return text;
			}

			char token = (char)1;
			var transAndMarkerInfo = new Dictionary<char, KeyValuePair<string, string>>();

			// This loop will go through the phonetic string and replace each occurance of
			// an experimental transcription with a single character token. Each experimental
			// transcription receives a unique token which is later replaced by the experimantal
			// transcription itself.
			foreach (var kvp in ConversionList)
			{
				if (kvp.Key != null && kvp.Value != null &&
					text.IndexOf(kvp.Key, StringComparison.Ordinal) >= 0)
				{
					text = text.Replace(kvp.Key, token.ToString());
					transAndMarkerInfo[token] = kvp;
					token++;
				}
			}

			actualReplacements = new Dictionary<string, string>();
			
			// Now replace each token with it's experimental transcription
			// and build a list of the conversions that were actually made.
			foreach (var kvp in transAndMarkerInfo)
			{
				if (text.IndexOf(kvp.Key) >= 0)
				{
					string convertTo = kvp.Value.Value;
					text = text.Replace(kvp.Key.ToString(), convertTo);
					actualReplacements[kvp.Value.Key] = convertTo;
				}
			}

			if (actualReplacements.Count == 0)
				actualReplacements = null;

			return text;
		}
	}

	/// ----------------------------------------------------------------------------------------
	[XmlType("change")]
	public class TranscriptionChange
	{
		private string m_replaceWith;

		/// ------------------------------------------------------------------------------------
		public TranscriptionChange()
		{
			ReplacementOptions = new List<ReplacementOption>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the experimental transcription to be converted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("findWhat")]
		public string WhatToReplace { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the current item to which the experimental transcription will be
		/// converted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("replaceWith")]
		public string ReplaceWith
		{
			get
			{
				return (ReplacementOptions.Any(x => x.Literal == m_replaceWith) ?
					m_replaceWith : null);
			}
			set
			{
				m_replaceWith = (value != null ? value.Trim() : value);
				if (m_replaceWith == string.Empty)
					m_replaceWith = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of possible replacement transcriptions. (The setting is
		/// mainly for XML deserialization.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("replacementOption")]
		public List<ReplacementOption> ReplacementOptions { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		internal int DisplayIndex { get; set; }

		/// ------------------------------------------------------------------------------------
		public void SetReplacementOptions(List<string> replacementOptions)
		{
			ReplacementOptions.Clear();

			foreach (var option in replacementOptions)
				ReplacementOptions.Add(new ReplacementOption { Literal = option });
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return WhatToReplace + (string.IsNullOrEmpty(ReplaceWith) ?
				" will not be replaced" : " -> " + ReplaceWith);
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class ReplacementOption
	{
		[XmlAttribute("literal")]
		public string Literal { get; set; }

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Literal;
		}
	}
}
