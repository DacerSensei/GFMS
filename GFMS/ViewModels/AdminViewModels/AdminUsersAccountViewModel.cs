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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels.AdminViewModels
{
    public class AdminUsersAccountViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public AdminUsersAccountViewModel()
        {
            LoadAll();
            BanCommand = new Command(async obj =>
            {
                UsersDisplay? user = obj as UsersDisplay;
                if (user != null)
                {
                    if (user.User == null)
                    {
                        return;
                    }
                    if (user.User.Status == 1)
                    {
                        var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want to deactivate this account?"), "RootDialog");
                        if ((bool)result! == false)
                        {
                            return;
                        }
                        await Credentials.UpdateStudentAsync(new Request { status = 0 }, new { id = user.User!.Id }, "users");
                    }else
                    {
                        var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want to activate this account?"), "RootDialog");
                        if ((bool)result! == false)
                        {
                            return;
                        }
                        await Credentials.UpdateStudentAsync(new Request { status = 1 }, new { id = user.User!.Id }, "users");
                    }
                    LoadAll();
                }

            });
        }

        private async void LoadAll()
        {
            UserList.Clear();
            var userList = await Credentials.GetAllDataAsync<Users>("users");

            foreach (var user in userList)
            {
                // Find the requirements associated with the current student using the Student_ID
                var userDisplay = new UsersDisplay
                {
                    User = user,
                };
                UserList.Add(userDisplay);
            }
        }

        public ObservableCollection<UsersDisplay> UserList { get; set; } = new ObservableCollection<UsersDisplay>();

        public ICommand BanCommand { get; }

        private class Request
        {
            public int status { get; set; }
        }

    }
}
