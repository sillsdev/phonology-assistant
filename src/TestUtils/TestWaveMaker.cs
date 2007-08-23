using System.IO;
using System.Reflection;

namespace SIL.SpeechTools.TestUtils
{
	public class TestWaveMaker
	{
		private static string m_wavDir;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a test wave file from the resources. The wave file created is specified
		/// by waveFileNumber (i.e. 1 or 2);
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string MakeTestWave(int waveFileNumber)
		{
			// First thing is to extract the test wave file from the resources and write
			// it to the disk. The folder where it is written is the same folder where
			// this assembly's DLL is located.
			Assembly assembly = Assembly.GetExecutingAssembly();

			using (Stream stream = assembly.GetManifestResourceStream(
				string.Format("SIL.SpeechTools.TestUtils.Wave{0}.wav", waveFileNumber)))
			{
				// CodeBase prepends "file:/", which must be removed.
				m_wavDir = Path.GetDirectoryName(assembly.CodeBase).Substring(6);
				string wavFile = Path.Combine(m_wavDir, string.Format("!TestWave{0}.wav", waveFileNumber));

				// Read all file into a big byte buffer.
				byte[] buff = new byte[stream.Length];
				stream.Read(buff, 0, (int)stream.Length);
				stream.Close();
				File.WriteAllBytes(wavFile, buff);
				return wavFile;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the specified test wave file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Delete(int waveFileNumber)
		{
			try
			{
				string wavFile = Path.Combine(m_wavDir, string.Format("!TestWave{0}.wav", waveFileNumber));
				File.Delete(wavFile);
			}
			catch {}
		}
	}
}
