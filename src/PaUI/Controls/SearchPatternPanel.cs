using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using SilUtils;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SearchPatternPanel : Panel
	{
		public event EventHandler MouseCaptured;
		public event EventHandler InsertionMarkerMoved;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Types of patterns a panel may be.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public enum SearchPatternType
		{
			SearchItem,
			EnvironmentBefore,
			EnvironmentAfter
		}

		/// <summary>
		/// Set the InsertIndex property to this when you want to move the insertion
		/// marker to the end of the panel's pattern.
		/// </summary>
		public const int kMaxInsertIndex = -2;

		private int m_insertIndex = -1;
		private bool m_insertMode = false;
		private Point m_insertPoint = Point.Empty;
		private List<PatternLabel> m_labels = new List<PatternLabel>();
		private SearchPatternType m_patternType = SearchPatternType.SearchItem;
		private ToolTip m_tooltip = new ToolTip();
		private string m_emptyPatternTooltip;
		private int m_defaultWidth;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchPatternPanel()
		{
			AutoSize = false;
			AllowDrop = true;

			m_emptyPatternTooltip =
				string.Format(Properties.Resources.kstidEmptySearchPatternToolTip, "\n", "\n");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the panel's original width so we know what to restore it to if the user
		/// clears the contents of the panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			m_defaultWidth = Width;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the index where new items will be inserted when in insert mode.
		/// Returns -1 when not in insert mode.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int InsertIndex
		{
			get { return m_insertIndex; }
			set
			{
				if (m_insertMode && m_insertIndex != value && value <= m_labels.Count)
				{
					m_insertIndex = (value == kMaxInsertIndex ? m_labels.Count : value);
					Invalidate();
				}
			}
		}


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the panel is in the mode in which
		/// new search items may be inserted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool InsertMode
		{
			get {return m_insertMode;}
			set
			{
				if (m_insertMode != value)
				{
					m_insertMode = value;
					m_insertIndex = (value ? 0 : -1);
					Invalidate();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the point where the x coordinate is the x coordinate of the insertion marker
		/// and the y coordinate is the bottom of the panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Point InsertPoint
		{
			get {return m_insertMode ? m_insertPoint : Point.Empty;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the insertion marker can move any further
		/// to the right.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CanIncrementInsertIndex
		{
			get {return m_insertIndex < m_labels.Count;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the insertion marker can move any further
		/// to the left.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CanDecrementInsertIndex
		{
			get {return m_insertIndex > 0;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the type of the pattern panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchPatternType PatternType
		{
			get {return m_patternType;}
			set {m_patternType = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the panel contains any pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsEmpty
		{
			get {return m_labels.Count == 0;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of strings representing the human-readable pieces of the search
		/// pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<PatternLabel> PatternPieces
		{
			get {return m_labels;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a string representing how the panel's pattern is stored in the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string PatternStorageFormat
		{
			get
			{
				StringBuilder format = new StringBuilder();

				foreach (PatternLabel lbl in m_labels)
				{
					int classId = lbl.ClassId;
					format.Append(classId > 0 ? classId.ToString() : lbl.Text);
					
					// Append a comma if we're not on the last label.
					if (m_labels.IndexOf(lbl) < m_labels.Count - 1)
						format.Append(",");
				}

				return (format.Length == 0 ? null : format.ToString());
			}
		}

		#endregion

		#region Methods for inserting and verifying classes and IPA character groups
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears any existing patterns. copies the collection of specified patterns and
		/// inserts them in the panel. This method will not perform any validation. It assumes
		/// the patterns are valid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Insert(List<PatternLabel> patterns)
		{
			if (patterns == null || patterns.Count == 0)
				return;
			
			SuspendLayout();
			m_labels.Clear();
			Controls.Clear();

			foreach (PatternLabel lbl in patterns)
			{
				PatternLabel newlbl = PatternLabel.Copy(lbl);
				lbl.MouseEnter += new EventHandler(lbl_MouseEnter);
				Controls.Add(lbl);
				m_labels.Add(lbl);
			}

			ArrangeItems();
			ResumeLayout(true);
			Invalidate();			// This is necessary sometimes.
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an item to the search pattern
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Insert(string ipaText)
		{
			PatternLabel lbl = new PatternLabel(0, ClassType.NotApplicable, ipaText);
			lbl.MouseEnter += new EventHandler(lbl_MouseEnter);
			Controls.Add(lbl);
			m_labels.Insert(m_insertIndex, lbl);
			ArrangeItems();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an item to the search pattern
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Insert(int classId, SearchClassType type, string patternText)
		{
			if (VerifyClassToInsert(classId, type))
			{
				PatternLabel lbl = new PatternLabel(classId, type, patternText);
				lbl.MouseEnter += new EventHandler(lbl_MouseEnter);
				Controls.Add(lbl);
				m_labels.Insert(m_insertIndex, lbl);
				ArrangeItems();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies various things before allowing a class to be inserted in the pattern panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool VerifyClassToInsert(int classId, SearchClassType type)
		{
			if (type == ClassType.WordBoundary)
			{
				// The word boundary class is invalid for the search item.
				if (m_patternType == SearchPatternType.SearchItem)
				{
					System.Media.SystemSounds.Beep.Play();
					return false;
				}

				// Now verify that the word boundary class isn't already in the pattern.
				foreach (PatternLabel lbl in m_labels)
				{
					if (lbl.ClassType == ClassType.WordBoundary)
						return false;
				}

				// At this point, we now our class is the word boundary class and it's valid
				// to insert. But we need to make sure the index where it will be inserted is
				// correct for the environment this pattern panel is for.
				m_insertIndex = (m_patternType == SearchPatternType.EnvironmentBefore ?
						0 : m_labels.Count);
			}
			else if (type == ClassType.ZeroOneOrMore)
			{
				// The zero or more or one or more characters class cannot be inserted
				// into a pattern panel that contains anything else. Therefore, make sure
				// the panel's contents are cleared before inserting.
				Clear();
			}
			else
			{
				// At this point, we know we're inserting a normal class or group of IPA
				// characters.
				if (m_labels.Count > 0)
				{
					// Make sure the pattern doesn't contain a class of type ZeroOneOrMore
					// since those cannot coexist with any other items in a panel's pattern.
					// If we find a class of that type, then remove it.
					if (m_labels[0].ClassType == ClassType.ZeroOneOrMore)
					{
						Controls.Clear();
						m_labels.Clear();
					}

					// When inserting into the environment after at the very end, make
					// sure we don't insert after a word boundary since word boundaries in
					// the environment after always have to be the last item in the pattern.
					if (m_patternType == SearchPatternType.EnvironmentAfter && m_insertIndex == m_labels.Count)
					{
						foreach (PatternLabel lbl in m_labels)
						{
							if (lbl.ClassType == ClassType.WordBoundary)
							{
								m_insertIndex--;
								break;
							}
						}
					}
				}
			}

			return true;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void lbl_MouseEnter(object sender, EventArgs e)
		{
			// When the mouse moves over a pattern label, then we want
			// the label's owning panel to handle mouse events.
			BeginMouseCapture();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			BeginMouseCapture();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Turn capture on so any mouse moves over a pattern label are handled by the panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BeginMouseCapture()
		{
			// Don't do anything if we're already captured.
			if (Capture)
				return;

			Capture = true;
			Invalidate();

			if (MouseCaptured != null && !m_insertMode)
				MouseCaptured(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			// If the mouse has moved outside the bounds of the panel, then turn off capturing.
			if (Capture && !ClientRectangle.Contains(e.Location))
			{
				Capture = false;
				m_tooltip.SetToolTip(this, null);

				// If we're not in the insert mode and the mouse moved away from
				// the panel then redraw the panel without a selected look.
				if (!m_insertMode)
					Invalidate();
				
				return;
			}

			if (m_insertMode && m_labels.Count > 0)
				PositionInsertionMarker(e.X);

			if (!m_insertMode && m_labels.Count == 0)
			{
				//string tiptext = string.Format("Double-click, press enter on, or drag{0}to copy classes or IPA characters{1}to the search pattern", "\n", "\n");
				//m_tooltip.SetToolTip(this, tiptext);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Given the specified X coordinate of the mouse cursor, this method will determine
		/// where the desired location for the insertion marker is. If it's not at that
		/// desired location, it will set it to the desired location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void PositionInsertionMarker(int currentMouseX)
		{
			int index = 0;
			foreach (PatternLabel lbl in m_labels)
			{
				int midpoint = lbl.Left + (lbl.Width / 2);
				if (currentMouseX < midpoint)
					break;
				else
					index++;
			}

			// If we calculated an insertion index that's different from the current
			// one, then set the current insertion index to the calculated one.
			if (index != m_insertIndex)
			{
				InsertIndex = index;
				if (InsertionMarkerMoved != null)
					InsertionMarkerMoved(this, EventArgs.Empty);
			}
		}

		#region Misc. public methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears the pattern from the panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Clear()
		{
			m_labels.Clear();
			Controls.Clear();
			Width = m_defaultWidth;
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Arranges the items from left to right, and centered vertically.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ArrangeItems()
		{
			Point pt = new Point(3, 0);
			int width = 0;

			foreach (PatternLabel lbl in m_labels)
			{
				pt.Y = (ClientSize.Height - lbl.Height) / 2;
				lbl.Location = pt;
				pt.X += lbl.Width;
				width += lbl.Width;
			}

			// Account for a 3 pixel margin on each side.
			Width = width + 6;
		}

		#endregion

		#region Drawing methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Rectangle rc = ClientRectangle;

			if (m_insertMode || Capture)
			{
				PaintingHelper.DrawHotBackground(e.Graphics, rc, PaintState.Hot);
				if (m_labels.Count > 0 && m_insertMode)
					DrawInsertionMarker(e.Graphics);
			}
			else
			{
				m_insertPoint = new Point(0, rc.Bottom - 3);

				if (m_labels.Count > 0)
					e.Graphics.FillRectangle(SystemBrushes.Control, rc);
				else
				{
					// At this point, we know the panel is empty and the mouse isn't over it.
					// Therefore, fill in the background so the user can see there is something
					// here. Otherwise the panel would be invisible.
					Color clr1 = SystemColors.ControlDark;
					Color clr2 = ColorHelper.CalculateColor(Color.White, clr1, 30);
					using (LinearGradientBrush br = new LinearGradientBrush(rc, Color.White, clr2, 60))
						e.Graphics.FillRectangle(br, rc);

					// Draw a border around the label.
					ControlPaint.DrawBorder(e.Graphics, rc, SystemColors.ButtonShadow,
						ButtonBorderStyle.Solid);
				}
			}

			// Put the word "Empty" (or localized equivalent) in the panel when
			// it doesn't yet contain a search pattern.
			if (m_labels.Count == 0)
			{
				using (Font fnt = new Font(FontHelper.UIFont.Name, 8, GraphicsUnit.Point))
				using (StringFormat sf = STUtils.GetStringFormat(true))
				{
					e.Graphics.DrawString(Properties.Resources.kstidEmptySearchPatternText, fnt,
						SystemBrushes.ControlText, rc, sf);
				}

				if (!m_insertMode && m_tooltip.GetToolTip(this) != m_emptyPatternTooltip)
					m_tooltip.SetToolTip(this, m_emptyPatternTooltip);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawInsertionMarker(Graphics g)
		{
			using (Pen pen = new Pen(Color.Black))
			{
				int lineX = (m_insertIndex == m_labels.Count ?
					(m_labels[m_insertIndex - 1]).Right - 2 :
					(m_labels[m_insertIndex]).Left + 2);

				Point pt1 = new Point(lineX, 3);
				Point pt2 = new Point(pt1.X, ClientRectangle.Bottom - 3);
				m_insertPoint = pt2;

				// Draw insertion line
				g.DrawLine(pen, pt1, pt2);

				// Draw top arrow (pointing down)
				pt1 = new Point(lineX - 3, 3);
				pt2 = new Point(lineX + 3, 3);
				g.DrawLine(pen, pt1, pt2);

				for (int i = 0; i < 2; i++)
				{
					pt1.Offset(1, 1);
					pt2.Offset(-1, 1);
					g.DrawLine(pen, pt1, pt2);
				}

				// Draw bottom arrow (pointing up)
				pt1 = new Point(lineX - 3, ClientRectangle.Bottom - 3);
				pt2 = new Point(lineX + 3, pt1.Y);
				g.DrawLine(pen, pt1, pt2);

				for (int i = 0; i < 2; i++)
				{
					pt1.Offset(1, -1);
					pt2.Offset(-1, -1);
					g.DrawLine(pen, pt1, pt2);
				}
			}
		}

		#endregion
	}

	#region PatternLabel class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class encapsulating a single class or group of IPA characters within a pattern panel.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PatternLabel : Label
	{
		public int ClassId;
		public ClassType ClassType;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a new instance of a pattern label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PatternLabel(int id, ClassType type, string text) : base()
		{
			if (id == 0)
				Font = new Font(FontHelper.PhoneticFont.Name, 16, GraphicsUnit.Point);
			else if (type == ClassType.WordBoundary || type == ClassType.ZeroOneOrMore)
			{
				// Use a bigger font since there's just a single character
				// word boundary and zero/one or more characters classes.
				Font = new Font(FontHelper.PhoneticFont.Name, 16, GraphicsUnit.Point);
			}
			else
				Font = FontHelper.PhoneticFont;

			ClassId = id;
			ClassType = type;
			Text = text;
			BackColor = Color.Transparent;
			AutoSize = true;
			TextAlign = ContentAlignment.MiddleCenter;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new pattern label whose contenst are identical to the one specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PatternLabel Copy(PatternLabel source)
		{
			return new PatternLabel(source.ClassId, source.ClassType, source.Text);
		}
	}

	#endregion
}
