using System;
using System.Net;
using System.Windows.Forms;
using System.Globalization;

namespace SIL.Localize.LocalizingUtils
{
	#region GoogleTranslator class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class GoogleTranslator : IDisposable
	{
		private const string kUrlFmt = "http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}";
		protected WebClient m_webClient = null;
		protected string m_srcCultureId = null;
		protected string m_tgtCultureId = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static GoogleTranslator Create(string tgtCultureId)
		{
			return Create("en", tgtCultureId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static GoogleTranslator Create(string srcCultureId, string tgtCultureId)
		{
			GoogleTranslator translator = new GoogleTranslator();
			translator.m_webClient = new WebClient();
			translator.m_webClient.Encoding = System.Text.Encoding.UTF8;

			try
			{
				translator.m_webClient.DownloadString("http://translate.google.com/translate_t?hl=en#");
			}
			catch
			{
				// TODO: Internationalize
				string msg = "The Google translation site cannot be accessed. Check that you are connected to the internet.";
				MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				translator.Dispose();
				translator = null;
			}

			if (translator != null)
			{
				// Google can't handle minor cultures (or whatever they're called).
				int i = srcCultureId.IndexOf('-');
				if (i >= 0)
					srcCultureId = srcCultureId.Substring(0, i);

				i = tgtCultureId.IndexOf('-');
				if (i >= 0)
					tgtCultureId = tgtCultureId.Substring(0, i);

				translator.m_srcCultureId = srcCultureId;
				translator.m_tgtCultureId = tgtCultureId;
			}

			return translator;
		}

		#region IDisposable Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (m_webClient != null)
				m_webClient.Dispose();

			m_webClient = null;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string TranslateText(string srcText)
		{
			string langPair = m_srcCultureId + "|" + m_tgtCultureId;
			string url = String.Format(kUrlFmt, srcText, langPair);
			string result = null;

			try
			{
				m_webClient.Encoding = System.Text.Encoding.UTF7;
				result = m_webClient.DownloadString(url);
			}
			catch
			{
				return null;
			}

			if (result == null)
				return null;

			int start = result.IndexOf("id=result_box");
			if (start < 0)
				return null;

			start = result.IndexOf(">", start);
			if (start < 0)
				return null;

			start++;
			int end = result.IndexOf("</div>", start);
			if (end < 0)
				return null;

			result = result.Substring(start, end - start);
			result = ConvertNumbers(result, "&#x");
			return ConvertNumbers(result, "&#");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string ConvertNumbers(string text, string xmlNumSpec)
		{
			NumberStyles numStyle = (xmlNumSpec.EndsWith("x") ?
				NumberStyles.HexNumber : NumberStyles.Integer);

			while (true)
			{
				int start = text.IndexOf(xmlNumSpec);
				if (start < 0)
					break;

				start += xmlNumSpec.Length;
				int end = text.IndexOf(";", start);
				if (end < 0)
					break;

				string num = text.Substring(start, end - start);
				int chr;
				if (int.TryParse(num, numStyle, null, out chr))
					text = text.Replace(xmlNumSpec + num + ";", ((char)chr).ToString());
			}

			return text;
		}
	}

	#endregion
}