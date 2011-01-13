using System;
using System.Collections.Specialized;
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
	public class MruProjectsTests
	{
		private string _mruFileFolder;
		private StringCollection _paths;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs before each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_paths = new StringCollection();
			MruFiles.Initialize(_paths, 3);
			_mruFileFolder = Path.Combine(Path.GetTempPath(), "~mrufolder~");
			Directory.CreateDirectory(_mruFileFolder);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs after each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TestTearDown()
		{
			try
			{
				Directory.Delete(_mruFileFolder, true);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the RemoveStalePaths method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveStalePaths()
		{
			Assert.IsEmpty(_paths);

			var path1 = Path.Combine(_mruFileFolder, "mru1.tmp");
			File.CreateText(path1).Close();
			MruFiles.AddNewPath(path1);

			var path2 = Path.Combine(_mruFileFolder, "mru2.tmp");
			File.CreateText(path2).Close();
			MruFiles.AddNewPath(path2);

			Assert.AreEqual(2, _paths.Count);
			Assert.Contains(path1, _paths);
			Assert.Contains(path2, _paths);

			File.Delete(path1);
			ReflectionHelper.CallMethod(typeof(MruFiles), "RemoveStalePaths", null);
			Assert.AreEqual(1, _paths.Count);
			Assert.IsFalse(_paths.Contains(path1));
			Assert.Contains(path2, _paths);

			File.Delete(path2);
			ReflectionHelper.CallMethod(typeof(MruFiles), "RemoveStalePaths", null);
			Assert.IsEmpty(_paths);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddNewPath method when a null path is passed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddNewNullPath()
		{
			MruFiles.AddNewPath(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddNewPath method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddNewPath()
		{
			Assert.IsEmpty(_paths);

			var path1 = Path.Combine(_mruFileFolder, "mru1.tmp");
			File.CreateText(path1).Close();
			Assert.IsTrue(MruFiles.AddNewPath(path1));
			Assert.AreEqual(1, _paths.Count);
			Assert.Contains(path1, _paths);
			
			var path2 = Path.Combine(_mruFileFolder, "mru2.tmp");
			MruFiles.AddNewPath(path2);
			Assert.IsFalse(MruFiles.AddNewPath(path2));
			Assert.AreEqual(1, _paths.Count);
			Assert.Contains(path1, _paths);

			// Readd a path that already exists.
			Assert.IsTrue(MruFiles.AddNewPath(path1));
			Assert.AreEqual(1, _paths.Count);
			Assert.Contains(path1, _paths);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddNewPath method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddNewPath_DefaultAdd()
		{
			Assert.IsEmpty(_paths);

			var path1 = Path.Combine(_mruFileFolder, "mru1.tmp");
			File.CreateText(path1).Close();
			Assert.IsTrue(MruFiles.AddNewPath(path1));

			var path2 = Path.Combine(_mruFileFolder, "mru2.tmp");
			File.CreateText(path2).Close();
			Assert.IsTrue(MruFiles.AddNewPath(path2));

			var path3 = Path.Combine(_mruFileFolder, "mru3.tmp");
			File.CreateText(path3).Close();
			Assert.IsTrue(MruFiles.AddNewPath(path3));

			Assert.AreEqual(3, _paths.Count);
			Assert.AreEqual(path1, _paths[2]);
			Assert.AreEqual(path2, _paths[1]);
			Assert.AreEqual(path3, _paths[0]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddNewPath method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddNewPath_AddToEnd()
		{
			Assert.IsEmpty(_paths);

			var path1 = Path.Combine(_mruFileFolder, "mru1.tmp");
			File.CreateText(path1).Close();
			Assert.IsTrue(MruFiles.AddNewPath(path1, true));

			var path2 = Path.Combine(_mruFileFolder, "mru2.tmp");
			File.CreateText(path2).Close();
			Assert.IsTrue(MruFiles.AddNewPath(path2, true));

			var path3 = Path.Combine(_mruFileFolder, "mru3.tmp");
			File.CreateText(path3).Close();
			Assert.IsTrue(MruFiles.AddNewPath(path3, true));

			Assert.AreEqual(3, _paths.Count);
			Assert.AreEqual(path1, _paths[0]);
			Assert.AreEqual(path2, _paths[1]);
			Assert.AreEqual(path3, _paths[2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddNewPath method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddNewPath_AddMoreThanMax()
		{
			Assert.IsEmpty(_paths);

			var path1 = Path.Combine(_mruFileFolder, "mru1.tmp");
			File.CreateText(path1).Close();
			Assert.IsTrue(MruFiles.AddNewPath(path1));

			var path2 = Path.Combine(_mruFileFolder, "mru2.tmp");
			File.CreateText(path2).Close();
			Assert.IsTrue(MruFiles.AddNewPath(path2));

			var path3 = Path.Combine(_mruFileFolder, "mru3.tmp");
			File.CreateText(path3).Close();
			Assert.IsTrue(MruFiles.AddNewPath(path3));

			var path4 = Path.Combine(_mruFileFolder, "mru4.tmp");
			File.CreateText(path4).Close();
			Assert.IsTrue(MruFiles.AddNewPath(path4));

			Assert.AreEqual(3, _paths.Count);
			Assert.AreEqual(path2, _paths[2]);
			Assert.AreEqual(path3, _paths[1]);
			Assert.AreEqual(path4, _paths[0]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Latest property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Latest()
		{
			Assert.IsEmpty(_paths);

			var path1 = Path.Combine(_mruFileFolder, "mru1.tmp");
			File.CreateText(path1).Close();
			MruFiles.AddNewPath(path1);
			Assert.AreEqual(path1, MruFiles.Latest);

			var path2 = Path.Combine(_mruFileFolder, "mru2.tmp");
			File.CreateText(path2).Close();
			MruFiles.AddNewPath(path2);
			Assert.AreEqual(path2, MruFiles.Latest);

			var path3 = Path.Combine(_mruFileFolder, "mru3.tmp");
			File.CreateText(path3).Close();
			MruFiles.AddNewPath(path3, true);
			Assert.AreEqual(path2, MruFiles.Latest);
		}
	}
}
