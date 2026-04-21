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
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        public AccountPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            GridBookings.ItemsSource = App.db.Booking
                .Where(b => b.ClientID == App.CurrentUser.ID)
                .ToList()
                .Select(b => new
                {
                    ServiceName = App.db.ServiceType.FirstOrDefault(s => s.ID == b.ServiceTypeID)?.Name,
                    MasterName = App.db.User.FirstOrDefault(u => u.ID == b.MasterID)?.FullName,
                    b.DateTime,
                    b.Status
                }).ToList();

            GridOrders.ItemsSource = App.db.Order
                .Where(o => o.ClientID == App.CurrentUser.ID)
                .ToList();
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
