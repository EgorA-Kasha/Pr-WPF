using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private CarConfiguration _config;
        private int _currentStep = 1;
        
        public int CurrentStep
        {
            get => _currentStep;
            set 
            { 
                _currentStep = value; 
                OnPropertyChanged(nameof(CurrentStep));
                OnPropertyChanged(nameof(StepTitle));
                OnPropertyChanged(nameof(NextButtonText));
                OnPropertyChanged(nameof(CanGoBack));
                OnPropertyChanged(nameof(CanGoNext));
                OnPropertyChanged(nameof(CurrentPage));
            }
        }
        
        public string StepTitle
        {
            get
            {
                switch (_currentStep)
                {
                    case 1: return "Шаг 1: Выбор модели и двигателя";
                    case 2: return "Шаг 2: Выбор цвета и опций";
                    case 3: return "Шаг 3: Итоговая стоимость";
                    case 4: return "Шаг 4: Расчёт кредита";
                    case 5: return "Шаг 5: Оформление заявки";
                    default: return "";
                }
            }
        }
        
        public string NextButtonText => _currentStep == 5 ? "Отправить заявку" : "Далее";
        
        public bool CanGoBack => _currentStep > 1;
        
        public bool CanGoNext
        {
            get
            {
                switch (_currentStep)
                {
                    case 1: return _config.IsStep1Valid();
                    case 2: return _config.IsStep2Valid();
                    case 3: return true;
                    case 4: return true;
                    case 5: return _config.IsStep5Valid();
                    default: return false;
                }
            }
        }
        
        public FrameworkElement CurrentPage
        {
            get
            {
                switch (_currentStep)
                {
                    case 1: return CreateStep1();
                    case 2: return CreateStep2();
                    case 3: return CreateStep3();
                    case 4: return CreateStep4();
                    case 5: return CreateStep5();
                    default: return null;
                }
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();
            _config = new CarConfiguration();
            _config.PropertyChanged += Config_PropertyChanged;
            DataContext = this;
        }
        
        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_config.SelectedModel) ||
                e.PropertyName == nameof(_config.SelectedEngine) ||
                e.PropertyName == nameof(_config.SelectedColor) ||
                e.PropertyName == nameof(_config.Name) ||
                e.PropertyName == nameof(_config.Phone) ||
                e.PropertyName == nameof(_config.Email))
            {
                OnPropertyChanged(nameof(CanGoNext));
            }
        }
        
        private FrameworkElement CreateStep1()
        {
            var stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(20);
            
            var title = new TextBlock 
            { 
                Text = "Выберите модель автомобиля", 
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(title);
            
            var modelCombo = new ComboBox 
            { 
                FontSize = 14,
                Padding = new Thickness(10, 5, 0, 5),
                Margin = new Thickness(0, 5, 0, 15),
                MinWidth = 200,
                ItemsSource = _config.Models.Keys
            };
            
            if (!string.IsNullOrEmpty(_config.SelectedModel))
                modelCombo.SelectedItem = _config.SelectedModel;
            
            modelCombo.SelectionChanged += (s, e) =>
            {
                if (modelCombo.SelectedItem != null)
                    _config.SelectedModel = modelCombo.SelectedItem.ToString();
            };
            stackPanel.Children.Add(modelCombo);
            
            var subtitle = new TextBlock 
            { 
                Text = "Выберите тип двигателя", 
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 20, 0, 10)
            };
            stackPanel.Children.Add(subtitle);
            
            var engineCombo = new ComboBox 
            { 
                FontSize = 14,
                Padding = new Thickness(10, 5, 0, 5),
                Margin = new Thickness(0, 5, 0, 15),
                MinWidth = 200,
                ItemsSource = _config.Engines.Keys
            };
            
            if (!string.IsNullOrEmpty(_config.SelectedEngine))
                engineCombo.SelectedItem = _config.SelectedEngine;
            
            engineCombo.SelectionChanged += (s, e) =>
            {
                if (engineCombo.SelectedItem != null)
                    _config.SelectedEngine = engineCombo.SelectedItem.ToString();
            };
            stackPanel.Children.Add(engineCombo);
            
            var pricePanel = new StackPanel();
            pricePanel.Margin = new Thickness(0, 20, 0, 0);
            
            var priceLabel = new TextBlock 
            { 
                Text = "Базовая цена:", 
                FontSize = 14,
                Margin = new Thickness(0, 10, 0, 5)
            };
            pricePanel.Children.Add(priceLabel);
            
            var basePriceText = new TextBlock 
            { 
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.DarkGreen,
                Margin = new Thickness(0, 5, 0, 0)
            };
            basePriceText.Text = $"{_config.BasePrice:N0} руб.";
            _config.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_config.BasePrice))
                    basePriceText.Text = $"{_config.BasePrice:N0} руб.";
            };
            pricePanel.Children.Add(basePriceText);
            
            stackPanel.Children.Add(pricePanel);
            
            return stackPanel;
        }
        
        private FrameworkElement CreateStep2()
        {
            var stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(20);
            
            var title = new TextBlock 
            { 
                Text = "Выберите цвет кузова", 
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(title);
            
            var colorCombo = new ComboBox 
            { 
                FontSize = 14,
                Padding = new Thickness(10, 5, 0, 5),
                Margin = new Thickness(0, 5, 0, 15),
                MinWidth = 200,
                ItemsSource = _config.Colors.Keys
            };
            
            if (!string.IsNullOrEmpty(_config.SelectedColor))
                colorCombo.SelectedItem = _config.SelectedColor;
            
            colorCombo.SelectionChanged += (s, e) =>
            {
                if (colorCombo.SelectedItem != null)
                    _config.SelectedColor = colorCombo.SelectedItem.ToString();
            };
            stackPanel.Children.Add(colorCombo);
            
            var subtitle = new TextBlock 
            { 
                Text = "Дополнительные опции", 
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 20, 0, 10)
            };
            stackPanel.Children.Add(subtitle);
            
            var optionsPanel = new StackPanel();
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
            
            var scrollViewer = new ScrollViewer 
            { 
                Height = 200,
                Margin = new Thickness(0, 10, 0, 20)
            };
            scrollViewer.Content = optionsPanel;
            stackPanel.Children.Add(scrollViewer);
            
            var pricePanel = new StackPanel();
            var priceLabel = new TextBlock 
            { 
                Text = "Итоговая стоимость:", 
                FontSize = 14,
                Margin = new Thickness(0, 10, 0, 5)
            };
            pricePanel.Children.Add(priceLabel);
            
            var totalPriceText = new TextBlock 
            { 
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.DarkGreen,
                Margin = new Thickness(0, 5, 0, 0)
            };
            totalPriceText.Text = $"{_config.TotalPrice:N0} руб.";
            _config.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_config.TotalPrice))
                    totalPriceText.Text = $"{_config.TotalPrice:N0} руб.";
            };
            pricePanel.Children.Add(totalPriceText);
            
            stackPanel.Children.Add(pricePanel);
            
            return stackPanel;
        }
        
        private FrameworkElement CreateStep3()
        {
            var stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(20);
            
            var title = new TextBlock 
            { 
                Text = "Сводка конфигурации", 
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(title);
            
            var summaryPanel = new Border
            {
                Background = System.Windows.Media.Brushes.WhiteSmoke,
                Padding = new Thickness(15),
                Margin = new Thickness(0, 20, 0, 0)
            };
            
            var summaryStack = new StackPanel();
            
            AddSummaryItem(summaryStack, "Модель:", _config.SelectedModel);
            AddSummaryItem(summaryStack, "Двигатель:", _config.SelectedEngine);
            AddSummaryItem(summaryStack, "Цвет:", _config.SelectedColor);
            
            summaryStack.Children.Add(new Separator { Margin = new Thickness(0, 10, 0, 10) });
            
            var optionsLabel = new TextBlock 
            { 
                Text = "Выбранные опции:", 
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 5)
            };
            summaryStack.Children.Add(optionsLabel);
            
            foreach (var option in _config.SelectedOptions)
            {
                summaryStack.Children.Add(new TextBlock 
                { 
                    Text = $"• {option}",
                    Margin = new Thickness(20, 0, 0, 2)
                });
            }
            
            if (_config.SelectedOptions.Count == 0)
            {
                summaryStack.Children.Add(new TextBlock 
                { 
                    Text = "Нет выбранных опций",
                    Margin = new Thickness(20, 0, 0, 2),
                    FontStyle = FontStyles.Italic
                });
            }
            
            summaryPanel.Child = summaryStack;
            stackPanel.Children.Add(summaryPanel);
            
            var pricePanel = new StackPanel();
            pricePanel.Margin = new Thickness(0, 30, 0, 0);
            
            AddPriceItem(pricePanel, "Базовая цена модели:", _config.BasePrice);
            AddPriceItem(pricePanel, "Доплата за двигатель:", _config.EnginePrice);
            AddPriceItem(pricePanel, "Доплата за цвет:", _config.ColorPrice);
            AddPriceItem(pricePanel, "Дополнительные опции:", _config.OptionsPrice);
            
            pricePanel.Children.Add(new Separator { Margin = new Thickness(0, 10, 0, 10) });
            
            var totalPanel = new StackPanel();
            totalPanel.Orientation = Orientation.Horizontal;
            
            var totalLabel = new TextBlock 
            { 
                Text = "ИТОГО:",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 20, 0)
            };
            totalPanel.Children.Add(totalLabel);
            
            var totalText = new TextBlock 
            { 
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.DarkGreen
            };
            totalText.Text = $"{_config.TotalPrice:N0} руб.";
            totalPanel.Children.Add(totalText);
            
            pricePanel.Children.Add(totalPanel);
            stackPanel.Children.Add(pricePanel);
            
            return stackPanel;
        }
        
        private FrameworkElement CreateStep4()
        {
            var stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(20);
            
            var title = new TextBlock 
            { 
                Text = "Расчёт параметров кредита", 
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(title);
            
            var costLabel = new TextBlock 
            { 
                Text = "Стоимость автомобиля:",
                FontSize = 14,
                Margin = new Thickness(0, 20, 0, 5)
            };
            stackPanel.Children.Add(costLabel);
            
            var totalPriceText = new TextBlock 
            { 
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.DarkGreen,
                Margin = new Thickness(0, 5, 0, 0)
            };
            totalPriceText.Text = $"{_config.TotalPrice:N0} руб.";
            _config.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_config.TotalPrice))
                    totalPriceText.Text = $"{_config.TotalPrice:N0} руб.";
            };
            stackPanel.Children.Add(totalPriceText);
            
            // Первоначальный взнос
            var downPaymentPanel = new StackPanel();
            downPaymentPanel.Margin = new Thickness(0, 20, 0, 0);
            
            var downPaymentLabel = new TextBlock 
            { 
                Text = "Первоначальный взнос (%)",
                FontSize = 14,
                Margin = new Thickness(0, 10, 0, 5)
            };
            downPaymentPanel.Children.Add(downPaymentLabel);
            
            var downPaymentSlider = new Slider 
            {
                Minimum = 10,
                Maximum = 90,
                Value = (double)_config.DownPaymentPercent,
                TickFrequency = 5,
                IsSnapToTickEnabled = true,
                Margin = new Thickness(0, 0, 0, 5)
            };
            downPaymentSlider.ValueChanged += (s, e) => 
            {
                _config.DownPaymentPercent = (decimal)downPaymentSlider.Value;
                UpdateCreditDisplay(stackPanel);
            };
            downPaymentPanel.Children.Add(downPaymentSlider);
            
            var downPaymentValue = new TextBlock 
            { 
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 14
            };
            downPaymentValue.Text = $"{_config.DownPaymentPercent:F0}%";
            _config.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_config.DownPaymentPercent))
                    downPaymentValue.Text = $"{_config.DownPaymentPercent:F0}%";
            };
            downPaymentPanel.Children.Add(downPaymentValue);
            
            stackPanel.Children.Add(downPaymentPanel);
            
            // Срок кредита
            var termPanel = new StackPanel();
            termPanel.Margin = new Thickness(0, 20, 0, 0);
            
            var termLabel = new TextBlock 
            { 
                Text = "Срок кредита (месяцев)",
                FontSize = 14,
                Margin = new Thickness(0, 10, 0, 5)
            };
            termPanel.Children.Add(termLabel);
            
            var termSlider = new Slider 
            {
                Minimum = 12,
                Maximum = 96,
                Value = _config.LoanTerm,
                TickFrequency = 12,
                IsSnapToTickEnabled = true,
                Margin = new Thickness(0, 0, 0, 5)
            };
            termSlider.ValueChanged += (s, e) => 
            {
                _config.LoanTerm = (int)termSlider.Value;
                UpdateCreditDisplay(stackPanel);
            };
            termPanel.Children.Add(termSlider);
            
            var termValue = new TextBlock 
            { 
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 14
            };
            termValue.Text = $"{_config.LoanTerm} мес.";
            _config.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_config.LoanTerm))
                    termValue.Text = $"{_config.LoanTerm} мес.";
            };
            termPanel.Children.Add(termValue);
            
            stackPanel.Children.Add(termPanel);
            
            // Расчёт платежей
            var calculationPanel = CreateCreditCalculationPanel();
            stackPanel.Children.Add(calculationPanel);
            
            return stackPanel;
        }
        
        private void UpdateCreditDisplay(StackPanel stackPanel)
        {
            // Удаляем старую панель расчетов
            if (stackPanel.Children.Count > 0)
            {
                var lastChild = stackPanel.Children[stackPanel.Children.Count - 1];
                if (lastChild is Border border && border.Child is StackPanel childPanel &&
                    childPanel.Children.Count > 0 && childPanel.Children[0] is TextBlock titleBlock &&
                    titleBlock.Text == "Расчёт кредита")
                {
                    stackPanel.Children.Remove(lastChild);
                }
            }
            
            // Добавляем новую панель расчетов
            var newCalculationPanel = CreateCreditCalculationPanel();
            stackPanel.Children.Add(newCalculationPanel);
        }
        
        private Border CreateCreditCalculationPanel()
        {
            var calculationPanel = new Border
            {
                Background = System.Windows.Media.Brushes.WhiteSmoke,
                Padding = new Thickness(15),
                Margin = new Thickness(0, 30, 0, 0)
            };
            
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
            return calculationPanel;
        }
        
        private FrameworkElement CreateStep5()
        {
            var stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(20);
            
            var title = new TextBlock 
            { 
                Text = "Оформление заявки", 
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(title);
            
            // Контактные данные
            var contactPanel = new StackPanel();
            contactPanel.Margin = new Thickness(0, 20, 0, 0);
            
            var nameLabel = new TextBlock 
            { 
                Text = "Имя*", 
                FontSize = 14,
                Margin = new Thickness(0, 10, 0, 5)
            };
            contactPanel.Children.Add(nameLabel);
            
            var nameBox = new TextBox 
            { 
                FontSize = 14,
                Padding = new Thickness(10, 5, 0, 5),
                Margin = new Thickness(0, 5, 0, 15),
                MinWidth = 200
            };
            if (!string.IsNullOrEmpty(_config.Name))
                nameBox.Text = _config.Name;
            nameBox.TextChanged += (s, e) => _config.Name = nameBox.Text;
            contactPanel.Children.Add(nameBox);
            
            var phoneLabel = new TextBlock 
            { 
                Text = "Телефон* (формат: +7XXXXXXXXXX)", 
                FontSize = 14,
                Margin = new Thickness(0, 10, 0, 5)
            };
            contactPanel.Children.Add(phoneLabel);
            
            var phoneBox = new TextBox 
            { 
                FontSize = 14,
                Padding = new Thickness(10, 5, 0, 5),
                Margin = new Thickness(0, 5, 0, 15),
                MinWidth = 200
            };
            if (!string.IsNullOrEmpty(_config.Phone))
                phoneBox.Text = _config.Phone;
            phoneBox.TextChanged += (s, e) => _config.Phone = phoneBox.Text;
            contactPanel.Children.Add(phoneBox);
            
            var emailLabel = new TextBlock 
            { 
                Text = "Email*", 
                FontSize = 14,
                Margin = new Thickness(0, 10, 0, 5)
            };
            contactPanel.Children.Add(emailLabel);
            
            var emailBox = new TextBox 
            { 
                FontSize = 14,
                Padding = new Thickness(10, 5, 0, 5),
                Margin = new Thickness(0, 5, 0, 15),
                MinWidth = 200
            };
            if (!string.IsNullOrEmpty(_config.Email))
                emailBox.Text = _config.Email;
            emailBox.TextChanged += (s, e) => _config.Email = emailBox.Text;
            contactPanel.Children.Add(emailBox);
            
            stackPanel.Children.Add(contactPanel);
            
            // Краткая сводка
            var summaryPanel = new Border
            {
                Background = System.Windows.Media.Brushes.WhiteSmoke,
                Padding = new Thickness(15),
                Margin = new Thickness(0, 30, 0, 0)
            };
            
            var summaryStack = new StackPanel();
            
            summaryStack.Children.Add(new TextBlock 
            { 
                Text = "Итоговая конфигурация",
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 10)
            });
            
            summaryStack.Children.Add(new TextBlock 
            { 
                Text = $"{_config.SelectedModel}, {_config.SelectedEngine}",
                Margin = new Thickness(0, 0, 0, 5)
            });
            
            summaryStack.Children.Add(new TextBlock 
            { 
                Text = $"Цвет: {_config.SelectedColor}",
                Margin = new Thickness(0, 0, 0, 5)
            });
            
            if (_config.SelectedOptions.Count > 0)
            {
                var optionsText = $"Опции: {string.Join(", ", _config.SelectedOptions.Take(3))}";
                if (_config.SelectedOptions.Count > 3)
                    optionsText += "...";
                
                summaryStack.Children.Add(new TextBlock 
                { 
                    Text = optionsText,
                    Margin = new Thickness(0, 0, 0, 5)
                });
            }
            
            summaryStack.Children.Add(new Separator { Margin = new Thickness(0, 10, 0, 10) });
            
            var totalText = new TextBlock 
            { 
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.DarkGreen
            };
            totalText.Text = $"Итоговая стоимость: {_config.TotalPrice:N0} руб.";
            summaryStack.Children.Add(totalText);
            
            var monthlyPayment = _config.CalculateMonthlyPayment();
            var monthlyText = new TextBlock 
            { 
                FontSize = 16,
                Margin = new Thickness(0, 5, 0, 0)
            };
            monthlyText.Text = $"Ежемесячный платёж: {monthlyPayment:N0} руб.";
            summaryStack.Children.Add(monthlyText);
            
            summaryPanel.Child = summaryStack;
            stackPanel.Children.Add(summaryPanel);
            
            return stackPanel;
        }
        
        private void AddSummaryItem(Panel panel, string label, string value)
        {
            var itemPanel = new StackPanel();
            itemPanel.Orientation = Orientation.Horizontal;
            
            var labelText = new TextBlock 
            { 
                Text = label,
                FontWeight = FontWeights.Bold,
                Width = 150
            };
            itemPanel.Children.Add(labelText);
            
            var valueText = new TextBlock { Text = value ?? "" };
            itemPanel.Children.Add(valueText);
            
            panel.Children.Add(itemPanel);
        }
        
        private void AddPriceItem(Panel panel, string label, decimal value)
        {
            var itemPanel = new StackPanel();
            itemPanel.Orientation = Orientation.Horizontal;
            
            var labelText = new TextBlock 
            { 
                Text = label,
                Width = 250,
                Margin = new Thickness(0, 0, 10, 0)
            };
            itemPanel.Children.Add(labelText);
            
            var valueText = new TextBlock();
            valueText.Text = $"{value:N0} руб.";
            itemPanel.Children.Add(valueText);
            
            panel.Children.Add(itemPanel);
        }
        
        private void AddCalculationItem(Panel panel, string label, decimal value)
        {
            var itemPanel = new StackPanel();
            itemPanel.Orientation = Orientation.Horizontal;
            
            var labelText = new TextBlock 
            { 
                Text = label,
                Width = 200,
                Margin = new Thickness(0, 0, 10, 0)
            };
            itemPanel.Children.Add(labelText);
            
            var valueText = new TextBlock();
            valueText.Text = $"{value:N0} руб.";
            itemPanel.Children.Add(valueText);
            
            panel.Children.Add(itemPanel);
        }
        
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (_currentStep > 1)
            {
                CurrentStep--;
            }
        }
        
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_currentStep < 5)
            {
                CurrentStep++;
            }
            else if (_currentStep == 5)
            {
                SubmitApplication();
            }
        }
        
        private void SubmitApplication()
        {
            MessageBox.Show(
                $"Заявка успешно оформлена!\n\n" +
                $"Клиент: {_config.Name}\n" +
                $"Телефон: {_config.Phone}\n" +
                $"Email: {_config.Email}\n\n" +
                $"Конфигурация: {_config.SelectedModel}, {_config.SelectedEngine}\n" +
                $"Итоговая стоимость: {_config.TotalPrice:N0} руб.\n\n" +
                $"Спасибо за заявку! С вами свяжется наш менеджер в течение 24 часов.",
                "Заявка оформлена",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            
            Application.Current.Shutdown();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}