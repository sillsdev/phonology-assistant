using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.FFSearchEngine;
using SIL.SpeechTools.Utils;
using SIL.Pa.Data;

namespace SIL.Pa.Controls
{
	public partial class ChartOptionsDropDown : UserControl
	{
		private const int kItemSize = 36;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ChartOptionsDropDown(string ignoreList)
		{
			InitializeComponent();
			lblSSegsToIgnore.Font = FontHelper.UIFont;
			lnkRefresh.Font = FontHelper.UIFont;
			lnkHelp.Font = FontHelper.UIFont;

			pickerIgnore.ItemSize = new Size(kItemSize, kItemSize);
			pickerIgnore.Font =
				new Font(FontHelper.PhoneticFont.FontFamily, 16, GraphicsUnit.Point);
			pickerIgnore.ShouldLoadChar += new CharPicker.ShouldLoadCharHandler(pickerIgnore_ShouldLoadChar);
			pickerIgnore.LoadCharacters();
			SetIgnoredChars(ignoreList);

			Padding itemMargin = (pickerIgnore.Items.Count == 0 ?
				new Padding(0) : pickerIgnore.Items[0].Margin);

			int itemWidth = kItemSize + itemMargin.Left + itemMargin.Right;
			int itemHeight = kItemSize + itemMargin.Top + itemMargin.Bottom;

			// Adjust the size of the drop-down to fit nicely.
			int charCount = pickerIgnore.Items.Count;
			int colsPerRow = (int)Math.Ceiling((float)charCount / 3f);
			int propsedWidth = (itemWidth * colsPerRow) + 23;

			if (Width < propsedWidth)
			{
				Width = propsedWidth;
				Height = (itemHeight * 3) + 72;
			}
			else
			{
				colsPerRow = (int)Math.Ceiling((float)(Width - 23) / (float)itemWidth);
				Width = (itemWidth * colsPerRow) + 23;
				int rows = (int)Math.Ceiling((float)charCount / (float)colsPerRow);
				Height = (itemHeight * rows) + 72;
			}

			// Center the refresh and help labels vertically between the bottom of the
			// drop-down and the bottom of the picker.
			lnkRefresh.Top = ClientSize.Height -
				((ClientSize.Height - pickerIgnore.Bottom) / 2) - (lnkRefresh.Height / 2);

			lnkHelp.Top = lnkRefresh.Top;
			lnkHelp.Left = ClientRectangle.Right - lnkHelp.Width - 10;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a value indicating whether or not the character in the specified
		/// IPACharInfo object should be in the ignore list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool pickerIgnore_ShouldLoadChar(CharPicker picker, IPACharInfo charInfo)
		{
			return (!charInfo.IsBaseChar && charInfo.IgnoreType != IPACharIgnoreTypes.NotApplicable);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a string containing all the characters of the checked buttons in the
		/// specified chooser.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetIgnoredChars()
		{
			StringBuilder ignoreList = new StringBuilder();
			foreach (ToolStripButton item in pickerIgnore.Items)
			{
				if (item.Checked)
					ignoreList.Append(item.Text.Replace(DataUtils.kDottedCircle, string.Empty));
			}

			return (ignoreList.ToString());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetIgnoredChars(string ignoreList)
		{
			foreach (ToolStripButton item in pickerIgnore.Items)
			{
				// Remove the dotted circle (if there is one) from the button's text, then
				// check the button if its text is found in the ignore list.
				string chr = item.Text.Replace(DataUtils.kDottedCircle, string.Empty);
				item.Checked = (ignoreList != null && ignoreList.Contains(chr));
			}
		}
	}
}
