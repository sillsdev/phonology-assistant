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
// File: IPaLexSense.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace SIL.PaToFdoInterfaces
{
	#region IPaLexSense interface
	/// ----------------------------------------------------------------------------------------
	public interface IPaLexSense
	{
		/// ------------------------------------------------------------------------------------
		IPaMultiString Gloss { get; }

		/// ------------------------------------------------------------------------------------
		IPaCmPossibility SenseType { get; }

		/// ------------------------------------------------------------------------------------
		IPaCmPossibility Status { get; }

		/// ------------------------------------------------------------------------------------
		IPaCmPossibility PartOfSpeech { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString Definition { get; }

		/// ------------------------------------------------------------------------------------
		string ScientificName { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString Bibliography { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString AnthropologyNote { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString DiscourseNote { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString GeneralNote { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString GrammarNote { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString PhonologyNote { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString SemanticsNote { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString SociolinguisticsNote { get; }
		
		/// ------------------------------------------------------------------------------------
		IPaMultiString EncyclopedicInfo { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString Restrictions { get; }

		/// ------------------------------------------------------------------------------------
		string Source { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaCmPossibility> Usages { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaCmPossibility> SemanticDomains { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaCmPossibility> AnthroCodes { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaCmPossibility> DomainTypes { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaMultiString> ReversalEntries { get; }

		/// ------------------------------------------------------------------------------------
		string ImportResidue { get; }

		/// ------------------------------------------------------------------------------------
		Guid Guid { get; }
	}

	#endregion
}
