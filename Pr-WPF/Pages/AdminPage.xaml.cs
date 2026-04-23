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

            ColRole.ItemsSource = App.db.Role.ToList();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool hasAdminFreezeAttempt = false;

            foreach (var user in App.db.User.Local)
            {
                if (user.RoleID == 4 && user.IsFrozen)
                {
                    user.IsFrozen = false;
                    hasAdminFreezeAttempt = true;
                }
            }

            if (hasAdminFreezeAttempt)
            {
                MessageBox.Show("Нельзя замораживать администраторов (включая себя)! Статусы администраторов были восстановлены.");
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
