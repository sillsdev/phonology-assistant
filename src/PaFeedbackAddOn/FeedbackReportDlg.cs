using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.AddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class FeedbackReportDlg : Form
	{
		private Label[] m_lblRatings;
		private int m_launchCount = 0;
		private List<RatingSurveyCtrl> m_surveyItemCtrls = new List<RatingSurveyCtrl>();
		private string m_surveyItemsFile;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeedbackReportDlg(int launchCount)
		{
			m_launchCount = launchCount;

			InitializeComponent();
			m_lblRatings = new Label[] { lbl1, lbl2, lbl3, lbl4, lbl5 };
			PaApp.SettingsHandler.LoadFormProperties(this);

			// Strip off file:/
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Substring(6);
			m_surveyItemsFile = Path.Combine(path, "SurveyItems.xml");

			List<SurveyItem> surveyitems = STUtils.DeserializeData(m_surveyItemsFile,
				typeof(List<SurveyItem>)) as List<SurveyItem>;

			if (surveyitems == null)
				return;

			for (int i = 0; i < surveyitems.Count; i++)
			{
				RatingSurveyCtrl ctrl = new RatingSurveyCtrl();
				ctrl.Height = 40;
				ctrl.Dock = DockStyle.Top;
				ctrl.InfoText = surveyitems[i].ItemDescription;
				ctrl.Text = string.Format(Properties.Resources.kstidSurveyItemTextFormat,
					i + 1, surveyitems[i].ItemText);

				m_surveyItemCtrls.Add(ctrl);
				pnlSurveyInner.Controls.Add(ctrl);
				ctrl.BringToFront();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			foreach (RatingSurveyCtrl ctrl in m_surveyItemCtrls)
				ctrl.Clear();

			base.OnShown(e);
			pnlSurveyInner_Resize(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			PaApp.SettingsHandler.SaveFormProperties(this);
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlSurveyOuter_Paint(object sender, PaintEventArgs e)
		{
			using (Pen pen = new Pen(SystemColors.WindowText, 2))
			{
				e.Graphics.DrawLine(pen, 3, pnlSurveyInner.Top - 3,
					pnlSurveyInner.Right - 3, pnlSurveyInner.Top - 3);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlComments_Paint(object sender, PaintEventArgs e)
		{
			using (Pen pen = new Pen(SystemColors.WindowText, 2))
			{
				e.Graphics.DrawLine(pen, txtComments.Left, txtComments.Top - 3,
					txtComments.Right, txtComments.Top - 3);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlSurveyInner_Resize(object sender, EventArgs e)
		{
			if (m_surveyItemCtrls.Count > 0)
			{
				for (int i = 0; i < 5; i++)
				{
					Point pt = m_surveyItemCtrls[0].GetChoiceLocation(i);
					m_lblRatings[i].Left = pnlSurveyOuter.PointToClient(pt).X;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnSend_Click(object sender, EventArgs e)
		{
			if (VerifyRatings())
				Send();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCopy_Click(object sender, EventArgs e)
		{
			if (!VerifyRatings())
				return;

			Clipboard.SetText(BuildMessage(), TextDataFormat.UnicodeText);
			using (CopiedToClipboardDlg dlg = new CopiedToClipboardDlg())
				dlg.ShowDialog(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Send()
		{
			try
			{
				string invalidURIChars = @" /&:$";
				string mailAddress = Properties.Resources.kstidFeedbackMailAddress;
				string subject = "PA%20Feedback%20Report";
				string body = BuildMessage();
				body = body.Replace("%", "%25");
				body = body.Replace(System.Environment.NewLine, "%0D%0A");

				foreach (char c in invalidURIChars)
					body = body.Replace(c.ToString(), string.Format("%{0}", ((int)c).ToString("X2")));

				System.Diagnostics.Process prs = new System.Diagnostics.Process();
				prs.StartInfo.FileName = string.Format("mailto:{0}?subject={1}&body={2}",
					mailAddress, subject, body);
				
				prs.Start();
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool VerifyRatings()
		{
			foreach (RatingSurveyCtrl ctrl in m_surveyItemCtrls)
			{
				if (ctrl.Rating == 0)
				{
					string msg = Properties.Resources.kstidMissingRatingMsg;
					return (STUtils.STMsgBox(msg, MessageBoxButtons.YesNo) == DialogResult.Yes);
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string BuildMessage()
		{
			StringBuilder bldr = new StringBuilder();

			bldr.AppendLine("Phonology Assistant Feedback Report");
			bldr.AppendFormat("Date: {0}", DateTime.Now.ToLongDateString());
			bldr.AppendLine();
			bldr.AppendFormat("Launches: {0}", m_launchCount);
			bldr.AppendLine();
			bldr.AppendFormat("Version: {0}", PaApp.ProdVersion);
			bldr.AppendLine();
			
			Version ver = new Version(Application.ProductVersion);
			DateTime bldDate = new DateTime(2000, 1, 1).Add(new TimeSpan(ver.Build, 0, 0, 0));
			bldr.AppendFormat("Build: {0}", bldDate.ToString("dd-MMM-yyyy"));

			bldr.AppendLine();
			bldr.AppendLine();
			bldr.AppendLine();
			bldr.AppendLine("Ratings");
			bldr.AppendLine(string.Empty.PadLeft(30, '-'));

			foreach (RatingSurveyCtrl ctrl in m_surveyItemCtrls)
			{
				try
				{
					bldr.AppendFormat("({0}) - {1}", ctrl.Rating,
						ctrl.Text.Replace("&&", "&").TrimEnd(":".ToCharArray()));
					bldr.AppendLine();
				}
				catch { }
			}

			bldr.AppendLine();
			bldr.AppendLine();
			bldr.AppendLine("Comments/Suggestions");
			bldr.AppendLine(string.Empty.PadLeft(30, '-'));
			bldr.Append(txtComments.Text.Trim());

			return bldr.ToString();
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SurveyItem
	{
		[XmlAttribute]
		public string ItemText;
		[XmlAttribute]
		public string ItemDescription;
	}
}