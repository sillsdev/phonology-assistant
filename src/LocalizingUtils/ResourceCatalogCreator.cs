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
	public class ResourceCatalogCreator
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get information about each project found in the specified root path.
		/// </summary>
		/// <returns>A list of objects containing information about each project found in the
		/// specified root path. The information returned in each object is the project's
		/// path, root namespace and assembly name.</returns>
		/// ------------------------------------------------------------------------------------
		public static ResourceCatalog Create(List<string> csProjFiles)
		{
			return Create(csProjFiles, null);
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get information about each project found in the specified root path.
		/// </summary>
		/// <returns>A list of objects containing information about each project found in the
		/// specified root path. The information returned in each object is the project's
		/// path, root namespace and assembly name.</returns>
		/// ------------------------------------------------------------------------------------
		public static ResourceCatalog Create(List<string> csProjFiles,
			ProgressBar progressBar)
		{
			if (csProjFiles == null || csProjFiles.Count == 0)
				return null;

			if (progressBar != null)
			{
				progressBar.Value = 0;
				progressBar.Maximum = csProjFiles.Count;
			}

			ResourceCatalog prjCatalogList = new ResourceCatalog();

			// Find all the project files and extract each
			// project's root namespace and assembly name.
			foreach (string csproj in csProjFiles)
			{
				if (progressBar != null)
				{
					progressBar.Value++;
					Application.DoEvents();
				}

				if (!File.Exists(csproj))
					continue;
	
				string csPrjPath = Path.GetDirectoryName(csproj);
				string nameSpace = string.Empty;
				CSharpProject prjCatalog = new CSharpProject();
				XmlTextReader reader = new XmlTextReader(csproj);

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						if (reader.Name == "RootNamespace")
						{
							reader.Read();
							nameSpace = reader.Value;
							//prjCatalog.RootNameSpace = reader.Value;
						}
						else if (reader.Name == "AssemblyName")
						{
							reader.Read();
							prjCatalog.AssemblyName = reader.Value;
						}
						else if (reader.Name == "EmbeddedResource")
						{
							ResXCatalog rcc = GetResXCatalog(reader, csPrjPath, nameSpace);
							if (rcc != null)
								prjCatalog.ResXCatalogList.Add(rcc);
						}
					}
				}

				reader.Close();
				
				if (!string.IsNullOrEmpty(prjCatalog.AssemblyName) && prjCatalog.ResXCatalogList.Count > 0)
					prjCatalogList.Add(prjCatalog);
			}

			return prjCatalogList;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static ResXCatalog GetResXCatalog(XmlTextReader reader, string prjPath, string nameSpace)
		{
			if (reader == null || string.IsNullOrEmpty(prjPath) || string.IsNullOrEmpty(nameSpace))
				return null;

			reader.MoveToAttribute("Include");
			if (reader.NodeType != XmlNodeType.Attribute || reader.Name != "Include")
				return null;

			string resx = reader.Value as string;
			if (string.IsNullOrEmpty(resx) || !resx.EndsWith(".resx"))
				return null;

			string resXFile = Path.Combine(prjPath, resx);
			string newNameSpace = LocalizingHelper.VerifyNamespace(resXFile, nameSpace);
			
			// If the namespace going into the VerifyNamespace method is different from the
			// one coming out, then it means the resx file has a designer and it's namespace
			// was found in the designer file. Therefore, if the resx path contains a slash
			// (meaning it's found in a subfolder), we only want the portion of the path
			// that follows the last backslash.
			if (newNameSpace != nameSpace)
			{
				int i = resx.LastIndexOf('\\');
				if (i >= 0)
					resx = resx.Substring(i + 1);
			}

			ResXCatalog rcc = new ResXCatalog();
			resx = Path.GetFileNameWithoutExtension(resx);
			rcc.ResXName = newNameSpace + "." + resx.Replace('\\', '.');
			rcc.CatalogEntryList = ReadResXCatalogEntries(resXFile);
			return (rcc.CatalogEntryList.Count > 0 ? rcc : null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static List<ResXCatalogEntry> ReadResXCatalogEntries(string resXFile)
		{
			List<ResXCatalogEntry> entries = new List<ResXCatalogEntry>();

			ResXResourceReader reader = new ResXResourceReader(resXFile);
			if (reader == null)
				return entries;

			reader.UseResXDataNodes = true;
			IDictionaryEnumerator dict = reader.GetEnumerator();

			while (dict.MoveNext())
			{
				ResXDataNode node = dict.Value as ResXDataNode;
				if (node == null || node.Name.StartsWith(">>") || node.Comment == string.Empty)
					continue;

				try
				{
					object value = node.GetValue((ITypeResolutionService)null);
					if (value != null && value.GetType() == typeof(string))
					{
						ResXCatalogEntry entry = new ResXCatalogEntry();
						entry.comment = node.Comment;
						entry.value = value as string;
						entry.id = node.Name;
						entries.Add(entry);
					}
				}
				catch { }
			}

			reader.Close();
			return entries;
		}
	}
}
