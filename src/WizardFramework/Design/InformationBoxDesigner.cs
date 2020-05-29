using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace Divelements.WizardFramework
{
	internal class InformationBoxDesigner : ControlDesigner
	{
		// Members
		private DesignerVerbCollection verbs;

		public InformationBoxDesigner()
		{
		}

		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (verbs == null)
				{
					DesignerVerb fitParentVerb = new DesignerVerb("Fit Parent", new EventHandler(OnFitParentVerb));
					verbs = new DesignerVerbCollection(new DesignerVerb[] { fitParentVerb });
				}

				return verbs;
			}
		}

		private void OnFitParentVerb(object sender, System.EventArgs e)
		{
			FitParent();
		}

		private void FitParent()
		{
			// Move so that it has appropriate margins
			Rectangle bounds = Control.Bounds;
			bounds.X = Control.DefaultFont.Height * 2;
			bounds.Width = Control.Parent.ClientRectangle.Width - Control.DefaultFont.Height * 4;
			TypeDescriptor.GetProperties(Control)["Bounds"].SetValue(Control, bounds);

			// Set anchoring
			AnchorStyles anchor = Control.Anchor;
			anchor |= AnchorStyles.Left | AnchorStyles.Right;
			TypeDescriptor.GetProperties(Control)["Anchor"].SetValue(Control, anchor);
		}

		public override void OnSetComponentDefaults()
		{
			base.OnSetComponentDefaults();

			Control.Text = "Replace this text with a piece of information to show to the user.";
		}
	}
}
