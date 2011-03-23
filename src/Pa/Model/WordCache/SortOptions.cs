using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SIL.Pa.Model
{
	public enum PhoneticSortType { POA, MOA, Unicode };

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// The PhoneticSortOptions class holds the sort options information.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SortOptions
	{
		private PaProject m_project;

		#region Constructor and Loading
		/// ------------------------------------------------------------------------------------
		public SortOptions() : this(false, null)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// SortOptions constructor.
		/// </summary>
		/// <param name="initializeWithPhonetic">Indicates whether or not the new SortOptions
		/// object's SortInformationList should contain phonetic as the default field
		/// on which to sort.</param>
		/// <param name="project"></param>
		/// ------------------------------------------------------------------------------------
		public SortOptions(bool initializeWithPhonetic, PaProject project)
		{
			// Keeps track of the Before, Item, & After sorting order. Set the default
			// as follows.
			AdvSortOrder = new[] { 1, 0, 2 };

			// Keeps track of the R/L selections. Set the defaults as follows.
			AdvRlOptions = new[] { true, false, false };

			SortFields = new List<SortField>();

			// Default sort is by point of articulation and phonetic field.
			SortType = PhoneticSortType.POA;

			m_project = project;

			if (initializeWithPhonetic && project != null)
				SetPrimarySortField(project.GetPhoneticField(), false, true);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a deep copy of the sort options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptions Copy()
		{
			var copy = new SortOptions(false, m_project);
			copy.SortType = SortType;
			copy.SaveManuallySetSortOptions = SaveManuallySetSortOptions;
			copy.AdvancedEnabled = AdvancedEnabled;
			Array.Copy(AdvSortOrder, copy.AdvSortOrder, AdvSortOrder.Length);
			Array.Copy(AdvRlOptions, copy.AdvRlOptions, AdvRlOptions.Length);
			copy.SortFields = SortFields.Select(sf => sf.Copy()).ToList();

			return copy;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Compares the contents of this SortOptions object with the one specified.
		///// TODO: Write some tests for this method. It could be used to fix PA-830.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public bool AreEqual(SortOptions otherOptions)
		//{
		//    if (otherOptions == null)
		//        return false;

		//    if (m_sortType != otherOptions.m_sortType ||
		//        m_advancedEnabled != otherOptions.m_advancedEnabled ||
		//        m_saveManuallySetSortOptions != otherOptions.m_saveManuallySetSortOptions)
		//    {
		//        return false;
		//    }

		//    for (int i = 0; i < m_advSortOptions.Length; i++)
		//    {
		//        if (m_advSortOptions[i] != otherOptions.m_advSortOptions[i])
		//            return false;
		//    }

		//    for (int i = 0; i < m_advRlOptions.Length; i++)
		//    {
		//        if (m_advRlOptions[i] != otherOptions.m_advRlOptions[i])
		//            return false;
		//    }

		//    if (m_sortInfoList == null && otherOptions.m_sortInfoList != null ||
		//        m_sortInfoList != null && otherOptions.m_sortInfoList == null ||
		//        m_sortInfoList.Count != otherOptions.m_sortInfoList.Count)
		//    {
		//        return false;
		//    }

		//    for (int i = 0; i < m_sortInfoList.Count; i++)
		//    {
		//        if (m_sortInfoList[i].ascending != otherOptions.m_sortInfoList[i].ascending ||
		//            m_sortInfoList[i].FieldInfo.FieldName !=
		//            otherOptions.m_sortInfoList[i].FieldInfo.FieldName)
		//        {
		//            return false;
		//        }
		//    }

		//    return true;
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializing a project brings in only the field names for the sort options's
		/// SortFields property. We actually want each SortFields entry to contain a pointer
		/// to a project's field. This will make sure each one points to a real PaField.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void PostDeserializeInitialization(PaProject project)
		{
			m_project = project;

			foreach (var sf in SortFields)
			{
				sf.Field = project.Fields.SingleOrDefault(f => f.Name == sf.PaFieldName);
				sf.PaFieldName = null;
			}

			// Toss out any fields that couldn't be mapped, although that should probably never happen.
			SortFields = SortFields.Where(sf => sf.Field != null).ToList();

			// We have to have at least one sort field.
			if (SortFields.Count == 0)
			{
				SetPrimarySortField(project.Fields.SingleOrDefault(f =>
					f.Type == FieldType.Phonetic), false, true);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes the specified field the first, or primary, field on which to sort.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SetPrimarySortField(string newPrimarySortField, bool changeDirection)
		{
			return (m_project == null ? false :
				SetPrimarySortField(m_project.GetFieldForName(newPrimarySortField), changeDirection));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes the specified field the first, or primary, field on which to sort.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SetPrimarySortField(PaField field, bool changeDirection)
		{
			return SetPrimarySortField(field, changeDirection, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes the specified field the first, or primary, field on which to sort.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SetPrimarySortField(PaField field, bool changeDirection, bool ascending)
		{
			if (field == null)
				return ascending;

			var sortInfo = SortFields.SingleOrDefault(si => si.Field.Name == field.Name);
			int index = (sortInfo == null ? -1 : SortFields.IndexOf(sortInfo));

			// If the sort information list already contains an item for the specified field,
			// we need to remove it before reinserting it at the beginning of the list.
			if (index > -1)
			{
				ascending = SortFields[index].Ascending;
				SortFields.RemoveAt(index);
			}

			if (changeDirection)
				ascending = !ascending;

			// Now insert an item at the beginning of the list since the specified field
			// has now become the first (i.e. primary) field on which to sort.
			SortFields.Insert(0, new SortField(field, ascending));

			return ascending;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets SortType.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneticSortType SortType { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to save as the defaults, those sort
		/// sort options the user specifies as he clicks grid column headings or changes
		/// phonetic sort options via the phonetic sort options drop-down. Setting this value
		/// to true will cause the defaults set in the options dialog to be overridden as the
		/// user clicks column headings or changes phonetic sort options from the drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SaveManuallySetSortOptions { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets AdvancedChecked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool AdvancedEnabled { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets AdvSortOrder. This array holds three items. 0 = preceding environment,
		/// 1 = search item, and 2 = following environment.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int[] AdvSortOrder { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets AdvRlOptions. This array holds three items. 0 = preceding environment,
		/// 1 = search item, and 2 = following environment.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool[] AdvRlOptions { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets SortInformationList.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("sortFields"), XmlArrayItem("field")]
		public List<SortField> SortFields { get; set; }

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets 0 when the preceding environment is sorted first in the advanced search
		///// option order, 1 when it is second, and 2 when it is third. When advanced sorting
		///// is not enabled, return -1.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public int PrecedingEnvironmentsAdvOrder
		//{
		//    get { return (AdvancedEnabled ? AdvSortOrder[0] : -1); }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets 0 when the search item is sorted first in the advanced search option
		///// order, 1 when it is second, and 2 when it is third. When advanced sorting
		///// is not enabled, return -1.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public int SearchItemAdvOrder
		//{
		//    get { return (AdvancedEnabled ? AdvSortOrder[1] : -1); }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets 0 when the following environment is sorted first in the advanced search
		///// option order, 1 when it is second, and 2 when it is third. When advanced sorting
		///// is not enabled, return -1.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public int FollowingEnvironmentsAdvOrder
		//{
		//    get { return (AdvancedEnabled ? AdvSortOrder[2] : -1); }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets a value indicating whether or not (when advanced sorting is enabled) the
		///// preceding environment is sorted right-to-left.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public bool IsPrecedingEnvironmentsSortedRtoL
		//{
		//    get { return (AdvancedEnabled && AdvRlOptions[0]); }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets a value indicating whether or not (when advanced sorting is enabled) the
		///// search item is sorted right-to-left.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public bool IsSearchItemSortedRtoL
		//{
		//    get { return (AdvancedEnabled && AdvRlOptions[1]); }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets a value indicating whether or not (when advanced sorting is enabled) the
		///// following environment is sorted right-to-left.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public bool FollowingEnvironmentsSortedRtoL
		//{
		//    get { return (AdvancedEnabled && AdvRlOptions[2]); }
		//}
	}
}
