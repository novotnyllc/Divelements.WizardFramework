using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Divelements.WizardFramework.Rendering
{
	/// <summary>
	/// Provides commonly used drawing functionality to renderers.
	/// </summary>
	public class DrawingMethods
	{
		private DrawingMethods()
		{
		}

		/// <summary>
		/// A structure representing the Hue, Saturation and Value of a color.
		/// </summary>
		internal struct Hsv
		{
			/// <summary>
			/// The hue of the color.
			/// </summary>
			public int Hue;
			/// <summary>
			/// The saturation of the color.
			/// </summary>
			public int Saturation;
			/// <summary>
			/// The value of the color.
			/// </summary>
			public int Value;

			/// <summary>
			/// Initializes a new instance of the Hsv structure.
			/// </summary>
			/// <param name="hue">The initial value of the Hue field.</param>
			/// <param name="saturation">The initial value of the Saturation field.</param>
			/// <param name="value">The initial value of the Value field.</param>
			public Hsv(int hue, int saturation, int value)
			{
				this.Hue = hue;
				this.Saturation = saturation;
				this.Value = value;
			}
		}

		internal static Hsv ColorToHsv(Color color)
		{
			double min, max, delta;
			double r = color.R / 255.0;
			double g = color.G / 255.0;
			double b = color.B / 255.0;
			double h, s, v;

			min = Math.Min(Math.Min(r, g), b);
			max = Math.Max(Math.Max(r, g), b);
			v = max;
			delta = max - min;

			if (max == 0 || delta == 0)
			{
				// Shade of gray
				s = 0;
				h = 0;
			}
			else
			{
				s = delta / max;
				if (r == max)
				{
					// Between Yellow and Magenta
					h = (g - b) / delta;
				}
				else if (g == max)
				{
					// Between Cyan and Yellow
					h = 2 + (b - r) / delta;
				}
				else
				{
					// Between Magenta and Cyan
					h = 4 + (r - g) / delta;
				}
			}

			// Correct hue value
			h *= 60;
			if (h < 0)
				h += 360;

			return new Hsv((int)(h / 360 * 255), (int)(s * 255), (int)(v * 255));
		}

		internal static Color HsvToColor(Hsv HSV)
		{
			double h, s, v;
			double r = 0, g = 0, b = 0;

			// Perform scaling
			h = ((double)HSV.Hue / 255 * 360) % 360;
			s = (double)HSV.Saturation / 255;
			v = (double)HSV.Value / 255;

			if (s == 0)
			{
				// If s is 0, all colors are the same.
				// This is some flavor of gray.
				r = v;
				g = v;
				b = v;
			}
			else
			{
				double p, q, t;
				double fractionalSector;
				int sectorNumber;
				double sectorPos;

				// Calculate which sector of the color wheel we are in
				sectorPos = h / 60;
				sectorNumber = (int)(Math.Floor(sectorPos));

				// Get the fractional part of the sector
				fractionalSector = sectorPos - sectorNumber;

				// Calculate values for the three axes of the color
				p = v * (1 - s);
				q = v * (1 - (s * fractionalSector));
				t = v * (1 - (s * (1 - fractionalSector)));

				// Assign the fractional colors to r, g, and b
				// based on the sector the angle is in.
				switch (sectorNumber)
				{
					case 0:
						r = v;
						g = t;
						b = p;
						break;
					case 1:
						r = q;
						g = v;
						b = p;
						break;
					case 2:
						r = p;
						g = v;
						b = t;
						break;
					case 3:
						r = p;
						g = q;
						b = v;
						break;
					case 4:
						r = t;
						g = p;
						b = v;
						break;
					case 5:
						r = v;
						g = p;
						b = q;
						break;
				}
			}

			return Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
		}

		internal static void DrawThemeText(Graphics graphics, Rectangle bounds, string text, Font font, Color color, bool glow)
		{
			const int DTT_COMPOSITED = 8192;
			const int DTT_GLOWSIZE = 2048;
			const int DTT_TEXTCOLOR = 1;

			Win32.RECT r = new Win32.RECT(0, 0, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);
			IntPtr primaryHdc = graphics.GetHdc();
			IntPtr memoryHdc = Win32.CreateCompatibleDC(primaryHdc);

			// Create and select DIB section
			Win32.BITMAPINFO info = new Win32.BITMAPINFO();
			info.biSize = Marshal.SizeOf(info);
			info.biWidth = r.Right - r.Left;
			info.biHeight = -(r.Bottom - r.Top);
			info.biPlanes = 1;
			info.biBitCount = 32;
			info.biCompression = 0; // BI_RGB
			IntPtr dib = Win32.CreateDIBSection(primaryHdc, info, 0, IntPtr.Zero, IntPtr.Zero, 0);
			Win32.SelectObject(memoryHdc, dib);

			// Create and select font
			IntPtr fontHandle = font.ToHfont();
			Win32.SelectObject(memoryHdc, fontHandle);

			// Draw glowing text
#if NET20
			System.Windows.Forms.VisualStyles.VisualStyleRenderer renderer = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.Window.Caption.Active);
#else
			IntPtr theme = Win32.OpenThemeData(IntPtr.Zero, "WINDOW");
#endif
			Win32.DTTOPTS dttOpts = new Win32.DTTOPTS();
			dttOpts.dwSize = Marshal.SizeOf(typeof(Win32.DTTOPTS));
			dttOpts.dwFlags = DTT_COMPOSITED;
			if (color != Color.Transparent)
				dttOpts.dwFlags |= DTT_TEXTCOLOR;
			if (glow)
				dttOpts.dwFlags |= DTT_GLOWSIZE;
			dttOpts.crText = ColorTranslator.ToWin32(color);
			dttOpts.iGlowSize = 8;
#if NET20
			Win32.DrawThemeTextEx(renderer.Handle, memoryHdc, 0, 0, text, -1, 2084, ref r, ref dttOpts); //(int)(TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix)
#else
			Win32.DrawThemeTextEx(theme, memoryHdc, 0, 0, text, -1, 2084, ref r, ref dttOpts); //(int)(TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix)
#endif

			// Copy to foreground
			Win32.BitBlt(primaryHdc, bounds.Left, bounds.Top, bounds.Width, bounds.Height, memoryHdc, 0, 0, 0x00CC0020);

			// Clean up
			Win32.DeleteObject(fontHandle);
			Win32.DeleteObject(dib);
			Win32.DeleteDC(memoryHdc);
#if !NET20
			Win32.CloseThemeData(theme);
#endif

			graphics.ReleaseHdc(primaryHdc);
		}

		//internal static Bitmap CreateBlurBitmap(Bitmap originalUnadjusted, int distance)
		//{
		//    int oldWidth = originalUnadjusted.Width, oldHeight = originalUnadjusted.Height;
		//    int tempWidth = oldWidth + distance * 4, tempHeight = oldHeight + distance * 4;
		//    int newWidth = oldWidth + distance * 2, newHeight = oldHeight + distance * 2;

		//    using (Bitmap original = new Bitmap(tempWidth, tempHeight, PixelFormat.Format32bppArgb))
		//    {
		//        // Paint the original image onto our "new" original image with enough padding for the blur filter
		//        using (Graphics graphics = Graphics.FromImage(original))
		//            graphics.DrawImage(originalUnadjusted, new Rectangle(distance * 2, distance * 2, oldWidth, oldHeight));

		//        // Create new bitmap and get its data
		//        Bitmap newBitmap = new Bitmap(newWidth, newHeight);
		//        BitmapData newData = newBitmap.LockBits(new Rectangle(0, 0, newBitmap.Width, newBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
		//        byte[] newPixels = new byte[newWidth * newHeight * 4];

		//        // Get the data from the old bitmap
		//        BitmapData oldData = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
		//        byte[] oldPixels = new byte[tempWidth * tempHeight * 4];
		//        Marshal.Copy(oldData.Scan0, oldPixels, 0, oldPixels.Length);

		//        // Horizontal blur
		//        for (int newX = 0; newX < newWidth; newX++)
		//        {
		//            for (int newY = 0; newY < newHeight; newY++)
		//            {
		//                int originalBaseY = newY + distance;
		//                int a = 0;

		//                for (int i = 0; i < distance * 2 + 1; i++)
		//                {
		//                    int originalX = newX + i;
		//                    int originalIndex = (originalBaseY * tempWidth + originalX) * 4;
		//                    a += oldPixels[originalIndex + 3];// *weights[i];
		//                }

		//                a /= (distance * 2 + 1);
		//                //a *= 2;

		//                int newIndex = (newY * newBitmap.Width + newX) * 4;
		//                newPixels[newIndex] = 255;
		//                newPixels[newIndex + 1] = 255;
		//                newPixels[newIndex + 2] = 255;
		//                newPixels[newIndex + 3] = (byte)Math.Min(255, a);
		//            }
		//        }

		//        // Copy our new pixel data to the old bitmap
		//        for (int x = 0; x < newBitmap.Width; x++)
		//        {
		//            for (int y = 0; y < newBitmap.Height; y++)
		//            {
		//                int newIndex = (y * newBitmap.Width + x) * 4;
		//                int oldIndex = ((y + distance) * original.Width + (x + distance)) * 4;
		//                oldPixels[oldIndex] = newPixels[newIndex];
		//                oldPixels[oldIndex + 1] = newPixels[newIndex + 1];
		//                oldPixels[oldIndex + 2] = newPixels[newIndex + 2];
		//                oldPixels[oldIndex + 3] = newPixels[newIndex + 3];
		//            }
		//        }

		//        // Vertical blur
		//        for (int newX = 0; newX < newWidth; newX++)
		//        {
		//            for (int newY = 0; newY < newHeight; newY++)
		//            {
		//                int originalBaseX = newX + distance;
		//                int a = 0;

		//                for (int i = 0; i < distance * 2 + 1; i++)
		//                {
		//                    int originalY = newY + i;
		//                    int originalIndex = (originalY * tempWidth + originalBaseX) * 4;
		//                    a += oldPixels[originalIndex + 3]; // *weights[i];
		//                }

		//                a /= (distance * 2 + 1);
		//                //a *= 2;

		//                int newIndex = (newY * newBitmap.Width + newX) * 4;
		//                newPixels[newIndex] = 255;
		//                newPixels[newIndex + 1] = 255;
		//                newPixels[newIndex + 2] = 255;
		//                newPixels[newIndex + 3] = (byte)Math.Min(255, a);
		//            }
		//        }

		//        // Clean up
		//        Marshal.Copy(newPixels, 0, newData.Scan0, newPixels.Length);
		//        newBitmap.UnlockBits(newData);
		//        original.UnlockBits(oldData);

		//        return newBitmap;
		//    }
		//}

		//internal static void DrawRoundedRectangle(Graphics graphics, Rectangle bounds, int radius, Pen pen)
		//{
		//    using (GraphicsPath path = new GraphicsPath())
		//    {
		//        path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
		//        path.AddArc(bounds.X + bounds.Width - radius, bounds.Y, radius, radius, 270, 90);
		//        path.AddArc(bounds.X + bounds.Width - radius, bounds.Y + bounds.Height - radius, radius, radius, 0, 90);
		//        path.AddArc(bounds.X, bounds.Y + bounds.Height - radius, radius, radius, 90, 90);
		//        path.AddLine(bounds.X, bounds.Y + bounds.Height - radius, bounds.X, bounds.Y + radius / 2);

		//        graphics.DrawPath(pen, path);
		//    }
		//}

		//internal static void FillRoundedRectangle(Graphics graphics, Rectangle bounds, int radius, Brush brush)
		//{
		//    using (GraphicsPath path = new GraphicsPath())
		//    {
		//        path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
		//        path.AddArc(bounds.X + bounds.Width - radius, bounds.Y, radius, radius, 270, 90);
		//        path.AddArc(bounds.X + bounds.Width - radius, bounds.Y + bounds.Height - radius, radius, radius, 0, 90);
		//        path.AddArc(bounds.X, bounds.Y + bounds.Height - radius, radius, radius, 90, 90);
		//        path.AddLine(bounds.X, bounds.Y + bounds.Height - radius, bounds.X, bounds.Y + radius / 2);

		//        graphics.FillPath(brush, path);
		//    }
		//}

		///// <summary>
		///// Adjusts the brightness of a color.
		///// </summary>
		///// <param name="baseColor">The color to start with.</param>
		///// <param name="factor">The factor, from -255 to 255, to adjust the color by.</param>
		///// <returns>The new color.</returns>
		//public static Color AdjustColorBrightness(Color baseColor, int factor)
		//{
		//    Hsv hsv = ColorToHsv(baseColor);
		//    hsv.Value += factor;
		//    hsv.Value = Math.Min(hsv.Value, 255);
		//    hsv.Value = Math.Max(hsv.Value, 0);

		//    return HsvToColor(hsv);
		//}

		/// <summary>
		/// Mixes two colours together to form a new colour.
		/// </summary>
		/// <param name="color1">The first source colour.</param>
		/// <param name="color2">The second source colour.</param>
		/// <param name="percentage">A value that lies between 0 and 1, 0 being the first colour and 1 being the second.</param>
		/// <returns>The new colour produced after mixing.</returns>
		public static Color InterpolateColors(Color color1, Color color2, float percentage)
		{
			int r1, g1, b1, a1, r2, g2, b2, a2;
			byte r3, g3, b3, a3;

			r1 = color1.R;
			g1 = color1.G;
			b1 = color1.B;
			a1 = color1.A;
			r2 = color2.R;
			g2 = color2.G;
			b2 = color2.B;
			a2 = color2.A;

			r3 = Convert.ToByte(r1 + ((r2 - r1) * percentage));
			g3 = Convert.ToByte(g1 + ((g2 - g1) * percentage));
			b3 = Convert.ToByte(b1 + ((b2 - b1) * percentage));
			a3 = Convert.ToByte(a1 + ((a2 - a1) * percentage));

			return Color.FromArgb(a3, r3, g3, b3);
		}

		///// <summary>
		///// Draws a faded shadow around a rectangle.
		///// </summary>
		///// <param name="graphics">The Graphics object to draw with.</param>
		///// <param name="bounds">The bounds that represents the rectangle that will get a shadow.</param>
		///// <param name="size">The size, in pixels, of the shadow.</param>
		///// <param name="shadowColor">The color of the shadow.</param>
		///// <remarks>
		///// <para>This method is optimized for small shadows. It is not suitable for drawing larger shadows as the corners will stop looking rounded.</para>
		///// </remarks>
		//public static void DrawDropShadow(Graphics graphics, Rectangle bounds, int size, Color shadowColor)
		//{
		//    // Abort if no dimensions
		//    if (bounds.Width <= 0 || bounds.Height <= 0)
		//        return;

		//    // Draw corners
		//    using (Bitmap bitmap = CreateShadowCornerBitmap(size, shadowColor))
		//    {
		//        graphics.DrawImage(bitmap, bounds.X - size, bounds.Y - size);
		//        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
		//        graphics.DrawImage(bitmap, bounds.Right, bounds.Y - size);
		//        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
		//        graphics.DrawImage(bitmap, bounds.Right, bounds.Bottom);
		//        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
		//        graphics.DrawImage(bitmap, bounds.X - size, bounds.Bottom);
		//    }

		//    // Draw edges
		//    Rectangle horizontalEdgeBounds = new Rectangle(bounds.X, bounds.Y - size, bounds.Width, size);
		//    using (LinearGradientBrush brush = new LinearGradientBrush(horizontalEdgeBounds, Color.Transparent, shadowColor, LinearGradientMode.Vertical))
		//        graphics.FillRectangle(brush, horizontalEdgeBounds);
		//    horizontalEdgeBounds.Offset(0, bounds.Height + size);
		//    using (LinearGradientBrush brush = new LinearGradientBrush(horizontalEdgeBounds, shadowColor, Color.Transparent, LinearGradientMode.Vertical))
		//        graphics.FillRectangle(brush, horizontalEdgeBounds);
		//    Rectangle verticalEdgeBounds = new Rectangle(bounds.X - size, bounds.Y, size, bounds.Height);
		//    using (LinearGradientBrush brush = new LinearGradientBrush(verticalEdgeBounds, Color.Transparent, shadowColor, LinearGradientMode.Horizontal))
		//        graphics.FillRectangle(brush, verticalEdgeBounds);
		//    verticalEdgeBounds.Offset(bounds.Width + size, 0);
		//    using (LinearGradientBrush brush = new LinearGradientBrush(verticalEdgeBounds, shadowColor, Color.Transparent, LinearGradientMode.Horizontal))
		//        graphics.FillRectangle(brush, verticalEdgeBounds);
		//}

		//private static Bitmap CreateShadowCornerBitmap(int size, Color shadowColor)
		//{
		//    Bitmap bitmap = new Bitmap(size, size);

		//    // Create a pathgradientbrush to fill the ellipse
		//    using (Graphics graphics = Graphics.FromImage(bitmap))
		//    {
		//        using (GraphicsPath path = new GraphicsPath())
		//        {
		//            path.AddEllipse(new Rectangle(0, 0, size * 2, size * 2));
		//            using (PathGradientBrush brush = new PathGradientBrush(path))
		//            {
		//                brush.CenterColor = shadowColor;
		//                brush.SurroundColors = new Color[] { Color.Transparent };
		//                graphics.FillRectangle(brush, new Rectangle(0, 0, size, size));
		//            }
		//        }
		//    }

		//    return bitmap;
		//}

	}
}
