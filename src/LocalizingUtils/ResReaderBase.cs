using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace SIL.Localize.LocalizingUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Reads the resx files found in one or more .Net projects.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ResReaderBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual AssemblyResourceInfoList Read(string rootPath, ToolStripStatusLabel ssl,
			ToolStripProgressBar progressBar)
		{
			AssemblyResourceInfoList assemInfoList = GetAssemblyInfoList(rootPath);
			if (assemInfoList == null || assemInfoList.Count == 0)
				return new AssemblyResourceInfoList();

			if (progressBar != null)
			{
				progressBar.Maximum = assemInfoList.Count;
				progressBar.Value = 0;
				progressBar.Visible = true;
			}

			if (ssl != null)
				ssl.Visible = true;

			Application.DoEvents();
			for (int i = assemInfoList.Count - 1; i >= 0; i--)
			{
				ssl.Text = "Reading resource for " + assemInfoList[i].AssemblyName + ":";

				ReadResourceInfoForAssembly(assemInfoList[i]);
				
				// If there weren't any resx files in the project, then don't bother
				// keeping it in our list.
				if (assemInfoList[i].ResourceInfoList.Count == 0)
					assemInfoList.RemoveAt(i);

				progressBar.Value++;
				Application.DoEvents();
			}

			if (progressBar != null)
				progressBar.Visible = false;

			if (ssl != null)
				ssl.Visible = false;

			return assemInfoList;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get information about each project found in the specified root path.
		/// (Derived classes must override this method.)
		/// </summary>
		/// <returns>A list of objects containing information about each project found in the
		/// specified root path. The information returned in each object is the project's
		/// path, root namespace and assembly name.</returns>
		/// ------------------------------------------------------------------------------------
		protected virtual AssemblyResourceInfoList GetAssemblyInfoList(string rootPath)
		{
			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void ReadResourceInfoForAssembly(AssemblyResourceInfo assemblyInfo)
		{
		}
	}
}
