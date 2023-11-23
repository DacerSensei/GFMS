using GFMS.Core;
using GFMS.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using GFMSLibrary;
using GFMS.Commands;
using LiveChartsCore.Kernel.Sketches;

namespace GFMS.ViewModels.FinanceViewModels
{
    public class FinanceDashboardViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public FinanceDashboardViewModel()
        {
            LoadedCommand = new Command(async obj =>
            {
                await LoadDataAsync();
            });
        }

        private async Task LoadDataAsync()
        {
            UnpaidList.Clear();
            PaidList.Clear();
            StudentList.Clear();
            StudentSecondList.Clear();
            PieGraph.Clear();
            CrossX.Clear();
            LineGraph.Clear();
            var studentList = await Credentials.GetAllDataAsync<Student>("student");
            var registrationList = await Credentials.GetAllDataAsync<Registration>("registration");
            var accountingList = await Credentials.GetAllDataAsync<Accounting>("accounting");
            var previousSchoolList = await Credentials.GetAllDataAsync<PreviousSchool>("previous_school");

            foreach (var student in registrationList)
            {
                // Find the requirements associated with the current student using the Student_ID
                var studentAccounting = new StudentAccounting
                {
                    Registration = student,
                };
                studentAccounting.Student = studentList.Where(s => s.id.ToString() == student.Student_Id).ToList().FirstOrDefault();
                studentAccounting.PaymentList = accountingList.Where(a => a.Registration_Id == student.Id.ToString()).ToList();
                StudentList.Add(studentAccounting);
            }

            foreach (var student in studentList)
            {
                // Find the requirements associated with the current student using the Student_ID
                var registeredStudent = new RegisteredStudent
                {
                    Student = student,
                };
                registeredStudent.Registration = registrationList.Where(r => Convert.ToInt32(r.Student_Id) == student.id).ToList().FirstOrDefault();
                registeredStudent.PreviousSchool = previousSchoolList.Where(r => Convert.ToInt32(r.student_id) == student.id).ToList().FirstOrDefault();
                if (Convert.ToInt16(registeredStudent.Registration!.Status) == 1)
                {
                    registeredStudent.Status = "Enrolled";
                    registeredStudent.StatusColor = "#3dc03c";
                }
                else
                {
                    registeredStudent.Status = "Temporary Enrolled";
                    registeredStudent.StatusColor = "#ffb302";
                }
                StudentSecondList.Add(registeredStudent);
            }


            YearList = await Credentials.GetAllDataAsync<SchoolYear>("school_year");
            var Years = YearList.OrderBy(y => y.Id).Select(y => y.Year).ToList();

            PieGraph.Add(CreatePieSeries("Unpaid", new[] { StudentList.Where(s => s.Status == "Unpaid").ToList().Count() }, DataCategory.UNPAID));
            PieGraph.Add(CreatePieSeries("Paid", new[] { StudentList.Where(s => s.Status == "Paid").ToList().Count() }, DataCategory.PAID));
            PreSchool = StudentSecondList.Where(s => s.Registration!.Level!.ToUpper() == "PRE SCHOOL").ToList().Count();
            Elementary = StudentSecondList.Where(s => s.Registration!.Level!.ToUpper() == "ELEMENTARY").ToList().Count();
            JuniorHigh = StudentSecondList.Where(s => s.Registration!.Level!.ToUpper() == "JUNIOR HIGH SCHOOL").ToList().Count();
            SeniorHigh = StudentSecondList.Where(s => s.Registration!.Level!.ToUpper() == "SENIOR HIGH SCHOOL").ToList().Count();
            CrossX.Add(new Axis
            {
                Labels = Years!
            });

            List<DataYear> PreSchoolDateYear = StudentSecondList.Where(s => s.Registration != null && Years.Contains(s.Registration.Year) && s.Registration.Level == "PRE SCHOOL")
                                      .GroupBy(s => s.Registration!.Year)
                                      .Select(group => new DataYear { Year = group.Key, Count = group.Count() }).ToList();
            List<DataYear> ElementaryDateYear = StudentSecondList.Where(s => s.Registration != null && Years.Contains(s.Registration.Year) && s.Registration.Level == "ELEMENTARY")
                                      .GroupBy(s => s.Registration!.Year)
                                      .Select(group => new DataYear { Year = group.Key, Count = group.Count() }).ToList();
            List<DataYear> JuniorDateYear = StudentSecondList.Where(s => s.Registration != null && Years.Contains(s.Registration.Year) && s.Registration.Level == "JUNIOR HIGH SCHOOL")
                                      .GroupBy(s => s.Registration!.Year)
                                      .Select(group => new DataYear { Year = group.Key, Count = group.Count() }).ToList();
            List<DataYear> SeniorDateYear = StudentSecondList.Where(s => s.Registration != null && Years.Contains(s.Registration.Year) && s.Registration.Level == "SENIOR HIGH SCHOOL")
                                      .GroupBy(s => s.Registration!.Year)
                                      .Select(group => new DataYear { Year = group.Key, Count = group.Count() }).ToList();

            LineGraph.Add(CreateLineSeries("PRE SCHOOL" ,new ObservableCollection<int>(PreSchoolDateYear.Select(y => y.Count))));
            LineGraph.Add(CreateLineSeries("ELEMENTARY" ,new ObservableCollection<int>(ElementaryDateYear.Select(y => y.Count))));
            LineGraph.Add(CreateLineSeries("JHS" ,new ObservableCollection<int>(JuniorDateYear.Select(y => y.Count))));
            LineGraph.Add(CreateLineSeries("SHS" ,new ObservableCollection<int>(SeniorDateYear.Select(y => y.Count))));


            //BarGraph.Add(CreateBarSeries("Senior High School", new ObservableCollection<int>(dataYear.Select(y => y.Count)), DataCategory.PRESCHOOL));
        }

