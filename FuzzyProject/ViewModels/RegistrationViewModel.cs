using FuzzyProject.DB_EF;
using FuzzyProject.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF_MVVM_Classes;


namespace FuzzyProject.ViewModels
{
    internal class RegistrationViewModel : ViewModelBase
    {
        AppContextDB context;
        private readonly Window registrWindow;
        public RegistrationViewModel(Window _registrWindow)
        {
            registrWindow = _registrWindow;
            context = new AppContextDB();
        }


        private string _Login;
        public string Login
        {
            get { return _Login; }
            set
            {
                _Login = value;
                OnPropertyChanged();
            }
        }

        private string _Password; //с маленькой
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged();
            }
        }

        private string _role;
        public string Role
        {
            get { return _role; }
            set
            {
                _role = value;
                OnPropertyChanged();
            }
        }


        private Authorization? authorization = null;
        private AuthorizationViewModel authorizationViewModel;

        private RelayCommand _OpenAuthorization;
        public RelayCommand OpenAuthorization
        {
            get
            {
                return _OpenAuthorization ??= new RelayCommand(x =>
                {
                    if (string.IsNullOrWhiteSpace(Login))
                    {
                        MessageBox.Show("Вы не ввели логин");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(Password))
                    {
                        MessageBox.Show("Вы не ввели пароль");
                        return;
                    }

                    //добавить проверки на длину логина и пароля

                    Account newAccount = new Account { Login = Login, Password = Password, Role = "Оператор" };
                    context.Accounts.Add(newAccount);
                    context.SaveChanges();

                    MessageBox.Show("Регистрация прошла успешно!");
                    authorization = new Authorization();
                    authorizationViewModel = new AuthorizationViewModel(authorization);
                    authorization.DataContext = authorizationViewModel;
                    authorization.Show();

                    registrWindow.Close();

                });
            }
        }
    }
}
