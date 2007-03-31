using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A dialog that allows the user to specify a FieldWorks database.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class FwDatabaseDlg : SIL.Pa.Dialogs.OKCancelDlgBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDatabaseDlg()
		{
			InitializeComponent();

			lblMsg.Font = FontHelper.UIFont;
			lstFwDatabases.Font = FontHelper.UIFont;
			lstFwDatabases.Items.AddRange(FwDBUtils.FwDatabaseInfoList);
			lstFwDatabases.SelectedIndex = 0;

			Application.Idle += new EventHandler(Application_Idle);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the properties button is only enabled when there's a selected database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void Application_Idle(object sender, EventArgs e)
		{
			btnProperties.Enabled = (ChosenDatabase != null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			Application.Idle -= Application_Idle;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the chosen database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceInfo ChosenDatabase
		{
			get { return lstFwDatabases.SelectedItem as FwDataSourceInfo; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnProperties_Click(object sender, EventArgs e)
		{
			using (FwDataSourcePropertiesDlg dlg = new FwDataSourcePropertiesDlg(ChosenDatabase))
				dlg.ShowDialog(this);
		}
	}
}