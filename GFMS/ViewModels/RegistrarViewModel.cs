using GFMS.Commands;
using GFMS.Core;
using GFMS.ViewModels.RegistrarViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels
{
    public class RegistrarViewModel : ViewModelBase
    {
        public RegistrarViewModel()
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
