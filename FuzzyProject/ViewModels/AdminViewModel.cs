using FuzzyProject.DB_EF;
using FuzzyProject.Models;
using FuzzyProject.WorkWithColors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WPF_MVVM_Classes;
using MessageBox = System.Windows.MessageBox;

namespace FuzzyProject.ViewModels
{
    internal class AdminViewModel : ViewModelBase
    {
        AppContextDB context;
        CalculateImg calculate;
        private readonly Window adminWindow;
        private string selectedFileName;
        private string imgPath = "c:\\";
        private Bitmap img;
        private byte[] imgMaterial;

        public AdminViewModel(Window _adminWindow)
        {
            SetList();
            SetReportsList(); //изменить на датагрид
            SetMaterailList();

            Roles = new List<string>();
            Roles.Add("Администратор");
            Roles.Add("Оператор");
            adminWindow = _adminWindow;
        }

        public void SetList()
        {
            context = new AppContextDB();
            context.Accounts.Load();
            Account = context.Accounts.ToList();
        }

        public void SetReportsList()
        {
            context = new AppContextDB();
            Reports = new List<Report>();
            ReportsList = new List<string>();
            var reports = context.Reports.OrderBy(x => x.Date).ToList();

            foreach (var report in reports)
            {
                var nowReport = new Report { Date = report.Date };
                Reports.Add(nowReport);
                ReportsList.Add(nowReport.Date);
            }
        }

        public void SetMaterailList()
        {
            context = new AppContextDB();
            context.Materials.Load();
            Material = context.Materials.ToList();
        }

        #region Materails

        private List<Material> _material;
        public List<Material> Material
        {
            get { return _material; }
            set
            {
                _material = value;
                OnPropertyChanged();
            }
        }

        private string _newNameMaterial;
        public string NewNameMaterial
        {
            get { return _newNameMaterial; }
            set
            {
                _newNameMaterial = value;
                OnPropertyChanged();
            }
        }

        private string _newColorMaterial;
        public string NewColorMaterial
        {
            get { return _newColorMaterial; }
            set
            {
                _newColorMaterial = value;
                OnPropertyChanged();
            }
        }

        private string _newParamsMaterial;
        public string NewParamsMaterial
        {
            get { return _newParamsMaterial; }
            set
            {
                _newParamsMaterial = value;
                OnPropertyChanged();
            }
        }

        private Material _selectedMaterial;
        public Material SelectedMaterial
        {
            get { return _selectedMaterial; }
            set
            {
                _selectedMaterial = value;
                OnPropertyChanged();

                if (_selectedMaterial != null)
                {
                    NewNameMaterial = _selectedMaterial.Name;
                    NewColorMaterial = _selectedMaterial.Color;
                    NewParamsMaterial = _selectedMaterial.Parameters;
                }
            }
        }

        private RelayCommand _removeMaterial;
        public RelayCommand RemoveMaterial
        {
            get
            {
                return _removeMaterial ??= new RelayCommand(x =>
                {
                    if (SelectedMaterial != null)
                    {
                        var material = context.Materials.Single(a => a.Id == SelectedMaterial.Id);
                        context.Materials.Remove(material);
                        context.SaveChanges();

                        SetMaterailList();
                    }
                });
            }
        }

        private RelayCommand _addMaterial;
        public RelayCommand AddMaterial
        {
            get
            {
                return _addMaterial ??= new RelayCommand(x =>
                {
                    if (SelectedReport != null)
                    {
                        if (NewNameMaterial != string.Empty && NewNameMaterial != "" && NewNameMaterial != null && NewColorMaterial != string.Empty && NewColorMaterial != "" && NewColorMaterial != null && NewParamsMaterial != string.Empty && NewParamsMaterial != "" && NewParamsMaterial != null)
                        {
                            var newMaterial = new Material { Name = NewNameMaterial, Color = NewColorMaterial, Parameters = NewParamsMaterial };
                            context.Materials.Add(newMaterial);
                            context.SaveChanges();

                            SetMaterailList();
                        }
                        else
                        {
                            MessageBox.Show("Заполните все поля, чтобы добавить новый материал", "Ошибка при добавлении материала");
                        }
                    }
                });
            }
        }

