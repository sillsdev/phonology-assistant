using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Controls;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;
using SIL.Pa.FFSearchEngine;
using SIL.FieldWorks.Common.UIAdapters;
using XCore;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class VowelChartWnd : ChartWndBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public VowelChartWnd() : base()
		{
			InitializeComponent();
			Name = "vowelChartWnd";
			m_defaultHTMLOutputFile = Properties.Resources.kstidVowChartHTMLFileName;
			m_htmlChartName = Properties.Resources.kstidVowChartHTMLChartName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Derived classes must override this.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override IPACharacterType CharacterType
		{
			get { return IPACharacterType.Vowel; }
		}
	}
}