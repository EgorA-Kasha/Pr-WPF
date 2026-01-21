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
    public partial class Tovari : Page
    {
        public Tovari()
        {
            InitializeComponent();
            Loaded += Tovari_Loaded;
        }

        private void Tovari_Loaded(object sender, RoutedEventArgs e)
        {
            var products = Database.GetProducts();
            ProductsListBox.ItemsSource = products;
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var product = (Product)button.DataContext;
            Cart.AddProduct(product);
            MessageBox.Show($"Товар \"{product.Name}\" добавлен в корзину!");
        }
    }
}
