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
using static Pr_WPF.Model;

namespace Pr_WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для CompletedBuilds.xaml
    /// </summary>
    public partial class CompletedBuilds : Page
    {
        public CompletedBuilds()
        {
            private MainWindow mainWindow;
        }
        public CompletedBuilds()
        {
            InitializeComponent();
            mainWindow = Application.Current.MainWindow as MainWindow;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAssemblies();
        }

        private void LoadAssemblies()
        {
            var assemblies = build.db.assembly
                .Include(a => a.partassembly)
                .ThenInclude(p => p.part)
                .ToList();

            lvAssemblies.ItemsSource = assemblies.Select(a => new
            {
                a.id,
                a.name,
                a.author,
                PartsCount = a.partassembly.Count,
                TotalPrice = a.partassembly.Sum(p => p.part.price) ?? 0
            }).ToList();
        }

        private void LoadAssembly_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                int assemblyId = (int)button.Tag;
                LoadAssembly(assemblyId);
            }
        }

        private void lvAssemblies_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvAssemblies.SelectedItem == null) return;

            dynamic selected = lvAssemblies.SelectedItem;
            LoadAssembly(selected.id);
        }

        private void LoadAssembly(int assemblyId)
        {
            try
            {
                var assembly = build.db.assembly
                    .Include(a => a.partassembly)
                    .ThenInclude(p => p.part)
                    .FirstOrDefault(a => a.id == assemblyId);

                if (assembly != null)
                {
                    var builderPage = new BuildPC();

                    // Очищаем текущие выбранные детали
                    var selectedParts = new List<SelectedPart>();

                    foreach (var pa in assembly.partassembly)
                    {
                        var part = pa.part;
                        selectedParts.Add(new SelectedPart
                        {
                            Id = part.id,
                            Name = part.name,
                            Type = part.parttype?.name,
                            Manufacturer = part.manufacturer?.name,
                            Price = part.price ?? 0,
                            Image = part.image,
                            PartTypeId = part.parttypeid ?? 0,
                            Characteristics = GetCharacteristics(part)
                        });
                    }

                    // Здесь нужно добавить метод для установки выбранных деталей в BuilderPage
                    // Для простоты можно сохранять в статическую переменную или передавать через конструктор

                    MessageBox.Show($"Сборка \"{assembly.name}\" загружена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    mainWindow.MainFrame.Navigate(builderPage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetCharacteristics(basepart part)
        {
            switch (part.parttypeid)
            {
                case 1: // CPU
                    var cpu = build.db.cpu.FirstOrDefault(c => c.id == part.id);
                    return cpu != null ? $"{cpu.numberofcores} ядер, {cpu.basecorefrequency} ГГц" : "";
                case 2: // GPU
                    var gpu = build.db.gpu.FirstOrDefault(g => g.id == part.id);
                    return gpu != null ? $"{gpu.videomemory} ГБ, {gpu.chipfrequency} МГц" : "";
                case 3: // RAM
                    var ram = build.db.ram.FirstOrDefault(r => r.id == part.id);
                    return ram != null ? $"{ram.capacity} ГБ, {ram.ghz} МГц" : "";
                case 4: // Motherboard
                    var mb = build.db.motherboard.FirstOrDefault(m => m.id == part.id);
                    return mb != null ? $"Socket: {mb.socket.name}" : "";
                case 5: // Case
                    return "Корпус";
                case 6: // PowerSupply
                    var ps = build.db.powersupply.FirstOrDefault(p => p.id == part.id);
                    return ps != null ? $"{ps.power} Вт" : "";
                default:
                    return "";
            }
        }
    }
    }
}
