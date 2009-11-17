// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright from='2009' to='2009' company='SIL International'>
//		Copyright (c) 2009, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: FeatureCacheBaseTests.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using NUnit.Framework;
using SIL.Pa.TestUtils;
using SilUtils;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class FeatureCacheBaseTests : TestBase
	{
		private FeatureCacheBase<FeatureBase> m_fcache;
		private readonly List<string> m_fNames =
			new List<string> { "Bananas", "Cheese", "Ham", "Onions", "Peppers", "Pickles" };

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_fcache = new FeatureCacheBase<FeatureBase>();
			
			// Create a bunch of features and add them to the feature cache.
			int bit = 0;
			foreach (string name in m_fNames)
			{
				var feat = new FeatureBase { Name = name };
				ReflectionHelper.SetProperty(feat, "Bit", bit++);
				string cleanName = ReflectionHelper.GetStrResult(m_fcache.GetType(), "CleanUpFeatureName", name);
				m_fcache.Add(cleanName, feat);
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the integer indexer that a correct number of long integers is allocated for holding the
		/// specified number of bits.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IntIndexer_Get()
		{
			for (int i = 0; i < m_fNames.Count; i++)
				Assert.AreEqual(m_fNames[i], m_fcache[i].Name);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that setting a feature cache item (using the string indexer) by feature
		/// name works.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void StringIndexer_Set()
		{
			m_fcache.Clear();

			foreach (string name in m_fNames)
				m_fcache[name] = new FeatureBase { Name = name };

			Assert.AreEqual(m_fNames.Count, m_fcache.Values.Count);

			int i = 0;
			foreach (FeatureBase feat in m_fcache.Values)
				Assert.AreEqual(m_fNames[i++], feat.Name);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that getting a feature cache item (using the string indexer) by feature
		/// name works.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void StringIndexer_Get()
		{
			for (int i = 0; i < m_fNames.Count; i++)
				Assert.AreEqual(m_fNames[i], m_fcache[m_fNames[i]].Name);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that getting a feature cache item (using the string indexer) by feature
		/// name works.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void NullIndexer_GetAndSet()
		{
			// Test the null setter.
			int count = m_fcache.Count;
			m_fcache[null] = new FeatureBase();
			Assert.AreEqual(count, m_fcache.Count);

			// Test a null getter.
			Assert.IsNull(m_fcache[null]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test the CleanUpFeatureName method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CleanUpFeatureName()
		{
			string fname = "[UGLY FEATURE]";
			string cleanName = ReflectionHelper.GetStrResult(m_fcache.GetType(), "CleanUpFeatureName", fname);
			Assert.AreEqual("ugly feature", cleanName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that FeatureExists method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FeatureExists()
		{
			Assert.IsFalse(m_fcache.FeatureExits("Beef", false));
			Assert.IsFalse(m_fcache.FeatureExits("Chicken", false));
			Assert.IsFalse(m_fcache.FeatureExits(null, false));

			for (int i = 0; i < m_fNames.Count; i++)
				Assert.IsTrue(m_fcache.FeatureExits(m_fNames[i], false));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetFeatureList method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFeatureList()
		{
			var fmask = new FeatureMask(m_fcache.Count);
			fmask[m_fNames.IndexOf("Onions")] = true;
			fmask[m_fNames.IndexOf("Ham")] = true;
			fmask[m_fNames.IndexOf("Cheese")] = true;

			List<string> list = m_fcache.GetFeatureList(fmask);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual("Cheese", list[0]);
			Assert.AreEqual("Ham", list[1]);
			Assert.AreEqual("Onions", list[2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetFeatureText method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFeatureText()
		{
			var fmask = new FeatureMask(m_fcache.Count);
			fmask[m_fNames.IndexOf("Onions")] = true;
			fmask[m_fNames.IndexOf("Ham")] = true;
			fmask[m_fNames.IndexOf("Cheese")] = true;

			Assert.AreEqual("Cheese, Ham, Onions", m_fcache.GetFeaturesText(fmask));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetMask method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMask()
		{
			var list = new List<string> {"Pickles", "Bananas", "Peppers"};

			var fmask = m_fcache.GetMask(list);
			Assert.IsTrue(fmask[m_fNames.IndexOf("Pickles")]);
			Assert.IsTrue(fmask[m_fNames.IndexOf("Bananas")]);
			Assert.IsTrue(fmask[m_fNames.IndexOf("Peppers")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Onions")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Ham")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Cheese")]);
		}
	}
}
