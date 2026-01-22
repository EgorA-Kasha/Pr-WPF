using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Loaded += MainWindow_Loaded;
        }

        //private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    BtnProducts_Click(null, null);
        //}

        public void BtnProduct_Click(object sender, RoutedEventArgs e)
        {
            var page = new Tovari();
            MainFrame.Navigate(page);
        }

        public void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            var page = new Basket();
            MainFrame.Navigate(page);
        }

        public void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Items.Count == 0)
            {
                MessageBox.Show("Корзина пуста!");
                return;
            }
            var page = new ZakaZ();
            MainFrame.Navigate(page);
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public static class Cart
    {
        public static List<CartItem> Items { get; } = new List<CartItem>();

        public static void AddProduct(Product product)
        {
            var existingItem = Items.FirstOrDefault(x => x.Product.Id == product.Id);
            if (existingItem != null)
                existingItem.Quantity++;
            else
                Items.Add(new CartItem { Product = product, Quantity = 1 });
        }

        public static decimal GetTotalPrice()
        {
            return Items.Sum(x => x.Product.Price * x.Quantity);
        }

        public static void Clear()
        {
            Items.Clear();
        }
    }

    public static class Database
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static List<Product> GetProducts()
        {
            var products = new List<Product>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, Name, Price FROM Products", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Price = (decimal)reader["Price"]
                        });
                    }
                }
            }

            return products;
        }

        public static int SaveOrder(string fullName, string email, string address)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Orders (FullName, Email, Address) OUTPUT INSERTED.Id VALUES (@FullName, @Email, @Address)",
                    connection);

                command.Parameters.AddWithValue("@FullName", fullName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Address", address);

                return (int)command.ExecuteScalar();
            }
        }

        public static void SaveOrderItems(int orderId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var item in Cart.Items)
                {
                    var command = new SqlCommand(
                        "INSERT INTO OrderItems (OrderId, ProductId, Quantity) VALUES (@OrderId, @ProductId, @Quantity)",
                        connection);

                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.Parameters.AddWithValue("@ProductId", item.Product.Id);
                    command.Parameters.AddWithValue("@Quantity", item.Quantity);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
