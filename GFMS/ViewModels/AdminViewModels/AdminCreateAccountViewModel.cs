using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views;
using GFMS.Views.Modals;
using GFMSLibrary;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace GFMS.ViewModels.AdminViewModels
{
    public class AdminCreateAccountViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private LoginCredentials Credentials = new LoginCredentials();
        private readonly ErrorsViewModel ErrorsViewModel;
        public AdminCreateAccountViewModel()
        {
            ErrorsViewModel = new ErrorsViewModel();
            ErrorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged!;

            ChangePasswordCommand = new Command(async obj =>
            {
                ValidateForm();
                if (ErrorsViewModel.HasErrors)
                {
                    return;
                }
                var email = await Credentials.GetByEmailAsync<Users>(Email, "users");
                if (email != null)
                {
                    await DialogHost.Show(new MessageDialog($"Oops!", $"Your email is already exist, try different email"), "RootDialog");
                    return;
                }
                var username = await Credentials.GetByUsernameAsync<Users>(Username, "users");
                if (username != null)
                {
                    await DialogHost.Show(new MessageDialog($"Oops!", $"Your username is already used, try different username"), "RootDialog");
                    return;
                }
                Users user = new Users()
                {
                    Username = Username,
                    Password = Password,
                    Email = Email,
                    FirstName = FirstName,
                    LastName = LastName,
                    Usertype = SelectedType,
                    Status = 1
                };
                await Credentials.RegisterStudentAsync(user, "users");
                if (IsTeacher == Visibility.Visible)
                {
                    UserTeacher teacher = new UserTeacher()
                    {
                        User_Id = Convert.ToInt32(Credentials.GetLastInsertedId()),
                        Grade = SelectedGrade
                    };
                    await Credentials.RegisterStudentAsync(teacher, "teachers");
                }
                var result = await DialogHost.Show(new MessageDialog($"Welcome, {user.FirstName}", $"Account {user.Usertype} has been created"), "RootDialog");
                ClearForm();
            });
        }

        private void ClearForm()
        {
            FirstName = "";
            LastName = "";
            Email = "";
            Username = "";
            Password = "";
            SelectedType = null;
            ErrorsViewModel.ClearErrors(nameof(SelectedType));
        }

        private void ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                ErrorsViewModel.AddError(nameof(FirstName), "First Name cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(FirstName));
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                ErrorsViewModel.AddError(nameof(LastName), "Last Name cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(LastName));
            }

            if (string.IsNullOrWhiteSpace(SelectedType))
            {
                ErrorsViewModel.AddError(nameof(SelectedType), "Account Type cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(SelectedType));
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorsViewModel.AddError(nameof(Email), "Email cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(Email));
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorsViewModel.AddError(nameof(Username), "Username cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(Username));
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorsViewModel.AddError(nameof(Password), "Password cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(Password));
            }

            if (IsTeacher == Visibility.Visible)
            {
                if (string.IsNullOrWhiteSpace(SelectedGrade))
                {
                    ErrorsViewModel.AddError(nameof(SelectedGrade), "Grade cannot be empty");
                }
                else
                {
                    ErrorsViewModel.ClearErrors(nameof(SelectedGrade));
                }
            }
        }

        public ICommand ChangePasswordCommand { get; }

        public ObservableCollection<string> TypeList { get; set; } = new ObservableCollection<string>()
        {
            "ADMIN","REGISTRAR","FINANCE","TEACHER","PRINCIPAL"
        };

        public ObservableCollection<string> GradeList { get; set; } = new ObservableCollection<string>()
        {
            "TOODLER",
            "NURSERY",
            "KINDER 1",
            "KINDER 2",
            "Grade 1",
            "Grade 2",
            "Grade 3",
            "Grade 4",
            "Grade 5",
            "Grade 6",
            "Grade 7",
            "Grade 8",
            "Grade 9",
            "Grade 10",
            "Grade 11 - ABM",
            "Grade 11 - HUMSS",
            "Grade 11 - STEM",
            "Grade 11 - GAS",
            "Grade 12 - ABM",
            "Grade 12 - HUMSS",
            "Grade 12 - STEM",
            "Grade 12 - GAS"
        };

        private void ChangeType(string type)
        {
            if (type == "TEACHER")
            {
                IsTeacher = Visibility.Visible;
            }
            else
            {
                IsTeacher = Visibility.Hidden;
                SelectedGrade = null;
                ErrorsViewModel.ClearErrors(nameof(SelectedGrade));
            }
        }

        private Visibility _isTeacher = Visibility.Hidden;

        public Visibility IsTeacher
        {
            get { return _isTeacher; }
            set
            {
                if (_isTeacher == value)
                {
                    return;
                }
                _isTeacher = value;
                OnPropertyChanged(nameof(IsTeacher));
            }
        }

        private string? _selectedGrade;

        public string? SelectedGrade
        {
            get { return _selectedGrade; }
            set
            {
                if (_selectedGrade == value)
                {
                    return;
                }
                _selectedGrade = value;

                OnPropertyChanged(nameof(SelectedGrade));
            }
        }

        private string? _selectedType;

        public string? SelectedType
        {
            get { return _selectedType; }
            set
            {
                if (_selectedType == value)
                {
                    return;
                }
                _selectedType = value;
                ChangeType(value);
                OnPropertyChanged(nameof(SelectedType));
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName == value)
                {
                    return;
                }
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value)
                {
                    return;
                }
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email == value)
                {
                    return;
                }
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username == value)
                {
                    return;
                }
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                {
                    return;
                }
                _password = value;
                OnPropertyChanged(nameof(Password));
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
    }
}
