using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views;
using GFMS.Views.Modals;
using GFMSLibrary;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace GFMS.ViewModels.AdminViewModels
{
    public class AdminChangePasswordViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private LoginCredentials Credentials = new LoginCredentials();
        private readonly ErrorsViewModel ErrorsViewModel;
        public AdminChangePasswordViewModel()
        {
            ErrorsViewModel = new ErrorsViewModel();
            ErrorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged!;
            ChangePasswordCommand = new Command(async obj =>
            {
                ValidateOldPassword();
                ValidateNewPassword();
                ValidateRetypePassword();
                if (ErrorsViewModel.HasErrors)
                {
                    return;
                }
                await Credentials.UpdateStudentAsync(new Request { password = NewPassword }, new { id = MainWindow.User!.Id }, "users");
                MainWindow.User.Password = NewPassword;
                var result = await DialogHost.Show(new MessageDialog("Notice!", "Your password has been successfully changed", true), "RootDialog");
                OldPassword = string.Empty;
                NewPassword = string.Empty;
                RetypePassword = string.Empty;
            });

            ChangeSchoolYearCommand = new Command(async obj =>
            {
                var result = await DialogHost.Show(new AlertDialog("Notice!", "Are you sure you want to move next school year?"), "RootDialog");
                if (!(bool)result!)
                {
                    return;
                }
                var YearList = await Credentials.GetAllDataAsync<SchoolYear>("school_year");
                var Years = YearList.OrderBy(y => y.Id).Select(y => y.Year).ToList();

                int LastYear = Convert.ToInt16((Years.LastOrDefault() ?? "2023-2024").Substring(5));
                string newYear = $"{LastYear}-{LastYear + 1}";

                await Credentials.RegisterStudentAsync(new SchoolYear { Year = newYear }, "school_year");
                await DialogHost.Show(new MessageDialog("Notice!", "School year been successfully changed", true), "RootDialog");
            });
        }

        public ICommand ChangePasswordCommand { get; }
        public ICommand ChangeSchoolYearCommand { get; }

        private void ValidateOldPassword()
        {
            if (string.IsNullOrWhiteSpace(OldPassword))
            {
                ErrorsViewModel.AddError(nameof(OldPassword), "Old password cannot be empty");
            }
            else if (OldPassword != MainWindow.User!.Password)
            {
                ErrorsViewModel.AddError(nameof(OldPassword), "Old password didn't match");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(OldPassword));
            }
        }

        private void ValidateNewPassword()
        {
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                ErrorsViewModel.AddError(nameof(NewPassword), "New password cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(NewPassword));
            }
        }

        private void ValidateRetypePassword()
        {
            if (string.IsNullOrWhiteSpace(RetypePassword))
            {
                ErrorsViewModel.AddError(nameof(RetypePassword), "Re-type password cannot be empty");
            }
            else if (NewPassword != RetypePassword)
            {
                ErrorsViewModel.AddError(nameof(RetypePassword), "New password and retype password didn't match");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(RetypePassword));
            }
        }

        private string _oldPassword;

        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                if (_oldPassword == value)
                {
                    return;
                }
                _oldPassword = value;
                OnPropertyChanged(nameof(OldPassword));
            }
        }

        private string _newPassword;

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                if (_newPassword == value)
                {
                    return;
                }
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        private string _retypePassword;

        public string RetypePassword
        {
            get { return _retypePassword; }
            set
            {
                if (_retypePassword == value)
                {
                    return;
                }
                _retypePassword = value;
                OnPropertyChanged(nameof(RetypePassword));
            }
        }

        public bool CanCreate => !HasErrors;

        public bool HasErrors => ErrorsViewModel.HasErrors;

        public IEnumerable GetErrors(string? propertyName)
        {
            return ErrorsViewModel.GetErrors(propertyName);
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }

        private class Request
        {
            public string? password { get; set; }
        }

    }
}
