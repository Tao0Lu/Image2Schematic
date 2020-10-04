namespace Image2Schematic
{
    partial class mainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.paletteSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bumpyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adjustBrightnessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.algorithmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nearestColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orderedDitheringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorDiffusionDitheringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.image2SchematicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.imageSizeStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(784, 32);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.fileToolStripMenuItem.Text = "文件";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(128, 28);
            this.openToolStripMenuItem.Text = "打开";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(125, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(128, 28);
            this.exportToolStripMenuItem.Text = "导出";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(125, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(128, 28);
            this.exitToolStripMenuItem.Text = "退出";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sizeToolStripMenuItem,
            this.toolStripSeparator3,
            this.paletteSettingToolStripMenuItem,
            this.algorithmToolStripMenuItem});
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.settingToolStripMenuItem.Text = "设置";
            // 
            // sizeToolStripMenuItem
            // 
            this.sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
            this.sizeToolStripMenuItem.Size = new System.Drawing.Size(224, 28);
            this.sizeToolStripMenuItem.Text = "图片大小";
            this.sizeToolStripMenuItem.Click += new System.EventHandler(this.sizeToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(221, 6);
            // 
            // paletteSettingToolStripMenuItem
            // 
            this.paletteSettingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modeToolStripMenuItem,
            this.blockToolStripMenuItem,
            this.adjustBrightnessToolStripMenuItem});
            this.paletteSettingToolStripMenuItem.Name = "paletteSettingToolStripMenuItem";
            this.paletteSettingToolStripMenuItem.Size = new System.Drawing.Size(224, 28);
            this.paletteSettingToolStripMenuItem.Text = "调色板";
            // 
            // modeToolStripMenuItem
            // 
            this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.flatToolStripMenuItem,
            this.bumpyToolStripMenuItem});
            this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
            this.modeToolStripMenuItem.Size = new System.Drawing.Size(224, 28);
            this.modeToolStripMenuItem.Text = "模式";
            // 
            // flatToolStripMenuItem
            // 
            this.flatToolStripMenuItem.CheckOnClick = true;
            this.flatToolStripMenuItem.Name = "flatToolStripMenuItem";
            this.flatToolStripMenuItem.Size = new System.Drawing.Size(128, 28);
            this.flatToolStripMenuItem.Text = "平坦";
            this.flatToolStripMenuItem.Click += new System.EventHandler(this.flatToolStripMenuItem_Click);
            // 
            // bumpyToolStripMenuItem
            // 
            this.bumpyToolStripMenuItem.CheckOnClick = true;
            this.bumpyToolStripMenuItem.Name = "bumpyToolStripMenuItem";
            this.bumpyToolStripMenuItem.Size = new System.Drawing.Size(128, 28);
            this.bumpyToolStripMenuItem.Text = "立体";
            this.bumpyToolStripMenuItem.Click += new System.EventHandler(this.bumpyToolStripMenuItem_Click);
            // 
            // blockToolStripMenuItem
            // 
            this.blockToolStripMenuItem.Name = "blockToolStripMenuItem";
            this.blockToolStripMenuItem.Size = new System.Drawing.Size(224, 28);
            this.blockToolStripMenuItem.Text = "方块设置";
            this.blockToolStripMenuItem.Click += new System.EventHandler(this.blockToolStripMenuItem_Click);
            // 
            // adjustBrightnessToolStripMenuItem
            // 
            this.adjustBrightnessToolStripMenuItem.CheckOnClick = true;
            this.adjustBrightnessToolStripMenuItem.Name = "adjustBrightnessToolStripMenuItem";
            this.adjustBrightnessToolStripMenuItem.Size = new System.Drawing.Size(224, 28);
            this.adjustBrightnessToolStripMenuItem.Text = "调整亮度";
            this.adjustBrightnessToolStripMenuItem.Click += new System.EventHandler(this.adjustBrightnessToolStripMenuItem_Click);
            // 
            // algorithmToolStripMenuItem
            // 
            this.algorithmToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nearestColorToolStripMenuItem,
            this.orderedDitheringToolStripMenuItem,
            this.errorDiffusionDitheringToolStripMenuItem});
            this.algorithmToolStripMenuItem.Name = "algorithmToolStripMenuItem";
            this.algorithmToolStripMenuItem.Size = new System.Drawing.Size(224, 28);
            this.algorithmToolStripMenuItem.Text = "算法";
            // 
            // nearestColorToolStripMenuItem
            // 
            this.nearestColorToolStripMenuItem.CheckOnClick = true;
            this.nearestColorToolStripMenuItem.Name = "nearestColorToolStripMenuItem";
            this.nearestColorToolStripMenuItem.Size = new System.Drawing.Size(224, 28);
            this.nearestColorToolStripMenuItem.Text = "近似颜色";
            this.nearestColorToolStripMenuItem.Click += new System.EventHandler(this.nearestColorToolStripMenuItem_Click);
            // 
            // orderedDitheringToolStripMenuItem
            // 
            this.orderedDitheringToolStripMenuItem.CheckOnClick = true;
            this.orderedDitheringToolStripMenuItem.Name = "orderedDitheringToolStripMenuItem";
            this.orderedDitheringToolStripMenuItem.Size = new System.Drawing.Size(224, 28);
            this.orderedDitheringToolStripMenuItem.Text = "矩形抖动";
            this.orderedDitheringToolStripMenuItem.Click += new System.EventHandler(this.orderedDitheringToolStripMenuItem_Click);
            // 
            // errorDiffusionDitheringToolStripMenuItem
            // 
            this.errorDiffusionDitheringToolStripMenuItem.CheckOnClick = true;
            this.errorDiffusionDitheringToolStripMenuItem.Name = "errorDiffusionDitheringToolStripMenuItem";
            this.errorDiffusionDitheringToolStripMenuItem.Size = new System.Drawing.Size(224, 28);
            this.errorDiffusionDitheringToolStripMenuItem.Text = "误差扩散";
            this.errorDiffusionDitheringToolStripMenuItem.Click += new System.EventHandler(this.errorDiffusionDitheringToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.image2SchematicToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.aboutToolStripMenuItem.Text = "关于";
            // 
            // image2SchematicToolStripMenuItem
            // 
            this.image2SchematicToolStripMenuItem.Name = "image2SchematicToolStripMenuItem";
            this.image2SchematicToolStripMenuItem.Size = new System.Drawing.Size(230, 28);
            this.image2SchematicToolStripMenuItem.Text = "Image2Schematic";
            this.image2SchematicToolStripMenuItem.Click += new System.EventHandler(this.image2SchematicToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "图片文件|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Schematic 文件|*.schematic";
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 32);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(784, 418);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageSizeStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // imageSizeStripStatusLabel
            // 
            this.imageSizeStripStatusLabel.Name = "imageSizeStripStatusLabel";
            this.imageSizeStripStatusLabel.Size = new System.Drawing.Size(0, 16);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "mainForm";
            this.Text = "Image2Schematic";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripMenuItem sizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem paletteSettingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bumpyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem algorithmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nearestColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orderedDitheringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem errorDiffusionDitheringToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel imageSizeStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem adjustBrightnessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem image2SchematicToolStripMenuItem;
    }
}

