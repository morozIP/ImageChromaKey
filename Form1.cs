using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormCoreTest
{
    public partial class Form1 : Form
    {
        OpenFileDialog openFileDialog1;
        SaveFileDialog saveFileDialog1;
        Bitmap input;
        int greenIndex;
       

        public Form1()
        {
            InitializeComponent();

            #region 1tab
            button1.Click += button1_Click;
            button2.Click += button2_Click;
            button3.Click += button3_Click;

            textBox1.TextChanged += TextBox1_TextChanged;
            #endregion

            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            openFileDialog1.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png|Bmp Files (*.bmp)|*.bmp|Jpeg Files (*.jpg)|*.jpg|Png Files (*.png)|*.png";
            saveFileDialog1.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png|Bmp Files (*.bmp)|*.bmp|Jpeg Files (*.jpg)|*.jpg|Png Files (*.png)|*.png";


            #region tab2
            button4.Click += button4_Click;
            button5.Click += button5_Click;
            button6.Click += button6_Click;

            textBox2.TextChanged += TextBox2_TextChanged;
            #endregion

        }


        public byte[] ImageToByteArray()
        {
            using (var ms = new MemoryStream())
            {
                pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }



        #region 1tab
        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch))
            {
                e.Handled = true;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                greenIndex = Int32.Parse(textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                for (int i = 0; i < input.Width; i++)
                {
                    for (int j = 0; j < input.Height; j++)
                    {
                        Color pixel = input.GetPixel(i, j);

                        if (greenIndex < pixel.G)//TODO: необходимо ли учитывать еще и значения r и b, т.к. при проверке на 0 r и b удаляет меньше фона?
                        {
                            input.SetPixel(i, j, Color.Transparent);
                        }
                    }
                }
                pictureBox1.Image = input;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    input.Save(saveFileDialog1.FileName, ImageFormat.Png);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            pictureBox1.Image = null;
            string filename = openFileDialog1.FileName;
            input = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = input;

        }
        #endregion

        #region 2tab

        static unsafe Bitmap Test2(Bitmap input, int greenIndex)
        {
            const int pixelSize = 4;
            Bitmap target = new Bitmap(input.Width, input.Height, PixelFormat.Format32bppArgb);
            BitmapData inputData = null, targetData = null;

            try
            {
                inputData = input.LockBits(new Rectangle(0, 0, input.Width, input.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                targetData = target.LockBits(new Rectangle(0, 0, target.Width, target.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                for (int y = 0; y < input.Height; ++y)
                {
                    byte* inputRow = (byte*)inputData.Scan0 + (y * inputData.Stride);
                    byte* targetRow = (byte*)targetData.Scan0 + (y * targetData.Stride);

                    for (int x = 0; x < input.Width; ++x)
                    {
                        byte b = inputRow[x * pixelSize + 0];
                        byte g = inputRow[x * pixelSize + 1];
                        byte r = inputRow[x * pixelSize + 2];
                        byte a = inputRow[x * pixelSize + 3];

                        if (greenIndex < g)//TODO: необходимо ли учитывать еще и значения r и b, т.к. при проверке на 0 r и b удаляет меньше фона?
                        {
                            r = 0;
                            g = 0;
                            b = 0;
                            a = 0;
                        }

                        targetRow[x * pixelSize + 0] = b;
                        targetRow[x * pixelSize + 1] = g;
                        targetRow[x * pixelSize + 2] = r;
                        targetRow[x * pixelSize + 3] = a;

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (inputData!=null)
                {
                    input.UnlockBits(inputData);
                }
                if (targetData != null) 
                {
                    target.UnlockBits(targetData);
                }
            }
            return target;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Test2(input, greenIndex);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox2.Image.Save(saveFileDialog1.FileName, ImageFormat.Png);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            pictureBox2.Image = null;
            string filename = openFileDialog1.FileName;
            input = new Bitmap(openFileDialog1.FileName);
            pictureBox2.Image = input;
        }

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch))
            {
                e.Handled = true;
            }
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                greenIndex = Int32.Parse(textBox2.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        #endregion

    }
}
