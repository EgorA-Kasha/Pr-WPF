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
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Page
    {
        public Reg()
        {
            InitializeComponent();
        }

        private void button_log_reg_Click(object sender, RoutedEventArgs e)
        {
            var users = from x in Global.db.User where x.login == tb_login.Text select x;
            if (users.Count() != 0)
                MessageBox.Show("Такой юзер уже есть");
            var u = new User();
            u.login = tb_login.Text;
            u.passwd = tb_passwd.Text;
            Global.db.User.Add(u);
            Global.db.SaveChanges();
            Global.LoggedInAs = (from x in Global.db.User where x.login == tb_login.Text select x.id).First();
            MainWindow.main_window.navigate_to(new UserPage());
        }
    }
}
