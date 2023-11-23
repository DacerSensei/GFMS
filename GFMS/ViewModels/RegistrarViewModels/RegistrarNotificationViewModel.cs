using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views;
using GFMSLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            PrintCommand = new Command(obj =>
            {

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
                if(isLoading == value)
                {
                    return;
                }
                isLoading = value;
            }
        }

        private async Task LoadAll()
        {
            NotificationList.Clear();
            var userList = await Credentials.GetAllDataAsync<Users>("users");
            var notifications = await Credentials.GetAllDataAsync<Notification>("notification");

            foreach (var notification in notifications.Where(n => Convert.ToInt16(n.User_Id) == MainWindow.User!.Id).ToList())
            {
                var userNotification = new UsersNotification
                {
                    Notification = notification,
                };
                userNotification.User = userList.Where(u => u.Id == Convert.ToInt16(notification.User_Id)).ToList().FirstOrDefault();
                NotificationList.Add(userNotification);
            }
        }

        public ObservableCollection<UsersNotification> NotificationList { get; set; } = new ObservableCollection<UsersNotification>();

        public ICommand PrintCommand { get; }
    }
}
