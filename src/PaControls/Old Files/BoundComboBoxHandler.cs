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
	public class BoundComboBoxHandler
	{
		// TODO: Do we need an event handler that raises an event when new items are added?
		// e.g. typing in a new value in the text portion of the combo would raise an event
		// to allow subscribers to add the value to the database.

		protected ComboBox m_cbo;
		protected string m_dbField;
		protected string m_valueOnEnter;
		protected bool m_updateCacheOnDataChange = true;
		protected int m_noneIndex;
		protected int m_docId = 0;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new handler for binding combo boxes to database fields in the
		/// document cache.
		/// </summary>
		/// <param name="dbField">Database field to bind to.</param>
		/// <param name="cbo">Combo Box to bind.</param>
		/// ------------------------------------------------------------------------------------
		public BoundComboBoxHandler(string dbField, ComboBox cbo)
		{
			m_dbField = dbField;
			cbo.CausesValidation = true;
			ComboBox = cbo;

			m_noneIndex = cbo.FindStringExact(DBUtils.kstidNoneText);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual ComboBox ComboBox
		{
			get {return m_cbo;}
			set
			{
				if (m_cbo != null)
				{
					m_cbo.SelectedIndexChanged -= new EventHandler(HandleSelectedIndexChanged);
					m_cbo.Validating -= new CancelEventHandler(HandleValidating);
					m_cbo.Enter -= new EventHandler(HandleEnter);
				}

				m_cbo = value;
				m_cbo.SelectedIndexChanged += new EventHandler(HandleSelectedIndexChanged);
				m_cbo.Validating += new CancelEventHandler(HandleValidating);
				m_cbo.Enter += new EventHandler(HandleEnter);
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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleSelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_cbo.SelectedIndex == m_noneIndex)
			{
				m_cbo.SelectedIndex = -1;
				m_cbo.Text = string.Empty;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the combo's value when gaining focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleEnter(object sender, EventArgs e)
		{
			m_valueOnEnter = m_cbo.Text;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the value changed, update the document cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleValidating(object sender, CancelEventArgs e)
		{
			string newValue = m_cbo.Text.Trim();
			if (newValue == m_valueOnEnter || !m_updateCacheOnDataChange)
				return;

			if (m_docId == 0)
				m_docId = PaApp.CurrentDocumentId;

			if (m_docId == 0)
				return;

			ProcessSelection(newValue);
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
			m_updateCacheOnDataChange = false;

			if (docId == 0 || DBUtils.DocCache[docId][m_dbField] == null ||
				DBUtils.DocCache[docId][m_dbField].ToString().Trim() == string.Empty)
			{
				m_cbo.SelectedIndex = m_noneIndex;
				HandleSelectedIndexChanged(null, null);
			}
			else
			{
				m_cbo.Text = DBUtils.DocCache[docId][m_dbField].ToString();
			}

			m_updateCacheOnDataChange = true;
			m_docId = docId;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the document cache with the selected value and checks to see if the value
		/// is new (via typing in the text portion of the combo. box). Is so, the new value
		/// is added to the drop-down list of items.
		/// </summary>
		/// <param name="newValue"></param>
		/// ------------------------------------------------------------------------------------
		protected virtual void ProcessSelection(string newValue)
		{
			// Update the cache.
			DBUtils.DocCache[m_docId][m_dbField] = (newValue == string.Empty ? null : newValue);

			if (newValue == string.Empty)
				return;

			// Get the data source -- assume it'ss an array list. Then check
			// if the new value is already in the list.
			ArrayList source = m_cbo.DataSource as ArrayList;
			if (source == null || source.Contains(newValue))
				return;

			// Add the new value to the list in the proper sorted location.
			for (int i = 0; i < source.Count; i++)
			{
				if (i != m_noneIndex && string.Compare(newValue, source[i].ToString()) < 0)
				{
					source.Insert(i, newValue);
					break;
				}
			}

			// Remove and reattach the data source to the combo box so the drop-down
			// list will now include the newly added item.
			m_cbo.DataSource = null;
			m_cbo.Items.Clear();
			m_cbo.DataSource = source;
			m_cbo.Text = newValue;
		}

		#endregion
	}
}
