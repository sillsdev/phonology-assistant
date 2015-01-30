// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: FilterHelper.cs
// Responsibility: Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Palaso.IO;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SilTools;

namespace SIL.Pa.Filters
{
	#region FilterHelper static class
	/// ------------------------------------------------------------------------------------
	public class FilterHelper
	{
		public const string kFiltersFilePrefix = "Filters.xml";

		private readonly PaProject m_project;

		/// ------------------------------------------------------------------------------------
		public Filter CurrentFilter { get; private set; }

		/// ------------------------------------------------------------------------------------
		public List<Filter> Filters { get; private set; }

		/// ------------------------------------------------------------------------------------
		public FilterHelper(PaProject project)
		{
			m_project = project;
			Load();
		}

		#region Loading and saving filters
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of filters for the specified project. If the file containing a
		/// project's filters does not exist, then an empty list is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			var filename = m_project.ProjectPathFilePrefix + kFiltersFilePrefix;

			if (File.Exists(filename))
				Filters = XmlSerializationHelper.DeserializeFromFile<List<Filter>>(filename, "filters");
			else
			{
                var defaultFilterFileName = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, "DefaultFilters.xml");
                Filters = XmlSerializationHelper.DeserializeFromFile<List<Filter>>(defaultFilterFileName, "filters");
            }

			if (Filters == null)
				Filters = new List<Filter>();
			else
				Filters.Sort((x, y) => x.Name.CompareTo(y.Name));
		}

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			var filename = m_project.ProjectPathFilePrefix + kFiltersFilePrefix;
			XmlSerializationHelper.SerializeToFile(filename, Filters, "filters");
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the current filter to the one having the specified name. If apply is true,
		/// then that filter is immediately applies. If setting the current filter succeeds,
		/// then true is returned. Otherwise false is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SetCurrentFilter(string filterName, bool apply)
		{
			var filter = GetFilter(filterName);
			if (filter == null)
				return false;

			CurrentFilter = filter;
			if (apply)
				ApplyFilter(filter);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public Filter GetFilter(string filterName)
		{
			return Filters.FirstOrDefault(x => x.Name == filterName);
		}

		/// ------------------------------------------------------------------------------------
		public void TurnOffCurrentFilter()
		{
			TurnOffCurrentFilter(true);
		}

		/// ------------------------------------------------------------------------------------
		public void TurnOffCurrentFilter(bool apply)
		{
			if (apply)
				ApplyFilter(null);
			else
				CurrentFilter = null;

			App.MsgMediator.SendMessage("FilterTurnedOff", apply);
		}

		/// ------------------------------------------------------------------------------------
		public bool EntryMatchesCurrentFilter(WordCacheEntry entry)
		{
			return (CurrentFilter == null ? true : CurrentFilter.Matches(entry));
		}

		/// ------------------------------------------------------------------------------------
		public void ApplyFilter(Filter filter)
		{
			ApplyFilter(filter, false);
		}

		/// ------------------------------------------------------------------------------------
		public void ApplyFilter(Filter filter, bool forceReapplication)
		{
			if (m_project.RecordCache == null || (CurrentFilter == filter && !forceReapplication))
				return;

			CurrentFilter = filter;
			m_project.RecordCache.BuildFilteredWordCache();
			m_project.Save();
			App.MsgMediator.SendMessage("DataSourcesModified", m_project);
			App.MsgMediator.SendMessage("FilterChanged", filter);
		}

		/// ------------------------------------------------------------------------------------
		public SearchEngine CheckSearchQuery(SearchQuery query, bool showErrMsg)
		{
			var validator = new SearchQueryValidator(m_project);
			if (!validator.GetIsValid(query) && showErrMsg)
			{
				Utils.MsgBox(SearchQueryValidationError.GetSingleStringErrorMsgFromListOfErrors(query.Pattern, validator.Errors));
				return null;
			}

			SearchEngine.IgnoreUndefinedCharacters = m_project.IgnoreUndefinedCharsInSearches;
            return new SearchEngine(query, SearchEngine.PhoneCache ?? m_project.PhoneCache);

			//SearchQuery modifiedQuery;
			//if (!App.ConvertClassesToPatterns(query, out modifiedQuery, showErrMsg))
			//    return null;

			//SearchEngine.IgnoreUndefinedCharacters = m_project.IgnoreUndefinedCharsInSearches;
			//var engine = new SearchEngine(modifiedQuery, m_project.PhoneCache ?? SearchEngine.PhoneCache);
			//var msg = modifiedQuery.GetCombinedErrorMessages();

			//if (!string.IsNullOrEmpty(msg))
			//{
			//    if (showErrMsg)
			//        Utils.MsgBox(msg);

			//    query.Errors.AddRange(modifiedQuery.Errors);
			//    return null;
			//}

			//if (!ReflectionHelper.GetBoolResult(typeof(App),
			//    "VerifyMiscPatternConditions", new object[] { engine, showErrMsg }))
			//{
			//    query.Errors.AddRange(modifiedQuery.Errors);
			//    return null;
			//}

			//return engine;
		}

		/// ------------------------------------------------------------------------------------
		public void PostCacheBuildingFinalize()
		{
			CleanUpExpressionSearchEngines(CurrentFilter);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method is used after a filter is applied. It goes through the expressions
		/// and dereferences (so they'll get garbage collected) all the search engines
		/// created by expressions that are based on other filters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CleanUpExpressionSearchEngines(Filter filter)
		{
			if (filter == null)
				return;

			foreach (var expression in filter.Expressions.Where(e => e.ExpressionType == Filter.ExpressionType.PhoneticSrchPtrn))
			{
				CleanUpExpressionSearchEngines(GetFilter(expression.Pattern));
				expression.SearchEngine = null;
			}
		}
	}

	#endregion
}
