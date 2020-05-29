using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using System.Collections;


namespace Divelements.WizardFramework
{
	internal class WizardPageBaseDesigner : ParentControlDesigner
	{
		// Members
		private DesignerVerbCollection customVerbs;
		
		// Services
		private IComponentChangeService changeService;
		private IDesignerHost designerHost;
		private ISelectionService selectionService;

		public WizardPageBaseDesigner()
		{
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);

			// Get services
			changeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
			designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
			selectionService = (ISelectionService)GetService(typeof(ISelectionService));

			// Bind events
			changeService.ComponentRemoving += new ComponentEventHandler(OnComponentRemoving);
			selectionService.SelectionChanged += new EventHandler(OnSelectionChanged);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Unbind events
				changeService.ComponentRemoving -= new ComponentEventHandler(OnComponentRemoving);
				selectionService.SelectionChanged -= new EventHandler(OnSelectionChanged);
			}

			base.Dispose(disposing);
		}

		public override System.Collections.IList SnapLines
		{
			get
			{
				ArrayList snapLines = new ArrayList();

				int x = Control.ClientRectangle.Left + Wizard97Metrics.LayoutMarginSize;
				snapLines.Add(new SnapLine(SnapLineType.Left, x));
				x = Control.ClientRectangle.Right - Wizard97Metrics.LayoutMarginSize;
				snapLines.Add(new SnapLine(SnapLineType.Right, x));

				return snapLines;
			}
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				return SelectionRules.Visible | SelectionRules.Locked;
			}
		}

		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (customVerbs == null)
				{
					DesignerVerb verbAddPageAfter = new DesignerVerb("Add Page &After", new EventHandler(OnVerbAddPageAfter));
					DesignerVerb verbAddPageBefore = new DesignerVerb("Add Page &Before", new EventHandler(OnVerbAddPageBefore));
					customVerbs = new DesignerVerbCollection(new DesignerVerb[] { verbAddPageAfter, verbAddPageBefore });
					base.Verbs.AddRange(customVerbs);
				}

				return base.Verbs;
			}
		}

		private void OnVerbAddPageAfter(object sender, System.EventArgs e)
		{
			AddPage(true);
		}

		private void OnVerbAddPageBefore(object sender, System.EventArgs e)
		{
			AddPage(false);
		}

		private void AddPage(bool after)
		{
			Wizard wizard = (Wizard)Page.Parent;

			// Remember old "next" page
			WizardPageBase oldNextPage = Page.NextPage, oldPreviousPage = Page.PreviousPage;

			// Create new page and set it as the next page from this one
			DesignerTransaction transaction = designerHost.CreateTransaction("Add Wizard Page");
			try
			{
				WizardPageBase newPage = (WizardPageBase)designerHost.CreateComponent(typeof(WizardPage));
				((ComponentDesigner)designerHost.GetDesigner(newPage)).OnSetComponentDefaults();
				if (after)
				{
					TypeDescriptor.GetProperties(newPage)["PreviousPage"].SetValue(newPage, Page);
					TypeDescriptor.GetProperties(newPage)["NextPage"].SetValue(newPage, oldNextPage);
					TypeDescriptor.GetProperties(Page)["NextPage"].SetValue(Page, newPage);
					TypeDescriptor.GetProperties(oldNextPage)["PreviousPage"].SetValue(oldNextPage, newPage);
				}
				else
				{
					TypeDescriptor.GetProperties(newPage)["PreviousPage"].SetValue(newPage, oldPreviousPage);
					TypeDescriptor.GetProperties(newPage)["NextPage"].SetValue(newPage, Page);
					TypeDescriptor.GetProperties(Page)["PreviousPage"].SetValue(Page, newPage);
					TypeDescriptor.GetProperties(oldPreviousPage)["NextPage"].SetValue(oldPreviousPage, newPage);
				}

				// Add the new page to the wizard
				changeService.OnComponentChanging(wizard, TypeDescriptor.GetProperties(wizard)["Controls"]);
				wizard.Controls.Add(newPage);
				changeService.OnComponentChanged(wizard, TypeDescriptor.GetProperties(wizard)["Controls"], null, null);

				// Select the new page
				selectionService.SetSelectedComponents(new object[] { newPage });
			}
			catch
			{
				transaction.Cancel();
			}
			finally
			{
				transaction.Commit();
			}
		}

		private WizardPageBase Page
		{
			get
			{
				return Control as WizardPageBase;
			}
		}

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			base.OnPaintAdornments(pe);

			// Draw fake border
			using(Pen pen = new Pen(SystemColors.ControlDark))
			{
				pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
				Rectangle bounds = Page.ClientRectangle;
				bounds.Width--;
				bounds.Height--;
				pe.Graphics.DrawRectangle(pen, bounds);
			}
		}

		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
			if (e.Component == Component)
			{
				// Since we're removing this page, we need to hook up the surrounding pages so they bypass this one
				Wizard wizard = Page.Wizard;
				WizardPageBase previousPage = Page.PreviousPage;
				WizardPageBase nextPage = Page.NextPage;

				if (wizard != null)
				{
					// Make all pages that referenced this page as their next page have our next page instead
					foreach (WizardPageBase page in wizard.GetPagesWithNextPage(Page))
						TypeDescriptor.GetProperties(page)["NextPage"].SetValue(page, nextPage);
				
					// Make all pages that referenced this page as their previous page have our previous page instead
					foreach (WizardPageBase page in wizard.GetPagesWithPreviousPage(Page))
						TypeDescriptor.GetProperties(page)["PreviousPage"].SetValue(page, previousPage);
				}
			}
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			// If we have been made the primary selection, ensure the wizard is showing this page
			if (selectionService.PrimarySelection == Page && Page.Wizard.SelectedPage != Page)
				Page.Wizard.SelectedPage = Page;
		}

	}
}
