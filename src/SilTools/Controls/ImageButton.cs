using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SilTools.Controls
{
	public class ImageButton : Button
	{
		protected Image _image;
		protected Image _imageHot;

		/// ------------------------------------------------------------------------------------
		public ImageButton()
		{
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			FlatStyle = FlatStyle.Flat;
			FlatAppearance.BorderSize = 0;
			FlatAppearance.MouseDownBackColor = Color.Transparent;
			FlatAppearance.MouseOverBackColor = Color.Transparent;
			BackColor = Color.Transparent;
		}

		/// ------------------------------------------------------------------------------------
		public new Image Image
		{
			get { return null; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		public Image ButtonImage
		{
			get { return _image; }
			set
			{
				if (_imageHot != null)
					_imageHot.Dispose();

				_image = value;
				_imageHot = MakeHotImage(_image);
				PerformLayout();
			}
		}

		/// ------------------------------------------------------------------------------------
		public override Size GetPreferredSize(Size proposedSize)
		{
			return (_image == null ?
				base.GetPreferredSize(proposedSize) :
				new Size(_image.Width + 4, _image.Height + 4));
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ShowFocusCues
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var rc = ClientRectangle;

			if (Focused)
				ControlPaint.DrawFocusRectangle(e.Graphics, rc);

			var img = (rc.Contains(PointToClient(MousePosition)) ? _imageHot : _image);

			if (img == null)
				return;

			rc = new Rectangle(
				new Point((int)Math.Round((rc.Width - img.Width) / 2f, MidpointRounding.AwayFromZero),
					(int)Math.Round((rc.Height - img.Height) / 2f, MidpointRounding.AwayFromZero)), img.Size);

			e.Graphics.DrawImage(img, rc);
		}

		/// ------------------------------------------------------------------------------------
		private Image MakeHotImage(Image img)
		{
			if (img == null)
				return null;

			float[][] colorMatrixElements =
				{
					new[] {0.6f, 0, 0, 0, 0},
					new[] {0, 0.6f, 0, 0, 0},
					new[] {0, 0, 0.6f, 0, 0},
					new[] {0, 0, 0, 1f, 0},
					new[] {0.1f, 0.1f, 0.1f, 0, 1}
				};

			img = img.Clone() as Image;

			using (var imgattr = new ImageAttributes())
			using (var g = Graphics.FromImage(img))
			{
				var cm = new ColorMatrix(colorMatrixElements);
				var rc = new Rectangle(0, 0, img.Width, img.Height);
				imgattr.SetColorMatrix(cm);
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.DrawImage(img, rc, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgattr);
				return img;
			}
		}
	}
}
