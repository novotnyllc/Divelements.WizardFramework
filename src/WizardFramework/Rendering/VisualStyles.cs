using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Divelements.WizardFramework.Rendering
{
	internal class VisualStyles
	{
		[DllImportAttribute("uxtheme.dll", CharSet=CharSet.Auto)]
		private static extern int GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int dwMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);
																																																																																		 
		private VisualStyles()
		{
		}

		//public bool OriginalTheme
		//{
		//    get
		//    {
		//        string path = ThemeFilename;
		//        return string.Compare(System.IO.Path.GetFileName(path), "luna.msstyles", true) == 0;
		//    }
		//}

		public static bool IsAero
		{
			get
			{
				return ThemeTitle == "Aero";
			}
		}

		public static string ThemeTitle
		{
			get
			{
				return System.IO.Path.GetFileNameWithoutExtension(ThemeFilename);
			}
		}

		public static string ThemeFilename
		{
			get
			{
				StringBuilder s = new StringBuilder(512);
				GetCurrentThemeName(s, s.Capacity, null, 0, null, 0);
				return s.ToString();
			}
		}

		public static string ColorScheme
		{
			get
			{
				StringBuilder s = new StringBuilder(512);
				GetCurrentThemeName(null, 0, s, s.Capacity, null, 0);
				return s.ToString();
			}
		}
	}
}
