﻿using GFMS.Commands;
using GFMS.Core;
using GFMS.ViewModels.AdminViewModels;
using GFMS.ViewModels.RegistrarViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        public AdminViewModel()
        {
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
