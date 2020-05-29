using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Divelements.WizardFramework.Rendering
{
	/// <summary>
	/// Provides a wizard user experience consistent with the Wizard 97 guidelines.
	/// </summary>
	public class Wizard97UserExperience : WizardUserExperience
	{
		// Members
		private Font headerFont;
		private Color marginColor = Color.FromArgb(0, 0, 64);

		/// <summary>
		/// Initializes a new instance of the Wizard97UserExperience class.
		/// </summary>
		public Wizard97UserExperience()
		{
			// Create header font
			headerFont = new Font("Verdana", 12, FontStyle.Bold, GraphicsUnit.Point);
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override void ApplyTabIndices(Button backButton, Button nextButton, Button cancelButton, Button helpButton)
		{
			backButton.TabIndex = 1000;
			nextButton.TabIndex = 1001;
			cancelButton.TabIndex = 1002;
			helpButton.TabIndex = 1003;
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override Rectangle GetTransitionBounds(Rectangle bounds, out Color transitionColor)
		{
			transitionColor = SystemColors.Control;
			bounds.Height -= Wizard97Metrics.GutterSizeWithDivider;

			return bounds;
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override void DrawPageClient(Graphics graphics, Rectangle bounds, WizardPageBase page)
		{
			Color backColor;
			if (page is CoverWizardPage)
				backColor = SystemColors.Window;
			else
				backColor = page.BackColor;
			graphics.Clear(backColor);

			if (page.BackColor != backColor)
				page.BackColor = backColor;
		}

		/// <summary>
		/// The color of the margin on cover pages.
		/// </summary>
		[Category("Appearance"), DefaultValue(typeof(Color), "0,0,64"), Description("The color of the margin on cover pages.")]
		public Color MarginColor
		{
			get { return marginColor; }
			set
			{
				marginColor = value;
				OnVisualsChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override void DrawWizardClient(RenderingContext context, Rectangle bounds, WizardPageBase page)
		{
			// Draw gutter divider
			int top = bounds.Bottom - Wizard97Metrics.GutterSizeWithDivider;
			context.Graphics.DrawLine(SystemPens.ControlLightLight, bounds.Left, top + 1, bounds.Right, top + 1);
			context.Graphics.DrawLine(SystemPens.ControlDark, bounds.Left, top, bounds.Right, top);

			if (page is CoverWizardPage)
				PaintCoverPage97(context, bounds, (CoverWizardPage)page);
			else if (page is WizardPage)
				PaintContentPage97(context, bounds, (WizardPage)page);
		}

		private void PaintCoverPage97(RenderingContext context, Rectangle clientBounds, CoverWizardPage page)
		{
			// Paint page background
			Rectangle bounds = clientBounds;
			bounds.Height -= Wizard97Metrics.GutterSizeWithDivider;
			context.Graphics.FillRectangle(SystemBrushes.Window, bounds);

			// Paint margin
			bounds = clientBounds;
			bounds.Width = Wizard97Metrics.CoverMarginSize;
			bounds.Height -= Wizard97Metrics.GutterSizeWithDivider;
			if (Wizard.MarginImage == null || Wizard.MarginImageAppearance == MarginImageAppearance.Normal)
			{
				using (SolidBrush brush = new SolidBrush(MarginColor))
					context.Graphics.FillRectangle(brush, bounds);
			}
			if (Wizard.MarginImage != null)
			{
				if (Wizard.MarginImageAppearance == MarginImageAppearance.Normal)
					context.Graphics.DrawImage(Wizard.MarginImage, new Rectangle(0, 0, Wizard.MarginImage.Width, Wizard.MarginImage.Height));
				else
					context.Graphics.DrawImage(Wizard.MarginImage, bounds);
			}

			// Paint title
			Rectangle coverPageBounds = GetCoverPageBounds(page, clientBounds);
			bounds = coverPageBounds;
			bounds.Y = clientBounds.Top + Wizard97Metrics.LayoutMarginSize / 2;
			bounds.Height = coverPageBounds.Top - bounds.Y;
			using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(context.RightToLeft, true, StringAlignment.Near, StringAlignment.Near, false))
				IndependentText.DrawText(context.Graphics, page.Text, headerFont, bounds, textFormat, page.ForeColor);
		}

		private void PaintContentPage97(RenderingContext context, Rectangle clientBounds, WizardPage page)
		{
			// Paint banner background and divider
			Rectangle bounds = clientBounds;
			bounds.Height = Wizard97Metrics.HeaderSize;
			context.Graphics.FillRectangle(SystemBrushes.Window, bounds);
			context.Graphics.DrawLine(SystemPens.ControlDark, bounds.Left, bounds.Bottom, bounds.Right, bounds.Bottom);
			context.Graphics.DrawLine(SystemPens.ControlLightLight, bounds.Left, bounds.Bottom + 1, bounds.Right, bounds.Bottom + 1);

			// Paint banner bitmap if possible
			int bannerBitmapMargin = (Wizard97Metrics.HeaderSize - Wizard97Metrics.BannerBitmapSize) / 2;
			Rectangle bannerBitmapBounds = bounds;
			bannerBitmapBounds.X = bannerBitmapBounds.Right - (Wizard97Metrics.BannerBitmapSize + bannerBitmapMargin);
			bannerBitmapBounds.Y += bannerBitmapMargin;
			bannerBitmapBounds.Size = new Size(Wizard97Metrics.BannerBitmapSize, Wizard97Metrics.BannerBitmapSize);
			bounds.Width -= (Wizard97Metrics.BannerBitmapSize + bannerBitmapMargin);
			if (Wizard.BannerImage != null)
				context.Graphics.DrawImage(Wizard.BannerImage, bannerBitmapBounds);
			else
				context.Graphics.FillRectangle(Brushes.DarkBlue, bannerBitmapBounds);

			using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(context.RightToLeft, true, StringAlignment.Near, StringAlignment.Near, false))
			{
				// Paint header text
				bounds.X += Wizard97Metrics.LayoutMarginSize;
				bounds.Width -= (Wizard97Metrics.LayoutMarginSize + Wizard97Metrics.LayoutMarginSize / 2);
				bounds.Y += Wizard97Metrics.LayoutMarginSize / 2 - 1;
				SizeF requiredHeaderSize = Size.Empty;
				using (Font headerFont = new Font(page.Font, FontStyle.Bold))
				{
					IndependentText.DrawText(context.Graphics, page.Text, headerFont, bounds, textFormat, page.TextColor);
					requiredHeaderSize = IndependentText.MeasureText(context.Graphics, page.Text, headerFont, textFormat);
				}

				// Paint descriptive text
				bounds.Y += (int)Math.Ceiling(requiredHeaderSize.Height) + 2;
				bounds.X += Wizard97Metrics.LayoutMarginSize;
				bounds.Width -= Wizard97Metrics.LayoutMarginSize;
				IndependentText.DrawText(context.Graphics, page.Description, page.Font, bounds, textFormat, page.DescriptionColor);
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override void LayoutButtons(Rectangle bounds, ButtonBase cancelButton, ButtonBase nextButton, ButtonBase previousButton, ButtonBase helpButton)
		{
			// Move buttons
			int buttonTop = bounds.Bottom - (Wizard97Metrics.GutterSize / 2) - (Wizard97Metrics.ButtonSize.Height / 2);
			int buttonSpacing = buttonTop - (bounds.Bottom - Wizard97Metrics.GutterSize);
			Rectangle buttonBounds = new Rectangle(bounds.Right - buttonSpacing - Wizard97Metrics.ButtonSize.Width, buttonTop, Wizard97Metrics.ButtonSize.Width, Wizard97Metrics.ButtonSize.Height);
			if (helpButton.Visible)
			{
				helpButton.Bounds = buttonBounds;
				buttonBounds.Offset(-(Wizard97Metrics.ButtonSize.Width + buttonSpacing), 0);
			}
			cancelButton.Bounds = buttonBounds;
			buttonBounds.Offset(-(Wizard97Metrics.ButtonSize.Width + buttonSpacing), 0);
			nextButton.Bounds = buttonBounds;
			buttonBounds.Offset(-Wizard97Metrics.ButtonSize.Width, 0);
			previousButton.Bounds = buttonBounds;
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override Rectangle GetContentPageBounds(WizardPageBase page, Rectangle bounds)
		{
			// Trim top and bottom
			bounds.Y += Wizard97Metrics.HeaderSizeWithDivider;
			bounds.Height -= Wizard97Metrics.HeaderSizeWithDivider + Wizard97Metrics.GutterSizeWithDivider;

			// Apply margin
			bounds.Inflate(-Wizard97Metrics.LayoutMarginSize / 2, -Wizard97Metrics.LayoutMarginSize / 2);

			return bounds;
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override Rectangle GetCoverPageBounds(WizardPageBase page, Rectangle bounds)
		{
			// Trim top and bottom
			int headerHeight = Wizard97Metrics.GetScaledPixelSize(60);
			bounds.Y += headerHeight;
			bounds.Height -= Wizard97Metrics.GutterSizeWithDivider + headerHeight;

			// Trim left
			bounds.X += Wizard97Metrics.CoverMarginSize;
			bounds.Width -= Wizard97Metrics.CoverMarginSize;

			// Trim right
			bounds.Width -= Wizard97Metrics.LayoutMarginSize / 2;

			// Apply margin
			bounds.Inflate(-Wizard97Metrics.LayoutMarginSize / 2, -Wizard97Metrics.LayoutMarginSize / 2);

			return bounds;
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override void Dispose()
		{
			headerFont.Dispose();

			base.Dispose();
		}

	}
}
