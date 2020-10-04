namespace Image2Schematic
{
    partial class blockListForm
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
            this.ApplyButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SelectAll = new System.Windows.Forms.CheckBox();
            this.SelectInvert = new System.Windows.Forms.CheckBox();
            this.blockListBox = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // ApplyButton
            // 
            this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyButton.Location = new System.Drawing.Point(187, 173);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(73, 28);
            this.ApplyButton.TabIndex = 1;
            this.ApplyButton.Text = "应用";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.Location = new System.Drawing.Point(266, 173);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 28);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "取消";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SelectAll
            // 
            this.SelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectAll.AutoSize = true;
            this.SelectAll.Location = new System.Drawing.Point(2, 179);
            this.SelectAll.Name = "SelectAll";
            this.SelectAll.Size = new System.Drawing.Size(59, 19);
            this.SelectAll.TabIndex = 4;
            this.SelectAll.Text = "全选";
            this.SelectAll.UseVisualStyleBackColor = true;
            this.SelectAll.CheckedChanged += new System.EventHandler(this.SelectAll_CheckedChanged);
            // 
            // SelectInvert
            // 
            this.SelectInvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectInvert.AutoSize = true;
            this.SelectInvert.Location = new System.Drawing.Point(62, 179);
            this.SelectInvert.Name = "SelectInvert";
            this.SelectInvert.Size = new System.Drawing.Size(59, 19);
            this.SelectInvert.TabIndex = 5;
            this.SelectInvert.Text = "反选";
            this.SelectInvert.UseVisualStyleBackColor = true;
            this.SelectInvert.CheckedChanged += new System.EventHandler(this.SelectInvert_CheckedChanged);
            // 
            // blockListBox
            // 
            this.blockListBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.blockListBox.FormattingEnabled = true;
            this.blockListBox.Location = new System.Drawing.Point(0, 0);
            this.blockListBox.Name = "blockListBox";
            this.blockListBox.Size = new System.Drawing.Size(353, 164);
            this.blockListBox.TabIndex = 6;
            // 
            // blockListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 213);
            this.Controls.Add(this.blockListBox);
            this.Controls.Add(this.SelectInvert);
            this.Controls.Add(this.SelectAll);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ApplyButton);
            this.Name = "blockListForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "方块设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.blockListForm_FormClosing);
            this.Load += new System.EventHandler(this.blockListForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.CheckBox SelectAll;
        private System.Windows.Forms.CheckBox SelectInvert;
        private System.Windows.Forms.CheckedListBox blockListBox;
    }
}