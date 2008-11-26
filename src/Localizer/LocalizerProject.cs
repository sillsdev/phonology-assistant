using System.Drawing;
using System.Xml.Serialization;
using System.IO;
using SIL.Localize.LocalizingUtils;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SIL.Localize.Localizer
{
	#region LocalizerProject class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class LocalizerProject
	{
		private const string kPrjFileNamePrefix = "LocalizerProject.";

		private string m_prjName;
		private string m_cultureId;
		private string m_exePath;
		private string m_resCatalogPath;
		private List<string> m_srcPaths;
		private SerializableFont m_fntSrc;
		private SerializableFont m_fntTarget;
		private AssemblyResourceInfoList m_assemblyInfoList;
		private ResourceCatalog m_resCatalog;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public LocalizerProject()
		{
			m_fntSrc = new SerializableFont(
				new Font("Tahoma", 9.0f, FontStyle.Regular, GraphicsUnit.Point));

			m_fntTarget = new SerializableFont(
				new Font("Tahoma", 9.0f, FontStyle.Regular, GraphicsUnit.Point));
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string ProjectName
		{
			get { return m_prjName; }
			set { m_prjName = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string CultureId
		{
			get { return m_cultureId; }
			set { m_cultureId = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Font SourceTextFont
		{
			get { return (m_fntSrc == null ? null : m_fntSrc.Font); }
			set
			{
				if (m_fntSrc != null)
					m_fntSrc.Dispose();

				m_fntSrc = new SerializableFont(value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Font TargetLangFont
		{
			get { return (m_fntTarget == null ? null : m_fntTarget.Font); }
			set
			{
				if (m_fntTarget != null)
					m_fntTarget.Dispose();

				m_fntTarget = new SerializableFont(value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Used only for serialization. Use SourceTextFont property for use in the program.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("SourceTextFont")]
		public SerializableFont SrcTextFont
		{
			get { return m_fntSrc; }
			set { m_fntSrc = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Used only for serialization. Use TargetLangFont property for use in the program.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("TargetLangFont")]
		public SerializableFont TargetFont
		{
			get { return m_fntTarget; }
			set { m_fntTarget = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string ExePath
		{
			get { return m_exePath; }
			set { m_exePath = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string ResourceCatalogPath
		{
			get { return m_resCatalogPath; }
			set
			{
				m_resCatalogPath = value;
				if (File.Exists(m_resCatalogPath))
					m_resCatalog = ResourceCatalog.Load(m_resCatalogPath);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArrayItem("File")]
		public List<string> SourceFiles
		{
			get { return m_srcPaths; }
			set { m_srcPaths = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AssemblyResourceInfoList AssemblyInfoList
		{
			get { return m_assemblyInfoList; }
			set { m_assemblyInfoList = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public ResourceCatalog ResourceCatalog
		{
			get { return m_resCatalog; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static LocalizerProject Load(string fileName)
		{
			if (!File.Exists(fileName))
				return null;

			LocalizerProject project =
				LocalizingHelper.DeserializeData(fileName, typeof(LocalizerProject)) as LocalizerProject;

			if (project != null)
				project.m_assemblyInfoList.Sort();

			return project;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Scan()
		{
			m_assemblyInfoList = InternalScan(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Scan(ToolStripStatusLabel sslProgressBar, ToolStripProgressBar progressBar)
		{
			m_assemblyInfoList = InternalScan(sslProgressBar, progressBar);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ReScan(ToolStripStatusLabel sslProgressBar, ToolStripProgressBar progressBar)
		{
			if (!m_assemblyInfoList.Merge(InternalScan(sslProgressBar, progressBar)))
				return false;

			m_assemblyInfoList.Sort();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected AssemblyResourceInfoList InternalScan(ToolStripStatusLabel sslProgressBar,
			ToolStripProgressBar progressBar)
		{
			AssemblyResourceInfoList arInfoList;
			ResDllReader dllRreader = new ResDllReader(m_srcPaths);
			arInfoList = dllRreader.Read(null, sslProgressBar, progressBar);
			arInfoList.Sort();
			return arInfoList;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Save(string path)
		{
			if (string.IsNullOrEmpty(m_cultureId) || string.IsNullOrEmpty(path))
				return false;

			LocalizingHelper.SerializeData(path, this);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Compile(string outputPath)
		{
			m_assemblyInfoList.Compile(m_cultureId, outputPath);
		}
	}

	#endregion
}