using GFMS.Commands;
using GFMS.Models;
using GFMSLibrary;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GFMS.Views.Modals
{
    /// <summary>
    /// Interaction logic for RecieptPayment.xaml
    /// </summary>
    public partial class RecieptPayment : Window, INotifyPropertyChanged
    {
        private readonly LoginCredentials Credentials = new LoginCredentials();
        public RecieptPayment(StudentAccounting student)
        {
            InitializeComponent();
            LoadedCommand = new Command(obj =>
            {

            });
            LoadAllDataAsync(student);
            SaveCommand = new Command(async obj =>
            {
                var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want to save?"), "SecondaryDialog");
                if ((bool)result! == false)
                {
                    return;
                }
                DialogResult = true;
                Close();
            });
            CancelCommand = new Command(obj =>
            {
                DialogResult = false;
                Close();
            });
            DataContext = this;
        }
        private void LoadAllDataAsync(StudentAccounting student)
        {
            CompleteName = $"{student.Student!.LastName} {student.Student.FirstName} {student.Student.MiddleName![0].ToString().ToUpper()}.";
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand LoadedCommand { get; }

        private string completeName;

        public string CompleteName
        {
            get { return completeName; }
            set { completeName = value; OnPropertyChanged(nameof(CompleteName)); }
        }

        private string date = DateTime.Now.Date.ToShortDateString();

        public string Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged(nameof(Date)); }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
