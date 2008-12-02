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
		/// ------------------------------------------------------------------------------------
		public AssemblyResourceInfo this[string assemblyName]
		{
			get
			{
				foreach (AssemblyResourceInfo info in this)
				{
					if (info.AssemblyName == assemblyName)
						return info;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int StringEntryCount
		{
			get
			{
				int count = 0;
				foreach (AssemblyResourceInfo info in this)
					count += info.StringEntryCount;

				return count;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ResourceEntry GetResourceEntry(int i)
		{
			int upperIndex = 0;
			int lowerIndex = 0;
			foreach (AssemblyResourceInfo info in this)
			{
				upperIndex += info.StringEntryCount;
				if (i < upperIndex)
					return info[i - lowerIndex];

				lowerIndex = upperIndex;
			}

			return null;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new void Sort()
		{
			base.Sort(LocalizingHelper.AssemblyResourceInfoComparer);
			foreach (AssemblyResourceInfo info in this)
				info.Sort();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetBackReferences()
		{
			foreach (AssemblyResourceInfo assembly in this)
				assembly.SetBackReferences();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Merge(AssemblyResourceInfoList arInfoList)
		{
			if (arInfoList == null)
				return false;

			bool merged = false;

			foreach (AssemblyResourceInfo info in arInfoList)
			{
				AssemblyResourceInfo matchedValue = this[info.AssemblyName];
				if (matchedValue == null && info.ResourceInfoList.Count > 0)
				{
					Add(info);
					merged = true;
				}
				else if (matchedValue.Merge(info.ResourceInfoList))
					merged = true;
			}

			return merged;
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
		private bool m_omitted = false;
		private string m_assemblyFolder;
		private string m_assemblyName;
		private string m_rootNamespace;
		private List<ResourceInfo> m_resourceInfoList;

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
			m_resourceInfoList = new List<ResourceInfo>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ResourceInfo this[string resourceName]
		{
			get
			{
				foreach (ResourceInfo resInfo in m_resourceInfoList)
				{
					if (resInfo.ResourceName == resourceName)
						return resInfo;
				}
			
				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ResourceEntry this[int i]
		{
			get
			{
				int upperIndex = 0;
				int lowerIndex = 0;
				foreach (ResourceInfo resource in m_resourceInfoList)
				{
					upperIndex += resource.StringEntryCount;
					if (i < upperIndex)
						return resource[i - lowerIndex];

					lowerIndex = upperIndex;
				}

				return null;
			}
		}

		#region Properties
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
		public bool Omitted
		{
			get { return m_omitted; }
			set { m_omitted = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<ResourceInfo> ResourceInfoList
		{
			get { return m_resourceInfoList; }
			set { m_resourceInfoList = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int StringEntryCount
		{
			get 
			{
				int count = 0;
				foreach (ResourceInfo resource in m_resourceInfoList)
					count += resource.StringEntryCount;

				return count;
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void SetBackReferences()
		{
			if (m_resourceInfoList != null)
			{
				foreach (ResourceInfo resource in m_resourceInfoList)
					resource.SetBackReferences(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Sort()
		{
			m_resourceInfoList.Sort(LocalizingHelper.ResourceInfoComparer);
			foreach (ResourceInfo info in m_resourceInfoList)
				info.Sort();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal bool Merge(List<ResourceInfo> resInfoList)
		{
			if (resInfoList == null)
				return false;

			bool merged = false;

			foreach (ResourceInfo resInfo in resInfoList)
			{
				ResourceInfo matchingValue = this[resInfo.ResourceName];
				if (matchingValue == null && resInfo.StringEntries.Count > 0)
				{
					m_resourceInfoList.Add(resInfo);
					merged = true;
				}
				else if (matchingValue.Merge(resInfo))
					merged = true;
			}

			return merged;
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
			foreach (ResourceInfo resInfo in m_resourceInfoList)
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return m_assemblyName;
		}
	}

	#endregion
}
