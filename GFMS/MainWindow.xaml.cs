using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GFMS.DataObjects;
using GFMSLibrary;

namespace GFMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoginCredentials loginCredentials = new LoginCredentials();
            Users users = new Users() { FirstName = "Dave", LastName = "Orijuela", Username = "MyUsername", Password = "MyPassword", Status = 1, Usertype = "admin", Id = 30 };
            Users usersUpdate = new Users() { Password = "2", FirstName = "D", LastName = "O" };
            Test test = new Test();
            //test.UpdateTest(usersUpdate, new { id = "10" }, "users");

            //loginCredentials.Register(users, "users");
            var list = loginCredentials.Login<Users>("MyUsername", "MyPassword", "users");
            foreach (var item in list)
            {
                Debug.WriteLine(item.Status);
            }
        }
    }
}
