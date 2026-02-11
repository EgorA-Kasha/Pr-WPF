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
    /// Логика взаимодействия для Movies.xaml
    /// </summary>
    public partial class Movies : Page {
        public Movies() {
            InitializeComponent();
            b_next.Visibility = Global.LoggedInAs == null ? Visibility.Collapsed : Visibility.Visible;
            update();
        }
        public void update() {
            var l = (from x in Global.db.Movie where x.title.Contains(tb_filter.Text) select x).ToList();
            if (cb_sort_by_name.IsChecked ?? false)
                l.Sort((x1, x2) => x1.title.CompareTo(x2.title));
            else
                l.Sort((x1, x2) => -x1.rating.CompareTo(x2.rating));
            lb_movies.ItemsSource = l;
        }

        private void cb_sort_by_name_Checked(object sender, RoutedEventArgs e) {
            update();
        }

        private void tb_filter_TextChanged(object sender, TextChangedEventArgs e) {
            update();
        }

        private void button_next_Click(object sender, RoutedEventArgs e) {
            Global.MovieSelected = lb_movies.SelectedValue as Movie;
            MainWindow.main_window.navigate_to(new MovieDetails());
        }

        private void lb_movies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Global.MovieSelected = lb_movies.SelectedValue as Movie;
            //MainWindow.main_window.navigate_to(new MovieDetails());
        }
    }
}
