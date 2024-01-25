using GFMS.Core;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using GFMS.Models;
using System.Net;
using GFMSLibrary;
using Microsoft.VisualBasic;
using System.Windows.Input;
using GFMS.Commands;
using System.Diagnostics;
using System.Drawing.Text;
using System.Windows.Documents;

namespace GFMS.ViewModels.RegistrarViewModels
{
    public class RegistrarDashboardViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public RegistrarDashboardViewModel()
        {
            LoadedCommand = new Command(async obj =>
            {
                await LoadDataAsync();
            });
            PreSchoolCommand = new Command(obj =>
            {
                BarGraph.Clear();
                var Years = YearList.OrderBy(y => y.Id).Select(y => y.Year).ToList();
                var dataYear = StudentList.Where(s => s.Registration != null && Years.Contains(s.Registration.Year) && s.Registration.Level == "PRE SCHOOL")
                                        .GroupBy(s => s.Registration!.Year)
                                        .Select(group => new DataYear { Year = group.Key, Count = group.Count() }).ToList();
                BarGraph.Add(CreateBarSeries("Pre School", new ObservableCollection<int>(dataYear.Select(y => y.Count)), DataCategory.PRESCHOOL));
            });
            ElementaryCommand = new Command(obj =>
            {
                BarGraph.Clear();
                var Years = YearList.OrderBy(y => y.Id).Select(y => y.Year).ToList();
                var dataYear = StudentList.Where(s => s.Registration != null && Years.Contains(s.Registration.Year) && s.Registration.Level == "ELEMENTARY")
                                        .GroupBy(s => s.Registration!.Year)
                                        .Select(group => new DataYear { Year = group.Key, Count = group.Count() }).ToList();
                BarGraph.Add(CreateBarSeries("Elementary", new ObservableCollection<int>(dataYear.Select(y => y.Count)), DataCategory.ELEMENTARY));
            });
            JuniorHighCommand = new Command(obj =>
            {
                BarGraph.Clear();
                var Years = YearList.OrderBy(y => y.Id).Select(y => y.Year).ToList();
                var dataYear = StudentList.Where(s => s.Registration != null && Years.Contains(s.Registration.Year) && s.Registration.Level == "JUNIOR HIGH SCHOOL")
                                        .GroupBy(s => s.Registration!.Year)
                                        .Select(group => new DataYear { Year = group.Key, Count = group.Count() }).ToList();
                BarGraph.Add(CreateBarSeries("Junior High School", new ObservableCollection<int>(dataYear.Select(y => y.Count)), DataCategory.JUNIOR));
            });
            SeniorHighCommand = new Command(obj =>
            {
                BarGraph.Clear();
                var Years = YearList.OrderBy(y => y.Id).Select(y => y.Year).ToList();
                var dataYear = StudentList.Where(s => s.Registration != null && Years.Contains(s.Registration.Year) && s.Registration.Level == "SENIOR HIGH SCHOOL")
                                        .GroupBy(s => s.Registration!.Year)
                                        .Select(group => new DataYear { Year = group.Key, Count = group.Count() }).ToList();
                BarGraph.Add(CreateBarSeries("Senior High School", new ObservableCollection<int>(dataYear.Select(y => y.Count)), DataCategory.SENIOR));
            });
        }

        public ObservableCollection<Axis> BarX { get; set; } = new ObservableCollection<Axis>();


        public List<Axis> BarY = new List<Axis>
        {
            new Axis
            {
                Labeler = Labelers.SixRepresentativeDigits
            }
        };

