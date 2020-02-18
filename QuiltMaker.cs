using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessingTestForm
{
    public partial class QuiltMaker : Form
    {
        string picFilename;
        Random rng = new Random();

        public QuiltMaker()
        {
            InitializeComponent();
            picFilename = "";
            label1.Text = "";
            numericUpDown1.Value = 25;
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picFilename = "";
            if( openFileDialog1.ShowDialog() == DialogResult.OK )
            {
                picFilename = openFileDialog1.FileName;
                label1.Text = picFilename;
                pictureBox1.Image = new Bitmap(picFilename);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if( picFilename == "" )
            {
                return;  // don't have a pic selected
            }

            Bitmap b = new Bitmap(picFilename);
            int size = (int)numericUpDown1.Value;
            bool flip = true;
            for( int i = 0; i < b.Width; i+=size )
            {
                for( int j = 0; j < b.Height; j+=size )
                {
                    if( radioButton1.Checked )
                    {
                        AvgColor(ref b, i, j, size, checkBox1.Checked, true);
                    }
                    else if( radioButton2.Checked )
                    {
                        AvgColor(ref b, i, j, size, checkBox1.Checked, false);
                    }
                    else if( radioButton3.Checked )
                    {
                        flip = !flip;
                        AvgColor(ref b, i, j, size, checkBox1.Checked, flip);
                    }
                    else // Randomize
                    {
                        int r = rng.Next(2);
                        flip = r % 2 == 0;
                        AvgColor(ref b, i, j, size, checkBox1.Checked, flip);
                    }
                }
            }

            pictureBox1.Image = b;
        }

        private Color AvgColor(ref Bitmap b, int i, int j, int size=25, bool border=false, bool lr = true)
        {
            Color top;
            Color bot;
            if ( lr)
            {
                top = b.GetPixel(Math.Min(i + size * 2 / 3, b.Width - 1), Math.Min(j + size * 1 / 3, b.Height - 1));
                bot = b.GetPixel(Math.Min(i + size * 1 / 3, b.Width - 1), Math.Min(j + size * 2 / 3, b.Height - 1));
            }
            else
            {
                top = b.GetPixel(Math.Min(i + size * 1 / 3, b.Width - 1), Math.Min(j + size * 1 / 3, b.Height - 1));
                bot = b.GetPixel(Math.Min(i + size * 2 / 3, b.Width - 1), Math.Min(j + size * 2 / 3, b.Height - 1));
            }
            for (int x = i; x < Math.Min(i+size,b.Width); x++)
            {
                // 
                for (int y = j; y < Math.Min(j + size, b.Height-1); y++)
                {
                    if( lr )
                    {
                        if ((x - i) > ((x - i) + (y - j)) / 2)
                        {
                            b.SetPixel(x, y, top);
                        }
                        else
                        {
                            b.SetPixel(x, y, bot);
                        }
                    }
                    else
                    {
                        if( (x-i)+(y-j) < size )
                        {
                            b.SetPixel(x, y, top);
                        }
                        else
                        {
                            b.SetPixel(x, y, bot);
                        }
                    }
                }
            }

            if( border )
            {
                for(int x = i; x < Math.Min(i + size, b.Width); x++)
                {
                    b.SetPixel(x, j, Color.Black);
                }
                for (int y = j; y < Math.Min(j + size, b.Height - 1); y++)
                {
                    b.SetPixel(i, y, Color.Black);
                }
                for (int x = i; x < Math.Min(i + size, b.Width); x++)
                {
                    for (int y = j; y < Math.Min(j + size, b.Height - 1); y++)
                    {
                        if( lr )
                        {
                            if( (x-i) == (y-j) )
                            {
                                b.SetPixel(x, y, Color.Black);
                            }
                        }
                        else
                        {
                            if ((x - i) + (y - j) == size)
                            {
                                b.SetPixel(x, y, Color.Black);
                            }
                        }
                    }
                }
            }

            
            return Color.Red;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
