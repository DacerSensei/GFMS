using GFMS.Commands;
using GFMS.Core;
using GFMS.ViewModels.FinanceViewModels;
using GFMS.Views;
using GFMS.Views.Modals;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Input;

namespace GFMS.ViewModels
{
    public class FinanceViewModel : ViewModelBase
    {
        public FinanceViewModel()
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
            ReportCommand = new Command(action =>
            {
                CurrentView = ReportView;
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
        public ICommand ReportCommand { get; }

        private readonly MainPageViewModel MainPageView = new();
        private readonly FinanceDashboardViewModel DashboardView = new();
        private readonly FinanceOfficiallyEnrolledViewModel OfficiallyEnrolledView = new();
        private readonly FinanceReportViewModel ReportView = new();

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
