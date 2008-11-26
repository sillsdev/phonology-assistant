using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SIL.Localize.LocalizingUtils
{
	#region ResourceCatalog class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ResourceCatalog
	{
		private string m_applicationName;
		private List<CSharpProject> m_projectList = new List<CSharpProject>();
		private Dictionary<string, string> m_commentDictionary;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string ApplicationName
		{
			get { return m_applicationName; }
			set { m_applicationName = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("CSharpProject")]
		public List<CSharpProject> ProjectList
		{
			get { return m_projectList; }
			set { m_projectList = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Count
		{
			get { return m_projectList.Count; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Add(CSharpProject project)
		{
			m_projectList.Add(project);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Add(IEnumerable<CSharpProject> projectList)
		{
			m_projectList.AddRange(projectList);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Remove(CSharpProject project)
		{
			m_projectList.Remove(project);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveAt(int index)
		{
			m_projectList.RemoveAt(index);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="resource"></param>
		/// <param name="stringId"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public string GetComment(string assembly, string resource, string stringId)
		{
			string key = assembly + "." + resource + "." + stringId;
			string comment;
			return (m_commentDictionary != null &&
				m_commentDictionary.TryGetValue(key, out comment) ? comment : null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildCommentDictionary()
		{
			m_commentDictionary = new Dictionary<string,string>();

			foreach (CSharpProject prj in m_projectList)
			{
				foreach (ResXCatalog resXCat in prj.ResXCatalogList)
				{
					foreach (ResXCatalogEntry entry in resXCat.CatalogEntryList)
					{
						string key = prj.AssemblyName + "." + resXCat.ResXName + "." + entry.id;
						m_commentDictionary[key] = entry.comment;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmntCatPath"></param>
		/// ------------------------------------------------------------------------------------
		public void Save(string cmntCatPath)
		{
			LocalizingHelper.SerializeData(cmntCatPath, this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmntCatPath"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static ResourceCatalog Load(string cmntCatPath)
		{
			ResourceCatalog catalog = LocalizingHelper.DeserializeData(cmntCatPath,
				typeof(ResourceCatalog)) as ResourceCatalog;
			
			if (catalog != null)
				catalog.BuildCommentDictionary();

			return catalog;
		}
	}

	#endregion

	#region CSharpProject class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CSharpProject
	{
		private string m_assemblyName;
		private List<ResXCatalog> m_resXCatalogList = new List<ResXCatalog>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string AssemblyName
		{
			get { return m_assemblyName; }
			set { m_assemblyName = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("Resource")]
		public List<ResXCatalog> ResXCatalogList
		{
			get { return m_resXCatalogList; }
			set { m_resXCatalogList = value; }
		}
	}

	#endregion

	#region ResXCatalog class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ResXCatalog
	{
		[XmlAttribute("Name")]
		public string ResXName;

		[XmlElement("StringEntry")]
		public List<ResXCatalogEntry> CatalogEntryList = new List<ResXCatalogEntry>();
	}

	#endregion

	#region ResXCatalogEntry class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ResXCatalogEntry
	{
		[XmlAttribute]
		public string id;
		public string value;
		public string comment;
	}

	#endregion
}
