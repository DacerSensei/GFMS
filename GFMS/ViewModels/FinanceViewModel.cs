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
            CurrentView = MainPageView;

            DashboardCommand = new Command(action =>
            {
                CurrentView = DashboardView;
            });
            OfficiallyEnrolledCommand = new Command(action =>
            {
                CurrentView = OfficiallyEnrolledView;
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

        private readonly MainPageViewModel MainPageView = new();
        private readonly FinanceDashboardViewModel DashboardView = new();
        private readonly FinanceOfficiallyEnrolledViewModel OfficiallyEnrolledView = new();

        public ICommand LogoutCommand { get; }

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
