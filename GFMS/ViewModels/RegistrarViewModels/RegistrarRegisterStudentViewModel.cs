using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using GFMS.ViewModels.Modals;
using GFMS.Views.Modals;
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
    public class RegistrarRegisterStudentViewModel : ViewModelBase
    {
        public RegistrarRegisterStudentViewModel()
        {
            RequirementList = new ObservableCollection<Requirement>();
            DeleteCommand = new Command(obj =>
            {
                Requirement? requirement = obj as Requirement;
                if (requirement != null)
                {
                    RequirementList.Remove(requirement);
                }
            });
            AddCommand = new DialogCommand(async obj =>
            {
                RequirementDialog Dialog = new RequirementDialog()
                {
                    DataContext = new RequirementDialogViewModel(RequirementList)
                };
                var result = await DialogHost.Show(Dialog, "RootDialog");

                Debug.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
            });
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        => Debug.WriteLine("You can intercept the closing event, and cancel here.");

        private void ClosedEventHandler(object sender, DialogClosedEventArgs eventArgs)
            => Debug.WriteLine("You can intercept the closed event here (1).");

        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }

        public ObservableCollection<Requirement> RequirementList { get; set; }
    }
}
