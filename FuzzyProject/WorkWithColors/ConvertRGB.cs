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

        //используется этот метод
        public int[] GetLabToRGB(double l, double a, double b)
        {

            double _y = (l + 16) / 116;
            double _x = a / 500 + _y;
            double _z = _y - b / 200;

            if (Math.Pow(_y, 3) > 0.008856) _y = Math.Pow(_y, 3);
            else _y = (_y - 16 / 116) / 7.787;

            if (Math.Pow(_x, 3) > 0.008856) _x = Math.Pow(_x, 3);
            else _x = (_x - 16 / 116) / 7.787;

            if (Math.Pow(_z, 3) > 0.008856) _z = Math.Pow(_z, 3);
            else _z = (_z - 16 / 116) / 7.787;

            _x *= 95.047;
            _y *= 100;
            _z *= 108.883;

            _x /= 100;
            _y /= 100;
            _z /= 100;

            double _r = _x * 2.04137 + _y * -0.56495 + _z * -0.34469;
            double _g = _x * -0.96927 + _y * 1.87601 + _z * 0.04156;
            double _b = _x * 0.01345 + _y * -0.11839 + _z * 1.01541;

            _r = Math.Pow(_r, (1 / 2.19921875));
            _g = Math.Pow(_g, (1 / 2.19921875));
            _b = Math.Pow(_b, (1 / 2.19921875));

            _r *= 255;
            _g *= 255;
            _b *= 255;

            if (_r > 255) _r = 255;
            else if (_g > 255) _g = 255;
            else if (_b > 255) _b = 255;

            int[] colorsRGB = new int[] { (int)_r, (int)_g, (int)_b };
                        
            return colorsRGB;
        }
    }
}
