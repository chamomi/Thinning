using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Thinning
{
    class KMM
    {
        int[] deletion = new int[] { 3, 5, 7, 12, 13, 14, 15, 20,
            21, 22, 23, 28, 29, 30, 31, 48,
            52, 53, 54, 55, 56, 60, 61, 62,
            63, 65, 67, 69, 71, 77, 79, 80,
            81, 83, 84, 85, 86, 87, 88, 89,
            91, 92, 93, 94, 95, 97, 99, 101,
            103, 109, 111, 112, 113, 115, 116,
            117, 118, 119, 120, 121, 123, 124,
            125, 126, 127, 131, 133, 135, 141,
            143, 149, 151, 157, 159, 181, 183,
            189, 191, 192, 193, 195, 197, 199,
            205, 207, 208, 209, 211, 212, 213,
            214, 215, 216, 217, 219, 220, 221,
            222, 223, 224, 225, 227, 229, 231,
            237, 239, 240, 241, 243, 244, 245,
            246, 247, 248, 249, 251, 252, 253, 254, 255 };

        public KMM() { }

        public Bitmap Thin(Bitmap img)
        {
            Bitmap b = Check23(Set4(Set23(Start(img))));
            while (true)
            {
                if (Is1pxl(b)) break;
                b = Check23(Set4(Set23(img)));
            }

            return ToImage(b);
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
                        if ((c.R + c.G + c.B)/3 < 30)
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

        private Bitmap Set23(Bitmap img)
        {
            Color c;
            for (int j = 1; j < img.Height - 1; j++)
                for (int i = 1; i < img.Width - 1; i++)
                {
                    c = img.GetPixel(i, j);
                    try
                    {
                        if ((c.R > 0) && (img.GetPixel(i - 1, j).R == 0 || img.GetPixel(i + 1, j).R == 0 || img.GetPixel(i, j - 1).R == 0 || img.GetPixel(i, j + 1).R == 0))
                            img.SetPixel(i, j, Color.FromArgb(c.A, 2, 2, 2));
                        else if ((c.R > 0) && (img.GetPixel(i - 1, j - 1).R == 0 || img.GetPixel(i + 1, j - 1).R == 0 || img.GetPixel(i + 1, j - 1).R == 0 || img.GetPixel(i + 1, j + 1).R == 0))
                            img.SetPixel(i, j, Color.FromArgb(c.A, 3, 3, 3));
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Images with indexed pixels are unfortunately unsupported");
                        break;
                    }
                    catch (ArgumentOutOfRangeException) { }
                }
            return img;
        }

        private Bitmap Set4(Bitmap img)
        {
            Color c;
            for (int j = 1; j < img.Height - 1; j++)
                for (int i = 1; i < img.Width - 1; i++)
                {
                    c = img.GetPixel(i, j);
                    try
                    {
                        if (c.R > 1)
                        {
                            int stick = 0;
                            int[] neighs = new int[8];
                            if (img.GetPixel(i - 1, j).R > 0)
                            {
                                neighs[stick] = 1;
                                stick++;
                            }
                            if (img.GetPixel(i - 1, j + 1).R > 0)
                            {
                                neighs[stick] = 2;
                                stick++;
                            }
                            if (img.GetPixel(i, j + 1).R > 0)
                            {
                                neighs[stick] = 3;
                                stick++;
                            }
                            if (img.GetPixel(i + 1, j + 1).R > 0)
                            {
                                neighs[stick] = 4;
                                stick++;
                            }
                            if (img.GetPixel(i + 1, j).R > 0)
                            {
                                neighs[stick] = 5;
                                stick++;
                            }
                            if (img.GetPixel(i + 1, j - 1).R > 0)
                            {
                                neighs[stick] = 6;
                                stick++;
                            }
                            if (img.GetPixel(i, j - 1).R > 0)
                            {
                                neighs[stick] = 7;
                                stick++;
                            }
                            if (img.GetPixel(i - 1, j - 1).R > 0)
                            {
                                neighs[stick] = 8;
                                stick++;
                            }

                            if ((stick == 2) && ((neighs[1] - neighs[0]) == 1)) img.SetPixel(i, j, Color.FromArgb(c.A, 4, 4, 4));
                            if ((stick == 3) && ((neighs[1] - neighs[0]) == 1) && ((neighs[2] - neighs[1]) == 1)) img.SetPixel(i, j, Color.FromArgb(c.A, 4, 4, 4));
                            if ((stick == 3) && ((neighs[1] - neighs[0]) == 1) && ((neighs[2] - neighs[1]) == 1) && ((neighs[3] - neighs[2]) == 1)) img.SetPixel(i, j, Color.FromArgb(c.A, 4, 4, 4));
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Images with indexed pixels are unfortunately unsupported");
                        break;
                    }
                    catch (ArgumentOutOfRangeException) { }
                }

            for (int j = 1; j < img.Height - 1; j++)
                for (int i = 1; i < img.Width - 1; i++)
                {
                    c = img.GetPixel(i, j);
                    try
                    {
                        if (c.R == 4)
                            img.SetPixel(i, j, Color.FromArgb(c.A, 0, 0, 0));
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Images with indexed pixels are unfortunately unsupported");
                        break;
                    }
                    catch (ArgumentOutOfRangeException) { }
                }

            return img;
        }

        private Bitmap Check23(Bitmap img)
        {
            Color c;
            int N = 2;
            while (N < 4)
            {
                for (int j = 1; j < img.Height - 1; j++)
                    for (int i = 1; i < img.Width - 1; i++)
                    {
                        c = img.GetPixel(i, j);
                        try
                        {
                            if (c.R == N)
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

                                if (deletion.Contains(weight)) img.SetPixel(i, j, Color.FromArgb(c.A, 0, 0, 0));
                                else img.SetPixel(i, j, Color.FromArgb(c.A, 1, 1, 1));
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            MessageBox.Show("Images with indexed pixels are unfortunately unsupported");
                            break;
                        }
                        catch (ArgumentOutOfRangeException) { }
                    }
                N++;
            }
            return img;
        }

        private bool Is1pxl(Bitmap img)
        {
            Color c;
            for (int j = 1; j < img.Height-1; j++)
                for (int i = 1; i < img.Width-1; i++)
                {
                    c = img.GetPixel(i, j);
                    try
                    {
                        if ((c.R > 0) && ((img.GetPixel(i - 1, j).R +
                                img.GetPixel(i - 1, j + 1).R +
                                img.GetPixel(i, j + 1).R +
                                img.GetPixel(i + 1, j + 1).R +
                                img.GetPixel(i + 1, j).R +
                                img.GetPixel(i + 1, j - 1).R +
                                img.GetPixel(i, j - 1).R +
                                img.GetPixel(i - 1, j - 1).R) > 4))
                            return false;
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Images with indexed pixels are unfortunately unsupported");
                        break;
                    }
                    catch (ArgumentOutOfRangeException) { }
                }

            return true;
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
                        if(c.R==1)
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
    }
}
