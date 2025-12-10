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
    public partial class Step2Page : UserControl
    {
        private CarConfiguration _config;

        public Step2Page(CarConfiguration config)
        {
            InitializeComponent();
            _config = config;
            LoadData();
            SetupBindings();
        }

        private void LoadData()
        {
            colorCombo.ItemsSource = _config.Colors.Keys;

            if (!string.IsNullOrEmpty(_config.SelectedColor))
                colorCombo.SelectedItem = _config.SelectedColor;

            // Загружаем опции
            optionsPanel.Children.Clear();
            foreach (var option in _config.Options)
            {
                var checkBox = new CheckBox
                {
                    FontSize = 14,
                    Margin = new Thickness(0, 5, 0, 5),
                    Content = $"{option.Key} (+{option.Value:N0} руб.)",
                    IsChecked = _config.SelectedOptions.Contains(option.Key)
                };
                checkBox.Checked += (s, e) => _config.ToggleOption(option.Key);
                checkBox.Unchecked += (s, e) => _config.ToggleOption(option.Key);
                optionsPanel.Children.Add(checkBox);
            }
        }

        private void SetupBindings()
        {
            colorCombo.SelectionChanged += (s, e) =>
            {
                if (colorCombo.SelectedItem != null)
                    _config.SelectedColor = colorCombo.SelectedItem.ToString();
            };

            _config.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_config.TotalPrice))
                    totalPriceText.Text = $"{_config.TotalPrice:N0} руб.";
            };

            totalPriceText.Text = $"{_config.TotalPrice:N0} руб.";
        }
    }
}
