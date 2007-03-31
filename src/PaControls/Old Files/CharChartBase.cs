using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;
using SIL.SpeechTools.Database;
using SIL.Pa.FFSearchEngine;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Provides a base class for a vowel, consonant, diacritic or suprasegmental charts.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class CharChartBase : UserControl
	{
		protected List<XButton> m_chars;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharChartBase()
		{
			InitializeComponent();
			DoubleBuffered = true;

			if (DBUtils.IPACharCache == null)
				return;

			m_chars = new List<XButton>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a chart view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Initialize(IPACharacterType charType)
		{
			foreach (Control ctrl in Controls)
			{
				if (ctrl.Name.StartsWith("lbl"))
					ctrl.Font = FontHelper.UIFont;
			}


			if (DBUtils.IPACharCache == null)
				return;

			foreach (IPACharInfo charInfo in DBUtils.IPACharCache.Values)
			{
				if (charInfo.CharType != charType)
					continue;

				string ctrlName = string.Format("chr{0}", charInfo.DisplayOrder - 1);
				if (Controls[ctrlName] != null)
				{
					m_chars.Add(Controls[ctrlName] as XButton);
					Controls[ctrlName].Text = charInfo.IPAChar;
					Controls[ctrlName].Click += new EventHandler(HandleCharClick);
					Controls[ctrlName].DoubleClick += new EventHandler(HandleCharDoubleClick);
				}
			}

			UpdateFonts();
			UpdateChars();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the font of the characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateFonts()
		{
			foreach (XButton btn in m_chars)
				btn.Font = new Font(FontHelper.PhoneticFont.FontFamily, 16, GraphicsUnit.Point);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the enabled state of the characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateChars()
		{
			if (PaApp.PhoneCache == null)
				return;

			foreach (XButton btn in m_chars)
				btn.Enabled = PaApp.PhoneCache.ContainsKey(btn.Text);
		}
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the current character
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string CurrentPhone
		{
			get
			{
				foreach (XButton btn in m_chars)
				{
					if (btn.Checked)
						return btn.Text;
				}

				return null;
			}
			set
			{
				foreach (XButton btn in m_chars)
				{
					if (btn.Text == value)
					{
						btn.Checked = true;
						HandleCharClick(btn, null);
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCharDoubleClick(object sender, EventArgs e)
		{
			XButton clickedButton = sender as XButton;

			if (clickedButton != null)
			{
				SearchQuery query = new SearchQuery();
				query.Pattern = clickedButton.Text + "/*_*";
				PaApp.MsgMediator.SendMessage("ViewFindPhones", query);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Uncheck all the buttons except the one clicked on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCharClick(object sender, EventArgs e)
		{
			XButton clickedButton = sender as XButton;
			foreach (XButton btn in m_chars)
				btn.Checked = (clickedButton == btn);
		}
	}
}
