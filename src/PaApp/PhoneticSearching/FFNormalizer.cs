using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using SilUtils;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Ecapsulates an object containing normalized, decomposed strings that need to be
	/// converted back to their unnormalized form.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class NormalizationException
	{
		[XmlAttribute]
		public string DecomposedForm;
		[XmlAttribute]
		public string ComposedForm;
	}
	
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Ecapsulates a method for normalizing a string and recoposing exceptions specified
	/// in an XML file.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public static class FFNormalizer
	{
		private const string kstidNormalizationExceptionsFile = "NormalizationExceptions.xml";
		private static List<NormalizationException> s_exceptionsList = null;
		private static bool s_loadAttempted = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void Load()
		{
			s_loadAttempted = true;

			string filename = Path.Combine(Application.StartupPath, kstidNormalizationExceptionsFile);
			if (File.Exists(filename))
			{
				s_exceptionsList = Utils.DeserializeData(filename,
					typeof(List<NormalizationException>)) as List<NormalizationException>;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string Normalize(string toConvert)
		{
			if (toConvert == null)
				return toConvert;

			// First normalize and decompose the string.
			toConvert = toConvert.Normalize(NormalizationForm.FormD);

			// Check the exceptions list needs to be built.
			if (s_exceptionsList == null)
			{
				if (s_loadAttempted)
					return toConvert;

				Load();
			}

			// If there's an excptions list, then "recompose" those
			// items in the exceptions list.
			if (s_exceptionsList != null)
			{
				foreach (NormalizationException normException in s_exceptionsList)
				{
					toConvert = toConvert.Replace(normException.DecomposedForm,
						normException.ComposedForm);
				}
			}

			return toConvert;
		}
	}
}
