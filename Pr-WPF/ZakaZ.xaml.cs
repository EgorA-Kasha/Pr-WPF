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
    public partial class ZakaZ : Page
    {
        public ZakaZ()
        {
            InitializeComponent();
            Loaded += ZakaZ_Loaded;
        }

        private void ZakaZ_Loaded(object sender, RoutedEventArgs e)
        {
            OrderSummaryText.Text = GetOrderSummary();
        }

        private string GetOrderSummary()
        {
            var summary = "Товары в заказе:\n";
            foreach (var item in Cart.Items)
            {
                summary += $"{item.Product.Name} x{item.Quantity} - {item.Product.Price * item.Quantity:C}\n";
            }
            summary += $"\nОбщая сумма: {Cart.GetTotalPrice():C}";
            return summary;
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            try
            {
                int orderId = Database.SaveOrder(txtFullName.Text, txtEmail.Text, txtAddress.Text);
                Database.SaveOrderItems(orderId);

                MessageBox.Show($"Заказ №{orderId} успешно оформлен!");
                Cart.Clear();

                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.BtnProduct_Click(null, null);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заказа: {ex.Message}");
            }
        }
    }
}
