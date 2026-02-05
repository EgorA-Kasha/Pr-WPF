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
    /// Логика взаимодействия для MovieDetails.xaml
    /// </summary>
    public partial class MovieDetails : Page
    {
        public class T
        {
            public Screening sc { get; set; }
            public Room room { get; set; }
        }
        public MovieDetails()
        {
            InitializeComponent();
            lb_movie.Items.Add(Global.MovieSelected);
            lb_screenings.ItemsSource = (
                from y in Global.db.Screening
                join z in Global.db.Room on y.room equals z.id
                where y.movie == Global.MovieSelected.id
                select new T { sc = y, room = z }
            ).ToList();
        }

        private void b_next_next_Click(object sender, RoutedEventArgs e)
        {
            Global.ScreeningSelected = lb_screenings.SelectedValue as T;
            MainWindow.main_window.navigate_to(new ScreeningDetails());
        }
    }
}
