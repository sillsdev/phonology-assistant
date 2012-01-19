using NUnit.Framework;
using SIL.Pa.DataSource;

namespace SIL.Pa.DataSourceClasses
{
	/// --------------------------------------------------------------------------------
	[TestFixture]
	public class SfmDataSourceReaderTests
	{
		private long _startPoint;
		private long _endPoint;
		private string _filename;
		private SfmDataSourceReader _reader;
		
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_filename = @"c:\junk1\junk2\junk3.wav";
			_reader = new SfmDataSourceReader();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAudioFileInfo_InputStringDoesNotContainPoints_ReturnsFileNameAndZeroPoints()
		{
			var result = _reader.GetAudioFileInfo(_filename, out _startPoint, out _endPoint);
			Assert.AreEqual(result, _filename);
			Assert.AreEqual(0, _startPoint);
			Assert.AreEqual(0, _endPoint);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAudioFileInfo_InputStringContainsStartPoint_ReturnsFileNameAndStartPoint()
		{
			var result = _reader.GetAudioFileInfo(_filename + " 2.123", out _startPoint, out _endPoint);

			Assert.AreEqual(result, _filename);
			Assert.AreEqual(2123, _startPoint);
			Assert.AreEqual(0, _endPoint);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAudioFileInfo_InputStringContainsStartStopPoints_ReturnsFileNameAndStartStopPoints()
		{
			var result = _reader.GetAudioFileInfo(_filename + " 2.123 4.43265", out _startPoint, out _endPoint);

			Assert.AreEqual(result, _filename);
			Assert.AreEqual(2123, _startPoint);
			Assert.AreEqual(4433, _endPoint);
		}
	}
}
