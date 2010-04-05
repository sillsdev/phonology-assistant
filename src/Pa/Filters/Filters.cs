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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Filter()
		{
			ShowInToolbarList = true;
			Expressions = new List<FilterExpression>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Filter Clone()
		{
			Filter clone = new Filter();
			clone.Name = Name;
			clone.OrExpressions = OrExpressions;
			clone.Expressions = Expressions.ToList();
			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("name")]
		public string Name { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("showInToolbarList")]
		public bool ShowInToolbarList { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("orExpression")]
		public bool OrExpressions { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("expression")]
		public List<FilterExpression> Expressions { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Matches(WordCacheEntry entry)
		{
			bool match = false;

			foreach (FilterExpression expression in Expressions)
			{
				if (expression.ExpressionType == ExpressionType.PhoneticSrchPtrn &&
					expression.SearchEngine == null)
				{
					expression.SearchEngine = FilterHelper.CheckSearchQuery(expression.SearchQuery, false);
					if (expression.SearchEngine == null)
						return false;
				}

				match = expression.Matches(entry);

				// If the entry matches and we're logically OR'ing the expressions,
				// then we're done here. The entry matches the filter.
				if (match && OrExpressions)
					return true;

				// If the entry didn't match and the expressions are logically AND'd,
				// then we're done here. The entry doesn't match the filter.
				if (!match && !OrExpressions)
					return false;
			}

			return match;
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
	}

	#endregion
}
