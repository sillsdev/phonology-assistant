using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
{
	public class PaSettingsHandler : SettingsHandler
	{
		private const string kMarkerMapNode = kRootNodeName + "/markermap";
		private const string kPhoneticSortOptNode = kRootNodeName + "/phoneticsortoptions";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new settings file handler to manage application settings in the
		/// specified file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaSettingsHandler(string settingsFile) : base(settingsFile)
		{
		}

		#region PhoneticSortOptions Related Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an integer value from the settings file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int GetIntPhoneticSortValue(string name, string property, int defaultValue)
		{
			return GetIntValue(kPhoneticSortOptNode, name, property, defaultValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a boolean value from the settings file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool GetBoolPhoneticSortValue(string name, string property, bool defaultValue)
		{
			return GetBoolValue(kPhoneticSortOptNode, name, property, defaultValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a string value from the settings file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetStringPhoneticSortValue(string name, string property, string defaultValue)
		{
			return GetStringValue(kPhoneticSortOptNode, name, property, defaultValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves a value to the settings file for the specified window.
		/// </summary>
		/// <param name="windowName">Name of window.</param>
		/// <param name="property">Name of the item being saved (this is used for the
		/// attribute name in the XML node). For example: "splitter1Location"</param>
		/// <param name="value">value being saved.</param>
		/// ------------------------------------------------------------------------------------
		public void SavePhoneticSortValue(string name, string property, object value)
		{
			SaveValue(kPhoneticSortOptNode, "sortoption", name, property, value);
		}

		#endregion
	}
}
