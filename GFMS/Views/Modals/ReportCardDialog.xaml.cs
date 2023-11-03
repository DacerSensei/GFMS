using GFMS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GFMS.Views.Modals
{
    /// <summary>
    /// Interaction logic for ReportCardDialog.xaml
    /// </summary>
    public partial class ReportCardDialog : Window
    {
        public ObservableCollection<Subject> SubjectList { get; set; } = new ObservableCollection<Subject>();

        public ReportCardDialog()
        {
            InitializeComponent();
            SubjectList.Add(new Subject { SubjectName = "P.E", FirstGrading = 50, SecondGrading = 60, ThirdGrading = 70, FourthGrading = 100 });
            SubjectList.Add(new Subject { SubjectName = "Sample1", FirstGrading = 50, SecondGrading = 60, ThirdGrading = 70, FourthGrading = 100 });
            SubjectList.Add(new Subject { SubjectName = "Sample2", FirstGrading = 50, SecondGrading = 60, ThirdGrading = 70, FourthGrading = 100 });
            SubjectList.Add(new Subject { SubjectName = "Sample3", FirstGrading = 50, SecondGrading = 60, ThirdGrading = 70, FourthGrading = 100 });
            SubjectList.Add(new Subject { SubjectName = "Sample4", FirstGrading = 50, SecondGrading = 60, ThirdGrading = 70, FourthGrading = 100 });

            DataContext = this;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
