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
		private List<ResourceEntry> m_stringEntries;
		private AssemblyResourceInfo m_owningAssembly;
		
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
			m_stringEntries = new List<ResourceEntry>();
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ResourceEntry this[int i]
		{
			get { return (i >= 0 && i < m_stringEntries.Count ? m_stringEntries[i] : null); }
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AssemblyResourceInfo OwningAssembly
		{
			get { return m_owningAssembly; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int StringEntryCount
		{
			get { return m_stringEntries.Count; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void SetBackReferences(AssemblyResourceInfo assembly)
		{
			m_owningAssembly = assembly;

			if (m_stringEntries == null)
				return;

			foreach (ResourceEntry entry in m_stringEntries)
			{
				entry.OwningAssembly = assembly;
				entry.OwningResource = this;
			}
		}

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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return m_resourceName;
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
		public bool Omitted = false;
		[XmlAttribute]
		public TranslationStatus TranslationStatus = TranslationStatus.Untranslated;
		public string SourceText = null;
		public string TargetText = null;
		public string Comment = null;

		private AssemblyResourceInfo m_owningAssembly;
		private ResourceInfo m_OwningResource;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public AssemblyResourceInfo OwningAssembly
		{
			get { return m_owningAssembly; }
			internal set { m_owningAssembly = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public ResourceInfo OwningResource
		{
			get { return m_OwningResource; }
			internal set { m_OwningResource = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return StringId;
		}
	}

	#endregion
}