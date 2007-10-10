using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SIL.Pa.AddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class RatingSurveyCtrl : UserControl
	{
		private ToolTip m_infoTip;
		private string m_infoText;
		private RadioButton[] m_rbChoices;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RatingSurveyCtrl()
		{
			DoubleBuffered = true;
			InitializeComponent();
			m_rbChoices = new RadioButton[] { rb1, rb2, rb3, rb4, rb5 };
			Icon infoIcon = new Icon(Properties.Resources.kicoItemInformation, 16, 16);
			picInfo.Image = infoIcon.ToBitmap();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			lblItem.Size = new Size(Width - (pnlRatings.Width + 1), Height);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Clear()
		{
			foreach (RadioButton rb in m_rbChoices)
				rb.Checked = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		public override string Text
		{
			get	{return lblItem.Text;}
			set	{lblItem.Text = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		public string InfoText
		{
			get { return m_infoText; }
			set
			{
				m_infoText = value;

				if (string.IsNullOrEmpty(value))
				{
					if (m_infoTip != null)
					{
						m_infoTip.Dispose();
						m_infoTip = null;
						picInfo.Visible = false;
					}
				}
				else
				{
					m_infoTip = new ToolTip();
					m_infoTip.ToolTipTitle = Properties.Resources.kstidSurveyItemInfoTitle;
					m_infoTip.ToolTipIcon = ToolTipIcon.Info;
					picInfo.Visible = true;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public int Rating
		{
			get
			{
				for (int i = 0; i < m_rbChoices.Length; i++)
				{
					if (m_rbChoices[i].Checked)
						return i + 1;
				}

				return 0;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the screen location of the radio button for the specified choice.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Point GetChoiceLocation(int choice)
		{
			return (choice < 0 || choice >= m_rbChoices.Length ? Point.Empty :
				m_rbChoices[choice].PointToScreen(new Point(0, 0)));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void picInfo_MouseEnter(object sender, EventArgs e)
		{
			if (m_infoTip != null && !string.IsNullOrEmpty(m_infoText))
			{
				Point pt = picInfo.PointToClient(MousePosition);
				string tip = SIL.SpeechTools.Utils.STUtils.ConvertLiteralNewLines(m_infoText);
				m_infoTip.Show(tip, picInfo, pt.X, picInfo.Height + 3);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void picInfo_MouseLeave(object sender, EventArgs e)
		{
			if (m_infoTip != null)
				m_infoTip.Hide(picInfo);
		}
	}
}
