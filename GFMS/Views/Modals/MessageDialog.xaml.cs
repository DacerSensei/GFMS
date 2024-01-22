using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GFMS.Views.Modals
{
    /// <summary>
    /// Interaction logic for RequirementDialog.xaml
    /// </summary>
    public partial class MessageDialog : UserControl
    {
        public MessageDialog(string title, string message, bool isChecked = false)
        {
            InitializeComponent();
            if (isChecked)
            {
                MyIcon.Width = 25;
                MyIcon.Height = 25;
            }
            else
            {
                MyIcon.Width = 0;
                MyIcon.Height = 0;
            }
            Title.Text = title;
            Message.Text = message;
        }

        private void OkButton(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(true, null);
        }

    }
}
