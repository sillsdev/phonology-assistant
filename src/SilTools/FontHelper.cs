using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SilTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a font object that can be serialized.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("Font")]
	public class SerializableFont
	{
		[XmlAttribute]
		public string Name ;
		[XmlAttribute]
		public float Size = 10;
		[XmlAttribute]
		public bool Bold;
		[XmlAttribute]
		public bool Italic;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SerializableFont()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Intializes a new Serializable font object from the specified font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SerializableFont(Font fnt)
		{
			Font = fnt;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a font object based on the SerializableFont's settings or sets the
		/// SerializableFont's settings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Font Font
		{
			get
			{
				if (Name == null)
					return null;

				FontStyle style = FontStyle.Regular;
				
				if (Bold)
					style = FontStyle.Bold;

				if (Italic)
					style |= FontStyle.Italic;

				return FontHelper.MakeFont(Name, (int)Size, style);
			}
			set
			{
				if (value == null)
					Name = null;
				else
				{
					Name = value.Name;
					Size = value.SizeInPoints;
					Bold = value.Bold;
					Italic = value.Italic;
				}
			}
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Misc. font helper methods.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public static class FontHelper
	{
		/// --------------------------------------------------------------------------------
		static FontHelper()
		{
			ResetFonts();
		}

		/// ------------------------------------------------------------------------------------
		public static void ResetFonts()
		{
			// These should get intialized via the settings file, but in case there is no
			// settings file, this will ensure at least something.
			if (FontInstalled("Doulos SIL"))
				PhoneticFont = MakeFont("Doulos SIL", 13, FontStyle.Regular);
			else if (FontInstalled("Arial Unicode"))
				PhoneticFont = MakeFont("Arial Unicode", 11, FontStyle.Regular);
			else if (FontInstalled("Lucida Sans Unicode"))
				PhoneticFont = MakeFont("Lucida Sans Unicode", 11, FontStyle.Regular);
			else
				PhoneticFont = (Font)SystemInformation.MenuFont.Clone();

			DefaultPhoneticFont = (Font)PhoneticFont.Clone();
			PhonemicFont = (Font)PhoneticFont.Clone();
			ToneFont = (Font)SystemInformation.MenuFont.Clone();
			OrthograpicFont = (Font)SystemInformation.MenuFont.Clone();
			GlossFont = (Font)SystemInformation.MenuFont.Clone();
			POSFont = (Font)SystemInformation.MenuFont.Clone();
			ReferenceFont = (Font)SystemInformation.MenuFont.Clone();
			UIFont = (Font)SystemInformation.MenuFont.Clone();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines if the specified font is installed on the computer.
		/// </summary>
		/// <param name="fontName"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static bool FontInstalled(string fontName)
		{
			fontName = fontName.ToLower();

			using (InstalledFontCollection installedFonts = new InstalledFontCollection())
			{
				foreach (FontFamily family in installedFonts.Families)
				{
					if (family.Name.ToLower() == fontName)
						return true;
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a font object that is a phonetic font derivative with the specified size
		/// and a regular style.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Font MakeEticRegFontDerivative(float size)
		{
			return MakeFont(PhoneticFont.FontFamily.Name, size, FontStyle.Regular);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a string containing three pieces of information about the specified font:
		/// the name, size and style.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FontToString(Font fnt)
		{
			if (fnt == null)
				return null;

			return fnt.Name + ", " + fnt.SizeInPoints + ", " + fnt.Style;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a font object with the specified properties. If an error occurs while
		/// making the font (e.g. because the font doesn't support a particular style) a
		/// fallback scheme is used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Font MakeFont(string fontString)
		{
			if (fontString == null)
				return SystemFonts.DefaultFont;

			string name = SystemFonts.DefaultFont.FontFamily.Name;
			float size = SystemFonts.DefaultFont.SizeInPoints;
			FontStyle style = FontStyle.Regular;

			string[] parts = fontString.Split(',');
			if (parts.Length > 0)
				name = parts[0];

			if (parts.Length > 1)
				float.TryParse(parts[1], out size);

			if (parts.Length > 2)
			{
				try
				{
					style = (FontStyle)Enum.Parse(typeof(FontStyle), parts[2]);
				}
				catch { }
			}

			return MakeFont(name, size, style);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a font object with the specified properties. If an error occurs while
		/// making the font (e.g. because the font doesn't support a particular style) a
		/// fallback scheme is used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Font MakeFont(Font fnt, float size, FontStyle style)
		{
			return MakeFont(fnt.FontFamily.Name, size, style);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a font object with the specified properties. If an error occurs while
		/// making the font (e.g. because the font doesn't support a particular style) a
		/// fallback scheme is used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Font MakeFont(Font fnt, float size)
		{
			return MakeFont(fnt.FontFamily.Name, size, fnt.Style);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a font object with the specified properties. If an error occurs while
		/// making the font (e.g. because the font doesn't support a particular style) a
		/// fallback scheme is used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Font MakeFont(Font fnt, FontStyle style)
		{
			return MakeFont(fnt.FontFamily.Name, fnt.SizeInPoints, style);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a font object with the specified properties. If an error occurs while
		/// making the font (e.g. because the font doesn't support a particular style) a
		/// fallback scheme is used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Font MakeFont(string family, float size, FontStyle style)
		{
		    Font fnt;

			while (true)
			{
				try
				{
					fnt = new Font(family, size, style, GraphicsUnit.Point);
					break;
				}
				catch (Exception e)
				{
					string msg = e.Message.ToLower();
					if (!msg.Contains("does not support style"))
						return (Font)SystemFonts.IconTitleFont.Clone();
					
					if (msg.Contains("bold"))
						style &= ~FontStyle.Bold;
					else if (msg.Contains("italic"))
						style &= ~FontStyle.Italic;
					else if (msg.Contains("regular"))
						style = FontStyle.Bold;
				}
			}

			return fnt;
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Compares the font face name, size and style of two fonts.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static bool AreFontsSame(Font x, Font y)
		{
			if (x == null || y == null)
				return false;

			return (x.Name == y.Name && x.Size == y.Size && x.Style == y.Style);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the default font for displaying phonetic data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font DefaultPhoneticFont { get; private set; }

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying phonetic data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font PhoneticFont { get; set; }

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying tone data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font ToneFont { get; set; }

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying phonemic data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font PhonemicFont { get; set; }

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying orthographic data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font OrthograpicFont { get; set; }

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying gloss data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font GlossFont { get; set; }

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying part of speech data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font POSFont { get; set; }

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying reference data fields.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font ReferenceFont { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the desired font for most UI elements.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Font UIFont { get; set; }
	}
}
