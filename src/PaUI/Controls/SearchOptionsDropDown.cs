using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Data;
using SIL.Pa.FFSearchEngine;
using SilUtils;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class SearchOptionsDropDown : UserControl
	{
		private SearchQuery m_query;
		private bool m_showApplyToAll;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchOptionsDropDown()
		{
			InitializeComponent();

			if (PaApp.DesignMode)
				return;

			lnkApplyToAll.Font = FontHelper.UIFont;
			lnkHelp.Font = FontHelper.UIFont;
			chkIgnoreDiacritics.Font = FontHelper.UIFont;
			chkShowAllWords.Font = FontHelper.UIFont;
			chkLength.Font = FontHelper.UIFont;
			chkTone.Font = FontHelper.UIFont;
			chkStress.Font = FontHelper.UIFont;
			lblUncertainties.Font = FontHelper.UIFont;
			rbPrimaryOnly.Font = FontHelper.UIFont;
			rbAllUncertainties.Font = FontHelper.UIFont;

			int fontsize = PaApp.SettingsHandler.GetIntSettingsValue(
				"searchoptionsdropdown", "pickerlabelfontsize", 0);

			if (fontsize > 0)
			{
				stressPicker.Font = FontHelper.MakeEticRegFontDerivative(fontsize);
				tonePicker.Font = FontHelper.MakeEticRegFontDerivative(fontsize);
				lengthPicker.Font = FontHelper.MakeEticRegFontDerivative(fontsize);
			}

			chkStress.Tag = stressPicker;
			chkTone.Tag = tonePicker;
			chkLength.Tag = lengthPicker;
			stressPicker.Tag = chkStress;
			tonePicker.Tag = chkTone;
			lengthPicker.Tag = chkLength;

			stressPicker.LoadCharacterType(IPACharIgnoreTypes.StressSyllable);
			tonePicker.LoadCharacterType(IPACharIgnoreTypes.Tone);
			lengthPicker.LoadCharacterType(IPACharIgnoreTypes.Length);
			LayoutDropDown();

			ShowApplyToAll = false;
			m_query = new SearchQuery();

			// Center the apply to all and help labels vertically between the bottom of the
			// drop-down and the bottom of the picker.
			lnkApplyToAll.Top = ClientSize.Height -
				((ClientSize.Height - grpUncertainties.Bottom) / 2) - (lnkApplyToAll.Height / 2);

			lnkHelp.Top = lnkApplyToAll.Top;
			lnkHelp.Left = ClientRectangle.Right - lnkHelp.Width - lnkApplyToAll.Left;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchOptionsDropDown(SearchQuery query) : this()
		{
			SearchQuery = query;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void LayoutDropDown()
		{
			Padding grpPadding = new Padding(7, 7, 7, 7);
			grpStress.Padding = grpTone.Padding = grpLength.Padding = grpPadding;

			// Difference between the height of the picker controls
			// and the group in which they are contained.
			int dyGrpPickerDiff = stressPicker.Top + grpPadding.Bottom;

			// Difference between the width of the picker controls
			// and the group in which they are contained.
			int dxGrpPickerDiff = grpPadding.Left + grpPadding.Right;

			int extraWidth = PaApp.SettingsHandler.GetIntSettingsValue(
				"searchoptionsdropdown", "extrapickerlabelwidth", 0);

			int extraHeight = PaApp.SettingsHandler.GetIntSettingsValue(
				"searchoptionsdropdown", "extrapickerlabelheight", 3);

			// Get the difference between a group's left edge and the
			// left edge of the check box associated with that group.
			int dxGrpChkDiff = chkTone.Left - grpTone.Left;

			stressPicker.ItemSize = new Size(stressPicker.PreferredItemHeight + extraWidth,
				stressPicker.PreferredItemHeight + extraHeight);

			tonePicker.ItemSize = new Size(tonePicker.PreferredItemHeight + extraWidth,
				tonePicker.PreferredItemHeight + extraHeight);

			lengthPicker.ItemSize = new Size(lengthPicker.PreferredItemHeight + extraWidth,
				lengthPicker.PreferredItemHeight + extraHeight);

			// Set widths of groups.
			int maxGrpsWidth = tonePicker.GetPreferredWidth(7) + dxGrpPickerDiff;
			if (grpUncertainties.Visible)
				maxGrpsWidth = Math.Max(maxGrpsWidth, grpUncertainties.Width);

			grpStress.Width = grpTone.Width = maxGrpsWidth;
			grpLength.Width = grpUncertainties.Width = maxGrpsWidth;

			// Add 24 which is the sum of the group's left and right padding,
			// plus a margin of 5 on either side of the group boxes.
			Width = maxGrpsWidth + 24;

			// Center the controls. (Subtract 1 to account for the border.
			int newLeft = ((Width - maxGrpsWidth) / 2) - 1;
			grpStress.Left = grpTone.Left = newLeft;
			grpLength.Left = grpUncertainties.Left = newLeft;

			// Align the checkboxes properly
			newLeft = grpTone.Left + dxGrpChkDiff;
			chkStress.Left = chkTone.Left = newLeft;
			chkLength.Left = chkIgnoreDiacritics.Left = newLeft;

			// Set heights of groups. For some reason, adding two
			// is necessary when using Vista in 120dpi.
			grpStress.Height = stressPicker.PreferredHeight + dyGrpPickerDiff + 2;
			grpTone.Height = tonePicker.PreferredHeight + dyGrpPickerDiff + 2;
			grpLength.Height = lengthPicker.PreferredHeight + dyGrpPickerDiff + 2;

			// Set tops of groups.
			grpTone.Top = grpStress.Bottom + 15;
			grpLength.Top = grpTone.Bottom + 15;
			grpUncertainties.Top = grpLength.Bottom + 15;
			chkTone.Top = grpTone.Top - 3;
			chkLength.Top = grpLength.Top - 3;
			
			Height = (grpUncertainties.Visible ? grpUncertainties.Bottom :
				grpLength.Bottom) + lnkHelp.Height + 15;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			// This is a kludge but I don't know any othe way to give the drop-down focus.
			if (Visible && !PaApp.DesignMode)
				SendKeys.Send("{DOWN}");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the apply to all link label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LinkLabel ApplyToAllLinkLabel
		{
			get { return lnkApplyToAll; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the apply to all link label should
		/// be shown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowApplyToAll
		{
			get { return m_showApplyToAll; }
			set
			{
				m_showApplyToAll = value;
				lnkApplyToAll.Visible = value;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the search options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SearchQuery SearchQuery
		{
			get
			{
				m_query.ShowAllOccurrences = chkShowAllWords.Checked;
				m_query.IgnoreDiacritics = chkIgnoreDiacritics.Checked;
				m_query.IncludeAllUncertainPossibilities = rbAllUncertainties.Checked;
				m_query.IgnoredStressChars = GetIgnoredChars(stressPicker);
				m_query.IgnoredToneChars = GetIgnoredChars(tonePicker);
				m_query.IgnoredLengthChars = GetIgnoredChars(lengthPicker);

				return m_query;
			}
			set
			{
				m_query = value.Clone();
				chkShowAllWords.Checked = m_query.ShowAllOccurrences;
				chkIgnoreDiacritics.Checked = m_query.IgnoreDiacritics;
				SetIgnoredChars(chkStress, stressPicker, m_query.IgnoredStressList);
				SetIgnoredChars(chkTone, tonePicker, m_query.IgnoredToneList);
				SetIgnoredChars(chkLength, lengthPicker, m_query.IgnoredLengthList);

				rbAllUncertainties.Checked = m_query.IncludeAllUncertainPossibilities;
				rbPrimaryOnly.Checked = !rbAllUncertainties.Checked;

				chkShowAllWords.Tag = chkShowAllWords.Checked;
				chkIgnoreDiacritics.Tag = chkIgnoreDiacritics.Checked;
				grpUncertainties.Tag = rbAllUncertainties.Checked;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the search options were changed since the
		/// last time the search query was set for the drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool OptionsChanged
		{
			get
			{
				if ((bool)chkShowAllWords.Tag != chkShowAllWords.Checked ||
					(bool)chkIgnoreDiacritics.Tag != chkIgnoreDiacritics.Checked ||
					(bool)grpUncertainties.Tag != rbAllUncertainties.Checked)
				{
					return true;
				}

				foreach (ToolStripButton item in stressPicker.Items)
				{
					if ((bool)item.Tag != item.Checked)
						return true;
				}

				foreach (ToolStripButton item in tonePicker.Items)
				{
					if ((bool)item.Tag != item.Checked)
						return true;
				}

				foreach (ToolStripButton item in lengthPicker.Items)
				{
					if ((bool)item.Tag != item.Checked)
						return true;
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a string containing all the characters of the checked buttons in the
		/// specified chooser.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetIgnoredChars(CharPicker picker)
		{
			StringBuilder ignoreList = new StringBuilder();
			foreach (ToolStripButton item in picker.Items)
			{
				if (item.Checked)
				{
					ignoreList.Append(item.Text.Replace(DataUtils.kDottedCircle, string.Empty));
					ignoreList.Append(',');
				}
			}

			return (ignoreList.ToString());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void SetIgnoredChars(CheckBox chk, CharPicker picker, List<string> ignoreList)
		{
			foreach (ToolStripButton item in picker.Items)
			{
				// Remove the dotted circle (if there is one) from the button's text, then
				// check the button's text to see if it's found in the ignore list.
				string chr = item.Text.Replace(DataUtils.kDottedCircle, string.Empty);
				item.Checked = (ignoreList != null && ignoreList.Contains(chr));
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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleIgnoreClick(object sender, EventArgs e)
		{
			CheckBox chk = sender as CheckBox;
			if (chk == null)
				return;

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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCharChecked(CharPicker picker, ToolStripButton item)
		{
			CheckBox chk = picker.Tag as CheckBox;
			if (chk == null)
				return;

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
		/// Handles the help link label being clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleHelpClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to close the drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Close()
		{
			if (Parent is ToolStripDropDown)
				((ToolStripDropDown)Parent).Close();
			else if (Parent != null)
			{
				try
				{
					Parent.GetType().InvokeMember("Close", BindingFlags.Instance |
						BindingFlags.InvokeMethod | BindingFlags.Public, null, Parent, null);
				}
				catch { }
			}
		}
	}
}
