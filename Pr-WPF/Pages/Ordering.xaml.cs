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
    /// Логика взаимодействия для Ordering.xaml
    /// </summary>
    public partial class Ordering : Page
    {
        public Ordering()
        {
            InitializeComponent();
            lb_ticket.Items.Add(new
            {
                movie = Global.MovieSelected,
                screening = Global.ScreeningSelected.sc,
                seat = Global.SeatSelected,
                price = Global.price
            });
        }

        private void button_order_Click(object sender, RoutedEventArgs e)
        {
            Global.db.Ticket.Add(new Ticket
            {
                user = Global.LoggedInAs.Value,
                screening = Global.ScreeningSelected.sc.id,
                seat = Global.SeatSelected.Value,
                price = Global.price
            });
            Global.db.SaveChanges();
            MainWindow.main_window.navigate_to(new HomePage());
        }
    }
}
