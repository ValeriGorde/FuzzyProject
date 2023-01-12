using FuzzyProject.WorkWithColors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF_MVVM_Classes;

namespace FuzzyProject.ViewModels
{
    internal class ImagePageViewModel: ViewModelBase
    {
        private readonly Window imageWiindow;
        private Bitmap imageNew;
        CalculateImg calculate;

        public ImagePageViewModel(Window _imageWindow, Bitmap _imageNew) 
        {
            imageWiindow = _imageWindow;
            imageNew = _imageNew;

            calculate = new CalculateImg();
            Image = calculate.BitmapToImageSource(imageNew);
        }

        private BitmapSource _image;
        public BitmapSource Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }


    }
}
