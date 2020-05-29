using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Divelements.WizardFramework
{
	/// <summary>
	/// Represents the method that will handle wizard page events.
	/// </summary>
	public delegate void WizardPageEventHandler(object sender, CancelEventArgs e);

	/// <summary>
	/// Represents the method that will handle wizard finish page events;
	/// </summary>
	public delegate void WizardFinishPageEventHandler(object sender, WizardFinishPageEventArgs e);

	/// <summary>
	/// Represents data passed to a procedure handling an event associated with a wizard finish page.
	/// </summary>
	public class WizardFinishPageEventArgs
	{
		// Members
		private ArrayList names, values;

		internal WizardFinishPageEventArgs()
		{
			// Initialize collections
			names = new ArrayList();
			values = new ArrayList();
		}

		internal void GetNameValuePairs(out string[] names, out string[] values)
		{
			names = (string[])this.names.ToArray(typeof(string));
			values = (string[])this.values.ToArray(typeof(string));
		}

		/// <summary>
		/// Adds a pair of settings to the list displayed to the user.
		/// </summary>
		/// <param name="name">The setting name.</param>
		/// <param name="value">The setting value.</param>
		/// <remarks>
		/// <para>By calling this method for every setting to add, you can present the options configured by the user in an intuitive manner at the end of the wizard.</para>
		/// </remarks>
		public void AddNameValuePair(string name, string value)
		{
			// Protect against nulls
			if (name == null || value == null)
				throw new ArgumentNullException();

			// Add to tables
			names.Add(name);
			values.Add(value);
		}
	}

}
