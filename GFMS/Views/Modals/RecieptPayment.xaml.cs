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
        public RecieptPayment()
        {
            InitializeComponent();
            LoadedCommand = new Command(obj =>
            {

            });
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
        //private void LoadAllDataAsync(StudentReport student, Users teacher, Users principal)
        //{
        //    FullName = $"{student.Student!.LastName} {student.Student.FirstName} {student.Student.MiddleName![0].ToString().ToUpper()}.";
        //    Sex = student.Student.Gender;
        //    LRN = student.Student.LRN;
        //    Grade = student.Registration!.Grade;
        //    if (teacher != null)
        //    {
        //        Adviser = $"{teacher.FirstName} {teacher.LastName}";
        //    }
        //    if (principal != null)
        //    {
        //        Principal = $"{principal.FirstName} {principal.LastName}";
        //    }
        //    DateTime today = DateTime.Today;
        //    var BirthDate = Convert.ToDateTime(student.Student!.Birthdate);
        //    int ageValue = today.Year - BirthDate.Year;
        //    if (BirthDate.Date > today.AddYears(-ageValue))
        //    {
        //        ageValue--;
        //    }
        //    Age = ageValue.ToString();

        //    if (student.ReportCard != null)
        //    {
        //        if (student.ReportCard.Behavior != null)
        //        {
        //            List<Behavior>? list = JsonConvert.DeserializeObject<List<Behavior>>(student.ReportCard.Behavior);
        //            foreach (var item in list!)
        //            {
        //                BehaviorList.Add(item);

        //            }
        //        }
        //        if (student.ReportCard.Subjects != null)
        //        {
        //            List<Subject>? list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Subject>>(student.ReportCard.Subjects);
        //            foreach (var item in list!)
        //            {
        //                SubjectList.Add(item);

        //            }
        //        }
        //        if (student.ReportCard.Attendance != null)
        //        {
        //            List<Attendance>? list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Attendance>>(student.ReportCard.Attendance);
        //            foreach (var item in list!)
        //            {
        //                AttendanceList.Add(item);
        //            }
        //        }
        //        if (student.ReportCard.Narrative != null)
        //        {
        //            List<Narrative>? list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Narrative>>(student.ReportCard.Narrative);
        //            foreach (var item in list!)
        //            {
        //                NarrativeList.Add(item);
        //            }
        //        }
        //    }
        //}

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand LoadedCommand { get; }



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
