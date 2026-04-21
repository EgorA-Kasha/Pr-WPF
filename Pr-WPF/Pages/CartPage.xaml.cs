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
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
            PickerDate.DisplayDateStart = DateTime.Now;
            PickerDate.DisplayDateEnd = DateTime.Now.AddDays(7);
            UpdateCart();
        }

        private void UpdateCart()
        {
            ListCart.ItemsSource = null;
            ListCart.ItemsSource = App.Cart.ToList();
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            var p = (sender as Button).Tag as Product;
            if (App.Cart[p] > 1) App.Cart[p]--;
            UpdateCart();
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            var p = (sender as Button).Tag as Product;
            App.Cart[p]++;
            UpdateCart();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var p = (sender as Button).Tag as Product;
            App.Cart.Remove(p);
            UpdateCart();
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            if (App.Cart.Count == 0) return;
            if (PickerDate.SelectedDate == null || ComboPayment.SelectedItem == null)
            {
                MessageBox.Show("Заполните данные для заказа.");
                return;
            }

            Order o = new Order
            {
                ClientID = App.CurrentUser.ID,
                OrderDate = DateTime.Now.ToString("dd.MM.yyyy"),
                ReceiveDate = PickerDate.SelectedDate.Value.ToString("dd.MM.yyyy"),
                PaymentMethod = (ComboPayment.SelectedItem as ComboBoxItem).Content.ToString(),
                Status = "Активен"
            };
            App.db.Order.Add(o);
            App.db.SaveChanges();

            foreach (var item in App.Cart)
            {
                App.db.OrderProduct.Add(new OrderProduct
                {
                    OrderID = o.ID,
                    ProductID = item.Key.ID,
                    Quantity = item.Value
                });
            }
            App.db.SaveChanges();
            App.Cart.Clear();
            MessageBox.Show("Заказ оформлен!");
            NavigationService.GoBack();
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
