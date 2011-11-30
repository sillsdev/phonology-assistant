using System.Drawing;
using System.Windows.Forms;
using SilTools;
using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
	public partial class SearchResultTabPopup : UserControl
	{
		private readonly SilPopup m_popup;

		/// ------------------------------------------------------------------------------------
		public SearchResultTabPopup()
		{
			InitializeComponent();
			lblName.Font = FontHelper.UIFont;
			lblNameValue.Font = App.PhoneticFont;
			lblPattern.Font = FontHelper.UIFont;
			lblPatternValue.Font = App.PhoneticFont;
			lblRecordCount.Font = FontHelper.UIFont;
			lblRecordsValue.Font = FontHelper.UIFont;

			m_popup = new SilPopup();
			m_popup.Padding = new Padding(0);
			m_popup.Margin = new Padding(0);
			m_popup.AutoSize = true;
			m_popup.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			m_popup.Controls.Add(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the popup with the specified information and near the mouse pointer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Show(SearchResultTab tab)
		{
			if (tab == null || tab.ResultView == null)
				return;

			if (tab.ResultView.SearchQuery.Errors.Count > 0)
			{
				lblRecordCount.Visible = false;
				lblRecordsValue.Text = App.GetString("SearchResultTabPopup.PatternContainsErrorsMsg",
					"Search pattern contains errors.");
			}
			else
			{
				lblRecordCount.Visible = true;
				lblPatternValue.Text = tab.ResultView.SearchQuery.Pattern;
				lblRecordsValue.Text = (tab.ResultView.Cache == null ?
					"0" : tab.ResultView.Cache.Count.ToString("#,###,##0"));
			}

			bool queryHasName = !string.IsNullOrEmpty(tab.ResultView.SearchQuery.Name);

			if (queryHasName && tab.TabTextClipped)
			{
				lblName.Visible = lblNameValue.Visible = true;
				lblNameValue.Text = tab.ResultView.SearchQuery.Name;
			}
			else
			{
				lblName.Visible = false;
				lblNameValue.Visible = false;
			}

			if (queryHasName || tab.TabTextClipped)
			{
				lblPattern.Visible = true;
				lblPatternValue.Visible = true;
			}
			else
			{
				lblPattern.Visible = false;
				lblPatternValue.Visible = false;
			}

			tlpInfo.PerformLayout();
			m_popup.Size = Size;
		
			Point pt = MousePosition;
			pt.X += SystemInformation.CursorSize.Width / 2;
			pt.Y += SystemInformation.CursorSize.Height / 3;
			m_popup.Show(pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Hides the popup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void HidePopup()
		{
			m_popup.Hide();
		}
	}
}
