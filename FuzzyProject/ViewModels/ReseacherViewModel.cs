using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using FuzzyProject.DB_EF;
using FuzzyProject.FuzzyLogic;
using FuzzyProject.Models;
using FuzzyProject.Views;
using FuzzyProject.WorkWithColors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using WPF_MVVM_Classes;
using Material = FuzzyProject.Models.Material;
using MessageBox = System.Windows.MessageBox;
using Parameter = FuzzyProject.Models.Parameter;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.Win32;
using FuzzyProject.Export;
using System.Windows.Controls;

namespace FuzzyProject.ViewModels
{
    internal class ReseacherViewModel : ViewModelBase
    {
        CalculateImg calculate;
        SetVariable setVariable;
        ReccomendsForOperator recommends;
        AppContextDB context;

        public string login;
        private readonly System.Windows.Window reseacherWindow;
        private Bitmap imgFirst;
        private Bitmap imgSecond;
        private Bitmap imgSpectr;
        private string selectedFileName;
        private string imgPath = "c:\\";

        public ReseacherViewModel(System.Windows.Window _reseacherWindow, string _login)
        {
            BrushesL = BrushesA = BrushesB = System.Drawing.Color.Gray.Name.ToString();
            reseacherWindow = _reseacherWindow;
            login = _login;
            context = new AppContextDB();

            //загрузка из бд
            context.Colorants.Load();
            context.Materials.Load();
            context.PolymerTypes.Load();
            Colorant = context.Colorants.ToList();
            PolymerType = context.PolymerTypes.ToList();
        }

        #region From Image

        private ReferenceParam _referencesFromImg;
        public ReferenceParam ReferencesFromImg
        {
            get { return _referencesFromImg; }
            set
            {
                _referencesFromImg = value;
                OnPropertyChanged();
            }
        }

        private List<Colorant> _colorant;
        public List<Colorant> Colorant
        {
            get { return _colorant; }
            set
            {
                _colorant = value;
                OnPropertyChanged();
            }
        }

        private List<PolymerType> _polymerType;
        public List<PolymerType> PolymerType
        {
            get { return _polymerType; }
            set
            {
                _polymerType = value;
                OnPropertyChanged();
            }
        }

        private Colorant _chosenColorantFromImg;
        public Colorant ChosenColorantFromImg
        {
            get { return _chosenColorantFromImg; }
            set
            {
                _chosenColorantFromImg = value;
                OnPropertyChanged();
            }
        }

