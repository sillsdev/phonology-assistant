// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using NUnit.Framework;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.TestUtils;

namespace SIL.Pa.Tests
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class FFSearchEngineTests : TestBase
	{
		/// ------------------------------------------------------------------------------------
		[Test][Ignore("Rewrite using SearchQueryValidator")]
		public void GetPhonesInPatternTest()
		{
			App.BFeatureCache.LoadFromList(new[] { new Feature { Name = "+rnd" }});
			var query = new SearchQuery();
			query.Pattern = "ab{o,e}/[C]_[+rnd]xyz";
			var engine = new SearchEngine(query);

			//var phones = engine.GetPhonesInPattern();
			//Assert.AreEqual(7, phones.Length);
			//Assert.AreEqual("a", phones[0]);
			//Assert.AreEqual("b", phones[1]);
			//Assert.AreEqual("o", phones[2]);
			//Assert.AreEqual("e", phones[3]);
			//Assert.AreEqual("x", phones[4]);
			//Assert.AreEqual("y", phones[5]);
			//Assert.AreEqual("z", phones[6]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ParsePhoneTest()
		{
			string resultPhone;
			string resultDiacritics;

			const string dental = "\u032A";
			string dentalT = "t" + dental;
			string dentalS = "s" + dental;

			string phone = dentalT + App.kTopTieBar + dentalS;
			SearchEngine.ParsePhone(phone, out resultPhone, out	resultDiacritics);
			Assert.AreEqual(resultPhone, "t" + App.kTopTieBar + "s");
			Assert.AreEqual(resultDiacritics, dental);

			phone = "t" + App.kTopTieBar + dentalS;
			SearchEngine.ParsePhone(phone, out resultPhone, out	resultDiacritics);
			Assert.AreEqual(resultPhone, "t" + App.kTopTieBar + "s");
			Assert.AreEqual(resultDiacritics, dental);

			phone = dentalT + App.kBottomTieBar + "s";
			SearchEngine.ParsePhone(phone, out resultPhone, out	resultDiacritics);
			Assert.AreEqual(resultPhone, "t" + App.kBottomTieBar + "s");
			Assert.AreEqual(resultDiacritics, dental);

			phone = dentalT;
			SearchEngine.ParsePhone(phone, out resultPhone, out	resultDiacritics);
			Assert.AreEqual(resultPhone, "t");
			Assert.AreEqual(resultDiacritics, dental);

			phone = dentalT + "\u02B0";
			SearchEngine.ParsePhone(phone, out resultPhone, out	resultDiacritics);
			Assert.AreEqual(resultPhone, "t");
			Assert.AreEqual(resultDiacritics, dental + "\u02B0");
		}
	}
}
