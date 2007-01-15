namespace AIMLGUI
{
    partial class aimlForm
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
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromAIMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromDatFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCustomLib = new System.Windows.Forms.ToolStripMenuItem();
            this.saveBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemSpeech = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastRequestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.licenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripBottom = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessages = new System.Windows.Forms.ToolStripStatusLabel();
            this.richTextBoxOutput = new System.Windows.Forms.RichTextBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.richTextBoxInput = new System.Windows.Forms.RichTextBox();
            this.saveFileDialogDump = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogDump = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialogAIML = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStripMain.SuspendLayout();
            this.statusStripBottom.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(292, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newBotToolStripMenuItem,
            this.toolStripMenuItemCustomLib,
            this.saveBotToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItemSpeech,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newBotToolStripMenuItem
            // 
            this.newBotToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromAIMLToolStripMenuItem,
            this.fromDatFileToolStripMenuItem,
            this.singleFileToolStripMenuItem});
            this.newBotToolStripMenuItem.Name = "newBotToolStripMenuItem";
            this.newBotToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.newBotToolStripMenuItem.Text = "Open Bot";
            // 
            // fromAIMLToolStripMenuItem
            // 
            this.fromAIMLToolStripMenuItem.Name = "fromAIMLToolStripMenuItem";
            this.fromAIMLToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.fromAIMLToolStripMenuItem.Text = "From AIML files";
            this.fromAIMLToolStripMenuItem.Click += new System.EventHandler(this.fromAIMLToolStripMenuItem_Click);
            // 
            // fromDatFileToolStripMenuItem
            // 
            this.fromDatFileToolStripMenuItem.Name = "fromDatFileToolStripMenuItem";
            this.fromDatFileToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.fromDatFileToolStripMenuItem.Text = "From dat file";
            this.fromDatFileToolStripMenuItem.Click += new System.EventHandler(this.fromDatFileToolStripMenuItem_Click);
            // 
            // singleFileToolStripMenuItem
            // 
            this.singleFileToolStripMenuItem.Name = "singleFileToolStripMenuItem";
            this.singleFileToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.singleFileToolStripMenuItem.Text = "Single File";
            this.singleFileToolStripMenuItem.Click += new System.EventHandler(this.singleFileToolStripMenuItem_Click);
            // 
            // toolStripMenuItemCustomLib
            // 
            this.toolStripMenuItemCustomLib.Name = "toolStripMenuItemCustomLib";
            this.toolStripMenuItemCustomLib.Size = new System.Drawing.Size(204, 22);
            this.toolStripMenuItemCustomLib.Text = "Load Custom Tag Library";
            this.toolStripMenuItemCustomLib.Click += new System.EventHandler(this.toolStripMenuItemCustomLib_Click);
            // 
            // saveBotToolStripMenuItem
            // 
            this.saveBotToolStripMenuItem.Name = "saveBotToolStripMenuItem";
            this.saveBotToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.saveBotToolStripMenuItem.Text = "Save Bot";
            this.saveBotToolStripMenuItem.Click += new System.EventHandler(this.saveBotToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(201, 6);
            // 
            // toolStripMenuItemSpeech
            // 
            this.toolStripMenuItemSpeech.Checked = true;
            this.toolStripMenuItemSpeech.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemSpeech.Name = "toolStripMenuItemSpeech";
            this.toolStripMenuItemSpeech.Size = new System.Drawing.Size(204, 22);
            this.toolStripMenuItemSpeech.Text = "Synthesize Speech";
            this.toolStripMenuItemSpeech.Click += new System.EventHandler(this.toolStripMenuItemSpeech_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(201, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.userToolStripMenuItem,
            this.lastRequestToolStripMenuItem,
            this.lastResultToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // userToolStripMenuItem
            // 
            this.userToolStripMenuItem.Name = "userToolStripMenuItem";
            this.userToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.userToolStripMenuItem.Text = "User";
            this.userToolStripMenuItem.Click += new System.EventHandler(this.userToolStripMenuItem_Click);
            // 
            // lastRequestToolStripMenuItem
            // 
            this.lastRequestToolStripMenuItem.Name = "lastRequestToolStripMenuItem";
            this.lastRequestToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.lastRequestToolStripMenuItem.Text = "Last Request";
            this.lastRequestToolStripMenuItem.Click += new System.EventHandler(this.lastRequestToolStripMenuItem_Click);
            // 
            // lastResultToolStripMenuItem
            // 
            this.lastResultToolStripMenuItem.Name = "lastResultToolStripMenuItem";
            this.lastResultToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.lastResultToolStripMenuItem.Text = "Last Result";
            this.lastResultToolStripMenuItem.Click += new System.EventHandler(this.lastResultToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.licenseToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // licenseToolStripMenuItem
            // 
            this.licenseToolStripMenuItem.Name = "licenseToolStripMenuItem";
            this.licenseToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.licenseToolStripMenuItem.Text = "License";
            this.licenseToolStripMenuItem.Click += new System.EventHandler(this.licenseToolStripMenuItem_Click);
            // 
            // statusStripBottom
            // 
            this.statusStripBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessages});
            this.statusStripBottom.Location = new System.Drawing.Point(0, 251);
            this.statusStripBottom.Name = "statusStripBottom";
            this.statusStripBottom.Size = new System.Drawing.Size(292, 22);
            this.statusStripBottom.TabIndex = 1;
            this.statusStripBottom.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessages
            // 
            this.toolStripStatusLabelMessages.Name = "toolStripStatusLabelMessages";
            this.toolStripStatusLabelMessages.Size = new System.Drawing.Size(0, 17);
            // 
            // richTextBoxOutput
            // 
            this.richTextBoxOutput.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxOutput.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxOutput.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxOutput.Name = "richTextBoxOutput";
            this.richTextBoxOutput.ReadOnly = true;
            this.richTextBoxOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxOutput.Size = new System.Drawing.Size(292, 198);
            this.richTextBoxOutput.TabIndex = 99;
            this.richTextBoxOutput.Text = "Use the \"File\" -> \"Open Bot\" -> \"From AIML files\" menu to load AIML files into th" +
                "e bot\'s brain. (Clicking \"OK\" in the \"Browse for folder\" box will load the AIML " +
                "files from the default location.)";
            this.richTextBoxOutput.TextChanged += new System.EventHandler(this.richTextBoxOutput_TextChanged);
            // 
            // buttonGo
            // 
            this.buttonGo.AutoSize = true;
            this.buttonGo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGo.Location = new System.Drawing.Point(0, 0);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(81, 25);
            this.buttonGo.TabIndex = 4;
            this.buttonGo.Text = "GO";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.richTextBoxOutput);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(292, 227);
            this.splitContainer1.SplitterDistance = 198;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.richTextBoxInput);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.buttonGo);
            this.splitContainer2.Size = new System.Drawing.Size(292, 25);
            this.splitContainer2.SplitterDistance = 207;
            this.splitContainer2.TabIndex = 5;
            // 
            // richTextBoxInput
            // 
            this.richTextBoxInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxInput.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxInput.Name = "richTextBoxInput";
            this.richTextBoxInput.Size = new System.Drawing.Size(207, 25);
            this.richTextBoxInput.TabIndex = 0;
            this.richTextBoxInput.Text = "";
            this.richTextBoxInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.richTextBoxInput_KeyPress);
            // 
            // saveFileDialogDump
            // 
            this.saveFileDialogDump.FileName = "Graphmaster.dat";
            this.saveFileDialogDump.Title = "Select location to save graphmaster";
            // 
            // openFileDialogDump
            // 
            this.openFileDialogDump.FileName = "Graphmaster.dat";
            this.openFileDialogDump.Title = "Select the binary file to load into memory";
            // 
            // folderBrowserDialogAIML
            // 
            this.folderBrowserDialogAIML.Description = "Select AIML folder";
            this.folderBrowserDialogAIML.RootFolder = System.Environment.SpecialFolder.ApplicationData;
            this.folderBrowserDialogAIML.ShowNewFolderButton = false;
            // 
            // aimlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStripBottom);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "aimlForm";
            this.Text = "AIML GUI";
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.statusStripBottom.ResumeLayout(false);
            this.statusStripBottom.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem licenseToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStripBottom;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessages;
        private System.Windows.Forms.ToolStripMenuItem newBotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromAIMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromDatFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveBotToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastRequestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastResultToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBoxOutput;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox richTextBoxInput;
        private System.Windows.Forms.SaveFileDialog saveFileDialogDump;
        private System.Windows.Forms.OpenFileDialog openFileDialogDump;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogAIML;
        private System.Windows.Forms.ToolStripMenuItem singleFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCustomLib;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSpeech;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

