using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;
using XCore;

namespace SIL.Pa.Controls
{
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
		public FeatureListView.CustomDoubleClickHandler FeatureListDoubleClickHandler;
		public ItemDragEventHandler ClassListItemDragHandler;
		public KeyPressEventHandler ClassListKeyPressHandler;
		public MouseEventHandler ClassListDoubleClickHandler;

		private FeatureListView m_lvArticulatoryFeatures;
		private FeatureListView m_lvBinaryFeatures;
		private CharPickerRows m_conPicker;
		private CharPickerRows m_vowPicker;
		private List<char> m_diacriticsInCache;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PatternBuilderComponents()
		{
			InitializeComponent();
			PaApp.AddMediatorColleague(this);

			Disposed += PatternBuilderComponents_Disposed;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void PatternBuilderComponents_Disposed(object sender, EventArgs e)
		{
			Disposed -= PatternBuilderComponents_Disposed;
			PaApp.RemoveMediatorColleague(this);
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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

			m_lvArticulatoryFeatures = InitializeFeatureList(PaApp.FeatureType.Articulatory);
			m_lvBinaryFeatures = InitializeFeatureList(PaApp.FeatureType.Binary);
			tpgAFeatures.Controls.Add(m_lvArticulatoryFeatures);
			tpgBFeatures.Controls.Add(m_lvBinaryFeatures);

			tpgBFeatures.ResumeLayout(false);
			tpgAFeatures.ResumeLayout(false);
			pnlVowels.ResumeLayout(false);
			pnlConsonants.ResumeLayout(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads settings using the specified form name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadSettings(string frmName)
		{
			charExplorer.LoadSettings(frmName);
			lvClasses.LoadSettings(frmName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves settings using the specified form name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveSettings(string frmName)
		{
			charExplorer.SaveSettings(frmName);
			lvClasses.SaveSettings(frmName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The classes were updated in the class dialog, so rebuild the class list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSearchClassesChanged(object args)
		{
			lvClasses.Load();
			return false;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureListView ArticulatoryFeaturesList
		{
			get { return m_lvArticulatoryFeatures; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureListView BinaryFeaturesList
		{
			get { return m_lvBinaryFeatures; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharPickerRows ConsonantPicker
		{
			get { return m_conPicker; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharPickerRows VowelPicker
		{
			get { return m_vowPicker; }
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupVowConPickers(bool firstTime)
		{
			if (!firstTime)
			{
				m_conPicker.ItemDrag -= ConPickerDragHandler;
				m_conPicker.ItemClicked -= ConPickerClickedHandler;
				m_vowPicker.ItemDrag -= VowPickerDragHandler;
				m_vowPicker.ItemClicked -= VowPickerClickedHandler;
				pnlVowels.Controls.Clear();
				pnlConsonants.Controls.Clear();
				m_conPicker.Dispose();
				m_vowPicker.Dispose();
			}

			// Create the consonant picker on the con. tab.
			m_conPicker = new CharPickerRows();
			m_conPicker.Location = new Point(0, 0);
			m_conPicker.BackColor = pnlConsonants.BackColor;
			CharGridBuilder bldr = new CharGridBuilder(m_conPicker, IPACharacterType.Consonant);
			bldr.Build();
			pnlConsonants.Controls.Add(m_conPicker);

			// Create the consonant picker on the vow. tab.
			m_vowPicker = new CharPickerRows();
			m_vowPicker.Location = new Point(0, 0);
			m_vowPicker.BackColor = pnlVowels.BackColor;
			bldr = new CharGridBuilder(m_vowPicker, IPACharacterType.Vowel);
			bldr.Build();
			pnlVowels.Controls.Add(m_vowPicker);

			if (firstTime)
			{
				m_conPicker.ItemDrag += ConPickerDragHandler;
				m_conPicker.ItemClicked += ConPickerClickedHandler;
				m_vowPicker.ItemDrag += VowPickerDragHandler;
				m_vowPicker.ItemClicked += VowPickerClickedHandler;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets up the character explorer on the "Other" tab of the side panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupOtherPicker()
		{
			m_diacriticsInCache = new List<char>();

			// Go through all the phones in the cache and strip off their diacritics,
			// add the diacritics to a collection of diacritics.
			foreach (KeyValuePair<string, IPhoneInfo> phoneInfo in PaApp.PhoneCache)
			{
				foreach (char c in phoneInfo.Key)
				{
					IPACharInfo charInfo = DataUtils.IPACharCache[c];
					if (charInfo != null && !charInfo.IsBaseChar)
						m_diacriticsInCache.Add(c);
				}
			}

			List<IPACharacterTypeInfo> typesToShow = new List<IPACharacterTypeInfo>();

			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Diacritics));

			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Suprasegmentals,
				IPACharacterSubType.StressAndLength));

			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Suprasegmentals,
				IPACharacterSubType.ToneAndAccents));

			typesToShow.Add(new IPACharacterTypeInfo(IPACharacterType.Consonant,
				IPACharacterSubType.OtherSymbols));

			charExplorer.TypesToShow = typesToShow;
			charExplorer.ShouldLoadChar += OtherCharShouldLoadChar;
			charExplorer.ItemDrag += OtherCharDragHandler;
			charExplorer.CharPicked += OtherCharPickedHandler;
			charExplorer.Load();

			m_diacriticsInCache = null;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates and initializes a feature list resultView and returns it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private FeatureListView InitializeFeatureList(PaApp.FeatureType featureType)
		{
			FeatureListView flv = new FeatureListView(featureType);
			flv.Load();
			flv.Dock = DockStyle.Fill;
			flv.LabelEdit = false;
			flv.AllowDoubleClickToChangeCheckState = false;
			flv.EmphasizeCheckedItems = false;
			flv.CheckBoxes = (featureType == PaApp.FeatureType.Binary);
			flv.ItemDrag += FeatureListsItemDragHandler;
			flv.KeyPress += FeatureListsKeyPressHandler;
			flv.CustomDoubleClick += FeatureListDoubleClickHandler;

			return flv;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified modifying characters should be loaded into
		/// the character explorer on the "Other" tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool OtherCharShouldLoadChar(CharPicker picker, IPACharInfo charInfo)
		{
			// TODO: Fix this when chao characters are supported.

			// Always allow non consonants.
			if (charInfo.CharType != IPACharacterType.Consonant)
				return true;

			char chr = charInfo.IPAChar[0];

			// The only consonants to allow are the tie bars.
			return (m_diacriticsInCache.Contains(chr) ||
				chr == DataUtils.kTopTieBarC || chr == DataUtils.kBottomTieBarC);
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
		/// <summary>
		/// Rebuilds the components when the project query changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshComponents()
		{
			SetupVowConPickers(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides a way to force the components to update their fonts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshFonts()
		{
			m_conPicker.RefreshFont();
			m_vowPicker.RefreshFont();
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, XmlNode configurationParameters)
		{
			// Not used in PA
		}

		#endregion
	}
}
