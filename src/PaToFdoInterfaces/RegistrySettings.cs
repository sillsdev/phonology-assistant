// --------------------------------------------------------------------------------------------
// <copyright file="RegistryHelperLite.cs" from='2010' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>TE Team</author>
// Last reviewed:
//
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Microsoft.Win32;

namespace SIL.PaToFdoInterfaces
{
    public class RegistrySettings
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if a registry value exists.
		/// </summary>
		/// <param name="key">The base registry key of the key to check</param>
		/// <param name="subKey">Name of the group key, or string.Empty if there is no
		/// groupKeyName.</param>
		/// <param name="regEntry">The name of the registry entry.</param>
		/// <param name="value">[out] value of the registry entry if it exists; null otherwise.</param>
		/// <returns><c>true</c> if the registry entry exists, otherwise <c>false</c></returns>
		/// ------------------------------------------------------------------------------------
		public static bool RegEntryExists(RegistryKey key, string subKey, string regEntry, out object value)
		{
			value = null;

            if (key == null)
                return false;

            if (string.IsNullOrEmpty(subKey))
			{
				value = key.GetValue(regEntry);
				if (value != null)
					return true;
				return false;
			}

			if (!KeyExists(key, subKey))
				return false;

			using (RegistryKey regSubKey = key.OpenSubKey(subKey))
			{
				Debug.Assert(regSubKey != null, "Should have caught this in the KeyExists call above");
				if (Array.IndexOf(regSubKey.GetValueNames(), regEntry) >= 0)
				{
					value = regSubKey.GetValue(regEntry);
					return true;
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if a registry key exists.
		/// </summary>
		/// <param name="key">The base registry key of the key to check</param>
		/// <param name="subKey">The key to check</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static bool KeyExists(RegistryKey key, string subKey)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			foreach (string s in key.GetSubKeyNames())
			{
				if (String.Compare(s, subKey, true) == 0)
					return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the registry key for the current application's company from the local machine
		/// settings. This is 'HKLM\Software\{Application.CompanyName}'
		/// NOTE: This key is not opened for write access because it will fail on
		/// non-administrator logins.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static RegistryKey CompanyKeyLocalMachine
		{
			get
			{
				RegistryKey softwareKey = Registry.LocalMachine.OpenSubKey("Software");
				Debug.Assert(softwareKey != null);
				var silKey = softwareKey.OpenSubKey("SIL");
                if (silKey == null)
                    silKey = softwareKey.OpenSubKey(@"Wow6432Node\SIL");
			    return silKey;
			}
		}
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the registry key for the current application's company from the local machine
        /// settings. This is 'HKLM\Software\{Application.CompanyName}'
        /// NOTE: This key is not opened for write access because it will fail on
        /// non-administrator logins.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static RegistryKey CompanyKeyCurrentUser
        {
            get
            {
                RegistryKey softwareKey = Registry.CurrentUser.OpenSubKey("Software");
                Debug.Assert(softwareKey != null);
                var silKey = softwareKey.OpenSubKey("SIL");
                if (silKey == null)
                    silKey = softwareKey.OpenSubKey(@"Wow6432Node\SIL");
                return silKey;
            }
        }
        /// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the registry key for the Paratext application from the local machine
		/// settings. This is 'HKLM\Software\ScrChecks\1.0'
		/// NOTE: This key is not opened for write access because it will fail on
		/// non-administrator logins.
		/// </summary>
		/// ------------------------------------------------------------------------------------
        public static RegistryKey ParatextKey
        {
            get
            {
                RegistryKey softwareKey = Registry.LocalMachine.OpenSubKey("software");
                var scrChecksKey = softwareKey.OpenSubKey(@"ScrChecks\1.0");
                if (scrChecksKey == null)
                    scrChecksKey = softwareKey.OpenSubKey(@"Wow6432Node\ScrChecks\1.0");
                return scrChecksKey;
            }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the registry key for theWord application.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static RegistryKey TheWordKey
        {
            get
            {
                return Registry.ClassesRoot.OpenSubKey("theWordModule");
            }
        }

        public static String SwordKeyCommandValue
        {
            get { return Registry.ClassesRoot.OpenSubKey("sword").OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command").GetValue("") as string; }
        }
        public static RegistryKey JarFile
        {
            get { return Registry.ClassesRoot.OpenSubKey("jarfile"); }
        }

		public static string FallbackStringValue(string key)
		{
			return FallbackStringValue(key, null);
		}

		public static string FallbackStringValue(string key, string value)
		{
			object result;
			if (CheckSoftwareKey(Registry.CurrentUser, key, value, out result)) return (string) result;
			if (UnixVersionCheck()) return UnixFallbackStringValue(key, value);
			if (CheckSoftwareKey(Registry.LocalMachine, key, value, out result)) return (string) result;
			// See: https://stackoverflow.com/questions/974038/reading-64bit-registry-from-a-32bit-application
			using (var hive64 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
			{
				if (CheckSoftwareKey(hive64, key, value, out result)) return (string)result;
			}
			using (var hive64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
			{
				if (CheckSoftwareKey(hive64, key, value, out result)) return (string)result;
			}
			return null;
		}

		private static bool CheckSoftwareKey(RegistryKey hive, string key, string value, out object result)
		{
			var levels = new List<string> {"software"};
			levels.AddRange(key.Split('/'));
			result = GetLevelValue(hive, levels, value);
			return result != null;
		}

		private static object GetLevelValue(RegistryKey hive, IList<string> levels, string value)
		{
			using (var key = hive.OpenSubKey(levels[0]))
			{
				if (key == null) return null;
				if (levels.Count <= 1) return key.GetValue(value);
				levels.RemoveAt(0);
				return GetLevelValue(key, levels, value);
			}
		}

		private static readonly XmlDocument XDoc = new XmlDocument();
		private static string UnixFallbackStringValue(string key, string value)
		{
			var userName = Environment.UserName;
			foreach (var program in new List<string> { "paratext", "fieldworks" })
			{
				var registryPath = "/home/" + userName + "/.config/"+ program +"/registry/LocalMachine/software/";
				var valueFile = Path.Combine(registryPath, key.ToLower(), "values.xml");
				if (!File.Exists(valueFile)) continue;
				XDoc.RemoveAll();
				var xr = XmlReader.Create(valueFile);
				XDoc.Load(xr);
				xr.Close();
				var node = value != null? XDoc.SelectSingleNode("//*[@name='" + value + "']"): XDoc.DocumentElement;
				if (node != null) return node.InnerText;
			}
			return null;
		}

        public static bool UnixVersionCheck()
        {
            bool isRecentVersion = false;
            try
            {
                string getOSName = GetOsName();
                string majorVersion = string.Empty;
                if (getOSName.IndexOf("Unix") >= 0)
                {
                    isRecentVersion = true;
                    majorVersion = getOSName.Substring(0, 11);
                }
                if (majorVersion == "Unix 3.2.0.")
                {
                    isRecentVersion = true;
                }
            }
            catch { }
            return isRecentVersion;
        }

        public static string GetOsName()
        {
            OperatingSystem osInfo = Environment.OSVersion;
            var versionString = osInfo.VersionString;
            switch (osInfo.Platform)
            {
                case System.PlatformID.Win32NT:
                    switch (osInfo.Version.Major)
                    {
                        case 3:
                            versionString = "Windows NT 3.51";
                            break;
                        case 4:
                            versionString = "Windows NT 4.0";
                            break;
                        case 5:
                            versionString = osInfo.Version.Minor == 0 ? "Windows 2000" : "Windows XP";
                            break;
                        case 6:
                            versionString = osInfo.Version.Minor == 1 ? "Windows7" : "Windows8";
                            break;
                    }
                    break;

            }
            return versionString;
        }

    }
}
