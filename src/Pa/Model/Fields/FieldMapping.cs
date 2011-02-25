using System.Xml.Serialization;
using SIL.Pa.DataSource.FieldWorks;

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
		public string FwWritingSystem { get; set; }

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
				FwWritingSystem = FwWritingSystem,
				PaFieldName = PaFieldName,
				Field = (Field == null ? null : Field.Copy()),
			};
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return NameInDataSource ?? base.ToString();
		}
	}
}
