// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SIL.Reporting;
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
		public static IPASymbolCache IPASymbolCache { get; private set; }

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
				ErrorReport.ReportFatalException(exception);
			}

			IPASymbolCache = new IPASymbolCache();
			IPASymbolCache.LoadFromList(ih.IPASymbols);
			ih.IPASymbols = null;

			return ih;
		}
	}
}