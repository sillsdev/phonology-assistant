// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
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

namespace SIL.Pa.Model
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
