using System;
using System.Linq;
using System.Windows.Forms;
using Localization;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Dialog for defining ambiguous sequences.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class AmbiguousSequencesDlg : OKCancelDlgBase, IxCoreColleague
	{
		/// ------------------------------------------------------------------------------------
		public AmbiguousSequencesDlg()
		{
			InitializeComponent();

			if (!PaintingHelper.CanPaintVisualStyle())
				pnlGrid.BorderStyle = BorderStyle.Fixed3D;

			BuildGrid();
			LoadGrid();
			
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

			FeaturesDlg.AdjustGridRows(m_grid, Settings.Default.AmbiguousSequencesDlgGridExtraRowHeight);
			App.AddMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			App.RemoveMediatorColleague(this);
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
			App.SetGridSelectionColors(m_grid, true);

			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("seq");
			col.Width = 90;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			col.HeaderText = App.LocalizeString(Name + ".AmbiguousSeqColumnHdg",
				"Sequence", "Column heading in ambiguous sequences dialog box.",
				locExtender.LocalizationGroup, LocalizationCategory.Other,
				LocalizationPriority.High);
			
			m_grid.Columns.Add(col);

			col = SilGrid.CreateCheckBoxColumn("convert");
			col.Width = 75;
			col.CellTemplate.ValueType = typeof(bool);
			col.HeaderText = App.LocalizeString(Name + ".AmbiguousConvertColumnHdg",
				"Treat as one unit?", "Column heading in ambiguous sequences dialog box.",
				locExtender.LocalizationGroup, LocalizationCategory.Other,
				LocalizationPriority.High);

			m_grid.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("base");
			col.Width = 75;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			col.HeaderText = App.LocalizeString(Name + ".AmbiguousBaseCharColumnHdg",
				"Base Character", "Column heading in ambiguous sequences dialog box.",
				locExtender.LocalizationGroup, LocalizationCategory.Other,
				LocalizationPriority.High);
			
			m_grid.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("cvpattern");
			col.ReadOnly = true;
			col.Width = 70;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			col.HeaderText = App.LocalizeString(Name + ".AmbiguousCVPatternColumnHdg",
				"CV Pattern", "Column heading in ambiguous sequences dialog box.",
				locExtender.LocalizationGroup, LocalizationCategory.Other,
				LocalizationPriority.High);

			m_grid.Columns.Add(col);

			col = SilGrid.CreateCheckBoxColumn("generated");
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
			int prevRow = m_grid.CurrentCellAddress.Y;

			m_grid.Rows.Clear();
			var ambigSeqList = App.IPASymbolCache.AmbiguousSequences;

			if (ambigSeqList == null || ambigSeqList.Count == 0)
			{
				m_grid.IsDirty = false;
				return;
			}

			m_grid.Rows.Add(ambigSeqList.Count);

			for (int i = 0; i < ambigSeqList.Count; i++)
			{
				m_grid["seq", i].Value = ambigSeqList[i].Literal;
				m_grid["convert", i].Value = ambigSeqList[i].Convert;
				m_grid["base", i].Value = ambigSeqList[i].BaseChar;
				m_grid["generated", i].Value = ambigSeqList[i].IsGenerated;

				if (!string.IsNullOrEmpty(ambigSeqList[i].BaseChar))
				{
					m_grid["cvpattern", i].Value =
						App.PhoneCache.GetCVPattern(ambigSeqList[i].BaseChar);
				}

				if (ambigSeqList[i].IsGenerated)
				{
					m_grid.Rows[i].Cells[0].ReadOnly = true;
					if (!chkShowGenerated.Checked)
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

			if (Settings.Default.AmbiguousSequencesDlgGrid != null)
				Settings.Default.AmbiguousSequencesDlgGrid.InitializeGrid(m_grid);

			FeaturesDlg.AdjustGridRows(m_grid, Settings.Default.AmbiguousSequencesDlgGridExtraRowHeight);
			m_grid.IsDirty = false;
			chkShowGenerated.Visible = ambigSeqList.Any(x => x.IsGenerated);
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
			Settings.Default.AmbiguousSequencesDlgGrid = GridSettings.Create(m_grid);
			base.SaveSettings();
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

						seq.IsGenerated = (bool)row.Cells["generated"].Value;
						ambigSeqList.Add(seq);
					}
				}
			}

			App.MsgMediator.SendMessage("BeforeAmbiguousSequencesSaved", ambigSeqList);
			ambigSeqList.Save(App.Project.ProjectPathFilePrefix);
			App.IPASymbolCache.AmbiguousSequences = AmbiguousSequences.Load(App.Project.ProjectPathFilePrefix);
			App.MsgMediator.SendMessage("AfterAmbiguousSequencesSaved", ambigSeqList);
			App.Project.ReloadDataSources();
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
				if (App.IPASymbolCache.AmbiguousSequences == null)
				{
					if (m_grid.RowCountLessNewRow > 0)
						return true;
				}
				else if (App.IPASymbolCache.AmbiguousSequences.Count !=
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
						App.IPASymbolCache.AmbiguousSequences.GetAmbiguousSeq(seq, false);

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
			FeaturesDlg.AdjustGridRows(m_grid, Settings.Default.AmbiguousSequencesDlgGridExtraRowHeight);
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
			e.Row.Cells["generated"].Value = false;
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
			for (int i = 0; i < m_grid.NewRowIndex; i++)
			{
				if (i != row && m_grid[0, i].Value as string == newSeq)
				{
					string msg;
					if ((bool)m_grid["generated", row].Value)
					{
						msg = App.LocalizeString("AmbiguousSequencesDlg.DuplicateSeqMsg1",
							"That sequence already exists.", "Message displayed in ambiguous sequences " +
							"dialog box when identical sequences exist.", App.kLocalizationGroupDialogs);
					}
					else
					{
						msg = App.LocalizeString("AmbiguousSequencesDlg.DuplicateSeqMsg2",
							"That sequence already exists as a generated sequence.", "Message displayed in " +
							"ambiguous sequences dialog box when a user-added sequence is identical to a " +
							"generated sequences.",	App.kLocalizationGroupDialogs);
					}

					Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
				msg = App.LocalizeString("AmbiguousSequencesDlg.BaseCharMissingMsg",
					"You must specify a base character.", "Message displayed when trying to " +
					"save ambiguous sequences in the ambiguous sequences dialog box, when one " +
					"or more sequence does not have a base character specified.",
					App.kLocalizationGroupDialogs);
			}

			if (msg == null)
			{
				// Make sure there is an ambiguous sequence before specifying a base character.
				if (string.IsNullOrEmpty(phone))
				{
					msg = App.LocalizeString("AmbiguousSequencesDlg.MissingSequenceMsg",
						"A base character may not be specified\nuntil you have specified an ambiguous sequence.",
						"Message dislpayed in ambiguous sequences dialog box.",
						App.kLocalizationGroupDialogs);
				}
			}

			// Make sure the new base character is part of the ambiguous sequence.
			if (msg == null && phone != null && !phone.Contains(newBaseChar))
			{
				msg = App.LocalizeString("AmbiguousSequencesDlg.BaseCharNotInSeqMsg",
					"Your base character must be contained\nwithin its associated ambiguous sequence.",
					"Message dislpayed in ambiguous sequences dialog box.",
					App.kLocalizationGroupDialogs);
			}

			if (msg != null)
			{
				Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
				App.MsgMediator.PostMessage("RemoveAmbiguousSeqRow", e.RowIndex);
				return;
			}

			// When the base character was edited then automatically determine
			// the C or V type of the ambiguous sequence.
			if (e.ColumnIndex == 2)
			{
				string newBaseChar = m_grid["base", e.RowIndex].Value as string;
				m_grid["cvpattern", e.RowIndex].Value =
					App.PhoneCache.GetCVPattern(newBaseChar);
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
						App.PhoneCache.GetCVPattern(newBaseChar);
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
		/// Don't allow deleting generated sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			if (e.Row == null)
				return;

			if (e.Row.Cells["generated"].Value != null && (bool)e.Row.Cells["generated"].Value)
			{
				var msg = App.LocalizeString("AmbiguousSequencesDlg.CantDeleteGeneratedAmbiguousSeqMsg",
					"This ambiguous sequence was automatically generated based\non phonetic " +
					"transcriptions found in one or more data sources.\nAutomatically " +
					"generated ambiguous sequences may not be\ndeleted. If you do not want " +
					"Phonology Assistant to treat this\nsequence as a unit, clear the 'Treat " +
					"as one Unit?’check box.", "Message displayed when trying to delete an " +
					"automatically generated ambiguous sequence in the ambiguous sequence " +
					App.kLocalizationGroupDialogs);
				
				Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				e.Cancel = true;
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Change the visible state of the generated ambiguous sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleShowGeneratedCheckedChanged(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in m_grid.Rows)
			{
				if (row.Index == m_grid.NewRowIndex)
					continue;

				if ((bool)row.Cells["generated"].Value)
					row.Visible = chkShowGenerated.Checked;
			}

			if (chkShowGenerated.Checked)
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
			App.ShowHelpTopic("hidAmbiguousSequencesDlg");
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