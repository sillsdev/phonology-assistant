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
	public class ResXReader
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="rootPath">Path (and all its children) to search for .resx files.
		/// ------------------------------------------------------------------------------------
		public AssemblyResourceInfoList Read(string rootPath, ToolStripStatusLabel ssl,
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

			for (int i = assemInfoList.Count - 1; i >= 0; i--)
			{
				ssl.Text = "Reading resource for " + assemInfoList[i].AssemblyName + ":";

				ReadResXInfoForAssembly(assemInfoList[i]);
				
				// If there weren't any resx files in the project, then don't bother
				// keeping it in our list.
				if (assemInfoList[i].ResourceInfoList.Count == 0)
					assemInfoList.RemoveAt(i);

				progressBar.Value++;
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
		/// </summary>
		/// <returns>A list of objects containing information about each project found in the
		/// specified root path. The information returned in each object is the project's
		/// path, root namespace and assembly name.</returns>
		/// ------------------------------------------------------------------------------------
		private AssemblyResourceInfoList GetAssemblyInfoList(string rootPath)
		{
			// Find all the project files. ENHANCE: allow for VB.Net and other .Net prj. types.
			string[] prjFiles = Directory.GetFiles(rootPath, "*.csproj", SearchOption.AllDirectories);
			if (prjFiles == null || prjFiles.Length == 0)
				return null;

			List<string> sortedPrjFiles = new List<string>(prjFiles);
			sortedPrjFiles.Sort();
			AssemblyResourceInfoList prjInfoList = new AssemblyResourceInfoList();

			// Find all the project files and extract each
			// project's root namespace and assembly name.
			foreach (string prj in sortedPrjFiles)
			{
				string prjPath = Path.GetDirectoryName(prj);
				AssemblyResourceInfo prjInfo = new AssemblyResourceInfo(prjPath);
				XmlTextReader reader = new XmlTextReader(prj);

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						if (reader.Name == "RootNamespace")
						{
							reader.Read();
							prjInfo.RootNamespace = reader.Value;
						}
						else if (reader.Name == "AssemblyName")
						{
							reader.Read();
							prjInfo.AssemblyName = reader.Value;
						}
					}
				}

				reader.Close();
				prjInfoList.Add(prjInfo);
			}

			return prjInfoList;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ReadResXInfoForAssembly(AssemblyResourceInfo assemblyInfo)
		{
			// Find all the .resx files in the project folder.
			string[] resxFiles = Directory.GetFiles(assemblyInfo.AssemblyFolder, "*.resx",
				SearchOption.AllDirectories);

			if (resxFiles.Length == 0)
				return;

			foreach (string resx in resxFiles)
			{
				try
				{
					string rootNamespace = VerifyNamespace(resx, assemblyInfo.RootNamespace);
					string internalResName = Path.GetFileName(resx);
					internalResName = rootNamespace + "." + internalResName;
					internalResName = internalResName.Replace(".resx", string.Empty);
					RessourceInfo resInfo = new RessourceInfo(resx, internalResName);
					assemblyInfo.ResourceInfoList.Add(resInfo);
				}
				catch { }
			}

			assemblyInfo.ResourceInfoList.Sort(ResourceInfoComparer);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static int ResourceInfoComparer(RessourceInfo x, RessourceInfo y)
		{
			if (string.IsNullOrEmpty(x.ResourceName) && string.IsNullOrEmpty(y.ResourceName))
				return 0;

			if (string.IsNullOrEmpty(x.ResourceName) && !string.IsNullOrEmpty(y.ResourceName))
				return -1;

			if (!string.IsNullOrEmpty(x.ResourceName) && string.IsNullOrEmpty(y.ResourceName))
				return 1;

			return x.ResourceName.CompareTo(y.ResourceName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks to see if the resx file has a designer file with it. If it does, then get
		/// the namespace from it rather than use the default root namespace for the project.
		/// </summary>
		/// <returns>Returns the namespace found in the designer file, if there is one.
		/// Otherwise, the default namespace is returned.</returns>
		/// ------------------------------------------------------------------------------------
		private string VerifyNamespace(string resxFullPath, string defaultNamespace)
		{
			string resxFile = Path.GetFileNameWithoutExtension(resxFullPath);
			string resxPath = Path.GetDirectoryName(resxFullPath);

			string[] designerFiles = Directory.GetFiles(resxPath,
				resxFile + ".designer.*", SearchOption.TopDirectoryOnly);

			if (designerFiles == null || designerFiles.Length == 0)
				return defaultNamespace;

			string fileContents = File.ReadAllText(designerFiles[0]);
			int i = fileContents.IndexOf("namespace ", StringComparison.Ordinal);
			if (i >= 0)
			{
				int eol = fileContents.IndexOf('\n', i);
				defaultNamespace = fileContents.Substring(i + 10, eol - (i + 10));
				defaultNamespace = defaultNamespace.Replace("{", string.Empty);
				defaultNamespace = defaultNamespace.Trim();
			}

			return defaultNamespace;
		}
	}
}
