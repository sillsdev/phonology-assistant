using System.Xml.Serialization;
using SIL.Pa.DataSource.FieldWorks;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	[XmlType("mapping")]
	public class FieldMapping
	{
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
		[XmlElement("isParsed")]
		public bool IsParsed { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("isInterlinear")]
		public bool IsInterlinear { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("fwWritingSystemType")]
		public FwDBUtils.FwWritingSystemType FwWritingSystemType { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("field")]
		public PaField Field { get; set; }

		/// ------------------------------------------------------------------------------------
		public FieldMapping Copy()
		{
			return new FieldMapping
			{
				NameInDataSource = NameInDataSource,
				IsParsed = IsParsed,
				IsInterlinear = IsInterlinear,
				FwWritingSystemType = FwWritingSystemType,
				Field = (Field == null ? Field : Field.Copy()),
			};
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return NameInDataSource ?? base.ToString();
		}
	}
}
