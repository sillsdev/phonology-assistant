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
// File: IPaWritingSystem.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;

namespace SIL.FdoToPaInterfaces
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
		string ICULocale { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the writing system in the user default writing system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string Name { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the guid of the writing system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		Guid Guid { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the hvo of the writing system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		int Hvo { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the default body text font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string DefaultBodyFont { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the default serif font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string DefaultSerifFont { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the default sans serif font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string DefaultSansSerifFont { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the default mono space font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string DefaultMonoSpaceFont { get; }
	}
	
	#endregion
}
