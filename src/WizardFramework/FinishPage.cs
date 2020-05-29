using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Divelements.WizardFramework.Rendering;

namespace Divelements.WizardFramework
{
	/// <summary>
	/// A page designed to show a summary of activity undertaken in the wizard process.
	/// </summary>
	public class FinishPage : CoverWizardPage
	{
		// Members
		private string finishText = "You have successfully completed the Sample Wizard.";
		private string settingsHeader = "You specified the following settings:";
		private string[] names, values;

		// Events
		/// <summary>
		/// Occurs when the wizard needs to know all the settings configured by the user.
		/// </summary>
		/// <remarks>
		/// <para>You should respond to this event by calling AddNameValuePair on the event arguments passed to the handling procedure as many times
		/// as is necessary to reflect the options chosen by the user.</para>
		/// </remarks>
		public event WizardFinishPageEventHandler CollectSettings;

		/// <summary>
		/// Initializes a new instance of the FinishPage class.
		/// </summary>
		public FinishPage()
		{
			// Change base property defaults
			Text = "Completing the Sample Wizard";
			ProceedText = "To close this wizard, click Finish.";
		}

		/// <summary>
		/// Overridden. Calls the OnCollectSettings method.
		/// </summary>
		/// <param name="e">Arguments for the event handling procedure.</param>
		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);

			// Collect settings information
			WizardFinishPageEventArgs e2 = new WizardFinishPageEventArgs();
			OnCollectSettings(e2);
			e2.GetNameValuePairs(out names, out values);
		}

		/// <summary>
		/// Raises the CollectSettings event.
		/// </summary>
		/// <param name="e">Arguments for the event handling procedure.</param>
		protected virtual void OnCollectSettings(WizardFinishPageEventArgs e)
		{
			if (CollectSettings != null)
				CollectSettings(this, e);
		}

		/// <summary>
		/// The text used as an introduction to the name and value pair display of configured settings.
		/// </summary>
		[Category("Appearance"), DefaultValue("You specified the following settings:"), Description("The text used as an introduction to the name and value pair display of configured settings.")]
		public string SettingsHeader
		{
			get
			{
				return settingsHeader;
			}
			set
			{
				settingsHeader = value;
			}
		}

		/// <summary>
		/// The text that informs the user that they have completed the wizard.
		/// </summary>
		[Category("Appearance"), Description("The text that informs the user that they have completed the wizard."), Localizable(true)]
#if NET20
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
#endif
		public string FinishText
		{
			get
			{
				return finishText;
			}
			set
			{
				finishText = value;

				Invalidate();
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		[DefaultValue("To close this wizard, click Finish.")]
		public override string ProceedText
		{
			get
			{
				return base.ProceedText;
			}
			set
			{
				base.ProceedText = value;
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(RightToLeft == RightToLeft.Yes, true, StringAlignment.Near, StringAlignment.Near, false))
			{
				// Draw finish text
				IndependentText.DrawText(e.Graphics, FinishText, Font, ClientRectangle, textFormat, ForeColor);
				int finishHeight = IndependentText.MeasureText(e.Graphics, FinishText, Font, ClientRectangle.Width, textFormat).Height;
				int top = ClientRectangle.Top + finishHeight;

				if (names != null && names.Length > 0)
				{
					// Draw settings header
					IndependentText.DrawText(e.Graphics, SettingsHeader, Font, new Rectangle(0, top, ClientRectangle.Width, Font.Height), textFormat, ForeColor);
					top += Font.Height * 2;

					// Get maximum name width
					int maximumNameWidth = 0;
					foreach (string name in names)
						maximumNameWidth = Math.Max(maximumNameWidth, IndependentText.MeasureText(e.Graphics, name, Font, textFormat).Width);

					// Draw names and values
					for (int i = 0; i < names.Length; i++)
					{
						IndependentText.DrawText(e.Graphics, names[i] + ":", Font, new Rectangle(0, top, maximumNameWidth + 8, Font.Height), textFormat, ForeColor);
						IndependentText.DrawText(e.Graphics, values[i], Font, new Rectangle(maximumNameWidth + 8, top, ClientRectangle.Width - (maximumNameWidth + 8), Font.Height), textFormat, ForeColor);
						top += Font.Height;
					}
				}
			}
		}
	
	}
}
