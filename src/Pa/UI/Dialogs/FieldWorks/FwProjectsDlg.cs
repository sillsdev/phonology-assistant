using System;
using System.Diagnostics;
using System.Windows.Forms;
using SIL.Pa.DataSourceClasses.FieldWorks;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
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
		public FwProjectsDlg()
		{
			InitializeComponent();

			if (DesignMode)
				return;

			btnProperties.Margin = new Padding(0, btnOK.Margin.Top, 0, btnOK.Margin.Bottom);
			tblLayoutButtons.Controls.Add(btnProperties, 0, 0);
		}

		/// ------------------------------------------------------------------------------------
		public FwProjectsDlg(PaProject project) : this()
		{
			Debug.Assert(project != null);
			m_project = project;

			lblMsg.Font = FontHelper.UIFont;
			lstFwProjects.Font = FontHelper.UIFont;
			lblProjects.Font = FontHelper.UIFont;
			lblNetwork.Font = FontHelper.UIFont;
			tvNetwork.Font = FontHelper.UIFont;
			txtMsg.Font = FontHelper.UIFont;

			txtMsg.Dock = DockStyle.Fill;
			txtMsg.BringToFront();

			lblProjects.Height = FontHelper.UIFont.Height + 10;
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
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			var loc = Settings.Default.FwProjectsDlgSplitLoc;
			if (loc > 0 && loc >= splitContainer1.Panel1MinSize)
				splitContainer1.SplitterDistance = loc;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			App.MsgMediator.SendMessage(Name + "HandleCreated", this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.FwProjectsDlgSplitLoc = splitContainer1.SplitterDistance;
			base.SaveSettings();
		}

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
		public Fw6DataSourceInfo ChosenDatabase
		{
			get { return lstFwProjects.SelectedItem as Fw6DataSourceInfo; }
		}

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
				txtMsg.Text = App.LocalizeString(
					"FwProjectsDlg.SearchingForFwDatabasesMsg", "Searching...",
					App.kLocalizationGroupDialogs);
				
				txtMsg.Visible = true;
				Application.DoEvents();

				Fw6DataSourceInfo[] fwDataSourceInfo =
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
					var fmt = App.LocalizeString(
						"FwProjectsDlg.NoFwProjectsFoundMsg", "No projects found on '{0}'.",
						App.kLocalizationGroupDialogs);
		
					txtMsg.Text = string.Format(fmt, node.MachineName);
				}
			}

			Cursor = Cursors.Default;
		}
	}
}