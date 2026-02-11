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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            if (Global.LoggedInAs == null)
            {
                tb_userdata.Text = "\n\n\n\n\n\n\n\nВойдите или зарегестрируйтесь, чтобы увидеть инфу";
            }
            else
            {
                tb_userdata.Text = "Вы "
                + (from x in Global.db.User where x.id == Global.LoggedInAs select x).First().login;
                lb_tickets.ItemsSource = (
                    from x in Global.db.User
                    where x.id == Global.LoggedInAs
                    join y in Global.db.Ticket on x.id equals y.user
                    join z in Global.db.Screening on y.screening equals z.id
                    join w in Global.db.Movie on z.movie equals w.id
                    select new { screening = z, movie = w }
                ).ToList();
            }
        }
    }
}
