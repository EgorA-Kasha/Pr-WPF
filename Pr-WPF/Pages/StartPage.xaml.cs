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
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        private MainWindow _parent;

        public StartPage(MainWindow parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            _parent.NavigateTo(new GamePage(_parent));
        }
    }
}
