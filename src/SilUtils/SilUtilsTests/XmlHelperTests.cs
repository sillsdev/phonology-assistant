using System.IO;
using NUnit.Framework;

namespace SilUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class XmlnHelperTests
	{
		#region IsEmptyOrInvalid tests
		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_Null_True()
		{
			Assert.IsTrue(XmlHelper.IsEmptyOrInvalid(null));
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_EmptyFileName_True()
		{
			Assert.IsTrue(XmlHelper.IsEmptyOrInvalid(string.Empty));
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_NonExistentFile_True()
		{
			var tmpFile = Path.GetTempFileName();
			File.Delete(tmpFile);
			Assert.IsTrue(XmlHelper.IsEmptyOrInvalid(tmpFile));
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_EmptyFile_True()
		{
			var tmpFile = Path.GetTempFileName();
			Assert.IsTrue(XmlHelper.IsEmptyOrInvalid(tmpFile));
			File.Delete(tmpFile);
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_XmlDeclarationOnly_True()
		{
			var tmpFile = Path.GetTempFileName();
			using (var stream = File.CreateText(tmpFile))
			{
				stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				stream.Close();
				Assert.IsTrue(XmlHelper.IsEmptyOrInvalid(tmpFile));
				File.Delete(tmpFile);
			}
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_MissingRootClose_True()
		{
			var tmpFile = Path.GetTempFileName();
			using (var stream = File.CreateText(tmpFile))
			{
				stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				stream.WriteLine("<root>");
				stream.Close();
				Assert.IsTrue(XmlHelper.IsEmptyOrInvalid(tmpFile));
				File.Delete(tmpFile);
			}
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_MissingRootOpen_True()
		{
			var tmpFile = Path.GetTempFileName();
			using (var stream = File.CreateText(tmpFile))
			{
				stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				stream.WriteLine("</root>");
				stream.Close();
				Assert.IsTrue(XmlHelper.IsEmptyOrInvalid(tmpFile));
				File.Delete(tmpFile);
			}
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_MinimalValid_False()
		{
			var tmpFile = Path.GetTempFileName();
			using (var stream = File.CreateText(tmpFile))
			{
				stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				stream.WriteLine("<root/>");
				stream.Close();
				Assert.IsFalse(XmlHelper.IsEmptyOrInvalid(tmpFile));
				File.Delete(tmpFile);
			}
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_Valid_False()
		{
			var tmpFile = Path.GetTempFileName();
			using (var stream = File.CreateText(tmpFile))
			{
				stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				stream.WriteLine("<root>blah blah blah</root>");
				stream.Close();
				Assert.IsFalse(XmlHelper.IsEmptyOrInvalid(tmpFile));
				File.Delete(tmpFile);
			}
		}

		#endregion
	}
}