        private RelayCommand _updateMaterial;
        public RelayCommand UpdateMaterial
        {
            get
            {
                return _updateMaterial ??= new RelayCommand(x =>
                {
                    if (SelectedMaterial != null)
                    {
                        var newMaterial = context.Materials.Find(SelectedMaterial.Id);
                        newMaterial.Name = NewNameMaterial;
                        newMaterial.Color = NewColorMaterial;
                        newMaterial.Parameters = NewParamsMaterial;
                        newMaterial.Image = imgMaterial;
                        context.SaveChanges();

                        SetMaterailList();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали материал!", "Ошибка при обновлении записи");
                    }
                });
            }
        }

        public Material FromDBMaterial(string date)
        {
            var report = context.Reports.FirstOrDefault(r => r.Date == date);
            var material = context.Materials.FirstOrDefault(m => m.Id == report.MaterialId);
            return material;
        }

        #endregion

        #region Account
        private string _newLogin;
        public string NewLogin
        {
            get { return _newLogin; }
            set
            {
                _newLogin = value;
                OnPropertyChanged();
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }

        private string _newRole;
        public string NewRole
        {
            get { return _newRole; }
            set
            {
                _newRole = value;
                OnPropertyChanged();
            }
        }

        private List<string> _roles;
        public List<string> Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
                OnPropertyChanged();
            }
        }

