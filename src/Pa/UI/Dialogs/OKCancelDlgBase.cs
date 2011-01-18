using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class OKCancelDlgBase : Form
	{
		protected bool m_cancelButtonPressed;
		protected bool m_dirty;
		private bool m_changesWereMade;

		/// ------------------------------------------------------------------------------------
		public OKCancelDlgBase()
		{
			InitializeComponent();
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets a value indicating whether or not the control is currently in design mode.
		///// I have had some problems with the base class' DesignMode property being true
		///// when in design mode. I'm not sure why, but adding a couple more checks fixes the
		///// problem.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected new bool DesignMode
		//{
		//    get
		//    {
		//        return (base.DesignMode || GetService(typeof(IDesignerHost)) != null) ||
		//            (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			if (!DesignMode)
			{
				try
				{
					Settings.Default[Name] = App.InitializeForm(this, Settings.Default[Name] as FormSettings);
				}
				catch
				{
					StartPosition = FormStartPosition.CenterScreen;
				}
			}

			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Center the form in the screen or parent if the form's location is in the upper left
		/// corner. The only time this isn't the right thing to do is if the user moved their
		/// dialog to the upper left corner and that was the location saved. Otherwise, we
		/// assume it's the first time it's been shown and no location for it has yet been
		/// saved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			// This will hide visible layout ugliness.
			// The opacity is set to 1 in the shown event.
			Opacity = 0;

			base.OnHandleCreated(e);

			if (Location.X == 0 && Location.Y == 0)
			{
				Rectangle rc = (Parent != null ? Parent.Bounds : Screen.PrimaryScreen.WorkingArea);
				Location = new Point((rc.Width - Width) / 2, (rc.Height - Height) / 2);
			}

			if (!DesignMode)
			{
				App.MsgMediator.SendMessage(Name + "HandleCreated", this);
				App.MsgMediator.SendMessage("OKCancelDlgHandleCreated", this);
			}

			if (Parent is Form)
				((Form)Parent).AddOwnedForm(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			Utils.UpdateWindow(Handle);

			// At this point, the opacity should be zero. Now that we're shown and the handles
			// are all created, show the form at full opacity. This will avoid visible layout
			// ugliness that happens on dialogs with lots of panels and splitters, etc.
			Opacity = 1;

			// This is needed because some dialogs have PaPanels whose border
			// doesn't get fully painted properly when the opacity goes to full.
			Invalidate(true);
			Utils.UpdateWindow(Handle);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (Parent is Form)
				((Form)Parent).RemoveOwnedForm(this);

			InternalSaveSettings();
		    base.OnFormClosing(e);

		    if (e.Cancel || !IsDirty)
		        return;

			if (m_cancelButtonPressed)
			{
				InternalThrowAwayChanges();
				return;
			}

			if (!InternalVerify())
			{
				e.Cancel = true;
				return;
			}

		    // By this time, we know the window is not closing because the user clicked
		    // the cancel button. If he didn't explicitly click the OK button and there
		    // are changes to the data, ask if he wants the changes saved.
			if (DialogResult != DialogResult.OK)
			{
				DialogResult result = Utils.MsgBox(App.kstidSaveChangesMsg,
					MessageBoxButtons.YesNoCancel);
				
				if (result == DialogResult.Cancel)
				{
					e.Cancel = true;
					return;
				}

				if (result == DialogResult.No)
				{
					DialogResult = DialogResult.No;
					return;
				}

				DialogResult = DialogResult.OK;
			}

			if (!(m_changesWereMade = InternalSaveChanges()))
		        e.Cancel = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void ReAddButtons(int startIndexInTablePanel)
		{
			tblLayoutButtons.Controls.Add(btnOK, startIndexInTablePanel, 0);
			tblLayoutButtons.Controls.Add(btnCancel, startIndexInTablePanel + 1, 0);
			tblLayoutButtons.Controls.Add(btnHelp, startIndexInTablePanel + 2, 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set a flag indicating whether or not the cancel button was pressed. That's because
		/// in the form's closing event, we don't know if a DialogResult of Cancel is due to
		/// the user clicking on the cancel button or closing the form in some other way
		/// beside clicking on the OK button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCancel_Click(object sender, EventArgs e)
		{
			m_cancelButtonPressed = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the user saved the changes
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ChangesWereMade
		{
			get { return m_changesWereMade; }
		}

		#region Virtual Properties/Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Override in derived classes
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual bool IsDirty
		{
		    get
			{
				// Broadcast a message to anyone who cares (e.g. an AddOn).
				DlgSendMessageInfo dsmi = new DlgSendMessageInfo(this);
				App.MsgMediator.SendMessage("DialogSaveSettings", dsmi);
				return (m_dirty || dsmi.IsDirty);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when the user clicks the cancel button and the form is closing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InternalThrowAwayChanges()
		{
			// Broadcast a message to anyone who cares (e.g. an AddOn).
			DlgSendMessageInfo dsmi = new DlgSendMessageInfo(this, IsDirty);
			App.MsgMediator.SendMessage("DialogSaveSettings", dsmi);

			if (dsmi.Continue)
				ThrowAwayChanges();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when the user clicks the cancel button and the form is closing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void ThrowAwayChanges()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return true if data is OK.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool InternalVerify()
		{
			// Broadcast a message to anyone who cares (e.g. an AddOn).
			DlgSendMessageInfo dsmi = new DlgSendMessageInfo(this, IsDirty);
			if (App.MsgMediator.SendMessage("DialogSaveChanges", dsmi))
			{
				if (!dsmi.Continue)
					return dsmi.BoolToReturn;
			}

			return Verify();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return true if data is OK. Override in derived classes
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual bool Verify()
		{
		    return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called before the base class OnClosing to allow derived classes to save form
		/// settings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InternalSaveSettings()
		{
			// Broadcast a message to anyone who cares (e.g. an AddOn).
			DlgSendMessageInfo dsmi = new DlgSendMessageInfo(this, IsDirty);
			App.MsgMediator.SendMessage("DialogSaveSettings", dsmi);

			if (dsmi.Continue)
				SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called before the base class OnClosing to allow derived classes to save form
		/// settings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void SaveSettings()
		{
			//try
			//{
			//    Settings.Default[Name + "Bounds"] = Bounds;
			//}
			//catch { }

			Settings.Default.Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called after data has been determined to be dirty, verified and OK is clicked or
		/// the user has confirmed saving the changes. Override in derived classes.
		/// </summary>
		/// <returns>False if closing the form should be canceled. Otherwise, true.</returns>
		/// ------------------------------------------------------------------------------------
		private bool InternalSaveChanges()
		{
			// Broadcast a message to anyone who cares (e.g. an AddOn).
			DlgSendMessageInfo dsmi = new DlgSendMessageInfo(this, IsDirty);
			if (App.MsgMediator.SendMessage("DialogSaveChanges", dsmi))
			{
				if (!dsmi.Continue)
					return dsmi.BoolToReturn;
			}

			return SaveChanges();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called after data has been determined to be dirty, verified and OK is clicked or
		/// the user has confirmed saving the changes. Override in derived classes.
		/// </summary>
		/// <returns>False if closing the form should be canceled. Otherwise, true.</returns>
		/// ------------------------------------------------------------------------------------
		protected virtual bool SaveChanges()
		{
		    return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when the help button is clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InternalHandleHelpClick(object sender, EventArgs e)
		{
			// Broadcast a message to anyone who cares (e.g. an AddOn).
			DlgSendMessageInfo dsmi = new DlgSendMessageInfo(this, IsDirty);
			App.MsgMediator.SendMessage("DialogSaveSettings", dsmi);

			if (dsmi.Continue)
				HandleHelpClick(sender, e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when the help button is clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleHelpClick(object sender, EventArgs e)
		{
			App.ShowHelpTopic(this);
		}

		#endregion
	}

	#region DlgSendMessageInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class DlgSendMessageInfo
	{
		public Form Dialog;
		public bool IsDirty;
		public object Tag;

		// When Continue comes back from the send message as false, this value determines whether
		// or not the method issuing the send message should continue or return immediately.
		public bool Continue = true;

		// When Continue comes back from the send message as false, this value is returned
		// from the method that issued the send message when that method returns a bool.
		public bool BoolToReturn = true;

		// When Continue comes back from the send message as false, this value is returned
		// from the method that issued the send message when that method returns something
		// other than a bool.
		public object ObjToReturn;
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DlgSendMessageInfo(Form dialog) : this(dialog, false)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DlgSendMessageInfo(Form dialog, bool isDirty)
		{
			Dialog = dialog;
			IsDirty = isDirty;
		}
	}

	#endregion
}