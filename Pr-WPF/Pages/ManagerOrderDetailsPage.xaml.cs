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
    /// Логика взаимодействия для ManagerOrderDetailsPage.xaml
    /// </summary>
    public partial class ManagerOrderDetailsPage : Page
    {
        private Order _currentOrder;

        public ManagerOrderDetailsPage(Order order)
        {
            InitializeComponent();
            _currentOrder = order;
            LoadDetails();
        }

        private void LoadDetails()
        {
            var client = App.db.User.FirstOrDefault(u => u.ID == _currentOrder.ClientID);

            TxtOrderId.Text = "Заказ № " + _currentOrder.ID;
            TxtClient.Text = "Клиент: " + client?.FullName;
            TxtDates.Text = "Оформлен: " + _currentOrder.OrderDate + " | Желаемая дата выдачи: " + _currentOrder.ReceiveDate;
            TxtStatus.Text = "Текущий статус: " + _currentOrder.Status;

            GridProducts.ItemsSource = App.db.OrderProduct
                .Where(op => op.OrderID == _currentOrder.ID)
                .ToList();

            if (_currentOrder.Status == "Выдан" || _currentOrder.Status == "Отменен")
            {
                BtnIssue.Visibility = Visibility.Collapsed;
            }
        }

        private void IssueOrder_Click(object sender, RoutedEventArgs e)
        {
            var dbOrder = App.db.Order.FirstOrDefault(o => o.ID == _currentOrder.ID);
            if (dbOrder != null)
            {
                dbOrder.Status = "Выдан";
                App.db.SaveChanges();
                MessageBox.Show("Статус заказа изменён на 'Выдан'.");
                NavigationService.GoBack();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
