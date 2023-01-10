using FuzzyProject.DB_EF;
using FuzzyProject.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF_MVVM_Classes;

namespace FuzzyProject.ViewModels
{
    internal class AuthorizationViewModel : ViewModelBase
    {
        private ApplicationContext context;
        private readonly Window authWindow;
        public AuthorizationViewModel(Window _authWindow)
        {
            context = new ApplicationContext();
            authWindow = _authWindow;
            BrushesLogin = System.Drawing.Color.Gray.Name.ToString();
            BrushesPassword = System.Drawing.Color.Gray.Name.ToString();
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

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged();
            }
        }

        private string _BrushesLogin;
        public string BrushesLogin
        {
            get { return _BrushesLogin; }
            set
            {
                _BrushesLogin = value;
                OnPropertyChanged();
            }
        }
        private string _BrushesPassword;
        public string BrushesPassword
        {
            get { return _BrushesPassword; }
            set
            {
                _BrushesPassword = value;
                OnPropertyChanged();
            }
        }

        private AdminPage? adminPage = null;
        private AdminViewModel adminViewModel;

        private ReseacherPage? reseacherPage = null;
        private ReseacherViewModel reseacherViewModel;

        private Registration? registration = null;
        private RegistrationViewModel registrationViewModel;

        private RelayCommand _OpenWindow;
        public RelayCommand OpenWindow
        {
            get
            {
                return _OpenWindow ??= new RelayCommand(x =>
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

                    var account = context.Accounts.ToList().FirstOrDefault(x => x.Login == Login && x.Password == Password);
                    if (account != null)
                    {
                        if (account.Role == "Оператор")
                        {
                            reseacherPage = new ReseacherPage();
                            reseacherViewModel = new ReseacherViewModel(reseacherPage, Login);
                            reseacherPage.DataContext = reseacherViewModel;
                            reseacherPage.Show();
                        }
                        else if (account.Role == "Администратор")
                        {
                            adminPage = new AdminPage();
                            adminViewModel = new AdminViewModel(adminPage);
                            adminPage.DataContext = adminViewModel;
                            adminPage.Show();
                        }
                        authWindow.Close();
                    }
                });
            }
        }

        private RelayCommand _OpenRegistration;
        public RelayCommand OpenRegistration
        {
            get
            {
                return _OpenRegistration ??= new RelayCommand(x =>
                {
                    registration = new Registration();
                    registrationViewModel = new RegistrationViewModel(registration);
                    registration.DataContext = registrationViewModel;
                    registration.Show();
                    authWindow.Close();
                });
            }
        }
    }
}
