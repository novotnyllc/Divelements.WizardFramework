using System;
using System.Windows.Forms;
using System.Drawing;

namespace Divelements.WizardFramework.Rendering
{
	/// <summary>
	/// Provides a wizard user experience consistent with the new Aero wizard style introduced in Windows Vista.
	/// </summary>
	public class Wizard07UserExperience : WizardUserExperience
	{
		// Statics
		private Image previousButtonImage = Resources.BackButton;

		// Members
		private OwnerFormSubclass subclass;
		private Wizard07ColorTable colorTable;
		private Form ownerForm;
		//private bool oldShowIcon;

		/// <summary>
		/// Initializes a new instance of the Wizard07UserExperience class.
		/// </summary>
		public Wizard07UserExperience()
		{
			ChooseColorTable();
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override void ApplyTabIndices(Button backButton, Button nextButton, Button cancelButton, Button helpButton)
		{
			backButton.TabIndex = 1004;
			nextButton.TabIndex = 1001;
			cancelButton.TabIndex = 1002;
			helpButton.TabIndex = 1003;
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override Rectangle GetTransitionBounds(Rectangle bounds, out Color transitionColor)
		{
			bounds.Y += WizardAeroMetrics.TitleBarSize;
			bounds.Height -= WizardAeroMetrics.TitleBarSize + WizardAeroMetrics.GutterSize;
			transitionColor = colorTable.ContentBackground;

			return bounds;
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override Rectangle GetAeroGlassBounds(Rectangle bounds)
		{
			bounds.Height = WizardAeroMetrics.TitleBarSize;
			return bounds;
		}

		private void ChooseColorTable()
		{
			if (IsXP() && VisualStyles.IsAero)
				ColorTable = new Aero07ColorTable();
			else
				ColorTable = new System07ColorTable();
		}

		private static bool IsXP()
		{
			bool xp = false;

			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				xp = (Environment.OSVersion.Version >= new Version(5, 1, 0, 0));

			return xp;
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnSystemColorsChanged()
		{
			base.OnSystemColorsChanged();

			ChooseColorTable();
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override void Dispose()
		{
			OwnerForm = null;

			base.Dispose();
		}

		/// <summary>
		/// The color table in use by the user experience.
		/// </summary>
		public Wizard07ColorTable ColorTable
		{
			get { return colorTable; }
			set
			{
				// Validate
				if (value == null)
					throw new ArgumentNullException("value");

				// Unhook
				if (colorTable != null)
					colorTable.ColorsChanged -= new EventHandler(OnColorTableColorsChanged);

				colorTable = value;

				// Hook
				if (colorTable != null)
					colorTable.ColorsChanged += new EventHandler(OnColorTableColorsChanged);
			}
		}

		private void OnColorTableColorsChanged(object sender, EventArgs e)
		{
			OnVisualsChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected internal override void OnOwnerFormChanged()
		{
			if (Wizard != null)
				OwnerForm = Wizard.OwnerForm;
			else
				OwnerForm = null;

			if (subclass != null)
				subclass.CheckEligibility();
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnWizardChanged(Wizard oldWizard, Wizard newWizard)
		{
			// Unhook
			if (oldWizard != null)
			{
				oldWizard.MouseDown -= new MouseEventHandler(OnWizardMouseDown);
				oldWizard.PreviousButton.StateImage = null;
				oldWizard.PreviousButton.FlatStyle = FlatStyle.System;
			}

			// Hook
			if (newWizard != null)
			{
				newWizard.MouseDown += new MouseEventHandler(OnWizardMouseDown);
				newWizard.PreviousButton.StateImage = previousButtonImage;
				newWizard.PreviousButton.FlatStyle = FlatStyle.Standard;
			}

			OnOwnerFormChanged();
		}

		private void OnWizardMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && new Rectangle(0, 0, Wizard.ClientRectangle.Width, WizardAeroMetrics.TitleBarSize).Contains(e.X, e.Y) && Wizard.OwnerForm != null)
			{
				Win32.ReleaseCapture();
				const int HTCAPTION = 0x2;
				Win32.SendMessage(Wizard.OwnerForm.Handle, Win32.WM_NCLBUTTONDOWN, new IntPtr(HTCAPTION), IntPtr.Zero);
			}
		}

		private Form OwnerForm
		{
			get { return ownerForm; }
			set
			{
				if (value != ownerForm)
				{
					// Unhook
					if (subclass != null)
					{
						subclass.Dispose();
						subclass = null;
					}
					if (ownerForm != null)
					{
						//ownerForm.ShowIcon = oldShowIcon;
						ownerForm.Activated -= new EventHandler(OnOwnerFormActivate);
						ownerForm.Deactivate -= new EventHandler(OnOwnerFormActivate);
					}

					ownerForm = value;

					// Hook
					if (ownerForm != null)
					{
						subclass = new OwnerFormSubclass(ownerForm, Wizard);
						//oldShowIcon = ownerForm.ShowIcon;
						//ownerForm.ShowIcon = false;
						ownerForm.Activated += new EventHandler(OnOwnerFormActivate);
						ownerForm.Deactivate += new EventHandler(OnOwnerFormActivate);
					}
				}
			}
		}

		private void OnOwnerFormActivate(object sender, EventArgs e)
		{
			if (Wizard != null)
				Wizard.Invalidate(new Rectangle(0, 0, Wizard.ClientRectangle.Width, WizardAeroMetrics.TitleBarSize));
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override void DrawPageClient(Graphics graphics, Rectangle bounds, WizardPageBase page)
		{
			graphics.Clear(colorTable.ContentBackground);
			if (page.BackColor != colorTable.ContentBackground)
				page.BackColor = colorTable.ContentBackground;
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override void DrawWizardClient(RenderingContext context, Rectangle clientBounds, WizardPageBase page)
		{
			// Titlebar
			Rectangle bounds = clientBounds;
			bounds.Height = WizardAeroMetrics.TitleBarSize;
			if (!OwnerFormSubclass.IsCompositionEnabled)
			{
				using (SolidBrush brush = new SolidBrush(OwnerForm == null || OwnerForm == Form.ActiveForm ? colorTable.TitleBarBackgroundActive : colorTable.TitleBarBackgroundInactive))
					context.Graphics.FillRectangle(brush, bounds);
			}
			if (!context.RightToLeft)
				bounds.X += WizardAeroMetrics.BackButtonSize + WizardAeroMetrics.TitleBarSeparatorSize;
			bounds.Width -= WizardAeroMetrics.BackButtonSize + WizardAeroMetrics.TitleBarSeparatorSize;

			if (OwnerFormSubclass.IsVista)
			{
				if (Wizard.OwnerForm != null && Wizard.OwnerForm.Icon != null)
				{
					Rectangle iconBounds = new Rectangle(context.RightToLeft ? bounds.Right - SystemInformation.SmallIconSize.Width : bounds.X, bounds.Y + bounds.Height / 2 - SystemInformation.SmallIconSize.Height / 2, SystemInformation.SmallIconSize.Width, SystemInformation.SmallIconSize.Height);
					using (Icon smallIcon = new Icon(Wizard.OwnerForm.Icon, SystemInformation.SmallIconSize.Width, SystemInformation.SmallIconSize.Height))
						context.Graphics.DrawIcon(smallIcon, iconBounds);
					if (!context.RightToLeft)
						bounds.X += iconBounds.Width;
					bounds.Width -= iconBounds.Width;
				}
				if (!context.RightToLeft)
					bounds.X += WizardAeroMetrics.TitleBarSeparatorSize * 2;
				bounds.Width -= WizardAeroMetrics.TitleBarSeparatorSize * 2;
				//if (OwnerFormSubclass.IsCompositionEnabled)
				//{
					Color textColor = (Wizard.OwnerForm != null && Wizard.OwnerForm.WindowState == FormWindowState.Maximized) ? Color.White : SystemColors.ActiveCaptionText;

					using (Font font = SystemFonts.CaptionFont)
					{
						if (OwnerFormSubclass.IsCompositionEnabled)
							DrawingMethods.DrawThemeText(context.Graphics, bounds, Wizard.OwnerForm.Text, font, textColor, Wizard.OwnerForm.WindowState != FormWindowState.Maximized);
						else
						{
							using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(context.RightToLeft, false, StringAlignment.Near, StringAlignment.Center, false))
								IndependentText.DrawText(context.Graphics, Wizard.OwnerForm.Text, font, bounds, textFormat, textColor);
						}
					}
				//}
				//else
				//{
				//    using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(context.RightToLeft, false, StringAlignment.Near, StringAlignment.Center, false))
				//        IndependentText.DrawText(context.Graphics, Wizard.OwnerForm.Text, context.ControlFont, bounds, textFormat, colorTable.TitleBarText);
				//}
			}

			// Gutter
			bounds = clientBounds;
			bounds.Y = bounds.Bottom - WizardAeroMetrics.GutterSize;
			bounds.Height = WizardAeroMetrics.GutterSize;
			using (SolidBrush brush = new SolidBrush(colorTable.GutterBackground))
				context.Graphics.FillRectangle(brush, bounds);
			using (Pen pen = new Pen(colorTable.GutterDivider))
				context.Graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);

			// Header
			int headerTextHeight = 0;
			if (page != null)
			{
				using (Font font = CreateAeroHeaderFont())
				{
					using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(context.RightToLeft, true, StringAlignment.Near, StringAlignment.Near, false))
					{
						headerTextHeight = IndependentText.MeasureText(context.Graphics, page.Text, font, textFormat).Height;
						bounds = clientBounds;
						bounds.Y += WizardAeroMetrics.TitleBarSize;
						bounds.Height = WizardAeroMetrics.HeaderPadding * 2 + headerTextHeight;
						using (SolidBrush brush = new SolidBrush(colorTable.HeaderBackground))
							context.Graphics.FillRectangle(brush, bounds);
						bounds.X += WizardAeroMetrics.LeftMargin;
						bounds.Width -= WizardAeroMetrics.LeftMargin + WizardAeroMetrics.RightMargin;
						bounds.Y += WizardAeroMetrics.HeaderPadding;
						IndependentText.DrawText(context.Graphics, page.Text, font, bounds, textFormat, colorTable.HeaderText);
					}
				}
			}

			// Body
			bounds = clientBounds;
			bounds.Y += WizardAeroMetrics.TitleBarSize + WizardAeroMetrics.HeaderPadding * 2 + headerTextHeight;
			bounds.Height -= WizardAeroMetrics.TitleBarSize + WizardAeroMetrics.GutterSize + WizardAeroMetrics.HeaderPadding * 2 + headerTextHeight;
			using (SolidBrush brush = new SolidBrush(colorTable.ContentBackground))
				context.Graphics.FillRectangle(brush, bounds);

			// Content border
			bounds = clientBounds;
			bounds.Y += WizardAeroMetrics.TitleBarSize;
			bounds.Height -= WizardAeroMetrics.TitleBarSize + WizardAeroMetrics.GutterSize;
			bounds.Width--;
			bounds.Height--;
			using (Pen pen = new Pen(colorTable.ContentBorder))
				context.Graphics.DrawRectangle(pen, bounds);
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override void LayoutButtons(Rectangle bounds, ButtonBase cancelButton, ButtonBase nextButton, ButtonBase previousButton, ButtonBase helpButton)
		{
			// Move buttons
			int buttonTop = bounds.Bottom - (WizardAeroMetrics.GutterSize / 2) - (Wizard97Metrics.ButtonSize.Height / 2);
			int buttonSpacing = WizardAeroMetrics.TitleBarSeparatorSize;
			Rectangle buttonBounds = new Rectangle(bounds.Right - WizardAeroMetrics.RightMargin - Wizard97Metrics.ButtonSize.Width, buttonTop, Wizard97Metrics.ButtonSize.Width, Wizard97Metrics.ButtonSize.Height);
			if (helpButton.Visible)
			{
				helpButton.Bounds = buttonBounds;
				buttonBounds.Offset(-(Wizard97Metrics.ButtonSize.Width + buttonSpacing), 0);
			}
			cancelButton.Bounds = buttonBounds;
			buttonBounds.Offset(-(Wizard97Metrics.ButtonSize.Width + buttonSpacing), 0);
			nextButton.Bounds = buttonBounds;
			buttonBounds.Offset(-Wizard97Metrics.ButtonSize.Width, 0);

			// Back button goes at the top
			previousButton.Bounds = new Rectangle(bounds.Left + WizardAeroMetrics.GetScaledPixelSize(2), bounds.Top + WizardAeroMetrics.TitleBarSize / 2 - WizardAeroMetrics.BackButtonSize / 2, WizardAeroMetrics.BackButtonSize, WizardAeroMetrics.BackButtonSize);
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override Rectangle GetContentPageBounds(WizardPageBase page, Rectangle bounds)
		{
			bounds.Y += WizardAeroMetrics.TitleBarSize;
			bounds.Height -= WizardAeroMetrics.TitleBarSize + WizardAeroMetrics.GutterSize;
			bounds.X += WizardAeroMetrics.LeftMargin;
			bounds.Width -= WizardAeroMetrics.LeftMargin + WizardAeroMetrics.RightMargin;
			bounds.Y += WizardAeroMetrics.HeaderPadding * 2;
			bounds.Height -= WizardAeroMetrics.HeaderPadding * 3;

			// If there is a selected page, make way for its header
			using (Font font = CreateAeroHeaderFont())
			{
				using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(Wizard.RightToLeft == RightToLeft.Yes, true, StringAlignment.Near, StringAlignment.Near, false))
				{
					using (Graphics graphics = Wizard.CreateGraphics())
					{
						int headerHeight = IndependentText.MeasureText(graphics, page.Text, font, bounds.Width, textFormat).Height;
						bounds.Y += headerHeight;
						bounds.Height -= headerHeight;
					}
				}
			}

			return bounds;
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override Rectangle GetCoverPageBounds(WizardPageBase page, Rectangle bounds)
		{
			return GetContentPageBounds(page, bounds);
		}

		private Font CreateAeroHeaderFont()
		{
			return new Font(Wizard.Font.FontFamily, colorTable.HeaderFontSize, colorTable.HeaderFontStyle, GraphicsUnit.Point);
		}

	}
}
