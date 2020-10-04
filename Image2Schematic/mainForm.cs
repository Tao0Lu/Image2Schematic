using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace Image2Schematic
{
    public partial class mainForm : Form
    {
        private Bitmap bitmap;
        Color[] m_Palette;
        byte[] m_BlockID;
        byte[] m_Nbt;
        string[] m_Names;
        Color[] m_PaletteColors;
        FackColor m_MinPalRGB = new FackColor(0, 0, 0);
        FackColor m_MaxPalRGB = new FackColor(0, 0, 0);
        FackColor m_PalMinMaxRGBDiff = new FackColor(0, 0, 0);
        static int m_MatrixSize = 16;
        int[] m_DitherMatrix = new int[m_MatrixSize * m_MatrixSize];
        int[] m_ColorIndices;
        int m_RDiff, m_GDiff, m_BDiff;
        int m_NumColors;
        int[] m_Bumpy;

        string imageFilePath, saveFilePath;

        public static mainForm MainForm;

        private blockListForm blockListForm = new blockListForm();
        private imageSizeForm imageSizeForm = new imageSizeForm();

        public class FackColor
        {
            public int r;
            public int g;
            public int b;

            public FackColor(int tr,int tg,int tb)
            {
                r = tr;
                g = tg;
                b = tb;
            }

            public int R
            {
                get { return r; }
                set { r = value; }
            }
            public int G
            {
                get { return g; }
                set { g = value; }
            }
            public int B
            {
                get{return b;}
                set{b = value;}
            }
        }

        public static int blockPaletteIndex = 27;
        public bool[] blockPalette = new bool[blockPaletteIndex];
        public int imageWidth;
        public int imageHeight;
        public int paletteTotal;

        public mainForm()
        {
            InitializeComponent();
            MainForm = this;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK) { return; }
            imageFilePath = openFileDialog.FileName;
            bitmap = (Bitmap)Image.FromFile(imageFilePath);
            imageWidth = bitmap.Width;
            imageHeight = bitmap.Height;
            bitmap = resizeImage(bitmap, imageWidth, imageHeight);//处理GIF
            if (imageWidth<16 || imageHeight < 16)
            {
                MessageBox.Show("图片太小了。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            genDitherMatrix();
            this.imageSizeStripStatusLabel.Text = Convert.ToString(imageWidth) + 'x'+ Convert.ToString(imageHeight);
            reloadImage();
        }

        public void reloadImage()
        {
            if (bitmap == null) { return; }
            bitmap = (Bitmap)Image.FromFile(imageFilePath);

            this.imageSizeStripStatusLabel.Text = Convert.ToString(imageWidth) + 'x' + Convert.ToString(imageHeight);

            bitmap = resizeImage(bitmap, imageWidth, imageHeight);

            applyDithering();
            pictureBox.Image = bitmap.Clone() as Image;
        }

        public Bitmap resizeImage(Bitmap bmp, int width, int height)
        {
            //设置图片大小
            Image imgSource = bmp;
            Bitmap outBmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle(0, 0, width, height + 1), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            imgSource.Dispose();
            bmp.Dispose();
            return outBmp;
        }

        private void genDitherMatrix()
        {
            int i = 0;
            for (int y = 0; y <= 15; y++)
            {
                for (int x = 0; x <= 15; x++)
                {
                    m_DitherMatrix[i] = (argb2int(bitmap.GetPixel(x, y)) & 0xFF) -127;
                    i++;
                }
            }
        }

        private int argb2int(Color color)
        {
            return color.R * 256 * 256 + color.B * 256 + color.G;
        }

        private void downGrade()
        {
            m_ColorIndices = new int[imageWidth * imageHeight];
            int i, CIPtr;
            Color ThisPix;

            for (int y = 0; y < imageHeight; y++)
            {
                CIPtr = imageWidth * (imageHeight - 1 - y);
                for (int x = 0; x < imageWidth; x++)
                {
                    ThisPix = bitmap.GetPixel(x, y);
                    if (adjustBrightnessToolStripMenuItem.Checked) { ThisPix = adjustBrightness(ThisPix); }
                    i = getNearestColor(ThisPix.R,ThisPix.G,ThisPix.B);
                    m_ColorIndices[CIPtr] = i;
                    bitmap.SetPixel(x, y, m_PaletteColors[i]);
                    CIPtr++;
                }
            }
        }

        private void orderedDithering()
        {
            m_ColorIndices = new int[imageWidth * imageHeight];
            int i, CIPtr;
            Color ThisPix;

            for (int y = 0; y < imageHeight; y++)
            {
                CIPtr = imageWidth * (imageHeight - 1 - y);
                for (int x = 0; x < imageWidth; x++)
                {
                    ThisPix = bitmap.GetPixel(x, y);
                    if (adjustBrightnessToolStripMenuItem.Checked) { ThisPix = adjustBrightness(ThisPix); }
                    i = getNearestColor(ThisPix.R + m_RDiff * m_DitherMatrix[(y & 0xF) * m_MatrixSize + (x & 0xF)] / 192,
                                    ThisPix.G + m_GDiff * m_DitherMatrix[(y & 0xF) * m_MatrixSize + (x & 0xF)] / 192,
                                    ThisPix.B + m_BDiff * m_DitherMatrix[(y & 0xF) * m_MatrixSize + (x & 0xF)] / 192);
                    m_ColorIndices[CIPtr] = i;
                    bitmap.SetPixel(x, y, m_PaletteColors[i]);
                    CIPtr++;
                }
            }
        }

        private int limitColor(int value)
        {
            if (value < 0) { value = 0; }
            if (value > 255) { value = 255; }
            return value;
        }
        public byte[] int2bytes8(int value)
        {
            byte[] src = new byte[4];
            src[0] = (byte)((value >> 24) & 0xFF);
            src[1] = (byte)((value >> 16) & 0xFF);
            src[2] = (byte)((value >> 8) & 0xFF);
            src[3] = (byte)(value & 0xFF);
            return src;
        }

        public byte[] int2bytes4(int value)
        {
            byte[] src = new byte[2];
            src[0] = (byte)((value >> 8) & 0xFF);
            src[1] = (byte)(value & 0xFF);
            return src;
        }

        private void outPut()
        {
            int LastUp, LastDown, Bumpy,
                y,i,
                XOff = 1,YOff = 1,ZOff = 1,
                Y_Min_Limit = 0, //最小Y值
                Y_Max_Limit = 255; //最大Y值
            bool HeightLimitBreak = false;
            int[] YPositions = new int[imageWidth * imageHeight];
            int[] BlockId = new int[m_NumColors];

            if (flatToolStripMenuItem.Checked)
            {
                for (int k = 0; k < m_NumColors; k++)
                {
                    BlockId[k] = k;
                }
            }
            else
            {
                for (int k = -1; k < m_NumColors-1; k += 3)
                {
                    BlockId[k + 1] = k + 1;
                    BlockId[k + 2] = k + 1;
                    BlockId[k + 3] = k + 1;
                }
            }

            //调整位置
            int mapHeight = 1;
            if (bumpyToolStripMenuItem.Checked)
            {
                for (int x = 0; x < imageWidth; x++)
                {
                    y = YOff;
                    i = x;
                    LastUp = i;
                    LastDown = i;
                    for (int z = 0; z < imageHeight; z++)
                    {
                        YPositions[i] = y;
                        //深，中，浅，-1, 0, 1
                        //当前像素亮度，取决于后方的方块的高度位置
                        Bumpy = m_Bumpy[m_ColorIndices[i]];   // 后方的方块需要做出的调整

                        if (Bumpy != 0) 
                        {
                            mapHeight = mapHeight < y ? y : mapHeight;
                            mapHeight = mapHeight <= 255 ? mapHeight : 255;
                            y = y + Bumpy;                  // y值此时是下一个方块的高度

                            if (Bumpy < 0)
                            {
                                // 过低，则需抬高前面的方块 
                                if (y < Y_Min_Limit)
                                {
                                    for (int j = LastUp; j <= i; j += imageWidth)
                                    {
                                        YPositions[j] = YPositions[j] + 1;
                                        if (YPositions[j] > Y_Max_Limit)
                                        {
                                            YPositions[j] = Y_Min_Limit;
                                            HeightLimitBreak = true;
                                        }
                                    }
                                    y = Y_Min_Limit;
                                }

                                LastDown = i;

                            }
                            else if (Bumpy > 0)
                            {
                                // 过高，则要压低前面的方块 
                                if (y > Y_Max_Limit)
                                {
                                    for (int j = LastDown; j <= i; j = j + imageWidth)
                                    {
                                        YPositions[j] = YPositions[j] - 1;
                                        if (YPositions[j] < Y_Min_Limit)
                                        {
                                            YPositions[j] = Y_Max_Limit;
                                            HeightLimitBreak = true;
                                        }
                                    }
                                    y = Y_Max_Limit;
                                }

                                LastUp = i;
                            }
                        }
                        i = i + imageWidth;
                    }
                }
            }

            //统计范围
            int ty;
            int MinY = Y_Max_Limit + 1;
            int MaxY = Y_Min_Limit - 1;
            for (int u = 0; u < YPositions.Length; u++)
            {
                ty = YPositions[u];
                MinY = ty < MinY ? ty : MinY;
                MaxY = ty > MaxY ? ty : MaxY;
            }

            if (HeightLimitBreak)
            {
                if (MessageBox.Show("超过高度限制!是否继续?\n(将会自动调节到高度限制以下)", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel) {
                    return;
                }
            }

            byte[] Header1 = new byte[] { 0xa, 0x0, 0x9, 0x53, 0x63, 0x68, 0x65, 0x6d, 0x61, 0x74, 0x69, 0x63, 0x2, 0x0, 0x6, 0x48, 0x65, 0x69, 0x67, 0x68, 0x74 };
            byte[] MapHeight = int2bytes4(mapHeight);
            byte[] Header2 = new byte[] { 0x2, 0x0, 0x6, 0x4c, 0x65, 0x6e, 0x67, 0x74, 0x68 };
            byte[] Height = int2bytes4(imageHeight);
            byte[] Header3 = new byte[] { 0x2, 0x0, 0x5, 0x57, 0x69, 0x64, 0x74, 0x68 };
            byte[] Width = int2bytes4(imageWidth);
            byte[] Header4 = new byte[] { 0x9, 0x0, 0x8, 0x45, 0x6e, 0x74, 0x69, 0x74, 0x69, 0x65, 0x73, 0x1, 0x0, 0x0, 0x0, 0x0, 0x9, 0x0, 0xc, 0x54, 0x69, 0x6c, 0x65, 0x45, 0x6e, 0x74, 0x69, 0x74, 0x69, 0x65, 0x73, 0x1, 0x0, 0x0, 0x0, 0x0, 0x9, 0x0, 0x9, 0x54, 0x69, 0x6c, 0x65, 0x54, 0x69, 0x63, 0x6b, 0x73, 0x1, 0x0, 0x0, 0x0, 0x0, 0x8, 0x0, 0x9, 0x4d, 0x61, 0x74, 0x65, 0x72, 0x69, 0x61, 0x6c, 0x73, 0x0, 0x5, 0x41, 0x6c, 0x70, 0x68, 0x61, 0x7, 0x0, 0x4, 0x44, 0x61, 0x74, 0x61 };

            int mapLength = imageHeight * imageWidth * mapHeight;

            byte[] Length = int2bytes8(mapLength);
            //data

            //block
            byte[] Header5 = new byte[] { 0x7, 0x0, 0x6, 0x42, 0x6c, 0x6f, 0x63, 0x6b, 0x73 };
            
            FileStream outFile =new FileStream(saveFilePath, FileMode.Create);
            outFile.Seek(0, SeekOrigin.Begin);
            outFile.Write(Header1, 0, Header1.Length);
            outFile.Write(MapHeight, 0, MapHeight.Length);
            outFile.Write(Header2, 0, Header2.Length);
            outFile.Write(Height, 0, Height.Length);
            outFile.Write(Header3, 0, Header3.Length);
            outFile.Write(Width, 0, Width.Length);
            outFile.Write(Header4, 0, Header4.Length);
            outFile.Write(Length, 0, Length.Length);

            outFile.Seek(127 + mapLength, SeekOrigin.Begin);
            outFile.Write(Header5, 0, Header5.Length);
            outFile.Write(Length, 0, Length.Length);

            //生成
            int MapX, MapZ;
            int LastZ;
            int currentPix;
            for (MapX = 0; MapX < imageWidth; MapX += 127)
            {
                for (MapZ = 0; MapZ < imageHeight; MapZ += 127)
                {
                    //if (DoTP) { TP(TP_PlayerName, MapX +XOff + 64, MaxY + 2, MapZ + ZOff + 64); }
                    for (int x = 0; x < 127; x++)
                    {
                        int LastBlock, LastY;

                        if (MapX + x > imageWidth - 1) { break; }

                        i = MapZ * imageWidth + MapX + x;
                        LastBlock = BlockId[m_ColorIndices[i]];
                        LastY = YPositions[i];
                        LastZ = 0;
                        int z;
                        i = i + imageWidth;
                        for (z = 1; z < 127; z++)
                        {
                            if (MapZ + z > imageHeight - 1) { break; }
                            //if (DoExtraCmd) { DoExtra(ExtraCmdTemplate, XOff + MapX + x, LastY, ZOff + MapZ + z); }
                            if (BlockId[m_ColorIndices[i]] != LastBlock || YPositions[i] != LastY)
                            {
                                if (z - LastZ <= 1)
                                {
                                    //if (DoSurroundWater && m_IsFlowWaterID(LastBlock)) { Fill(XOff + MapX + x - 1, LastY, ZOff + MapZ + LastZ - 1, XOff + MapX + x + 1, LastY - 1, ZOff + MapZ + LastZ + 1, SurroundWaterBlockName, "keep"); }
                                    //currentPix = LastY * (imageWidth * imageHeight) + (XOff + MapX + x - 1) * imageHeight + ZOff + MapZ + LastZ - 1;
                                    currentPix = LastY * (imageWidth * imageHeight) + (ZOff + MapZ + LastZ - 1) * imageWidth + XOff + MapX + x - 1;
                                    outFile.Seek(127 + 13 + mapLength + currentPix, SeekOrigin.Begin);
                                    outFile.WriteByte(m_BlockID[LastBlock]);
                                    if (m_Nbt[LastBlock] != 0)
                                    {
                                        outFile.Seek(127 + currentPix, SeekOrigin.Begin);
                                        outFile.WriteByte(m_Nbt[LastBlock]);
                                    }
                                }
                                else
                                {
                                    //if (DoSurroundWater && m_IsFlowWaterID(LastBlock)) { Fill(XOff + MapX + x - 1, LastY, ZOff + MapZ + LastZ - 1, XOff + MapX + x + 1, LastY - 1, ZOff + MapZ + z, SurroundWaterBlockName, "keep"); }
                                    for (int b = 0; b <= (ZOff + MapZ + z - 1) - (ZOff + MapZ + LastZ); b++)
                                    {
                                        //currentPix = LastY * (imageWidth * imageHeight) + (XOff + MapX + x - 1) * imageHeight + ZOff + MapZ + LastZ + b - 1;
                                        currentPix = LastY * (imageWidth * imageHeight) + (ZOff + MapZ + LastZ + b - 1) * imageWidth + XOff + MapX + x - 1;
                                        outFile.Seek(127 + 13 + mapLength + currentPix, SeekOrigin.Begin);
                                        outFile.WriteByte(m_BlockID[LastBlock]);
                                        if (m_Nbt[LastBlock] != 0)
                                        {
                                            outFile.Seek(127 + currentPix, SeekOrigin.Begin);
                                            outFile.WriteByte(m_Nbt[LastBlock]);
                                        }
                                    }
                                }
                                LastBlock = BlockId[m_ColorIndices[i]];
                                LastY = YPositions[i];
                                LastZ = z;
                            }
                            i = i + imageWidth;
                        }
                        if (z - LastZ <= 1)
                        {
                            //if (DoSurroundWater && m_IsFlowWaterID(LastBlock)) { Fill(XOff + MapX + x - 1, LastY, ZOff + MapZ + LastZ - 1, XOff + MapX + x + 1, LastY - 1, ZOff + MapZ + LastZ + 1, SurroundWaterBlockName, "keep"); }
                            //currentPix = LastY * (imageWidth * imageHeight) + (XOff + MapX + x - 1) * imageHeight + ZOff + MapZ + LastZ - 1;
                            currentPix = LastY * (imageWidth * imageHeight) + (ZOff + MapZ + LastZ - 1) * imageWidth + XOff + MapX + x - 1;
                            outFile.Seek(127 + 13 + mapLength + currentPix, SeekOrigin.Begin);
                            outFile.WriteByte(m_BlockID[LastBlock]);
                            if (m_Nbt[LastBlock] != 0)
                            {
                                outFile.Seek(127 + currentPix, SeekOrigin.Begin);
                                outFile.WriteByte(m_Nbt[LastBlock]);
                            }
                        }
                        else
                        {
                            //if (DoSurroundWater && m_IsFlowWaterID(LastBlock)) { Fill(XOff + MapX + x - 1, LastY, ZOff + MapZ + LastZ - 1, XOff + MapX + x + 1, LastY - 1, ZOff + MapZ + z, SurroundWaterBlockName, "keep"); }
                            for (int b = 0; b <= (ZOff + MapZ + z - 1) - (ZOff + MapZ + LastZ); b++)
                            {
                                //currentPix = LastY * (imageWidth * imageHeight) + (XOff + MapX + x - 1) * imageHeight + ZOff + MapZ + LastZ + b - 1;
                                currentPix = LastY * (imageWidth * imageHeight) + (ZOff + MapZ + LastZ + b - 1) * imageWidth + XOff + MapX + x - 1;
                                outFile.Seek(127 + 13 + mapLength + currentPix, SeekOrigin.Begin);
                                outFile.WriteByte(m_BlockID[LastBlock]);
                                if (m_Nbt[LastBlock] != 0)
                                {
                                    outFile.Seek(127 + currentPix, SeekOrigin.Begin);
                                    outFile.WriteByte(m_Nbt[LastBlock]);
                                }
                            }
                        }
                    }
                }
            }
            outFile.Seek(127 + 13 + 2 * mapLength, SeekOrigin.Begin);
            outFile.WriteByte(0);
            int TempLen = 128 + 13 + 2 * mapLength;
            byte[] Temp2Gzip = new byte[TempLen];
            outFile.Seek(0, SeekOrigin.Begin);
            outFile.Read(Temp2Gzip, 0, TempLen);
            byte[] newFile = WriteGzip(Temp2Gzip);
            outFile.SetLength(0);
            outFile.Write(newFile, 0, newFile.Length);
            outFile.Flush();
            outFile.Close();
            outFile.Dispose();
            //释放内存
            Temp2Gzip = null;
            newFile = null;
            GC.Collect();
            MessageBox.Show("导出Schematic成功。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool m_IsFlowWaterID(int index)
        {
            return m_Names[index].StartsWith("water");
        }

        private void floydSteinbergDithering()
        {
            m_ColorIndices = new int[imageWidth * imageHeight];
            int i, CIPtr;
            int TempR, TempG, TempB;
            Color ThisPix;                      //当前像素颜色值
            FackColor NextPix = new FackColor(0, 0, 0), //下一个像素的误差扩散值
                PixErr = new FackColor(0, 0, 0);       //误差
            FackColor[] Down3 = new FackColor[3];
            FackColor[] LinePix = new FackColor[imageWidth];

            for (int w = 0; w < 3; w++)
            {
                Down3[w] = new FackColor(0, 0, 0);
            }

            for (int w = 0; w < imageWidth; w++)
            {
                LinePix[w] = new FackColor(0, 0, 0);
            }

            for (int y = 0; y < imageHeight; y++) 
            {
                CIPtr = imageWidth * (imageHeight - 1 - y);
                for (int x = 0; x < imageWidth; x++)
                {
                    ThisPix = bitmap.GetPixel(x, y);
                    if (adjustBrightnessToolStripMenuItem.Checked) { ThisPix = adjustBrightness(ThisPix); }

                    //计算新的颜色值
                    TempR = ThisPix.R + NextPix.R + LinePix[x].R;
                    TempG = ThisPix.G + NextPix.G + LinePix[x].G;
                    TempB = ThisPix.B + NextPix.B + LinePix[x].B;

                    //限定偏差
                    ThisPix = Color.FromArgb(limitColor(TempR),limitColor(TempG),limitColor(TempB));

                    //取得调色板最接近颜色值
                    i = getNearestColor(ThisPix.R,ThisPix.G,ThisPix.B);

                    //取得误差值
                    PixErr.R = ThisPix.R - m_PaletteColors[i].R;
                    PixErr.G = ThisPix.G - m_PaletteColors[i].G;
                    PixErr.B = ThisPix.B - m_PaletteColors[i].B;

                    //向右扩散误差值
                    NextPix.R = PixErr.R * 7 / 16;
                    NextPix.G = PixErr.G * 7 / 16;
                    NextPix.B = PixErr.B * 7 / 16;

                    //然后将误差值向下扩散
                    if (x >= 1) { LinePix[x] = Down3[0]; }
                    Down3[0].R = Down3[1].R + PixErr.R * 3 / 16;
                    Down3[0].G = Down3[1].G + PixErr.G * 3 / 16;
                    Down3[0].B = Down3[1].B + PixErr.B * 3 / 16;
                    Down3[1].R = Down3[2].R + PixErr.R * 5 / 16;
                    Down3[1].G = Down3[2].G + PixErr.G * 5 / 16;
                    Down3[1].B = Down3[2].B + PixErr.B * 5 / 16;
                    Down3[2].R = PixErr.R * 1 / 16;
                    Down3[2].G = PixErr.G * 1 / 16;
                    Down3[2].B = PixErr.B * 1 / 16;

                    m_ColorIndices[CIPtr] = i;
                    bitmap.SetPixel(x, y, m_PaletteColors[i]);
                    CIPtr++;
                }
                for (int w=0;w< imageWidth; w++)
                {
                    LinePix[w] = Down3[1];
                }
            }
        }

        private void applyDithering()
        {
            if (bitmap == null) { return; }
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);//翻转
            if (nearestColorToolStripMenuItem.Checked){
                downGrade();
            } else if (orderedDitheringToolStripMenuItem.Checked){
                orderedDithering();
            } else if (errorDiffusionDitheringToolStripMenuItem.Checked){
                floydSteinbergDithering();
            }
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);//翻转回来
        }

        private int getNearestColor(int R,int G,int B)
        {
            int RD, GD, BD;
            int DistSq;
            int MinDistSq = 16777216;
            int NearestColor = 0;
            for (int i = 0; i < m_NumColors; i++)
            {
                RD = m_PaletteColors[i].R - R;
                GD = m_PaletteColors[i].G - G;
                BD = m_PaletteColors[i].B - B;
                DistSq = RD * RD + GD * GD + BD * BD;
                if (DistSq < MinDistSq)
                {
                    MinDistSq = DistSq;
                    NearestColor = i;
                }
            }
            return NearestColor;
        }

        private Color adjustBrightness(Color color)
        {
            return Color.FromArgb(m_MinPalRGB.R + color.R * m_PalMinMaxRGBDiff.R / 255,
                                    m_MinPalRGB.G + color.G * m_PalMinMaxRGBDiff.R / 255,
                                    m_MinPalRGB.B + color.B * m_PalMinMaxRGBDiff.R / 255);
        }

        private void getPaletteProperties()
        {
            m_MaxPalRGB.R = -1;
            m_MaxPalRGB.G = -1;
            m_MaxPalRGB.B = -1;
            m_MinPalRGB.R = 256;
            m_MinPalRGB.G = 256;
            m_MinPalRGB.B = 256;
            m_PaletteColors = new Color[m_NumColors];

            for (int i = 0; i < m_NumColors; i++)
            {
                m_PaletteColors[i] = Color.FromArgb(m_Palette[i].R, m_Palette[i].G, m_Palette[i].B);

                m_MaxPalRGB.R = m_MaxPalRGB.R < m_PaletteColors[i].R ? m_PaletteColors[i].R : m_MaxPalRGB.R;
                m_MaxPalRGB.G = m_MaxPalRGB.G < m_PaletteColors[i].G ? m_PaletteColors[i].G : m_MaxPalRGB.G;
                m_MaxPalRGB.B = m_MaxPalRGB.B < m_PaletteColors[i].B ? m_PaletteColors[i].B : m_MaxPalRGB.B;

                m_MinPalRGB.R = m_MinPalRGB.R > m_PaletteColors[i].R ? m_PaletteColors[i].R : m_MinPalRGB.R;
                m_MinPalRGB.G = m_MinPalRGB.G > m_PaletteColors[i].G ? m_PaletteColors[i].G : m_MinPalRGB.G;
                m_MinPalRGB.B = m_MinPalRGB.B > m_PaletteColors[i].B ? m_PaletteColors[i].B : m_MinPalRGB.B;
            }
            m_PalMinMaxRGBDiff.R = m_MaxPalRGB.R - m_MinPalRGB.R;
            m_PalMinMaxRGBDiff.G = m_MaxPalRGB.G - m_MinPalRGB.G;
            m_PalMinMaxRGBDiff.B = m_MaxPalRGB.B - m_MinPalRGB.B;
            
            //找出调色板最大幅度差
            int RN = 0, GN = 0, BN = 0, CD = 0;
            m_RDiff = 0;
            m_GDiff = 0;
            m_BDiff = 0;
            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j < m_NumColors; j++)
                {
                    if (m_PaletteColors[j].R == i)
                    {
                        CD = i - RN;
                        if (m_RDiff < CD) { m_RDiff = CD; }
                        RN = i;
                    }
                    if (m_PaletteColors[j].G == i)
                    {
                        CD = i - GN;
                        if (m_GDiff < CD) { m_GDiff = CD; }
                        GN = i;
                    }
                    if (m_PaletteColors[j].B == i)
                    {
                        CD = i - BN;
                        if (m_BDiff < CD) { m_BDiff = CD; }
                        BN = i;
                    }
                }
            }
        }

        public void LoadPalette()
        {
            int j = 0;
            if (flatToolStripMenuItem.Checked) {
                m_NumColors = paletteTotal + 15;
                m_Palette = new Color[m_NumColors];
                m_Names = new string[m_NumColors];
                m_BlockID = new byte[m_NumColors];
                m_Nbt = new byte[m_NumColors];
                if (blockPalette[0])
                {
                    j = 15;
                    m_Palette[j - 15] = Color.FromArgb(220, 220, 220);
                    m_Palette[j - 14] = Color.FromArgb(186, 109, 44);
                    m_Palette[j - 13] = Color.FromArgb(153, 65, 186);
                    m_Palette[j - 12] = Color.FromArgb(88, 132, 186);
                    m_Palette[j - 11] = Color.FromArgb(197, 197, 44);
                    m_Palette[j - 10] = Color.FromArgb(109, 176, 21);
                    m_Palette[j - 9] = Color.FromArgb(208, 109, 142);
                    m_Palette[j - 8] = Color.FromArgb(65, 65, 65);
                    m_Palette[j - 7] = Color.FromArgb(132, 132, 132);
                    m_Palette[j - 6] = Color.FromArgb(65, 109, 132);
                    m_Palette[j - 5] = Color.FromArgb(109, 54, 153);
                    m_Palette[j - 4] = Color.FromArgb(44, 65, 153);
                    m_Palette[j - 3] = Color.FromArgb(88, 65, 44);
                    m_Palette[j - 2] = Color.FromArgb(88, 109, 44);
                    m_Palette[j - 1] = Color.FromArgb(132, 44, 44);
                    m_Palette[j] = Color.FromArgb(21, 21, 21);
                    for (int i = j - 15; i <= j; i++)
                    {
                        //m_Names[i] = "wool "+ Convert.ToString(i);
                        m_BlockID[i] = 0x23;
                    }
                    m_Nbt[j - 14] = 0x1;
                    m_Nbt[j - 13] = 0x2;
                    m_Nbt[j - 12] = 0x3;
                    m_Nbt[j - 11] = 0x4;
                    m_Nbt[j - 10] = 0x5;
                    m_Nbt[j - 9] = 0x6;
                    m_Nbt[j - 8] = 0x7;
                    m_Nbt[j - 7] = 0x8;
                    m_Nbt[j - 6] = 0x9;
                    m_Nbt[j - 5] = 0xA;
                    m_Nbt[j - 4] = 0xB;
                    m_Nbt[j - 3] = 0xC;
                    m_Nbt[j - 2] = 0xD;
                    m_Nbt[j - 1] = 0xE;
                    m_Nbt[j] = 0xF;
                }
                if (blockPalette[1])
                {
                    j = j + 1;
                    //m_Names[j] = "stone 0";
                    m_Palette[j] = Color.FromArgb(96, 96, 96);
                    m_BlockID[j] = 0x1;
                }
                if (blockPalette[2])
                {
                    j = j + 1;
                    //m_Names[j] = "grass 0";
                    m_Palette[j] = Color.FromArgb(109, 153, 48);
                    m_BlockID[j] = 0x2;
                }
                if (blockPalette[3])
                {
                    j = j + 1;
                    //m_Names[j] = "dirt 1";
                    m_Palette[j] = Color.FromArgb(157, 91, 40);
                    m_BlockID[j] = 0x3;
                    m_Nbt[j] = 0x1;
                }
                if (blockPalette[4])
                {
                    j = j + 1;
                    //m_Names[j] = "planks 0";
                    m_Palette[j] = Color.FromArgb(123, 103, 62);
                    m_BlockID[j] = 0x5;
                }
                if (blockPalette[5])
                {
                    j = j + 1;
                    //m_Names[j] = "planks 1";
                    m_Palette[j] = Color.FromArgb(110, 73, 41);
                    m_BlockID[j] = 0x5;
                    m_Nbt[j] = 0x1;
                }
                if (blockPalette[6])
                {
                    j = j + 1;
                    //m_Names[j] = "water 0";
                    m_Palette[j] = Color.FromArgb(55, 55, 220);
                    m_BlockID[j] = 0x9;
                }
                if (blockPalette[7])
                {
                    j = j + 1;
                    //m_Names[j] = "melon_block 0";
                    m_Palette[j] = Color.FromArgb(0, 106, 0);
                    m_BlockID[j] = 0x12;
                }
                if (blockPalette[8])
                {
                    j = j + 1;
                    //m_Names[j] = "lapis_block 0";
                    m_Palette[j] = Color.FromArgb(63, 110, 220);
                    m_BlockID[j] = 0x16;
                }
                if (blockPalette[9])
                {
                    j = j + 1;
                    //m_Names[j] = "sandstone 0";
                    m_Palette[j] = Color.FromArgb(213, 201, 140);
                    m_BlockID[j] = 0x18;
                }
                if (blockPalette[10])
                {
                    j = j + 1;
                    //m_Names[j] = "web 0";
                    m_Palette[j] = Color.FromArgb(169, 169, 169);
                    m_BlockID[j] = 0x1E;
                }
                if (blockPalette[11])
                {
                    j = j + 1;
                    //m_Names[j] = "gold_block 0";
                    m_Palette[j] = Color.FromArgb(215, 205, 66);
                    m_BlockID[j] = 0x29;
                }
                if (blockPalette[12])
                {
                    j = j + 1;
                    //m_Names[j] = "iron_block 0";
                    m_Palette[j] = Color.FromArgb(144, 144, 144);
                    m_BlockID[j] = 0x2A;
                }
                if (blockPalette[13])
                {
                    j = j + 1;
                    //m_Names[j] = "tnt 0";
                    m_Palette[j] = Color.FromArgb(220, 0, 0);
                    m_BlockID[j] = 0x2E;
                }
                if (blockPalette[14])
                {
                    j = j + 1;
                    //m_Names[j] = "diamond_block 0";
                    m_Palette[j] = Color.FromArgb(79, 188, 183);
                    m_BlockID[j] = 0x39;
                }
                if (blockPalette[15])
                {
                    j = j + 1;
                    //m_Names[j] = "ice 0";
                    m_Palette[j] = Color.FromArgb(138, 138, 220);
                    m_BlockID[j] = 0x4F;
                }
                if (blockPalette[16])
                {
                    j = j + 1;
                    //m_Names[j] = "clay 0";
                    m_Palette[j] = Color.FromArgb(141, 144, 158);
                    m_BlockID[j] = 0x52;
                }
                if (blockPalette[17])
                {
                    j = j + 1;
                    //m_Names[j] = "netherrack 0";
                    m_Palette[j] = Color.FromArgb(96, 1, 0);
                    m_BlockID[j] = 0x57;
                }
                if (blockPalette[18])
                {
                    j = j + 1;
                    //m_Names[j] = "emerald_block 0";
                    m_Palette[j] = Color.FromArgb(0, 187, 50);
                    m_BlockID[j] = 0x85;
                }
                if (blockPalette[19])
                {
                    j = j + 1;
                    //m_Names[j] = "leaves 0";
                    m_Palette[j] = Color.FromArgb(0, 106, 0);
                    m_BlockID[j] = 0x67;
                }
                if (blockPalette[20])
                {
                    j = j + 1;
                    //m_Names[j] = "red_sandstone 0";
                    m_Palette[j] = Color.FromArgb(184, 108, 43);
                    m_BlockID[j] = 0x98;
                }
                if (blockPalette[21])
                {
                    j = j + 1;
                    //m_Names[j] = "quartz_block 0";
                    m_Palette[j] = Color.FromArgb(220, 217, 211);
                    m_BlockID[j] = 0x9B;
                }
                if (blockPalette[22])
                {
                    j = j + 1;
                    //m_Names[j] = "prismarine 0";
                    m_Palette[j] = Color.FromArgb(79, 188, 183);
                    m_BlockID[j] = 0xA8;
                }
                if (blockPalette[23])
                {
                    j = j + 1;
                    //m_Names[j] = "redstone_block 0";
                    m_Palette[j] = Color.FromArgb(220, 0, 0);
                    m_BlockID[j] = 0xB3;
                }
                if (blockPalette[24])
                {
                    j = j + 1;
                    //m_Names[j] = "purpur_block 0";
                    m_Palette[j] = Color.FromArgb(151, 64, 184);
                    m_BlockID[j] = 0xC9;
                }
                if (blockPalette[25])
                {
                    j = j + 1;
                    //m_Names[j] = "nether_wart_block 0";
                    m_Palette[j] = Color.FromArgb(130, 43, 43);
                    m_BlockID[j] = 0xD6;
                }
                if (blockPalette[26])
                {
                    j = j + 1;
                    //m_Names[j] = "bone_block 0";
                    m_Palette[j] = Color.FromArgb(210, 199, 138);
                    m_BlockID[j] = 0xD8;
                }
            } else if (bumpyToolStripMenuItem.Checked) {
                m_NumColors = (paletteTotal + 16) * 3;
                m_Palette = new Color[m_NumColors];
                m_BlockID = new byte[m_NumColors] ;
                m_Bumpy = new int[m_NumColors];
                m_Names = new string[m_NumColors];
                m_Nbt = new byte[m_NumColors];
                if (blockPalette[0])
                {
                    j = 47;
                    m_Palette[j - 47] = Color.FromArgb(180, 180, 180);
                    m_Palette[j - 46] = Color.FromArgb(220, 220, 220);
                    m_Palette[j - 45] = Color.FromArgb(255, 255, 255);
                    m_Palette[j - 44] = Color.FromArgb(152, 89, 36);
                    m_Palette[j - 43] = Color.FromArgb(186, 109, 44);
                    m_Palette[j - 42] = Color.FromArgb(216, 127, 51);
                    m_Palette[j - 41] = Color.FromArgb(125, 53, 152);
                    m_Palette[j - 40] = Color.FromArgb(153, 65, 186);
                    m_Palette[j - 39] = Color.FromArgb(178, 76, 216);
                    m_Palette[j - 38] = Color.FromArgb(72, 108, 152);
                    m_Palette[j - 37] = Color.FromArgb(88, 132, 186);
                    m_Palette[j - 36] = Color.FromArgb(102, 153, 216);
                    m_Palette[j - 35] = Color.FromArgb(161, 161, 36);
                    m_Palette[j - 34] = Color.FromArgb(197, 197, 44);
                    m_Palette[j - 33] = Color.FromArgb(229, 229, 51);
                    m_Palette[j - 32] = Color.FromArgb(89, 144, 17);
                    m_Palette[j - 31] = Color.FromArgb(109, 176, 21);
                    m_Palette[j - 30] = Color.FromArgb(127, 204, 25);
                    m_Palette[j - 29] = Color.FromArgb(170, 89, 116);
                    m_Palette[j - 28] = Color.FromArgb(208, 109, 142);
                    m_Palette[j - 27] = Color.FromArgb(242, 127, 165);
                    m_Palette[j - 26] = Color.FromArgb(53, 53, 53);
                    m_Palette[j - 25] = Color.FromArgb(65, 65, 65);
                    m_Palette[j - 24] = Color.FromArgb(76, 76, 76);
                    m_Palette[j - 23] = Color.FromArgb(108, 108, 108);
                    m_Palette[j - 22] = Color.FromArgb(132, 132, 132);
                    m_Palette[j - 21] = Color.FromArgb(153, 153, 153);
                    m_Palette[j - 20] = Color.FromArgb(53, 89, 108);
                    m_Palette[j - 19] = Color.FromArgb(65, 109, 132);
                    m_Palette[j - 18] = Color.FromArgb(76, 127, 153);
                    m_Palette[j - 17] = Color.FromArgb(89, 44, 125);
                    m_Palette[j - 16] = Color.FromArgb(109, 54, 153);
                    m_Palette[j - 15] = Color.FromArgb(127, 63, 178);
                    m_Palette[j - 14] = Color.FromArgb(36, 53, 125);
                    m_Palette[j - 13] = Color.FromArgb(44, 65, 153);
                    m_Palette[j - 12] = Color.FromArgb(51, 76, 178);
                    m_Palette[j - 11] = Color.FromArgb(72, 53, 36);
                    m_Palette[j - 10] = Color.FromArgb(88, 65, 44);
                    m_Palette[j - 9] = Color.FromArgb(102, 76, 51);
                    m_Palette[j - 8] = Color.FromArgb(72, 89, 36);
                    m_Palette[j - 7] = Color.FromArgb(88, 109, 44);
                    m_Palette[j - 6] = Color.FromArgb(102, 127, 51);
                    m_Palette[j - 5] = Color.FromArgb(108, 36, 36);
                    m_Palette[j - 4] = Color.FromArgb(132, 44, 44);
                    m_Palette[j - 3] = Color.FromArgb(153, 51, 51);
                    m_Palette[j - 2] = Color.FromArgb(17, 17, 17);
                    m_Palette[j - 1] = Color.FromArgb(21, 21, 21);
                    m_Palette[j] = Color.FromArgb(25, 25, 25);
                    for (int q = 0; q <= 2; q++)
                    {
                        m_Nbt[j - 42 + q] = 0x1;
                        m_Nbt[j - 41 + q] = 0x2;
                        m_Nbt[j - 38 + q] = 0x3;
                        m_Nbt[j - 35 + q] = 0x4;
                        m_Nbt[j - 32 + q] = 0x5;
                        m_Nbt[j - 29 + q] = 0x6;
                        m_Nbt[j - 26 + q] = 0x7;
                        m_Nbt[j - 23 + q] = 0x8;
                        m_Nbt[j - 20 + q] = 0x9;
                        m_Nbt[j - 17 + q] = 0xA;
                        m_Nbt[j - 14 + q] = 0xB;
                        m_Nbt[j - 11 + q] = 0xC;
                        m_Nbt[j - 8 + q] = 0xD;
                        m_Nbt[j - 5 + q] = 0xE;
                        m_Nbt[j - 2 + q] = 0xF;
                    }
                    for (int p = j - 47; p <= j; p++)
                    {
                        //m_Names[p] = "wool " + Convert.ToString(m_Nbt[p]);
                        m_BlockID[p] = 0x23;
                    }
                }
                if (blockPalette[1])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(79, 79, 79);
                    m_Palette[j + 1] = Color.FromArgb(96, 96, 96);
                    m_Palette[j + 2] = Color.FromArgb(112, 112, 112);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j+o] = "stone 0";
                        m_BlockID[j+o] = 0x1;
                    }
                }
                if (blockPalette[2])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(89, 125, 39);
                    m_Palette[j + 1] = Color.FromArgb(109, 153, 48);
                    m_Palette[j + 2] = Color.FromArgb(127, 178, 56);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "grass 0";
                        m_BlockID[j + o] = 0x2;
                    }
                }
                if (blockPalette[3])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(127, 74, 33);
                    m_Palette[j + 1] = Color.FromArgb(157, 91, 40);
                    m_Palette[j + 2] = Color.FromArgb(183, 106, 47);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "dirt 1";
                        m_BlockID[j + o] = 0x3;
                        m_Nbt[j + o] = 0x1;
                    }
                }
                if (blockPalette[4])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(101, 84, 51);
                    m_Palette[j + 1] = Color.FromArgb(123, 103, 62);
                    m_Palette[j + 2] = Color.FromArgb(143, 119, 72);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "planks 0";
                        m_BlockID[j + o] = 0x5;
                    }
                }
                if (blockPalette[5])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(90, 59, 34);
                    m_Palette[j + 1] = Color.FromArgb(110, 73, 41);
                    m_Palette[j + 2] = Color.FromArgb(127, 85, 48);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "planks 1";
                        m_BlockID[j + o] = 0x5;
                        m_Nbt[j + o] = 0x1;
                    }
                }
                if (blockPalette[6])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(45, 45, 180);
                    m_Palette[j + 1] = Color.FromArgb(55, 55, 220);
                    m_Palette[j + 2] = Color.FromArgb(64, 64, 255);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "water 0";
                        m_BlockID[j + o] = 0x9;
                    }
                }
                if (blockPalette[7])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(0, 87, 0);
                    m_Palette[j + 1] = Color.FromArgb(0, 106, 0);
                    m_Palette[j + 2] = Color.FromArgb(0, 124, 0);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "melon_block 0";
                        m_BlockID[j + o] = 0x12;
                    }
                }
                if (blockPalette[8])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(52, 90, 180);
                    m_Palette[j + 1] = Color.FromArgb(63, 110, 220);
                    m_Palette[j + 2] = Color.FromArgb(74, 127, 255);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "lapis_block 0";
                        m_BlockID[j + o] = 0x16;
                    }
                }
                if (blockPalette[9])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(174, 164, 115);
                    m_Palette[j + 1] = Color.FromArgb(213, 201, 140);
                    m_Palette[j + 2] = Color.FromArgb(247, 233, 163);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "sandstone 0";
                        m_BlockID[j + o] = 0x18;
                    }
                }
                if (blockPalette[10])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(138, 138, 138);
                    m_Palette[j + 1] = Color.FromArgb(169, 169, 169);
                    m_Palette[j + 2] = Color.FromArgb(197, 197, 197);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "web 0";
                        m_BlockID[j + o] = 0x1E;
                    }
                }
                if (blockPalette[11])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(176, 168, 54);
                    m_Palette[j + 1] = Color.FromArgb(215, 205, 66);
                    m_Palette[j + 2] = Color.FromArgb(250, 238, 77);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "gold_block 0";
                        m_BlockID[j + o] = 0x29;
                    }
                }
                if (blockPalette[12])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(117, 117, 117);
                    m_Palette[j + 1] = Color.FromArgb(144, 144, 144);
                    m_Palette[j + 2] = Color.FromArgb(167, 167, 167);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "iron_block 0";
                        m_BlockID[j + o] = 0x2A;
                    }
                }
                if (blockPalette[13])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(180, 0, 0);
                    m_Palette[j + 1] = Color.FromArgb(220, 0, 0);
                    m_Palette[j + 2] = Color.FromArgb(255, 0, 0);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "tnt 0";
                        m_BlockID[j + o] = 0x2E;
                    }
                }
                if (blockPalette[14])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(64, 154, 150);
                    m_Palette[j + 1] = Color.FromArgb(79, 188, 183);
                    m_Palette[j + 2] = Color.FromArgb(92, 219, 213);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "diamond_block 0";
                        m_BlockID[j + o] = 0x39;
                    }
                }
                if (blockPalette[15])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(112, 112, 180);
                    m_Palette[j + 1] = Color.FromArgb(138, 138, 220);
                    m_Palette[j + 2] = Color.FromArgb(160, 160, 255);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "ice 0";
                        m_BlockID[j + o] = 0x4F;
                    }
                }
                if (blockPalette[16])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(115, 118, 127);
                    m_Palette[j + 1] = Color.FromArgb(141, 144, 158);
                    m_Palette[j + 2] = Color.FromArgb(164, 168, 184);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "clay 0";
                        m_BlockID[j + o] = 0x52;
                    }
                }
                if (blockPalette[17])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(79, 1, 0);
                    m_Palette[j + 1] = Color.FromArgb(96, 1, 0);
                    m_Palette[j + 2] = Color.FromArgb(112, 2, 0);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "netherrack 0";
                        m_BlockID[j + o] = 0x57;
                    }
                }
                if (blockPalette[18])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(0, 153, 40);
                    m_Palette[j + 1] = Color.FromArgb(0, 187, 50);
                    m_Palette[j + 2] = Color.FromArgb(0, 217, 58);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "emerald_block 0";
                        m_BlockID[j + o] = 0x85;
                    }
                }
                if (blockPalette[19])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(0, 87, 0);
                    m_Palette[j + 1] = Color.FromArgb(0, 106, 0);
                    m_Palette[j + 2] = Color.FromArgb(0, 124, 0);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "leaves 0";
                        m_BlockID[j + o] = 0x67;
                    }
                }
                if (blockPalette[20])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(150, 88, 36);
                    m_Palette[j + 1] = Color.FromArgb(184, 108, 43);
                    m_Palette[j + 2] = Color.FromArgb(213, 125, 50);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "red_sandstone 0";
                        m_BlockID[j + o] = 0x98;
                    }
                }
                if (blockPalette[21])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(180, 177, 172);
                    m_Palette[j + 1] = Color.FromArgb(220, 217, 211);
                    m_Palette[j + 2] = Color.FromArgb(255, 252, 245);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "quartz_block 0";
                        m_BlockID[j + o] = 0x9B;
                    }
                }
                if (blockPalette[22])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(64, 154, 150);
                    m_Palette[j + 1] = Color.FromArgb(79, 188, 183);
                    m_Palette[j + 2] = Color.FromArgb(92, 219, 213);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "prismarine 0";
                        m_BlockID[j + o] = 0xA8;
                    }
                }
                if (blockPalette[23])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(180, 0, 0);
                    m_Palette[j + 1] = Color.FromArgb(220, 0, 0);
                    m_Palette[j + 2] = Color.FromArgb(255, 0, 0);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "redstone_block 0";
                        m_BlockID[j + o] = 0xB3;
                    }
                }
                if (blockPalette[24])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(124, 52, 150);
                    m_Palette[j + 1] = Color.FromArgb(151, 64, 184);
                    m_Palette[j + 2] = Color.FromArgb(176, 75, 213);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "purpur_block 0";
                        m_BlockID[j + o] = 0xC9;
                    }
                }
                if (blockPalette[25])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(107, 36, 36);
                    m_Palette[j + 1] = Color.FromArgb(130, 43, 43);
                    m_Palette[j + 2] = Color.FromArgb(151, 50, 50);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "nether_wart_block 0";
                        m_BlockID[j + o] = 0xD6;
                    }
                }
                if (blockPalette[26])
                {
                    j = j + 3;
                    m_Palette[j] = Color.FromArgb(172, 162, 114);
                    m_Palette[j + 1] = Color.FromArgb(210, 199, 138);
                    m_Palette[j + 2] = Color.FromArgb(244, 230, 161);
                    for (int o = 0; o < 3; o++)
                    {
                        //m_Names[j + o] = "bone_block 0";
                        m_BlockID[j + o] = 0xD8;
                    }
                }
                for (int i = 0; i < m_NumColors; i += 3) 
                {
                    m_Bumpy[i] = -1;
                    m_Bumpy[i + 2] = 1;
                }
            }
            getPaletteProperties();
        }

        public static byte[] WriteGzip(byte[] rawData)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
                compressedzipStream.Write(rawData, 0, rawData.Length);
                compressedzipStream.Close();
                return ms.ToArray();
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void blockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            blockListForm.ShowDialog();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < blockPaletteIndex; i++)
            {
                blockPalette[i] = true;
            }
            paletteTotal = 27;
            adjustBrightnessToolStripMenuItem.Checked = true;
            errorDiffusionDitheringToolStripMenuItem.Checked = true;
            flatToolStripMenuItem.Checked = true;
            LoadPalette();
        }

        private void sizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bitmap != null)
            {
                imageSizeForm.ShowDialog();
            }
        }

        private void clearSelectAlgorithm()
        {
            nearestColorToolStripMenuItem.Checked = false;
            orderedDitheringToolStripMenuItem.Checked = false;
            errorDiffusionDitheringToolStripMenuItem.Checked = false;
        }

        private void nearestColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearSelectAlgorithm();
            nearestColorToolStripMenuItem.Checked = true;
            reloadImage();
        }

        private void flatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bumpyToolStripMenuItem.Checked = false;
            LoadPalette();
            reloadImage();
        }

        private void bumpyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flatToolStripMenuItem.Checked = false;
            LoadPalette();
            reloadImage();
        }

        private void image2SchematicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Tao0Lu/Image2Schematic");
        }

        private void adjustBrightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reloadImage();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bitmap == null || saveFileDialog.ShowDialog() != DialogResult.OK) { return; }
            saveFilePath = saveFileDialog.FileName;
            outPut();
        }

        private void orderedDitheringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearSelectAlgorithm();
            orderedDitheringToolStripMenuItem.Checked = true;
            reloadImage();
        }

        private void errorDiffusionDitheringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearSelectAlgorithm();
            errorDiffusionDitheringToolStripMenuItem.Checked = true;
            reloadImage();
        }
    }
}
