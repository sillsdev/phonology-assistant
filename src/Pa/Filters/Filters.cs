// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using SIL.Pa.Model;

namespace SIL.Pa.Filters
{
	#region Filter class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a single filter
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("filter")]
	public class Filter
	{
		#region Enumerations
		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public enum Operator
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
            DoesNotMatches,
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

		#endregion
		
		//public Dictionary<FilterOperator, string> FilterUIOperatorStrings;

		/// ------------------------------------------------------------------------------------
		public Filter()
		{
			ShowInToolbarList = true;
			Expressions = new List<FilterExpression>();
			MatchAny = true;
		}

		/// ------------------------------------------------------------------------------------
		public Filter Clone()
		{
			Filter clone = new Filter();
			clone.Name = Name;
			clone.MatchAny = MatchAny;
			clone.Expressions = Expressions.ToList();
			return clone;
		}

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("name")]
		public string Name { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("showInToolbarList")]
		public bool ShowInToolbarList { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("matchAny")]
		public bool MatchAny { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("expression")]
		public List<FilterExpression> Expressions { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool Matches(WordCacheEntry entry)
		{
			bool match = false;

			foreach (var expression in Expressions)
			{
				if (expression.ExpressionType == ExpressionType.PhoneticSrchPtrn && 
					expression.SearchEngine == null)
				{
					expression.SearchEngine = entry.Project.FilterHelper.CheckSearchQuery(expression.SearchQuery, false);
					if (expression.SearchEngine == null)
						return false;
				}

				match = expression.Matches(entry);

				// If the entry matches and we're logically OR'ing the expressions,
				// then we're done here. The entry matches the filter.
				if (match && MatchAny)
					return true;

				// If the entry didn't match and the expressions are logically AND'd,
				// then we're done here. The entry doesn't match the filter.
				if (!match && !MatchAny)
					return false;
			}

			return match;
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}
	}

	#endregion
}
