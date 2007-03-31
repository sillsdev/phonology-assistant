using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Text;
using SIL.Pa.Database;

namespace SIL.Pa
{
	/// <summary>
	/// This form is supposed to allow the user to define classes, but due to problems
	/// with saving data back into the database, no modification / creation of classes
	/// is currently possible. Due to the automatic mapping from ansi to unicode by
	/// windows, calls Have to be made to System.Text.Encoding.Convert() in order to
	/// obtain the correct values for checking against SILIPACharNum in the table
	/// IPACharSet.
	/// </summary>
	public class Classes : System.Windows.Forms.Form
	{
		private ArrayList lblFeatCons;
		private ArrayList lblFeatVow;
		private ArrayList lblIPACon;
		private ArrayList lblIPAVow;
		private ArrayList lblIPANP;
		private ArrayList lblIPAOther;
		private ArrayList lblIPASSeg;
		private ArrayList lblIPATone;
		private ArrayList lblIPADia;
		private PaDataTable m_table;
		private const string m_DoubleBaseLinks = "\x83\xE9\xED";
		private PaDataTable m_ClassesTable;
		// private PaDataTable m_EticClassTable;
		// private OleDbDataAdapter m_adapter;
		/// <summary>
		/// PaDataTable class which encapsulates the table and adapter.
		/// </summary>
		private PaDataTable m_EticClassTable;
		private BinaryFeatureStruct[] m_BinFeatures;
		private IPAFeatureStruct[] m_IPAFeatures;
		private MultiStateListBox lstIPAFeat;
		private MultiStateListBox lstBinFeatures;
		private int m_ClassID; // PhoneticClassID in the table
		private int[] m_IPAMask = new int[4];
		private int[] m_BinaryMask = new int[2];

		#region Member variables added by designer
		private System.Windows.Forms.Label label1;
		private SIL.Pa.LeftLabelledBox llbClassName;
		private System.Windows.Forms.ComboBox cboClassType;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Panel pnlIPAChars;
		private System.Windows.Forms.GroupBox grpCons;
		private SIL.Pa.LeftLabelledBox llbIPAFeatures;
		private System.Windows.Forms.RadioButton rbOpAll;
		private System.Windows.Forms.RadioButton rbOpOne;
		private System.Windows.Forms.Panel pnlWildcard;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlConBinFeature;
		private System.Windows.Forms.GroupBox grpVowBinFeatures;
		private SIL.Pa.LeftLabelledBox llbFeatures;
		private System.Windows.Forms.GroupBox grpIPATone;
		private System.Windows.Forms.GroupBox grpIPASSeg;
		private System.Windows.Forms.GroupBox grpIPAVow;
		private System.Windows.Forms.GroupBox grpIPACon;
		private System.Windows.Forms.GroupBox grpIPAOther;
		private System.Windows.Forms.GroupBox grpIPANP;
		private System.Windows.Forms.GroupBox grpIPADia;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;
		#endregion

