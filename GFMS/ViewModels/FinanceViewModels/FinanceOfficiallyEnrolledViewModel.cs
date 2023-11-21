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

namespace GFMS.ViewModels.FinanceViewModels
{
    public class FinanceOfficiallyEnrolledViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public FinanceOfficiallyEnrolledViewModel()
        {
            LoadedCommand = new Command(obj =>
            {

            });
            LoadAllAsync();
            PayCommand = new Command(async obj =>
            {
                StudentAccounting? student = obj as StudentAccounting;
                if (student != null)
                {
                    RecieptPayment window = new RecieptPayment(student);
                    if (window.ShowDialog() == true)
                    {
                        LoadAllAsync();
                    }
                }
            });
            HistoryCommand = new Command(async obj =>
            {
                StudentAccounting? student = obj as StudentAccounting;
                if (student != null)
                {
                    PaymentHistoryDialog window = new PaymentHistoryDialog();
                    if (window.ShowDialog() == true)
                    {
                        LoadAllAsync();
                    }
                }
            });
        }

        private async void LoadAllAsync()
        {
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
        }

        public ObservableCollection<StudentAccounting> StudentList { get; set; } = new ObservableCollection<StudentAccounting>();

        public ICommand PayCommand { get; }
        public ICommand HistoryCommand { get; }
        public ICommand LoadedCommand { get; }
    }
}
