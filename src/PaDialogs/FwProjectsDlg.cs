using System;
using System.Diagnostics;
using System.Windows.Forms;
using SIL.Pa.Data;
using SIL.Pa.Controls;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A dialog that allows the user to specify a FieldWorks database.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class FwProjectsDlg : OKCancelDlgBase
	{
		private readonly PaProject m_project;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwProjectsDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwProjectsDlg(PaProject project) : this()
		{
			Debug.Assert(project != null);
			m_project = project;

			lblMsg.Font = FontHelper.UIFont;
			lstFwProjects.Font = FontHelper.UIFont;
			lblDB.Font = FontHelper.UIFont;
			lblNetwork.Font = FontHelper.UIFont;
			tvNetwork.Font = FontHelper.UIFont;
			txtMsg.Font = FontHelper.UIFont;

			txtMsg.Dock = DockStyle.Fill;
			txtMsg.BringToFront();

			lblDB.Height = FontHelper.UIFont.Height + 10;
			lblNetwork.Height = FontHelper.UIFont.Height + 10;

			tvNetwork.Load();
			Application.Idle += Application_Idle;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the properties button is only enabled when there's a selected database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void Application_Idle(object sender, EventArgs e)
		{
			btnOK.Enabled = btnProperties.Enabled = (ChosenDatabase != null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			float splitRatio =
				PaApp.SettingsHandler.GetFloatSettingsValue(Name, "splitratio", 0f);

			// If the split ratio is zero, assume any form settings found were for the
			// dialog as it was before my significant design changes made on 03-Oct-07.
			if (splitRatio > 0)
			{
				PaApp.SettingsHandler.LoadFormProperties(this);
				splitContainer1.SplitterDistance = (int)(splitContainer1.Width * splitRatio);
			}

			PaApp.MsgMediator.SendMessage(Name + "HandleCreated", this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			PaApp.SettingsHandler.SaveFormProperties(this);
			float splitRatio = splitContainer1.SplitterDistance / (float)splitContainer1.Width;
			PaApp.SettingsHandler.SaveSettingsValue(Name, "splitratio", splitRatio);
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosed(EventArgs e)
		{
			Application.Idle -= Application_Idle;
			base.OnClosed(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the chosen database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceInfo ChosenDatabase
		{
			get { return lstFwProjects.SelectedItem as FwDataSourceInfo; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnProperties_Click(object sender, EventArgs e)
		{
			using (FwDataSourcePropertiesDlg dlg =
				new FwDataSourcePropertiesDlg(m_project, ChosenDatabase))
			{
				dlg.ShowDialog(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tvNetwork_AfterSelect(object sender, TreeViewEventArgs e)
		{
			NetworkTreeNode node = e.Node as NetworkTreeNode;
			if (node == null)
				return;

			Cursor = Cursors.WaitCursor;
			btnProperties.Enabled = false;
			btnOK.Enabled = false;
			lstFwProjects.SelectedIndex = -1;
			lstFwProjects.Items.Clear();
			txtMsg.Text = string.Empty;
			txtMsg.Visible = true;
			lstFwProjects.Visible = false;

			if (!string.IsNullOrEmpty(node.MachineName))
			{
				txtMsg.Text = Properties.Resources.kstidSearchingForFwDatabasesMsg;
				txtMsg.Visible = true;
				Application.DoEvents();

				FwDataSourceInfo[] fwDataSourceInfo =
					FwDBUtils.GetFwDataSourceInfoList(node.MachineName, false);

				lstFwProjects.Items.Clear();

				if (fwDataSourceInfo != null && fwDataSourceInfo.Length > 0)
				{
					lstFwProjects.Items.AddRange(fwDataSourceInfo);
					lstFwProjects.SelectedIndex = 0;
					lstFwProjects.Visible = true;
					txtMsg.Visible = false;
				}
				else
				{
					txtMsg.Text = string.Format(
						Properties.Resources.kstidNoFwProjectsFoundMsg,	node.MachineName);
				}
			}

			Cursor = Cursors.Default;
		}
	}
}