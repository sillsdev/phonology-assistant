using System.Xml.Serialization;
using SIL.PaToFdoInterfaces;

namespace SIL.FieldWorks.PaObjects
{
	/// ----------------------------------------------------------------------------------------
	public class PaCmPossibility : IPaCmPossibility
	{
		/// ------------------------------------------------------------------------------------
		public PaCmPossibility()
		{
		}

		/// ------------------------------------------------------------------------------------
		internal static PaCmPossibility Create(dynamic poss, dynamic svcloc)
		{
			return (poss == null ? null : new PaCmPossibility(poss, svcloc));
		}

		/// ------------------------------------------------------------------------------------
        private PaCmPossibility(dynamic poss, dynamic svcloc)
		{
			xAbbreviation = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(poss, "Abbreviation"), svcloc);
			xName = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(poss, "Name"), svcloc);
		}

		#region IPaCmPossibility Members
		/// ------------------------------------------------------------------------------------
		public PaMultiString xAbbreviation { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString Abbreviation
		{
			get { return xAbbreviation; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xName { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString Name
		{
			get { return xName; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return string.Format("{0} ({1})", Name, Abbreviation);
		}
	}
}
