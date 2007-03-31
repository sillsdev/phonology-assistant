using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using SIL.Pa.Database;
using SIL.FieldWorks.Common.Controls;

namespace SIL.Pa
{
	/// <summary>
	/// Due to time constraints which prevent us from attempting to modify the code
	/// of FwGrid etc. to allow use of multiple fonts like in SSDBGrid (styles), we
	/// shall simply display everything using System.MenuFont. this should not be
	/// a problem when the database is converted to unicode as we would merely need
	/// to change the font to one containing glyphs for the necessary codepoints.
	/// currently haven't figured ou thow to update the grid to reflect the current
	/// condition of the list of classes, but have succeeded in adding and deleting
	/// classes.
	/// Grid-related bug: Bottom row hidden by horizontal scrollbar.
	/// The bottom row is not visible even when you scroll to the absolute bottom if
	/// the horizontal scrollbar is visible. May also want to check if anything gets
	/// hidden by the vertical scrollbar in a similar manner.
	/// </summary>
	public class ClassDlg : System.Windows.Forms.Form
	{
		private PaDataTable m_table;
		private FwGrid m_grid;
		private BinaryFeatureStruct[] m_BinFeatures;
		private IPAFeatureStruct[] m_IPAFeatures;
		// private PaDataTable m_EticClassTable;
		// private OleDbDataAdapter m_adapter;
		private PaDataTable m_EticClassTable;
		#region Variables added by Designer
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnCopy;
		private System.Windows.Forms.Button btnModify;
		private System.Windows.Forms.Button btnDelete;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		public ClassDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			m_table = new PaDataTable("SELECT DISTINCTROW * FROM PhoneticClass " +
				"WHERE ShowInDefClassList=True " +
				"ORDER BY DisplayType, SortOrder, ClassName;");
			SetupGrid();
			LoadFeatureInfo();
			LoadGrid(0);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnClose = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnCopy = new System.Windows.Forms.Button();
			this.btnModify = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnClose.Location = new System.Drawing.Point(314, 16);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnAdd.Location = new System.Drawing.Point(314, 76);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "&Add...";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnCopy
			// 
			this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCopy.Location = new System.Drawing.Point(314, 104);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.TabIndex = 3;
			this.btnCopy.Text = "&Copy";
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnModify
			// 
			this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnModify.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnModify.Location = new System.Drawing.Point(314, 132);
			this.btnModify.Name = "btnModify";
			this.btnModify.TabIndex = 4;
			this.btnModify.Text = "&Modify...";
			this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnDelete.Location = new System.Drawing.Point(314, 160);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 5;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// ClassDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(404, 224);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnModify);
			this.Controls.Add(this.btnCopy);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.btnClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MinimumSize = new System.Drawing.Size(412, 250);
			this.Name = "ClassDlg";
			this.ShowInTaskbar = false;
			this.Text = "Define Classes";
			this.ResumeLayout(false);

		}
		#endregion

		private void SetupGrid()
		{
			m_grid = new FwGrid();
			m_grid.Location = new Point(0,0);
			m_grid.ShowColumnHeadings = true;
			m_grid.GridLineColor = m_grid.BackColor;
			m_grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			m_grid.Size = new Size(310, this.ClientRectangle.Height);
			m_grid.RowHeight = SystemInformation.MenuFont.Height + 3;

			int i = m_grid.Columns.Add(new FwGridColumn("",0));
			m_grid.Columns[i].Visible = false;
			m_grid.Columns[i].Name = "type";

			i = m_grid.Columns.Add(new FwGridColumn("Class Name", 140));
			m_grid.Columns[i].Name = "clsname";
			m_grid.Columns[i].HeaderIsClickable = false;
			m_grid.Columns[i].Font = SystemInformation.MenuFont;
			m_grid.Columns[i].AllowEdit = false;

			i = m_grid.Columns.Add(new FwGridColumn("",0));
			m_grid.Columns[i].Visible = false;
			m_grid.Columns[i].Name = "clsid";

			i = m_grid.Columns.Add(new FwGridColumn("Members", 150));
			m_grid.Columns[i].Name = "members";
			m_grid.Columns[i].HeaderIsClickable = false;
			m_grid.Columns[i].Font = SystemInformation.MenuFont;
			m_grid.Columns[i].AllowEdit = false;

			i = m_grid.Columns.Add(new FwGridColumn("",0));
			m_grid.Columns[i].Visible = false;
			m_grid.Columns[i].Name = "clstype";

			i = m_grid.Columns.Add(new FwGridColumn("",0));
			m_grid.Columns[i].Visible = false;
			m_grid.Columns[i].Name = "allowmod";

			m_grid.BeforeRowChange += new SIL.FieldWorks.Common.Controls.FwGrid.RowColChangeHandler(m_grid_BeforeRowChange);

			this.Controls.Add(m_grid);
		}

