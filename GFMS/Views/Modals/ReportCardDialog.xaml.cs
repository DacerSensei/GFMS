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
    /// Interaction logic for ReportCardDialog.xaml
    /// </summary>
    public partial class ReportCardDialog : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Subject> SubjectList { get; set; } = new ObservableCollection<Subject>();
        public ObservableCollection<Behavior> BehaviorList { get; set; } = new ObservableCollection<Behavior>();
        public ObservableCollection<Attendance> AttendanceList { get; set; } = new ObservableCollection<Attendance>();
        public ObservableCollection<Narrative> NarrativeList { get; set; } = new ObservableCollection<Narrative>();
        private readonly LoginCredentials Credentials = new LoginCredentials();
        public ReportCardDialog(StudentReport student, Users teacher, Users principal, bool isEditable = false)
        {
            InitializeComponent();
            LoadedCommand = new Command(obj =>
            {

            });
            if (isEditable)
            {
                IsTeacher = "SAVE";
                IsReadOnly = false;
            }
            else
            {
                IsTeacher = "REQUEST PRINT";
            }
            LoadAllDataAsync(student, teacher, principal);
            SaveCommand = new Command(async obj =>
            {
                if (isEditable)
                {
                    var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure you want to save?"), "SecondaryDialog");
                    if ((bool)result! == false)
                    {
                        return;
                    }
                    ReportCard reportCard = new ReportCard()
                    {
                        Attendance = JsonConvert.SerializeObject(AttendanceList),
                        Behavior = JsonConvert.SerializeObject(BehaviorList),
                        Narrative = JsonConvert.SerializeObject(NarrativeList),
                        Subjects = JsonConvert.SerializeObject(SubjectList)
                    };
                    await Credentials.UpdateStudentAsync(reportCard, new { id = student.ReportCard!.Id }, "studentgrades");
                    DialogResult = true;
                    Close();
                }else
                {
                    await Credentials.RegisterStudentAsync(new Notification { User_Id = MainWindow.User!.Id.ToString(),  Message = $"Request print for { student.StudentName }", Status = "Pending" } ,"notification");
                    await DialogHost.Show(new MessageDialog("Notice", "You just send a request to principal"), "SecondaryDialog");
                    Close();
                }
            });
            CancelCommand = new Command(obj =>
            {
                DialogResult = false;
                Close();
            });
            DataContext = this;
        }

        private void LoadAllDataAsync(StudentReport student, Users teacher, Users principal)
        {
            FullName = $"{student.Student!.LastName} {student.Student.FirstName} {student.Student.MiddleName![0].ToString().ToUpper()}.";
            Sex = student.Student.Gender;
            LRN = student.Student.LRN;
            Grade = student.Registration!.Grade;
            if (teacher != null)
            {
                Adviser = $"{teacher.FirstName} {teacher.LastName}";
            }
            if (principal != null)
            {
                Principal = $"{principal.FirstName} {principal.LastName}";
            }
            DateTime today = DateTime.Today;
            var BirthDate = Convert.ToDateTime(student.Student!.Birthdate);
            int ageValue = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-ageValue))
            {
                ageValue--;
            }
            Age = ageValue.ToString();

            if (student.ReportCard != null)
            {
                if (student.ReportCard.Behavior != null)
                {
                    List<Behavior>? list = JsonConvert.DeserializeObject<List<Behavior>>(student.ReportCard.Behavior);
                    foreach (var item in list!)
                    {
                        BehaviorList.Add(item);

                    }
                }
                if (student.ReportCard.Subjects != null)
                {
                    List<Subject>? list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Subject>>(student.ReportCard.Subjects);
                    foreach (var item in list!)
                    {
                        SubjectList.Add(item);

                    }
                }
                if (student.ReportCard.Attendance != null)
                {
                    List<Attendance>? list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Attendance>>(student.ReportCard.Attendance);
                    foreach (var item in list!)
                    {
                        AttendanceList.Add(item);
                    }
                }
                if (student.ReportCard.Narrative != null)
                {
                    List<Narrative>? list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Narrative>>(student.ReportCard.Narrative);
                    foreach (var item in list!)
                    {
                        NarrativeList.Add(item);
                    }
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand LoadedCommand { get; }

        public string? AverageFirstGrading
        {
            get
            {
                if (SubjectList.Count == 0)
                {
                    return string.Empty;
                }
                bool hasValue = true;
                int average = 0;
                foreach (var item in SubjectList)
                {
                    if (item.FirstGrading.HasValue && item.SecondGrading.HasValue && item.ThirdGrading.HasValue && item.FourthGrading.HasValue)
                    {
                        average += item.FirstGrading.Value;
                        continue;
                    }
                    else
                    {
                        hasValue = false;
                        break;
                    }
                }
                if (hasValue)
                {
                    return $"{average / SubjectList.Count}";
                }
                return string.Empty;
            }
        }
        public string? AverageSecondGrading
        {
            get
            {
                if (SubjectList.Count == 0)
                {
                    return string.Empty;
                }
                bool hasValue = true;
                int average = 0;
                foreach (var item in SubjectList)
                {
                    if (item.FirstGrading.HasValue && item.SecondGrading.HasValue && item.ThirdGrading.HasValue && item.FourthGrading.HasValue)
                    {
                        average += item.SecondGrading.Value;
                        continue;
                    }
                    else
                    {
                        hasValue = false;
                        break;
                    }
                }
                if (hasValue)
                {
                    return $"{average / SubjectList.Count}";
                }
                return string.Empty;
            }
        }
        public string? AverageThirdGrading
        {
            get
            {
                if (SubjectList.Count == 0)
                {
                    return string.Empty;
                }
                bool hasValue = true;
                int average = 0;
                foreach (var item in SubjectList)
                {
                    if (item.FirstGrading.HasValue && item.SecondGrading.HasValue && item.ThirdGrading.HasValue && item.FourthGrading.HasValue)
                    {
                        average += item.ThirdGrading.Value;
                        continue;
                    }
                    else
                    {
                        hasValue = false;
                        break;
                    }
                }
                if (hasValue)
                {
                    return $"{average / SubjectList.Count}";
                }
                return string.Empty;
            }
        }
        public string? AverageFourthGrading
        {
            get
            {
                if (SubjectList.Count == 0)
                {
                    return string.Empty;
                }
                bool hasValue = true;
                int average = 0;
                foreach (var item in SubjectList)
                {
                    if (item.FirstGrading.HasValue && item.SecondGrading.HasValue && item.ThirdGrading.HasValue && item.FourthGrading.HasValue)
                    {
                        average += item.FourthGrading.Value;
                        continue;
                    }
                    else
                    {
                        hasValue = false;
                        break;
                    }
                }
                if (hasValue)
                {
                    return $"{average / SubjectList.Count}";
                }
                return string.Empty;
            }
        }
        public string? AverageFinalRating
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(AverageFirstGrading) && !string.IsNullOrWhiteSpace(AverageSecondGrading) && !string.IsNullOrWhiteSpace(AverageThirdGrading) && !string.IsNullOrWhiteSpace(AverageFourthGrading))
                {
                    return $"{(Convert.ToInt16(AverageFirstGrading) + Convert.ToInt16(AverageSecondGrading) + Convert.ToInt16(AverageThirdGrading) + Convert.ToInt16(AverageFourthGrading)) / 4}";
                }
                return string.Empty;
            }
        }
        public string? AverageRemarks
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(AverageFinalRating))
                {
                    if (Convert.ToInt16(AverageFinalRating) > 74)
                    {
                        return "PASSED";
                    }
                    else
                    {
                        return "FAILED";
                    }
                }

                return string.Empty;
            }
        }

        public string? AverageRemarksColor
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(AverageFinalRating))
                {
                    if (Convert.ToInt16(AverageFinalRating) > 74)
                    {
                        return "LimeGreen";
                    }
                    else
                    {
                        return "#c12d2b";
                    }
                }

                return "#c12d2b";
            }
        }

        private string? _fullName;
        public string? FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
        }

        private string? _age;
        public string? Age
        {
            get { return _age; }
            set { _age = value; OnPropertyChanged(nameof(Age)); }
        }

        private string? _sex;
        public string? Sex
        {
            get { return _sex; }
            set { _sex = value; OnPropertyChanged(nameof(Sex)); }
        }

        private string? _lrn;
        public string? LRN
        {
            get { return _lrn; }
            set { _lrn = value; OnPropertyChanged(nameof(LRN)); }
        }

        private string? _grade;
        public string? Grade
        {
            get { return _grade; }
            set { _grade = value; OnPropertyChanged(nameof(Grade)); }
        }

        private string? _adviser;
        public string? Adviser
        {
            get { return _adviser; }
            set { _adviser = value; OnPropertyChanged(nameof(Adviser)); }
        }

        private string? _principal;
        public string? Principal
        {
            get { return _principal; }
            set { _principal = value; OnPropertyChanged(nameof(Principal)); }
        }

        private string isTeacher;

        public string IsTeacher
        {
            get { return isTeacher; }
            set { isTeacher = value; OnPropertyChanged(nameof(IsTeacher)); }
        }


        private bool _isReadOnly = true;
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); }
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
