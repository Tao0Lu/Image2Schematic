using System;
using System.Windows.Forms;

namespace Image2Schematic
{
    public partial class blockListForm : Form
    {
        private bool aiuserop = false;

        public blockListForm()
        {
            InitializeComponent();
        }

        private void blockListForm_Load(object sender, EventArgs e)
        {
            string[] blockListItem = new string[mainForm.blockPaletteIndex];
            blockListBox.Items.Clear();
            blockListItem[0] = "羊毛";//0
            blockListItem[1] = "石头";//1
            blockListItem[2] = "草方块";//2
            blockListItem[3] = "砂土";//3
            blockListItem[4] = "橡木木板";//4
            blockListItem[5] = "云杉木板";//5
            blockListItem[6] = "水";//6
            blockListItem[7] = "橡树树叶";//7
            blockListItem[8] = "青金石块";//8
            blockListItem[9] = "沙岩";//9
            blockListItem[10] = "蜘蛛网";//10
            blockListItem[11] = "金块";//11
            blockListItem[12] = "铁块";//12
            blockListItem[13] = "TNT";//13
            blockListItem[14] = "钻石块";//14
            blockListItem[15] = "冰块";//15
            blockListItem[16] = "粘土块";//16
            blockListItem[17] = "地狱岩";//17
            blockListItem[18] = "绿宝石块";//18
            blockListItem[19] = "西瓜块";//19
            blockListItem[20] = "红沙岩";//20
            blockListItem[21] = "石英块";//21
            ;//1.8+
            blockListItem[22] = "海晶石";//22
            blockListItem[23] = "红石块";//23
            ;//1.10+
            blockListItem[24] = "紫珀块";//24
            blockListItem[25] = "地狱疣块";//25
            blockListItem[26] = "骨块";//26

            for (int i = 0; i <= 26; i++)
            {
                blockListBox.Items.Add(blockListItem[i]);
            }
            reloadForm();
        }

        private void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (aiuserop) { return; }
            for (int i = 0; i < blockListBox.Items.Count; i++)
            {
                blockListBox.SetItemChecked(i, SelectAll.Checked);
            }
            if (SelectInvert.Checked)
            {
                aiuserop = true;
                SelectInvert.Checked = false;
                aiuserop = false;
            }
        }

        private void SelectInvert_CheckedChanged(object sender, EventArgs e)
        {
            if (aiuserop) { return; }
            for (int i = 0; i < blockListBox.Items.Count; i++)
            {
                blockListBox.SetItemChecked(i, !blockListBox.GetItemChecked(i));

            }
            if (SelectAll.Checked) 
            {
                aiuserop = true;
                SelectAll.Checked = false;
                aiuserop = false;
            }
        }

        private void savePalette()
        {
            for (int i = 0; i < mainForm.blockPaletteIndex; i++)
            {
                mainForm.MainForm.blockPalette[i] = blockListBox.GetItemChecked(i);
            }
            mainForm.MainForm.paletteTotal = blockListBox.CheckedItems.Count;
            mainForm.MainForm.reloadImage();
        }

        private void reloadForm()
        {
            aiuserop = true;
            SelectAll.Checked = false;
            SelectInvert.Checked = false;
            aiuserop = false;
            for (int i = 0; i < mainForm.blockPaletteIndex; i++)
            {
                blockListBox.SetItemChecked(i, mainForm.MainForm.blockPalette[i]);
            }
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            savePalette();
            mainForm.MainForm.LoadPalette();
            mainForm.MainForm.reloadImage();
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void blockListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }
    }
}
