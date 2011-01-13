// Copyright 2006 Herre Kuijpers - <herre@xs4all.nl>
//
// This source file(s) may be redistributed, altered and custimized
// by any means PROVIDING the authors name and all copyright
// notices remain intact.
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUR OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace SilTools
{
    public partial class OutlookBar : UserControl
    {
        /// <summary>
        /// the OutlookBarButtons class contains the list of buttons
        /// it manages adding and removing buttons, and updates the Outlookbar control
        /// respectively. Note that this is a class, not a control!
        /// </summary>
        #region OutlookBarButtons list
        public class OutlookBarButtons : CollectionBase
        {
            //protected ArrayList List;
            protected OutlookBar parent;
            public OutlookBar Parent
            {
                get { return parent; }
            }

            internal OutlookBarButtons(OutlookBar parent)   : base()
            {
                this.parent = parent;
            }

            public OutlookBarButton this[int index]
            {
                get { return (OutlookBarButton)List[index]; }
            }

            public void Add(OutlookBarButton item)
            {
                if (List.Count == 0) Parent.SelectedButton = item;
                List.Add(item);
                item.Parent = this.Parent;
                Parent.ButtonlistChanged();
            }

            public OutlookBarButton Add(string text, Image image)
            {
                OutlookBarButton b = new OutlookBarButton(this.parent);
                b.Text = text;
                b.Image = image;
                this.Add(b);
                return b;
            }

            public OutlookBarButton Add(string text)
            {
                return this.Add(text, null);
            }

            public OutlookBarButton Add()
            {
                return this.Add();
            }

            public void Remove(OutlookBarButton button)
            {
                List.Remove(button);
                Parent.ButtonlistChanged();
            }

            public int IndexOf(object value)
            {
                return List.IndexOf(value);
            }

            #region handle CollectionBase events
            protected override void OnInsertComplete(int index, object value)
            {
                OutlookBarButton b = (OutlookBarButton)value;
                b.Parent = this.parent;
                Parent.ButtonlistChanged();
                base.OnInsertComplete(index, value);
            }

            protected override void OnSetComplete(int index, object oldValue, object newValue)
            {
                OutlookBarButton b = (OutlookBarButton)newValue;
                b.Parent = this.parent;
                Parent.ButtonlistChanged();
                base.OnSetComplete(index, oldValue, newValue);
            }

            protected override void OnClearComplete()
            {
                Parent.ButtonlistChanged();
                base.OnClearComplete();
            }
            #endregion handle CollectionBase events
        }
        #endregion OutlookBarButtons list

		public delegate void SelectedButtonChangedHandler(object sender, OutlookBarButton newButton);
		public event SelectedButtonChangedHandler SelectedButtonChanged;

        #region OutlookBar property definitions

        /// <summary>
        /// buttons contains the list of clickable OutlookBarButtons
        /// </summary>
        protected OutlookBarButtons buttons;

        /// <summary>
        /// this variable remembers which button is currently selected
        /// </summary>
        protected OutlookBarButton selectedButton = null;

        /// <summary>
        /// this variable remembers the button index over which the mouse is moving
        /// </summary>
        protected int hoveringButtonIndex = -1;

        /// <summary>
        /// property to set the buttonHeigt
        /// default is 30
        /// </summary>
        protected int buttonHeight;
        [Description("Specifies the height of each button on the OutlookBar"), Category("Layout")]
        public int ButtonHeight
        {
            get { return buttonHeight; }
            set {
                if (value > 18)
                    buttonHeight = value;
                else
                    buttonHeight = 18;
            }
        }
        
        protected Color gradientButtonDark = Color.FromArgb(178, 193, 140);
        [Description("Dark gradient color of the button"), Category("Appearance")]
        public Color GradientButtonNormalDark
        {
            get { return gradientButtonDark; }
            set { gradientButtonDark = value; }
        }

        protected Color gradientButtonLight = Color.FromArgb(234, 240, 207);
        [Description("Light gradient color of the button"), Category("Appearance")]
        public Color GradientButtonNormalLight
        {
            get { return gradientButtonLight; }
            set { gradientButtonLight = value; }
        }

        protected Color gradientButtonHoverDark = Color.FromArgb(247, 192, 91);
        [Description("Dark gradient color of the button when the mouse is moving over it"), Category("Appearance")]
        public Color GradientButtonHoverDark
        {
            get { return gradientButtonHoverDark; }
            set { gradientButtonHoverDark = value; }
        }

        protected Color gradientButtonHoverLight = Color.FromArgb(255, 255, 220);
        [Description("Light gradient color of the button when the mouse is moving over it"), Category("Appearance")]
        public Color GradientButtonHoverLight
        {
            get { return gradientButtonHoverLight; }
            set { gradientButtonHoverLight = value; }
        }

        protected Color gradientButtonSelectedDark = Color.FromArgb(239, 150, 21);
        [Description("Dark gradient color of the seleced button"), Category("Appearance")]
        public Color GradientButtonSelectedDark
        {
            get { return gradientButtonSelectedDark; }
            set { gradientButtonSelectedDark = value; }
        }

        protected Color gradientButtonSelectedLight = Color.FromArgb(251, 230, 148);
        [Description("Light gradient color of the seleced button"), Category("Appearance")]
        public Color GradientButtonSelectedLight
        {
            get { return gradientButtonSelectedLight; }
            set { gradientButtonSelectedLight = value; }
        }


        /// <summary>
        /// when a button is selected programatically, it must update the control
        /// and repaint the buttons
        /// </summary>
        [Browsable(false)]
        public OutlookBarButton SelectedButton
        {
            get { return selectedButton; }
            set
			{
				if (value == selectedButton)
					return;
				
				// assign new selected button
                PaintSelectedButton(selectedButton, value);

                // assign new selected button
                selectedButton = value;

				if (SelectedButtonChanged != null)
					SelectedButtonChanged(this, selectedButton);
            }
        }

        /// <summary>
        /// readonly list of buttons
        /// </summary>
        //[Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public OutlookBarButtons Buttons
        {
            get { return buttons; }
        }

        #endregion OutlookBar property definitions

        #region OutlookBar events


        [Serializable]
        public class ButtonClickEventArgs : MouseEventArgs
        {
            public ButtonClickEventArgs(OutlookBarButton button, MouseEventArgs evt) : base(evt.Button, evt.Clicks, evt.X, evt.Y, evt.Delta)
            {
                SelectedButton = button;
            }
           
            public readonly OutlookBarButton SelectedButton;
        }

        public delegate void ButtonClickEventHandler(object sender, ButtonClickEventArgs e);

        public new event ButtonClickEventHandler Click;

        #endregion OutlookBar events

        #region OutlookBar functions

        public OutlookBar()
        {
            InitializeComponent();
            buttons = new OutlookBarButtons(this);
            buttonHeight = 30; // set default to 30
        }

        private void PaintSelectedButton(OutlookBarButton prevButton,OutlookBarButton newButton)
        {
            if (prevButton == newButton)
                return; // no change so return immediately

            int selIdx = -1;
            int valIdx = -1;
            
            // find the indexes of the previous and new button
            selIdx = buttons.IndexOf(prevButton);
            valIdx = buttons.IndexOf(newButton);

            // now reset selected button
            // mouse is leaving control, so unhighlight anythign that is highlighted
            Graphics g = Graphics.FromHwnd(this.Handle);
            if (selIdx >= 0)
                // un-highlight current hovering button
                buttons[selIdx].PaintButton(g, 1, selIdx * (buttonHeight + 1) + 1, false, false);

            if (valIdx >= 0)
                // highlight newly selected button
                buttons[valIdx].PaintButton(g, 1, valIdx * (buttonHeight + 1) + 1, true, false);
            g.Dispose();
        }

        /// <summary>
        /// returns the button given the coordinates relative to the Outlookbar control
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public OutlookBarButton HitTest(int x, int y)
        {
            int index = (y - 1) / (buttonHeight + 1);
            if (index >= 0 && index < buttons.Count)
                return buttons[index];
            else
                return null;
        }

        /// <summary>
        /// this function will setup the control to cope with changes in the buttonlist 
        /// that is, addition and removal of buttons
        /// </summary>
        private void ButtonlistChanged()
        {
			int height = 0;
			foreach (OutlookBarButton btn in buttons)
				height = Math.Max(btn.Height, height);

			Height = height;
			//if (!this.DesignMode) // only set sizes automatically at runtime
			//    this.MaximumSize = new Size(0, buttons.Count * (buttonHeight + 1) + 1);

            this.Invalidate();
        }

        #endregion OutlookBar functions

        #region OutlookBar control event handlers

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			// initiate the render style flags of the control
			SetStyle(
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.ResizeRedraw |
				ControlStyles.DoubleBuffer |
				ControlStyles.Selectable |
				ControlStyles.UserMouse,
				true
				);
		}

		internal bool IsImageless
		{
			get
			{
				foreach (OutlookBarButton b in Buttons)
				{
					if (b.Image != null)
						return false;
				}

				return true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			int top = 1;
			foreach (OutlookBarButton b in Buttons)
			{
				var rc = new Rectangle(0, top - 1, Width - 1, b.Height);
				e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, rc);
				b.PaintButton(e.Graphics, 1, top, b.Equals(this.selectedButton), false);
				top += b.Height + 1;
			}

			e.Graphics.DrawLine(SystemPens.ControlDarkDark, 0, top - 1, Width, top - 1);
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);

			if (!(e is MouseEventArgs)) return;

            // case to MouseEventArgs so position and mousebutton clicked can be used
            var mea = (MouseEventArgs)e;

            // only continue if left mouse button was clicked
            if (mea.Button != MouseButtons.Left) return;
            
            int index = (mea.Y - 1) / (buttonHeight + 1);

            if (index < 0 || index >= buttons.Count)
                return;

            OutlookBarButton button = buttons[index];
            if (button == null) return;
            if (!button.Enabled) return;

            // ok, all checks passed so assign the new selected button
            // and raise the event
            SelectedButton = button;

            var bce = new ButtonClickEventArgs(selectedButton, mea);
            if (Click != null) // only invoke on left mouse click
                Click.Invoke(this, bce);
        }

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			// mouse is leaving control, so unhighlight anything that is highlighted
            if (hoveringButtonIndex >= 0)
            {
                // so we need to change the hoveringButtonIndex to the new index
                Graphics g = Graphics.FromHwnd(this.Handle);
                OutlookBarButton b1 = buttons[hoveringButtonIndex];

                // un-highlight current hovering button
                b1.PaintButton(g, 1, hoveringButtonIndex * (buttonHeight + 1) + 1, b1.Equals(selectedButton), false);
                hoveringButtonIndex = -1;
                g.Dispose();
            }
        }

        private void OutlookBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
            {
                // determine over which button the mouse is moving
                int index = (e.Location.Y - 1) / (buttonHeight + 1);
                if (index >= 0 && index < buttons.Count)
                {
                    if (hoveringButtonIndex == index )
                        return; // nothing changed so we're done, current button stays highlighted

                    // so we need to change the hoveringButtonIndex to the new index
                    Graphics g = Graphics.FromHwnd(this.Handle);

                    if (hoveringButtonIndex >= 0)
                    {
                        OutlookBarButton b1 = buttons[hoveringButtonIndex];

                        // un-highlight current hovering button
                        b1.PaintButton(g, 1, hoveringButtonIndex * (buttonHeight + 1) + 1, b1.Equals(selectedButton), false);
                    }
                    
                    // highlight new hovering button
                    OutlookBarButton b2 = buttons[index];
                    b2.PaintButton(g, 1, index * (buttonHeight + 1) + 1, b2.Equals(selectedButton), true);
                    hoveringButtonIndex = index; // set to new index
                    g.Dispose();

                }
                else
                {
                    // no hovering button, so un-highlight all.
                    if (hoveringButtonIndex >= 0)
                    {
                        // so we need to change the hoveringButtonIndex to the new index
                        Graphics g = Graphics.FromHwnd(this.Handle);
                        OutlookBarButton b1 = buttons[hoveringButtonIndex];

                        // un-highlight current hovering button
                        b1.PaintButton(g, 1, hoveringButtonIndex * (buttonHeight + 1) + 1, b1.Equals(selectedButton), false);
                        hoveringButtonIndex = -1;
                        g.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// isResizing is used as a signal, so this method is not called recusively
        /// this prevents a stack overflow
        /// </summary>
		private void OutlookBar_Resize(object sender, EventArgs e)
		{
			// only set sizes automatically at runtime
			if (DesignMode)
				return;

			Resize -= OutlookBar_Resize;
			if ((this.Height - 1) % (buttonHeight + 1) > 0)
				this.Height = ((this.Height - 1) / (buttonHeight + 1)) * (buttonHeight + 1) + 1;
			this.Invalidate();
			Resize += OutlookBar_Resize;
		}

        #endregion OutlookBar control event handlers

    }

    /// <summary>
    /// OutlookbarButton represents a button on the Outlookbar
    /// this is an internally used class (not a control!)
    /// </summary>
    #region OutlookBarButton
    public class OutlookBarButton // : IComponent
    {
        private bool enabled = true;

        [Description("Indicates wether the button is enabled"), Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        protected Image image = null;
        [Description("The image that will be displayed on the button"), Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                parent.Invalidate();
            }
        }

        protected object tag = null;
        [Description("User-defined data to be associated with the button"), Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public OutlookBarButton()
        {
            this.parent = new OutlookBar(); // set it to a dummy outlookbar control
            text = "";
        }

        public OutlookBarButton(OutlookBar parent)
        {
            this.parent = parent;
            text = "";
        }

        protected OutlookBar parent;


        internal OutlookBar Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        protected string text;
        [Description("The text that will be displayed on the button"), Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                parent.Invalidate();
            }
        }

        protected int height;
        public int Height
        {
            get { return parent == null ? 30 : parent.ButtonHeight; }

        }

        public int Width
        {
            get { return parent == null ? 60 : parent.Width - 2; }
        }

        /// <summary>
        /// the outlook button will paint itself on its container (the OutlookBar)
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="IsSelected"></param>
        /// <param name="IsHovering"></param>
		public void PaintButton(Graphics graphics, int x, int y, bool IsSelected, bool IsHovering)
		{
			Rectangle rect = new Rectangle(0, y, Width, Height);
			Brush br;
			if (enabled)
			{
				if (IsSelected)
					if (IsHovering)
						br = new LinearGradientBrush(rect, parent.GradientButtonSelectedDark, parent.GradientButtonSelectedLight, 90f);
					else
						br = new LinearGradientBrush(rect, parent.GradientButtonSelectedLight, parent.GradientButtonSelectedDark, 90f);
				else
					if (IsHovering)
						br = new LinearGradientBrush(rect, parent.GradientButtonHoverLight, parent.GradientButtonHoverDark, 90f);
					else
						br = new LinearGradientBrush(rect, parent.GradientButtonNormalLight, parent.GradientButtonNormalDark, 90f);
			}
			else
				br = new LinearGradientBrush(rect, parent.GradientButtonNormalLight, parent.GradientButtonNormalDark, 90f);

			rect = new Rectangle(x, y, Width, Height);
			graphics.FillRectangle(br, rect);
			br.Dispose();

			if (text.Length > 0)
			{
				rect.Width -= 8;
				rect.X = x + 8;

				TextFormatFlags flags = TextFormatFlags.SingleLine |
					TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;

				TextRenderer.DrawText(graphics, Text, parent.Font, rect, Color.Black, flags);
			}

			if (image != null)
			{
				graphics.DrawImage(image, 36 / 2 - image.Width / 2,
					y + Height / 2 - image.Height / 2,
					image.Width, image.Height);
			}
		}
    }
    #endregion
}
