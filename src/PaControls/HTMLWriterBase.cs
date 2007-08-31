using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using SIL.Pa.Resources;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Provides a base class for exporting grids and vowel consonant charts to html.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class HTMLWriterBase
	{
		private const string kXSLPhoneticFontInfoMarker = "Phonetic-Font-Name-Goes-Here";

		protected XmlDocument m_xmlDoc;
		protected XmlNode m_currNode;
		protected Font m_groupHeadingFont = null;
		protected string m_tmpXMLFile;
		protected string m_xslFileBase;
		protected bool m_error = false;
		private readonly string m_htmlOutputFile;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create an instance of the writer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public HTMLWriterBase(string defaultHTMLFileName, string[] rootAttribValues)
		{
			// Make sure there are no invalid characters in the file name.
			foreach (char invalidChar in Path.GetInvalidFileNameChars())
				defaultHTMLFileName = defaultHTMLFileName.Replace(invalidChar, '-');

			m_error = !VerifyXslFile();

			if (m_error)
				return;

			CreateTempXMLFile(rootAttribValues);
			m_htmlOutputFile = GetHTMLOutputFileName(defaultHTMLFileName);
			m_error = string.IsNullOrEmpty(m_htmlOutputFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the file to which the HTML was output.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected string HtmlOutputFile
		{
			get { return m_htmlOutputFile; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure the xsl file exists that's used to transform the xml.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool VerifyXslFile()
		{
			m_xslFileBase = Path.Combine(Application.StartupPath, XSLFileName);

			if (!File.Exists(m_xslFileBase))
			{
				string filePath = STUtils.PrepFilePathForSTMsgBox(m_xslFileBase);
				STUtils.STMsgBox(
					string.Format(Properties.Resources.kstidHTMLExportFileMissingMsg,
					filePath));
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CreateTempXMLFile(string[] rootAttribValues)
		{
			m_xmlDoc = new XmlDocument();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = "\t";
			settings.CloseOutput = true;
			m_tmpXMLFile = Path.GetTempFileName();
			XmlWriter writer = XmlWriter.Create(m_tmpXMLFile, settings);

			WriteOuterElements(writer, string.IsNullOrEmpty(PaApp.Project.Language) ?
				"Unknown" : PaApp.Project.Language, rootAttribValues);
	
			writer.Flush();
			writer.Close();
			m_xmlDoc.Load(m_tmpXMLFile);

			// Move the current node to the deepest child node.
			m_currNode = m_xmlDoc.DocumentElement;
			while (m_currNode.FirstChild != null)
				m_currNode = m_currNode.FirstChild;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteOuterElements(XmlWriter writer, string languageName,
			string[] rootAttribValues)
		{
			throw new Exception("Derived classes must override WriteOuterElements().");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string XSLFileName
		{
			get {throw new Exception("Derived classes must override XSLFileName.");}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the save file dialog, asking the user for the html file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected string GetHTMLOutputFileName(string defaultHTMLFileName)
		{
			defaultHTMLFileName = defaultHTMLFileName.Replace(" ", string.Empty);

			string fileTypes = ResourceHelper.GetString("kstidFileTypeHTML") + "|" +
				ResourceHelper.GetString("kstidFileTypeAllFiles");

			int filterIndex = 0;
			return PaApp.SaveFileDialog("html", fileTypes, ref filterIndex,
				ResourceHelper.GetString("kstidSaveFileDialogGenericCaption"),
				defaultHTMLFileName, PaApp.Project.ProjectPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates the HTML file with it's accompanying cascading style sheet.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void WriteHTMLFile()
		{
			// Save the XML output to a temporary file.
			m_xmlDoc.Save(m_tmpXMLFile);
		
			// Get a temporary XSL file that is modified to contain font information.
			string tmpXSLFile = InternalModifyCSS();

			try
			{
				// Load the XSL file.
				XslCompiledTransform xslt = new XslCompiledTransform();
				xslt.Load(tmpXSLFile);

				// Execute the transform and output the results to the specified HTML file.
				xslt.Transform(m_tmpXMLFile, m_htmlOutputFile);

				try
				{
					// Copy the temporary XML -- used to generate the html file -- into the
					// same folder, with the same name (but with an xml extension), as the
					// html file. This gives power users a chance to use the XML file for
					// whatever catches their fancy.
					string xmlFile = Path.GetFileNameWithoutExtension(m_htmlOutputFile);
					xmlFile += (m_htmlOutputFile.ToLower().EndsWith(".xml") ? "1.xml" : ".xml");
					xmlFile = Path.Combine(Path.GetDirectoryName(m_htmlOutputFile), xmlFile);
					File.Copy(m_tmpXMLFile, xmlFile, true);
				}
				catch { }
			}
			catch (Exception e)
			{
				// Of course, you know we should never get here. :o)
				STUtils.STMsgBox(e.Message);
			}
			finally
			{
				// Delete the XML file now that the html has been created.
				try { File.Delete(m_tmpXMLFile); }
				catch { }
				try { File.Delete(tmpXSLFile); }
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a temporary XSL file after modifications have been made to it's CSS.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string InternalModifyCSS()
		{
			// Read the content of the entire XSL file.
			string xslContent = File.ReadAllText(m_xslFileBase);

			// Modify it.
			xslContent = ModifyCSS(xslContent);
			
			// Write it all to a temporary file and return the path to the file.
			string tmpXSLFile = Path.GetTempFileName();
			File.WriteAllText(tmpXSLFile, xslContent);
			return tmpXSLFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Modifies the CSS portion of the xslt file to include the field's font names and
		/// sizes and other class specific settings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string ModifyCSS(string xslContent)
		{
			// Determine whether or not the file contains an override for the font size.
			float altSize = 0;
			int i = xslContent.IndexOf("/*Alternate-Phonetic-Font-Size [");
			if (i >= 0)
			{
				int open = i + 32;
				int closed = xslContent.IndexOf("]", open);
				if (closed > open)
				{
					CultureInfo ci =
						CultureInfo.CreateSpecificCulture("en");
					STUtils.TryFloatParse(
						xslContent.Substring(open, closed - open), ci, out altSize);
				}
			}

			if (m_groupHeadingFont != null)
				xslContent = WriteFontInfoForGroupHeading(xslContent, altSize);

			xslContent = ((xslContent.IndexOf(kXSLPhoneticFontInfoMarker) >= 0) ?
				WriteFontInfoForPhonetic(xslContent, altSize) :
				WriteFieldInfoForGridColumns(xslContent, altSize));

			xslContent = xslContent.Replace("/*~~|", string.Empty);
			xslContent = xslContent.Replace("|~~*/", string.Empty);

			return xslContent;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add the font information to the XSL style sheet for the grid's group by field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string WriteFontInfoForGroupHeading(string xslContent, float fontSize)
		{
			// If an alternate size was found in the XSL file, then assume it's in 'em'
			// units. Otherwise, use the phonetic field's font size in points.
			string units = (fontSize == 0 ? "pt" : "em");

			if (fontSize == 0)
				fontSize = m_groupHeadingFont.SizeInPoints;

			string replacementText = string.Format("font-family: \"{0}\";", m_groupHeadingFont.Name);
			xslContent = xslContent.Replace("Group-Head-Font-Name-Goes-Here", replacementText);

			replacementText = string.Format("font-size: {0}{1};", fontSize, units);
			return xslContent.Replace("Group-Head-Font-Size-Goes-Here", replacementText);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add the phonetic font info. to the XSL style sheet.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string WriteFontInfoForPhonetic(string xslContent, float fontSize)
		{
			// If an alternate size was found in the XSL file, then assume it's in 'em'
			// units. Otherwise, use the phonetic field's font size in points.
			string units = (fontSize == 0 ? "pt" : "em");

			if (fontSize == 0)
				fontSize = PaApp.Project.FieldInfo.PhoneticField.Font.SizeInPoints;

			string replacementText = string.Format("font-family: \"{0}\";",
				PaApp.Project.FieldInfo.PhoneticField.Font.Name);

			xslContent = xslContent.Replace(kXSLPhoneticFontInfoMarker, replacementText);

			replacementText = string.Format("font-size: {0}{1};", fontSize, units);
			xslContent = xslContent.Replace("Phonetic-Font-Size-Goes-Here", replacementText);

			if (PaApp.Project.FieldInfo.PhoneticField.Font.Bold)
			{
				xslContent = xslContent.Replace("/*--|", string.Empty);
				xslContent = xslContent.Replace("|--*/", string.Empty);
			}

			return xslContent;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes the font information to the xsl file for all the fields visible in a
		/// word list grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string WriteFieldInfoForGridColumns(string xslContent, float fontSize)
		{
			string bdrWidth = GetCSSSettingForDefaultCell(xslContent, "border-width");
			string bdrColor = GetCSSSettingForDefaultCell(xslContent, "border-color");
			string padding = GetCSSSettingForDefaultCell(xslContent, "padding");

			StringBuilder info = new StringBuilder();
			string replacementText = "/*Field-Settings-Go-Here*/";

			// Write all the field's font information to the XSLT file.
			foreach (PaFieldInfo fieldInfo in PaApp.Project.FieldInfo)
			{
				if (fieldInfo.Font != null && fieldInfo.VisibleInGrid)
				{
					WriteFieldInfoForSingleColumn(fieldInfo, info, fontSize,
						bdrWidth, bdrColor, padding);
				}
			}

			return xslContent.Replace(replacementText, info.ToString());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches for the specified setting for the default table cell in the body of text
		/// specified by xslContent.
		/// </summary>
		/// <remarks>
		/// For example, if xslContent contains the following two
		/// settings for border-width and color:
		/// 
		/// td.d {border-width: 1px;}
		/// td.d {border-color: rgb(153, 153, 153);}
		/// 
		/// Then the call to this method
		///	
		///		GetCSSSettingForDefaultCell(xslContent, "border-width")
		///
		/// would return "1px" and the call
		/// 
		///		GetCSSSettingForDefaultCell(xslContent, "border-color")
		/// 
		/// would return "rgb(153, 153, 153)"
		/// 
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		private string GetCSSSettingForDefaultCell(string xslContent, string setting)
		{
			if (string.IsNullOrEmpty(setting))
				return null;

			string[] findVariations = new string[] { "td.d {" + setting, "td.d{" + setting };

			foreach (string find in findVariations)
			{
				int start = xslContent.IndexOf(find);
				if (start >= 0)
				{
					start += find.Length;
					int end = xslContent.IndexOf(';', start);
					if (end > start)
					{
						string match = xslContent.Substring(start, end - start);
						match = match.Replace(":", string.Empty);
						return match.Trim();
					}
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add the font info. to the specified string builder for the specified string
		/// builder for output to the td.d.x (where 'x' is a field name) class of the
		/// XSL's cascading style sheet.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteFieldInfoForSingleColumn(PaFieldInfo fieldInfo, StringBuilder info,
			float fontSize, string bdrWidth, string bdrColor, string padding)
		{
			// If an alternate size was found in the XSL file, then assume it's in 'em'
			// units. Otherwise, use the PaFieldInfo's font size in points.
			string units = (fontSize == 0 ? "pt" : "em");

			if (fontSize == 0)
				fontSize = fieldInfo.Font.SizeInPoints;

			string className = string.Format("\t\t\t\ttd.{0} ", fieldInfo.FieldName);
			info.Append(className);
			info.AppendFormat("{{font-family: \"{0}\";}}\r\n", fieldInfo.Font.Name);
			info.Append(className);
			info.AppendFormat("{{font-size: {0}{1};}}\r\n", fontSize, units);

			if (!string.IsNullOrEmpty(bdrWidth))
			{
				info.Append(className);
				info.AppendFormat("{{border-width: {0};}}\r\n", bdrWidth);
			}

			if (!string.IsNullOrEmpty(bdrColor))
			{
				info.Append(className);
				info.AppendFormat("{{border-color: {0};}}\r\n", bdrColor);
			}

			if (!string.IsNullOrEmpty(padding))
			{
				info.Append(className);
				info.AppendFormat("{{padding: {0};}}\r\n", padding);
			}

			if (fieldInfo.Font.Bold)
			{
				info.Append(className);
				info.Append("{font-weight: bold;}\r\n");
			}

			if (fieldInfo.RightToLeft)
			{
				info.Append(className);
				info.Append("{text-align: right;}\r\n");
			}

			// Add special fields for the phonetic's before, after
			// and target fields for search result word lists.
			if (fieldInfo.IsPhonetic)
			{
				info.AppendFormat("\t\t\t\ttd.phbefore {{font-family: \"{0}\";}}\r\n",
					fieldInfo.Font.Name);

				info.AppendFormat("\t\t\t\ttd.phbefore {{font-size: {0}{1};}}\r\n",
					fontSize, units);

				info.AppendFormat("\t\t\t\ttd.phtarget {{font-family: \"{0}\";}}\r\n",
					fieldInfo.Font.Name);

				info.AppendFormat("\t\t\t\ttd.phtarget {{font-size: {0}{1};}}\r\n",
					fontSize, units);

				info.AppendFormat("\t\t\t\ttd.phafter {{font-family: \"{0}\";}}\r\n",
					fieldInfo.Font.Name);

				info.AppendFormat("\t\t\t\ttd.phafter {{font-size: {0}{1};}}\r\n",
					fontSize, units);
			}
		}
	}
}
