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
                    UpdateCreditDisplay();
                }
            };
        }

        private void UpdateCreditDisplay()
        {
            carPriceText.Text = $"{_config.TotalPrice:N0} руб.";
            downPaymentValueText.Text = $"{_config.DownPaymentPercent:F0}%";
            termValueText.Text = $"{_config.LoanTerm} мес.";

            downPaymentAmountText.Text = $"{_config.DownPaymentAmount:N0} руб.";

            decimal loanAmount = _config.TotalPrice - _config.DownPaymentAmount;
            loanAmountText.Text = $"{loanAmount:N0} руб.";

            var monthlyPayment = _config.CalculateMonthlyPayment();
            monthlyPaymentText.Text = $"{monthlyPayment:N0} руб.";

            var totalPayment = monthlyPayment * _config.LoanTerm + _config.DownPaymentAmount;
            totalPaymentText.Text = $"{totalPayment:N0} руб.";
        }
    }
}