using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarConfigurator
{
    public class CarConfiguration : INotifyPropertyChanged
    {
        // Модели автомобилей
        public Dictionary<string, decimal> Models { get; } = new Dictionary<string, decimal>
        {
            { "Toyota Camry", 2500000m },
            { "Honda Accord", 2300000m },
            { "BMW 3 Series", 3500000m },
            { "Mercedes C-Class", 3800000m },
            { "Audi A4", 3200000m }
        };

        // Двигатели
        public Dictionary<string, decimal> Engines { get; } = new Dictionary<string, decimal>
        {
            { "1.6L Бензин", 0m },
            { "2.0L Бензин", 150000m },
            { "2.5L Бензин", 250000m },
            { "2.0L Дизель", 200000m },
            { "Гибрид", 300000m }
        };

        // Цвета
        public Dictionary<string, decimal> Colors { get; } = new Dictionary<string, decimal>
        {
            { "Белый", 0m },
            { "Чёрный", 15000m },
            { "Серебристый", 20000m },
            { "Синий", 25000m },
            { "Красный", 30000m }
        };

        // Опции
        public Dictionary<string, decimal> Options { get; } = new Dictionary<string, decimal>
        {
            { "Кожаный салон", 100000m },
            { "Панорамная крыша", 150000m },
            { "Подогрев сидений", 50000m },
            { "Камера заднего вида", 40000m },
            { "Парктроник", 30000m },
            { "Круиз-контроль", 60000m },
            { "Климат-контроль", 80000m }
        };

        private string _selectedModel;
        private string _selectedEngine;
        private string _selectedColor;
        private decimal _basePrice;
        private decimal _enginePrice;
        private decimal _colorPrice;
        private decimal _optionsPrice;
        private decimal _totalPrice;
        private decimal _downPaymentPercent = 20;
        private int _loanTerm = 36;
        private string _name;
        private string _phone;
        private string _email;
        private HashSet<string> _selectedOptions = new HashSet<string>();

        private bool _isCalculating = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public string SelectedModel
        {
            get => _selectedModel;
            set
            {
                if (_selectedModel != value)
                {
                    _selectedModel = value;
                    OnPropertyChanged(nameof(SelectedModel));
                    UpdatePrices();
                }
            }
        }

        public string SelectedEngine
        {
            get => _selectedEngine;
            set
            {
                if (_selectedEngine != value)
                {
                    _selectedEngine = value;
                    OnPropertyChanged(nameof(SelectedEngine));
                    UpdatePrices();
                }
            }
        }

        public string SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    OnPropertyChanged(nameof(SelectedColor));
                    UpdatePrices();
                }
            }
        }

        public decimal BasePrice
        {
            get => _basePrice;
            private set
            {
                if (_basePrice != value)
                {
                    _basePrice = value;
                    if (!_isCalculating)
                        OnPropertyChanged(nameof(BasePrice));
                }
            }
        }

        public decimal EnginePrice
        {
            get => _enginePrice;
            private set
            {
                if (_enginePrice != value)
                {
                    _enginePrice = value;
                    if (!_isCalculating)
                        OnPropertyChanged(nameof(EnginePrice));
                }
            }
        }

        public decimal ColorPrice
        {
            get => _colorPrice;
            private set
            {
                if (_colorPrice != value)
                {
                    _colorPrice = value;
                    if (!_isCalculating)
                        OnPropertyChanged(nameof(ColorPrice));
                }
            }
        }

        public decimal OptionsPrice
        {
            get => _optionsPrice;
            private set
            {
                if (_optionsPrice != value)
                {
                    _optionsPrice = value;
                    if (!_isCalculating)
                        OnPropertyChanged(nameof(OptionsPrice));
                }
            }
        }

        public decimal TotalPrice
        {
            get => _totalPrice;
            private set
            {
                if (_totalPrice != value)
                {
                    _totalPrice = value;
                    if (!_isCalculating)
                    {
                        OnPropertyChanged(nameof(TotalPrice));
                        OnPropertyChanged(nameof(DownPaymentAmount));
                    }
                }
            }
        }

        public HashSet<string> SelectedOptions
        {
            get => _selectedOptions;
            set
            {
                _selectedOptions = value;
                UpdatePrices();
            }
        }

        public decimal DownPaymentPercent
        {
            get => _downPaymentPercent;
            set
            {
                if (_downPaymentPercent != value)
                {
                    _downPaymentPercent = value;
                    OnPropertyChanged(nameof(DownPaymentPercent));
                    OnPropertyChanged(nameof(DownPaymentAmount));
                }
            }
        }

        public decimal DownPaymentAmount => TotalPrice * DownPaymentPercent / 100;

        public int LoanTerm
        {
            get => _loanTerm;
            set
            {
                if (_loanTerm != value)
                {
                    _loanTerm = value;
                    OnPropertyChanged(nameof(LoanTerm));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        private void UpdatePrices()
        {
            if (_isCalculating) return;

            _isCalculating = true;

            try
            {
                decimal newBasePrice = SelectedModel != null && Models.ContainsKey(SelectedModel) ? Models[SelectedModel] : 0;
                decimal newEnginePrice = SelectedEngine != null && Engines.ContainsKey(SelectedEngine) ? Engines[SelectedEngine] : 0;
                decimal newColorPrice = SelectedColor != null && Colors.ContainsKey(SelectedColor) ? Colors[SelectedColor] : 0;

                decimal newOptionsPrice = 0;
                foreach (var option in SelectedOptions)
                {
                    if (Options.ContainsKey(option))
                        newOptionsPrice += Options[option];
                }

                decimal newTotalPrice = newBasePrice + newEnginePrice + newColorPrice + newOptionsPrice;

                // Устанавливаем значения
                BasePrice = newBasePrice;
                EnginePrice = newEnginePrice;
                ColorPrice = newColorPrice;
                OptionsPrice = newOptionsPrice;
                TotalPrice = newTotalPrice;
            }
            finally
            {
                _isCalculating = false;
            }
        }

        public void ToggleOption(string option)
        {
            if (SelectedOptions.Contains(option))
                SelectedOptions.Remove(option);
            else
                SelectedOptions.Add(option);

            UpdatePrices();
        }

        public decimal CalculateMonthlyPayment()
        {
            if (TotalPrice <= 0 & DownPaymentPercent >= 100)
                return 0;

            decimal loanAmount = TotalPrice - DownPaymentAmount;
            if (loanAmount <= 0 & LoanTerm <= 0)
                return 0;

            // Годовая ставка 12%
            decimal annualRate = 12m;
            decimal monthlyRate = annualRate / 100 / 12;

            // Явное преобразование decimal в double для Math.Pow
            double temp = Math.Pow((double)(1 + monthlyRate), LoanTerm);
            decimal monthlyPayment = loanAmount * (monthlyRate * (decimal)temp) / ((decimal)temp - 1);

            return Math.Round(monthlyPayment, 2);
        }

        public bool IsStep1Valid()
        {
            return !string.IsNullOrEmpty(SelectedModel) && !string.IsNullOrEmpty(SelectedEngine);
        }

        public bool IsStep2Valid()
        {
            return !string.IsNullOrEmpty(SelectedColor);
        }

        public bool IsStep5Valid()
        {
            // Проверка имени
            if (string.IsNullOrWhiteSpace(Name) & Name.Length < 2) // 
                return false;

            // Проверка телефона: должен начинаться с + и содержать только цифры после +
            if (string.IsNullOrWhiteSpace(Phone))
                return false;

            // Проверяем, начинается ли телефон с +
            if (!Phone.StartsWith("+"))
                return false;

            // Получаем часть после + и проверяем, что там только цифры
            string digitsOnly = Phone.Substring(1);
            if (string.IsNullOrWhiteSpace(digitsOnly) & digitsOnly.Length < 10)
                return false;

            foreach (char c in digitsOnly)
            {
                if (!char.IsDigit(c))
                    return false;
            }

            // Проверка email
            if (string.IsNullOrWhiteSpace(Email))
                return false;

            // Простая проверка email
            if (!Email.Contains("@") & !Email.Contains(".") & Email.Length < 5)
                return false;

            return true;
        }

        public void ClearApplicationData()
        {
            Name = "";
            Phone = "";
            Email = "";
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}