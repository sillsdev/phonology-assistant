using System.Collections.Generic;
using System.Linq;
using SIL.PaToFdoInterfaces;

namespace SIL.FieldWorks.PaObjects
{
	/// ----------------------------------------------------------------------------------------
	public class PaMultiString : IPaMultiString
	{
		/// ------------------------------------------------------------------------------------
		public static PaMultiString Create(dynamic msa, dynamic svcloc)
		{
			return (msa == null || msa.Count == 0 ? null : new PaMultiString(msa, svcloc));
		}

		/// ------------------------------------------------------------------------------------
		public List<string> Texts { get; set; }

		/// ------------------------------------------------------------------------------------
		public List<string> WsIds { get; set; }

		/// ------------------------------------------------------------------------------------
		public PaMultiString()
		{
		}

		/// ------------------------------------------------------------------------------------
        private PaMultiString(dynamic msa, dynamic svcloc)
		{
			Texts = new List<string>(msa.Count);
			WsIds = new List<string>(msa.Count);

			for (int i = 0; i < msa.Count; i++)
			{
				int hvoWs;
				dynamic tss = msa.GetStringFromIndex(i, out hvoWs);
				Texts.Add(tss.Text);

				// hvoWs should *always* be found in AllWritingSystems.
				dynamic ws = null;
                foreach (var w in svcloc.WritingSystems.AllWritingSystems)
                {
                    if (w.Handle == hvoWs)
                    {
                        ws = w;
                        break;
                    }
                }
				WsIds.Add(ws == null ? null : ws.Id);
			}
		}

		#region IPaMultiString Members
		/// ------------------------------------------------------------------------------------
		public string GetString(string wsId)
		{
			if (string.IsNullOrEmpty(wsId))
				return null;

			int i = WsIds.IndexOf(wsId);
			return (i < 0 ? null : Texts[i]);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Texts[0];
		}
	}
}
