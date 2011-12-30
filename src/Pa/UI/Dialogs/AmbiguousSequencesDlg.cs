using System;
using System.Linq;
using System.Windows.Forms;
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
		private readonly PaProject _project;

		/// ------------------------------------------------------------------------------------
		public AmbiguousSequencesDlg()
		{
			InitializeComponent();

			if (!PaintingHelper.CanPaintVisualStyle())
				pnlGrid.BorderStyle = BorderStyle.Fixed3D;

			BuildGrid();
			
			_grid.Columns["seq"].DefaultCellStyle.Font = App.PhoneticFont;
			_grid.Columns["seq"].CellTemplate.Style.Font = App.PhoneticFont;
			_grid.Columns["base"].DefaultCellStyle.Font = App.PhoneticFont;
			_grid.Columns["base"].CellTemplate.Style.Font = App.PhoneticFont;
			_grid.Columns["cvpattern"].DefaultCellStyle.Font = App.PhoneticFont;
			_grid.Columns["cvpattern"].CellTemplate.Style.Font = App.PhoneticFont;
		}

		/// ------------------------------------------------------------------------------------
		public AmbiguousSequencesDlg(PaProject project)
			: this()
		{
			_project = project;
			LoadGrid();

			foreach (var row in _grid.GetRows())
			{
				row.Cells["seq"].Style.Font = App.PhoneticFont;
				row.Cells["base"].Style.Font = App.PhoneticFont;
				row.Cells["cvpattern"].Style.Font = App.PhoneticFont;
			}

			_grid.AdjustGridRows(Settings.Default.AmbiguousSequencesDlgGridExtraRowHeight);
			App.AddMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			App.RemoveMediatorColleague(this);
			base.OnFormClosed(e);
		}

		/// ------------------------------------------------------------------------------------
		private void BuildGrid()
		{
			_grid.Name = Name + "AmbigGrid";
			_grid.AutoGenerateColumns = false;
			_grid.AllowUserToAddRows = true;
			_grid.AllowUserToDeleteRows = true;
			_grid.AllowUserToOrderColumns = false;
			_grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
			_grid.Font = FontHelper.UIFont;
			App.SetGridSelectionColors(_grid, true);

			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("seq");
			col.Width = 90;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = App.PhoneticFont;
			col.CellTemplate.Style.Font = App.PhoneticFont;
			col.HeaderText = App.GetString("AmbiguousSequencesDlg.SeqColumnHdg",
				"Sequence", "Column heading in ambiguous sequences dialog box.");
			
			_grid.Columns.Add(col);

			col = SilGrid.CreateCheckBoxColumn("convert");
			col.Width = 75;
			col.CellTemplate.ValueType = typeof(bool);
			col.HeaderText = App.GetString("AmbiguousSequencesDlg.ConvertColumnHdg",
				"Treat as one unit?", "Column heading in ambiguous sequences dialog box.");

			_grid.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("base");
			col.Width = 75;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = App.PhoneticFont;
			col.CellTemplate.Style.Font = App.PhoneticFont;
			col.HeaderText = App.GetString("AmbiguousSequencesDlg.BaseCharColumnHdg",
				"Base Character", "Column heading in ambiguous sequences dialog box.");
			
			_grid.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("cvpattern");
			col.ReadOnly = true;
			col.Width = 70;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = App.PhoneticFont;
			col.CellTemplate.Style.Font = App.PhoneticFont;
			col.HeaderText = App.GetString("AmbiguousSequencesDlg.AmbiguousCVPatternColumnHdg",
				"CV Pattern", "Column heading in ambiguous sequences dialog box.");

			_grid.Columns.Add(col);

			col = SilGrid.CreateCheckBoxColumn("generated");
			col.Visible = false;
			_grid.Columns.Add(col);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the grid with the ambiguous items information from the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadGrid()
		{
			int prevRow = _grid.CurrentCellAddress.Y;

			_grid.Rows.Clear();
			var ambigSeqList = _project.AmbiguousSequences;

			if (ambigSeqList == null || ambigSeqList.Count == 0)
			{
				_grid.IsDirty = false;
				return;
			}

			_grid.Rows.Add(ambigSeqList.Count);

			for (int i = 0; i < ambigSeqList.Count; i++)
			{
				_grid["seq", i].Value = ambigSeqList[i].Literal;
				_grid["convert", i].Value = ambigSeqList[i].Convert;
				_grid["base", i].Value = ambigSeqList[i].BaseChar;
				_grid["generated", i].Value = ambigSeqList[i].IsGenerated;

				if (!string.IsNullOrEmpty(ambigSeqList[i].BaseChar))
				{
					_grid["cvpattern", i].Value =
						_project.PhoneCache.GetCVPattern(ambigSeqList[i].BaseChar);
				}

				if (ambigSeqList[i].IsGenerated)
					_grid.Rows[i].Cells[0].ReadOnly = true;
			}

			// Select a row if there isn't one currently selected.
			if (_grid.CurrentCellAddress.Y < 0 && _grid.RowCountLessNewRow > 0 &&
				_grid.Rows.GetRowCount(DataGridViewElementStates.Visible) > 0)
			{
				// Check if the previous row is a valid row.
				if (prevRow < 0 || prevRow >= _grid.RowCountLessNewRow ||
					!_grid.Rows[prevRow].Visible)
				{
					prevRow = _grid.FirstDisplayedScrollingRowIndex;
				}

				_grid.CurrentCell = _grid[0, prevRow];
			}

			if (Settings.Default.AmbiguousSequencesDlgGrid != null)
				Settings.Default.AmbiguousSequencesDlgGrid.InitializeGrid(_grid);

			_grid.AdjustGridRows(Settings.Default.AmbiguousSequencesDlgGridExtraRowHeight);
			_grid.IsDirty = false;
		}

		#region Overridden methods of base class
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return _grid.IsDirty; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.AmbiguousSequencesDlgGrid = GridSettings.Create(_grid);
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			if (!AmbiguousSequencesChanged)
			{
				_grid.IsDirty = false;
				return true;
			}
			
			foreach (var row in _grid.GetRows().Where(r => r.Index != _grid.NewRowIndex))
			{
				var phone = row.Cells["seq"].Value as string;
				var basechar = row.Cells["base"].Value as string;
				string msg = null;

				if (phone == null || phone.Trim().Length == 0)
				{
					_grid.CurrentCell = _grid["seq", row.Index];
					msg = App.GetString("DialogBoxes.AmbiguousSequencesDlg.MustSpecifySequenceMsg",
						"You must specify a sequence.");
				}
				if (basechar == null || basechar.Trim().Length == 0)
				{
					_grid.CurrentCell = _grid["base", row.Index];
					msg = App.GetString("DialogBoxes.AmbiguousSequencesDlg.MustSpecifyBaseCharacterMsg",
						"You must specify a base character.");
				}

				if (msg != null)
				{
					App.NotifyUserOfProblem(msg);
					_grid.BeginEdit(false);
					return false;
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			if (!AmbiguousSequencesChanged)
			{
				_grid.IsDirty = false;
				return false;
			}

			var ambigSeqList = new AmbiguousSequences();

			foreach (var row in _grid.GetRows().Where(r => r.Index != _grid.NewRowIndex))
			{
				var phone = row.Cells["seq"].Value as string;
				var basechar = row.Cells["base"].Value as string;

				// Don't bother saving anything if there isn't
				// a phone (sequence) or base character.
				if (phone != null && phone.Trim().Length > 0 &&
					basechar != null && basechar.Trim().Length > 0)
				{
					var seq = new AmbiguousSeq(phone.Trim());
					seq.BaseChar = basechar.Trim();
					seq.Convert = (row.Cells["convert"].Value != null && (bool)row.Cells["convert"].Value);
					seq.IsGenerated = (bool)row.Cells["generated"].Value;
					ambigSeqList.Add(seq);
				}
			}

			App.MsgMediator.SendMessage("BeforeAmbiguousSequencesSaved", ambigSeqList);
			_project.SaveAndLoadAmbiguousSequences(ambigSeqList);
			App.MsgMediator.SendMessage("AfterAmbiguousSequencesSaved", ambigSeqList);
			_project.ReloadDataSources();
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
				if (_project.AmbiguousSequences == null)
				{
					if (_grid.RowCountLessNewRow > 0)
						return true;
				}
				else if (_project.AmbiguousSequences.Count != _grid.RowCountLessNewRow)
				{
					return true;
				}

				// Go through the ambiguous sequences in the grid and check them against
				// those found in the project's list of ambiguous sequences.
				foreach (var row in _grid.GetRows().Where(r => r.Index != _grid.NewRowIndex))
				{
					string seq = row.Cells["seq"].Value as string;
					string baseChar = row.Cells["base"].Value as string;
					bool convert = (bool)row.Cells["convert"].Value;

					var ambigSeq = _project.AmbiguousSequences.GetAmbiguousSeq(seq, false);
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
		private void HandleGridRowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			_grid.AdjustGridRows(Settings.Default.AmbiguousSequencesDlgGridExtraRowHeight);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure new rows get proper default values
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleGridDefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			e.Row.Cells["seq"].Value = string.Empty;
			e.Row.Cells["convert"].Value = true;
			e.Row.Cells["base"].Value = string.Empty;
			e.Row.Cells["generated"].Value = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Validate the edited base character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleGridCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			if (e.RowIndex == _grid.NewRowIndex)
				return;

			if (_grid.GetColumnName(e.ColumnIndex) == "seq")
				e.Cancel = !ValidateSequence(e.RowIndex, e.FormattedValue as string);
			else if (_grid.GetColumnName(e.ColumnIndex) == "base")
				e.Cancel = !ValidateBaseCharacter(e.RowIndex, e.FormattedValue as string);	
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (_grid.GetColumnName(e.ColumnIndex) == "convert" &&
				(bool)_grid["generated", e.RowIndex].Value &&
				!(bool)_grid["convert", e.RowIndex].EditedFormattedValue)
			{
				App.NotifyUserOfProblem(App.GetString("DialogBoxes.AmbiguousSequencesDlg.MustTreatGeneratedSequencesAsUnitMsg",
					"This ambiguous sequence was automatically generated based on phonetic " +
					"transcriptions found in one or more data sources. Automatically " +
					"generated ambiguous sequences must be treated as one unit."));

				_grid.CancelEdit();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns whether or not the row edit should be cancelled due to a duplicate
		/// sequence.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ValidateSequence(int row, string newSeq)
		{
			for (int i = 0; i < _grid.NewRowIndex; i++)
			{
				if (i != row && _grid[0, i].Value as string == newSeq)
				{
					App.NotifyUserOfProblem(App.GetString("DialogBoxes.AmbiguousSequencesDlg.DuplicateSeqMsg1",
						"That sequence already exists.", "Message displayed in ambiguous sequences " +
						"dialog box when identical sequences exist."));
					return false;
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns whether or not the row edit should be cancelled due to an invalid
		/// base character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ValidateBaseCharacter(int row, string newBaseChar)
		{

			if (row < 0 || row >= _grid.RowCount)
				return true;

			string msg = null;
			string phone = _grid["seq", row].Value as string;

			// Check if a base character has been specified.
			if (string.IsNullOrEmpty(newBaseChar))
			{
				// No base character is fine when there isn't a sequence specified.
				if (string.IsNullOrEmpty(phone))
					return true;

				// At this point, we know we have a sequence but no base character
				msg = App.GetString("DialogBoxes.AmbiguousSequencesDlg.BaseCharMissingMsg",
					"You must specify a base character.", "Message displayed when trying to " +
					"save ambiguous sequences in the ambiguous sequences dialog box, when one " +
					"or more sequence does not have a base character specified.");
			}

			if (msg == null)
			{
				// Make sure there is an ambiguous sequence before specifying a base character.
				if (string.IsNullOrEmpty(phone))
				{
					msg = App.GetString("DialogBoxes.AmbiguousSequencesDlg.MissingSequenceMsg",
						"A base character may not be specified until you have specified an ambiguous sequence.",
						"Message dislpayed in ambiguous sequences dialog box.");
				}
			}

			// Make sure the new base character is part of the ambiguous sequence.
			if (msg == null && phone != null && !phone.Contains(newBaseChar))
			{
				msg = App.GetString("DialogBoxes.AmbiguousSequencesDlg.BaseCharNotInSeqMsg",
					"Your base character must be contained within its associated ambiguous sequence.",
					"Message dislpayed in ambiguous sequences dialog box.");
			}

			if (msg != null)
			{
				App.NotifyUserOfProblem(msg);
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			if (e.ColumnIndex == 1 && e.RowIndex == _grid.NewRowIndex)
				e.Cancel = true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			// Get the ambiguous sequence.
			string phone = _grid["seq", e.RowIndex].Value as string;
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
				string newBaseChar = _grid["base", e.RowIndex].Value as string;
				_grid["cvpattern", e.RowIndex].Value =
					_project.PhoneCache.GetCVPattern(newBaseChar);
			}
			else if (e.ColumnIndex == 0)
			{
				var phoneInfo = new PhoneInfo(_project.AmbiguousSequences, phone);
				var prevBaseChar = _grid["base", e.RowIndex].Value as string;
				if (prevBaseChar == null || !phone.Contains(prevBaseChar))
				{
					string newBaseChar = phoneInfo.BaseCharacter.ToString();
					_grid["base", e.RowIndex].Value = newBaseChar;
					_grid["cvpattern", e.RowIndex].Value =
						_project.PhoneCache.GetCVPattern(newBaseChar);
				}
			}

			_grid.IsDirty = AmbiguousSequencesChanged;
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
			if (args is int)
			{
				int rowIndex = (int)args;
				if (rowIndex >= 0 && rowIndex < _grid.RowCountLessNewRow)
				{
					_grid.Rows.RemoveAt(rowIndex);

					while (rowIndex >= 0 && rowIndex >= _grid.RowCount)
						rowIndex--;

					if (rowIndex >= 0 && rowIndex < _grid.RowCountLessNewRow)
						_grid.CurrentCell = _grid["seq", rowIndex];
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Don't allow deleting generated sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleGridUserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			if (e.Row == null)
				return;

			if (e.Row.Cells["generated"].Value != null && (bool)e.Row.Cells["generated"].Value)
			{
				var msg = App.GetString("DialogBoxes.AmbiguousSequencesDlg.CantDeleteGeneratedAmbiguousSeqMsg",
					"This ambiguous sequence was automatically generated based on phonetic " +
					"transcriptions found in one or more data sources. Automatically " +
					"generated ambiguous sequences may not be deleted.",
					"Message displayed when trying to delete an automatically generated ambiguous " +
					"sequence in the ambiguous sequence dialog box.");
				
				App.NotifyUserOfProblem(msg);
				e.Cancel = true;
			}
		}

		#endregion

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