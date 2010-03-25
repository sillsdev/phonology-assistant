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
	public class XmlSerializationHelperTests
	{
		#region IsEmptyOrInvalid tests
		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_Null_False()
		{
			Assert.IsFalse(XmlSerializationHelper.IsEmptyOrInvalid(null));
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_EmptyFileName_False()
		{
			Assert.IsFalse(XmlSerializationHelper.IsEmptyOrInvalid(string.Empty));
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_NonExistentFile_False()
		{
			var tmpFile = Path.GetTempFileName();
			File.Delete(tmpFile);
			Assert.IsFalse(XmlSerializationHelper.IsEmptyOrInvalid(tmpFile));
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_EmptyFile_False()
		{
			var tmpFile = Path.GetTempFileName();
			Assert.IsFalse(XmlSerializationHelper.IsEmptyOrInvalid(tmpFile));
			File.Delete(tmpFile);
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_XmlDeclarationOnly_False()
		{
			var tmpFile = Path.GetTempFileName();
			using (var stream = File.CreateText(tmpFile))
			{
				stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				stream.Close();
				Assert.IsFalse(XmlSerializationHelper.IsEmptyOrInvalid(tmpFile));
				File.Delete(tmpFile);
			}
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_MissingRootClose_False()
		{
			var tmpFile = Path.GetTempFileName();
			using (var stream = File.CreateText(tmpFile))
			{
				stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				stream.WriteLine("<root>");
				stream.Close();
				Assert.IsFalse(XmlSerializationHelper.IsEmptyOrInvalid(tmpFile));
				File.Delete(tmpFile);
			}
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_MissingRootOpen_False()
		{
			var tmpFile = Path.GetTempFileName();
			using (var stream = File.CreateText(tmpFile))
			{
				stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				stream.WriteLine("</root>");
				stream.Close();
				Assert.IsFalse(XmlSerializationHelper.IsEmptyOrInvalid(tmpFile));
				File.Delete(tmpFile);
			}
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_MinimalValid_True()
		{
			var tmpFile = Path.GetTempFileName();
			using (var stream = File.CreateText(tmpFile))
			{
				stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				stream.WriteLine("<root/>");
				stream.Close();
				Assert.IsTrue(XmlSerializationHelper.IsEmptyOrInvalid(tmpFile));
				File.Delete(tmpFile);
			}
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IsEmptyOrInvalid_Valid_True()
		{
			var tmpFile = Path.GetTempFileName();
			using (var stream = File.CreateText(tmpFile))
			{
				stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				stream.WriteLine("<root>blah blah blah</root>");
				stream.Close();
				Assert.IsTrue(XmlSerializationHelper.IsEmptyOrInvalid(tmpFile));
				File.Delete(tmpFile);
			}
		}

		#endregion
	}
}
