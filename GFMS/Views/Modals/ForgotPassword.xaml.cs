using GFMS.Models;
using GFMSLibrary;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace GFMS.Views.Modals
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : UserControl, INotifyPropertyChanged
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public ForgotPassword()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void YesButton(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Email cannot be empty");
                return;
            }
            var User = await Credentials.GetByAnonymousAsync<Users>("email", Email, "users");
            if (User != null)
            {
                var result = await EmailProcessor.SendEmail(Email);
                if (result == true)
                {
                    DialogHost.CloseDialogCommand.Execute(true, null);
                }
            }
            else
            {
                MessageBox.Show("Email doesn't exist");
                return;
            }
        }

        private string? _email;

        public string? Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }


        private void NoButton(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
