using System.Drawing;
using System.Xml.Serialization;
using System.IO;
using SIL.Localize.LocalizingUtils;
using System.Windows.Forms;

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

		private bool m_scanResXFiles = false;
		private string m_prjName;
		private string m_exePath;
		private string m_srcPath;
		private string m_cultureId;
		private SerializableFont m_fntSrc;
		private SerializableFont m_fntTrans;
		private AssemblyResourceInfoList m_assemblyInfoList;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public LocalizerProject()
		{
			m_fntSrc = new SerializableFont(
				new Font("Tahoma", 9.0f, FontStyle.Regular, GraphicsUnit.Point));

			m_fntTrans = new SerializableFont(
				new Font("Tahoma", 9.0f, FontStyle.Regular, GraphicsUnit.Point));
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public LocalizerProject Clone()
		//{
		//    LocalizerProject clone = new LocalizerProject();
		//    clone.m_scanResXFiles = m_scanResXFiles;
		//    clone.m_prjName = m_prjName;
		//    clone.m_cultureId = m_cultureId;
		//    clone.m_exePath = m_exePath;
		//    clone.m_srcPath = m_srcPath;
		//    clone.m_fntSrc = m_fntSrc.Clone();
		//    clone.m_fntTrans = m_fntTrans.Clone();
		//    return clone;
		//}

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
		public bool ScanResXFiles
		{
			get { return m_scanResXFiles; }
			set { m_scanResXFiles = value; }
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
		public Font TranslationFont
		{
			get { return (m_fntTrans == null ? null : m_fntTrans.Font); }
			set
			{
				if (m_fntTrans != null)
					m_fntTrans.Dispose();

				m_fntTrans = new SerializableFont(value);
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
		/// Used only for serialization. Use TranslationFont property for use in the program.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("TranslationFont")]
		public SerializableFont TransFont
		{
			get { return m_fntTrans; }
			set { m_fntTrans = value; }
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
		public string SourcePath
		{
			get { return m_srcPath; }
			set { m_srcPath = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Filename
		{
			get { return kPrjFileNamePrefix + CultureId + ".lop"; }
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
				Program.DeserializeData(fileName, typeof(LocalizerProject)) as LocalizerProject;

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

			if (m_scanResXFiles)
			{
				ResXReader resXRreader = new ResXReader();
				arInfoList = resXRreader.Read(m_srcPath, sslProgressBar, progressBar);
			}
			else
			{
				ResDllReader dllRreader = new ResDllReader();
				arInfoList = dllRreader.Read(m_srcPath, sslProgressBar, progressBar);
			}

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

			Program.SerializeData(path, this);
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