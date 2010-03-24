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
using SilUtils;
using System.Windows.Forms;

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
		private static string s_filePath;

		internal static AFeatureCache AFeatureCache { get; set; }
		internal static BFeatureCache BFeatureCache { get; set; }
		internal static IPASymbolCache IPASymbolCache { get; set; }

		[XmlArray("articulatoryFeatures"), XmlArrayItem("feature")]
		public List<Feature> AFeatures { get; set; }

		[XmlArray("binaryFeatures"), XmlArrayItem("feature")]
		public List<Feature> BFeatures { get; set; }

		[XmlArray("symbols"), XmlArrayItem("symbol")]
		public List<IPASymbol> IPASymbols { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the IPA symbol and features inventory assuming its location is in the
		/// same folder as the running application.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Load()
		{
			string path = Path.GetDirectoryName(Application.ExecutablePath);
			path = Path.Combine(path, kDefaultInventoryFileName);
			Load(path);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the specified IPA symbol and features inventory file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Load(string filePath)
		{
			s_filePath = filePath;
			ReLoad();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reloads the IPA symbol and features inventory file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ReLoad()
		{
			InventoryHelper ir = DeserializeInventory();

			IPASymbolCache = new IPASymbolCache();
			IPASymbolCache.LoadFromList(ir.IPASymbols);

			AFeatureCache = new AFeatureCache();
			AFeatureCache.LoadFromList(ir.AFeatures);

			BFeatureCache = new BFeatureCache();
			BFeatureCache.LoadFromList(ir.BFeatures);

			ir.IPASymbols = null;
			ir.AFeatures = null;
			ir.BFeatures = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializes the inventory file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static InventoryHelper DeserializeInventory()
		{
			if (!File.Exists(s_filePath))
			{
				throw new FileNotFoundException("IPA symbol and feature inventory file '" +
					s_filePath + "' not found.");
			}

			var ir = Utils.DeserializeData(s_filePath, typeof(InventoryHelper)) as InventoryHelper;
			if (ir == null)
			{
				throw new Exception("Error reading IPA symbol and feature inventory file '" +
					s_filePath + "'");
			}

			return ir;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the IPA symbol and feature inventory to the file that was previously.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool Save()
		{
			return Save(s_filePath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the IPA symbol and feature inventory to the specified file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool Save(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
				return false;

			var ir = new InventoryHelper();
			ir.IPASymbols = IPASymbolCache.Values.ToList();
			ir.AFeatures = AFeatureCache.Values.ToList();
			ir.BFeatures = BFeatureCache.Values.ToList();
			
			Utils.SerializeData(s_filePath, ir);

			ir.IPASymbols = null;
			ir.AFeatures = null;
			ir.BFeatures = null;

			return true;
		}
	}
}

