using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Dialogs;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.FiltersAddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class DefineFiltersDlg : OKCancelDlgBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DefineFiltersDlg()
		{
			InitializeComponent();
			lblFilters.Font = FontHelper.UIFont;
			lvFilters.Font = FontHelper.UIFont;
			PaApp.SettingsHandler.LoadFormProperties(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			PaApp.SettingsHandler.SaveFormProperties(this);

			if (!IsDirty)
				return;
		}
	}
}
