
namespace SIL.Pa.DataSource.FieldWorks
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a single item in the writing system drop-downs in the grid.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FwWritingSysInfo
	{
		public string WsName { get; set; }
		public int WsHvo { get; set; }
		public FwDBUtils.FwWritingSystemType WsType { get; set; }
		public string DefaultFontName { get; set; }
		public bool IsDefaultVernacular { get; set; }
		public bool IsDefaultAnalysis { get; set; }

		/// ------------------------------------------------------------------------------------
		public FwWritingSysInfo(FwDBUtils.FwWritingSystemType wsType, int wsHvo,
			string wsName)
		{
			WsType = wsType;
			WsHvo = wsHvo;
			WsName = wsName;
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return WsName;
		}
	}
}
