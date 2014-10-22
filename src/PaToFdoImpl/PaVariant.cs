using System.Collections.Generic;
using System.Xml.Serialization;
using SIL.PaToFdoInterfaces;

namespace SIL.FieldWorks.PaObjects
{
	/// ----------------------------------------------------------------------------------------
	public class PaVariant : IPaVariant
	{
		/// ------------------------------------------------------------------------------------
		public PaVariant()
		{
		}

		/// ------------------------------------------------------------------------------------
		internal PaVariant(dynamic lxEntryRef)
		{
            var lx = PaLexicalInfo.GetLexEntryRefOwner(lxEntryRef);
			xVariantForm = PaMultiString.Create(lx.LexemeFormOA.Form, lxEntryRef.Cache.ServiceLocator);
			xVariantInfo = new PaVariantOfInfo(lxEntryRef);
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xVariantForm { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString VariantForm
		{
			get { return xVariantForm; }
		}

		/// ------------------------------------------------------------------------------------
		public PaVariantOfInfo xVariantInfo { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaCmPossibility> VariantType
		{
			get { return xVariantInfo.VariantType; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString VariantComment
		{
			get { return xVariantInfo.VariantComment; }
		}
	}
}
