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
    /// Логика взаимодействия для MasterPage.xaml
    /// </summary>
    public partial class MasterPage : Page
    {
        public MasterPage()
        {
            InitializeComponent();
            LoadBookings();
            LoadServices();
        }

        private void LoadBookings()
        {
            GridBookings.ItemsSource = App.db.Booking
                .Where(b => b.MasterID == App.CurrentUser.ID)
                .ToList()
                .Select(b => new
                {
                    OriginalBooking = b,
                    ServiceName = App.db.ServiceType.FirstOrDefault(s => s.ID == b.ServiceTypeID)?.Name
                }).ToList();
        }

        private void LoadServices()
        {
            var allServices = App.db.ServiceType.ToList();
            var myServices = App.db.MasterService.Where(ms => ms.MasterID == App.CurrentUser.ID).Select(ms => ms.ServiceTypeID).ToList();

            var displayList = allServices.Select(s => new ServiceItem
            {
                ID = s.ID,
                Name = s.Name,
                IsSelected = myServices.Contains(s.ID)
            }).ToList();

            ListServices.ItemsSource = displayList;
        }

        private void SaveServices_Click(object sender, RoutedEventArgs e)
        {
            var oldRecords = App.db.MasterService.Where(ms => ms.MasterID == App.CurrentUser.ID).ToList();
            App.db.MasterService.RemoveRange(oldRecords);
            App.db.SaveChanges();

            var currentList = ListServices.ItemsSource as System.Collections.Generic.List<ServiceItem>;
            foreach (var item in currentList)
            {
                if (item.IsSelected)
                {
                    App.db.MasterService.Add(new MasterService
                    {
                        MasterID = App.CurrentUser.ID,
                        ServiceTypeID = item.ID
                    });
                }
            }
            App.db.SaveChanges();
            MessageBox.Show("Список услуг обновлен.");
        }

        private void GridBookings_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (GridBookings.SelectedItem != null)
            {
                dynamic selected = GridBookings.SelectedItem;
                Booking b = selected.OriginalBooking;
                NavigationService.Navigate(new MasterBookingDetailPage(b));
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }

    public class ServiceItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
