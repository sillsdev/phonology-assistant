using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa;
using SIL.Pa.Controls;
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
		private ITMAdapter m_adapter;
		private TMItemProperties m_feedbackItemProps;

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
		protected bool OnMainViewOpened(object args)
		{
			try
			{
				if (args is PaMainWnd)
				{
					m_mainWnd = args as PaMainWnd;
					m_adapter = PaApp.TMAdapter;

					m_adapter.AddCommandItem("CmdSendFeedback", "SendFeedback",
						Properties.Resources.kstidFeedbackMenuText, null, null, null, null,
						null, Keys.None, null, Properties.Resources.kimidSendFeedback);

					m_feedbackItemProps = new TMItemProperties();
					m_feedbackItemProps.BeginGroup = true;
					m_feedbackItemProps.CommandId = "CmdSendFeedback";
					m_feedbackItemProps.Name = "mnuSendFeedback";
					m_feedbackItemProps.Text = null;
					m_adapter.AddMenuItem(m_feedbackItemProps, "mnuHelp", "mnuHelpAbout");

					// Get the number of times PA has been launched.
					int launchCount = PaApp.SettingsHandler.GetIntSettingsValue(
						"feedbackaddon", "launchcount", 0);

					// Increase the launch count by one and save it.
					PaApp.SettingsHandler.SaveSettingsValue(
						"feedbackaddon", "launchcount", ++launchCount);

					// If we've reached the tenth time PA has been run, then show
					// the user the dialog requesting feedback.
					if (launchCount == 10)
					{
						using (RequestDlg dlg = new RequestDlg())
						{
							if (dlg.ShowDialog(m_mainWnd) == DialogResult.Yes)
								OnSendFeedback(null);
						}
					}
				}
			}
			catch { }

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSendFeedback(object args)
		{
			int launchCount = PaApp.SettingsHandler.GetIntSettingsValue(
				"feedbackaddon", "launchcount", 0);

			using (FeedbackReportDlg dlg = new FeedbackReportDlg(launchCount))
				dlg.ShowDialog(m_mainWnd);
			
			return true;
		}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void IxCoreColleague.Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		IxCoreColleague[] IxCoreColleague.GetMessageTargets()
		{
			return new IxCoreColleague[] { this };
		}

		#endregion
	}
}
