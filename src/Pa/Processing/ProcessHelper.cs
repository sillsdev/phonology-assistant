// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
// File: ProcessHelper.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using L10NSharp;
using SIL.IO;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	public static class ProcessHelper
	{
		/// ------------------------------------------------------------------------------------
		public static string ProcessFile
		{
			get { return FileLocationUtilities.GetFileDistributedWithApplication(App.ProcessingFolderName, "Processing.xml"); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the settings file specifies a temporary folder (which is assumed to be relative
		/// to the current project's folder) then this method removes the file name from the
		/// specified defaultPath and builds a full path directing the file to the temp. folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string MakeTempFilePath(PaProject project, string defaultPath)
		{
			var path = defaultPath;

			if (!string.IsNullOrEmpty(Properties.Settings.Default.TempProcessingFilesFolder))
			{
				path = Path.Combine(project.Folder, Properties.Settings.Default.TempProcessingFilesFolder);

				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
				
				path = Path.Combine(path, Path.GetFileName(defaultPath));
			}

			return path;
		}

		/// ------------------------------------------------------------------------------------
		public static Pipeline CreatePipeline(Pipeline.ProcessType prsType)
		{
			var processingFolder = FileLocationUtilities.GetDirectoryDistributedWithApplication(App.ProcessingFolderName);
			return Pipeline.Create(prsType, ProcessFile, processingFolder);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Copies cascading stylesheets and java script files from the process folder to the
		/// default user folder (i.e. the folder PA suggests as the parent for projects).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void CopyFilesThatMakePrettyHTMLExports()
		{
			IfNecessaryUpgradeFilesThatMakePrettyHTMLExports("*.css");
			IfNecessaryUpgradeFilesThatMakePrettyHTMLExports("*.js");
		}

		/// ------------------------------------------------------------------------------------
		private static void IfNecessaryUpgradeFilesThatMakePrettyHTMLExports(string fileTypeExtension)
		{
			var processingFolder =
				FileLocationUtilities.GetDirectoryDistributedWithApplication(App.ProcessingFolderName);

			foreach (var filename in Directory.GetFiles(processingFolder, fileTypeExtension))
			{
				try
				{
					var dst = Path.Combine(App.ProjectFolder, Path.GetFileName(filename));

					if (File.Exists(dst))
					{
						var srcfi = new FileInfo(filename);
						var dstfi = new FileInfo(dst);

						if (srcfi.LastWriteTime != dstfi.LastWriteTime || srcfi.Length != dstfi.Length)
							File.Delete(dst);
					}

					if (!File.Exists(dst))
						File.Copy(filename, dst);
				}
				catch (Exception e)
				{
					var msg = LocalizationManager.GetString("Miscellaneous.Messages.UpgradingProcessFileErrorMsg",
						"There was an error copying the file '{0}' to '{1}'. Make sure the file is not in use by another program.");

					App.NotifyUserOfProblem(e, msg, filename, App.ProjectFolder);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This should only be done for debugging.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WriteStreamToFile(MemoryStream stream, string outputFileName)
		{
			WriteStreamToFile(stream, outputFileName, true);
		}

		/// ------------------------------------------------------------------------------------
		public static bool WriteStreamToFile(MemoryStream stream, string outputFileName, bool tidy)
		{
			return WriteStreamToFile(stream, outputFileName, false, tidy);
		}

		/// ------------------------------------------------------------------------------------
		public static bool WriteStreamToFile(MemoryStream stream, string outputFileName,
			bool showExceptions, bool tidy)
		{
			while (true)
			{
				try
				{
					using (var fileStream = new FileStream(outputFileName, FileMode.Create))
					{
						stream.WriteTo(fileStream);
						fileStream.Close();
						break;
					}
				}
				catch (Exception e)
				{
					if (showExceptions &&
						Utils.MsgBox(e.Message, MessageBoxButtons.RetryCancel) == DialogResult.Retry)
					{
						continue;
					}

					return false;
				}
			}

			if (tidy)
			{
				// This makes it all pretty, with proper indentation and line-breaking.
				var doc = new XmlDocument();
				doc.Load(outputFileName);
				doc.Save(outputFileName);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public static void WriteMetadata(XmlWriter writer, PaProject project, bool closeDiv)
		{
			writer.WriteStartElement("div");
			writer.WriteAttributeString("id", "metadata");

			// Open ul and div
			writer.WriteStartElement("ul");
			writer.WriteAttributeString("class", "settings");

			writer.WriteStartElement("li");
			writer.WriteAttributeString("class", "programConfigurationFolder");

			var path = FileLocationUtilities.GetDirectoryDistributedWithApplication(App.ConfigFolderName);
			writer.WriteString(TerminateFolderPath(path));
			writer.WriteEndElement();

			writer.WriteStartElement("li");
			writer.WriteAttributeString("class", "programPhoneticInventoryFile");
			writer.WriteString(App.kDefaultInventoryFileName);
			writer.WriteEndElement();

			writer.WriteStartElement("li");
			writer.WriteAttributeString("class", "programDistinctiveFeaturesName");
			writer.WriteString(project.DistinctiveFeatureSet);
			writer.WriteEndElement();

			writer.WriteStartElement("li");
			writer.WriteAttributeString("class", "userFolder");
			writer.WriteString(TerminateFolderPath(App.ProjectFolder));
			writer.WriteEndElement();

			writer.WriteStartElement("li");
			writer.WriteAttributeString("class", "projectFolder");
			writer.WriteString(TerminateFolderPath(project.Folder));
			writer.WriteEndElement();

			writer.WriteStartElement("li");
			writer.WriteAttributeString("class", "projectPhoneticInventoryFile");
			writer.WriteString(Path.GetFileName(project.ProjectInventoryFileName));
			writer.WriteEndElement();

			// Close ul
			writer.WriteEndElement();

			if (closeDiv)
				writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		public static void WriteFieldFormattingInfo(XmlWriter writer, string fieldName, Font fnt)
		{
			// Open table row
			writer.WriteStartElement("tr");

			WriteStartElementWithAttribAndValue(writer, "td", "class", "name", fieldName);
			WriteStartElementWithAttribAndValue(writer, "td", "class", "class", MakeAlphaNumeric(fieldName));
			WriteStartElementWithAttribAndValue(writer, "td", "class", "font-family", fnt.Name);
			WriteStartElementWithAttribAndValue(writer, "td", "class", "font-size", fnt.SizeInPoints.ToString());

			if (!fnt.Bold)
				WriteEmptyElement(writer, "td");
			else
				WriteStartElementWithAttribAndValue(writer, "td", "class", "font-weight", "bold");

			if (!fnt.Italic)
				WriteEmptyElement(writer, "td");
			else
				WriteStartElementWithAttribAndValue(writer, "td", "class", "font-style", "italic");

			// Close tr
			writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		public static void WriteColumnGroup(XmlWriter writer, int colsInGroup)
		{
			writer.WriteStartElement("colgroup");
			
			for (int i = 0; i < colsInGroup; i++)
				WriteEmptyElement(writer, "col");
			
			writer.WriteEndElement();
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all non alphanumeric characters (including spaces) from the specified text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string MakeAlphaNumeric(string text)
		{
			if (string.IsNullOrEmpty(text))
				return text;

			var bldr = new StringBuilder();
			foreach (char c in text)
			{
				if (char.IsLetterOrDigit(c))
					bldr.Append(c);
			}

			return bldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure the last character in the specified path is a directory separator
		/// character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string TerminateFolderPath(string path)
		{
			path = path.Trim();
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
				path += Path.DirectorySeparatorChar.ToString();

			return path;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the element.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WriteEmptyElement(XmlWriter writer, string elementName)
		{
			writer.WriteStartElement(elementName);
			writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Does not close the element.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WriteStartElementWithAttrib(XmlWriter writer, string elementName,
			string attribName, string attribValue)
		{
			writer.WriteStartElement(elementName);
			writer.WriteAttributeString(attribName, attribValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the element.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WriteStartElementWithAttribAndValue(XmlWriter writer,
			string elementName, string attribName, string attribValue, string elementValue)
		{
			WriteStartElementWithAttrib(writer, elementName, attribName, attribValue);
			writer.WriteString(elementValue);
			writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the element.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WriteStartElementWithAttribAndEmptyValue(XmlWriter writer,
			string elementName, string attribName, string attribValue)
		{
			WriteStartElementWithAttrib(writer, elementName, attribName, attribValue);
			writer.WriteEndElement();
		}
	}
}
