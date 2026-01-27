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

namespace Pr_WPF
{
    public partial class Basket : Page
    {
        public Basket()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Cart.Items == null || Cart.Items.Count == 0)
            {
                CartGrid.Visibility = Visibility.Collapsed;
                TotalPriceText.Text = "Корзина пуста";
            }
            else
            {
                CartGrid.Visibility = Visibility.Visible;
                CartItemsControl.ItemsSource = Cart.Items;
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Items == null || Cart.Items.Count == 0)
            {
                MessageBox.Show("Корзина пуста!");
                return;
            }

            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.BtnOrder_Click(null, null);
        }
    }
}
