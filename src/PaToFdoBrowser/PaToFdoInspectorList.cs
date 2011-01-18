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
			"xName", "xAbbreviation", "xPronunciations", "xSenses", "xAlomorphs",
			"xLexemeForm", "xMorphType", "xCitationForm", "xNote", "xLiteralMeaning",
			"xBibliography", "xRestrictions", "xVariantComment", "xSummaryDefinition",
			"xVariant", "xVariantType", "xEtymology", "xForm", "xLocation", "xMediaFiles",
			"xAnthropologyNote", "xBibliography", "xDefinition", "xDiscourseNote",
			"xEncyclopedicInfoInfo", "xGeneralNote", "xGloss", "xGrammarNote", "xPhonologyNote",
			"xRestrictions", "xSemanticsNote", "xSociolinguisticsNote", "xAnthroCodes",
			"xDomainTypes", "xEncyclopedicInfo", "xUsages", "xSemanticDomains", "xStatus",
			"xSenseType", "xPartOfSpeech", "xLabel"
		});

		/// ------------------------------------------------------------------------------------
		protected override List<IInspectorObject> GetInspectorObjects(object obj, int level)
		{
			var list = base.GetInspectorObjects(obj, level);
			return list.Where(x => !_excludeList.Contains(x.DisplayName)).ToList();
		}
	}
}
