using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Divelements.WizardFramework
{
	internal class OwnerFormSubclass : NativeWindow, IDisposable
	{
		// Members
		private Form ownerForm;
		private Wizard wizard;
		private static bool isCompositionEnabled;

		public OwnerFormSubclass(Form ownerForm, Wizard wizard)
		{
			// Record members
			this.ownerForm = ownerForm;
			this.wizard = wizard;

			// Set the resize redraw style on the form
			typeof(Form).GetMethod("SetStyle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(ownerForm, new object[] { ControlStyles.ResizeRedraw, true });

			ownerForm.HandleCreated += new EventHandler(OnOwnerFormHandleCreated);
			ownerForm.HandleDestroyed += new EventHandler(OnOwnerFormHandleDestroyed);
			ownerForm.ParentChanged += new EventHandler(OnOwnerFormParentChanged);
			if (ownerForm.IsHandleCreated)
			{
				AssignHandle(ownerForm.Handle);
				if (IsCompositionEnabled)
					ExtendGlassIntoClientArea(true);
				if (IsVista)
					ApplyChromeSettings();
			}

			isCompositionEnabled = GetCompositionEnabled();
		}

		private void OnOwnerFormParentChanged(object sender, EventArgs e)
		{
			// Beause if someone makes it non top-level, we need to cancel composition
			isCompositionEnabled = GetCompositionEnabled();
		}

		private void OnOwnerFormHandleCreated(object sender, EventArgs e)
		{
			AssignHandle(ownerForm.Handle);

			if (IsCompositionEnabled)
				ExtendGlassIntoClientArea(true);
			if (IsVista)
				ApplyChromeSettings();
		}

		private void ApplyChromeSettings()
		{
			Win32.WTA_OPTIONS options = new Win32.WTA_OPTIONS();
			options.dwMask = (Win32.WTNCA.NODRAWCAPTION | Win32.WTNCA.NODRAWICON);
			//if (isGlassEnabled)
			{
				//if (!showCaption)
				{
					options.dwFlags |= Win32.WTNCA.NODRAWCAPTION;
				}
				//if (!showIcon)
				{
					options.dwFlags |= Win32.WTNCA.NODRAWICON;
				}
			}

			Win32.SetWindowThemeAttribute(ownerForm.Handle, Win32.WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT, ref options, Win32.WTA_OPTIONS.Size);
		}

		private void OnOwnerFormHandleDestroyed(object sender, EventArgs e)
		{
			ReleaseHandle();
		}

		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case Win32.WM_DWMCOMPOSITIONCHANGED:
					isCompositionEnabled = GetCompositionEnabled();
					typeof(Form).GetMethod("RecreateHandle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(ownerForm, null);
					//RecreateHandle();
					break;
				case Win32.WM_ERASEBKGND:
					if (IsCompositionEnabled)
					{
						base.WndProc(ref m);
						using (Graphics graphics = Graphics.FromHdc(m.WParam))
							graphics.FillRectangle(Brushes.Black, 0, 0, ownerForm.ClientRectangle.Width, WizardAeroMetrics.TitleBarSize);
						m.Result = new IntPtr(1);
						return;
					}
					break;
			}

			base.WndProc(ref m);
		}

		private void ExtendGlassIntoClientArea(bool extend)
		{
			Win32.MARGINS m = new Win32.MARGINS();
			m.Left = 0;
			m.Right = 0;
			m.Top = extend ? WizardAeroMetrics.TitleBarSize : 0;
			m.Bottom = 0;

			Win32.DwmExtendFrameIntoClientArea(ownerForm.Handle, ref m);
		}

		public static bool IsVista
		{
			get
			{
				return Environment.OSVersion.Version.Major >= 6;
			}
		}

		private bool GetCompositionEnabled()
		{
			if (!ownerForm.Controls.Contains(wizard))
				return false;
			if (!IsVista || (ownerForm.Site != null && ownerForm.Site.DesignMode) || !ownerForm.TopLevel)
				return false;

			bool compositionEnabled = false;
			Win32.DwmIsCompositionEnabled(ref compositionEnabled);
			return compositionEnabled;
		}

		public static bool IsCompositionEnabled
		{
			get { return isCompositionEnabled; }
		}

		#region IDisposable Members

		public void Dispose()
		{
			// Unhook owner form events
			ownerForm.HandleCreated -= new EventHandler(OnOwnerFormHandleCreated);
			ownerForm.HandleDestroyed -= new EventHandler(OnOwnerFormHandleDestroyed);

			if (Handle != IntPtr.Zero)
			{
				ExtendGlassIntoClientArea(false);
				ReleaseHandle();
			}
		}

		#endregion

		internal void CheckEligibility()
		{
			isCompositionEnabled = GetCompositionEnabled();
			typeof(Form).GetMethod("RecreateHandle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(ownerForm, null);
		}
	}
}
