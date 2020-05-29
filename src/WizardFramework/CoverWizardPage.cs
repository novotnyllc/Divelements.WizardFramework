using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Divelements.WizardFramework.Rendering;

namespace Divelements.WizardFramework
{
	/// <summary>
	/// A base class from which special pages such as IntroductionPage and FinishPage are derived.
	/// </summary>
	/// <remarks>
	/// <para>You can derive from this page to create other cover pages that show a margin and have specially-formatted headings.</para>
	/// </remarks>
	public abstract class CoverWizardPage : WizardPageBase
	{
		// Members
		private string proceedText = "To continue, click Next.";

		/// <summary>
		/// Initializes a new instance of the CoverWizardPage class.
		/// </summary>
		protected CoverWizardPage()
		{
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set
			{
				base.ForeColor = value;

				// Redraw the parent as it is using this property to draw the main heading
				if (IsCurrentPage)
					Parent.Invalidate();
			}
		}

		/// <summary>
		/// The text that informs the user how to proceed to the next page.
		/// </summary>
		[Category("Appearance"), DefaultValue("To continue, click Next."), Description("The text that informs the user how to proceed to the next page."), Localizable(true)]
#if NET20
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
#endif
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
		/// Overridden.
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (ProceedText.Length != 0)
			{
				using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(RightToLeft == RightToLeft.Yes, true, StringAlignment.Near, StringAlignment.Far, false))
					IndependentText.DrawText(e.Graphics, ProceedText, Font, ClientRectangle, textFormat, ForeColor);
			}
		}

	}
}
