
using GFMS.Models;
using GFMSLibrary;
using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using MySqlConnector;
using System.Diagnostics;
using GFMS.Commands;

namespace GFMS.Views.Modals
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : UserControl, INotifyPropertyChanged
    {
        private LoginCredentials Credentials = new LoginCredentials();
        private readonly string? Server = "localhost";
        private readonly string? Username = "root";
        private readonly string? Password = "";
        private readonly string? Database = "school_db";
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
            string OTP = new Random().Next(100000, 999999).ToString();
            if (User != null)
            {
                
                var result = await EmailProcessor.SendEmail(Email, OTP);
                if (result == true)
                {
                    string userotp = Microsoft.VisualBasic.Interaction.InputBox("Enter the OTP you received in your email address:", "OTP Validation", "");
                    if (userotp != null)
                    {
                        if (userotp.Equals(OTP))
                        {
                            string newpass = Microsoft.VisualBasic.Interaction.InputBox("Enter your new password:", "Password Reset", "");
                            if (newpass != null)
                            {
                                string ConnectionString = $"Server={Server};Database={Database};Username={Username};Password={Password}";

                                try
                                {
                                    MySqlConnection Connectiona = new MySqlConnection(ConnectionString);
                                    Connectiona.Open();
                                    MySqlCommand x = new MySqlCommand("UPDATE users SET password = @pass WHERE email = @email", Connectiona);
                                    x.Parameters.AddWithValue("@email", Email);
                                    x.Parameters.AddWithValue("@pass", newpass);
                                    x.ExecuteNonQuery();
                                    Connectiona.Close();
                                    MessageBox.Show("Password is changed! You can now close the forgot password dialog.", "Password Reset", MessageBoxButton.OK, MessageBoxImage.Information);
                                    DialogHost.CloseDialogCommand.Execute(true, null);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);
                                }


                            } else
                            {
                                MessageBox.Show("Invalid Password!", "Password Reset", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            DialogHost.CloseDialogCommand.Execute(true, null);
                            return;
                        } else
                        {
                            MessageBox.Show("Wrong OTP!", "OTP Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                            DialogHost.CloseDialogCommand.Execute(true, null);
                            return;
                        }
                    }
                    
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
