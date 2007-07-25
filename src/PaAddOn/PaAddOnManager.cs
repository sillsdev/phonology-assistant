using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.Pa.Controls;
using SIL.Pa;
using SIL.Pa.FFSearchEngine;
using SIL.Pa.Resources;
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
	public class PaAddOnManager : IxCoreColleague
	{
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
		public PaAddOnManager()
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRegExpressionShowSearchResults(object args)
		{
			SearchQuery query = args as SearchQuery;
			if (query == null || m_findPhoneWnd == null || m_findPhoneWnd.ResultViewManger == null)
				return true;

			ISearchResultsViewHost srchRsltVwHost = ReflectionHelper.GetField(
				m_findPhoneWnd.ResultViewManger, "m_srchRsltVwHost") as ISearchResultsViewHost;

			if (srchRsltVwHost != null)
				srchRsltVwHost.BeforeSearchPerformed(query, null);

			//PaApp.InitializeProgressBar(ResourceHelper.GetString("kstidQuerySearchingMsg"));

			RegExpressionSearch regExpSrch = new RegExpressionSearch(query);
			WordListCache resultCache = regExpSrch.Search();

			if (resultCache != null)
			{
				resultCache.SearchQuery = query.Clone();
				ReflectionHelper.CallMethod(m_findPhoneWnd.ResultViewManger,
					"ShowResults", resultCache, SearchResultLocation.CurrentTabGroup);
			}

			//PaApp.UninitializeProgressBar();

			return true;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// This is a hidden feature.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnRegularExpressionSearchDialog(object args)
		//{
		//    if (m_regExDlg != null)
		//        m_regExDlg.Close();
		//    else
		//    {
		//        m_regExDlg = new RegExpressionSearchDlg();
		//        m_regExDlg.Show();
		//        m_regExDlg.FormClosed += delegate
		//        {
		//            m_regExDlg.Dispose();
		//            m_regExDlg = null;
		//        };
		//    }

		//    return true;
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnUpdateRegularExpressionSearchDialog(object args)
		//{
		//    TMItemProperties itemProps = args as TMItemProperties;
		//    if (itemProps == null)
		//        return false;

		//    bool shouldBeVisible = m_showRegExDlgButton;
		//    bool shouldBeChecked = (m_regExDlg != null);

		//    if (shouldBeChecked != itemProps.Checked || shouldBeVisible != itemProps.Visible)
		//    {
		//        itemProps.Checked = shouldBeChecked;
		//        itemProps.Visible = shouldBeVisible;
		//        itemProps.Update = true;
		//    }

		//    return true;
		//}


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
