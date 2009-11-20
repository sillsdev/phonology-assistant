// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright from='2009' to='2009' company='SIL International'>
//		Copyright (c) 2009, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: UndefinedCharClasses.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;

namespace SIL.Pa.Data
{
	#region UndefinedPhoneticCharactersInfoList class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class UndefinedPhoneticCharactersInfoList : List<UndefinedPhoneticCharactersInfo>
	{
		public string CurrentDataSourceName { get; set; }
		public string CurrentReference { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an individual code point that is not defined in the IPA character cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Add(char c, string transcription)
		{
			if (string.IsNullOrEmpty(transcription))
				return;

			UndefinedPhoneticCharactersInfo ucpInfo = new UndefinedPhoneticCharactersInfo();
			ucpInfo.Character = c;
			ucpInfo.SourceName = CurrentDataSourceName;
			ucpInfo.Reference = CurrentReference;
			ucpInfo.Transcription = transcription;
			Add(ucpInfo);
		}
	}
	
	#endregion

	#region UndefinedPhoneticCharactersInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Contains information about a code point read from a data source that cannot be found
	/// in the IPA character cache.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class UndefinedPhoneticCharactersInfo
	{
		public char Character { get; set; }
		public string Transcription { get; set; }
		public string SourceName { get; set; }
		public string Reference { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Character.ToString();
		}
	}

	#endregion
}
