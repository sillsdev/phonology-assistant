using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.IO;
using SIL.Pa;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;
using XCore;

// I don't want to use a custom attribute, so I'm
// kludging what I want by using this attribute.
[assembly: System.Reflection.AssemblyDefaultAlias("CanBeDisabled")]

namespace SIL.Pa.SaDataSourceAddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaAddOnManager : IxCoreColleague
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaAddOnManager()
		{
			try
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				string settingName = Path.GetFileNameWithoutExtension(assembly.CodeBase);
				if (PaApp.SettingsHandler.GetBoolSettingsValue(settingName, "Enabled", true))
					PaApp.AddMediatorColleague(this);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAfterLoadingDataSources(object args)
		{
			PaProject project = args as PaProject;

			if (project != null && project.DataSources != null && project.DataSources.Count > 0)
			{
				// Go through all the data sources and if any one is not an SA data source, then
				// do not bother fixing up the project so it treats references specially for SA
				// data sources.
				bool fixRefs = true;
				foreach (PaDataSource source in project.DataSources)
				{
					if (source.DataSourceType != DataSourceType.SA)
					{
						fixRefs = false;
						break;
					}
				}

				if (fixRefs)
					FixReferenceForSAProject(project);
			}
			
			PaApp.MsgMediator.SendMessage("AfterSaDataSourceAddOnHandledLoadedingDataSources", args);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FixReferenceForSAProject(PaProject project)
		{
			PaFieldInfo refFieldInfo = project.FieldInfo.ReferenceField;

			if (!refFieldInfo.IsParsed || refFieldInfo.SaFieldName != "Reference")
			{
				refFieldInfo.IsParsed = true;
				refFieldInfo.SaFieldName = "Reference";
				project.FieldInfo.Save(project);
				project.ReloadDataSources();
			}

			// Setting the parse type for each of the SA data sources has no bearing on how
			// the data is parsed, but it is a back-handed way of keeping the PaWordListGrid
			// from displaying all the reference fields for a transcription for those entries
			// that don't have a reference specified.
			foreach (PaDataSource source in project.DataSources)
				source.ParseType = DataSourceParseType.OneToOne;

			FixedAudioDocReader reader = new FixedAudioDocReader();
			foreach (RecordCacheEntry recEntry in PaApp.RecordCache)
			{
				if (recEntry.DataSource.DataSourceType == DataSourceType.SA)
					reader.Read(recEntry);
			}

			PaApp.RecordCache.BuildWordCache(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Use a special sort to make sure the reference fields for word list cache entries
		/// that come from audio files are taken from the word list cache entries and not the
		/// record cache entries that own the word list entries.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSortCache(object args)
		{
			object[] argArray = args as object[];
			if (argArray == null || argArray.Length < 2)
				return false;

			WordListCache wcache = argArray[0] as WordListCache;
			SortOptions sortOptions = argArray[1] as SortOptions;
			if (wcache == null || sortOptions == null)
				return false;

			wcache.Sort(new SACacheSortComparer(sortOptions));
			return true;
		}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] {this});
		}

		#endregion
	}

	#region SACacheSortComparer class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	internal class SACacheSortComparer : IComparer<WordListCacheEntry>
	{
		private readonly SortInformationList m_sortInfoList;
		private readonly SortOptions m_sortOptions;
		private readonly CacheSortComparer m_baseCasheSortComparer;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The CacheSort constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SACacheSortComparer(SortOptions sortOptions)
		{
			m_baseCasheSortComparer = new CacheSortComparer(sortOptions);

			m_sortInfoList = sortOptions.SortInformationList;
			m_sortOptions = sortOptions;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Compare(WordListCacheEntry x, WordListCacheEntry y)
		{
			if (x == null && y == null)
				return 0;

			bool ascending = (m_sortInfoList.Count > 0 ? m_sortInfoList[0].ascending : true);

			if (x == null)
				return (ascending ? -1 : 1);

			if (y == null)
				return (ascending ? 1 : -1);
			
			// First compare CIE group Id's before anything else.
			if (x.CIEGroupId >= 0 && y.CIEGroupId >= 0 && x.CIEGroupId != y.CIEGroupId)
				return (x.CIEGroupId - y.CIEGroupId);

			// Continue with the next iteration if the fieldValues are EQUAL
			for (int i = 0; i < m_sortInfoList.Count; i++)
			{
				ascending = m_sortInfoList[i].ascending;
				int compareResult;

				// Use a special comparison for phonetic fields.
				if (m_sortInfoList[i].FieldInfo.IsPhonetic)
				{
					compareResult = m_baseCasheSortComparer.ComparePhonetic(x, y);
					if (compareResult == 0)
						continue;
					
					return (ascending ? compareResult : -compareResult);
				}

				string fieldName = m_sortInfoList[i].FieldInfo.FieldName;
				string fieldValue1 = x[fieldName];
				string fieldValue2 = y[fieldName];

				if (m_sortInfoList[i].FieldInfo.IsReference)
				{
					if (x.WordCacheEntry.RecordEntry.DataSource.DataSourceType == DataSourceType.SA)
						fieldValue1 = x.WordCacheEntry.GetField(fieldName, false);

					if (y.WordCacheEntry.RecordEntry.DataSource.DataSourceType == DataSourceType.SA)
						fieldValue2 = y.WordCacheEntry.GetField(fieldName, false);

					// Try to compare references as numeric values before
					// assuming they're string values.
					float val1, val2;
					if (STUtils.TryFloatParse(fieldValue1, out val1) &&
						STUtils.TryFloatParse(fieldValue2, out val2) && val1 != val2)
					{
						compareResult = (val1 > val2 ? 1 : -1);
						return (ascending ? compareResult : -compareResult);
					}
				}

				if (fieldValue1 == fieldValue2)
				{
					// Use a special comparison for references.
					if (m_sortInfoList[i].FieldInfo.IsReference)
					{
						compareResult = m_baseCasheSortComparer.CompareReferences(x, y);
						if (compareResult == 0)
							continue;

						return (ascending ? compareResult : -compareResult);
					}

					// If we're sorting by the entry's audio file and the audio file for each
					// entry is the same, then compare the order in which the words occur within
					// the sound file transcription.
					if (m_sortInfoList[i].FieldInfo.IsAudioFile &&
						x.WordCacheEntry.WordIndex != y.WordCacheEntry.WordIndex)
					{
						compareResult = x.WordCacheEntry.WordIndex - y.WordCacheEntry.WordIndex;
						return (ascending ? compareResult : -compareResult);
					}

					// Fields are equal, so continue onto the next comparison column
					continue;
				}

				// Check for date or numeric fields and compare appropriately.
				if (m_sortInfoList[i].FieldInfo.IsDate)
					compareResult = m_baseCasheSortComparer.CompareDates(fieldValue1, fieldValue2);
				else if (m_sortInfoList[i].FieldInfo.IsNumeric)
					compareResult = m_baseCasheSortComparer.CompareNumerics(fieldValue1, fieldValue2);
				else
					compareResult = m_baseCasheSortComparer.CompareStrings(fieldValue1, fieldValue2);

				if (compareResult == 0)
					continue;

				// Return a negative value for descending order
				return (ascending ? compareResult : -compareResult);
			}

			return 0; // They are equal
		}
	}

	#endregion
}
