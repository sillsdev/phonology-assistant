using System.Collections.Generic;
using System.Resources;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel.Design;
using System.Reflection;

namespace SIL.Localize.LocalizingUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public enum TranslationStatus
	{
		Untranslated,
		Unreviewed,
		Completed
	}
	
	#region ResourceInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ResourceInfo
	{
		private string m_resourceName = null;
		private bool m_omitted = false;
		private List<ResourceEntry> m_stringEntries = null;
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ResourceInfo()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ResourceInfo(string resName)
		{
			m_resourceName = resName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ResourceEntry this[string stringId]
		{
			get
			{
				foreach (ResourceEntry entry in m_stringEntries)
				{
					if (entry.StringId == stringId)
						return entry;
				}

				return null;
			}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string ResourceName
		{
			get { return m_resourceName; }
			set { m_resourceName = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool Omitted
		{
			get { return m_omitted; }
			set { m_omitted = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<ResourceEntry> StringEntries
		{
			get { return m_stringEntries; }
			set { m_stringEntries = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool AnyTranslatedStrings
		{
			get
			{
				if (m_stringEntries != null)
				{
					foreach (ResourceEntry entry in m_stringEntries)
					{
						if (!string.IsNullOrEmpty(entry.TargetText))
							return true;
					}
				}

				return false;
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Sort()
		{
			m_stringEntries.Sort(LocalizingHelper.ResourceEntryComparer);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal string BuildBinaryResourceFile(string cultureId, string outputPath)
		{
			if (string.IsNullOrEmpty(outputPath) || string.IsNullOrEmpty(cultureId) || !AnyTranslatedStrings)
				return null;

			string binResFile = m_resourceName + "." + cultureId + ".resources";
			binResFile = Path.Combine(outputPath, binResFile);

			using (ResourceWriter writer = new ResourceWriter(binResFile))
			{
				foreach (ResourceEntry entry in m_stringEntries)
				{
					if (!string.IsNullOrEmpty(entry.TargetText))
						writer.AddResource(entry.StringId, entry.TargetText);
				}

				writer.Close();
			}

			return binResFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal bool Merge(ResourceInfo resInfo)
		{
			if (resInfo == null)
				return false;

			bool merged = false;

			foreach (ResourceEntry entry in resInfo.StringEntries)
			{
				ResourceEntry matchingValue = this[entry.StringId];
				if (matchingValue == null)
				{
					m_stringEntries.Add(entry);
					merged = true;
				}
			}

			return merged;
		}
	}

	#endregion

	#region ResourceEntry class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ResourceEntry
	{
		[XmlAttribute]
		public string StringId = null;
		[XmlAttribute]
		public TranslationStatus TranslationStatus = TranslationStatus.Untranslated;
		public string SourceText = null;
		public string TargetText = null;
		public string Comment = null;
	}

	#endregion
}