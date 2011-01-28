using System.Xml.Serialization;

namespace SIL.Pa.DataSourceClasses.FieldWorks
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Serialized with an FwDataSourceInfo class.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("FieldWsInfo")]
	public class FwDataSourceWsInfo
	{
		[XmlAttribute]
		public string FieldName { get; set; }
	
		[XmlAttribute]
		public int Ws { get; set; }

		[XmlAttribute]
		public string WsName { get; set; }

		/// ------------------------------------------------------------------------------------
		public FwDataSourceWsInfo()
		{
		}

		/// ------------------------------------------------------------------------------------
		public FwDataSourceWsInfo(string fieldname, int ws)
		{
			FieldName = fieldname;
			Ws = ws;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes a deep copy of the FwDataSourceWsInfo object and returns it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceWsInfo Clone()
		{
			FwDataSourceWsInfo clone = new FwDataSourceWsInfo();
			clone.FieldName = FieldName;
			clone.Ws = Ws;
			clone.WsName = WsName;
			return clone;
		}
	}
}
