// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
//  
// File: IPaCmPossibility.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------

namespace SIL.PaToFdoInterfaces
{
	#region IPaCmPossibility interface
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Provides an interface to deliver a information about a CmPossibility from a FieldWorks
	/// project to Phonology Assistant.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IPaCmPossibility
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of abbreviations keyed on writing system hvo.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaMultiString Abbreviation { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of names keyed on writing system hvo.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaMultiString Name { get; }
	}

	#endregion
}
