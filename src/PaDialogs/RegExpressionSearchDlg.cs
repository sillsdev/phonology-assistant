using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;
using SIL.Pa.FFSearchEngine;
using SIL.Pa.Data;

namespace SIL.Pa.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class RegExpressionSearchDlg : Form
	{
		private const string kZeroOrMore = ".?";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RegExpressionSearchDlg()
		{
			InitializeComponent();

			pnlEvnBefore.Font = FontHelper.UIFont;
			pnlSrchItem.Font = FontHelper.UIFont;
			pnlEnvAfter.Font = FontHelper.UIFont;

			txtEnvBefore.Font = FontHelper.PhoneticFont;
			txtSrchItem.Font = FontHelper.PhoneticFont;
			txtEnvAfter.Font = FontHelper.PhoneticFont;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tbShowResults_Click(object sender, EventArgs e)
		{
			if (txtSrchItem.Text.Trim().Length == 0)
			{
				System.Media.SystemSounds.Beep.Play();
				txtSrchItem.Focus();
				return;
			}

			if (txtEnvBefore.Text.Trim().Length == 0)
				txtEnvBefore.Text = kZeroOrMore;

			if (txtEnvAfter.Text.Trim().Length == 0)
				txtEnvAfter.Text = kZeroOrMore;

			SearchQuery query = new SearchQuery();
			query.IsPatternRegExpression = true;
			query.Pattern = txtEnvBefore.Text.Trim() + DataUtils.kOrc.ToString() +
				txtSrchItem.Text.Trim() + DataUtils.kOrc.ToString() + txtEnvAfter.Text.Trim();

			PaApp.MsgMediator.SendMessage("RegExpressionShowSearchResults", query);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tbClear_Click(object sender, EventArgs e)
		{
			txtEnvBefore.Text = txtSrchItem.Text = txtEnvAfter.Text = string.Empty;
			txtEnvBefore.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}