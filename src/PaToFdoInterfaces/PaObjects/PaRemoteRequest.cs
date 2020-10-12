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

using System.Collections.Generic;
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
		public List<PaWritingSystem> GetWritingSystems()
		{
			return PaWritingSystem.GetWritingSystems(cache.ServiceLocator);
		}

		/// ------------------------------------------------------------------------------------
		public List<PaLexEntry> GetLexEntries()
		{
			return PaLexEntry.GetAll(cache.ServiceLocator);
		}

		/// ------------------------------------------------------------------------------------
		public void ExitProcess()
		{
			System.Windows.Forms.Application.Exit();
		}
	}
}
