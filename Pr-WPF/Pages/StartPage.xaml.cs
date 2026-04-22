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
        public StartPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (App.CurrentUser != null)
            {
                BtnAuth.Visibility = Visibility.Collapsed;
                BtnAccount.Visibility = Visibility.Visible;
                BtnLogout.Visibility = Visibility.Visible;
            }
            else
            {
                BtnAuth.Visibility = Visibility.Visible;
                BtnAccount.Visibility = Visibility.Collapsed;
                BtnLogout.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadData()
        {
            ListServices.ItemsSource = App.db.ServiceType.ToList();
        }

        private void ListServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListServices.SelectedItem is ServiceType service)
            {
                ListMasters.ItemsSource = App.db.MasterService
                    .Where(ms => ms.ServiceTypeID == service.ID)
                    .Select(ms => ms.User)
                    .ToList();
                PanelBooking.Visibility = Visibility.Collapsed;
            }
        }

        private void ListMasters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListMasters.SelectedItem is User master && ListServices.SelectedItem is ServiceType service)
            {
                PanelBooking.Visibility = Visibility.Visible;
                TxtSelectedService.Text = "Услуга: " + service.Name;
                TxtSelectedMaster.Text = "Мастер: " + master.FullName;
            }
        }

        private void Book_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser == null || App.CurrentUser.RoleID != 1)
            {
                MessageBox.Show("Только авторизованные клиенты могут записываться на сеанс.");
                return;
            }

            if (PickerDate.SelectedDate == null || ComboTime.SelectedItem == null || ComboPayment.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля для записи.");
                return;
            }

            string targetDateTime = PickerDate.SelectedDate.Value.ToString("dd.MM.yyyy") + " " + (ComboTime.SelectedItem as ComboBoxItem).Content.ToString();
            int targetMasterID = (ListMasters.SelectedItem as User).ID;

            bool isTimeTaken = App.db.Booking.Any(b => b.MasterID == targetMasterID && b.DateTime == targetDateTime && b.Status == "Активна");

            if (isTimeTaken)
            {
                MessageBox.Show("Это время у данного мастера уже занято. Пожалуйста, выберите другое время.");
                return;
            }

            if (MessageBox.Show("Подтверждаете запись?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Booking b = new Booking
                {
                    ClientID = App.CurrentUser.ID,
                    MasterID = targetMasterID,
                    ServiceTypeID = (ListServices.SelectedItem as ServiceType).ID,
                    DateTime = targetDateTime,
                    PaymentMethod = (ComboPayment.SelectedItem as ComboBoxItem).Content.ToString(),
                    Comment = TxtComment.Text,
                    Status = "Активна"
                };
                App.db.Booking.Add(b);
                App.db.SaveChanges();
                MessageBox.Show("Вы успешно записаны!");
                PanelBooking.Visibility = Visibility.Collapsed;
            }
        }

        private void Auth_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new AuthPage());

        private void Products_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ProductsPage());

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            UpdateUI();
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            switch (App.CurrentUser.RoleID)
            {
                case 1:
                    NavigationService.Navigate(new AccountPage());
                    break;
                case 2:
                    NavigationService.Navigate(new MasterPage());
                    break;
                case 3:
                    NavigationService.Navigate(new ManagerPage());
                    break;
                case 4:
                    NavigationService.Navigate(new AdminPage());
                    break;
            }
        }
    }
}
