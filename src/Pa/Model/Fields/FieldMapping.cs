// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Localization;
using SIL.Pa.DataSource.FieldWorks;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	[XmlType("mapping")]
	public class FieldMapping
	{
		private string m_paFieldName;

		#region Constructors
		/// ------------------------------------------------------------------------------------
		public FieldMapping()
		{
		}

		/// ------------------------------------------------------------------------------------
		public FieldMapping(PaField field, IEnumerable<string> parsedFields)
			: this(field, parsedFields.Contains(field.Name))
		{
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// parsedFields is a delimited (e.g. comma or semi-colon) string of the parsed fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FieldMapping(PaField field, string parsedFields)
			: this(field.Name, field, parsedFields.Contains(field.Name))
		{
		}

		/// ------------------------------------------------------------------------------------
		public FieldMapping(PaField field, bool isParsed) : this(field.Name, field, isParsed)
		{
		}

		/// ------------------------------------------------------------------------------------
		public FieldMapping(string nameInSource, PaField field, bool isParsed)
		{
			NameInDataSource = nameInSource;
			Field = field;
			IsParsed = isParsed;
		}
	
		#endregion

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("nameInSource")]
		public string NameInDataSource { get; set; }
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is used for serialization and deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("paFieldName")]
		public string PaFieldName
		{
			get { return (Field != null ? Field.Name : m_paFieldName); }
			set { m_paFieldName = value; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlElement("isParsed")]
		public bool IsParsed { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("isInterlinear")]
		public bool IsInterlinear { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("fwWritingSystem")]
		public string FwWsId { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public PaField Field { get; set; }

		/// ------------------------------------------------------------------------------------
		public FieldMapping Copy()
		{
			return new FieldMapping
			{
				NameInDataSource = NameInDataSource,
				IsParsed = IsParsed,
				IsInterlinear = IsInterlinear,
				FwWsId = FwWsId,
				PaFieldName = PaFieldName,
				Field = (Field == null ? null : Field.Copy()),
			};
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return NameInDataSource ?? base.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method returns the default FW6 writing system for the specified field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetDefaultFw6WsIdForField(PaField field,
			IEnumerable<FwWritingSysInfo> writingSystems)
		{
			var wsList = writingSystems.ToArray();
			FwWritingSysInfo ws;

			if (field.FwWsType == FwDBUtils.FwWritingSystemType.Vernacular)
			{
				ws = wsList.SingleOrDefault(w => w.IsDefaultVernacular);
				return (ws != null ? ws.Id :
					wsList.First(w => w.Type == FwDBUtils.FwWritingSystemType.Vernacular).Id);
			}

			ws = wsList.SingleOrDefault(w => w.IsDefaultAnalysis);
			return (ws != null ? ws.Id :
				wsList.First(w => w.Type == FwDBUtils.FwWritingSystemType.Analysis).Id);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method verifies that the writing system in the specified mapping is valid
		/// for the mapping's field. If not, then the mapping's writing system is set to the
		/// default writing system of the appropriate type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void CheckMappingsFw7WritingSystem(FieldMapping mapping,
			IEnumerable<FwWritingSysInfo> writingSystems)
		{
			var wsList = writingSystems.ToArray();

			var ws = wsList.SingleOrDefault(w => w.Id == mapping.FwWsId);
			if (ws != null && mapping.Field.FwWsType == ws.Type)
				return;

			ws = null;

			switch (mapping.Field.FwWsType)
			{
				case FwDBUtils.FwWritingSystemType.Analysis:
					ws = wsList.SingleOrDefault(w => w.IsDefaultAnalysis);
					break;
				case FwDBUtils.FwWritingSystemType.Vernacular:
					ws = wsList.SingleOrDefault(w => w.IsDefaultVernacular);
					break;
			}

			mapping.FwWsId = (ws != null ? ws.Id : null);
		}

		/// ------------------------------------------------------------------------------------
		public static bool IsPhoneticMapped(IEnumerable<FieldMapping> mappings, bool showIfNotMapped)
		{
			if (mappings == null)
			{
				App.NotifyUserOfProblem(LocalizationManager.GetString(
					"ProjectFields.NoFieldMappingsMsg",
					"You must specify field mappings."));
				return false;
			}

			var mapped = mappings.Any(m => m.Field != null && m.Field.Type == FieldType.Phonetic);

			if (!mapped && showIfNotMapped)
			{
				App.NotifyUserOfProblem(LocalizationManager.GetString(
					"ProjectFields.NoPhoneticMappingsMsg",
					"You must specify a mapping for the phonetic field."));
			}

			return mapped;
		}
	}
}
