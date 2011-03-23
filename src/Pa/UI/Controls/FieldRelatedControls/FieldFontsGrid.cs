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
			m_potentialFields = potentialFields.OrderBy(f => f.DisplayName);
			m_mappings = m_potentialFields.Select(f => new FieldMapping { Field = f }).ToList();
			
			LockTargetFieldColumn();
			ShowFontColumn(false);
			RowCount = m_mappings.Count;
		}
	}
}
