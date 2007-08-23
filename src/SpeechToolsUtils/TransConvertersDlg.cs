using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SIL.SpeechTools.Utils.Properties;
using SilEncConverters22;

namespace SIL.SpeechTools.Utils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This dialog displays a list of the various transcriptions that can be found in pre SA
	/// 3.0 transcribed wave files and encoding converters found in the encoding converters
	/// repository that may be applied to converting those transcriptions to Unicode. The user
	/// specifies one encoding converter to apply to each transcription type.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class TransConvertersDlg : Form
	{
		private static string s_helpURL = "file://" + Application.StartupPath + "\\Speech_Analyzer_Help.chm";
		private static string s_hlpTopic = "User_Interface/Menus/File/Transcription_Encoding_Converters.htm";

		private SilGrid m_grid;
		private TransConverterInfo m_transConverters;
		private bool m_appUsingWaitCursor;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TransConvertersDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TransConvertersDlg(TransConverterInfo transConverters,
			string[] legacyFontNames) : this()
		{
			m_transConverters = transConverters;

			BuildGrid();
			lblHelp.Font = SystemInformation.MenuFont;
			chkMakeBackup.Font = SystemInformation.MenuFont;
			int fontIndex = 0;

			// Load the grid with the transcription fields and their assigned converters.
			foreach (TransConverter converter in m_transConverters)
			{
				m_grid.Rows.Add(new object[] { converter.TransName,
					legacyFontNames[fontIndex++], converter.Converter });

				// Save the transcription converter object in its corresponding row's tag.
				DataGridViewRow row = m_grid.Rows[m_grid.Rows.Count - 1];
				row.Tag = converter;
			}

			BuildConvertersPopupList();
			AcceptButton = btnOK;
			CancelButton = btnCancel;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildGrid()
		{
			m_grid = new SilGrid();
			m_grid.Name = "TransConverterGrid";
			m_grid.Dock = DockStyle.Fill;
			m_grid.Font = SystemInformation.MenuFont;
			m_grid.TabIndex = 0;
			m_grid.AllowUserToOrderColumns = false;
			m_grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
			m_grid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;

			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("transcription");
			col.HeaderText = Resources.kstidCnvtrGridTransHdg;
			col.Width = 150;
			col.ReadOnly = true;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("font");
			col.HeaderText = Resources.kstidCnvtrGridFontHdg;
			col.Width = 150;
			col.ReadOnly = true;
			m_grid.Columns.Add(col);

			col = SilGrid.CreateSilButtonColumn("converter");
			col.Width = 150;
			col.ReadOnly = true;
			col.HeaderText = Resources.kstidCnvtrGridCnvtrHdg;
			((SilButtonColumn)col).ButtonWidth = 17;
			((SilButtonColumn)col).UseComboButtonStyle = true;
			((SilButtonColumn)col).ButtonClicked +=
				new DataGridViewCellMouseEventHandler(HandleConvertersDropDownButtonClicked);
			
			m_grid.Columns.Add(col);

			Controls.Add(m_grid);
			m_grid.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds a list of converters for the popup list of the converter column in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildConvertersPopupList()
		{
			int separatorIndex = cmnuConverters.Items.IndexOf(tsSeparator);

			// Remove all but the "None", "Browse..." and separator items.
			if (cmnuConverters.Items.Count > 3)
			{
				for (int i = separatorIndex - 1; i > 0; i--)
					cmnuConverters.Items.RemoveAt(i);
			}

			if (STUtils.EncodingConverters == null)
				return;

			// Load the converters into the popup menu.
			foreach (string mapping in STUtils.EncodingConverters.Mappings)
			{
				cmnuConverters.Items.Insert(cmnuConverters.Items.Count - 2,
					new ToolStripMenuItem(mapping));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			m_appUsingWaitCursor = Application.UseWaitCursor;
			Application.UseWaitCursor = false;
			Application.DoEvents();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			cmnuConverters.Hide();
			base.OnFormClosing(e);

			// Save any changes to converters transcription fields are assigned to.
			foreach (DataGridViewRow row in m_grid.Rows)
			{
				// Converter should be stored in the row's tag.
				TransConverter transConverter = row.Tag as TransConverter;
				if (transConverter != null)
				{
					string mapping = row.Cells["converter"].Value as string;
					transConverter.Converter = (mapping == null ? string.Empty : mapping);
				}
			}

			// Write changes to disk.
			m_transConverters.Save();
			m_grid.Dispose();
			m_grid = null;

			Application.UseWaitCursor = m_appUsingWaitCursor;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the encoding converters drop-down list under the cell whose button was just
		/// clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void HandleConvertersDropDownButtonClicked(object sender, DataGridViewCellMouseEventArgs e)
		{
			// Put a check by the converter that's assigned to the transcription in the current row.
			string converter = m_grid[e.ColumnIndex, e.RowIndex].Value as string;
			foreach (ToolStripItem menu in cmnuConverters.Items)
			{
				if (menu is ToolStripMenuItem)
					((ToolStripMenuItem)menu).Checked = (converter == menu.Text);
			}

			// Determine where to popup the encoding converter list and show it.
			Rectangle rc = m_grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
			cmnuConverters.Show(m_grid, rc.X, rc.Bottom);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Process a choice from the encoding converters drop-down list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cmnuConverters_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			cmnuConverters.Hide();

			if (e.ClickedItem == tsmiNone)
				m_grid.CurrentRow.Cells["converter"].Value = null;
			else if (e.ClickedItem != tsmiBrowse)
				m_grid.CurrentRow.Cells["converter"].Value = e.ClickedItem.Text;
			else
			{
				if (STUtils.EncodingConverters == null)
					return;
				
				// At this point, we know the user clicked the browse option.
				OpenFileDialog dlg = new OpenFileDialog();
				dlg.Multiselect = false;
				dlg.Title = Resources.kstidOFDFindConverterCaption;
				dlg.CheckPathExists = true;
				dlg.CheckFileExists = true;
				dlg.Filter = Resources.kstidFileTypeTecFile + "|" +
					Resources.kstidFileTypeMapFile + "|" +
					Resources.kstidFileTypeCCFile + "|" +
					Resources.kstidFileTypeAll;

				if (dlg.ShowDialog() != DialogResult.OK || string.IsNullOrEmpty(dlg.FileName))
					return;

				try
				{
					if (STUtils.GetConverter(dlg.FileName) == null)
					{
						// Add a converter for the default mapping file and make it's name the same as the
						// default mapping file's name.
						STUtils.EncodingConverters.Add(Path.GetFileName(dlg.FileName),
							dlg.FileName, ConvType.Unknown, string.Empty, string.Empty,
							ProcessTypeFlags.DontKnow);
					}
				}
				catch (Exception exp)
				{
					STUtils.STMsgBox(exp.Message, MessageBoxButtons.OK);
				}

				// The converter must be OK so save it in the grid and
				// rebuild the encoding converter popup list.
				m_grid.CurrentRow.Cells["converter"].Value = Path.GetFileName(dlg.FileName);
				BuildConvertersPopupList();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the help file to open when the dialog's help button is clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string HelpFilePath
		{
			get { return s_helpURL; }
			set { s_helpURL = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the help file topic to open when the dialog's help button is clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string HelpTopic
		{
			get { return s_hlpTopic; }
			set { s_hlpTopic = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnHelp_Click(object sender, EventArgs e)
		{
			Help.ShowHelp(this, HelpFilePath, HelpTopic);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not to backup the audio file before it's
		/// converted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool BackupAudioFile
		{
			get { return chkMakeBackup.Checked; }
		}
	}
}