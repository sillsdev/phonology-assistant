// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
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
using SIL.Pa.Properties;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ProjectCssBuilder : ExporterBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool Process(PaProject project)
		{
			if (File.Exists(project.CssFileName))
				return true;

			var builder = new ProjectCssBuilder(project);
			return builder.InternalProcess(Settings.Default.KeepTempProjectCssXhtmlFile,
				Pipeline.ProcessType.ExportToCss);
		}

		///  ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected ProjectCssBuilder(PaProject project) : base(project)
		{
			m_outputFileName = project.CssFileName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string Title
		{
			get { return "CSS"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
