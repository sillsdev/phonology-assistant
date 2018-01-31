// Copyright (c) 2015 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System.Collections.Generic;
using System.Linq;
using SIL.LCModel.Core.KernelInterfaces;
using SIL.LCModel;
using SIL.PaToFdoInterfaces;

namespace SIL.FieldWorks.PaObjects
{
	/// ----------------------------------------------------------------------------------------
	public class PaMultiString : IPaMultiString
	{
		/// ------------------------------------------------------------------------------------
		public static PaMultiString Create(ITsMultiString msa, ILcmServiceLocator svcloc)
		{
			return (msa == null || msa.StringCount == 0 ? null : new PaMultiString(msa, svcloc));
		}

		/// <summary>
		/// Append to the PaMultiString the contents of the tsMultiString.
		/// For each writing system the contents will end up in a comma seperated list
		/// </summary>
		public static void Append(PaMultiString paMultiString, ITsMultiString tsMultiString, ILcmServiceLocator svcloc)
		{
			for (int i = 0; i < tsMultiString.StringCount; ++i)
			{
				int hvoWs;
				var tss = tsMultiString.GetStringFromIndex(i, out hvoWs);

				// hvoWs should *always* be found in AllWritingSystems.
				var ws = svcloc.WritingSystems.AllWritingSystems.SingleOrDefault(w => w.Handle == hvoWs);
				paMultiString.AddString(ws == null ? null : ws.Id, tss.Text);
			}
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
		private PaMultiString(ITsMultiString msa, ILcmServiceLocator svcloc)
		{
			Texts = new List<string>(msa.StringCount);
			WsIds = new List<string>(msa.StringCount);

			for (int i = 0; i < msa.StringCount; i++)
			{
				int hvoWs;
				ITsString tss = msa.GetStringFromIndex(i, out hvoWs);
				Texts.Add(tss.Text);

				// hvoWs should *always* be found in AllWritingSystems.
				var ws = svcloc.WritingSystems.AllWritingSystems.SingleOrDefault(w => w.Handle == hvoWs);
				WsIds.Add(ws == null ? null : ws.Id);
			}
		}

		private void AddString(string ws, string text)
		{
			if (Texts == null)
			{
				Texts = new List<string>();
				WsIds = new List<string>();
			}
			var index = WsIds.IndexOf(ws);
			if (index >= 0)
			{
				Texts[index] = Texts[index] + string.Format(", {0}", text);
			}
			else
			{
				Texts.Add(text);
				WsIds.Add(ws);
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
