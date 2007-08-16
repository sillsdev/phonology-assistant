using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
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
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("SearchClasses")]
	public class SearchClassList : List<SearchClass>
	{
		public const string kSearchClassesFilePrefix = "SearchClasses.xml";
		public const string kDefaultSearchClassesFile = "DefaultSearchClasses.xml";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the SearchClass with the specified name. If the specified class name is
		/// surrounded by the open and close class brackets, they are stripped off before
		/// searching the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchClass this[string className]
		{
			get
			{
				// Strip off the brackets if they're there.
				string modifiedName = className.Replace(PaApp.kOpenClassBracket, string.Empty);
				modifiedName = modifiedName.Replace(PaApp.kCloseClassBracket, string.Empty);

				foreach (SearchClass srchClass in this)
				{
					if (srchClass.Name == modifiedName)
						return srchClass;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of default search classes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SearchClassList Load()
		{
			string filename = (PaApp.Project != null ?
				PaApp.Project.ProjectPathFilePrefix + kSearchClassesFilePrefix :
				Path.Combine(Application.StartupPath, kDefaultSearchClassesFile));

			SearchClassList srchClasses = null;

			if (File.Exists(filename))
			{
				srchClasses = STUtils.DeserializeData(filename,
					typeof(SearchClassList)) as SearchClassList;
			}

			if (srchClasses != null)
			{
				bool upgradeMade = false;

				// Run through this for the sake of classes created before 26-Jul-07
				// that used the enumeration PhoneticChars instead of Phones.
				foreach (SearchClass srchClass in srchClasses)
				{
					if (srchClass.SearchClassType == SearchClassType.PhoneticChars)
					{
						srchClass.SearchClassType = SearchClassType.Phones;
						upgradeMade = true;
					}
				}

				if (upgradeMade)
					srchClasses.Save();
			}

			return (srchClasses ?? new SearchClassList());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			if (PaApp.Project != null)
			{
				string filename = PaApp.Project.ProjectPathFilePrefix + kSearchClassesFilePrefix;
				STUtils.SerializeData(filename, this);
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Modify the tab's pattern text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string ModifyPatternText(string tabText)
		{
			foreach (SearchClass srchClass in this)
			{
				string className = PaApp.kOpenClassBracket + srchClass.Name + PaApp.kCloseClassBracket;
				string oldText = (PaApp.Project.ShowClassNamesInSearchPatterns ? srchClass.Pattern : className);

				if (tabText.Contains(oldText))
				{
					string newText = (PaApp.Project.ShowClassNamesInSearchPatterns ? className : srchClass.Pattern);
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
		private string m_name;
		private string m_pattern;
		private string m_group;
		private SearchClassType m_type;

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Pattern
		{
			get { return m_pattern; }
			set	{m_pattern = (value == null ? value : value.Replace("], [", "],["));}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Group
		{
			get { return m_group; }
			set { m_group = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public SearchClassType SearchClassType
		{
			get { return m_type; }
			set { m_type = value; }
		}

		#endregion
	}

	#endregion
}
