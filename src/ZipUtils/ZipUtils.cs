using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace ZipUtils
{
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void UncompressFilesInZip(string zipFileName, string outputPath)
		{
			FastZip zipper = new FastZip();
			zipper.RestoreDateTimeOnExtract = true;
			zipper.ExtractZip(zipFileName, outputPath, null);
		}
	}
}
