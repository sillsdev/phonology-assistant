using System.Xml.Serialization;

namespace SIL.Pa.DataSource.FieldWorks
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Serialized with an FwDataSourceInfo class. This class maps a writing system to a
	/// FieldWorks field.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("FieldWsInfo")]
	public class FwFieldWsMapping
	{
		[XmlAttribute]
		public string FieldName { get; set; }
	
		[XmlAttribute("Ws")]
		public int WsHvo { get; set; }

		[XmlAttribute("WsName")]
		public string WsName { get; set; }

		/// ------------------------------------------------------------------------------------
		public FwFieldWsMapping()
		{
		}

		/// ------------------------------------------------------------------------------------
		public FwFieldWsMapping(string fieldname, int hvo)
		{
			FieldName = fieldname;
			WsHvo = hvo;
		}

		/// ------------------------------------------------------------------------------------
		public FwFieldWsMapping(string fieldname, FwWritingSysInfo fwWsInfo)
			: this(fieldname, (fwWsInfo != null ? fwWsInfo.WsHvo : 0))
		{
			if (fwWsInfo != null)
				WsName = fwWsInfo.WsName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes a deep copy of the FwDataSourceWsInfo object and returns it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwFieldWsMapping Copy()
		{
			return new FwFieldWsMapping(FieldName, WsHvo) { WsName = WsName };
		}
	}
}
