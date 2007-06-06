using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SIL.Pa.Resources;
using SIL.Pa.Controls;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
{
	public partial class DefineClassDlg : OKCancelDlgBase
	{
		private const string kWildcardDiacritic = "*";

		private FeatureListView m_lvArticulatoryFeatures;
		private FeatureListView m_lvBinaryFeatures;
		private Control[] m_ctrls = new Control[4];
		private ClassListViewItem m_classInfo;
		//private bool m_cancelButtonPressed = false;
		private PhonesInFeatureViewer m_conViewer;
		private PhonesInFeatureViewer m_vowViewer;
		private PhonesInFeatureViewer m_otherPhonesViewer;
		private ClassesDlg m_classesDlg;
		private bool m_addClass = false;
		private bool m_splitterSettingsLoaded = false;

		#region Construction and setup
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DefineClassDlg(ClassListViewItem classInfo)
		{
			InitializeComponent();

			lblClassName.Font = FontHelper.UIFont;
			txtClassName.Font = FontHelper.UIFont;
			lblMembers.Font = FontHelper.UIFont;
			rdoAnd.Font = FontHelper.UIFont;
			rdoOr.Font = FontHelper.UIFont;
			lblBasedOn.Font = FontHelper.UIFont;
			cboBasedOn.Font = FontHelper.UIFont;

			lblMembers.Top = (pnlMembers.Height - txtMembers.Height) / 2;
			txtMembers.Left = lblMembers.Right + 6;

			lvClasses.Dock = DockStyle.Fill;
			lvClasses.LoadSettings(Name);

			IinitializeCharExplorer();

			m_lvArticulatoryFeatures = InitializeFeatureList(PaApp.FeatureType.Articulatory);
			m_lvBinaryFeatures = InitializeFeatureList(PaApp.FeatureType.Binary);

			cboBasedOn.Items.Add(ResourceHelper.GetString("kstidClassBasedOnPhoneticChars"));
			cboBasedOn.Items.Add(ResourceHelper.GetString("kstidClassBasedOnArticulatoryFeatures"));
			cboBasedOn.Items.Add(ResourceHelper.GetString("kstidClassBasedOnBinaryFeatures"));
			//cboBasedOn.Items.Add(ResourceHelper.GetString("kstidClassBasedOnOtherClasses"));

			m_ctrls[0] = charExplorer;
			m_ctrls[1] = splitOuter;
			m_ctrls[2] = splitOuter;
			m_ctrls[3] = lvClasses;

			splitOuter.Dock = DockStyle.Fill;

			SetupPhoneViewers();

			m_classInfo = (classInfo == null ?
				new ClassListViewItem() : new ClassListViewItem(classInfo));

			rdoOr.Left = rdoAnd.Left;
			rdoAnd.Checked = m_classInfo.ANDFeatures;
			rdoOr.Checked = !rdoAnd.Checked;
			txtClassName.Text = m_classInfo.Text;
			cboBasedOn.SelectedIndex = (int)m_classInfo.ClassType;
			m_lvArticulatoryFeatures.CurrentMasks = m_classInfo.Masks;
			m_lvBinaryFeatures.CurrentMasks = m_classInfo.Masks;

			UpdateCharacterViewers();

			m_classInfo.IsDirty = false;
			btnOK.Enabled = false;

			LoadSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Iinitializes the IPA character explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void IinitializeCharExplorer()
		{
			List<IPACharacterTypeInfo> typesToShow = new List<IPACharacterTypeInfo>();
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Consonant,
				IPACharacterSubType.Pulmonic));
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Consonant,
				IPACharacterSubType.NonPulmonic));
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Consonant,
				IPACharacterSubType.OtherSymbols));
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Vowel));
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Diacritics));
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Suprasegmentals,
				IPACharacterSubType.StressAndLength));
			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Suprasegmentals,
				IPACharacterSubType.ToneAndAccents));

			charExplorer.TypesToShow = typesToShow;
			charExplorer.Dock = DockStyle.Fill;
			charExplorer.Load();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private FeatureListView InitializeFeatureList(PaApp.FeatureType featureType)
		{
			FeatureListView flv = new FeatureListView(featureType);
			flv.Load();
			flv.Dock = DockStyle.Fill;
			flv.Visible = true;
			flv.LabelEdit = false;
			flv.FeatureChanged += new FeatureListView.FeatureChangedHandler(HandleFeatureChanged);
			flv.TabIndex = cboBasedOn.TabIndex + 1;
			splitOuter.Panel2.Controls.Add(flv);
			return flv;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupPhoneViewers()
		{
			m_conViewer = new PhonesInFeatureViewer(IPACharacterType.Consonant, Name, "ConVwr");
			m_conViewer.HeaderText = Properties.Resources.kstidDefClassPhoneHdrCon;
			m_conViewer.Dock = DockStyle.Fill;
			splitCV.Panel1.Controls.Add(m_conViewer);
			m_conViewer.BringToFront();

			m_vowViewer = new PhonesInFeatureViewer(IPACharacterType.Vowel, Name, "VowVwr");
			m_vowViewer.HeaderText = Properties.Resources.kstidDefClassPhoneHdrVow;
			m_vowViewer.Dock = DockStyle.Fill;
			splitCV.Panel2.Controls.Add(m_vowViewer);
			m_vowViewer.BringToFront();

			m_otherPhonesViewer = new PhonesInFeatureViewer(IPACharacterType.Unknown, Name, "OtherVwr");
			m_otherPhonesViewer.HeaderText = Properties.Resources.kstidDefClassPhoneHdrOther;
			m_otherPhonesViewer.CanViewExpandAndCompact = false;
			m_otherPhonesViewer.Dock = DockStyle.Fill;
			splitPhoneViewers.Panel2.Controls.Add(m_otherPhonesViewer);
			m_otherPhonesViewer.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadSettings()
		{
			// Restore query from setting file.
			PaApp.SettingsHandler.LoadFormProperties(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the splitter locations from the settings file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadSplitterSettings()
		{
			try
			{
				// These are in a try/catch because sometimes they might throw an exception
				// in rare cases. The exception has to do with a condition in the underlying
				// .Net framework that I haven't been able to make sense of. Anyway, if an
				// exception is thrown, no big deal, the splitter distances will just be set
				// to their default values.
				int splitDistance = PaApp.SettingsHandler.GetIntSettingsValue(Name, "split3", 0);
				if (splitDistance > 0)
					splitOuter.SplitterDistance = splitDistance;

				splitDistance = PaApp.SettingsHandler.GetIntSettingsValue(Name, "split2", 0);
				if (splitDistance > 0)
					splitPhoneViewers.SplitterDistance = splitDistance;

				splitDistance = PaApp.SettingsHandler.GetIntSettingsValue(Name, "split1", 0);
				if (splitDistance > 0)
					splitCV.SplitterDistance = splitDistance;
			}
			catch { }

			m_splitterSettingsLoaded = true;
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the information for the class being added or modified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ClassListViewItem ClassInfo
		{
			get {return m_classInfo;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the information for the class being added or modified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ClassesDlg ClassesDlg
		{
			get { return m_classesDlg; }
			set { m_classesDlg = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets AddClass.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AddClass
		{
			get { return m_addClass; }
			set { m_addClass = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets TxtClassName.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TextBox TxtClassName
		{
			get { return txtClassName; }
		}
		#endregion

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the saved query for the IPA explorer bar after the form has shown the
		/// first time.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			// This is best done after the form has become visible.
			charExplorer.LoadSettings(Name);

			UpdateCharacterViewers();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get	{return (m_classInfo.IsDirty || base.IsDirty);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			base.SaveSettings();
			PaApp.SettingsHandler.SaveFormProperties(this);
			charExplorer.SaveSettings(Name);
			lvClasses.SaveSettings(Name);

			PaApp.SettingsHandler.SaveSettingsValue(Name, "split1", splitCV.SplitterDistance);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "split2", splitPhoneViewers.SplitterDistance);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "split3", splitOuter.SplitterDistance);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			if (m_classInfo.ClassType != SearchClassType.PhoneticChars)
				m_classInfo.Pattern = txtMembers.Text.Trim();
			else
			{
				string phones = txtMembers.Text.Trim().Replace(",", string.Empty);
				phones = IPACharCache.PhoneticParser_CommaDelimited(phones, true);
				m_classInfo.Pattern = "{" + phones + "}";
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return true if data is OK.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			if (m_addClass)
			{
				// Ensure the new class doesn't have an empty class name
				if (txtClassName.Text == string.Empty)
				{
					STUtils.STMsgBox(Properties.Resources.kstidDefineClassEmptyClassName,
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}

				// Ensure the new class doesn't have a duplicate class name
				foreach (ClassListViewItem item in ClassesDlg.ClassListView.Items)
				{
					if (item.Text == txtClassName.Text)
					{
						STUtils.STMsgBox(string.Format(Properties.Resources.kstidDefineClassDupClassName,
							txtClassName.Text), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
				}
			}
			return true;
		}

		#endregion

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the class name changing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtClassName_TextChanged(object sender, EventArgs e)
		{
			m_classInfo.Text = txtClassName.Text.Trim();
			m_classInfo.IsDirty = true;
			btnOK.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user clicking on an IPA character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleIPACharPicked(CharPicker chooser, ToolStripButton item)
		{
			string itemText = item.Text.Replace(DataUtils.kDottedCircle, string.Empty);

			// Split the IPA string into what's before the IP and what's after.
			string beforeIP = txtMembers.Text.Substring(0, txtMembers.SelectionStart);
			string afterIP = txtMembers.Text.Substring(txtMembers.SelectionStart + txtMembers.SelectionLength);

			// Parse the string into individual phones.
			string phones =
				IPACharCache.PhoneticParser_CommaDelimited(beforeIP + itemText + afterIP, false);

			txtMembers.Text = (string.IsNullOrEmpty(phones) ? string.Empty : phones);
			txtMembers.SelectionStart = txtMembers.Text.Length + 1;
			m_classInfo.IsDirty = true;
			btnOK.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user choosing a feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleFeatureChanged(object sender, ulong[] newMasks)
		{
			m_classInfo.Masks = newMasks;
			txtMembers.Text = m_classInfo.FormattedMembersString;
			m_classInfo.IsDirty = true;
			UpdateCharacterViewers();
			btnOK.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user choosing a class (by double-clicking on a class in the class list)
		/// as a member of the class being defined.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lvClasses_DoubleClick(object sender, EventArgs e)
		{
			if (lvClasses.SelectedItems == null || lvClasses.SelectedItems.Count == 0)
				return;

			//ClassListViewItem item = lvClasses.SelectedItems[0] as ClassListViewItem;
			//m_classInfo.OtherClassIds += (string.IsNullOrEmpty(m_classInfo.OtherClassIds) ?
			//    "" : ",") + item.Id.ToString();

			//txtMembers.Text = m_classInfo.FormattedMembersString;
			//m_classInfo.IsDirty = true;
			//btnOK.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user choosing a class (by pressing enter on a class in the class list)
		/// as a member of the class being defined.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lvClasses_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
				lvClasses_DoubleClick(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Change the resultView for the "Based on" class type that was chosen.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cboBasedOn_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboBasedOn.SelectedIndex < 0)
				return;

			splitOuter.SuspendLayout();

			m_classInfo.ClassType = (SearchClassType)cboBasedOn.SelectedIndex;
			m_classInfo.IsDirty = true;

			for (int i = 0; i < m_ctrls.Length; i++)
				m_ctrls[i].Visible = false;
			
			m_ctrls[cboBasedOn.SelectedIndex].Visible = true;
			m_ctrls[cboBasedOn.SelectedIndex].BringToFront();

			m_lvArticulatoryFeatures.Visible = (m_classInfo.ClassType == SearchClassType.Articulatory);
			m_lvBinaryFeatures.Visible = (m_classInfo.ClassType == SearchClassType.Binary);
			
			// The scope button is irrelevant for IPA character classes.
			// So hide it when that's the case.
			rdoAnd.Visible = rdoOr.Visible = (m_classInfo.ClassType != SearchClassType.PhoneticChars);

			UpdateCharacterViewers();
			
			// Adjust properties of the members text box accordingly.
			txtMembers.Font = (cboBasedOn.SelectedIndex > 0 ?
				FontHelper.UIFont : FontHelper.MakeEticRegFontDerivative(16));

			txtMembers.Top = (pnlMembers.Height - txtMembers.Height) / 2 - 1;
			txtMembers.Text = m_classInfo.FormattedMembersString;
			txtMembers.ReadOnly = (m_classInfo.ClassType != SearchClassType.PhoneticChars);
			txtMembers.SelectionStart = txtMembers.Text.Length + 1;
			btnOK.Enabled = true;

			if (!m_splitterSettingsLoaded &&
				(m_classInfo.ClassType == SearchClassType.Articulatory ||
				m_classInfo.ClassType == SearchClassType.Binary))
			{
				LoadSplitterSettings();
			}

			splitOuter.ResumeLayout();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user choosing one of the items in the tsbWhatToInclude drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleScopeClick(object sender, EventArgs e)
		{
			m_classInfo.ANDFeatures = rdoAnd.Checked;
			txtMembers.Text = m_classInfo.FormattedMembersString;
			m_classInfo.IsDirty = true;
			btnOK.Enabled = true;
			UpdateCharacterViewers();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the appropriate character viewer is up to date.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateCharacterViewers()
		{
			if (m_classInfo.ClassType == SearchClassType.Binary)
			{
				m_conViewer.SetBMask(m_classInfo.Masks[0], m_classInfo.ANDFeatures);
				m_vowViewer.SetBMask(m_classInfo.Masks[0], m_classInfo.ANDFeatures);
				m_otherPhonesViewer.SetBMask(m_classInfo.Masks[0], m_classInfo.ANDFeatures);
			}
			else if (m_classInfo.ClassType == SearchClassType.Articulatory)
			{
				m_conViewer.SetAMasks(m_classInfo.Masks, m_classInfo.ANDFeatures);
				m_vowViewer.SetAMasks(m_classInfo.Masks, m_classInfo.ANDFeatures);
				m_otherPhonesViewer.SetAMasks(m_classInfo.Masks, m_classInfo.ANDFeatures);
			}
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
		/// When class type is an IPA character class, handle (i.e. ignore) all keyboard entry
		/// of characters that aren't found in the IPA cache (i.e. the IPACharacter table
		/// from the database). 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtMembers_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (m_classInfo.ClassType == SearchClassType.PhoneticChars && e.KeyChar != (char)Keys.Back)
				e.Handled = (DataUtils.IPACharCache[e.KeyChar] == null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For classes other than for IPA characters, delete all the text when the user
		/// presses the backspace or delete keys.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtMembers_KeyDown(object sender, KeyEventArgs e)
		{
			if (m_classInfo.ClassType == SearchClassType.PhoneticChars)
				return;

			if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
			{
				txtMembers.Text = string.Empty;
				//if (m_classInfo.ClassType == ClassType.OtherClasses)
				//    m_classInfo.OtherClassIds = null;
				//else
					m_classInfo.Masks = new ulong[] {0, 0};
			}
		}

		#endregion

		#region Misc. Resizing event handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the IPACharChooser controls have their height and with adjusted as
		/// their parent, the explorer bar, changes sizes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void explorerBar_Resize(object sender, EventArgs e)
		{
			//exbarIPAChars.SuspendLayout();

			//foreach (ExplorerBarItem item in exbarIPAChars.Items)
			//{
			//    IPACharChooser chooser = item.Control as IPACharChooser;
			//    item.SetHostedControlHeight(chooser.PreferredHeight);
			//}

			//exbarIPAChars.ResumeLayout();
		}

		#endregion
	}
}