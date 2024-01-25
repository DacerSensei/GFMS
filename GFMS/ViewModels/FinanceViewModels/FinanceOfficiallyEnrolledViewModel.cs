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
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace GFMS.ViewModels.FinanceViewModels
{
    public class FinanceOfficiallyEnrolledViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public FinanceOfficiallyEnrolledViewModel()
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
                await LoadAllAsync();
            });
            ClearCommand = new Command(async obj =>
            {
                GradeLevelSelected = null;
                YearSelected = null;
                SearchText = null;
                await LoadAllAsync();
            });
            RefreshCommand = new Command(async obj =>
            {
                await LoadAllAsync();
            });
            PayCommand = new Command(async obj =>
            {
                StudentAccounting? student = obj as StudentAccounting;
                if (student != null)
                {
                    RecieptPayment window = new RecieptPayment(student);
                    if (window.ShowDialog() == true)
                    {
                        await LoadAllAsync();
                    }
                }
            });
            HistoryCommand = new Command(async obj =>
            {
                StudentAccounting? student = obj as StudentAccounting;
                if (student != null)
                {
                    PaymentHistoryDialog window = new PaymentHistoryDialog(student);
                    if (window.ShowDialog() == true)
                    {
                        await LoadAllAsync();
                    }
                }
            });
        }

        private async Task LoadAllAsync()
        {
            StudentList.Clear();
            Task<List<Student>>? studentListTask = Credentials.GetAllDataAsync<Student>("student");
            Task<List<Registration>>? registrationListTask = Credentials.GetAllDataAsync<Registration>("registration");
            Task<List<Accounting>>? accountingListTask = Credentials.GetAllDataAsync<Accounting>("accounting");

            await Task.WhenAll(studentListTask, registrationListTask, accountingListTask);

            List<Student> studentList = studentListTask.Result;
            List<Registration> registrationList = registrationListTask.Result;
            List<Accounting> accountingList = accountingListTask.Result;

            foreach (var student in registrationList.Reverse<Registration>())
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
                        if(result != null)
                        {
                            studentAccounting.TuitionDetailsList.Add(result);
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
                    if (studentAccounting.Student.LRN.Contains(searchText) || studentAccounting.Student.LastName.ToLower().Contains(searchText))
                    {
                        if (!string.IsNullOrEmpty(GradeLevelSelected))
                        {
                            if (studentAccounting.Registration.Grade == GradeLevelSelected)
                            {
                                if (!string.IsNullOrEmpty(YearSelected))
                                {
                                    if (studentAccounting.Registration.Year == YearSelected)
                                    {
                                        StudentList.Add(studentAccounting);
                                    }
                                }
                                else
                                {
                                    StudentList.Add(studentAccounting);
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(YearSelected))
                            {
                                if (studentAccounting.Registration.Year == YearSelected)
                                {
                                    StudentList.Add(studentAccounting);
                                }
                            }
                            else
                            {
                                StudentList.Add(studentAccounting);
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(GradeLevelSelected))
                    {
                        if (studentAccounting.Registration.Grade == GradeLevelSelected)
                        {
                            if (!string.IsNullOrEmpty(YearSelected))
                            {
                                if (studentAccounting.Registration.Year == YearSelected)
                                {
                                    StudentList.Add(studentAccounting);
                                }
                            }
                            else
                            {
                                StudentList.Add(studentAccounting);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(YearSelected))
                        {
                            if (studentAccounting.Registration.Year == YearSelected)
                            {
                                StudentList.Add(studentAccounting);
                            }
                        }
                        else
                        {
                            StudentList.Add(studentAccounting);
                        }
                    }
                }

            }
        }

        public ObservableCollection<StudentAccounting> StudentList { get; set; } = new ObservableCollection<StudentAccounting>();
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
                LoadAllAsync();
            }

        }

        public ICommand PayCommand { get; }
        public ICommand HistoryCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand LoadedCommand { get; }
    }
}
