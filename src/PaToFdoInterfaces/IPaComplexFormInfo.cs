using System.Collections.Generic;

namespace SIL.PaToFdoInterfaces
{
	/// ----------------------------------------------------------------------------------------
	public interface IPaComplexFormInfo
	{
		/// ------------------------------------------------------------------------------------
		List<string> Components { get; }
		
		/// ------------------------------------------------------------------------------------
		IPaMultiString ComplexFormComment { get; }

		/// ------------------------------------------------------------------------------------
		IEnumerable<IPaCmPossibility> ComplexFormType { get; }
	}
}
