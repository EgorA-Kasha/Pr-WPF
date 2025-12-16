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
        private bool _isLeavingStep5 = false;

        public int CurrentStep
        {
            get => _currentStep;
            set
            {
                if (_currentStep == 5 && value < 5 && !_config.IsStep5Valid() && !_isLeavingStep5)
                {
                    // показываем подтверждение при уходе с незаполненной заявки
                    var result = MessageBox.Show(
                        "Вы не завершили оформление заявки. Все введённые данные будут потеряны.\n\nПродолжить?",
                        "Подтверждение",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        _isLeavingStep5 = true;
                        // очищаем данные заявки
                        _config.ClearApplicationData();
                        _currentStep = value;
                        _isLeavingStep5 = false;

                        OnPropertyChanged(nameof(CurrentStep));
                        OnPropertyChanged(nameof(StepTitle));
                        OnPropertyChanged(nameof(NextButtonText));
                        OnPropertyChanged(nameof(CanGoBack));
                        OnPropertyChanged(nameof(CanGoNext));
                        OnPropertyChanged(nameof(CurrentPage));
                    }
                }
                else
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
                    case 1: return new Step1Page(_config);
                    case 2: return new Step2Page(_config);
                    case 3: return new Step3Page(_config);
                    case 4: return new Step4Page(_config);
                    case 5: return new Step5Page(_config);
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
                MessageBoxImage.Error);

            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_currentStep == 5 && !_config.IsStep5Valid())
            {
                var result = MessageBox.Show(
                    "Вы не завершили оформление заявки. Все введённые данные будут потеряны.\n\nВыйти из программы?",
                    "Подтверждение выхода",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}