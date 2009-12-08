using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Resources;
using SilUtils;

namespace SIL.Pa.UI.Dialogs
{
	public partial class OKCancelDlgBase : Form
	{
		protected bool m_cancelButtonPressed;
		protected bool m_dirty;
		private bool m_changesWereMade;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public OKCancelDlgBase()
		{
			InitializeComponent();
			InitStrings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Allows subclasses to initialize via the LocalizationManager.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void InitStrings()
		{
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

			PaApp.MsgMediator.SendMessage(Name + "HandleCreated", this);
			PaApp.MsgMediator.SendMessage("OKCancelDlgHandleCreated", this);

			if (Parent is Form)
				((Form)Parent).AddOwnedForm(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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
				DialogResult result = Utils.MsgBox(
					ResourceHelper.GetString("kstidSaveChangesMsg"),
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
				PaApp.MsgMediator.SendMessage("DialogSaveSettings", dsmi);
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
			PaApp.MsgMediator.SendMessage("DialogSaveSettings", dsmi);

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
			if (PaApp.MsgMediator.SendMessage("DialogSaveChanges", dsmi))
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
			PaApp.MsgMediator.SendMessage("DialogSaveSettings", dsmi);

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
			if (PaApp.MsgMediator.SendMessage("DialogSaveChanges", dsmi))
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
			PaApp.MsgMediator.SendMessage("DialogSaveSettings", dsmi);

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
			PaApp.ShowHelpTopic(this);
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