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
		private DataCorpusVw m_dataCorpusVw;
		private SearchVw m_findPhoneWnd;
		private ConsonantChartVw m_consonantChartWnd;
		private VowelChartVw m_vowelChartWnd;
		private XYChartVw m_xyChartWnd;
		private PhoneInventoryVw m_phoneInventoryWnd;

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
				else if (args.GetType() == typeof(DataCorpusVw))
					m_dataCorpusVw = args as DataCorpusVw;
				else if (args.GetType() == typeof(SearchVw))
					m_findPhoneWnd = args as SearchVw;
				else if (args.GetType() == typeof(ConsonantChartVw))
					m_consonantChartWnd = args as ConsonantChartVw;
				else if (args.GetType() == typeof(VowelChartVw))
					m_vowelChartWnd = args as VowelChartVw;
				else if (args.GetType() == typeof(XYChartVw))
					m_xyChartWnd = args as XYChartVw;
				else if (args.GetType() == typeof(PhoneInventoryVw))
					m_phoneInventoryWnd = args as PhoneInventoryVw;
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
