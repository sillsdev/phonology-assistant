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
// File: SetMasks.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using SIL.Pa.Database;
using XCore;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for SetMasks.
	/// </summary>
	public partial class SetMasks : System.Windows.Forms.Form, IxCoreColleague
	{
		public enum FeatureSet : int
		{
			Articulatory = 0,
			Binary = 1,
		}

		#region Class Variables
		private int m_prevIPACharIndex = -1;
		private string m_currCharSortOrder;
		private string m_currFeatureSortOrder;
		private FeatureSet m_featureSet;
		
		private bool m_inEditMode;
		private bool CharInfoBuilt;
		private bool m_defaultsWereRestored;
		private bool KeypressCausedClick;
		private bool ClickInFeatureName;
		private int CharSortIndex;
		private int FeatSortIndex;
		private int PrevListIndex;
		private int PrevFeatureIndex;
		private string m_sortCharField;
		#endregion

		#region Constants
		private const string kANSISortField = "SILIPACharNum";
		private const string kPOASortField = "POArticulation";
		private const string kMOASortField = "MOArticulation";
		private const string kCharTypeSortField = "TypeSubOrder";
		private const string kAlphaSortField = "Features";
		#endregion
		
		#region Construction
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SetMasks(FeatureSet featureSet)
		{
			InitializeComponent();
			
			PaApp.MsgMediator.AddColleague(this);

			m_featureSet = featureSet;
			
			Text = "Define ";
			if (featureSet == FeatureSet.Articulatory)
				Text += "Articulatory";
			else
			{
				Text += "Binary";
				mslbFeatures.MultiStateType = MultiStateListBox.MultiStateTypes.TriStatePlusMinus;
			}

			Text += " Features";

			lstIPA.Font = new Font(PaApp.PhoneticFont.FontFamily, 16,
				(PaApp.PhoneticFont.Bold ? FontStyle.Bold : 0) |
				(PaApp.PhoneticFont.Italic ? FontStyle.Italic : 0));

			mslbFeatures.Font = PaApp.UIFont;
			lblDesc.Font = PaApp.UIFont;
			lblIPA.Font = PaApp.UIFont;

			m_currCharSortOrder = kANSISortField;
			m_currFeatureSortOrder = kCharTypeSortField;

			LoadFeatureList();	
			LoadIPAList();
		}

		#endregion

		#region Overridden Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			if (!e.Cancel && m_inEditMode && !m_defaultsWereRestored)
				e.Cancel = !SaveChanges(true);

			if (!e.Cancel)
				PaApp.MsgMediator.RemoveColleague(this);
		}

		#endregion
		
		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used in PA.
		/// </summary>
		/// <param name="mediator"></param>
		/// <param name="configurationParameters"></param>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public virtual IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] {this};
		}

		#endregion

		#region IPA Phone and Feature List Loading
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fill the feature grid (i.e. list) from the DB.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadFeatureList()
		{
			PaDataTable table = new PaDataTable("SELECT * FROM BitMasks WHERE (FeatureType > -1) AND " +
				"(FeatureSet=" + (int)m_featureSet + ") ORDER BY " + m_currFeatureSortOrder);

			if (table == null)
			{
				// TODO: log error
				return;
			}

			mslbFeatures.Items.Clear();

			foreach (DataRow dbRow in table.Rows)
			{
				FeatureItemInfo info = new FeatureItemInfo();
				info.m_featureName = (string)dbRow["Features"];
				info.m_maskNum = (System.Int16)dbRow["MaskNum"];
				info.m_mask = (System.Int32)dbRow["Mask"];
				info.m_minusMask = (System.Int32)dbRow["MinusMask"];
				info.m_maskId = (System.Int32)dbRow["BitMaskID"];
				mslbFeatures.Items.Add(info);
			}

			if (mslbFeatures.Items.Count >= 0)
				mslbFeatures.SelectedIndex = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fills the IPA Listbox.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadIPAList()
		{
			string sql = "SELECT * FROM IPACharSet WHERE CharType>0 " +
				(m_featureSet == FeatureSet.Articulatory ?
				string.Empty : " AND (BinaryFeatureCharOnly) ") + "ORDER BY " + m_currCharSortOrder;

			using (PaDataTable table = new PaDataTable(sql))
			{
				if (table == null)
				{
					// TODO: Log and show error
					return;
				}

				lstIPA.Items.Clear();

				foreach (DataRow dbRow in table.Rows)
				{
					if (dbRow["IPAChar"] is DBNull)
						continue;

					CharInfo charInfo = new CharInfo();
					
					// TODO: Deal with diaplaying zero-width chars.
					charInfo.m_ipaChar = (string)dbRow["IPAChar"];
					charInfo.m_description =
						(dbRow["IPADesc"] is DBNull ? string.Empty : (string)dbRow["IPADesc"]);

					if (m_featureSet == FeatureSet.Articulatory)
					{
						charInfo.m_mask[0] =
							(dbRow["Mask0"] is DBNull ? 0 : (System.Int32)dbRow["Mask0"]);
						charInfo.m_mask[1] =
							(dbRow["Mask1"] is DBNull ? 0 : (System.Int32)dbRow["Mask1"]);
						charInfo.m_mask[2] =
							(dbRow["Mask2"] is DBNull ? 0 : (System.Int32)dbRow["Mask2"]);
						charInfo.m_mask[3] =
							(dbRow["Mask3"] is DBNull ? 0 : (System.Int32)dbRow["Mask3"]);
					}
					else
					{
						charInfo.m_mask[0] =
							(dbRow["BinaryMask0"] is DBNull ? 0 : (System.Int32)dbRow["BinaryMask0"]);
						charInfo.m_mask[1] =
							(dbRow["BinaryMask1"] is DBNull ? 0 : (System.Int32)dbRow["BinaryMask1"]);
					}

					lstIPA.Items.Add(charInfo);
				}
			}

			if (lstIPA.Items.Count > 0)
				lstIPA.SelectedIndex = 0;
		}

		#endregion

		#region Save to memory the changes to a characters features
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This saves to memory any changes to a phone's feature settings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveMaskChanges(int index)
		{
			if (lstIPA.SelectedItem == null || index < 0 || index >= lstIPA.Items.Count)
				return;
			
			CharInfo charInfo = (CharInfo)lstIPA.Items[index];
			charInfo.Reset();

			for (int i = 0; i < mslbFeatures.Items.Count; i++)
			{
				FeatureItemInfo info = (FeatureItemInfo)mslbFeatures.Items[i];
				int maskNum = info.m_maskNum;

				switch (mslbFeatures.GetItemState(i))
				{
					case MultiStateListBox.MultiStateValues.Checked:
					case MultiStateListBox.MultiStateValues.Plus:
						charInfo.m_mask[info.m_maskNum] |= info.m_mask;
						break;

					case MultiStateListBox.MultiStateValues.Minus:
						charInfo.m_mask[info.m_maskNum] |= info.m_minusMask;
						break;

					default:
						break;
				}
			}
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
		private void lstIPA_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (lstIPA.SelectedIndex < 0)
				return;

			// Make sure that any changes to a phone's features gets saved to memory.
			if (m_prevIPACharIndex >= 0)
				SaveMaskChanges(m_prevIPACharIndex);

			m_prevIPACharIndex = lstIPA.SelectedIndex;
			CharInfo charInfo = (CharInfo)lstIPA.SelectedItem;
			lblDesc.Text = charInfo.m_description;

			// Loop through items in the feature list and set their state according to the
			// masks associated with the selected IPA phone.
			for (int i = 0; i < mslbFeatures.Items.Count; i++)
			{
				FeatureItemInfo info = (FeatureItemInfo)mslbFeatures.Items[i];
				
				int charactersMask = charInfo.m_mask[info.m_maskNum];
				int featuresMask = info.m_mask;
				int featuresMinusMask = info.m_minusMask;
			
				if (m_featureSet == FeatureSet.Articulatory)
				{
					mslbFeatures.SetItemState(i, ((charactersMask & featuresMask) != 0) ?
						MultiStateListBox.MultiStateValues.Checked :
						MultiStateListBox.MultiStateValues.NotSet);
				}
				else
				{
					if ((charactersMask & featuresMask) != 0)
						mslbFeatures.SetItemState(i, MultiStateListBox.MultiStateValues.Plus);
					else if ((charactersMask & featuresMinusMask) != 0)
						mslbFeatures.SetItemState(i, MultiStateListBox.MultiStateValues.Minus);
					else
						mslbFeatures.SetItemState(i, MultiStateListBox.MultiStateValues.NotSet);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Validate and save the change to a feature name.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="index"></param>
		/// <param name="newValue"></param>
		/// <returns>true if we handle the post edit details. Otherwise, false is returned and
		/// the multi-state listbox handles updating the item. We want to always make sure
		/// we return true from here because the type of our items is FeatureItemInfo and
		/// if we let the list box handle the update it will just replace our FeatureItemInfo
		/// item with a string value.</returns>
		/// ------------------------------------------------------------------------------------
		private bool mslbFeatures_AfterItemEdit(object sender, int index, string newValue)
		{
			FeatureItemInfo info = (FeatureItemInfo)mslbFeatures.Items[index];
			
			if (mslbFeatures.FindString(newValue) < 0)
				info.m_featureName = newValue;
			else	
				MessageBox.Show(this, "The feature '" + newValue + "' already exists.",
					Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if (m_inEditMode && !m_defaultsWereRestored)
			{
				SaveChanges(false);
				m_inEditMode = false;
			}

			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			m_inEditMode = false;
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void btnRestore_Click(object sender, System.EventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to restore\n" +
				"the default feature definitions?\n" +
				"This operation cannot be undone.", Application.ProductName,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				OleDbCommand command = DBUtils.DatabaseConnection.CreateCommand();

				if (m_featureSet == FeatureSet.Articulatory)
				{
					command.CommandText = "UPDATE DISTINCTROW IPACharSet SET " +
						"Mask0 = [DefaultMask0], Mask1 = [DefaultMask1], " +
						"Mask2 = [DefaultMask2], Mask3 = [DefaultMask3]";
				}
				else
				{
					command.CommandText = "UPDATE DISTINCTROW IPACharSet SET " +
						"BinaryMask0 = [DefaultBinaryMask0], " +
						"BinaryMask1 = [DefaultBinaryMask1]";
				}
				
				command.ExecuteNonQuery();
				LoadFeatureList();	
				LoadIPAList();
				m_defaultsWereRestored = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			if (MessageBox.Show("Editing features will influence phonological searching" +
				"\nresults. Do you still want to edit feature settings?", Application.ProductName,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				m_inEditMode = true;
				mslbFeatures.AllowLabelEdit = true;
				mslbFeatures.AllowStateEdit = true;
				mslbFeatures.Focus();
				btnEdit.Enabled = false;
			}
		}

		#endregion

		#region Menu Command Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnSortANSI(object args)
		{
			m_currCharSortOrder = kANSISortField;
			LoadIPAList();
			lstIPA_SelectedIndexChanged(null, null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnSortMOA(object args)
		{
			m_currCharSortOrder = kMOASortField;
			LoadIPAList();
			lstIPA_SelectedIndexChanged(null, null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnSortPOA(object args)
		{
			m_currCharSortOrder = kPOASortField;
			LoadIPAList();
			lstIPA_SelectedIndexChanged(null, null);
			return true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnSortCharType(object args)
		{
			m_currFeatureSortOrder = kCharTypeSortField;
			LoadFeatureList();
			lstIPA_SelectedIndexChanged(null, null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnSortAlpha(object args)
		{
			m_currFeatureSortOrder = kAlphaSortField;
			LoadFeatureList();
			lstIPA_SelectedIndexChanged(null, null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnEditFeatureName(object args)
		{
			mslbFeatures.BeginEdit(mslbFeatures.SelectedIndex);
			return true;
		}

		#endregion

		#region Menu Command Update Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateSortANSI(object args)
		{
			mnuSortANSI.Checked = (m_currCharSortOrder == kANSISortField);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateSortMOA(object args)
		{
			mnuSortMoA.Checked = (m_currCharSortOrder == kMOASortField);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateSortPOA(object args)
		{
			mnuSortPoA.Checked = (m_currCharSortOrder == kPOASortField);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateSortCharType(object args)
		{
			mnuSortCharType.Checked = (m_currFeatureSortOrder == kCharTypeSortField);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateSortAlpha(object args)
		{
			mnuSortAlpha.Checked = (m_currFeatureSortOrder == kAlphaSortField);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditFeatureName(object args)
		{
			mnuEditFeatureName.Enabled = m_inEditMode;
			return true;
		}
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves any changes to the DB the user has made.
		/// </summary>
		/// <param name="askToSave"></param>
		/// <returns>False if the user pressed the cancel button. Otherwise, true.</returns>
		/// ------------------------------------------------------------------------------------
		private bool SaveChanges(bool askToSave)
		{
			DialogResult result = DialogResult.OK;

			if (askToSave)
			{
				result = MessageBox.Show(this, "Would you like to save changes?",
					Application.ProductName, MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Question);
			}

			if (result == DialogResult.Cancel)
				return false;
			else if (result == DialogResult.OK)
			{
				// SaveMasksToMemory();
				// SaveFeatureNames();
				// SaveMasksToDB();
			}

			return true;
		}
	}

	#region Classes to encapsulate list box items
	/// ------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ------------------------------------------------------------------------------------
	internal class FeatureItemInfo
	{
		internal string m_featureName;
		internal int m_maskNum;
		internal int m_mask;
		internal int m_minusMask;
		internal int m_maskId;

		public override string ToString()
		{
			return m_featureName;
		}
	}

	/// ------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ------------------------------------------------------------------------------------
	internal class CharInfo
	{
		internal string m_ipaChar = null;
		internal string m_description = null;
		internal int[] m_mask = new int[4];

		public void Reset()
		{
			for (int i = 0; i < m_mask.Length; i++)
				m_mask[i] = 0;
		}
		
		public override string ToString()
		{
			return m_ipaChar;
		}
	}

	#endregion
}
