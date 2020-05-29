using System;
using System.Drawing;
using System.Windows.Forms;

namespace Divelements.WizardFramework.Rendering
{
	/// <summary>
	/// Provides a way of storing text formatting information that is not tied to either GDI or GDI+.
	/// </summary>
	public struct TextFormattingInformation : IDisposable
	{
		/// <summary>
		/// Flags that control how text is drawn.
		/// </summary>
		public TextFormatFlags TextFormatFlags;
		/// <summary>
		/// An object that controls how text is drawn.
		/// </summary>
		public StringFormat StringFormat;

		/// <summary>
		/// Creates formatting information.
		/// </summary>
		/// <param name="rtl">Whether text is laid out in right-to-left reading order.</param>
		/// <param name="allowWrap">Whether text is allowed to wrap.</param>
		/// <param name="horizontalAlignment">How text is aligned.</param>
		/// <param name="showMnemonics">Whether to show mnemonics in text.</param>
		/// <returns>A new TextFormattingInformation instance.</returns>
		public static TextFormattingInformation CreateFormattingInformation(bool rtl, bool allowWrap, StringAlignment horizontalAlignment, StringAlignment verticalAlignment, bool showMnemonics)
		{
			TextFormattingInformation textInfo = new TextFormattingInformation();

			// Create GDI text formatting information
			TextFormatFlags flags = TextFormatFlags.EndEllipsis | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform | TextFormatFlags.NoPadding;
			if (rtl)
				flags |= TextFormatFlags.RightToLeft;
			if (allowWrap)
				flags |= TextFormatFlags.WordBreak;
			else
				flags |= TextFormatFlags.SingleLine;
			switch (verticalAlignment)
			{
				case StringAlignment.Near:
					flags |= TextFormatFlags.Top;
					break;
				case StringAlignment.Far:
					flags |= TextFormatFlags.Bottom;
					break;
				case StringAlignment.Center:
					flags |= TextFormatFlags.VerticalCenter;
					break;
			}
			switch (horizontalAlignment)
			{
				case StringAlignment.Center:
					flags |= TextFormatFlags.HorizontalCenter;
					break;
				case StringAlignment.Far:
					if (rtl)
						flags |= TextFormatFlags.Left;
					else
						flags |= TextFormatFlags.Right;
					break;
				case StringAlignment.Near:
					if (rtl)
						flags |= TextFormatFlags.Right;
					else
						flags |= TextFormatFlags.Left;
					break;
			}
			if (!showMnemonics)
				flags |= TextFormatFlags.HidePrefix;
			textInfo.TextFormatFlags = flags;

			// Create GDI+ text formatting information
			StringFormat format = new StringFormat(StringFormat.GenericTypographic);
			if (rtl)
				format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
			if (!allowWrap)
				format.FormatFlags = StringFormatFlags.NoWrap;
			format.Trimming = StringTrimming.EllipsisCharacter;
			format.LineAlignment = verticalAlignment	;
			format.Alignment = (System.Drawing.StringAlignment)horizontalAlignment;
			format.HotkeyPrefix = showMnemonics ? System.Drawing.Text.HotkeyPrefix.Show : System.Drawing.Text.HotkeyPrefix.Hide;
			textInfo.StringFormat = format;

			return textInfo;
		}

		void IDisposable.Dispose()
		{
			if (StringFormat != null)
				StringFormat.Dispose();
		}

	}
}
