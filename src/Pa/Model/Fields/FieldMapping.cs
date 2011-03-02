using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Properties;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	[XmlType("mapping")]
	public class FieldMapping
	{
		private string m_paFieldName;

		/// ------------------------------------------------------------------------------------
		public FieldMapping()
		{
		}

		/// ------------------------------------------------------------------------------------
		public FieldMapping(string nameInSource, PaField field, bool isParsed)
		{
			NameInDataSource = nameInSource;
			Field = field;
			IsParsed = isParsed;
		}
	
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
		public static IEnumerable<FieldMapping> GetDefaultFw7Mappings(PaDataSource ds)
		{
			return GetDefaultFw7Mappings(ds.FwDataSourceInfo.GetWritingSystems());
		}

		/// ------------------------------------------------------------------------------------
		public static IEnumerable<FieldMapping> GetDefaultFw7Mappings(
			IEnumerable<FwWritingSysInfo> writingSystems)
		{
			return PaField.GetDefaultFw7Fields()
				.Where(f => Settings.Default.DefaultMappedFw7Fields.Contains(f.Name))
				.Select(f =>
				{
					var mapping = new FieldMapping(f.Name, f, f.Type == FieldType.Phonetic);
					CheckMappingsFw7WritingSystem(mapping, writingSystems);
					return mapping;
				});
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
			var ws = writingSystems.SingleOrDefault(w => w.Id == mapping.FwWsId);
			if (ws != null && mapping.Field.FwWsType == ws.Type)
				return;

			switch (mapping.Field.FwWsType)
			{
				case FwDBUtils.FwWritingSystemType.None:
					mapping.FwWsId = null;
					break;

				case FwDBUtils.FwWritingSystemType.Mixed:
				case FwDBUtils.FwWritingSystemType.Analysis:
					ws = writingSystems.SingleOrDefault(w => w.IsDefaultAnalysis);
					mapping.FwWsId = (ws != null ? ws.Id : null);
					break;

				case FwDBUtils.FwWritingSystemType.Vernacular:
					ws = writingSystems.SingleOrDefault(w => w.IsDefaultVernacular);
					mapping.FwWsId = (ws != null ? ws.Id : null);
					break;
			}
		}
	}
}
