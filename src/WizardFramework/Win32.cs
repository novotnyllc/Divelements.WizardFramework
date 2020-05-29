using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Divelements.WizardFramework
{
	internal class Win32
	{
		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateDIBSection(IntPtr hdc, BITMAPINFO pbmi, uint iUsage, IntPtr ppvBits, IntPtr hSection, uint dwOffset);
		[DllImport("gdi32.dll", ExactSpelling = true)]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
		[DllImport("UxTheme.dll", CharSet = CharSet.Unicode)]
		public static extern int DrawThemeTextEx(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, string text, int iCharCount, int dwFlags, ref RECT pRect, ref DTTOPTS pOptions);
		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);
		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteObject(IntPtr hObject);
		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteDC(IntPtr hdc);
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetDC(IntPtr hWnd);
		[DllImport("gdi32.dll")]
		public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
		[DllImport("user32.dll")]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
		[DllImport("user32", SetLastError = true)]
		public static extern bool AnimateWindow(IntPtr hwnd, int time, AnimateWindowFlags flags);
		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		[DllImport("user32.dll")]
		public static extern int GetSystemMetrics(int smIndex);
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr OpenThemeData(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszClassList);
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int CloseThemeData(IntPtr hTheme);
		[DllImport("dwmapi.dll")]
		public static extern void DwmIsCompositionEnabled(ref bool pfEnabled);
		[DllImport("dwmapi.dll")]
		public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMargins);
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
		[DllImport("uxtheme.dll", PreserveSig = false)]
		public static extern void SetWindowThemeAttribute([In] IntPtr hwnd, [In] WINDOWTHEMEATTRIBUTETYPE eAttribute, [In] ref WTA_OPTIONS pvAttribute, [In] uint cbAttribute);

		public static bool DisableRichDrawing
		{
			get
			{
				return TerminalServerSession || System.Windows.Forms.SystemInformation.HighContrast;
			}
		}

		public static bool TerminalServerSession
		{
			get
			{
				return GetSystemMetrics(SM_REMOTESESSION) != 0;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MARGINS
		{
			public int Left, Right, Top, Bottom;
		}

		[Flags()]
		public enum AnimateWindowFlags : uint
		{
			AW_HOR_POSITIVE = 0x00000001,
			AW_HOR_NEGATIVE = 0x00000002,
			AW_VER_POSITIVE = 0x00000004,
			AW_VER_NEGATIVE = 0x00000008,
			AW_CENTER = 0x00000010,
			AW_HIDE = 0x00010000,
			AW_ACTIVATE = 0x00020000,
			AW_SLIDE = 0x00040000,
			AW_BLEND = 0x00080000
		}

		internal enum WINDOWTHEMEATTRIBUTETYPE : uint
		{
			/// <summary>Non-client area window attributes will be set.</summary>
			WTA_NONCLIENT = 1,
		}

		/// <summary>
		/// WindowThemeNonClientAttributes
		/// </summary>
		[Flags]
		internal enum WTNCA : uint
		{
			/// <summary>Prevents the window caption from being drawn.</summary>
			NODRAWCAPTION = 0x00000001,
			/// <summary>Prevents the system icon from being drawn.</summary>
			NODRAWICON = 0x00000002,
			/// <summary>Prevents the system icon menu from appearing.</summary>
			NOSYSMENU = 0x00000004,
			/// <summary>Prevents mirroring of the question mark, even in right-to-left (RTL) layout.</summary>
			NOMIRRORHELP = 0x00000008,
			/// <summary> A mask that contains all the valid bits.</summary>
			VALIDBITS = NODRAWCAPTION | NODRAWICON | NOMIRRORHELP | NOSYSMENU,
		}

		/// <summary>Defines options that are used to set window visual style attributes.</summary>
		[StructLayout(LayoutKind.Explicit)]
		internal struct WTA_OPTIONS
		{
			// public static readonly uint Size = (uint)Marshal.SizeOf(typeof(WTA_OPTIONS));
			public const uint Size = 8;

			/// <summary>
			/// A combination of flags that modify window visual style attributes.
			/// Can be a combination of the WTNCA constants.
			/// </summary>
			[FieldOffset(0)]
			public WTNCA dwFlags;

			/// <summary>
			/// A bitmask that describes how the values specified in dwFlags should be applied.
			/// If the bit corresponding to a value in dwFlags is 0, that flag will be removed.
			/// If the bit is 1, the flag will be added.
			/// </summary>
			[FieldOffset(4)]
			public WTNCA dwMask;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class BITMAPINFO
		{
			public int biSize;
			public int biWidth;
			public int biHeight;
			public short biPlanes;
			public short biBitCount;
			public int biCompression;
			public int biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public int biClrUsed;
			public int biClrImportant;
			public byte bmiColors_rgbBlue;
			public byte bmiColors_rgbGreen;
			public byte bmiColors_rgbRed;
			public byte bmiColors_rgbReserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DTTOPTS
		{
			public int dwSize;
			public int dwFlags;
			public int crText;
			public int crBorder;
			public int crShadow;
			public int iTextShadowType;
			public POINT ptShadowOffset;
			public int iBorderSize;
			public int iFontPropId;
			public int iColorPropId;
			public int iStateId;
			public bool fApplyOverlay;
			public int iGlowSize;
			public int pfnDrawTextCallback;
			public IntPtr lParam;
		}

		[Serializable, StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;

			public RECT(int left, int top, int right, int bottom)
			{
				Left = left;
				Top = top;
				Right = right;
				Bottom = bottom;
			}

			public RECT(Rectangle rectangle)
			{
				Left = rectangle.X;
				Top = rectangle.Y;
				Right = rectangle.Right;
				Bottom = rectangle.Bottom;
			}

			public Rectangle ToRectangle()
			{
				return new Rectangle(Left, Top, Right - Left, Bottom - Top);
			}

			public override string ToString()
			{
				return "Left: " + Left + ", " + "Top: " + Top + ", Right: " + Right + ", Bottom: " + Bottom;
			}
		}

		public const int WM_DWMCOMPOSITIONCHANGED = 0x31E;
		public const int WM_ERASEBKGND = 0x0014;
		public const int WM_NCLBUTTONDOWN = 0xA1;

		public const UInt32 SWP_NOACTIVATE = 0x0010;
		public const UInt32 SWP_HIDEWINDOW = 0x0080;
		public const UInt32 SWP_NOSIZE = 0x0001;
		public const UInt32 SWP_NOMOVE = 0x0002;
		public const UInt32 SWP_SHOWWINDOW = 0x0040;
		public const int SW_SHOWNOACTIVATE = 0x8;

		public const int SM_REMOTESESSION = 0x1000;

	}
}
