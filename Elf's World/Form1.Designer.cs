namespace Elf_s_World
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entityMarkersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tilesetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataDumpToTxtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.detailsPanel = new System.Windows.Forms.Panel();
            this.detailsDivider = new System.Windows.Forms.Label();
            this.detailsLabel = new System.Windows.Forms.Label();
            this.timeOfDayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.morningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.middayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.detailsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenROM);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.entityMarkersToolStripMenuItem,
            this.tilesetToolStripMenuItem,
            this.timeOfDayToolStripMenuItem});
            this.viewToolStripMenuItem.Enabled = false;
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // entityMarkersToolStripMenuItem
            // 
            this.entityMarkersToolStripMenuItem.CheckOnClick = true;
            this.entityMarkersToolStripMenuItem.Name = "entityMarkersToolStripMenuItem";
            this.entityMarkersToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.entityMarkersToolStripMenuItem.Text = "Entity Markers";
            this.entityMarkersToolStripMenuItem.Click += new System.EventHandler(this.entityMarkersToolStripMenuItem_Click);
            // 
            // tilesetToolStripMenuItem
            // 
            this.tilesetToolStripMenuItem.Name = "tilesetToolStripMenuItem";
            this.tilesetToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.tilesetToolStripMenuItem.Text = "Tileset";
            this.tilesetToolStripMenuItem.Click += new System.EventHandler(this.tilesetToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapOnlyToolStripMenuItem,
            this.dataDumpToTxtToolStripMenuItem});
            this.exportToolStripMenuItem.Enabled = false;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // mapOnlyToolStripMenuItem
            // 
            this.mapOnlyToolStripMenuItem.Name = "mapOnlyToolStripMenuItem";
            this.mapOnlyToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.mapOnlyToolStripMenuItem.Text = "Map only";
            this.mapOnlyToolStripMenuItem.Click += new System.EventHandler(this.mapOnlyToolStripMenuItem_Click);
            // 
            // dataDumpToTxtToolStripMenuItem
            // 
            this.dataDumpToTxtToolStripMenuItem.Name = "dataDumpToTxtToolStripMenuItem";
            this.dataDumpToTxtToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.dataDumpToTxtToolStripMenuItem.Text = "Data dump to txt";
            this.dataDumpToTxtToolStripMenuItem.Click += new System.EventHandler(this.dataDumpToTxtToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AllowMerge = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 249);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(284, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.ActiveLinkColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel2.AutoSize = false;
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(33, 17);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel3.Text = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStripStatusLabel3_MouseDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(284, 225);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.SizeChanged += new System.EventHandler(this.onResize);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // detailsPanel
            // 
            this.detailsPanel.Controls.Add(this.detailsDivider);
            this.detailsPanel.Controls.Add(this.detailsLabel);
            this.detailsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.detailsPanel.Location = new System.Drawing.Point(161, 24);
            this.detailsPanel.Name = "detailsPanel";
            this.detailsPanel.Size = new System.Drawing.Size(123, 225);
            this.detailsPanel.TabIndex = 3;
            this.detailsPanel.Visible = false;
            // 
            // detailsDivider
            // 
            this.detailsDivider.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.detailsDivider.Dock = System.Windows.Forms.DockStyle.Left;
            this.detailsDivider.Location = new System.Drawing.Point(0, 0);
            this.detailsDivider.Name = "detailsDivider";
            this.detailsDivider.Size = new System.Drawing.Size(2, 225);
            this.detailsDivider.TabIndex = 1;
            // 
            // detailsLabel
            // 
            this.detailsLabel.AutoSize = true;
            this.detailsLabel.Location = new System.Drawing.Point(4, 4);
            this.detailsLabel.Name = "detailsLabel";
            this.detailsLabel.Size = new System.Drawing.Size(40, 13);
            this.detailsLabel.TabIndex = 0;
            this.detailsLabel.Text = "dummy";
            this.detailsLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.detailsLabel_MouseClick);
            // 
            // timeOfDayToolStripMenuItem
            // 
            this.timeOfDayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.morningToolStripMenuItem,
            this.middayToolStripMenuItem,
            this.nightToolStripMenuItem});
            this.timeOfDayToolStripMenuItem.Name = "timeOfDayToolStripMenuItem";
            this.timeOfDayToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.timeOfDayToolStripMenuItem.Text = "Time Of Day";
            // 
            // morningToolStripMenuItem
            // 
            this.morningToolStripMenuItem.Name = "morningToolStripMenuItem";
            this.morningToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.morningToolStripMenuItem.Text = "Morning";
            this.morningToolStripMenuItem.Click += new System.EventHandler(this.morningToolStripMenuItem_Click);
            // 
            // middayToolStripMenuItem
            // 
            this.middayToolStripMenuItem.Checked = true;
            this.middayToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.middayToolStripMenuItem.Name = "middayToolStripMenuItem";
            this.middayToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.middayToolStripMenuItem.Text = "Midday";
            this.middayToolStripMenuItem.Click += new System.EventHandler(this.middayToolStripMenuItem_Click);
            // 
            // nightToolStripMenuItem
            // 
            this.nightToolStripMenuItem.Name = "nightToolStripMenuItem";
            this.nightToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.nightToolStripMenuItem.Text = "Night";
            this.nightToolStripMenuItem.Click += new System.EventHandler(this.nightToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 271);
            this.Controls.Add(this.detailsPanel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Elf\'s2World";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.detailsPanel.ResumeLayout(false);
            this.detailsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem entityMarkersToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.Panel detailsPanel;
        private System.Windows.Forms.Label detailsLabel;
        private System.Windows.Forms.Label detailsDivider;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapOnlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tilesetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataDumpToTxtToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timeOfDayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem morningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem middayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nightToolStripMenuItem;
    }
}

