using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Divelements.WizardFramework
{
	internal class frmChoosePage : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmChoosePage(Wizard wizard)
		{
			InitializeComponent();

			this.wizard = wizard;

			listView1.TileSize = new System.Drawing.Size(335, 34);
			listView1.View = View.Tile;
			listView1.UseCompatibleStateImageBehavior = false;

			Initialize();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Enabled = false;
			this.btnOK.Location = new System.Drawing.Point(376, 32);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(376, 60);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "&Cancel";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(440, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "Select a page to view:";
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.listView1.FullRowSelect = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(8, 32);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(356, 238);
			this.listView1.TabIndex = 0;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.ItemActivate += new System.EventHandler(this.listView1_ItemActivate);
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			this.listView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView1_ItemDrag);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Page";
			this.columnHeader1.Width = 145;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Description";
			this.columnHeader2.Width = 206;
			// 
			// frmChoosePage
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(458, 279);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmChoosePage";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Choose Page";
			this.ResumeLayout(false);

		}
		#endregion

		// Members
		private Wizard wizard;
		private WizardPageBase newSelectedPage;

		private WizardPageBase[] GetSortedPageList()
		{
			ArrayList pages = new ArrayList();
			while(true)
			{
				bool added = false;
				foreach(Control c in wizard.Controls)
				{
					if (c is WizardPageBase)
					{
						WizardPageBase page = (WizardPageBase)c;
						if (!pages.Contains(page))
						{
							if (pages.Contains(page.PreviousPage))
							{
								pages.Insert(pages.IndexOf(page.PreviousPage) + 1, page);
								added = true;
							}
							else if (pages.Contains(page.NextPage))
							{
								pages.Insert(pages.IndexOf(page.NextPage), page);
								added = true;
							}
							else if (pages.Count == 0)
							{
								pages.Add(page);
								added = true;
							}
						}
					}
				}
				if (!added)
					break;
			}
			foreach(Control c in wizard.Controls)
			{
				if (c is WizardPageBase && !pages.Contains(c))
					pages.Add(c);
			}

			return (WizardPageBase[])pages.ToArray(typeof(WizardPageBase));
		}

		private void Initialize()
		{
			// Display all pages in listview
			foreach (WizardPageBase page in GetSortedPageList())
			{
				// Create item
				ListViewItem item = new ListViewItem(page.Text);
				CoverWizardPage coverPage = page as CoverWizardPage;
				WizardPage wizardPage = page as WizardPage;
				if (wizardPage != null)
					item.SubItems.Add(wizardPage.Description);
				else if (page is IntroductionPage)
					item.SubItems.Add("Introduction Page");
				else if (page is FinishPage)
					item.SubItems.Add("Finish Page");
				else
					item.SubItems.Add(string.Empty);
				item.SubItems[1].ForeColor = SystemColors.GrayText;
				if (wizardPage == null)
					item.SubItems[0].Font = new Font(Font, FontStyle.Bold);

				item.Tag = page;
				listView1.Items.Add(item);
				if (wizard.SelectedPage == page)
					item.Selected = true;
			}
		}

		private void listView1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			btnOK.Enabled = listView1.SelectedItems.Count > 0;

			// Show this page
			if (listView1.SelectedItems.Count > 0)
				wizard.SelectedPage = (WizardPageBase)listView1.SelectedItems[0].Tag;
		}

		private void listView1_ItemActivate(object sender, System.EventArgs e)
		{
			if (btnOK.Enabled)
				btnOK.PerformClick();
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			newSelectedPage = (WizardPageBase)listView1.SelectedItems[0].Tag;
			DialogResult = DialogResult.OK;
		}

		private void listView1_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			listView1.DoDragDrop(e.Item, DragDropEffects.Copy);
		}

		public WizardPageBase NewSelectedPage
		{
			get
			{
				return newSelectedPage;
			}
		}

	}
}
