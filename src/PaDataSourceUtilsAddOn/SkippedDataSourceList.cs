using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using SIL.SpeechTools.Utils;
using System.Windows.Forms;

namespace SIL.Pa.DataSourceUtilsAddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("SkippedDataSources")]
	public class InternalSkipList : List<string>
	{
	}

	public class SkippedDataSourceList : Dictionary<string, bool>
	{
		public const string kDataSourceInfoFilePrefix = "SkippedDataSources.xml";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SkippedDataSourceList Load(PaProject project)
		{
			if (project != null)
			{
				SkippedDataSourceList sdsl = new SkippedDataSourceList();
				foreach (PaDataSource ds in project.DataSources)
					sdsl[ds.ToString(true)] = false;
				
				string filename = project.ProjectPathFilePrefix + kDataSourceInfoFilePrefix;

				if (File.Exists(filename))
				{
					InternalSkipList skipList = STUtils.DeserializeData(filename,
						typeof(InternalSkipList)) as InternalSkipList;

					if (skipList != null)
					{
						foreach (string dsName in skipList)
						{
							if (sdsl.ContainsKey(dsName))
								sdsl[dsName] = true;
						}
					}
				}

				return sdsl;
			}

			return new SkippedDataSourceList();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(PaProject project)
		{
			if (project != null)
			{
				InternalSkipList skipList = new InternalSkipList();
				foreach (KeyValuePair<string, bool> kvp in this)
				{
					if (kvp.Value)
						skipList.Add(kvp.Key);
				}

				string filename = project.ProjectPathFilePrefix + kDataSourceInfoFilePrefix;
				STUtils.SerializeData(filename, skipList);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SkipDataSource(string dsName)
		{
			bool fSkip;
			return (!string.IsNullOrEmpty(dsName) && TryGetValue(dsName, out fSkip) ?
				fSkip : false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will fill our list with the data sources from the specified project.
		/// The skipped values are saved before rebuilding the list so we can use them as the
		/// list is being built.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InitializeFromProject(PaProject project)
		{
			SkippedDataSourceList tmpList = new SkippedDataSourceList();
			
			// Copy this list to a temp. list to save it's values before clearing this list.
			foreach (KeyValuePair<string, bool> kvp in this)
				tmpList[kvp.Key] = kvp.Value;

			Clear();

			// Go through the data sources, adding them to this list and setting each data
			// sources skip value to those found in the temp. list. If there is a data
			// source in the project that isn't in the temp. list, then don't skip it.
			foreach (PaDataSource ds in project.DataSources)
			{
				bool fSkip;
				string dsName = ds.ToString(true);
				if (!tmpList.TryGetValue(dsName, out fSkip))
					fSkip = false;

				this[dsName] = fSkip;
			}
		}
	}
}
