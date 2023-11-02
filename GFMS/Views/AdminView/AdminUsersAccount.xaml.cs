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

namespace GFMS.Views.AdminView
{
    /// <summary>
    /// Interaction logic for AdminUsersAccount.xaml
    /// </summary>
    public partial class AdminUsersAccount : UserControl
    {
        public AdminUsersAccount()
        {
            InitializeComponent();
        }

        private StackPanel? PreviousHighlight = null;
        private void DataHighlight(object sender, MouseButtonEventArgs e)
        {
            if (PreviousHighlight == null)
            {
                PreviousHighlight = sender as StackPanel;
            }
            else
            {
                PreviousHighlight.Background = new SolidColorBrush(Colors.White);
                PreviousHighlight = sender as StackPanel;
            }
            var ObjectSender = (StackPanel)sender;
            ObjectSender.Background = new SolidColorBrush(Color.FromRgb(173, 216, 255));
        }
    }
}
