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
            ViewCommand = new Command(async obj =>
            {
                Student? student = obj as Student;
                if (student != null)
                {
                    List<Requirement> list = await Credentials.GetAllDataAsync<Requirement, Where>("student_requirements", new Where { student_id = student.id });
                    RequirementListDialog Dialog = new RequirementListDialog(list);
                    Dialog.ShowDialog();
                }
            });
        }

        private async void LoadAll()
        {
            var list = await Credentials.GetAllDataAsync<Student>("student");
            foreach (var student in list )
            {
                StudentList.Add(student);
            }
        }

        public ObservableCollection<Student> StudentList { get; set; } = new ObservableCollection<Student>();

        public ICommand ViewCommand { get; }

        private class Where
        {
            public int student_id { get; set; }
        }
    }
}
