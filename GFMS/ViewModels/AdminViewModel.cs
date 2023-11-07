using GFMS.Commands;
using GFMS.Core;
using GFMS.ViewModels.AdminViewModels;
using GFMS.ViewModels.RegistrarViewModels;
using GFMS.Views;
using GFMS.Views.Modals;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GFMS.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        public AdminViewModel()
        {
            if (MainWindow.User != null)
            {
                UserType = MainWindow.User.Usertype;
                FullName = $"Hi, {MainWindow.User.FirstName}";
            }

            CurrentView = MainPageView;

            DashboardCommand = new Command(action =>
            {
                CurrentView = DashboardView;
            });
            UsersAccountCommand = new Command(action =>
            {
                CurrentView = UsersAccountView;
            });
            ChangePasswordCommand = new Command(action =>
            {
                CurrentView = ChangePasswordView;
            });
            CreateAccountCommand = new Command(action =>
            {
                CurrentView = CreateAccountView;
            });

            LogoutCommand = new Command(async action =>
            {
                var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want to logout?"), "RootDialog");
                if((bool)result! != true)
                {
                    return;
                }
                Login login = new Login();
                login.Show();
                Application.Current.MainWindow.Close();
                Application.Current.MainWindow = login;

            });
        }

        public ICommand DashboardCommand { get; }
        public ICommand UsersAccountCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        public ICommand CreateAccountCommand { get; }

        private readonly MainPageViewModel MainPageView = new();
        private readonly AdminDashboardViewModel DashboardView = new();
        private readonly AdminUsersAccountViewModel UsersAccountView = new();
        private readonly AdminChangePasswordViewModel ChangePasswordView = new();
        private readonly AdminCreateAccountViewModel CreateAccountView = new();

        public ICommand LogoutCommand { get; }

        private string? _fullName;

        public string? FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
        }

        private string? _userType;

        public string? UserType
        {
            get { return _userType; }
            set { _userType = value; OnPropertyChanged(nameof(UserType)); }
        }

        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
    }
}
