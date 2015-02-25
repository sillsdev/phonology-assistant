// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Drawing;
using System.Windows.Forms;
using SilTools;

namespace SIL.Pa.PhoneticSearching
{
	public partial class BracketedTextErrorMsgBox : Form
	{
		/// ------------------------------------------------------------------------------------
		public BracketedTextErrorMsgBox(string bracketedText)
		{
			InitializeComponent();

			_labelMessage.Font = FontHelper.UIFont;
			_linkHelp.Font = FontHelper.UIFont;
			_pictureIcon.Image = SystemIcons.Information.ToBitmap();

			_labelMessage.Text = string.Format(_labelMessage.Text, bracketedText);
			
			var helpTopic1 = "Examples of search pattern elements";
			var helpTopic2 = "AND groups";

			_linkHelp.Text = string.Format(_linkHelp.Text, helpTopic1, helpTopic2);

			_linkHelp.Links.Clear();
			int i = _linkHelp.Text.IndexOf(helpTopic1);
			_linkHelp.Links.Add(i, helpTopic1.Length, "hidSearchPatternExamples");
			i = _linkHelp.Text.IndexOf(helpTopic2);
			_linkHelp.Links.Add(i, helpTopic2.Length, "hidAndGroups");

			AutoSize = true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleHelpLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			App.ShowHelpTopic(e.Link.LinkData as string);
		}
	}
}