        private PolymerType _chosenPolymerFromImg;
        public PolymerType ChosenPolymerFromImg
        {
            get { return _chosenPolymerFromImg; }
            set
            {
                _chosenPolymerFromImg = value;
                OnPropertyChanged();

                if (_chosenColorantFromImg != null && ChosenPolymerFromImg != null)
                {
                    var material = context.Materials.FirstOrDefault(m => m.Colorants == _chosenColorantFromImg && m.PolymerTypes == _chosenPolymerFromImg && m.IsReference == true);
                    if (material != null)
                    {
                        var parametrs = context.Parameters.FirstOrDefault(p => p.Id == material.ParametersId);
                        ImgReferenceCoorL = parametrs._L;
                        ImgReferenceCoorA = parametrs._A;
                        ImgReferenceCoorB = parametrs._B;
                    }
                    else
                        MessageBox.Show("К сожалению, эталонные координаты по данным характеристикам не найдены.\n" +
                            "Обратитесь к администратору.", "Ошибка получение эталонных координат");
                }
                else
                    MessageBox.Show("Необходимо выбрать краситель, для отображение эталонных характеристик!", "Ошибка получения эталонных координат");
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

        private double _coorL;
        public double CoorL
        {
            get { return _coorL; }
            set
            {
                _coorL = value;
                OnPropertyChanged();
            }
        }

        private double _coorA;
        public double CoorA
        {
            get { return _coorA; }
            set
            {
                _coorA = value;
                OnPropertyChanged();
            }
        }

        private double _coorB;
        public double CoorB
        {
            get { return _coorB; }
            set
            {
                _coorB = value;
                OnPropertyChanged();
            }
        }

        private double _imgReferenceCoorL;
        public double ImgReferenceCoorL
        {
            get { return _imgReferenceCoorL; }
            set
            {
                _imgReferenceCoorL = value;
                OnPropertyChanged();
            }
        }

        private double _imgReferenceCoorA;
        public double ImgReferenceCoorA
        {
            get { return _imgReferenceCoorA; }
            set
            {
                _imgReferenceCoorA = value;
                OnPropertyChanged();
            }
        }

        private double _imgReferenceCoorB;
        public double ImgReferenceCoorB
        {
            get { return _imgReferenceCoorB; }
            set
            {
                _imgReferenceCoorB = value;
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

        private RelayCommand _openImg;
        public RelayCommand OpenImg
        {
            get
            {
                return _openImg ??= new RelayCommand(x =>
                {
                    calculate = new CalculateImg();
                    var dlg = new Microsoft.Win32.OpenFileDialog();
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

                        var colorsLAB = calculate.GetLAB(newImg);
                        CoorL = colorsLAB[0];
                        CoorA = colorsLAB[1];
                        CoorB = colorsLAB[2];


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
                    CoorL = 0;
                    CoorA = 0;
                    CoorB = 0;
                    ImgReferenceCoorL = 0;
                    ImgReferenceCoorA = 0;
                    ImgReferenceCoorB = 0;
                    StartImg = null;
                    EndImg = null;
                });
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
                    if (imgSecond != null && ChosenPolymerFromImg != null && ChosenColorantFromImg != null)
                    {
                        setVariable = new SetVariable();

                        var coorL = Convert.ToDouble(CoorL);
                        var coorA = Convert.ToDouble(CoorA);
                        var coorB = Convert.ToDouble(CoorB);

                        //поиск эталонов
                        var material = context.Materials.FirstOrDefault(m => m.IsReference == true
                        && m.Colorants == ChosenColorantFromImg && m.PolymerTypes == ChosenPolymerFromImg);

                        var referenceParam = context.Parameters.FirstOrDefault(r => r.Id == material.ParametersId);

                        var coors = new double[] { coorL, coorA, coorB };
                        var refCoors = new double[] { referenceParam._L, referenceParam._A, referenceParam._B };

                        var rec = setVariable.GetDeltas(coors, refCoors, "поливинилхлорид");

                        var date = DateTime.Now.Date;
                        string parameters = CoorL + " " + CoorA + " " + CoorB;
                        byte[] imgArr;

                        //перевод изображения в биты для сохранения в БД
                        using (MemoryStream ms = new MemoryStream())
                        {
                            imgSecond.Save(ms, ImageFormat.Bmp);
                            imgSecond.Save(ms, ImageFormat.Bmp);
                            imgArr = ms.ToArray();
                        }

                        byte[] img;
                        img = material.Image;

                        var refImgBitmap = calculate.FromBytesToBitmap(img);
                        var refImgSource = calculate.BitmapToImageSource(refImgBitmap);

                        Result = rec;
                        recommendations = new Recommendations();
                        recommendationsViewModel = new RecommendationsViewModel(rec, refImgSource, recommendations);
                        recommendations.DataContext = recommendationsViewModel;
                        recommendations.Show();

                        //Добавление материала в БД
                        Parameter newParameter = new Parameter { _L = coorL, _A = coorA, _B = coorB };
                        context.Parameters.Add(newParameter);
                        context.SaveChanges();

                        var newMaterial = new Material
                        {
                            ColorantId = ChosenColorantFromImg.Id,
                            PolymerTypeId = ChosenPolymerFromImg.Id,
                            ParametersId = newParameter.Id,
                            Image = imgArr,
                            IsReference = false
                        };
                        context.Materials.Add(newMaterial);
                        context.SaveChanges();
                    }
                });
            }
        }
        #endregion

        #region From Spectrophotometer
        private Colorant _chosenColorantFromSpect;
        public Colorant ChosenColorantFromSpect
        {
            get { return _chosenColorantFromSpect; }
            set
            {
                _chosenColorantFromSpect = value;
                OnPropertyChanged();
            }
        }

        private PolymerType _chosenPolymerFromSpect;
        public PolymerType ChosenPolymerFromSpect
        {
            get { return _chosenPolymerFromSpect; }
            set
            {
                _chosenPolymerFromSpect = value;
                OnPropertyChanged();

                if (_chosenColorantFromSpect != null && ChosenPolymerFromSpect != null)
                {
                    var material = context.Materials.FirstOrDefault(m => m.Colorants == _chosenColorantFromSpect && m.PolymerTypes == _chosenPolymerFromSpect && m.IsReference == true);
                    if (material != null)
                    {
                        var parametrs = context.Parameters.FirstOrDefault(p => p.Id == material.ParametersId);
                        SpectReferenceCoorL = parametrs._L;
                        SpectReferenceCoorA = parametrs._A;
                        SpectReferenceCoorB = parametrs._B;
                    }
                    else
                        MessageBox.Show("К сожалению, эталонные координаты по данным характеристикам не найдены.\n" +
                            "Обратитесь к администратору.", "Ошибка получение эталонных координат");
                }
                else
                    MessageBox.Show("Необходимо выбрать краситель, для отображение эталонных характеристик!", "Ошибка получения эталонных координат");
            }
        }

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

        private double _spectCoorL;
        public double SpectCoorL
        {
            get { return _spectCoorL; }
            set
            {
                _spectCoorL = value;
                OnPropertyChanged();
            }
        }

        private double _spectCoorA;
        public double SpectCoorA
        {
            get { return _spectCoorA; }
            set
            {
                _spectCoorA = value;
                OnPropertyChanged();
            }
        }

        private double _spectCoorB;
        public double SpectCoorB
        {
            get { return _spectCoorB; }
            set
            {
                _spectCoorB = value;
                OnPropertyChanged();
            }
        }

        private double _spectReferenceCoorL;
        public double SpectReferenceCoorL
        {
            get { return _spectReferenceCoorL; }
            set
            {
                _spectReferenceCoorL = value;
                OnPropertyChanged();
            }
        }

        private double _spectReferenceCoorA;
        public double SpectReferenceCoorA
        {
            get { return _spectReferenceCoorA; }
            set
            {
                _spectReferenceCoorA = value;
                OnPropertyChanged();
            }
        }

        private double _spectReferenceCoorB;
        public double SpectReferenceCoorB
        {
            get { return _spectReferenceCoorB; }
            set
            {
                _spectReferenceCoorB = value;
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
                    setVariable = new SetVariable();

                    var coorL = Convert.ToDouble(SpectCoorL);
                    var coorA = Convert.ToDouble(SpectCoorA);
                    var coorB = Convert.ToDouble(SpectCoorB);

                    var material = context.Materials.FirstOrDefault(m => m.IsReference == true
                        && m.Colorants == ChosenColorantFromSpect && m.PolymerTypes == ChosenPolymerFromSpect);

                    var referenceParam = context.Parameters.FirstOrDefault(r => r.Id == material.ParametersId);

                    var coors = new double[] { coorL, coorA, coorB };
                    var refCoors = new double[] { referenceParam._L, referenceParam._A, referenceParam._B };


                    var rec = setVariable.GetDeltas(coors, refCoors, ChosenPolymerFromSpect.Name);

                    byte[] img;
                    img = material.Image;

                    var refImgBitmap = calculate.FromBytesToBitmap(img);
                    var refImgSource = calculate.BitmapToImageSource(refImgBitmap);

                    recommendations = new Recommendations();
                    recommendationsViewModel = new RecommendationsViewModel(rec, refImgSource, recommendations);
                    recommendations.DataContext = recommendationsViewModel;
                    recommendations.Show();

                    var date = DateTime.Now.Date;
                    string parameters = SpectCoorL + " " + SpectCoorA + " " + SpectCoorB;
                    byte[] imgArr;

                    //перевод изображения в биты для сохранения в БД
                    using (MemoryStream ms = new MemoryStream())
                    {
                        imgSpectr.Save(ms, ImageFormat.Bmp);
                        imgArr = ms.ToArray();
                    }

                    //Добавление материала в БД
                    Parameter newParameter = new Parameter { _L = coorL, _A = coorA, _B = coorB };
                    context.Parameters.Add(newParameter);
                    context.SaveChanges();

                    var newMaterial = new Material
                    {
                        ColorantId = ChosenColorantFromSpect.Id,
                        PolymerTypeId = ChosenPolymerFromSpect.Id,
                        ParametersId = newParameter.Id,
                        Image = imgArr,
                        IsReference = false
                    };
                    context.Materials.Add(newMaterial);
                    context.SaveChanges();
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
                        //int[] rgb = convert.ConvertLabToRgb(coorL, coorA, coorB);


                        calculate = new CalculateImg();
                        var newImg = calculate.GetColor(rgb[0], rgb[1], rgb[2]);
                        SpectrImg = calculate.BitmapToImageSource(newImg);
                        imgSpectr = newImg;
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
                    SpectCoorL = 0;
                    SpectCoorA = 0;
                    SpectCoorB = 0;
                    SpectReferenceCoorL = 0;
                    SpectReferenceCoorA = 0;
                    SpectReferenceCoorB = 0;
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
                    if (ChosenColorantFromSpect != null)
                    {
                        using (AppContextDB context = new AppContextDB())
                        {
                            SpectReferenceCoorL = 85;
                            SpectReferenceCoorA = 8;
                            SpectReferenceCoorB = -15;
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


        #region Menu
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

        private RelayCommand _info;
        public RelayCommand Info
        {
            get
            {
                return _info ??= new RelayCommand(x =>
                {
                    MessageBox.Show("Данный программный комплекс был выполнен в соответствии\n" +
                        "с заданием выпускной квалификационной работы.\n" +
                        "Тема ВКР: Программный комплекс для анализа цветовых характеристик экструдата на базе нечётких моделей.\n" +
                        "Цель работы: Разработка программного комплекса,\n" +
                        "позволяющего провести анализ измеренных цветовых координат экструдата\n" +
                        "в пространстве CIELab и оценить степень термической деструкции\n" +
                        "экструдата в производстве неокрашенной полимерной пленки по величине отклонения\n" +
                        "цвета экструдата от эталона для различных типов полимеров.\n" +
                        "Работу выполнила студентка 494 группы Гордеева Валерия Александровна\n" +
                        "Год: 2023\n", "Описание");
                });
            }
        }

        private RelayCommand _infoForOperator;
        public RelayCommand InfoForOperator
        {
            get
            {
                return _infoForOperator ??= new RelayCommand(x =>
                {
                    MessageBox.Show("Данный программный комплекс позволяет провести анализ\n" +
                        "измеренных цветовых координат экструдата\n" +
                        "в пространстве CIELab и оценить степень термической деструкции\n" +
                        "экструдата в производстве неокрашенной полимерной плёнки по величине отклонения\n" +
                        "цвета экструдата от эталона для различных типов полимеров.\n" +
                        "Необходимые действия для анализа экструдата:\n" +
                        "1) Выбор исходных данных (фотоизображение, либо цветовые координаты экструдата)\n" +
                        "2) Загрузка исходного изображения/ввод цветовых координат\n" +
                        "3) Выбор типа полимерной плёнки\n" +
                        "4) Нажатие на кнопку 'Анализ'", "Информация для оператора экструдера");
                });
            }
        }
        #endregion

        #region Создание отчёта
        private RelayCommand _report;
        public RelayCommand Report
        {
            get
            {
                return _report ??= new RelayCommand(x =>
                {
                    // запрашиваем у пользователя путь для сохранения файла
                    string path = ShowFolderBrowserDialog();

                    if (path != "/")
                    {
                        SaveInWord save = new SaveInWord();

                        // определяем номер отчёта как количество файлов с именами, начинающимися на "отчёт" за сегодняшний день
                        string pathFile = Path.Combine(path, $"Отчёт №1. {DateTime.Today.ToShortDateString()}.docx");
                        var file = new FileInfo(Path.Combine(path, pathFile));

                        int numFiles = 1;
                        while (file.Exists)
                        {
                            pathFile = Path.Combine(path, $"Отчет №{numFiles}. {DateTime.Today.ToShortDateString()}.docx");
                            file = new FileInfo(Path.Combine(pathFile));
                            numFiles++;
                        }

                        var coordinates = $"L = {CoorL}, a = {CoorA}, b = {CoorB}";
                        save.Export(pathFile, imgSecond, coordinates, ChosenPolymerFromImg.Name, ChosenColorantFromImg.Name, Result, imgFirst);
                    }
                    else return;

                });
            }
        }

        private RelayCommand _reportSpect;
        public RelayCommand ReportSpect
        {
            get
            {
                return _reportSpect ??= new RelayCommand(x =>
                {
                    // запрашиваем у пользователя путь для сохранения файла
                    string path = ShowFolderBrowserDialog();

                    if (path != "/")
                    {
                        SaveInWord save = new SaveInWord();

                        // определяем номер отчёта как количество файлов с именами, начинающимися на "отчёт" за сегодняшний день
                        string pathFile = Path.Combine(path, $"Отчёт №1. {DateTime.Today.ToShortDateString()}.docx");
                        var file = new FileInfo(Path.Combine(path, pathFile));

                        int numFiles = 1;
                        while (file.Exists)
                        {
                            pathFile = Path.Combine(path, $"Отчет №{numFiles}. {DateTime.Today.ToShortDateString()}.docx");
                            file = new FileInfo(Path.Combine(pathFile));
                            numFiles++;
                        }

                        var coordinates = $"L = {SpectCoorL}, a = {SpectCoorA}, b = {SpectCoorB}";
                        save.ExportSpect(pathFile, imgSpectr, coordinates, ChosenPolymerFromSpect.Name, ChosenColorantFromSpect.Name, Result);
                    }
                    else return;

                });
            }
        }

        private string ShowFolderBrowserDialog()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                return dialog.SelectedPath;
            }
            else
            {
                return "/";
            }
        }
        #endregion


    }
}
