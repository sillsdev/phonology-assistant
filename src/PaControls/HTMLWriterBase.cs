using System;
using System.IO;
using System.Collections.Generic;
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
		protected XmlDocument m_xmlDoc;
		protected XmlNode m_currNode;
		private string m_htmlOutputFile;
		protected string m_tmpXMLFile;
		protected string m_xslFileBase;
		protected bool m_error = false;

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
				STUtils.STMsgBox(
					string.Format(Properties.Resources.kstidHTMLExportFileMissingMsg,
					m_xslFileBase), MessageBoxButtons.OK);
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
			string tmpXSLFile = AddFontInfoToXSL();

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
					File.Copy(m_tmpXMLFile, xmlFile);
				}
				catch { }
			}
			catch (Exception e)
			{
				// Of course, you know we should never get here. :o)
				STUtils.STMsgBox(e.Message, MessageBoxButtons.OK);
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
		/// Modifies the xslt file to include the field's font names and sizes in the  
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string AddFontInfoToXSL()
		{
			// Read the content of the entire XSL file.
			string xslContent = File.ReadAllText(m_xslFileBase);

			// Determine whether or not the file contains a an override for the font size.
			float altSize = 0;
			int i = xslContent.IndexOf("/*Alternate-Font-Size [");
			if (i >= 0)
			{
				int open = i + 23;
				int closed = xslContent.IndexOf("]", open);
				if (closed > open)
				{
					System.Globalization.CultureInfo ci =
						System.Globalization.CultureInfo.CreateSpecificCulture("en");
					STUtils.TryFloatParse(xslContent.Substring(open, closed - open), ci, out altSize);
				}
			}

			StringBuilder fontInfo = new StringBuilder();
			string replacementText = "/*Phonetic-Font-Settings-Go-Here*/";

			if (xslContent.IndexOf(replacementText) >= 0)
				GetPhoneticFontInfo(fontInfo, altSize);
			else
			{
				replacementText = "/*Font-Settings-Go-Here*/";

				// Write all the field's font information to the XSLT file.
				foreach (PaFieldInfo fieldInfo in PaApp.Project.FieldInfo)
				{
					if (fieldInfo.Font != null && fieldInfo.VisibleInGrid)
						AddFontInfoForField(fieldInfo, fontInfo, altSize);
				}
			}

			xslContent = xslContent.Replace(replacementText, fontInfo.ToString());
			string tmpXSLFile = Path.GetTempFileName();
			File.WriteAllText(tmpXSLFile, xslContent);
			return tmpXSLFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add the phonetic font info. to the specified string builder for output to the
		/// td.d class in the XSL's cascading style sheet.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetPhoneticFontInfo(StringBuilder fontInfo, float fontSize)
		{
			// If an alternate size was found in the XSL file, then assume it's in 'em'
			// units. Otherwise, use the phonetic field's font size in points.
			string units = (fontSize == 0 ? "pt" : "em");

			if (fontSize == 0)
				fontSize = PaApp.Project.FieldInfo.PhoneticField.Font.SizeInPoints;

			string className = "\t\t\ttd.d ";
			
			fontInfo.Append(className);
			fontInfo.AppendFormat("{{font-family: \"{0}\";}}\r\n",
				PaApp.Project.FieldInfo.PhoneticField.Font.Name);
			
			fontInfo.Append(className);
			fontInfo.AppendFormat("{{font-size: {0}{1};}}\r\n", fontSize, units);

			if (PaApp.Project.FieldInfo.PhoneticField.Font.Bold)
			{
				fontInfo.Append(className);
				fontInfo.Append("{{font-weight: bold;}}\r\n");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add the font info. to the specified string builder for the specified string
		/// builder for output to the td.d.x (where 'x' is a field name) class of the
		/// XSL's cascading style sheet.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddFontInfoForField(PaFieldInfo fieldInfo, StringBuilder fontInfo,
			float fontSize)
		{
			// If an alternate size was found in the XSL file, then assume it's in 'em'
			// units. Otherwise, use the PaFieldInfo's font size in points.
			string units = (fontSize == 0 ? "pt" : "em");

			if (fontSize == 0)
				fontSize = fieldInfo.Font.SizeInPoints;

			string className = string.Format("\t\t\t\ttd.d.{0} ", fieldInfo.FieldName);
			fontInfo.Append(className);
			fontInfo.AppendFormat("{{font-family: \"{0}\";}}\r\n", fieldInfo.Font.Name);
			fontInfo.Append(className);
			fontInfo.AppendFormat("{{font-size: {0}{1};}}\r\n", fontSize, units);

			if (fieldInfo.Font.Bold)
			{
				fontInfo.Append(className);
				fontInfo.Append("{{font-weight: bold;}}\r\n");
			}

			// Add special fields for the phonetic's before, after
			// and target fields for search result word lists.
			if (fieldInfo.IsPhonetic)
			{
				fontInfo.AppendFormat("\t\t\t\ttd.d.phbefore {{font-family: \"{0}\";}}\r\n",
					fieldInfo.Font.Name);

				fontInfo.AppendFormat("\t\t\t\ttd.d.phbefore {{font-size: {0}{1};}}\r\n",
					fontSize, units);

				fontInfo.AppendFormat("\t\t\t\ttd.d.phtarget {{font-family: \"{0}\";}}\r\n",
					fieldInfo.Font.Name);

				fontInfo.AppendFormat("\t\t\t\ttd.d.phtarget {{font-size: {0}{1};}}\r\n",
					fontSize, units);

				fontInfo.AppendFormat("\t\t\t\ttd.d.phafter {{font-family: \"{0}\";}}\r\n",
					fieldInfo.Font.Name);

				fontInfo.AppendFormat("\t\t\t\ttd.d.phafter {{font-size: {0}{1};}}\r\n",
					fontSize, units);
			}
		}
	}
}