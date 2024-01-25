using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views.Modals;
using GFMSLibrary;
using iText.Html2pdf;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GFMS.ViewModels.PrincipalViewModels
{
    public class PrincipalTeacherViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public PrincipalTeacherViewModel()
        {
            LoadedCommand = new Command(async obj =>
            {
                await LoadAll();
            });

            ClearCommand = new Command(async obj =>
            {
                GradeLevelSelected = null;
                StatusSelected = null;
                SearchText = null;
                await LoadAll();
            });
            RefreshCommand = new Command(async obj =>
            {

                await LoadAll();
            });

            PrintCommand = new Command(obj =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                // Set the file name and extension filter
                saveFileDialog.FileName = "Officially Enrolled.pdf";
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string htmlContent = "";
                    htmlContent += @"<h1 style=""text-align: center;"">Officially Enrolled</h1><table border=""1"" style=""width: 100%; border-collapse: collapse;"">";
                    htmlContent += @"<tr><th>LRN</th><th>Student Name</th><th>Grade Level</th><th>Status</th></tr>";
                    for (int i = 0; i < StudentList.Count; i++)
                    {
                        htmlContent += @$"<tr><td>{StudentList[i].Student.LRN}</td><td>{StudentList[i].StudentName}</td><td>{StudentList[i].Registration.Grade}</td><td>{StudentList[i].Status}</td></tr>";
                    }

                    htmlContent += @"</table>";

                    HtmlConverter.ConvertToPdf(htmlContent, new FileStream(saveFileDialog.FileName, FileMode.Create));
                }
            });
            EditCommand = new Command(async obj =>
            {
                RegisteredStudent? student = obj as RegisteredStudent;
                EditOfficiallyEnrolled window = new EditOfficiallyEnrolled(student!);
                if (window.ShowDialog() == true)
                {
                    await LoadAll();
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

                if (registeredStudent.Registration.Year == (Years.LastOrDefault() ?? "2023-2024"))
                {

                    if (!string.IsNullOrWhiteSpace(SearchText))
                    {
                        string searchText = SearchText.ToLower();
                        if (student.LRN.Contains(searchText) || student.LastName.ToLower().Contains(searchText))
                        {
                            if (!string.IsNullOrEmpty(GradeLevelSelected))
                            {
                                if (registeredStudent.Registration.Grade == GradeLevelSelected)
                                {
                                    if (!string.IsNullOrEmpty(StatusSelected))
                                    {
                                        if (registeredStudent.Status == StatusSelected)
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
                                if (!string.IsNullOrEmpty(StatusSelected))
                                {
                                    if (registeredStudent.Status == StatusSelected)
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
                                if (!string.IsNullOrEmpty(StatusSelected))
                                {
                                    if (registeredStudent.Status == StatusSelected)
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
                            if (!string.IsNullOrEmpty(StatusSelected))
                            {
                                if (registeredStudent.Status == StatusSelected)
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

        public ObservableCollection<string> StatusList { get; set; } = new ObservableCollection<string>
        {
            "Officially Enrolled",
            "Temporary Enrolled",
        };

        private string? gradeLevelSelected;
        public string? GradeLevelSelected
        {
            get => gradeLevelSelected;
            set => SetProperty(ref gradeLevelSelected, value);
        }

        private string? statusSelected;
        public string? StatusSelected
        {
            get => statusSelected;
            set => SetProperty(ref statusSelected, value);
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
        public ICommand PrintCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand LoadedCommand { get; }
    }
}
