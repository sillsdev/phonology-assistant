using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace SIL.SpeechTools.Database
{
	/// ------------------------------------------------------------------------------------
	/// <summary>
	/// A class encapsulating an adapter as well as the means to update tables in a PA
	/// database.
	/// </summary>
	/// ------------------------------------------------------------------------------------
	public class PaDataTable : DataTable
	{
		private OleDbConnection m_dbConnection;
		private OleDbDataAdapter m_adapter;
		private OleDbCommandBuilder m_cmdBldr;
		private OleDbCommand m_autoIncCmd;
		private int m_newRecAutoIncValue;

		#region Construction and Disposal
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When this constructor is used, the connection is assumed to be
		/// DBUtils.DatabaseConnection.
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// ------------------------------------------------------------------------------------
		public PaDataTable(string sqlCommand) : this(DBUtils.DatabaseConnection, sqlCommand)
		{
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="sqlCommand"></param>
		/// --------------------------------------------------------------------------------
		public PaDataTable(OleDbConnection connection, string sqlCommand) : base()
		{
			Debug.Assert(connection != null);
			Debug.Assert(sqlCommand != null);
			Debug.Assert(sqlCommand != string.Empty);

			// If the sql command contains only one word, then assume it is the name
			// of a table. So return all rows in the table.
			if (sqlCommand.Split(" ".ToCharArray()).Length == 1)
				sqlCommand = "SELECT * FROM " + sqlCommand;

			m_dbConnection = connection;
			m_adapter = new OleDbDataAdapter(sqlCommand, m_dbConnection);
			m_cmdBldr = new OleDbCommandBuilder(m_adapter);
			m_autoIncCmd = new OleDbCommand("SELECT @@IDENTITY", m_dbConnection);
			m_autoIncCmd.Transaction = DBUtils.m_dbTransaction;

			if (m_adapter.UpdateCommand != null)
				m_adapter.UpdateCommand.Transaction = DBUtils.m_dbTransaction;
			if (m_adapter.DeleteCommand != null)
				m_adapter.DeleteCommand.Transaction = DBUtils.m_dbTransaction;
			if (m_adapter.SelectCommand != null)
				m_adapter.SelectCommand.Transaction = DBUtils.m_dbTransaction;
			if (m_adapter.InsertCommand != null)
				m_adapter.InsertCommand.Transaction = DBUtils.m_dbTransaction;

			m_adapter.RowUpdated += new OleDbRowUpdatedEventHandler(HandleRowUpdated);
			m_adapter.Fill(this);
			m_adapter.FillSchema(this, SchemaType.Source);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Creates a data table from a query stored in the database.
		/// </summary>
		/// <param name="queryName"></param>
		/// <param name="param"></param>
		/// --------------------------------------------------------------------------------
		public PaDataTable(string queryName, object param) : this(queryName, new object[] {param})
		{
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Creates a data table from a query stored in the database.
		/// </summary>
		/// <param name="queryName"></param>
		/// <param name="paramList"></param>
		/// --------------------------------------------------------------------------------
		public PaDataTable(string queryName, object[] paramList) : base()
		{
			Debug.Assert(DBUtils.DatabaseConnection != null);
			Debug.Assert(queryName != null);
			Debug.Assert(queryName != string.Empty);

			m_adapter = new OleDbDataAdapter();
			m_dbConnection = DBUtils.DatabaseConnection;
			OleDbCommand cmd = new OleDbCommand(queryName, m_dbConnection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Transaction = DBUtils.m_dbTransaction;

			foreach (object param in paramList)
				cmd.Parameters.Add(new OleDbParameter("?", param));

			m_adapter.SelectCommand = cmd;
			m_adapter.Fill(this);
			m_adapter.FillSchema(this, SchemaType.Source);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		~PaDataTable()
		{
			m_cmdBldr.Dispose();
			m_autoIncCmd.Dispose();
			m_adapter.Dispose();
			Dispose();
		}
		
		#endregion

		#region Event Handlers
		/// --------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// --------------------------------------------------------------------------------
		private void HandleRowUpdated(object sender, OleDbRowUpdatedEventArgs e)
		{
			if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert)
			{
				// Get the new record's auto incremented ID, and, assuming it's
				// the first field in the row, update that field in the row.
				m_newRecAutoIncValue = (int)m_autoIncCmd.ExecuteScalar();
				e.Row[0] = m_newRecAutoIncValue;
				e.Row.AcceptChanges();
			}
			else
				m_newRecAutoIncValue = 0;
		}

		#endregion

		#region Properties
		/// --------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// --------------------------------------------------------------------------------
		public int NewRecordId
		{
			get {return m_newRecAutoIncValue;}
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the command for updating rows in the data table.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public OleDbCommand UpdateCommand
		{
			get {return m_adapter.UpdateCommand;}
			set {m_adapter.UpdateCommand = value;}
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the command for inserting rows into the data table.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public OleDbCommand InsertCommand
		{
			get {return m_adapter.InsertCommand;}
			set {m_adapter.InsertCommand = value;}
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the command for deleting rows from the data table.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public OleDbCommand DeleteCommand
		{
			get {return m_adapter.DeleteCommand;}
			set {m_adapter.DeleteCommand = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets field (or column) name of the DB table's primary key. The PaDataTable may
		/// have more than one primary key. I suspect that when the query used to create the
		/// PaDataTable contains joins involving two or more tables, then there will be more
		/// than one primary key. If that's the case, then this property will return null.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string PrimaryKeyField
		{
			get
			{
				return (PrimaryKey != null && PrimaryKey.Length == 1 ?
					PrimaryKey[0].ColumnName : null);
			}
		}

		#endregion

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Commits any changes made to the data table to the DB.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public void Commit()
		{
			m_adapter.Update(this);
		}
	}
}
