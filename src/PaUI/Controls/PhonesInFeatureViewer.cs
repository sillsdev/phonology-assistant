using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Data;
using SilUtils;

namespace SIL.Pa
{
	public partial class PhonesInFeatureViewer : UserControl, IPhoneListViewer
	{
		private bool m_showAll;
		private bool m_compactView;
		private bool m_canViewExpandAndCompact = true;
		private bool m_allFeaturesMustMatch;
		private FeatureMask m_aMask = DataUtils.AFeatureCache.GetEmptyMask();
		private FeatureMask m_bMask = DataUtils.BFeatureCache.GetEmptyMask();
		private Control m_pnlView;
		private SearchClassType m_srchClassType;
		private int m_lblHeight;
		private readonly int m_extraPhoneHeight;
		private readonly string m_settingValPrefix;
		private readonly string m_frmName;
		private readonly IPACharacterType m_charType;
		private readonly SortedList<int, List<Label>> m_lableRows;
		private readonly CompactViewerPanel pnlCompactView;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhonesInFeatureViewer()
		{
			InitializeComponent();

			pnlCompactView = new CompactViewerPanel();
			pnlOuter.Controls.Add(pnlCompactView);
			pnlCompactView.BringToFront();

			base.DoubleBuffered = true;
			base.BackColor = Color.Transparent;
			m_lableRows = new SortedList<int, List<Label>>();
			header.Font = FontHelper.UIFont;

			if (!PaApp.DesignMode)
				base.Font = FontHelper.MakeEticRegFontDerivative(14);

			m_extraPhoneHeight += PaApp.SettingsHandler.GetIntSettingsValue(
				"phonesinfeatureslists", "extraphoneheight", 3);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhonesInFeatureViewer(IPACharacterType charType, string frmName,
			string settingValPrefix) : this()
		{
			m_settingValPrefix = settingValPrefix;
			m_frmName = frmName;
			
			m_showAll = PaApp.SettingsHandler.GetBoolSettingsValue(frmName,
				m_settingValPrefix + "ShowAll", true);
			
			m_compactView = PaApp.SettingsHandler.GetBoolSettingsValue(frmName,
				m_settingValPrefix + "CompactView", false);

			pnlCompactView.Dock = DockStyle.Fill;
			pnlExpandedView.Dock = DockStyle.Fill;

			m_charType = charType;
			RefreshLayout();
			Disposed += PhonesInFeatureViewer_Disposed;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the show all setting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PhonesInFeatureViewer_Disposed(object sender, EventArgs e)
		{
			Disposed -= PhonesInFeatureViewer_Disposed;

			PaApp.SettingsHandler.SaveSettingsValue(m_frmName,
				m_settingValPrefix + "ShowAll", m_showAll);

			PaApp.SettingsHandler.SaveSettingsValue(m_frmName,
				m_settingValPrefix + "CompactView", m_compactView);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Reset()
		{
			// The panel that hosts all the labels will be different depending upon whether
			// we're showing the compact view or not. When showing the compact view, a flow
			// layout panel is used. Otherwise a normal panel is used.
			if (m_compactView && (m_pnlView != pnlCompactView || !pnlCompactView.Visible))
			{
				pnlExpandedView.Visible = false;
				pnlCompactView.Visible = true;
				m_pnlView = pnlCompactView;
			}
			else if (!m_compactView && (m_pnlView != pnlExpandedView || !pnlExpandedView.Visible))
			{
				pnlExpandedView.Visible = true;
				pnlCompactView.Visible = false;
				m_pnlView = pnlExpandedView;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshLayout()
		{
			Reset();

			if (m_pnlView != null)
				m_pnlView.Controls.Clear();

			m_lableRows.Clear();

			if (m_charType != IPACharacterType.Unknown)
			{
				// Load vowels or consonants from a builder so the phones can be layed out
				// in POA and MOA order.
				CharGridBuilder bldr = new CharGridBuilder(this, m_charType);
				bldr.Build();
			}
			else
			{
				// We should never need this key, but just in case a phone's first character
				// cannot be found in the IPA character cache, we'll use this as the key
				// to store the phone in the sorted list of labels.
				int key = 1000;

				SortedList<string, Label> labels = new SortedList<string, Label>();

				// Load all non consonant/vowel phones by first adding them to a list
				// in POA order.
				foreach (KeyValuePair<string, IPhoneInfo> kvpPhoneInfo in PaApp.PhoneCache)
				{
					IPhoneInfo iPhoneInfo = kvpPhoneInfo.Value;
					PhoneInfo phoneInfo = iPhoneInfo as PhoneInfo;

					if (iPhoneInfo.CharType != IPACharacterType.Consonant &&
						iPhoneInfo.CharType != IPACharacterType.Vowel &&
						(phoneInfo == null || !phoneInfo.IsUndefined))
					{
						Label lbl = CreateLabel(kvpPhoneInfo.Key, iPhoneInfo);
						if (!labels.ContainsKey(iPhoneInfo.POAKey))
							labels[iPhoneInfo.POAKey] = lbl;
						else
						{
							labels[key.ToString()] = lbl;
							key++;
						}
					}
				}

				// Now move the labels into the list that LoadLayout uses to add the
				// labels to the appropriate panel control.
				if (labels.Count > 0)
				{
					m_lableRows[0] = new List<Label>();
					foreach (Label lbl in labels.Values)
						m_lableRows[0].Add(lbl);
				}

				LayoutPhones();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadPhones(List<CharGridCell> phoneList)
		{
			if (phoneList == null)
				return;

			int prevRow = -1;

			// Create a collection of rows of phones. Each row is
			// a collection of those labels that go on that row.
			foreach (CharGridCell cgc in phoneList)
			{
				Label lblPhone = CreateLabel(cgc.Phone);

				if (prevRow == -1)
					prevRow = cgc.Row;

				if (prevRow != cgc.Row)
					prevRow = cgc.Row;

				if (!m_lableRows.ContainsKey(cgc.Row))
					m_lableRows[cgc.Row] = new List<Label>();

				m_lableRows[cgc.Row].Add(lblPhone);
			}

			LayoutPhones();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Label CreateLabel(string phone)
		{
			IPhoneInfo phoneInfo = PaApp.PhoneCache[phone];
			return CreateLabel(phone, phoneInfo);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Label CreateLabel(string phone, IPhoneInfo phoneInfo)
		{
			Label lbl = new Label();
			lbl.Font = Font;
			lbl.Text = phone;
			lbl.Size = lbl.PreferredSize;
			lbl.Height += m_extraPhoneHeight;
			lbl.BackColor = Color.Transparent;
			lbl.Visible = false;
			lbl.Tag = phoneInfo;

			if (m_lblHeight == 0)
				m_lblHeight = lbl.Height;

			return lbl;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Lays out all the phone labels in the proper order and within the proper rows.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LayoutPhones()
		{
			Reset();

			bool addPhonesToViewPanel = (m_pnlView.Controls.Count == 0);
			m_pnlView.SuspendLayout();
			int dy = 0;

			// Add the labels to the control collection.
			foreach (List<Label> row in m_lableRows.Values)
			{
				int dx = 0;

				foreach (Label lbl in row)
				{
					if (addPhonesToViewPanel)
						m_pnlView.Controls.Add(lbl);

					lbl.Enabled = GetPhonesEnabledState(lbl.Tag as IPhoneInfo);
					if (!lbl.Enabled && !m_showAll)
						lbl.Visible = false;
					else
					{
						if (m_pnlView is Panel)
						{
							lbl.Location = new Point(dx, dy);
							dx += lbl.Width;
						}

						lbl.Visible = true;
					}
				}

				dy += m_lblHeight;
			}
	
			m_pnlView.ResumeLayout(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the specified phone information matches the
		/// current mask(s).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool GetPhonesEnabledState(IPhoneInfo phoneInfo)
		{
			if (phoneInfo == null)
				return false;

			FeatureMask mask1 =
				(m_srchClassType == SearchClassType.Articulatory ? m_aMask : m_bMask);

			FeatureMask mask2 = (m_srchClassType == SearchClassType.Articulatory ?
				phoneInfo.AMask : phoneInfo.BMask);

			if (mask1.IsEmpty)
				return false;

			return (m_allFeaturesMustMatch ? mask2.ContainsAll(mask1) : mask2.ContainsOneOrMore(mask1));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the viewer's current articulatory masks. Calling this method will also set
		/// the viewer's current search class type to Articulatory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetAMasks(FeatureMask mask, bool allFeaturesMustMatch)
		{
			m_aMask = mask;
			m_allFeaturesMustMatch = allFeaturesMustMatch;
			m_srchClassType = SearchClassType.Articulatory;
			LayoutPhones();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the viewer's current binary mask. Calling this method will also set the
		/// viewer's current search class type to Binary.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetBMask(FeatureMask mask, bool allFeaturesMustMatch)
		{
			m_bMask = mask;
			m_allFeaturesMustMatch = allFeaturesMustMatch;
			m_srchClassType = SearchClassType.Binary;
			LayoutPhones();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Don't need this for this class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SupraSegsToIgnore
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the text of the header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string HeaderText
		{
			get { return header.Text; }
			set { header.Text = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchClassType SearchClassType
		{
			get { return m_srchClassType; }
			set
			{
				m_srchClassType = value;
				LayoutPhones();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the expanded and compacted views
		/// are available. When this value is false, the view is automatically in a compact
		/// view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CanViewExpandAndCompact
		{
			get { return m_canViewExpandAndCompact; }
			set
			{
				m_canViewExpandAndCompact = value;
				mnuCompact.Visible = value;
				mnuExpanded.Visible = value;
				mnuSep.Visible = value;
				m_compactView = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void header_Click(object sender, EventArgs e)
		{
			btnDropDownArrow_Click(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnDropDownArrow_Click(object sender, EventArgs e)
		{
			mnuShowAll.Checked = m_showAll;
			mnuCompact.Checked = m_compactView;
			mnuExpanded.Checked = !m_compactView;

			int width = Math.Max(header.Width, cmnuViewOptions.PreferredSize.Width);
			cmnuViewOptions.Size = new Size(width, cmnuViewOptions.PreferredSize.Height);
			cmnuViewOptions.Show(header, new Point(0, header.Height));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuShowAll_Click(object sender, EventArgs e)
		{
			m_showAll = !m_showAll;
			LayoutPhones();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuCompact_Click(object sender, EventArgs e)
		{
			if (!m_compactView)
			{
				m_compactView = true;
				LayoutPhones();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuExpanded_Click(object sender, EventArgs e)
		{
			if (m_compactView)
			{
				m_compactView = false;
				LayoutPhones();
			}
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	internal class CompactViewerPanel : FlowLayoutPanel
	{
		internal CompactViewerPanel()
		{
			base.AutoScroll = true;
			base.Dock = DockStyle.Fill;
			base.BackColor = Color.Transparent;
			base.DoubleBuffered = true;
		}
	}
}
