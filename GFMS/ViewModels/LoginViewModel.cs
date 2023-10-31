﻿using GFMS.Commands;
using GFMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using GFMSLibrary;
using GFMS.Models;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections;
using System.Runtime.CompilerServices;
using GFMS.Views;
using System.Threading.Channels;
using GFMS.Enum;

namespace GFMS.ViewModels
{
    public class LoginViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel ErrorsViewModel;
        public LoginViewModel()
        {
            ErrorsViewModel = new ErrorsViewModel();
            ErrorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged!;
        }

        private Command? _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new Command(async obj =>
                    {
                        ValidateUsername();
                        ValidatePassword();
                        if (ErrorsViewModel.HasErrors)
                        {
                            return;
                        }
                        LoginCredentials credentials = new LoginCredentials();
                        Users user = await credentials.Login<Users>(Username!, Password!, "users");
                        if (user != null)
                        {
                            AccountType type;
                            if (user.Usertype!.ToUpper() == AccountType.ADMIN.ToString())
                            {
                                type = AccountType.ADMIN;
                            }else if(user.Usertype!.ToUpper() == AccountType.FINANCE.ToString())
                            {
                                type = AccountType.FINANCE;
                            }else if(user.Usertype!.ToUpper() == AccountType.PRINCIPAL.ToString())
                            {
                                type = AccountType.PRINCIPAL;
                            }else if(user.Usertype!.ToUpper() == AccountType.REGISTRAR.ToString())
                            {
                                type = AccountType.REGISTRAR;
                            }else if(user.Usertype!.ToUpper() == AccountType.TEACHER.ToString())
                            {
                                type = AccountType.TEACHER;
                            }else
                            {
                                return;
                            }
                            MainWindow window = new MainWindow(type);
                            window.Show();
                            Application.Current.MainWindow.Close();
                        }
                        else
                        {
                            Debug.WriteLine("Wrong Password");
                        }
                    });
                }
                return _loginCommand;
            }
        }

        private Command? _registerCommand;
        public ICommand RegisterCommand
        {
            get
            {
                if (_registerCommand == null)
                {
                    _registerCommand = new Command(obj =>
                    {

                    });
                }
                return _registerCommand;
            }
        }

        public bool CanCreate => !HasErrors;

        public bool HasErrors => ErrorsViewModel.HasErrors;

        private string? _password;
        private string? _username;
        public string? Username
        {
            get => _username;
            set
            {
                if (_username == value)
                {
                    return;
                }
                _username = value;
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    ErrorsViewModel.ClearErrors(nameof(Username));
                }
                OnPropertyChanged(nameof(Username));
            }
        }
        public string? Password
        {
            get => _password;
            set
            {
                if (_password == value)
                {
                    return;
                }
                _password = value;
                if(!string.IsNullOrWhiteSpace(Password))
                {
                    ErrorsViewModel.ClearErrors(nameof(Password));
                }
                OnPropertyChanged(nameof(Password));

            }
        }
        public IEnumerable GetErrors(string? propertyName)
        {
            return ErrorsViewModel.GetErrors(propertyName);
        }

        private void ValidateUsername()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorsViewModel.AddError(nameof(Username), "Username cannot be empty.");
            }
            else
            {   
                ErrorsViewModel.ClearErrors(nameof(Username));
            }

        }

        private void ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorsViewModel.AddError(nameof(Password), "Password cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(Password));
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }
    }
}