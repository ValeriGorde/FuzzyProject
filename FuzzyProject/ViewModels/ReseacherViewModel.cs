using FuzzyProject.DB_EF;
using FuzzyProject.FuzzyLogic;
using FuzzyProject.Models;
using FuzzyProject.WorkWithColors;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF_MVVM_Classes;
using Material = FuzzyProject.Models.Material;

namespace FuzzyProject.ViewModels
{
    internal class ReseacherViewModel : ViewModelBase
    {
        CalculateImg calculate;
        SetVariable setVariable;
        ReccomendsForOperator recommends;

        public string login;
        private readonly Window reseacherWindow;
        private Bitmap imgFirst;
        private Bitmap imgSecond;
        private Bitmap imgSpectr;
        private string selectedFileName;
        private string imgPath = "c:\\";

        public ReseacherViewModel(Window _reseacherWindow, string _login)
        {
            BrushesL = BrushesA = BrushesB = System.Drawing.Color.Gray.Name.ToString();

            ColorsChoice = "белый";
            ColorsChoiceSpect = "белый";

            ColorsView = new List<string>();
            ColorsView.Add("белый");
            ColorsView.Add("оранжевый");
            ColorsView.Add("голубой");
            ColorsView.Add("зелёный");

            ColorsViewSpect = new List<string>();
            ColorsViewSpect.Add("белый");
            ColorsViewSpect.Add("оранжевый");
            ColorsViewSpect.Add("голубой");
            ColorsViewSpect.Add("зелёный");

            reseacherWindow = _reseacherWindow;
            login = _login;
        }

        #region References

        private string _referencesCoorL;
        public string ReferenceCoorL
        {
            get { return _referencesCoorL; }
            set
            {
                _referencesCoorL = value;
                OnPropertyChanged();
            }
        }

        private string _referencesCoorA;
        public string ReferenceCoorA
        {
            get { return _referencesCoorA; }
            set
            {
                _referencesCoorA = value;
                OnPropertyChanged();
            }
        }

