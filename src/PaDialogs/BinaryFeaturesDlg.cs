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
using System.ComponentModel;

namespace SIL.Pa.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Dialog class to allowing modifying articulatory feature names and specify
	/// IPA character features.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class BinaryFeaturesDlg : DefineFeaturesDlgBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public BinaryFeaturesDlg() : base("DefineBinaryFeaturesDlg")
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the features list should display
		/// articulatory features or binary features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override PaApp.FeatureType FeatureType
		{
			get {return PaApp.FeatureType.Binary;}
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
				PaApp.Project.SaveBFeatureCache();

			return base.SaveChanges();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void ThrowAwayChanges()
		{
			PaApp.Project.LoadBFeatureCache();
			base.ThrowAwayChanges();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the user choosing to restore default feature names and character features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleRestoringDefaults()
		{
			//DataUtils.RestoreDefaultBinaryFeatures();
		}

		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(BinaryFeaturesDlg));
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
			// BinaryFeaturesDlg
			// 
			resources.ApplyResources(this, "$this");
			this.Name = "BinaryFeaturesDlg";
			this.splitContainer1.ResumeLayout(false);
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
