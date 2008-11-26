using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Collections;
using System.Resources;
using System.ComponentModel.Design;

namespace SIL.Localize.LocalizingUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Reads the resx files found in one or more .Net projects.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ResXReader : ResReaderBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get information about each project found in the specified root path.
		/// </summary>
		/// <returns>A list of objects containing information about each project found in the
		/// specified root path. The information returned in each object is the project's
		/// path, root namespace and assembly name.</returns>
		/// ------------------------------------------------------------------------------------
		protected override AssemblyResourceInfoList GetAssemblyInfoList(string rootPath)
		{
			// Find all the project files. ENHANCE: allow for VB.Net and other .Net prj. types.
			string[] prjFiles = Directory.GetFiles(rootPath, "*.csproj", SearchOption.AllDirectories);
			if (prjFiles == null || prjFiles.Length == 0)
				return null;

			AssemblyResourceInfoList prjInfoList = new AssemblyResourceInfoList();

			// Find all the project files and extract each
			// project's root namespace and assembly name.
			foreach (string prj in prjFiles)
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
		protected override void ReadResourceInfoForAssembly(AssemblyResourceInfo assemblyInfo)
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
					string rootNamespace = LocalizingHelper.VerifyNamespace(resx, assemblyInfo.RootNamespace);
					string internalResName = Path.GetFileName(resx);
					internalResName = rootNamespace + "." + internalResName;
					internalResName = internalResName.Replace(".resx", string.Empty);
					ResourceInfo resInfo = new ResourceInfo(internalResName);
					resInfo.StringEntries = ReadResXStringEntries(resx);
					assemblyInfo.ResourceInfoList.Add(resInfo);
				}
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<ResourceEntry> ReadResXStringEntries(string resXPath)
		{
			List<ResourceEntry> stringEntries = new List<ResourceEntry>();

			ResXResourceReader reader = new ResXResourceReader(resXPath);
			if (reader == null)
				return stringEntries;

			reader.UseResXDataNodes = true;
			IDictionaryEnumerator dict = reader.GetEnumerator();

			while (dict.MoveNext())
			{
				ResXDataNode node = dict.Value as ResXDataNode;
				if (node == null || node.Name.StartsWith(">>"))
					continue;

				try
				{
					object value = node.GetValue((ITypeResolutionService)null);
					if (value != null && value.GetType() == typeof(string))
					{
						ResourceEntry entry = new ResourceEntry();
						entry.Comment = node.Comment;
						entry.StringId = node.Name;
						entry.SourceText = value as string;
						stringEntries.Add(entry);
					}
				}
				catch { }
			}

			reader.Close();
			return stringEntries;
		}
	}
}
