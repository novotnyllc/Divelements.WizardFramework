using System;
using System.Drawing;

namespace Divelements.WizardFramework.Rendering
{
	/// <summary>
	/// Provides wizard colors fitting the Aero Windows Vista theme.
	/// </summary>
	public class Aero07ColorTable : Wizard07ColorTable
	{
		/// <summary>
		/// Initializes a new instance of the Aero07ColorTable class.
		/// </summary>
		public Aero07ColorTable()
		{
			TitleBarText = Color.Black;
			HeaderText = Color.FromArgb(0, 51, 153);
			ContentBorder = Color.Transparent;
			GutterDivider = Color.FromArgb(223, 223, 223);
			GutterBackground = Color.FromArgb(240, 240, 240);
			ContentBackground = Color.White;
			HeaderBackground = Color.White;
			TitleBarBackgroundActive = Color.FromArgb(185, 209, 234);
			TitleBarBackgroundInactive = Color.FromArgb(215, 228, 242);

			HeaderFontSize = 12.0f;
			HeaderFontStyle = FontStyle.Regular;
		}
	}
}
