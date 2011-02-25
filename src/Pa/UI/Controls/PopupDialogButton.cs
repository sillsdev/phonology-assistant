using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIL.Pa.UI.Controls
{
	#region PopupDialogButton class
	/// ----------------------------------------------------------------------------------------
	public class PopupDialogButton : Button
	{
		protected Image m_imgHot;
		protected Image m_imageNormal;

		/// ------------------------------------------------------------------------------------
		public PopupDialogButton()
		{
			AutoSize = false;
			FlatAppearance.BorderSize = 0;
			FlatStyle = FlatStyle.Flat;
			Size = new Size(22, 22);
			UseVisualStyleBackColor = false;
			FlatAppearance.MouseOverBackColor = Color.Transparent;
			BackgroundImageLayout = ImageLayout.Center;

			// Use the background image instead of the Image property because when
			// using the Image property there is a lot of painting flicker as the
			// Image changes from the mouse moving over the button.
			BackgroundImage = ImageNormal;
		}

		/// ------------------------------------------------------------------------------------
		public virtual Image ImageHot
		{
			get { return (m_imgHot ?? BackgroundImage); }
			set { m_imgHot = value; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual Image ImageNormal
		{
			get { return (m_imageNormal ?? BackgroundImage); }
			set { m_imageNormal = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			BackgroundImage = ImageHot;
			base.OnMouseEnter(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			BackgroundImage = ImageNormal;
			base.OnMouseLeave(e);
		}
	}

	#endregion

	#region PopupDialogOKButton class
	/// ----------------------------------------------------------------------------------------
	public class PopupDialogOKButton : PopupDialogButton
	{
		/// ------------------------------------------------------------------------------------
		public override Image ImageHot
		{
			get { return Properties.Resources.PopupDialogButtonOK_Hot; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		public override Image ImageNormal
		{
			get { return Properties.Resources.PopupDialogButtonOK; }
			set { }
		}
	}

	#endregion

	#region PopupDialogCancelButton class
	/// ----------------------------------------------------------------------------------------
	public class PopupDialogCancelButton : PopupDialogButton
	{
		/// ------------------------------------------------------------------------------------
		public override Image ImageHot
		{
			get { return Properties.Resources.PopupDialogButtonCancel_Hot; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		public override Image ImageNormal
		{
			get { return Properties.Resources.PopupDialogButtonCancel; }
			set { }
		}
	}

	#endregion

	#region PopupDialogCloseButton class
	/// ----------------------------------------------------------------------------------------
	public class PopupDialogCloseButton : PopupDialogButton
	{
		/// ------------------------------------------------------------------------------------
		public override Image ImageHot
		{
			get { return Properties.Resources.PopupDialogButtonClose_Hot; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		public override Image ImageNormal
		{
			get { return Properties.Resources.PopupDialogButtonClose; }
			set { }
		}
	}

	#endregion

	#region PopupDialogHelpButton class
	/// ----------------------------------------------------------------------------------------
	public class PopupDialogHelpButton : PopupDialogButton
	{
		/// ------------------------------------------------------------------------------------
		public override Image ImageHot
		{
			get { return Properties.Resources.PopupDialogButtonHelp_Hot; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		public override Image ImageNormal
		{
			get { return Properties.Resources.PopupDialogButtonHelp; }
			set { }
		}
	}

	#endregion
}
