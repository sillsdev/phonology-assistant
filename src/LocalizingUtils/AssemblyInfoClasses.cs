using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SIL.Localize.LocalizingUtils
{
	#region AssemblyResourceInfoList class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AssemblyResourceInfoList : List<AssemblyResourceInfo>
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AssemblyResourceInfoList()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cultureId"></param>
		/// <param name="exePath"></param>
		/// ------------------------------------------------------------------------------------
		public void Compile(string cultureId, string exePath)
		{
			foreach (AssemblyResourceInfo assemblyInfo in this)
				assemblyInfo.Compile(cultureId, exePath);
		}
	}

	#endregion

	#region AssemblyResourceInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AssemblyResourceInfo
	{
		private string m_assemblyFolder;
		private string m_assemblyName;
		private string m_rootNamespace;
		private List<RessourceInfo> m_resourceInfoList;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AssemblyResourceInfo()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal AssemblyResourceInfo(string assemblyFolder)
		{
			m_assemblyFolder = assemblyFolder;
			m_resourceInfoList = new List<RessourceInfo>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string AssemblyFolder
		{
			get { return m_assemblyFolder; }
			set { m_assemblyFolder = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string RootNamespace
		{
			get { return m_rootNamespace; }
			set { m_rootNamespace = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string AssemblyName
		{
			get { return m_assemblyName; }
			set { m_assemblyName = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<RessourceInfo> ResourceInfoList
		{
			get { return m_resourceInfoList; }
			set { m_resourceInfoList = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void Compile(string cultureId, string exePath)
		{
			string tmpPath = Path.GetTempPath();
			tmpPath = Path.Combine(tmpPath, "~LocalizingUtilsBuild~");
			Directory.CreateDirectory(tmpPath);

			// Build the binary resource files.
			List<string> binaryResFiles = new List<string>();
			foreach (RessourceInfo resInfo in m_resourceInfoList)
			{
				string binRes = resInfo.BuildBinaryResourceFile(cultureId, tmpPath);
				if (!string.IsNullOrEmpty(binRes))
					binaryResFiles.Add(binRes);
			}

			if (binaryResFiles.Count > 0)
			{
				// Compile the binary resource files into a satellite resource .dll.
				LocalizingHelper.CompileLocalizedAssembly(cultureId,
					 m_assemblyName, binaryResFiles.ToArray(), exePath);
			}

			try
			{
				foreach (string binRes in binaryResFiles)
					File.Delete(binRes);

				Directory.Delete(tmpPath);
			}
			catch { }
		}
	}

	#endregion
}
