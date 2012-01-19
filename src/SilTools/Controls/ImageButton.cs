using System;
using System.Drawing;
using System.Windows.Forms;

namespace SilTools.Controls
{
	public class ImageButton : NicerButton
	{
		protected Image _image;
		protected Image _imageHot;
		protected Size _imageMargin = new Size(2, 2);

		/// ------------------------------------------------------------------------------------
		public ImageButton()
		{
			FlatAppearance.MouseDownBackColor = Color.Transparent;
			FlatAppearance.MouseOverBackColor = Color.Transparent;
		}

		/// ------------------------------------------------------------------------------------
		public Size ImageMargin
		{
			get { return _imageMargin; }
			set { _imageMargin = value; }
		}

		/// ------------------------------------------------------------------------------------
		public new Image Image
		{
			get { return null; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Can't use the button's Image property because it makes various other handling
		/// more difficult.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Image ButtonImage
		{
			get { return _image; }
			set
			{
				if (_imageHot != null)
					_imageHot.Dispose();

				_image = value;
				_imageHot = PaintingHelper.MakeHotImage(_image);
				PerformLayout();
			}
		}

		/// ------------------------------------------------------------------------------------
		public override Size GetPreferredSize(Size proposedSize)
		{
			return (_image == null ? base.GetPreferredSize(proposedSize) :
				 new Size(_image.Width + (ImageMargin.Width * 2),
					_image.Height + (_imageMargin.Height * 2)));
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var rc = ClientRectangle;
			var img = (rc.Contains(PointToClient(MousePosition)) ? _imageHot : _image);

			if (img == null)
				return;

			var dx = _imageMargin.Width;
			
			if (Text == string.Empty || TextImageRelation == TextImageRelation.ImageBeforeText)
				dx = _imageMargin.Width;
			else
				dx = rc.Width - (_imageMargin.Width + img.Width) + 1;

			rc = new Rectangle(
				new Point(dx, (int)Math.Round((rc.Height - img.Height) / 2f, MidpointRounding.AwayFromZero)),
				img.Size);

			e.Graphics.DrawImage(img, rc);
		}
	}
}
