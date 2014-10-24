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
		internal PaVariant(dynamic lxEntryRef, dynamic svcloc)
		{
            dynamic lxEntry = SilTools.ReflectionHelper.GetProperty(lxEntryRef, "OwningEntry");
            dynamic lxForm = SilTools.ReflectionHelper.GetProperty(lxEntry, "LexemeFormOA");
            xVariantForm = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxForm, "Form"), svcloc);
            xVariantInfo = new PaVariantOfInfo(lxEntryRef, svcloc);
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
