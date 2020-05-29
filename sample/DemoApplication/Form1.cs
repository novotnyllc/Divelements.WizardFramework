using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace DemoApplication
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private Divelements.WizardFramework.Wizard wizard1;
		private Divelements.WizardFramework.IntroductionPage introductionPage1;
		private Divelements.WizardFramework.FinishPage finishPage1;
		private Divelements.WizardFramework.InformationBox informationBox2;
		private Divelements.WizardFramework.WizardPage wizardPage1;
		private Divelements.WizardFramework.WizardPage wizardPage2;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.Panel panel1;
		private Button button1;
		private Button button2;
		private System.ComponentModel.IContainer components;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("asdasd");
			this.wizard1 = new Divelements.WizardFramework.Wizard();
			this.finishPage1 = new Divelements.WizardFramework.FinishPage();
			this.wizardPage2 = new Divelements.WizardFramework.WizardPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.wizardPage1 = new Divelements.WizardFramework.WizardPage();
			this.button1 = new System.Windows.Forms.Button();
			this.listView1 = new System.Windows.Forms.ListView();
			this.introductionPage1 = new Divelements.WizardFramework.IntroductionPage();
			this.informationBox2 = new Divelements.WizardFramework.InformationBox();
			this.button2 = new System.Windows.Forms.Button();
			this.wizard1.SuspendLayout();
			this.wizardPage2.SuspendLayout();
			this.wizardPage1.SuspendLayout();
			this.introductionPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// wizard1
			// 
			this.wizard1.BannerImage = ((System.Drawing.Image)(resources.GetObject("wizard1.BannerImage")));
			this.wizard1.Controls.Add(this.finishPage1);
			this.wizard1.Controls.Add(this.introductionPage1);
			this.wizard1.Controls.Add(this.wizardPage2);
			this.wizard1.Controls.Add(this.wizardPage1);
			this.wizard1.Location = new System.Drawing.Point(0, 0);
			this.wizard1.MarginImage = ((System.Drawing.Image)(resources.GetObject("wizard1.MarginImage")));
			this.wizard1.Name = "wizard1";
			this.wizard1.OwnerForm = this;
			this.wizard1.Size = new System.Drawing.Size(548, 377);
			this.wizard1.TabIndex = 0;
			this.wizard1.Text = "wizard1";
			this.wizard1.Finish += new System.EventHandler(this.wizard1_Finish);
			this.wizard1.Click += new System.EventHandler(this.wizard1_Click);
			this.wizard1.Cancel += new System.EventHandler(this.wizard1_Cancel);
			// 
			// finishPage1
			// 
			this.finishPage1.FinishText = "You have successfully completed the Sample Wizard.";
			this.finishPage1.Location = new System.Drawing.Point(177, 66);
			this.finishPage1.Name = "finishPage1";
			this.finishPage1.PreviousPage = this.wizardPage2;
			this.finishPage1.Size = new System.Drawing.Size(358, 245);
			this.finishPage1.TabIndex = 5;
			this.finishPage1.Text = "Completing the Sample Wizard";
			this.finishPage1.CollectSettings += new Divelements.WizardFramework.WizardFinishPageEventHandler(this.finishPage1_CollectSettings);
			// 
			// wizardPage2
			// 
			this.wizardPage2.Controls.Add(this.panel1);
			this.wizardPage2.Description = "This should be a piece of descriptive text about this page.";
			this.wizardPage2.Location = new System.Drawing.Point(11, 71);
			this.wizardPage2.Name = "wizardPage2";
			this.wizardPage2.NextPage = this.finishPage1;
			this.wizardPage2.PreviousPage = this.wizardPage1;
			this.wizardPage2.Size = new System.Drawing.Size(526, 242);
			this.wizardPage2.TabIndex = 1005;
			this.wizardPage2.Text = "Wizard Page";
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.IndianRed;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(526, 242);
			this.panel1.TabIndex = 0;
			// 
			// wizardPage1
			// 
			this.wizardPage1.Controls.Add(this.button1);
			this.wizardPage1.Controls.Add(this.listView1);
			this.wizardPage1.Description = "This should be a piece of descriptive text about this page.";
			this.wizardPage1.Location = new System.Drawing.Point(11, 71);
			this.wizardPage1.Name = "wizardPage1";
			this.wizardPage1.NextPage = this.wizardPage2;
			this.wizardPage1.PreviousPage = this.introductionPage1;
			this.wizardPage1.ProceedText = "goodness me";
			this.wizardPage1.Size = new System.Drawing.Size(526, 242);
			this.wizardPage1.TabIndex = 1006;
			this.wizardPage1.Text = "Wizard Page";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(210, 200);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
			this.listView1.Location = new System.Drawing.Point(34, 12);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(459, 194);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			// 
			// introductionPage1
			// 
			this.introductionPage1.Controls.Add(this.button2);
			this.introductionPage1.Controls.Add(this.informationBox2);
			this.introductionPage1.IntroductionText = "This wizard helps you perform a task by providing an intuitive interface for conf" +
				"iguring how the task is performed.";
			this.introductionPage1.Location = new System.Drawing.Point(38, 91);
			this.introductionPage1.Name = "introductionPage1";
			this.introductionPage1.NextPage = this.wizardPage1;
			this.introductionPage1.Size = new System.Drawing.Size(488, 216);
			this.introductionPage1.TabIndex = 3;
			this.introductionPage1.Text = "Welcome to the Demonstration Wizard";
			this.introductionPage1.AfterDisplay += new System.EventHandler(this.introductionPage1_AfterDisplay);
			this.introductionPage1.BeforeDisplay += new System.EventHandler(this.introductionPage1_BeforeDisplay);
			// 
			// informationBox2
			// 
			this.informationBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.informationBox2.Location = new System.Drawing.Point(23, 76);
			this.informationBox2.Name = "informationBox2";
			this.informationBox2.Size = new System.Drawing.Size(443, 80);
			this.informationBox2.TabIndex = 1;
			this.informationBox2.Text = "This wizard may perform an action that you need to be warned about.\r\n\r\nHelpful wa" +
				"rnings such as these are made easy by the companion InformationBox control.";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(343, 186);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(548, 377);
			this.Controls.Add(this.wizard1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Demonstration Wizard";
			this.wizard1.ResumeLayout(false);
			this.wizardPage2.ResumeLayout(false);
			this.wizardPage1.ResumeLayout(false);
			this.introductionPage1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void wizard1_Click(object sender, System.EventArgs e)
		{
		
		}

		private void wizard1_Cancel(object sender, System.EventArgs e)
		{
			Close();
		}

		private void wizard1_Finish(object sender, System.EventArgs e)
		{
			Close();
		}

		private void introductionPage1_BeforeDisplay(object sender, System.EventArgs e)
		{
		
		}

		private void introductionPage1_AfterDisplay(object sender, System.EventArgs e)
		{
		
		}

		private void finishPage1_CollectSettings(object sender, Divelements.WizardFramework.WizardFinishPageEventArgs e)
		{
			e.AddNameValuePair("Scrote", "Balls");
			e.AddNameValuePair("Printer", "Lexmark");
			e.AddNameValuePair("Apple", "Orange");
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Text = "hello!";
		}

	}
}
