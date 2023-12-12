using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views.Modals;
using GFMSLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels.PrincipalViewModels
{
    public class PrincipalTeacherViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public PrincipalTeacherViewModel()
        {
            LoadAll();
            ViewCommand = new Command(async obj =>
            {
                StudentReport? student = obj as StudentReport;
                if (student != null)
                {
                    Users principal = await Credentials.GetByAnonymousAsync<Users>("usertype", "PRINCIPAL", "users");
                    UserTeacher findTeacher = await Credentials.GetByAnonymousAsync<UserTeacher>("grade", student.Registration!.Grade!, "teachers");
                    Users teacher = await Credentials.GetByIdAsync<Users>(findTeacher.User_Id.ToString(), "users");
                    ReportCardDialog window = new ReportCardDialog(student, teacher, principal, false, false);
                    if (window.ShowDialog() == true)
                    {
                        LoadAll();
                    }
                }
            });
        }

        private async void LoadAll()
        {
            StudentList.Clear();
            var studentListTask = Credentials.GetAllDataAsync<Student>("student");
            var registrationListTask = Credentials.GetAllDataAsync<Registration>("registration");
            var studentGradeListTask = Credentials.GetAllDataAsync<ReportCard>("studentgrades");
            var YearListTask = Credentials.GetAllDataAsync<SchoolYear>("school_year");

            await Task.WhenAll(studentListTask, registrationListTask, studentGradeListTask, YearListTask);

            var studentList = studentListTask.Result;
            var registrationList = registrationListTask.Result;
            var studentGradeList = studentGradeListTask.Result;
            var YearList = YearListTask.Result;
            var Years = YearList.OrderBy(y => y.Id).Select(y => y.Year).ToList();

            foreach (var student in registrationList)
            {
                // Find the requirements associated with the current student using the Student_ID
                var studentReport = new StudentReport
                {
                    Registration = student,
                };
                studentReport.Student = studentList.Where(r => r.id == Convert.ToInt16(student.Student_Id)).ToList().FirstOrDefault();
                studentReport.ReportCard = studentGradeList.Where(r => Convert.ToInt32(r.Registration_Id) == student.Id).ToList().FirstOrDefault();
                if (studentReport.Registration.Year == (Years.LastOrDefault() ?? "2023-2024"))
                {
                    StudentList.Add(studentReport);
                }
            }
        }

        public ObservableCollection<StudentReport> StudentList { get; set; } = new ObservableCollection<StudentReport>();

        public ICommand ViewCommand { get; }
    }
}
