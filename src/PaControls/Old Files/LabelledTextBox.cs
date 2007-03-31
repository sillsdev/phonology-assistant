using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Resources;
using SIL.SpeechTools.Database;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class LabelledTextBox : System.Windows.Forms.UserControl
	{
		private bool m_allowDropDown = false;
		private bool m_updateCacheOnTextChange = true;
		private int m_docId = 0;
		private string m_dbField = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public LabelledTextBox()
		{
			InitializeComponent();

			tsddLabel.Font = FontHelper.UIFont;
			tslLabel.Font = FontHelper.UIFont;
			m_text.Font = FontHelper.UIFont;

			// After the font is set on the labels (i.e. the toolstrip items) then adjust
			// the toolstrip's height as well as it's container. The toolstrip is not
			// docked in its container because it's left edge is negative and it's width
			// is a little greater than the width of the control so the rounded bottom
			// corners aren't visible.
			toolStrip1.Height = FontHelper.UIFont.Height + 7;
			pnlLabelContainer.Height = toolStrip1.Height + 5;

			tsmiEtic.Text = ResourceHelper.GetDBFieldStringWithHotKey(DBFields.Phonetic);
			tsmiEtic.Tag = DBFields.Phonetic;
			tsmiEmic.Text = ResourceHelper.GetDBFieldStringWithHotKey(DBFields.Phonemic);
			tsmiEmic.Tag = DBFields.Phonemic;
			tsmiTone.Text = ResourceHelper.GetDBFieldStringWithHotKey(DBFields.Tone);
			tsmiTone.Tag = DBFields.Tone;
			tsmiOrtho.Text = ResourceHelper.GetDBFieldStringWithHotKey(DBFields.Ortho);
			tsmiOrtho.Tag = DBFields.Ortho;
			tsmiGloss.Text = ResourceHelper.GetDBFieldStringWithHotKey(DBFields.Gloss);
			tsmiGloss.Tag = DBFields.Gloss;
			tsmiPOS.Text = ResourceHelper.GetDBFieldStringWithHotKey(DBFields.POS);
			tsmiPOS.Tag = DBFields.POS;
			tsmiRef.Text = ResourceHelper.GetDBFieldStringWithHotKey(DBFields.Reference);
			tsmiRef.Tag = DBFields.Reference;
		}

		#region Overridden and Event methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rc = ClientRectangle;
			e.Graphics.FillRectangle((TextBox.Enabled ? SystemBrushes.Window : SystemBrushes.Control), rc);

			rc.Height--;
			rc.Width--;
			ControlPaint.DrawVisualStyleBorder(e.Graphics, rc);

			base.OnPaint(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void tsmiItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem item = sender as ToolStripMenuItem;
			if (item == null)
				return;

			foreach (ToolStripMenuItem mi in tsddLabel.DropDownItems)
				mi.Checked = false;

			item.Checked = true;
			m_dbField = item.Tag as string;
			tsddLabel.Text = item.Text;

			RefreshFont();

			// If we got here from a user clicking on a drop-down item then broadcast a
			// message to whomever cares that a different transcription type was chosen.
			if (e != EventArgs.Empty)
			{
				object[] args = new object[2];
				args[0] = this;
				args[1] = item.Tag as string;
				PaApp.MsgMediator.SendMessage("TranscriptionTypeChanged", args);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void tsddLabel_Click(object sender, EventArgs e)
		{
			m_text.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void m_text_TextChanged(object sender, EventArgs e)
		{
			// Update cache with new text.
			//if (m_docId > 0 && m_dbField != null && m_updateCacheOnTextChange)
			//    DBUtils.DocCache[m_docId][m_dbField] = m_text.Text;

		}
		#endregion

		#region Misc. Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the contents of the text box from the document cache for the specified
		/// document id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshContent(int docId)
		{
			//m_updateCacheOnTextChange = false;

			//if (docId <= 0)
			//    m_text.Text = string.Empty;
			//else if (DBUtils.IsTranscritionField(m_dbField))
			//    m_text.Text = DBUtils.DocCache[docId].GetFullTranscription(m_dbField);
			//else
			//    m_text.Text = DBUtils.DocCache[docId][m_dbField] as string;

			//m_updateCacheOnTextChange = true;
			//m_docId = docId;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the text box's font appropriately.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshFont()
		{
			m_text.Font = FontHelper.GetDBFieldFont(m_dbField);
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		public bool AllowDropDown
		{
			get {return m_allowDropDown;}
			set
			{
				tslLabel.Visible = !value;
				tsddLabel.Visible = value;
				m_allowDropDown = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When this control is used for editing a field in a document, this gets or sets
		/// the id of the document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public int DocId
		{
			get {return m_docId;}
			set {m_docId = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the database field to which this control is loosely bound.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public string DBField
		{
			get {return m_dbField;}
			set
			{
				m_dbField = value;

				if (m_allowDropDown)
					tsddLabel.Text = ResourceHelper.GetDBFieldStringWithHotKey(value);
				else
					tslLabel.Text = ResourceHelper.GetDBFieldStringWithHotKey(value);

				if (m_docId > 0 && value != null)
					m_text.Text = DBUtils.DocCache[m_docId][value] as string;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		public string LabelText
		{
			get {return tslLabel.Text;}
			set	{tslLabel.Text = value;}
		}

		/// ----------------------------------------------------------------------------------
		/// <summary>
		/// Gets the textbox portion of the control.
		/// </summary>
		/// ----------------------------------------------------------------------------------
		[Browsable(false)]
		public TextBox TextBox
		{
			get {return m_text;}
		}

		#endregion
	}
}
