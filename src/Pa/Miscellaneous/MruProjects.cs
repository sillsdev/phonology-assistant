using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SIL.Pa.Properties;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a class to manage the list of most recently used project paths.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public static class MruProjects
	{
		public const int MaxMRUListSize = 9;

		private static readonly List<string> s_paths = new List<string>(MaxMRUListSize);

		/// ------------------------------------------------------------------------------------
		static MruProjects()
		{
			if (Settings.Default.MRUList != null)
				LoadList(Settings.Default.MRUList);
		}

		/// ------------------------------------------------------------------------------------
		public static string[] Paths
		{
			get
			{
				RemoveStalePaths();
				return s_paths.ToArray();
			}
			set { LoadList(value); }
		}

		/// ------------------------------------------------------------------------------------
		private static void LoadList(ICollection values)
		{ 
			s_paths.Clear();
			if (values == null)
				return;

			int i = 0;
			foreach (object val in values)
			{
				string path = val as string;
				if (path == null)
					continue;

				if (!File.Exists(path))
					continue;

				if (i++ == MaxMRUListSize)
					break;

				if (!s_paths.Contains(path))
					s_paths.Add(path);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the path to the most recently used (i.e. opened) project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string Latest
		{
			get { return (s_paths.Count == 0 ? null : s_paths[0]); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes non existant paths from the MRU list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void RemoveStalePaths()
		{
			if (s_paths.Count > 0)
			{
				for (int i = s_paths.Count - 1; i >= 0; i--)
				{
					if (!File.Exists(s_paths[i]))
						s_paths.RemoveAt(i);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified file path to top of list of most recently used files if it 
		/// exists (returns false if it doesn't exist)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool AddNewPath(string path)
		{
			return AddNewPath(path, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified file path to top of list of most recently used files if it 
		/// exists (returns false if it doesn't exist)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool AddNewPath(string path, bool addToEnd)
		{
			if (path == null)
				throw new ArgumentNullException("path");

			if (!File.Exists(path))
				return false;

			// Remove the path from the list if it exists already.
			s_paths.Remove(path);

			// Make sure inserting a new path at the beginning will not exceed our max.
			if (s_paths.Count >= MaxMRUListSize)
				s_paths.RemoveAt(s_paths.Count - 1);

			if (addToEnd)
				s_paths.Add(path);
			else
				s_paths.Insert(0, path);
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public static void Save()
		{
			var collection = new System.Collections.Specialized.StringCollection();
			collection.AddRange(s_paths.ToArray());
			Settings.Default.MRUList = (collection.Count == 0 ? null : collection);
			Settings.Default.Save();
		}
	}
}