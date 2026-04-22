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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var user = App.db.User.FirstOrDefault(u => u.Login == TxtLogin.Text && u.Password == TxtPassword.Password);
            if (user != null)
            {
                if (user.IsFrozen)
                {
                    MessageBox.Show("Ваш аккаунт заморожен.");
                    return;
                }

                App.CurrentUser = user;

                switch (user.RoleID)
                {
                    case 1:
                        NavigationService.GoBack();
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
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
