// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public partial class SearchOptionsDropDown : UserControl
	{
		private SearchQuery m_query;
		private bool m_showApplyToAll;

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets a value indicating whether or not the cancel link was clicked.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public bool Canceled { get; set; }

		/// ------------------------------------------------------------------------------------
		public SearchOptionsDropDown()
		{
			InitializeComponent();
            Canceled = false;
			if (App.DesignMode)
				return;

			_linkApplyToAll.Font = FontHelper.UIFont;
			_linkHelp.Font = FontHelper.UIFont;
			_chkIgnoreDiacritics.Font = FontHelper.UIFont;
			_chkShowAllWords.Font = FontHelper.UIFont;
			_chkLength.Font = FontHelper.UIFont;
			_chkTone.Font = FontHelper.UIFont;
			_chkStress.Font = FontHelper.UIFont;
			_chkBoundary.Font = FontHelper.UIFont;
            _chkPitchPhonation.Font = FontHelper.UIFont;
			_groupUncertainties.Font = FontHelper.UIFont;
			rbPrimaryOnly.Font = FontHelper.UIFont;
			rbAllUncertainties.Font = FontHelper.UIFont;
            lnkOk.LinkClicked += delegate { Close(); };

			int fontsize = Properties.Settings.Default.SearchOptionsDropDownPickerLabelFontSize;
			if (fontsize > 0)
			{
				var fnt = FontHelper.MakeRegularFontDerivative(App.PhoneticFont, fontsize);
				_pickerStress.Font = fnt;
				_pickerLength.Font = fnt;
				_pickerBoundary.Font = fnt;
				_pickerTone.Font = fnt;
			    _panelPitchPhonation.Font = fnt;
			}

			_chkStress.Tag = _pickerStress;
			_chkTone.Tag = _pickerTone;
			_chkLength.Tag = _pickerLength;
			_chkBoundary.Tag = _pickerBoundary;
            _chkPitchPhonation.Tag = _pickerPitchPhonation;
			_pickerStress.Tag = _chkStress;
			_pickerTone.Tag = _chkTone;
			_pickerLength.Tag = _chkLength;
			_pickerBoundary.Tag = _chkBoundary;
		    _pickerPitchPhonation.Tag = _chkPitchPhonation;

			_pickerStress.LoadCharacterType(IPASymbolSubType.stress);
			_pickerTone.LoadCharacterType(IPASymbolSubType.tone);
			_pickerBoundary.LoadCharacterType(IPASymbolSubType.boundary);
			_pickerLength.LoadCharacterType(IPASymbolSubType.length);
            _pickerPitchPhonation.LoadCharacterType(IPASymbolSubType.pitchphonation);

			ShowApplyToAll = false;
			m_query = new SearchQuery();
		}

		/// ------------------------------------------------------------------------------------
		public SearchOptionsDropDown(SearchQuery query) : this()
		{
			SearchQuery = query;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			_groupTone.Width = _pickerTone.GetPreferredWidth(15) + _groupTone.Padding.Left + _groupTone.Padding.Right;
			_groupBoundary.Width = _groupTone.Width;
			_groupLength.Width = _groupTone.Width;
			_groupStress.Width = _groupTone.Width;
			_groupUncertainties.Width = _groupTone.Width;
            _groupPitchPhonation.Width = _groupTone.Width;

			_groupStress.Height = _pickerStress.GetPreferredHeight() + _pickerStress.Top + _groupStress.Padding.Bottom;
			_groupLength.Height = _pickerLength.GetPreferredHeight() + _pickerLength.Top + _groupLength.Padding.Bottom;
			_groupBoundary.Height = _pickerBoundary.GetPreferredHeight() + _pickerBoundary.Top + _groupBoundary.Padding.Bottom;
			_groupTone.Height = _pickerTone.GetPreferredHeight() + _pickerTone.Top + _groupTone.Padding.Bottom;
            _groupPitchPhonation.Height = _pickerPitchPhonation.GetPreferredHeight() + _pickerPitchPhonation.Top + _groupPitchPhonation.Padding.Bottom;

			_panelStress.AutoSize = true;
			_panelLength.AutoSize = true;
			_panelTone.AutoSize = true;
			_panelBoundary.AutoSize = true;
            _panelPitchPhonation.AutoSize = true;
			_tableLayout.AutoSize = true;

			// Add 2 to both dimensions because most of the time this is hosted on a drop-down
			// and when that's the case, the drop-down adds a single-line border all around.
			Size = new Size(_tableLayout.Width + (_tableLayout.Left * 2),
				_tableLayout.Height + (_tableLayout.Top * 2));
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			// This is a kludge but I don't know any othe way to give the drop-down focus.
			if (Visible && !App.DesignMode)
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
			get { return _linkApplyToAll; }
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
				_linkApplyToAll.Visible = value;
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
				m_query.ShowAllOccurrences = _chkShowAllWords.Checked;
				m_query.IgnoreDiacritics = _chkIgnoreDiacritics.Checked;
				m_query.IncludeAllUncertainPossibilities = rbAllUncertainties.Checked;
				m_query.IgnoredCharacters = GetAllIgnoredCharacters();
				return m_query;
			}
			set
			{
                Canceled = false;
				m_query = value.Clone();
				_chkShowAllWords.Checked = m_query.ShowAllOccurrences;
				_chkIgnoreDiacritics.Checked = m_query.IgnoreDiacritics;

				var querysIgnoredCharacters = m_query.GetIgnoredCharacters().ToList();
				SetIgnoredChars(_chkStress, _pickerStress, querysIgnoredCharacters);
				SetIgnoredChars(_chkTone, _pickerTone, querysIgnoredCharacters);
				SetIgnoredChars(_chkLength, _pickerLength, querysIgnoredCharacters);
				SetIgnoredChars(_chkBoundary, _pickerBoundary, querysIgnoredCharacters);
                SetIgnoredChars(_chkPitchPhonation, _pickerPitchPhonation, querysIgnoredCharacters);

				rbAllUncertainties.Checked = m_query.IncludeAllUncertainPossibilities;
				rbPrimaryOnly.Checked = !rbAllUncertainties.Checked;

				_chkShowAllWords.Tag = _chkShowAllWords.Checked;
				_chkIgnoreDiacritics.Tag = _chkIgnoreDiacritics.Checked;
				_groupUncertainties.Tag = rbAllUncertainties.Checked;
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
                if (Canceled)
                    return false;

				if ((bool)_chkShowAllWords.Tag != _chkShowAllWords.Checked ||
					(bool)_chkIgnoreDiacritics.Tag != _chkIgnoreDiacritics.Checked ||
					(bool)_groupUncertainties.Tag != rbAllUncertainties.Checked)
				{
					return true;
				}

				if (_pickerStress.Items.Cast<ToolStripButton>().Any(item => (bool)item.Tag != item.Checked))
					return true;

				if (_pickerTone.Items.Cast<ToolStripButton>().Any(item => (bool)item.Tag != item.Checked))
					return true;

				if (_pickerBoundary.Items.Cast<ToolStripButton>().Any(item => (bool)item.Tag != item.Checked))
					return true;

                if (_pickerPitchPhonation.Items.Cast<ToolStripButton>().Any(item => (bool)item.Tag != item.Checked))
                    return true;

				return _pickerLength.Items.Cast<ToolStripButton>().Any(item => (bool) item.Tag != item.Checked);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a string containing all the characters of the checked buttons in all the
		/// choosers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetAllIgnoredCharacters()
		{
			return GetIgnoredChars(_pickerStress) +
				GetIgnoredChars(_pickerLength) +
				GetIgnoredChars(_pickerBoundary) +
                GetIgnoredChars(_pickerPitchPhonation) +
				GetIgnoredChars(_pickerTone).TrimEnd(',');
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a string containing all the characters of the checked buttons in the
		/// specified chooser.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetIgnoredChars(CharPicker picker)
		{
			var ignoreList = new StringBuilder();
			foreach (var item in picker.Items.Cast<ToolStripButton>().Where(item => item.Checked))
				ignoreList.AppendFormat("{0},", item.Text.Replace(App.DottedCircle, string.Empty));

			return (ignoreList.Length == 0 ? string.Empty : ignoreList.ToString());
		}


		/// ------------------------------------------------------------------------------------
		private static void SetIgnoredChars(CheckBox chk, CharPicker picker, IEnumerable<string> ignoredCharacters)
		{
			var ignoreList = ignoredCharacters.ToList();

			foreach (var item in picker.GetItems())
			{
				// Remove the dotted circle (if there is one) from the button's text, then
				// check the button's text to see if it's found in the ignore list.
				var chr = item.Text.Replace(App.DottedCircle, string.Empty);
				item.Checked = (ignoreList.Contains(chr));
				item.Tag = item.Checked;
			}

			chk.CheckState = picker.GetRelevantCheckState();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleIgnoreClick(object sender, EventArgs e)
		{
			var chk = sender as CheckBox;
			if (chk == null)
				return;

			if (chk.CheckState == CheckState.Indeterminate)
			{
				chk.CheckState = CheckState.Unchecked;
				chk.Checked = false;
			}

			var picker = chk.Tag as CharPicker;
			if (picker == null)
				return;

			foreach (ToolStripButton item in picker.Items)
				item.Checked = chk.Checked;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCharChecked(CharPicker picker, ToolStripButton item)
		{
			var chk = picker.Tag as CheckBox;
			if (chk != null)
				chk.CheckState = picker.GetRelevantCheckState();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleHelpClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
            Canceled = true;
            Close();
            App.ShowHelpTopic("hidSearchOptionsOnFilters");
		}

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

        /// ------------------------------------------------------------------------------------
        protected virtual void HandleCloseClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Canceled = true;
            Close();
        }
	}
}
