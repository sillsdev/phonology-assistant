using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;

namespace SIL.Pa.Filters
{
	#region FilterExpression class
	/// ----------------------------------------------------------------------------------------
	[XmlType("expression")]
	public class FilterExpression
	{
		public static string OtherFilterField = App.GetString(
			"FiltersDlg.FilterExpressionOperators.OtherFilter", "(OTHER FILTER)",
			"Displayed in the filters dialog.");

		private Filter.ExpressionType m_expTypep = Filter.ExpressionType.Normal;
		private string m_pattern;
		private bool m_fieldIsDate;
		private bool m_fieldIsNumeric;
		private bool m_fieldIsFilter;
		private bool m_fieldTypeDetermined;
		private bool m_patternContainsWordBoundaries;
		private SearchQuery m_searchQuery;

		/// ------------------------------------------------------------------------------------
		public FilterExpression()
		{
			Operator = Filter.Operator.Equals;
		}

		/// ------------------------------------------------------------------------------------
		public FilterExpression Clone()
		{
			FilterExpression clone = new FilterExpression();
			clone.FieldName = FieldName;
			clone.Pattern = Pattern;
			clone.Operator = Operator;
			clone.ExpressionType = ExpressionType;
			clone.m_fieldIsDate = m_fieldIsDate;
			clone.m_fieldIsNumeric = m_fieldIsNumeric;
			clone.m_fieldIsFilter = m_fieldIsFilter;
			clone.m_fieldTypeDetermined = m_fieldTypeDetermined;
			clone.m_searchQuery = (m_searchQuery == null ? null : m_searchQuery.Clone());
			return clone;
		}

