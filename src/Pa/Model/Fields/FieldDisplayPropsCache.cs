// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Localization;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.Model
{
	#region FieldDisplayPropsCache class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class extends a generic list of PaFieldDisplayProperties instances by providing
	/// methods for saving, loading and getting/setting various display properties for
	/// specified fields.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FieldDisplayPropsCache : List<PaFieldDisplayProperties>
	{
		private static readonly List<Font> s_fontCache = new List<Font>();

		/// ------------------------------------------------------------------------------------
		public Exception SaveProjectFieldDisplayProps(PaProject project)
		{
			var path = PaFieldDisplayProperties.GetFileForProject(project.ProjectPathFilePrefix);
			Exception e = null;
			XmlSerializationHelper.SerializeToFile(path, this, "FieldDisplayProps", out e);
			return e;
		}

		#region static methods
		/// ------------------------------------------------------------------------------------
		public static FieldDisplayPropsCache LoadProjectFieldDisplayProps(PaProject project)
		{
			var path = PaFieldDisplayProperties.GetFileForProject(project.ProjectPathFilePrefix);
			if (!File.Exists(path))
				return GetDefaultCache();

			Exception e;
			var cache = XmlSerializationHelper.DeserializeFromFile<FieldDisplayPropsCache>(path, "FieldDisplayProps", out e);

			if (e == null)
				return cache;

			var msg = LocalizationManager.GetString(
				"ProjectFields.ErrorReadingFieldDisplayPropertiesFileMsg",
				"The following error occurred when reading the file\n\n'{0}'\n\n{1}");

			while (e.InnerException != null)
				e = e.InnerException;

			Utils.MsgBox(string.Format(msg, path, e.Message));

			return null;
		}

		/// ------------------------------------------------------------------------------------
		private static FieldDisplayPropsCache GetDefaultCache()
		{
			int displayIndex = 0;
			var list = from string name in Settings.Default.DefaultVisibleFields
					   select new PaFieldDisplayProperties(name, true, true)
							{ DisplayIndexInGrid = displayIndex, DisplayIndexInRecView = displayIndex++ };
			
			var cache = new FieldDisplayPropsCache();
			cache.AddRange(list);
			return cache;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public PaFieldDisplayProperties GetDisplayProps(string name)
		{
			return this.SingleOrDefault(dp => dp.Name == name);
		}

		#region getter methods
		/// ------------------------------------------------------------------------------------
		public Font GetFont(string name)
		{
			var props = GetDisplayProps(name);
			return (props == null ? FontHelper.UIFont : props.Font);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsRightToLeft(string name)
		{
			var props = GetDisplayProps(name);
			return (props != null && props.RightToLeft);
		}

		/// ------------------------------------------------------------------------------------
		public int GetWidthInGrid(string name)
		{
			var props = GetDisplayProps(name);
			return (props == null ? PaFieldDisplayProperties.kDefaultWidthInGrid : props.WidthInGrid);
		}

		/// ------------------------------------------------------------------------------------
		public int GetIndexInGrid(string name)
		{
			var props = GetDisplayProps(name);
			return (props == null ? 999 : props.DisplayIndexInGrid);
		}

        /// ------------------------------------------------------------------------------------
        public string GetNote(string name)
        {
            var props = GetDisplayProps(name);
            return (props == null ? "P" : props.Note);
        }

		/// ------------------------------------------------------------------------------------
		public int GetIndexInRecView(string name)
		{
			var props = GetDisplayProps(name);
			return (props == null ? 999 : props.DisplayIndexInRecView);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsVisibleInGrid(string name)
		{
			var props = GetDisplayProps(name);
			return (props != null && props.VisibleInGrid);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsVisibleInRecView(string name)
		{
			var props = GetDisplayProps(name);
			return (props != null && props.VisibleInRecView);
		}

		#endregion

		#region setter methods
		/// ------------------------------------------------------------------------------------
		public bool SetFont(string name, Font fntNew)
		{
			var fnt = s_fontCache.SingleOrDefault(f => FontHelper.AreFontsSame(f, fntNew));
			if (fnt == null)
			{
				fnt = fntNew;
				s_fontCache.Add(fnt);
			}

			var props = GetDisplayProps(name);
			if (props == null)
			{
				Add(new PaFieldDisplayProperties(name, false, false) { Font = fnt });
				return true;
			}

			props.Font = fnt;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public bool SetIsRightToLeft(string name, bool isRightToLeft)
		{
			var props = GetDisplayProps(name);
			if (props == null)
			{
				Add(new PaFieldDisplayProperties(name, false, false) { RightToLeft = isRightToLeft });
				return true;
			}

			props.RightToLeft = isRightToLeft;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public bool SetWidthInGrid(string name, int width)
		{
			var props = GetDisplayProps(name);
			if (props == null)
			{
				Add(new PaFieldDisplayProperties(name, false, false) { WidthInGrid = width });
				return true;
			}

			props.WidthInGrid = width;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public bool SetIndexInGrid(string name, int index)
		{
			var props = GetDisplayProps(name);
			if (props == null)
			{
				Add(new PaFieldDisplayProperties(name, false, false) { DisplayIndexInGrid = index });
				return true;
			}

			props.DisplayIndexInGrid = index;
			return false;
		}

        /// ------------------------------------------------------------------------------------
        public bool SetNote(string name, string index)
        {
            var props = GetDisplayProps(name);
            if (props == null)
            {
                Add(new PaFieldDisplayProperties(name, false, false) { Note = index });
                return true;
            }

            props.Note = index;
            return false;
        }

		/// ------------------------------------------------------------------------------------
		public bool SetIndexInRecView(string name, int index)
		{
			var props = GetDisplayProps(name);
			if (props == null)
			{
				Add(new PaFieldDisplayProperties(name, false, false) { DisplayIndexInRecView = index });
				return true;
			}

			props.DisplayIndexInRecView = index;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public bool SetIsVisibleInGrid(string name, bool visible)
		{
			var props = GetDisplayProps(name);
			if (props == null)
			{
				Add(new PaFieldDisplayProperties(name, false, false) { VisibleInGrid = visible });
				return true;
			}
	
			props.VisibleInGrid = visible;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public bool SetIsVisibleInRecView(string name, bool visible)
		{
			var props = GetDisplayProps(name);
			if (props == null)
			{
				Add(new PaFieldDisplayProperties(name, false, false) { VisibleInRecView = visible });
				return true;
			}

			props.VisibleInRecView = visible;
			return false;
		}

		#endregion
	}

	#endregion
}
