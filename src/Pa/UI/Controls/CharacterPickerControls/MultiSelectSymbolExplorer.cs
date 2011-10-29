using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public class MultiSelectSymbolExplorer : IPACharacterExplorer
	{
		/// ------------------------------------------------------------------------------------
		public override void Load(int typesToLoad, System.Func<IPASymbol, bool> shouldLoadCharDelegate)
		{
			base.Load(typesToLoad, shouldLoadCharDelegate);
		
			foreach (var item in Items)
			{
				item.CanCollapse = false;
				item.ShowCheckBox = true;
				item.ButtonVerticalPadding = 2;
				item.Font = FontHelper.UIFont;
				item.ShowButtonFocusCues = false;
				item.Button.TabStop = false;
				item.CheckBoxChecked += HandleSymbolTypeHeadingCheckBoxChecked;
			}

			foreach (var picker in _pickers)
			{
				picker.CheckItemsOnClick = true;
				picker.CharPicked += ((p, i) => SetPickersCheckStateBasedOnItsCheckedItems(p));
			}
		}

		/// ------------------------------------------------------------------------------------
		private static void HandleSymbolTypeHeadingCheckBoxChecked(bool checkBoxChecked, Control ctrl)
		{
			var picker = ctrl as CharPicker;
			if (picker != null)
			{
				if (checkBoxChecked)
					picker.CheckAllItems();
				else
					picker.UncheckAllItems();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void SetPickersCheckStateBasedOnItsCheckedItems(CharPicker picker)
		{
			var symbolTypeHeading = picker.Parent as ExplorerBarItem;
			if (symbolTypeHeading != null)
				symbolTypeHeading.CheckedBoxState = picker.GetRelevantCheckState();
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetTextsOfAllCheckedItems()
		{
			return _pickers.SelectMany(picker => picker.GetTextsOfCheckedItems());
		}

		/// ------------------------------------------------------------------------------------
		public void SetCheckedItemsByText(IEnumerable<string> textsOfItemsToCheck)
		{
			var textList = textsOfItemsToCheck.ToArray();

			foreach (var picker in _pickers)
			{
				picker.SetCheckedItemsByText(textList);
				SetPickersCheckStateBasedOnItsCheckedItems(picker);
			}
		}
	}
}
