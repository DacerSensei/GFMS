using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMS.ViewModels.FinanceViewModels
{
    public class FinanceReportViewModel : ViewModelBase
    {
        public FinanceReportViewModel()
        {
            LoadedCommand = new Command(obj =>
            {

            });
        }

        public ObservableCollection<Student> IncomeList { get; set; } = new ObservableCollection<Student>();

        public ICommand LoadedCommand { get; }
    }
}
