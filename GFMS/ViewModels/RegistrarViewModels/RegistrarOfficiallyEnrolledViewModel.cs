using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views.Modals;
using GFMSLibrary;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels.RegistrarViewModels
{
    public class RegistrarOfficiallyEnrolledViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public RegistrarOfficiallyEnrolledViewModel()
        {
            LoadedCommand = new Command(async obj =>
            {
                await LoadAll();
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
                StudentList.Add(registeredStudent);
            }
        }

        public ObservableCollection<RegisteredStudent> StudentList { get; set; } = new ObservableCollection<RegisteredStudent>();

        public ICommand EditCommand { get; }
        public ICommand LoadedCommand { get; }
    }
}
