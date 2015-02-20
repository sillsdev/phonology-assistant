// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
// File: IPaWritingSystem.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;

namespace SIL.PaToFdoInterfaces
{
	#region IPaWritingSystem interface
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Provides an interface to deliver information about a single FieldWorks writing
	/// systems to Phonology Assistant.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IPaWritingSystem
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the two-letter ICU locale of the writing system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string IcuLocale { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the ISO-blah, blah code.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string Id { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the writing system in the user default writing system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string DisplayName { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the writing system language name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string LanguageName { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the writing system abbreviation.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string Abbreviation { get; }
			
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the hvo of the writing system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		int Hvo { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the writing system is a vernacular writing
		/// system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool IsVernacular { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the writing system is a analysis writing
		/// system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool IsAnalysis { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the writing system is the default
		/// vernacular writing system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool IsDefaultVernacular { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the writing system is the default
		/// analysis writing system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool IsDefaultAnalysis { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the writing system is the default
		/// pronunciation writing system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool IsDefaultPronunciation { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the default body text font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string DefaultFontName { get; }
	}
	
	#endregion
}
