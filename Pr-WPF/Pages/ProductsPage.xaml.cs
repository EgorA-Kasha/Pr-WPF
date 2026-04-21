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
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            LoadFilters();
            UpdateList();
        }

        private void LoadFilters()
        {
            var types = App.db.ProductType.ToList();
            types.Insert(0, new ProductType { Name = "Все типы" });
            ComboType.ItemsSource = types;
            ComboType.SelectedIndex = 0;

            var manufs = App.db.Manufacturer.ToList();
            manufs.Insert(0, new Manufacturer { Name = "Все производители" });
            ComboManuf.ItemsSource = manufs;
            ComboManuf.SelectedIndex = 0;
        }

        private void UpdateList()
        {
            var query = App.db.Product.Where(p => !p.IsFrozen).ToList();

            if (!string.IsNullOrEmpty(TxtSearch.Text))
                query = query.Where(p => p.Name.ToLower().Contains(TxtSearch.Text.ToLower())).ToList();

            if (ComboType.SelectedIndex > 0)
                query = query.Where(p => p.ProductTypeID == (ComboType.SelectedItem as ProductType).ID).ToList();

            if (ComboManuf.SelectedIndex > 0)
                query = query.Where(p => p.ManufacturerID == (ComboManuf.SelectedItem as Manufacturer).ID).ToList();

            query = query.OrderByDescending(p => p.Rating).ToList();

            var displayList = query.Select(p => new
            {
                Prod = p,
                BgColor = p.Discount > 15 ? Brushes.LightGreen : Brushes.White
            }).ToList();

            ListProducts.ItemsSource = displayList;
        }

        private void Filter_Changed(object sender, RoutedEventArgs e) => UpdateList();

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser == null || App.CurrentUser.RoleID != 1)
            {
                MessageBox.Show("Авторизуйтесь как клиент для добавления в корзину.");
                return;
            }

            var prod = (sender as Button).Tag as Product;
            if (App.Cart.ContainsKey(prod)) App.Cart[prod]++;
            else App.Cart.Add(prod, 1);
            MessageBox.Show("Товар добавлен в корзину.");
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser != null && App.CurrentUser.RoleID == 1) NavigationService.Navigate(new CartPage());
            else MessageBox.Show("Корзина доступна только клиентам.");
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
