using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	public partial class PlaybackSpeedAdjuster : UserControl
	{
		private string m_valueFmt;
		private Font m_font;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PlaybackSpeedAdjuster()
		{
			InitializeComponent();

			m_valueFmt = lblValue.Text;
			trkSpeed_ValueChanged(null, null);
			lblValue.Font = FontHelper.UIFont;
			lnkPlay.Font = FontHelper.UIFont;

			m_font = Font;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Correct the changing of fonts for the percent labels. The font that's used in
		/// designer is the one that should be used because there isn't much room for change.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// For some reason the labels get their font changed while the control is
			// being constructed. I expect it's because it's being added to a tool strip
			// menu/toolbar item which automatically take on the system's menu font. That
			// probably is inherited by hosted control items too.
			lblZero.Font = m_font;
			lbl25.Font = m_font;
			lbl50.Font = m_font;
			lbl75.Font = m_font;
			lbl100.Font = m_font;
			lbl125.Font = m_font;
			lbl150.Font = m_font;
			lbl175.Font = m_font;
			lbl200.Font = m_font;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void trkSpeed_ValueChanged(object sender, EventArgs e)
		{
			lblValue.Text = string.Format(m_valueFmt, trkSpeed.Value);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the current playback speed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int PlaybackSpeed
		{
			get { return trkSpeed.Value; }
			set
			{
				if (value <= trkSpeed.Maximum && value >= trkSpeed.Minimum)
					trkSpeed.Value = value;
			}
		}
	}
}
