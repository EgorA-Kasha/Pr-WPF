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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            App.db.User.ToList();
            GridUsers.ItemsSource = App.db.User.Local;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var self = App.db.User.Local.FirstOrDefault(u => u.ID == App.CurrentUser.ID);
            if (self != null && self.IsFrozen)
            {
                MessageBox.Show("Вы не можете заморозить сами себя!");
                self.IsFrozen = false;
                GridUsers.Items.Refresh();
                return;
            }

            App.db.SaveChanges();
            MessageBox.Show("Изменения сохранены.");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            NavigationService.Navigate(new StartPage());
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
