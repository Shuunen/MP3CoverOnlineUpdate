namespace Mp3AlbumCoverUpdater
{
	partial class frmMp3Album
	{
		System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		void InitializeComponent()
		{
			btnStart = new System.Windows.Forms.Button();
			txtKeyWord = new System.Windows.Forms.TextBox();
			ptbNew = new System.Windows.Forms.PictureBox();
			btnAutoUpdate = new System.Windows.Forms.Button();
			btnUpdate = new System.Windows.Forms.Button();
			menuStrip1 = new System.Windows.Forms.MenuStrip();
			OpenFile = new System.Windows.Forms.ToolStripMenuItem();
			ptpOld = new System.Windows.Forms.PictureBox();
			dgvList = new System.Windows.Forms.DataGridView();
			cobEngine = new System.Windows.Forms.ComboBox();
			flpPicture = new System.Windows.Forms.FlowLayoutPanel();
			label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(ptbNew)).BeginInit();
			menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(ptpOld)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(dgvList)).BeginInit();
			SuspendLayout();
			
			// btnStart
			btnStart.Location = new System.Drawing.Point(647, 25);
			btnStart.Name = "btnStart";
			btnStart.Size = new System.Drawing.Size(70, 23);
			btnStart.TabIndex = 0;
			btnStart.Text = "Search";
			btnStart.UseVisualStyleBackColor = true;
			btnStart.Click += btnStart_Click;
			
			// txtKeyWord
			txtKeyWord.Location = new System.Drawing.Point(419, 26);
			txtKeyWord.Name = "txtKeyWord";
			txtKeyWord.Size = new System.Drawing.Size(228, 21);
			txtKeyWord.TabIndex = 2;
			
			// ptbNew
			ptbNew.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			ptbNew.Location = new System.Drawing.Point(723, 281);
			ptbNew.Name = "ptbNew";
			ptbNew.Size = new System.Drawing.Size(240, 240);
			ptbNew.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			ptbNew.TabIndex = 5;
			ptbNew.TabStop = false;
			ptbNew.DoubleClick += ptbNew_DoubleClick;
			
			// btnAutoUpdate
			btnAutoUpdate.Location = new System.Drawing.Point(737, 524);
			btnAutoUpdate.Name = "btnAutoUpdate";
			btnAutoUpdate.Size = new System.Drawing.Size(75, 23);
			btnAutoUpdate.TabIndex = 6;
			btnAutoUpdate.Text = "Auto update";
			btnAutoUpdate.UseVisualStyleBackColor = true;
			btnAutoUpdate.Click += btnAutoUpdate_Click;
			
			// btnUpdate
			btnUpdate.Location = new System.Drawing.Point(876, 525);
			btnUpdate.Name = "btnUpdate";
			btnUpdate.Size = new System.Drawing.Size(75, 23);
			btnUpdate.TabIndex = 7;
			btnUpdate.Text = "Update";
			btnUpdate.UseVisualStyleBackColor = true;
			btnUpdate.Click += btnUpdate_Click;
			
			// menuStrip1
			menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				OpenFile
			});
			menuStrip1.Location = new System.Drawing.Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Size = new System.Drawing.Size(975, 24);
			menuStrip1.TabIndex = 8;
			menuStrip1.Text = "menuStrip1";
			
			// OpenFile
			OpenFile.Name = "OpenFile";
			OpenFile.Size = new System.Drawing.Size(79, 23);
			OpenFile.Text = "Open folder";
			OpenFile.Click += OpenFile_Click;
			
			// ptpOld
			ptpOld.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			ptpOld.Location = new System.Drawing.Point(723, 27);
			ptpOld.Name = "ptpOld";
			ptpOld.Size = new System.Drawing.Size(240, 240);
			ptpOld.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			ptpOld.TabIndex = 5;
			ptpOld.TabStop = false;
			
			// dgvList
			dgvList.AllowUserToAddRows = false;
			dgvList.AllowUserToDeleteRows = false;
			dgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvList.Dock = System.Windows.Forms.DockStyle.Left;
			dgvList.Location = new System.Drawing.Point(0, 24);
			dgvList.Name = "dgvList";
			dgvList.ReadOnly = true;
			dgvList.RowTemplate.Height = 23;
			dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			dgvList.Size = new System.Drawing.Size(347, 532);
			dgvList.TabIndex = 9;
			dgvList.SelectionChanged += dgvList_SelectionChanged;
			
			// cobEngine
			cobEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cobEngine.FormattingEnabled = true;
			foreach (var provider in Program.Providers) {
				cobEngine.Items.AddRange(new object[] { provider.ID });
			}
			
			cobEngine.Location = new System.Drawing.Point(353, 27);
			cobEngine.Name = "cobEngine";
			cobEngine.Size = new System.Drawing.Size(64, 20);
			cobEngine.TabIndex = 10;
			
			// flpPicture
			flpPicture.AutoScroll = true;
			flpPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			flpPicture.Location = new System.Drawing.Point(353, 50);
			flpPicture.Name = "flpPicture";
			flpPicture.Size = new System.Drawing.Size(364, 506);
			flpPicture.TabIndex = 11;
			flpPicture.TabStop = true;
			
			// label1
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(727, 269);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(0, 12);
			label1.TabIndex = 12;
			
			// frmMp3Album
			AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(975, 556);
			Controls.Add(label1);
			Controls.Add(flpPicture);
			Controls.Add(cobEngine);
			Controls.Add(dgvList);
			Controls.Add(btnUpdate);
			Controls.Add(btnAutoUpdate);
			Controls.Add(ptpOld);
			Controls.Add(ptbNew);
			Controls.Add(txtKeyWord);
			Controls.Add(btnStart);
			Controls.Add(menuStrip1);
			MainMenuStrip = menuStrip1;
			Name = "frmMp3Album";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Mp3 Cover Updater";
			Load += new System.EventHandler(Form1_Load);
			((System.ComponentModel.ISupportInitialize)(ptbNew)).EndInit();
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(ptpOld)).EndInit();
			((System.ComponentModel.ISupportInitialize)(dgvList)).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		System.Windows.Forms.Button btnStart;
		System.Windows.Forms.TextBox txtKeyWord;
		System.Windows.Forms.PictureBox ptbNew;
		System.Windows.Forms.Button btnAutoUpdate;
		System.Windows.Forms.Button btnUpdate;
		System.Windows.Forms.MenuStrip menuStrip1;
		System.Windows.Forms.ToolStripMenuItem OpenFile;
		System.Windows.Forms.PictureBox ptpOld;
		System.Windows.Forms.DataGridView dgvList;
		System.Windows.Forms.ComboBox cobEngine;
		System.Windows.Forms.FlowLayoutPanel flpPicture;
		System.Windows.Forms.Label label1;
	}
}