		private void LoadFeatureInfo()
		{
			int i;

			PaDataTable table = new PaDataTable("SELECT * FROM BitMasks " +
				"WHERE (FeatureSet = 1) AND (Features <> '(Add Custom Feature Here)') " +
				"ORDER BY TypeSubOrder;");
			i = 0;
			m_BinFeatures = new BinaryFeatureStruct [table.Rows.Count];

			foreach (DataRow row in table.Rows)
			{
				m_BinFeatures[i].FeatureName = (string) row["Features"];
				m_BinFeatures[i].PlusMask0 = ((((short)row["MaskNum"]) == 0) ? ((int)row["Mask"]) : 0);
				m_BinFeatures[i].MinusMask0 = ((((short)row["MaskNum"]) == 0) ? ((int)row["MinusMask"]) : 0);
				m_BinFeatures[i].PlusMask1 = ((((short)row["MaskNum"]) == 1) ? ((int)row["Mask"]) : 0);
				m_BinFeatures[i].MinusMask1 = ((((short)row["MaskNum"]) == 1) ? ((int)row["MinusMask"]) : 0);
				i++;
			}

			table = new PaDataTable("SELECT * FROM BitMasks WHERE (FeatureType > -1) " +
				"AND (FeatureType <= 5) AND (Features <> '(Add Custom Feature Here)') " +
				"ORDER BY FeatureType, TypeSubOrder;");

			i = 0;
			m_IPAFeatures = new IPAFeatureStruct[table.Rows.Count];

			foreach (DataRow row in table.Rows)
			{
				m_IPAFeatures[i].FeatureName = (string) row["Features"];
				m_IPAFeatures[i].Mask = (int) row["Mask"];
				m_IPAFeatures[i].MaskNum = (short) row["MaskNum"];
				i++;
			}
		}

		private void LoadGrid(int ID)
		{
			short Type = -1;
			int iRow = 0, iSavRow = 0;
			int [] Masks = new int[4];
			string sMembers;
			m_grid.Rows.Clear();
			foreach (DataRow row in m_table.Rows)
			{
				if (Type != (short) row["DisplayType"])
				{
					Type = (short) row["DisplayType"];
					switch (Type)
					{
						case 0: // General
							m_grid.Rows.Add(new FwGridRow(new object[] {Type, "General", 0, "", 0, 0}));
							break;
						case 1: // User Def
							m_grid.Rows.Add(new FwGridRow(new object[] {Type, "User Defined Classes", 0, "", 0, 0}));
							break;
						case 2: // Voicing
							m_grid.Rows.Add(new FwGridRow(new object[] {Type, "Voicing", 0, "", 0, 0}));
							break;
						case 3: // Con Place
							m_grid.Rows.Add(new FwGridRow(new object[] {Type, "Consonant Place of Articulation", 0, "", 0, 0}));
							break;
						case 4: // Con Manner
							m_grid.Rows.Add(new FwGridRow(new object[] {Type, "Consonant Manner of Articulation", 0, "", 0, 0}));
							break;
						case 5: // Vowel
							m_grid.Rows.Add(new FwGridRow(new object[] {Type, "Vowels", 0, "", 0, 0}));
							break;
					}
					iRow++;
				}
				// get the text that goes in the members column.
				if ((short)row["ClassType"] == 2) // IPA Phone Class
					sMembers = (string) row["IPAChars"];
				else if ((short)row["ClassType"] == 3) // IPA Feature Class
				{
					Masks[0] = (int) row["Mask0"];
					Masks[1] = (int) row["Mask1"];
					Masks[2] = (int) row["Mask2"];
					Masks[3] = (int) row["Mask3"];
					sMembers = GetIPAFeaturesMembersText(Masks, (bool) row["ANDFeatures"]);
				}
				else if ((short)row["ClassType"] == -1) // Special Class
					sMembers = "Special Class";
				else
					sMembers = GetBinFeaturesMembersText((int) row["BinaryMask0"],
						(int) row["BinaryMask0"], (bool) row["ANDFeatures"]);

				m_grid.Rows.Add(new FwGridRow(
					new object[] {-1, "      " + row["ClassName"], row["PhoneticClassID"],
									 sMembers, row["ClassType"], (((short)row["DisplayType"] == 1))}));
				if ((ID > 0) && ((int)row["PhoneticClassID"] == ID))
					iSavRow = iRow;
				iRow++;
			}
			m_grid.Refresh();
		}

