namespace Image2Schematic
{
    partial class imageSizeForm
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
            this.CancelButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.imageHeight = new System.Windows.Forms.NumericUpDown();
            this.imageWidth = new System.Windows.Forms.NumericUpDown();
            this.keepAspectRatio = new System.Windows.Forms.CheckBox();
            this.widthLabel = new System.Windows.Forms.Label();
            this.heightLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imageHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.Location = new System.Drawing.Point(175, 94);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 28);
            this.CancelButton.TabIndex = 4;
            this.CancelButton.Text = "取消";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyButton.Location = new System.Drawing.Point(96, 94);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(73, 28);
            this.ApplyButton.TabIndex = 3;
            this.ApplyButton.Text = "应用";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // imageHeight
            // 
            this.imageHeight.Location = new System.Drawing.Point(94, 43);
            this.imageHeight.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.imageHeight.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.imageHeight.Name = "imageHeight";
            this.imageHeight.Size = new System.Drawing.Size(120, 25);
            this.imageHeight.TabIndex = 5;
            this.imageHeight.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.imageHeight.ValueChanged += new System.EventHandler(this.imageHeight_ValueChanged);
            // 
            // imageWidth
            // 
            this.imageWidth.Location = new System.Drawing.Point(94, 12);
            this.imageWidth.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.imageWidth.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.imageWidth.Name = "imageWidth";
            this.imageWidth.Size = new System.Drawing.Size(120, 25);
            this.imageWidth.TabIndex = 6;
            this.imageWidth.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.imageWidth.ValueChanged += new System.EventHandler(this.imageWidth_ValueChanged);
            // 
            // keepAspectRatio
            // 
            this.keepAspectRatio.AutoSize = true;
            this.keepAspectRatio.Location = new System.Drawing.Point(12, 74);
            this.keepAspectRatio.Name = "keepAspectRatio";
            this.keepAspectRatio.Size = new System.Drawing.Size(104, 19);
            this.keepAspectRatio.TabIndex = 7;
            this.keepAspectRatio.Text = "保持宽高比";
            this.keepAspectRatio.UseMnemonic = false;
            this.keepAspectRatio.UseVisualStyleBackColor = true;
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(16, 14);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(30, 15);
            this.widthLabel.TabIndex = 8;
            this.widthLabel.Text = "宽:";
            this.widthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(16, 45);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(30, 15);
            this.heightLabel.TabIndex = 9;
            this.heightLabel.Text = "高:";
            this.heightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageSizeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 134);
            this.Controls.Add(this.heightLabel);
            this.Controls.Add(this.widthLabel);
            this.Controls.Add(this.keepAspectRatio);
            this.Controls.Add(this.imageWidth);
            this.Controls.Add(this.imageHeight);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ApplyButton);
            this.Name = "imageSizeForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "图片大小";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.imageSizeForm_FormClosing);
            this.Load += new System.EventHandler(this.imageSizeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.NumericUpDown imageHeight;
        private System.Windows.Forms.NumericUpDown imageWidth;
        private System.Windows.Forms.CheckBox keepAspectRatio;
        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.Label heightLabel;
    }
}