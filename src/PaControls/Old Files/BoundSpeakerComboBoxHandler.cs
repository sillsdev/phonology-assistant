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
	/// Handles binding a speaker combo box to a database field in the document cache.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class BoundSpeakerComboBoxHandler : BoundComboBoxHandler
	{
		private RadioButton m_rbMale;
		private RadioButton m_rbFemale;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new handler for binding combo boxes to database fields in the
		/// document cache.
		/// </summary>
		/// <param name="dbField">Database field to bind to.</param>
		/// <param name="cbo">Combo Box to bind.</param>
		/// ------------------------------------------------------------------------------------
		public BoundSpeakerComboBoxHandler(ComboBox cbo, RadioButton rbMale, RadioButton rbFemale) :
			base(DBFields.SpeakerName, cbo)
		{
			m_rbMale = rbMale;
			m_rbMale.Tag = Gender.Male;
			m_rbFemale = rbFemale;
			m_rbFemale.Tag = Gender.Female;

			m_rbMale.CheckedChanged += new EventHandler(HandleGenderCheckedChanged);
			m_rbFemale.CheckedChanged += new EventHandler(HandleGenderCheckedChanged);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override ComboBox ComboBox
		{
			get {return base.ComboBox;}
			set
			{
				if (m_cbo != null)
					m_cbo.TextChanged -= new EventHandler(HandleTextChanged);

				base.ComboBox = value;
				value.TextChanged += new EventHandler(HandleTextChanged);
			}
		}

		#endregion

		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleSelectedIndexChanged(object sender, EventArgs e)
		{
			base.HandleSelectedIndexChanged(sender, e);
			RefreshGender();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This event is only fired when the combo box is for a speaker. Make sure the
		/// gender radio buttons are enabled if there is something in the combo's text portion
		/// and also set the gender radio buttons appropriately.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleTextChanged(object sender, EventArgs e)
		{
			// Enable the gender radio buttons when the text is anything but blank and "none".
			m_rbMale.Enabled = m_rbFemale.Enabled = (m_cbo.Text.Trim() != string.Empty &&
				m_cbo.Text != DBUtils.kstidNoneText);

			RefreshGender();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the gender changing
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleGenderCheckedChanged(object sender, EventArgs e)
		{
			RadioButton rb = sender as RadioButton;
			if (!m_updateCacheOnDataChange || rb == null || !rb.Checked)
				return;

			SpeakerInfo spkrInfo = m_cbo.SelectedItem as SpeakerInfo;
			if (spkrInfo == null)
				return;

			// Don't update the database if the gender didn't change.
			if (spkrInfo.Gender == (Gender)rb.Tag)
				return;

			// Update the database with a new gender for the current speaker.
			if (DBUtils.UpdateSpeakersGender(spkrInfo.Name, spkrInfo.Gender, (Gender)rb.Tag) > 0)
				spkrInfo.Gender = (Gender)rb.Tag;
		}

		#endregion

		#region Misc. Overridden Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Refreshes the contents of the combo box based on the specified document id and
		/// the database field to which the combo box is bound.
		/// </summary>
		/// <param name="docId"></param>
		/// ------------------------------------------------------------------------------------
		public override void RefreshContent(int docId)
		{
			base.RefreshContent(docId);

			if (docId > 0 && DBUtils.DocCache[docId][m_dbField] != null &&
				DBUtils.DocCache[docId][m_dbField].ToString().Trim() != string.Empty)
			{
				RefreshGender();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the document cache with the selected value and checks to see if the speaker
		/// is new (via typing in the text portion of the combo. box). If so, the new speaker
		/// is added to the database and the combo's data source is updated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void ProcessSelection(string newValue)
		{
			SpeakerInfo spkrInfo;
			int id = 0;

			// Use FindStringExact instead of checking the selected index because I found that
			// when starting to type a new name, the selected index didn't change to -1 until
			// after the second phone was typed. Argh!
			int i = m_cbo.FindStringExact(newValue);
			if (i > -1)
			{
				spkrInfo = m_cbo.Items[i] as SpeakerInfo;
				id = (spkrInfo == null ? 0 : spkrInfo.Id);
			}
			else
			{
				Gender newGender = Gender.Unspecified;
				if (m_rbMale.Checked)
					newGender = Gender.Male;
				else if (m_rbFemale.Checked)
					newGender = Gender.Female;

				// Check if the speaker with the specified gender exists.
				id = DBUtils.GetSpeakerId(newValue, newGender, false);
				if (id == 0)
				{
					// Add the new speaker, making male the default gender if it wasn't specified.
					if (newGender == Gender.Unspecified)
						newGender = Gender.Male;

					id = DBUtils.GetSpeakerId(newValue, newGender, true);
				}

				if (id > 0)
				{
					// Now that the new name has been added, refresh the drop-down list.
					m_cbo.DataSource = null;
					m_cbo.Items.Clear();
					m_cbo.DataSource = DBUtils.CachedSpeakerList;
					m_cbo.Text = newValue;
					RefreshGender();
				}
			}

			DBUtils.DocCache[m_docId][DBFields.SpeakerId] = id;
			DBUtils.DocCache[m_docId][DBFields.SpeakerName] = (id > 0 ? newValue : null);
		}

		#endregion

		#region Gender related methods.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the value and enabled state of the gender radio buttons based on the current
		/// speaker's name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RefreshGender()
		{
			Gender gender = GetSelectedItemsGender(m_cbo.Text.Trim());

			m_updateCacheOnDataChange = false;
			m_rbMale.Checked = ((gender == Gender.Male || gender == Gender.Unspecified) && m_rbMale.Enabled);
			m_rbFemale.Checked = (gender == Gender.Female && m_rbFemale.Enabled);
			m_updateCacheOnDataChange = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the gender from the list items for the specified name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Gender GetSelectedItemsGender(string name)
		{
			// Use FindStringExact instead of checking the selected index because I found that
			// when starting to type a new name, the selected index didn't change to -1 until
			// after the second phone was typed. Argh!
			int i = m_cbo.FindStringExact(name);
			if (i == -1)
				return Gender.Unspecified;
			
			SpeakerInfo spkrInfo = m_cbo.Items[i] as SpeakerInfo;
			return (spkrInfo == null ? Gender.Unspecified : spkrInfo.Gender);
		}

		#endregion
	}
}
