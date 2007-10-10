using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIL.Pa.AddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class RequestDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RequestDlg()
		{
			try
			{
				InitializeComponent();
				string msg = Properties.Resources.kstidFeedbackRequestMsg.Replace("\\r", "\r");
				msg = msg.Replace("\\n", "\n");
				txtRequest.Text = SIL.SpeechTools.Utils.STUtils.ConvertLiteralNewLines(msg);
			}
			catch { }
		}
	}
}