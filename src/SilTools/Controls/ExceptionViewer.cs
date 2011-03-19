using System;
using System.Windows.Forms;

namespace SilTools
{
	public partial class ExceptionViewer : Form
	{
		/// ------------------------------------------------------------------------------------
		public static void Show(Exception e)
		{
			var exVwr = new ExceptionViewer(e);
			exVwr.ShowDialog();
		}

		/// ------------------------------------------------------------------------------------
		public ExceptionViewer()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		public ExceptionViewer(Exception e)
			: this()
		{
			Exception = e;
		}

		/// ------------------------------------------------------------------------------------
		public Exception Exception
		{
			set
			{
				if (value == null)
					return;

				txtOuterMsg.Text = value.Message;
				txtOuterStackTrace.Text = value.StackTrace;

				int i = 1;
				var e = value;
				while (e.InnerException != null)
				{
					LoadInnerTab(string.Format("Inner Exception ({0})", i++), e.InnerException);
					e = e.InnerException;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void LoadInnerTab(string tabText, Exception e)
		{
			var tpage = new TabPage(tabText)
			{
				Padding = tpgOuter.Padding,
				BackColor = tpgOuter.BackColor,
				UseVisualStyleBackColor = true,
			};
			
			var split = new SplitContainer
			{
				Orientation = Orientation.Horizontal,
				Dock = DockStyle.Fill,
				BackColor = splitOuter.BackColor
			};

			var txtMsg = GetMsgTextBox(e.Message);
			var txtTrace = GetMsgTextBox(e.StackTrace);

			split.Panel1.Controls.Add(txtMsg);
			split.Panel2.Controls.Add(txtTrace);
			tpage.Controls.Add(split);
			tabControl1.Controls.Add(tpage);
		}

		/// ------------------------------------------------------------------------------------
		private TextBox GetMsgTextBox(string text)
		{
			return new TextBox
			{
				Text = text,
				Dock = DockStyle.Fill,
				ReadOnly = true,
				BackColor = txtOuterMsg.BackColor,
				Multiline = true,
			};
		}
	}
}