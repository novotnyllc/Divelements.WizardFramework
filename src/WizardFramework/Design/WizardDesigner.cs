using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using Divelements.WizardFramework.Rendering;

namespace Divelements.WizardFramework
{
	internal class WizardDesigner : ParentControlDesigner
	{
		// Members
		private DesignerVerbCollection verbs;

		public WizardDesigner() : base()
		{
		}

		//public override void Initialize(IComponent component)
		//{
		//    base.Initialize(component);
		//}

		//protected override void Dispose(bool disposing)
		//{
		//    if (disposing)
		//    {
		//    }

		//    base.Dispose(disposing);
		//}

		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (verbs == null)
				{
					DesignerVerb verbChoosePage = new DesignerVerb("Choose &Page", new EventHandler(OnVerbChoosePage));
					verbs = new DesignerVerbCollection(new DesignerVerb[] { verbChoosePage });
				}

				return verbs;
			}
		}

		private void OnVerbChoosePage(object sender, System.EventArgs e)
		{
			ChoosePage();
		}

		private void ChoosePage()
		{
			IUIService uiService = (IUIService)GetService(typeof(IUIService));

			// Show choose new page form
			using(frmChoosePage f = new frmChoosePage(Wizard))
			{
				WizardPageBase originalSelectedPage = Wizard.SelectedPage;
				if (f.ShowDialog(uiService.GetDialogOwnerWindow()) == DialogResult.Cancel)
				{
					Wizard.SelectedPage = originalSelectedPage;
					return;
				}
				SetSelectedPage(f.NewSelectedPage);
			}
		}

		private Wizard Wizard
		{
			get
			{
				return Control as Wizard;
			}
		}

		public override void OnSetComponentDefaults()
		{
			base.OnSetComponentDefaults();

			IDesignerHost designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));

			// Create default pages and add them to the wizard
			IntroductionPage introPage = (IntroductionPage)designerHost.CreateComponent(typeof(IntroductionPage));
			FinishPage finishPage = (FinishPage)designerHost.CreateComponent(typeof(FinishPage));
			WizardPage page = (WizardPage)designerHost.CreateComponent(typeof(WizardPage));
			Wizard.Controls.AddRange(new Control[] { introPage, page, finishPage });
			introPage.NextPage = page;
			page.NextPage = finishPage;
			page.PreviousPage = introPage;
			finishPage.PreviousPage = page;
		}

		protected override void OnSetCursor()
		{
			// Show a hand for the previous and next buttons
			Point clientPoint = Wizard.PointToClient(Cursor.Position);
			if (Wizard.PreviousButtonBounds.Contains(clientPoint) || Wizard.NextButtonBounds.Contains(clientPoint) || ChoosePageBounds.Contains(clientPoint))
				Cursor.Current = Cursors.Hand;
			else
				base.OnSetCursor();
		}

		protected override void OnMouseDragBegin(int x, int y)
		{
			Point clientPoint = Wizard.PointToClient(new Point(x, y));

			if (Wizard.PreviousButtonBounds.Contains(clientPoint))
			{
				if (Wizard.SelectedPage != null && Wizard.SelectedPage.PreviousPage != null)
					SetSelectedPage(Wizard.SelectedPage.PreviousPage);
				return;
			}
			else if (Wizard.NextButtonBounds.Contains(clientPoint))
			{
				if (Wizard.SelectedPage != null && Wizard.SelectedPage.NextPage != null)
					SetSelectedPage(Wizard.SelectedPage.NextPage);
				return;
			}
			else if (ChoosePageBounds.Contains(clientPoint))
				return;

			base.OnMouseDragBegin(x, y);
		}
		
		internal void SetSelectedPage(WizardPageBase page)
		{
			ISelectionService selectionService = (ISelectionService)GetService(typeof(ISelectionService));
			selectionService.SetSelectedComponents(new object[] { page });

			//// Set the selected page, with undo/redo support
			//TypeDescriptor.GetProperties(Wizard)["SelectedPage"].SetValue(Wizard, page);

			//// Select the page in the designer
			//if (Wizard.SelectedPage != null)
			//    selectionService.SetSelectedComponents(new object[] { Wizard.SelectedPage }, SelectionTypes.Replace);
		}

		protected override void OnMouseDragEnd(bool cancel)
		{
			Point clientPoint = Wizard.PointToClient(Cursor.Position);

			if (Wizard.PreviousButtonBounds.Contains(clientPoint) || Wizard.NextButtonBounds.Contains(clientPoint))
				return;
			else if (ChoosePageBounds.Contains(clientPoint))
			{
				ChoosePage();
				return;
			}

			base.OnMouseDragEnd (cancel);
		}

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			base.OnPaintAdornments(pe);

			// Draw choose page link
			using (Font font = new Font(Control.Font, FontStyle.Underline))
			{
				using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(Control.RightToLeft == RightToLeft.Yes, false, StringAlignment.Near, StringAlignment.Center, false))
					IndependentText.DrawText(pe.Graphics, "Choose Page", font, ChoosePageBounds, textFormat, SystemColors.HotTrack);
			}
		}

		private Rectangle ChoosePageBounds
		{
			get
			{
				Rectangle bounds = Wizard.NextButtonBounds;
				bounds.X = 10;
				bounds.Width = 200;

				return bounds;
			}
		}

	}
}
