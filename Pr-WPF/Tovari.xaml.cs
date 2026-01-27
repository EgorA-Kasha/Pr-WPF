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
            ProductsListBox.SelectionChanged += ProductsListBox_SelectionChanged;
        }

        private void Tovari_Loaded(object sender, RoutedEventArgs e)
        {
            var products = Database.GetProducts();
            ProductsListBox.ItemsSource = products;
        }

        private void ProductsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddToCartButton.IsEnabled = ProductsListBox.SelectedItem != null;
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsListBox.SelectedItem as Product;
            if (selectedProduct != null)
            {
                Cart.AddProduct(selectedProduct);
                MessageBox.Show($"Товар \"{selectedProduct.Name}\" добавлен в корзину!");

                ProductsListBox.SelectedItem = null;
                AddToCartButton.IsEnabled = false;
            }
        }
    }
}