		/// <summary>
		/// Retrieves the feature members specified by the bit masks.
		/// </summary>
		/// <param name="Mask0">The Mask0 field.</param>
		/// <param name="Mask1">The Mask1 field.</param>
		/// <param name="ANDFeatures">Whether to use "with" or commas.</param>
		/// <returns>The human-readable string form of the feature members.</returns>
		private string GetBinFeaturesMembersText(int Mask0, int Mask1, bool ANDFeatures)
		{
			string sFeat = "";

			for (int i = 0; i < m_BinFeatures.Length; i++)
			{
				if (((Mask0 & m_BinFeatures[i].PlusMask0) != 0) ||
					((Mask1 & m_BinFeatures[i].PlusMask1) != 0))
					sFeat += "+";
				else if (((Mask0 & m_BinFeatures[i].MinusMask0) != 0) ||
					((Mask1 & m_BinFeatures[i].MinusMask1) != 0))
					sFeat += "-";

				if (sFeat.EndsWith("+") || sFeat.EndsWith("-"))
					sFeat += (m_BinFeatures[i].FeatureName + (ANDFeatures ? " {with} " : ", "));
			}

			sFeat = sFeat.Trim();

			if (sFeat.EndsWith("{with}"))
				sFeat = sFeat.Substring(0, sFeat.Length - 7);
			else if (sFeat.EndsWith(","))
				sFeat = sFeat.Substring(0, sFeat.Length - 1);

			if (sFeat.Length == 0)
				return "";
			else
				return "<" + sFeat + ">";
		}

		/// <summary>
		/// Retrieves the feature members specified by the bit masks.
		/// </summary>
		/// <param name="Masks">The array of IPA (or articulatory)  feature masks.</param>
		/// <param name="ANDFeatures">Whether to use "with" or commas.</param>
		/// <returns>The human-readable string form of the feature members.</returns>
		private string GetIPAFeaturesMembersText(int [] Masks, bool ANDFeatures)
		{
			string sFeat = "";

			for (int i = 0; i < m_IPAFeatures.Length; i++)
			{
				if ((Masks[m_IPAFeatures[i].MaskNum] & m_IPAFeatures[i].Mask) != 0)
					sFeat += (m_IPAFeatures[i].FeatureName + (ANDFeatures ? " {with} " : ", "));
			}

			sFeat = sFeat.Trim();
			if (sFeat.EndsWith("{with}"))
				sFeat = sFeat.Substring(0, sFeat.Length - 7);
			else if (sFeat.EndsWith(","))
				sFeat = sFeat.Substring(0, sFeat.Length - 1);

			if (sFeat.Length == 0)
				return "";
			else
				return "<" + sFeat + ">";
		}

		public void SetupDatabaseUpdate()
		{
			m_EticClassTable = new PaDataTable("SELECT * FROM PhoneticClass");
			m_EticClassTable.PrimaryKey = new DataColumn[] {m_EticClassTable.Columns["PhoneticClassID"]};
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

		#region Event handlers
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			Classes cls = new Classes();
			cls.ClassesTable = m_table;
			cls.BinFeatures = m_BinFeatures;
			cls.IPAFeatures = m_IPAFeatures;
			cls.ShowDialog();
		}

		private void btnCopy_Click(object sender, System.EventArgs e)
		{
			///For now display messagebox to say not done yet
			MessageBox.Show("Not Completed.","Phonology Assistant",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btnModify_Click(object sender, System.EventArgs e)
		{
			///For now display messagebox to say not done yet
			MessageBox.Show("Not Completed.","Phonology Assistant",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (MessageBox.Show(this, "Are you sure you want to delete class: '" +
				m_grid.CurrentRow.Cells[m_grid.Columns.IndexOf(m_grid.Columns["clsname"])].Text.Trim() +
				"'?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return;
			m_EticClassTable.Rows.Find(int.Parse(
				m_grid.CurrentRow.Cells[m_grid.Columns.IndexOf(m_grid.Columns["clsid"])].Text)).Delete();
			m_EticClassTable.Commit();
		}

		private void m_grid_BeforeRowChange(object sender, FwGridRowColChangeArgs args)
		{
			if ((args.NewRow.Cells[m_grid.Columns["type"].Index].Text) == "-1")
			{
				btnModify.Enabled = (bool)args.NewRow.Cells[m_grid.Columns["allowmod"].Index].Value;
				btnDelete.Enabled = (bool)args.NewRow.Cells[m_grid.Columns["allowmod"].Index].Value;
			}
			else
			{
				if ((m_grid.Rows.IndexOf(args.NewRow) < m_grid.Rows.IndexOf(args.PreviousRow)) &&
					(m_grid.Rows.IndexOf(args.NewRow) > 0))
					SendKeys.Send("{UP}");
				else
					SendKeys.Send("{DOWN}");
			}
		}
		#endregion
	}

	// the structs corresponding to those defined in GVARS.BAS shall be defined
	// here since they have no use outside of Classes and ClassDlg.
	public struct BinaryFeatureStruct
	{
		public string FeatureName;
		public int PlusMask0, MinusMask0, PlusMask1, MinusMask1;
	}

	public struct IPAFeatureStruct
	{
		public string FeatureName;
		public short MaskNum;
		public int Mask;
	}
}
