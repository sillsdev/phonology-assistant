// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace SIL.PaToFdoInterfaces
{
	/// ----------------------------------------------------------------------------------------
	public interface IPaLexPronunciation
	{
		/// ------------------------------------------------------------------------------------
		IPaMultiString Form { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaMediaFile> MediaFiles { get; }
		
		/// ------------------------------------------------------------------------------------
		string CVPattern { get; }

		/// ------------------------------------------------------------------------------------
		string Tone { get; }

		/// ------------------------------------------------------------------------------------
		IPaCmPossibility Location { get; }
	
		/// ------------------------------------------------------------------------------------
		Guid Guid { get; }
	}
}
