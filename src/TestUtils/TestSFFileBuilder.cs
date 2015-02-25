// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.IO;

namespace SIL.Pa.TestUtils
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
