using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using SIL.Pa.DataSource;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	public class PaProjectLite
	{
		[XmlIgnore]
		public string Name { get; private set; }
		[XmlIgnore]
		public string FilePath { get; private set; }
		[XmlIgnore]
		public string Version { get; private set; }
		[XmlIgnore]
		public string DataSourceTypes { get; private set; }

		/// ------------------------------------------------------------------------------------
		public static PaProjectLite Create(string prjFile)
		{
			try
			{
				var root = XElement.Load(prjFile);
				if (root.Name.LocalName != "PaProject")
					return null;

				var prjInfo = new PaProjectLite
				{
					FilePath = prjFile,
					Version = (root.Attribute("version") == null ? "3.0.1" : root.Attribute("version").Value),
				};

				prjInfo.Name = GetProjectName(root);
				if (prjInfo.Name == null)
					return null;

				prjInfo.DataSourceTypes = GetDataSourceTypes(root);
				return prjInfo;
			}
			catch
			{
				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		private static string GetProjectName(XElement root)
		{
			if (root.Element("name") != null)
				return root.Element("name").Value;

			return (root.Element("ProjectName") != null ?
				root.Element("ProjectName").Value : null);
		}

		/// ------------------------------------------------------------------------------------
		private static string GetDataSourceTypes(XElement root)
		{
			var dsTypes = root.Element("DataSources").Elements("DataSource")
				.Where(e => e.Element("Type") != null)
				.Select(e => e.Element("Type").Value)
				.Distinct(StringComparer.Ordinal).ToArray();

			if (dsTypes.Length == 0)
			{
				dsTypes = root.Element("DataSources").Elements("DataSource")
					.Where(e => e.Element("DataSourceType") != null)
					.Select(e => e.Element("DataSourceType").Value)
					.Distinct(StringComparer.Ordinal).ToArray();
			}

			if (dsTypes.Length == 0)
				return DataSourceType.Unknown.ToString();

			var bldr = new StringBuilder();

			foreach (var type in dsTypes)
				bldr.AppendFormat("{0}, ", type);

			bldr.Length -= 2;
			return bldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}
	}
}
