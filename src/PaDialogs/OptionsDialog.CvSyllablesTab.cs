using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Text;
using SIL.Pa;
using SIL.Pa.Resources;
using SIL.Pa.Controls;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
{
	public partial class OptionsDlg
	{
		private const int kTxtCustomHeightDiff = 22;
		private bool m_handleTextChange = true;
		private Dictionary<string, IPACharIgnoreTypes> m_allPickerPhones =
			new Dictionary<string, IPACharIgnoreTypes>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tab Initialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeCVSyllablesTab()
		{
			// This tab isn't valid if there is no project loaded.
			if (PaApp.Project == null)
			{
				tabOptions.TabPages.Remove(tpgCVSyllables);
				return;
			}

			// Assign the fonts
			chkLength.Font = FontHelper.UIFont;
			chkTone.Font = FontHelper.UIFont;
			chkStress.Font = FontHelper.UIFont;
			grpDisplayChars.Font = FontHelper.UIFont;
			lblInstruction.Font = FontHelper.UIFont;
			lblExampleDesc1.Font = FontHelper.UIFont;
			lblExampleDesc2.Font = FontHelper.UIFont;
			txtCustomChars.Font = new Font(FontHelper.PhoneticFont.Name, txtCustomChars.Font.Size);
			txtExampleInput.Font = new Font(FontHelper.PhoneticFont.Name, txtExampleInput.Font.Size);
			lblExampleCV.Font = new Font(FontHelper.PhoneticFont.Name, lblExampleCV.Font.Size);
			lblExampleCVCV.Font = new Font(FontHelper.PhoneticFont.Name, lblExampleCVCV.Font.Size);

			AdjustExampleControls();

			chkStress.Tag = stressPicker;
			chkTone.Tag = tonePicker;
			chkLength.Tag = lengthPicker;
			stressPicker.Tag = chkStress;
			tonePicker.Tag = chkTone;
			lengthPicker.Tag = chkLength;

			// Load the characters
			stressPicker.LoadCharacterType(IPACharIgnoreTypes.StressSyllable);
			tonePicker.LoadCharacterType(IPACharIgnoreTypes.Tone);
			lengthPicker.LoadCharacterType(IPACharIgnoreTypes.Length);
			LoadCustomType();

			SetDisplayedChars(chkStress, stressPicker);
			SetDisplayedChars(chkLength, lengthPicker);
			SetDisplayedChars(chkTone, tonePicker);

			// Assign event handlers
			chkStress.Click += new EventHandler(HandleGroupCheckChanged);
			chkLength.Click += new EventHandler(HandleGroupCheckChanged);
			chkTone.Click += new EventHandler(HandleGroupCheckChanged);

			stressPicker.CharPicked += new CharPicker.CharPickedHandler(HandleCharPicked);
			lengthPicker.CharPicked += new CharPicker.CharPickedHandler(HandleCharPicked);
			tonePicker.CharPicked += new CharPicker.CharPickedHandler(HandleCharPicked);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Perform misc. location and size adjustments to the controls in the right-hand
		/// group box on the tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustExampleControls()
		{
			// Determine what the height of the instructional text label should be.
			Size sz = new Size(lblInstruction.Width, int.MaxValue);
			lblInstruction.Height = TextRenderer.MeasureText(lblInstruction.Text,
				lblInstruction.Font, sz, TextFormatFlags.WordBreak).Height + 4;

			txtCustomChars.Top = lblInstruction.Bottom;
			lblExampleDesc1.Top = txtCustomChars.Bottom + 15;

			// Determine what the height of the text above the example text box should be.
			sz.Width = lblExampleDesc1.Width;
			lblExampleDesc1.Height = TextRenderer.MeasureText(lblExampleDesc1.Text,
				lblExampleDesc1.Font, sz, TextFormatFlags.WordBreak).Height + 4;

			txtExampleInput.Top = lblExampleDesc1.Bottom;
			lblExampleDesc2.Top = txtExampleInput.Bottom + 15;
			
			// Vertically locate the two CV pattern examples.
			lblExampleCVCV.Top = lblExampleDesc2.Bottom + 10;
			lblExampleCV.Top = lblExampleDesc2.Bottom + 10;
		
			// Center the two CV pattern examples.
			int left = (grpDisplayChars.Width - (lblExampleCVCV.Width + lblExampleCV.Width + 8)) / 2;
			lblExampleCVCV.Left = left;
			lblExampleCV.Left = lblExampleCVCV.Right + 8;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load custom type display characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadCustomType()
		{
			txtCustomChars.TextChanged += new System.EventHandler(txtCustomChars_TextChanged);
			foreach (CVPatternInfo info in PaApp.Project.CVPatternInfoList)
			{
				// Using 'NotApplicable' for custom type
				if (info.PatternType == IPACharIgnoreTypes.NotApplicable)
				{
					if (txtCustomChars.TextLength == 0)
						txtCustomChars.Text = info.Phone;
					else
						txtCustomChars.Text += (" " + info.Phone);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the selected and custom characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveCvSyllablesTabSettings()
		{
			if (!IsDirty)
				return;

			PaApp.Project.CVPatternInfoList.Clear();
			m_allPickerPhones.Clear();

			SaveDisplayLists(stressPicker, IPACharIgnoreTypes.StressSyllable);
			SaveDisplayLists(lengthPicker, IPACharIgnoreTypes.Length);
			SaveDisplayLists(tonePicker, IPACharIgnoreTypes.Tone);
			SaveCustomList();

			try
			{
				PaApp.Project.Save();
				PaApp.MsgMediator.SendMessage("CVSyllablesChanged", null);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the specified display list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveDisplayLists(CharPicker picker, IPACharIgnoreTypes cvPatternType)
		{
			string phone = string.Empty;
			foreach (ToolStripButton item in picker.Items)
			{
				phone = item.Text.Replace(DataUtils.kDottedCircle, string.Empty);
				m_allPickerPhones.Add(phone, cvPatternType);

				if (item.Checked)
					CreateCVPatternInfo(phone, cvPatternType);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the custom display list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveCustomList()
		{
			string phone = string.Empty;
			string[] split = txtCustomChars.Text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			ArrayList customList = new ArrayList();

			for (int i = 0; i < split.Length; i++)
			{
				// Remove duplicates
				if (!customList.Contains(split[i]))
					customList.Add(split[i]);
			}

			foreach (string cvString in customList)
			{
				phone = cvString.Replace(DataUtils.kDottedCircle, string.Empty);
				if (phone == string.Empty)
					continue;

				// If the custom phone already exists in pickers,
				// then save the phone with the correct cvPatternType.
				if (m_allPickerPhones.ContainsKey(phone))
					CreateCVPatternInfo(phone, m_allPickerPhones[phone]);
				else
					CreateCVPatternInfo(phone, IPACharIgnoreTypes.NotApplicable); // NA is 'custom' type
			}
		}

		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create a CVPatternInfo object and add it to the pattern list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CreateCVPatternInfo(string phone, IPACharIgnoreTypes cvPatternType)
		{
			CVPatternInfo cv = new CVPatternInfo();
			cv.Phone = phone;

			// We assume 'NotApplicable' means it's part of the Custom character list.
			cv.PatternType = cvPatternType;

			IPACharInfo charInfo = DataUtils.IPACharCache[phone];
			cv.IsBase = charInfo.IsBaseChar;
			PaApp.Project.CVPatternInfoList.Add(cv);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set/initialize the selected characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetDisplayedChars(CheckBox chk, CharPicker picker)
		{
			foreach (ToolStripButton item in picker.Items)
			{
				// Remove the dotted circle (if there is one) from the button's text, then
				// check the button if its text is found in the display list.
				string chr = item.Text.Replace(DataUtils.kDottedCircle, string.Empty);
				foreach (CVPatternInfo info in PaApp.Project.CVPatternInfoList)
				{
					// Don't check item's that are already custom types
					if (chr == info.Phone && info.PatternType != IPACharIgnoreTypes.NotApplicable)
					{
						item.Checked = true;
						break;
					}
				}
				item.Tag = item.Checked;
			}

			if (picker.CheckedItems == null)
				chk.CheckState = CheckState.Unchecked;
			else
			{
				chk.CheckState = (picker.Items.Count == picker.CheckedItems.Length ?
					CheckState.Checked : CheckState.Indeterminate);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the clicking of the picker display checkbox.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleGroupCheckChanged(object sender, EventArgs e)
		{
			CheckBox chk = sender as CheckBox;
			if (chk == null)
				return;

			m_dirty = true;

			if (chk.CheckState == CheckState.Indeterminate)
			{
				chk.CheckState = CheckState.Unchecked;
				chk.Checked = false;
			}

			CharPicker picker = chk.Tag as CharPicker;
			if (picker == null)
				return;

			foreach (ToolStripButton item in picker.Items)
				item.Checked = chk.Checked;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the clicking of the picker character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCharPicked(CharPicker picker, ToolStripButton item)
		{
			CheckBox chk = picker.Tag as CheckBox;
			if (chk == null)
				return;

			m_dirty = true;

			if (picker.CheckedItems == null)
				chk.CheckState = CheckState.Unchecked;
			else
			{
				chk.CheckState = (picker.CheckedItems.Length == picker.Items.Count ?
					CheckState.Checked : CheckState.Indeterminate);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the tbDisplayChars's text input.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtCustomChars_TextChanged(object sender, EventArgs e)
		{
			// Return if already handling a text change with this method
			if (!m_handleTextChange)
				return;
			m_handleTextChange = false;

			m_dirty = true;

			StringBuilder verifiedText = new StringBuilder();
			IPACharInfo charInfo;
			int textPosition = txtCustomChars.SelectionStart;

			if (txtCustomChars.TextLength > 0)
			{
				// Verify the text character by character
				for (int i = 0; i < txtCustomChars.TextLength; i++)
				{
					// Continue if it's the dotted circle
					if (txtCustomChars.Text[i].ToString() == DataUtils.kDottedCircle)
					{
						verifiedText.Append(DataUtils.kDottedCircle);
						continue;
					}

					charInfo = DataUtils.IPACharCache[txtCustomChars.Text[i]];

					// Eat the character if it is not in the IPACharCache
					if (charInfo == null)
						textPosition--;
					else
					{
						// Insert the dotted circle if the character is at the beginning and
						// the character should be displayed with the dotted circle
						if (i == 0 && charInfo.DisplayWDottedCircle)
						{
							verifiedText.Append(DataUtils.kDottedCircle);
							textPosition++;
						}

						if (i > 0)
						{
							// Insert the dotted circle if the prev character is a SPACE and
							// the character should be displayed with the dotted circle
							if (txtCustomChars.Text[i - 1] == ' ' && charInfo.DisplayWDottedCircle)
							{
								verifiedText.Append(DataUtils.kDottedCircle);
								textPosition++;
							}
						}

						verifiedText.Append(txtCustomChars.Text[i]);
					}
				}
				txtCustomChars.Text = verifiedText.ToString();
				// Set the cursor at the end of the text
				txtCustomChars.SelectionStart = textPosition;
			}

			m_handleTextChange = true;
		}
	}
}
