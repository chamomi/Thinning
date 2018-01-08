using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Thinning
{
    class K3M
    {
        bool changed = true;
        int[] A0 = new int[] { 3, 6, 7, 12, 14, 15, 24, 28, 30,
            31, 48, 56, 60, 62, 63, 96, 112, 120,
            124, 126, 127, 129, 131, 135, 143, 159,
            191, 192, 193, 195, 199, 207, 223, 224,
            225, 227, 231, 239, 240, 241, 243, 247,
            248, 249, 251, 252, 253, 254 };
        int[] A1 = new int[] { 7, 14, 28, 56, 112, 131, 193, 224 };
        int[] A2 = new int[] { 7, 14, 15, 28, 30, 56, 60, 112, 120, 131, 135, 193, 195, 224, 225, 240 };
        int[] A3 = new int[] { 7, 14, 15, 28, 30, 31, 56, 60, 62, 112, 120, 124, 131, 135, 143, 193, 195, 199, 224, 225, 227, 240, 241, 248 };
        int[] A4 = new int[] {7, 14, 15, 28, 30, 31, 56, 60, 62,
            63, 112, 120, 124, 126, 131, 135, 143,
            159, 193, 195, 199, 207, 224, 225, 227,
            231, 240, 241, 243, 248, 249, 252};
        int[] A5 = new int[] {7, 14, 15, 28, 30, 31, 56, 60,
            62, 63, 112, 120, 124, 126, 131, 135,
            143, 159, 191, 193, 195, 199, 207, 224,
            225, 227, 231, 239, 240, 241, 243, 248, 249, 251, 252, 254};
        int[] Apxl = new int[] {3, 6, 7, 12, 14, 15, 24, 28, 30,
            31, 48, 56, 60, 62, 63, 96, 112, 120,
            124, 126, 127, 129, 131, 135, 143, 159,
            191, 192, 193, 195, 199, 207, 223, 224,
            225, 227, 231, 239, 240, 241, 243, 247,
            248, 249, 251, 252, 253, 254};

        public K3M() { }

        public Bitmap Thin(Bitmap img)
        {
            img = Start(img);

            while(changed)
            {
                changed = false;

                img = Phase0(img);
                for (int i = 1; i < 6; i++)
                    img = Phase15(img, i);
                img = Phase6(img);
            }
            img = PhasePxl(img);

            return ToImage(img);
        }

        private Bitmap Start(Bitmap img)
        {
            Color c;
            for (int j = 0; j < img.Height; j++)
                for (int i = 0; i < img.Width; i++)
                {
                    c = img.GetPixel(i, j);
                    try
                    {
                        if ((c.R + c.G + c.B) / 3 < 30)
                            img.SetPixel(i, j, Color.FromArgb(c.A, 1, 1, 1));
                        else img.SetPixel(i, j, Color.FromArgb(c.A, 0, 0, 0));
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Images with indexed pixels are unfortunately unsupported");
                        break;
                    }
                }
            return img;
        }

        private Bitmap Phase0(Bitmap img)
        {
            Color c;
            for (int j = 1; j < img.Height-1; j++)
                for (int i = 1; i < img.Width-1; i++)
                {
                    c = img.GetPixel(i, j);
                    try
                    {
                        if(c.R>0)
                        {
                            if (A0.Contains(Weight(img, i, j)))
                                img.SetPixel(i, j, Color.FromArgb(c.A, 2, 2, 2));
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Images with indexed pixels are unfortunately unsupported");
                        break;
                    }
                }
            return img;
        }

        private Bitmap Phase15(Bitmap img, int ph)
        {
            Color c;
            for (int j = 1; j < img.Height - 1; j++)
                for (int i = 1; i < img.Width - 1; i++)
                {
                    c = img.GetPixel(i, j);
                    try
                    {
                        if (c.R == 2)
                        {
                            switch(ph)
                            {
                                case 1:
                                    if (A1.Contains(Weight(img, i, j)))
                                    {
                                        img.SetPixel(i, j, Color.FromArgb(c.A, 0, 0, 0));
                                        changed = true;
                                    }
                                    break;
                                case 2:
                                    if (A2.Contains(Weight(img, i, j)))
                                    {
                                        img.SetPixel(i, j, Color.FromArgb(c.A, 0, 0, 0));
                                        changed = true;
                                    }
                                    break;
                                case 3:
                                    if (A3.Contains(Weight(img, i, j)))
                                    {
                                        img.SetPixel(i, j, Color.FromArgb(c.A, 0, 0, 0));
                                        changed = true;
                                    }
                                    break;
                                case 4:
                                    if (A4.Contains(Weight(img, i, j)))
                                    {
                                        img.SetPixel(i, j, Color.FromArgb(c.A, 0, 0, 0));
                                        changed = true;
                                    }
                                    break;
                                case 5:
                                    if (A5.Contains(Weight(img, i, j)))
                                    {
                                        img.SetPixel(i, j, Color.FromArgb(c.A, 0, 0, 0));
                                        changed = true;
                                    }
                                    break;
                            }
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Images with indexed pixels are unfortunately unsupported");
                        break;
                    }
                }
            return img;
        }

        private Bitmap Phase6(Bitmap img)
        {
            Color c;
            for (int j = 1; j < img.Height - 1; j++)
                for (int i = 1; i < img.Width - 1; i++)
                {
                    c = img.GetPixel(i, j);
                    try
                    {
                        if (c.R > 0)
                            img.SetPixel(i, j, Color.FromArgb(c.A, 1, 1, 1));
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Images with indexed pixels are unfortunately unsupported");
                        break;
                    }
                }
            return img;
        }

        private Bitmap PhasePxl(Bitmap img)
        {
            Color c;
            for (int j = 1; j < img.Height - 1; j++)
                for (int i = 1; i < img.Width - 1; i++)
                {
                    c = img.GetPixel(i, j);
                    try
                    {
                        if (c.R > 0)
                        {
                            if (Apxl.Contains(Weight(img, i, j)))
                                img.SetPixel(i, j, Color.FromArgb(c.A, 0, 0, 0));
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Images with indexed pixels are unfortunately unsupported");
                        break;
                    }
                }
            return img;
        }

        private Bitmap ToImage(Bitmap img)
        {
            Color c;
            for (int j = 0; j < img.Height; j++)
                for (int i = 0; i < img.Width; i++)
                {
                    c = img.GetPixel(i, j);
                    try
                    {
                        if (c.R == 1)
                            img.SetPixel(i, j, Color.FromArgb(c.A, 0, 0, 0));
                        else img.SetPixel(i, j, Color.FromArgb(c.A, 255, 255, 255));
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Images with indexed pixels are unfortunately unsupported");
                        break;
                    }
                }
            return img;
        }

        private int Weight(Bitmap img, int i, int j)
        {
            int weight = 0;
            if (img.GetPixel(i - 1, j).R > 0) weight += 1;
            if (img.GetPixel(i - 1, j + 1).R > 0) weight += 2;
            if (img.GetPixel(i, j + 1).R > 0) weight += 4;
            if (img.GetPixel(i + 1, j + 1).R > 0) weight += 8;
            if (img.GetPixel(i + 1, j).R > 0) weight += 16;
            if (img.GetPixel(i + 1, j - 1).R > 0) weight += 32;
            if (img.GetPixel(i, j - 1).R > 0) weight += 64;
            if (img.GetPixel(i - 1, j - 1).R > 0) weight += 128;

            return weight;
        }
    }
}
