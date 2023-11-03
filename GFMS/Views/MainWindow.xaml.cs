using GFMS.Commands;
using GFMS.Core;
using GFMS.Enum;
using GFMS.Models;
using GFMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GFMS.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static Users? User;
        public static UserTeacher? Teacher;
        public MainWindow(AccountType accountType, Users user, UserTeacher? teacher)
        {
            InitializeComponent();
            if(teacher != null)
            {
                Teacher = teacher;
            }
            User = user;
            switch (accountType)
            {
                case AccountType.ADMIN:
                    {
                        CurrentView = new AdminViewModel();
                        break;
                    }
                case AccountType.FINANCE:
                    {
                        CurrentView = new FinanceViewModel();
                        break;
                    }
                case AccountType.PRINCIPAL:
                    {
                        CurrentView = new PrincipalViewModel();
                        break;
                    }
                case AccountType.REGISTRAR:
                    {
                        CurrentView = new RegistrarViewModel();
                        break;
                    }
                case AccountType.TEACHER:
                    {
                        CurrentView = new TeacherViewModel();
                        break;
                    }
            }
            DataContext = this;
        }

        private Command _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new Command(CloseCommandExecute);
                }
                return _closeCommand;
            }
        }
        private Command _maximizeCommand;
        public ICommand MaximizeCommand
        {
            get
            {
                if (_maximizeCommand == null)
                {
                    _maximizeCommand = new Command(MaximizeCommandExecute);
                }
                return _maximizeCommand;
            }
        }
        private Command _minimizeCommand;
        public ICommand MinimizeCommand
        {
            get
            {
                if (_minimizeCommand == null)
                {
                    _minimizeCommand = new Command(MinimizeCommandExecute);
                }
                return _minimizeCommand;
            }
        }

        private void CloseCommandExecute(object sender)
        {
            Close();
        }
        private void MaximizeCommandExecute(object sender)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }
        private void MinimizeCommandExecute(object sender)
        {
            WindowState = WindowState.Minimized;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView == value)
                {
                    return;
                }
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
    }
}
