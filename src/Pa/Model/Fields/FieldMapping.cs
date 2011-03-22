using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using SIL.Pa.DataSource.FieldWorks;
using SilTools;

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
			FwWritingSysInfo ws;

			if (field.FwWsType == FwDBUtils.FwWritingSystemType.Vernacular)
			{
				ws = writingSystems.SingleOrDefault(w => w.IsDefaultVernacular);
				return (ws != null ? ws.Id :
					writingSystems.First(w => w.Type == FwDBUtils.FwWritingSystemType.Vernacular).Id);
			}

			ws = writingSystems.SingleOrDefault(w => w.IsDefaultAnalysis);
			return (ws != null ? ws.Id :
				writingSystems.First(w => w.Type == FwDBUtils.FwWritingSystemType.Analysis).Id);
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

			ws = null;

			if (mapping.Field.FwWsType == FwDBUtils.FwWritingSystemType.Analysis)
				ws = writingSystems.SingleOrDefault(w => w.IsDefaultAnalysis);
			else if (mapping.Field.FwWsType == FwDBUtils.FwWritingSystemType.Vernacular)
				ws = writingSystems.SingleOrDefault(w => w.IsDefaultVernacular);

			mapping.FwWsId = (ws != null ? ws.Id : null);
		}

		/// ------------------------------------------------------------------------------------
		public static bool IsPhoneticMapped(IEnumerable<FieldMapping> mappings, bool showIfNotMapped)
		{
			var mapped = mappings.Any(m => m.Field != null && m.Field.Type == FieldType.Phonetic);

			if (!mapped && showIfNotMapped)
				Utils.MsgBox(App.LocalizeString("NoPhoneticMappingsMsg", "You must specify a mapping for the phonetic field."));

			return mapped;
		}
	}
}
