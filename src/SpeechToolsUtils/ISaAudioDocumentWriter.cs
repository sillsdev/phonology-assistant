using System.Runtime.InteropServices;

namespace SIL.SpeechTools.Utils
{
	[ComVisible(true)]
	[Guid("CA9D7080-0DDB-4b3f-919F-F4AE1E0B1B5B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	public interface ISaAudioDocumentWriter
	{
		#region methods
		bool Initialize(string audioFilePath, string origMD5HashCode, bool isTempDB);
		void Close();
		void AddSegment(int annotationType, uint offset, uint length, string annotation);
		void AddMarkSegment(uint offset, uint length, string gloss, string partOfSpeech,
			string reference, bool isBookmark);
		void DeleteSegments();
		void WriteAsMusicXML(string SAMAString);
        void ExportMusicXML(string SAMAString, string fileName);
		void Copy(string destFilePath, bool md5MustMatch);
		void DeleteTempDB();
		#endregion

		#region Properties
		string Country { set; }
		string Dialect { set; }
		string EthnologueId { set; }
		string Family { set; }
		string FreeFormTranslation { set; }
		string LanguageName { set; }
		string NoteBookReference { set; }
		string Region { set; }
		string Transcriber { set; }
		string SpeakerName { set; }
		char SpeakerGender { set; }
		int DataChunkSize { set; }
		int AverageBytesPerSecond { set; }
		int BitsPerSample { set; }
		int BlockAlignment { set; }
		int CalcFreqHigh { set; }
		int CalcFreqLow { set; }
		int CalcGrpSize { set; }
		int CalcIntrpGap { set; }
		int CalcPercntChng { set; }
		int CalcVoicingThd { set; }
		int Channels { set; }
		bool Commit();
		int FormatTag { set; }
		int NumberOfSamples { set; }
		int RecordBandWidth { set; }
		int RecordFileFormat { set; }
		int RecordSampleSize { set; }
		int RecordTimeStamp { set; }
		string SADescription { set; }
		int SAFlags { set; }
		int SamplesPerSecond { set; }
		int SignalBandWidth { set; }
		int SignalEffSampSize { set; }
		int SignalMax { set; }
		int SignalMin { set; }
		#endregion
	}
}
