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
		internal PaVariantOfInfo(dynamic lxEntryRef, dynamic svcloc)
		{
            xVariantComment = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxEntryRef, "Summary"), svcloc);
            xVariantType = new List<PaCmPossibility>();
            dynamic variantTypes = SilTools.ReflectionHelper.GetProperty(lxEntryRef, "VariantEntryTypesRS");
            foreach (var x in variantTypes)
                xVariantType.Add(PaCmPossibility.Create(x, svcloc));
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
