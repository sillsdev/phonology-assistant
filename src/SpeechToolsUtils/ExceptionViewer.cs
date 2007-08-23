using System;
using System.Windows.Forms;

namespace SIL.SpeechTools.Utils
{
	public partial class ExceptionViewer : Form
	{
		public ExceptionViewer()
		{
			InitializeComponent();
		}

		public ExceptionViewer(Exception e) : this()
		{
			Exception = e;
		}

		public Exception Exception
		{
			set
			{
				if (value == null)
					return;

				txtOuterMsg.Text = value.Message;
				txtOuterStackTrace.Text = value.StackTrace;

				if (value.InnerException == null)
					tabControl1.TabPages.Remove(tpgInner);
				else
				{
					txtInnerMsg.Text = value.InnerException.Message;
					txtInnerStackTrace.Text = value.InnerException.StackTrace;
				}
			}
		}
	}
}