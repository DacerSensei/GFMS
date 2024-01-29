using Aspose.Words;
using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views;
using GFMS.Views.Modals;
using GFMSLibrary;
using MaterialDesignThemes.Wpf;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GFMS.ViewModels.RegistrarViewModels
{
    public class RegistrarNotificationViewModel : ViewModelBase
    {
        public RegistrarNotificationViewModel()
        {
            LoadedCommand = new Command(async obj =>
            {
                await LoadAll();
            });

            ClearCommand = new Command(async obj =>
            {
                GradeLevelSelected = null;
                SearchText = null;
                await LoadAll();
            });
            RefreshCommand = new Command(async obj =>
            {
                await LoadAll();
            });

            PrintCommand = new Command(obj =>
            {
                UsersNotification? notification = obj as UsersNotification;
                string hashedString = (BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(notification.StudentName))).Replace("-", "").ToLower()) + ".dtt";
                if (notification.Notification.Type == "FORM 137")
                {
                    hashedString = (BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(notification.StudentName))).Replace("-", "").ToLower()) + ".f137";
                }
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "PDF document (*.pdf)|*.pdf";
                string mfiln = " Report Card ";
                if (notification.Notification.Type == "FORM 137")
                {
                    mfiln = " Form 137 ";
                }
                dialog.FileName = notification.FullName + mfiln + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".pdf";

                var result = dialog.ShowDialog();
                string fileName = dialog.FileName;
                if (result == true)
                {
                    try
                    {
                        File.Copy(hashedString, fileName, true);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                    DialogHost.CloseDialogCommand.Execute(true, null);
                }

            });
        }

        public ICommand LoadedCommand { get; }

        private LoginCredentials Credentials = new LoginCredentials();

        private bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                if (isLoading == value)
                {
                    return;
                }
                isLoading = value;
            }
        }

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
                //if(notification.Status!.ToLower() != "pending")
                //{
                //    continue;
                //}
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

        public ICommand PrintCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ClearCommand { get; }
    }
}
