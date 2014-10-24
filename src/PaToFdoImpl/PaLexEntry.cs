using System;
using System.Collections.Generic;
using System.Linq;
using SIL.PaToFdoInterfaces;
using System.Xml.Serialization;
using SIL.Utils;

namespace SIL.FieldWorks.PaObjects
{
	/// ----------------------------------------------------------------------------------------
	public class PaLexEntry : IPaLexEntry
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads all the lexical entries from the specified service locator into a collection
		/// of PaLexEntry objects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static List<IPaLexEntry> GetAll(IEnumerable<dynamic> allInstances, dynamic svcLoc)
		{
            List<IPaLexEntry> result = new List<IPaLexEntry>();
            foreach (var lx in allInstances)
            {
                dynamic lexForm = SilTools.ReflectionHelper.GetProperty(lx, "LexemeFormOA");
                if (lexForm == null)
                    continue;
                dynamic form = SilTools.ReflectionHelper.GetProperty(lexForm, "Form");
                if (form.Count > 0)
                    result.Add(new PaLexEntry(lx, svcLoc));
            }
            return result;
		}

		/// ------------------------------------------------------------------------------------
		public PaLexEntry()
		{
		}

		/// ------------------------------------------------------------------------------------
        internal PaLexEntry(dynamic lxEntry, dynamic svcloc)
		{
			DateCreated = SilTools.ReflectionHelper.GetProperty(lxEntry, "DateCreated");
			DateModified = SilTools.ReflectionHelper.GetProperty(lxEntry, "DateModified");
			//ExcludeAsHeadword = lxEntry.ExcludeAsHeadword; remove
			ExcludeAsHeadword = false; // MDL: remove when IPaLexEntry is updated
			// ShowMainEntryIn = lxEntry.ShowMainEntryIn.Select(x => new PaLexShowMainEntryIn(x)).ToList(); // MDL: uncomment when IPaLexEntry is updated

            ImportResidue = PaLexicalInfo.GetTsStringText(lxEntry, "ImportResidue");

            xPronunciations = new List<PaLexPronunciation>();
            dynamic pronunications = SilTools.ReflectionHelper.GetProperty(lxEntry, "PronunciationsOS");
            foreach (var x in pronunications)
                xPronunciations.Add(new PaLexPronunciation(x, svcloc));

            xSenses = new List<PaLexSense>();
            dynamic senses = SilTools.ReflectionHelper.GetProperty(lxEntry, "SensesOS");
            foreach (var x in senses)
                xSenses.Add(new PaLexSense(x, svcloc));

            xComplexForms = new List<PaMultiString>();
            dynamic complexEntries = SilTools.ReflectionHelper.GetProperty(lxEntry, "ComplexFormEntries");
            foreach (var x in complexEntries)
            {
                dynamic complexForm = SilTools.ReflectionHelper.GetProperty(x, "LexemeFormOA");
                xComplexForms.Add(PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(complexForm, "Form"), svcloc));
            }

