using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views;
using GFMS.Views.Modals;
using GFMSLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels.TeacherViewModels
{
    public class TeacherReportCardViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();

        public TeacherReportCardViewModel()
        {
            LoadAll();
            ViewCommand = new Command(async obj =>
            {
                StudentReport? student = obj as StudentReport;
                if (student != null)
                {
                    Users principal = await Credentials.GetByAnonymousAsync<Users>("usertype", "PRINCIPAL", "users");
                    ReportCardDialog window = new ReportCardDialog(student, MainWindow.User!, principal);
                    if (window.ShowDialog() == true)
                    {
                        LoadAll();
                    }
                }
            });

            EditCommand = new Command(async obj =>
            {
                StudentReport? student = obj as StudentReport;
                if (student != null)
                {
                    Users principal = await Credentials.GetByAnonymousAsync<Users>("usertype", "PRINCIPAL", "users");
                    ReportCardDialog window = new ReportCardDialog(student, MainWindow.User!, principal, true);
                    if(window.ShowDialog() == true)
                    {
                        LoadAll();
                    }
                }
            });
        }

        private async void LoadAll()
        {
            if (MainWindow.Teacher == null)
            {
                return;
            }
            StudentList.Clear();
            Where where = new Where()
            {
                Grade = MainWindow.Teacher.Grade,
                Year = "2024-2025"

            };
            var studentList = await Credentials.GetAllDataAsync<Student>("student");
            var registrationList = await Credentials.GetAllDataAsync<Registration, Where>("registration", where);
            var studentGradeList = await Credentials.GetAllDataAsync<ReportCard>("studentgrades");

            foreach (var student in registrationList)
            {
                // Find the requirements associated with the current student using the Student_ID
                var studentReport = new StudentReport
                {
                    Registration = student,
                };
                studentReport.Student = studentList.Where(r => r.id == Convert.ToInt16(student.Student_Id)).ToList().FirstOrDefault();
                studentReport.ReportCard = studentGradeList.Where(r => Convert.ToInt32(r.Registration_Id) == student.Id).ToList().FirstOrDefault();
                StudentList.Add(studentReport);
            }
        }

        public ObservableCollection<StudentReport> StudentList { get; set; } = new ObservableCollection<StudentReport>();

        public ICommand ViewCommand { get; }
        public ICommand EditCommand { get; }

        private class Where
        {
            public string? Grade { get; set; }
            public string? Year { get; set; }
        }
    }
}
