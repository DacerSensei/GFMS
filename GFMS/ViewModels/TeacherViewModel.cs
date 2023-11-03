using GFMS.Commands;
using GFMS.Core;
using GFMS.ViewModels.AdminViewModels;
using GFMS.ViewModels.TeacherViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
