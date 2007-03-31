using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using SIL.SpeechTools.Database;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for SortOrders.
	/// </summary>
	public class SortOrders : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnuDefaults;
		private System.Windows.Forms.MenuItem mnuRestoreMoA;
		private System.Windows.Forms.MenuItem mnuRestorePoA;
		private System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private int m_sorttype;
		private System.Windows.Forms.Label lblEditAllLight;
		private int m_sortfield;
		// carried over from vb code. to be removed?
		private const char smcEmSpace = '!';
		private string [] SortFields;
		private const int MaxSortVal = 0xFFE;
		// remember to use table.AcceptChanges to commit changes made --JonL
		private static DataSet m_dataset = new DataSet();
		private PaDataTable m_table;
		private DataView dv;

		public SortOrders()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			SortFields = new string[] {"ANSINum", "Type", "IsBase",
										  "MOArticulation", "POArticulation",
										  "UDSortOrder", "TotalCount"};
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
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.mnuDefaults = new System.Windows.Forms.MenuItem();
			this.mnuRestoreMoA = new System.Windows.Forms.MenuItem();
			this.mnuRestorePoA = new System.Windows.Forms.MenuItem();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblEditAllLight = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuDefaults});
			// 
			// mnuDefaults
			// 
			this.mnuDefaults.Index = 0;
			this.mnuDefaults.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mnuRestoreMoA,
																						this.mnuRestorePoA});
			this.mnuDefaults.Text = "&Defaults";
			// 
			// mnuRestoreMoA
			// 
			this.mnuRestoreMoA.Index = 0;
			this.mnuRestoreMoA.Text = "Restore Manner of Articulation Defaults";
			this.mnuRestoreMoA.Click += new System.EventHandler(this.mnuRestoreMoA_Click);
			// 
			// mnuRestorePoA
			// 
			this.mnuRestorePoA.Index = 1;
			this.mnuRestorePoA.Text = "Restore Place of Articulation Defaults";
			this.mnuRestorePoA.Click += new System.EventHandler(this.mnuRestorePoA_Click);
			// 
			// dataGrid1
			// 
			this.dataGrid1.BackgroundColor = System.Drawing.SystemColors.Window;
			this.dataGrid1.DataMember = "";
			this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Top;
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(0, 0);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.Size = new System.Drawing.Size(280, 208);
			this.dataGrid1.TabIndex = 0;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(56, 216);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new EventHandler(btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(144, 216);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			// 
			// lblEditAllLight
			// 
			this.lblEditAllLight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblEditAllLight.Location = new System.Drawing.Point(8, 4);
			this.lblEditAllLight.Name = "lblEditAllLight";
			this.lblEditAllLight.Size = new System.Drawing.Size(10, 14);
			this.lblEditAllLight.TabIndex = 2;
			// 
			// SortOrders
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(280, 257);
			this.Controls.Add(this.lblEditAllLight);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.dataGrid1);
			this.Controls.Add(this.btnCancel);
			this.Menu = this.mainMenu1;
			this.Name = "SortOrders";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "IPA Character Sort Orders";
			this.Resize += new System.EventHandler(this.SortOrders_Resize);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void SortOrders_Resize(object sender, EventArgs e)
		{
			int offset = (this.Width - (btnCancel.Right - btnOK.Left)) / 2;

			btnOK.Left = offset;
			btnCancel.Left = btnOK.Right + 8;
			dataGrid1.Height = this.Height - 96;
		}

		private void mnuRestoreMoA_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Are you sure you want to restore " +
				"the default Manner of Articulation sort order?", "Query", MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);
			if (result == DialogResult.Yes)
			{
				new PaDataTable("UPDATE DISTINCTROW TempChar SET " +
					"MOArticulation = IIF([DefaultMOA] = 4095, 0, [DefaultMOA])" + ";");
				UpdateView();
			}
		}

		private void mnuRestorePoA_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Are you sure you want to restore " +
				"the default Place of Articulation sort order?", "Query", MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);
			if (result == DialogResult.Yes)
			{
				UpdateView();
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			mnuDefaults.Visible = (m_sorttype == 0);
			this.Text = ((m_sorttype==0) ? "Manner & Place of Articulation Sort Orders":
				"User-Defined Sort Order");
			UpdateView();
		}

		#region Properties
		/// <summary>
		/// Sets the sort type for the SortOrders window.
		/// </summary>
		[Browsable(false)]
		public int SortType
		{
			set { m_sorttype = value; }
		}
		#endregion

		#region Miscellaneous
		private string GetSQL(int SortIndex, bool LimitToNonFFFVals)
		{
			string CharTypeTxtFld, CharFld, sql;

			CharTypeTxtFld = "IIf([Type]=0,'Undefined', " +
				"IIf([Type]=1 Or [Type]=3,'Consonant', " +
				"IIf([Type]=2,'Vowel', " +
				"IIf([Type]=4,'Other Symbol', " +
				"IIf([Type]=5,'Suprasegmental', " +
				"IIf([Type]=6,'Tone or Accent', " +
				"IIf([Type]=7,'Diacritic', " +
				"'Misc.'))))))) AS TypeText ";

			CharFld = "IIF([DsplyWEmSpace] OR [ANSINum]=213 OR " +
				"[ANSINum]=233 OR [ANSINum]=242 OR " +
				"[ANSINum]=161, '" + smcEmSpace + "' & [Char], [Char]) As DispChar";

			sql = "SELECT *, " + CharTypeTxtFld + ", " + CharFld +
				" FROM TempChar WHERE (ANSINum>0) " +
				(!(lblEditAllLight.Visible) ? "AND (Type>0) " : " ") +
				(LimitToNonFFFVals ? "AND (" + SortFields[SortIndex] + "<=" + MaxSortVal + ") " : " ") +
				"ORDER BY " + ((SortIndex == 1) ? "IIf([Type]=3,1,[Type]), " : "") +
				SortFields[SortIndex];
			return sql;
		}

		private void UpdateView()
		{
			SetMyState();
			m_table = new PaDataTable(GetSQL(m_sortfield, false));
			m_table.Columns["TypeText"].ColumnMapping = MappingType.Hidden;
			m_table.Columns["CharID"].ColumnMapping = MappingType.Hidden;
			m_table.Columns["DefaultMOA"].ColumnMapping = MappingType.Hidden;
			m_table.Columns["DefaultPOA"].ColumnMapping = MappingType.Hidden;
			m_table.Columns["UDSortOrder"].ColumnMapping = MappingType.Hidden;
			m_table.Columns["TotalCount"].ColumnMapping = MappingType.Hidden;
			m_table.Columns["DsplyOrder"].ColumnMapping = MappingType.Hidden;
			m_table.Columns["DsplyWEmSpace"].ColumnMapping = MappingType.Hidden;
			m_table.Columns["Dups1"].ColumnMapping = MappingType.Hidden;
			m_table.Columns["Dups2"].ColumnMapping = MappingType.Hidden;
			m_table.Columns["DispChar"].ColumnMapping = MappingType.Hidden;
//CharID
//Char*
//ANSINum*
//Name*
//Desc*
//Type*
//IsBase*
//MOArticulation*
//POArticulation*
//DefaultMOA
//DefaultPOA
//UDSortOrder
//TotalCount
//DsplyOrder
//DsplyWEmSpace
//Dups1
//Dups2
			m_dataset.Tables.Add(m_table);
			dv = new DataView(m_table,"Type <> 0","",DataViewRowState.CurrentRows);
			dataGrid1.DataSource = dv;
		}

		private void SetMyState()
		{
			return;
		}

		private bool TableExchange(int direction)
		{
			if (direction == 0)
				DeleteTempTableContents();
			PaDataTable keep = new PaDataTable("SELECT * FROM " + ((m_sorttype == 0)? "IPACharSet" : "PhoneticList"));
			PaDataTable temp;
			if (direction == 0)
			{
				temp = new PaDataTable("TempChar");
				foreach (DataColumn col in keep.Columns)
				{
					string tmpname = GetTmpName(col.ColumnName);
					temp.Columns.Add(new DataColumn(tmpname, col.DataType, col.Expression, col.ColumnMapping));
				}
				temp.Columns.Add("Dups1");
				temp.Columns.Add("Dups2");
				int i = 0;
				foreach (DataRow row in keep.Rows)
				{
					if (m_sorttype == 0)
					{
						temp.NewRow();
						temp.Rows[i]["CharID"] = row["IPACharSetID"];
						temp.Rows[i]["Char"] = row["IPAChar"];
						temp.Rows[i]["ANSINum"] = row["SILIPACharNum"];
						temp.Rows[i]["Name"] = row["IPAName"];
						temp.Rows[i]["Desc"] = row["IPADesc"];
						temp.Rows[i]["Type"] = row["CharType"];
						temp.Rows[i]["IsBase"] = row["IsBaseChar"];
						temp.Rows[i]["MOArticulation"] = (((((long)row["MOArticulation"]) == 0) ||
							(((long)row["MOArticulation"]) == 0xFFF))? 0 : row["MOArticulation"]);
						temp.Rows[i]["POArticulation"] = (((((long)row["POArticulation"]) == 0) ||
							(((long)row["POArticulation"]) == 0xFFF)) ? 0 : row["POArticulation"]);
						temp.Rows[i]["DefaultMOA"] = row["DefaultMOA"];
						temp.Rows[i]["DefaultPOA"] = row["DefaultPOA"];
						temp.Rows[i]["DsplyOrder"] = row["DsplyOrder"];
						temp.Rows[i]["DsplyWEmSpace"] = row["DsplyWEmSpace"];
					}
					else
					{
						temp.Rows[i]["CharID"] = row["CharListID"];
						temp.Rows[i]["Char"] = row["ANSICharStr"];
						temp.Rows[i]["UDSortOrder"] = (((((long)row["UDSortOrder"]) == 0) ||
							(((long)row["UDSortOrder"]) == 0xFFF)) ? 0 : row["UDSortOrder"]);
						temp.Rows[i]["TotalCount"] = row["TotalCount"];
						temp.Rows[i]["ANSINum"] = 999;
						temp.Rows[i]["Type"] = 999;
					}
					temp.Rows[i]["Dups1"] = "";
					temp.Rows[i]["Dups2"] = "";
					i++;
				}
				// we're supposed to put the tempchar table into the database, right?
				// well, how do we do that, eh?
			}
			else
			{
				temp = new PaDataTable("SELECT * FROM TempChar");
				foreach (DataRow row in keep.Rows)
				{
					DataRow tmprow = temp.Rows.Find(row["CharID"]);
					if (tmprow != null)
					{
						if (m_sorttype == 0)
						{
							row["MOArticulation"] = tmprow["MOArticulation"];
							row["POArticulation"] = tmprow["POArticulation"];
							if (lblEditAllLight.Visible)
							{
								row["CharType"] = tmprow["Type"];
								row["IsBaseChar"] = tmprow["BaseChar"];
								row["DefaultMOA"] = tmprow["DefaultMOA"];
								row["DefaultPOA"] = tmprow["DefaultPOA"];
								row["DsplyOrder"] = tmprow["DsplyOrder"];
								row["DsplyWEmSpace"] = tmprow["DsplyWEmSpace"];
							}
						}
						else
							row["UDSortOrder"] = tmprow["UDSortOrder"];
					}
				}
				// we'll need to put the updated table back into the database. how?
			}
			return true;
		}

		/// <summary>
		/// Deletes the existing TempChar table from the database.
		/// </summary>
		private void DeleteTempTableContents()
		{
			// this function is pretty straight-forward. is there a better way?
			OleDbCommand cmd = DBUtils.DatabaseConnection.CreateCommand();
			OleDbTransaction trans = DBUtils.DatabaseConnection.BeginTransaction();
			cmd.Connection = DBUtils.DatabaseConnection;
			cmd.Transaction = trans;
			try
			{
				cmd.CommandText = "DELETE * FROM TempChar";
				trans.Commit();
			}
			catch { }
		}

		private string GetTmpName(string colname)
		{
			if (m_sorttype == 0)
			{
				if (colname.CompareTo("IPACharSetID") == 0)
					return "CharID";
				else if (colname.CompareTo("IPAChar") == 0)
					return "Char";
				else if (colname.CompareTo("SILIPACharNum") == 0)
					return "ANSINum";
				else if (colname.CompareTo("IPAName") == 0)
					return "Name";
				else if (colname.CompareTo("IPADesc") == 0)
					return "Desc";
				else if (colname.CompareTo("CharType") == 0)
					return "Type";
				else if (colname.CompareTo("IsBaseChar") == 0)
					return "IsBase";
				else return colname;
			}
			else
			{
				if (colname.CompareTo("CharListID") == 0)
					return "CharID";
				else if (colname.CompareTo("ANSICharStr") == 0)
					return "Char";
				else return colname;
			}
		}
		#endregion

		private void btnOK_Click(object sender, EventArgs e)
		{
			// Is there a problem with the Update command generated?
			// This program crashes if one has changes to save.
			// therefore, we cannot do this directly. what do we do?
			// m_table.Commit();
			TableExchange(1);
		}
	}
}