		public Classes()
		{
			lstIPAFeat = new MultiStateListBox();
			lstBinFeatures = new MultiStateListBox();
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			CreateLabels();
			cboClassType.SelectedIndex = 0;
			grpVowBinFeatures.Left = (this.ClientRectangle.Width -
				grpVowBinFeatures.Width) / 2;
			grpVowBinFeatures.Top = pnlIPAChars.Top;
			llbIPAFeatures.KeyPress += new KeyPressEventHandler(llbIPAFeatures_KeyPress);
			llbClassName.KeyPress += new KeyPressEventHandler(llbClassName_KeyPress);
			label3.MouseDown += new MouseEventHandler(lbl_MouseDown);
			label3.MouseUp += new MouseEventHandler(lbl_MouseUp);
			m_table.PrimaryKey = new DataColumn[] {m_table.Columns["SILIPACharNum"]};
			LoadEachConAndVowFeature();
			SetupIPAList();
			SetupBinaryList();
			llbFeatures.KeyPress += new KeyPressEventHandler(llbFeatures_KeyPress);
			SetupDatabaseUpdate();
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
			//SetMyState
			//BuildIPACharView
			m_ClassesTable.PrimaryKey = new DataColumn[] {m_ClassesTable.Columns["ClassName"]};
			LoadFeatureLists();
			base.OnLoad (e);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (DialogResult == DialogResult.OK)
			{
				if (!CheckIPAString())
					e.Cancel = true;
				else if (!VerifyClassDefined())
					e.Cancel = true;
				else if (llbClassName.Enabled)
					if (!VerifyClassName())
						e.Cancel = true;
				if (!e.Cancel) // we are clear to save this class
				{
					bool RowIsNew = false;
					DataRow row;
					if (!llbClassName.Enabled)
					{
						row = m_EticClassTable.Rows.Find(m_ClassID);
					}
					else
					{
						row = m_EticClassTable.NewRow();
						row["ClassName"] = llbClassName.Text;
						row["ShowInDefClassList"] = true;
						row["DisplayType"] = 1; // User Defined
						row["EditFlag"] = 2; // Allow edit and delete
						row["SortOrder"] = 999;
						// the order of classes in the combo box differs slightly from
						// those values defined in the database. classID = ((index + 2) % 4)
						row["ClassType"] = ((cboClassType.SelectedIndex + 2) % 4);
						RowIsNew = true;
					}
					row["ANDFeatures"] = rbOpAll.Checked;
					if (cboClassType.SelectedIndex == 1) // Articulatory Features
					{
						row["Mask0"] = m_IPAMask[0];
						row["Mask1"] = m_IPAMask[1];
						row["Mask2"] = m_IPAMask[2];
						row["Mask3"] = m_IPAMask[3];
					}
					else if (cboClassType.SelectedIndex == 0) // IPA Characters
					{
						row["IPAChars"] = llbIPAFeatures.Text;
					}
					else
					{
						row["BinaryMask0"] = m_BinaryMask[0];
						row["BinaryMask1"] = m_BinaryMask[1];
					}
					if (RowIsNew)
						m_EticClassTable.Rows.Add(row);
					m_EticClassTable.Commit();
				}
			}
			base.OnClosing (e);
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.llbClassName = new SIL.Pa.LeftLabelledBox();
			this.cboClassType = new System.Windows.Forms.ComboBox();
			this.pnlConBinFeature = new System.Windows.Forms.Panel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.grpCons = new System.Windows.Forms.GroupBox();
			this.pnlIPAChars = new System.Windows.Forms.Panel();
			this.llbIPAFeatures = new SIL.Pa.LeftLabelledBox();
			this.grpIPATone = new System.Windows.Forms.GroupBox();
			this.grpIPASSeg = new System.Windows.Forms.GroupBox();
			this.grpIPAVow = new System.Windows.Forms.GroupBox();
			this.grpIPACon = new System.Windows.Forms.GroupBox();
			this.grpIPAOther = new System.Windows.Forms.GroupBox();
			this.grpIPANP = new System.Windows.Forms.GroupBox();
			this.grpIPADia = new System.Windows.Forms.GroupBox();
			this.llbFeatures = new SIL.Pa.LeftLabelledBox();
			this.rbOpAll = new System.Windows.Forms.RadioButton();
			this.rbOpOne = new System.Windows.Forms.RadioButton();
			this.pnlWildcard = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpVowBinFeatures = new System.Windows.Forms.GroupBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.pnlConBinFeature.SuspendLayout();
			this.pnlIPAChars.SuspendLayout();
			this.pnlWildcard.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(129, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Define Class &Based On:";
			// 
			// llbClassName
			// 
			this.llbClassName.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.llbClassName.LabelText = "&Class Name:";
			this.llbClassName.Location = new System.Drawing.Point(8, 8);
			this.llbClassName.Name = "llbClassName";
			this.llbClassName.Size = new System.Drawing.Size(256, 24);
			this.llbClassName.TabIndex = 1;
			// 
			// cboClassType
			// 
			this.cboClassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboClassType.Items.AddRange(new object[] {
															  "Set of specified IPA characters",
															  "Set of specified articulatory features",
															  "Set of specified consonant binary features",
															  "Set of specified vowel binary features"});
			this.cboClassType.Location = new System.Drawing.Point(136, 32);
			this.cboClassType.Name = "cboClassType";
			this.cboClassType.Size = new System.Drawing.Size(200, 21);
			this.cboClassType.TabIndex = 2;
			this.cboClassType.SelectedIndexChanged += new System.EventHandler(this.cboClassType_SelectedIndexChanged);
			// 
			// pnlConBinFeature
			// 
			this.pnlConBinFeature.Controls.Add(this.groupBox2);
			this.pnlConBinFeature.Controls.Add(this.grpCons);
			this.pnlConBinFeature.Location = new System.Drawing.Point(8, 80);
			this.pnlConBinFeature.Name = "pnlConBinFeature";
			this.pnlConBinFeature.Size = new System.Drawing.Size(480, 208);
			this.pnlConBinFeature.TabIndex = 4;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.groupBox2.Location = new System.Drawing.Point(352, -4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(120, 216);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			// 
			// grpCons
			// 
			this.grpCons.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.grpCons.Location = new System.Drawing.Point(0, -4);
			this.grpCons.Name = "grpCons";
			this.grpCons.Size = new System.Drawing.Size(344, 216);
			this.grpCons.TabIndex = 0;
			this.grpCons.TabStop = false;
			// 
			// pnlIPAChars
			// 
			this.pnlIPAChars.Controls.Add(this.llbIPAFeatures);
			this.pnlIPAChars.Controls.Add(this.grpIPATone);
			this.pnlIPAChars.Controls.Add(this.grpIPASSeg);
			this.pnlIPAChars.Controls.Add(this.grpIPAVow);
			this.pnlIPAChars.Controls.Add(this.grpIPACon);
			this.pnlIPAChars.Controls.Add(this.grpIPAOther);
			this.pnlIPAChars.Controls.Add(this.grpIPANP);
			this.pnlIPAChars.Controls.Add(this.grpIPADia);
			this.pnlIPAChars.Location = new System.Drawing.Point(8, 64);
			this.pnlIPAChars.Name = "pnlIPAChars";
			this.pnlIPAChars.Size = new System.Drawing.Size(472, 320);
			this.pnlIPAChars.TabIndex = 5;
			// 
			// llbIPAFeatures
			// 
			this.llbIPAFeatures.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.llbIPAFeatures.LabelText = "&Members:";
			this.llbIPAFeatures.Font = new System.Drawing.Font("ASAP SILManuscript", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.llbIPAFeatures.Location = new System.Drawing.Point(8, 280);
			this.llbIPAFeatures.Name = "llbIPAFeatures";
			this.llbIPAFeatures.Size = new System.Drawing.Size(448, 32);
			this.llbIPAFeatures.TabIndex = 5;
			// 
			// grpIPATone
			// 
			this.grpIPATone.Location = new System.Drawing.Point(88, 160);
			this.grpIPATone.Name = "grpIPATone";
			this.grpIPATone.Size = new System.Drawing.Size(184, 112);
			this.grpIPATone.TabIndex = 4;
			this.grpIPATone.TabStop = false;
			this.grpIPATone.Text = "Tone && Accents";
			// 
			// grpIPASSeg
			// 
			this.grpIPASSeg.Location = new System.Drawing.Point(8, 160);
			this.grpIPASSeg.Name = "grpIPASSeg";
			this.grpIPASSeg.Size = new System.Drawing.Size(72, 112);
			this.grpIPASSeg.TabIndex = 3;
			this.grpIPASSeg.TabStop = false;
			this.grpIPASSeg.Text = "S. Seg.";
			// 
			// grpIPAVow
			// 
			this.grpIPAVow.Location = new System.Drawing.Point(328, 8);
			this.grpIPAVow.Name = "grpIPAVow";
			this.grpIPAVow.Size = new System.Drawing.Size(128, 152);
			this.grpIPAVow.TabIndex = 2;
			this.grpIPAVow.TabStop = false;
			this.grpIPAVow.Text = "Vowels";
			// 
			// grpIPACon
			// 
			this.grpIPACon.Location = new System.Drawing.Point(8, 8);
			this.grpIPACon.Name = "grpIPACon";
			this.grpIPACon.Size = new System.Drawing.Size(192, 152);
			this.grpIPACon.TabIndex = 0;
			this.grpIPACon.TabStop = false;
			this.grpIPACon.Text = "Consonants";
			// 
			// grpIPAOther
			// 
			this.grpIPAOther.Location = new System.Drawing.Point(208, 88);
			this.grpIPAOther.Name = "grpIPAOther";
			this.grpIPAOther.Size = new System.Drawing.Size(112, 72);
			this.grpIPAOther.TabIndex = 1;
			this.grpIPAOther.TabStop = false;
			this.grpIPAOther.Text = "Other";
			// 
			// grpIPANP
			// 
			this.grpIPANP.Location = new System.Drawing.Point(208, 8);
			this.grpIPANP.Name = "grpIPANP";
			this.grpIPANP.Size = new System.Drawing.Size(112, 72);
			this.grpIPANP.TabIndex = 1;
			this.grpIPANP.TabStop = false;
			this.grpIPANP.Text = "Non Pulmonics";
			// 
			// grpIPADia
			// 
			this.grpIPADia.Location = new System.Drawing.Point(280, 160);
			this.grpIPADia.Name = "grpIPADia";
			this.grpIPADia.Size = new System.Drawing.Size(176, 112);
			this.grpIPADia.TabIndex = 4;
			this.grpIPADia.TabStop = false;
			this.grpIPADia.Text = "Diacritics";
			// 
			// llbFeatures
			// 
			this.llbFeatures.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.llbFeatures.LabelText = "Members:";
			this.llbFeatures.Location = new System.Drawing.Point(8, 384);
			this.llbFeatures.Name = "llbFeatures";
			this.llbFeatures.Size = new System.Drawing.Size(472, 24);
			this.llbFeatures.TabIndex = 6;
			// 
			// rbOpAll
			// 
			this.rbOpAll.Location = new System.Drawing.Point(8, 408);
			this.rbOpAll.Name = "rbOpAll";
			this.rbOpAll.Size = new System.Drawing.Size(256, 16);
			this.rbOpAll.TabIndex = 7;
			this.rbOpAll.Text = "Data must include &all specified features(with)";
			// 
			// rbOpOne
			// 
			this.rbOpOne.Location = new System.Drawing.Point(8, 424);
			this.rbOpOne.Name = "rbOpOne";
			this.rbOpOne.Size = new System.Drawing.Size(296, 16);
			this.rbOpOne.TabIndex = 7;
			this.rbOpOne.Text = "Data must include &one or more specified features ( , )";
			// 
			// pnlWildcard
			// 
			this.pnlWildcard.Controls.Add(this.label3);
			this.pnlWildcard.Controls.Add(this.label2);
			this.pnlWildcard.Location = new System.Drawing.Point(8, 400);
			this.pnlWildcard.Name = "pnlWildcard";
			this.pnlWildcard.Size = new System.Drawing.Size(208, 24);
			this.pnlWildcard.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(27, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(176, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Wildcard (zero or more diacritics)";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("ASAP SILManuscript", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(2)));
			this.label3.Location = new System.Drawing.Point(8, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(16, 14);
			this.label3.TabIndex = 1;
			this.label3.Text = "*";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(336, 416);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 9;
			this.btnOk.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(416, 416);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Cancel";
			// 
			// grpVowBinFeatures
			// 
			this.grpVowBinFeatures.Location = new System.Drawing.Point(360, 192);
			this.grpVowBinFeatures.Name = "grpVowBinFeatures";
			this.grpVowBinFeatures.Size = new System.Drawing.Size(192, 150);
			this.grpVowBinFeatures.TabIndex = 10;
			this.grpVowBinFeatures.TabStop = false;
			// 
			// toolTip1
			// 
			this.toolTip1.AutomaticDelay = 100;
			// 
			// Classes
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(504, 445);
			this.Controls.Add(this.pnlIPAChars);
			this.Controls.Add(this.grpVowBinFeatures);
			this.Controls.Add(this.pnlConBinFeature);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.rbOpAll);
			this.Controls.Add(this.llbFeatures);
			this.Controls.Add(this.cboClassType);
			this.Controls.Add(this.llbClassName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.rbOpOne);
			this.Controls.Add(this.pnlWildcard);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "Classes";
			this.ShowInTaskbar = false;
			this.Text = "Define Class";
			this.pnlConBinFeature.ResumeLayout(false);
			this.pnlIPAChars.ResumeLayout(false);
			this.pnlWildcard.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void CreateLabels()
		{
			//We're not hard coding the characters for both grpCons and 
			//grpVowBinFeatures because unlike VB6 we are unable to add 
			//an array of labels in design so we used ArrayList to put all
			//the labels into an array. However by doing it in code, we
			//didn't put the characters in the layout as how VB6 did it.
			//not sure if that layout is crucial or can we do without it.

			//Arraylist of labels for grpCons
			lblFeatCons = new ArrayList();
			int left = 2, top = 2;
			string str = "pbtdÿêcïkgqG?mMn÷øN²õr{R}¸BfvTDszSZ§½CÆxÄXÒðÀhúÂLV¨Ójålñ´;";
			foreach (char ch in str)
			{
				Label lbl = new Label();
				lbl.Text = "" + ch;
				lbl.Font = PaApp.PhoneticFont;
				lbl.AutoSize = true;
				if (left + lbl.Width > grpCons.ClientRectangle.Width)
				{
					left = 2;
					top += PaApp.PhoneticFont.Height;
				}
				lbl.Location = new Point(left, top);
				lblFeatCons.Add(lbl);
				left += lbl.Width;
			}
			grpCons.SuspendLayout();
			grpCons.Controls.AddRange((Control[])lblFeatCons.ToArray(typeof(Label)));
			grpCons.ResumeLayout();

			//Array list of labels for grpVowBinFeatures
			lblFeatVow = new ArrayList();
			str = "iyö¬µuIYUeO?PFo«E¿ÎÏÃQ?a¯A";
			left = 2; top = 14;
			foreach (char ch in str)
			{
				Label lbl = new Label();
				lbl.Text = "" + ch;
				lbl.Font = PaApp.PhoneticFont;
				lbl.AutoSize = true;
			//	if (left + lbl.Width > grpVowBinFeatures.ClientRectangle.Width)
			//	{
			//		left = 2;
			//		top += PaApp.PhoneticFont.Height;
			//	}
			//	lbl.Location = new Point(left, top);
				lblFeatVow.Add(lbl);
			//	left += lbl.Width;
			}
			((Label)lblFeatVow[0]).Location = new Point(2, top);
			((Label)lblFeatVow[1]).Location = new Point(((Label)lblFeatVow[0]).Right, top);
			((Label)lblFeatVow[2]).Location = new Point(grpVowBinFeatures.ClientRectangle.Width / 2 -
				((Label)lblFeatVow[2]).Width, top);
			((Label)lblFeatVow[3]).Location = new Point(grpVowBinFeatures.ClientRectangle.Width / 2, top);
			((Label)lblFeatVow[4]).Location = new Point(grpVowBinFeatures.ClientRectangle.Right -
				(((Label)lblFeatVow[4]).Width + ((Label)lblFeatVow[5]).Width) - 2, top);
			((Label)lblFeatVow[5]).Location = new Point(((Label)lblFeatVow[4]).Right, top);
			top += PaApp.PhoneticFont.Height;
			((Label)lblFeatVow[6]).Location = new Point(((Label)lblFeatVow[1]).Right, top);
			((Label)lblFeatVow[7]).Location = new Point(((Label)lblFeatVow[6]).Right, top);
			((Label)lblFeatVow[8]).Location = new Point(((Label)lblFeatVow[3]).Right, top);
			top += PaApp.PhoneticFont.Height;
			((Label)lblFeatVow[9]).Location = new Point(2, top);
			((Label)lblFeatVow[10]).Location = new Point(((Label)lblFeatVow[9]).Right, top);
			((Label)lblFeatVow[11]).Location = new Point(grpVowBinFeatures.ClientRectangle.Width / 2 -
				((Label)lblFeatVow[11]).Width, top);
			((Label)lblFeatVow[12]).Location = new Point(grpVowBinFeatures.ClientRectangle.Width / 2, top);
			((Label)lblFeatVow[13]).Location = new Point(grpVowBinFeatures.ClientRectangle.Right -
				(((Label)lblFeatVow[13]).Width + ((Label)lblFeatVow[14]).Width) - 2, top);
			((Label)lblFeatVow[14]).Location = new Point(((Label)lblFeatVow[13]).Right, top);
			top += PaApp.PhoneticFont.Height;
			((Label)lblFeatVow[15]).Location = new Point((grpVowBinFeatures.ClientRectangle.Width -
				((Label)lblFeatVow[15]).Width) / 2, top);
			top += PaApp.PhoneticFont.Height;
			((Label)lblFeatVow[16]).Location = new Point(2, top);
			((Label)lblFeatVow[17]).Location = new Point(((Label)lblFeatVow[16]).Right, top);
			((Label)lblFeatVow[18]).Location = new Point(grpVowBinFeatures.ClientRectangle.Width / 2 -
				((Label)lblFeatVow[18]).Width, top);
			((Label)lblFeatVow[19]).Location = new Point(grpVowBinFeatures.ClientRectangle.Width / 2, top);
			((Label)lblFeatVow[20]).Location = new Point(grpVowBinFeatures.ClientRectangle.Right -
				(((Label)lblFeatVow[20]).Width + ((Label)lblFeatVow[21]).Width) - 2, top);
			((Label)lblFeatVow[21]).Location = new Point(((Label)lblFeatVow[20]).Right, top);
			top += PaApp.PhoneticFont.Height;
			((Label)lblFeatVow[22]).Location = new Point(((Label)lblFeatVow[16]).Right, top);
			((Label)lblFeatVow[23]).Location = new Point(((Label)lblFeatVow[18]).Right, top);
			top += PaApp.PhoneticFont.Height;
			((Label)lblFeatVow[24]).Location = new Point(2, top);
			((Label)lblFeatVow[25]).Location = new Point(((Label)lblFeatVow[24]).Right, top);
			((Label)lblFeatVow[26]).Location = new Point(grpVowBinFeatures.ClientRectangle.Right -
				(((Label)lblFeatVow[26]).Width + ((Label)lblFeatVow[27]).Width) - 2, top);
			((Label)lblFeatVow[27]).Location = new Point(((Label)lblFeatVow[26]).Right, top);
			grpVowBinFeatures.SuspendLayout();
			grpVowBinFeatures.Height = ((Label)lblFeatVow[27]).Bottom + 4;
			grpVowBinFeatures.Controls.AddRange((Control[])lblFeatVow.ToArray(typeof(Label)));
			grpVowBinFeatures.ResumeLayout();

			///--------------------------------------------------------
			///The following 6 while loops are meant to assign every 
			///phone to their respective group box according to
			///their char type. The chars are extrated from the database
			///Of course this is a very crude way of doing it and should
			///be refinded some what in the future. This seems to be very
			///memory intensive too as alot of labels are created.
			///--------------------------------------------------------
			m_table = new PaDataTable("SELECT DISTINCTROW IPAChar, CharType, IsBaseChar," +
				"IPAName, IPADesc, DsplyOrder, DsplyWEmSpace, " +
				"SILIPACharNum FROM IPACharSet WHERE (CharType > 0) " +
				"ORDER BY CharType, DsplyOrder;");
		
			//Labels for grpIPACon
			int iCharType, i = 0;
			lblIPACon = new ArrayList();
			left = 2; top = 14;
			iCharType = (byte)m_table.Rows[i]["CharType"];
			while ((byte)m_table.Rows[i]["CharType"] == iCharType)
			{
				Label lbl = new Label();
				lbl.Text = m_table.Rows[i]["IPAChar"].ToString();
				lbl.Font = PaApp.PhoneticFont;
				lbl.AutoSize = true;
				if (left + lbl.Width > grpIPACon.ClientRectangle.Width)
				{
					left = 2;
					top += PaApp.PhoneticFont.Height;
				}
				lbl.Location = new Point(left, top);
				toolTip1.SetToolTip(lbl, "" + m_table.Rows[i]["IPADesc"]);
				lbl.MouseDown += new MouseEventHandler(lbl_MouseDown);
				lbl.MouseUp += new MouseEventHandler(lbl_MouseUp);
				lblIPACon.Add(lbl);
				left += lbl.Width;
				i++;
			}
			grpIPACon.SuspendLayout();
			grpIPACon.Controls.AddRange((Control[])lblIPACon.ToArray(typeof(Label)));
			grpIPACon.ResumeLayout();

			//labels for grpIPAVow
			lblIPAVow = new ArrayList();
			left = 2; top = 14;
			iCharType = (byte)m_table.Rows[i]["CharType"];
			while ((byte)m_table.Rows[i]["CharType"] == iCharType)
			{
				Label lbl = new Label();
				lbl.Text = m_table.Rows[i]["IPAChar"].ToString();
				lbl.Font = PaApp.PhoneticFont;
				lbl.AutoSize = true;
				if (left + lbl.Width > grpIPAVow.ClientRectangle.Width)
				{
					left = 2;
					top += PaApp.PhoneticFont.Height;
				}
				lbl.Location = new Point(left, top);
				toolTip1.SetToolTip(lbl, "" + m_table.Rows[i]["IPADesc"]);
				lbl.MouseDown += new MouseEventHandler(lbl_MouseDown);
				lbl.MouseUp += new MouseEventHandler(lbl_MouseUp);
				lblIPAVow.Add(lbl);
				left += lbl.Width;
				i++;
			}
			grpIPAVow.SuspendLayout();
			grpIPAVow.Controls.AddRange((Control[])lblIPAVow.ToArray(typeof(Label)));
			grpIPAVow.ResumeLayout();

			//labels for grpIPANP
			lblIPANP = new ArrayList();
			left = 2; top = 14;
			iCharType = (byte)m_table.Rows[i]["CharType"];
			while ((byte)m_table.Rows[i]["CharType"] == iCharType)
			{
				Label lbl = new Label();
				lbl.Text = m_table.Rows[i]["IPAChar"].ToString();
				lbl.Font = PaApp.PhoneticFont;
				lbl.AutoSize = true;
				if (left + lbl.Width > grpIPANP.ClientRectangle.Width)
				{
					left = 2;
					top += PaApp.PhoneticFont.Height;
				}
				lbl.Location = new Point(left, top);
				toolTip1.SetToolTip(lbl, "" + m_table.Rows[i]["IPADesc"]);
				lbl.MouseDown += new MouseEventHandler(lbl_MouseDown);
				lbl.MouseUp += new MouseEventHandler(lbl_MouseUp);
				lblIPANP.Add(lbl);
				left += lbl.Width;
				i++;
			}
			grpIPANP.SuspendLayout();
			grpIPANP.Controls.AddRange((Control[])lblIPANP.ToArray(typeof(Label)));
			grpIPANP.ResumeLayout();

			//labels for grpIPAOther
			lblIPAOther = new ArrayList();
			left = 2; top = 14;
			iCharType = (byte)m_table.Rows[i]["CharType"];
			while ((byte)m_table.Rows[i]["CharType"] == iCharType)
			{
				Label lbl = new Label();
				lbl.Text = m_table.Rows[i]["IPAChar"].ToString();
				if ((bool)m_table.Rows[i]["DsplyWEmSpace"])
					lbl.Text = " " + lbl.Text;
				if ((byte)m_table.Rows[i]["SILIPACharNum"] == 0xE9) // bottom tie
					lbl.Text = "\"" + lbl.Text + "\"";
				if ((byte)m_table.Rows[i]["SILIPACharNum"] == 0x83) // top tie
					lbl.Text += "\"";
				lbl.Font = PaApp.PhoneticFont;
				lbl.AutoSize = true;
				if (left + lbl.Width > grpIPAOther.ClientRectangle.Width)
				{
					left = 2;
					top += PaApp.PhoneticFont.Height + 2;
				}
				lbl.Location = new Point(left, top);
				toolTip1.SetToolTip(lbl, "" + m_table.Rows[i]["IPADesc"]);
				lbl.MouseDown += new MouseEventHandler(lbl_MouseDown);
				lbl.MouseUp += new MouseEventHandler(lbl_MouseUp);
				lblIPAOther.Add(lbl);
				left += lbl.Width;
				i++;
			}
			grpIPAOther.SuspendLayout();
			grpIPAOther.Controls.AddRange((Control[])lblIPAOther.ToArray(typeof(Label)));
			grpIPAOther.ResumeLayout();

			//labels for grpIPASSeg
			lblIPASSeg = new ArrayList();
			left = 2; top = 14;
			iCharType = (byte)m_table.Rows[i]["CharType"];
			while ((byte)m_table.Rows[i]["CharType"] == iCharType)
			{
				Label lbl = new Label();
				lbl.Text = m_table.Rows[i]["IPAChar"].ToString();
				if ((bool)m_table.Rows[i]["DsplyWEmSpace"])
					lbl.Text = " " + lbl.Text;
				lbl.Font = PaApp.PhoneticFont;
				lbl.AutoSize = true;
				if (left + lbl.Width > grpIPASSeg.ClientRectangle.Width)
				{
					left = 2;
					top += PaApp.PhoneticFont.Height;
				}
				lbl.Location = new Point(left, top);
				toolTip1.SetToolTip(lbl, "" + m_table.Rows[i]["IPADesc"]);
				lbl.MouseDown += new MouseEventHandler(lbl_MouseDown);
				lbl.MouseUp += new MouseEventHandler(lbl_MouseUp);
				lblIPASSeg.Add(lbl);
				left += lbl.Width;
				i++;
			}
			grpIPASSeg.SuspendLayout();
			grpIPASSeg.Controls.AddRange((Control[])lblIPASSeg.ToArray(typeof(Label)));
			grpIPASSeg.ResumeLayout();

			//labels for grpIPATone
			lblIPATone = new ArrayList();
			left = 2; top = 14;
			iCharType = (byte)m_table.Rows[i]["CharType"];
			while ((byte)m_table.Rows[i]["CharType"] == iCharType)
			{
				Label lbl = new Label();
				lbl.Text = m_table.Rows[i]["IPAChar"].ToString();
				if ((bool)m_table.Rows[i]["DsplyWEmSpace"])
					lbl.Text = " " + lbl.Text;
				lbl.Font = PaApp.PhoneticFont;
				lbl.AutoSize = true;
				if (left + lbl.Width > grpIPATone.ClientRectangle.Width)
				{
					left = 2;
					top += PaApp.PhoneticFont.Height + 2;
				}
				lbl.Location = new Point(left, top);
				toolTip1.SetToolTip(lbl, "" + m_table.Rows[i]["IPADesc"]);
				lbl.MouseDown += new MouseEventHandler(lbl_MouseDown);
				lbl.MouseUp += new MouseEventHandler(lbl_MouseUp);
				lblIPATone.Add(lbl);
				left += lbl.Width;
				i++;
			}
			grpIPATone.SuspendLayout();
			grpIPATone.Controls.AddRange((Control[])lblIPATone.ToArray(typeof(Label)));
			grpIPATone.ResumeLayout();

			//labels for grpIPADia
			lblIPADia = new ArrayList();
			left = 2; top = 14;
			iCharType = (byte)m_table.Rows[i]["CharType"];
			while ((i < m_table.Rows.Count) &&
				((byte)m_table.Rows[i]["CharType"] == iCharType))
			{
				Label lbl = new Label();
				lbl.Text = m_table.Rows[i]["IPAChar"].ToString();
				if ((bool)m_table.Rows[i]["DsplyWEmSpace"])
					lbl.Text = " " + lbl.Text;
				if ((byte)m_table.Rows[i]["SILIPACharNum"] == 0xD5) // rhotic hook
					lbl.Text = "\"" + lbl.Text + "\"";
				lbl.Font = PaApp.PhoneticFont;
				lbl.AutoSize = true;
				if (left + lbl.Width > grpIPADia.ClientRectangle.Width)
				{
					left = 2;
					top += PaApp.PhoneticFont.Height;
				}
				lbl.Location = new Point(left, top);
				toolTip1.SetToolTip(lbl, "" + m_table.Rows[i]["IPADesc"]);
				lbl.MouseDown += new MouseEventHandler(lbl_MouseDown);
				lbl.MouseUp += new MouseEventHandler(lbl_MouseUp);
				lblIPADia.Add(lbl);
				left += lbl.Width;
				i++;
			}
			grpIPADia.SuspendLayout();
			grpIPADia.Controls.AddRange((Control[])lblIPADia.ToArray(typeof(Label)));
			grpIPADia.ResumeLayout();
		}

		/// <summary>
		/// Functions pretty much the same as in VB6, but the characters
		/// need to be converted from unicode to ansi before comparing with
		/// the byte value of "SILIPACharNum", particularly for characters
		/// inside the range 0x80 to 0x9F.
		/// </summary>
		/// <param name="ch">The phone clicked or key pressed.</param>
		/// <returns>The string to be inserted, may be an empty string.</returns>
		private string PrsIPAChar(char ch)
		{
			byte[] chBytes = Encoding.Unicode.GetBytes(new char[]{ch});
			byte[] PrevBytes, NextBytes;
			bool bCharBase = false, bPrevBase = false, bNextBase = false;
			char PrevChr = '\0', NextChr = '\0';
			if (!llbIPAFeatures.Focused)
				llbIPAFeatures.Focus();
			if (llbIPAFeatures.TextBox.SelectionStart > 0)
				PrevChr = llbIPAFeatures.Text[llbIPAFeatures.TextBox.SelectionStart - 1];
			if (llbIPAFeatures.TextBox.SelectionStart < llbIPAFeatures.Text.Length)
				NextChr = llbIPAFeatures.Text[llbIPAFeatures.TextBox.SelectionStart];
			if (ch == ',')
			{
				if (PrevChr == '\0' || PrevChr == ',' || NextChr == ',')
					return "";
				else return ",";
			}
			else if (ch == '*')
			{
				if (PrevChr == '\0' || PrevChr == '*' || NextChr == '*')
					return "";
				if ((NextChr != '\0' && NextChr != ',') || PrevChr == ',')
					return "";
			}
			if (ch != '*')
			{
				chBytes = Encoding.Convert(Encoding.Unicode, Encoding.Default, chBytes);
				DataRow row = m_table.Rows.Find((int)chBytes[0]);
				if (row == null)
					return "";
				else
					bCharBase = (bool) row["IsBaseChar"];
			}
			if (PrevChr != '\0')
			{
				PrevBytes = Encoding.Convert(Encoding.Unicode, Encoding.Default,
					Encoding.Unicode.GetBytes("" + PrevChr));
				DataRow row = m_table.Rows.Find((int)PrevBytes[0]);
				if (row != null) bPrevBase = (bool) row["IsBaseChar"];
			}
			if (NextChr != '\0')
			{
				NextBytes = Encoding.Convert(Encoding.Unicode, Encoding.Default,
					Encoding.Unicode.GetBytes("" + NextChr));
				DataRow row = m_table.Rows.Find((int)NextBytes[0]);
				if (row != null) bNextBase = (bool) row["IsBaseChar"];
			}
			if ((ch == '*') && (NextChr == '\0'))
				return "" + ch;
			if (!bCharBase)
			{
				if ((m_DoubleBaseLinks.IndexOf(ch) >= 0) && !bPrevBase)
					return "";
				if ((PrevChr == '\0') || (PrevChr == ','))
					return "";
				if (PrevChr == '*')
					return "";
				if (m_DoubleBaseLinks.IndexOf(PrevChr) >= 0)
					return "";
			}
			else
			{
				if ((PrevChr != '\0') && (PrevChr != ','))
					return "," + ch;
				if (bNextBase)
					return ch + ",";
			}
			return "" + ch;
		}

		private bool CheckIPAString()
		{
			char cBase = '1', cNonBase = '0', cCurrChar;
			string sChkString = "", sErrMsg;
			int i;
			llbIPAFeatures.Text = llbIPAFeatures.Text.Trim();
			// instead of the if-then used in vb6, just trim any leading and
			// trailing commas.
			llbIPAFeatures.Text = llbIPAFeatures.Text.Trim(new char[] {','});
			for (i = 0; i < llbIPAFeatures.Text.Length; i++)
			{
				cCurrChar = llbIPAFeatures.Text[i];
				if (cCurrChar ==  ',' || cCurrChar == '*')
					sChkString += cCurrChar;
				else
				{
					byte[] chBytes = Encoding.Unicode.GetBytes(new char[] {cCurrChar});
                    chBytes = Encoding.Convert(Encoding.Unicode, Encoding.Default, chBytes);
					DataRow row = m_table.Rows.Find((int)chBytes[0]);
					if (row == null)
						sChkString += " ";
					else
						sChkString += (bool)row["IsBaseChar"] ? cBase : cNonBase;
				}
			}

			if (sChkString[0] == cNonBase || sChkString[0] == '*')
				sErrMsg = "You cannot begin the string with a non base character or a wildcard.";
			else
			{
				i = sChkString.IndexOf(",,");
				if (i >= 0)
					sErrMsg = "You cannot have two commas in a row.";
				else
				{
					i = sChkString.IndexOf("," + cNonBase);
					if (i >= 0)
						sErrMsg = "You cannot have a comma followed by a non base character.";
					else
					{
						i = sChkString.IndexOf(",*");
						if (i >= 0)
							sErrMsg = "You cannot have a comma followed by a wildcard.";
						else
						{
							i = sChkString.IndexOf("*" + cNonBase);
							if (i >= 0)
								sErrMsg = "You cannot have a wildcard followed by a non base character.";
							else
							{
								i = sChkString.IndexOf("**");
								if (i >= 0)
									sErrMsg = "You cannot have two wildcards in a row.";
								else
									return true;
							}
						}
					}
				}
			}

			MessageBox.Show(this, "Invalid IPA String: " + sErrMsg, Application.ProductName,
				MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			llbIPAFeatures.Focus();
			return false;
		}

		private bool VerifyClassDefined()
		{
			string sErrMsg = "";

			if (cboClassType.SelectedIndex == 0)
			{
				if (llbIPAFeatures.Text.Length == 0)
					sErrMsg = "You must specify one or more IPA characters.";

			}
			else
			{
				if (llbFeatures.Text.Length == 0)
					sErrMsg = "You must specify one or more features.";
			}

			if (sErrMsg.Length > 0)
			{
				MessageBox.Show(this,sErrMsg,Application.ProductName,MessageBoxButtons.OK,
					MessageBoxIcon.Information);
				return false;
			}

			return true;
		}

		private bool VerifyClassName()
		{
			llbClassName.Text = llbClassName.Text.Trim();
			if (llbClassName.Text.Length == 0)
			{
				MessageBox.Show(this, "You must specify a class name.", Application.ProductName,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				llbClassName.Focus();
				return false;
			}

			if (m_ClassesTable.Rows.Contains(llbClassName.Text))
			{
				MessageBox.Show(this, "'" + llbClassName.Text + "' already exists.",
					Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				llbClassName.Focus();
				llbClassName.TextBox.SelectAll();
				return false;
			}

			return true;
		}

		private void LoadEachConAndVowFeature()
		{
			PaDataTable table = new PaDataTable("SELECT * FROM IPACharSet");
			table.PrimaryKey = new DataColumn[] {table.Columns["SILIPACharNum"]};
			foreach (object obj in lblFeatCons)
			{
				Label lbl = obj as Label;
				byte[] chBytes = Encoding.Unicode.GetBytes(lbl.Text);
				chBytes = Encoding.Convert(Encoding.Unicode, Encoding.Default, chBytes);
				DataRow row = table.Rows.Find((int)chBytes[0]);
				if (row != null)
				{
					FeatureMasks fm = new FeatureMasks();
					fm.BinaryMask0 = (int) row["BinaryMask0"];
					fm.BinaryMask1 = (int) row["BinaryMask1"];
					lbl.Tag = fm;
				}
			}
			foreach (object obj in lblFeatVow)
			{
				Label lbl = obj as Label;
				byte[] chBytes = Encoding.Unicode.GetBytes(lbl.Text);
				chBytes = Encoding.Convert(Encoding.Unicode, Encoding.Default, chBytes);
				DataRow row = table.Rows.Find((int)chBytes[0]);
				if (row != null)
				{
					FeatureMasks fm = new FeatureMasks();
					fm.BinaryMask0 = (int) row["BinaryMask0"];
					fm.BinaryMask1 = (int) row["BinaryMask1"];
					lbl.Tag = fm;
				}
			}
		}

		private void SetupIPAList()
		{
			lstIPAFeat.Name = "lstIPAFeat";
			lstIPAFeat.Location = new Point(label1.Left, label1.Bottom);
			lstIPAFeat.Size = new Size(llbFeatures.Width, llbFeatures.Top - label1.Bottom);
			lstIPAFeat.MultiColumn = true;
			lstIPAFeat.MultiStateType = MultiStateListBox.MultiStateTypes.CheckBox;
			lstIPAFeat.AllowStateEdit = true;
			lstIPAFeat.AllowLabelEdit = false;
			this.Controls.Add(lstIPAFeat);
		}

		private void SetupBinaryList()
		{
			lstBinFeatures.Name = "lstBinFeatures";
			lstBinFeatures.Location = new Point(pnlConBinFeature.Left, pnlConBinFeature.Bottom);
			lstBinFeatures.Size = new Size(pnlConBinFeature.Width, llbFeatures.Top - pnlConBinFeature.Bottom);
			lstBinFeatures.MultiColumn = true;
			lstBinFeatures.MultiStateType = MultiStateListBox.MultiStateTypes.TriStatePlusMinus;
			lstBinFeatures.AllowStateEdit = true;
			lstBinFeatures.AllowLabelEdit = false;
			this.Controls.Add(lstBinFeatures);
		}

		private void LoadFeatureLists()
		{
			foreach (BinaryFeatureStruct bfs in m_BinFeatures)
			{
				lstBinFeatures.Items.Add(bfs.FeatureName);
			}
			foreach (IPAFeatureStruct ifs in m_IPAFeatures)
			{
				lstIPAFeat.Items.Add(ifs.FeatureName);
			}
		}

		public void SetupDatabaseUpdate()
		{
			m_EticClassTable = new PaDataTable("SELECT * FROM PhoneticClass");
			m_EticClassTable.InsertCommand = new OleDbCommand("INSERT INTO PhoneticClass " +
				"(ShowInDefClassList, DisplayType, ClassType, ClassName, SortOrder, EditFlag, IPAChars, " +
				"Mask0, Mask1, Mask2, Mask3, BinaryMask0, BinaryMask1, AllConClass, AllVowClass, BaseConClass, " +
				"BaseVowClass, ANDFeatures) VALUES " +
				"(@ShowInDefClassList, @DisplayType, @ClassType, @ClassName, @SortOrder, @EditFlag, @IPAChars, " +
				"@Mask0, @Mask1, @Mask2, @Mask3, @BinaryMask0, @BinaryMask1, @AllConClass, @AllVowClass, @BaseConClass, " +
				"@BaseVowClass, @ANDFeatures);", DBUtils.DatabaseConnection);
			m_EticClassTable.InsertCommand.Parameters.Add("@ShowInDefClassList", DBUtils.ConvertToDbType(m_EticClassTable.Columns["ShowInDefClassList"].DataType), 1, "ShowInDefClassList");
			m_EticClassTable.InsertCommand.Parameters.Add("@DisplayType", DBUtils.ConvertToDbType(m_EticClassTable.Columns["DisplayType"].DataType), 1, "DisplayType");
			m_EticClassTable.InsertCommand.Parameters.Add("@ClassType", DBUtils.ConvertToDbType(m_EticClassTable.Columns["ClassType"].DataType), 1, "ClassType");
			m_EticClassTable.InsertCommand.Parameters.Add("@ClassName", DBUtils.ConvertToDbType(m_EticClassTable.Columns["ClassName"].DataType), 50, "ClassName");
			m_EticClassTable.InsertCommand.Parameters.Add("@SortOrder", DBUtils.ConvertToDbType(m_EticClassTable.Columns["SortOrder"].DataType), 1, "SortOrder");
			m_EticClassTable.InsertCommand.Parameters.Add("@EditFlag", DBUtils.ConvertToDbType(m_EticClassTable.Columns["EditFlag"].DataType), 1, "EditFlag");
			m_EticClassTable.InsertCommand.Parameters.Add("@IPAChars", DBUtils.ConvertToDbType(m_EticClassTable.Columns["IPAChars"].DataType), 255, "IPAChars");
			m_EticClassTable.InsertCommand.Parameters.Add("@Mask0", DBUtils.ConvertToDbType(m_EticClassTable.Columns["Mask0"].DataType), 1, "Mask0");
			m_EticClassTable.InsertCommand.Parameters.Add("@Mask1", DBUtils.ConvertToDbType(m_EticClassTable.Columns["Mask1"].DataType), 1, "Mask1");
			m_EticClassTable.InsertCommand.Parameters.Add("@Mask2", DBUtils.ConvertToDbType(m_EticClassTable.Columns["Mask2"].DataType), 1, "Mask2");
			m_EticClassTable.InsertCommand.Parameters.Add("@Mask3", DBUtils.ConvertToDbType(m_EticClassTable.Columns["Mask3"].DataType), 1, "Mask3");
			m_EticClassTable.InsertCommand.Parameters.Add("@BinaryMask0", DBUtils.ConvertToDbType(m_EticClassTable.Columns["BinaryMask0"].DataType), 1, "BinaryMask0");
			m_EticClassTable.InsertCommand.Parameters.Add("@BinaryMask1", DBUtils.ConvertToDbType(m_EticClassTable.Columns["BinaryMask1"].DataType), 1, "BinaryMask1");
			m_EticClassTable.InsertCommand.Parameters.Add("@AllConClass", DBUtils.ConvertToDbType(m_EticClassTable.Columns["AllConClass"].DataType), 1, "AllConClass");
			m_EticClassTable.InsertCommand.Parameters.Add("@AllVowClass", DBUtils.ConvertToDbType(m_EticClassTable.Columns["AllVowClass"].DataType), 1, "AllVowClass");
			m_EticClassTable.InsertCommand.Parameters.Add("@BaseConClass", DBUtils.ConvertToDbType(m_EticClassTable.Columns["BaseConClass"].DataType), 1, "BaseConClass");
			m_EticClassTable.InsertCommand.Parameters.Add("@BaseVowClass", DBUtils.ConvertToDbType(m_EticClassTable.Columns["BaseVowClass"].DataType), 1, "BaseVowClass");
			m_EticClassTable.InsertCommand.Parameters.Add("@ANDFeatures", DBUtils.ConvertToDbType(m_EticClassTable.Columns["ANDFeatures"].DataType), 1, "ANDFeatures");
			m_EticClassTable.UpdateCommand = new OleDbCommand("UPDATE PhoneticClass " +
				"SET ShowInDefClassList = @ShowInDefClassList, DisplayType = @DisplayType, ClassType = @ClassType, " +
				"ClassName = @ClassName, SortOrder = @SortOrder, EditFlag = @EditFlag, IPAChars = @IPAChars, " +
				"Mask0 = @Mask0, Mask1 = @Mask1, Mask2 = @Mask2, Mask3 = @Mask3, BinaryMask0 = @BinaryMask0, " +
				"BinaryMask1 = @BinaryMask1, AllConClass = @AllConClass, AllVowClass = @AllVowClass, " +
				"BaseConClass = @BaseConClass, BaseVowClass = @BaseVowClass, ANDFeatures = @ANDFeatures " +
				"WHERE PhoneticClassID = @PhoneticClassID;", DBUtils.DatabaseConnection);
			m_EticClassTable.UpdateCommand.Parameters.Add("@PhoneticClassID", DBUtils.ConvertToDbType(m_EticClassTable.Columns["PhoneticClassID"].DataType), 1, "PhoneticClassID");
			m_EticClassTable.UpdateCommand.Parameters.Add("@ShowInDefClassList", DBUtils.ConvertToDbType(m_EticClassTable.Columns["ShowInDefClassList"].DataType), 1, "ShowInDefClassList");
			m_EticClassTable.UpdateCommand.Parameters.Add("@DisplayType", DBUtils.ConvertToDbType(m_EticClassTable.Columns["DisplayType"].DataType), 1, "DisplayType");
			m_EticClassTable.UpdateCommand.Parameters.Add("@ClassType", DBUtils.ConvertToDbType(m_EticClassTable.Columns["ClassType"].DataType), 1, "ClassType");
			m_EticClassTable.UpdateCommand.Parameters.Add("@ClassName", DBUtils.ConvertToDbType(m_EticClassTable.Columns["ClassName"].DataType), 50, "ClassName");
			m_EticClassTable.UpdateCommand.Parameters.Add("@SortOrder", DBUtils.ConvertToDbType(m_EticClassTable.Columns["SortOrder"].DataType), 1, "SortOrder");
			m_EticClassTable.UpdateCommand.Parameters.Add("@EditFlag", DBUtils.ConvertToDbType(m_EticClassTable.Columns["EditFlag"].DataType), 1, "EditFlag");
			m_EticClassTable.UpdateCommand.Parameters.Add("@IPAChars", DBUtils.ConvertToDbType(m_EticClassTable.Columns["IPAChars"].DataType), 255, "IPAChars");
			m_EticClassTable.UpdateCommand.Parameters.Add("@Mask0", DBUtils.ConvertToDbType(m_EticClassTable.Columns["Mask0"].DataType), 1, "Mask0");
			m_EticClassTable.UpdateCommand.Parameters.Add("@Mask1", DBUtils.ConvertToDbType(m_EticClassTable.Columns["Mask1"].DataType), 1, "Mask1");
			m_EticClassTable.UpdateCommand.Parameters.Add("@Mask2", DBUtils.ConvertToDbType(m_EticClassTable.Columns["Mask2"].DataType), 1, "Mask2");
			m_EticClassTable.UpdateCommand.Parameters.Add("@Mask3", DBUtils.ConvertToDbType(m_EticClassTable.Columns["Mask3"].DataType), 1, "Mask3");
			m_EticClassTable.UpdateCommand.Parameters.Add("@BinaryMask0", DBUtils.ConvertToDbType(m_EticClassTable.Columns["BinaryMask0"].DataType), 1, "BinaryMask0");
			m_EticClassTable.UpdateCommand.Parameters.Add("@BinaryMask1", DBUtils.ConvertToDbType(m_EticClassTable.Columns["BinaryMask1"].DataType), 1, "BinaryMask1");
			m_EticClassTable.UpdateCommand.Parameters.Add("@AllConClass", DBUtils.ConvertToDbType(m_EticClassTable.Columns["AllConClass"].DataType), 1, "AllConClass");
			m_EticClassTable.UpdateCommand.Parameters.Add("@AllVowClass", DBUtils.ConvertToDbType(m_EticClassTable.Columns["AllVowClass"].DataType), 1, "AllVowClass");
			m_EticClassTable.UpdateCommand.Parameters.Add("@BaseConClass", DBUtils.ConvertToDbType(m_EticClassTable.Columns["BaseConClass"].DataType), 1, "BaseConClass");
			m_EticClassTable.UpdateCommand.Parameters.Add("@BaseVowClass", DBUtils.ConvertToDbType(m_EticClassTable.Columns["BaseVowClass"].DataType), 1, "BaseVowClass");
			m_EticClassTable.UpdateCommand.Parameters.Add("@ANDFeatures", DBUtils.ConvertToDbType(m_EticClassTable.Columns["ANDFeatures"].DataType), 1, "ANDFeatures");
			// m_adapter.MissingSchemaAction = MissingSchemaAction.Add;
			m_EticClassTable.DeleteCommand = new OleDbCommand("DELETE FROM PhoneticClass WHERE PhoneticClassID = @PhoneticClassID;", DBUtils.DatabaseConnection);
			m_EticClassTable.DeleteCommand.Parameters.Add("@PhoneticClassID", DBUtils.ConvertToDbType(m_EticClassTable.Columns["PhoneticClassID"].DataType), 1, "PhoneticClassID");
		}

//	Shifted over to PaApp.cs as a static method.
//		private OleDbType ConvertToDbType(System.Type whatType)
//		{
//			switch (System.Type.GetTypeCode(whatType))
//			{
//				case TypeCode.UInt64: return OleDbType.UnsignedBigInt;
//				case TypeCode.UInt32: return OleDbType.UnsignedInt;
//				case TypeCode.UInt16: return OleDbType.UnsignedSmallInt;
//				case TypeCode.Byte: return OleDbType.UnsignedTinyInt;
//				case TypeCode.Int64: return OleDbType.BigInt;
//				case TypeCode.Int32: return OleDbType.Integer;
//				case TypeCode.Int16: return OleDbType.SmallInt;
//				case TypeCode.String: return OleDbType.VarChar;
//				case TypeCode.SByte: return OleDbType.TinyInt;
//				case TypeCode.Boolean: return OleDbType.Boolean;
//				case TypeCode.Decimal: return OleDbType.Numeric;
//				case TypeCode.Double: return OleDbType.Double;
//				case TypeCode.Single: return OleDbType.Single;
//				default: return OleDbType.Variant;
//			}
//		}

		/// <summary>
		/// To be called after ClassID has been set.
		/// </summary>
		public void LoadClass()
		{
			DataRow row = m_EticClassTable.Rows.Find(m_ClassID);
			cboClassType.SelectedIndex = ((short)row["ClassType"] + 2) % 4;
		}
		#region Properties
		public PaDataTable ClassesTable
		{
			set { m_ClassesTable = value; }
		}

		public BinaryFeatureStruct[] BinFeatures
		{
			set { m_BinFeatures = value; }
		}

		public IPAFeatureStruct[] IPAFeatures
		{
			set { m_IPAFeatures = value; }
		}
		#endregion

		#region Event handlers
		private void cboClassType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//	"Set of specified IPA characters" = 0
			//	"Set of specified articulatory features" = 1
			//	"Set of specified consonant binary features" = 2
			//	"Set of specified vowel binary features" = 3
			pnlIPAChars.Visible = (cboClassType.SelectedIndex == 0);
			pnlConBinFeature.Visible = (cboClassType.SelectedIndex == 2);
			grpVowBinFeatures.Visible = (cboClassType.SelectedIndex == 3);
			lstBinFeatures.Visible = (cboClassType.SelectedIndex >= 2);
			llbFeatures.Visible = (cboClassType.SelectedIndex >= 1);
			lstIPAFeat.Visible = (cboClassType.SelectedIndex == 1);

			rbOpAll.Visible = (cboClassType.SelectedIndex > 0);
			rbOpOne.Visible = (cboClassType.SelectedIndex > 0);
			pnlWildcard.Visible = (cboClassType.SelectedIndex == 0);

			if (cboClassType.SelectedIndex == 1)
			{
			}
			else if (cboClassType.SelectedIndex >= 2)
			{
			}
		}

		private void llbIPAFeatures_KeyPress(object sender, KeyPressEventArgs e)
		{
			// if it's backspace, allow its default behaviour
			if (e.KeyChar == '\x8') return;
			e.Handled = true;
			string sChr = PrsIPAChar(e.KeyChar);
			int iCur = llbIPAFeatures.TextBox.SelectionStart;
			if (iCur > 0)
				llbIPAFeatures.Text = llbIPAFeatures.Text.Substring(0, iCur) +
					sChr + llbIPAFeatures.Text.Substring(iCur);
			else
				llbIPAFeatures.Text = sChr + llbIPAFeatures.Text;
			llbIPAFeatures.TextBox.SelectionStart = iCur + sChr.Length;
		}

		private void lbl_MouseDown(object sender, MouseEventArgs e)
		{
			Label lbl = sender as Label;
			lbl.BackColor = SystemColors.Highlight;
			lbl.ForeColor = SystemColors.HighlightText;
		}

		private void lbl_MouseUp(object sender, MouseEventArgs e)
		{
			Label lbl = sender as Label;
			lbl.BackColor = SystemColors.Menu;
			lbl.ForeColor = SystemColors.MenuText;
			if (e.Button != MouseButtons.Left)
				return;
			char ch;
			if (lbl.Text.Length > 1)
				ch = lbl.Text[1];
			else
				ch = lbl.Text[0];
			llbIPAFeatures_KeyPress(llbIPAFeatures, new KeyPressEventArgs(ch));
		}

		private void llbClassName_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (((int)e.KeyChar < 33) || (((int)e.KeyChar >= 97) && ((int)e.KeyChar <= 122)))
				return;
			e.Handled = true;
		}

		private void llbFeatures_KeyPress(object sender, KeyPressEventArgs e)
		{
			// Basically, we don't want to allow anything to be typed into this textbox
			// Only the feature lists will be allowed to change its contents.
			e.Handled = true;
		}
		#endregion
	}

	internal struct FeatureMasks
	{
		public int BinaryMask0, BinaryMask1;
	}
}
