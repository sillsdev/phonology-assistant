// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class SaveSearchQueryDlg : OKCancelDlgBase
	{
		private readonly SearchQuery m_query;
		private readonly SearchPatternTreeView m_tvSrchPatterns;
		
		/// ------------------------------------------------------------------------------------
		public SaveSearchQueryDlg()
		{
			InitializeComponent();

			lblCategories.Text = Utils.ConvertLiteralNewLines(lblCategories.Text);

			lblPatternLabel.Font = FontHelper.UIFont;
			lblName.Font = FontHelper.UIFont;
			lblCategories.Font = FontHelper.UIFont;

			lblPattern.Font = App.PhoneticFont;
			txtName.Font = App.PhoneticFont;
			cboCategories.Font = App.PhoneticFont;

			AdjustLabelLocations();

			foreach (var group in App.Project.SearchQueryGroups)
				cboCategories.Items.Add(group.Name);
		}

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
				var group = App.Project.SearchQueryGroups.GetGroupFromQueryId(m_query.Id);
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
			Height = SystemInformation.CaptionHeight + tblLayoutButtons.Height + cboCategories.Bottom + 18;

			// Center the pattern with its label.
			lblPatternLabel.Top = lblPattern.Top + (lblPattern.Height - lblPatternLabel.Height) / 2;

			// Center the name text box with its label.
			lblName.Top = txtName.Top + (txtName.Height - lblName.Height) / 2;

			// Center the category combo box with its label.
			lblCategories.Top = cboCategories.Top + (cboCategories.Height - lblCategories.Height) / 2;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			string name = txtName.Text.Trim();

			// Don't bother saving the name if the name and the pattern are identical.
			m_query.Name = (name != m_query.Pattern ? name : null);

			return m_tvSrchPatterns.AddPattern(m_query, cboCategories.Text.Trim());
		}

		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			if (txtName.Text.Trim() == string.Empty)
			{
				Utils.MsgBox(LocalizationManager.GetString(
					"DialogBoxes.SaveSearchQueryDlg.NoSavedPatternNameMsg",
					"You must specify a name for your saved pattern."));
				
				txtName.SelectAll();
				txtName.Focus();
				return false;
			}
			
			if (cboCategories.Text.Trim() == string.Empty)
			{
				Utils.MsgBox(LocalizationManager.GetString(
					"DialogBoxes.SaveSearchQueryDlg.NoSavedPatternCategoryMsg",
					"You must specify a category for your saved pattern."));
				
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

			var pt1 = new Point(tblLayoutButtons.Left, tblLayoutButtons.Top - 2);
			var pt2 = new Point(tblLayoutButtons.Right - 1, tblLayoutButtons.Top - 2);

			using (var pen = new Pen(SystemColors.ControlDark))
			{
				e.Graphics.DrawLine(pen, pt1, pt2);
				pt1.Y++;
				pt2.Y++;
				pen.Color = SystemColors.ControlLight;
				e.Graphics.DrawLine(pen, pt1, pt2);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTextChanged(object sender, EventArgs e)
		{
			m_dirty = true;
		}

        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.Escape:
                    {
                        this.Close();
                        return true;
                    }
                case Keys.Control | Keys.Tab:
                    {
                        return true;
                    }
                case Keys.Control | Keys.Shift | Keys.Tab:
                    {
                        return true;
                    }
            }
            return base.ProcessCmdKey(ref message, keys);
        }
	}
}