        private string _referencesCoorB;
        public string ReferenceCoorB
        {
            get { return _referencesCoorB; }
            set
            {
                _referencesCoorB = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _getReferences;
        public RelayCommand GetReferences
        {
            get
            {
                return _getReferences ??= new RelayCommand(x =>
            {
                if (ColorsChoice != null)
                {
                    using (AppContextDB context = new AppContextDB())
                    {
                        ReferenceParam reference;
                        reference = context.ReferencesParams.FirstOrDefault(r => r.Color == ColorsChoice);
                        ReferenceCoorL = reference.CoordinateL.ToString();
                        ReferenceCoorA = reference.CoordinateA.ToString();
                        ReferenceCoorB = reference.CoordinateB.ToString();
                    }
                }
            });
            }
        }
        #endregion

        #region From Image
        private string _slValue;
        public string SlValue
        {
            get { return _slValue; }
            set
            {
                _slValue = value;
                OnPropertyChanged();
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

        private double _sliderImg;
        public double SliderImg
        {
            get { return _sliderImg; }
            set
            {
                _sliderImg = value;
                OnPropertyChanged();
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
                        imgFirst = new Bitmap(selectedFileName);
                        StartImg = calculate.BitmapToImageSource(imgFirst);
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
                        var newImg = calculate.Convert(imgFirst);
                        EndImg = calculate.BitmapToImageSource(newImg);
                        imgSecond = newImg;

                        var colorsLAB = calculate.GetLAB(imgFirst);
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
                    ReferenceCoorL = " ";
                    ReferenceCoorA = " ";
                    ReferenceCoorB = " ";
                    ColorsChoice = null;
                    StartImg = null;
                    EndImg = null;
                });
            }
        }

        private RelayCommand _saveReport;
        public RelayCommand SaveReport
        {
            get
            {
                return _saveReport ??= new RelayCommand(x => { });
            }
        }


        private Recommendations? recommendations = null;
        private RecommendationsViewModel recommendationsViewModel;

        private RelayCommand _analyse;
        public RelayCommand Analyse
        {
            get
            {
                return _analyse ??= new RelayCommand(x =>
                {

                    if (ColorsChoice != null)
                    {
                        setVariable = new SetVariable();

                        var coorL = Convert.ToDouble(CoorL);
                        var coorA = Convert.ToDouble(CoorA);
                        var coorB = Convert.ToDouble(CoorB);

                        var coors = new double[] { coorL, coorA, coorB };

                        var result = setVariable.SetCharachteristics(coors, ColorsChoice);
                        string recommendation = " ";

                        //вывод рекомендаций
                        recommends = new ReccomendsForOperator();
                        recommendation = recommends.GiveRecommend(result);


                        string date = DateTime.Now.ToString();
                        string parameters = CoorL + " " + CoorA + " " + CoorB;
                        byte[] imgArr;

                        //перевод изображения в биты для сохранения в БД
                        using (MemoryStream ms = new MemoryStream())
                        {
                            imgSecond.Save(ms, ImageFormat.Bmp);
                            imgSecond.Save(ms, ImageFormat.Bmp);
                            imgArr = ms.ToArray();
                        }

                        //перенести потом ниже, когда все рекоммендации добавяться
                        byte[] img;
                        using (AppContextDB contextDB = new AppContextDB())
                        {
                            var referenceImg = contextDB.ReferencesParams.FirstOrDefault(r => r.Color == ColorsChoice);
                            img = referenceImg.Image;
                        }

                        var refImgBitmap = calculate.FromBytesToBitmap(img);
                        var refImgSource = calculate.BitmapToImageSource(refImgBitmap);

                        recommendations = new Recommendations();
                        recommendationsViewModel = new RecommendationsViewModel(recommendation, refImgSource, recommendations);
                        recommendations.DataContext = recommendationsViewModel;
                        recommendations.Show();
                        //

                        using (AppContextDB context = new AppContextDB())
                        {
                            var account = context.Accounts.FirstOrDefault(a => a.Login == login);
                            var material = context.Materials.FirstOrDefault(m => m.Image == imgArr);

                            Report report = new Report { Date = date, Login = login, Reccomendation = recommendation };
                            context.Reports.Add(report);

                            if (material == null)
                            {
                                Material newMaterial = new Material { Name = "Экструдат", Color = ColorsChoice, Parameters = parameters, Image = imgArr };
                                context.Materials.Add(newMaterial);
                                report.Account = account;
                                report.Material = newMaterial;
                            }
                            else
                            {
                                //привязка к пользователю
                                report.Account = account;
                                report.Material = material;
                            }
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
        #endregion

        #region From Spectrophotometer
        private BitmapSource _spectrImg;
        public BitmapSource SpectrImg
        {
            get { return _spectrImg; }
            set
            {
                _spectrImg = value;
                OnPropertyChanged();
            }
        }

        private string _spectCoorL;
        public string SpectCoorL
        {
            get { return _spectCoorL; }
            set
            {
                _spectCoorL = value;
                OnPropertyChanged();
            }
        }

        private string _spectCoorA;
        public string SpectCoorA
        {
            get { return _spectCoorA; }
            set
            {
                _spectCoorA = value;
                OnPropertyChanged();
            }
        }

        private string _spectCoorB;
        public string SpectCoorB
        {
            get { return _spectCoorB; }
            set
            {
                _spectCoorB = value;
                OnPropertyChanged();
            }
        }

        private string _spectReferenceCoorL;
        public string SpectReferenceCoorL
        {
            get { return _spectReferenceCoorL; }
            set
            {
                _spectReferenceCoorL = value;
                OnPropertyChanged();
            }
        }

        private string _spectReferenceCoorA;
        public string SpectReferenceCoorA
        {
            get { return _spectReferenceCoorA; }
            set
            {
                _spectReferenceCoorA = value;
                OnPropertyChanged();
            }
        }

        private string _spectReferenceCoorB;
        public string SpectReferenceCoorB
        {
            get { return _spectReferenceCoorB; }
            set
            {
                _spectReferenceCoorB = value;
                OnPropertyChanged();
            }
        }

        private List<string> _colorsViewSpect;
        public List<string> ColorsViewSpect
        {
            get { return _colorsViewSpect; }
            set
            {
                _colorsViewSpect = value;
                OnPropertyChanged();
            }
        }

        private string _colorsChoiceSpect;
        public string ColorsChoiceSpect
        {
            get { return _colorsChoiceSpect; }
            set
            {
                _colorsChoiceSpect = value;
                OnPropertyChanged();
            }
        }

        private double _sliderSpectr;
        public double SliderSpectr
        {
            get { return _sliderSpectr; }
            set
            {
                _sliderSpectr = value;
                OnPropertyChanged();
            }
        }

        private string _brushesL;
        public string BrushesL
        {
            get { return _brushesL; }
            set
            {
                _brushesL = value;
                OnPropertyChanged();
            }
        }

        private string _brushesA;
        public string BrushesA
        {
            get { return _brushesA; }
            set
            {
                _brushesA = value;
                OnPropertyChanged();
            }
        }

        private string _brushesB;
        public string BrushesB
        {
            get { return _brushesB; }
            set
            {
                _brushesB = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _analyseSpectr;
        public RelayCommand AnalyseSpectr
        {
            get
            {
                return _analyseSpectr ??= new RelayCommand(x =>
                {
                   
                    if (ColorsChoiceSpect != null)
                    {
                        setVariable = new SetVariable();

                        var coorL = Convert.ToDouble(SpectCoorL);
                        var coorA = Convert.ToDouble(SpectCoorA);
                        var coorB = Convert.ToDouble(SpectCoorB);

                        var coors = new double[] { coorL, coorA, coorB };

                        var result = setVariable.SetCharachteristics(coors, ColorsChoiceSpect);
                        string recommendation = " ";

                        recommends = new ReccomendsForOperator();
                        recommendation = recommends.GiveRecommend(result);

                        //перенести потом ниже, когда все рекоммендации добавяться
                        byte[] img;
                        using (AppContextDB contextDB = new AppContextDB())
                        {
                            var referenceImg = contextDB.ReferencesParams.FirstOrDefault(r => r.Color == ColorsChoiceSpect);
                            img = referenceImg.Image;
                        }

                        var refImgBitmap = calculate.FromBytesToBitmap(img);
                        var refImgSource = calculate.BitmapToImageSource(refImgBitmap);

                        recommendations = new Recommendations();
                        recommendationsViewModel = new RecommendationsViewModel(recommendation, refImgSource, recommendations);
                        recommendations.DataContext = recommendationsViewModel;
                        recommendations.Show();

                        string date = DateTime.Now.ToString();
                        string parameters = SpectCoorL + " " + SpectCoorA + " " + SpectCoorB;
                        byte[] imgArr;

                        //перевод изображения в биты для сохранения в БД
                        using (MemoryStream ms = new MemoryStream())
                        {
                            imgSpectr.Save(ms, ImageFormat.Bmp);
                            imgArr = ms.ToArray();
                        }

                        using (AppContextDB context = new AppContextDB())
                        {
                            var account = context.Accounts.FirstOrDefault(a => a.Login == login);
                            var material = context.Materials.FirstOrDefault(m => m.Image == imgArr);

                            Report report = new Report { Date = date, Login = login, Reccomendation = recommendation };
                            context.Reports.Add(report);

                            if (material == null)
                            {
                                Material newMaterial = new Material { Name = "Экструдат", Color = ColorsChoiceSpect, Parameters = parameters, Image = imgArr };
                                context.Materials.Add(newMaterial);
                                report.Account = account;
                                report.Material = newMaterial;
                            }
                            else
                            {
                                //привязка к пользователю
                                report.Account = account;
                                report.Material = material;
                            }
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

        private RelayCommand _getSpectrColor;
        public RelayCommand GetSpectrColor
        {
            get
            {
                return _getSpectrColor ??= new RelayCommand(x =>
                {
                    if (SpectCoorL != "" && SpectCoorA != "" && SpectCoorB != "")
                    {
                        BrushesL = BrushesA = BrushesB = System.Drawing.Color.Gray.Name.ToString();

                        SpectrImg = null;
                        double coorL = Convert.ToDouble(SpectCoorL);
                        double coorA = Convert.ToDouble(SpectCoorA);
                        double coorB = Convert.ToDouble(SpectCoorB);

                        if (coorL < 0 || coorL > 100)
                        {
                            BrushesL = System.Drawing.Color.Red.Name.ToString();
                        }
                        else if (coorA < -127 || coorA > 128)
                        {
                            BrushesA = System.Drawing.Color.Red.Name.ToString(); 
                        }
                        else if (coorB < -127 || coorB > 128)
                        {
                            BrushesB = System.Drawing.Color.Red.Name.ToString();
                        }
                        else 
                        {
                            ConvertRGB convert = new ConvertRGB();
                            int[] rgb = convert.GetLabToRGB(coorL, coorA, coorB);

                            calculate = new CalculateImg();
                            var newImg = calculate.GetColor(rgb[0], rgb[1], rgb[2]);
                            SpectrImg = calculate.BitmapToImageSource(newImg);
                            imgSpectr = newImg;
                        }
                    }
                    else 
                    {
                        MessageBox.Show("Заполните все поля!", "Ошибка");
                    }
                    
                });
            }
        }

        private RelayCommand _clearSpectrForm;
        public RelayCommand ClearSpectrForm
        {
            get
            {
                return _clearSpectrForm ??= new RelayCommand(x =>
                {
                    SpectrImg = null;
                    SpectCoorL = " ";
                    SpectCoorA = " ";
                    SpectCoorB = " ";
                    SpectReferenceCoorL = " ";
                    SpectReferenceCoorA = " ";
                    SpectReferenceCoorB = " ";
                    ColorsChoiceSpect = null;
                });
            }
        }

        private RelayCommand _getSpectReferences;
        public RelayCommand GetSpectReferences
        {
            get
            {
                return _getSpectReferences ??= new RelayCommand(x =>
                {
                    if (ColorsChoiceSpect != null)
                    {
                        using (AppContextDB context = new AppContextDB())
                        {
                            ReferenceParam reference;
                            reference = context.ReferencesParams.FirstOrDefault(r => r.Color == ColorsChoiceSpect);
                            SpectReferenceCoorL = reference.CoordinateL.ToString();
                            SpectReferenceCoorA = reference.CoordinateA.ToString();
                            SpectReferenceCoorB = reference.CoordinateB.ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Установите необходимый цвет!", "Ошибка при загрузке эталонов");
                    }

                    ConvertRGB convert = new ConvertRGB();
                    convert.ConvertLabToRGB(60, 60, -13);
                });
            }
        }

        #endregion


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
