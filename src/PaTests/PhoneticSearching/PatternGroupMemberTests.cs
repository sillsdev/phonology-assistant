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
using System.Linq;
using System.Text;
using NUnit.Framework;
using SIL.Pa.TestUtils;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class PatternGroupMemberTests : TestBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CloseIPACharacterMember methods in PatternGroupMember.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseSinglePhoneMemberTest1()
		{
			const char dental = '\u032A';
			const char aspiration = '\u02B0';

			var member = new PatternGroupMember();
			member.AddToMember('t');
			member.AddToMember(dental);
			member.AddToMember(App.kBottomTieBarC);
			member.AddToMember('s');
			member.DiacriticPattern = aspiration.ToString();
			member.CloseMember();

			Assert.AreEqual(string.Format("t{0}s", App.kBottomTieBarC), member.Member);
			Assert.IsTrue(member.DiacriticPattern.IndexOf(dental) >= 0);
			Assert.IsTrue(member.DiacriticPattern.IndexOf(aspiration) >= 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CloseIPACharacterMember methods in PatternGroupMember.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseSinglePhoneMemberTest2()
		{
			const char dental = '\u032A';
			const char aspiration = '\u02B0';

			var member = new PatternGroupMember();
			member.AddToMember('t');
			member.AddToMember(App.kBottomTieBarC);
			member.AddToMember('s');
			member.AddToMember(dental);
			member.DiacriticPattern = aspiration + "+";
			member.CloseMember();

			Assert.AreEqual(string.Format("t{0}s", App.kBottomTieBarC), member.Member);
			Assert.IsTrue(member.DiacriticPattern.IndexOf(dental) >= 0);
			Assert.IsTrue(member.DiacriticPattern.IndexOf(aspiration) >= 0);
			Assert.IsTrue(member.DiacriticPattern.IndexOf('+') >= 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CloseIPACharacterMember methods in PatternGroupMember.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseSinglePhoneMemberTest3()
		{
			const char dental = '\u032A';
			const char aspiration = '\u02B0';

			var member = new PatternGroupMember();
			member.AddToMember('t');
			member.AddToMember(App.kTopTieBarC);
			member.AddToMember('s');
			member.AddToMember(dental);
			member.DiacriticPattern = aspiration + "*";
			member.CloseMember();

			Assert.AreEqual(string.Format("t{0}s", App.kTopTieBarC), member.Member);
			Assert.IsTrue(member.DiacriticPattern.IndexOf(dental) >= 0);
			Assert.IsTrue(member.DiacriticPattern.IndexOf(aspiration) >= 0);
			Assert.IsTrue(member.DiacriticPattern.IndexOf('*') >= 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CloseIPACharacterMember methods in PatternGroupMember.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseSinglePhoneMemberTest4()
		{
			const char dental = '\u032A';
			const char aspiration = '\u02B0';

			var member = new PatternGroupMember();
			member.AddToMember('t');
			member.AddToMember(dental);
			member.AddToMember(App.kTopTieBarC);
			member.AddToMember('s');
			member.AddToMember(aspiration);
			member.CloseMember();

			Assert.AreEqual(string.Format("t{0}s", App.kTopTieBarC), member.Member);
			Assert.AreEqual(string.Format("{0}{1}", dental, aspiration), member.DiacriticPattern);
		}

	}
}
