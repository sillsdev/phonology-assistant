using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SilUtils;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class RtfHelper
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font table portion of an RTF string. A list of PA fields and their font
		/// numbers is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FontTable(Dictionary<string, int> fontNumbers, ref int uiFontNumber)
		{
			if (fontNumbers == null)
				fontNumbers = new Dictionary<string, int>();
			else
				fontNumbers.Clear();

			PaFieldInfoList fields = (PaApp.Project != null ?
				PaApp.Project.FieldInfo : PaApp.FieldInfo);
			
			string fontFmt = "{{\\f{0}\\fnil {1};}}";

			// Save the font information that will be written to the RTF.
			StringBuilder bldr = new StringBuilder();
			bldr.AppendLine("{\\fonttbl");
			for (int i = 0; i < fields.Count; i++)
			{
				if (fields[i].Font != null)
				{
					bldr.AppendLine(string.Format(fontFmt, i, fields[i].Font.Name));
					fontNumbers[fields[i].FieldName] = i;
				}
			}

			// Now write the UI font to the font table.
			uiFontNumber = fields.Count;
			bldr.AppendFormat(fontFmt, uiFontNumber, FontHelper.UIFont.Name);

			return bldr.Append("}").ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font table portion of an RTF string for the specified fonts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FontTable(Font[] fonts)
		{
			string fontFmt = "{{\\f{0}\\fnil {1};}}";

			// Save the font information that will be written to the RTF.
			StringBuilder bldr = new StringBuilder();
			bldr.AppendLine("{\\fonttbl");
			for (int i = 0; i < fonts.Length; i++)
				bldr.AppendLine(string.Format(fontFmt, i, fonts[i].Name));

			return bldr.Append("}").ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds an RTF color table. The hash table returned is keyed on color values
		/// converted to int via the ToArgb() method. The value in each table entry is the
		/// RTF color reference number (which always starts at one).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ColorTable(Color color, out Dictionary<int, int> colorReferences)
		{
			return ColorTable(new List<Color>(new Color[] {color}), out colorReferences);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds an RTF color table. The hash table returned is keyed on color values
		/// converted to int via the ToArgb() method. The value in each table entry is the
		/// RTF color reference number (which always starts at one).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ColorTable(List<Color> colors,
			out Dictionary<int, int> colorReferences)
		{
			colorReferences = null;
			if (colors == null || colors.Count == 0)
				return string.Empty;

			colorReferences = new Dictionary<int,int>();
			StringBuilder bldr = new StringBuilder("{\\colortbl ;");
			
			for (int i = 0; i < colors.Count; i++)
			{
				colorReferences[colors[i].ToArgb()] = i + 1;
				bldr.AppendFormat("\\red{0}\\green{1}\\blue{2};",
					colors[i].R, colors[i].G, colors[i].B);
			}

			return bldr + "}";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Translates all unicode characters in the specified string to the form \uddd where
		/// ddd is the decimal Unicode value of the character. The question mark after the
		/// \uddd is an RTF requirement. Normally, it is the closest ANSI character equivalent
		/// to the Unicode. In my case, I'm not going to the trouble to figure it out.
		/// Therefore, the unknown '?' will have to suffice.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string TranslateUnicodeChars(string rtf)
		{
			string unicodeMarkup = "\\u{0}?";
			StringBuilder newRtf = new StringBuilder();

			foreach (char c in rtf)
			{
				int codepoint = Convert.ToInt32(c);
				if (codepoint < 128)
					newRtf.Append(c);
				else
					newRtf.Append(string.Format(unicodeMarkup, codepoint));
			}

			return newRtf.ToString();
		}
	}
}
