// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright from='2009' to='2009' company='SIL International'>
//		Copyright (c) 2009, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: InventoryReader.cs
// Responsibility: Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Palaso.IO;
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
		public const string kDefaultInventoryFileName = "PhoneticInventory.xml";

		public static AFeatureCache AFeatureCache { get; set; }
		public static BFeatureCache BFeatureCache { get; set; }
		public static IPASymbolCache IPASymbolCache { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Use only for serialization/deserialization
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("articulatoryFeatures"), XmlArrayItem("feature")]
		public List<Feature> AFeatures { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Use only for serialization/deserialization
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("binaryFeatures"), XmlArrayItem("feature")]
		public List<Feature> BFeatures { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Use only for serialization/deserialization
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("symbols"), XmlArrayItem("symbol")]
		public List<IPASymbol> IPASymbols { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the specified phonetic inventory file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static InventoryHelper Load()
		{
			var ih = DeserializeInventory();

			IPASymbolCache = new IPASymbolCache();
			IPASymbolCache.LoadFromList(ih.IPASymbols);

			AFeatureCache = new AFeatureCache();
			AFeatureCache.LoadFromList(ih.AFeatures);

			BFeatureCache = new BFeatureCache();
			BFeatureCache.LoadFromList(ih.BFeatures);

			ih.IPASymbols = null;
			ih.AFeatures = null;
			ih.BFeatures = null;

			return ih;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializes the inventory file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static InventoryHelper DeserializeInventory()
		{
			var filePath = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName,
				kDefaultInventoryFileName);

			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("Phonetic inventory file '" +
					filePath + "' not found.");
			}

			var ih = XmlSerializationHelper.DeserializeFromFile<InventoryHelper>(filePath);
			if (ih == null)
				throw new Exception(string.Format("Error reading phonetic inventory file '{0}'", filePath));

			return ih;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the IPA symbol and feature inventory to the specified file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Save()
		{
			var filePath = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName,
				kDefaultInventoryFileName);

			if (string.IsNullOrEmpty(filePath))
				return false;

			var ih = new InventoryHelper();
			ih.IPASymbols = IPASymbolCache.Values.ToList();
			ih.AFeatures = AFeatureCache.Values.ToList();
			ih.BFeatures = BFeatureCache.Values.ToList();

			XmlSerializationHelper.SerializeToFile(filePath, this);

			ih.IPASymbols = null;
			ih.AFeatures = null;
			ih.BFeatures = null;

			return true;
		}
	}
}