		/// ------------------------------------------------------------------------------------
		[XmlElement("fieldName")]
		public string FieldName { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("operator")]
		public Filter.Operator Operator { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the pattern used to filter data. When 'Field' is another filter then
		/// Pattern is the name of the other filter. When the 'Type' is a phonetic search
		/// pattern, then this is the the search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("pattern")]
		public string Pattern
		{
			get { return m_pattern; }
			set
			{
				m_pattern = value;
				m_patternContainsWordBoundaries = (value != null && value.IndexOf('#') >= 0);
			}
		}

		/// ------------------------------------------------------------------------------------
		[XmlElement("expressionType")]
		public Filter.ExpressionType ExpressionType
		{
			get { return m_expTypep; }
			set
			{
				m_expTypep = value;
				m_searchQuery = (value == Filter.ExpressionType.PhoneticSrchPtrn ?
					new SearchQuery() : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		[XmlElement("searchQuery")]
		public SearchQuery SearchQuery { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public SearchEngine SearchEngine { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool Matches(WordCacheEntry entry)
		{
			// If this expression is to match the entry against a filter, then find
			// the filter and return whether or not the entry matches the filter.
			if (FieldName == OtherFilterField)
				return MatchesFilter(entry);

			if (m_expTypep == Filter.ExpressionType.PhoneticSrchPtrn)
				return MatchesSearchPattern(entry);

			if (!m_fieldTypeDetermined)
			{
				var field = entry.Project.GetFieldForName(FieldName);
				if (field != null)
				{
					m_fieldIsDate = (field.Type == FieldType.Date);
					m_fieldIsNumeric = (field.Type == FieldType.GeneralNumeric ||
						field.Type == FieldType.AudioLength || field.Type == FieldType.AudioOffset);
					m_fieldTypeDetermined = true;
				}
			}

			string entryValue = (entry[FieldName] ?? string.Empty);

			if (m_expTypep == Filter.ExpressionType.RegExp)
				return Regex.IsMatch(entryValue, m_pattern);

			if (m_fieldIsNumeric || Operator == Filter.Operator.GreaterThan ||
				Operator == Filter.Operator.GreaterThanOrEqual || Operator == Filter.Operator.LessThan ||
				Operator == Filter.Operator.LessThanOrEqual)
			{
				return MatchesNumeric(entryValue);
			}

			if (m_fieldIsDate)
				return MatchesDate(entryValue);

			switch (Operator)
			{
				case Filter.Operator.Matches:
				case Filter.Operator.Equals: return entryValue.Equals(m_pattern, StringComparison.InvariantCulture);
				case Filter.Operator.NotEquals: return !entryValue.Equals(m_pattern, StringComparison.InvariantCulture);
				case Filter.Operator.Contains: return entryValue.Contains(m_pattern);
				case Filter.Operator.DoesNotContain: return !entryValue.Contains(m_pattern);
				case Filter.Operator.BeginsWith: return entryValue.StartsWith(m_pattern, StringComparison.InvariantCulture);
				case Filter.Operator.EndsWith: return entryValue.EndsWith(m_pattern, StringComparison.InvariantCulture);
				case Filter.Operator.DoesNotBeginsWith: return !entryValue.StartsWith(m_pattern, StringComparison.InvariantCulture);
				case Filter.Operator.DoesNotEndsWith: return !entryValue.EndsWith(m_pattern, StringComparison.InvariantCulture);
				case Filter.Operator.PathExists: return File.Exists(entryValue);
				case Filter.Operator.PathDoesNotExist: return !File.Exists(entryValue);
				case Filter.Operator.GreaterThan:
				case Filter.Operator.GreaterThanOrEqual:
				case Filter.Operator.LessThan:
				case Filter.Operator.LessThanOrEqual: return MatchesNumeric(entryValue);
				default: break;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		private bool MatchesFilter(WordCacheEntry entry)
		{
			if (entry.Project.FilterHelper.Filters != null)
			{
				var filter = entry.Project.FilterHelper.GetFilter(m_pattern);
				if (filter != null)
					return filter.Matches(entry);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		private bool MatchesNumeric(string entryValue)
		{
			double num1, num2;

			bool fParsed1 = double.TryParse(entryValue, out num1);
			bool fParsed2 = double.TryParse(m_pattern, out num2);

			if (Operator == Filter.Operator.NotEquals)
			{
				if ((fParsed1 && !fParsed2) || (!fParsed1 && fParsed2))
					return true;
			}

			if (!fParsed1 || !fParsed2)
				return false;

			switch (Operator)
			{
				case Filter.Operator.Matches:
				case Filter.Operator.Equals: return (num1 == num2);
				case Filter.Operator.NotEquals: return (num1 != num2);
				case Filter.Operator.GreaterThan: return (num1 > num2);
				case Filter.Operator.GreaterThanOrEqual: return (num1 >= num2);
				case Filter.Operator.LessThan: return (num1 < num2);
				case Filter.Operator.LessThanOrEqual: return (num1 <= num2);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		private bool MatchesDate(string entryValue)
		{
			DateTime date1, date2;

			bool fParsed1 = DateTime.TryParse(entryValue, out date1);
			bool fParsed2 = DateTime.TryParse(m_pattern, out date2);

			if (Operator == Filter.Operator.NotEquals)
			{
				if ((fParsed1 && !fParsed2) || (!fParsed1 && fParsed2))
					return true;
			}

			if (!fParsed1 || !fParsed2)
				return false;

			switch (Operator)
			{
				case Filter.Operator.Matches:
				case Filter.Operator.Equals: return date1.Equals(date2);
				case Filter.Operator.NotEquals: return !date1.Equals(date2);
				case Filter.Operator.GreaterThan: return (date1 > date2);
				case Filter.Operator.GreaterThanOrEqual: return (date1 >= date2);
				case Filter.Operator.LessThan: return (date1 < date2);
				case Filter.Operator.LessThanOrEqual: return (date1 <= date2);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public bool MatchesSearchPattern(WordCacheEntry entry)
		{
			if (m_searchQuery == null || string.IsNullOrEmpty(m_pattern))
				return false;

			string[][] eticWords = new string[1][];

			if (m_searchQuery.IncludeAllUncertainPossibilities && entry.ContiansUncertainties)
			{
				// Get a list of all the words (each word being in the form of
				// an array of phones) that can be derived from all the primary
				// and non primary uncertainties.
				eticWords = entry.GetAllPossibleUncertainWords(false);
				if (eticWords == null)
					return false;
			}
			else
			{
				// Not all uncertain possibilities should be included in the search, so
				// just load up the phones that only include the primary uncertain Phone(s).
				eticWords[0] = entry.Phones;
				if (eticWords[0] == null)
					return false;
			}

			// If eticWords.Length = 1 then either the word we're searching doesn't contain
			// uncertain phones or it does but they are only primary uncertain phones. When
			// eticWords.Length > 1, we know the uncertain phones in the first word are only
			// primary uncertainities while at least one phone in the remaining words is a
			// non primary uncertainy.
			for (int i = 0; i < eticWords.Length; i++)
			{
				// If the search pattern contains the word breaking character, then add a
				// space at the beginning and end of the array of phones so the word breaking
				// character has something to match at the extremes of the phonetic values.
				if (m_patternContainsWordBoundaries)
				{
					List<string> tmpChars = new List<string>(eticWords[i]);
					tmpChars.Insert(0, " ");
					tmpChars.Add(" ");
					eticWords[i] = tmpChars.ToArray();
				}

				int[] result;
				if (SearchEngine.SearchWord(eticWords[i], out result))
					return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return FieldName + " " + Operator + " '" + Pattern + "'";
		}
	}

	#endregion
}
