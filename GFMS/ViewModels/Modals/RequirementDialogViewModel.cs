using GFMS.Core;
using GFMS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.ViewModels.Modals
{
    public class RequirementDialogViewModel : ViewModelBase
    {
        private ObservableCollection<Requirement> requirementsList = new ObservableCollection<Requirement>();
        public RequirementDialogViewModel(ObservableCollection<Requirement> requirementsList)
        {

        }
    }
}
