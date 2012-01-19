using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Palaso.IO;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Types on which search classes are based.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public enum SearchClassType
	{
		/// <summary>
		/// PhoneticChars has been deprecated in favor of Phones. But PhoneticChars is retained
		/// so classes created by beta users before this change will still have their classes
		/// read by the program. However, classes of type PhoneticChars are updated the first
		/// time they are read to classes of type Phones.
		/// </summary>
		PhoneticChars = 0,
		Articulatory = 1,
		Binary = 2,
		OtherClass = 3,
		Phones = 4,
	}

	#region SearchClassList class
	/// ----------------------------------------------------------------------------------------
	[XmlType("SearchClasses")]
	public class SearchClassList : List<SearchClass>
	{
		public const string kFileName = "SearchClasses.xml";

		private PaProject m_project;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is for serialization/deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchClassList()
		{
		}

		/// ------------------------------------------------------------------------------------
		public SearchClassList(PaProject project)
		{
			m_project = project;
		}

		/// ------------------------------------------------------------------------------------
		public static string GetFileForProject(string projectPathPrefix)
		{
			return projectPathPrefix + kFileName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of default search classes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SearchClassList LoadDefaults(PaProject project)
		{
			return InternalLoad(project,
				FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, "DefaultSearchClasses.xml"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of search classes for the specified project. If the project is
		/// null, then the default list is loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SearchClassList Load(PaProject project)
		{
			return InternalLoad(project, GetFileForProject(project.ProjectPathFilePrefix));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of search classes for the specified project. If the project is
		/// null, then the default list is loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static SearchClassList InternalLoad(PaProject project, string filename)
		{
			SearchClassList srchClasses = null;

			if (File.Exists(filename))
				srchClasses = XmlSerializationHelper.DeserializeFromFile<SearchClassList>(filename);

			if (srchClasses == null)
				return new SearchClassList(project);

			srchClasses.m_project = project;
			bool upgradeMade = false;

			// Run through this for the sake of classes created before 26-Jul-07
			// that used the enumeration PhoneticChars instead of Phones.
			foreach (var srchClass in srchClasses.Where(c => c.Type == SearchClassType.PhoneticChars))
			{
				srchClass.Type = SearchClassType.Phones;
				upgradeMade = true;
			}

			if (upgradeMade)
				srchClasses.Save();

			return srchClasses;
		}

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			XmlSerializationHelper.SerializeToFile(
				GetFileForProject(m_project.ProjectPathFilePrefix), this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the SearchClass with the specified name. If the specified class name is
		/// surrounded by the open and close class brackets, they are stripped off before
		/// searching the list. This overload does not ignore case.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchClass this[string className]
		{
			get { return GetSearchClass(className, false); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the search class object from the list for the specified class name and
		/// factors in whether or not to ignore case.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchClass GetSearchClass(string className, bool ignoreCase)
		{
			// Strip off the brackets if they're there.
			var modifiedName = className.Replace(App.kOpenClassBracket, string.Empty);
			modifiedName = modifiedName.Replace(App.kCloseClassBracket, string.Empty);

			if (ignoreCase)
				modifiedName = modifiedName.ToLower();

			return (from srchClass in this
					let storedName = (ignoreCase ? srchClass.Name.ToLower() : srchClass.Name)
					where storedName == modifiedName
					select srchClass).FirstOrDefault();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Modify the tab's pattern text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string ModifyPatternText(string tabText)
		{
			foreach (var srchClass in this)
			{
				var className = App.kOpenClassBracket + srchClass.Name + App.kCloseClassBracket;
				var oldText = (Settings.Default.ShowClassNamesInSearchPatterns ? srchClass.Pattern : className);

				if (tabText.Contains(oldText))
				{
					var newText = (Settings.Default.ShowClassNamesInSearchPatterns ? className : srchClass.Pattern);
					return tabText.Replace(oldText, newText);
				}
			}

			return string.Empty;
		}
	}

	#endregion

	#region SearchClass class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a single search class
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SearchClass
	{
		private string _pattern;

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Name { get; set; }

		/// ------------------------------------------------------------------------------------
		public string Pattern
		{
			get { return _pattern; }
			set	{_pattern = (value == null ? null : value.Replace(" ", string.Empty));}
		}

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Group { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("SearchClassType")]
		public SearchClassType Type { get; set; }

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return (Name ?? base.ToString());
		}
	}

	#endregion
}
