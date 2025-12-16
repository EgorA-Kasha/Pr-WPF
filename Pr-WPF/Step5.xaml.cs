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
        private bool _nameWasEdited = false;
        private bool _phoneWasEdited = false;
        private bool _emailWasEdited = false;

        public Step5Page(CarConfiguration config)
        {
            InitializeComponent();
            _config = config;
            LoadData();
            SetupBindings();
            UpdateSummary();
        }

        private void LoadData()
        {
            if (!string.IsNullOrEmpty(_config.Name))
            {
                nameBox.Text = _config.Name;
                _nameWasEdited = true;
            }

            if (!string.IsNullOrEmpty(_config.Phone))
            {
                phoneBox.Text = _config.Phone;
                _phoneWasEdited = true;
            }

            if (!string.IsNullOrEmpty(_config.Email))
            {
                emailBox.Text = _config.Email;
                _emailWasEdited = true;
            }
        }

        private void SetupBindings()
        {
            nameBox.LostFocus += (s, e) =>
            {
                _nameWasEdited = true;
                ValidateName();
            };

            phoneBox.LostFocus += (s, e) =>
            {
                _phoneWasEdited = true;
                ValidatePhone();
            };

            emailBox.LostFocus += (s, e) =>
            {
                _emailWasEdited = true;
                ValidateEmail();
            };

            nameBox.TextChanged += (s, e) =>
            {
                _config.Name = nameBox.Text;
                if (_nameWasEdited)
                    ValidateName();
            };

            phoneBox.TextChanged += (s, e) =>
            {
                _config.Phone = phoneBox.Text;
                if (_phoneWasEdited)
                    ValidatePhone();
            };

            emailBox.TextChanged += (s, e) =>
            {
                _config.Email = emailBox.Text;
                if (_emailWasEdited)
                    ValidateEmail();
            };

            _config.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_config.SelectedModel) ||
                    e.PropertyName == nameof(_config.SelectedEngine) ||
                    e.PropertyName == nameof(_config.SelectedColor) ||
                    e.PropertyName == nameof(_config.TotalPrice) ||
                    e.PropertyName == nameof(_config.SelectedOptions))
                {
                    UpdateSummary();
                }
            };

            if (_nameWasEdited) ValidateName();
            if (_phoneWasEdited) ValidatePhone();
            if (_emailWasEdited) ValidateEmail();
        }

        private void ValidateName()
        {
            bool isValid = !string.IsNullOrWhiteSpace(_config.Name) && _config.Name.Length >= 2;
            UpdateValidation(nameBox, isValid,
                isValid ? "Поле заполнено корректно" : "Имя должно содержать минимум 2 символа");
        }

        private void ValidatePhone()
        {
            bool isValid = !string.IsNullOrWhiteSpace(_config.Phone) &&
                          _config.Phone.StartsWith("+") &&
                          _config.Phone.Substring(1).All(char.IsDigit) &&
                          _config.Phone.Substring(1).Length >= 10;
            UpdateValidation(phoneBox, isValid,
                isValid ? "Поле заполнено корректно" : "Телефон должен начинаться с + и содержать минимум 10 цифр");
        }

        private void ValidateEmail()
        {
            bool isValid = !string.IsNullOrWhiteSpace(_config.Email) &&
                          _config.
Email.Contains("@") &&
                          _config.Email.Contains(".") &&
                          _config.Email.Length >= 5;
            UpdateValidation(emailBox, isValid,
                isValid ? "Поле заполнено корректно" : "Введите корректный email адрес");
        }

        private void UpdateValidation(TextBox textBox, bool isValid, string tooltipText)
        {
            if (!IsFieldEdited(textBox))
            {
                textBox.BorderBrush = Brushes.Gray;
                textBox.ToolTip = GetDefaultTooltip(textBox);
            }
            else if (isValid)
            {
                textBox.BorderBrush = Brushes.Green;
                textBox.ToolTip = tooltipText;
            }
            else
            {
                textBox.BorderBrush = Brushes.Red;
                textBox.ToolTip = tooltipText;
            }
        }

        private bool IsFieldEdited(TextBox textBox)
        {
            if (textBox == nameBox) return _nameWasEdited;
            if (textBox == phoneBox) return _phoneWasEdited;
            if (textBox == emailBox) return _emailWasEdited;
            return false;
        }

        private string GetDefaultTooltip(TextBox textBox)
        {
            if (textBox == nameBox)
                return "Введите имя (минимум 2 символа)";
            else if (textBox == phoneBox)
                return "Введите телефон в формате: +7XXXXXXXXXX (минимум 10 цифр после +)";
            else if (textBox == emailBox)
                return "Введите email адрес";
            return "";
        }

        private void UpdateSummary()
        {
            modelEngineText.Text = $"{_config.SelectedModel}, {_config.SelectedEngine}";

            colorSummaryText.Text = _config.SelectedColor ?? "";

            if (_config.SelectedOptions.Count > 0)
            {
                var options = string.Join(", ", _config.SelectedOptions.Take(3));
                if (_config.SelectedOptions.Count > 3)
                    options += "...";
                optionsSummaryText.Text = options;
            }
            else
            {
                optionsSummaryText.Text = "Нет выбранных опций";
            }

            // Обновляем цены
            finalPriceText.Text = $"{_config.TotalPrice:N0} руб.";

            var monthlyPayment = _config.CalculateMonthlyPayment();
            monthlyPaymentSummaryText.Text = $"{monthlyPayment:N0} руб.";
        }
    }
}