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
    /// Interaction logic for PaymentHistoryDialog.xaml
    /// </summary>
    public partial class PaymentHistoryDialog : Window, INotifyPropertyChanged
    {
        private readonly LoginCredentials Credentials = new LoginCredentials();
        public PaymentHistoryDialog(StudentAccounting student)
        {
            InitializeComponent();
            LoadedCommand = new Command(obj =>
            {

            });
            LoadAllDataAsync(student);
            CancelCommand = new Command(obj =>
            {
                DialogResult = false;
                Close();
            });
            DataContext = this;
        }

        public ObservableCollection<TuitionDetails> HistoryList { get; set; }

        private void LoadAllDataAsync(StudentAccounting student)
        {
            CompleteName = $"{student.Student!.LastName} {student.Student.FirstName} {student.Student.MiddleName![0].ToString().ToUpper()}.";
            LRN = student.Student.LRN!;
            GradeLevel = student.Registration!.Grade!;
            Status = student.Status!;
            HistoryList = new ObservableCollection<TuitionDetails>(student.TuitionDetailsList!);
        }

        public ICommand CancelCommand { get; }
        public ICommand LoadedCommand { get; }

        private string completeName;

        public string CompleteName
        {
            get { return completeName; }
            set { completeName = value; OnPropertyChanged(nameof(CompleteName)); }
        }

        private string lrn;

        public string LRN
        {
            get { return lrn; }
            set { lrn = value; OnPropertyChanged(nameof(LRN)); }
        }

        private string gradeLevel;

        public string GradeLevel
        {
            get { return gradeLevel; }
            set { gradeLevel = value; OnPropertyChanged(nameof(GradeLevel)); }
        }

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(nameof(Status)); }
        }

        public string BalanceToBePaid
        {
            get
            {
                if (HistoryList != null && HistoryList.Count > 0 && !HistoryList.All(details => string.IsNullOrEmpty(details.TotalTuitionFee)))
                {
                    decimal totalPaid = 0m;
                    for (int i = 0; i < HistoryList.Count; i++)
                    {
                        if (string.IsNullOrWhiteSpace(HistoryList[i].Payment) || string.IsNullOrWhiteSpace(HistoryList[i].TuitionFee))
                        {
                            continue;
                        }
                        totalPaid += Convert.ToDecimal(HistoryList[i].Payment);
                    }
                    var totalTuitionFee = HistoryList.FirstOrDefault(details => !string.IsNullOrEmpty(details.TotalTuitionFee));
                    var tuition = Convert.ToDecimal(totalTuitionFee!.TotalTuitionFee ?? "0");
                    var total = tuition - totalPaid;

                    return total > tuition ? "0.00" : total.ToString("N2");
                }
                else
                {
                    return "0.00";
                }
            }
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
