using System.Windows.Forms;

namespace SilTools.Controls
{
	public partial class ProgressForm : Form, IProgressControl
	{
		/// ------------------------------------------------------------------------------------
		public ProgressForm()
		{
			InitializeComponent();
		}

		#region IProgressControl Members
		/// ------------------------------------------------------------------------------------
		public int Maximum
		{
			get { return prgWheel.Maximum; }
			set { prgWheel.Maximum = value; }
		}

		/// ------------------------------------------------------------------------------------
		public int Value
		{
			get { return prgWheel.Value; }
			set { prgWheel.Value = value; }
		}

		/// ------------------------------------------------------------------------------------
		public string Message
		{
			get { return prgWheel.Message; }
			set { prgWheel.Message = value; }
		}

		///// ------------------------------------------------------------------------------------
		//public bool ShowPercent
		//{
		//    get { return prgWheel.ShowPercent; }
		//    set { prgWheel.ShowPercent = value; }
		//}

		/// ------------------------------------------------------------------------------------
		public void Increment(int value)
		{
			prgWheel.Increment(value);
		}

		#endregion
	}
}
