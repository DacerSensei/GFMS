using GFMS.Commands;
using GFMS.Core;
using GFMS.ViewModels.AdminViewModels;
using GFMS.ViewModels.TeacherViewModels;
using GFMS.Views.Modals;
using GFMS.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace GFMS.ViewModels
{
    public class TeacherViewModel : ViewModelBase
    {
        public TeacherViewModel()
        {
            if (MainWindow.User != null)
            {
                UserType = MainWindow.User.Usertype == "TEACHER" ? MainWindow.Teacher.Grade + " Teacher" : MainWindow.User.Usertype;
                FullName = $"Hi, {MainWindow.User.FirstName}";
            }
            CurrentView = MainPageView;

            ReportCardCommand = new Command(action =>
            {
                CurrentView = ReportCardView;
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

        public ICommand ReportCardCommand { get; }

        private readonly MainPageViewModel MainPageView = new();
        private readonly TeacherReportCardViewModel ReportCardView = new();

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
