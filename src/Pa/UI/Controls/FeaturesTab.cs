using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public partial class FeaturesTab : UserControl
	{
		private FeatureListView m_lvAFeatures;
		private FeatureListView m_lvBFeatures;

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IFeatureBearer CurrentFeatureInfo { get; set; }

		/// ------------------------------------------------------------------------------------
		public FeaturesTab()
		{
			InitializeComponent();
			_tabCtrl.Font = FontHelper.UIFont;
			lblAFeatures.Font = new Font(FontHelper.UIFont, FontStyle.Bold);
			SetupFeatureLists();
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
				
				if (m_lvAFeatures != null && !m_lvAFeatures.IsDisposed)
				{
					m_lvAFeatures.Dispose();
					m_lvAFeatures = null;
				}

				if (m_lvBFeatures != null && !m_lvBFeatures.IsDisposed)
				{
					m_lvBFeatures.Dispose();
					m_lvBFeatures = null;
				}
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			
			// There's a painting bug that manifests itself when tab control's change sizes.
			_tabCtrl.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private void SetupFeatureLists()
		{
			if (App.DesignMode)
				return;

			m_lvAFeatures = new FeatureListView(App.FeatureType.Articulatory);
			m_lvAFeatures.Dock = DockStyle.Fill;
			m_lvAFeatures.Load();
			tpgAFeatures.Controls.Add(m_lvAFeatures);
			m_lvAFeatures.BringToFront();

			m_lvAFeatures.FeatureChanged += HandleArticulatoryFeatureCheckChanged;

			m_lvBFeatures = new FeatureListView(App.FeatureType.Binary);
			m_lvBFeatures.Dock = DockStyle.Fill;
			m_lvBFeatures.Load();
			tpgBFeatures.Controls.Add(m_lvBFeatures);
			m_lvBFeatures.BringToFront();

			m_lvAFeatures.BorderStyle = BorderStyle.None;
			m_lvBFeatures.BorderStyle = BorderStyle.None;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleArticulatoryFeatureCheckChanged(object sender, FeatureMask newMask)
		{
			lblAFeatures.Text = m_lvAFeatures.FormattedFeaturesString;
		}

		/// ------------------------------------------------------------------------------------
		public void SetCurrentInfo(IFeatureBearer info)
		{
			CurrentFeatureInfo = info;
			m_lvAFeatures.CurrentMask = CurrentFeatureInfo.AMask;
			m_lvBFeatures.CurrentMask = CurrentFeatureInfo.BMask;
			lblAFeatures.Text = m_lvAFeatures.FormattedFeaturesString;
		}

		/// ------------------------------------------------------------------------------------
		public void Reset()
		{
			if (CurrentFeatureInfo == null)
				return;

			if (_tabCtrl.SelectedTab == tpgAFeatures)
			{
				CurrentFeatureInfo.ResetAFeatures();
				m_lvAFeatures.CurrentMask = CurrentFeatureInfo.AMask;
				lblAFeatures.Text = m_lvAFeatures.FormattedFeaturesString;
			}
			else if (_tabCtrl.SelectedTab == tpgBFeatures)
			{
				CurrentFeatureInfo.ResetBFeatures();
				m_lvBFeatures.CurrentMask = CurrentFeatureInfo.BMask;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTableLayoutPaint(object sender, PaintEventArgs e)
		{
			var rc = ((Control)sender).ClientRectangle;
			e.Graphics.DrawLine(SystemPens.GrayText, rc.X, rc.Bottom - 6,
				rc.Right - 1, rc.Bottom - 6);
		}
	}
}
