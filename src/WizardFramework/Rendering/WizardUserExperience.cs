using System;
using System.Windows.Forms;
using System.Drawing;

namespace Divelements.WizardFramework.Rendering
{
	/// <summary>
	/// Provides a base class for wizard user experience definitions to derive from.
	/// </summary>
	public abstract class WizardUserExperience : IDisposable
	{
		// Members
		private Wizard wizard;

		// Events
		/// <summary>
		/// Occurs when the visuals of the user experience have changed.
		/// </summary>
		public event EventHandler VisualsChanged;
		/// <summary>
		/// Occurs when the layout of the user experience has changed.
		/// </summary>
		public event EventHandler LayoutChanged;

		/// <summary>
		/// Initializes a new instance of the WizardUserExperience class.
		/// </summary>
		protected WizardUserExperience()
		{
			// Hook user preference events
			Microsoft.Win32.SystemEvents.UserPreferenceChanged += new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged);
		}

		/// <summary>
		/// Calculates the bounds of the area occupied by Aero glass.
		/// </summary>
		/// <param name="bounds">The bounds of the wizard client area.</param>
		/// <returns>A rectangle identifying the glass bounds.</returns>
		public virtual Rectangle GetAeroGlassBounds(Rectangle bounds)
		{
			return Rectangle.Empty;
		}

		/// <summary>
		/// Calculates the bounds of the area used for transitions between pages.
		/// </summary>
		/// <param name="bounds">The bounds of the wizard client area.</param>
		/// <param name="transitionColor">The color that should be used for the transition.</param>
		/// <returns>The area that will be used for the transition.</returns>
		public abstract Rectangle GetTransitionBounds(Rectangle bounds, out Color transitionColor);

		/// <summary>
		/// Raises the VisualsChanged event.
		/// </summary>
		/// <param name="e">The arguments associated with the event.</param>
		protected virtual void OnVisualsChanged(EventArgs e)
		{
			if (VisualsChanged != null)
				VisualsChanged(this, e);
		}

		/// <summary>
		/// Raises the LayoutChanged event.
		/// </summary>
		/// <param name="e">The arguments associated with the event.</param>
		protected virtual void OnLayoutChanged(EventArgs e)
		{
			if (LayoutChanged != null)
				LayoutChanged(this, e);
		}

		/// <summary>
		/// Draws the client area of a wizard.
		/// </summary>
		/// <param name="context">The rendering context in which to draw.</param>
		/// <param name="bounds">The bounds of the wizard client area.</param>
		/// <param name="page">The selected page.</param>
		public abstract void DrawWizardClient(RenderingContext context, Rectangle bounds, WizardPageBase page);

		/// <summary>
		/// Draws the client area of a wizard page.
		/// </summary>
		/// <param name="graphics">The Graphics object with which to draw.</param>
		/// <param name="bounds">The bounds of the wizard client area.</param>
		/// <param name="page">The page being drawn.</param>
		public abstract void DrawPageClient(Graphics graphics, Rectangle bounds, WizardPageBase page);

		/// <summary>
		/// Lays out wizard buttons.
		/// </summary>
		/// <param name="bounds">The bounds of the wizard client area.</param>
		/// <param name="cancelButton">The cancel button.</param>
		/// <param name="nextButton">The next button.</param>
		/// <param name="previousButton">The previous button.</param>
		/// <param name="helpButton">The help button.</param>
		public abstract void LayoutButtons(Rectangle bounds, ButtonBase cancelButton, ButtonBase nextButton, ButtonBase previousButton, ButtonBase helpButton);

		/// <summary>
		/// Calculates the bounds of a content page.
		/// </summary>
		/// <param name="page">The page to be laid out.</param>
		/// <param name="bounds">The bounds of the wizard client area.</param>
		/// <returns>The bounds of a content page.</returns>
		public abstract Rectangle GetContentPageBounds(WizardPageBase page, Rectangle bounds);

		/// <summary>
		/// Calculates the bounds of a cover page.
		/// </summary>
		/// <param name="page">The page to be laid out.</param>
		/// <param name="bounds">The bounds of the wizard client area.</param>
		/// <returns>The bounds of a cover page.</returns>
		public abstract Rectangle GetCoverPageBounds(WizardPageBase page, Rectangle bounds);

		/// <summary>
		/// Applies user experience-specific tab indices to the standard wizard buttons.
		/// </summary>
		/// <param name="backButton">The back button.</param>
		/// <param name="nextButton">The next button.</param>
		/// <param name="cancelButton">The cancel button.</param>
		/// <param name="helpButton">The help button.</param>
		public abstract void ApplyTabIndices(Button backButton, Button nextButton, Button cancelButton, Button helpButton);

		/// <summary>
		/// The wizard with which the user experience is associated.
		/// </summary>
		public Wizard Wizard
		{
			get { return wizard; }
		}

		internal void SetWizard(Wizard value)
		{
			Wizard oldWizard = wizard;
			this.wizard = value;
			OnWizardChanged(oldWizard, wizard);
		}

		/// <summary>
		/// Called when the OwnerForm property on the wizard changes.
		/// </summary>
		protected internal virtual void OnOwnerFormChanged()
		{
		}

		/// <summary>
		/// Called when the wizard that the user experience is providing services for changes.
		/// </summary>
		/// <param name="oldWizard">The old wizard.</param>
		/// <param name="newWizard">The new wizard.</param>
		protected virtual void OnWizardChanged(Wizard oldWizard, Wizard newWizard)
		{
		}

		private void OnUserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
		{
			if (e.Category == Microsoft.Win32.UserPreferenceCategory.Color)
				OnSystemColorsChanged();
		}

		/// <summary>
		/// Called when the system colors are changed by the user.
		/// </summary>
		protected virtual void OnSystemColorsChanged()
		{
		}

		/// <summary>
		/// Releases the resources consumed by the object.
		/// </summary>
		public virtual void Dispose()
		{
			// Unhook user preference events
			Microsoft.Win32.SystemEvents.UserPreferenceChanged -= new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged);
		}

	}
}
