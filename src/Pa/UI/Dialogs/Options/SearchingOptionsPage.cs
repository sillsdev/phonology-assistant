// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	public partial class SearchingOptionsPage : OptionsDlgPageBase
	{
		/// ------------------------------------------------------------------------------------
		public SearchingOptionsPage(PaProject project) : base(project)
		{
			InitializeComponent();

			_checkBoxSearchEngineSelction.Font = FontHelper.UIFont;
		}

		/// ------------------------------------------------------------------------------------
		public override string TabPageText
		{
			get { return "Searching"; }
			//get { return LocalizationManager.GetString("DialogBoxes.OptionsDlg.UserInterfaceTab.TabText", "User Interface"); }
		}

		/// ------------------------------------------------------------------------------------
		public override string HelpId
		{
			get { return ""; }
		}
			
		/// ------------------------------------------------------------------------------------
		public override bool IsDirty
		{
			get { return _checkBoxSearchEngineSelction.Checked != Properties.Settings.Default.ShowSearchEngineChoiceButtons ; }
		}

		/// ------------------------------------------------------------------------------------
		public override void Save()
		{
			Properties.Settings.Default.ShowSearchEngineChoiceButtons = _checkBoxSearchEngineSelction.Checked;
			App.MsgMediator.SendMessage("ShowSearchSelectionButtonsChagned", null);
		}
	}
}
