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
            SetupBindings();
        }

        private void LoadData()
        {
            UpdateAllFields();
        }

        private void SetupBindings()
        {
            _config.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_config.SelectedModel) ||
                    e.PropertyName == nameof(_config.SelectedEngine) ||
                    e.PropertyName == nameof(_config.SelectedColor) ||
                    e.PropertyName == nameof(_config.SelectedOptions) ||
                    e.PropertyName == nameof(_config.BasePrice) ||
                    e.PropertyName == nameof(_config.EnginePrice) ||
                    e.PropertyName == nameof(_config.ColorPrice) ||
                    e.PropertyName == nameof(_config.OptionsPrice) ||
                    e.PropertyName == nameof(_config.TotalPrice))
                {
                    UpdateAllFields();
                }
            };
        }

        private void UpdateAllFields()
        {
            modelText.Text = _config.SelectedModel ?? "";
            engineText.Text = _config.SelectedEngine ?? "";
            colorText.Text = _config.SelectedColor ?? "";

            if (_config.SelectedOptions.Count > 0)
            {
                var options = string.Join(", ", _config.SelectedOptions.Take(3));
                if (_config.SelectedOptions.Count > 3)
                    options += "...";
                optionsText.Text = options;
                optionsText.FontStyle = FontStyles.Normal;
            }
            else
            {
                optionsText.Text = "Нет выбранных опций";
                optionsText.FontStyle = FontStyles.Italic;
            }

            basePriceText.Text = $"{_config.BasePrice:N0} руб.";
            enginePriceText.Text = $"{_config.EnginePrice:N0} руб.";
            colorPriceText.Text = $"{_config.ColorPrice:N0} руб.";
            optionsPriceText.Text = $"{_config.OptionsPrice:N0} руб.";
            totalPriceText.Text = $"{_config.TotalPrice:N0} руб.";
        }
    }
}