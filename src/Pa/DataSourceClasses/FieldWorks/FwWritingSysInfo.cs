
namespace SIL.Pa.DataSource.FieldWorks
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a single item in the writing system drop-downs in the grid.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FwWritingSysInfo
	{
		public string Name { get; set; }
		public int Hvo { get; set; }
		public string Id { get; set; }
		public FwDBUtils.FwWritingSystemType Type { get; set; }
		public string DefaultFontName { get; set; }
		public bool IsDefaultVernacular { get; set; }
		public bool IsDefaultAnalysis { get; set; }

		/// ------------------------------------------------------------------------------------
		public FwWritingSysInfo(FwDBUtils.FwWritingSystemType type, int hvo, string name)
		{
			Type = type;
			Hvo = hvo;
			Name = name;
		}

		/// ------------------------------------------------------------------------------------
		public FwWritingSysInfo(FwDBUtils.FwWritingSystemType type, string id, string name)
		{
			Type = type;
			Id = id;
			Name = name;
		}

		/// ------------------------------------------------------------------------------------
		public FwWritingSysInfo Copy()
		{
			return new FwWritingSysInfo(Type, Id, Name)
			{
				DefaultFontName = DefaultFontName,
				IsDefaultAnalysis = IsDefaultAnalysis,
				IsDefaultVernacular = IsDefaultVernacular,
			};
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}
	}
}
