using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using SIL.PaToFdoInterfaces;

namespace SIL.FieldWorks.PaObjects
{
	/// ----------------------------------------------------------------------------------------
	public class PaVariantOfInfo : IPaVariantOfInfo
	{
		/// ------------------------------------------------------------------------------------
		public PaVariantOfInfo()
		{
		}

		/// ------------------------------------------------------------------------------------
		internal PaVariantOfInfo(dynamic lxEntryRef)
		{
			xVariantComment = PaMultiString.Create(lxEntryRef.Summary, lxEntryRef.Cache.ServiceLocator);
            xVariantType = new List<PaCmPossibility>();
            foreach (var x in lxEntryRef.VariantEntryTypesRS)
                xVariantType.Add(PaCmPossibility.Create(x));
		}

		/// ------------------------------------------------------------------------------------
		public List<PaCmPossibility> xVariantType { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaCmPossibility> VariantType
		{
			get { return xVariantType.Cast<IPaCmPossibility>(); }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xVariantComment { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString VariantComment
		{
			get { return xVariantComment; }
		}
	}
}
