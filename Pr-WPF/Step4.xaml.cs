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
    public partial class Step4Page : UserControl
    {
        private CarConfiguration _config;

        public Step4Page(CarConfiguration config)
        {
            InitializeComponent();
            _config = config;
            LoadData();
            SetupBindings();
            UpdateCreditDisplay();
        }

        private void LoadData()
        {
            downPaymentSlider.Value = (double)_config.DownPaymentPercent;
            termSlider.Value = _config.LoanTerm;
        }

        private void SetupBindings()
        {
            downPaymentSlider.ValueChanged += (s, e) =>
            {
                _config.DownPaymentPercent = (decimal)downPaymentSlider.Value;
                UpdateCreditDisplay();
            };

            termSlider.ValueChanged += (s, e) =>
            {
                _config.LoanTerm = (int)termSlider.Value;
                UpdateCreditDisplay();
            };

            _config.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_config.TotalPrice) ||
                    e.PropertyName == nameof(_config.DownPaymentPercent) ||
                    e.PropertyName == nameof(_config.LoanTerm))
                {
                    totalPriceText.Text = $"{_config.TotalPrice:N0} руб.";
                    downPaymentValue.Text = $"{_config.DownPaymentPercent:F0}%";
                    termValue.Text = $"{_config.LoanTerm} мес.";
                    UpdateCreditDisplay();
                }
            };

            totalPriceText.Text = $"{_config.TotalPrice:N0} руб.";
            downPaymentValue.Text = $"{_config.DownPaymentPercent:F0}%";
            termValue.Text = $"{_config.LoanTerm} мес.";
        }

        private void UpdateCreditDisplay()
        {
            var calcStack = new StackPanel();

            calcStack.Children.Add(new TextBlock
            {
                Text = "Расчёт кредита",
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 10)
            });

            AddCalculationItem(calcStack, "Первоначальный взнос:", _config.DownPaymentAmount);
            AddCalculationItem(calcStack, "Сумма кредита:", _config.TotalPrice - _config.DownPaymentAmount);

            var monthlyPayment = _config.CalculateMonthlyPayment();
            AddCalculationItem(calcStack, "Ежемесячный платёж:", monthlyPayment);

            var totalPayment = monthlyPayment * _config.LoanTerm + _config.DownPaymentAmount;
            AddCalculationItem(calcStack, "Общая сумма выплат:", totalPayment);

            calculationPanel.Child = calcStack;
        }

        private void AddCalculationItem(Panel panel, string label, decimal value)
        {
            var itemPanel = new StackPanel { Orientation = Orientation.Horizontal };
            itemPanel.Children.Add(new TextBlock
            {
                Text = label,
                Width = 200,
                Margin = new Thickness(0, 0, 10, 0)
            });

            var valueText = new TextBlock { Text = $"{value:N0} руб." };
            itemPanel.Children.Add(valueText);
            panel.Children.Add(itemPanel);
        }
    }
}
