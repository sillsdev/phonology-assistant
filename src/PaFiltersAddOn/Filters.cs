using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.FiltersAddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public enum FilterOperator
	{
		Equals,
		NotEquals,
	}

	#region FiltersList class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("PaFilters")]
	public class PaFiltersList : List<PaFilter>
	{
		public const string kFiltersFilePrefix = "Filters.xml";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the filter with the specified name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFilter this[string filterName]
		{
			get
			{
				foreach (PaFilter filter in this)
				{
					if (filter.Name == filterName)
						return filter;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of filters for the currently loaded project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaFiltersList Load()
		{
			return Load(PaApp.Project);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of filters for the specified project. If the project is
		/// null, then an empty list is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaFiltersList Load(PaProject project)
		{
			string filename = (project != null ?
				project.ProjectPathFilePrefix + kFiltersFilePrefix : null);

			PaFiltersList filtersList = null;

			if (File.Exists(filename))
			{
				filtersList = STUtils.DeserializeData(filename,
					typeof(PaFiltersList)) as PaFiltersList;
			}

			return (filtersList ?? new PaFiltersList());
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
				string filename = project.ProjectPathFilePrefix + kFiltersFilePrefix;
				STUtils.SerializeData(filename, this);
			}
		}
	}

	#endregion

	#region PaFilter class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a single filter
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("Filter")]
	public class PaFilter
	{
		private string m_name;
		private List<FilterExpression> m_expressions = new List<FilterExpression>();

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
		public List<FilterExpression> Expressions
		{
			get { return m_expressions; }
			set { m_expressions = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ApplyFilter()
		{
			ReclamationBucket.Restore();
			WordCache cache = PaApp.WordCache;
			if (cache != null)
			{
				for (int i = cache.Count - 1; i >= 0; i--)
				{
					foreach (FilterExpression expression in m_expressions)
					{
						WordCacheEntry entry = cache[i];
						if (!expression.Matches(cache[i]))
						{
							ReclamationBucket.UnusedWordsCache.Add(cache[i]);
							cache.RemoveAt(i);
						}
					}
				}
			}

			ReclamationBucket.UpdateViews();
		}
	}

	#endregion

	#region FilterElement class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("Expression")]
	public class FilterExpression
	{
		private string m_fieldName;
		private string m_pattern;
		private FilterOperator m_operator;
		private bool m_isRegExp = false;
		private bool m_isAndedWithNext = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FieldName
		{
			get { return m_fieldName; }
			set { m_fieldName = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Pattern
		{
			get { return m_pattern; }
			set	{ m_pattern = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool IsRegExpression
		{
			get { return m_isRegExp; }
			set { m_isRegExp = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool IsAndedWithNext
		{
			get { return m_isAndedWithNext; }
			set { m_isAndedWithNext = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FilterOperator Operator
		{
			get { return m_operator; }
			set { m_operator = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Matches(WordCacheEntry entry)
		{
			switch (m_operator)
			{
				case FilterOperator.Equals:
					if (entry[m_fieldName] == m_pattern)
						return true;

					break;

				default:
					break;
			}

			return false;
		}
	}

	#endregion
}
