using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using SilUtils;

namespace SIL.Pa.PhoneticSearching
{
	#region SearchQueryGroupList class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("SearchQueries")]
	public class SearchQueryGroupList : List<SearchQueryGroup>
	{
		public const string kSearchQueriesFilePrefix = "SearchQueries.xml";
		public const string kDefaultSearchQueriesFile = "DefaultSearchQueries.xml";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of default search queries.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SearchQueryGroupList Load()
		{
			return Load(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of search queries for the specified project. If project is null
		/// then the default list is loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SearchQueryGroupList Load(PaProject project)
		{
			string filename = (project != null ?
				project.ProjectPathFilePrefix + kSearchQueriesFilePrefix :
				Path.Combine(PaApp.ConfigFolder, kDefaultSearchQueriesFile));

			SearchQueryGroupList srchQueries = null;

			if (File.Exists(filename))
			{
				srchQueries = Utils.DeserializeData(filename,
					typeof(SearchQueryGroupList)) as SearchQueryGroupList;
			}

			if (srchQueries == null)
				return new SearchQueryGroupList();

			// Sort the items by group name.
			SortedList<string, SearchQueryGroup> sortedGroups =
				new SortedList<string, SearchQueryGroup>();

			foreach (SearchQueryGroup grp in srchQueries)
				sortedGroups[grp.Name] = grp;

			srchQueries = new SearchQueryGroupList();
			foreach (SearchQueryGroup grp in sortedGroups.Values)
				srchQueries.Add(grp);

			// Assign a unique id to each query. One of the purposes for the id is to
			// mark each of these queries as one that was saved in persisted storage.
			// Also make sure the ignore lists in the queries are modified to include
			// comma delineation.
			int id = 1;
			foreach (SearchQueryGroup grp in srchQueries)
			{
				if (grp.Queries != null)
				{
					foreach (SearchQuery query in grp.Queries)
						query.Id = id++;
				}
			}

			return srchQueries;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			Save(PaApp.Project);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(PaProject project)
		{
			if (project != null)
			{
				string filename = project.ProjectPathFilePrefix + kSearchQueriesFilePrefix;
				Utils.SerializeData(filename, this);
			}
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

			foreach (SearchQueryGroup grp in this)
			{
				if (grp.Queries == null)
					continue;

				foreach (SearchQuery query in grp.Queries)
				{
					if (query.Id == id)
						return query;
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the group that "owns" the search query whose Id is that of the one specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQueryGroup GetGroupFromQueryId(int id)
		{
			foreach (SearchQueryGroup grp in this)
			{
				if (grp.Queries == null)
					continue;

				foreach (SearchQuery query in grp.Queries)
				{
					if (query.Id == id)
						return grp;
				}
			}

			return null;
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

			foreach (SearchQueryGroup grp in this)
			{
				if (grp.Queries == null)
					continue;

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
				foreach (SearchQueryGroup grp in this)
				{
					if (grp.Queries == null)
						continue;

					foreach (SearchQuery query in grp.Queries)
					{
						if (query.Id == id)
						{
							id++;
							continue;
						}
					}
				}

				return id;
			}
		}
	}

	#endregion

	#region SearchQueryGroup class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("QueryGroup")]
	public class SearchQueryGroup
	{
		private string m_name;
		private List<SearchQuery> m_queries;
		private bool m_expanded;
		private bool m_expandedInPopup;

		public override string ToString()
		{
			return m_name;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool Expanded
		{
			get { return m_expanded; }
			set { m_expanded = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool ExpandedInPopup
		{
			get { return m_expandedInPopup; }
			set { m_expandedInPopup = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray]
		public List<SearchQuery> Queries
		{
			get { return m_queries; }
			set { m_queries = value; }
		}

		#endregion
	}

	#endregion
}
