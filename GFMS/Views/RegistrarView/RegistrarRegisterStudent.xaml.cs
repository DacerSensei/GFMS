﻿using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GFMS.Views.RegistrarView
{
    /// <summary>
    /// Interaction logic for RegistrarRegisterStudents.xaml
    /// </summary>
    public partial class RegistrarRegisterStudent : UserControl
    {
        public RegistrarRegisterStudent()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
