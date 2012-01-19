using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SIL.Pa.Model;

namespace SIL.Pa.Tests
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class BFeatureCacheTests
	{
		private BFeatureCache m_cache;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_cache = new BFeatureCache();

			var list = new List<Feature>();
			list.Add(new Feature { Name = "red" });
			list.Add(new Feature { Name = "blue" });
			m_cache.LoadFromList(list);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the LoadFromList method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadFromList()
		{
			m_cache.Clear();
			Assert.AreEqual(0, m_cache.Count);

			var list = new List<Feature>(2);
			list.Add(new Feature { Name = " -lion" });
			list.Add(new Feature { Name = " +bear " });
			m_cache.LoadFromList(list);

			Assert.AreEqual(4, m_cache.Count);
			Assert.AreEqual("+lion", m_cache[0].Name);
			Assert.AreEqual("+bear", m_cache[1].Name);
			Assert.AreEqual("-lion", m_cache[2].Name);
			Assert.AreEqual("-bear", m_cache[3].Name);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CleanNameForLoad method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CleanNameForLoad()
		{
			Assert.IsNull(m_cache.CleanNameForLoad(null));
			Assert.AreEqual("+dog", m_cache.CleanNameForLoad("   +dog "));
			Assert.AreEqual("+cat", m_cache.CleanNameForLoad("-cat "));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the PlusFeatures property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPlusFeatures()
		{
			var pfeatures = m_cache.PlusFeatures.ToList();
			Assert.AreEqual(2, pfeatures.Count);
			Assert.IsTrue(pfeatures.Exists(x => x.Name == "+blue"));
			Assert.IsTrue(pfeatures.Exists(x => x.Name == "+red"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the MinusFeatures property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMinusFeatures()
		{
			var mfeatures = m_cache.MinusFeatures.ToList();
			Assert.AreEqual(2, mfeatures.Count);
			Assert.IsTrue(mfeatures.Exists(x => x.Name == "-blue"));
			Assert.IsTrue(mfeatures.Exists(x => x.Name == "-red"));
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetOppositeFeature method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetOppositeFeature_FromFeature()
		{
			Assert.IsNull(m_cache.GetOppositeFeature((Feature)null));

			Assert.AreEqual(m_cache["-blue"], m_cache.GetOppositeFeature(m_cache["+blue"]));
			Assert.AreEqual(m_cache["-red"], m_cache.GetOppositeFeature(m_cache["+red"]));
			Assert.AreEqual(m_cache["+blue"], m_cache.GetOppositeFeature(m_cache["-blue"]));
			Assert.AreEqual(m_cache["+red"], m_cache.GetOppositeFeature(m_cache["-red"]));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetOppositeFeature method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetOppositeFeature_FromString()
		{
			Assert.IsNull(m_cache.GetOppositeFeature((string)null));
			Assert.AreEqual(m_cache["-red"], m_cache.GetOppositeFeature("+red"));
			Assert.AreEqual(m_cache["-blue"], m_cache.GetOppositeFeature("+blue"));
			Assert.AreEqual(m_cache["+red"], m_cache.GetOppositeFeature("-red"));
			Assert.AreEqual(m_cache["+blue"], m_cache.GetOppositeFeature("-blue"));
		}
	}
}
