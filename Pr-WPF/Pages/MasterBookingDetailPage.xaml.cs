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
    /// Логика взаимодействия для MasterBookingDetailPage.xaml
    /// </summary>
    public partial class MasterBookingDetailPage : Page
    {
        private Booking _currentBooking;

        public MasterBookingDetailPage(Booking booking)
        {
            InitializeComponent();
            _currentBooking = booking;
            LoadDetails();
        }

        private void LoadDetails()
        {
            var client = App.db.User.FirstOrDefault(u => u.ID == _currentBooking.ClientID);
            var service = App.db.ServiceType.FirstOrDefault(s => s.ID == _currentBooking.ServiceTypeID);

            TxtDateTime.Text = "Дата и время: " + _currentBooking.DateTime;
            TxtClientName.Text = "Клиент: " + client?.FullName;
            TxtClientPhone.Text = "Телефон: " + client?.Phone;
            TxtService.Text = "Услуга: " + service?.Name;
            TxtStatus.Text = "Статус: " + _currentBooking.Status;
            TxtComment.Text = "Комментарий: " + _currentBooking.Comment;

            if (_currentBooking.Status == "Выполнена" || _currentBooking.Status == "Отменена")
            {
                BtnComplete.Visibility = Visibility.Collapsed;
            }
        }

        private void Complete_Click(object sender, RoutedEventArgs e)
        {
            var dbBooking = App.db.Booking.FirstOrDefault(b => b.ID == _currentBooking.ID);
            if (dbBooking != null)
            {
                dbBooking.Status = "Выполнена";
                App.db.SaveChanges();
                MessageBox.Show("Запись успешно завершена.");
                NavigationService.GoBack();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
