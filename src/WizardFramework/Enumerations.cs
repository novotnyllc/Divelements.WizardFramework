using System;

namespace Divelements.WizardFramework
{

	/// <summary>
	/// Specifies a type of user experience to use in a wizard.
	/// </summary>
	public enum WizardUserExperienceType : int
	{
		/// <summary>
		/// The Wizard 97 user experience is used.
		/// </summary>
		Wizard97,
		/// <summary>
		/// The "Aero" user experience is used.
		/// </summary>
		WizardAero,
		/// <summary>
		/// The experience used it chosen automatically based on the operating system version.
		/// </summary>
		Automatic
	}

	/// <summary>
	/// A type of icon native to the system.
	/// </summary>
	public enum SystemIconType : int
	{
		/// <summary>
		/// The default icon for an application window.
		/// </summary>
		Application,
		/// <summary>
		/// An icon indicating an error has occured.
		/// </summary>
		Error,
		/// <summary>
		/// An icon indicating some information needs to be read.
		/// </summary>
		Information,
		/// <summary>
		/// An icon indicating some information is required.
		/// </summary>
		Question,
		/// <summary>
		/// An icon indicating a warning.
		/// </summary>
		Warning
	}

	/// <summary>
	/// Specifies how an image displayed on a cover page is drawn.
	/// </summary>
	public enum MarginImageAppearance : int
	{
		/// <summary>
		/// The image is drawn at its native size.
		/// </summary>
		Normal,
		/// <summary>
		/// The image is stretched to fill the space available.
		/// </summary>
		Stretch
	}

}
