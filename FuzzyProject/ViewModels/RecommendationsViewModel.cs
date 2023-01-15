using FuzzyProject.WorkWithColors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF_MVVM_Classes;

namespace FuzzyProject.ViewModels
{
    internal class RecommendationsViewModel: ViewModelBase
    {
        private readonly Window _recommendWindow;
        public RecommendationsViewModel(string textRecommend, BitmapSource imgReccomend, Window recommendWindow) 
        {
            _recommendText = textRecommend;
            _recommendImg = imgReccomend;
            _recommendWindow = recommendWindow;
        }

        private string _recommendText;
        public string RecommendText 
        {
            get { return _recommendText; }
            set 
            {
                _recommendText = value;
                OnPropertyChanged();
            }
        }

        private BitmapSource _recommendImg;
        public BitmapSource RecommendImg
        {
            get { return _recommendImg; }
            set
            {
                _recommendImg = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _close;
        public RelayCommand Close 
        {
            get 
            {
                return _close ??= new RelayCommand(x =>
                {
                    _recommendWindow.Close();
                });
            }
        }

    }
}
