using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Thinning
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }

            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            if (op.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(op.FileName);
            }
        }

        private void KMM_Click(object sender, EventArgs e)
        {
            Bitmap b = new Bitmap((Bitmap)pictureBox1.Image);
            KMM th = new KMM();
            var wind = new Form2();
            wind.Text = "KMM";
            wind.pictureBox3.Image = th.Thin(b);
            wind.Show();
            pictureBox2.Image = th.Thin(b);
        }

        private void K3M_Click(object sender, EventArgs e)
        {
            Bitmap b = new Bitmap((Bitmap)pictureBox1.Image);
            K3M th = new K3M();
            var wind = new Form2();
            wind.Text = "K3M";
            wind.pictureBox3.Image = th.Thin(b);
            wind.Show();
            pictureBox2.Image = th.Thin(b);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null)
                MessageBox.Show("No image to save");
            else
            {
                SaveFileDialog sv = new SaveFileDialog();
                sv.DefaultExt = ".jpg";
                sv.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
                sv.FileName = "Thinned_image";
                if (sv.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox2.Image.Save(sv.FileName);
                    }
                    catch (ExternalException)
                    {
                        MessageBox.Show("File is in use and cannot be modified\nTry saving to another file");
                    }
                }
            }
        }
    }
}
