// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2003, SIL International. All Rights Reserved.   
// <copyright from='2003' to='2003' company='SIL International'>
//		Copyright (c) 2003, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: ProgressDialog.cs
// Responsibility: _Aman
// Last reviewed: 
// 
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIL.Pa
{
	/// ---------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for ProgressDialog.
	/// </summary>
	/// ---------------------------------------------------------------------------------------
	public class ProgressDialog : System.Windows.Forms.Form
	{
		private bool m_fReturnFocusToOwner;

		/// <summary>
		/// Delegate for trapping the cancel event.
		/// </summary>
		public delegate void CancelHandler(object sender);
	
		/// <summary>
		/// Event handler for listening to whether or the cancel button is pressed.
		/// </summary>
		public event CancelHandler Cancel;

		/// <summary>...uhh...the cancel button?...</summary>
		protected System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label lblStatusMessage;
		
		private long m_ticksSinceLastIncrement = 0;
		private long m_ticksStart;

		//This number must not be so large that you can't cancel, nor too small to slow down the
		// process if it gets call for lots of small steps. (A tick is 100 nanoseconds)
		private int m_appEventsIntervalTicks = 2000000; //don't change this lightly... it took me an hour

		/// -----------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ProgressDialog"/> class. Use this form
		/// of the constructor to create a progress bar from a dialog.
		/// This should only be called if the user can't mess things up by clicking on stuff 
		/// behind the dialog.
		/// </summary>
		/// -----------------------------------------------------------------------------------
		public ProgressDialog() : this(null)
		{
			StatusMessage = string.Empty;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Use this constructor to display the progress bar directly from a main window.
		/// </summary>
		/// <param name="owner">The main window that owns the progress bar</param>
		/// ------------------------------------------------------------------------------------
		public ProgressDialog(Form owner)
		{
			InitializeComponent();
			Owner = owner;
			
			if (Owner != null)
				owner.Enabled = false; // disable owner so user can't interact with owner
			
			ShowInTaskbar = (Owner == null);
			
			progressBar.Minimum = 0;
			progressBar.Value = 0;
			m_ticksStart = DateTime.Now.Ticks;
		}

		#region Windows Form Designer generated code
		/// -----------------------------------------------------------------------------------
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		/// -----------------------------------------------------------------------------------
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ProgressDialog));
			this.btnCancel = new System.Windows.Forms.Button();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.lblStatusMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.AccessibleDescription = resources.GetString("btnCancel.AccessibleDescription");
			this.btnCancel.AccessibleName = resources.GetString("btnCancel.AccessibleName");
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnCancel.Anchor")));
			this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
			this.btnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnCancel.Dock")));
			this.btnCancel.Enabled = ((bool)(resources.GetObject("btnCancel.Enabled")));
			this.btnCancel.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnCancel.FlatStyle")));
			this.btnCancel.Font = ((System.Drawing.Font)(resources.GetObject("btnCancel.Font")));
			this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
			this.btnCancel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnCancel.ImageAlign")));
			this.btnCancel.ImageIndex = ((int)(resources.GetObject("btnCancel.ImageIndex")));
			this.btnCancel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnCancel.ImeMode")));
			this.btnCancel.Location = ((System.Drawing.Point)(resources.GetObject("btnCancel.Location")));
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnCancel.RightToLeft")));
			this.btnCancel.Size = ((System.Drawing.Size)(resources.GetObject("btnCancel.Size")));
			this.btnCancel.TabIndex = ((int)(resources.GetObject("btnCancel.TabIndex")));
			this.btnCancel.Text = resources.GetString("btnCancel.Text");
			this.btnCancel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnCancel.TextAlign")));
			this.btnCancel.Visible = ((bool)(resources.GetObject("btnCancel.Visible")));
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// progressBar
			// 
			this.progressBar.AccessibleDescription = resources.GetString("progressBar.AccessibleDescription");
			this.progressBar.AccessibleName = resources.GetString("progressBar.AccessibleName");
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("progressBar.Anchor")));
			this.progressBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("progressBar.BackgroundImage")));
			this.progressBar.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("progressBar.Dock")));
			this.progressBar.Enabled = ((bool)(resources.GetObject("progressBar.Enabled")));
			this.progressBar.Font = ((System.Drawing.Font)(resources.GetObject("progressBar.Font")));
			this.progressBar.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("progressBar.ImeMode")));
			this.progressBar.Location = ((System.Drawing.Point)(resources.GetObject("progressBar.Location")));
			this.progressBar.Name = "progressBar";
			this.progressBar.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("progressBar.RightToLeft")));
			this.progressBar.Size = ((System.Drawing.Size)(resources.GetObject("progressBar.Size")));
			this.progressBar.TabIndex = ((int)(resources.GetObject("progressBar.TabIndex")));
			this.progressBar.Text = resources.GetString("progressBar.Text");
			this.progressBar.Visible = ((bool)(resources.GetObject("progressBar.Visible")));
			// 
			// lblStatusMessage
			// 
			this.lblStatusMessage.AccessibleDescription = resources.GetString("lblStatusMessage.AccessibleDescription");
			this.lblStatusMessage.AccessibleName = resources.GetString("lblStatusMessage.AccessibleName");
			this.lblStatusMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblStatusMessage.Anchor")));
			this.lblStatusMessage.AutoSize = ((bool)(resources.GetObject("lblStatusMessage.AutoSize")));
			this.lblStatusMessage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblStatusMessage.Dock")));
			this.lblStatusMessage.Enabled = ((bool)(resources.GetObject("lblStatusMessage.Enabled")));
			this.lblStatusMessage.Font = ((System.Drawing.Font)(resources.GetObject("lblStatusMessage.Font")));
			this.lblStatusMessage.Image = ((System.Drawing.Image)(resources.GetObject("lblStatusMessage.Image")));
			this.lblStatusMessage.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblStatusMessage.ImageAlign")));
			this.lblStatusMessage.ImageIndex = ((int)(resources.GetObject("lblStatusMessage.ImageIndex")));
			this.lblStatusMessage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblStatusMessage.ImeMode")));
			this.lblStatusMessage.Location = ((System.Drawing.Point)(resources.GetObject("lblStatusMessage.Location")));
			this.lblStatusMessage.Name = "lblStatusMessage";
			this.lblStatusMessage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblStatusMessage.RightToLeft")));
			this.lblStatusMessage.Size = ((System.Drawing.Size)(resources.GetObject("lblStatusMessage.Size")));
			this.lblStatusMessage.TabIndex = ((int)(resources.GetObject("lblStatusMessage.TabIndex")));
			this.lblStatusMessage.Text = resources.GetString("lblStatusMessage.Text");
			this.lblStatusMessage.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblStatusMessage.TextAlign")));
			this.lblStatusMessage.Visible = ((bool)(resources.GetObject("lblStatusMessage.Visible")));
			// 
			// ProgressDialog
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.CancelButton = this.btnCancel;
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.ControlBox = false;
			this.Controls.Add(this.lblStatusMessage);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.btnCancel);
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximizeBox = false;
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimizeBox = false;
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "ProgressDialog";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.ShowInTaskbar = false;
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.Closing += new System.ComponentModel.CancelEventHandler(this.ProgressDialog_Closing);
			this.Closed += new System.EventHandler(this.ProgressDialog_Closed);
			this.ResumeLayout(false);

		}
		#endregion
	
		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets/sets the time (in 100 nanosecond increments) that the progress dialog should be
		/// updated (default is 2000000 or about 0.2 seconds)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int EventIntervalTicks
		{
			get {return m_appEventsIntervalTicks;}
			set {m_appEventsIntervalTicks = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a message indicating progress status.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string StatusMessage
		{
			get {return lblStatusMessage.Text;}
			set
			{
				try
				{
					lblStatusMessage.Text = value;
					Refresh();
				}
				catch {}

				// Do this to allow the user to click on the cancel button.
				Application.DoEvents();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the maximum number of steps or increments corresponding to a progress
		/// bar that's 100% filled.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Maximum
		{
			get {return progressBar.Maximum;}
			set	{progressBar.Maximum = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating the number of steps (or increments) having been
		/// completed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Value
		{
			get {return progressBar.Value;}
			set
			{
				progressBar.Value = (value > progressBar.Maximum) ? progressBar.Maximum : value;
				//	UpdateRecord();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the cancel button is visible.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CancelButtonVisible
		{
			get {return btnCancel.Visible;}
			set {btnCancel.Visible = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the text on the cancel button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CancelButtonText
		{
			get {return btnCancel.Text;}
			set {btnCancel.Text = value;}
		}

		#endregion

		#region Misc. Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Increment the progress counter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Increment()
		{
			Value++;
			
			//Calling DoEvents was taking the majority of time for lexicon export
			if((DateTime.Now.Ticks - m_ticksSinceLastIncrement) > m_appEventsIntervalTicks)
			{
				// Do this to allow the user to click on the cancel button.
				Application.DoEvents();
				m_ticksSinceLastIncrement = DateTime.Now.Ticks;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If no one has subscribed to the cancel event, then don't bother showing the cancel
		/// button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			if (Visible)
				btnCancel.Visible = (Cancel != null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Occurs when the cancel button is pressed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected void btnCancel_Click(object sender, EventArgs e)
		{
			OnCancel();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calls subscribers to the cancel event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void OnCancel()
		{
			if (Cancel != null)
				Cancel(this);
		}
		
		#endregion

		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void ProgressDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Make sure the main form gets focus back if the progress dialog still has focus.
			m_fReturnFocusToOwner = ContainsFocus;

			if (Owner != null)
				Owner.Enabled = true;

			System.Diagnostics.Debug.WriteLine("Progress Dialog: Elapsed Time: "+ TimeSpan.FromTicks(DateTime.Now.Ticks - m_ticksStart).ToString());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void ProgressDialog_Closed(object sender, System.EventArgs e)
		{
			if (m_fReturnFocusToOwner && Owner != null)
				Owner.Activate();
		}
		#endregion
	}
	
	#region CancelException Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Useful little exception class that clients can throw as part of their processing the
	/// Progress Dialog's cancel event.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CancelException : Exception
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructor that allows a message to be set.
		/// </summary>
		/// <param name="msg"></param>
		/// ------------------------------------------------------------------------------------
		public CancelException(string msg) : base(msg)
		{
		}
	}
	#endregion
}
