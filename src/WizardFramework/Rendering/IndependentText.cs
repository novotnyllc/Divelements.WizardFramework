using System;
using System.Drawing;
using System.Windows.Forms;

namespace Divelements.WizardFramework.Rendering
{
	/// <summary>
	/// Provides technology independent text rendering and measuring methods.
	/// </summary>
	public class IndependentText
	{
		internal static bool useGdiPlus = false;

		// Vista Glass
		internal static Rectangle glassTextBounds;
		internal static Graphics glassGraphics;
		internal static bool glassUseGlow;

		/// <summary>
		/// Draws a block of text.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw with.</param>
		/// <param name="text">The text to draw.</param>
		/// <param name="font">The font to use for text.</param>
		/// <param name="bounds">The bounds around the area to draw.</param>
		/// <param name="textFormat">Information on text formatting.</param>
		/// <param name="foreColor">The color of the text.</param>
		public static void DrawText(Graphics graphics, string text, Font font, Rectangle bounds, TextFormattingInformation textFormat, Color foreColor)
		{
			// Use glass if necessary
			if (glassGraphics == graphics && glassTextBounds.IntersectsWith(bounds))
			{
				DrawingMethods.DrawThemeText(graphics, bounds, text, font, foreColor, glassUseGlow);
				return;
			}

			if (!useGdiPlus)
				//OptimizedRendering.DrawText(graphics, text, font, bounds, textFormat.TextFormatFlags, foreColor);
				TextRenderer.DrawText(graphics, text, font, bounds, foreColor, textFormat.TextFormatFlags);
			else

			using (SolidBrush brush = new SolidBrush(foreColor))
				graphics.DrawString(text, font, brush, bounds, textFormat.StringFormat);
		}

		/// <summary>
		/// Draws a block of text.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw with.</param>
		/// <param name="text">The text to draw.</param>
		/// <param name="font">The font to use for text.</param>
		/// <param name="bounds">The bounds around the area to draw.</param>
		/// <param name="textFormat">Information on text formatting.</param>
		/// <param name="foreColor">The color of the text.</param>
		/// <param name="brush">A brush corresponding to the color of the text.</param>
		public static void DrawText(Graphics graphics, string text, Font font, Rectangle bounds, TextFormattingInformation textFormat, Color foreColor, Brush brush)
		{
			// Use glass if necessary
			if (glassGraphics == graphics && glassTextBounds.IntersectsWith(bounds))
			{
				DrawingMethods.DrawThemeText(graphics, bounds, text, font, foreColor, glassUseGlow);
				return;
			}

			if (!useGdiPlus)
				//OptimizedRendering.DrawText(graphics, text, font, bounds, textFormat.TextFormatFlags, foreColor);
				TextRenderer.DrawText(graphics, text, font, bounds, foreColor, textFormat.TextFormatFlags);
			else
				graphics.DrawString(text, font, brush, bounds, textFormat.StringFormat);
		}

		/// <summary>
		/// Measures the extent of a block of text.
		/// </summary>
		/// <param name="graphics">The Graphics object to measure with.</param>
		/// <param name="text">The text to measure.</param>
		/// <param name="font">The font to use for text.</param>
		/// <param name="textFormat">Information on text formatting.</param>
		/// <returns>The size of the text.</returns>
		public static Size MeasureText(Graphics graphics, string text, Font font, TextFormattingInformation textFormat)
		{
			if (!useGdiPlus)
				return TextRenderer.MeasureText(graphics, text, font, new Size(int.MaxValue, int.MaxValue), textFormat.TextFormatFlags);
			else
				return Size.Ceiling(graphics.MeasureString(text, font, int.MaxValue, textFormat.StringFormat));
		}

		/// <summary>
		/// Measures the extent of a block of text.
		/// </summary>
		/// <param name="graphics">The Graphics object to measure with.</param>
		/// <param name="text">The text to measure.</param>
		/// <param name="font">The font to use for text.</param>
		/// <param name="width">The width that the text is constrained by.</param>
		/// <param name="textFormat">Information on text formatting.</param>
		/// <returns>The size of the text.</returns>
		public static Size MeasureText(Graphics graphics, string text, Font font, int width, TextFormattingInformation textFormat)
		{
			if (!useGdiPlus)
				return TextRenderer.MeasureText(graphics, text, font, new Size(width, int.MaxValue), textFormat.TextFormatFlags);
			else
				return Size.Ceiling(graphics.MeasureString(text, font, width, textFormat.StringFormat));
		}
	}
}
