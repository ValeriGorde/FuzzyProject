using FuzzyProject.DB_EF;
using FuzzyProject.Models;
using FuzzyProject.Views;
using FuzzyProject.WorkWithColors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            SetColorantList();
            SetReferenceList();
            SetParameterList();

            //добавить отдельную модель для роли
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
            context.Reports.Load();
            Report = context.Reports.ToList();
        }

        public void SetMaterailList()
        {
            context = new AppContextDB();
            context.Materials.Load();
            Material = context.Materials.ToList();
        }

        public void SetColorantList()
        {
            context = new AppContextDB();
            context.Colorants.Load();
            Colorant = context.Colorants.ToList();
        }

        public void SetParameterList()
        {
            context = new AppContextDB();
            context.Parameters.Load();
            Parameter = context.Parameters.ToList();
        }

        public void SetReferenceList()
        {
            context = new AppContextDB();
            context.ReferencesParams.Load();
            Reference = context.ReferencesParams.ToList();
        }

        #region Materials

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

        private Colorant _chosenColorantMaterial;
        public Colorant ChosenColorantMaterial
        {
            get { return _chosenColorantMaterial; }
            set
            {
                _chosenColorantMaterial = value;
                OnPropertyChanged();
            }
        }

        private Parameter _chosenParamMaterial;
        public Parameter ChosenParamMaterial
        {
            get { return _chosenParamMaterial; }
            set
            {
                _chosenParamMaterial = value;
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
                    ChosenColorantMaterial = _selectedMaterial.Colorants;
                    ChosenParamMaterial = _selectedMaterial.Parameters;
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

                        var report = context.Reports.Where(r => r.MaterialId == SelectedMaterial.Id).ToList();
                        if (report.Count == 0)
                        {
                            context.Materials.Remove(material);
                            context.SaveChanges();

                            SetMaterailList();
                        }
                        else
                        {
                            MessageBox.Show("Вы не можете удалить материал, так как он имеет ссылку в отчете. Для начала удалите отчёт!");
                        }

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
                    if (SelectedMaterial != null)
                    {
                        if (NewNameMaterial != string.Empty && NewNameMaterial != "" && NewNameMaterial != null && ChosenColorantMaterial != null &&
                        ChosenParamMaterial != null)
                        {
                            var newMaterial = new Material { Name = NewNameMaterial, ColorantId = ChosenColorantMaterial.Id, ParametersId = ChosenParamMaterial.Id };
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
                        newMaterial.Colorants = ChosenColorantMaterial;
                        newMaterial.Parameters = ChosenParamMaterial;
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

                            imgMaterial = calculate.FromImgToBytes(img);
                            MessageBox.Show("Изображение успешно загружено!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали материал!", "Ошибка");
                    }
                });
            }
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
        private DateTime _newDateReport;
        public DateTime NewDateReport
        {
            get { return _newDateReport; }
            set
            {
                _newDateReport = value;
                OnPropertyChanged();
            }
        }

        private Material _chosenMaterialReport;
        public Material ChosenMaterialReport
        {
            get { return _chosenMaterialReport; }
            set
            {
                _chosenMaterialReport = value;
                OnPropertyChanged();
            }
        }

        private Parameter _chosenParamReport;
        public Parameter ChosenParamReport
        {
            get { return _chosenParamReport; }
            set
            {
                _chosenParamReport = value;
                OnPropertyChanged();
            }
        }

        private Colorant _chosenColorantReport;
        public Colorant ChosenColorantReport
        {
            get { return _chosenColorantReport; }
            set
            {
                _chosenColorantReport = value;
                OnPropertyChanged();
            }
        }

        private string _newMessageReport;
        public string NewMessageReport
        {
            get { return _newMessageReport; }
            set
            {
                _newMessageReport = value;
                OnPropertyChanged();
            }
        }

        private List<Report> _report;
        public List<Report> Report
        {
            get { return _report; }
            set
            {
                _report = value;
                OnPropertyChanged();
            }
        }

        private Report _selectedReport;
        public Report SelectedReport
        {
            get { return _selectedReport; }
            set
            {
                _selectedReport = value;
                OnPropertyChanged();

                if (_selectedReport != null)
                {
                    NewDateReport = _selectedReport.Date;
                    ChosenMaterialReport = _selectedReport.Material;
                    ChosenColorantReport = _selectedReport.Colorants;
                    ChosenParamReport = _selectedReport.Parameters;
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
                        var report = context.Reports.Single(r => r.Id == SelectedReport.Id);
                        context.Reports.Remove(report);
                        context.SaveChanges();

                        SetReportsList();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку для удаления!", "Ошибка при удалении записи");
                    }

                });
            }
        }

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
                        var nowReport = Report.FirstOrDefault(r => r.Id == SelectedReport.Id);
                        var pic = nowReport.Material.Image;

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
        #endregion

        #region Colorants
        private string _newColorantName;
        public string NewColorantName
        {
            get { return _newColorantName; }
            set
            {
                _newColorantName = value;
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

        private Account _selectedColorant;
        public Account SelectedColorant
        {
            get { return _selectedColorant; }
            set
            {
                _selectedColorant = value;
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

        private RelayCommand _addColorant;
        public RelayCommand AddColorant
        {
            get
            {
                return _addAccount ??= new RelayCommand(x =>
                {
                    if (NewColorantName != string.Empty && NewColorantName != "" && NewColorantName != null)
                    {
                        Colorant newColorant = new Colorant { Name = NewColorantName };
                        context.Colorants.Add(newColorant);
                        context.SaveChanges();

                        SetColorantList();
                    }
                    else
                    {
                        MessageBox.Show("Заполните поле наименования, чтобы добавить новый краситель", "Ошибка при добавлении красителя");
                    }
                });
            }
        }

        private RelayCommand _updateColorant;
        public RelayCommand UpdateColorant
        {
            get
            {
                return _updateColorant ??= new RelayCommand(x =>
                {
                    if (SelectedColorant != null)
                    {
                        var newColorant = context.Colorants.Find(SelectedColorant.Id);
                        newColorant.Name = NewColorantName;
                        context.SaveChanges();

                        SetColorantList();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали краситель!", "Ошибка при обновлении записи");
                    }
                });
            }
        }

        private RelayCommand _removeColorant;
        public RelayCommand RemoveColorant
        {
            get
            {
                return _removeColorant ??= new RelayCommand(x =>
                {
                    if (SelectedColorant != null)
                    {
                        var colorant = context.Colorants.Single(a => a.Id == SelectedColorant.Id);
                        context.Colorants.Remove(colorant);
                        context.SaveChanges();

                        SetColorantList();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали краситель!", "Ошибка при удалении записи");
                    }
                });
            }
        }
        #endregion

        #region Parameters
        private double _newParamL;
        public double NewParamL
        {
            get { return _newParamL; }
            set
            {
                _newParamL = value;
                OnPropertyChanged();
            }
        }

        private double _newParamA;
        public double NewParamA
        {
            get { return _newParamA; }
            set
            {
                _newParamA = value;
                OnPropertyChanged();
            }
        }


        private double _newParamB;
        public double NewParamB
        {
            get { return _newParamB; }
            set
            {
                _newParamB = value;
                OnPropertyChanged();
            }
        }

        private List<Parameter> _parameter;
        public List<Parameter> Parameter
        {
            get { return _parameter; }
            set
            {
                _parameter = value;
                OnPropertyChanged();
            }
        }

        private Parameter _selectedParameter;
        public Parameter SelectedParameter
        {
            get { return _selectedParameter; }
            set
            {
                _selectedParameter = value;
                OnPropertyChanged();

                if (_selectedParameter != null)
                {
                    NewParamL = _selectedParameter._L;
                    NewParamA = _selectedParameter._A;
                    NewParamB = _selectedParameter._B;
                }
            }
        }

        private RelayCommand _addParameter;
        public RelayCommand AddParameter
        {
            get
            {
                return _addParameter ??= new RelayCommand(x =>
                {
                    Parameter newParameter = new Parameter { _L = NewParamL, _A = NewParamA, _B = NewParamB };
                    context.Parameters.Add(newParameter);
                    context.SaveChanges();

                    SetParameterList();
                });
            }
        }

        private RelayCommand _updateParameter;
        public RelayCommand UpdateParameter
        {
            get
            {
                return _updateParameter ??= new RelayCommand(x =>
                {
                    if (SelectedParameter != null)
                    {
                        var newParameter = context.Parameters.Find(SelectedParameter.Id);
                        newParameter._L = NewParamL;
                        newParameter._A = NewParamA;
                        newParameter._B = NewParamB;
                        context.SaveChanges();

                        SetParameterList();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку для обновления!", "Ошибка при обновлении записи");
                    }
                });
            }
        }

        private RelayCommand _removeParameter;
        public RelayCommand RemoveParameter
        {
            get
            {
                return _removeParameter ??= new RelayCommand(x =>
                {
                    if (SelectedParameter != null)
                    {
                        var param = context.Parameters.Single(a => a.Id == SelectedParameter.Id);
                        context.Parameters.Remove(param);
                        context.SaveChanges();

                        SetParameterList();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку для удаления!", "Ошибка при удалении записи");
                    }
                });
            }
        }
        #endregion

        #region References
        private Colorant _chosenColorant;
        public Colorant ChosenColorant
        {
            get { return _chosenColorant; }
            set
            {
                _chosenColorant = value;
                OnPropertyChanged();
            }
        }

        private Parameter _chosenParam;
        public Parameter ChosenParam
        {
            get { return _chosenParam; }
            set
            {
                _chosenParam = value;
                OnPropertyChanged();
            }
        }

        private List<ReferenceParam> _reference;
        public List<ReferenceParam> Reference
        {
            get { return _reference; }
            set
            {
                _reference = value;
                OnPropertyChanged();
            }
        }

        private ReferenceParam _selectedReference;
        public ReferenceParam SelectedReference
        {
            get { return _selectedReference; }
            set
            {
                _selectedReference = value;
                OnPropertyChanged();

                if (_selectedReference != null)
                {
                    ChosenColorant = _selectedReference.Colorant;
                    ChosenParam = _selectedReference.Parameters;
                }
            }
        }

        private RelayCommand _addReference;
        public RelayCommand AddReference
        {
            get
            {
                return _addReference ??= new RelayCommand(x =>
                {
                    ReferenceParam reference = new ReferenceParam
                    {
                        ColorantId = ChosenColorant.Id,
                        ParametersId = ChosenParam.Id
                    }; //как сделать чтобы каждым параметр по отдельности сохранялся и показывался

                    context.ReferencesParams.Add(reference);
                    context.SaveChanges();

                    SetReferenceList();
                });
            }
        }

        private RelayCommand _updateReference;
        public RelayCommand UpdateReference
        {
            get
            {
                return _updateReference ??= new RelayCommand(x =>
                {
                    if (SelectedReference != null)
                    {
                        var reference = context.ReferencesParams.Find(SelectedReference.Id);
                        reference.Colorant = ChosenColorant;
                        reference.Parameters._L = ChosenParam._L;
                        reference.Parameters._A = ChosenParam._A;
                        reference.Parameters._B = ChosenParam._B;

                        context.SaveChanges();

                        SetReferenceList();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку для обновления!", "Ошибка при обновлении записи");
                    }
                });
            }
        }

        private RelayCommand _removeReference;
        public RelayCommand RemoveReference
        {
            get
            {
                return _removeReference ??= new RelayCommand(x =>
                {
                    if (SelectedReference != null)
                    {
                        var reference = context.ReferencesParams.Single(a => a.Id == SelectedReference.Id);
                        context.ReferencesParams.Remove(reference);
                        context.SaveChanges();

                        SetReferenceList();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку для удаления!", "Ошибка при удалении записи");
                    }
                });
            }
        }

        private RelayCommand _openReferenceImg;
        public RelayCommand OpenReferenceImg
        {
            get
            {
                return _openReferenceImg ??= new RelayCommand(x =>
                {
                    if (SelectedReference != null)
                    {
                        var reference = context.ReferencesParams.FirstOrDefault(m => m.Id == SelectedReference.Id);
                        var pic = reference.Image;

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

        private RelayCommand _uploadReferenceImg;
        public RelayCommand UploadReferenceImg
        {
            get
            {
                return _uploadReferenceImg ??= new RelayCommand(x =>
                {
                    if (SelectedReference != null)
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

                            imgMaterial = calculate.FromImgToBytes(img);
                            MessageBox.Show("Изображение успешно загружено!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали эталон!", "Ошибка");
                    }
                });
            }
        }
        #endregion

        #region Buttons

        private RelayCommand _about;
        public RelayCommand About
        {
            get
            {
                return _about ??= new RelayCommand(x =>
                {
                    MessageBox.Show("Данный программный комплекс был выполнен\n" +
                        "студенткой 494 группы - Гордеевой Валерией Александровной");
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

        #endregion
    }
}
