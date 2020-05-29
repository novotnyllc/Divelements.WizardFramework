using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Collections;
using System.Windows.Forms.Design.Behavior;

namespace Divelements.WizardFramework
{
	internal class WizardPageDesigner : WizardPageBaseDesigner
	{
		public WizardPageDesigner()
		{
		}

		private WizardPage Page
		{
			get
			{
				return Control as WizardPage;
			}
		}

		public override IList SnapLines
		{
			get
			{
				ArrayList snapLines = new ArrayList();

				int x = Control.ClientRectangle.Left + Wizard97Metrics.LayoutMarginSize / 2;
				snapLines.Add(new SnapLine(SnapLineType.Left, x));
				x += Wizard97Metrics.LayoutMarginSize;
				snapLines.Add(new SnapLine(SnapLineType.Left, x));
				x = Control.ClientRectangle.Right - Wizard97Metrics.LayoutMarginSize / 2;
				snapLines.Add(new SnapLine(SnapLineType.Right, x));
				x -= Wizard97Metrics.LayoutMarginSize;
				snapLines.Add(new SnapLine(SnapLineType.Right, x));

				return snapLines;
			}
		}
	}
}
