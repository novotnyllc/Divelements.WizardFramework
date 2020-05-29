using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Divelements.WizardFramework.Rendering;

namespace Divelements.WizardFramework
{
	/// <summary>
	/// A page designed to show an introduction to the wizard, and indicate its purpose.
	/// </summary>
	public class IntroductionPage : CoverWizardPage
	{
		// Members
		private string introductionText = "This wizard helps you perform a task by providing an intuitive interface for configuring how the task is performed.";

		/// <summary>
		/// Initializes a new instance of the IntroductionPage class.
		/// </summary>
		public IntroductionPage()
		{
			// Change base property defaults
			Text = "Welcome to the Sample Wizard";
		}

		/// <summary>
		/// The text that informs the user of the purpose of the wizard.
		/// </summary>
		[Category("Appearance"), Description("The text that informs the user of the purpose of the wizard."), Localizable(true)]
#if NET20
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
#endif
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
		/// Overridden.
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(RightToLeft == RightToLeft.Yes, true, StringAlignment.Near, StringAlignment.Near, false))
				IndependentText.DrawText(e.Graphics, IntroductionText, Font, ClientRectangle, textFormat, ForeColor);
		}

	}
}
