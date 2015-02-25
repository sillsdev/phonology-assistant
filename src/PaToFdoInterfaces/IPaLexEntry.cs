// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;

namespace SIL.PaToFdoInterfaces
{
	#region IPaLexEntry interface
	/// ----------------------------------------------------------------------------------------
	public interface IPaLexEntry
	{
		/// ------------------------------------------------------------------------------------
		IPaMultiString LexemeForm { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaLexPronunciation> Pronunciations { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaLexSense> Senses { get; }

		/// ------------------------------------------------------------------------------------
		bool ExcludeAsHeadword { get; }

		/// ------------------------------------------------------------------------------------
		string ImportResidue { get; }

		/// ------------------------------------------------------------------------------------
		DateTime DateCreated { get; }

		/// ------------------------------------------------------------------------------------
		DateTime DateModified { get; }

		/// ------------------------------------------------------------------------------------
		IPaCmPossibility MorphType { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString CitationForm { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString SummaryDefinition { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString Etymology { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString Note { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString LiteralMeaning { get; }
		
		/// ------------------------------------------------------------------------------------
		IPaMultiString Bibliography { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString Restrictions { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaMultiString> Allomorphs { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaMultiString> ComplexForms { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaComplexFormInfo> ComplexFormInfo { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaVariant> Variants { get; }
	
		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaVariantOfInfo> VariantOfInfo { get; }

		/// ------------------------------------------------------------------------------------
		Guid Guid { get; }
	}

	#endregion
}
