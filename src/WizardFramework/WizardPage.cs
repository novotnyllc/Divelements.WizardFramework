using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Divelements.WizardFramework.Rendering;

namespace Divelements.WizardFramework
{
	/// <summary>
	/// A content page designed to hold other controls to be shown in the wizard.
	/// </summary>
	[Designer(typeof(WizardPageDesigner))]
	public class WizardPage : WizardPageBase
	{
		// Members
		private string description = "This should be a piece of descriptive text about this page.";
		private string proceedText = string.Empty, introductionText = string.Empty;
		private Color textColor = SystemColors.WindowText, descriptionColor = SystemColors.WindowText;

		/// <summary>
		/// Initializes a new instance of the WizardPage class.
		/// </summary>
		public WizardPage()
		{
			// Change default base properties
			Text = "Wizard Page";
		}

		/// <summary>
		/// Indicates the color used to draw the title of the page.
		/// </summary>
		[Category("Appearance"), DefaultValue(typeof(Color), "WindowText"), Description("Indicates the color used to draw the title of the page.")]
		public Color TextColor
		{
			get
			{
				return textColor;
			}
			set
			{
				textColor = value;

				// Update the wizard if necessaru
				if (IsCurrentPage)
					Parent.Invalidate();
			}
		}

		/// <summary>
		/// Indicates the color used to draw the description of the page.
		/// </summary>
		[Category("Appearance"), DefaultValue(typeof(Color), "WindowText"), Description("Indicates the color used to draw the description of the page.")]
		public Color DescriptionColor
		{
			get
			{
				return descriptionColor;
			}
			set
			{
				descriptionColor = value;

				// Update the wizard if necessaru
				if (IsCurrentPage)
					Parent.Invalidate();
			}
		}

		/// <summary>
		/// The text that informs the user how to proceed to the next page.
		/// </summary>
		[Category("Appearance"), DefaultValue(""), Description("The text that informs the user how to proceed to the next page."), Localizable(true)]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
		public virtual string ProceedText
		{
			get { return proceedText; }
			set
			{
				// Validate
				if (value == null)
					value = string.Empty;

				proceedText = value;
				Invalidate();
			}
		}

		/// <summary>
		/// The text that is shown at the top of the page as an introduction to the page.
		/// </summary>
		[Category("Appearance"), DefaultValue(""), Description("The text that is shown at the top of the page as an introduction to the page.")]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
		public string IntroductionText
		{
			get { return introductionText; }
			set
			{
				// Validate
				if (value == null)
					value = string.Empty;

				introductionText = value;
				Invalidate();
			}
		}

		/// <summary>
		/// A piece of descriptive text about the page.
		/// </summary>
		[Localizable(true), Category("Appearance"), Description("A piece of descriptive text about the page.")]
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;

				// Update the wizard if necessary
				if (IsCurrentPage)
					Parent.Invalidate();
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Rectangle bounds = ClientRectangle;
			bounds.Inflate(-(Wizard97Metrics.LayoutMarginSize + Wizard97Metrics.LayoutMarginSize / 2), 0);

			if (IntroductionText.Length != 0)
			{
				using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(RightToLeft == RightToLeft.Yes, true, StringAlignment.Near, StringAlignment.Near, false))
					IndependentText.DrawText(e.Graphics, IntroductionText, Font, bounds, textFormat, ForeColor);
			}

			if (ProceedText.Length != 0)
			{
				using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(RightToLeft == RightToLeft.Yes, true, StringAlignment.Near, StringAlignment.Far, false))
					IndependentText.DrawText(e.Graphics, ProceedText, Font, bounds, textFormat, ForeColor);
			}
		}

	}
}
