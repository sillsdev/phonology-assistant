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
// File: IPaMultiString.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace SIL.FdoToPaInterfaces
{
	#region IPaMultiString interface
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Interface for returning information for a FW multi. string.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IPaMultiString
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the collection of texts and their associated writing systems.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IDictionary<IPaWritingSystem, string> Texts { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the string for the writing system having the specified hvo.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string GetString(int hvoWs);
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the string for the writing system having the specified guid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string GetString(Guid guidWs);
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the string for the specified writing system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string GetString(IPaWritingSystem ws);

	
	}

	#endregion
}
