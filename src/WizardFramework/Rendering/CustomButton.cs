using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Divelements.WizardFramework.Rendering
{
	internal class CustomButton : Button
	{
		// Members
		private Image stateImage;

		public CustomButton()
		{
			BackColor = Color.Transparent;
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			if (StateImage != null)
			{
				if (!OwnerFormSubclass.IsCompositionEnabled)
				{
					Wizard wizard = (Wizard)Parent;
					Wizard07UserExperience uex = (Wizard07UserExperience)wizard.UserExperience;
					pevent.Graphics.Clear(wizard.OwnerForm == null || wizard.OwnerForm == Form.ActiveForm ? uex.ColorTable.TitleBarBackgroundActive : uex.ColorTable.TitleBarBackgroundInactive);
				}
				else
					pevent.Graphics.Clear(Color.Transparent);
				if (!Enabled)
					PaintStateImage(pevent.Graphics, new Rectangle(72, 0, 24, 24));
				else if (MouseIsDown)
					PaintStateImage(pevent.Graphics, new Rectangle(48, 0, 24, 24));
				else if (MouseIsOver || Focused)
					PaintStateImage(pevent.Graphics, new Rectangle(24, 0, 24, 24));
				else
					PaintStateImage(pevent.Graphics, new Rectangle(0, 0, 24, 24));
                if (Focused && ShowFocusCues)
                    ControlPaint.DrawFocusRectangle(pevent.Graphics, ClientRectangle, Color.Black, Color.Transparent);
			}
			else
				base.OnPaint(pevent);
		}

		private void PaintStateImage(Graphics graphics, Rectangle stateImageBounds)
		{
			graphics.DrawImage(StateImage, ClientRectangle, stateImageBounds, GraphicsUnit.Pixel);
		}

		public Image StateImage
		{
			get { return stateImage; }
			set
			{
				stateImage = value;
				Invalidate();
			}
		}

		private bool MouseIsDown
		{
			get
			{
				return (bool)typeof(ButtonBase).GetProperty("MouseIsDown", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(this, null);
			}
		}

		private bool MouseIsOver
		{
			get
			{
				return (bool)typeof(ButtonBase).GetProperty("MouseIsOver", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(this, null);
			}
		}

	}
}
