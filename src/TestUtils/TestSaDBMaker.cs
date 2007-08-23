using System;
using System.IO;
using System.Reflection;

namespace SIL.SpeechTools.TestUtils
{
	/// <summary>
	/// Summary description for TestDBMaker.
	/// </summary>
	public class TestSaDBMaker
	{
		private static TestSaDBMaker m_testSaDBMaker = new TestSaDBMaker();
		private static string m_dbPath = null;
		private static string m_dbDir = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the temp. DB is deleted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		~TestSaDBMaker()
		{
			DeleteDB();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string MakeEmptyTestDB()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream stream = assembly.GetManifestResourceStream(
				"SIL.SpeechTools.TestUtils.EmptyTestSaDB.mdb");

			// CodeBase prepends "file:/", which must be removed.
			m_dbDir = Path.GetDirectoryName(assembly.CodeBase).Substring(6);
			m_dbPath = Path.Combine(m_dbDir, "!emptytestsadb-" + DateTime.Now.Millisecond.ToString("X5") + ".mdb");
			DeleteDB();

			byte[] buff = new byte[stream.Length];
			stream.Read(buff, 0, (int)stream.Length);
			stream.Close();

			FileStream fs = new FileStream(m_dbPath, FileMode.CreateNew);
			BinaryWriter bw = new BinaryWriter(fs);
			bw.Write(buff);
			bw.Close();
			fs.Close();

			return m_dbPath;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool DeleteDB()
		{
			GC.Collect();
			
			string[] files = Directory.GetFiles(m_dbDir, "!emptytestsadb-*.?db");

			foreach (string file in files)
			{
				try
				{
					File.Delete(file);
				}
				catch { }
			}

			return true;
		}
	}
}
