using GFMS.Commands;
using GFMS.Core;
using GFMS.ViewModels.FinanceViewModels;
using GFMS.ViewModels.PrincipalViewModels;
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
    public class PrincipalViewModel : ViewModelBase
    {
        public PrincipalViewModel()
        {
            if (MainWindow.User != null)
            {
                UserType = MainWindow.User.Usertype;
                FullName = $"Hi, {MainWindow.User.FirstName}";
            }
            
            CurrentView = MainPageView;

            RegistrarCommand = new Command(action =>
            {
                CurrentView = RegistrarView;
            });
            FinanceCommand = new Command(action =>
            {
                CurrentView = FinanceView;
            });
            TeacherCommand = new Command(action =>
            {
                CurrentView = TeacherView;
            });

            LogoutCommand = new Command(async action =>
            {
                var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want to logout?"), "RootDialog");
                if ((bool)result! != true)
                {
                    return;
                }
                Login login = new Login();
                login.Show();
                Application.Current.MainWindow.Close();
                Application.Current.MainWindow = login;

            });
        }

        public ICommand RegistrarCommand { get; }
        public ICommand FinanceCommand { get; }
        public ICommand TeacherCommand { get; }

        private readonly MainPageViewModel MainPageView = new();
        private readonly PrincipalRegistrarViewModel RegistrarView = new();
        private readonly PrincipalFinanceViewModel FinanceView = new();
        private readonly PrincipalTeacherViewModel TeacherView = new();

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
