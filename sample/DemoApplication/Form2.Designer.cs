namespace DemoApplication
{
	partial class Form2
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.wizard1 = new Divelements.WizardFramework.Wizard();
			this.introductionPage1 = new Divelements.WizardFramework.IntroductionPage();
			this.finishPage1 = new Divelements.WizardFramework.FinishPage();
			this.wizardPage1 = new Divelements.WizardFramework.WizardPage();
			this.wizard1.SuspendLayout();
			this.SuspendLayout();
			// 
			// wizard1
			// 
			this.wizard1.Controls.Add(this.introductionPage1);
			this.wizard1.Controls.Add(this.wizardPage1);
			this.wizard1.Controls.Add(this.finishPage1);
			this.wizard1.Location = new System.Drawing.Point(0, 0);
			this.wizard1.Name = "wizard1";
			this.wizard1.OwnerForm = this;
			this.wizard1.Size = new System.Drawing.Size(529, 376);
			this.wizard1.TabIndex = 0;
			// 
			// introductionPage1
			// 
			this.introductionPage1.IntroductionText = "This wizard helps you perform a task by providing an intuitive interface for conf" +
				"iguring how the task is performed.";
			this.introductionPage1.Location = new System.Drawing.Point(175, 71);
			this.introductionPage1.Name = "introductionPage1";
			this.introductionPage1.NextPage = this.wizardPage1;
			this.introductionPage1.Size = new System.Drawing.Size(332, 247);
			this.introductionPage1.Text = "Welcome to the Sample Wizard";
			// 
			// finishPage1
			// 
			this.finishPage1.FinishText = "You have successfully completed the Sample Wizard.";
			this.finishPage1.Location = new System.Drawing.Point(0, 0);
			this.finishPage1.Name = "finishPage1";
			this.finishPage1.PreviousPage = this.wizardPage1;
			this.finishPage1.Size = new System.Drawing.Size(0, 0);
			this.finishPage1.Text = "Completing the Sample Wizard";
			// 
			// wizardPage1
			// 
			this.wizardPage1.Description = "This should be a piece of descriptive text about this page.";
			this.wizardPage1.Location = new System.Drawing.Point(0, 0);
			this.wizardPage1.Name = "wizardPage1";
			this.wizardPage1.NextPage = this.finishPage1;
			this.wizardPage1.PreviousPage = this.introductionPage1;
			this.wizardPage1.Size = new System.Drawing.Size(0, 0);
			this.wizardPage1.Text = "Wizard Page";
			// 
			// Form2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(529, 376);
			this.Controls.Add(this.wizard1);
			this.Name = "Form2";
			this.Text = "Form2";
			this.wizard1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Divelements.WizardFramework.Wizard wizard1;
		private Divelements.WizardFramework.IntroductionPage introductionPage1;
		private Divelements.WizardFramework.WizardPage wizardPage1;
		private Divelements.WizardFramework.FinishPage finishPage1;
	}
}