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
    public partial class Step3Page : UserControl
    {
        private CarConfiguration _config;

        public Step3Page(CarConfiguration config)
        {
            InitializeComponent();
            _config = config;
            LoadData();
        }

        private void LoadData()
        {
            // Очищаем панели
            summaryPanel.Children.Clear();
            pricePanel.Children.Clear();

            // Добавляем сводку
            AddSummaryItem("Модель:", _config.SelectedModel);
            AddSummaryItem("Двигатель:", _config.SelectedEngine);
            AddSummaryItem("Цвет:", _config.SelectedColor);

            summaryPanel.Children.Add(new Separator { Margin = new Thickness(0, 10, 0, 10) });

            var optionsLabel = new TextBlock
            {
                Text = "Выбранные опции:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 5)
            };
            summaryPanel.Children.Add(optionsLabel);

            foreach (var option in _config.SelectedOptions)
            {
                summaryPanel.Children.Add(new TextBlock
                {
                    Text = $"• {option}",
                    Margin = new Thickness(20, 0, 0, 2)
                });
            }

            if (_config.SelectedOptions.Count == 0)
            {
                summaryPanel.Children.Add(new TextBlock
                {
                    Text = "Нет выбранных опций",
                    Margin = new Thickness(20, 0, 0, 2),
                    FontStyle = FontStyles.Italic
                });
            }

            // Добавляем цены
            AddPriceItem("Базовая цена модели:", _config.BasePrice);
            AddPriceItem("Доплата за двигатель:", _config.EnginePrice);
            AddPriceItem("Доплата за цвет:", _config.ColorPrice);
            AddPriceItem("Дополнительные опции:", _config.OptionsPrice);

            pricePanel.Children.Add(new Separator { Margin = new Thickness(0, 10, 0, 10) });

            var totalPanel = new StackPanel { Orientation = Orientation.Horizontal };
            totalPanel.Children.Add(new TextBlock
            {
                Text = "ИТОГО:",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 20, 0)
            });

            var totalText = new TextBlock
            {
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.DarkGreen,
                Text = $"{_config.TotalPrice:N0} руб."
            };
            totalPanel.Children.Add(totalText);

            pricePanel.Children.Add(totalPanel);
        }

        private void AddSummaryItem(string label, string value)
        {
            var itemPanel = new StackPanel { Orientation = Orientation.Horizontal };
            itemPanel.Children.Add(new TextBlock
            {
                Text = label,
                FontWeight = FontWeights.Bold,
                Width = 150
            });
            itemPanel.Children.Add(new TextBlock { Text = value ?? "" });
            summaryPanel.Children.Add(itemPanel);
        }

        private void AddPriceItem(string label, decimal value)
        {
            var itemPanel = new StackPanel { Orientation = Orientation.Horizontal };
            itemPanel.Children.Add(new TextBlock
            {
                Text = label,
                Width = 250,
                Margin = new Thickness(0, 0, 10, 0)
            });

            var valueText = new TextBlock { Text = $"{value:N0} руб." };
            itemPanel.Children.Add(valueText);
            pricePanel.Children.Add(itemPanel);
        }
    }
}