        private async Task LoadDataAsync()
        {
            StudentList.Clear();
            YearList.Clear();
            LineGraph.Clear();
            BarGraph.Clear();
            PieGraph.Clear();
            BarX.Clear();
            CrossX.Clear();
            var studentList = await Credentials.GetAllDataAsync<Student>("student");
            var registrationList = await Credentials.GetAllDataAsync<Registration>("registration");
            var previousSchoolList = await Credentials.GetAllDataAsync<PreviousSchool>("previous_school");

            foreach (var student in studentList)
            {
                // Find the requirements associated with the current student using the Student_ID
                var registeredStudent = new RegisteredStudent
                {
                    Student = student,
                };
                registeredStudent.Registration = registrationList.Where(r => Convert.ToInt32(r.Student_Id) == student.id).ToList().FirstOrDefault();
                registeredStudent.PreviousSchool = previousSchoolList.Where(r => Convert.ToInt32(r.student_id) == student.id).ToList().FirstOrDefault();
                StudentList.Add(registeredStudent);
            }

            YearList = await Credentials.GetAllDataAsync<SchoolYear>("school_year");
            var Years = YearList.OrderBy(y => y.Id).Select(y => y.Year).ToList();

            PreSchool = StudentList.Where(s => s.Registration!.Level!.ToUpper() == "PRE SCHOOL").ToList().Count();
            Elementary = StudentList.Where(s => s.Registration!.Level!.ToUpper() == "ELEMENTARY").ToList().Count();
            JuniorHigh = StudentList.Where(s => s.Registration!.Level!.ToUpper() == "JUNIOR HIGH SCHOOL").ToList().Count();
            SeniorHigh = StudentList.Where(s => s.Registration!.Level!.ToUpper() == "SENIOR HIGH SCHOOL").ToList().Count();
            PieGraph.Add(CreatePieSeries("Pre School", new[] { PreSchool }, DataCategory.PRESCHOOL));
            PieGraph.Add(CreatePieSeries("Elementary", new[] { Elementary }, DataCategory.ELEMENTARY));
            PieGraph.Add(CreatePieSeries("Junior High School", new[] { JuniorHigh }, DataCategory.JUNIOR));
            PieGraph.Add(CreatePieSeries("Senior High School", new[] { SeniorHigh }, DataCategory.SENIOR));
            // BarGraph Initial
            BarX.Add(new Axis { Labels = Years!, Name = "School Year", TextSize = 9 });
            CrossX.Add(new Axis
            {
                Labels = Years!
            });


            List<DataYear> dataYear = StudentList.Where(s => s.Registration != null && Years.Contains(s.Registration.Year) && s.Registration.Level == "PRE SCHOOL")
                                      .GroupBy(s => s.Registration!.Year)
                                      .Select(group => new DataYear { Year = group.Key, Count = group.Count() }).ToList();

            List<DataYear> TotalStudentEveryYear = Years.Select(year => new DataYear { Year = year, Count = StudentList.Count(s => s.Registration != null && s.Registration.Year == year) }).ToList();

            BarGraph.Add(CreateBarSeries("Senior High School", new ObservableCollection<int>(dataYear.Select(y => y.Count)), DataCategory.PRESCHOOL));
            LineGraph.Add(CreateLineSeries(new ObservableCollection<int>(TotalStudentEveryYear.Select(y => y.Count))));
        }

        public ICommand PreSchoolCommand { get; }
        public ICommand ElementaryCommand { get; }
        public ICommand JuniorHighCommand { get; }
        public ICommand SeniorHighCommand { get; }
        public ICommand LoadedCommand { get; }

        public ObservableCollection<ISeries> BarGraph { get; set; } = new ObservableCollection<ISeries>();
        public ObservableCollection<ISeries> PieGraph { get; set; } = new ObservableCollection<ISeries>();
        public ObservableCollection<ISeries> LineGraph { get; set; } = new ObservableCollection<ISeries>();

        private ISeries<int> CreateLineSeries(ObservableCollection<int> values)
        {
            var line = new LineSeries<int>
            {
                Name = "Total Student",
                Values = values,
            };
            return line;
        }

        private ISeries<int> CreateBarSeries(string name, ObservableCollection<int> values, DataCategory category)
        {
            LinearGradientPaint? Color = category switch
            {
                DataCategory.PRESCHOOL => new LinearGradientPaint(new[] { new SKColor(255, 103, 115), new SKColor(255, 140, 113) }),
                DataCategory.ELEMENTARY => new LinearGradientPaint(new[] { new SKColor(111, 104, 210), new SKColor(158, 94, 168) }),
                DataCategory.JUNIOR => new LinearGradientPaint(new[] { new SKColor(15, 132, 185), new SKColor(115, 202, 219) }),
                DataCategory.SENIOR => new LinearGradientPaint(new[] { new SKColor(26, 191, 50), new SKColor(124, 225, 149) }),
                _ => null,
            }; ;
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

        private static double pushout = 2.5;
        private ISeries<int> CreatePieSeries(string name, int[] values, DataCategory category)
        {
            RadialGradientPaint? Color = category switch
            {
                DataCategory.PRESCHOOL => new RadialGradientPaint(new SKColor(255, 103, 115), new SKColor(255, 140, 113)),
                DataCategory.ELEMENTARY => new RadialGradientPaint(new SKColor(111, 104, 210), new SKColor(158, 94, 168)),
                DataCategory.JUNIOR => new RadialGradientPaint(new SKColor(15, 132, 185), new SKColor(115, 202, 219)),
                DataCategory.SENIOR => new RadialGradientPaint(new SKColor(26, 191, 50), new SKColor(124, 225, 149)),
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

        public ObservableCollection<Axis> CrossX { get; set; } = new ObservableCollection<Axis>();
        public ObservableCollection<Axis> CrossY { get; set; } = new ObservableCollection<Axis>()
        {
            new Axis
            {
                CrosshairSnapEnabled = true // snapping is also supported
            }
        };

        private enum DataCategory
        {
            PRESCHOOL,
            ELEMENTARY,
            JUNIOR,
            SENIOR
        }

        public List<RegisteredStudent> StudentList { get; set; } = new List<RegisteredStudent>();
        public List<SchoolYear> YearList { get; set; } = new List<SchoolYear>();

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

        private class DataYear
        {
            public string? Year { get; set; }
            public int Count { get; set; }
        }

    }
}
