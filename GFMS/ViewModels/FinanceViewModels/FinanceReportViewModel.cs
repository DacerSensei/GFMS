using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels.FinanceViewModels
{
    public class FinanceReportViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public FinanceReportViewModel()
        {
            LoadedCommand = new Command(async obj =>
            {
                SchoolYearList.Clear();
                var YearList = await Credentials.GetAllDataAsync<SchoolYear>("school_year");
                foreach (var item in YearList.OrderBy(y => y.Id).Select(y => y.Year).ToList())
                {
                    if (item == null)
                    {
                        continue;
                    }
                    SchoolYearList.Add(item);
                }
                SelectedYear = SchoolYearList.LastOrDefault() ?? "";
                await LoadData();
                SelectedYearChanged();
            });
        }

        public ObservableCollection<GradeAndTotalIncome> IncomeList { get; set; } = new ObservableCollection<GradeAndTotalIncome>();
        public ObservableCollection<string> SchoolYearList { get; set; } = new ObservableCollection<string>();
        public List<StudentAccounting> StudentList { get; set; } = new List<StudentAccounting>();

        private string selectedYear;

        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                if (value == selectedYear)
                {
                    return;
                }
                selectedYear = value;
                SelectedYearChanged();
                OnPropertyChanged(nameof(SelectedYear));
            }
        }

        private async Task LoadData()
        {
            IncomeList.Clear();
            StudentList.Clear();
            Task<List<Student>>? studentListTask = Credentials.GetAllDataAsync<Student>("student");
            Task<List<Registration>>? registrationListTask = Credentials.GetAllDataAsync<Registration>("registration");
            Task<List<Accounting>>? accountingListTask = Credentials.GetAllDataAsync<Accounting>("accounting");

            await Task.WhenAll(studentListTask, registrationListTask, accountingListTask);

            List<Student> studentList = studentListTask.Result;
            List<Registration> registrationList = registrationListTask.Result;
            List<Accounting> accountingList = accountingListTask.Result;

            foreach (var student in registrationList)
            {
                // Find the requirements associated with the current student using the Student_ID
                var studentAccounting = new StudentAccounting
                {
                    Registration = student,
                    Student = studentList.Where(s => s.id.ToString() == student.Student_Id).ToList().FirstOrDefault(),
                    PaymentList = accountingList.Where(a => a.Registration_Id == student.Id.ToString()).ToList(),
                    TuitionDetailsList = new List<TuitionDetails>()
                };
                foreach (var payment in studentAccounting.PaymentList)
                {
                    try
                    {
                        var result = JsonConvert.DeserializeObject<TuitionDetails>(payment.Payment ?? "");
                        if (result != null)
                        {
                            studentAccounting.TuitionDetailsList.Add(result);
                        }
                    }
                    catch (JsonReaderException ex)
                    {
                        Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                    }
                }
                StudentList.Add(studentAccounting);
            }
        }

        private void SelectedYearChanged()
        {
            var NewList = StudentList.Where(s => s.Registration!.Year == SelectedYear).ToList();
            var GroupBy = NewList.GroupBy(n => n.Registration!.Grade).Select(group => new GradeAndTotalIncome { GradeLevel = group.Key, TotalIncome = group.Sum(s => Convert.ToDecimal(s.TotalPaid)).ToString("N2") });
            IncomeList.Clear();
            foreach (var group in GroupBy)
            {
                IncomeList.Add(group);
            }
        }

        public ICommand LoadedCommand { get; }

        public class GradeAndTotalIncome
        {
            public string? GradeLevel { get; set; }
            public string? TotalIncome { get; set; }
        }
    }
}
