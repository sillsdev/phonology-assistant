// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Data;

namespace SIL.Pa.AddOn
{
	public partial class QueryResultOutputDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public QueryResultOutputDlg()
		{
			InitializeComponent();
			tabCtrl.Font = SystemInformation.MenuFont;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public QueryResultOutputDlg(FwDataSourceInfo dataSourceInfo) : this()
		{
			Text = string.Format("Database: {0} on {1}",
				dataSourceInfo.DBName, dataSourceInfo.MachineName);

			tpgData.Text = string.Format(tpgData.Text, dataSourceInfo.DBName);

			FwDataReader reader = new FwDataReader(dataSourceInfo);
			reader.GetData(HandleReadingFwData);

			GetSQL(dataSourceInfo);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetSQL(FwDataSourceInfo dataSourceInfo)
		{
			if (dataSourceInfo.WritingSystemInfo == null || dataSourceInfo.WritingSystemInfo.Count == 0)
				return;

			string sql =
				(dataSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm ?
					dataSourceInfo.Queries.LexemeFormSQL : dataSourceInfo.Queries.PronunciationFieldSQL);

			foreach (FwDataSourceWsInfo dswsi in dataSourceInfo.WritingSystemInfo)
			{
				string replace = string.Format("${0}Ws$", dswsi.FieldName);
				sql = sql.Replace(replace, dswsi.Ws.ToString());
			}

			sql = sql.Replace("    ", "   ");
			sql = sql.Replace("   ", Environment.NewLine + "  ");
			txtSQL.Text = sql;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleReadingFwData(SqlDataReader reader)
		{
			if (reader == null || reader.IsClosed)
				return;

			// First, get a list of the fields returned from
			// the query and create columns for each one.
			for (int i = 0; i < reader.FieldCount; i++)
			{
				string field = reader.GetName(i);
				m_grid.Columns.Add(field, field);
			}

			int row = 0;

			while (reader.Read())
			{
				m_grid.Rows.Add();
				
				// Read the data for all columns. If there are columns the record
				// or word entries don't recognize, they'll just be ignored.
				for (int i = 0; i < reader.FieldCount; i++)
				{
					if (!(reader[i] is DBNull))
						m_grid.Rows[row].Cells[i].Value = reader[i].ToString();
				}

				row++;
			}
		}
	}
}