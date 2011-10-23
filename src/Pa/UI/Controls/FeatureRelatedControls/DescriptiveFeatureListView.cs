using System;
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
	public class DescriptiveFeatureListView : FeatureListViewBase
	{
		/// ------------------------------------------------------------------------------------
		public DescriptiveFeatureListView() : base(App.AFeatureCache.GetEmptyMask())
		{
			Name = "lvFeatures-Descriptive";
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (!Application.RenderWithVisualStyles)
				return;

			var renderer = new VisualStyleRenderer(VisualStyleElement.Button.CheckBox.UncheckedNormal);
			_glyphColor = renderer.GetColor(ColorProperty.BorderColor);

			using (var g = CreateGraphics())
				_chkBoxSize = renderer.GetPartSize(g, ThemeSizeType.Draw);
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<Feature> GetFeaturesToLoad()
		{
			return App.AFeatureCache.Values.OrderBy(x => x.Name);
		}

		/// ------------------------------------------------------------------------------------
		protected override void InitializeLoadedItem(Feature feature, FeatureItemInfo itemInfo)
		{
			itemInfo.Bit = feature.Bit;
		}

		/// ------------------------------------------------------------------------------------
		protected override void SetFeatureInfoStateFromMask(FeatureItemInfo itemInfo, FeatureMask mask)
		{
			itemInfo.Checked = mask[itemInfo.Bit];
		}

		/// ------------------------------------------------------------------------------------
		protected override void CycleFeatureStateValue(FeatureItemInfo itemInfo, FeatureMask mask)
		{
			itemInfo.Checked = !itemInfo.Checked;
			mask[itemInfo.Bit] = itemInfo.Checked;
		}

		/// ------------------------------------------------------------------------------------
		public override void SetMaskFromPhoneInfo(IPhoneInfo phoneInfo)
		{
			CurrentMask = phoneInfo.AMask;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw normal checked/unchecked check box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void DrawFeatureState(Graphics g, FeatureItemInfo itemInfo, Rectangle rc)
		{
			CheckBoxRenderer.DrawCheckBox(g, rc.Location, (itemInfo != null && itemInfo.Checked ?
				CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal));
		}

		/// ------------------------------------------------------------------------------------
		protected override bool GetIsItemSet(FeatureItemInfo itemInfo)
		{
			return (itemInfo != null && itemInfo.Checked);
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetFormattedFeatureName(FeatureItemInfo itemInfo, bool includeBrackets)
		{
			var fmt = (includeBrackets ? "[{0}]" : "{0}");
			return string.Format(fmt, itemInfo.Name);
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
					bldr.AppendFormat("{0}, ", info.Name);

				return bldr.ToString().TrimEnd(',', ' ');
			}
		}
	}
}
