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
