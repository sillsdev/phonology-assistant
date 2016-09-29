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
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public partial class PhonesInFeatureViewer : UserControl
	{
		private bool _compactView;
		private bool _allFeaturesMustMatch;
		private FeatureMask _aMask = App.AFeatureCache.GetEmptyMask();
		private FeatureMask _bMask = App.BFeatureCache.GetEmptyMask();
		private SearchClassType _srchClassType;
		private readonly int _extraPhoneHeight;
		private readonly PhoneInfo[] _phonesToLoad;
		private readonly Action<bool> _saveCompactViewAction;
		
		/// ------------------------------------------------------------------------------------
		public PhonesInFeatureViewer()
		{
			InitializeComponent();

			base.DoubleBuffered = true;
			base.BackColor = Color.Transparent;
			header.Font = FontHelper.UIFont;

			if (!App.DesignMode)
				base.Font = FontHelper.MakeRegularFontDerivative(App.PhoneticFont, 14);

			_extraPhoneHeight += Properties.Settings.Default.PhonesInFeaturesListExtraPhoneHeight;
		}

		/// ------------------------------------------------------------------------------------
		public PhonesInFeatureViewer(IEnumerable<PhoneInfo> phonesToLoad,
			bool initiallyShowCompactView, Action<bool> saveCompactViewAction) : this()
		{
			_phonesToLoad = phonesToLoad.ToArray();
			_compactView = initiallyShowCompactView;
			_saveCompactViewAction = saveCompactViewAction;

			_flowLayout.Dock = DockStyle.Fill;
			_tableLayout.Dock = DockStyle.Fill;

			LoadPhones();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (_saveCompactViewAction != null)
				_saveCompactViewAction(_compactView);
			
			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		private void RefreshEnabledPhones()
		{
			var ctrlCollection = (_compactView ? _flowLayout.Controls : _tableLayout.Controls);

			foreach (Label lbl in ctrlCollection)
				lbl.Enabled = GetIsPhoneEnabled(lbl.Tag as PhoneInfo);
		}

		/// ------------------------------------------------------------------------------------
		public void LoadPhones()
		{
			Utils.SetWindowRedraw(this, false);
			
			_flowLayout.Visible = false;
			_tableLayout.Visible = false;
			_tableLayout.Controls.Clear();
			_flowLayout.Controls.Clear();

			if (_phonesToLoad.Length == 0)
			{
				Utils.SetWindowRedraw(this, true);
				return;
			}

			if (_compactView)
				LoadCompactView();
			else
				LoadExpandedView();

			Utils.SetWindowRedraw(this, true);
			RefreshEnabledPhones();
		}

		/// ------------------------------------------------------------------------------------
		private void LoadCompactView()
		{
			_flowLayout.Controls.AddRange(_phonesToLoad.Select(p => CreateLabel(p)).ToArray());
			_flowLayout.Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		private void LoadExpandedView()
		{
			_tableLayout.RowCount = 0;
			_tableLayout.ColumnCount = 0;
			_tableLayout.RowStyles.Clear();
			_tableLayout.ColumnStyles.Clear();
			int row = 0;
			int col = 0;
			PhoneInfo prevPhoneInfo = null;

			foreach (var phoneInfo in _phonesToLoad)
			{
				var lbl = CreateLabel(phoneInfo);

				if (prevPhoneInfo == null || prevPhoneInfo.RowGroup == phoneInfo.RowGroup)
				{
					_tableLayout.ColumnCount++;
					_tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
				}
				else
				{
					row++;
					col = 0;
					_tableLayout.RowCount++;
					_tableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				}

				_tableLayout.Controls.Add(lbl, col++, row);
				prevPhoneInfo = phoneInfo;
			}

			_tableLayout.Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		private Label CreateLabel(IPhoneInfo phoneInfo)
		{
			var lbl = new Label();
			lbl.Font = Font;
			lbl.Text = phoneInfo.Phone;
			lbl.BackColor = Color.Transparent;
			lbl.AutoSize = false;
			lbl.Size = lbl.PreferredSize;
			lbl.Height += _extraPhoneHeight;
			lbl.Margin = new Padding(0);
			lbl.Tag = phoneInfo;
			return lbl;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the specified phone information matches the
		/// current mask(s).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool GetIsPhoneEnabled(IPhoneInfo phoneInfo)
		{
			if (phoneInfo == null)
				return false;

			var mask1 = (_srchClassType == SearchClassType.Articulatory ? _aMask : _bMask);
			var mask2 = (_srchClassType == SearchClassType.Articulatory ? phoneInfo.AMask : phoneInfo.BMask);

			if (mask1.IsEmpty)
				return false;

			return (_allFeaturesMustMatch ? mask2.ContainsAll(mask1) : mask2.ContainsOneOrMore(mask1));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the viewer's current articulatory masks. Calling this method will also set
		/// the viewer's current search class type to Articulatory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetAMasks(FeatureMask mask, bool allFeaturesMustMatch)
		{
			_aMask = mask;
			_allFeaturesMustMatch = allFeaturesMustMatch;
			_srchClassType = SearchClassType.Articulatory;
			RefreshEnabledPhones();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the viewer's current binary mask. Calling this method will also set the
		/// viewer's current search class type to Binary.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetBMask(FeatureMask mask, bool allFeaturesMustMatch)
		{
			_bMask = mask;
			_allFeaturesMustMatch = allFeaturesMustMatch;
			_srchClassType = SearchClassType.Binary;
			RefreshEnabledPhones();
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
		public SearchClassType SearchClassType
		{
			get { return _srchClassType; }
			set
			{
				_srchClassType = value;
				RefreshEnabledPhones();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void header_Click(object sender, EventArgs e)
		{
			btnDropDownArrow_Click(null, null);
		}

		/// ------------------------------------------------------------------------------------
		private void btnDropDownArrow_Click(object sender, EventArgs e)
		{
			mnuCompact.Checked = _compactView;
			mnuExpanded.Checked = !_compactView;

			int width = Math.Max(header.Width, cmnuViewOptions.PreferredSize.Width);
			cmnuViewOptions.Size = new Size(width, cmnuViewOptions.PreferredSize.Height);
			cmnuViewOptions.Show(header, new Point(0, header.Height));
		}

		/// ------------------------------------------------------------------------------------
		private void mnuCompact_Click(object sender, EventArgs e)
		{
			if (!_compactView)
			{
				_compactView = true;
				LoadPhones();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void mnuExpanded_Click(object sender, EventArgs e)
		{
			if (_compactView)
			{
				_compactView = false;
				LoadPhones();
			}
		}
	}
}
