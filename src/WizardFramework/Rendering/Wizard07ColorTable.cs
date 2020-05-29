using System;
using System.Drawing;

namespace Divelements.WizardFramework.Rendering
{
	/// <summary>
	/// Provides a table of colors suitable for drawing the elements in an Aero-style wizard.
	/// </summary>
	public class Wizard07ColorTable
	{
		// Colors
		private Color titleBarBackgroundActive, headerBackground, contentBackground, gutterBackground, gutterDivider, contentBorder, headerText, titleBarText, titleBarBackgroundInactive;
		private float headerFontSize;
		private FontStyle headerFontStyle;

		// Events
		/// <summary>
		/// Occurs when one of the colors defined in the table has changed.
		/// </summary>
		public event EventHandler ColorsChanged;

		/// <summary>
		/// Initializes a new instance of the WizardAeroColorTable class.
		/// </summary>
		protected Wizard07ColorTable()
		{
		}

		/// <summary>
		/// Raises the ColorsChanged event.
		/// </summary>
		/// <param name="e">The arguments associated with the event.</param>
		protected virtual void OnColorsChanged(EventArgs e)
		{
			if (ColorsChanged != null)
				ColorsChanged(this, e);
		}

		#region Color Properties

		/// <summary>
		/// The style to use for the header font.
		/// </summary>
		public FontStyle HeaderFontStyle
		{
			get { return headerFontStyle; }
			set { headerFontStyle = value; }
		}

		/// <summary>
		/// The size to use for the header font.
		/// </summary>
		public float HeaderFontSize
		{
			get { return headerFontSize; }
			set { headerFontSize = value; }
		}

		/// <summary>
		/// The color used to draw text in the titlebar.
		/// </summary>
		public Color TitleBarText
		{
			get { return titleBarText; }
			set
			{
				titleBarText = value;
				OnColorsChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// The color used to draw text in the header.
		/// </summary>
		public Color HeaderText
		{
			get { return headerText; }
			set
			{
				headerText = value; 
				OnColorsChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// The color used to draw the border around wizard content.
		/// </summary>
		public Color ContentBorder
		{
			get { return contentBorder; }
			set
			{
				contentBorder = value;
				OnColorsChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// The color used to draw the divider along the gutter.
		/// </summary>
		public Color GutterDivider
		{
			get { return gutterDivider; }
			set
			{
				gutterDivider = value;
				OnColorsChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// The color used to draw the background of the gutter.
		/// </summary>
		public Color GutterBackground
		{
			get { return gutterBackground; }
			set
			{
				gutterBackground = value;
				OnColorsChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// The color used to draw the background of the content.
		/// </summary>
		public Color ContentBackground
		{
			get { return contentBackground; }
			set
			{
				contentBackground = value;
				OnColorsChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// The color used to draw the background of the header.
		/// </summary>
		public Color HeaderBackground
		{
			get { return headerBackground; }
			set
			{
				headerBackground = value;
				OnColorsChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// The color used to draw the background of the titlebar.
		/// </summary>
		public Color TitleBarBackgroundActive
		{
			get { return titleBarBackgroundActive; }
			set
			{
				titleBarBackgroundActive = value;
				OnColorsChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// The color used to draw the background of the titlebar.
		/// </summary>
		public Color TitleBarBackgroundInactive
		{
			get { return titleBarBackgroundInactive; }
			set
			{
				titleBarBackgroundInactive = value;
				OnColorsChanged(EventArgs.Empty);
			}
		}

		#endregion

	}
}
