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
// File: ErrorLogWnd.cs
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

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Displays the contents of the error log database.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ErrorLogWnd : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCompact;
		private System.Windows.Forms.DataGrid dbGrid;
		private SIL.Pa.Controls.PaPanel paPanel1;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorLogWnd"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ErrorLogWnd()
		{
			InitializeComponent();

			//dbConnection = new OleDbConnection(
			//    @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + PaApp.AppPath + "errorlog.mdb");

			//dbConnection.Open();
			//string sql = "SELECT Format([ErrTime], 'dd-mmm-yyyy') + ',  ' + " +
			//    "Format([ErrTime], 'Long Time') As [Date & Time], ErrNum As [Number], " +
			//    "ErrMsg As [Message], ErrModule As [Module], ErrProc As [Procedure] " +
			//    "FROM ErrorLog ORDER BY ErrTime";
			
			//OleDbDataAdapter adapter = new OleDbDataAdapter(sql, dbConnection);
			//DataTable table = new DataTable();
			//adapter.Fill(table);
			//dbGrid.DataSource = table;
			//dbGrid.Font = SystemInformation.MenuFont;
			//dbGrid.HeaderFont = SystemInformation.MenuFont;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged 
		/// resources; <c>false</c> to release only unmanaged resources. 
		/// </param>
		/// ------------------------------------------------------------------------------------
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
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeComponent()
		{
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCompact = new System.Windows.Forms.Button();
			this.dbGrid = new System.Windows.Forms.DataGrid();
			this.paPanel1 = new SIL.Pa.Controls.PaPanel();
			((System.ComponentModel.ISupportInitialize)(this.dbGrid)).BeginInit();
			this.paPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(231, 392);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			// 
			// btnCompact
			// 
			this.btnCompact.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCompact.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCompact.Location = new System.Drawing.Point(319, 392);
			this.btnCompact.Name = "btnCompact";
			this.btnCompact.Size = new System.Drawing.Size(75, 23);
			this.btnCompact.TabIndex = 2;
			this.btnCompact.Text = "&Compact";
			// 
			// dbGrid
			// 
			this.dbGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.dbGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dbGrid.CaptionVisible = false;
			this.dbGrid.DataMember = "";
			this.dbGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dbGrid.HeaderBackColor = System.Drawing.SystemColors.ControlLightLight;
			this.dbGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dbGrid.Location = new System.Drawing.Point(0, 0);
			this.dbGrid.Name = "dbGrid";
			this.dbGrid.ReadOnly = true;
			this.dbGrid.Size = new System.Drawing.Size(602, 370);
			this.dbGrid.TabIndex = 4;
			// 
			// paPanel1
			// 
			this.paPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.paPanel1.Controls.Add(this.dbGrid);
			this.paPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.paPanel1.DoubleBuffered = false;
			this.paPanel1.Location = new System.Drawing.Point(10, 10);
			this.paPanel1.Name = "paPanel1";
			this.paPanel1.Size = new System.Drawing.Size(604, 372);
			this.paPanel1.TabIndex = 5;
			// 
			// ErrorLogWnd
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(624, 422);
			this.Controls.Add(this.paPanel1);
			this.Controls.Add(this.btnCompact);
			this.Controls.Add(this.btnOK);
			this.Name = "ErrorLogWnd";
			this.Padding = new System.Windows.Forms.Padding(10, 10, 10, 40);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Error Log";
			((System.ComponentModel.ISupportInitialize)(this.dbGrid)).EndInit();
			this.paPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
