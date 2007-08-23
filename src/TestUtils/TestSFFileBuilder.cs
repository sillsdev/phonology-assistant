using System.IO;

namespace SIL.SpeechTools.TestUtils
{
	/// <summary>
	/// Summary description for TestSFFileBuilder.
	/// </summary>
	public class TestSFFileBuilder
	{
		public StreamWriter Writer;
		public string Filename;

		#region Construction/Destructor
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TestSFFileBuilder()
		{
			Filename = Path.GetTempFileName();
			Writer = new StreamWriter(Filename);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		~TestSFFileBuilder()
		{
			try
			{
				Writer.Close();
			}
			catch {}

			try
			{
				File.Delete(Filename);
			}
			catch {}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="marker"></param>
		/// <param name="data"></param>
		/// ------------------------------------------------------------------------------------
		public void AddLine(string marker, string data)
		{
			if (marker != null && marker != string.Empty)
				Writer.Write("\\" + marker.Trim() + " ");

			Writer.WriteLine(data.Trim());
		}
	}
}
