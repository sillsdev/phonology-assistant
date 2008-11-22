using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using System.Collections;

namespace SIL.Localize.LocalizingUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Reads the assembly DLL files found in one or more .Net projects.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ResDllReader : ResReaderBase
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
			// Find all the DLLs in the root folder and all its subfolders.
			string[] dllFiles = Directory.GetFiles(rootPath, "*.dll", SearchOption.AllDirectories);
			if (dllFiles == null || dllFiles.Length == 0)
				return null;

			AssemblyResourceInfoList dllInfoList = new AssemblyResourceInfoList();

			// Find all the project files and extract each
			// project's root namespace and assembly name.
			foreach (string dll in dllFiles)
			{
				string dllPath = Path.GetDirectoryName(dll);
				AssemblyResourceInfo arInfo = new AssemblyResourceInfo(dllPath);
				arInfo.AssemblyName = dll;
				dllInfoList.Add(arInfo);
			}

			return dllInfoList;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void ReadResourceInfoForAssembly(AssemblyResourceInfo assemblyInfo)
		{
			string dll = assemblyInfo.AssemblyName;
			assemblyInfo.AssemblyName = Path.GetFileNameWithoutExtension(dll);
			Assembly assembly = null;

			try
			{
				assembly = Assembly.LoadFile(dll);
			}
			catch
			{
				return;
			}
			
			List<string> resNames = new List<string>(assembly.GetManifestResourceNames());
			if (resNames.Count == 0)
			    return;

			for (int i = resNames.Count - 1; i >= 0; i--)
			{
				try
				{
					string internalResName = resNames[i];
					int index = internalResName.LastIndexOf(".resources");
					if (index >= 0)
						internalResName = internalResName.Remove(index);

					List<ResourceEntry> entries = ReadResStringEntries(assembly, resNames[i]);
					if (entries.Count > 0)
					{
						ResourceInfo resInfo = new ResourceInfo(internalResName);
						resInfo.StringEntries = entries;
						assemblyInfo.ResourceInfoList.Add(resInfo);
					}
				}
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<ResourceEntry> ReadResStringEntries(Assembly assembly, string resName)
		{
		    List<ResourceEntry> stringEntries = new List<ResourceEntry>();

			using (Stream stream = assembly.GetManifestResourceStream(resName))
			{
				ResourceReader reader = new ResourceReader(stream);
				if (reader == null)
					return stringEntries;

				IDictionaryEnumerator dict = reader.GetEnumerator();
				while (dict.MoveNext())
				{
					string value = dict.Value as string;
					string stringId = dict.Key as string;
					if (value != null && stringId != null && !stringId.StartsWith(">>"))
					{
						ResourceEntry entry = new ResourceEntry();
						entry.StringId = stringId;
						entry.SourceText = value;
						stringEntries.Add(entry);
					}
				}

				reader.Close();
			}

			return stringEntries;
		}
	}
}
