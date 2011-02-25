using System.Collections.Generic;
using System.Linq;
using SIL.Pa.Model;

namespace SIL.Pa.UI.Controls
{
	public class FieldFontsGrid : FieldMappingGridBase
	{
		/// ------------------------------------------------------------------------------------
		public FieldFontsGrid(IEnumerable<PaField> potentialFields)
		{
			m_potentialFields = potentialFields;
			m_mappings = potentialFields.Select(f => new FieldMapping { Field = f.Copy() }).ToList();
			
			LockTargetFieldColumn();
			ShowFontColumn(false);
			RowCount = m_mappings.Count;
		}
	}
}
