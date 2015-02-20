// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace SIL.PaToFdoInterfaces
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Provides an interface to deliver a collection of lex. entries from a FieldWorks
	/// project to Phonology Assistant.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IPaLexicalInfo
	{
		bool ShowOpenProject(Form owner, ref Rectangle dialogBounds, ref int dialogSplitterPos, out string name, out string server);
		bool LoadOnlyWritingSystems(string name, string server, int timeToWaitForProcessStart, int timeToWaitForLoadingData);
		bool Initialize(string name, string server, int timeToWaitForProcessStart, int timeToWaitForLoadingData);
		IEnumerable<IPaWritingSystem> WritingSystems { get; }
		IEnumerable<IPaLexEntry> LexEntries { get; }
	}
}
