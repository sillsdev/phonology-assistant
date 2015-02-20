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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a single item within a SimpleExplorerBar control.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ExplorerBarItem : Panel
	{
		public event EventHandler Collapsed;
		public event EventHandler Expanded;

		public delegate void ExplorerBarItemCheckBoxCheckedHandler(bool checkBoxCheckedd, Control hostedControl);
		public event ExplorerBarItemCheckBoxCheckedHandler CheckBoxChecked;

		private int _controlsExpandedHeight;
		private int _buttonVerticalPadding = 8;
		private bool _drawHot;
		private bool _expanded = true;
		private bool _gradientButton = true;
		private bool _canCollapse = true;
		private int _glyphButtonWidth;

		/// ------------------------------------------------------------------------------------
		public ExplorerBarItem()
		{
			ButtonBackColor = Color.Empty;
			Button = new ExpBarButton();
			Button.Dock = DockStyle.Top;
			Button.Font = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold);
			Button.TextChanged += HandleButtonTextChanged;
			//m_button.Text = Utils.ConvertLiteralNewLines(text);
			//string[] lines = m_button.Text.Split('\n');
			//m_button.Height = 13 + (m_button.Font.Height * lines.Length);
			Button.Cursor = Cursors.Hand;
			Button.Click += HandleButtonClick;
			Button.Paint += HandleButtonPaint;
			Button.MouseEnter += delegate { _drawHot = true; Button.Invalidate(); };
			Button.MouseLeave += delegate { _drawHot = false; Button.Invalidate(); };
			Controls.Add(Button);
			
			CheckBox = new ExpBarCheckBox(Button) { Visible = false };
			CheckBox.CheckedChanged += delegate
			{
				if (CheckBoxChecked != null)
					CheckBoxChecked(CheckBox.Checked, Control);
			};
		}

		/// ------------------------------------------------------------------------------------
		public ExplorerBarItem(string text, Control hostedControl) : this()
		{
			Button.Text = text;
			SetHostedControl(hostedControl);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && !Button.IsDisposed)
			{
				Button.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public void SetHostedControl(Control hostedControl)
		{
			Control = hostedControl;
			Control.Dock = DockStyle.Fill;
			Controls.Add(Control);
			Control.BringToFront();
			SetHostedControlHeight(Control.Height);
		}

		/// ------------------------------------------------------------------------------------
		public void SetHostedControlHeight(int height)
		{
			_controlsExpandedHeight = height;

			if (IsExpanded)
				Height = Button.Height + height + Control.Margin.Top + Control.Margin.Bottom;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public Control Control { get; private set; }

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public ExpBarCheckBox CheckBox { get; private set; }

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public ExpBarButton Button { get; private set; }

		/// ------------------------------------------------------------------------------------
		public Color ButtonBackColor { get; set; }

		/// ------------------------------------------------------------------------------------
		[DefaultValue(true)]
		public bool CanButtonGetFocus
		{
			get { return Button.CanGetFocus; }
			set { Button.CanGetFocus = value; }
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(true)]
		public bool ShowButtonFocusCues
		{
			get { return Button.GetShowFocusCues(); }
			set
			{
				Button.SetShowFocusCues(value);
				Button.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get {return base.BackColor;}
			set
			{
				base.BackColor = value;
				if (ButtonBackColor == Color.Empty)
					ButtonBackColor = ColorHelper.CalculateColor(Color.Black, SystemColors.Window, 25);

				Button.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the item's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		public override string Text
		{
			get { return Button.Text; }
			set
			{
				Button.Text = value;
				Button.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the font used to display the item's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Font Font
		{
			get { return Button.Font; }
			set
			{
				Button.Font = value;
				Button.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public CheckState CheckedBoxState
		{
			get { return CheckBox.CheckState; }
			set
			{
				// Setting this value causes the check box's CheckedChanged to fire and
				// we don't want that to be passed on to delegates. Therefore, unook
				// delegates and rehook them after changing the state.
				var savedCheckBoxCheckedDelegate = CheckBoxChecked;
				CheckBoxChecked = null;
				CheckBox.CheckState = value;
				CheckBoxChecked = savedCheckBoxCheckedDelegate;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the item is expanded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public bool IsExpanded
		{
			get { return _expanded; }
			set
			{
				if (_expanded != value)
					HandleButtonClick(null, null);
			}
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(true)]
		public bool CanCollapse
		{
			get { return _canCollapse; }
			set
			{
				if (!value && !_expanded)
					Button.PerformClick();

				_canCollapse = value;
				Button.Cursor = (value ? Cursors.Hand : Cursors.Default);
				Button.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(true)]
		public bool GradientButton
		{
			get { return _gradientButton; }
			set
			{
				_gradientButton = value;
				Button.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(false)]
		public bool ShowCheckBox
		{
			get { return CheckBox.Visible; }
			set
			{
				CheckBox.Visible = value;
				Button.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(8)]
		public int ButtonVerticalPadding
		{
			get { return _buttonVerticalPadding; }
			set 
			{
				_buttonVerticalPadding = value;
				HandleButtonTextChanged(null, null);
			}
		}

		#endregion

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the TextChanged event of the m_button control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleButtonTextChanged(object sender, EventArgs e)
		{
			if (App.DesignMode)
				return;

			// Make the expand/collapse glyph width the height of
			// one line of button text plus the fudge factor.
			_glyphButtonWidth = _buttonVerticalPadding + Button.Font.Height;

			var lines = Button.Text.Split('\n');
			Button.Height = _buttonVerticalPadding + (Button.Font.Height * lines.Length);
			
			if (Control != null)
				SetHostedControlHeight(Control.Height);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Toggle item's expanded state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleButtonClick(object sender, EventArgs e)
		{
			if (App.DesignMode || !CanCollapse)
				return;

			_expanded = !_expanded;
			Control.Visible = _expanded;
			Height = Button.Height + (Control.Visible ? _controlsExpandedHeight : 0);

			if (Control.Visible && Expanded != null)
				Expanded(this, EventArgs.Empty);
			else if (!Control.Visible && Collapsed != null)
				Collapsed(this, EventArgs.Empty);

			// Force the expand/collase glyph to be repainted.
			var rc = Button.ClientRectangle;
			rc.X = rc.Right - rc.Height + 2;
			rc.Width = rc.Height + 2;
			Button.Invalidate(rc);
		}

		#endregion

		#region Painting methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// We don't want a typical looking button. Therefore, draw it ourselves.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleButtonPaint(object sender, PaintEventArgs e)
		{
			DrawButtonBackground(e.Graphics);

			if (_canCollapse)
				DrawExpandCollapseGlyph(e.Graphics);

			DrawButtonText(e.Graphics);

			var rc = Button.ClientRectangle;
			
			//// Draw a line separating the button area from what collapses and expands below it.
			//var clr1 = ColorHelper.CalculateColor(Color.White, SystemColors.MenuHighlight, 90);
			//var pt1 = new Point(rc.X + 1, rc.Bottom - 3);
			//var pt2 = new Point(rc.Right, rc.Bottom - 3);
			//using (var br = new LinearGradientBrush(pt1, pt2, clr1, SystemColors.Window))
			//    e.Graphics.DrawLine(new Pen(br, 1), pt1, pt2);

			rc.Inflate(-1, -1);
			if (Button.Focused && Button.GetShowFocusCues())
				ControlPaint.DrawFocusRectangle(e.Graphics, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawButtonText(Graphics g)
		{
			var rc = Button.ClientRectangle;

			if (!ShowCheckBox)
				rc.Inflate(-2, 0);
			else
			{
				rc.X = CheckBox.Right + 4;
				rc.Width -= rc.X;
			}
			
			TextRenderer.DrawText(g, Button.Text, Button.Font, rc, SystemColors.WindowText,
				TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawButtonBackground(Graphics g)
		{
			var rc = Button.ClientRectangle;

			if (!_gradientButton || ButtonBackColor == Color.Empty || ButtonBackColor == Color.Transparent)
			{
				using (var br = new SolidBrush(BackColor))
					g.FillRectangle(br, rc);
			}
			else
			{
				PaintingHelper.DrawGradientBackground(g, rc);
				//using (var br = new LinearGradientBrush(rc, BackColor, ButtonBackColor, 91f))
				//    g.FillRectangle(br, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the expand or collapse glyph. The glyph drawn depends on the visible state
		/// of the hosted control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawExpandCollapseGlyph(Graphics g)
		{
			// Determine the rectangle in which the expanding/collapsing button will be drawn.
			var rc = new Rectangle(0, 0, _glyphButtonWidth, Button.Height);
			if (RightToLeft == RightToLeft.No)
				rc.X = (Button.ClientRectangle.Right - rc.Width);

			VisualStyleElement element;

			if (_drawHot)
			{
				element = (Control != null && Control.Visible ?
					VisualStyleElement.ExplorerBar.NormalGroupCollapse.Hot :
					VisualStyleElement.ExplorerBar.NormalGroupExpand.Hot);
			}
			else
			{
				element = (Control != null && Control.Visible ?
					VisualStyleElement.ExplorerBar.NormalGroupCollapse.Normal :
					VisualStyleElement.ExplorerBar.NormalGroupExpand.Normal);
			}

			if (PaintingHelper.CanPaintVisualStyle(element))
			{
				VisualStyleRenderer renderer = new VisualStyleRenderer(element);
				renderer.DrawBackground(g, rc);
			}
			else
			{
				var glyph = (_expanded ? Properties.Resources.kimidExplorerBarCollapseGlyph :
					Properties.Resources.kimidExplorerBarEpandGlyph);

				if (RightToLeft == RightToLeft.No)
					rc.X = rc.Right - (glyph.Width + 1);

				rc.Y += (Button.Height - glyph.Height) / 2;
				rc.Width = glyph.Width;
				rc.Height = glyph.Height;
				g.DrawImage(glyph, rc);
			}
		}

		#endregion

		#region ExpBarCheckBox class
		/// ------------------------------------------------------------------------------------
		public class ExpBarButton : Button
		{
			private bool _showFocusCues = true;
			private bool _canGetFocus = true;

			public bool CanGetFocus
			{
				get { return _canGetFocus; }
				set
				{
					_canGetFocus = value;
					SetStyle(ControlStyles.Selectable, _canGetFocus);
				}
			}

			public void SetShowFocusCues(bool show)
			{
				_showFocusCues = show;
			}

			public bool GetShowFocusCues()
			{
				return _showFocusCues;
			}

			protected override bool ShowFocusCues
			{
				get { return _showFocusCues; }
			}
		}

		#endregion

		#region ExpBarCheckBox class
		/// ------------------------------------------------------------------------------------
		public class ExpBarCheckBox : CheckBox
		{
			public ExpBarCheckBox(Button button)
			{
				Anchor = AnchorStyles.Left;
				BackColor = Color.Transparent;
				CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
				AutoSize = true;
				TabStop = false;
				Cursor = Cursors.Default;

				button.Controls.Add(this);
				BringToFront();

				button.SizeChanged += delegate
				{
					var dy = (int)Math.Round((button.Height - Height) / 2f, MidpointRounding.AwayFromZero);
					Location = new Point(dy, dy);
				};
			}

			protected override bool ShowFocusCues
			{
				get { return false; }
			}
		}

		#endregion
	}
}
