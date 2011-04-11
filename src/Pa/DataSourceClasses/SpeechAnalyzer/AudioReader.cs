using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SIL.Pa.DataSource.Sa
{
	/// ----------------------------------------------------------------------------------------
	public class AudioReader : IDisposable
	{
		/// ------------------------------------------------------------------------------------
		public enum InitResult
		{
			/// <summary></summary>
			FileNotFound,
			/// <summary></summary>
			InvalidFormat,
			/// <summary></summary>
			NoSAData,
			/// <summary></summary>
			Success
		}
		
		internal const string kidRiff = "RIFF";
		internal const string kidWave = "WAVE";
		internal const string kidFmtChunk = "fmt ";
		internal const string kidDataChunk = "data";
		internal const string kidSAChunk = "sa  ";
		internal const string kidUttChunk = "utt ";
		internal const string kidEticChunk = "etic";
		internal const string kidEmicChunk = "emic";
		internal const string kidToneChunk = "tone";
		internal const string kidOrthoChunk = "orth";
		internal const string kidMarkChunk = "mark";
		internal const string kidFontChunk = "font";
		internal const string kidSpkrChunk = "spkr";
		internal const string kidLangChunk = "lang";
		internal const string kidRefChunk = "ref ";
		internal const string kidMdatChunk = "mdat";
		internal const string kidMPL1Chunk = "mpl1";
		internal const string kidMPL2Chunk = "mpl2";
		internal const string kidMPL3Chunk = "mpl3";
		internal const string kidMPL4Chunk = "mpl4";
		internal const string kidMFontChunk = "mfon";

		private FileStream m_stream;
		private BinaryReader m_reader;
		private bool m_isWave;

		#region IDisposable Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Play nice and clean up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (m_stream != null)
				m_stream.Close();
			
			m_stream = null;
			m_reader = null;
		}

		#endregion
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initialize WaveReader object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public InitResult Initialize(string audioFile)
		{
			if (!File.Exists(audioFile))
				return InitResult.FileNotFound;
			
			AudioFile = audioFile;
			m_stream = File.Open(audioFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			m_reader = new BinaryReader(m_stream, Encoding.ASCII);

			m_isWave = IsValidWaveFile();
			if (!m_isWave || (GetChunkOffset(m_stream, kidSAChunk) == -1))
				return InitResult.NoSAData;

			return InitResult.Success;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if this is a valid wave file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool IsValidWaveFile(FileStream stream)
		{
			var reader = new BinaryReader(stream);

			// Verify RIFF chunk exists
			stream.Position = 0;
			if (new string(Encoding.ASCII.GetChars(reader.ReadBytes(4))) != kidRiff)
				return false;

			// Verify WAVE chunk exists
			stream.Position = 8;
			if (new string(Encoding.ASCII.GetChars(reader.ReadBytes(4))) != kidWave)
				return false;

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if this is a valid wave file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsValidWaveFile()
		{
			return IsValidWaveFile(m_stream);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full file name and path to the audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string AudioFile { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the reader's MD5 Hash Code for the audio file.
		/// For wave files it is based on audio content alone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string MD5HashCode
		{
			get {return GetMD5HashCode(m_stream);}
		}

        /// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the MD5 hash code for the specified file.
		/// For wave files it is based on audio content alone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetMD5HashCode(string audioFile)
		{
			return (File.Exists(audioFile) ?
				GetMD5FromByteArray(File.ReadAllBytes(audioFile)) : null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the MD5 hash code for the file opened in the specified stream.
		/// For wave files it is based on audio content alone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetMD5HashCode(FileStream stream)
		{
			BinaryReader reader = new BinaryReader(stream);

			stream.Position = 0;
			int audioSize = (int)stream.Length;

			// if this is a wave file, only calculate on the data chunk
			if (IsValidWaveFile(stream))
			{
				stream.Position = GetChunkOffset(stream, kidDataChunk) + 4;
				audioSize = reader.ReadInt32();
			}

			return GetMD5FromByteArray(reader.ReadBytes(audioSize));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculates the MD5 hash code for the specified byte array.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetMD5FromByteArray(byte[] byteArray)
		{
			MD5 md5 = new MD5CryptoServiceProvider();
			Guid guid = new Guid(md5.ComputeHash(byteArray));
			return guid.ToString().ToUpper();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the wave file's bytes per sample (i.e. Hz value).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public long BytesPerSecond
		{
			get
			{
				if (!m_isWave)
					return 0;

				m_stream.Position = GetChunkOffset(m_stream, kidFmtChunk) + 8;
				m_reader.ReadUInt16();
				int channels = m_reader.ReadUInt16();
				int samplesPerSec = m_reader.ReadInt32();
				m_reader.ReadInt32();
				m_reader.ReadUInt16();
				int bitsPerSample = m_reader.ReadUInt16();

				int bytesPerSample = (bitsPerSample / 8) * channels;
				return (bytesPerSample * samplesPerSec);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary> 
		/// This function will return the byte offset, in a WAV file, of the beginning of the
		/// desired chunk. The chunk is specified by passing it's four character ID via sChunk.
		/// Each chunk begins with a 4 character ID followed by 4 bytes that indicate how long
		/// the chunk is. That will allow us to find out where the next chunk begins. If,
		/// however, a chunks length is odd, there will be a 1 byte pad at the end of that
		/// chunk so the next chunk always starts on an even byte boundary.  
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static long GetChunkOffset(FileStream stream, string chunkId)
		{
			var reader = new BinaryReader(stream);

			// Start at beginning of 'fmt' chunk.
			stream.Position = 12;

			while (stream.Position < stream.Length)
			{
				// Read the chunk id.
				string id = new string(Encoding.ASCII.GetChars(reader.ReadBytes(4)));

				// If we've found the chunk id we're looking for, then return the
				// current position less the 4 bytes we just read to get the id.
				if (id == chunkId)
					return stream.Position - 4;

				// Get chunk length.
				uint chunkLen = reader.ReadUInt32();

				//Account for odd length chunks.
				if ((chunkLen & 1) != 0)
					chunkLen++;

				// Make sure we don't move the file pointer beyond EOF.
				if ((stream.Position + chunkLen) >= stream.Length - 4)
					break;

				// Move the file pointer over the chunk to the beginning of the next chunk.
				stream.Position += chunkLen;
			}

			return -1;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Method for reading the audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsOldSaFormat
		{
			get { return (GetChunkOffset(m_stream, kidSAChunk) > -1); }
		}
	}
}
