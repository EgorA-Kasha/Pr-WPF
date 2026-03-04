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
using Pr_WPF.Pages;

namespace Pr_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new BuildPC());
        }

        private void btnBuilder_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BuildPC());
        }

        private void btnSaved_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CompletedBuilds());
        }
    }
}

