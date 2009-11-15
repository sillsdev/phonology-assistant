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
// File: IPaLexPronunciation.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;

namespace SIL.FdoToPaInterfaces
{
	#region IPaLexPronunciation interface
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IPaLexPronunciation
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaMultiString Form { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the media files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaMediaFile> MediaFiles { get; }
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the CV pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string CVPattern { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string Tone { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaCmPossibility Location { get; }
	}

	#endregion
}
