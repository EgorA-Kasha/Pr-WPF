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
    /// Логика взаимодействия для ManagerPage.xaml
    /// </summary>
    public partial class ManagerPage : Page
    {
        public ManagerPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            GridBookings.ItemsSource = App.db.Booking.ToList();
            GridOrders.ItemsSource = App.db.Order.ToList();
            GridProducts.ItemsSource = App.db.Product.ToList();
            GridServices.ItemsSource = App.db.ServiceType.ToList();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            App.db.SaveChanges();
            MessageBox.Show("Данные сохранены");
            LoadData();
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
