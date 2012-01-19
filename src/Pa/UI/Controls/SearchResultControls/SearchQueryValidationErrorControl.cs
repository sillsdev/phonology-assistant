using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Localization;
using Palaso.Email;
using Palaso.Reporting;
using SIL.Pa.PhoneticSearching;
using SilTools;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace SIL.Pa.UI.Controls
{
	public partial class SearchQueryValidationErrorControl : UserControl, IWaterMarkHost
	{
		private bool _paintWaterMark;
		private readonly IList<SearchQueryValidationError> _errors;
		private readonly Dictionary<string, string> _helpTopicIds = new Dictionary<string, string>();

		/// ------------------------------------------------------------------------------------
		public SearchQueryValidationErrorControl()
		{
			InitializeComponent();

			DoubleBuffered = true;

			_labelHeading.Font = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold);
			_labelPattern.Font = App.PhoneticFont;
			_linkCopyToClipboard.Font = FontHelper.UIFont;
			_linkEmail.Font = FontHelper.UIFont;
			_labelPattern.Visible = false;

			_helpTopicIds["hidSearchPatternsOverview"] = "Search Patterns overview";
			_helpTopicIds["hidSearchPatternsExamples"] = "Examples of search pattern elements";
			_helpTopicIds["hidSearchPatternsZeroOrMore"] = "Zero or more phones or diacritics";
			_helpTopicIds["hidSearchPatternsOneOrMore"] = "One or more phones or diacritics";
			_helpTopicIds["hidSearchPatternsSpaceOrWrdBoundary"] = "Space or word boundary";
			_helpTopicIds["hidSearchPatternsOrGroups"] = "OR groups";
			_helpTopicIds["hidSearchPatternsDiacriticPlaceholders"] = "Diacritic placeholders";
			_helpTopicIds["hidSearchPatternsAndGroups"] = "AND groups";
			_helpTopicIds["hidSearchPatternsTroubleshooting"] = "Troubleshooting incorrect search results";
			_helpTopicIds["hidTroubleshootingUndefinedPhoneticCharacters"] = "Troubleshooting undefined phonetic characters";
		}

		/// ------------------------------------------------------------------------------------
		public SearchQueryValidationErrorControl(string pattern,
			IList<SearchQueryValidationError> errors, bool showPatternInHeading) : this()
		{
			_errors = errors;
			_labelPattern.Visible = showPatternInHeading;
			_labelPattern.Text = pattern;
			_labelHeading.Text = SearchQueryValidationError.GetHeadingTextForErrorList(null);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			var tableLayoutErrors = new TableLayoutPanel();
			tableLayoutErrors.AutoSize = true;
			tableLayoutErrors.Dock = DockStyle.Top;
			tableLayoutErrors.BackColor = _panelScrolling.BackColor;
			tableLayoutErrors.ColumnCount = 2;
			tableLayoutErrors.ColumnStyles.Add(new ColumnStyle());
			tableLayoutErrors.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutErrors.RowCount = _errors.Count;
			
			// Don't bother with the water mark right now. For it to work, the labels with all
			// the error messages have to have transparent backgrounds and that causes a lot of
			// extra painting when the control is resized. That looks ugly so I'm not going to
			// show the water mark for now. Perhaps when the errors are displayed in an HTML
			// control I'll put it back.
			//tableLayoutErrors.Paint += HandleWatermarkPaint;

			var numberFormat = LocalizationManager.GetString("PhoneticSearchingMessages.SearchPatternErrorNumberFormat", "{0})");

			int row = 0;
			for (int i = 0; i < _errors.Count; i++)
			{
				var lblNumber = GetLabel(string.Format(numberFormat, i + 1));
				var errMsgCtrl = (_errors[i].Exception == null ? GetLabel(_errors[i].Message) : GetExceptionLink(_errors[i]));


				//lblNumber.Margin = new Padding(0, 0, 0, lbl == null ? lblNumber.Margin.Bottom : 3);
				//errMsgCtrl.Margin = new Padding(0, 0, 0, lbl == null ? errMsgCtrl.Margin.Bottom : 3);

				tableLayoutErrors.Controls.Add(lblNumber, 0, row);
				tableLayoutErrors.Controls.Add(errMsgCtrl, 1, row++);
				tableLayoutErrors.RowStyles.Add(new RowStyle());

				var lbl = GetUnknownPhonesOrSymbolsLabel(
					_errors[i].GetUnknownPhonesDisplayText() ?? _errors[i].GetUnknownSymbolsDisplayText());
				
				if (lbl != null)
				{
					tableLayoutErrors.Controls.Add(lbl, 1, row++);
					tableLayoutErrors.RowStyles.Add(new RowStyle());
				}

				lbl = GetHelpLink(_errors[i]);
				
				if (lbl != null)
				{
					tableLayoutErrors.Controls.Add(lbl, 1, row++);
					tableLayoutErrors.RowStyles.Add(new RowStyle());
				}
			}

			_panelScrolling.Controls.Add(tableLayoutErrors);
		}

		/// ------------------------------------------------------------------------------------
		private Control GetUnknownPhonesOrSymbolsLabel(string text)
		{
			if (text == null)
				return null;

			var lbl = GetLabel(text);
			lbl.Font = App.PhoneticFont;
			return lbl;
		}

		/// ------------------------------------------------------------------------------------
		private Label GetLabel(string text)
		{
			return new Label
			{
				Text = text,
				TextAlign = ContentAlignment.TopLeft,
				AutoSize = true,
				Font = FontHelper.UIFont,
				BackColor = _panelScrolling.BackColor,
				Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
				Margin = new Padding(0, 0, 0, 3),
			};
		}

		/// ------------------------------------------------------------------------------------
		private Control GetHelpLink(SearchQueryValidationError error)
		{
			if (error.Exception != null)
				return null;

			var text = LocalizationManager.GetString("PhoneticSearchingMessages.ErrorsSuggestedHelpLinksMsg",
				"Suggested help topics to review: {0}");

			var topics = error.HelpLinks.Aggregate(string.Empty, (current, id) => current + (_helpTopicIds[id] + ", "));

			if (!error.HelpLinks.Contains("hidSearchPatternsTroubleshooting"))
				topics += _helpTopicIds["hidSearchPatternsTroubleshooting"];
			
			text = string.Format(text, topics.TrimEnd(',', ' '));

			var linkLabel = new LinkLabel
			{
				Text = text,
				TextAlign = ContentAlignment.TopLeft,
				AutoSize = true,
				Font = FontHelper.UIFont,
				BackColor = _panelScrolling.BackColor,
				Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
				Margin = new Padding(0, 4, 0, 12),
			};

			linkLabel.LinkClicked += HandleHelpLinkClicked;
			linkLabel.Links.Clear();

			foreach (var id in error.HelpLinks)
				linkLabel.Links.Add(text.IndexOf(_helpTopicIds[id], StringComparison.Ordinal), _helpTopicIds[id].Length, id);

			if (!error.HelpLinks.Contains("hidSearchPatternsTroubleshooting"))
			{
				linkLabel.Links.Add(text.IndexOf(_helpTopicIds["hidSearchPatternsTroubleshooting"], StringComparison.Ordinal),
					_helpTopicIds["hidSearchPatternsTroubleshooting"].Length, "hidSearchPatternsTroubleshooting");
			}
		
			return linkLabel;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleHelpLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			App.ShowHelpTopic(e.Link.LinkData as string);
		}

		/// ------------------------------------------------------------------------------------
		private Control GetExceptionLink(SearchQueryValidationError error)
		{
			var text = LocalizationManager.GetString("PhoneticSearchingMessages.ExceptionLinkText.FullMsg",
				"Unhandled Exception encountered. Click here for details.");

			var linkText = LocalizationManager.GetString("PhoneticSearchingMessages.ExceptionLinkText.LinkText",
				"Click here");

			var linkLabel = new LinkLabel
			{
				Text = text,
				TextAlign = ContentAlignment.TopLeft,
				AutoSize = true,
				Font = FontHelper.UIFont,
				BackColor = _panelScrolling.BackColor,
				Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
				Margin = new Padding(0, 0, 0, 12),
			};

			linkLabel.LinkClicked += HandleExceptionLinkClicked;
			linkLabel.Links.Clear();
			linkLabel.Links.Add(text.IndexOf(linkText, StringComparison.Ordinal), linkText.Length, error.Exception);
			return linkLabel;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleExceptionLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ErrorReport.ReportNonFatalException(e.Link.LinkData as Exception);
		}

		/// ------------------------------------------------------------------------------------
		public int GetPreferredHeight()
		{
			return _panelScrolling.Controls[0].Height + _panelScrolling.Padding.Top +
				_panelScrolling.Padding.Bottom + Padding.Top + Padding.Bottom +
				(_tableLayout.Height - _panelScrolling.Height);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var rc = ClientRectangle;
			rc.Height--;
			rc.Width--;

			var clr = (PaintingHelper.CanPaintVisualStyle() ? VisualStyleInformation.TextControlBorder : Color.Gray);

			using (var pen = new Pen(clr))
				e.Graphics.DrawRectangle(pen, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleOuterTableLayoutPaint(object sender, PaintEventArgs e)
		{
			var rc = _tableLayout.ClientRectangle;
			rc.Height = _panelScrolling.Top - 1;

			var clr1 = ColorHelper.CalculateColor(SystemColors.ActiveCaption, SystemColors.ActiveCaption, 0);
			var clr2 = ColorHelper.CalculateColor(Color.White, SystemColors.ActiveCaption, 70);

			using (var br = new LinearGradientBrush(rc, clr1, clr2, 0f))
			{
				e.Graphics.FillRectangle(br, rc);
				rc.Y = _panelScrolling.Bottom;
				rc.Height = _tableLayout.ClientRectangle.Bottom - rc.Y;
				e.Graphics.FillRectangle(br, rc);
			}

			using (var pen = new Pen(clr1))
			{
				var pt1 = new Point(0, _panelScrolling.Top - 1);
				var pt2 = new Point(rc.Right, _panelScrolling.Top - 1);
				e.Graphics.DrawLine(pen, pt1, pt2);

				pt1 = new Point(0, _panelScrolling.Bottom);
				pt2 = new Point(rc.Right, _panelScrolling.Bottom);
				e.Graphics.DrawLine(pen, pt1, pt2);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleSendEmailLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				var emailProvider = EmailProviderFactory.PreferredEmailProvider();
				var emailMessage = emailProvider.CreateMessage();
				emailMessage.To.Add(ErrorReport.EmailAddress);
				emailMessage.Subject = ErrorReport.EmailSubject;
				emailMessage.Body = SearchQueryValidationError.GetSingleStringErrorMsgFromListOfErrors(
					_labelPattern.Text, _errors);
				
				if (emailMessage.Send(emailProvider))
					return;
			}
			catch
			{
				//swallow it and go to the alternate method
			}

			try
			{
				var emailProvider = EmailProviderFactory.PreferredEmailProvider();
				var emailMessage = emailProvider.CreateMessage();
				emailMessage.To.Add(ErrorReport.EmailAddress);
				emailMessage.Subject = ErrorReport.EmailSubject;
				if (Environment.OSVersion.Platform == PlatformID.Unix)
				{
					emailMessage.Body = SearchQueryValidationError.GetSingleStringErrorMsgFromListOfErrors(
						_labelPattern.Text, _errors);
				}
				else
				{
					PutOnClipboard();
					emailMessage.Body = LocalizationManager.GetString("PhoneticSearchingMessages.SendEmailPasteFromClipboardPromptMsg",
						"<Details of search errror have been copied to the clipboard. Please paste them here>");
				}

				emailMessage.Send(emailProvider);
			}
			catch (Exception error)
			{
				PutOnClipboard();
				var text = LocalizationManager.GetString("PhoneticSearchingMessages.SendEmailFailureMsg",
					"This program was not able to get your email program, if you have one, to send the " +
					"error message. The contents of the error message has been placed on your Clipboard.");

				ErrorReport.NotifyUserOfProblem(error, text);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCopyToClipboardLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			PutOnClipboard();
		}

		/// ------------------------------------------------------------------------------------
		private void PutOnClipboard()
		{
			var text = SearchQueryValidationError.GetSingleStringErrorMsgFromListOfErrors(
				_labelPattern.Text, _errors);
			
			Clipboard.SetDataObject(text, true);
		}

		#region Watermark handling stuff
		/// ------------------------------------------------------------------------------------
		public bool AreResultsStale
		{
			get { return _paintWaterMark; }
			set
			{
				//if (_paintWaterMark != value)
				//{
				//    _paintWaterMark = value;
				//    _panelScrolling.Invalidate();
				//}
			}
		}

		///// ------------------------------------------------------------------------------------
		//private void HandleWatermarkPaint(object sender, PaintEventArgs e)
		//{
		//    if (!_paintWaterMark)
		//        return;

		//    var rc = ((Control)sender).ClientRectangle;
		//    rc.Width = (int)(rc.Width * 0.5f);
		//    rc.Height = (int)(rc.Height * 0.5f);
		//    rc.X = (((Control)sender).ClientRectangle.Width - rc.Width) / 2;
		//    rc.Y = (((Control)sender).ClientRectangle.Height - rc.Height) / 2;
			
		//    PaWordListGrid.DrawStaleResultsWaterMark(e.Graphics, rc, ForeColor);
		//}

		#endregion	
	}
}
