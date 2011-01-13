using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace SIL.FdoToPaInterfaces
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
		bool Initialize(string name, string server, int timeToWaitForProcessStart, int timeToWaitForLoadingData);
		IEnumerable<IPaWritingSystem> WritingSystems { get; }
		IEnumerable<IPaLexEntry> LexEntries { get; }
	}
}
