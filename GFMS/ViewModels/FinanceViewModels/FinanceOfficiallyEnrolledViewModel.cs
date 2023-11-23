using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.Views;
using GFMS.Views.Modals;
using GFMSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace GFMS.ViewModels.FinanceViewModels
{
    public class FinanceOfficiallyEnrolledViewModel : ViewModelBase
    {
        private LoginCredentials Credentials = new LoginCredentials();
        public FinanceOfficiallyEnrolledViewModel()
        {
            LoadedCommand = new Command(async obj =>
            {
                await LoadAllAsync();
            });

            PayCommand = new Command(async obj =>
            {
                StudentAccounting? student = obj as StudentAccounting;
                if (student != null)
                {
                    RecieptPayment window = new RecieptPayment(student);
                    if (window.ShowDialog() == true)
                    {
                        await LoadAllAsync();
                    }
                }
            });
            HistoryCommand = new Command(async obj =>
            {
                StudentAccounting? student = obj as StudentAccounting;
                if (student != null)
                {
                    PaymentHistoryDialog window = new PaymentHistoryDialog(student);
                    if (window.ShowDialog() == true)
                    {
                        await LoadAllAsync();
                    }
                }
            });
        }

        private async Task LoadAllAsync()
        {
            StudentList.Clear();
            Task<List<Student>>? studentListTask = Credentials.GetAllDataAsync<Student>("student");
            Task<List<Registration>>? registrationListTask = Credentials.GetAllDataAsync<Registration>("registration");
            Task<List<Accounting>>? accountingListTask = Credentials.GetAllDataAsync<Accounting>("accounting");

            await Task.WhenAll(studentListTask, registrationListTask, accountingListTask);

            List<Student> studentList = studentListTask.Result;
            List<Registration> registrationList = registrationListTask.Result;
            List<Accounting> accountingList = accountingListTask.Result;

            foreach (var student in registrationList)
            {
                // Find the requirements associated with the current student using the Student_ID
                var studentAccounting = new StudentAccounting
                {
                    Registration = student,
                    Student = studentList.Where(s => s.id.ToString() == student.Student_Id).ToList().FirstOrDefault(),
                    PaymentList = accountingList.Where(a => a.Registration_Id == student.Id.ToString()).ToList(),
                    TuitionDetailsList = new List<TuitionDetails>()
                };
                foreach (var payment in studentAccounting.PaymentList)
                {
                    try
                    {
                        var result = JsonConvert.DeserializeObject<TuitionDetails>(payment.Payment ?? "");
                        if(result != null)
                        {
                            studentAccounting.TuitionDetailsList.Add(result);
                        }
                    }
                    catch (JsonReaderException ex)
                    {
                        Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                    }
                }
                StudentList.Add(studentAccounting);
            }
        }

        public ObservableCollection<StudentAccounting> StudentList { get; set; } = new ObservableCollection<StudentAccounting>();

        public ICommand PayCommand { get; }
        public ICommand HistoryCommand { get; }
        public ICommand LoadedCommand { get; }
    }
}
