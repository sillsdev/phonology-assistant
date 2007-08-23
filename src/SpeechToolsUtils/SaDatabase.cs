using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace SIL.SpeechTools.Utils
{
    [XmlRoot("SaDocCache")]
    public class SaDocumentCache : List<SaAudioDocument>
    {
    }

    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public class SaDatabase
    {
		private const string kSaDatabaseName = "SaTranscriptions.xml";
        private static SaDocumentCache s_docCache;
        private static string s_dbPath = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool Load()
		{
			// Get the location of the database from the registry.
			const string keyName = @"HKEY_CURRENT_USER\Software\SIL\Speech Analyzer";
			string dbDir = (string)Registry.GetValue(keyName, "DataLocation", string.Empty);
			if (string.IsNullOrEmpty(dbDir))
			{
				// The registry key doesn't exist so just assume the following path.
				dbDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				dbDir = Path.Combine(dbDir, @"SIL Software\Speech Analyzer");
			}

			// Check if that location exists.
			if (!Directory.Exists(dbDir))
				return false;

			// Add the database name to the path and check if the DB file exisits.
			string fullDbPath = Path.Combine(dbDir, kSaDatabaseName);
			if (!File.Exists(fullDbPath))
				return false;

			// Load the contents of the database into the cache.
			s_docCache =
				STUtils.DeserializeData(fullDbPath, typeof(SaDocumentCache)) as SaDocumentCache;

			if (s_docCache == null)
				return false;

			s_dbPath = fullDbPath;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Delete()
		{
			if (File.Exists(s_dbPath))
				File.Delete(s_dbPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes the contents of the document cache to the SA database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool Save()
		{
			if (s_dbPath == null || s_docCache == null || s_docCache.Count == 0)
				return false;

			try
			{
				STUtils.SerializeData(s_dbPath, s_docCache);
			}
			catch { }

			return true;
		}

		/// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the document cache.
        /// </summary>
        /// ------------------------------------------------------------------------------------
		[XmlElement("SaAudioDocument")]
        public static List<SaAudioDocument> Cache
        {
            get { return s_docCache as List<SaAudioDocument>; }
        }
	}
}
