// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Localization;
using Palaso.Reporting;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	public class SearchQueryValidationError
	{
		public string Message { get; set; }
		public List<string> PhonesNotInCache { get; private set; }
		public List<char> SymbolsNotInInventory { get; private set; }
		public List<string> HelpLinks { get; private set; }
		public Exception Exception { get; private set; }

		/// ------------------------------------------------------------------------------------
		public SearchQueryValidationError(string msg)
		{
			Message = msg;
			HelpLinks = new List<string>();
			PhonesNotInCache = new List<string>();
			SymbolsNotInInventory = new List<char>();
		}

		/// ------------------------------------------------------------------------------------
		public SearchQueryValidationError(Exception exception) : this(null as string)
		{
			Exception = exception;
		}

		/// ------------------------------------------------------------------------------------
		public SearchQueryValidationError Copy()
		{
			var error = new SearchQueryValidationError(Message);
			error.HelpLinks.AddRange(HelpLinks);
			error.Exception = Exception;
			error.PhonesNotInCache = PhonesNotInCache.ToList();
			error.SymbolsNotInInventory = SymbolsNotInInventory.ToList();
			return error;
		}

		/// ------------------------------------------------------------------------------------
		public string GetUnknownPhonesDisplayText()
		{
			if (PhonesNotInCache.Count == 0)
				return null;

			var text = PhonesNotInCache
				.Aggregate(string.Empty, (curr, p) => curr + (p == " " ? "(space)" : p + ", "));
			
			return text.TrimEnd(',', ' ');
		}

		/// ------------------------------------------------------------------------------------
		public string GetUnknownSymbolsDisplayText()
		{
			if (SymbolsNotInInventory.Count == 0)
				return null;

			var bldr = new StringBuilder();
			var fmt = LocalizationManager.GetString(
				"PhoneticSearchingMessages.UndefinedSymbolsFormatMsg", "{0} (U+{1:X4}), ",
				"'{1:X4}' displays the second paramter as a four digit hexadecimal number.");

			foreach (var symbol in SymbolsNotInInventory)
				bldr.AppendFormat(fmt, symbol, (int)symbol);

			bldr.Length -= 2;
			return bldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			var bldr = new StringBuilder();

			if (!String.IsNullOrEmpty(Message))
			{
				bldr.AppendLine(Message);
				if (PhonesNotInCache.Count > 0)
					bldr.AppendLine(GetUnknownPhonesDisplayText());
				if (SymbolsNotInInventory.Count > 0)
					bldr.AppendLine(GetUnknownSymbolsDisplayText());
			}
			
			if (Exception != null)
				bldr.AppendLine(ExceptionHelper.GetAllExceptionMessages(Exception));

			return bldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		public static SearchQueryValidationError MakeErrorFromException(Exception e, string pattern)
		{
			var error = new SearchQueryValidationError(e);

			var msg = LocalizationManager.GetString(
				"PhoneticSearchingMessages.UnhandledExceptionWhileValidatingMsg",
				"Validating the pattern '{0}' caused the following exception:");

			error.Message = string.Format(msg, pattern);
			return error;
		}

		/// ------------------------------------------------------------------------------------
		public static string GetSingleStringErrorMsgFromListOfErrors(string pattern,
			IEnumerable<SearchQueryValidationError> errList)
		{
			var errors = errList.ToArray();
			int i = 0;
			var bldr = new StringBuilder(GetHeadingTextForErrorList(pattern));
			bldr.AppendLine();
			bldr.AppendLine();

			foreach (var err in errors.Where(e => e.Exception == null))
			{
				bldr.Append(++i + ") ");
				bldr.AppendLine(err.ToString());
				bldr.AppendLine();
			}

			if (errors.Any(e => e.Exception != null))
			{
				bldr.AppendLine();
				bldr.AppendLine(
					LocalizationManager.GetString("PhoneticSearchingMessages.SearchPatternUnhandledExceptionsSummaryHeading",
						"=============== Exceptions ==============="));
				bldr.AppendLine();
			}

			foreach (var err in errors.Where(e => e.Exception != null))
			{
				bldr.Append(++i + ") ");
				bldr.AppendLine(err.ToString());
				bldr.AppendLine();
				bldr.AppendLine(new string('-', 50));
				bldr.AppendLine();
			}

			return bldr.ToString().Trim();
		}

		/// ------------------------------------------------------------------------------------
		public static string GetHeadingTextForErrorList(string pattern)
		{
			if (pattern != null)
			{
				return String.Format(LocalizationManager.GetString(
					"PhoneticSearchingMessages.SearchPatternErrorSummaryHeading.WithPattern",
					"The search pattern '{0}' contains the following error(s):"), pattern);
			}

			return LocalizationManager.GetString(
				"PhoneticSearchingMessages.SearchPatternErrorSummaryHeading.WithoutPattern",
				"The search pattern contains the following error(s):");
		}
	}
}
