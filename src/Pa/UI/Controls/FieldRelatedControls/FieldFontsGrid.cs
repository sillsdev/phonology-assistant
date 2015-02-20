// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
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
			ShowFontColumn(true);
			RowCount = m_mappings.Count;
		}
	}
}
