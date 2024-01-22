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
            RefreshCommand = new Command(async obj =>
            {
                await LoadAll();
            });

            PrintCommand = new Command(obj =>
            {
                UsersNotification? nt = obj as UsersNotification;
                string hashedString = (BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(nt.FullNamee))).Replace("-", "").ToLower()) + ".dtt";
                if (nt.Notification.Message.Contains("FORM 137")) {
                    hashedString = (BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(nt.FullNamee))).Replace("-", "").ToLower()) + ".f137";
                }
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "PDF document (*.pdf)|*.pdf";
                string mfiln = " Report Card ";
                if (nt.Notification.Message.Contains("FORM 137"))
                {
                    mfiln = " Form 137 ";
                }
                dialog.FileName = nt.FullName + mfiln + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".pdf";
                
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
        public ICommand RefreshCommand { get; }
    }
}
