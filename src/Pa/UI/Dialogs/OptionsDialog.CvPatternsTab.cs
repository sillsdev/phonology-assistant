using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilUtils;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.UI.Dialogs
{
	public partial class OptionsDlg
	{
		private bool m_handleTextChange = true;
		private readonly Dictionary<string, IPASymbolIgnoreType> m_allPickerPhones =
			new Dictionary<string, IPASymbolIgnoreType>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tab Initialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeCVPatternsTab()
		{
			// This tab isn't valid if there is no project loaded.
			if (App.Project == null)
			{
				tabOptions.TabPages.Remove(tpgCVPatterns);
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
			txtCustomChars.Font = FontHelper.MakeEticRegFontDerivative(txtCustomChars.Font.Size);
			txtExampleInput.Font = FontHelper.MakeEticRegFontDerivative(txtExampleInput.Font.Size);
			lblExampleCV.Font = FontHelper.MakeEticRegFontDerivative(lblExampleCV.Font.Size);
			lblExampleCVCV.Font = FontHelper.MakeEticRegFontDerivative(lblExampleCVCV.Font.Size);

			AdjustExampleControls();

			chkStress.Tag = stressPicker;
			chkTone.Tag = tonePicker;
			chkLength.Tag = lengthPicker;
			stressPicker.Tag = chkStress;
			tonePicker.Tag = chkTone;
			lengthPicker.Tag = chkLength;

			// Load the characters
			stressPicker.LoadCharacterType(IPASymbolIgnoreType.StressSyllable);
			tonePicker.LoadCharacterType(IPASymbolIgnoreType.Tone);
			lengthPicker.LoadCharacterType(IPASymbolIgnoreType.Length);
			LoadCustomType();

			SetDisplayedChars(chkStress, stressPicker);
			SetDisplayedChars(chkLength, lengthPicker);
			SetDisplayedChars(chkTone, tonePicker);

			AdjustPickerSizes();

			// Assign event handlers
			chkStress.Click += HandleGroupCheckChanged;
			chkLength.Click += HandleGroupCheckChanged;
			chkTone.Click += HandleGroupCheckChanged;

			stressPicker.CharPicked += HandleCharPicked;
			lengthPicker.CharPicked += HandleCharPicked;
			tonePicker.CharPicked += HandleCharPicked;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustPickerSizes()
		{
			stressPicker.Location = lengthPicker.Location = tonePicker.Location = new Point(0, 0);
			
			stressPicker.Width = stressPicker.GetPreferredWidth(7);
			stressPicker.Height = stressPicker.PreferredHeight;

			lengthPicker.Width = lengthPicker.GetPreferredWidth(7);
			lengthPicker.Height = lengthPicker.PreferredHeight;

			tonePicker.Width = tonePicker.GetPreferredWidth(7);
			tonePicker.Height = tonePicker.PreferredHeight;

			grpStress.Width = stressPicker.Width + grpStress.Padding.Left +
				grpStress.Padding.Right + SystemInformation.VerticalScrollBarWidth + 5;

			grpLength.Width = lengthPicker.Width + grpLength.Padding.Left +
				grpLength.Padding.Right + SystemInformation.VerticalScrollBarWidth + 5;

			grpTone.Width = tonePicker.Width + grpTone.Padding.Left +
				grpTone.Padding.Right + SystemInformation.VerticalScrollBarWidth + 5;
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
			txtCustomChars.TextChanged += txtCustomChars_TextChanged;
			
			foreach (CVPatternInfo info in App.Project.CVPatternInfoList)
			{
				// Using 'NotApplicable' for custom type
				if (info.PatternType == IPASymbolIgnoreType.NotApplicable)
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
		private void SaveCvPatternsTabSettings()
		{
			txtCustomChars.TextChanged -= txtCustomChars_TextChanged;

			if (!IsDirty)
				return;

			App.Project.CVPatternInfoList.Clear();
			m_allPickerPhones.Clear();

			SaveDisplayLists(stressPicker, IPASymbolIgnoreType.StressSyllable);
			SaveDisplayLists(lengthPicker, IPASymbolIgnoreType.Length);
			SaveDisplayLists(tonePicker, IPASymbolIgnoreType.Tone);
			SaveCustomList();

			try
			{
				App.Project.Save();
				App.MsgMediator.SendMessage("CVPatternsChanged", null);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the specified display list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveDisplayLists(CharPicker picker, IPASymbolIgnoreType cvPatternType)
		{
			string phone = string.Empty;
			foreach (ToolStripButton item in picker.Items)
			{
				phone = item.Text.Replace(App.kDottedCircle, string.Empty);
				m_allPickerPhones.Add(phone, cvPatternType);

				if (item.Checked)
				{
					CVPatternInfo cvpi = CVPatternInfo.Create(phone, cvPatternType);
					if (cvpi != null)
						App.Project.CVPatternInfoList.Add(cvpi);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the custom display list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveCustomList()
		{
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
				string chr = cvString.Replace(App.kDottedCircle, string.Empty);
				if (chr == string.Empty)
					continue;

				// If the custom phone already exists in pickers, then save the phone with
				// the correct IPACharIgnoreTypes. Otherwise, save the phone with the N/A
				// type which we're using here for custom characters.
				CVPatternInfo cvpi = CVPatternInfo.Create(cvString,
					(m_allPickerPhones.ContainsKey(chr) ? m_allPickerPhones[chr] :
					IPASymbolIgnoreType.NotApplicable));

				if (cvpi != null)
					App.Project.CVPatternInfoList.Add(cvpi);
			}
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
				string chr = item.Text.Replace(App.kDottedCircle, string.Empty);
				foreach (CVPatternInfo info in App.Project.CVPatternInfoList)
				{
					// Don't check item's that are already custom types
					if (chr == info.Phone && info.PatternType != IPASymbolIgnoreType.NotApplicable)
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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtCustomChars_KeyDown(object sender, KeyEventArgs e)
		{
			// Ctrl-0 inserts a dotted circle.
			if (e.Control && e.KeyCode == Keys.D0)
			{
				int selStart = txtCustomChars.SelectionStart;
				if (txtCustomChars.SelectionLength > 0)
				{
					// Remove any selected text.
					m_handleTextChange = false;
					txtCustomChars.Text =
						txtCustomChars.Text.Remove(selStart, txtCustomChars.SelectionLength);
					m_handleTextChange = true;
				}

				txtCustomChars.Text = txtCustomChars.Text.Insert(selStart, App.kDottedCircle);
				txtCustomChars.SelectionStart = selStart + 1;
				e.SuppressKeyPress = true;
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
			IPASymbol charInfo;
			int selStart = txtCustomChars.SelectionStart;

			if (txtCustomChars.TextLength > 0)
			{
				// Verify the text character by character
				for (int i = 0; i < txtCustomChars.TextLength; i++)
				{
					// Continue if it's the dotted circle
					if (txtCustomChars.Text[i].ToString() == App.kDottedCircle)
					{
						verifiedText.Append(App.kDottedCircle);
						continue;
					}

					charInfo = App.IPASymbolCache[txtCustomChars.Text[i]];

					// Eat the character if it is not in the IPACharCache
					if (charInfo == null)
						selStart--;
					else
					{
						// Insert the dotted circle if the character is at the beginning or after
						// a space and the character should be displayed with the dotted circle.
						if (charInfo.DisplayWithDottedCircle &&
							((i == 0 || txtCustomChars.Text[i - 1] == ' ')) &&
							(i + 1 < txtCustomChars.Text.Length && txtCustomChars.Text[i + 1] != App.kDottedCircleC))
						{
							verifiedText.Append(App.kDottedCircle);
							selStart++;
						}

						verifiedText.Append(txtCustomChars.Text[i]);
					}
				}

				txtCustomChars.Text = verifiedText.ToString();
				// Set the cursor at the end of the text
				txtCustomChars.SelectionStart = selStart;
			}

			m_handleTextChange = true;
		}
	}
}
