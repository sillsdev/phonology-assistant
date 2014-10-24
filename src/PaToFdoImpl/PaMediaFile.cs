using System.IO;
using System.Xml.Serialization;
using SIL.PaToFdoInterfaces;

namespace SIL.FieldWorks.PaObjects
{
	/// ----------------------------------------------------------------------------------------
	public class PaMediaFile : IPaMediaFile
	{
		/// ------------------------------------------------------------------------------------
		public PaMediaFile()
		{
		}

		/// ------------------------------------------------------------------------------------
		internal PaMediaFile(dynamic mediaFile, dynamic svcloc)
        {
            dynamic mediaFileRA = SilTools.ReflectionHelper.GetProperty(mediaFile, "MediaFileRA");
            OriginalPath = SilTools.ReflectionHelper.GetProperty(mediaFileRA, "OriginalPath");
            AbsoluteInternalPath = SilTools.ReflectionHelper.GetProperty(mediaFileRA, "AbsoluteInternalPath");
            InternalPath = SilTools.ReflectionHelper.GetProperty(mediaFileRA, "InternalPath");
            xLabel = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(mediaFile, "Label"), svcloc);
		}

		#region IPaMediaFile Members
		/// ------------------------------------------------------------------------------------
		public PaMultiString xLabel { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString Label
		{
			get { return xLabel; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the media absolute, internal file path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string AbsoluteInternalPath { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal media file path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string InternalPath { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the original media file path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string OriginalPath { get; set; }

		#endregion

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Path.GetFileName(AbsoluteInternalPath);
		}
	}
}
