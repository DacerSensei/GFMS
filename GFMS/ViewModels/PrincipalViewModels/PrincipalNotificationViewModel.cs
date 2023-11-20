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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels.PrincipalViewModels
{
    public class PrincipalNotificationViewModel :ViewModelBase
    {
        public PrincipalNotificationViewModel()
        {
            LoadAll();
            ApproveCommand = new Command(async obj =>
            {
                UsersNotification usersNotification = obj as UsersNotification;
                var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want approve?"), "RootDialog");
                if ((bool)result! != true)
                {
                    return;
                }
                await Credentials.UpdateStudentAsync<Notification, Where>(new Notification { Status = "Approved" }, new Where { id = usersNotification!.Notification!.Id }, "notification");
                LoadAll();
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
                LoadAll();
            });
        }

        private LoginCredentials Credentials = new LoginCredentials();

        private async void LoadAll()
        {
            NotificationList.Clear();
            var userList = await Credentials.GetAllDataAsync<Users>("users");
            var notifications = await Credentials.GetAllDataAsync<Notification>("notification");

            foreach (var notification in notifications)
            {
                if(notification.Status!.ToLower() != "pending")
                {
                    continue;
                }
                var userNotification = new UsersNotification
                {
                    Notification = notification,
                };
                userNotification.User = userList.Where(u => u.Id == Convert.ToInt16(notification.User_Id)).ToList().FirstOrDefault();
                NotificationList.Add(userNotification);
            }
        }

        public ObservableCollection<UsersNotification> NotificationList { get; set; } = new ObservableCollection<UsersNotification>();

        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

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
