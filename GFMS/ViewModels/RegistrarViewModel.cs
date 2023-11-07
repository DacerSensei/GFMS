using GFMS.Commands;
using GFMS.Core;
using GFMS.ViewModels.RegistrarViewModels;
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
    public class RegistrarViewModel : ViewModelBase
    {
        public RegistrarViewModel()
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
            OfficiallyEnrolledCommand = new Command(action =>
            {
                CurrentView = OfficiallyEnrolledView;
            });
            RegisterStudentCommand = new Command(action =>
            {
                CurrentView = RegisterStudentView;
            });
            ReportCardCommand = new Command(action =>
            {
                CurrentView = ReportCardView;
            });
            RequirementsCommand = new Command(action =>
            {
                CurrentView = RequirementsView;
            });
            StudentRecordsCommand = new Command(action =>
            {
                CurrentView = StudentRecordsView;
            });
            MainPageCommand = new Command(action =>
            {
                CurrentView = MainPageView;
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

        public ICommand DashboardCommand { get; }
        public ICommand OfficiallyEnrolledCommand { get; }
        public ICommand RegisterStudentCommand { get; }
        public ICommand ReportCardCommand { get; }
        public ICommand RequirementsCommand { get; }
        public ICommand StudentRecordsCommand { get; }
        public ICommand MainPageCommand { get; }

        private RegistrarDashboardViewModel DashboardView = new();
        private RegistrarOfficiallyEnrolledViewModel OfficiallyEnrolledView = new();
        private RegistrarRegisterStudentViewModel RegisterStudentView = new();
        private RegistrarReportCardViewModel ReportCardView = new();
        private RegistrarRequirementsViewModel RequirementsView = new();
        private RegistrarStudentRecordsViewModel StudentRecordsView = new();
        private MainPageViewModel MainPageView = new();

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
