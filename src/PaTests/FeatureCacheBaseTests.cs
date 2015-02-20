// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
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
using System.Linq;
using NUnit.Framework;
using SIL.Pa.Model;
using SIL.Pa.TestUtils;
using SilTools;

namespace SIL.Pa.Tests
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class FeatureCacheBaseTests : TestBase
	{
		private FeatureCacheBase m_cache;
		private readonly List<string> m_fNames =
			new List<string> { "Bananas", "Cheese", "Ham", "Onions", "Peppers", "Pickles" };

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_cache = new FeatureCacheBase();
			m_cache.LoadFromList(m_fNames.Select(n => new Feature { Name = n }));

			//// Create a bunch of features and add them to the feature cache.
			//int bit = 0;
			//foreach (string name in m_fNames)
			//{
			//    var feat = new Feature { Name = name };
			//    ReflectionHelper.SetProperty(feat, "Bit", bit++);
			//    string cleanName = ReflectionHelper.GetStrResult(m_cache.GetType(), "CleanUpFeatureName", name);
			//    m_cache.Add(cleanName, feat);
			//}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CleanNameForLoad method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CleanNameForLoad()
		{
			Assert.IsNull(ReflectionHelper.GetStrResult(m_cache, "CleanNameForLoad", null));
			Assert.AreEqual("dog", ReflectionHelper.GetStrResult(m_cache, "CleanNameForLoad", "   dog "));
			Assert.AreEqual("cat", ReflectionHelper.GetStrResult(m_cache, "CleanNameForLoad", "cat "));
		}

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
				Assert.AreEqual(m_fNames[i], m_cache[i].Name);
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
			m_cache.Clear();

			foreach (string name in m_fNames)
				m_cache[name] = new Feature { Name = name };

			Assert.AreEqual(m_fNames.Count, m_cache.Values.Count);

			int i = 0;
			foreach (Feature feat in m_cache.Values)
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
				Assert.AreEqual(m_fNames[i], m_cache[m_fNames[i]].Name);
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
			int count = m_cache.Count;
			m_cache[null] = new Feature();
			Assert.AreEqual(count, m_cache.Count);

			// Test a null getter.
			Assert.IsNull(m_cache[null]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Add method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Add()
		{
			int count = m_cache.Count;
	
			m_cache.Add(null);
			Assert.AreEqual(count, m_cache.Count);

			var feat = new Feature();
			m_cache.Add(feat);
			Assert.AreEqual(count, m_cache.Count);

			feat.Name = "+Camel";
			m_cache.Add(feat);
			Assert.AreEqual(count + 1, m_cache.Count);
			Assert.IsNotNull(m_cache["+Camel"]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CleanUpFeatureName_SendBracketedName_RemovesBrackets()
		{
			Assert.AreEqual("UGLY FEATURE", FeatureCacheBase.CleanUpFeatureName("[UGLY FEATURE]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CleanUpFeatureName_SendUntrimmedName_TrimsName()
		{
			Assert.AreEqual("UGLY FEATURE", FeatureCacheBase.CleanUpFeatureName("[ UGLY FEATURE ]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CleanUpFeatureName_SendCleanName_ReturnsInput()
		{
			Assert.AreEqual("UGLY FEATURE", FeatureCacheBase.CleanUpFeatureName("UGLY FEATURE"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void FeatureExists_SendNonExistentFeature_ReturnFalse()
		{
			Assert.IsFalse(m_cache.FeatureExits("Beef", false));
			Assert.IsFalse(m_cache.FeatureExits("Chicken", false));
		}
		
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FeatureExists_SendNull_ReturnFalse()
		{
			Assert.IsFalse(m_cache.FeatureExits(null, false));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void FeatureExists_SendExistingFeatures_ReturnTrue()
		{
			foreach (var name in m_fNames)
				Assert.IsTrue(m_cache.FeatureExits(name, false));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void FeatureExists_SendNameThatNeedsCleaning_ReturnTrue()
		{
			Assert.IsTrue(m_cache.FeatureExits("[  " + m_fNames[2] + " ]", false));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetFeatureList method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFeatureList()
		{
			var fmask = new FeatureMask(m_cache.Count);
			fmask[m_fNames.IndexOf("Onions")] = true;
			fmask[m_fNames.IndexOf("Ham")] = true;
			fmask[m_fNames.IndexOf("Cheese")] = true;

			var list = m_cache.GetFeatureList(fmask).ToArray();
			Assert.AreEqual(3, list.Length);
			Assert.AreEqual("Cheese", list[0]);
			Assert.AreEqual("Ham", list[1]);
			Assert.AreEqual("Onions", list[2]);
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests the GetFeatureText method.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetFeatureText()
		//{
		//    var fmask = new FeatureMask(m_cache.Count);
		//    fmask[m_fNames.IndexOf("Onions")] = true;
		//    fmask[m_fNames.IndexOf("Ham")] = true;
		//    fmask[m_fNames.IndexOf("Cheese")] = true;

		//    Assert.AreEqual("Cheese, Ham, Onions", m_cache.GetFeaturesText(fmask));
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetMask method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMask_FromString()
		{
			var fmask = m_cache.GetMask("Peppers");
			Assert.IsTrue(fmask[m_fNames.IndexOf("Peppers")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Pickles")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Bananas")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Onions")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Ham")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Cheese")]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetMask method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMask_FromFeature()
		{
			var feat = m_cache["Pickles"];
			var fmask = m_cache.GetMask(feat);
			Assert.IsTrue(fmask[m_fNames.IndexOf("Pickles")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Bananas")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Peppers")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Onions")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Ham")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Cheese")]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetMask method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMask_FromList()
		{
			var list = new List<string> {"Pickles", "Bananas", "Peppers"};

			var fmask = m_cache.GetMask(list);
			Assert.IsTrue(fmask[m_fNames.IndexOf("Pickles")]);
			Assert.IsTrue(fmask[m_fNames.IndexOf("Bananas")]);
			Assert.IsTrue(fmask[m_fNames.IndexOf("Peppers")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Onions")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Ham")]);
			Assert.IsFalse(fmask[m_fNames.IndexOf("Cheese")]);
		}
	}
}
