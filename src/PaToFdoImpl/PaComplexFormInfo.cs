using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using SIL.PaToFdoInterfaces;

namespace SIL.FieldWorks.PaObjects
{
	/// ----------------------------------------------------------------------------------------
	public class PaComplexFormInfo : IPaComplexFormInfo
	{
		/// ------------------------------------------------------------------------------------
		public PaComplexFormInfo()
		{
			xComponents = new List<string>();
		}

		/// ------------------------------------------------------------------------------------
		internal static PaComplexFormInfo Create(dynamic lxEntryRef, dynamic svcloc)
		{
            if (!PaLexicalInfo.IsComplexForm(lxEntryRef))
				return null;

			var pcfi = new PaComplexFormInfo();
			pcfi.xComplexFormComment = PaMultiString.Create(lxEntryRef.Summary, lxEntryRef.Cache.ServiceLocator);
            pcfi.xComplexFormType = new List<PaCmPossibility>();
            foreach (var x in lxEntryRef.ComplexEntryTypesRS)
                pcfi.xComplexFormType.Add(PaCmPossibility.Create(x, svcloc));

			foreach (var component in lxEntryRef.ComponentLexemesRS)
			{
				if (PaLexicalInfo.IsLexEntry(component))
					pcfi.xComponents.Add(component.HeadWord.Text);
				else if (PaLexicalInfo.IsLexSense(component))
				{
                    var text = component.Entry.HeadWord.Text;
                    if (component.Entry.SensesOS.Count > 1)
                        text += string.Format(" {0}", component.IndexInOwner + 1);

					pcfi.xComponents.Add(text);
				}
			}

			return pcfi;
		}

		/// ------------------------------------------------------------------------------------
		public List<string> xComponents { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<string> Components
		{
			get { return xComponents; }
		}

		/// ------------------------------------------------------------------------------------
		public PaMultiString xComplexFormComment { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IPaMultiString ComplexFormComment
		{
			get { return xComplexFormComment; }
		}

		/// ------------------------------------------------------------------------------------
		public List<PaCmPossibility> xComplexFormType { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<IPaCmPossibility> ComplexFormType
		{
			get { return xComplexFormType.Cast<IPaCmPossibility>(); }
		}
	}
}
