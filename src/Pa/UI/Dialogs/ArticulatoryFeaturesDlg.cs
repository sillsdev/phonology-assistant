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
// File: ArticulatoryFeaturesDlg.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Dialog class to allowing modifying articulatory feature names and specify
	/// IPA character features.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ArticulatoryFeaturesDlg : DefineFeaturesDlgBase
	{
		#region Construction/Setup
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ArticulatoryFeaturesDlg() : base("DefineArticulatoryFeaturesDlg")
		{

			btnAdd.Click += new EventHandler(btnAdd_Click);
			btnRemove.Click += new EventHandler(btnRemove_Click);
			btnAdd.Visible = true;
			btnRemove.Visible = true;
			m_lvFeatures.KeyDown += new KeyEventHandler(m_lvFeatures_KeyDown);
			m_lvFeatures.SelectedIndexChanged += new EventHandler(m_lvFeatures_SelectedIndexChanged);
			btnAdd.Enabled = m_lvFeatures.CanCustomFeaturesBeAdded;
		}

		#endregion

		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determine whether or not the remove button should be enabled.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_lvFeatures_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Only enable the remove button when the feature list has focus and the
			// selected item is a custom feature.
			btnRemove.Enabled = m_lvFeatures.IsCurrentFeatureCustom;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Trap delete key to remove custom features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_lvFeatures_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete && btnRemove.Enabled)
				btnRemove_Click(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void btnAdd_Click(object sender, EventArgs e)
		{
			m_lvFeatures.AddCustomArticulatoryFeature();
			btnAdd.Enabled = m_lvFeatures.CanCustomFeaturesBeAdded;
			btnRemove.Enabled = m_lvFeatures.IsCurrentFeatureCustom;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void btnRemove_Click(object sender, EventArgs e)
		{
			m_lvFeatures.RemoveCustomArticulatoryFeature();
			btnAdd.Enabled = m_lvFeatures.CanCustomFeaturesBeAdded;
			btnRemove.Enabled = m_lvFeatures.IsCurrentFeatureCustom;
		}

		#endregion

		#region Overridden Methods/Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get	{return	m_lvFeatures.IsDirty || base.IsDirty;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			// Only save the feature cache when it changed.
			if (m_lvFeatures.IsDirty)
				PaApp.Project.SaveAFeatureCache();
			
			return base.SaveChanges();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void ThrowAwayChanges()
		{
			PaApp.Project.LoadAFeatureCache();
			base.ThrowAwayChanges();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the user choosing to restore default feature names and character features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleRestoringDefaults()
		{
			//DataUtils.RestoreDefaultArticulatoryFeatures();
		}

		#endregion

		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(ArticulatoryFeaturesDlg));
			this.splitContainer1.SuspendLayout();
			this.pnlButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			// 
			// pnlButtons
			// 
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// ArticulatoryFeaturesDlg
			// 
			resources.ApplyResources(this, "$this");
			this.Name = "ArticulatoryFeaturesDlg";
			this.splitContainer1.ResumeLayout(false);
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
