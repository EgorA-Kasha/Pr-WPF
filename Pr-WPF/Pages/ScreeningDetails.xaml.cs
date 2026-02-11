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
    /// Логика взаимодействия для ScreeningDetails.xaml
    /// </summary>
    public partial class ScreeningDetails : Page
    {
        public ScreeningDetails()
        {
            InitializeComponent();
            update();
        }

        private void cb_hide_taken_Checked(object sender, RoutedEventArgs e)
        {
            lb_seats.UnselectAll();
            update();
        }
        private void update()
        {
            var taken = from x in Global.db.Ticket where x.screening == Global.ScreeningSelected.sc.id select x.seat;
            var total = Global.ScreeningSelected.room.seats;
            var free = new List<int>();
            for (int nr = 1; nr <= total; nr++)
                if (!taken.Contains(nr) || cb_hide_taken.IsChecked == false)
                    free.Add(nr);
            lb_seats.ItemsSource = free;
        }

        private void lb_seats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_hide_taken.IsChecked == true && lb_seats.SelectedValue != null)
            {
                Global.SeatSelected = lb_seats.SelectedValue as int?;
                MainWindow.main_window.navigate_to(new Ordering());
            }
        }
    }
}
