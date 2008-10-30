using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using SIL.SpeechTools.Utils;
using System.Text.RegularExpressions;

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
		Matches,
		GreaterThan,
		GreaterThanOrEqual,
		LessThan,
		LessThanOrEqual,
		Contains,
		DoesNotContain,
		BeginsWith,
		EndsWith,
		DoesNotBeginsWith,
		DoesNotEndsWith,
		PathExists,
		PathDoesNotExist,
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public enum ExpressionType
	{
		Normal,
		RegExp,
		PhoneticSrchPtrn
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
		private bool m_fOrExpressions = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFilter Clone()
		{
			PaFilter clone = new PaFilter();
			clone.m_name = m_name;
			clone.m_fOrExpressions = m_fOrExpressions;
			foreach (FilterExpression expression in m_expressions)
				clone.Expressions.Add(expression.Clone());

			return clone;
		}

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
		public bool OrExpressions
		{
			get { return m_fOrExpressions; }
			set { m_fOrExpressions = value; }
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
			FilterHelper.Restore();
			WordCache cache = PaApp.WordCache;

			if (cache != null)
			{
				for (int i = cache.Count - 1; i >= 0; i--)
				{
					if (!Matches(cache[i]))
					{
						FilterHelper.UnusedWordsCache.Add(cache[i]);
						cache.RemoveAt(i);
					}
				}
			}

			FilterHelper.FilterApplied(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Matches(WordCacheEntry entry)
		{
			bool match = false;

			foreach (FilterExpression expression in m_expressions)
			{
				match = expression.Matches(entry);

				// If the entry matches and we're logically OR'ing the expressions,
				// then we're done here. The entry matches the filter.
				if (match && m_fOrExpressions)
					return true;

				// If the entry didn't match and the expressions are logically AND'd,
				// then we're done here. The entry doesn't match the filter.
				if (!match && !m_fOrExpressions)
					return false;
			}
		
			return match;
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
		public static string OtherFilterField = Properties.Resources.kstidOtherFilterFieldName;

		private string m_fieldName;
		private string m_pattern;
		private FilterOperator m_operator = FilterOperator.Equals;
		private ExpressionType m_expTypep = ExpressionType.Normal;
		private bool m_fieldIsDate = false;
		private bool m_fieldIsNumeric = false;
		private bool m_fieldIsFilter = false;
		private bool m_fieldTypeDetermined = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FilterExpression Clone()
		{
			FilterExpression clone = new FilterExpression();
			clone.m_fieldName = m_fieldName;
			clone.m_pattern = m_pattern;
			clone.m_operator = m_operator;
			clone.m_expTypep = m_expTypep;
			clone.m_fieldIsDate = m_fieldIsDate;
			clone.m_fieldIsNumeric = m_fieldIsNumeric;
			clone.m_fieldIsFilter = m_fieldIsFilter;
			clone.m_fieldTypeDetermined = m_fieldTypeDetermined;
			return clone;
		}

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
		public ExpressionType ExpressionType
		{
			get { return m_expTypep; }
			set { m_expTypep = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Matches(WordCacheEntry entry)
		{
			// If this expression is to match the entry against a filter, then find
			// the filter and return whether or not the entry matches the filter.
			if (m_fieldName == OtherFilterField)
				return MatchesFilter(entry);

			if (!m_fieldTypeDetermined)
			{
				PaFieldInfo fieldInfo = PaApp.FieldInfo[m_fieldName];
				if (fieldInfo != null)
				{
					m_fieldIsDate = fieldInfo.IsDate;
					m_fieldIsNumeric = fieldInfo.IsNumeric ||
						fieldInfo.IsAudioLength || fieldInfo.IsAudioOffset;
					m_fieldTypeDetermined = true;
				}
			}

			string entryValue = (entry[m_fieldName] ?? string.Empty);

			if (m_expTypep == ExpressionType.RegExp)
				return Regex.IsMatch(entryValue, m_pattern);

			if (m_fieldIsNumeric || m_operator == FilterOperator.GreaterThan ||
				m_operator == FilterOperator.GreaterThanOrEqual || m_operator == FilterOperator.LessThan ||
				m_operator == FilterOperator.LessThanOrEqual)
			{
				return MatchesNumeric(entryValue);
			}

			if (m_fieldIsDate)
				return MatchesDate(entryValue);

			switch (m_operator)
			{
				case FilterOperator.Equals: return entryValue.Equals(m_pattern, StringComparison.InvariantCulture);
				case FilterOperator.NotEquals: return !entryValue.Equals(m_pattern, StringComparison.InvariantCulture);
				case FilterOperator.Contains: return entryValue.Contains(m_pattern);
				case FilterOperator.DoesNotContain: return !entryValue.Contains(m_pattern);
				case FilterOperator.BeginsWith: return entryValue.StartsWith(m_pattern, StringComparison.InvariantCulture);
				case FilterOperator.EndsWith: return entryValue.EndsWith(m_pattern, StringComparison.InvariantCulture);
				case FilterOperator.DoesNotBeginsWith: return !entryValue.StartsWith(m_pattern, StringComparison.InvariantCulture);
				case FilterOperator.DoesNotEndsWith: return !entryValue.EndsWith(m_pattern, StringComparison.InvariantCulture);
				case FilterOperator.PathExists: return File.Exists(entryValue);
				case FilterOperator.PathDoesNotExist: return !File.Exists(entryValue);
				case FilterOperator.GreaterThan:
				case FilterOperator.GreaterThanOrEqual:
				case FilterOperator.LessThan:
				case FilterOperator.LessThanOrEqual:
					return MatchesNumeric(entryValue);
				
				default: break;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool MatchesFilter(WordCacheEntry entry)
		{
			if (FilterHelper.FilterList != null)
			{
				foreach (PaFilter filter in FilterHelper.FilterList)
				{
					if (m_pattern == filter.Name)
						return filter.Matches(entry);
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool MatchesNumeric(string entryValue)
		{
			double num1, num2;

			bool fParsed1 = double.TryParse(entryValue, out num1);
			bool fParsed2 = double.TryParse(m_pattern, out num2);

			if (m_operator == FilterOperator.NotEquals)
			{
				if ((fParsed1 && !fParsed2) || (!fParsed1 && fParsed2))
					return true;
			}

			if (!fParsed1 || !fParsed2)
				return false;

			switch (m_operator)
			{
				case FilterOperator.Equals: return (num1 == num2);
				case FilterOperator.NotEquals: return (num1 != num2);
				case FilterOperator.GreaterThan: return (num1 > num2);
				case FilterOperator.GreaterThanOrEqual: return (num1 >= num2);
				case FilterOperator.LessThan: return (num1 < num2);
				case FilterOperator.LessThanOrEqual: return (num1 <= num2);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool MatchesDate(string entryValue)
		{
			DateTime date1, date2;

			bool fParsed1 = DateTime.TryParse(entryValue, out date1);
			bool fParsed2 = DateTime.TryParse(m_pattern, out date2);

			if (m_operator == FilterOperator.NotEquals)
			{
				if ((fParsed1 && !fParsed2) || (!fParsed1 && fParsed2))
					return true;
			}

			if (!fParsed1 || !fParsed2)
				return false;

			switch (m_operator)
			{
				case FilterOperator.Equals: return date1.Equals(date2);
				case FilterOperator.NotEquals: return !date1.Equals(date2);
				case FilterOperator.GreaterThan: return (date1 > date2);
				case FilterOperator.GreaterThanOrEqual: return (date1 >= date2);
				case FilterOperator.LessThan: return (date1 < date2);
				case FilterOperator.LessThanOrEqual: return (date1 <= date2);
			}

			return false;
		}
	}

	#endregion
}
