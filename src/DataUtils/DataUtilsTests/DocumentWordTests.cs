using System;
using System.Text;
using System.Collections.Generic;
using SIL.SpeechTools.Database;
using System.Collections;
using NUnit.Framework;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This is a test class for SIL.SpeechTools.Database.DocumentWord and is intended
	/// to contain all SIL.SpeechTools.Database.DocumentWord Unit Tests
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class DocumentWordTest
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verify that phonetic strings get normalized in decomposed form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyPhoneticNormalization()
		{
			DocumentWord docWord = new DocumentWord(0);

			docWord.Phonetic = "ë";
			Assert.AreEqual(2, docWord.PhoneticAnnotationLength);
			Assert.AreEqual("\u0065\u0308", docWord.Phonetic);

			string normalizedWord = "X\u0061\u0306\u0301X";

			docWord.Phonetic = "X\u1EAFX";
			Assert.AreEqual(5, docWord.PhoneticAnnotationLength);
			Assert.AreEqual(normalizedWord, docWord.Phonetic);

			docWord.Phonetic = "X\u0103\u0301X";
			Assert.AreEqual(5, docWord.PhoneticAnnotationLength);
			Assert.AreEqual(normalizedWord, docWord.Phonetic);

			// Test a bunch, but obviously not all, modified vowels
			docWord.Phonetic = "àáâãäåçèéêëìíîïñòóôõöùúûüāăąĉċčēĕėěę";
			Assert.AreEqual(72, docWord.PhoneticAnnotationLength);
		}
	}
}
