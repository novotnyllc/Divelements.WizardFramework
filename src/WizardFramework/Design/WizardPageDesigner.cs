using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Collections;
#if NET20
using System.Windows.Forms.Design.Behavior;
#endif

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

#if NET20
		public override System.Collections.IList SnapLines
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
#else
		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			base.OnPaintAdornments(pe);

			// Draw fake border
			using (Pen pen = new Pen(SystemColors.ControlDark))
			{
				pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
				int x = Control.ClientRectangle.Left + Wizard97Metrics.LayoutMarginSize / 2;
				pe.Graphics.DrawLine(pen, x, 0, x, Page.ClientRectangle.Height);
				x += Wizard97Metrics.LayoutMarginSize;
				pe.Graphics.DrawLine(pen, x, 0, x, Page.ClientRectangle.Height);
				x = Control.ClientRectangle.Right - Wizard97Metrics.LayoutMarginSize / 2;
				pe.Graphics.DrawLine(pen, x, 0, x, Page.ClientRectangle.Height);
				x -= Wizard97Metrics.LayoutMarginSize;
				pe.Graphics.DrawLine(pen, x, 0, x, Page.ClientRectangle.Height);
			}
		}
#endif

	}
}
