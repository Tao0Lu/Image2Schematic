using System;
using System.Windows.Forms;

namespace Image2Schematic
{
    public partial class imageSizeForm : Form
    {
        public imageSizeForm()
        {
            InitializeComponent();
        }

        private bool funcEdit = false;

        private void saveSize()
        {
            mainForm.MainForm.imageWidth = (int)imageWidth.Value;
            mainForm.MainForm.imageHeight = (int)imageHeight.Value;
            mainForm.MainForm.reloadImage();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            saveSize();
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void imageSizeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }

        private void imageWidth_ValueChanged(object sender, EventArgs e)
        {
            if (funcEdit) { return; }
            if (keepAspectRatio.Checked)
            {
                funcEdit = true;
                imageHeight.Value = mainForm.MainForm.imageHeight * imageWidth.Value / mainForm.MainForm.imageWidth;
                funcEdit = false;
            }
        }

        private void imageHeight_ValueChanged(object sender, EventArgs e)
        {
            if (funcEdit) { return; }
            if (keepAspectRatio.Checked)
            {
                funcEdit = true;
                imageWidth.Value = mainForm.MainForm.imageWidth * imageHeight.Value / mainForm.MainForm.imageHeight;
                funcEdit = false;
            }
        }

        private void imageSizeForm_Load(object sender, EventArgs e)
        {
            imageWidth.Value = mainForm.MainForm.imageWidth;
            imageHeight.Value = mainForm.MainForm.imageHeight;
        }
    }
}
