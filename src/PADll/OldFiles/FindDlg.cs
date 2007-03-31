using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for FindDlg.
	/// </summary>
	public class FindDlg : System.Windows.Forms.Form
	{
		#region Variables added by Designer
		private SIL.Pa.LabelledTextBox ltbFindWhat;
		private System.Windows.Forms.Button btnFind;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox grpField;
		private System.Windows.Forms.RadioButton rbColView;
		private System.Windows.Forms.RadioButton rbTransView;
		private System.Windows.Forms.ComboBox cboField;
		private System.Windows.Forms.GroupBox grpFindIn;
		private System.Windows.Forms.RadioButton rbCurrDoc;
		private System.Windows.Forms.RadioButton rbAllDocs;
		private System.Windows.Forms.GroupBox grpSpeaker;
		private System.Windows.Forms.RadioButton rbFemale;
		private System.Windows.Forms.RadioButton rbMale;
		private System.Windows.Forms.ComboBox cboSpeaker;
		private System.Windows.Forms.CheckBox chkMatchCase;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		private int m_PrevFieldIndex;
		private Font m_NormalFont;

		public FindDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			ltbFindWhat.TextBox.BorderStyle = BorderStyle.Fixed3D;
			ltbFindWhat.TextBox.Multiline = false;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		protected override void OnLoad(EventArgs e)
		{
			m_NormalFont = ltbFindWhat.Font;
			cboField.SelectedIndex = 0;
			UpdateForm(true);
			base.OnLoad (e);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			string errMsg = "";

			if (this.DialogResult == DialogResult.OK)
			{
				if (cboField.SelectedIndex == FindClass.SpkrIndex)
				{
					if ((cboSpeaker.SelectedIndex == -1) &&
						!(rbMale.Checked) && !(rbFemale.Checked))
						errMsg = "You have not specified anything to find.";
				}
				else if (ltbFindWhat.Text.Length == 0)
					errMsg = "You have not specified anything to find.";

				if (errMsg.Length > 0)
				{
					MessageBox.Show(errMsg, "Phonology Assistant",  MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation);
					if (cboField.SelectedIndex == FindClass.SpkrIndex)
						cboSpeaker.Select();
					else
						ltbFindWhat.Select();
					e.Cancel = true;
					return;
				}

				MessageBox.Show("Should be searching for something", "FindDlg",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			// if we've made it here, we're supposed to set the FindClass stuff as needed.
			// after that, we just quit
			base.OnClosing (e);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnFind = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpField = new System.Windows.Forms.GroupBox();
			this.rbColView = new System.Windows.Forms.RadioButton();
			this.rbTransView = new System.Windows.Forms.RadioButton();
			this.cboField = new System.Windows.Forms.ComboBox();
			this.ltbFindWhat = new SIL.Pa.LabelledTextBox();
			this.grpFindIn = new System.Windows.Forms.GroupBox();
			this.rbCurrDoc = new System.Windows.Forms.RadioButton();
			this.rbAllDocs = new System.Windows.Forms.RadioButton();
			this.grpSpeaker = new System.Windows.Forms.GroupBox();
			this.rbFemale = new System.Windows.Forms.RadioButton();
			this.rbMale = new System.Windows.Forms.RadioButton();
			this.cboSpeaker = new System.Windows.Forms.ComboBox();
			this.chkMatchCase = new System.Windows.Forms.CheckBox();
			this.grpField.SuspendLayout();
			this.grpFindIn.SuspendLayout();
			this.grpSpeaker.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnFind
			// 
			this.btnFind.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnFind.Location = new System.Drawing.Point(328, 16);
			this.btnFind.Name = "btnFind";
			this.btnFind.TabIndex = 0;
			this.btnFind.Text = "Find";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(328, 48);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			// 
			// grpField
			// 
			this.grpField.Controls.Add(this.rbColView);
			this.grpField.Controls.Add(this.rbTransView);
			this.grpField.Controls.Add(this.cboField);
			this.grpField.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.grpField.Location = new System.Drawing.Point(8, 8);
			this.grpField.Name = "grpField";
			this.grpField.Size = new System.Drawing.Size(312, 48);
			this.grpField.TabIndex = 2;
			this.grpField.TabStop = false;
			this.grpField.Text = "&Field";
			// 
			// rbColView
			// 
			this.rbColView.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbColView.Location = new System.Drawing.Point(184, 24);
			this.rbColView.Name = "rbColView";
			this.rbColView.Size = new System.Drawing.Size(104, 16);
			this.rbColView.TabIndex = 2;
			this.rbColView.Text = "Column &View";
			// 
			// rbTransView
			// 
			this.rbTransView.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbTransView.Location = new System.Drawing.Point(184, 8);
			this.rbTransView.Name = "rbTransView";
			this.rbTransView.Size = new System.Drawing.Size(120, 16);
			this.rbTransView.TabIndex = 1;
			this.rbTransView.Text = "Tran&scription View";
			// 
			// cboField
			// 
			this.cboField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboField.Items.AddRange(new object[] {
														  "Contents",
														  "Phonetic",
														  "Tone",
														  "Phonemic",
														  "Orthography",
														  "Gloss",
														  "Part of Speech",
														  "Reference",
														  "Freeform Translation",
														  "Comments",
														  "Transcriber",
														  "Notebook Reference",
														  "Speaker"});
			this.cboField.Location = new System.Drawing.Point(8, 16);
			this.cboField.Name = "cboField";
			this.cboField.Size = new System.Drawing.Size(152, 21);
			this.cboField.TabIndex = 0;
			this.cboField.SelectedIndexChanged += new System.EventHandler(this.cboField_SelectedIndexChanged);
			// 
			// ltbFindWhat
			// 
			this.ltbFindWhat.BackColor = System.Drawing.SystemColors.Control;
			this.ltbFindWhat.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ltbFindWhat.LabelText = "Find &What";
			this.ltbFindWhat.Location = new System.Drawing.Point(16, 64);
			this.ltbFindWhat.Name = "ltbFindWhat";
			this.ltbFindWhat.Size = new System.Drawing.Size(304, 40);
			this.ltbFindWhat.TabIndex = 3;
			// 
			// grpFindIn
			// 
			this.grpFindIn.Controls.Add(this.rbCurrDoc);
			this.grpFindIn.Controls.Add(this.rbAllDocs);
			this.grpFindIn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.grpFindIn.Location = new System.Drawing.Point(8, 112);
			this.grpFindIn.Name = "grpFindIn";
			this.grpFindIn.Size = new System.Drawing.Size(312, 48);
			this.grpFindIn.TabIndex = 4;
			this.grpFindIn.TabStop = false;
			this.grpFindIn.Text = "Find In";
			// 
			// rbCurrDoc
			// 
			this.rbCurrDoc.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbCurrDoc.Location = new System.Drawing.Point(136, 16);
			this.rbCurrDoc.Name = "rbCurrDoc";
			this.rbCurrDoc.Size = new System.Drawing.Size(128, 24);
			this.rbCurrDoc.TabIndex = 1;
			this.rbCurrDoc.Text = "C&urrent Document";
			// 
			// rbAllDocs
			// 
			this.rbAllDocs.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbAllDocs.Location = new System.Drawing.Point(16, 16);
			this.rbAllDocs.Name = "rbAllDocs";
			this.rbAllDocs.TabIndex = 0;
			this.rbAllDocs.Text = "&All Documents";
			// 
			// grpSpeaker
			// 
			this.grpSpeaker.Controls.Add(this.rbFemale);
			this.grpSpeaker.Controls.Add(this.rbMale);
			this.grpSpeaker.Controls.Add(this.cboSpeaker);
			this.grpSpeaker.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.grpSpeaker.Location = new System.Drawing.Point(8, 168);
			this.grpSpeaker.Name = "grpSpeaker";
			this.grpSpeaker.Size = new System.Drawing.Size(312, 48);
			this.grpSpeaker.TabIndex = 5;
			this.grpSpeaker.TabStop = false;
			this.grpSpeaker.Text = "Find Spea&ker";
			// 
			// rbFemale
			// 
			this.rbFemale.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbFemale.Location = new System.Drawing.Point(232, 16);
			this.rbFemale.Name = "rbFemale";
			this.rbFemale.Size = new System.Drawing.Size(64, 24);
			this.rbFemale.TabIndex = 2;
			this.rbFemale.Text = "Fem&ale";
			// 
			// rbMale
			// 
			this.rbMale.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbMale.Location = new System.Drawing.Point(168, 16);
			this.rbMale.Name = "rbMale";
			this.rbMale.Size = new System.Drawing.Size(64, 24);
			this.rbMale.TabIndex = 1;
			this.rbMale.Text = "&Male";
			// 
			// cboSpeaker
			// 
			this.cboSpeaker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSpeaker.Location = new System.Drawing.Point(8, 16);
			this.cboSpeaker.Name = "cboSpeaker";
			this.cboSpeaker.Size = new System.Drawing.Size(152, 21);
			this.cboSpeaker.TabIndex = 0;
			// 
			// chkMatchCase
			// 
			this.chkMatchCase.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkMatchCase.Location = new System.Drawing.Point(328, 184);
			this.chkMatchCase.Name = "chkMatchCase";
			this.chkMatchCase.Size = new System.Drawing.Size(88, 16);
			this.chkMatchCase.TabIndex = 6;
			this.chkMatchCase.Text = "Match &Case";
			// 
			// FindDlg
			// 
			this.AcceptButton = this.btnFind;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(410, 223);
			this.Controls.Add(this.chkMatchCase);
			this.Controls.Add(this.grpSpeaker);
			this.Controls.Add(this.grpFindIn);
			this.Controls.Add(this.ltbFindWhat);
			this.Controls.Add(this.grpField);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnFind);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Find Form";
			this.grpField.ResumeLayout(false);
			this.grpFindIn.ResumeLayout(false);
			this.grpSpeaker.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// This function will perform several form managment duties.
		/// The first thing is to determine whether or not we even
		/// need to be in here. If the find field (i.e. field shown
		/// in cboField combo. box) doesn't change then don't bother
		/// doing anything unless this function is called with the
		/// ForceUpd flag is True. The only time that is done is
		/// from the form load event.
		/// </summary>
		/// <param name="ForceUpd">Refer to summary.</param>
		private void UpdateForm(bool ForceUpd)
		{
			int Field;
			string newCap;
			try
			{
				if (!Visible) return;

				Field = cboField.SelectedIndex;
				if (!ForceUpd && Field == m_PrevFieldIndex) return;
				chkMatchCase.Enabled = true;
				m_PrevFieldIndex = Field;
				newCap = "Find ";

				if (Field == FindClass.SpkrIndex)
					newCap += "a ";
				else
				{
					newCap += "in";
					if (Field != FindClass.TreeIndex)
						newCap += " field";
					newCap += ": ";
				}
				newCap += cboField.SelectedItem.ToString();

				Text = newCap;

				// If (frmMain!Tree.Indent(frmMain!Tree.ListIndex) <> smcDocIndent Or _
				// 	iField = .TreeIndex Or iField = .SpeakerIndex Or _
				// 	iField = .NotebookRefIndex Or iField = .ScribeIndex) Then
				// 	optFindIn(0).Value = True
				// 	optFindIn(1).Enabled = False
				// Else
				// 	optFindIn(1).Enabled = True
				// End If

				// enable rbColView and rbTransView only for etic/emic/ortho
				// rbColView = rbTransView.Enabled =
				//									(Field == smFind.EticIndex) ||
				//									(Field == smFind.EmicIndex) ||
				//									(Field == smFind.OrthoIndex);

				// if etic/emic/ortho, and neither view is selected, then set based on
				// currently visible tab
				//	If ((iField = .EticIndex Or iField = .EmicIndex Or _
				//		 iField = .OrthoIndex) And Not optView(0).Value And _
				//		 Not optView(1).Value) Then _
				//		optView(IIf(frmMain.Tab1.Tab = 1, 1, 0)).Value = True
				// if etic/emic/tone disable match case since it's meaningless
				// chkMatchCase.Enabled = !((Field == smFind.EticIndex) ||
				//	(Field == smFind.ToneIndex) || (Field == smFind.EmicIndex));

				//	if (Field == smFind.SpeakerIndex)
				//	{
				//		ltbFindWhat.Enabled = false;
				//		cboSpeaker.Enabled = true;
				//		grpSpeaker.Enabled = true;
				//		chkMatchCase.Enabled = false;

				// if no speaker/gender specified, get from display data form & use as default.

				// if no speaker, then allow user to choose a gender
				//	}
				//	else
				//	{
				// the find field must not be speaker, so disable relevant controls
				// and enable "Find What"
				//		ltbFindWhat.Enabled = true;
				//		grpSpeaker.Enabled = false;
				//	}

				grpSpeaker.Enabled = (Field == FindClass.SpkrIndex);
				ltbFindWhat.Enabled = (Field != FindClass.SpkrIndex);
				rbColView.Enabled = rbTransView.Enabled = (Field == FindClass.EticIndex) ||
					(Field == FindClass.EmicIndex) || (Field == FindClass.OrthoIndex);
				rbCurrDoc.Enabled = (Field != FindClass.TreeIndex) &&
					(Field != FindClass.ScribeIndex) &&
					(Field != FindClass.NBRefIndex) &&
					(Field != FindClass.SpkrIndex);
				chkMatchCase.Enabled = (Field != FindClass.EticIndex) &&
					(Field != FindClass.ToneIndex) &&
					(Field != FindClass.EmicIndex) &&
					(Field != FindClass.SpkrIndex);
				if (!(rbColView.Checked) && !(rbTransView.Checked))
					rbTransView.Checked = true;
				if (!(rbAllDocs.Checked) && (!(rbCurrDoc.Checked) || !(rbCurrDoc.Enabled)))
					rbAllDocs.Checked = true;
				//	change ltbFindWhat.Font based on the field selected.
				switch (Field)
				{
					//Phonetic
					case FindClass.EticIndex:
						ltbFindWhat.Font = new Font(FontHelper.PhoneticFont.Name,10,
							(FontHelper.PhoneticFont.Italic ? FontStyle.Italic : FontStyle.Regular));
						break;
					//Tone
					case FindClass.ToneIndex:
						ltbFindWhat.Font = new Font(FontHelper.ToneFont.Name, 10,
							(FontHelper.ToneFont.Italic ? FontStyle.Italic : FontStyle.Regular));
						break;
					//Phonemic
					case FindClass.EmicIndex:
						ltbFindWhat.Font = new Font(FontHelper.PhonemicFont.Name, 10,
							(FontHelper.PhonemicFont.Italic ? FontStyle.Italic : FontStyle.Regular));
						break;
					//Orthography
					case FindClass.OrthoIndex:
						ltbFindWhat.Font = new Font(FontHelper.OrthograpicFont.Name, 10,
							(FontHelper.OrthograpicFont.Italic ? FontStyle.Italic : FontStyle.Regular));
						break;
					//Gloss
					case FindClass.GlossIndex:
						ltbFindWhat.Font = new Font(FontHelper.GlossFont.Name, 10,
							(FontHelper.GlossFont.Italic ? FontStyle.Italic : FontStyle.Regular));
						break;
					//Part of Speech
					case FindClass.POSIndex:
						ltbFindWhat.Font = new Font(FontHelper.PartOfSpeechFont.Name, 10,
							(FontHelper.PartOfSpeechFont.Italic ? FontStyle.Italic : FontStyle.Regular));
						break;
					//Contents
					//NoteBookRef
					//Freeform Translation
					//Comments
					//Transcriber
					//Notebook NoteBookRef
					//Speaker
					default:
						ltbFindWhat.Font = m_NormalFont;
						break;
				}
			}
			catch
			{
				//LogError(Name, "UpdateForm", True)
			}
		}

		private void cboField_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UpdateForm(false);
		}
	}
}
