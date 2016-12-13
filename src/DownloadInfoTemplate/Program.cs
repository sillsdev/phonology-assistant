// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2016, SIL International. All Rights Reserved.
// <copyright from='2016' to='2016' company='SIL International'>
//		Copyright (c) 2016, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: program.cs for Download Info substitution
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace DownloadInfoTemplate
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var substitutions = new Dictionary<string, string>();
			var files = new List<string>();

			foreach (var arg in args)
			{
				if (arg.StartsWith("-f") || arg.StartsWith("--file"))
				{
					var terms = arg.Split('=');
					substitutions["_FILE_"] = Path.GetFileName(terms[1]);
					files.Add(Path.Combine(Path.GetDirectoryName(terms[1]), Path.GetFileNameWithoutExtension(terms[1]) + ".download_info"));
				}
				else if (arg.StartsWith("-")) throw new UsageException("Option argument");
				if (arg.Contains("="))
				{
					var equalPosition = arg.IndexOf('=');
					substitutions[arg.Substring(0, equalPosition)] = arg.Substring(equalPosition + 1);
				}
				else
				{
					files.Add(arg);
				}
			}
			if (!substitutions.ContainsKey("_DATE_")) substitutions["_DATE_"] = DateTime.Today.ToString("o").Substring(0, 10);
			string fileName;
			if (files.Count == 0 || files.Count == 1 && files[0].Contains(".download_info"))
				if (substitutions.ContainsKey("_FILE_"))
				{
					fileName = substitutions["_FILE_"];
					// ReSharper disable once AssignNullToNotNullAttribute
					files.Add(Path.Combine(Path.GetFileNameWithoutExtension(fileName), ".download_info"));
				}
			if (files.Count == 0 || files.Count > 2) throw new UsageException("Wrong number of file arguments");
			if (files.Count == 1)
			{
				files.Insert(0, "template.download_info");
			}
			if (!substitutions.ContainsKey("_FILE_"))
			{
				var folder = Path.GetDirectoryName(files[1]);
				var targetName = Path.GetFileNameWithoutExtension(files[1]);
				// ReSharper disable once AssignNullToNotNullAttribute
				foreach (var fileInfo in new DirectoryInfo(folder).GetFiles())
				{
					if (fileInfo.Name.ToLower().Contains(".download_info")) continue;
					// ReSharper disable once AssignNullToNotNullAttribute
					if (!fileInfo.Name.StartsWith(targetName)) continue;
					substitutions["_FILE_"] = fileInfo.Name;
					break;
				}
			}
			fileName = substitutions["_FILE_"];
			if (!substitutions.ContainsKey("_NAME_")) substitutions["_NAME_"] = GetName(fileName);
			if (!substitutions.ContainsKey("_VERSION_")) substitutions["_VERSION_"] = Regex.Match(fileName, @"\d+\.\d+(\.\d+)?").Value;
			if (fileName.Contains("BTE") && !substitutions.ContainsKey("_EDITION_")) substitutions["_EDITION_"] = "BTE";
			if (fileName.Contains("SE") && !substitutions.ContainsKey("_EDITION_")) substitutions["_EDITION_"] = "SE";
			if (fileName.Contains(".msi") && !substitutions.ContainsKey("_PLATFORM_")) substitutions["_PLATFORM_"] = "win";
			if (fileName.Contains(".exe") && !substitutions.ContainsKey("_PLATFORM_")) substitutions["_PLATFORM_"] = "win";
			if (fileName.Contains(".deb") && !substitutions.ContainsKey("_PLATFORM_")) substitutions["_PLATFORM_"] = "linux";
			if (fileName.Contains("amd64") && !substitutions.ContainsKey("_ARCHITECTURE_")) substitutions["_ARCHITECTURE_"] = "x84_64";
			if (fileName.Contains("i386") && !substitutions.ContainsKey("_ARCHITECTURE_")) substitutions["_ARCHITECTURE_"] = "x84_32";
			if (!substitutions.ContainsKey("_STABILITY_")) substitutions["_STABILITY_"] = fileName.ToLower().Contains("test") ? "testing" : "stable";
			if (!substitutions.ContainsKey("_NATURE_")) substitutions["_NATURE_"] = GetNature(fileName);
			// ReSharper disable once AssignNullToNotNullAttribute
			if (!substitutions.ContainsKey("_MD5_")) substitutions["_MD5_"] = GetMd5(Path.Combine(Path.GetDirectoryName(files[1]), fileName));
			if (!substitutions.ContainsKey("_TYPE_")) substitutions["_TYPE_"] = Path.GetExtension(fileName).Substring(1).ToUpper(CultureInfo.InvariantCulture);
			var templateFile = new StreamReader(files[0]);
			var template = templateFile.ReadToEnd();
			templateFile.Close();
			foreach (var substitutionsKey in substitutions.Keys)
			{
				template = template.Replace(substitutionsKey, substitutions[substitutionsKey]);
			}
			var missingPattern = new Regex(@"_[A-Z0-9-]*_");
			template = missingPattern.Replace(template, "");
			var outputFile = new StreamWriter(files[1]);
			outputFile.Write(template);
			outputFile.Close();
		}

		private static string GetName(string fileName)
		{
			// ReSharper disable once PossibleNullReferenceException
			fileName = Path.GetFileNameWithoutExtension(fileName).Replace("-", " ").Replace("_"," ").Replace(" amd64"," 64-bit").Replace(" i386"," 32-bit").Replace(" all","").Replace("Test","").Replace("Setup","").Replace("BTE"," Translation Edition").Replace("SE"," Standard Edition").Replace("  ", " ");
			fileName = Regex.Replace(fileName, @"\s*\d+\.\d+(\.\d+)?(\.\d+)?", "");
			var xelatexPattern = new Regex("xelatex", RegexOptions.IgnoreCase);
			fileName = xelatexPattern.Replace(fileName, "Xelatex");
			fileName = Regex.Replace(fileName, "([a-z])([A-Z])", "$1 $2");
			fileName = xelatexPattern.Replace(fileName, "XeLaTeX");
			fileName = fileName.Substring(0, 1).ToUpper() + fileName.Substring(1);
			return fileName;
		}

		private static string GetNature(string fileName)
		{
			var val = fileName.ToLower();
			if (val.Contains("xelatex"))
			{
				return "option";
			}
			if (val.Contains(".exe"))
			{
				return "installer";
			}
			if (val.Contains("Test"))
			{
				return "test";
			}
			if (val.Contains(".deb"))
			{
				return "installer";
			}
			return "support";
		}

		public static string GetMd5(string filename)
		{
			using (var md5 = MD5.Create())
			{
				using (var stream = File.OpenRead(filename))
				{
					return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-","");
				}
			}
		}
	}

	internal class UsageException : Exception
	{
		public UsageException(string message)
		{
			Console.WriteLine(message);
			Console.WriteLine("Usage: DownloadInfoTemplate [match=value]* templateFullPath (downloadInfoFullPath | (-f|--file)=inputfullpath");
			Console.WriteLine("\twhere match is one of the names between underscores in the template file");
			Console.WriteLine("Example:\nDownloadInfoTemplate _COMPATIBLE_=\"Fw <= 8.3.x\" ..\\template.download_info -f=..\\output\\Pathway-1.15.4.exe");
		}
	}
}
