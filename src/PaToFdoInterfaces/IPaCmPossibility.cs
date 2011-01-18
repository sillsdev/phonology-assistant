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
