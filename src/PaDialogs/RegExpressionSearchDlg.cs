using System;
using System.ComponentModel;
using System.Media;
using System.Windows.Forms;
using SIL.Pa.Data;
using SIL.Pa.FFSearchEngine;
using SIL.SpeechTools.Utils;

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

			PaApp.SettingsHandler.LoadFormProperties(this);

			foreach (AFeature afeat in DataUtils.AFeatureCache.Values)
			{
				if (!afeat.IsBlank)
				{
					ToolStripMenuItem item = new ToolStripMenuItem(afeat.Name);
					item.Click += new EventHandler(HandleFeatureItemClick);
					mnuPhonesInAFeature.DropDownItems.Add(item);
				}
			}

			foreach (BFeature bfeat in DataUtils.BFeatureCache.Values)
			{
				ToolStripMenuItem item = new ToolStripMenuItem("+" + bfeat.Name);
				item.Click += new EventHandler(HandleFeatureItemClick);
				mnuPhonesInBFeature.DropDownItems.Add(item);

				item = new ToolStripMenuItem("-" + bfeat.Name);
				item.Click += new EventHandler(HandleFeatureItemClick);
				mnuPhonesInBFeature.DropDownItems.Add(item);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFeatureItemClick(object sender, EventArgs e)
		{
			ToolStripMenuItem item = sender as ToolStripMenuItem;
			if (item == null)
				return;

			string phones = PaApp.PhoneCache.GetCommaDelimitedPhonesInFeature(item.Text);
			if (phones == null)
				return;

			Insert("(" + phones.Replace(',', '|') + ")");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			PaApp.SettingsHandler.SaveFormProperties(this);
			base.OnClosing(e);
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
				SystemSounds.Beep.Play();
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleTextBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (int)Keys.Enter)
				tbShowResults.PerformClick();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void matchOnZeroOrMoreCharactersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Insert(".?");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void oneOrMoreCharactersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Insert(".");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void wordBoundaryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Insert("\\b");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void consonantPhoneGroupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string cons = PaApp.PhoneCache.CommaDelimitedConsonants;
			Insert("(" + cons.Replace(',', '|') + ")");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void vowelPhoneGroupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string vows = PaApp.PhoneCache.CommaDelimitedVowels;
			Insert("(" + vows.Replace(',', '|') + ")");

		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Insert(string text)
		{
			TextBox txt = RemoveSelectedText();
			if (txt == null)
				return;

			try
			{
				int selStart = txt.SelectionStart;
				txt.Text = txt.Text.Insert(selStart, text);
				txt.SelectionStart = selStart + text.Length;
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private TextBox RemoveSelectedText()
		{
			TextBox txt;

			if (txtEnvBefore.Focused)
				txt = txtEnvBefore;
			else if (txtSrchItem.Focused)
				txt = txtSrchItem;
			else if (txtEnvAfter.Focused)
				txt = txtEnvAfter;
			else
				return null;

			// Remove any selected text.
			if (txt.SelectionLength > 0)
			{
				int selStart = txt.SelectionStart;
				txt.Text = txt.Text.Remove(selStart, txt.SelectionLength);
				txt.SelectionStart = selStart;
			}

			return txt;
		}
	}
}