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

namespace Pr_WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void Button_Click_Completed(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.navigate_to(new CompletedBuilds());
        }

        private void Button_Click_Build(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.navigate_to(new BuildPC());
        }
    }
}
