using FuzzyProject.DB_EF;
using FuzzyProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_MVVM_Classes;

namespace FuzzyProject.ViewModels
{
    internal class AdminViewModel : ViewModelBase
    {
        ApplicationContext context;
        private readonly Window adminWindow;
        public AdminViewModel(Window _adminWindow)
        {
            SetList();

            context = new ApplicationContext();
            Reports = new List<string>();
            var reports = context.Reports.OrderBy(x => x.Date).ToList();

            foreach (var report in reports)
            {
                Reports.Add(report.Date);
            }

            Roles = new List<string>();
            Roles.Add("Администратор");
            Roles.Add("Оператор");
            adminWindow = _adminWindow;
        }

        public void SetList()
        {
            context = new ApplicationContext();
            context.Accounts.Load();
            Account = context.Accounts.ToList();
        }


        #region Пользователь
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
        #endregion


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


        private List<string> _reports;
        public List<string> Reports
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
                    var report = FromDBReport(_selectedReport);
                    NewDateReport = _selectedReport;
                    NewLoginReport = report.Login;
                    NewRecomReport = report.Reccomendation;

                    //Взять из Материалов
                    //NewParamsReport = report.Material;
                    //NewMaterialReport = report.Material;
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
                        if(SelectedAccount.Role == "Администратор")
                        {
                            if (Account.Count(a => a.Role == "Администратор") == 1)
                            {
                                MessageBox.Show("Вы не можете удалить последнего администратора!", "Ошибка при удалении администратора");
                            }
                        }
                        else
                        {
                            var account = context.Accounts.Single(a => a.Id == SelectedAccount.Id);
                            context.Accounts.Remove(account);
                            context.SaveChanges();

                            SetList();                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы не выбрали пользователя!", "Ошибка при удалении записи");
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
