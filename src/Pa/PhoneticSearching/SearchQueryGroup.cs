using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Palaso.IO;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.PhoneticSearching
{
	#region SearchQueryGroupList class
	/// ----------------------------------------------------------------------------------------
	[XmlType("SearchQueries")]
	public class SearchQueryGroupList : List<SearchQueryGroup>
	{
		public const string kSearchQueriesFilePrefix = "SearchQueries.xml";

		private PaProject m_project;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is for serialization/deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQueryGroupList()
		{
		}

		/// ------------------------------------------------------------------------------------
		public SearchQueryGroupList(PaProject project)
		{
			m_project = project;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of default search queries.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SearchQueryGroupList LoadDefaults(PaProject project)
		{
			return InternalLoad(project,
				FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, "DefaultSearchQueries.xml"));
		}

		/// ------------------------------------------------------------------------------------
		public static string GetSearchQueryFileForProject(string projectPathFilePrefix)
		{
			return projectPathFilePrefix + kSearchQueriesFilePrefix;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of search queries for the specified project. If project is null
		/// then the default list is loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SearchQueryGroupList Load(PaProject project)
		{
			return InternalLoad(project, GetSearchQueryFileForProject(project.ProjectPathFilePrefix));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of search queries for the specified project. If project is null
		/// then the default list is loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static SearchQueryGroupList InternalLoad(PaProject project, string filename)
		{
			var srchQueries = (File.Exists(filename) ?
				XmlSerializationHelper.DeserializeFromFile<SearchQueryGroupList>(filename) :
				new SearchQueryGroupList(project));

			srchQueries.m_project = project;

			// Sort the items by group name.
			srchQueries.Sort((x, y) => x.Name.CompareTo(y.Name));

			// Assign a unique id to each query. One of the purposes for the id is to
			// mark each of these queries as one that was saved in persisted storage.
			// Also make sure the ignore lists in the queries are modified to include
			// comma delineation.
			int id = 1;
			foreach (var q in srchQueries.Where(grp => grp.Queries != null).SelectMany(grp => grp.Queries))
				q.Id = id++;

			return srchQueries;
		}

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			var filename = GetSearchQueryFileForProject(m_project.ProjectPathFilePrefix);
			XmlSerializationHelper.SerializeToFile(filename, this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the search query whose Id is that of the one specified. If the specified
		/// Id is zero, then null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery GetQueryForId(int id)
		{
			if (id == 0)
				return null;

			return this.Where(grp => grp.Queries != null)
				.SelectMany(grp => grp.Queries).FirstOrDefault(query => query.Id == id);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the group that "owns" the search query whose Id is that of the one specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQueryGroup GetGroupFromQueryId(int id)
		{
			return this.Where(g => g.Queries != null).FirstOrDefault(grp => grp.Queries.Where(q => q.Id == id).Any());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the query in the project's collection with the specified query. The query
		/// updated is the one whose id is the same as the one in the specified query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateQuery(SearchQuery query)
		{
			Debug.Assert(query != null);
			Debug.Assert(query.Id > 0);

			foreach (var grp in this.Where(grp => grp.Queries != null))
			{
				for (int i = 0; i < grp.Queries.Count; i++)
				{
					if (grp.Queries[i].Id == query.Id)
					{
						grp.Queries[i] = query;
						Save();
						return;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the next available (i.e. unused) id that can be assigned to a saved query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int NextAvailableId
		{
			get
			{
				int id = 1;
				foreach (var grp in this.Where(g => g.Queries != null))
				{
					foreach (var query in grp.Queries.Where(q => q.Id == id))
					{
						id++;
						continue;
					}
				}

				return id;
			}
		}
	}

	#endregion

	#region SearchQueryGroup class
	/// ----------------------------------------------------------------------------------------
	[XmlType("QueryGroup")]
	public class SearchQueryGroup
	{
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Name { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool Expanded { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool ExpandedInPopup { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlArray]
		public List<SearchQuery> Queries { get; set; }

		#endregion
	}

	#endregion
}
