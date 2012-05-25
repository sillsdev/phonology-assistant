using System.Collections.Generic;
using System.IO;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ZipHelper
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of the names of the compressed files found in the specified zip file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string[] GetFilesInZip(string zipFileName)
		{
			List<string> files = new List<string>();

			ZipFile zip = new ZipFile(File.OpenRead(zipFileName));
			foreach (ZipEntry entry in zip)
				files.Add(entry.Name);

			return (files.Count == 0 ? null : files.ToArray());
		}

		/// ------------------------------------------------------------------------------------
		public static void UncompressFilesInZip(string zipFileName, string outputPath)
		{
			// FastZip has a bug here where on Linux, all files are extracted to one
			// directory but with filenames that include the folder name and a backslash,
			// e.g. a filename (not pathname) like "Sekpele 1\Sekpele 1.db".
			// Web search only came up with http://community.sharpdevelop.net/forums/p/9187/25577.aspx
			// The code below based on http://wiki.sharpdevelop.net/SharpZipLib-Zip-Samples.ashx
			// works around this bug.
			// Old method:  FastZip zipper = new FastZip(); zipper.RestoreDateTimeOnExtract = true; zipper.ExtractZip(zipFileName, outputPath, null);
			ZipFile zf = null;
			try {
				FileStream fs = File.OpenRead(zipFileName);
				zf = new ZipFile(fs);
				foreach (ZipEntry zipEntry in zf) {
					if (!zipEntry.IsFile)
						continue; // ignore directories
					string entryFileName = zipEntry.Name;
					string fullZipToPath = Path.Combine(outputPath, entryFileName);
					string directoryName = Path.GetDirectoryName(fullZipToPath);
					if (directoryName.Length > 0)
						Directory.CreateDirectory(directoryName);
					byte[] buffer = new byte[4096]; // 4K is optimum
					Stream zipStream = zf.GetInputStream(zipEntry);
					using (FileStream streamWriter = File.Create(fullZipToPath)) { // "using" closes stream upon exception
						StreamUtils.Copy(zipStream, streamWriter, buffer);
					}
					File.SetLastWriteTime(fullZipToPath,zipEntry.DateTime);
				}
			} finally {
				if (zf != null) {
					zf.IsStreamOwner = true; // makes Close() also shut the underlying stream
					zf.Close();
				}
			}
		}
	}
}
