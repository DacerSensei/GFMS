using GFMS.Commands;
using GFMS.Models;
using GFMS.ViewModels.Modals;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GFMS.Views.Modals
{
    /// <summary>
    /// Interaction logic for RequirementListDialog.xaml
    /// </summary>
    public partial class RequirementListDialog : Window
    {
        public ObservableCollection<Requirement> RequirementList { get; set; }
        public RequirementListDialog(List<Requirement> requirementList)
        {
            InitializeComponent();
            RequirementList = new ObservableCollection<Requirement>(requirementList);
            AddCommand = new Command(async obj =>
            {
                RequirementDialog Dialog = new RequirementDialog()
                {
                    DataContext = new RequirementDialogViewModel()
                };
                var result = await DialogHost.Show(Dialog, "SecondaryDialog");
                if (result != null)
                {
                    Requirement? requirement = result as Requirement;
                    if (requirement != null)
                    {
                        if (!RequirementList.Any(r => r.Description == requirement.Description))
                        {
                            RequirementList.Add(requirement);
                        }
                    }
                }
            });
            RemoveCommand = new Command(obj =>
            {
                Requirement? requirement = obj as Requirement;
                if (requirement != null)
                {
                    RequirementList.Remove(requirement);
                }
            }); 
            SaveCommand = new Command(async obj =>
            {
                var result = await DialogHost.Show(new AlertDialog("Notice", "Are you sure?"), "SecondaryDialog");
                if ((bool)result! == false)
                {
                    return;
                }
                Close();
            }); 
            CancelCommand = new Command(obj =>
            {
                Close();
            });
            DataContext = this;
        }

        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
    }
}
