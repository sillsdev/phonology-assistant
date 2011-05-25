using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SIL.Pa.UI.Controls
{
	public partial class CheckedGroupBox : UserControl
	{
		public event EventHandler CheckedChanged;
		private Padding _padding = new Padding(0);

		/// ------------------------------------------------------------------------------------
		public CheckedGroupBox()
		{
			InitializeComponent();

			_checkBox.CheckedChanged += delegate
			{
				if (CheckedChanged != null)
					CheckedChanged(this, EventArgs.Empty);
			};
		}

		/// ------------------------------------------------------------------------------------
		public override Font Font
		{
			get { return base.Font; }
			set { base.Font = _checkBox.Font = _groupBox.Font = value; }
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		public override string Text
		{
			get { return base.Text; }
			set { base.Text = _checkBox.Text = value; }
		}

		/// ------------------------------------------------------------------------------------
		public new Padding Padding
		{
			get { return _padding; }
			set { _padding = _groupBox.Padding = value; }
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Checked
		{
			get { return _checkBox.Checked; }
			set { _checkBox.Checked = value; }
		}
	}
}
