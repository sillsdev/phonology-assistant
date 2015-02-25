// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
namespace SIL.PaToFdoInterfaces
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
		/// Gets the string for the writing system having the specified Id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string GetString(string wsId);
	}

	#endregion
}
