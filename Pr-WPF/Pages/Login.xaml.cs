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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button_log_reg_Click(object sender, RoutedEventArgs e)
        {
            var users = from x in Global.db.User where x.login == tb_login.Text select x;
            if (users.Count() == 0)
            {
                var u = new User();
                u.login = tb_login.Text;
                u.passwd = tb_passwd.Text;
                Global.db.User.Add(u);
                Global.db.SaveChanges();
                button_log_reg_Click(sender, e);
            }
            else
            {
                var u = users.First();
                if (u.passwd != tb_passwd.Text)
                {
                    MessageBox.Show("Неверный пароль!");
                    return;
                }
                else
                {
                    Global.LoggedInAs = u.id;
                    MainWindow.main_window.navigate_to(new UserPage());
                }
            }
        }
    }
}
