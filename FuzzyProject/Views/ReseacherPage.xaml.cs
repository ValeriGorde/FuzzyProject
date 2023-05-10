using FuzzyProject.ViewModels;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace FuzzyProject.Views
{
    /// <summary>
    /// Логика взаимодействия для ReseacherPage.xaml
    /// </summary>
    public partial class ReseacherPage : Window
    {
        public ReseacherPage()
        {
            InitializeComponent();
        }

        RenderTargetBitmap bitmapImage;
        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            bitmapImage = getPixels(image);

            Point position = e.GetPosition(image);
            if ((position.X <= bitmapImage.PixelWidth) && (position.Y <= bitmapImage.PixelHeight))
            {
                var croppedBitmap = new CroppedBitmap(bitmapImage, new Int32Rect((int)position.X, (int)position.Y, 1, 1));

                var pixels = new byte[4];
                croppedBitmap.CopyPixels(pixels, 4, 0);

                //border.Background = new SolidColorBrush(Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]));
            }
        }

        RenderTargetBitmap getPixels(FrameworkElement elem)
        {
            PresentationSource source = PresentationSource.FromVisual(this);
            double dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
            double dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;

            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)elem.ActualWidth, (int)elem.ActualHeight, dpiX, dpiY, PixelFormats.Pbgra32);
            bitmap.Render(elem);

            return bitmap;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
