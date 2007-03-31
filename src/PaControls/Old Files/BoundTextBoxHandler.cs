using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using SIL.SpeechTools.Database;
using SIL.Pa.Resources;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Handles binding a combo box to a database field in the document cache.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class BoundTextBoxHandler
	{
		// TODO: Do we need an event handler that raises an event when new items are added?
		// e.g. typing in a new value in the text portion of the combo would raise an event
		// to allow subscribers to add the value to the database.

		protected TextBox m_txt;
		protected string m_dbField;
		protected string m_valueOnEnter;
		protected bool m_updateCacheOnDataChange = true;
		protected int m_docId = 0;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new handler for binding combo boxes to database fields in the
		/// document cache.
		/// </summary>
		/// <param name="dbField">Database field to bind to.</param>
		/// <param name="cbo">Combo Box to bind.</param>
		/// ------------------------------------------------------------------------------------
		public BoundTextBoxHandler(string dbField, TextBox txt)
		{
			m_dbField = dbField;
			txt.CausesValidation = true;
			TextBox = txt;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual TextBox TextBox
		{
			get {return m_txt;}
			set
			{
				if (m_txt != null)
				{
					m_txt.Validating -= new CancelEventHandler(HandleValidating);
					m_txt.Enter -= new EventHandler(HandleEnter);
				}

				m_txt = value;
				m_txt.Validating += new CancelEventHandler(HandleValidating);
				m_txt.Enter += new EventHandler(HandleEnter);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DBField
		{
			get {return m_dbField;}
			set {m_dbField = value;}
		}

		#endregion

		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the combo's value when gaining focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleEnter(object sender, EventArgs e)
		{
			m_valueOnEnter = m_txt.Text;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the value changed, update the document cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleValidating(object sender, CancelEventArgs e)
		{
			string newValue = m_txt.Text.Trim();
			if (newValue == m_valueOnEnter || !m_updateCacheOnDataChange)
				return;

			if (m_docId == 0)
				m_docId = PaApp.CurrentDocumentId;

			// Update the cache.
			if (m_docId != 0)
				DBUtils.DocCache[m_docId][m_dbField] = (newValue == string.Empty ? null : newValue);
		}

		#endregion

		#region Misc. Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Refreshes the contents of the combo box based on the specified document id and
		/// the database field to which the combo box is bound.
		/// </summary>
		/// <param name="docId"></param>
		/// ------------------------------------------------------------------------------------
		public virtual void RefreshContent(int docId)
		{
			m_txt.Text = (docId == 0 || DBUtils.DocCache[docId][m_dbField] == null ?
				string.Empty : DBUtils.DocCache[docId][m_dbField].ToString().Trim());
			
			m_docId = docId;
		}

		#endregion
	}
}
