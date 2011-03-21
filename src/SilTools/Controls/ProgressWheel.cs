using System;
using System.Drawing;
using System.Windows.Forms;

namespace SilTools.Controls
{
	/// ----------------------------------------------------------------------------------------
	public interface IProgressControl
	{
		int Maximum { get; set; }
		int Value { get; set; }
		string Message { get; set; }
		//bool ShowPercent { get; set; }
		void Increment(int value);
	}

	/// ----------------------------------------------------------------------------------------
	public partial class ProgressWheel : UserControl, IProgressControl
	{
		private int m_value;

		/// ------------------------------------------------------------------------------------
		public ProgressWheel()
		{
			InitializeComponent();
			BackColor = Color.Transparent;
			lblMessage.Font = FontHelper.MakeFont(FontHelper.UIFont, 9, FontStyle.Bold);
			Maximum = 100;
			Value = 0;
		}

		/// ------------------------------------------------------------------------------------
		public int Maximum { get; set; }

		/// ------------------------------------------------------------------------------------
		public int Value
		{
			get { return m_value; }
			set
			{
				m_value = Math.Min(value, Maximum);
				lblPercent.Text = string.Format("{0}%", (value / Maximum) * 100);
			}
		}

		/// ------------------------------------------------------------------------------------
		public string Message
		{
			get { return lblMessage.Text; }
			set { lblMessage.Text = value; }
		}

		///// ------------------------------------------------------------------------------------
		//public bool ShowPercent
		//{
		//    get { return m_showPercent; }
		//    set 
		//    {
		//        m_showPercent = value;
		//        lblPercent.Visible = value;
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		public void Increment(int value)
		{
			Value++;
		}
	}
}
