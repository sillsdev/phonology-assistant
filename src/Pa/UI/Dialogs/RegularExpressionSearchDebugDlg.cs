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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	public partial class RegularExpressionSearchDebugDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		public RegularExpressionSearchDebugDlg()
		{
			InitializeComponent();
			_labelSearchPatternValue.Font = App.PhoneticFont;
			_textBoxRegExpression.Font = App.PhoneticFont;
			_colPhonetic.DefaultCellStyle.Font = App.PhoneticFont;
			_colMatch.DefaultCellStyle.Font = App.PhoneticFont;

			_buttonClose.Click += delegate { Close(); };
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.RegularExpressionSearchDebugDlg =
				App.InitializeForm(this, Settings.Default.RegularExpressionSearchDebugDlg);
			
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		public void LoadExpressions(SearchQuery query, Regex srchItem, Regex preceding, Regex following)
		{
			_grid.Rows.Clear();

			_labelSearchPatternValue.Text = query.Pattern;

			_textBoxRegExpression.Text =
				string.Format("Search Item Regular Expression:" + Environment.NewLine +
				srchItem + Environment.NewLine + Environment.NewLine +
				"Preceding Environment Regular Expression:" + Environment.NewLine +
				preceding + Environment.NewLine + Environment.NewLine +
				"Following Environment Regular Expression:" + Environment.NewLine +
				following);
		}

		/// ------------------------------------------------------------------------------------
		public void LoadMatch(string phonetic, Match match)
		{
			_grid.Rows.Add(phonetic, match.Value, match.Index, match.Length);
		}
	}
}
