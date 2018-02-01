// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright>
#endregion
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using SIL.Pa.DataSource;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	public class PaProjectLite
	{
		private static readonly List<string> Sources = new List<string>();
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
			if (prjFile.EndsWith(".fwdata") && !Sources.Contains(prjFile))
			{
				var prjInfo = new PaProjectLite()
				{
					Name = Path.GetFileNameWithoutExtension(prjFile),
					DataSourceTypes = "New",
					FilePath = prjFile,
					Version = "7.0+"
				};
				return prjInfo;
			}
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

					var source = root.XPathSelectElement("//DataSourceFile[1]");
				if (source != null) Sources.Add(source.Value.Trim());
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
