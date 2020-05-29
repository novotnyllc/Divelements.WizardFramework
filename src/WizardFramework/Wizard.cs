using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Divelements.WizardFramework.Rendering;

namespace Divelements.WizardFramework
{
	/// <summary>
	/// Provides a Wizard-97 style interface for an application with a number of discreet steps.
	/// </summary>
	[Designer(typeof(WizardDesigner)), ToolboxBitmap(typeof(Wizard)), DefaultEvent("Cancel")]
	public class Wizard : ContainerControl
	{
		// Members
		private bool autoCloseModalForm = true, animatePageTransitions = true;
		private int animationTime = 300;
		private MarginImageAppearance marginImageAppearance = MarginImageAppearance.Stretch;
		private Form ownerForm;
		private WizardUserExperienceType userExperienceType;
		private WizardUserExperience userExperience;

		// Text
		private string previousText = "< &Back";
		private string nextText = "&Next >";
		private string cancelText = "Cancel";
		private string finishText = "Finish";
		private string helpText = "&Help";

		// Layout
		private CustomButton previousButton;
		private Button nextButton, cancelButton, helpButton;
		private WizardPageBase selectedPage;
		private bool busy = false;
		private bool helpVisible = false;

		// Drawing
		private Image marginImage, bannerImage;

		// Events
		/// <summary>
		/// Occurs when the wizard is cancelled by the user.
		/// </summary>
		public event EventHandler Cancel;
		/// <summary>
		/// Occurs when the wizard is finished by the user.
		/// </summary>
		public event EventHandler Finish;

		/// <summary>
		/// Initializes a new instance of the Wizard class.
		/// </summary>
		public Wizard()
		{
			// Set drawing styles
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);

			// Create buttons
			previousButton = new CustomButton();
			previousButton.FlatStyle = FlatStyle.System;
			previousButton.Click += new EventHandler(OnPreviousButtonClick);
			nextButton = new Button();
			nextButton.FlatStyle = FlatStyle.System;
			nextButton.Click += new EventHandler(OnNextButtonClick);
			cancelButton = new Button();
			cancelButton.FlatStyle = FlatStyle.System;
			cancelButton.Click += new EventHandler(OnCancelButtonClick);
			helpButton = new Button();
			helpButton.FlatStyle = FlatStyle.System;
			helpButton.Click += new EventHandler(OnHelpButtonClick);
			helpButton.Visible = false;

			// Defaults
			UserExperienceType = WizardUserExperienceType.Automatic;
			Dock = DockStyle.Fill;
			Controls.AddRange(new Control[] { previousButton, nextButton, cancelButton, helpButton });

			// Set initial layout metrics
			SetButtonTextAndAvailability();
		}

		/// <summary>
		/// Raises the Cancel event.
		/// </summary>
		/// <param name="e">Arguments for the event handling procedure.</param>
		protected virtual void OnCancel(System.EventArgs e)
		{
			// If we're in a modal form, set its dialog result
			if (AutoCloseModalForm)
			{
				Form f = FindForm();
				if (f != null && f.Modal)
					f.DialogResult = DialogResult.Cancel;
			}

			// Raise the cancel event
			if (Cancel != null)
				Cancel(this, e);
		}

		/// <summary>
		/// Raises the Finish event.
		/// </summary>
		/// <param name="e">Arguments for the event handling procedure.</param>
		protected virtual void OnFinish(System.EventArgs e)
		{
			// If we're in a modal form, set its dialog result
			if (AutoCloseModalForm)
			{
				Form f = FindForm();
				if (f != null && f.Modal)
					f.DialogResult = DialogResult.OK;
			}

			// Raise the finish event
			if (Finish != null)
				Finish(this, e);
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Clean up user experience
				UserExperience.Dispose();
			}

