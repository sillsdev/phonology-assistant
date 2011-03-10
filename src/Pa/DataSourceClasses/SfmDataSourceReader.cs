using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.DataSource
{
	public class SfmDataSourceReader : IDisposable
	{
		private PaProject m_project;
		private PaDataSource m_dataSource;
		protected Dictionary<string, FieldMapping> m_markerMappings;
		protected IEnumerable<string> m_interlinearFields;
		private BackgroundWorker m_worker;

		/// ------------------------------------------------------------------------------------
		public static SfmDataSourceReader Create(BackgroundWorker worker, PaProject project, PaDataSource ds)
		{
			if (ds.FieldMappings.Any(m => m.Field.Type == FieldType.Phonetic))
				return null;

			var reader = new SfmDataSourceReader();
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
			var recMarker = m_dataSource.SfmRecordMarker;
			if (string.IsNullOrEmpty(recMarker))
			{
				// TODO: report error.
				return false;
			}

			m_interlinearFields = m_dataSource.FieldMappings
				.Where(m => m.IsInterlinear).Select(m => m.Field.Name);

			m_markerMappings = m_dataSource.FieldMappings
				.ToDictionary(m => m.NameInDataSource, m => m);

			var recordLines = new Dictionary<string, string>();

			bool foundFirstRecord = false;

			foreach (var line in File.ReadAllLines(m_dataSource.SourceFile))
			{
				m_worker.ReportProgress(0);
				var currLine = line.Trim();

				// Toss out lines that don't begin with a backslash or that precede
				// the first line in the file that begins with our record marker.
				if (!currLine.StartsWith("\\") || (!foundFirstRecord && !currLine.StartsWith(recMarker)))
					continue;

				foundFirstRecord = true;

				// If we've stored up a record and we've come to a new record, then write
				// the data of the stored one before beginning to store the new one.
				if (currLine.StartsWith(recMarker) && recordLines.Count > 0)
				{
					recCache.Add(SaveSingleRecord(recordLines));
					recordLines.Clear();
				}

				// Split the line between its marker and its data.
				var split = currLine.Split(" ".ToCharArray(), 2);
				if (split.Length >= 2)
					recordLines[split[0]] = split[1].TrimStart();
			}

			// Save last record, if there is one.
			if (recordLines.Count > 0)
				recCache.Add(SaveSingleRecord(recordLines));

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private RecordCacheEntry SaveSingleRecord(IDictionary<string, string> recordLines)
		{
			var recCacheEntry = new RecordCacheEntry(true, m_project)
			{
				DataSource = m_dataSource,
				NeedsParsing = true,
				FirstInterlinearField = m_dataSource.FirstInterlinearField,
				InterlinearFields = m_interlinearFields.ToList(),
			};

			foreach (var kvp in m_markerMappings)
			{
				string fieldData;
				if (!recordLines.TryGetValue(kvp.Key, out fieldData) || fieldData.Length == 0)
					continue;

				if (kvp.Value.Field.Type == FieldType.AudioFilePath)
					SaveAudioFileInfo(recCacheEntry, kvp.Value.Field, fieldData);
				else
					recCacheEntry.SetValue(kvp.Value.Field.Name, fieldData);
			}

			return recCacheEntry;
		}

		#region Methods for saving audio file information.
		/// ------------------------------------------------------------------------------------
		private void SaveAudioFileInfo(RecordCacheEntry recCacheEntry, PaField audioFilefield,
			string fieldData)
		{
			long start, end;
			var fileName = GetAudioFileInfo(fieldData, out start, out end);
			if (fileName == null)
				return;

			var length = end - start;
			var offsetField = m_project.GetAudioOffsetField();
			var lengthField = m_project.GetAudioLengthField();
			recCacheEntry.SetValue(offsetField.Name, start.ToString());
			recCacheEntry.SetValue(lengthField.Name, length.ToString());
			recCacheEntry.SetValue(audioFilefield.Name, fileName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parse the sound file name returning the file name and beginning / ending points
		/// for playing, if any.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetAudioFileInfo(string fileName, out long startPoint, out long endPoint)
		{
			startPoint = 0L;
			endPoint = 0L;

			// Handle null
			if (fileName == null)
				return string.Empty;

			// Handle empty string
			fileName = fileName.Trim();
			if (fileName == string.Empty)
				return string.Empty;

			// Check if the end of the file name string is one of the common audio file types.
			// If so, there's no sense in checking for start and end points since the entire
			// string ends with a file extension.
			if (new[] { ".wav", ".mp3", ".wma", ".ogg", ".ram", ".aif", ".au", ".voc" }.Any(ext => fileName.ToLower().EndsWith(ext)))
			{
				return fileName;
			}

			// Find the last space in the file name string. If one cannot be
			// found then we know there is no start and stop point.
			int iSpace = fileName.LastIndexOf(' ');
			if (iSpace < 0)
				return fileName;

			// Remove everything following the last space and assume it's a numeric value
			// indicating a start or stop point (in seconds) in the audio file.
			string sTime2 = fileName.Substring(iSpace);

			// Try to convert the value to a numeric.
			float fTime2;
			if (!Utils.TryFloatParse(sTime2, out fTime2))
				return fileName;

			// Remove from the file name string the text removed that represents a numeric value.
			fileName = fileName.Substring(0, iSpace);

			// Find the last space in the file name string. If one cannot be
			// found then we know there is only a start point and stop point.
			iSpace = fileName.LastIndexOf(' ');
			if (iSpace < 0)
			{
				// Assume the time was specified in seconds. We want it in milliseconds.
				startPoint = (long)(Math.Ceiling(fTime2 * 1000f));
				return fileName;
			}

			// Remove everything following the last space and assume it's a numeric value
			// indicating a start point (in seconds) in the audio file.
			string sTime1 = fileName.Substring(iSpace);

			// Try to convert the value to a numeric.
			float fTime1;
			if (!Utils.TryFloatParse(sTime1, out fTime1))
			{
				// Assume the time was specified in seconds. We want it in milliseconds.
				// The parse attempt failed so just make the start and end times the same.
				startPoint = (long)(Math.Ceiling(fTime2 * 1000f));
				return fileName;
			}

			// Assume the times are specified in seconds. We want it in milliseconds.
			startPoint = (long)(Math.Ceiling(fTime1 * 1000f));
			endPoint = (long)(Math.Ceiling(fTime2 * 1000f));

			// Remove from the file name string the text removed that represents the start point.
			return fileName.Substring(0, iSpace);
		}

		#endregion
	}
}
