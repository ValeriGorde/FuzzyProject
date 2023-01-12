﻿using FuzzyProject.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
            var list = FindList(img);
            var maxKey = list.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            Bitmap newBitmap = new Bitmap(img.Width, img.Height);
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    System.Drawing.Color pixel = img.GetPixel(i, j);
                    newBitmap.SetPixel(i, j, maxKey);
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
