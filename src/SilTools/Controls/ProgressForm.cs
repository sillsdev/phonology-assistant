// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
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
