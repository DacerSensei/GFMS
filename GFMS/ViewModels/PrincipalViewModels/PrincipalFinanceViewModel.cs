using GFMS.Core;
using GFMS.Models;
using GFMSLibrary;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.ViewModels.PrincipalViewModels
{
    public class PrincipalFinanceViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public PrincipalFinanceViewModel()
        {
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            UnpaidList.Clear();
            PaidList.Clear();
            var studentList = await Credentials.GetAllDataAsync<Student>("student");
            var registrationList = await Credentials.GetAllDataAsync<Registration>("registration");
            var accountingList = await Credentials.GetAllDataAsync<Accounting>("accounting");

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

            Unpaid = StudentList.Where(s => s.Status == "Unpaid").ToList().Count();
            Paid = StudentList.Where(s => s.Status == "Paid").ToList().Count();
            // BarGraph Initial
            //BarX.Add(new Axis { Labels = Years!, Name = "School Year", TextSize = 9 });
            //CrossX.Add(new Axis
            //{
            //    Labels = Years!
            //});

            //BarGraph.Add(CreateBarSeries("Senior High School", new ObservableCollection<int>(dataYear.Select(y => y.Count)), DataCategory.PRESCHOOL));
        }

        public ObservableCollection<StudentAccounting> UnpaidList { get; set; } = new ObservableCollection<StudentAccounting>();
        public ObservableCollection<StudentAccounting> PaidList { get; set; } = new ObservableCollection<StudentAccounting>();
        public ObservableCollection<StudentAccounting> StudentList { get; set; } = new ObservableCollection<StudentAccounting>();

        public ObservableCollection<ISeries> BarGraph { get; set; } = new ObservableCollection<ISeries>();

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
    }
}
