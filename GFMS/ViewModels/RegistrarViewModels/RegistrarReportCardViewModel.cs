using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views.Modals;
using GFMS.Views;
using GFMSLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace GFMS.ViewModels.RegistrarViewModels
{
    public class RegistrarReportCardViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public RegistrarReportCardViewModel()
        {
            LoadedCommand = new Command(async obj =>
            {
                this.YearList.Clear();
                var YearList = await Credentials.GetAllDataAsync<SchoolYear>("school_year");
                var Years = YearList.OrderBy(y => y.Id).Select(y => y.Year).ToList();
                foreach (var item in Years)
                {
                    if (item != null)
                    {
                        this.YearList.Add(item);
                    }
                }
                await LoadAll();
            });
            ClearCommand = new Command(async obj =>
            {
                GradeLevelSelected = null;
                YearSelected = null;
                SearchText = null;
                await LoadAll();
            });
            RefreshCommand = new Command(async obj =>
            {
                await LoadAll();
            });
            ViewCommand = new Command(async obj =>
            {
                StudentReport? student = obj as StudentReport;
                if (student != null)
                {
                    Users principal = await Credentials.GetByAnonymousAsync<Users>("usertype", "PRINCIPAL", "users");
                    UserTeacher findTeacher = await Credentials.GetByAnonymousAsync<UserTeacher>("grade", student.Registration!.Grade!, "teachers");
                    if (findTeacher != null)
                    {
                        Users teacher = await Credentials.GetByIdAsync<Users>(findTeacher.User_Id.ToString(), "users");
                        ReportCardDialog window = new ReportCardDialog(student, teacher, principal, false, true, false, student.IsPaid);
                        if (window.ShowDialog() == true)
                        {
                            await LoadAll();
                        }
                    }
                    else
                    {
                        await DialogHost.Show(new MessageDialog("Oops!", "This student dont have teacher yet, please create a teacher for this grade level"), "RootDialog");
                    }
                }
            });
        }

        private async Task LoadAll()
        {
            StudentList.Clear();
            var studentListTask = Credentials.GetAllDataAsync<Student>("student");
            var registrationListTask = Credentials.GetAllDataAsync<Registration>("registration");
            var studentGradeListTask = Credentials.GetAllDataAsync<ReportCard>("studentgrades");
            Task<List<Accounting>>? accountingListTask = Credentials.GetAllDataAsync<Accounting>("accounting");
            var requirementListTask = Credentials.GetAllDataAsync<Requirement>("student_requirements");


            await Task.WhenAll(studentListTask, registrationListTask, studentGradeListTask, accountingListTask, requirementListTask);

            var studentList = studentListTask.Result;
            var registrationList = registrationListTask.Result;
            var studentGradeList = studentGradeListTask.Result;
            List<Accounting> accountingList = accountingListTask.Result;
            var requirementList = requirementListTask.Result;


            foreach (var student in registrationList.Reverse<Registration>())
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

                if (studentReport.Registration.Year == (YearList.LastOrDefault() ?? "2023-2024") || !string.IsNullOrEmpty(YearSelected))
                {
                    if (!string.IsNullOrWhiteSpace(SearchText))
                    {
                        string searchText = SearchText.ToLower();
                        if (studentReport.Student.LRN.Contains(searchText) || studentReport.Student.LastName.ToLower().Contains(searchText))
                        {
                            if (!string.IsNullOrEmpty(GradeLevelSelected))
                            {
                                if (studentReport.Registration.Grade == GradeLevelSelected)
                                {
                                    if (!string.IsNullOrEmpty(YearSelected))
                                    {
                                        if (studentReport.Registration.Year == YearSelected)
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
                            else
                            {
                                if (!string.IsNullOrEmpty(YearSelected))
                                {
                                    if (studentReport.Registration.Year == YearSelected)
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
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(GradeLevelSelected))
                        {
                            if (studentReport.Registration.Grade == GradeLevelSelected)
                            {
                                if (!string.IsNullOrEmpty(YearSelected))
                                {
                                    if (studentReport.Registration.Year == YearSelected)
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
                        else
                        {
                            if (!string.IsNullOrEmpty(YearSelected))
                            {
                                if (studentReport.Registration.Year == YearSelected)
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
                }
            }
        }

        public ObservableCollection<StudentReport> StudentList { get; set; } = new ObservableCollection<StudentReport>();
        public ObservableCollection<string> GradeLevel { get; set; } = new ObservableCollection<string>
        {
            "TODDLER",
            "NURSERY",
            "KINDER 1",
            "KINDER 2",
            "Grade 1",
            "Grade 2",
            "Grade 3",
            "Grade 4",
            "Grade 5",
            "Grade 6",
            "Grade 7",
            "Grade 8",
            "Grade 9",
            "Grade 10",
            "Grade 11 - ABM",
            "Grade 11 - HUMSS",
            "Grade 11 - STEM",
            "Grade 11 - GAS",
            "Grade 12 - ABM",
            "Grade 12 - HUMMS",
            "Grade 12 - STEM",
            "Grade 12 - GAS"
        };

        public ObservableCollection<string> YearList { get; set; } = new ObservableCollection<string>();

        private string? gradeLevelSelected;
        public string? GradeLevelSelected
        {
            get => gradeLevelSelected;
            set => SetProperty(ref gradeLevelSelected, value);
        }

        private string? yearSelected;
        public string? YearSelected
        {
            get => yearSelected;
            set => SetProperty(ref yearSelected, value);
        }

        private string? _searchText;

        public string? SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
            {
                return;
            }

            property = value;
            OnPropertyChanged(propertyName);
            if (value != null)
            {
                LoadAll();
            }

        }

        public ICommand ViewCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand LoadedCommand { get; }
    }
}
