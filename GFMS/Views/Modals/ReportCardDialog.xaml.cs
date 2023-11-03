using GFMS.Models;
using GFMSLibrary;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        private readonly LoginCredentials Credentials = new LoginCredentials();
        public ReportCardDialog()
        {
            InitializeComponent();
            LoadAllDataAsync();
            

            DataContext = this;
        }

        private async void LoadAllDataAsync()
        {
            var JsonBehavior = await Credentials.GetByIdAsync<SubjectJSON>("6", "subjects");
            List<Behavior>? result1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Behavior>>(JsonBehavior.Subjects!);
            foreach (var item in result1)
            {
                BehaviorList.Add(item);
            }
            var JsonSubject = await Credentials.GetByIdAsync<SubjectJSON>("3", "subjects");
            List<Subject>? result2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Subject>>(JsonSubject.Subjects!);
            foreach (var item in result2)
            {
                SubjectList.Add(item);
            }
            var JsonAttendance = await Credentials.GetByIdAsync<SubjectJSON>("5", "subjects");
            List<Attendance>? result3 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Attendance>>(JsonAttendance.Subjects!);
            foreach (var item in result3)
            {
                AttendanceList.Add(item);
            }
        }

        public string? AverageFirstGrading
        {
            get
            {
                if(SubjectList.Count == 0)
                {
                    return string.Empty;
                }
                bool hasValue = true;
                int average = 0;
                foreach(var item in SubjectList)
                {
                    if(item.FirstGrading.HasValue && item.SecondGrading.HasValue && item.ThirdGrading.HasValue && item.FourthGrading.HasValue)
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
                if(hasValue)
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
                    if(Convert.ToInt16(AverageFinalRating) > 74)
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
