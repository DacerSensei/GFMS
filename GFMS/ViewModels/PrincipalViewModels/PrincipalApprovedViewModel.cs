using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views.Modals;
using GFMSLibrary;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels.PrincipalViewModels
{
    public class PrincipalApprovedViewModel : ViewModelBase
    {
        public PrincipalApprovedViewModel()
        {
            LoadedCommand = new Command(async obj =>
            {
                await LoadAll();
            });

            ClearCommand = new Command(async obj =>
            {
                GradeLevelSelected = null;
                StatusSelected = null;
                SearchText = null;
                await LoadAll();
            });
            ApproveCommand = new Command(async obj =>
            {
                UsersNotification usersNotification = obj as UsersNotification;
                var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want approve?"), "RootDialog");
                if ((bool)result! != true)
                {
                    return;
                }
                await Credentials.UpdateStudentAsync<Notification, Where>(new Notification { Status = "Approved", Approved = DateTime.Now.ToShortDateString() }, new Where { id = usersNotification!.Notification!.Id }, "notification");
                await LoadAll();
            });
            RejectCommand = new Command(async obj =>
            {
                UsersNotification usersNotification = obj as UsersNotification;
                var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want reject?"), "RootDialog");
                if ((bool)result! != true)
                {
                    return;
                }
                await Credentials.UpdateStudentAsync<Notification, Where>(new Notification { Status = "Rejected" }, new Where { id = usersNotification!.Notification!.Id }, "notification");
                await LoadAll();
            });
        }

        private LoginCredentials Credentials = new LoginCredentials();

        private async Task LoadAll()
        {
            NotificationList.Clear();
            var userListTask = Credentials.GetAllDataAsync<Users>("users");
            var notificationListTask = Credentials.GetAllDataAsync<Notification>("notification");
            var studentListTask = Credentials.GetAllDataAsync<Student>("student");
            var registrationListTask = Credentials.GetAllDataAsync<Registration>("registration");

            await Task.WhenAll(userListTask, notificationListTask, studentListTask, registrationListTask);

            var userList = userListTask.Result;
            var notificationList = notificationListTask.Result;
            var studentList = studentListTask.Result;
            var registrationList = registrationListTask.Result;


            foreach (var notification in notificationList.Reverse<Notification>())
            {
                if (notification.Status!.ToLower() != "approved")
                {
                    continue;
                }
                var userNotification = new UsersNotification
                {
                    Notification = notification,
                    Student = studentList.Where(s => s.id == Convert.ToInt16(notification.Student_Id)).ToList().FirstOrDefault(),
                    User = userList.Where(u => u.Id == Convert.ToInt16(notification.User_Id)).ToList().FirstOrDefault(),
                };

                userNotification.Registration = registrationList.Where(r => Convert.ToInt16(r.Student_Id) == userNotification.Student.id).ToList().LastOrDefault();


                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    string searchText = SearchText.ToLower();
                    if (userNotification.Student.LastName.ToLower().Contains(searchText))
                    {
                        if (!string.IsNullOrEmpty(GradeLevelSelected))
                        {
                            if (userNotification.Registration.Grade == GradeLevelSelected)
                            {
                                NotificationList.Add(userNotification);
                            }
                        }
                        else
                        {
                            NotificationList.Add(userNotification);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(GradeLevelSelected))
                    {
                        if (userNotification.Registration.Grade == GradeLevelSelected)
                        {
                            NotificationList.Add(userNotification);
                        }
                    }
                    else
                    {
                        NotificationList.Add(userNotification);
                    }
                }
            }
        }

        public ObservableCollection<UsersNotification> NotificationList { get; set; } = new ObservableCollection<UsersNotification>();
        public ObservableCollection<string> GradeLevel { get; set; } = new ObservableCollection<string>
        {
            "TODDLER",
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
            "Grade 12 - HUMMS",
            "Grade 12 - STEM",
            "Grade 12 - GAS"
        };

        private string? gradeLevelSelected;
        public string? GradeLevelSelected
        {
            get => gradeLevelSelected;
            set => SetProperty(ref gradeLevelSelected, value);
        }

        private string? statusSelected;
        public string? StatusSelected
        {
            get => statusSelected;
            set => SetProperty(ref statusSelected, value);
        }

        private string? _searchText;

        public string? SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
            {
                return;
            }

            property = value;
            OnPropertyChanged(propertyName);
            if (value != null)
            {
                LoadAll();
            }

        }

        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand LoadedCommand { get; }

        private class Data
        {
            public string? status { get; set; }
        }

        private class Where
        {
            public int id { get; set; }
        }
    }
}
