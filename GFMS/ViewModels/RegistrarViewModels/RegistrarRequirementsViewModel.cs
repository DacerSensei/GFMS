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
            LoadAll();
            ViewCommand = new Command(obj =>
            {
                StudentRequirements? student = obj as StudentRequirements;
                if (student != null)
                {
                    RequirementListDialog Dialog = new RequirementListDialog(student.Requirement!, student.Student!.id.ToString());
                    if(Dialog.ShowDialog() == true)
                    {
                        LoadAll();
                    }
                }
            });
        }

        private async void LoadAll()
        {
            StudentList.Clear();
            var studentList = await Credentials.GetAllDataAsync<Student>("student");
            var requirementList = await Credentials.GetAllDataAsync<Requirement>("student_requirements");

            foreach (var student in studentList)
            {
                // Find the requirements associated with the current student using the Student_ID
                var studentRequirements = new StudentRequirements
                {
                    Student = student,
                };

                var requirements = requirementList.Where(r => r.Student_ID == student.id).ToList();
                studentRequirements.Requirement = requirements;
                if (requirements.Count == 9)
                {
                    studentRequirements.Status = "Completed";
                    studentRequirements.StatusColor = "#3dc03c";
                }
                else
                {
                    studentRequirements.Status = $"Missing {9 - requirements.Count}";
                    studentRequirements.StatusColor = "#fe3839";
                }

                // Add the StudentRequirements instance to the ObservableCollection
                StudentList.Add(studentRequirements);
            }
        }

        public ObservableCollection<StudentRequirements> StudentList { get; set; } = new ObservableCollection<StudentRequirements>();

        public ICommand ViewCommand { get; }
    }
}
