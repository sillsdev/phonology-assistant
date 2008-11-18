using System.Collections.Generic;
using System.Resources;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel.Design;

namespace SIL.Localize.LocalizingUtils
{
	#region RessourceInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class RessourceInfo
	{
		private string m_resourceName = null;
		private List<ResourceEntry> m_stringEntries = null;
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RessourceInfo()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RessourceInfo(string resPath, string resFileName)
		{
			m_resourceName = resFileName;
			m_stringEntries = ReadStringEntries(resPath);
		}

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
		private List<ResourceEntry> ReadStringEntries(string resXPath)
		{
			List<ResourceEntry> stringEntries = new List<ResourceEntry>();

			ResXResourceReader reader = new ResXResourceReader(resXPath);
			if (reader == null)
				return stringEntries;

			reader.UseResXDataNodes = true;
			IDictionaryEnumerator dict = reader.GetEnumerator();

			while (dict.MoveNext())
			{
				ResXDataNode node = dict.Value as ResXDataNode;
				if (node == null || node.Name.StartsWith(">>"))
					continue;

				try
				{
					object value = node.GetValue((ITypeResolutionService)null);
					if (value != null && value.GetType() == typeof(string))
					{
						ResourceEntry entry = new ResourceEntry();
						entry.Comment = node.Comment;
						entry.StringId = node.Name;
						entry.SourceText = value as string;
						stringEntries.Add(entry);
					}
				}
				catch { }
			}

			reader.Close();
			return stringEntries;
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
		public string SourceText = null;
		public string TargetText = null;
		public string Comment = null;
	}

	#endregion
}