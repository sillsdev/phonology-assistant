using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SilTools;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Provides reading and saving methods for the file that stores IPA symbols and
	/// articulatory and binary features.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("inventory")]
	public class InventoryHelper
	{
		public static IPASymbolCache IPASymbolCache { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Use only for serialization/deserialization
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("symbolDefinitions"), XmlArrayItem("symbolDefinition")]
		public List<IPASymbol> IPASymbols { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the specified phonetic inventory file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static InventoryHelper Load()
		{
			Exception exception;
			var ih = XmlSerializationHelper.DeserializeFromFile<InventoryHelper>(
				App.PhoneticInventoryFilePath, out exception);

			if (ih == null)
			{
				App.CloseSplashScreen();
				Palaso.Reporting.ErrorReport.ReportFatalException(exception);
			}

			IPASymbolCache = new IPASymbolCache();
			IPASymbolCache.LoadFromList(ih.IPASymbols);
			ih.IPASymbols = null;

			return ih;
		}
	}
}