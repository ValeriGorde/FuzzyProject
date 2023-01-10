using FLS;
using FuzzyProject.DB_EF;
using FuzzyProject.FuzzyLogic;
using FuzzyProject.Models;
using FuzzyProject.WorkWithColors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF_MVVM_Classes;
using static System.Windows.Forms.DataFormats;

namespace FuzzyProject.ViewModels
{
    internal class ReseacherViewModel: ViewModelBase
    {
        Models.Color color;
        CalculateImg calculate;
        SetVariable setVariable;
        ReccomendsForOperator recommends;

        public string login;
        private readonly Window reseacherWindow;
        private Bitmap img;
        private string selectedFileName;
        private string imgPath = "c:\\";

        public ReseacherViewModel(Window _reseacherWindow, string _login) 
        {
            ColorsView = new List<string>();
            ColorsView.Add("Жёлтый");
            ColorsView.Add("Синий");
            reseacherWindow = _reseacherWindow;
            login = _login;
        }

        public void UpdateBD()
        {
            using (ApplicationContext context = new ApplicationContext()) 
            {
                context.Reports.Load();
            }
        }

        private List<string> _colorsView;
        public List<string> ColorsView
        {
            get { return _colorsView; }
            set
            {
                _colorsView = value;
                OnPropertyChanged();
            }
        }

        private string _colorsChoice;
        public string ColorsChoice
        {
            get { return _colorsChoice; }
            set
            {
                _colorsChoice = value;
                OnPropertyChanged();
            }
        }

        private string _result;
        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        private string _coorL;
        public string CoorL 
        {
            get { return _coorL; }
            set 
            {
                _coorL = value;
                OnPropertyChanged();
            }
        }

        private string _coorA;
        public string CoorA
        {
            get { return _coorA; }
            set
            {
                _coorA = value;
                OnPropertyChanged();
            }
        }

        private string _coorB;
        public string CoorB
        {
            get { return _coorB; }
            set
            {
                _coorB = value;
                OnPropertyChanged();
            }
        }

        private BitmapSource _startImg;
        public BitmapSource StartImg 
        {
            get { return _startImg; }
            set 
            {
                _startImg = value;
                OnPropertyChanged();
            }
        }

        private BitmapSource _endImg;
        public BitmapSource EndImg
        {
            get { return _endImg; }
            set
            {
                _endImg = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _getResult;
        public RelayCommand GetResult
        {
            get
            {
                return _getResult ??= new RelayCommand(x =>
                {
                   
                });
            }
        }

        private RelayCommand _openImg;
        public RelayCommand OpenImg 
        {
            get 
            {
                return _openImg ??= new RelayCommand(x => 
                {
                    calculate = new CalculateImg();
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.InitialDirectory = imgPath;
                    dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
                    dlg.RestoreDirectory = true;
                    if (dlg.ShowDialog() == true)
                    {
                        selectedFileName = dlg.FileName;
                        img = new Bitmap(selectedFileName);
                        StartImg = calculate.BitmapToImageSource(img);
                    }
                });
            }
        }

        private RelayCommand _getColor;
        public RelayCommand GetColor 
        {
            get 
            {
                return _getColor ??= new RelayCommand(x => 
                {
                    if (StartImg != null)
                    {
                        var newImg = calculate.Convert(img);
                        EndImg = calculate.BitmapToImageSource(newImg);

                        var colorsLAB = calculate.GetLAB(img);
                        CoorL = colorsLAB[0].ToString();
                        CoorA = colorsLAB[1].ToString();
                        CoorB = colorsLAB[2].ToString();
                    }
                    else 
                    {
                        MessageBox.Show("Загрузите исходное изображение!", "Ошибка получения цвета");
                    }
                });
            }
        }

        private RelayCommand _clearForm;
        public RelayCommand ClearForm
        {
            get
            {
                return _clearForm ??= new RelayCommand(x => 
                {
                    CoorL = "";
                    CoorA = "";
                    CoorB = "";
                    StartImg = null;
                    EndImg = null;
                });
            }
        }        

        private RelayCommand _analyse;
        public RelayCommand Analyse
        {
            get
            {
                return _analyse ??= new RelayCommand(x =>
                {
                    setVariable = new SetVariable();

                    var coorL = Convert.ToDouble(CoorL);
                    var coorA = Convert.ToDouble(CoorA);
                    var coorB = Convert.ToDouble(CoorB);
                    var result = setVariable.SetCharachteristics(coorL, coorA, coorB);
                    string recommendation = " ";

                    if (ColorsChoice != null)
                    {
                        if (ColorsChoice == ColorsView[0])
                        {
                            recommends = new ReccomendsForOperator();
                            recommendation = recommends.GiveRecommend(result);
                        }
                        else
                        {
                            MessageBox.Show("В разработке", "Извините");
                        }

                        string date = DateTime.Now.ToString();
                        string parameters = CoorL + " " + CoorA + " " + CoorB;
                        byte[] imgArr;

                        //перевод изображения в биты для сохранения в БД
                        using (MemoryStream ms = new MemoryStream())
                        {
                            img.Save(ms, ImageFormat.Bmp);
                            imgArr = ms.ToArray();
                        }

                        using (ApplicationContext context = new ApplicationContext())
                        {
                            var account = context.Accounts.FirstOrDefault(a => a.Login == login);

                            Material material = new Material { Name = "Экструдат", Color = ColorsChoice, Parameters = parameters, Image = imgArr };
                            Report report = new Report { Date = date, Login = login, Reccomendation = recommendation };
                            context.Materials.Add(material);
                            context.Reports.Add(report);
                            //привязка к пользователю
                            report.Account = account;
                            report.Material = material;
                            context.SaveChanges();
                        }
                    }
                    else 
                    {
                        MessageBox.Show("Некоторые поля пустые", "Ошибка");
                    }
                });
            }
        }

        private Authorization? authorization = null;
        private AuthorizationViewModel authorizationViewModel;

        private RelayCommand _exit;
        public RelayCommand Exit
        {
            get
            {
                return _exit ??= new RelayCommand(x =>
                {

                    authorization = new Authorization();
                    authorizationViewModel = new AuthorizationViewModel(authorization);
                    authorization.DataContext = authorizationViewModel;
                    authorization.Show();
                    reseacherWindow.Close();
                });
            }
        }

    }
}
