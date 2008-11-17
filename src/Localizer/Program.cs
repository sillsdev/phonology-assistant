using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;

namespace SIL.Localize.Localizer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainWnd());
		}

		#region Methods for XML serializing and deserializing data
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Serializes an object to the specified file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool SerializeData(string path, object data)
		{
			try
			{
				using (TextWriter writer = new StreamWriter(path))
				{
					XmlSerializerNamespaces nameSpace = new XmlSerializerNamespaces();
					nameSpace.Add(string.Empty, string.Empty);
					XmlSerializer serializer = new XmlSerializer(data.GetType());
					serializer.Serialize(writer, data, nameSpace);
					writer.Close();
				}

				return true;
			}
			catch { }

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializes data from the specified file to an object of the specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static object DeserializeData(string filename, Type type)
		{
			Exception e;
			return (DeserializeData(filename, type, out e));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializes data from the specified file to an object of the specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static object DeserializeData(string path, Type type, out Exception e)
		{
			object data;
			e = null;

			try
			{
				if (!File.Exists(path))
					return null;

				using (TextReader reader = new StreamReader(path))
				{
					XmlSerializer deserializer = new XmlSerializer(type);
					data = deserializer.Deserialize(reader);
					reader.Close();
				}
			}
			catch (Exception outEx)
			{
				data = null;
				e = outEx;
			}

			return data;
		}

		#endregion
	}
	
	#region SerializableFont class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a font object that can be serialized.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("Font")]
	public class SerializableFont : IDisposable
	{
		[XmlAttribute]
		public string Name = null;
		[XmlAttribute]
		public float Size = 10;
		[XmlAttribute]
		public bool Bold = false;
		[XmlAttribute]
		public bool Italic = false;

		private Font m_font = null;

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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (m_font != null)
				m_font.Dispose();
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
				if (m_font != null)
					return m_font;

				if (Name == null)
					return null;

				FontStyle style = FontStyle.Regular;

				if (Bold)
					style = FontStyle.Bold;

				if (Italic)
					style |= FontStyle.Italic;

				m_font = new Font(Name, (int)Size, style, GraphicsUnit.Point);
				return m_font;
			}
			set
			{
				if (m_font != null)
					m_font.Dispose();

				m_font = value;

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

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// --------------------------------------------------------------------------------
		public SerializableFont Clone()
		{
			return new SerializableFont(m_font.Clone() as Font);
		}
	}

	#endregion
}

