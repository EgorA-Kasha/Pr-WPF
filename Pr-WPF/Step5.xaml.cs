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

namespace CarConfigurator
{
    public partial class Step5Page : UserControl
    {
        private CarConfiguration _config;

        public Step5Page(CarConfiguration config)
        {
            InitializeComponent();
            _config = config;
            LoadData();
            SetupBindings();
        }

        private void LoadData()
        {
            if (!string.IsNullOrEmpty(_config.Name))
                nameBox.Text = _config.Name;

            if (!string.IsNullOrEmpty(_config.Phone))
                phoneBox.Text = _config.Phone;

            if (!string.IsNullOrEmpty(_config.Email))
                emailBox.Text = _config.Email;

            UpdateSummary();
        }

        private void SetupBindings()
        {
            nameBox.TextChanged += (s, e) => _config.Name = nameBox.Text;
            phoneBox.TextChanged += (s, e) => _config.Phone = phoneBox.Text;
            emailBox.TextChanged += (s, e) => _config.Email = emailBox.Text;

            _config.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_config.TotalPrice) ||
                    e.PropertyName == nameof(_config.SelectedModel) ||
                    e.PropertyName == nameof(_config.SelectedEngine) ||
                    e.PropertyName == nameof(_config.SelectedColor))
                {
                    UpdateSummary();
                }
            };
        }

        private void UpdateSummary()
        {
            summaryPanel.Children.Clear();

            summaryPanel.Children.Add(new TextBlock
            {
                Text = "Итоговая конфигурация",
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 10)
            });

            summaryPanel.Children.Add(new TextBlock
            {
                Text = $"{_config.SelectedModel}, {_config.SelectedEngine}",
                Margin = new Thickness(0, 0, 0, 5)
            });

            summaryPanel.Children.Add(new TextBlock
            {
                Text = $"Цвет: {_config.SelectedColor}",
                Margin = new Thickness(0, 0, 0, 5)
            });

            if (_config.SelectedOptions.Count > 0)
            {
                var optionsText = $"Опции: {string.Join(", ", _config.SelectedOptions.Take(3))}";
                if (_config.SelectedOptions.Count > 3)
                    optionsText += "...";

                summaryPanel.Children.Add(new TextBlock
                {
                    Text = optionsText,
                    Margin = new Thickness(0, 0, 0, 5)
                });
            }

            summaryPanel.Children.Add(new Separator { Margin = new Thickness(0, 10, 0, 10) });

            var totalText = new TextBlock
            {
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.DarkGreen,
                Text = $"Итоговая стоимость: {_config.TotalPrice:N0} руб."
            };
            summaryPanel.Children.Add(totalText);

            var monthlyPayment = _config.CalculateMonthlyPayment();
            var monthlyText = new TextBlock
            {
                FontSize = 16,
                Margin = new Thickness(0, 5, 0, 0),
                Text = $"Ежемесячный платёж: {monthlyPayment:N0} руб."
            };
            summaryPanel.Children.Add(monthlyText);
        }
    }
}
