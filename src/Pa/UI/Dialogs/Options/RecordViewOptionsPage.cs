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
using L10NSharp;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	public partial class RecordViewOptionsPage : OptionsDlgPageBase
	{
		/// ------------------------------------------------------------------------------------
		public RecordViewOptionsPage(PaProject project) : base(project)
		{
			InitializeComponent();

			lblShowFields.Font = FontHelper.UIFont;
			grpFieldSettings.Font = FontHelper.UIFont;

			fldSelGridRecView.Load(from field in m_project.GetMappedFields()
								   orderby field.DisplayIndexInRecView
								   select new KeyValuePair<PaField, bool>(field, field.VisibleInRecView));

			fldSelGridRecView.CurrentRowChanged += delegate { UpdateDisplay(); };
			
			_buttonMoveDown.Click += delegate
			{
				fldSelGridRecView.MoveSelectedItemDown();
				UpdateDisplay();
			};

			_buttonMoveUp.Click += delegate
			{
				fldSelGridRecView.MoveSelectedItemUp();
				UpdateDisplay();
			};
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(System.EventArgs e)
		{
			base.OnHandleCreated(e);
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		public override string TabPageText
		{
			get { return LocalizationManager.GetString("DialogBoxes.OptionsDlg.RecordViewTab.TabText", "Record View"); }
		}

		/// ------------------------------------------------------------------------------------
		public override string HelpId
		{
			get { return "hidRecordViewOptions"; }
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsDirty
		{
			get { return (fldSelGridRecView != null && fldSelGridRecView.IsDirty); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the values on the word list tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override void Save()
		{
			int i = 0;
			foreach (var kvp in fldSelGridRecView.GetSelections())
			{
				var field = m_project.GetFieldForDisplayName(kvp.Key);
				field.VisibleInRecView = kvp.Value;
				field.DisplayIndexInRecView = i++;
			}

			App.MsgMediator.SendMessage("RecordViewOptionsChanged", null);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_buttonMoveDown.Enabled = fldSelGridRecView.CanMoveSelectedItemDown;
			_buttonMoveUp.Enabled = fldSelGridRecView.CanMoveSelectedItemUp;
		}
	}
}
