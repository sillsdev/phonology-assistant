using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;
using Localization;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public partial class CIEOptionsDropDown : SearchOptionsDropDown
	{
		private CIEOptions _cieOptions;
		private RadioButton _radioBoth;
		private RadioButton _radioAfter;
		private RadioButton _radioBefore;
		public LinkLabel _linkApply;
		public LinkLabel _linkCancel;

		/// ------------------------------------------------------------------------------------
		public CIEOptionsDropDown()
		{
			Canceled = false;
			InitializeComponent();
			_groupUncertainties.Visible = false;
		    lnkCancel.Visible = false;
		    lnkOk.Visible = false;
			SetupControls();
			CIEOptions = (App.Project != null ? App.Project.CIEOptions.Clone() : new CIEOptions());
		}

		/// ------------------------------------------------------------------------------------
		private void SetupControls()
		{
			_radioBoth = new RadioButton { AutoSize = true, UseVisualStyleBackColor = true, Font = FontHelper.UIFont };
			_radioBoth.TabIndex = 0;
			_radioBoth.Margin = new Padding(_chkIgnoreDiacritics.Margin.Left, 0, _chkIgnoreDiacritics.Margin.Right, 3);
			LocalizationManager.GetString("Views.WordLists.SearchResults.MinimalPairsOptionsPopup.BothEnvironmentRadioButton",
				"B&oth Environments Identical", null, _radioBoth);
			_tableLayout.SetColumnSpan(_radioBoth, 4);
			_tableLayout.Controls.Add(_radioBoth, 0, 0);

			_radioBefore = new RadioButton { AutoSize = true, UseVisualStyleBackColor = true, Font = FontHelper.UIFont };
			_radioBefore.TabIndex = 1;
			_radioBefore.Margin = new Padding(_chkIgnoreDiacritics.Margin.Left, 3, _chkIgnoreDiacritics.Margin.Right, 3);
			LocalizationManager.GetString("Views.WordLists.SearchResults.MinimalPairsOptionsPopup.PrecedingEnvironmentRadioButton",
				"Identical &Preceding Environment", null, _radioBefore);
			_tableLayout.SetColumnSpan(_radioBefore, 4);
			_tableLayout.Controls.Add(_radioBefore, 0, 1);

			_radioAfter = new RadioButton { AutoSize = true, UseVisualStyleBackColor = true, Font = FontHelper.UIFont };
			_radioAfter.TabIndex = 2;
			_radioAfter.Margin = new Padding(_chkIgnoreDiacritics.Margin.Left, 3, _chkIgnoreDiacritics.Margin.Right, 8);
			LocalizationManager.GetString("Views.WordLists.SearchResults.MinimalPairsOptionsPopup.FollowingEnvironmentRadioButton",
				"Identical &Following Environment", null, _radioAfter);
			_tableLayout.SetColumnSpan(_radioAfter, 4);
			_tableLayout.Controls.Add(_radioAfter, 0, 2);

			_linkApply = new LinkLabel { AutoSize = true, Font = FontHelper.UIFont };
			_linkApply.TabIndex = 10;
			_linkApply.LinkClicked += delegate { Close(); };
			_linkApply.Margin = new Padding(3, _linkHelp.Margin.Top, 3, _linkHelp.Margin.Bottom);
			LocalizationManager.GetString("Views.WordLists.SearchResults.MinimalPairsOptionsPopup.OKLink",
                "OK", null, _linkApply);
			_tableLayout.Controls.Add(_linkApply, 1, 10);

			_linkCancel = new LinkLabel { AutoSize = true, Font = FontHelper.UIFont };
			_linkCancel.TabIndex = 11;
			_linkCancel.LinkClicked += delegate { Canceled = true; Close(); };
			_linkCancel.Margin = new Padding(3, _linkHelp.Margin.Top, 3, _linkHelp.Margin.Bottom);
			LocalizationManager.GetString("Views.WordLists.SearchResults.MinimalPairsOptionsPopup.CancelLink",
				"Cancel", null, _linkCancel);
			_tableLayout.Controls.Add(_linkCancel, 2, 10);
		}

		/// ------------------------------------------------------------------------------------
		public CIEOptions CIEOptions
		{
			get
			{
				if (_radioAfter.Checked)
					_cieOptions.Type = CIEOptions.IdenticalType.After;
				else if (_radioBefore.Checked)
					_cieOptions.Type = CIEOptions.IdenticalType.Before;
				else
					_cieOptions.Type = CIEOptions.IdenticalType.Both;

				_cieOptions.SearchQuery = SearchQuery;
				return _cieOptions;
			}
			set
			{
				_cieOptions = value;
				Canceled = false;

				if (_cieOptions != null)
				{
					SearchQuery = _cieOptions.SearchQuery;
					_radioAfter.Checked = false;
					_radioBefore.Checked = false;
					_radioBoth.Checked = false;

					switch (_cieOptions.Type)
					{
						case CIEOptions.IdenticalType.After: _radioAfter.Checked = true; break;
						case CIEOptions.IdenticalType.Before: _radioBefore.Checked = true; break;
						case CIEOptions.IdenticalType.Both: _radioBoth.Checked = true; break;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool OptionsChanged
		{
			get
			{
				bool changed = false;

				switch (_cieOptions.Type)
				{
					case CIEOptions.IdenticalType.After: changed = !_radioAfter.Checked; break;
					case CIEOptions.IdenticalType.Before: changed = !_radioBefore.Checked; break;
					case CIEOptions.IdenticalType.Both: changed = !_radioBoth.Checked; break;
				}

				return (!Canceled && (changed || base.OptionsChanged));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the cancel link was clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Canceled { get; set; }

		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Canceled = true;
			base.HandleHelpClicked(sender, e);
		    string _helpTopic = string.Empty;
		    if(CIEBuilder.IsMinimalpair)
		        _helpTopic = "hidMinimalPairsOptions";
		    else
		        _helpTopic = "hidSimilarEnvironmentsOptions";
			App.ShowHelpTopic(_helpTopic);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Treat the escape key like clicking cancel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape)
				Canceled = true;

			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
