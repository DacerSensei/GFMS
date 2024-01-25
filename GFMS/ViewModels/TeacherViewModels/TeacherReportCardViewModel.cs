using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views;
using GFMS.Views.Modals;
using GFMSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels.TeacherViewModels
{
    public class TeacherReportCardViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();

        public TeacherReportCardViewModel()
        {
            LoadAll();
            ViewCommand = new Command(async obj =>
            {
                StudentReport? student = obj as StudentReport;
                if (student != null)
                {
                    Users principal = await Credentials.GetByAnonymousAsync<Users>("usertype", "PRINCIPAL", "users");
                    ReportCardDialog window = new ReportCardDialog(student, MainWindow.User!, principal, false, false);
                    if (window.ShowDialog() == true)
                    {
                        LoadAll();
                    }
                }
            });

            EditCommand = new Command(async obj =>
            {
                StudentReport? student = obj as StudentReport;
                if (student != null)
                {
                    Users principal = await Credentials.GetByAnonymousAsync<Users>("usertype", "PRINCIPAL", "users");
                    ReportCardDialog window = new ReportCardDialog(student, MainWindow.User!, principal, true);
                    if (window.ShowDialog() == true)
                    {
                        LoadAll();
                    }
                }
            });
        }

        private async void LoadAll()
        {
            if (MainWindow.Teacher == null)
            {
                return;
            }
            StudentList.Clear();
            var YearList = await Credentials.GetAllDataAsync<SchoolYear>("school_year");
            var Years = YearList.OrderBy(y => y.Id).Select(y => y.Year).ToList();
            Where where = new Where()
            {
                Grade = MainWindow.Teacher.Grade,
                Year = Years.LastOrDefault() ?? "2023 - 2024"

            };

            var studentListTask = Credentials.GetAllDataAsync<Student>("student");
            var registrationListTask = Credentials.GetAllDataAsync<Registration, Where>("registration", where);
            var studentGradeListTask = Credentials.GetAllDataAsync<ReportCard>("studentgrades");
            Task<List<Accounting>>? accountingListTask = Credentials.GetAllDataAsync<Accounting>("accounting");
            var requirementListTask = Credentials.GetAllDataAsync<Requirement>("student_requirements");

            await Task.WhenAll(studentListTask, registrationListTask, studentGradeListTask, accountingListTask, requirementListTask);

            var studentList = studentListTask.Result;
            var registrationList = registrationListTask.Result;
            var studentGradeList = studentGradeListTask.Result;
            List<Accounting> accountingList = accountingListTask.Result;
            var requirementList = requirementListTask.Result;

            foreach (var student in registrationList)
            {
                // Find the requirements associated with the current student using the Student_ID
                var studentReport = new StudentReport
                {
                    Registration = student,
                    Student = studentList.Where(r => r.id == Convert.ToInt16(student.Student_Id)).ToList().FirstOrDefault(),
                    ReportCard = studentGradeList.Where(r => Convert.ToInt32(r.Registration_Id) == student.Id).ToList().FirstOrDefault(),
                    PaymentList = accountingList.Where(a => a.Registration_Id == student.Id.ToString()).ToList(),
                    TuitionDetailsList = new List<TuitionDetails>()

                };

                studentReport.Requirement = requirementList.Where(r => r.Student_ID == studentReport.Student.id).ToList();

                foreach (var payment in studentReport.PaymentList)
                {
                    try
                    {
                        var result = JsonConvert.DeserializeObject<TuitionDetails>(payment.Payment ?? "");
                        if (result != null)
                        {
                            studentReport.TuitionDetailsList.Add(result);
                        }
                    }
                    catch (JsonReaderException ex)
                    {
                        Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                    }
                }


                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    string x = SearchText.ToLower();
                    if (

                        (studentReport.Student.LRN.Contains(x)) ||
                        (studentReport.Student.FirstName.ToLower().Contains(x)) ||
                        (studentReport.Student.LastName.ToLower().Contains(x))

                        )
                    {
                        StudentList.Add(studentReport);
                    }
                }
                else
                {
                    StudentList.Add(studentReport);
                }

            }
        }

        public ObservableCollection<StudentReport> StudentList { get; set; } = new ObservableCollection<StudentReport>();

        public ICommand ViewCommand { get; }
        public ICommand EditCommand { get; }
        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText == value)
                {
                    return;
                }
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                LoadAll();
            }
        }
        private class Where
        {
            public string? Grade { get; set; }
            public string? Year { get; set; }
        }
    }
}
