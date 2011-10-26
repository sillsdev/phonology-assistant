using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SIL.Pa.Model;

namespace SIL.Pa.UI.Controls
{
	public class DistinctiveFeatureListView : FeatureListViewBase
	{
		/// ------------------------------------------------------------------------------------
		public DistinctiveFeatureListView() : base(App.BFeatureCache.GetEmptyMask())
		{
			Name = "lvFeatures-Distinctve";
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<Feature> GetFeaturesToLoad()
		{
			return App.BFeatureCache.PlusFeatures.OrderBy(x => x.Name);
		}

		/// ------------------------------------------------------------------------------------
		protected override void InitializeLoadedItem(Feature feature, FeatureItemInfo itemInfo)
		{
			itemInfo.Name = feature.Name.TrimStart('-', '+');
			itemInfo.FullName = feature.Name.TrimStart('-', '+');
			itemInfo.PlusBit = feature.Bit;
			itemInfo.MinusBit = App.BFeatureCache.GetOppositeFeature(feature).Bit;
			itemInfo.IsBinary = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void SetFeatureInfoStateFromMask(FeatureItemInfo itemInfo, FeatureMask mask)
		{
			if (mask[itemInfo.PlusBit])
				itemInfo.TriStateValue = BinaryFeatureValue.Plus;
			else if (mask[itemInfo.MinusBit])
				itemInfo.TriStateValue = BinaryFeatureValue.Minus;
			else
				itemInfo.TriStateValue = BinaryFeatureValue.None;
		}

		/// ------------------------------------------------------------------------------------
		protected override void CycleFeatureStateValue(FeatureItemInfo itemInfo, FeatureMask mask)
		{
			switch (itemInfo.TriStateValue)
			{
				case BinaryFeatureValue.None:
					itemInfo.TriStateValue = BinaryFeatureValue.Plus;
					mask[itemInfo.MinusBit] = false;
					mask[itemInfo.PlusBit] = true;
					break;

				case BinaryFeatureValue.Plus:
					itemInfo.TriStateValue = BinaryFeatureValue.Minus;
					mask[itemInfo.MinusBit] = true;
					mask[itemInfo.PlusBit] = false;
					break;

				default:
					itemInfo.TriStateValue = BinaryFeatureValue.None;
					mask[itemInfo.MinusBit] = false;
					mask[itemInfo.PlusBit] = false;
					break;
			}
		}

		/// ------------------------------------------------------------------------------------
		public override void SetMaskFromPhoneInfo(IPhoneInfo phoneInfo)
		{
			CurrentMask = phoneInfo.BMask;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the check box with a plus, minus or nothing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void DrawFeatureState(Graphics g, FeatureItemInfo itemInfo, Rectangle rc)
		{
			// Draw an empty checkbox.
			CheckBoxRenderer.DrawCheckBox(g, rc.Location, CheckBoxState.UncheckedNormal);

			if (itemInfo == null || itemInfo.TriStateValue == BinaryFeatureValue.None)
				return;

			// Draw a plus or minus in the empty check box.
			using (Pen pen = new Pen(_glyphColor, 1))
			{
				var ptCenter = new Point(rc.X + (rc.Width / 2), rc.Y + (rc.Height / 2));

				// Draw the minus
				g.DrawLine(pen, ptCenter.X - 3, ptCenter.Y, ptCenter.X + 3, ptCenter.Y);

				// Draw the vertical line to make a plus if the feature's value is such.
				if (itemInfo.TriStateValue == BinaryFeatureValue.Plus)
					g.DrawLine(pen, ptCenter.X, ptCenter.Y - 3, ptCenter.X, ptCenter.Y + 3);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override bool GetIsItemSet(FeatureItemInfo itemInfo)
		{
			return (itemInfo != null && itemInfo.TriStateValue != BinaryFeatureValue.None);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool GetIsItemNotInDefaultState(FeatureItemInfo itemInfo)
		{
			var currentFeaturesName = GetFormattedFeatureName(itemInfo, false);
			var defaultListContainsNameWithoutPrefix = _defaultFeatures.Any(n => n.Substring(1) == itemInfo.Name);
			var isItemSet = GetIsItemSet(itemInfo);
			return !_defaultFeatures.Contains(currentFeaturesName) && (isItemSet || defaultListContainsNameWithoutPrefix);
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetFormattedFeatureName(FeatureItemInfo itemInfo,
			bool includeBrackets)
		{
			if (itemInfo.TriStateValue == BinaryFeatureValue.None)
				return itemInfo.Name;

			var fmt = (includeBrackets ? "[{0}{1}]" : "{0}{1}");
			return string.Format(fmt, (char)itemInfo.TriStateValue, itemInfo.Name);
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string FormattedFeaturesString
		{
			get
			{
				var bldr = new StringBuilder();

				foreach (var info in GetItemsThatAreSet())
					bldr.AppendFormat("{0}{1}, ", (char)info.TriStateValue, info.Name);

				return bldr.ToString().TrimEnd(',', ' ');
			}
		}
	}
}
