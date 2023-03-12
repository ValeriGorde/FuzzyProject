using FuzzyProject.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace FuzzyProject.WorkWithColors
{
    internal class CalculateImg
    {
        Models.Color color;
        ConvertRGB convert;

        public byte[] FromImgToBytes(Bitmap image) 
        {
            byte[] imgMaterial;

            //перевод изображения в биты для сохранения в БД
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Bmp);
                image.Save(ms, ImageFormat.Bmp);
                imgMaterial = ms.ToArray();
                
            }
            return imgMaterial;
        }

        public BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        

        public double[] GetLAB(Bitmap img) 
        {
            var list = FindList(img);
            var maxKey = list.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            color.R = maxKey.R;
            color.G = maxKey.G;
            color.B = maxKey.B;

            convert = new ConvertRGB();
            var colorsLAB = convert.ConvertRGBToLab(color.R, color.G, color.B);

            return colorsLAB;
        }

        public System.Drawing.Color FindAveragePixcel(Bitmap img)
        {
            color = new Models.Color();

            int r = 0;
            int g = 0;
            int b = 0;

            int sum = 0;

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    System.Drawing.Color pixel = img.GetPixel(i, j);
                    r += pixel.R;
                    g += pixel.G;
                    b += pixel.B;

                    sum++;
                }
            }

            r /= sum;
            g /= sum;
            b /= sum;

            return Color.FromArgb(r, g, b);
        }

        public Dictionary<System.Drawing.Color, int> FindList(Bitmap img) 
        {
            color = new Models.Color();
            var list = new Dictionary<System.Drawing.Color, int>();
            list.Clear();

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    System.Drawing.Color pixel = img.GetPixel(i, j);

                    if (!list.ContainsKey(pixel))
                    {
                        list.Add(pixel, 1);
                    }
                    else
                        list[pixel]++;
                }
            }
            return list;
        }

        public Bitmap Convert(Bitmap img) 
        {
            //var list = FindList(img);
            //var maxKey = list.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

            var avrPix = FindAveragePixcel(img);

            Bitmap newBitmap = new Bitmap(img.Width, img.Height);
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    System.Drawing.Color pixel = img.GetPixel(i, j);
                    newBitmap.SetPixel(i, j, avrPix);
                }
            }
            return newBitmap;
        }

        public Bitmap GetColor(int red, int green, int blue)
        {
            Color newColor = Color.FromArgb(red, green, blue);

            Bitmap newBitmap = new Bitmap(80, 70);
            for (int i = 0; i < 80; i++)
            {
                for (int j = 0; j < 70; j++)
                {
                    newBitmap.SetPixel(i, j, newColor);
                }
            }
            return newBitmap;
        }

        public Bitmap FromBytesToBitmap(byte[] pic) 
        {
            ImageConverter ic = new ImageConverter();
            Image img = (Image)ic.ConvertFrom(pic);
            Bitmap newPic = new Bitmap(img);

            return newPic;
        }
    }
}
