using GFMS.Commands;
using GFMS.Core;
using GFMS.Models;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GFMS.ViewModels.Modals
{
    public class RequirementDialogViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel ErrorsViewModel;
        public RequirementDialogViewModel()
        {
            ErrorsViewModel = new ErrorsViewModel();
            ErrorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged!;
            RequirementTypes = new ObservableCollection<string>()
            {
                "Report Card","PSA","Good Moral Certificate","Form 137/SF 10",
                "Certificate of Completion/Diploma","NCAE","ESC Certificate and Subsidy Certificate",
                "ID Picture","Medical Record"
            };
            AddCommand = new DialogCommand(obj =>
            {
                ValidateRequirement();
                ValidateDocument(); 
                if (ErrorsViewModel.HasErrors)
                {
                    return;
                }
                DialogHost.CloseDialogCommand.Execute(new Requirement { Description = SelectedRequirement, Path = Document }, null);
            });
            CancelCommand = new DialogCommand(obj =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
            SelectDocumentCommand = new DialogCommand(obj =>
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Choose Document(*.jpg;*.png)|*.jpg;*png";
                if (fileDialog.ShowDialog() == true)
                {
                    Document = fileDialog.FileName;
                }
            });
        }
        public ObservableCollection<string> RequirementTypes { get; set; }

        private string _document;
        public string Document
        {
            get { return _document; }
            set
            {
                _document = value;
                ValidateDocument();
                OnPropertyChanged(nameof(Document));
            }
        }

        private string _selectedRequirement;
        public string SelectedRequirement
        {
            get { return _selectedRequirement; }
            set
            {
                _selectedRequirement = value;
                ValidateRequirement();
                OnPropertyChanged(nameof(SelectedRequirement));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SelectDocumentCommand { get; }

        private void ValidateRequirement()
        {
            if (string.IsNullOrWhiteSpace(SelectedRequirement))
            {
                ErrorsViewModel.AddError(nameof(SelectedRequirement), "Requirement cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(SelectedRequirement));
            }
        }

        private void ValidateDocument()
        {
            if (string.IsNullOrWhiteSpace(Document))
            {
                ErrorsViewModel.AddError(nameof(Document), "Document cannot be empty");
            }
            else
            {
                ErrorsViewModel.ClearErrors(nameof(Document));
            }
        }

        public bool CanCreate => !HasErrors;

        public bool HasErrors => ErrorsViewModel.HasErrors;

        public IEnumerable GetErrors(string? propertyName)
        {
            return ErrorsViewModel.GetErrors(propertyName);
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }
    }
}
