using System;
using System.Text;
using System.Drawing;

namespace Divelements.WizardFramework
{
	internal class WizardAeroMetrics
	{
		// Layout constants
		private const int TITLEBARSIZE96DPI = 34, BACKBUTTONSIZE96DPI = 24, TITLEBARSEPARATORSIZE96DPI = 8, GUTTERSIZE96DPI = 51, LEFTMARGIN96DPI = 38, RIGHTMARGIN96DPI = 22, HEADERPADDING96DPI = 19;
		private static bool calculatedLayout;
		private static Size dpi;
		private static double pixelMultiplier;

		// Calculated layout
		private static int titleBarSize, backButtonSize, titleBarSeparatorSize, gutterSize, leftMargin, rightMargin, headerPadding;

		private WizardAeroMetrics()
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
			Size dpi = Dpi;
			titleBarSize = GetScaledPixelSize(TITLEBARSIZE96DPI);
			backButtonSize = GetScaledPixelSize(BACKBUTTONSIZE96DPI);
			titleBarSeparatorSize = GetScaledPixelSize(TITLEBARSEPARATORSIZE96DPI);
			gutterSize = GetScaledPixelSize(GUTTERSIZE96DPI);
			leftMargin = GetScaledPixelSize(LEFTMARGIN96DPI);
			rightMargin = GetScaledPixelSize(RIGHTMARGIN96DPI);
			headerPadding = GetScaledPixelSize(HEADERPADDING96DPI);

			calculatedLayout = true;
		}

		public static int HeaderPadding
		{
			get
			{
				if (!calculatedLayout)
					CalculateLayout();
				return headerPadding;
			}
		}

		public static int LeftMargin
		{
			get
			{
				if (!calculatedLayout)
					CalculateLayout();
				return leftMargin;
			}
		}

		public static int RightMargin
		{
			get
			{
				if (!calculatedLayout)
					CalculateLayout();
				return rightMargin;
			}
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

		public static int TitleBarSeparatorSize
		{
			get
			{
				if (!calculatedLayout)
					CalculateLayout();
				return titleBarSeparatorSize;
			}
		}

		public static int BackButtonSize
		{
			get
			{
				if (!calculatedLayout)
					CalculateLayout();
				return backButtonSize;
			}
		}

		public static int TitleBarSize
		{
			get
			{
				if (!calculatedLayout)
					CalculateLayout();
				return titleBarSize;
			}
		}

	}
}
