using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views.Modals;
using GFMSLibrary;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels.RegistrarViewModels
{
    public class RegistrarRequirementsViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public RegistrarRequirementsViewModel()
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
                StudentRequirements? student = obj as StudentRequirements;
                if (student != null)
                {
                    RequirementListDialog Dialog = new RequirementListDialog(student.Requirement!, student.Student!.id.ToString());
                    if(Dialog.ShowDialog() == true)
                    {
                        await LoadAll();
                    }
                }
            });
        }

        private async Task LoadAll()
        {
            StudentList.Clear();
            var studentList = await Credentials.GetAllDataAsync<Student>("student");
            var requirementList = await Credentials.GetAllDataAsync<Requirement>("student_requirements");
            var registrationList = await Credentials.GetAllDataAsync<Registration>("registration");

            foreach (var student in studentList.Reverse<Student>())
            {
                // Find the requirements associated with the current student using the Student_ID
                var studentRequirements = new StudentRequirements
                {
                    Student = student,
                };

                var requirements = requirementList.Where(r => r.Student_ID == student.id).ToList();
                studentRequirements.Registration = registrationList.Where(r => Convert.ToInt32(r.Student_Id) == student.id).ToList().FirstOrDefault();
                studentRequirements.Requirement = requirements;

                if (requirements.Count == 9)
                {
                    studentRequirements.Status = "Completed";
                    studentRequirements.StatusColor = "#3dc03c";
                }
                else
                {
                    studentRequirements.Status = $"Missing {9 - requirements.Count} / 9";
                    studentRequirements.StatusColor = "#fe3839";
                }


                // Add the StudentRequirements instance to the ObservableCollection
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    string searchText = SearchText.ToLower();
                    if (studentRequirements.Student.LRN.Contains(searchText) || studentRequirements.Student.LastName.ToLower().Contains(searchText))
                    {
                        if (!string.IsNullOrEmpty(GradeLevelSelected))
                        {
                            if (studentRequirements.Registration.Grade == GradeLevelSelected)
                            {
                                if (!string.IsNullOrEmpty(YearSelected))
                                {
                                    if (studentRequirements.Registration.Year == YearSelected)
                                    {
                                        StudentList.Add(studentRequirements);
                                    }
                                }
                                else
                                {
                                    StudentList.Add(studentRequirements);
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(YearSelected))
                            {
                                if (studentRequirements.Registration.Year == YearSelected)
                                {
                                    StudentList.Add(studentRequirements);
                                }
                            }
                            else
                            {
                                StudentList.Add(studentRequirements);
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(GradeLevelSelected))
                    {
                        if (studentRequirements.Registration.Grade == GradeLevelSelected)
                        {
                            if (!string.IsNullOrEmpty(YearSelected))
                            {
                                if (studentRequirements.Registration.Year == YearSelected)
                                {
                                    StudentList.Add(studentRequirements);
                                }
                            }
                            else
                            {
                                StudentList.Add(studentRequirements);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(YearSelected))
                        {
                            if (studentRequirements.Registration.Year == YearSelected)
                            {
                                StudentList.Add(studentRequirements);
                            }
                        }
                        else
                        {
                            StudentList.Add(studentRequirements);
                        }
                    }
                }   
            }
        }

        public ObservableCollection<StudentRequirements> StudentList { get; set; } = new ObservableCollection<StudentRequirements>();
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
