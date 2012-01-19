using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public partial class PatternBuilderComponents : UserControl, IxCoreColleague
	{
		public ItemDragEventHandler ConPickerDragHandler;
		public ItemDragEventHandler VowPickerDragHandler;
		public ToolStripItemClickedEventHandler ConPickerClickedHandler;
		public ToolStripItemClickedEventHandler VowPickerClickedHandler;
		public ItemDragEventHandler OtherCharDragHandler;
		public CharPicker.CharPickedHandler OtherCharPickedHandler;
		public ItemDragEventHandler FeatureListsItemDragHandler;
		public KeyPressEventHandler FeatureListsKeyPressHandler;
		public FeatureListViewBase.CustomDoubleClickHandler FeatureListDoubleClickHandler;
		public ItemDragEventHandler ClassListItemDragHandler;
		public KeyPressEventHandler ClassListKeyPressHandler;
		public MouseEventHandler ClassListDoubleClickHandler;

		private FeatureListViewBase _lvArticulatoryFeatures;
		private FeatureListViewBase _lvBinaryFeatures;
		private CharPickerRows _conPicker;
		private CharPickerRows _vowPicker;
		private List<char> _diacriticsInCache;

		/// ------------------------------------------------------------------------------------
		public PatternBuilderComponents()
		{
			InitializeComponent();
			App.AddMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();

				App.RemoveMediatorColleague(this);
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public void Initialize()
		{
			pnlConsonants.SuspendLayout();
			pnlVowels.SuspendLayout();
			tpgAFeatures.SuspendLayout();
			tpgBFeatures.SuspendLayout();

			tabPatternBlding.Font = FontHelper.UIFont;
			tpgClasses.Tag = lvClasses.Items;
			
			lvClasses.Load();
			lvClasses.LoadSettings(Name);
			lvClasses.ItemDrag += ClassListItemDragHandler;
			lvClasses.KeyPress += ClassListKeyPressHandler;
			lvClasses.MouseDoubleClick += ClassListDoubleClickHandler;

			SetupVowConPickers(true);
			SetupOtherPicker();

			_lvArticulatoryFeatures = InitializeFeatureList(new DescriptiveFeatureListView { CheckBoxes = false });
			_lvBinaryFeatures = InitializeFeatureList(new DistinctiveFeatureListView { CheckBoxes = true });
			tpgAFeatures.Controls.Add(_lvArticulatoryFeatures);
			tpgBFeatures.Controls.Add(_lvBinaryFeatures);

			tpgBFeatures.ResumeLayout(false);
			tpgAFeatures.ResumeLayout(false);
			pnlVowels.ResumeLayout(false);
			pnlConsonants.ResumeLayout(false);
		}

		/// ------------------------------------------------------------------------------------
		public void LoadSettings(string frmName, StringCollection explorerSettings)
		{
			charExplorer.LoadSettings(explorerSettings);
			lvClasses.LoadSettings(frmName);
		}

		/// ------------------------------------------------------------------------------------
		public void SaveSettings(string frmName, Action<StringCollection> getCharExplorerStatesAction)
		{
			lvClasses.SaveSettings(frmName);
			getCharExplorerStatesAction(charExplorer.GetExpandedStates());
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnSearchClassesChanged(object args)
		{
			lvClasses.Load();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDataSourcesModified(object args)
		{
			_lvArticulatoryFeatures.Load();
			_lvBinaryFeatures.Load();
			return false;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureListViewBase ArticulatoryFeaturesList
		{
			get { return _lvArticulatoryFeatures; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureListViewBase BinaryFeaturesList
		{
			get { return _lvBinaryFeatures; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharPickerRows ConsonantPicker
		{
			get { return _conPicker; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharPickerRows VowelPicker
		{
			get { return _vowPicker; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ClassListView ClassListView
		{
			get { return lvClasses; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void SetupVowConPickers(bool firstTime)
		{
			if (!firstTime)
			{
				_conPicker.ItemDrag -= ConPickerDragHandler;
				_conPicker.ItemClicked -= ConPickerClickedHandler;
				_vowPicker.ItemDrag -= VowPickerDragHandler;
				_vowPicker.ItemClicked -= VowPickerClickedHandler;
				pnlVowels.Controls.Clear();
				pnlConsonants.Controls.Clear();
				_conPicker.Dispose();
				_vowPicker.Dispose();
			}

			// Create the consonant picker on the con. tab.
			_conPicker = new CharPickerRows(() => App.Project.PhoneCache.Values
				.Where(p => p.CharType == IPASymbolType.consonant)
				.OrderBy(p => p.MOAKey).Cast<PhoneInfo>());
			
			_conPicker.Location = new Point(0, 0);
			_conPicker.BackColor = pnlConsonants.BackColor;
			pnlConsonants.Controls.Add(_conPicker);

			// Create the consonant picker on the vow. tab.
			_vowPicker = new CharPickerRows(() => App.Project.PhoneCache.Values
				.Where(p => p.CharType == IPASymbolType.vowel)
				.OrderBy(p => p.MOAKey).Cast<PhoneInfo>());

			_vowPicker.Location = new Point(0, 0);
			_vowPicker.BackColor = pnlVowels.BackColor;
			pnlVowels.Controls.Add(_vowPicker);

			_conPicker.ItemDrag += ConPickerDragHandler;
			_conPicker.ItemClicked += ConPickerClickedHandler;
			_vowPicker.ItemDrag += VowPickerDragHandler;
			_vowPicker.ItemClicked += VowPickerClickedHandler;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets up the character explorer on the "Other" tab of the side panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupOtherPicker()
		{
			_diacriticsInCache = new List<char>();

			// Go through all the phones in the cache and strip off their diacritics,
			// add the diacritics to a collection of diacritics.
			foreach (var phoneInfo in App.Project.PhoneCache)
			{
				_diacriticsInCache.AddRange((from c in phoneInfo.Key
											 let ci = App.IPASymbolCache[c]
											 where ci != null && !ci.IsBase
											 select c).ToArray());
			}

			charExplorer.ItemDrag += OtherCharDragHandler;
			charExplorer.CharPicked += OtherCharPickedHandler;
			charExplorer.Load((int)IPASymbolType.diacritic | (int)IPASymbolSubType.All, ci =>
			{
				// TODO: Fix this when chao characters are supported.

				// Always allow non consonants.
				if (ci.Type != IPASymbolType.consonant)
					return true;

				char chr = ci.Literal[0];

				// The only consonants to allow are the tie bars.
				return (_diacriticsInCache.Contains(chr) ||
					chr == App.kTopTieBarC || chr == App.kBottomTieBarC);
			});

			_diacriticsInCache = null;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates and initializes a feature list resultView and returns it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private FeatureListViewBase InitializeFeatureList(FeatureListViewBase flv)
		{
			flv.Load();
			flv.Dock = DockStyle.Fill;
			flv.LabelEdit = false;
			flv.AllowDoubleClickToChangeCheckState = false;
			flv.EmphasizeCheckedItems = false;
			flv.ItemDrag += FeatureListsItemDragHandler;
			flv.KeyPress += FeatureListsKeyPressHandler;
			flv.CustomDoubleClick += FeatureListDoubleClickHandler;

			return flv;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For some reason .Net doesn't paint tab page backgrounds properly when visual
		/// styles are active and the tab control changes sizes. Therefore, this will force
		/// repainting after a resize.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tabPatternBlding_ClientSizeChanged(object sender, EventArgs e)
		{
			tabPatternBlding.SelectedTab.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshComponents()
		{
			SetupVowConPickers(false);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnPhoneChartArrangementChanged(object args)
		{
			RefreshComponents();
			return false;
		}
		
		/// ------------------------------------------------------------------------------------
		public void RefreshFonts()
		{
			_conPicker.RefreshFont();
			_vowPicker.RefreshFont();
			charExplorer.RefreshFont();
		}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] {this};
		}

		#endregion
	}
}
