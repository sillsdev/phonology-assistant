// Copyright (c) 2011-2013 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)
//
// File: PaRemoteRequest.cs
// Responsibility: Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------

using SIL.LCModel;

namespace SIL.PaToFdoInterfaces
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaRemoteRequest
	{
	    public LcmCache cache;
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether [is same project] [the specified name].
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShouldWait(string name, string server)
		{
		    return false;
		}

		/// ------------------------------------------------------------------------------------
		public bool IsMyProject(string name, string server)
		{
		    return true;
		}

		/// ------------------------------------------------------------------------------------
		public string GetWritingSystems()
		{
			return PaWritingSystem.GetWritingSystemsAsXml(cache.ServiceLocator);
		}

		/// ------------------------------------------------------------------------------------
		public string GetLexEntries()
		{
			return PaLexEntry.GetAllAsXml(cache.ServiceLocator);
		}

		/// ------------------------------------------------------------------------------------
		public void ExitProcess()
		{
			System.Windows.Forms.Application.Exit();
		}
	}
}
