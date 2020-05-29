using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Divelements.WizardFramework.Rendering;

namespace Divelements.WizardFramework
{
	/// <summary>
	/// A presentation of text next to a system icon used to indicate information, a warning or an error.
	/// </summary>
	[Designer(typeof(InformationBoxDesigner)), ToolboxBitmap(typeof(InformationBox))]
	public class InformationBox : Control
	{
		// Members
		private SystemIconType iconType = SystemIconType.Information;
		private string text2 = string.Empty;

		/// <summary>
		/// Initializes a new instance of the InformationBox class.
		/// </summary>
		public InformationBox()
		{
			// Set drawing styles
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.Selectable, false);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			// Defaults
			BackColor = Color.Transparent;
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		/// <param name="e">Arguments for the event handling procedure.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			Invalidate();
		}

#if !NET20
		/// <summary>
		/// A second paragraph of text to display.
		/// </summary>
		[Category("Appearance"), DefaultValue(""), Description("A second paragraph of text to display."), Localizable(true)]
		public string Text2
		{
			get { return text2; }
			set
			{
				text2 = value;
				Invalidate();
			}
		}
#endif

		/// <summary>
		/// Indicates the type of icon to show next to the information box.
		/// </summary>
		[Category("Appearance"), DefaultValue(typeof(SystemIconType), "Information"), Description("Indicates the type of icon to show next to the information box.")]
		public SystemIconType Icon
		{
			get
			{
				return iconType;
			}
			set
			{
				iconType = value;
				Invalidate();
			}
		}

		private Icon WorkingIcon
		{
			get
			{
				switch(Icon)
				{
					case SystemIconType.Application:
						return SystemIcons.Application;
//					case SystemIconType.Asterisk:
//						return SystemIcons.Asterisk;
					case SystemIconType.Error:
						return SystemIcons.Error;
//					case SystemIconType.Exclamation:
//						return SystemIcons.Exclamation;
//					case SystemIconType.Hand:
//						return SystemIcons.Hand;
					case SystemIconType.Information:
					default:
						return SystemIcons.Information;
					case SystemIconType.Question:
						return SystemIcons.Question;
					case SystemIconType.Warning:
						return SystemIcons.Warning;
//					case SystemIconType.WinLogo:
//						return SystemIcons.WinLogo;
				}
			}
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		[DefaultValue(typeof(Color), "Transparent")]
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			// Draw icon
			using (Icon largeIcon = new Icon(WorkingIcon, SystemInformation.IconSize.Width, SystemInformation.IconSize.Height))
			{
				if (RightToLeft == RightToLeft.Yes)
					e.Graphics.DrawIcon(largeIcon, new Rectangle(ClientRectangle.Right - SystemInformation.IconSize.Width, ClientRectangle.Top, SystemInformation.IconSize.Width, SystemInformation.IconSize.Height));
				else
					e.Graphics.DrawIcon(largeIcon, new Rectangle(ClientRectangle.Left, ClientRectangle.Top, SystemInformation.IconSize.Width, SystemInformation.IconSize.Height));
			}

			// Draw text
			Rectangle bounds = ClientRectangle;
			if (RightToLeft != RightToLeft.Yes)
				bounds.X += SystemInformation.IconSize.Width + Wizard97Metrics.GetScaledPixelSize(8);
			bounds.Width -= SystemInformation.IconSize.Width + Wizard97Metrics.GetScaledPixelSize(8);
			string text = Text;
#if !NET20
			if (Text2.Length != 0)
				text += Environment.NewLine + Environment.NewLine + Text2;
#endif
			using (TextFormattingInformation textFormat = TextFormattingInformation.CreateFormattingInformation(RightToLeft == RightToLeft.Yes, true, StringAlignment.Near, StringAlignment.Near, false))
				IndependentText.DrawText(e.Graphics, text, Font, bounds, textFormat, ForeColor);
		}

		/// <summary>
		/// Overridden.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(300, 80);
			}
		}

#if NET20
		/// <summary>
		/// Overridden.
		/// </summary>
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
		public override string Text
		{
			get { return base.Text; }
			set { base.Text = value; }
		}
#endif

	}
}
