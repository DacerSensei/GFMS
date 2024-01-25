using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views.Modals;
using GFMSLibrary;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels.RegistrarViewModels
{
    public class RegistrarStudentRecordsViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public RegistrarStudentRecordsViewModel()
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

            EditCommand = new Command(async obj =>
            {
                RegisteredStudent? student = obj as RegisteredStudent;
                var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want to change year?"), "RootDialog");
                if ((bool)result! == false)
                {
                    return;
                }

                string currentGradeLevel = student.Registration.Grade;
                int currentIndex = GradeLevel.IndexOf(currentGradeLevel);
                if (currentGradeLevel == "Grade 10")
                {
                    MessageDialog Dialog = new MessageDialog("Notice", "You cannot change into SHS please enroll now.");
                    await DialogHost.Show(Dialog, "RootDialog");
                    return;
                }

                if (currentIndex != -1 && currentIndex < GradeLevel.Count - 1)
                {
                    string nextGradeLevel = GradeLevel[currentIndex + 1];
                    await Credentials.UpdateStudentAsync(new Registration { Grade = nextGradeLevel, Year = YearList.LastOrDefault() ?? "2023-2024" }, new { id = student.Registration.Id }, "registration");
                    MessageDialog Dialog = new MessageDialog("Notice", "Successfully changed the year of student.", true);
                    await DialogHost.Show(Dialog, "RootDialog");
                    await LoadAll();
                }
                else
                {
                    Debug.WriteLine("Current grade level not found or is the last one.");
                }
            });
        }

        private async Task LoadAll()
        {
            StudentList.Clear();

            var studentListTask = Credentials.GetAllDataAsync<Student>("student");
            var registrationListTask = Credentials.GetAllDataAsync<Registration>("registration");
            var previousSchoolListTask = Credentials.GetAllDataAsync<PreviousSchool>("previous_school");
            var YearListTask = Credentials.GetAllDataAsync<SchoolYear>("school_year");
            Task<List<Accounting>>? accountingListTask = Credentials.GetAllDataAsync<Accounting>("accounting");
            var requirementListTask = Credentials.GetAllDataAsync<Requirement>("student_requirements");

            await Task.WhenAll(studentListTask, registrationListTask, previousSchoolListTask, YearListTask, accountingListTask, requirementListTask);

            var studentList = studentListTask.Result;
            var registrationList = registrationListTask.Result;
            var previousSchoolList = previousSchoolListTask.Result;
            var YearList = YearListTask.Result;
            List<Accounting> accountingList = accountingListTask.Result;
            var requirementList = requirementListTask.Result;

            var Years = YearList.OrderBy(y => y.Id).Select(y => y.Year).ToList();

            foreach (var student in studentList.Reverse<Student>())
            {
                // Find the requirements associated with the current student using the Student_ID
                var registeredStudent = new RegisteredStudent
                {
                    Student = student,
                    Registration = registrationList.Where(r => Convert.ToInt32(r.Student_Id) == student.id).ToList().FirstOrDefault(),
                    PreviousSchool = previousSchoolList.Where(r => Convert.ToInt32(r.student_id) == student.id).ToList().FirstOrDefault(),
                    Requirement = requirementList.Where(r => r.Student_ID == student.id).ToList(),
                    TuitionDetailsList = new List<TuitionDetails>()
                };
                registeredStudent.PaymentList = accountingList.Where(a => a.Registration_Id == registeredStudent.Registration.Id.ToString()).ToList();

                foreach (var payment in registeredStudent.PaymentList)
                {
                    try
                    {
                        var result = JsonConvert.DeserializeObject<TuitionDetails>(payment.Payment ?? "");
                        if (result != null)
                        {
                            registeredStudent.TuitionDetailsList.Add(result);
                        }
                    }
                    catch (JsonReaderException ex)
                    {
                        Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                    }
                }

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    string searchText = SearchText.ToLower();
                    if (student.LRN.Contains(searchText) || student.LastName.ToLower().Contains(searchText))
                    {
                        if (!string.IsNullOrEmpty(GradeLevelSelected))
                        {
                            if (registeredStudent.Registration.Grade == GradeLevelSelected)
                            {
                                if (!string.IsNullOrEmpty(YearSelected))
                                {
                                    if (registeredStudent.Registration.Year == YearSelected)
                                    {
                                        StudentList.Add(registeredStudent);
                                    }
                                }
                                else
                                {
                                    StudentList.Add(registeredStudent);
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(YearSelected))
                            {
                                if (registeredStudent.Registration.Year == YearSelected)
                                {
                                    StudentList.Add(registeredStudent);
                                }
                            }
                            else
                            {
                                StudentList.Add(registeredStudent);
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(GradeLevelSelected))
                    {
                        if (registeredStudent.Registration.Grade == GradeLevelSelected)
                        {
                            if (!string.IsNullOrEmpty(YearSelected))
                            {
                                if (registeredStudent.Registration.Year == YearSelected)
                                {
                                    StudentList.Add(registeredStudent);
                                }
                            }
                            else
                            {
                                StudentList.Add(registeredStudent);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(YearSelected))
                        {
                            if (registeredStudent.Registration.Year == YearSelected)
                            {
                                StudentList.Add(registeredStudent);
                            }
                        }
                        else
                        {
                            StudentList.Add(registeredStudent);
                        }
                    }
                }
            }
        }

        public ObservableCollection<RegisteredStudent> StudentList { get; set; } = new ObservableCollection<RegisteredStudent>();
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

        public ICommand EditCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand LoadedCommand { get; }
    }
}
