using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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

        public int[] FromBitmapToAverage(Bitmap bmp) 
        {
            double total_R = 0.0;
            double total_G = 0.0;
            double total_B = 0.0;
            int numPixels = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);
                    int red = pixelColor.R;
                    int green = pixelColor.G;
                    int blue = pixelColor.B;

                    total_R += red;
                    total_G += green;
                    total_B += blue;
                    numPixels++;
                }
            }

            int avg_R = (int)(total_R / numPixels);
            int avg_G = (int)(total_G / numPixels);
            int avg_B = (int)(total_B / numPixels);

            return new int[] { avg_R, avg_G, avg_B};
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



        public int[] FromFistPicToSecond(Bitmap bmp) 
        {

            // Find the average color of the image
            double total_R = 0.0;
            double total_G = 0.0;
            double total_B = 0.0;
            int numPixels = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);
                    int red = pixelColor.R;
                    int green = pixelColor.G;
                    int blue = pixelColor.B;

                    total_R += red;
                    total_G += green;
                    total_B += blue;
                    numPixels++;
                }
            }

            int avg_R = (int)(total_R / numPixels);
            int avg_G = (int)(total_G / numPixels);
            int avg_B = (int)(total_B / numPixels);

            return new int[] { avg_R, avg_G, avg_B };


            

        }

        public Bitmap NewBitmap(int R, int G, int B) 
        {
            Color avgColor = Color.FromArgb(R, G, B);


            int size = 100;
            Bitmap newBmp = new Bitmap(size, size);

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    newBmp.SetPixel(x, y, avgColor);
                }
            }

            return newBmp;
        }
    }
}
