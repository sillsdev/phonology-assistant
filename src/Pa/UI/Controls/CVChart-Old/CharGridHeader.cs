using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using SilUtils;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CharGridHeader : Panel
	{
		private static TextBox m_textbox;

		private bool m_subHeadingsVisible = false;
		private int m_group;
		private bool m_selected = false;
		private readonly List<DataGridViewColumn> m_ownedCols;
		private readonly List<DataGridViewRow> m_ownedRows;
		private readonly Label m_heading;
		private readonly List<Label> m_subHeadings;
		private readonly bool m_isForColumns = true;
		private readonly TextFormatFlags m_txtFmtFlags = TextFormatFlags.HidePrefix;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeader(string text, bool forColumns)
		{
			m_isForColumns = forColumns;
			DoubleBuffered = true;

			if (forColumns)
			{
				// Leave room to paint a vertical line.
				Padding = new Padding(0, 0, 1, 0);
				Dock = DockStyle.Left;
				m_ownedCols = new List<DataGridViewColumn>();
				m_txtFmtFlags |= TextFormatFlags.HorizontalCenter |	TextFormatFlags.Bottom |
					TextFormatFlags.NoFullWidthCharacterBreak;
			}
			else
			{
				// Leave room to paint a vertical line.
				Padding = new Padding(0, 0, 0, 1);
				Dock = DockStyle.Top;
				m_ownedRows = new List<DataGridViewRow>();
				m_txtFmtFlags |= TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
					TextFormatFlags.EndEllipsis | TextFormatFlags.ModifyString;
			}

			m_heading = CreateLabel(text, false);
			m_subHeadings = new List<Label>();

			if (m_textbox == null)
			{
				m_textbox = new TextBox();
				m_textbox.BorderStyle = BorderStyle.None;
				m_textbox.Multiline = true;
				m_textbox.Font = FontHelper.UIFont;
				m_textbox.KeyPress += new KeyPressEventHandler(m_textbox_KeyPress);
				m_textbox.Leave += new EventHandler(m_textbox_Leave);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Label CreateLabel(string text, bool isSubHeading)
		{
			Label lbl = new Label();
			lbl.AutoSize = false;
			lbl.AutoEllipsis = true;
			lbl.Text = text;
			lbl.Font = FontHelper.UIFont;
			lbl.BackColor = Color.Transparent;
			lbl.ForeColor = SystemColors.WindowText;
			lbl.MouseDown += new MouseEventHandler(HandleLabelMouseDown);
			lbl.MouseDoubleClick += new MouseEventHandler(HandleLabelMouseDoubleClick);
			lbl.Paint += new PaintEventHandler(HandleLabelPaint);
			lbl.TextAlign = (m_isForColumns ? ContentAlignment.BottomCenter :
				ContentAlignment.MiddleLeft);

			if (!isSubHeading)
			{
				lbl.Dock = DockStyle.Fill;
				//lbl.Paint += new PaintEventHandler(m_heading_Paint);
			}
			else
			{
				// Do some extra stuff when the label is a sub heading.
				Label lastSubHdg = (m_subHeadings.Count == 0 ? null :
					m_subHeadings[m_subHeadings.Count - 1]);

				if (m_isForColumns)
				{
					lbl.Left = (lastSubHdg == null ? 0 : lastSubHdg.Right);
					lbl.Width = m_ownedCols[0].Width;
				}
				else
				{
					lbl.Top = (lastSubHdg == null ? 0 : lastSubHdg.Bottom);
					lbl.Height = m_ownedRows[0].Height;
				}

				lbl.Paint += new PaintEventHandler(HandleSubHeadingPaint);
				m_subHeadings.Add(lbl);
			}

			Controls.Add(lbl);
			return lbl;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return m_heading.Text;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddColumn(DataGridViewColumn col)
		{
			AddColumn(col, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddColumn(DataGridViewColumn col, string subheadtext)
		{
			if (m_isForColumns)
			{
				col.Tag = this;
				m_ownedCols.Add(col);
				Width = m_ownedCols.Count * col.Width;
				AddSubHeading(subheadtext);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddRow(DataGridViewRow row)
		{
			AddRow(row, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddRow(DataGridViewRow row, string subheadtext)
		{
			if (!m_isForColumns)
			{
				row.Tag = this;
				m_ownedRows.Add(row);
				Height = m_ownedRows.Count * row.Height;
				AddSubHeading(subheadtext);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the header is selected.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Selected
		{
			get { return m_selected; }
			set
			{
				m_selected = value;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not sub headings are visible.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SubHeadingsVisible
		{
			get { return m_subHeadingsVisible; }
			set
			{
				m_subHeadingsVisible = value;

				if (m_subHeadingsVisible)
					m_heading.Dock = (m_isForColumns ? DockStyle.Top : DockStyle.Left);
				else
					m_heading.Dock = DockStyle.Fill;

				ResizeHeading(((CharGridHeaderCollectionPanel)Parent).SplitPosition);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddSubHeading(string text)
		{
			int splitPos = ((CharGridHeaderCollectionPanel)Parent).SplitPosition;

			if (m_isForColumns)
			{
				// Make sure there's a sub heading for each column.
				while (m_subHeadings.Count < m_ownedCols.Count)
					CreateLabel(text, true);
			}
			else
			{
				// Make sure there's a sub heading for each row.
				while (m_subHeadings.Count < m_ownedRows.Count)
					CreateLabel(text, true);
			}

			ResizeHeading(splitPos);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all the heading's owned rows from the grid to which the rows belong.
		/// Then it removes the row from the heading's owned row collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveOwnedRows()
		{
			// Make a local copy of the owned rows list so we can clear the member
			// variable copy because it will be referenced during removal of grid rows
			// and when it's referenced, it shouldn't contain rows that were just removed.
			DataGridViewRow[] tmpList = new DataGridViewRow[m_ownedRows.Count];
			m_ownedRows.CopyTo(tmpList);
			m_ownedRows.Clear();

			foreach (DataGridViewRow row in tmpList)
			{
				if (row.DataGridView != null)
					row.DataGridView.Rows.Remove(row);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all the heading's owned columns from the grid to which the columns belong.
		/// Then it removes the columns from the heading's owned column collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveOwnedColumns()
		{
			// Make a local copy of the owned column list so we can clear the member
			// variable copy because it will be referenced during removal of grid columns
			// and when it's referenced, it shouldn't contain columns that were just removed.
			DataGridViewColumn[] tmpList = new DataGridViewColumn[m_ownedCols.Count];
			m_ownedCols.CopyTo(tmpList);
			m_ownedCols.Clear();

			foreach (DataGridViewColumn col in tmpList)
			{
				if (col.DataGridView != null)
					col.DataGridView.Columns.Remove(col);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a row and its subheading from the header's internal collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveRow(DataGridViewRow row)
		{
			// Remove the row's subheading.
			for (int i = 0; i < m_ownedRows.Count; i++)
			{
				if (m_ownedRows[i] == row)
					m_subHeadings.RemoveAt(i);
			}

			m_ownedRows.Remove(row);
			Height = m_ownedRows.Count * row.Height;

			if (m_subHeadings.Count == 1 && SubHeadingsVisible)
				SubHeadingsVisible = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a column and its subheading from the header's internal collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveColumn(DataGridViewColumn col)
		{
			// Remove the column's subheading.
			for (int i = 0; i < m_ownedCols.Count; i++)
			{
				if (m_ownedCols[i] == col)
					m_subHeadings.RemoveAt(i);
			}

			m_ownedCols.Remove(col);
			Width = m_ownedCols.Count * col.Width;

			if (m_subHeadings.Count == 1 && SubHeadingsVisible)
				SubHeadingsVisible = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjusts the size of the heading and sub headings based on the specified split
		/// position.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ResizeHeading(int splitPosition)
		{
			// Resize the heading.
			if (splitPosition > 0 && m_subHeadingsVisible)
			{
				if (m_isForColumns)
					m_heading.Height = splitPosition;
				else
					m_heading.Width = splitPosition;
			}

			// Resize the sub headings accordingly.
			foreach (Label lbl in m_subHeadings)
			{
				lbl.Visible = m_subHeadingsVisible;
				if (m_subHeadingsVisible)
				{
					if (m_isForColumns)
					{
						lbl.Height = Height - m_heading.Height;
						lbl.Top = m_heading.Bottom;
					}
					else
					{
						lbl.Width = Width - m_heading.Width;
						lbl.Left = m_heading.Right;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the labels are adjusted when their container changes size.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ResizeHeading(0);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the text of the heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string HeadingText
		{
			get { return m_heading.Text; }
			set { m_heading.Text = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a user-defined value to associate with the header. Internally, the
		/// header does nothing with this except store it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public int Group
		{
			get { return m_group; }
			set { m_group = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the sub headings in the Header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<Label> SubHeaders
		{
			get	{return m_subHeadings;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the heading is for columns. If not, then
		/// it's assumed the heading is for rows.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsForColumnHeadings
		{
			get { return m_isForColumns; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get's the collection of grid rows owned by the header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<DataGridViewRow> OwnedRows
		{
			get { return m_ownedRows; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get's the collection of grid columns owned by the header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<DataGridViewColumn> OwnedColumns
		{
			get { return m_ownedCols; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not any owned columns are empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsAnyOwnedColumnEmpty
		{
			get
			{
				// Go through each owned column.
				foreach (DataGridViewColumn col in m_ownedCols)
				{
					if (IsColumnEmpty(col))
						return true;
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not all the cells in the owned columns are
		/// empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AreAllOwnedColumnsEmpty
		{
			get
			{
				// Go through each owned column.
				foreach (DataGridViewColumn col in m_ownedCols)
				{
					if (!IsColumnEmpty(col))
						return false;
				}

				return true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a value indicating whether or not all the cells in the specified column
		/// are emtpy.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsColumnEmpty(DataGridViewColumn col)
		{
			DataGridView grid = col.DataGridView;
			if (grid == null || grid.Rows == null)
				return true;

			// Go through each row, checking if the specified row and column are empty.
			for (int r = 0; r < grid.Rows.Count; r++)
			{
				CharGridCell cgc = grid[col.Index, r].Value as CharGridCell;
				if (cgc != null && cgc.Visible && !string.IsNullOrEmpty(cgc.Phone))
					return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not any owned rows are empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsAnyOwnedRowEmpty
		{
			get
			{
				// Go through each owned row.
				foreach (DataGridViewRow row in m_ownedRows)
				{
					if (IsRowEmpty(row))
						return true;
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not all the cells in the owned rows are empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AreAllOwnedRowsEmpty
		{
			get
			{
				// Go through each owned row.
				foreach (DataGridViewRow row in m_ownedRows)
				{
					if (!IsRowEmpty(row))
						return false;
				}

				return true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a value indicating whether or not all the cells in the specified row are
		/// emtpy.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsRowEmpty(DataGridViewRow row)
		{
			Debug.Assert(row != null);

			DataGridView grid = row.DataGridView;
			if (grid == null || grid.Rows == null)
				return true;

			// Go through each column, checking if the specified row and column are empty.
			for (int c = 0; c < grid.Columns.Count; c++)
			{
				CharGridCell cgc = grid[c, row.Index].Value as CharGridCell;
				if (cgc != null && cgc.Visible && !string.IsNullOrEmpty(cgc.Phone))
					return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the last row in the header's row collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataGridViewRow LastRow
		{
			get
			{
				return (m_ownedRows == null || m_ownedRows.Count == 0 ?
					null : m_ownedRows[m_ownedRows.Count - 1]);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the last column in the header's column collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataGridViewColumn LastColumn
		{
			get
			{
				return (m_ownedCols == null || m_ownedCols.Count == 0 ?
					null : m_ownedCols[m_ownedCols.Count - 1]);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the heading or a sub heading
		/// is being edited.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool InLabelEditMode
		{
			get { return m_textbox.Focused; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Put's the user in the edit mode for the label right-clicked on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void EditLabel()
		{
			Label lbl = Tag as Label;
			HandleLabelMouseDoubleClick((lbl == null ? m_heading : lbl), null);
			Tag = null;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Takes the user out of edit label mode.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void EndEditLabel()
		{
			if (InLabelEditMode)
				m_textbox_Leave(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sends a message to anyone who cares that one of the labels was right-clicked on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleLabelMouseDown(object sender, MouseEventArgs e)
		{
			Label lbl = sender as Label;
			if (lbl == null)
				return;

			if (m_textbox.Focused)
				m_textbox_Leave(null, null);

			Tag = lbl;

			App.MsgMediator.SendMessage("CharGridHeaderClicked", this);
			if (e.Button == MouseButtons.Right)
			{
				App.MsgMediator.SendMessage("CharGridHeaderRightClicked", this);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Put user in the label edit mode on a row header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleLabelMouseDoubleClick(object sender, MouseEventArgs e)
		{
			Label lbl = sender as Label;
			if (lbl == null)
				return;

			m_textbox.Size = new Size(lbl.Width - 2, lbl.Height - 2);
			m_textbox.Location = new Point(lbl.Left + 1, lbl.Top + 1);
			m_textbox.Text = lbl.Text;
			m_textbox.SelectAll();
			m_textbox.Tag = lbl;

			Controls.Add(m_textbox);
			m_textbox.BringToFront();
			m_textbox.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Leave the edit mode when the text box loses focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void m_textbox_Leave(object sender, EventArgs e)
		{
			if (m_textbox.Tag is Label)
				((Label)m_textbox.Tag).Text = m_textbox.Text;

			m_textbox.Parent.Controls.Remove(m_textbox);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Trap Enter and Escape when editing labels.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void m_textbox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Escape || e.KeyChar == (char)Keys.Enter)
			{
				if (e.KeyChar == (char)Keys.Enter)
					m_textbox_Leave(null, null);
				else if (e.KeyChar == (char)Keys.Escape)
				{
					m_textbox.Tag = null;
					m_textbox.Parent.Controls.Remove(m_textbox);
				}

				// This prevents a beep.
				e.Handled = true;
			}
		}

		#region Painting methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint a vertical line down the right edge for column headings or across the bottom
		/// for row headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			using (Pen pen = new Pen(CharGrid.kGridColor))
			{
				Rectangle rc = ClientRectangle;
				e.Graphics.DrawLine(pen, (m_isForColumns ?
					new Point(rc.Right - 1, 0) : new Point(0, rc.Bottom - 1)),
					new Point(rc.Right - 1, rc.Bottom - 1));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the heading has sub headings, then paint a line on the bottom or right of
		/// the heading to separate it from the sub headings. Also highlight the background
		/// of selected headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleLabelPaint(object sender, PaintEventArgs e)
		{
			Label lbl = sender as Label;
			if (lbl == null)
				return;

			if (m_selected)
			{
				// Draw the heading/sub-heading differently when the header is selected.
				using (SolidBrush br = new SolidBrush(ColorHelper.LightLightHighlight))
					e.Graphics.FillRectangle(br, lbl.ClientRectangle);

				using (StringFormat sf = Utils.GetStringFormat(m_isForColumns))
				using (SolidBrush br = new SolidBrush(lbl.ForeColor))
				{
					// Turn on wrapping
					sf.FormatFlags &= ~StringFormatFlags.NoWrap;

					if (m_isForColumns)
						sf.LineAlignment = StringAlignment.Far;

					e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
					e.Graphics.DrawString(lbl.Text, lbl.Font, br, lbl.ClientRectangle, sf);
				}
			}

			// Draw the lines on the bottom and right of the
			// heading to separate it from the sub headings.
			if (lbl == m_heading && m_subHeadingsVisible)
			{
				using (Pen pen = new Pen(CharGrid.kGridColor))
				{
					Rectangle rc = m_heading.ClientRectangle;
					e.Graphics.DrawLine(pen, (m_isForColumns ?
							new Point(0, rc.Bottom - 1) : new Point(rc.Right - 1, 0)),
							new Point(rc.Right - 1, rc.Bottom - 1));
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint dividing lines between each sub heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleSubHeadingPaint(object sender, PaintEventArgs e)
		{
			Label lbl = sender as Label;
			if (lbl == null)
				return;

			using (Pen pen = new Pen(CharGrid.kGridColor))
			{
				Rectangle rc = lbl.ClientRectangle;
				e.Graphics.DrawLine(pen, (m_isForColumns ?
					new Point(rc.Right - 1, 0) : new Point(0, rc.Bottom - 1)),
					new Point(rc.Right - 1, rc.Bottom - 1));
			}
		}

		#endregion
	}
}
