using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Data;
using SilUtils;
using XCore;

namespace SIL.Pa.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Dialog for defining ambiguous sequences.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class AmbiguousSequencesDlg : OKCancelDlgBase, IxCoreColleague
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AmbiguousSequencesDlg()
		{
			InitializeComponent();
			PaApp.SettingsHandler.LoadFormProperties(this);

			if (!PaintingHelper.CanPaintVisualStyle())
				pnlGrid.BorderStyle = BorderStyle.Fixed3D;

			BuildGrid();
			LoadGrid();
			
			m_grid.Font = FontHelper.UIFont;
			m_grid.Columns["seq"].DefaultCellStyle.Font = FontHelper.PhoneticFont;
			m_grid.Columns["seq"].CellTemplate.Style.Font = FontHelper.PhoneticFont;
			m_grid.Columns["base"].DefaultCellStyle.Font = FontHelper.PhoneticFont;
			m_grid.Columns["base"].CellTemplate.Style.Font = FontHelper.PhoneticFont;
			m_grid.Columns["cvpattern"].DefaultCellStyle.Font = FontHelper.PhoneticFont;
			m_grid.Columns["cvpattern"].CellTemplate.Style.Font = FontHelper.PhoneticFont;

			foreach (DataGridViewRow row in m_grid.Rows)
			{
				row.Cells["seq"].Style.Font = FontHelper.PhoneticFont;
				row.Cells["base"].Style.Font = FontHelper.PhoneticFont;
				row.Cells["cvpattern"].Style.Font = FontHelper.PhoneticFont;
			}

			AdjustGridRows();
			PaApp.AddMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			PaApp.RemoveMediatorColleague(this);
			base.OnFormClosed(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildGrid()
		{
			m_grid.Name = Name + "AmbigGrid";
			m_grid.AutoGenerateColumns = false;
			m_grid.AllowUserToAddRows = true;
			m_grid.AllowUserToDeleteRows = true;
			m_grid.AllowUserToOrderColumns = false;
			m_grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
			m_grid.Font = FontHelper.UIFont;

			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("seq");
			col.Width = 90;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			col.HeaderText = Properties.Resources.kstidAmbiguousSeqHdg;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateCheckBoxColumn("convert");
			col.Width = 75;
			col.HeaderText = Properties.Resources.kstidAmbiguousConvertHdg;
			col.CellTemplate.ValueType = typeof(bool);
			m_grid.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("base");
			col.Width = 75;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			col.HeaderText = Properties.Resources.kstidAmbiguousBaseCharHdg;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("cvpattern");
			col.ReadOnly = true;
			col.Width = 70;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			col.HeaderText = Properties.Resources.kstidAmbiguousCVPatternHdg;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateCheckBoxColumn("default");
			col.Visible = false;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateCheckBoxColumn("autodefault");
			col.Visible = false;
			m_grid.Columns.Add(col);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the grid with the ambiguous items information from the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadGrid()
		{
			// Uncomment if we ever go back to having a default set of ambiguous sequences.
			//bool showDefaults = chkShowDefaults.Checked ||
			//    PaApp.SettingsHandler.GetBoolSettingsValue(Name, "showdefault", true);

			bool showDefaults = true;
			int prevRow = m_grid.CurrentCellAddress.Y;

			m_grid.Rows.Clear();
			AmbiguousSequences ambigSeqList = DataUtils.IPACharCache.AmbiguousSequences;

			if (ambigSeqList == null || ambigSeqList.Count == 0)
			{
				m_grid.IsDirty = false;
				return;
			}

			bool hasDefaultSequences = false;
			m_grid.Rows.Add(ambigSeqList.Count);

			for (int i = 0; i < ambigSeqList.Count; i++)
			{
				m_grid["seq", i].Value = ambigSeqList[i].Unit;
				m_grid["convert", i].Value = ambigSeqList[i].Convert;
				m_grid["base", i].Value = ambigSeqList[i].BaseChar;
				m_grid["default", i].Value = ambigSeqList[i].IsDefault;
				m_grid["autodefault", i].Value = ambigSeqList[i].IsAutoGeneratedDefault;

				if (!string.IsNullOrEmpty(ambigSeqList[i].BaseChar))
				{
					m_grid["cvpattern", i].Value =
						PaApp.PhoneCache.GetCVPattern(ambigSeqList[i].BaseChar);
				}

				if (ambigSeqList[i].IsDefault || ambigSeqList[i].IsAutoGeneratedDefault)
				{
					m_grid.Rows[i].Cells[0].ReadOnly = true;
					hasDefaultSequences = true;
					if (!showDefaults)
						m_grid.Rows[i].Visible = false;
				}
			}

			// Select a row if there isn't one currently selected.
			if (m_grid.CurrentCellAddress.Y < 0 && m_grid.RowCountLessNewRow > 0 &&
				m_grid.Rows.GetRowCount(DataGridViewElementStates.Visible) > 0)
			{
				// Check if the previous row is a valid row.
				if (prevRow < 0 || prevRow >= m_grid.RowCountLessNewRow ||
					!m_grid.Rows[prevRow].Visible)
				{
					prevRow = m_grid.FirstDisplayedScrollingRowIndex;
				}

				m_grid.CurrentCell = m_grid[0, prevRow];
			}

			PaApp.SettingsHandler.LoadGridProperties(m_grid);
			AdjustGridRows();
			m_grid.IsDirty = false;
			chkShowDefaults.Enabled = hasDefaultSequences;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjusts the rows in the specified grid by letting the grid calculate the row
		/// heights automatically, then adds an extra amount, found in the settings file,
		/// to each row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustGridRows()
		{
			try
			{
				// Sometimes (or maybe always) this throws an exception when
				// the first row is the only row and is the NewRowIndex row.
				m_grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			}
			catch { }

			m_grid.AutoResizeRows();

			int extraRowHeight =
				PaApp.SettingsHandler.GetIntSettingsValue(Name, "ambiggridextrarowheight", 3);

			foreach (DataGridViewRow row in m_grid.Rows)
				row.Height += extraRowHeight;
		}

		#region Overridden methods of base class
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return m_grid.IsDirty; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			//foreach (DataGridViewColumn col in m_grid.Columns)
			//    col.Name = "col" + col.DisplayIndex;

			PaApp.SettingsHandler.SaveGridProperties(m_grid);
			PaApp.SettingsHandler.SaveFormProperties(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			if (!AmbiguousSequencesChanged)
			{
				m_grid.IsDirty = false;
				return false;
			}

			AmbiguousSequences ambigSeqList = new AmbiguousSequences();

			foreach (DataGridViewRow row in m_grid.Rows)
			{
				if (row.Index != m_grid.NewRowIndex)
				{
					string phone = row.Cells["seq"].Value as string;
					string basechar = row.Cells["base"].Value as string;

					// Don't bother saving anything if there isn't
					// a phone (sequence) or base character.
					if (phone != null && phone.Trim().Length > 0 &&
						basechar != null && basechar.Trim().Length > 0)
					{
						AmbiguousSeq seq = new AmbiguousSeq(phone.Trim());
						seq.BaseChar = basechar.Trim();
						seq.Convert = (row.Cells["convert"].Value == null ?
							false : (bool)row.Cells["convert"].Value);

						seq.IsDefault = (bool)row.Cells["default"].Value;
						seq.IsAutoGeneratedDefault = (bool)row.Cells["autodefault"].Value;
						ambigSeqList.Add(seq);
					}
				}
			}

			ambigSeqList.Save(PaApp.Project.ProjectPathFilePrefix);
			DataUtils.IPACharCache.AmbiguousSequences =
				AmbiguousSequences.Load(PaApp.Project.ProjectPathFilePrefix);

			PaApp.Project.ReloadDataSources();

			//PaApp.MsgMediator.SendMessage("AmbiguousSequencesSaved", ambigSeqList);
			return true;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not there are any changes to the ambiguous
		/// sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool AmbiguousSequencesChanged
		{
			get
			{
				if (DataUtils.IPACharCache.AmbiguousSequences == null)
				{
					if (m_grid.RowCountLessNewRow > 0)
						return true;
				}
				else if (DataUtils.IPACharCache.AmbiguousSequences.Count !=
					m_grid.RowCountLessNewRow)
				{
					return true;
				}

				// Go through the ambiguous sequences in the grid and check them against
				// those found in the project's list of ambiguous sequences.
				foreach (DataGridViewRow row in m_grid.Rows)
				{
					if (row.Index == m_grid.NewRowIndex)
						continue;

					string seq = row.Cells["seq"].Value as string;
					string baseChar = row.Cells["base"].Value as string;
					bool convert = (bool)row.Cells["convert"].Value;

					AmbiguousSeq ambigSeq =
						DataUtils.IPACharCache.AmbiguousSequences.GetAmbiguousSeq(seq, false);

					if (ambigSeq == null || ambigSeq.Convert != convert || ambigSeq.BaseChar != baseChar)
						return true;
				}

				return false;
			}
		}

		#region Grid event methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the new row has its height set correctly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			AdjustGridRows();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure new rows get proper default values
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			e.Row.Cells["seq"].Value = string.Empty;
			e.Row.Cells["convert"].Value = true;
			e.Row.Cells["default"].Value = false;
			e.Row.Cells["autodefault"].Value = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Validate the edited base character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			if (e.RowIndex == m_grid.NewRowIndex)
				return;

			if (e.ColumnIndex == 0)
				e.Cancel = ValidateSequence(e.RowIndex, e.FormattedValue as string);
			else if (e.ColumnIndex == 2)
				e.Cancel = ValidateBaseCharacter(e.RowIndex, e.FormattedValue as string);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns whether or not the row edit should be cancelled due to a duplicate
		/// sequence.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ValidateSequence(int row, string newSeq)
		{
			// Make sure a unit was specified.
			//if (string.IsNullOrEmpty(newUnit))
			//    msg = Properties.Resources.kstidAmbiguousBaseCharMissingMsg;

			for (int i = 0; i < m_grid.NewRowIndex; i++)
			{
				if (i != row && m_grid[0, i].Value as string == newSeq)
				{
					bool isDefault = ((bool)m_grid["default", row].Value ||
						(bool)m_grid["autodefault", row].Value);

					string msg = (isDefault ?
						Properties.Resources.kstidAmbiguousSeqDuplicateMsg2 :
						Properties.Resources.kstidAmbiguousSeqDuplicateMsg1);

					SilUtils.Utils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return true;
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns whether or not the row edit should be cancelled due to an invalid
		/// base character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ValidateBaseCharacter(int row, string newBaseChar)
		{

			if (row < 0 || row >= m_grid.RowCount)
				return false;

			string msg = null;
			string phone = m_grid["seq", row].Value as string;

			// Check if a base character has been specified.
			if (string.IsNullOrEmpty(newBaseChar))
			{
				// No base character is fine when there isn't a sequence specified.
				if (string.IsNullOrEmpty(phone))
					return false;

				// At this point, we know we have a sequence but no base character
				msg = Properties.Resources.kstidAmbiguousBaseCharMissingMsg;
			}

			if (msg == null)
			{
				// Make sure there is an ambiguous sequence before specifying a base character.
				if (string.IsNullOrEmpty(phone))
					msg = Properties.Resources.kstidAmbiguousTransMissingMsg;
			}

			// Make sure the new base character is part of the ambiguous sequence.
			if (msg == null && phone != null && !phone.Contains(newBaseChar))
				msg = Properties.Resources.kstidAmbiguousBaseCharNotInTransMsg;

			if (msg != null)
			{
				SilUtils.Utils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			if (e.ColumnIndex == 1 && e.RowIndex == m_grid.NewRowIndex)
				e.Cancel = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			// Get the ambiguous sequence.
			string phone = m_grid["seq", e.RowIndex].Value as string;
			if (phone != null)
				phone = phone.Trim();

			if (string.IsNullOrEmpty(phone))
			{
				PaApp.MsgMediator.PostMessage("RemoveAmbiguousSeqRow", e.RowIndex);
				return;
			}

			// When the base character was edited then automatically determine
			// the C or V type of the ambiguous sequence.
			if (e.ColumnIndex == 2)
			{
				string newBaseChar = m_grid["base", e.RowIndex].Value as string;
				m_grid["cvpattern", e.RowIndex].Value =
					PaApp.PhoneCache.GetCVPattern(newBaseChar);
			}
			else if (e.ColumnIndex == 0)
			{
				PhoneInfo phoneInfo = new PhoneInfo(phone);

				string prevBaseChar = m_grid["base", e.RowIndex].Value as string;
				if (prevBaseChar == null || !phone.Contains(prevBaseChar))
				{
					string newBaseChar = phoneInfo.BaseCharacter.ToString();
					m_grid["base", e.RowIndex].Value = newBaseChar;
					m_grid["cvpattern", e.RowIndex].Value =
						PaApp.PhoneCache.GetCVPattern(newBaseChar);
				}
			}

			m_grid.IsDirty = AmbiguousSequencesChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message is called when a user edits away the ambiguous transcription. It is
		/// posted in the after cell edit event because rows cannot be removed in the after
		/// cell edit event handler.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveAmbiguousSeqRow(object args)
		{
			if (args.GetType() == typeof(int))
			{
				int rowIndex = (int)args;
				if (rowIndex >= 0 && rowIndex < m_grid.RowCountLessNewRow)
				{
					m_grid.Rows.RemoveAt(rowIndex);

					while (rowIndex >= 0 && rowIndex >= m_grid.RowCount)
						rowIndex--;

					if (rowIndex >= 0 && rowIndex < m_grid.RowCountLessNewRow)
						m_grid.CurrentCell = m_grid["seq", rowIndex];
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Don't allow deleting default sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			if (e.Row == null)
				return;

			string msg = null;

			if (e.Row.Cells["autodefault"].Value != null && (bool)e.Row.Cells["autodefault"].Value)
				msg = Properties.Resources.kstidAmbiguousSeqCantDeleteAutoGenMsg;
			else if (e.Row.Cells["default"].Value != null && (bool)e.Row.Cells["default"].Value)
				msg = Properties.Resources.kstidAmbiguousSeqCantDeleteDefaultMsg;

			if (msg != null)
			{
				SilUtils.Utils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				e.Cancel = true;
			}
		}










		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Change the visible state of the default ambiguous sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void chkShowDefaults_CheckedChanged(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in m_grid.Rows)
			{
				if (row.Index == m_grid.NewRowIndex)
					continue;

				if ((bool)row.Cells["default"].Value || (bool)row.Cells["autodefault"].Value)
					row.Visible = chkShowDefaults.Checked;
			}

			if (chkShowDefaults.Checked)
				return;

			int currRow = m_grid.CurrentCellAddress.Y;
			if (currRow < 0 || !m_grid.Rows[currRow].Visible)
			{
				foreach (DataGridViewRow row in m_grid.Rows)
				{
					if (row.Visible)
					{
						row.Selected = true;
						return;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			PaApp.ShowHelpTopic("hidAmbiguousSequencesDlg");
		}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used in PA.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] {this};
		}

		#endregion
	}
}