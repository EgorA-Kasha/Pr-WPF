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
        public static MainWindow main_window;
        public MainWindow() {
            InitializeComponent();
            main_window = this;
            navigate_to(null);
        }
        public void navigate_to(object page = null) {
            if (page != null) {
                main_window.MainFrame.Navigate(page);
            } else {
                main_window.MainFrame.Navigate(new HomePage());
            }
        }

    }
}
