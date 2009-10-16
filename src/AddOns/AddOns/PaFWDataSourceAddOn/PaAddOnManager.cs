using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.Pa;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;
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
		protected bool OnAfterLoadingDataSources(object args)
		{
			PaProject project = args as PaProject;

			if (project == null || project.DataSources == null || project.DataSources.Count == 0)
				return false;

			// Go through all the data sources and if any one is not an SA data source, then
			// don't bother fixing up the project so it treats references specially for SA
			// data sources.
			foreach (PaDataSource source in project.DataSources)
			{
				if (source.DataSourceType == DataSourceType.FW && source.FwDataSourceInfo != null)
				{
					QueryResultOutputDlg dlg = new QueryResultOutputDlg(source.FwDataSourceInfo);
					dlg.Show();
				}
			}

			return false;
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
