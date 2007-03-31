using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using MCI;
using AxMCI;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for RecordDlg.
	/// </summary>
	public class RecordDlg : System.Windows.Forms.Form
	{
		private AxMMControl MCIRec;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private string m_filename;
		private long m_running;

		public RecordDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			m_filename = PaApp.AppPath + "test.wav";
			MCIRec.FileName = m_filename;
			MCIRec.TimeFormat = (int)FormatConstants.mciFormatMilliseconds;
			MCIRec.Command = "Open";
			m_running = 0;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			MCIRec.Command = "Save";
			MCIRec.Command = "Close";
			MCIRec = null;
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RecordDlg));
			this.MCIRec = new AxMCI.AxMMControl();
			((System.ComponentModel.ISupportInitialize)(this.MCIRec)).BeginInit();
			this.SuspendLayout();
			// 
			// MCIRec
			// 
			this.MCIRec.Enabled = true;
			this.MCIRec.Location = new System.Drawing.Point(24, 40);
			this.MCIRec.Name = "MCIRec";
			this.MCIRec.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MCIRec.OcxState")));
			this.MCIRec.Size = new System.Drawing.Size(106, 22);
			this.MCIRec.TabIndex = 0;
			this.MCIRec.Done += new AxMCI.DmciEvents_DoneEventHandler(this.MCIRec_Done);
			this.MCIRec.StatusUpdate += new System.EventHandler(this.MCIRec_StatusUpdate);
			this.MCIRec.PlayClick += new AxMCI.DmciEvents_PlayClickEventHandler(this.MCIRec_PlayClick);
			this.MCIRec.PauseClick += new AxMCI.DmciEvents_PauseClickEventHandler(this.MCIRec_PauseClick);
			this.MCIRec.StopClick += new AxMCI.DmciEvents_StopClickEventHandler(this.MCIRec_StopClick);
			this.MCIRec.RecordClick += new AxMCI.DmciEvents_RecordClickEventHandler(this.MCIRec_RecordClick);
			// 
			// RecordDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.MCIRec);
			this.Name = "RecordDlg";
			this.ShowInTaskbar = false;
			this.Text = "RecordDlg";
			((System.ComponentModel.ISupportInitialize)(this.MCIRec)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void MCIRec_PlayClick(object sender, DmciEvents_PlayClickEvent e)
		{
			MCIRec.Wait = false;
			MCIRec.Notify = true;
			MCIRec.PlayEnabled = false;
			MCIRec.RecordEnabled = false;
			MCIRec.StopEnabled = true;
		}

		private void MCIRec_RecordClick(object sender, DmciEvents_RecordClickEvent e)
		{
			MCIRec.Wait = false;
			MCIRec.Notify = true;
			MCIRec.PlayEnabled = false;
			MCIRec.PauseEnabled = true;
			MCIRec.StopEnabled = true;
			MCIRec.RecordEnabled = false;
		}

		private void MCIRec_StopClick(object sender, DmciEvents_StopClickEvent e)
		{
			MCIRec.StopEnabled = false;
			MCIRec.PlayEnabled = true;
			MCIRec.RecordEnabled = true;
			MCIRec.PauseEnabled = false;
			m_running = 0;
		}

		private void MCIRec_StatusUpdate(object sender, System.EventArgs e)
		{
			if ((MCIRec.Mode == (int)ModeConstants.mciModePlay) ||
				(MCIRec.Mode == (int)ModeConstants.mciModeRecord))
			{
				m_running += 100;
				Text = "RecordDlg: " + m_running + " ms";
			}
			else if (MCIRec.Mode == (int)ModeConstants.mciModePause)
			{
				Text = "RecordDlg: " + MCIRec.TrackPosition + " ms";
			}
			else if (MCIRec.Mode == (int)ModeConstants.mciModeStop)
			{
				Text = "RecordDlg: " + MCIRec.TrackLength + " ms";
			}
		}

		private void MCIRec_Done(object sender, AxMCI.DmciEvents_DoneEvent e)
		{
			if (e.notifyCode != (short)NotifyConstants.mciAborted)
			{
				MCIRec.Command = "Stop";
				m_running = 0;
			}
			MCIRec.Command = "Prev";
		}

		private void MCIRec_PauseClick(object sender, AxMCI.DmciEvents_PauseClickEvent e)
		{
			MCIRec.Notify = false;
		}
	}
}
