using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.Pa;
using SIL.Pa.Data;
using SIL.FieldWorks.Common.UIAdapters;
using XCore;

namespace SIL.Pa.AddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaAddOnManager : IxCoreColleague
	{
		private ITMAdapter m_adapter;
		private TMItemProperties m_backupItemProps;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaAddOnManager()
		{
			PaApp.AddMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnMainViewOpened(object args)
		{
			try
			{
				if (args is PaMainWnd)
				{
					m_adapter = PaApp.TMAdapter;

					m_adapter.AddCommandItem("CmdBackupProject", "BackupProject",
						Properties.Resources.kstidBackupMenuText, null, null, null, null,
						null, Keys.None, null, Properties.Resources.kimidBackup);

					m_backupItemProps = new TMItemProperties();
					m_backupItemProps.BeginGroup = true;
					m_backupItemProps.CommandId = "CmdBackupProject";
					m_backupItemProps.Name = "mnuBackupProject";
					m_backupItemProps.Text = null;
					m_adapter.AddMenuItem(m_backupItemProps, "mnuFile", "mnuFileExportAs");
				}
			}
			catch { }

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnBackupProject(object args)
		{
			BackupDlg.Backup();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateBackupProject(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			bool enable = (PaApp.Project != null);

			if (itemProps.Enabled != enable)
			{
				itemProps.Update = true;
				itemProps.Visible = true;
				itemProps.Enabled = enable;
			}

			return true;
		}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] {this});
		}

		#endregion
	}
}
