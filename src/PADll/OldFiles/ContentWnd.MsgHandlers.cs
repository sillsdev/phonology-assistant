// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005, SIL International. All Rights Reserved.   
// <copyright from='2005' to='2005' company='SIL International'>
//		Copyright (c) 2005, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: ContentWnd.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms.VisualStyles;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Resources;
using SIL.Pa.Dialogs;
using SIL.SpeechTools.Database;
using XCore;
using Audio;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for PaMainWnd.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ContentWnd
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCategory(object args)
		{
			string newTitle = null;
			int id = DBUtils.AddCategory(ref newTitle);
			RefreshTreeInfo refreshInfo = new RefreshTreeInfo(DBFields.CatId + id, true);
			PaApp.MsgMediator.SendMessage("RefreshTree", refreshInfo);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddFolder(object args)
		{
			string newTitle = null;

			int catId = (CurrentTreeNodeType == PaTreeNodeType.Category ?
				CurrentTreeNode.Id : CurrentTreeNode.NodesParentCategoryId);
			
			int id = DBUtils.AddFolder(catId, ref newTitle);
			RefreshTreeInfo refreshInfo = new RefreshTreeInfo(DBFields.FolderId + id, true);
			PaApp.MsgMediator.SendMessage("RefreshTree", refreshInfo);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Delete the currently selected Category, Folder, or Document from the database.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		public bool OnEditDelete(object args)
		{
			string item = ResourceHelper.GetString("kstid" +
				DBFields.TableFromField[CurrentTreeNode.DBField]);

			string msg = string.Format(ResourceHelper.GetString("kstidDeleteItemMsg"),
				item, CurrentTreeNode.Text);

			// Make sure the user really wants to do this.
			if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo,
				MessageBoxIcon.Question) == DialogResult.No)
			{
				return true;
			}

			// Look for the most logical node to select after deleting the current one.
			PaTreeNode node = CurrentTreeNode.NextNode as PaTreeNode;
			if (node == null)
				node = CurrentTreeNode.PrevNode as PaTreeNode;

			if (node == null)
				node = CurrentTreeNode.Parent as PaTreeNode;

			bool updateTree = false;
			bool deleteRecord = true;

			// If we're deleting a document node then first remove the link to the document.
			if (CurrentTreeNodeType == PaTreeNodeType.Document)
			{
				int remainingLinks =
					DBUtils.DeleteDocumentLink(CurrentTreeNode.DocLinkId, CurrentTreeNode.Id);

				// Update the tree if the link was deleted successfully.
				updateTree = (remainingLinks >= 0);

				// Delete the document record when there are no more remaining links to
				// the document.
				deleteRecord = (remainingLinks == 0);

				// TODO: Go through and make sure the documents that are down to one link have their text unbolded.
			}

			if (deleteRecord)
				updateTree = DBUtils.DeleteRecord(CurrentTreeNode.DBField, CurrentTreeNode.Id);

			if (updateTree)
			{
				// Set the previous node to null since we're about to
				// remove it, rendering m_prevTreeNode invalid.
				m_prevTreeNode = null;
				
				// Now remove the current node and select a new one if there are any left.
				tvContents.Nodes.Remove(CurrentTreeNode);
				if (tvContents.Nodes.Count > 0)
					tvContents.SelectedNode = (node != null ? node : tvContents.Nodes[0]);
			}

			return true;
		}


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		void gcvs_MouseLeftTimeOut(object sender, EventArgs e)
		{
			GridColumnVisibilitySetter gcvs = sender as GridColumnVisibilitySetter;

			// The name of the popup to close is in the tag property.
			if (gcvs != null && gcvs.Tag is string)
				PaApp.TMAdapter.HideBarItemsPopup(gcvs.Tag as string);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the transcription type changes in one of the three
		/// transcription text boxes on the Transcription tab.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnTranscriptionTypeChanged(object args)
		{
			if (CurrentTreeNodeType != PaTreeNodeType.Document)
				return false;

			object[] obj = args as object[];
			if (obj == null || obj.Length != 2)
				return false;

			LabelledTextBox ltb = obj[0] as LabelledTextBox;

			if (ltb == null)
				return false;

			ltb.RefreshContent(CurrentTreeNode.Id);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the transcription font(s) get changed in the options
		/// dialog.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnTranscriptionFontsChanged(object args)
		{
			//ltbTransWnd1.RefreshFont();
			//ltbTransWnd2.RefreshFont();
			//ltbTransWnd3.RefreshFont();

			//m_grid.RefreshColumnFonts();

			// Return false to allow other windows to update their fonts.
			return false;
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// --------------------------------------------------------------------------------
		protected bool OnFileImport(object args)
		{
			//string[] filenames = args as string[];

			//// If the arguments weren't a list of files, then get the list of import
			//// files from an open file dialog.
			//if (filenames == null)
			//{
			//    int filterIndex = PaApp.SettingsHandler.GetIntWindowValue("ImportOFD", "filter", 0);

			//    // TODO: Localize
			//    filenames = PaApp.OpenFileDialog("sfm",
			//        "Standard Format (*.sfm)|*.sfm|FindPhone to PA (*.smt)|*.smt|" +
			//        "Exported PA File (*.sm)|*.sm|All Files (*.*)|*.*", ref filterIndex,
			//        "Choose a files to import", true);

			//    PaApp.SettingsHandler.SaveWindowValue("ImportOFD", "filter", filterIndex);
			//}

			//if (filenames.Length == 0)
			//    return true;

			//using (SFMarkerMappingDlg dlg = new SFMarkerMappingDlg(filename))
			//{
			//    if (dlg.ShowDialog(this) == DialogResult.OK)
			//    {
			//        Enabled = false;
			//        Importer fileImporter = new Importer(filenames, dlg.Mappings);

			//        try
			//        {
			//            DBUtils.BeginTransaction();
			//            fileImporter.Process(dlg.TotalLinesInFile);
			//        }
			//        catch (Exception e)
			//        {
			//            DBUtils.RollBackTransaction();
			//            // TODO: Give a more meaningful message.
			//            MessageBox.Show(e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			//        }
			//        finally
			//        {
			//            Enabled = true;
			//        }

			//        DBUtils.CommitTransaction();
			//        DBUtils.UpdateTotalCounts();
			//        PaApp.MsgMediator.SendMessage("RefreshTree", null);
			//        Text = string.Format(ResourceHelper.GetString("kstidContentWindowCaption"),
			//            DBUtils.LanguageName, DBUtils.DocumentCount);
			//    }
			//}

			return true;
		}


	}
}