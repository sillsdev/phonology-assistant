// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
// File: ProjectCssBuilder.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using SIL.Pa.Model;
using SIL.Pa.Properties;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	public class ProjectCssBuilder : ExporterBase
	{
		/// ------------------------------------------------------------------------------------
		public static bool Process(PaProject project)
		{
			if (File.Exists(project.CssFileName))
				return true;

			var builder = new ProjectCssBuilder(project);
			return builder.InternalProcess(Settings.Default.KeepTempProjectCssXhtmlFile,
				Pipeline.ProcessType.ExportToCss);
		}

		/// ------------------------------------------------------------------------------------
		protected ProjectCssBuilder(PaProject project) : base(project)
		{
			m_outputFileName = project.CssFileName;
		}

		/// ------------------------------------------------------------------------------------
		protected override string Title
		{
			get { return "CSS"; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void WriteMetadata()
		{
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "div", "id", "metadata");
			WriteMetadataFormatting();
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<KeyValuePair<string, Font>> GetFormattingFieldInfo()
		{
			return m_project.Fields.Select(field =>
				new KeyValuePair<string, Font>(field.DisplayName, field.Font));
		}

		/// ------------------------------------------------------------------------------------
		protected override void WriteTable()
		{
		}
	}
}
