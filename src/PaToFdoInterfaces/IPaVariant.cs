
using System.Collections.Generic;

namespace SIL.PaToFdoInterfaces
{
	public interface IPaVariant
	{
		/// ------------------------------------------------------------------------------------
		IPaMultiString VariantForm { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaCmPossibility> VariantType { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString VariantComment { get; }
	}

	public interface IPaVariantOfInfo
	{
		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaCmPossibility> VariantType { get; }

		/// ------------------------------------------------------------------------------------
		IPaMultiString VariantComment { get; }
	}
}
