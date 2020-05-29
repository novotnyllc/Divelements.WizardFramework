using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Divelements.WizardFramework
{
	/// <summary>
	/// The base class from which all wizard pages derive.
	/// </summary>
	/// <remarks>
	/// <para>You should not inherit from this class directly. Instead, inherit from either CoverWizardPage or WizardPage or their derivatives.</para>
	/// </remarks>
	[Designer(typeof(WizardPageBaseDesigner)), DefaultEvent("BeforeDisplay"), ToolboxItem(false), Designer(typeof(System.Windows.Forms.Design.DocumentDesigner), typeof(System.ComponentModel.Design.IRootDesigner))]
	public abstract class WizardPageBase : Control
	{
		// Paging
		private WizardPageBase previousPage, nextPage;
		private bool allowMovePrevious = true, allowMoveNext = true, allowCancel = true;
		private bool settingVisibility;

		// Events
		/// <summary>
		/// Occurs when the user presses the Next button.
		/// </summary>
		/// <remarks>
		/// <para>This event gives the developer the opportunity to validate the data entered on the page, and cancel the move if necessary.</para>
		/// </remarks>
		public event WizardPageEventHandler BeforeMoveNext;
		/// <summary>
		/// Occurs when the user presses the Back button.
		/// </summary>
		/// <remarks>
		/// <para>This event gives the developer the opportunity to validate the data entered on the page, and cancel the move if necessary.</para>
		/// </remarks>
		public event WizardPageEventHandler BeforeMoveBack;
		/// <summary>
		/// Occurs after the page is displayed.
		/// </summary>
		/// <remarks>
		/// <para>This event is only raised when moving foward, as it is assumed that when moving backward the page will need no configuration.</para>
		/// </remarks>
		public event EventHandler AfterDisplay;
		/// <summary>
		/// Occurs before the page is displayed.
		/// </summary>
		/// <remarks>
		/// <para>This event is only raised when moving forward, as it is assumed that when moving backward the page will need no configuration.</para>
		/// </remarks>
		public event EventHandler BeforeDisplay;

		/// <summary>
		/// Initializes a new instance of the WizardPageBase class.
		/// </summary>
		protected WizardPageBase()
		{
			// Set drawing styles
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.Selectable | ControlStyles.ContainerControl, false);

			// This interferes wtih proper scaling for HiDPI
			//SetStyle(ControlStyles.FixedWidth | ControlStyles.FixedHeight, true);
		}

		internal void SetActualVisibility(bool visible)
		{
			settingVisibility = true;
			try
			{
				base.SetVisibleCore(visible);
			}
			finally
			{
				settingVisibility = false;
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			if (Wizard != null)
				Wizard.UserExperience.DrawPageClient(pevent.Graphics, ClientRectangle, this);
			else
				base.OnPaintBackground(pevent);
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected sealed override void SetVisibleCore(bool value)
		{
			if (!settingVisibility)
				return;
				//throw new InvalidOperationException();

			base.SetVisibleCore(value);
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		[Browsable(true)]
		public override string Text
		{
			get { return base.Text; }
			set { base.Text = value; }
		}

		/// <summary>
		/// Raises the BeforeDisplay event.
		/// </summary>
		/// <param name="e">Arguments for the event handling procedure.</param>
		protected internal virtual void OnBeforeDisplay(System.EventArgs e)
		{
			if (BeforeDisplay != null)
				BeforeDisplay(this, e);
		}

		/// <summary>
		/// Raises the AfterDisplay event.
		/// </summary>
		/// <param name="e">Arguments for the event handling procedure.</param>
		protected internal virtual void OnAfterDisplay(System.EventArgs e)
		{
			if (AfterDisplay != null)
				AfterDisplay(this, e);
		}

		/// <summary>
		/// Raises the BeforeMoveNext event.
		/// </summary>
		/// <param name="e">Arguments for the event handling procedure.</param>
		protected internal virtual void OnBeforeMoveNext(CancelEventArgs e)
		{
			if (BeforeMoveNext != null)
				BeforeMoveNext(this, e);
		}

		/// <summary>
		/// Raises the BeforeMoveBack event.
		/// </summary>
		/// <param name="e">Arguments for the event handling procedure.</param>
		protected internal virtual void OnBeforeMoveBack(CancelEventArgs e)
		{
			if (BeforeMoveBack != null)
				BeforeMoveBack(this, e);
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnTextChanged(EventArgs e)
		{
			// Update the wizard if necessary
			if (IsCurrentPage)
				Parent.Invalidate();

			base.OnTextChanged(e);
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnFontChanged(EventArgs e)
		{
			// Update the wizard if necessary
			if (IsCurrentPage)
				Parent.Invalidate();

			base.OnFontChanged(e);
		}

		/// <summary>
		/// Indicates whether the page is the selected page within the wizard.
		/// </summary>
		[Browsable(false)]
		public bool IsCurrentPage
		{
			get
			{
				return Parent is Wizard && ((Wizard)Parent).SelectedPage == this;
			}
		}

		/// <summary>
		/// The page to move to when the user presses the Back button.
		/// </summary>
		[Category("Paging"), DefaultValue(typeof(WizardPageBase), null), Description("The page to move to when the user presses the Back button.")]
		public WizardPageBase PreviousPage
		{
			get
			{
				return previousPage;
			}
			set
			{
				// Do not allow assignment of this page
				if (value == this)
					throw new ArgumentException("Cannot navigate from one page to the same page.");

				previousPage = value;

				// Update wizard if necessary
				if (IsCurrentPage)
					((Wizard)Parent).SetButtonTextAndAvailability();
			}
		}

		/// <summary>
		/// The page to move to when the user presses the Next button.
		/// </summary>
		[Category("Paging"), DefaultValue(typeof(WizardPageBase), null), Description("The page to move to when the user presses the Next button.")]
		public WizardPageBase NextPage
		{
			get
			{
				return nextPage;
			}
			set
			{
				// Do not allow assignment of this page
				if (value == this)
					throw new ArgumentException("Cannot navigate from one page to the same page.");

				nextPage = value;

				// Update wizard if necessary
				if (IsCurrentPage)
					((Wizard)Parent).SetButtonTextAndAvailability();
			}
		}

		/// <summary>
		/// Indicates whether the user will be allowed to cancel the wizard from this page.
		/// </summary>
		[Category("Behavior"), DefaultValue(true), Description("Indicates whether the user will be allowed to cancel the wizard from this page.")]
		public bool AllowCancel
		{
			get
			{
				return allowCancel;
			}
			set
			{
				allowCancel = value;

				// Update wizard if necessary
				if (IsCurrentPage)
					((Wizard)Parent).SetButtonTextAndAvailability();
			}
		}

		/// <summary>
		/// Indicates whether the user will be allowed to move forwards from this page.
		/// </summary>
		[Category("Paging"), DefaultValue(true), Description("Indicates whether the user will be allowed to move forwards from this page.")]
		public bool AllowMoveNext
		{
			get
			{
				return allowMoveNext;
			}
			set
			{
				allowMoveNext = value;

				// Update wizard if necessary
				if (IsCurrentPage)
					((Wizard)Parent).SetButtonTextAndAvailability();
			}
		}

		/// <summary>
		/// Indicates whether the user will be allowed to move backwards from the page.
		/// </summary>
		[Category("Paging"), DefaultValue(true), Description("Indicates whether the user will be allowed to move backwards from the page.")]
		public bool AllowMovePrevious
		{
			get
			{
				return allowMovePrevious;
			}
			set
			{
				allowMovePrevious = value;

				// Update wizard if necessary
				if (IsCurrentPage)
					((Wizard)Parent).SetButtonTextAndAvailability();
			}
		}

		/// <summary>
		/// The wizard to which the page belongs.
		/// </summary>
		[Browsable(false)]
		public Wizard Wizard
		{
			get
			{
				return Parent as Wizard;
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}

#region Base Properties

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

#endregion

	}
}
