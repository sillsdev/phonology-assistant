using System.Collections.Generic;
using System.Linq;
using SIL.PaToFdoInterfaces;
using SIL.Utils;

namespace SIL.FieldWorks.PaObjects
{
	/// ----------------------------------------------------------------------------------------
	public class PaWritingSystem : IPaWritingSystem
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads all the writing systems from the specified service locator into a
		/// collection of PaWritingSystem objects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static List<IPaWritingSystem> GetWritingSystems(IEnumerable<dynamic> writingSystems, dynamic svcloc)
		{
			var wsList = new List<IPaWritingSystem>();

            dynamic wsRepository = SilTools.ReflectionHelper.GetProperty(svcloc, "WritingSystems");
            dynamic vernWss = SilTools.ReflectionHelper.GetProperty(wsRepository, "VernacularWritingSystems");
            dynamic analWss = SilTools.ReflectionHelper.GetProperty(wsRepository, "AnalysisWritingSystems");

            foreach (var ws in writingSystems)
			{
				if (!wsList.Any(w => w.Id == ws.Id))
				{
                    bool isVern = (vernWss.Contains(ws));
                    bool isAnal = (analWss.Contains(ws));
					wsList.Add(new PaWritingSystem(ws, svcloc, isVern, isAnal));
				}
			}

			return wsList;
		}

		/// ------------------------------------------------------------------------------------
		public PaWritingSystem()
		{
		}

		/// ------------------------------------------------------------------------------------
		private PaWritingSystem(dynamic lgws, dynamic svcloc, bool isVern,
			bool isAnal)
		{
			Id = lgws.Id;
			DisplayName = lgws.DisplayLabel;
			LanguageName = lgws.LanguageName;
			Abbreviation = lgws.Abbreviation;
			IcuLocale = lgws.IcuLocale;
			Hvo = lgws.Handle;
			DefaultFontName = lgws.DefaultFontName;
			IsVernacular = isVern;
            IsAnalysis = isAnal;
            dynamic wsRepository = SilTools.ReflectionHelper.GetProperty(svcloc, "WritingSystems");
            IsDefaultAnalysis = (lgws == SilTools.ReflectionHelper.GetProperty(wsRepository, "DefaultAnalysisWritingSystem"));
            IsDefaultVernacular = (lgws == SilTools.ReflectionHelper.GetProperty(wsRepository, "DefaultVernacularWritingSystem"));
            IsDefaultPronunciation = (lgws == SilTools.ReflectionHelper.GetProperty(wsRepository, "DefaultPronunciationWritingSystem"));
		}

		#region IPaWritingSystem Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the ISO-blah, blah code.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Id { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DisplayName { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the writing system language name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string LanguageName { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the writing system abbreviation.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Abbreviation { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the ICU locale.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string IcuLocale { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the hvo.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Hvo { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the writing system is a vernacular writing
		/// system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsVernacular { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the writing system is a analysis writing
		/// system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsAnalysis { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the writing system is the default
		/// analysis writing system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsDefaultAnalysis { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the writing system is the default
		/// pronunciation writing system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsDefaultPronunciation { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the writing system is the default
		/// vernacular writing system.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsDefaultVernacular { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the default font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DefaultFontName { get; set; }

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return DisplayName ?? (Id ?? string.Empty);
		}
	}
}
