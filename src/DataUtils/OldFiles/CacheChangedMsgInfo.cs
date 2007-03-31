using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class holds information used to pass to SendMessage when the document cache
	/// contents change.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CacheChangedMsgInfo
	{
		/// <summary>The active form at the time the cache change was made.</summary>
		public Form ActiveForm;

		/// <summary>This is the active control at the time the cache change was made.</summary>
		public Control ActiveControl;

		/// <summary>
		/// If the active control is hosted on a UserControl, this is the outer-most
		/// UserControl in which it is contained.
		/// </summary>
		public UserControl ActiveUserControl;

		/// <summary>Id of the document whose cached value changed.</summary>
		public int DocId;

		/// <summary>When the item in the cache that changed is one of the transcription words
		/// (i.e. Phonetic, Phonemic, Tone, Orthographic, Gloss, Part of Speech, NoteBookRef,
		/// or CV Pattern) then this is the AllWordIndexId for the associated phonetic word.
		public int AllWordIndexId;

		/// <summary>The name of the database field that changed.</summary>
		public string DBField;

		/// <summary>The old value stored in the cache.</summary>
		public object OldValue;

		/// <summary>The new value stored in the cache.</summary>
		public object NewValue;

		/// <summary>Misc. data to pass to a message handler.</summary>
		public object Tag;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a CacheChangedMsgInfo object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CacheChangedMsgInfo()
		{
			Form mainWnd = DBUtils.MainWindow;

			// Can't do much here without a main MDI client.
			if (mainWnd == null)
				return;

			ActiveForm = (mainWnd.ActiveMdiChild == null ?  mainWnd : mainWnd.ActiveMdiChild);
			Control ctrl = ActiveForm.ActiveControl;

			// Drill down to the lowest level active control.
			while (ctrl is ContainerControl)
			{
				// If the active control is part of a UserControl, then save the UserControl
				// it's on. If the control is on a UserControl nested in one or more UserControls
				// then it's questionable how much value there is in saving the outer UserControl.
				// But, until it proves to be inadaquate, I'm doing it this way for now. YAGNI!
				if (ctrl is UserControl && ActiveUserControl == null)
					ActiveUserControl = ctrl as UserControl;

				if (((ContainerControl)ctrl).ActiveControl != null)
					ctrl = ((ContainerControl)ctrl).ActiveControl;
			}

			ActiveControl = ctrl;
		}
	}
}
