// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
// File: IPaMediaFile.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------

namespace SIL.PaToFdoInterfaces
{
	#region IPaMediaFile interface
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IPaMediaFile
	{
		string AbsoluteInternalPath { get; }
		string InternalPath { get; }
		string OriginalPath { get; }
		IPaMultiString Label { get; }
	}

	#endregion
}
