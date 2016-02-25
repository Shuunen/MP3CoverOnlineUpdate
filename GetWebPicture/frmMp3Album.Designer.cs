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
			btnSearch = new System.Windows.Forms.Button();
			searchInput = new System.Windows.Forms.TextBox();
			selectedCover = new System.Windows.Forms.PictureBox();
			btnOnlyMissing = new System.Windows.Forms.Button();
			btnUpdate = new System.Windows.Forms.Button();
			menuStrip1 = new System.Windows.Forms.MenuStrip();
			OpenFile = new System.Windows.Forms.ToolStripMenuItem();
			currentCover = new System.Windows.Forms.PictureBox();
			fileList = new System.Windows.Forms.DataGridView();
			cobEngine = new System.Windows.Forms.ComboBox();
			flpPicture = new System.Windows.Forms.FlowLayoutPanel();
			label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(selectedCover)).BeginInit();
			menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(currentCover)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(fileList)).BeginInit();
			SuspendLayout();
			
			// btnSearch
			btnSearch.Location = new System.Drawing.Point(647, 25);
			btnSearch.Name = "btnSearch";
			btnSearch.Size = new System.Drawing.Size(70, 23);
			btnSearch.TabIndex = 0;
			btnSearch.Text = "Search";
			btnSearch.UseVisualStyleBackColor = true;
			btnSearch.Click += btnSearch_Click;
			
			// searchInput
			searchInput.Location = new System.Drawing.Point(419, 26);
			searchInput.Name = "searchInput";
			searchInput.Size = new System.Drawing.Size(228, 21);
			searchInput.TabIndex = 2;
			
			// selectedCover
			selectedCover.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			selectedCover.Location = new System.Drawing.Point(723, 281);
			selectedCover.Name = "selectedCover";
			selectedCover.Size = new System.Drawing.Size(240, 240);
			selectedCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			selectedCover.TabIndex = 5;
			selectedCover.TabStop = false;
			selectedCover.DoubleClick += selectedCover_DoubleClick;
			
			// btnOnlyMissing
			btnOnlyMissing.Location = new System.Drawing.Point(737, 524);
			btnOnlyMissing.Name = "btnOnlyMissing";
			btnOnlyMissing.Size = new System.Drawing.Size(130, 23);
			btnOnlyMissing.TabIndex = 6;
			btnOnlyMissing.Text = "labelDefault";
			btnOnlyMissing.UseVisualStyleBackColor = true;
			btnOnlyMissing.Click += btnOnlyMissing_Click;
			
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
			
			// currentCover
			currentCover.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			currentCover.Location = new System.Drawing.Point(723, 27);
			currentCover.Name = "currentCover";
			currentCover.Size = new System.Drawing.Size(240, 240);
			currentCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			currentCover.TabIndex = 5;
			currentCover.TabStop = false;
			
			// fileList
			fileList.AllowUserToAddRows = false;
			fileList.AllowUserToDeleteRows = false;
			fileList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			fileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			fileList.Dock = System.Windows.Forms.DockStyle.Left;
			fileList.Location = new System.Drawing.Point(0, 24);
			fileList.Name = "fileList";
			fileList.ReadOnly = true;
			fileList.RowTemplate.Height = 23;
			fileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			fileList.Size = new System.Drawing.Size(347, 532);
			fileList.TabIndex = 9;
			fileList.SelectionChanged += fileList_SelectionChanged;
			
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
			Controls.Add(fileList);
			Controls.Add(btnUpdate);
			Controls.Add(btnOnlyMissing);
			Controls.Add(currentCover);
			Controls.Add(selectedCover);
			Controls.Add(searchInput);
			Controls.Add(btnSearch);
			Controls.Add(menuStrip1);
			MainMenuStrip = menuStrip1;
			Name = "frmMp3Album";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Mp3 Cover Updater";
			Load += new System.EventHandler(Form1_Load);
			((System.ComponentModel.ISupportInitialize)(selectedCover)).EndInit();
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(currentCover)).EndInit();
			((System.ComponentModel.ISupportInitialize)(fileList)).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		System.Windows.Forms.Button btnSearch;
		System.Windows.Forms.TextBox searchInput;
		System.Windows.Forms.PictureBox selectedCover;
		System.Windows.Forms.Button btnOnlyMissing;
		System.Windows.Forms.Button btnUpdate;
		System.Windows.Forms.MenuStrip menuStrip1;
		System.Windows.Forms.ToolStripMenuItem OpenFile;
		System.Windows.Forms.PictureBox currentCover;
		System.Windows.Forms.DataGridView fileList;
		System.Windows.Forms.ComboBox cobEngine;
		System.Windows.Forms.FlowLayoutPanel flpPicture;
		System.Windows.Forms.Label label1;
	}
}

