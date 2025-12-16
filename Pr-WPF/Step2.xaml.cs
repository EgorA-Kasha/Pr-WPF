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

            leatherInteriorCheck.IsChecked = _config.SelectedOptions.Contains("Кожаный салон");
            panoramicRoofCheck.IsChecked = _config.SelectedOptions.Contains("Панорамная крыша");
            heatedSeatsCheck.IsChecked = _config.SelectedOptions.Contains("Подогрев сидений");
            rearCameraCheck.IsChecked = _config.SelectedOptions.Contains("Камера заднего вида");
            parkingSensorsCheck.IsChecked = _config.SelectedOptions.Contains("Парктроник");
            cruiseControlCheck.IsChecked = _config.SelectedOptions.Contains("Круиз-контроль");
            climateControlCheck.IsChecked = _config.SelectedOptions.Contains("Климат-контроль");
        }

        private void SetupBindings()
        {
            colorCombo.SelectionChanged += (s, e) =>
            {
                if (colorCombo.SelectedItem != null)
                    _config.SelectedColor = colorCombo.SelectedItem.ToString();
            };

            leatherInteriorCheck.Checked += (s, e) => _config.ToggleOption("Кожаный салон");
            leatherInteriorCheck.Unchecked += (s, e) => _config.ToggleOption("Кожаный салон");

            panoramicRoofCheck.Checked += (s, e) => _config.ToggleOption("Панорамная крыша");
            panoramicRoofCheck.Unchecked += (s, e) => _config.ToggleOption("Панорамная крыша");

            heatedSeatsCheck.Checked += (s, e) => _config.ToggleOption("Подогрев сидений");
            heatedSeatsCheck.Unchecked += (s, e) => _config.ToggleOption("Подогрев сидений");

            rearCameraCheck.Checked += (s, e) => _config.ToggleOption("Камера заднего вида");
            rearCameraCheck.Unchecked += (s, e) => _config.ToggleOption("Камера заднего вида");

            parkingSensorsCheck.Checked += (s, e) => _config.ToggleOption("Парктроник");
            parkingSensorsCheck.Unchecked += (s, e) => _config.ToggleOption("Парктроник");

            cruiseControlCheck.Checked += (s, e) => _config.ToggleOption("Круиз-контроль");
            cruiseControlCheck.Unchecked += (s, e) => _config.ToggleOption("Круиз-контроль");

            climateControlCheck.Checked += (s, e) => _config.ToggleOption("Климат-контроль");
            climateControlCheck.Unchecked += (s, e) => _config.ToggleOption("Климат-контроль");

            _config.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_config.TotalPrice))
                    totalPriceText.Text = $"{_config.TotalPrice:N0} руб.";
            };

            totalPriceText.Text = $"{_config.TotalPrice:N0} руб.";
        }
    }
}