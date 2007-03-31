using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SIL.SpeechTools.Database;
using SIL.SpeechTools.Utils;
using SIL.Pa.Resources;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a chooser for IPA characters
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class IPACharChooser : UserControl
	{
		private struct ChooserItemInfo
		{
			internal string tooltip;
			internal string character;

			internal ChooserItemInfo(string chr, string tip)
			{
				character = chr;
				tooltip = tip;
			}
		}

		public delegate bool ShouldLoadCharHandler(IPACharChooser chooser, IPACharInfo charInfo);
		public delegate void CharChosenEventHandler(IPACharChooser chooser, ButtonLabel label,
			string ipaChar);

		public event ShouldLoadCharHandler ShouldLoadChar;
		public event CharChosenEventHandler CharChosen;
		public event ItemDragEventHandler ItemDrag;
		
		private Size m_cellSize = new Size(28, 28);
		private bool m_allowCheckableButtons = false;
		private SortedDictionary<int, ChooserItemInfo> m_charsToLoad;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPACharChooser()
		{
			InitializeComponent();

			if (!PaApp.DesignMode)
				Font = new Font(FontHelper.PhoneticFont.Name, 14, GraphicsUnit.Point);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs an instance of an IPACharChooser and loads the characters of the
		/// specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPACharChooser(IPACharacterTypeInfo typeInfo) : this()
		{
			LoadCharacterType(typeInfo);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the control's background color.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get {return base.BackColor;}
			set
			{
				base.BackColor = value;
				flPanel.BackColor = value;
				foreach (ButtonLabel bt in flPanel.Controls)
					bt.BackColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the font used for the IPA characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override Font Font
		{
			get	{return base.Font;}
			set
			{
				base.Font = value;
				foreach (ButtonLabel bt in flPanel.Controls)
					bt.Font = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the size of each IPA character ButtonLabel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Size CellSize
		{
			get {return m_cellSize;}
			set
			{
				m_cellSize = value;
				foreach (ButtonLabel bt in flPanel.Controls)
					bt.Size = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the minimum height allowed so all IPA characters are visible in chooser.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int PreferredHeight
		{
			get
			{
				int height = 0;

				foreach (ButtonLabel bt in flPanel.Controls)
				{
					if (bt.Left == 0)
						height = bt.Bottom;
				}

				return height;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the collection of ButtonLabels contained in the chooser.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ControlCollection CharacterLabels
		{
			get {return flPanel.Controls;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not clicking on the character buttons
		/// will toggle their checked state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AllowCheckableButtons
		{
			get {return m_allowCheckableButtons;}
			set
			{
				m_allowCheckableButtons = value;
				foreach (ButtonLabel bt in CharacterLabels)
					bt.ShowCheckedState = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an array of all the checked buttons in the chooser. When AllowCheckableButtons
		/// is set to false, or when there are no checked buttons, this method returns null.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ButtonLabel[] CheckedButtons
		{
			get
			{
				if (!m_allowCheckableButtons)
					return null;

				List<ButtonLabel> chkdButtons = new List<ButtonLabel>();
				foreach (ButtonLabel bt in CharacterLabels)
				{
					if (bt.Checked)
						chkdButtons.Add(bt);
				}

				return (chkdButtons.Count == 0 ? null : chkdButtons.ToArray());
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides a way to add characters to the chooser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Add(string character, string description)
		{
			ButtonLabel lbl = new ButtonLabel(character, description);
			lbl.Size = m_cellSize;
			lbl.Font = Font;
			lbl.BackColor = flPanel.BackColor;
			lbl.Name = character;
			lbl.ShowCheckedState = m_allowCheckableButtons;
			flPanel.Controls.Add(lbl);

			// Create a delegate to pass on clicking on one of the IPA characters.
			lbl.Click += delegate(object sender, EventArgs e)
			{
				ButtonLabel bt = sender as ButtonLabel;
				if (bt != null && CharChosen != null)
					CharChosen(this, bt, bt.Text);
			};

			// Create a delegate to pass on dragging an IPA characters.
			lbl.MouseMove += delegate(object sender, MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left)
				{
					// Only fire the ItemDrag event when the mouse cursor has moved outside
					// the bounds of the button label while the left mouse button was down.
					ButtonLabel bt = sender as ButtonLabel;
					if (bt != null && ItemDrag != null && !bt.ClientRectangle.Contains(e.Location))
					{
						ItemDragEventArgs args = new ItemDragEventArgs(e.Button, bt.Text);
						ItemDrag(bt, args);
					}
				}
			};
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the chooser with information from a collection of ChooserItemInfo objects
		/// created from one of the public loading methods.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InternalLoad()
		{
			if (m_charsToLoad != null && m_charsToLoad.Count > 0)
			{
				foreach (ChooserItemInfo chooserInfo in m_charsToLoad.Values)
					Add(chooserInfo.character, chooserInfo.tooltip);
			}
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with the specified character type.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacterType(IPACharacterTypeInfo typeInfo)
		{
			m_charsToLoad = new SortedDictionary<int, ChooserItemInfo>();

			foreach (IPACharInfo charInfo in DBUtils.IPACharCache.Values)
			{
				if (charInfo.CharType == typeInfo.Type && (charInfo.CharSubType == typeInfo.SubType ||
					typeInfo.SubType == IPACharacterSubType.Unknown))
				{
					if (ShouldLoadChar != null && !ShouldLoadChar(this, charInfo))
						continue;
					
					string chr = (charInfo.DisplayWDottedCircle ?
						DBUtils.kDottedCircle : string.Empty) + charInfo.IPAChar;
					
					string tooltip =
						string.Format(ResourceHelper.GetString("kstidIPAChooserTooltip"),
						charInfo.Name, "\n", charInfo.Description);

					// Characters will be sorted by place of articulation.
					m_charsToLoad[charInfo.POArticulation] = new ChooserItemInfo(chr, tooltip);
				}
			}

			InternalLoad();
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with the specified ignore character type.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacterType(IPACharIgnoreTypes type)
		{
			m_charsToLoad = new SortedDictionary<int, ChooserItemInfo>();

			foreach (IPACharInfo charInfo in DBUtils.IPACharCache.Values)
			{
				if (charInfo.IgnoreType == type)
				{
					if (ShouldLoadChar != null && !ShouldLoadChar(this, charInfo))
						continue;
					
					string chr = (charInfo.DisplayWDottedCircle ?
						DBUtils.kDottedCircle : string.Empty) + charInfo.IPAChar;
					
					string tooltip =
						string.Format(ResourceHelper.GetString("kstidIPAChooserTooltip"),
						charInfo.Name, "\n", charInfo.Description);

					// Characters will be sorted by place of articulation.
					m_charsToLoad[charInfo.POArticulation] = new ChooserItemInfo(chr, tooltip);
				}
			}

			InternalLoad();
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Load the chooser with characters checked by a loading delegate.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		public void LoadCharacters()
		{
			if (ShouldLoadChar == null)
				return;

			m_charsToLoad = new SortedDictionary<int, ChooserItemInfo>();

			foreach (IPACharInfo charInfo in DBUtils.IPACharCache.Values)
			{
				if (ShouldLoadChar(this, charInfo))
				{
					string chr = (charInfo.DisplayWDottedCircle ?
						DBUtils.kDottedCircle : string.Empty) + charInfo.IPAChar;

					string tooltip =
						string.Format(ResourceHelper.GetString("kstidIPAChooserTooltip"),
						charInfo.Name, "\n", charInfo.Description);

					// Characters will be sorted by place of articulation.
					m_charsToLoad[charInfo.POArticulation] = new ChooserItemInfo(chr, tooltip);
				}
			}

			InternalLoad();
		}
	}
}
