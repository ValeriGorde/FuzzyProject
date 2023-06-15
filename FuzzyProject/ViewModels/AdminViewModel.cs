﻿using FuzzyProject.DB_EF;
using FuzzyProject.Models;
using FuzzyProject.Views;
using FuzzyProject.WorkWithColors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            SetData();

            //добавить отдельную модель для роли
            Roles = new List<string>();
            Roles.Add("Администратор");
            Roles.Add("Оператор");
            adminWindow = _adminWindow;
        }

        public void SetData()
        {
            context = new AppContextDB();
            context.Accounts.Load();
            context.PolymerTypes.Load();
            context.Materials.Load();
            context.Colorants.Load();
            context.Parameters.Load();
            Account = context.Accounts.ToList();
            PolymerType = context.PolymerTypes.ToList();
            Material = context.Materials.ToList();
            Colorant = context.Colorants.ToList();
            Parameter = context.Parameters.ToList();
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

        private bool _isReferenceCheck;
        public bool IsReferenceCheck
        {
            get { return _isReferenceCheck; }
            set
            {
                _isReferenceCheck = value;
                OnPropertyChanged();
            }
        }

        private PolymerType _chosenPolymerTypeMaterial;
        public PolymerType ChosenPolymerTypeMaterial
        {
            get { return _chosenPolymerTypeMaterial; }
            set
            {
                _chosenPolymerTypeMaterial = value;
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
                    ChosenPolymerTypeMaterial = _selectedMaterial.PolymerTypes;
                    ChosenColorantMaterial = _selectedMaterial.Colorants;
                    ChosenParamMaterial = _selectedMaterial.Parameters;
                    IsReferenceCheck = _selectedMaterial.IsReference;
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
                        var material = context.Materials.FirstOrDefault(a => a.Id == SelectedMaterial.Id);
                        if(material.IsReference == true) 
                        {
                            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эталонный материал?", "Удаление эталона", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.No)
                                return;
                        }
                        context.Materials.Remove(material);
                        context.SaveChanges();

                        SetData();
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
                    if (ChosenColorantMaterial != null && ChosenParamMaterial != null && ChosenPolymerTypeMaterial != null && imgMaterial != null)
                    {
                        var newMaterial = new Material { ColorantId = ChosenColorantMaterial.Id, PolymerTypeId = ChosenPolymerTypeMaterial.Id, ParametersId = ChosenParamMaterial.Id, Image = imgMaterial, IsReference = IsReferenceCheck};
                        context.Materials.Add(newMaterial);
                        context.SaveChanges();

                        SetData();
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля, чтобы добавить новый материал", "Ошибка при добавлении материала");
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
                        newMaterial.PolymerTypes = ChosenPolymerTypeMaterial;
                        newMaterial.IsReference = IsReferenceCheck;
                        newMaterial.Colorants = ChosenColorantMaterial;
                        newMaterial.Parameters = ChosenParamMaterial;
                        newMaterial.Image = imgMaterial;
                        context.SaveChanges();

                        SetData();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали материал!", "Ошибка при обновлении записи");
                    }
                });
            }
        }

        private ImageShow? imageShow = null;
        private ImagePageViewModel imagePageViewModel;

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

                        //просмотр изображения
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

                        SetData();
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

                        SetData();
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

                        SetData();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали пользователя!", "Ошибка при удалении записи");
                    }
                });
            }
        }
        #endregion

        //#region Reports
        //private DateTime _newDateReport = DateTime.Now.Date;
        //public DateTime NewDateReport
        //{
        //    get { return _newDateReport; }
        //    set
        //    {
        //        _newDateReport = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Material _chosenMaterialReport;
        //public Material ChosenMaterialReport
        //{
        //    get { return _chosenMaterialReport; }
        //    set
        //    {
        //        _chosenMaterialReport = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Parameter _chosenParamReport;
        //public Parameter ChosenParamReport
        //{
        //    get { return _chosenParamReport; }
        //    set
        //    {
        //        _chosenParamReport = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Colorant _chosenColorantReport;
        //public Colorant ChosenColorantReport
        //{
        //    get { return _chosenColorantReport; }
        //    set
        //    {
        //        _chosenColorantReport = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private string _newMessageReport;
        //public string NewMessageReport
        //{
        //    get { return _newMessageReport; }
        //    set
        //    {
        //        _newMessageReport = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private List<Report> _report;
        //public List<Report> Report
        //{
        //    get { return _report; }
        //    set
        //    {
        //        _report = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Report _selectedReport;
        //public Report SelectedReport
        //{
        //    get { return _selectedReport; }
        //    set
        //    {
        //        _selectedReport = value;
        //        OnPropertyChanged();

        //        if (_selectedReport != null)
        //        {
        //            NewDateReport = _selectedReport.Date;
        //            ChosenMaterialReport = _selectedReport.Material;
        //            ChosenColorantReport = _selectedReport.Colorants;
        //            ChosenParamReport = _selectedReport.Parameters;
        //        }
        //    }
        //}

        //private RelayCommand _removeReport;
        //public RelayCommand RemoveReport
        //{
        //    get
        //    {
        //        return _removeReport ??= new RelayCommand(x =>
        //        {
        //            if (SelectedReport != null)
        //            {
        //                var report = context.Reports.Single(r => r.Id == SelectedReport.Id);
        //                context.Reports.Remove(report);
        //                context.SaveChanges();

        //                SetData();
        //            }
        //            else
        //            {
        //                MessageBox.Show("Вы не выбрали строку для удаления!", "Ошибка при удалении записи");
        //            }

        //        });
        //    }
        //}

        //private ImageShow? imageShow = null;
        //private ImagePageViewModel imagePageViewModel;

        //private RelayCommand _openImg;
        //public RelayCommand OpenImg
        //{
        //    get
        //    {
        //        return _openImg ??= new RelayCommand(x =>
        //        {
        //            if (SelectedReport != null)
        //            {
        //                var nowReport = Report.FirstOrDefault(r => r.Id == SelectedReport.Id);
        //                var pic = nowReport.Material.Image;

        //                calculate = new CalculateImg();
        //                var newPic = calculate.FromBytesToBitmap(pic);

        //                imageShow = new ImageShow();
        //                imagePageViewModel = new ImagePageViewModel(imageShow, newPic);
        //                imageShow.DataContext = imagePageViewModel;
        //                imageShow.Show();
        //            }
        //        });
        //    }
        //}
        //#endregion

        #region PolymerTypes
        private string _newPolymerTypeName;
        public string NewPolymerTypeName
        {
            get { return _newPolymerTypeName; }
            set
            {
                _newPolymerTypeName = value;
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

        private PolymerType _selectedPolymerType;
        public PolymerType SelectedPolymerType
        {
            get { return _selectedPolymerType; }
            set
            {
                _selectedPolymerType = value;
                OnPropertyChanged();
                if (_selectedPolymerType != null)
                {
                    NewPolymerTypeName = _selectedPolymerType.Name;
                }
            }
        }

        private RelayCommand _addPolymerType;
        public RelayCommand AddPolymerType
        {
            get
            {
                return _addPolymerType ??= new RelayCommand(x =>
                {
                    if (NewPolymerTypeName != string.Empty && NewPolymerTypeName != "" && NewPolymerTypeName != null)
                    {
                        PolymerType newPolymer = new PolymerType { Name = NewPolymerTypeName };
                        context.PolymerTypes.Add(newPolymer);
                        context.SaveChanges();

                        SetData();
                    }
                    else
                    {
                        MessageBox.Show("Заполните поле наименования, чтобы добавить новый тип полимера", "Ошибка при добавлении нового типа полимера");
                    }
                });
            }
        }

        private RelayCommand _updatePolymerType;
        public RelayCommand UpdatePolymerType
        {
            get
            {
                return _updatePolymerType ??= new RelayCommand(x =>
                {
                    if (SelectedPolymerType != null)
                    {
                        var newPolymerType = context.PolymerTypes.Find(SelectedPolymerType.Id);
                        newPolymerType.Name = NewPolymerTypeName;
                        context.SaveChanges();

                        SetData();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали тип полимера!", "Ошибка при обновлении записи");
                    }
                });
            }
        }

        private RelayCommand _removePolymerType;
        public RelayCommand RemovePolymerType
        {
            get
            {
                return _removePolymerType ??= new RelayCommand(x =>
                {
                    if (SelectedPolymerType != null)
                    {
                        var polymer = context.PolymerTypes.Single(a => a.Id == SelectedPolymerType.Id);
                        context.PolymerTypes.Remove(polymer);
                        context.SaveChanges();

                        SetData();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали тип полимера!", "Ошибка при удалении записи");
                    }
                });
            }
        }

        private RelayCommand _clearPolymerType;
        public RelayCommand ClearPolymerType
        {
            get
            {
                return _clearPolymerType ??= new RelayCommand(x =>
                {
                    NewPolymerTypeName = "";
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

        private Colorant _selectedColorant;
        public Colorant SelectedColorant
        {
            get { return _selectedColorant; }
            set
            {
                _selectedColorant = value;
                OnPropertyChanged();
                if (_selectedColorant != null)
                {
                    NewColorantName = _selectedColorant.Name;
                }
            }
        }

        private RelayCommand _addColorant;
        public RelayCommand AddColorant
        {
            get
            {
                return _addColorant ??= new RelayCommand(x =>
                {
                    if (NewColorantName != string.Empty && NewColorantName != "" && NewColorantName != null)
                    {
                        Colorant newColorant = new Colorant { Name = NewColorantName };
                        context.Colorants.Add(newColorant);
                        context.SaveChanges();

                        SetData();
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

                        SetData();
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

                        SetData();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали краситель!", "Ошибка при удалении записи");
                    }
                });
            }
        }


        private RelayCommand _clearColorants;
        public RelayCommand ClearColorants
        {
            get
            {
                return _clearColorants ??= new RelayCommand(x =>
                {
                    NewColorantName = "";
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

                    SetData();
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

                        SetData();
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

                        SetData();
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали строку для удаления!", "Ошибка при удалении записи");
                    }
                });
            }
        }
        #endregion

        //#region References
        //private Colorant _chosenColorant;
        //public Colorant ChosenColorant
        //{
        //    get { return _chosenColorant; }
        //    set
        //    {
        //        _chosenColorant = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Parameter _chosenParam;
        //public Parameter ChosenParam
        //{
        //    get { return _chosenParam; }
        //    set
        //    {
        //        _chosenParam = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private List<ReferenceParam> _reference;
        //public List<ReferenceParam> Reference
        //{
        //    get { return _reference; }
        //    set
        //    {
        //        _reference = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private ReferenceParam _selectedReference;
        //public ReferenceParam SelectedReference
        //{
        //    get { return _selectedReference; }
        //    set
        //    {
        //        _selectedReference = value;
        //        OnPropertyChanged();

        //        if (_selectedReference != null)
        //        {
        //            ChosenColorant = _selectedReference.Colorant;
        //            ChosenParam = _selectedReference.Parameters;
        //        }
        //    }
        //}

        //private RelayCommand _addReference;
        //public RelayCommand AddReference
        //{
        //    get
        //    {
        //        return _addReference ??= new RelayCommand(x =>
        //        {
        //            ReferenceParam reference = new ReferenceParam
        //            {
        //                ColorantId = ChosenColorant.Id,
        //                ParametersId = ChosenParam.Id,
        //                Image = imgMaterial
        //            };

        //            context.ReferencesParams.Add(reference);
        //            context.SaveChanges();

        //            SetData();
        //        });
        //    }
        //}

        //private RelayCommand _updateReference;
        //public RelayCommand UpdateReference
        //{
        //    get
        //    {
        //        return _updateReference ??= new RelayCommand(x =>
        //        {
        //            if (SelectedReference != null)
        //            {
        //                var reference = context.ReferencesParams.Find(SelectedReference.Id);
        //                reference.Colorant = ChosenColorant;
        //                reference.Parameters = ChosenParam;
        //                reference.Image = imgMaterial;

        //                context.SaveChanges();

        //                SetData();
        //            }
        //            else
        //            {
        //                MessageBox.Show("Вы не выбрали строку для обновления!", "Ошибка при обновлении записи");
        //            }
        //        });
        //    }
        //}

        //private RelayCommand _removeReference;
        //public RelayCommand RemoveReference
        //{
        //    get
        //    {
        //        return _removeReference ??= new RelayCommand(x =>
        //        {
        //            if (SelectedReference != null)
        //            {
        //                var reference = context.ReferencesParams.Single(a => a.Id == SelectedReference.Id);
        //                context.ReferencesParams.Remove(reference);
        //                context.SaveChanges();

        //                SetData();
        //            }
        //            else
        //            {
        //                MessageBox.Show("Вы не выбрали строку для удаления!", "Ошибка при удалении записи");
        //            }
        //        });
        //    }
        //}

        //private RelayCommand _openReferenceImg;
        //public RelayCommand OpenReferenceImg
        //{
        //    get
        //    {
        //        return _openReferenceImg ??= new RelayCommand(x =>
        //        {
        //            if (SelectedReference != null)
        //            {
        //                var reference = context.ReferencesParams.FirstOrDefault(m => m.Id == SelectedReference.Id);
        //                var pic = reference.Image;

        //                calculate = new CalculateImg();
        //                var newPic = calculate.FromBytesToBitmap(pic);

        //                imageShow = new ImageShow();
        //                imagePageViewModel = new ImagePageViewModel(imageShow, newPic);
        //                imageShow.DataContext = imagePageViewModel;
        //                imageShow.Show();
        //            }
        //            else
        //            {
        //                MessageBox.Show("Вы не выбрали эталон!", "Ошибка");
        //            }
        //        });
        //    }
        //}

        //private RelayCommand _uploadReferenceImg;
        //public RelayCommand UploadReferenceImg
        //{
        //    get
        //    {
        //        return _uploadReferenceImg ??= new RelayCommand(x =>
        //        {
        //            calculate = new CalculateImg();
        //            OpenFileDialog dlg = new OpenFileDialog();

        //            dlg.InitialDirectory = imgPath;
        //            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
        //            dlg.RestoreDirectory = true;

        //            if (dlg.ShowDialog() == DialogResult.OK)
        //            {
        //                selectedFileName = dlg.FileName;
        //                img = new Bitmap(selectedFileName);

        //                imgMaterial = calculate.FromImgToBytes(img);
        //                MessageBox.Show("Изображение успешно загружено!");
        //            }
        //        });
        //    }
        //}
        //#endregion

        #region Buttons

        private RelayCommand _about;
        public RelayCommand About
        {
            get
            {
                return _about ??= new RelayCommand(x =>
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
