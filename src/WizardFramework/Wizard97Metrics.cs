using System;
using System.Text;
using System.Drawing;

namespace Divelements.WizardFramework
{
	internal class Wizard97Metrics
	{
		// Layout constants
		private const int MARGINSIZE96DPI = 164, BANNERBITMAPSIZE96DPI = 49, GUTTERSIZE96DPI = 45, HEADERSIZE96DPI = 58, LAYOUTMARGINSIZE96DPI = 23;
		private static bool calculatedLayout;
		private static Size dpi, buttonSize;
		private static double pixelMultiplier;

		// Calculated layout
		private static int coverMarginSize, bannerBitmapSize, gutterSize, headerSize, layoutMarginSize;

		private Wizard97Metrics()
		{
		}

		private static Size Dpi
		{
			get
			{
				if (dpi == Size.Empty)
				{
					const int LOGPIXELSX = 88;
					IntPtr hdc = Win32.GetDC(IntPtr.Zero);
					int dpiX = Win32.GetDeviceCaps(hdc, LOGPIXELSX);
					dpi = new Size(dpiX, 0);
					pixelMultiplier = dpi.Width / 96.0;
				}

				return dpi;
			}
		}

		public static int GetScaledPixelSize(int size96dpi)
		{
			return Convert.ToInt32(size96dpi * pixelMultiplier);
		}

		private static void CalculateLayout()
		{
			coverMarginSize = Convert.ToInt32(MARGINSIZE96DPI * Dpi.Width / 96.0);
			bannerBitmapSize = Convert.ToInt32(BANNERBITMAPSIZE96DPI * Dpi.Width / 96.0);
			gutterSize = Convert.ToInt32(GUTTERSIZE96DPI * Dpi.Width / 96.0);
			headerSize = Convert.ToInt32(HEADERSIZE96DPI * Dpi.Width / 96.0);
			buttonSize = new Size(Convert.ToInt32(75 * Dpi.Width / 96.0), Convert.ToInt32(23 * Dpi.Width / 96.0));
			layoutMarginSize = GetScaledPixelSize(LAYOUTMARGINSIZE96DPI);

			calculatedLayout = true;
		}

		public static int LayoutMarginSize
		{
			get
			{
				if (!calculatedLayout)
					CalculateLayout();
				return layoutMarginSize;
			}
		}

		public static Size ButtonSize
		{
			get
			{
				if (!calculatedLayout)
					CalculateLayout();
				return buttonSize;
			}
		}

		public static int HeaderSize
		{
			get
			{
				if (!calculatedLayout)
					CalculateLayout();
				return headerSize;
			}
		}

		public static int HeaderSizeWithDivider
		{
			get { return HeaderSize + 2; }
		}

		public static int GutterSize
		{
			get
			{
				if (!calculatedLayout)
					CalculateLayout();
				return gutterSize;
			}
		}

		public static int GutterSizeWithDivider
		{
			get { return GutterSize + 2; }
		}

		public static int CoverMarginSize
		{
			get
			{
				if (!calculatedLayout)
					CalculateLayout();
				return coverMarginSize;
			}
		}

		public static int BannerBitmapSize
		{
			get
			{
				if (!calculatedLayout)
					CalculateLayout();
				return bannerBitmapSize;
			}
		}

	}
}
