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
		bool LoadOnlyWritingSystems(string name, string server);
		bool Initialize(string name, string server);
		IEnumerable<IPaWritingSystem> WritingSystems { get; }
		IEnumerable<IPaLexEntry> LexEntries { get; }
        IEnumerable<IPaCustomField> CustomFields { get; }
	}
}
