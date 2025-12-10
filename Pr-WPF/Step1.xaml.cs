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
    public partial class Step1Page : UserControl
    {
        private CarConfiguration _config;

        public Step1Page(CarConfiguration config)
        {
            InitializeComponent();
            _config = config;
            LoadData();
            SetupBindings();
        }

        private void LoadData()
        {
            modelCombo.ItemsSource = _config.Models.Keys;
            engineCombo.ItemsSource = _config.Engines.Keys;

            if (!string.IsNullOrEmpty(_config.SelectedModel))
                modelCombo.SelectedItem = _config.SelectedModel;

            if (!string.IsNullOrEmpty(_config.SelectedEngine))
                engineCombo.SelectedItem = _config.SelectedEngine;
        }

        private void SetupBindings()
        {
            modelCombo.SelectionChanged += (s, e) =>
            {
                if (modelCombo.SelectedItem != null)
                    _config.SelectedModel = modelCombo.SelectedItem.ToString();
            };

            engineCombo.SelectionChanged += (s, e) =>
            {
                if (engineCombo.SelectedItem != null)
                    _config.SelectedEngine = engineCombo.SelectedItem.ToString();
            };

            _config.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_config.BasePrice))
                    basePriceText.Text = $"{_config.BasePrice:N0} руб.";
            };

            basePriceText.Text = $"{_config.BasePrice:N0} руб.";
        }
    }
}