			base.Dispose(disposing);
		}

		private void OnUserExperienceVisualsChanged(object sender, EventArgs e)
		{
			Invalidate(true);
		}

		private void OnUserExperienceLayoutChanged(object sender, EventArgs e)
		{
			Invalidate(true);
			PerformLayout();
		}

		private void OnOwnerFormTextChanged(object sender, EventArgs e)
		{
			Invalidate();
		}

		#region Properties

		/// <summary>
		/// Indicates the type of user experience offered by the wizard.
		/// </summary>
		[Category("Appearance"), DefaultValue(typeof(WizardUserExperienceType), "Automatic"), Description("Indicates the type of user experience offered by the wizard.")]
		public WizardUserExperienceType UserExperienceType
		{
			get { return userExperienceType; }
			set
			{
				if (value != userExperienceType)
				{
					userExperienceType = value;
					switch (userExperienceType)
					{
						case WizardUserExperienceType.Automatic:
						default:
							UserExperience = Environment.OSVersion.Version.Major >= 6 ? (WizardUserExperience)new Wizard07UserExperience() : (WizardUserExperience)new Wizard97UserExperience();
							break;
						case WizardUserExperienceType.Wizard97:
							UserExperience = new Wizard97UserExperience();
							break;
						case WizardUserExperienceType.WizardAero:
							UserExperience = new Wizard07UserExperience();
							break;
					}
				}
			}
		}

		/// <summary>
		/// The user experience in use by the wizard.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WizardUserExperience UserExperience
		{
			get { return userExperience; }
			set
			{
				// Validate
				if (value == null)
					throw new ArgumentNullException("value");
				if (value != userExperience && value.Wizard != null)
					throw new ArgumentException("value");

				// Unhook
				if (userExperience != null)
				{
					userExperience.VisualsChanged -= new EventHandler(OnUserExperienceVisualsChanged);
					userExperience.LayoutChanged -= new EventHandler(OnUserExperienceLayoutChanged);
					userExperience.SetWizard(null);
					userExperience.Dispose();
				}

				userExperience = value;

				// Hook
				userExperience.SetWizard(this);
				userExperience.VisualsChanged += new EventHandler(OnUserExperienceVisualsChanged);
				userExperience.LayoutChanged += new EventHandler(OnUserExperienceLayoutChanged);

				userExperience.ApplyTabIndices(previousButton, nextButton, cancelButton, helpButton);
				PerformLayout();
				Invalidate();
			}
		}

		/// <summary>
		/// Indicates how the margin image is drawn.
		/// </summary>
		[Category("Appearance"), DefaultValue(typeof(MarginImageAppearance), "Stretch"), Description("Indicates how the margin image is drawn.")]
		public MarginImageAppearance MarginImageAppearance
		{
			get { return marginImageAppearance; }
			set
			{
				marginImageAppearance = value;

				// Redraw if necessary
				if (SelectedPage is CoverWizardPage)
					Invalidate();
			}
		}

		/// <summary>
		/// The image displayed on the left of introduction and finish pages.
		/// </summary>
		/// <remarks>
		/// <para>The image should have a width of 164 pixels.</para>
		/// </remarks>
		[Category("Appearance"), DefaultValue(typeof(Image), null), AmbientValue(typeof(Image), null), Description("The image displayed on the left of introduction and finish pages. The image should have a width of 164 pixels.")]
		public Image MarginImage
		{
			get { return marginImage; }
			set
			{
				marginImage = value;

				// Redraw if necessary
				if (SelectedPage is CoverWizardPage)
					Invalidate();
			}
		}

		/// <summary>
		/// The image displayed at the top right on content pages.
		/// </summary>
		/// <remarks>
		/// <para>The image should be 49x49 pixels.</para>
		/// </remarks>
		[Category("Appearance"), DefaultValue(typeof(Image), null), AmbientValue(typeof(Image), null), Description("The image displayed at the top right on content pages. The image should be 49 pixels square.")]
		public Image BannerImage
		{
			get { return bannerImage; }
			set
			{
				bannerImage = value;

				// Redraw if necessary
				if (SelectedPage is WizardPage)
					Invalidate();
			}
		}

		private bool Busy
		{
			get { return busy; }
			set
			{
				if (value != busy)
				{
					busy = value;

					SetButtonTextAndAvailability();
					Cursor = Busy ? Cursors.WaitCursor : null;
				}
			}
		}

		/// <summary>
		/// Indicates whether a Help button is displayed alongside the normal buttons.
		/// </summary>
		[Category("Appearance"), DefaultValue(false), Description("Indicates whether a Help button is displayed alongside the normal buttons.")]
		public bool HelpVisible
		{
			get { return helpVisible; }
			set
			{
				helpVisible = value;
				helpButton.Visible = value;
			}
		}

		internal CustomButton PreviousButton
		{
			get { return previousButton; }
		}

		internal Rectangle PreviousButtonBounds
		{
			get
			{
				return previousButton.Bounds;
			}
		}

		internal Rectangle NextButtonBounds
		{
			get
			{
				return nextButton.Bounds;
			}
		}

		/// <summary>
		/// Indicates the amount of time, in milliseconds, it takes for the transition between pages to animate.
		/// </summary>
		[Category("Behavior"), DefaultValue(300), Description("Indicates the amount of time, in milliseconds, it takes for the transition between pages to animate.")]
		public int AnimationTime
		{
			get { return animationTime; }
			set
			{
				// Validate
				if (value < 50 || value > 1000)
					throw new ArgumentException("value");

				animationTime = value;
			}
		}

		/// <summary>
		/// Indicates whether animations are used for transitions between wizard pages.
		/// </summary>
		[Category("Behavior"), DefaultValue(true), Description("Indicates whether animations are used for transitions between wizard pages.")]
		public bool AnimatePageTransitions
		{
			get { return animatePageTransitions; }
			set { animatePageTransitions = value; }
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override ISite Site
		{
			get { return base.Site; }
			set
			{
				base.Site = value;

				if (value != null)
				{
					// Try to find owner form
					System.ComponentModel.Design.IDesignerHost designerHost = (System.ComponentModel.Design.IDesignerHost)GetService(typeof(System.ComponentModel.Design.IDesignerHost));
					if (designerHost != null && designerHost.RootComponent is Form)
						OwnerForm = (Form)designerHost.RootComponent;
				}
			}
		}

		/// <summary>
		/// The form on which the wizard resides.
		/// </summary>
		[Browsable(false)]
		public Form OwnerForm
		{
			get { return ownerForm; }
			set
			{
				if (value != ownerForm)
				{
					// Unhook
					if (ownerForm != null)
						ownerForm.TextChanged -= new EventHandler(OnOwnerFormTextChanged);

					ownerForm = value;
					userExperience.OnOwnerFormChanged();

					// Hook
					if (ownerForm != null)
						ownerForm.TextChanged += new EventHandler(OnOwnerFormTextChanged);
				}
			}
		}

		/// <summary>
		/// Indicates whether the Wizard will automatically close the form on finish or cancel if it is displayed modally.
		/// </summary>
		[Category("Behavior"), DefaultValue(true), Description("Indicates whether the Wizard will automatically close the form on finish or cancel if it is displayed modally.")]
		public bool AutoCloseModalForm
		{
			get { return autoCloseModalForm; }
			set { autoCloseModalForm = value; }
		}

		/// <summary>
		/// Indicates the text that is used on the Previous button.
		/// </summary>
		[Category("Buttons"), DefaultValue("< &Back"), Description("Indicates the text that is used on the Previous button."), Localizable(true)]
		public string PreviousText
		{
			get { return previousText; }
			set
			{
				// Validate
				if (value == null)
					value = string.Empty;

				previousText = value;
				SetButtonTextAndAvailability();
			}
		}

		/// <summary>
		/// Indicates the text that is used on the Next button.
		/// </summary>
		[Category("Buttons"), DefaultValue("&Next >"), Description("Indicates the text that is used on the Next button."), Localizable(true)]
		public string NextText
		{
			get { return nextText; }
			set
			{
				// Validate
				if (value == null)
					value = string.Empty;

				nextText = value;
				SetButtonTextAndAvailability();
			}
		}

		/// <summary>
		/// Indicates the text that is used on the Cancel button.
		/// </summary>
		[Category("Buttons"), DefaultValue("Cancel"), Description("Indicates the text that is used on the Cancel button."), Localizable(true)]
		public string CancelText
		{
			get { return cancelText; }
			set
			{
				// Validate
				if (value == null)
					value = string.Empty;

				cancelText = value;
				SetButtonTextAndAvailability();
			}
		}

		/// <summary>
		/// Indicates the text that is used on the Finish button.
		/// </summary>
		[Category("Buttons"), DefaultValue("Finish"), Description("Indicates the text that is used on the Finish button."), Localizable(true)]
		public string FinishText
		{
			get { return finishText; }
			set
			{
				// Validate
				if (value == null)
					value = string.Empty;

				finishText = value;
				SetButtonTextAndAvailability();
			}
		}

		/// <summary>
		/// Indicates the text that is used on the Help button.
		/// </summary>
		[Category("Buttons"), DefaultValue("&Help"), Description("Indicates the text that is used on the Help button."), Localizable(true)]
		public string HelpText
		{
			get { return helpText; }
			set
			{
				// Validate
				if (value == null)
					value = string.Empty;

				helpText = value;
				SetButtonTextAndAvailability();
			}
		}

		/// <summary>
		/// The active wizard page.
		/// </summary>
		[Category("Paging"), Description("The active wizard page."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WizardPageBase SelectedPage
		{
			get { return selectedPage; }
			set
			{
				// Ensure page belongs to wizard
				if (value != null && !Controls.Contains(value))
					throw new ArgumentException("The specified page does not belong to the wizard.");

				// Hide previously selected page
				if (selectedPage != null)
					selectedPage.SetActualVisibility(false);

				selectedPage = value;
				Invalidate();
				
				// Show new page
				if (selectedPage != null)
				{
					selectedPage.Bounds = DisplayRectangle;
					selectedPage.SetActualVisibility(true);
				}
				SetButtonTextAndAvailability();
			}
		}

		#endregion

		private void SetFocusAppropriately()
		{
			if (SelectedPage != null)
			{
				ActiveControl = null;
				SelectedPage.SelectNextControl(null, true, true, true, true);
				if (ActiveControl != null)
					ActiveControl.Focus();
				else if (nextButton.Enabled)
					nextButton.Focus();
			}
		}

		private void SetAcceptCancelButtons()
		{
			Form parentForm = FindForm();
			if (parentForm != null && !DesignMode)
			{
				parentForm.AcceptButton = nextButton;
				parentForm.CancelButton = cancelButton;
				nextButton.DialogResult = DialogResult.None;
				cancelButton.DialogResult = DialogResult.None;
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			// If this was the selected page, have no selected page
			if (SelectedPage == e.Control)
			{
				if (Controls.Contains(SelectedPage.PreviousPage))
					SelectedPage = SelectedPage.PreviousPage;
				else
					SelectedPage = FindAppropriateFirstPage();
			}

			base.OnControlRemoved(e);
		}

		private WizardPageBase FindAppropriateFirstPage()
		{
			// Find the first page without a previous page
			foreach (Control c in Controls)
			{
				WizardPageBase page = c as WizardPageBase;
				if (page != null && page.PreviousPage == null)
					return page;
			}

			return null;
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// If we do not have a selected page at this time, set to the default
			if (SelectedPage == null)
			{
				SelectedPage = FindAppropriateFirstPage();
				if (SelectedPage != null)
					SelectedPage.OnAfterDisplay(EventArgs.Empty);
			}

			SetAcceptCancelButtons();
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			// If this is a page and the conditions are right, set it as the selected page
			WizardPageBase page = e.Control as WizardPageBase;
			if (page != null)
			{
				page.SetActualVisibility(false);
				if (SelectedPage == null && IsHandleCreated)
				{
					SelectedPage = page;
					page.OnAfterDisplay(EventArgs.Empty);
				}
			}

			base.OnControlAdded(e);
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		public override Rectangle DisplayRectangle
		{
			get
			{
				if (SelectedPage == null)
					return ClientRectangle;
				else if (SelectedPage is CoverWizardPage)
					return userExperience.GetCoverPageBounds(SelectedPage, ClientRectangle);
				else
					return userExperience.GetContentPageBounds(SelectedPage, ClientRectangle);
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			userExperience.OnOwnerFormChanged();
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnLayout(LayoutEventArgs levent)
		{
			// Set selected page bounds
			if (SelectedPage != null)
				SelectedPage.Bounds = DisplayRectangle;

			userExperience.LayoutButtons(ClientRectangle, cancelButton, nextButton, previousButton, helpButton);
		}

		internal void SetButtonTextAndAvailability()
		{
			// Apply text
			previousButton.Text = previousText;
			nextButton.Text = (SelectedPage == null || SelectedPage.NextPage != null) ? nextText : finishText;
			cancelButton.Text = cancelText;
			helpButton.Text = helpText;

			// Apply enabled state
			if (Busy)
			{
				previousButton.Enabled = false;
				nextButton.Enabled = false;
				cancelButton.Enabled = false;
				helpButton.Enabled = false;
			}
			else
			{
				previousButton.Enabled = SelectedPage != null && SelectedPage.PreviousPage != null && (SelectedPage.AllowMovePrevious || DesignMode);
				nextButton.Enabled = SelectedPage != null && (SelectedPage.AllowMoveNext || DesignMode);
				cancelButton.Enabled = SelectedPage != null && SelectedPage.AllowCancel;
				helpButton.Enabled = SelectedPage != null;
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if (OwnerFormSubclass.IsCompositionEnabled)
			{
				Rectangle bounds = UserExperience.GetAeroGlassBounds(ClientRectangle);
				if (bounds != Rectangle.Empty)
				{
					e.Graphics.ExcludeClip(bounds);
					using (SolidBrush brush = new SolidBrush(BackColor))
						e.Graphics.FillRectangle(brush, ClientRectangle);
				}
				else
					base.OnPaintBackground(e);
			}
			else
				base.OnPaintBackground(e);
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			using (RenderingContext context = new RenderingContext(e.Graphics, Font, RightToLeft == RightToLeft.Yes, ShowKeyboardCues, ForeColor, SystemColors.GrayText))
				userExperience.DrawWizardClient(context, ClientRectangle, SelectedPage);
		}

		/// <summary>
		/// Causes the wizard to advance to the next page.
		/// </summary>
		public virtual void GoNext()
		{
			// Validate
			if (SelectedPage == null)
				return;

			Busy = true;
			try
			{
				WizardPageBase currentPage = SelectedPage, nextPage;

				// Validate the currently displayed page
				CancelEventArgs e = new CancelEventArgs();
				currentPage.OnBeforeMoveNext(e);
				if (e.Cancel || SelectedPage != currentPage)
					return;
				nextPage = currentPage.NextPage;

				// If this is the last page, finish
				if (nextPage == null)
				{
					OnFinish(EventArgs.Empty);
					return;
				}

				// Prepare the next page
				nextPage.OnBeforeDisplay(EventArgs.Empty);
				if (SelectedPage != currentPage)
					return;

				// Animate if necessary
				AnimationForm animationForm = null;
				if (ShouldApplyTransitionAnimations)
				{
					Color transitionColor;
					Rectangle transitionBounds = UserExperience.GetTransitionBounds(ClientRectangle, out transitionColor);
					animationForm = new AnimationForm();
					animationForm.BackColor = transitionColor;
					animationForm.Bounds = new Rectangle(PointToScreen(transitionBounds.Location), transitionBounds.Size); ;
					Win32.SetWindowPos(animationForm.Handle, Handle, 0, 0, 0, 0, Win32.SWP_NOACTIVATE | Win32.SWP_HIDEWINDOW | Win32.SWP_NOMOVE | Win32.SWP_NOSIZE);
					Win32.AnimateWindow(animationForm.Handle, AnimationTime / 3, Win32.AnimateWindowFlags.AW_BLEND);
				}

				// Onward!
				SelectedPage = nextPage;

				// Animate if necessary
				if (animationForm != null)
				{
					Refresh();
					Win32.AnimateWindow(animationForm.Handle, AnimationTime / 3 * 2, Win32.AnimateWindowFlags.AW_HIDE | Win32.AnimateWindowFlags.AW_BLEND);
					animationForm.Dispose();
				}

				// Raise the afterdisplay event
				nextPage.OnAfterDisplay(EventArgs.Empty);
			}
			finally
			{
				Busy = false;
				SetFocusAppropriately();
			}
		}

		/// <summary>
		/// Causes the wizard to go back to the last page.
		/// </summary>
		public virtual void GoBack()
		{
			// Validate
			if (SelectedPage == null)
				return;

			Busy = true;
			try
			{
				WizardPageBase currentPage = SelectedPage;

				// Validate the currently displayed page
				CancelEventArgs e = new CancelEventArgs();
				currentPage.OnBeforeMoveBack(e);
				if (e.Cancel || SelectedPage != currentPage)
					return;
				if (currentPage.PreviousPage == null)
					return;

				// Animate if necessary
				AnimationForm animationForm = null;
				if (ShouldApplyTransitionAnimations)
				{
					Color transitionColor;
					Rectangle transitionBounds = UserExperience.GetTransitionBounds(ClientRectangle, out transitionColor);
					animationForm = new AnimationForm();
					animationForm.BackColor = transitionColor;
					animationForm.Bounds = new Rectangle(PointToScreen(transitionBounds.Location), transitionBounds.Size); ;
					Win32.SetWindowPos(animationForm.Handle, Handle, 0, 0, 0, 0, Win32.SWP_NOACTIVATE | Win32.SWP_HIDEWINDOW | Win32.SWP_NOMOVE | Win32.SWP_NOSIZE);
					Win32.AnimateWindow(animationForm.Handle, AnimationTime / 3, Win32.AnimateWindowFlags.AW_BLEND);
				}

				// Backward!
				SelectedPage = currentPage.PreviousPage;

				// Animate if necessary
				if (animationForm != null)
				{
					Refresh();
					Win32.AnimateWindow(animationForm.Handle, AnimationTime / 3 * 2, Win32.AnimateWindowFlags.AW_HIDE | Win32.AnimateWindowFlags.AW_BLEND);
					animationForm.Dispose();
				}
			}
			finally
			{
				Busy = false;
				SetFocusAppropriately();
			}
		}

		private bool ShouldApplyTransitionAnimations
		{
			get
			{
				return AnimatePageTransitions && !Win32.DisableRichDrawing && OSFeature.Feature.IsPresent(OSFeature.LayeredWindows);
			}
		}

		private class AnimationForm : Form
		{
			public AnimationForm()
			{
				ShowInTaskbar = false;
				FormBorderStyle = FormBorderStyle.None;
				StartPosition = FormStartPosition.Manual;
			}
		}

		private void OnPreviousButtonClick(object sender, EventArgs e)
		{
			GoBack();
		}

		private void OnNextButtonClick(object sender, EventArgs e)
		{
			GoNext();
		}

		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			OnCancel(EventArgs.Empty);
		}

		#region Base Properties

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(497, 360);
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		[DefaultValue(typeof(DockStyle), "Fill")]
		public override DockStyle Dock
		{
			get { return base.Dock; }
			set { base.Dock = value; }
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		[Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		[Browsable(false)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		[Browsable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		#endregion

		/// <summary>
		/// Finds all pages that reference the specified page in their NextPage property.
		/// </summary>
		/// <param name="nextPage">The page for which a reference must be found.</param>
		/// <returns>An array containing pages matching the criteria.</returns>
		public WizardPageBase[] GetPagesWithNextPage(WizardPageBase nextPage)
		{
			return GetPagesWithPage(nextPage, true);
		}

		/// <summary>
		/// Finds all pages that reference the specified page in their PreviousPage property.
		/// </summary>
		/// <param name="previousPage">The page for which a reference must be found.</param>
		/// <returns>An array containing pages matching the criteria.</returns>
		public WizardPageBase[] GetPagesWithPreviousPage(WizardPageBase previousPage)
		{
			return GetPagesWithPage(previousPage, false);
		}

		private WizardPageBase[] GetPagesWithPage(WizardPageBase targetPage, bool next)
		{
			ArrayList pages = new ArrayList();

			// Go through all pages looking for ones referencing this page in the specified way
			foreach(Control c in Controls)
			{
				if (c is WizardPageBase)
				{
					WizardPageBase page = (WizardPageBase)c;
					if ((page.NextPage == targetPage && next) || (page.PreviousPage == targetPage && !next))
						pages.Add(page);
				}
			}

			// Return array of the found pages
			return (WizardPageBase[])pages.ToArray(typeof(WizardPageBase));
		}

		private void OnHelpButtonClick(object sender, EventArgs e)
		{
			OnHelpRequested(new HelpEventArgs(Cursor.Position));
		}

		/// <summary>
		/// Sets the PreviousPage and NextPage properties appropriately for all specified pages, assuming a linear progression through the array.
		/// </summary>
		/// <param name="pages">An array of wizard pages.</param>
		/// <param name="enforceStartAndFinish">Sets the PreviousPage of the first page and the NextPage of the last page to null.</param>
		public static void SetPageOrder(WizardPageBase[] pages, bool enforceStartAndFinish)
		{
			// Validate
			if (pages == null)
				throw new ArgumentNullException("pages");
			foreach (WizardPageBase page in pages)
			{
				if (page == null)
					throw new ArgumentNullException("pages");
			}

			WizardPageBase previousPage = null, nextPage = null;
			for (int i = 0; i < pages.Length; i++)
			{
				// Set previous page
				if (i > 0 || enforceStartAndFinish)
					pages[i].PreviousPage = previousPage;

				// Set next page
				nextPage = i < pages.Length - 1 ? pages[i + 1] : null;
				if (i < pages.Length - 1 || enforceStartAndFinish)
					pages[i].NextPage = nextPage;

				previousPage = pages[i];
			}
		}

		/// <summary>
		/// Sets the PreviousPage and NextPage properties appropriately for all specified pages, assuming a linear progression through the array.
		/// </summary>
		/// <param name="pages">An array of wizard pages.</param>
		public static void SetPageOrder(WizardPageBase[] pages)
		{
			SetPageOrder(pages, false);
		}

		/// <summary>
		/// Configures a relationship between two wizard pages where the first navigates to the second, and vice versa.
		/// </summary>
		/// <param name="firstPage">The first page, that will navigate to the second page upon pressing Next.</param>
		/// <param name="secondPage">The second page, that will navigate to the first page upon pressing Back.</param>
		public static void SetPageOrder(WizardPageBase firstPage, WizardPageBase secondPage)
		{
			// Validate
			if (firstPage == null)
				throw new ArgumentNullException("firstPage");
			if (secondPage == null)
				throw new ArgumentNullException("secondPage");

			SetPageOrder(new WizardPageBase[] { firstPage, secondPage });
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case Win32.WM_ERASEBKGND:
					if (OwnerFormSubclass.IsCompositionEnabled)
					{
						m.Result = new IntPtr(1);
						return;
					}
					break;
			}

			base.WndProc(ref m);
		}

	}
}