        private List<Account> _account;
        public List<Account> Account
        {
            get { return _account; }
            set
            {
                _account = value;
                OnPropertyChanged();
            }
        }

        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;
                OnPropertyChanged();
                if (_selectedAccount != null)
                {
                    NewLogin = _selectedAccount.Login;
                    NewPassword = _selectedAccount.Password;
                    if (_selectedAccount.Role == "Администратор")
                        NewRole = Roles[0];
                    else
                        NewRole = Roles[1];
                }
            }
        }

        private RelayCommand _addAccount;
        public RelayCommand AddAccount
        {
            get
            {
                return _addAccount ??= new RelayCommand(x =>
                {
                    if (NewLogin != string.Empty && NewLogin != "" && NewLogin != null && NewPassword != string.Empty && NewPassword != "" && NewPassword != null && NewRole != string.Empty && NewRole != "" && NewRole != null)
                    {
                        Account newAccount = new Account { Login = NewLogin, Password = NewPassword, Role = NewRole };
                        context.Accounts.Add(newAccount);
                        context.SaveChanges();

                        SetList();
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля, чтобы добавить новую запись", "Ошибка при добавлении записи");
                    }
                });
            }
        }

        private RelayCommand _updateAccount;
        public RelayCommand UpdateAccount
        {
            get
            {
                return _updateAccount ??= new RelayCommand(x =>
                {
                    if (SelectedAccount != null)
                    {
                        var newAccount = context.Accounts.Find(SelectedAccount.Id);
                        newAccount.Login = NewLogin;
                        newAccount.Password = NewPassword;
                        newAccount.Role = NewRole;
                        context.SaveChanges();

                        SetList();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали пользователя!", "Ошибка при обновлении записи");
                    }
                });
            }
        }

        private RelayCommand _removeAccount;
        public RelayCommand RemoveAccount
        {
            get
            {
                return _removeAccount ??= new RelayCommand(x =>
                {
                    if (SelectedAccount != null)
                    {
                        if (SelectedAccount.Role == "Администратор")
                        {
                            if (Account.Count(a => a.Role == "Администратор") == 1)
                            {
                                MessageBox.Show("Вы не можете удалить последнего администратора!", "Ошибка при удалении администратора");
                            }
                        }
                        var account = context.Accounts.Single(a => a.Id == SelectedAccount.Id);
                        context.Accounts.Remove(account);
                        context.SaveChanges();

                        SetList();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали пользователя!", "Ошибка при удалении записи");
                    }
                });
            }
        }
        #endregion

        #region Reports
        public Report FromDBReport(string date)
        {
            var report = context.Reports.FirstOrDefault(r => r.Date == date);
            return report;
        }





        private string _loginReport;
        public string LoginReport
        {
            get { return _loginReport; }
            set
            {
                _loginReport = value;
                OnPropertyChanged();


            }
        }

        private string _newDateReport;
        public string NewDateReport
        {
            get { return _newDateReport; }
            set
            {
                _newDateReport = value;
                OnPropertyChanged();
            }
        }

        private string _newLoginReport;
        public string NewLoginReport
        {
            get { return _newLoginReport; }
            set
            {
                _newLoginReport = value;
                OnPropertyChanged();
            }
        }

        private string _newMaterialReport;
        public string NewMaterialReport
        {
            get { return _newMaterialReport; }
            set
            {
                _newMaterialReport = value;
                OnPropertyChanged();
            }
        }

        private string _newParamsReport;
        public string NewParamsReport
        {
            get { return _newParamsReport; }
            set
            {
                _newParamsReport = value;
                OnPropertyChanged();
            }
        }

        private string _newRecomReport;
        public string NewRecomReport
        {
            get { return _newRecomReport; }
            set
            {
                _newRecomReport = value;
                OnPropertyChanged();
            }
        }

        private List<string> _reportsList;
        public List<string> ReportsList
        {
            get { return _reportsList; }
            set
            {
                _reportsList = value;
                OnPropertyChanged();
            }
        }

        private List<Report> _reports;
        public List<Report> Reports
        {
            get { return _reports; }
            set
            {
                _reports = value;
                OnPropertyChanged();
            }
        }

        private string _selectedReport;
        public string SelectedReport
        {
            get { return _selectedReport; }
            set
            {
                _selectedReport = value;
                OnPropertyChanged();

                if (_selectedReport != null)
                {
                    var nowReport = Reports.FirstOrDefault(r => r.Date == _selectedReport);
                    var report = FromDBReport(nowReport.Date); //поиск по дате
                    var material = FromDBMaterial(nowReport.Date);
                    NewDateReport = nowReport.Date;
                    NewLoginReport = report.Login;
                    NewRecomReport = report.Reccomendation;
                    //из Материалов
                    NewParamsReport = material.Parameters;
                    NewMaterialReport = material.Name;
                }
            }
        }

        private RelayCommand _removeReport;
        public RelayCommand RemoveReport
        {
            get
            {
                return _removeReport ??= new RelayCommand(x =>
                {
                    if (SelectedReport != null)
                    {
                        var nowReport = Reports.FirstOrDefault(r => r.Date == _selectedReport);
                        var report = FromDBReport(nowReport.Date);
                        context.Reports.Remove(report);
                        context.SaveChanges();

                        Reports.Remove(Reports.Single(r => r.Date == nowReport.Date));
                        ReportsList.Remove(nowReport.Date);
                        SetReportsList();
                    }
                });
            }
        }
        #endregion




        private ImageShow? imageShow = null;
        private ImagePageViewModel imagePageViewModel;

        private RelayCommand _openImg;
        public RelayCommand OpenImg
        {
            get
            {
                return _openImg ??= new RelayCommand(x =>
                {
                    if (SelectedReport != null)
                    {
                        var nowReport = Reports.FirstOrDefault(r => r.Date == _selectedReport);
                        var material = FromDBMaterial(nowReport.Date);
                        var pic = material.Image;

                        calculate = new CalculateImg();
                        var newPic = calculate.FromBytesToBitmap(pic);

                        imageShow = new ImageShow();
                        imagePageViewModel = new ImagePageViewModel(imageShow, newPic);
                        imageShow.DataContext = imagePageViewModel;
                        imageShow.Show();
                    }
                });
            }
        }

        private RelayCommand _openMaterialImg;
        public RelayCommand OpenMaterialImg
        {
            get
            {
                return _openMaterialImg ??= new RelayCommand(x =>
                {
                    if (SelectedMaterial != null)
                    {
                        var material = context.Materials.FirstOrDefault(m => m.Id == SelectedMaterial.Id);
                        var pic = material.Image;

                        calculate = new CalculateImg();
                        var newPic = calculate.FromBytesToBitmap(pic);

                        imageShow = new ImageShow();
                        imagePageViewModel = new ImagePageViewModel(imageShow, newPic);
                        imageShow.DataContext = imagePageViewModel;
                        imageShow.Show();
                    }
                });
            }
        }

        private RelayCommand _loadMaterialImg;
        public RelayCommand LoadMaterialImg
        {
            get
            {
                return _loadMaterialImg ??= new RelayCommand(x =>
                {
                    if (SelectedMaterial != null)
                    {
                        calculate = new CalculateImg();
                        OpenFileDialog dlg = new OpenFileDialog();
                        dlg.InitialDirectory = imgPath;
                        dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
                        dlg.RestoreDirectory = true;
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            selectedFileName = dlg.FileName;
                            img = new Bitmap(selectedFileName);

                            //перевод изображения в биты для сохранения в БД
                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, ImageFormat.Bmp);
                                img.Save(ms, ImageFormat.Bmp);
                                imgMaterial = ms.ToArray();
                                MessageBox.Show("Изображение загружено!", "Успех");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали материал!", "Ошибка");
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
                    adminWindow.Close();
                });
            }
        }
    }
}
