using System.ComponentModel;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class CIEOptionsDropDown : SearchOptionsDropDown
	{
		private CIEOptions m_cieOptions;
		private bool m_canceled = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEOptionsDropDown()
		{
			InitializeComponent();
			grpUncertainties.Visible = false;
			LayoutDropDown();
			CIEOptions = (PaApp.Project != null ? PaApp.Project.CIEOptions.Clone() : new CIEOptions());
			
			rbAfter.Font = FontHelper.UIFont;
			rbBefore.Font = FontHelper.UIFont;
			rbBoth.Font = FontHelper.UIFont;
			lnkApply.Font = FontHelper.UIFont;

			rbBoth.Left = rbAfter.Left = rbBefore.Left = chkTone.Left;

			lnkApply.Top = ClientSize.Height -
				((ClientSize.Height - grpLength.Bottom) / 2) - (lnkApply.Height / 2);

			lnkHelp.Top = lnkCancel.Top = lnkApply.Top;

			int rightMargin = ClientSize.Width - grpLength.Right;
			lnkHelp.Left = ClientRectangle.Right - (lnkHelp.Width + rightMargin);
			lnkCancel.Left = lnkHelp.Left - (lnkCancel.Width + 10);
			lnkApply.Left = lnkCancel.Left - (lnkApply.Width + 10);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEOptions CIEOptions
		{
			get
			{
				if (rbAfter.Checked)
					m_cieOptions.Type = CIEOptions.IdenticalType.After;
				else if (rbBefore.Checked)
					m_cieOptions.Type = CIEOptions.IdenticalType.Before;
				else
					m_cieOptions.Type = CIEOptions.IdenticalType.Both;

				m_cieOptions.SearchQuery = SearchQuery;
				return m_cieOptions;
			}
			set
			{
				m_cieOptions = value;
				m_canceled = false;

				if (m_cieOptions != null)
				{
					SearchQuery = m_cieOptions.SearchQuery;
					rbAfter.Checked = false;
					rbBefore.Checked = false;
					rbBoth.Checked = false;

					switch (m_cieOptions.Type)
					{
						case CIEOptions.IdenticalType.After: rbAfter.Checked = true; break;
						case CIEOptions.IdenticalType.Before: rbBefore.Checked = true; break;
						case CIEOptions.IdenticalType.Both: rbBoth.Checked = true; break;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool OptionsChanged
		{
			get
			{
				bool changed = false;

				switch (m_cieOptions.Type)
				{
					case CIEOptions.IdenticalType.After: changed = !rbAfter.Checked; break;
					case CIEOptions.IdenticalType.Before: changed = !rbBefore.Checked; break;
					case CIEOptions.IdenticalType.Both: changed = !rbBoth.Checked; break;
				}

				return (!m_canceled && (changed || base.OptionsChanged));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the cancel link was clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Canceled
		{
			get { return m_canceled; }
			set { m_canceled = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the drop-down gets hidden.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lnkApply_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lnkCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			m_canceled = true;
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			m_canceled = true;
			base.HandleHelpClicked(sender, e);
			PaApp.ShowHelpTopic("hidMinimalPairsOptions");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Treat the escape key like clicking cancel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape)
				m_canceled = true;

			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
