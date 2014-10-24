using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using SIL.PaToFdoInterfaces;

namespace SIL.FieldWorks.PaObjects
{
	/// ----------------------------------------------------------------------------------------
	public class PaLexPronunciation : IPaLexPronunciation
	{
		/// ------------------------------------------------------------------------------------
		public PaLexPronunciation()
		{
		}

		/// ------------------------------------------------------------------------------------
		internal PaLexPronunciation(dynamic lxPro, dynamic svcloc)
		{
            dynamic lxProForm = SilTools.ReflectionHelper.GetProperty(lxPro, "Form");
            xForm = PaMultiString.Create(lxProForm, svcloc);
            dynamic lxProLoc = SilTools.ReflectionHelper.GetProperty(lxPro, "LocationRA");
            xLocation = PaCmPossibility.Create(lxProLoc, svcloc);
			CVPattern = PaLexicalInfo.GetTsStringText(lxPro, "CVPattern");
            Tone = PaLexicalInfo.GetTsStringText(lxPro, "Tone");
			xGuid = SilTools.ReflectionHelper.GetProperty(lxPro, "Guid");

            xMediaFiles = new List<PaMediaFile>();
            dynamic mediaFiles = SilTools.ReflectionHelper.GetProperty(lxPro, "MediaFilesOS");
            foreach (var x in mediaFiles)
                if (x != null)
                    xMediaFiles.Add(new PaMediaFile(x, svcloc));
		}

		/// ------------------------------------------------------------------------------------
		public string CVPattern { get; set; }

		/// ------------------------------------------------------------------------------------
		public string Tone { get; set; }

		/// ------------------------------------------------------------------------------------
		public PaMultiString xForm { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString Form
		{
			get { return xForm; }
		}

		/// ------------------------------------------------------------------------------------
		public List<PaMediaFile> xMediaFiles { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaMediaFile> MediaFiles
		{
			get { return xMediaFiles.Select(x => (IPaMediaFile)x); }
		}

		/// ------------------------------------------------------------------------------------
		public PaCmPossibility xLocation { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaCmPossibility Location
		{
			get { return xLocation; }
		}

		/// ------------------------------------------------------------------------------------
		public Guid xGuid { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Guid Guid
		{
			get { return xGuid; }
		}
	}
}
