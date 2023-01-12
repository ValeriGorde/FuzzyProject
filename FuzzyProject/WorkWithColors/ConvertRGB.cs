using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyProject.WorkWithColors
{
    internal class ConvertRGB
    {
        public double[] ConvertRGBToLab(double red, double green, double blue)
        {
            red /= 255;
            green /= 255;
            blue /= 255;

            if (red > 0.04045)
                red = Math.Pow(((red + 0.055) / 1.055), 2.4);
            else
                red = red / 12.92;

            if (green > 0.04045)
                green = Math.Pow(((green + 0.055) / 1.055), 2.4);
            else
                green = green / 12.92;

            if (blue > 0.04045)
                blue = Math.Pow(((blue + 0.055) / 1.055), 2.4);
            else
                blue = blue / 12.92;

            red *= 100;
            green *= 100;
            blue *= 100;

            //переводим в цветовое пространство XYZ
            double x, y, z;
            x = red * 0.4124 + green * 0.3576 + blue * 0.1805;
            y = red * 0.2126 + green * 0.7152 + blue * 0.0722;
            z = red * 0.0193 + green * 0.1192 + blue * 0.9505;

            //переводим из цветового пространства XYZ в CIE-L*ab 
            x /= 95.047;
            y /= 100;
            z /= 108.883;

            if (x > 0.008856)
                x = Math.Pow(x, (.3333333333));
            else
                x = (7.787 * x) + (16 / 116);

            if (y > 0.008856)
                y = Math.Pow(y, (.3333333333));
            else
                y = (7.787 * y) + (16 / 116);

            if (z > 0.008856)
                z = Math.Pow(z, (.3333333333));
            else
                z = (7.787 * z) + (16 / 116);

            double L, a, b;
            L = Math.Round((116 * y) - 16, 4);
            a = Math.Round(500 * (x - y), 4);
            b = Math.Round(200 * (y - z), 4);
            double[] colorsLAB = new double[] { L, a, b };

            return colorsLAB;
        }

        public int[] ConvertLabToRGB(double coorl, double coora, double coorb)
        {
            double x, y, z;
            double red, blue, green;

            double delta = 6.0 / 29;

            //перевод в XYZ 
            y = (coorl + 16) / 116;
            x = y + (coora / 500);
            z = y - (coorb / 200);

            if (x > delta)
                x = Math.Pow(x, 3);
            else
                x = (x - (4 / 29) * 3 * Math.Pow(delta, 2));

            if (y > delta)
                y = Math.Pow(y, 3);
            else
                y = (y - (4 / 29) * 3 * Math.Pow(delta, 2));

            if (z > delta)
                z = Math.Pow(z, 3);
            else
                z = (z - (4 / 29) * 3 * Math.Pow(delta, 2));

            x *= 95.047;
            y *= 100;
            z *= 108.883;

            red = x * 2.04137 + y * -0.56495 + z * -0.34469; // красный
            green = -x * 0.9692 + y * 1.8760 + z * 0.0416; // зеленый
            blue = x * 0.0556 - y * 0.2040 + z * 1.0570; // синий

            red /= 100;
            green /= 100;
            blue /= 100;

            if (red > 0.0031308)
                red = 1.055 * Math.Pow(red, 1 / 2.4) - 0.055;
            else
                red = 12.92 * red;

            if (green > 0.0031308)
                green = 1.055 * Math.Pow(green, 1 / 2.4) - 0.055;
            else
                green = 12.92 * green;

            if (blue > 0.0031308)
                blue = 1.055 * Math.Pow(blue, 1 / 2.4) - 0.055;
            else
                blue = 12.92 * blue;

            red *= 255;
            green *= 255;
            blue *= 255;

            int[] colorsRGB = new int[] { (int)red, (int)green, (int)blue };

                return colorsRGB;

        }

        public int[] GetLabToRGB(double l, double a, double b)
        {

            double _y = l / 116 + 0.1379310344827586;
            double _x = a / 500 + _y;
            double _z = _y - (b / 200);

            _x = _x > 0.2068965517241379 ? Math.Pow(_x, 3) : _x / 7.787 - 0.0177129876053369;
            _y = _y > 0.2068965517241379 ? Math.Pow(_y, 3) : _y / 7.787 - 0.0177129876053369;
            _z = _z > 0.2068965517241379 ? Math.Pow(_z, 3) : _z / 7.787 - 0.0177129876053369;

            // Observer = 2°, Illuminant = D65
            double R = 3.080093082 * _x - 1.5372 * _y - 0.542890638 * _z;
            double G = -0.920910383 * _x + 1.8758 * _y + 0.045186445 * _z;
            double B = 0.052941179 * _x - 0.2040 * _y + 1.150893310 * _z;

            R = R > 0.0031308 ? Math.Pow(R, 0.4166666666666667) * 269.025 - 14.025 : R * 3294.6;
            G = G > 0.0031308 ? Math.Pow(G, 0.4166666666666667) * 269.025 - 14.025 : G * 3294.6;
            B = B > 0.0031308 ? Math.Pow(B, 0.4166666666666667) * 269.025 - 14.025 : B * 3294.6;

            var red = Math.Round(R);
            var green = Math.Round(G);
            var blue = Math.Round(B);

            int[] colorsRGB = new int[] { (int)red, (int)green, (int)blue };

            return colorsRGB;
        }
    }
}
