﻿using FuzzyProject.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FuzzyProject.Views
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
            this.DataContext = new AuthorizationViewModel(this);
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //это позволяет передать пароль во viewModel
            ((dynamic)DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}
