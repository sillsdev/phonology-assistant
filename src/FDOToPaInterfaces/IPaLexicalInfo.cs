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
// File: IPaLexicalInfo.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;

namespace SIL.FdoToPaInterfaces
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Provides an interface to deliver a collection of lex. entries from a FieldWorks
	/// project to Phonology Assistant.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IPaLexicalInfo
	{
		void Initialize(string filename);
		IEnumerable<IPaWritingSystem> WritingSystems { get; }
		IEnumerable<IPaLexEntry> LexEntries { get; }
	}
}