            xAllomorphs = new List<PaMultiString>();
            dynamic allomorphs = SilTools.ReflectionHelper.GetProperty(lxEntry, "AllAllomorphs");
            foreach (var x in allomorphs)
                xAllomorphs.Add(PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(x, "Form"), svcloc));

            dynamic lexForm = SilTools.ReflectionHelper.GetProperty(lxEntry, "LexemeFormOA");
            xLexemeForm = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lexForm, "Form"), svcloc);
            xMorphType = PaCmPossibility.Create(SilTools.ReflectionHelper.GetProperty(lxEntry, "PrimaryMorphType"), svcloc);
			xCitationForm = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxEntry, "CitationForm"), svcloc);
			xNote = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxEntry, "Comment"), svcloc);
			xLiteralMeaning = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxEntry, "LiteralMeaning"), svcloc);
			xBibliography = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxEntry, "Bibliography"), svcloc);
			xRestrictions = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxEntry, "Restrictions"), svcloc);
			xSummaryDefinition = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(lxEntry, "SummaryDefinition"), svcloc);

            xVariantOfInfo = new List<PaVariantOfInfo>();
            dynamic variantRefs = SilTools.ReflectionHelper.GetProperty(lxEntry, "VariantEntryRefs");
            foreach (var x in variantRefs)
                xVariantOfInfo.Add(new PaVariantOfInfo(x, svcloc));

            xVariants = new List<PaVariant>();
            dynamic variants = SilTools.ReflectionHelper.GetProperty(lxEntry, "VariantFormEntryBackRefs");
            foreach (var x in variants)
                xVariants.Add(new PaVariant(x, svcloc));
            
			xGuid = SilTools.ReflectionHelper.GetProperty(lxEntry, "Guid");

            dynamic etymology = SilTools.ReflectionHelper.GetProperty(lxEntry, "EtymologyOA");
            if (etymology != null)
				xEtymology = PaMultiString.Create(SilTools.ReflectionHelper.GetProperty(etymology, "Form"), svcloc);

            // TODO: Handle complex forms
            //xComplexFormInfo = new List<PaComplexFormInfo>();
            //foreach (var eref in lxEntry.EntryRefsOS)
            //{
            //    var pcfi = PaComplexFormInfo.Create(eref, svcloc);
            //    if (pcfi != null)
            //        xComplexFormInfo.Add(pcfi);
            //}
		}

		#region IPaLexEntry implementation
		/// ------------------------------------------------------------------------------------
		/// MDL: Replace this with ShowMainEntryIn it behaves the same as ExcludeAsHeadword,
		/// but on a publicaton by publication basis including Main Dictionary and "$$all_entries$$".
		public bool ExcludeAsHeadword { get; set; }

		// ------------------------------------------------------------------------------------
		// Until you delete the comment markers, don't use three slashes
		// unless you want dreaded Error CS1591.
		//public List<PaLexShowMainEntryIn> xShowMainEntryIn { get; set; }  /// MDL: uncomment when added to IPaLexEntry
		//[XmlIgnore]
		// ------------------------------------------------------------------------------------
		// public IEnumerable<IPaLexShowMainEntryIn> xShowMainEntryIn /// MDL: uncomment when added to IPaLexEntry
		//{
		//	get { return xShowMainEntryIn.Cast<IPaLexPronunciation>(); }
		//}

		/// ------------------------------------------------------------------------------------
		public string ImportResidue { get; set; }

		/// ------------------------------------------------------------------------------------
		public DateTime DateCreated { get; set; }

		/// ------------------------------------------------------------------------------------
		public DateTime DateModified { get; set; }

		/// ------------------------------------------------------------------------------------
		public PaMultiString xLexemeForm { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString LexemeForm
		{
			get { return xLexemeForm; }
		}

		/// ------------------------------------------------------------------------------------
		public PaCmPossibility xMorphType { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaCmPossibility MorphType
		{
			get { return xMorphType; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xCitationForm { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString CitationForm
		{
			get { return xCitationForm; }
		}

		/// ------------------------------------------------------------------------------------
		public List<PaVariant> xVariants { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaVariant> Variants
		{
			get { return xVariants.Cast<IPaVariant>(); }
		}

		/// ------------------------------------------------------------------------------------
		public List<PaVariantOfInfo> xVariantOfInfo { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaVariantOfInfo> VariantOfInfo
		{
			get { return xVariantOfInfo.Cast<IPaVariantOfInfo>(); }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xSummaryDefinition { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString SummaryDefinition
		{
			get { return xSummaryDefinition; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xEtymology { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString Etymology
		{
			get { return xEtymology; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xNote { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString Note
		{
			get { return xNote; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xLiteralMeaning { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString LiteralMeaning
		{
			get { return xLiteralMeaning; }
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
		public PaMultiString xRestrictions { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString Restrictions
		{
			get { return xRestrictions; }
		}

		/// ------------------------------------------------------------------------------------
		public List<PaLexPronunciation> xPronunciations { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaLexPronunciation> Pronunciations
		{
			get { return xPronunciations.Cast<IPaLexPronunciation>(); }
		}

		/// ------------------------------------------------------------------------------------
		public List<PaLexSense> xSenses { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaLexSense> Senses
		{
			get { return xSenses.Cast<IPaLexSense>(); }
		}

		/// ------------------------------------------------------------------------------------
		public List<PaMultiString> xComplexForms { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaMultiString> ComplexForms
		{
			get { return xComplexForms.Cast<IPaMultiString>(); }
		}

		/// ------------------------------------------------------------------------------------
		public List<PaComplexFormInfo> xComplexFormInfo { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaComplexFormInfo> ComplexFormInfo
		{
			get { return xComplexFormInfo.Cast<IPaComplexFormInfo>(); }
		}

		/// ------------------------------------------------------------------------------------
		public List<PaMultiString> xAllomorphs { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaMultiString> Allomorphs
		{
			get { return xAllomorphs.Cast<IPaMultiString>(); }
		}

		/// ------------------------------------------------------------------------------------
		public Guid xGuid { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Guid Guid
		{
			get { return xGuid; }
		}

		#endregion
	}
}