        public ISeries[] SampleSeries { get; set; } =
    {
        new ColumnSeries<double>
        {
            Values = new ObservableCollection<double> { 2, 5, 4, 3 },
            IsVisible = true
        },
        new ColumnSeries<double>
        {
            Values = new ObservableCollection<double> { 6, 3, 2, 8 },
            IsVisible = true
        }
    };

        public ObservableCollection<StudentAccounting> UnpaidList { get; set; } = new ObservableCollection<StudentAccounting>();
        public ObservableCollection<StudentAccounting> PaidList { get; set; } = new ObservableCollection<StudentAccounting>();
        public ObservableCollection<StudentAccounting> StudentList { get; set; } = new ObservableCollection<StudentAccounting>();
        public ObservableCollection<RegisteredStudent> StudentSecondList { get; set; } = new ObservableCollection<RegisteredStudent>();

        public List<SchoolYear> YearList { get; set; } = new List<SchoolYear>();

        public ObservableCollection<ISeries> BarGraph { get; set; } = new ObservableCollection<ISeries>();
        public ObservableCollection<ISeries> PieGraph { get; set; } = new ObservableCollection<ISeries>();
        public ObservableCollection<ISeries> LineGraph { get; set; } = new ObservableCollection<ISeries>();

        public ICommand LoadedCommand { get; }

        private ISeries<int> CreateBarSeries(string name, ObservableCollection<int> values, LinearGradientPaint Color)
        {
            var bar = new ColumnSeries<int>
            {
                Name = name,
                Values = values,
                Stroke = null,
                Fill = Color,
                DataLabelsSize = 1
            };
            return bar;
        }


        private int _unpaid;

        public int Unpaid
        {
            get { return _unpaid; }
            set { _unpaid = value; OnPropertyChanged(nameof(Unpaid)); }
        }

        private int _paid;

        public int Paid
        {
            get { return _paid; }
            set { _paid = value; OnPropertyChanged(nameof(Paid)); }
        }

        private int _preSchool = 0;
        public int PreSchool
        {
            get { return _preSchool; }
            set
            {
                if (_preSchool == value)
                {
                    return;
                }
                _preSchool = value;
                OnPropertyChanged(nameof(PreSchool));
            }
        }

        private int _elementary = 0;
        public int Elementary
        {
            get { return _elementary; }
            set
            {
                if (_elementary == value)
                {
                    return;
                }
                _elementary = value;
                OnPropertyChanged(nameof(Elementary));
            }
        }

        private int _juniorHigh = 0;
        public int JuniorHigh
        {
            get { return _juniorHigh; }
            set
            {
                if (_juniorHigh == value)
                {
                    return;
                }
                _juniorHigh = value;
                OnPropertyChanged(nameof(JuniorHigh));
            }
        }

        private int _seniorHigh = 0;
        public int SeniorHigh
        {
            get { return _seniorHigh; }
            set
            {
                if (_seniorHigh == value)
                {
                    return;
                }
                _seniorHigh = value;
                OnPropertyChanged(nameof(SeniorHigh));
            }
        }

        private static double pushout = 2.5;
        private ISeries<int> CreatePieSeries(string name, int[] values, DataCategory category)
        {
            RadialGradientPaint? Color = category switch
            {
                DataCategory.UNPAID => new RadialGradientPaint(new SKColor(235, 113, 29), new SKColor(244, 130, 59)),
                DataCategory.PAID => new RadialGradientPaint(new SKColor(62, 112, 202), new SKColor(51, 101, 191)),
                _ => null,
            };
            PieSeries<int> pie = new()
            {
                Name = name,
                Values = values,
                Stroke = null,
                Fill = Color,
                Pushout = pushout,
                OuterRadiusOffset = 10
            };
            return pie;
        }

        private ISeries<int> CreateLineSeries(string name, ObservableCollection<int> values)
        {
            var line = new LineSeries<int>
            {
                Name = name,
                Values = values,
            };
            return line;
        }

        private enum DataCategory
        {
            PAID,
            UNPAID,
        }

        private class DataYear
        {
            public string? Year { get; set; }
            public int Count { get; set; }
        }


        private static readonly SKColor s_blue = new(25, 118, 210);
        private static readonly SKColor s_red = new(229, 57, 53);
        private static readonly SKColor s_yellow = new(198, 167, 0);

        public ObservableCollection<Axis> CrossX { get; set; } = new ObservableCollection<Axis>();
        public ObservableCollection<Axis> CrossY { get; set; } = new ObservableCollection<Axis>()
        {
            new Axis
            {
                CrosshairSnapEnabled = true // snapping is also supported
            }
        };

        public SolidColorPaint LegendTextPaint { get; set; } =
            new SolidColorPaint
            {
                Color = new SKColor(50, 50, 50),
                SKTypeface = SKTypeface.FromFamilyName("Courier New")
            };

        public SolidColorPaint LedgendBackgroundPaint { get; set; } =
            new SolidColorPaint(new SKColor(240, 240, 240));
    }
}
