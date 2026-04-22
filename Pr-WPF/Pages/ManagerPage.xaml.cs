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

            ComboServices.ItemsSource = App.db.ServiceType.ToList();
            ComboClients.ItemsSource = App.db.User.Where(u => u.RoleID == 1 && !u.IsFrozen).ToList();
        }

        private void ComboServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboServices.SelectedItem is ServiceType selectedService)
            {
                ComboMasters.ItemsSource = App.db.MasterService
                    .Where(ms => ms.ServiceTypeID == selectedService.ID)
                    .Select(ms => ms.User)
                    .ToList();
            }
        }

        private void AddBooking_Click(object sender, RoutedEventArgs e)
        {
            if (ComboClients.SelectedItem == null || ComboMasters.SelectedItem == null ||
                ComboServices.SelectedItem == null || PickerDate.SelectedDate == null ||
                ComboTime.SelectedItem == null || ComboPayment.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля для создания записи.");
                return;
            }

            string targetDateTime = PickerDate.SelectedDate.Value.ToString("dd.MM.yyyy") + " " + (ComboTime.SelectedItem as ComboBoxItem).Content.ToString();
            int targetMasterID = (ComboMasters.SelectedItem as User).ID;

            bool isTimeTaken = App.db.Booking.Any(b => b.MasterID == targetMasterID && b.DateTime == targetDateTime && b.Status == "Активна");

            if (isTimeTaken)
            {
                MessageBox.Show("Это время у данного мастера уже занято. Пожалуйста, выберите другое время.");
                return;
            }

            Booking newBooking = new Booking
            {
                ClientID = (ComboClients.SelectedItem as User).ID,
                MasterID = targetMasterID,
                ServiceTypeID = (ComboServices.SelectedItem as ServiceType).ID,
                DateTime = targetDateTime,
                PaymentMethod = (ComboPayment.SelectedItem as ComboBoxItem).Content.ToString(),
                Comment = TxtComment.Text ?? "",
                Status = "Активна"
            };

            App.db.Booking.Add(newBooking);
            App.db.SaveChanges();

            MessageBox.Show("Запись успешно создана!");
            LoadData();
        }

        private void GridBookings_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                foreach (var item in GridBookings.SelectedItems)
                {
                    if (item is Booking b)
                    {
                        App.db.Booking.Remove(b);
                    }
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            App.db.SaveChanges();
            MessageBox.Show("Данные сохранены.");
            LoadData();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            NavigationService.Navigate(new StartPage());
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
