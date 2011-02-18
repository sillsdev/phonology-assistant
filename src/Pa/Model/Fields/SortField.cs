using System.Xml.Serialization;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// The SortInformation struct holds sort information.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SortField
	{
		private string m_paFieldName;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is mainly used for serializing and deserializing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("name")]
		public string FieldName
		{
			get { return (Field != null ? Field.Name : m_paFieldName); }
			set { m_paFieldName = value; }
		}

		[XmlAttribute("ascending")]
		public bool Ascending { get; set; }

		[XmlIgnore]
		public PaField Field { get; set; }

		/// ------------------------------------------------------------------------------------
		public SortField()
		{
		}

		/// ------------------------------------------------------------------------------------
		public SortField(PaField field, bool sortDirection)
		{
			Field = field;
			Ascending = sortDirection;
		}

		/// ------------------------------------------------------------------------------------
		public SortField Copy()
		{
			return new SortField
			{
				m_paFieldName = m_paFieldName,
				Ascending = Ascending,
				Field = Field,		// REVIEW: I don't think we want a deep copy of the field, do we?
			};
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return FieldName + ": " + (Ascending ? "Ascending" : "Descending");
		}
	}
}
