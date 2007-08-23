using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SIL.SpeechTools.Utils
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
		public string Name = null;
		[XmlAttribute]
		public float Size = 10;
		[XmlAttribute]
		public bool Bold = false;
		[XmlAttribute]
		public bool Italic = false;

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
		private static Font s_fntDefaultEtic;
		private static Font s_fntEtic;
		private static Font s_fntTone;
		private static Font s_fntEmic;
		private static Font s_fntOrtho;
		private static Font s_fntGloss;
		private static Font s_fntPOS;
		private static Font s_fntRef;
		private static Font s_fntUI;
		
		/// --------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// --------------------------------------------------------------------------------
		static FontHelper()
		{
			ResetFonts();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ResetFonts()
		{
			// These should get intialized via the settings file, but in case there is no
			// settings file, this will ensure at least something.
			if (FontInstalled("Doulos SIL"))
				s_fntEtic = MakeFont("Doulos SIL", 12, FontStyle.Regular);
			else if (FontInstalled("Arial Unicode"))
				s_fntEtic = MakeFont("Arial Unicode", 11, FontStyle.Regular);
			else if (FontInstalled("Lucida Sans Unicode"))
				s_fntEtic = MakeFont("Lucida Sans Unicode", 11, FontStyle.Regular);
			else
				s_fntEtic = (Font)SystemInformation.MenuFont.Clone();

			s_fntDefaultEtic = (Font)s_fntEtic.Clone();
			s_fntEmic = (Font)s_fntEtic.Clone();
			s_fntTone = (Font)SystemInformation.MenuFont.Clone();
			s_fntOrtho = (Font)SystemInformation.MenuFont.Clone();
			s_fntGloss = (Font)SystemInformation.MenuFont.Clone();
			s_fntPOS = (Font)SystemInformation.MenuFont.Clone();
			s_fntRef = (Font)SystemInformation.MenuFont.Clone();
			s_fntUI = (Font)SystemInformation.MenuFont.Clone();

			SpeechToolsSettingsHandler spSettingsHandler = new SpeechToolsSettingsHandler();
			spSettingsHandler.LoadFonts();
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
		/// Saves the font settings in the Speech Tools settings file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void SaveFonts()
		{
			SpeechToolsSettingsHandler spSettingsHandler = new SpeechToolsSettingsHandler();
			spSettingsHandler.SaveFonts();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a font object that is a phonetic font derivative with the specified size
		/// and a regular style.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Font MakeEticRegFontDerivative(float size)
		{
			return MakeFont(s_fntEtic.FontFamily.Name, size, FontStyle.Regular);
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
						return (Font)SystemInformation.MenuFont.Clone();
					else
					{
						if (msg.Contains("bold"))
							style &= ~FontStyle.Bold;
						else if (msg.Contains("italic"))
							style &= ~FontStyle.Italic;
						else if (msg.Contains("regular"))
							style = FontStyle.Bold;
					}
				}
			}

			return fnt;
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the default font for displaying phonetic data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font DefaultPhoneticFont
		{
			get { return s_fntDefaultEtic; }
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying phonetic data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font PhoneticFont
		{
			get { return s_fntEtic; }
			set { s_fntEtic = value; }
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying tone data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font ToneFont
		{
			get { return s_fntTone; }
			set { s_fntTone = value; }
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying phonemic data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font PhonemicFont
		{
			get { return s_fntEmic; }
			set { s_fntEmic = value; }
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying orthographic data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font OrthograpicFont
		{
			get { return s_fntOrtho; }
			set { s_fntOrtho = value; }
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying gloss data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font GlossFont
		{
			get { return s_fntGloss; }
			set { s_fntGloss = value; }
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying part of speech data.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font POSFont
		{
			get { return s_fntPOS; }
			set { s_fntPOS = value; }
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the font specified for displaying reference data fields.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Font ReferenceFont
		{
			get { return s_fntRef; }
			set { s_fntRef = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the desired font for most UI elements.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Font UIFont
		{
			get { return s_fntUI; }
			set { s_fntUI = value; }
		}
	}
}
