using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using SilTools;

namespace SIL.Pa.UI.Controls
{
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

			const string fontFmt = "{{\\f{0}\\fnil {1};}}";

			// Save the font information that will be written to the RTF.
			var bldr = new StringBuilder();
			int i = 0;
			bldr.AppendLine("{\\fonttbl");
			foreach (var field in App.Project.Fields.Where(f => f.Font != null))
			{
				bldr.AppendLine(string.Format(fontFmt, i, field.Font.Name.ToString(CultureInfo.InvariantCulture)));
				fontNumbers[field.Name] = i++;
			}

			// Now write the UI font to the font table.
			bldr.AppendFormat(fontFmt, i, FontHelper.UIFont.Name.ToString(CultureInfo.InvariantCulture));

			return bldr.Append("}").ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font table portion of an RTF string for the specified fonts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FontTable(Font[] fonts)
		{
			const string fontFmt = "{{\\f{0}\\fnil {1};}}";

			// Save the font information that will be written to the RTF.
			var bldr = new StringBuilder();
			bldr.AppendLine("{\\fonttbl");
			for (int i = 0; i < fonts.Length; i++)
				bldr.AppendLine(string.Format(fontFmt, i, fonts[i].Name.ToString(CultureInfo.InvariantCulture)));

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
			return ColorTable(new List<Color>(new[] {color}), out colorReferences);
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
			var bldr = new StringBuilder("{\\colortbl ;");
			
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
			const string unicodeMarkup = "\\u{0}?";
			var newRtf = new StringBuilder();

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
