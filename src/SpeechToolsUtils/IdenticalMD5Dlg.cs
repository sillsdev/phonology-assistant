using System;
using System.Windows.Forms;

namespace SIL.SpeechTools.Utils
{
	public partial class IdenticalMD5Dlg : Form
	{
        private bool m_updatePath = false;
		private bool m_copyRecord = false;
        private bool m_convert = false;

		public IdenticalMD5Dlg()
		{
			InitializeComponent();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			m_updatePath = rbUpdatePath.Checked;
			m_copyRecord = rbCopy.Checked;
            m_convert = rbConvert.Checked;

			this.Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the message displayed on the dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Message
		{
			get { return (txtMessage.Text); }

			set 
			{
				txtMessage.Text = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the user's choice of whether to update the path in the existing record.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool UpdatePath
		{
			get { return (m_updatePath); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the user's choice of whether to copy the existing record.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CopyRecord
		{
			get { return (m_copyRecord); }
		}

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the user's choice of whether to create a new blank record (or convert
        /// a legacy transcription if available).
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public bool Convert
        {
            get { return (m_convert); }
        }
	}
}