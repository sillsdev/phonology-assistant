// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: PaSettingsProvider.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using SilUtils;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Subclass the settings provider so this application can specify the location for
	/// the user settings file.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaSettingsProvider : PortableSettingsProvider
	{
		// This allows tests to specify a temp. location which can be deleted on test cleanup.
		public static string SettingsFileFolder { get; set; }

		public override string SettingsFilePath
		{
			get { return SettingsFileFolder ?? App.DefaultProjectFolder; }
		}
	}
}
