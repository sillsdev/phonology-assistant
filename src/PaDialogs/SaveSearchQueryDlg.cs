using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;
using SIL.Pa.Controls;
using SIL.Pa.FFSearchEngine;

namespace SIL.Pa.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class SaveSearchQueryDlg : OKCancelDlgBase
	{
		private SearchQuery m_query;
		private SearchPatternTreeView m_tvSrchPatterns;
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SaveSearchQueryDlg()
		{
			InitializeComponent();

			lblCategories.Text = STUtils.ConvertLiteralNewLines(lblCategories.Text);

			lblPatternLabel.Font = FontHelper.UIFont;
			lblName.Font = FontHelper.UIFont;
			lblCategories.Font = FontHelper.UIFont;

			lblPattern.Font = FontHelper.PhoneticFont;
			txtName.Font = FontHelper.PhoneticFont;
			cboCategories.Font = FontHelper.PhoneticFont;

			AdjustLabelLocations();

			string tipText = m_toolTip.GetToolTip(cboCategories);
			tipText = STUtils.ConvertLiteralNewLines(tipText);
			m_toolTip.SetToolTip(cboCategories, tipText);

			tipText = m_toolTip.GetToolTip(txtName);
			tipText = STUtils.ConvertLiteralNewLines(tipText);
			m_toolTip.SetToolTip(txtName, tipText);

			foreach (SearchQueryGroup group in PaApp.Project.SearchQueryGroups)
				cboCategories.Items.Add(group.Name);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SaveSearchQueryDlg(SearchQuery query, SearchPatternTreeView tvSrchPatterns,
			bool canChangeQuerysCategory) : this()
		{
			m_query = query;
			m_tvSrchPatterns = tvSrchPatterns;
			lblPattern.Text = m_query.Pattern;
			txtName.Text = query.ToString();
			txtName.SelectAll();

			cboCategories.Enabled = canChangeQuerysCategory;

			if (m_query.Category != null)
			{
				// Get the category name from the query's "owning" rather than from the
				// query itself, just in case the user just renamed the category in the
				// saved pattern's tree view control. When that happens queries belonging
				// to that category that are already loaded into search result views
				// no longer have the correct category name but their Id is still good.
				SearchQueryGroup group = PaApp.Project.SearchQueryGroups.GetGroupFromQueryId(m_query.Id);
				cboCategories.Text = (group != null ? group.Name : m_query.Category);
			}
			else
			{
				if (m_tvSrchPatterns.SelectedNode == null)
					cboCategories.Text = string.Empty;
				else
				{
					cboCategories.Text = (m_tvSrchPatterns.SelectedNode.Level == 0 ?
						m_tvSrchPatterns.SelectedNode.Text :
						m_tvSrchPatterns.SelectedNode.Parent.Text);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjust the Y location for each label or control being labeled, depending upon
		/// what elements on the dialog are taller.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustLabelLocations()
		{
			txtName.Top = lblPattern.Bottom + 10;
			cboCategories.Top = txtName.Bottom + 10;
			Height = SystemInformation.CaptionHeight + pnlButtons.Height + cboCategories.Bottom + 18;

			// Center the pattern with its label.
			lblPatternLabel.Top = lblPattern.Top + (lblPattern.Height - lblPatternLabel.Height) / 2;

			// Center the name text box with its label.
			lblName.Top = txtName.Top + (txtName.Height - lblName.Height) / 2;

			// Center the category combo box with its label.
			lblCategories.Top = cboCategories.Top + (cboCategories.Height - lblCategories.Height) / 2;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			string name = txtName.Text.Trim();

			// Don't bother saving the name if the name and the pattern are identical.
			m_query.Name = (name != m_query.Pattern ? name : null);

			m_tvSrchPatterns.AddPattern(m_query, cboCategories.Text.Trim());
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			string queryName = txtName.Text.Trim();
			if (string.IsNullOrEmpty(queryName))
			{
				string msg = Properties.Resources.kstidNoSavedPatternNameMsg;
				STUtils.STMsgBox(msg, MessageBoxButtons.OK);
				txtName.SelectAll();
				txtName.Focus();
				return false;
			}

			string categoryName = cboCategories.Text.Trim();
			if (string.IsNullOrEmpty(categoryName))
			{
				string msg = Properties.Resources.kstidNoSavedPatternCategoryMsg;
				STUtils.STMsgBox(msg, MessageBoxButtons.OK);
				cboCategories.SelectAll();
				cboCategories.Focus();
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw an etched line between the buttons and what's above them.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Point pt1 = new Point(pnlButtons.Left, pnlButtons.Top - 2);
			Point pt2 = new Point(pnlButtons.Right - 1, pnlButtons.Top - 2);

			using (Pen pen = new Pen(SystemColors.ControlDark))
			{
				e.Graphics.DrawLine(pen, pt1, pt2);
				pt1.Y++;
				pt2.Y++;
				pen.Color = SystemColors.ControlLight;
				e.Graphics.DrawLine(pen, pt1, pt2);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleTextChanged(object sender, EventArgs e)
		{
			m_dirty = true;
		}
	}
}