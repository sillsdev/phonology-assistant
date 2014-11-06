using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using SIL.PaToFdoInterfaces;

namespace SIL.FieldWorks.PaObjects
{
	/// ----------------------------------------------------------------------------------------
	public class PaLexSense : IPaLexSense
	{
		/// ------------------------------------------------------------------------------------
		public PaLexSense()
		{
		}

		/// ------------------------------------------------------------------------------------
		internal PaLexSense(dynamic lxSense, List<PaCustomField> customFields, dynamic svcloc)
		{
            xAnthropologyNote = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "AnthroNote"), svcloc);
			xBibliography = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "Bibliography"), svcloc);
			xDefinition = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "Definition"), svcloc);
			xDiscourseNote = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "DiscourseNote"), svcloc);
			xEncyclopedicInfo = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "EncyclopedicInfo"), svcloc);
			xGeneralNote = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "GeneralNote"), svcloc);
			xGloss = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "Gloss"), svcloc);
			xGrammarNote = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "GrammarNote"), svcloc);
			xPhonologyNote = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "PhonologyNote"), svcloc);
			xRestrictions = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "Restrictions"), svcloc);
			xSemanticsNote = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "SemanticsNote"), svcloc);
            xSociolinguisticsNote = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "SocioLinguisticsNote"), svcloc);

            xReversalEntries = new List<PaMultiString>();
            dynamic reversals = SilTools.ReflectionHelper.GetProperty(lxSense, "ReversalEntriesRC");
            foreach (var x in reversals)
                xReversalEntries.Add(PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(x, "ReversalForm"), svcloc));

			xGuid = SilTools.ReflectionHelper.GetProperty(lxSense, "Guid");

			ImportResidue = PaLexicalInfo.GetTsStringText(lxSense, "ImportResidue");
			Source = PaLexicalInfo.GetTsStringText(lxSense, "Source");
			ScientificName = PaLexicalInfo.GetTsStringText(lxSense, "ScientificName");

            xAnthroCodes = new List<PaCmPossibility>();
            dynamic anthroCodes = SilTools.ReflectionHelper.GetProperty(lxSense, "AnthroCodesRC");
            foreach (var x in anthroCodes)
                xAnthroCodes.Add(PaCmPossibility.Create(x, svcloc));

            xDomainTypes = new List<PaCmPossibility>();
            dynamic domainTypes = SilTools.ReflectionHelper.GetProperty(lxSense, "DomainTypesRC");
            foreach (var x in domainTypes)
                xDomainTypes.Add(PaCmPossibility.Create(x, svcloc));

            xUsages = new List<PaCmPossibility>();
            dynamic usages = SilTools.ReflectionHelper.GetProperty(lxSense, "UsageTypesRC");
            foreach (var x in usages)
                xUsages.Add(PaCmPossibility.Create(x, svcloc));

            xSemanticDomains = new List<PaCmPossibility>();
            dynamic semanticDomains = SilTools.ReflectionHelper.GetProperty(lxSense, "SemanticDomainsRC");
            foreach (var x in semanticDomains)
                xSemanticDomains.Add(PaCmPossibility.Create(x, svcloc));

            xStatus = PaCmPossibility.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "StatusRA"), svcloc);
            xSenseType = PaCmPossibility.Create(SilTools.ReflectionHelper.GetProperty(lxSense, "SenseTypeRA"), svcloc);

            dynamic poss = null;
            var msa = SilTools.ReflectionHelper.GetProperty(lxSense, "MorphoSyntaxAnalysisRA");
            Type msaType = msa.GetType();
            foreach (var interfaceType in msaType.GetInterfaces())
            {
                if (interfaceType.Name.EndsWith("IMoDerivAffMsa"))
                {
                    poss = SilTools.ReflectionHelper.GetProperty(msa, "FromPartOfSpeechRA");
                    break;
                }
                if (interfaceType.Name.EndsWith("IMoDerivStepMsa"))
                {
                    poss = SilTools.ReflectionHelper.GetProperty(msa, "PartOfSpeechRA");
                    break;
                }
                if (interfaceType.Name.EndsWith("IMoStemMsa"))
                {
                    poss = SilTools.ReflectionHelper.GetProperty(msa, "PartOfSpeechRA");
                    break;
                }
                if (interfaceType.Name.EndsWith("IMoUnclassifiedAffixMsa"))
                {
                    poss = SilTools.ReflectionHelper.GetProperty(msa, "PartOfSpeechRA");
                    break;
                }
            }

            if (poss != null)
                xPartOfSpeech = PaCmPossibility.Create(poss, svcloc);

            xCustomFields = PaCustomFieldValue.GetCustomFields(lxSense, "LexSense", customFields, svcloc);

        }

		#region IPaLexSense Members
		/// ------------------------------------------------------------------------------------
		public List<PaCmPossibility> xAnthroCodes { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaCmPossibility> AnthroCodes
		{
			get { return xAnthroCodes.Select(x => (IPaCmPossibility)x); }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xAnthropologyNote { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString AnthropologyNote
		{
			get { return xAnthropologyNote; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xBibliography { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString Bibliography
		{
			get { return xBibliography; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xDefinition { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString Definition
		{
			get { return xDefinition; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xDiscourseNote { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString DiscourseNote
		{
			get { return xDiscourseNote; }
		}

		/// ------------------------------------------------------------------------------------
		public List<PaCmPossibility> xDomainTypes { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaCmPossibility> DomainTypes
		{
			get { return xDomainTypes.Select(x => (IPaCmPossibility)x); }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xEncyclopedicInfo { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString EncyclopedicInfo
		{
			get { return xEncyclopedicInfo; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xGeneralNote { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString GeneralNote
		{
			get { return xGeneralNote; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xGloss { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString Gloss
		{
			get { return xGloss; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xGrammarNote { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString GrammarNote
		{
			get { return xGrammarNote; }
		}

		/// ------------------------------------------------------------------------------------
		public string ImportResidue { get; set; }

		/// ------------------------------------------------------------------------------------
		public PaCmPossibility xPartOfSpeech { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaCmPossibility PartOfSpeech
		{
			get { return xPartOfSpeech; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xPhonologyNote { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString PhonologyNote
		{
			get { return xPhonologyNote; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xRestrictions { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString Restrictions
		{
			get { return xRestrictions; }
		}

		/// ------------------------------------------------------------------------------------
		public string ScientificName { get; set; }

		/// ------------------------------------------------------------------------------------
		public List<PaCmPossibility> xSemanticDomains { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaCmPossibility> SemanticDomains
		{
			get { return xSemanticDomains.Select(x => (IPaCmPossibility)x); }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xSemanticsNote { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString SemanticsNote
		{
			get { return xSemanticsNote; }
		}

		/// ------------------------------------------------------------------------------------
		public PaCmPossibility xSenseType { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaCmPossibility SenseType
		{
			get { return xSenseType; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xSociolinguisticsNote { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString SociolinguisticsNote
		{
			get { return xSociolinguisticsNote; }
		}

		/// ------------------------------------------------------------------------------------
		public string Source { get; set; }

		/// ------------------------------------------------------------------------------------
		public PaCmPossibility xStatus { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaCmPossibility Status
		{
			get { return xStatus; }
		}

		/// ------------------------------------------------------------------------------------
		public List<PaCmPossibility> xUsages { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaCmPossibility> Usages
		{
			get { return xUsages.Select(x => (IPaCmPossibility)x); }
		}

		/// ------------------------------------------------------------------------------------
		public List<PaMultiString> xReversalEntries { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaMultiString> ReversalEntries
		{
			get { return xReversalEntries.Select(x => (IPaMultiString)x); }
		}

		/// ------------------------------------------------------------------------------------
		public Guid xGuid { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Guid Guid
		{
			get { return xGuid; }
		}

        /// ------------------------------------------------------------------------------------
        public List<PaCustomFieldValue> xCustomFields { get; set; }

        /// ------------------------------------------------------------------------------------
        [XmlIgnore]
        public IEnumerable<IPaCustomFieldValue> CustomFields
        {
            get { return xCustomFields; }
        }
        #endregion
	}
}
