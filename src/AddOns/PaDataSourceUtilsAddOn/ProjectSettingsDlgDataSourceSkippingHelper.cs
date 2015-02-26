// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.Text;
using XCore;
using SIL.Pa.Dialogs;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;
using System.Reflection;
using System.ComponentModel;
using System.Reflection.Emit;

namespace SIL.Pa.DataSourceUtilsAddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ProjectSettingsDlgDataSourceSkippingHelper : IxCoreColleague
	{
		private const string kLoadColName = "dontSkip";
		private SilGrid m_grid;
		private PaProject m_project;
		private ProjectSettingsDlg m_dialog;
		private SkippedDataSourceList m_skippedList;

		#region Construction and initialization
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Initialize(ProjectSettingsDlg dlg)
		{
			if (dlg != null)
				new ProjectSettingsDlgDataSourceSkippingHelper(dlg);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ProjectSettingsDlgDataSourceSkippingHelper(ProjectSettingsDlg dlg)
		{
			m_dialog = dlg;
			m_dialog.FormClosed += m_dialog_FormClosed;

			m_grid = ReflectionHelper.GetField(m_dialog, "m_grid") as SilGrid;
			if (m_grid == null)
				return;

			// Add our column to allow the user to specify (via check boxes) the
			// data sources he wants PA to skip when loading data sources.
			DataGridViewColumn col = SilGrid.CreateCheckBoxColumn(kLoadColName);
			col.HeaderText = Properties.Resources.kstidLoadColumnHeadingText;
			col.Resizable = DataGridViewTriState.False;
			m_grid.Columns.Insert(0, col);
			m_grid.AutoResizeColumn(0, DataGridViewAutoSizeColumnMode.ColumnHeader);
			m_grid.RowsAdded += grid_RowsAddedOnLoad;
			m_grid.ColumnDisplayIndexChanged += m_grid_ColumnDisplayIndexChanged;

			PaApp.MsgMediator.AddColleague(this);

			// This is some experimental code used to see if I can remove the click event for
			// one of the drop-down menu options for adding a data source so that I could
			// insert my own click event in order to bring up a different dialog for adding
			// an FW data source. The other dialog would allow the user to choose different
			// queries for reading FW databases.
			//try
			//{
			//    ToolStripMenuItem cmnu = ReflectionHelper.GetField(m_dialog, "cmnuAddOtherDataSource") as ToolStripMenuItem;
			//    Delegate d = Delegate.CreateDelegate(typeof(EventHandler), m_dialog, "cmnuAddOtherDataSource_Click");
			//    EventInfo ei = typeof(ToolStripMenuItem).GetEvent("Click");
			//    ei.RemoveEventHandler(cmnu, d);
			//}
			//catch { }
		}

		#endregion

		#region Methods for initializing the values in the Load? column we added.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Use this event to initialize the Load? column when the grid is loaded for the
		/// first time as the dialog is being constructed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void grid_RowsAddedOnLoad(object sender, DataGridViewRowsAddedEventArgs e)
		{
			if (m_project == null)
				m_project = ReflectionHelper.GetField(m_dialog, "m_project") as PaProject;

			// Get the list of the skipped data source that will be used to initialize
			// our column in the OnInitializeLoadColumn method.
			m_skippedList = SkippedDataSourceList.Load(m_project);

			// Post a message here to initialize the "Load?" column values in the grid
			// because the rows don't actually have any data in them yet.
			PaApp.MsgMediator.PostMessage("InitializeLoadColumn", null);

			m_grid.RowsAdded -= grid_RowsAddedOnLoad;
			m_grid.RowsAdded += grid_RowsAdded;
			m_grid.RowsRemoved += m_grid_RowsRemoved;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initialize the Load? column for any new rows added by the user via the Add
		/// button on the dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			// Post a message here to initialize the "Load?" column values in the grid
			// because the rows don't actually have any data in them yet.
			PaApp.MsgMediator.PostMessage("InitializeLoadColumn", null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will initialize the values in the column we added in this add-on
		/// (i.e. the column allowing the user to specify whether or not they want to skip
		/// loading one or more data sources in the current project).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnInitializeLoadColumn(object args)
		{
			if (m_skippedList == null)
				return true;

			m_grid.CellValueChanged -= m_grid_CellValueChanged;
			bool wasDirty = m_grid.IsDirty;

			foreach (DataGridViewRow row in m_grid.Rows)
			{
				row.Cells[kLoadColName].Value =
					!m_skippedList.SkipDataSource(row.Cells["sourcefiles"].Value as string);
			}

			m_grid.IsDirty = wasDirty;
			m_grid.CellValueChanged += m_grid_CellValueChanged;

			// For some reason, the selected row's check box doesn't get painted
			// after being set. This seems to take care of the problem.
			m_grid.EndEdit();

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Keep the list of skipped data sources updated as the user changes values in the
		/// column we added for them to check or uncheck.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex >= 0 && m_grid.Columns[e.ColumnIndex].Name == kLoadColName)
			{
				string dsName = m_grid["sourcefiles", e.RowIndex].Value as string;
				if (m_skippedList.ContainsKey(dsName))
					m_skippedList[dsName] = !(bool)m_grid[kLoadColName, e.RowIndex].Value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			// Since rows get removed when the grid is completely rebuilt, which happens
			// whenever data sources are added or removed, we need to rebuild our list from 
			// the project (not the grid) because, at this point, the grid is empty.
			m_skippedList.InitializeFromProject(m_project);
		}

		#endregion

		#region Methods to handle the mediator messages from the dialog
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDialogSaveSettings(object args)
		{
			if (m_grid != null)
				m_grid.EndEdit();

			// Don't allow settings to get saved yet. Return false for Continue and let
			// our form closing event handle saving the settings.
			DlgSendMessageInfo dsmi = args as DlgSendMessageInfo;
			dsmi.Continue = false;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDialogSaveChanges(object args)
		{
			// I could kick myself! The OKCancelDlgBase sends a DialogSaveChanges message
			// from it's "InternalVerify" method *AND* it's InternalSaveChanges" method.
			// I only want to receive the "DialogSaveChanges" message once, not twice.
			// (I think I had intended to broadcast a "DailogVerifyChanges" message from
			// the "InternalSaveChanges" method, but forgot to change the message name after
			// pasting in the code. Argh!). This is my kludge to make sure I only process
			// this message when I'm here when the message is sent from the dialog's
			// "InternalSaveChanges" method.
			if (Environment.StackTrace.Contains("OKCancelDlgBase.InternalVerify()"))
				return false;

			m_skippedList.Save(m_project);
			return false;
		}

		#endregion

		#region Cleanup (FormClosed event)
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_dialog_FormClosed(object sender, FormClosedEventArgs e)
		{
			m_dialog.FormClosed -= m_dialog_FormClosed;

			try
			{
				// Before the grid's settings get saved by the call to SaveSettings below,
				// remove the column we added in the grid so it's display order (and all
				// that stuff) don't get saved. Put this in a try/catch because, it
				// triggers the row enter event in the dialog and the event handler in the
				// dialog references an object that's already null by this point.
				m_grid.Columns.Remove(kLoadColName);
			}
			catch { }

			// Since we interupted the settings being saved in out OnDialogSaveSettings
			// method, force the settings to be saved now.
			ReflectionHelper.CallMethod(m_dialog, "SaveSettings", null);

			if (m_grid != null)
			{
				// Release our event handlers from the grid.
				m_grid.RowsAdded -= grid_RowsAdded;
				m_grid.RowsRemoved -= m_grid_RowsRemoved;
				m_grid.ColumnDisplayIndexChanged -= m_grid_ColumnDisplayIndexChanged;
				m_grid.CellValueChanged -= m_grid_CellValueChanged;
				m_grid = null;
			}

			m_dialog = null;
			PaApp.MsgMediator.RemoveColleague(this);
		}

		#endregion

		#region Methods for preventing the Load? column from being moved.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the column that was moved is the one we added and it's not the first columnn
		/// in the grid, then post a message to make sure it gets put back. We can't put it
		/// back in this method because that will cause a recursive kind of thing which
		/// throws an exception.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
		{
			try
			{
				if (e.Column == m_grid.Columns[kLoadColName] && e.Column.DisplayIndex != 0)
					PaApp.MsgMediator.PostMessage("CancelLoadColumnMove", null);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message is received when the column we added was moved and needs to be put
		/// back to the first column in the grid. We don't want the user moving that column,
		/// but I don't know of a way to prevent only that column from being moved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCancelLoadColumnMove(object args)
		{
			try
			{
				m_grid.Columns[kLoadColName].DisplayIndex = 0;
				System.Media.SystemSounds.Beep.Play();
			}
			catch { }

			return true;
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] { this };
		}

		#endregion
	}
}
