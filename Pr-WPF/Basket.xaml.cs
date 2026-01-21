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
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCartDisplay();
        }

        private void UpdateCartDisplay()
        {
            // Очищаем список
            CartItemsList.Items.Clear();
            
            // Добавляем товары простым текстом
            foreach (var item in Cart.Items)
            {
                string itemText = $"{item.Product.Name} x{item.Quantity} - {item.Product.Price * item.Quantity:C}";
                CartItemsList.Items.Add(itemText);
            }
            
            // Общая сумма
            TotalPriceText.Text = $"Итого: {Cart.GetTotalPrice():C}";
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Items.Count == 0)
            {
                MessageBox.Show("Корзина пуста!");
                return;
            }
            
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.BtnOrder_Click(null, null);
            }
        }
    }
}
