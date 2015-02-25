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
using SIL.ObjectBrowser;

namespace SIL.Pa
{
	public class PaToFdoInspectorList : GenericInspectorObjectList
	{
		/// ------------------------------------------------------------------------------------
		private readonly List<string> _excludeList = new List<string>(new[]
		{
			"xName", "xAbbreviation", "xPronunciations", "xSenses", "xAllomorphs",
			"xLexemeForm", "xMorphType", "xCitationForm", "xNote", "xLiteralMeaning",
			"xBibliography", "xRestrictions", "xVariantComment", "xSummaryDefinition",
			"xVariant", "xVariantType", "xEtymology", "xForm", "xLocation", "xMediaFiles",
			"xAnthropologyNote", "xBibliography", "xDefinition", "xDiscourseNote",
			"xEncyclopedicInfoInfo", "xGeneralNote", "xGloss", "xGrammarNote", "xPhonologyNote",
			"xRestrictions", "xSemanticsNote", "xSociolinguisticsNote", "xAnthroCodes",
			"xDomainTypes", "xEncyclopedicInfo", "xUsages", "xSemanticDomains", "xStatus",
			"xSenseType", "xPartOfSpeech", "xLabel", "xComplexForms", "xSummary", "xOwningEntry",
			"xReferenceTypes", "xVariantType", "xVariantComment", "xVariantForm", "xComponents",
			"xComplexFormType", "xComplexForms", "xVariantOfInfo", "xVariants", "xVariantInfo",
			"xComplexFormInfo", "xComplexFormComment", "xReversalEntries", "xGuid",
		});

		/// ------------------------------------------------------------------------------------
		protected override List<IInspectorObject> GetInspectorObjects(object obj, int level)
		{
			var list = base.GetInspectorObjects(obj, level);
			return list.Where(x => !_excludeList.Contains(x.DisplayName)).ToList();
		}
	}
}
