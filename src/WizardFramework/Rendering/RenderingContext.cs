using System;
using System.Drawing;
using System.Windows.Forms;

namespace Divelements.WizardFramework.Rendering
{
	/// <summary>
	/// A class that contains common data used for rendering.
	/// </summary>
	public class RenderingContext : IDisposable
	{
		// Members
		private Graphics graphics;
		private Font controlFont;
		private bool rtl;
		private TextFormattingInformation standardNearsideText, standardCenteredText, standardFarsideText;
		private TextFormattingInformation standardNearsideWrappableText, standardCenteredWrappableText, standardFarsideWrappableText;
		private Color defaultTextColor, disabledTextColor;

		internal RenderingContext(Graphics graphics, Font controlFont, bool rtl, bool showMnemonics, Color defaultTextColor, Color disabledTextColor)
		{
			// Record members
			this.graphics = graphics;
			this.controlFont = controlFont;
			this.rtl = rtl;
			this.defaultTextColor = defaultTextColor;
			this.disabledTextColor = disabledTextColor;
			
			// Create text formatting information
			standardNearsideText = TextFormattingInformation.CreateFormattingInformation(rtl, false, StringAlignment.Near, StringAlignment.Center, showMnemonics);
			standardCenteredText = TextFormattingInformation.CreateFormattingInformation(rtl, false, StringAlignment.Center, StringAlignment.Center, showMnemonics);
			standardFarsideText = TextFormattingInformation.CreateFormattingInformation(rtl, false, StringAlignment.Far, StringAlignment.Center, showMnemonics);
			standardNearsideWrappableText = TextFormattingInformation.CreateFormattingInformation(rtl, true, StringAlignment.Near, StringAlignment.Center, showMnemonics);
			standardCenteredWrappableText = TextFormattingInformation.CreateFormattingInformation(rtl, true, StringAlignment.Center, StringAlignment.Center, showMnemonics);
			standardFarsideWrappableText = TextFormattingInformation.CreateFormattingInformation(rtl, true, StringAlignment.Far, StringAlignment.Center, showMnemonics);
		}

		/// <summary>
		/// Disposes of the resources used by the class.
		/// </summary>
		public void Dispose()
		{
			(standardNearsideText as IDisposable).Dispose();
			(standardCenteredText as IDisposable).Dispose();
			(standardFarsideText as IDisposable).Dispose();
			(standardNearsideWrappableText as IDisposable).Dispose();
			(standardCenteredWrappableText as IDisposable).Dispose();
			(standardFarsideWrappableText as IDisposable).Dispose();
		}

		#region Properties

		/// <summary>
		/// The color to use for drawing disabled text.
		/// </summary>
		public Color DisabledTextColor
		{
			get { return disabledTextColor; }
		}

		/// <summary>
		/// The default color to use for drawing text.
		/// </summary>
		public Color DefaultTextColor
		{
			get { return defaultTextColor; }
		}

		/// <summary>
		/// Whether right-to-left reading order is enabled.
		/// </summary>
		public bool RightToLeft
		{
			get { return rtl; }
		}

		/// <summary>
		/// Standard text formatting information for far-justified text.
		/// </summary>
		public TextFormattingInformation StandardFarsideText
		{
			get { return standardFarsideText; }
		}

		/// <summary>
		/// Standard text formatting information for near-justified text.
		/// </summary>
		public TextFormattingInformation StandardNearsideText
		{
			get { return standardNearsideText; }
		}

		/// <summary>
		/// Standard text formatting information for centered text.
		/// </summary>
		public TextFormattingInformation StandardCenteredText
		{
			get { return standardCenteredText; }
		}

		/// <summary>
		/// Standard text formatting information for far-justified text.
		/// </summary>
		public TextFormattingInformation StandardFarsideWrappableText
		{
			get { return standardFarsideWrappableText; }
		}

		/// <summary>
		/// Standard text formatting information for near-justified text.
		/// </summary>
		public TextFormattingInformation StandardNearsideWrappableText
		{
			get { return standardNearsideWrappableText; }
		}

		/// <summary>
		/// Standard text formatting information for centered text.
		/// </summary>
		public TextFormattingInformation StandardCenteredWrappableText
		{
			get { return standardCenteredWrappableText; }
		}

		/// <summary>
		/// A Graphics object that can be used to draw on the surface of the control.
		/// </summary>
		public Graphics Graphics
		{
			get { return graphics; }
		}

		/// <summary>
		/// A Font object that can be used to draw text.
		/// </summary>
		public Font ControlFont
		{
			get { return controlFont; }
		}

		#endregion

	}
}
