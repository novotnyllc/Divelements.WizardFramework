using System;
using System.Drawing;
using System.Windows.Forms;

namespace Divelements.WizardFramework.Rendering
{
	/// <summary>
	/// Provides wizard colors fitting the Windows system color table.
	/// </summary>
	public class System07ColorTable : Wizard07ColorTable
	{
		/// <summary>
		/// Initializes a new instance of the System07ColorTable.
		/// </summary>
		public System07ColorTable()
		{
			TitleBarText = SystemColors.ControlText;
			HeaderText = SystemColors.ControlText;
			ContentBorder = SystemColors.ControlDark;
			GutterDivider = Color.Transparent;
			GutterBackground = SystemColors.Control;
			ContentBackground = SystemColors.Control;
			HeaderBackground = SystemColors.Control;
			TitleBarBackgroundActive = SystemColors.Control;
			TitleBarBackgroundInactive = SystemColors.Control;

			HeaderFontSize = Control.DefaultFont.Size;
			HeaderFontStyle = FontStyle.Bold;
		}
	}
}
