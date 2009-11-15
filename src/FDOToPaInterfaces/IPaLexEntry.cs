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
// File: IPaLexEntry.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace SIL.FdoToPaInterfaces
{
	#region IPaLexEntry interface
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IPaLexEntry
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the lexeme form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaMultiString LexemeForm { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the lexical entry's pronunciations.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaLexPronunciation> Pronunciations { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the lexical entry's senses.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaLexSense> Senses { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the entry is excluded as a headword.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool ExcludeAsHeadword { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the import residue.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string ImportResidue { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the date created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		DateTime DateCreated { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the date modified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		DateTime DateModified { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the type of the morph.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaCmPossibility MorphType { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the citation form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaMultiString CitationForm { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the variant.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaMultiString Variant { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the type of the variant.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaCmPossibility VariantType { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the variant comment.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaMultiString VariantComment { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the summary definition.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaMultiString SummaryDefinition { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the etymology.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaMultiString Etymology { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the note.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaMultiString Note { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the literal meaning.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaMultiString LiteralMeaning { get; }
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the bibliography.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaMultiString Bibliography { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the restrictions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IPaMultiString Restrictions { get; }
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the alomorphs for the lexical entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaMultiString> Alomorphs { get; }

		//LexEntry[n].Complex Forms
		//LexEntry[n].MorphoSyntaxAnalysis[0].Components[n]
	}

	#endregion
}
