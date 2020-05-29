using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DemoApplication
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
#if NETCOREAPP3_1
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Application.Run(new Form1());
		}
	}
}