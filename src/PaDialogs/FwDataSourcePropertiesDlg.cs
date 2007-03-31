using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;
using SIL.Pa.Data;

namespace SIL.Pa.Dialogs
{
	#region FwDataSourcePropertiesDlg class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class FwDataSourcePropertiesDlg : OKCancelDlgBase
	{
		private FwDataSourceInfo m_fwSourceInfo;

		#region Construction and initialization
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourcePropertiesDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourcePropertiesDlg(FwDataSourceInfo fwSourceInfo) : this()
		{
			m_fwSourceInfo = fwSourceInfo;

			lblDatabaseValue.Text = m_fwSourceInfo.DBName;
			lblLangProjValue.Text = m_fwSourceInfo.LangProjName;

			SetControlFonts();
			LoadWsCombos();

			InitWsCombo(cboPhonetic, m_fwSourceInfo.PhoneticWs);
			InitWsCombo(cboPhonemic, m_fwSourceInfo.PhonemicWs);
			InitWsCombo(cboOrtho, m_fwSourceInfo.OrthographicWs);
			InitWsCombo(cboEngGloss, m_fwSourceInfo.EnglishGlossWs);
			InitWsCombo(cboNatGloss, m_fwSourceInfo.NationalGlossWs);

			rbLexForm.Checked = 
				(m_fwSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm);

			rbPronunField.Checked =
				(m_fwSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.PronunciationField);

			m_dirty = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add columns to fonts grid
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetControlFonts()
		{
			lblDatabase.Font = FontHelper.UIFont;
			lblDatabaseValue.Font = FontHelper.UIFont;
			lblLangProj.Font = FontHelper.UIFont;
			lblLangProjValue.Font = FontHelper.UIFont;
			grpWritingSystems.Font = FontHelper.UIFont;
			lblPhonetic.Font = FontHelper.UIFont;
			lblPhonemic.Font = FontHelper.UIFont;
			lblOrtho.Font = FontHelper.UIFont;
			lblEngGloss.Font = FontHelper.UIFont;
			lblNatGloss.Font = FontHelper.UIFont;
			cboPhonetic.Font = FontHelper.UIFont;
			cboPhonemic.Font = FontHelper.UIFont;
			cboOrtho.Font = FontHelper.UIFont;
			cboEngGloss.Font = FontHelper.UIFont;
			cboNatGloss.Font = FontHelper.UIFont;
			grpPhoneticDataStoreType.Font = FontHelper.UIFont;
			rbLexForm.Font = FontHelper.UIFont;
			rbPronunField.Font = FontHelper.UIFont;
		}

		#endregion

		#region Writing System Combos Setup
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the writing system combo boxes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadWsCombos()
		{
			FwDataReader reader = new FwDataReader(m_fwSourceInfo);

			// Build a list of the analysis writing systems to use in analysis WS combos.
			List<WritingSysInfo> wsAnalInfo = new List<WritingSysInfo>();
			foreach (KeyValuePair<int, string> wsAnal in reader.AnalysisWritingSystems)
				wsAnalInfo.Add(new WritingSysInfo(wsAnal.Value, wsAnal.Key));

			// Build a list of the analysis writing systems to use in analysis WS combos.
			List<WritingSysInfo> wsVernInfo = new List<WritingSysInfo>();
			foreach (KeyValuePair<int, string> wsVern in reader.VernacularWritingSystems)
				wsVernInfo.Add(new WritingSysInfo(wsVern.Value, wsVern.Key));

			// Add a (none) option to each list.
			wsAnalInfo.Insert(0, new WritingSysInfo(Properties.Resources.kstidFwWsNoneSpecified, 0));
			wsVernInfo.Insert(0, new WritingSysInfo(Properties.Resources.kstidFwWsNoneSpecified, 0));

			cboPhonetic.Items.AddRange(wsVernInfo.ToArray());
			cboPhonemic.Items.AddRange(wsVernInfo.ToArray());
			cboOrtho.Items.AddRange(wsVernInfo.ToArray());
			cboEngGloss.Items.AddRange(wsAnalInfo.ToArray());
			cboNatGloss.Items.AddRange(wsAnalInfo.ToArray());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the writing system combo boxes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitWsCombo(ComboBox combo, int wsNumber)
		{
			foreach (WritingSysInfo wsinfo in combo.Items)
			{
				if (wsinfo.WsNumber == wsNumber)
				{
					combo.SelectedItem = wsinfo;
					return;
				}
			}

			combo.SelectedIndex = 0;
		}

		#endregion

		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleWsSelectionChangeCommitted(object sender, EventArgs e)
		{
			m_dirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlePhoneticStorageTypeCheckedChanged(object sender, EventArgs e)
		{
			m_dirty = true;
		}

		#endregion

		#region Methods for verifying changes and saving them before closing
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure all the writing systems that should be specified, have been specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			if (!VerifyWritingSystem(cboPhonetic, lblPhonetic))
				return false;

			// Uncomment these if they should be mandatory.
			//if (!VerifyWritingSystem(cboPhonemic, lblPhonemic))
			//    return false;

			//if (!VerifyWritingSystem(cboOrtho, lblOrtho))
			//    return false;

			if (!VerifyWritingSystem(cboEngGloss, lblEngGloss))
				return false;

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that a writing system has been specified for the specified field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool VerifyWritingSystem(ComboBox combo, Label lblField)
		{
			if (combo.SelectedIndex != 0)
				return true;

			string msg = string.Format(Properties.Resources.kstidFwMissingWsMsg,
				lblField.Text.Replace("&", string.Empty));

			STUtils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			combo.Focus();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			try
			{
				m_fwSourceInfo.PhoneticWs = (cboPhonetic.SelectedItem as WritingSysInfo).WsNumber;
				m_fwSourceInfo.PhonemicWs = (cboPhonemic.SelectedItem as WritingSysInfo).WsNumber;
				m_fwSourceInfo.OrthographicWs = (cboOrtho.SelectedItem as WritingSysInfo).WsNumber;
				m_fwSourceInfo.EnglishGlossWs = (cboEngGloss.SelectedItem as WritingSysInfo).WsNumber;
				m_fwSourceInfo.NationalGlossWs = (cboNatGloss.SelectedItem as WritingSysInfo).WsNumber;

				m_fwSourceInfo.PhoneticStorageMethod = (rbLexForm.Checked ?
					FwDBUtils.PhoneticStorageMethod.LexemeForm :
					FwDBUtils.PhoneticStorageMethod.PronunciationField);
			}
			catch
			{
				return false;
			}

			return true;
		}

		#endregion
	}

	#endregion

	#region WritingSysInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a single item in the writing system drop-downs in the grid.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	internal class WritingSysInfo
	{
		internal string WsName;
		internal int WsNumber;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal WritingSysInfo(string wsName, int wsNumber)
		{
			WsName = wsName;
			WsNumber = wsNumber;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return WsName;
		}
	}

	#endregion
}