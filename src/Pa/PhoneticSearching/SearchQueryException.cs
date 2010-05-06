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
// File: SearchQueryException.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Text;
using SIL.Localization;
using SIL.Pa.Properties;
using SilUtils;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// An exception class for cells in an XY chart that caused an error when their search
	/// was performed.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SearchQueryException : Exception
	{
		private readonly Exception m_thrownException;
		private readonly string m_queryErrorMsg;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQueryException(SearchQuery query)
		{
			if (query.ErrorMessages.Count > 0)
			{
				var fmt = LocalizationManager.LocalizeString(
					"SearchQuery.ErrListMsg", "{0}) {1}\n\n",
					"This is a format string for a number list of error messages for a search query.",
					App.kLocalizationGroupInfoMsg);

				StringBuilder errors = new StringBuilder();
				for (int i = 0; i < query.ErrorMessages.Count; i++)
					errors.AppendFormat(fmt, i + 1, query.ErrorMessages[i]);

				m_queryErrorMsg = errors.ToString();
			}
			else
			{
				SearchQuery modifiedQuery;
				if (!App.ConvertClassesToPatterns(query, out modifiedQuery, false, out m_queryErrorMsg))
					return;

				SearchEngine.ConvertPatternWithTranscriptionChanges =
					Settings.Default.ConvertPatternsWithTranscriptionChanges;

				SearchEngine engine = new SearchEngine(modifiedQuery.Pattern);

				if (engine.GetWordBoundaryCondition() != SearchEngine.WordBoundaryCondition.NoCondition)
					m_queryErrorMsg = WordBoundaryError;
				else if (engine.GetZeroOrMoreCondition() != SearchEngine.ZeroOrMoreCondition.NoCondition)
					m_queryErrorMsg = ZeroOrMoreError;
				else if (engine.GetOneOrMoreCondition() != SearchEngine.OneOrMoreCondition.NoCondition)
					m_queryErrorMsg = OneOrMoreError;
			}

			if (string.IsNullOrEmpty(m_queryErrorMsg))
				m_queryErrorMsg = "Unknown Error.";

			m_queryErrorMsg = Utils.ConvertLiteralNewLines(m_queryErrorMsg);
			m_queryErrorMsg = m_queryErrorMsg.TrimEnd();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs an XYChartException object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQueryException(Exception thrownException, SearchQuery query)
			: this(query)
		{
			m_thrownException = thrownException;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string WordBoundaryError
		{
			get
			{
				return LocalizationManager.LocalizeString("SearchQuery.WordBoundaryError",
						"The space/word boundary symbol (#) may not be the first or last item in the search item portion (what precedes the slash) of the search pattern. Please correct this and try your search again.",
						App.kLocalizationGroupInfoMsg);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ZeroOrMoreError
		{
			get
			{
				return LocalizationManager.LocalizeString("SearchQuery.ZeroOrMoreError",
					"The zero-or-more symbol (*) was found in an invalid location within the search pattern. The zero-or-more symbol may only be the first item in the preceding environment and/or the last item in the environment after. Please correct this and try your search again.",
					App.kLocalizationGroupInfoMsg);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string OneOrMoreError
		{
			get
			{
				return LocalizationManager.LocalizeString("SearchQuery.OneOrMoreError",
					"The one-or-more symbol (+) was found in an invalid location within the search pattern. The one-or-more symbol may only be the first item in the preceding environment and/or the last item in the environment after. Please correct this and try your search again.",
					App.kLocalizationGroupInfoMsg);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the error thrown by .Net when the search was performed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Exception ThrownException
		{
			get { return m_thrownException; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the error generated by the search engine.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string QueryErrorMessage
		{
			get { return m_queryErrorMsg; }
		}
	}
}
