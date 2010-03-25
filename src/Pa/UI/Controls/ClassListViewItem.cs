using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.Localization;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SilUtils;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// ListViewItem subclass for the class list resultView.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ClassListViewItem : ListViewItem
	{
		public const string kClassNameSubitem = "ClassName";
		public const string kIPACharBracketing = "{{{0}}}";
		public const string kFeatureBracketing = "[{0}]";
		public const string kANDBracketing = "[{0}]";
		public const string kORBracketing = "{{{0}}}";
		public static string kClassBracketing = App.kOpenClassBracket + "{0}" +
			App.kCloseClassBracket;

		public SearchClassType ClassType = SearchClassType.Phones;
		public bool AllowEdit = true;
		public bool ANDFeatures = true;
		public bool IsDirty;
		public bool InEditMode;
		private FeatureMask m_mask;

		#region Constructor and Copy method
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Default constructor for a ClassListViewItem
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ClassListViewItem() : base(Properties.Resources.kstidNewClassName)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructor for a ClassListViewItem when assigning the classes name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ClassListViewItem(string text) : base(text)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Copy Constructor for a ClassListViewItem
		/// </summary>
		/// <param name="item">Item being copied.</param>
		/// ------------------------------------------------------------------------------------
		public ClassListViewItem(ClassListViewItem item) : base(item.Text)
		{
			// Create subitems to copy into. Start at 1 because the subitem collection
			// always includes the item that owns the subitems. I don't know why.
			for (int i = 1; i < item.SubItems.Count; i++)
				SubItems.Add(new ListViewSubItem());

			Copy(item);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Copies the information from the specified ClassListViewItem and its subitems.
		/// </summary>
		/// <param name="item">Item being copied</param>
		/// ------------------------------------------------------------------------------------
		public void Copy(ClassListViewItem item)
		{
			Text = item.Text;
			ClassType = item.ClassType;
			ANDFeatures = item.ANDFeatures;
			AllowEdit = item.AllowEdit;
			Mask = item.Mask.Clone();
			Pattern = item.Pattern;
			Tag = item.Tag;

			for (int i = 0; i < item.SubItems.Count; i++)
			{
				SubItems[i].Name = item.SubItems[i].Name;

				if (i == 1)
				{
					SubItems[1].Font = ClassMembersFont;
					SubItems[1].Text = Pattern;
				}
				else if (i == 2)
					SubItems[2].Text = ClassTypeText;
			}
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the classes masks.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public FeatureMask Mask
		{
			get { return m_mask; }
			set { m_mask = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font that should be used to display the item's members.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Font ClassMembersFont
		{
			get
			{
				return (ClassType == SearchClassType.Phones ?
					FontHelper.PhoneticFont : FontHelper.UIFont);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the display text indicating what the class is based on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string ClassTypeText
		{
			get
			{
				switch (ClassType)
				{
					case SearchClassType.Phones:
						return LocalizationManager.LocalizeString("PhonesClassTypeLabel",
							"Phones", "Label on the define classes dialog box indicating class type",
							App.kLocalizationGroupMisc, LocalizationCategory.Label,
							LocalizationPriority.High);

					case SearchClassType.Articulatory:
						return LocalizationManager.LocalizeString("ArticulatoryFeaturesClassTypeLabel",
							"Articulatory features", "Articulatory features class type label.",
							App.kLocalizationGroupMisc, LocalizationCategory.Label,
							LocalizationPriority.High);

					case SearchClassType.Binary:
						return LocalizationManager.LocalizeString("BinaryFeaturesClassTypeLabel",
							"Binary features", "Binary features class type label.",
							App.kLocalizationGroupMisc, LocalizationCategory.Label,
							LocalizationPriority.High);
				
					default:
						return null;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the class' pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string Pattern
		{
			get { return (SubItems.Count >= 2 ? SubItems[1].Text : null); }
			set
			{
				if (SubItems.Count < 2)
					SubItems.Add(new ListViewSubItem(this, value));
				else
					SubItems[1].Text = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a search class object built from the information in the list view item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public SearchClass SearchClass
		{
			get
			{
				SearchClass srchClass = new SearchClass();
				srchClass.Name = (Text == null ? string.Empty : Text.Trim());
				srchClass.SearchClassType = ClassType;
				srchClass.Pattern = Pattern;
				return srchClass;
			}
		}

		#endregion

		#region Methods/property to get a human-readable string of class members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an array of feature names that are the members in this class if the class
		/// type is articulatory or binary.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string[] FeatureNames
		{
			get
			{
				if (ClassType == SearchClassType.Articulatory)
					return App.AFeatureCache.GetFeatureList(Mask).ToArray();

				if (ClassType == SearchClassType.Binary)
					return App.BFeatureCache.GetFeatureList(Mask).ToArray();

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a displayable list of the members of the class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string FormattedMembersString
		{
			get
			{
				if (ClassType == SearchClassType.Phones)
				{
					if (Pattern == null)
						return string.Empty;

					string ptrn = Pattern.Replace("[", string.Empty);
					ptrn = ptrn.Replace("]", string.Empty);
					ptrn = ptrn.Replace("{", string.Empty);
					ptrn = ptrn.Replace("}", string.Empty);
					return ptrn;
				}

				string[] members;
				string brackets;

				//if (ClassType == ClassType.OtherClasses)
				//{
				//    // Get the other classes that make up this class.
				//    members = GetOtherClassesStrings(ref brackets);
				//}
				//else
				{
					// Get the features that make up this class.
					members = (ClassType == SearchClassType.Articulatory ?
						App.AFeatureCache.GetFeatureList(Mask).ToArray() :
						App.BFeatureCache.GetFeatureList(Mask).ToArray());

					brackets = kFeatureBracketing;
				}

				// Now build a displayable string of all the members of this class.
				StringBuilder readableMembers = new StringBuilder();
				for (int i = 0; i < members.Length; i++)
				{
					readableMembers.Append(string.Format(brackets, members[i]));

					// When features are OR'd together, put a comma between them.
					if (i < members.Length - 1 && !ANDFeatures)
						readableMembers.Append(",");
				}

				if (readableMembers.Length == 0)
					return string.Empty;

				// Don't bother to bracket anything when there is only one member in the class.
				if (members.Length == 1)
					return readableMembers.ToString();

				return string.Format((ANDFeatures ? kANDBracketing : kORBracketing),
					readableMembers);
			}
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Returns an array of feature names of those features contained in the specified
		///// masks.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private string[] GetOtherClassesStrings(ref string brackets)
		//{
		//    bool showClassNames = PaApp.ShowClassNames;

		//    // When showing other classes that make up this class, only show brackets around
		//    // the other classes when the user wants to see class names. If the user wants
		//    // to see class members instead, then don't put brackets around them.
		//    brackets = (showClassNames ? kClassBracketing : "{0}");
			
		//    if (OtherClassIds == null)
		//        return new string[] {};

		//    ArrayList classes = new ArrayList();

		//    string[] classIds = OtherClassIds.Split(",".ToCharArray());
		//    foreach (string id in classIds)
		//    {
		//        // Lookup the record for the class id.
		//        string sql = DataUtils.SelectSQL("PhoneticClass", "PhoneticClassID", int.Parse(id));
		//        using (OleDbDataReader reader = DataUtils.GetSQLResultsFromDB(sql))
		//        {
		//            if (reader.Read())
		//            {
		//                // If the user wants to see class names, then just pull the
		//                // class name from the record. Otherwise, we have to format
		//                // a string with the classes members.
		//                if (showClassNames)
		//                    classes.Add(reader["ClassName"] as string);
		//                else
		//                {
		//                    // Create a ClassListViewItem that contains all the
		//                    // information about the class and add that to the list
		//                    // of members for this class.
		//                    ClassListViewItem item = Create(reader);
		//                    string formattedMembers = item.FormattedMembersString;
		//                    if (!string.IsNullOrEmpty(formattedMembers))
		//                        classes.Add(formattedMembers);
		//                }
		//            }
		//        }
		//    }

		//    return (string[])classes.ToArray(typeof(string));
		//}

		#endregion

		#region Custom drawing
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides a way to custom draw the members sub item for IPA character classes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Draw(DrawListViewItemEventArgs e)
		{
			// This seems more reliable than using e.State. I was having trouble with that.
			bool selected = (ListView.SelectedItems.Contains(this) && !InEditMode);

			Color clrFore = SystemColors.WindowText;

			if (!selected)
				e.DrawBackground();
			else
			{
				clrFore = (ListView.Focused ? SystemColors.HighlightText : SystemColors.ControlText);
				Rectangle rc = e.Bounds;
				rc.X += 4;
				rc.Width -= 4;
				e.Graphics.FillRectangle((ListView.Focused ?
					SystemBrushes.Highlight : SystemBrushes.Control), rc);
			}

			// Draw the item and subitem texts.
			TextFormatFlags flags = TextFormatFlags.SingleLine |
				TextFormatFlags.TextBoxControl | TextFormatFlags.EndEllipsis |
				TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping;

			flags |= (ListView.RightToLeft == RightToLeft.Yes ?
				TextFormatFlags.RightToLeft : TextFormatFlags.Left);

			for (int i = 0; i < SubItems.Count; i++)
			{
				Rectangle rc;

				// If we're painting the class name, then get the bounds this way because
				// SubItems[0].Bounds gives the entire width of the list view.
				if (i == 0)
					rc = GetBounds(ItemBoundsPortion.Label);
				else
				{
					rc = SubItems[i].Bounds;
					rc.Inflate(-2, 2);
				}

				Font fnt = FontHelper.UIFont;
				if (i == 1 && ClassType == SearchClassType.Phones)
				{
					fnt = (ListView is ClassListView ?
						((ClassListView)ListView).PhoneticFont : FontHelper.PhoneticFont);
				}

				TextRenderer.DrawText(e.Graphics, SubItems[i].Text, fnt, rc, clrFore, flags);
			}

			if (ListView.Focused)
				e.DrawFocusRectangle();

			return true;
		}

		#endregion

		#region Static methods for creating a ClassListViewItem from SearchClass object.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new list resultView item for the specified search class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ClassListViewItem Create(SearchClass srchClass,
			bool addMembersAndClassTypeColumns)
		{
			ClassListViewItem item = new ClassListViewItem(srchClass.Name);
			item.Name = kClassNameSubitem;
			item.ClassType = srchClass.SearchClassType;

			if (addMembersAndClassTypeColumns)
			{
				item.SubItems.Add(srchClass.Pattern);
				item.SubItems.Add(item.ClassTypeText);
			}

			item.ANDFeatures = (string.IsNullOrEmpty(srchClass.Pattern) ||
				srchClass.Pattern[0] == '[');

			if (srchClass.SearchClassType == SearchClassType.Articulatory)
			{
				item.Mask = App.AFeatureCache.GetEmptyMask();
				GetMasksFromPattern(item, srchClass.Pattern);
			}
			else if (srchClass.SearchClassType == SearchClassType.Binary)
			{
				item.Mask = App.BFeatureCache.GetEmptyMask();
				GetMasksFromPattern(item, srchClass.Pattern);
			}

			return item;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void GetMasksFromPattern(ClassListViewItem item, string pattern)
		{
			item.Mask.Clear();
			
			pattern = pattern.Replace("][", ",");
			pattern = pattern.Replace("],]", ",");
			pattern = pattern.Replace("[", string.Empty);
			pattern = pattern.Replace("]", string.Empty);
			pattern = pattern.Replace("{", string.Empty);
			pattern = pattern.Replace("}", string.Empty);
			pattern = pattern.ToLower();

			string[] features = pattern.Split(",".ToCharArray(),
				StringSplitOptions.RemoveEmptyEntries);
			
			foreach (string feature in features)
			{
				Feature feat = (item.ClassType == SearchClassType.Articulatory ?
					App.AFeatureCache[feature] :
					App.BFeatureCache[feature]);
					
				if (feat != null)
					item.Mask[feat.Bit] = true;
			}
		}
		
		#endregion
	}
}
