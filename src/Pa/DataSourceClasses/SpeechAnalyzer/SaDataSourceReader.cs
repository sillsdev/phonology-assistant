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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using SIL.Pa.Model;

namespace SIL.Pa.DataSource.Sa
{
	public class SaDataSourceReader : IDisposable
	{
		private PaProject m_project;
		private PaDataSource m_dataSource;
		private BackgroundWorker m_worker;

		/// ------------------------------------------------------------------------------------
		public static SaDataSourceReader Create(BackgroundWorker worker, PaProject project, PaDataSource ds)
		{
			if (!ds.FieldMappings.Any(m => m.Field.Type == FieldType.Phonetic))
				return null;

			var reader = new SaDataSourceReader();
			reader.m_worker = worker;
			reader.m_project = project;
			reader.m_dataSource = ds;

			return reader;
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			m_worker = null;
			m_project = null;
			m_dataSource = null;
		}
	
		/// ------------------------------------------------------------------------------------
		public bool Read(RecordCache recCache)
		{
			var reader = new SaAudioDocumentReader(m_worker);
			if (!reader.Initialize(m_dataSource.SourceFile) || reader.Words == null)
				return false;

			// Make only a single record entry for the entire wave file.
			var recCacheEntry = new RecordCacheEntry(false, m_project)
			{
				DataSource = m_dataSource,
				NeedsParsing = false,
				Channels = reader.Channels,
				BitsPerSample = reader.BitsPerSample,
				SamplesPerSecond = reader.SamplesPerSecond,
			};

			var audioField = m_project.GetAudioFileField();
			recCacheEntry.SetValue(audioField.Name, m_dataSource.SourceFile);

			m_worker.ReportProgress(0);

			// Get all the record level fields.
			foreach (var fname in m_dataSource.FieldMappings.Where(m => !m.IsParsed).Select(m => m.Field.Name))
				SetFieldValueFromObject(typeof(SaAudioDocumentReader), fname, reader, recCacheEntry.SetValue);

			int wordIndex = 0;
			recCacheEntry.WordEntries = new List<WordCacheEntry>();
			foreach (var kvp in reader.Words)
			{
				var wentry = new WordCacheEntry(recCacheEntry, wordIndex++);

				foreach (var fname in m_dataSource.FieldMappings.Where(m => m.IsParsed).Select(m => m.Field.Name))
					SetFieldValueFromObject(typeof(AudioDocWords), fname, kvp.Value, wentry.SetValue);

				wentry.AudioOffset = kvp.Key;
				wentry.AudioLength = kvp.Value.AudioLength;
				recCacheEntry.WordEntries.Add(wentry);
			}

			recCache.Add(recCacheEntry);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void SetFieldValueFromObject(Type type, string fieldName, object srcObject,
			Action<string, string> setValueAction)
		{
            if (type.GetProperty(fieldName) != null)
            {
				var value = type.InvokeMember(fieldName, BindingFlags.GetProperty |
					BindingFlags.Instance | BindingFlags.Public, null, srcObject, null);
				
                if (value != null)
				    setValueAction(fieldName, value.ToString());
			}
		}

		///// ------------------------------------------------------------------------------------
		// TODO: Figure out if needed.
		//private static bool HandleWaveFileNeedsConverting(string filename)
		//{
		//    if (AudioPlayer.GetSaPath() != null)
		//        return true;

		//    var msg = App.LocalizeString("AudioConvertProblemMsg",
		//        "It appears the audio file '{0}' may have been created using an old version " +
		//        "of Speech Analyzer. In order for Phonology Assistant to read data associated " +
		//        "with the audio file it must first be converted using some components of " +
		//        "Speech Analyzer 3.0.1, but it is not installed. Please install Speech Analyzer " +
		//        "3.0.1 and try again.", "Message displayed when SA 3.0.1 is not installed and " +
		//        "PA is trying to read an audio file created using a version SA older than 3.0.1.",
		//        App.kLocalizationGroupInfoMsg);

		//    msg = string.Format(msg, filename);

		//    using (var dlg = new DownloadSaDlg(msg))
		//        dlg.ShowDialog();

		//    return false;
		//}
	}
}
