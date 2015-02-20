// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Localization;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	public partial class WordListsOptionsPage : OptionsDlgPageBase
	{
		/// ------------------------------------------------------------------------------------
		public WordListsOptionsPage(PaProject project) : base(project)
		{
			InitializeComponent();

			lblShowColumns.Font = FontHelper.UIFont;
			lblExplanation.Font = FontHelper.UIFont;
			grpColChanges.Font = FontHelper.UIFont;
			grpColSettings.Font = FontHelper.UIFont;
			grpGridLines.Font = FontHelper.UIFont;
			rbGridLinesBoth.Font = FontHelper.UIFont;
			rbGridLinesHorizontal.Font = FontHelper.UIFont;
			rbGridLinesNone.Font = FontHelper.UIFont;
			rbGridLinesVertical.Font = FontHelper.UIFont;
			chkSaveReorderedColumns.Font = FontHelper.UIFont;
			chkSaveColHdrHeight.Font = FontHelper.UIFont;
			chkSaveColWidths.Font = FontHelper.UIFont;
			chkAutoAdjustPhoneticCol.Font = FontHelper.UIFont;
			nudMaxEticColWidth.Font = FontHelper.UIFont;

			fldSelGridWrdList.CurrentRowChanged += delegate { UpdateDisplay(); };
			chkAutoAdjustPhoneticCol.CheckedChanged += delegate { UpdateDisplay(); };

			btnMoveColDown.Click += delegate
			{
				fldSelGridWrdList.MoveSelectedItemDown();
				UpdateDisplay();
			};

			btnMoveColUp.Click += delegate
			{
				fldSelGridWrdList.MoveSelectedItemUp();
				UpdateDisplay();
			};

			if (!App.DesignMode)
				InitializeFields();
		}

		/// ------------------------------------------------------------------------------------
		public override string TabPageText
		{
			get { return LocalizationManager.GetString("DialogBoxes.OptionsDlg.WordListTab.TabText", "Word Lists"); }
		}

		/// ------------------------------------------------------------------------------------
		public override string HelpId
		{
			get { return "hidWordListOptions"; }
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeFields()
		{
			fldSelGridWrdList.Load(from field in m_project.GetMappedFields()
								   orderby field.DisplayIndexInGrid
								   select new KeyValuePair<PaField, bool>(field, field.VisibleInGrid));

			chkSaveReorderedColumns.Checked = m_project.GridLayoutInfo.SaveReorderedCols;
			chkSaveColHdrHeight.Checked = m_project.GridLayoutInfo.SaveAdjustedColHeaderHeight;
			chkSaveColWidths.Checked = m_project.GridLayoutInfo.SaveAdjustedColWidths;
			chkAutoAdjustPhoneticCol.Checked = m_project.GridLayoutInfo.AutoAdjustPhoneticCol;
			nudMaxEticColWidth.Value = m_project.GridLayoutInfo.AutoAjustedMaxWidth;

			switch (m_project.GridLayoutInfo.GridLines)
			{
				case DataGridViewCellBorderStyle.Single:
					rbGridLinesBoth.Checked = true;
					break;

				case DataGridViewCellBorderStyle.SingleVertical:
					rbGridLinesVertical.Checked = true;
					break;

				case DataGridViewCellBorderStyle.SingleHorizontal:
					rbGridLinesHorizontal.Checked = true;
					break;

				case DataGridViewCellBorderStyle.None:
					rbGridLinesNone.Checked = true;
					break;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(System.EventArgs e)
		{
			base.OnHandleCreated(e);
			UpdateDisplay();
		}
		
		/// ------------------------------------------------------------------------------------
		public override void Save()
		{
			m_project.GridLayoutInfo.SaveReorderedCols = chkSaveReorderedColumns.Checked;
			m_project.GridLayoutInfo.SaveAdjustedColHeaderHeight = chkSaveColHdrHeight.Checked;
			m_project.GridLayoutInfo.SaveAdjustedColWidths = chkSaveColWidths.Checked;
			m_project.GridLayoutInfo.AutoAdjustPhoneticCol = chkAutoAdjustPhoneticCol.Checked;
			m_project.GridLayoutInfo.AutoAjustedMaxWidth = (int)nudMaxEticColWidth.Value;

			if (rbGridLinesBoth.Checked)
				m_project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.Single;
			else if (rbGridLinesVertical.Checked)
				m_project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.SingleVertical;
			else if (rbGridLinesHorizontal.Checked)
				m_project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.SingleHorizontal;
			else if (rbGridLinesNone.Checked)
				m_project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.None;

			int i = 0;
			foreach (var kvp in fldSelGridWrdList.GetSelections())
			{
				var field = m_project.GetFieldForDisplayName(kvp.Key);
				field.VisibleInGrid = kvp.Value;
				field.DisplayIndexInGrid = i++;
			}

			App.MsgMediator.SendMessage("WordListOptionsChanged", null);
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsDirty
		{
			get
			{
				bool gridLinesChanged = ((rbGridLinesBoth.Checked &&
					m_project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.Single) ||
					(rbGridLinesVertical.Checked &&
					m_project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.SingleVertical) ||
					(rbGridLinesHorizontal.Checked &&
					m_project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.SingleHorizontal) ||
					(rbGridLinesNone.Checked &&
					m_project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.None));

				return (fldSelGridWrdList.IsDirty || gridLinesChanged ||
					chkSaveReorderedColumns.Checked != m_project.GridLayoutInfo.SaveReorderedCols ||
					chkSaveColHdrHeight.Checked != m_project.GridLayoutInfo.SaveAdjustedColHeaderHeight ||
					chkSaveColWidths.Checked != m_project.GridLayoutInfo.SaveAdjustedColWidths ||
					chkAutoAdjustPhoneticCol.Checked != m_project.GridLayoutInfo.AutoAdjustPhoneticCol);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			nudMaxEticColWidth.Enabled = chkAutoAdjustPhoneticCol.Checked;
			btnMoveColDown.Enabled = fldSelGridWrdList.CanMoveSelectedItemDown;
			btnMoveColUp.Enabled = fldSelGridWrdList.CanMoveSelectedItemUp;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<PaField> GetSelectedFields()
		{
			return fldSelGridWrdList.GetCheckedFields();
		}
	}
}
