using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A customized label control for various uses within PA.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaLabel : Label
	{
		private ToolTip m_toolTip = null;
		private string m_toolTipText = null;
		private bool m_alwaysShowToolTip = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaLabel()
		{
			Font = FontHelper.UIFont;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the tooltip used to show the entire label text when it's clipped
		/// because the control is too small.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ToolTip ToolTip
		{
			get {return m_toolTip;}
			set {m_toolTip = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the tooltip text. Set this property when you would like to display
		/// something other than what's in the label's Text property. Setting this value to
		/// null will cause the label's Text to be displayed in the tool tip.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ToolTipText
		{
			get {return m_toolTipText == null ? Text : m_toolTipText;}
			set {m_toolTipText = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the tooltip will be displayed even
		/// when the width of the label control is wide enough to display the entire Text
		/// in the tooltip. You may want to do this when specifying a ToolTipText that is
		/// different from the label's Text property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AlwaysShowToolTip
		{
			get {return m_alwaysShowToolTip;}
			set {m_alwaysShowToolTip = value;}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the full label contents when the visible width of the control isn't wide
		/// enough to show all the text or when AlwaysShowToolTip is set to true.
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			if (m_toolTip == null)
				return;

			m_toolTip.SetToolTip(this,
				(PreferredWidth > Width || m_alwaysShowToolTip ? ToolTipText : null));
		}
	}
}
