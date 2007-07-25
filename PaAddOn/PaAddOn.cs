using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.Pa.Controls;
using SIL.Pa;
using SIL.Pa.Data;
using SIL.Pa.Dialogs;
using SIL.Pa.FFSearchEngine;
using SIL.SpeechTools.Utils;
using SIL.FieldWorks.Common.UIAdapters;
using XCore;

namespace SIL.Pa.AddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaAddOn : IxCoreColleague
	{
		private readonly Mediator m_mediator;
		private PaMainWnd m_mainWnd;
		private DataCorpusWnd m_dataCorpusWnd;
		private FindPhoneWnd m_findPhoneWnd;
		private ConsonantChartWnd m_consonantChartWnd;
		private VowelChartWnd m_vowelChartWnd;
		private XYChartWnd m_xyChartWnd;
		private PhoneInventoryWnd m_phoneInventoryWnd;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaAddOn()
		{
			PaApp.AddMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewOpened(object args)
		{
			try
			{
				if (args.GetType() == typeof(PaMainWnd))
					m_mainWnd = args as PaMainWnd;
				else if (args.GetType() == typeof(DataCorpusWnd))
					m_dataCorpusWnd = args as DataCorpusWnd;
				else if (args.GetType() == typeof(FindPhoneWnd))
					m_findPhoneWnd = args as FindPhoneWnd;
				else if (args.GetType() == typeof(ConsonantChartWnd))
					m_consonantChartWnd = args as ConsonantChartWnd;
				else if (args.GetType() == typeof(VowelChartWnd))
					m_vowelChartWnd = args as VowelChartWnd;
				else if (args.GetType() == typeof(XYChartWnd))
					m_xyChartWnd = args as XYChartWnd;
				else if (args.GetType() == typeof(PhoneInventoryWnd))
					m_phoneInventoryWnd = args as PhoneInventoryWnd;
			}
			catch { }

			return false;
		}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			List<IxCoreColleague> targets = new List<IxCoreColleague>();
			targets.Add(this);
			return (targets.ToArray());
		}

		#endregion
	}
}
