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
using System.Text;
using System.Xml.Serialization;
using Palaso.IO;
using SilTools;

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
		private static List<NormalizationException> s_exceptionsList;
		private static bool s_loadAttempted;

		/// ------------------------------------------------------------------------------------
		private static void Load()
		{
			s_loadAttempted = true;

			try
			{
				var filename = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName,
					"NormalizationExceptions.xml");

				s_exceptionsList = XmlSerializationHelper.DeserializeFromFile<List<NormalizationException>>(filename);
			}
			catch {	}
		}

		/// ------------------------------------------------------------------------------------
		public static string Normalize(string toConvert)
		{
			if (toConvert == null)
				return null;

			// First normalize and decompose the string.
			toConvert = toConvert.Normalize(NormalizationForm.FormD);

			// Check the exceptions list needs to be built.
			if (s_exceptionsList == null)
			{
				if (s_loadAttempted)
					return toConvert;

				Load();
			}

			// If there's an excptions list, then "recompose" those items in the exceptions list.
			if (s_exceptionsList != null)
			{
				toConvert = s_exceptionsList.Aggregate(toConvert, (curr, err) =>
					curr.Replace(err.DecomposedForm, err.ComposedForm));
			}

			return toConvert;
		}
	}
}
