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
    /// Логика взаимодействия для BuildPC.xaml
    /// </summary>
    public partial class BuildPC : Page
    {
        public partial class BuilderPage : Page
        {
            private List<SelectedPart> selectedParts = new List<SelectedPart>();
            private int currentPartType = 1;

            public BuilderPage()
            {
                InitializeComponent();
                LoadManufacturers();
                LoadParts();
            }

            private void Page_Loaded(object sender, RoutedEventArgs e)
            {
                LoadParts();
            }

            private void LoadManufacturers()
            {
                try
                {
                    var manufacturers = build.db.manufacturer.ToList();
                    manufacturers.Insert(0, new manufacturer { id = 0, name = "Все" });
                    cmbManufacturer.ItemsSource = manufacturers;
                    cmbManufacturer.DisplayMemberPath = "name";
                    cmbManufacturer.SelectedValuePath = "id";
                    cmbManufacturer.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки производителей: {ex.Message}");
                }
            }

            private void LoadParts()
            {
                try
                {
                    var query = build.db.basepart
                        .Include(b => b.manufacturer)
                        .Include(b => b.parttype)
                        .Where(b => b.parttypeid == currentPartType);

                    if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                    {
                        query = query.Where(b => b.name.Contains(txtSearch.Text));
                    }

                    if (cmbManufacturer.SelectedValue != null && (int)cmbManufacturer.SelectedValue != 0)
                    {
                        int manId = (int)cmbManufacturer.SelectedValue;
                        query = query.Where(b => b.manufacturerid == manId);
                    }

                    var parts = query.Select(b => new
                    {
                        b.id,
                        b.name,
                        ManufacturerName = b.manufacturer.name,
                        b.price,
                        b.image,
                        PartTypeName = b.parttype.name
                    }).ToList();

                    lvParts.ItemsSource = parts;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки: {ex.Message}");
                }
            }

            private void PartType_Click(object sender, RoutedEventArgs e)
            {
                var button = sender as Button;
                if (button != null)
                {
                    currentPartType = int.Parse(button.Tag.ToString());
                    LoadParts();
                }
            }

            private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
            {
                LoadParts();
            }

            private void cmbManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                LoadParts();
            }

            private void lvParts_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                if (lvParts.SelectedItem == null) return;

                dynamic selected = lvParts.SelectedItem;
                int partId = selected.id;

                var part = build.db.basepart
                    .Include(b => b.manufacturer)
                    .Include(b => b.parttype)
                    .FirstOrDefault(b => b.id == partId);

                if (part != null)
                {
                    var existingPart = selectedParts.FirstOrDefault(p => p.PartTypeId == part.parttypeid);
                    if (existingPart != null)
                    {
                        var result = MessageBox.Show($"Деталь типа '{part.parttype.name}' уже выбрана. Заменить?",
                            "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.No)
                            return;

                        selectedParts.Remove(existingPart);
                    }

                    var compatibility = CheckCompatibility(part);
                    if (!compatibility.IsCompatible)
                    {
                        MessageBox.Show(compatibility.Message, "Ошибка совместимости",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    selectedParts.Add(new SelectedPart
                    {
                        Id = part.id,
                        Name = part.name,
                        Type = part.parttype?.name ?? "",
                        Manufacturer = part.manufacturer?.name ?? "",
                        Price = part.price ?? 0,
                        Image = part.image,
                        PartTypeId = part.parttypeid ?? 0,
                        Characteristics = GetCharacteristics(part)
                    });

                    UpdateSelectedPartsList();
                }
            }

            private string GetCharacteristics(basepart part)
            {
                try
                {
                    if (part.parttypeid == 1)
                    {
                        var cpu = build.db.cpu.FirstOrDefault(c => c.id == part.id);
                        if (cpu != null)
                        {
                            var socket = build.db.socket.FirstOrDefault(s => s.id == cpu.socketid);
                            return $"Ядер: {cpu.numberofcores}, Частота: {cpu.basecorefrequency} ГГц, TDP: {cpu.thermalpower} Вт, Сокет: {socket?.name}";
                        }
                    }
                    else if (part.parttypeid == 2)
                    {
                        var gpu = build.db.gpu.FirstOrDefault(g => g.id == part.id);
                        if (gpu != null)
                        {
                            return $"Память: {gpu.videomemory} ГБ, Частота: {gpu.chipfrequency} МГц, Требуемый БП: {gpu.recommendpower} Вт";
                        }
                    }
                    else if (part.parttypeid == 3)
                    {
                        var ram = build.db.ram.FirstOrDefault(r => r.id == part.id);
                        if (ram != null)
                        {
                            var memType = build.db.memorytype.FirstOrDefault(m => m.id == ram.memorytypeid);
                            return $"{ram.capacity} ГБ, {ram.ghz} МГц, {memType?.name}";
                        }
                    }
                    else if (part.parttypeid == 4)
                    {
                        var mb = build.db.motherboard
                            .Include(m => m.socket)
                            .Include(m => m.formfactor)
                            .Include(m => m.memorytype)
                            .FirstOrDefault(m => m.id == part.id);
                        if (mb != null)
                        {
                            return $"Сокет: {mb.socket?.name}, Форм-фактор: {mb.formfactor?.name}, Память: {mb.memorytype?.name}";
                        }
                    }
                    else if (part.parttypeid == 5)
                    {
                        var pcCase = build.db.case
    
                            .Include(c => c.casesize)
                            .FirstOrDefault(c => c.id == part.id);
                            if (pcCase != null)
                            {
                                return $"Размер: {pcCase.casesize?.name}";
                            }
                        }
                else if (part.parttypeid == 6)
                        {
                            var ps = build.db.powersupply
                                .Include(p => p.certificate)
                                .FirstOrDefault(p => p.id == part.id);
                            if (ps != null)
                            {
                                return $"Мощность: {ps.power} Вт, {ps.certificate?.name}";
                            }
                        }
                        else if (part.parttypeid == 7)
                        {
                            var cooler = build.db.processorcooler
                                .Include(p => p.fandimension)
                                .FirstOrDefault(p => p.id == part.id);
                            if (cooler != null)
                            {
                                return $"Вентилятор: {cooler.fandimension?.name}";
                            }
                        }
                        else if (part.parttypeid == 8)
                        {
                            var storage = build.db.storagedevice
                                .Include(s => s.storagedeviceinterface)
                                .Include(s => s.storagedevicetype)
                                .FirstOrDefault(s => s.id == part.id);
                            if (storage != null)
                            {
                                return $"{storage.capacity} ГБ, {storage.storagedevicetype?.name}";
                            }
                        }
                    }
            catch { }

                return "";
            }

            private CompatibilityResult CheckCompatibility(basepart newPart)
            {
                var cpu = selectedParts.FirstOrDefault(p => p.PartTypeId == 1);
                var motherboard = selectedParts.FirstOrDefault(p => p.PartTypeId == 4);
                var ram = selectedParts.FirstOrDefault(p => p.PartTypeId == 3);
                var gpu = selectedParts.FirstOrDefault(p => p.PartTypeId == 2);
                var psu = selectedParts.FirstOrDefault(p => p.PartTypeId == 6);
                var cooler = selectedParts.FirstOrDefault(p => p.PartTypeId == 7);
                var pcCase = selectedParts.FirstOrDefault(p => p.PartTypeId == 5);

                if (newPart.parttypeid == 1 && motherboard != null)
                {
                    var cpuData = build.db.cpu.FirstOrDefault(c => c.id == newPart.id);
                    var mbData = build.db.motherboard.FirstOrDefault(m => m.id == motherboard.Id);

                    if (cpuData != null && mbData != null && cpuData.socketid != mbData.socketid)
                    {
                        var cpuSocket = build.db.socket.FirstOrDefault(s => s.id == cpuData.socketid)?.name;
                        var mbSocket = build.db.socket.FirstOrDefault(s => s.id == mbData.socketid)?.name;

                        return new CompatibilityResult
                        {
                            IsCompatible = false,
                            Message = $"Сокет процессора ({cpuSocket}) не совместим с сокетом материнской платы ({mbSocket})!"
                        };
                    }
                }

                if (newPart.parttypeid == 4)
                {
                    var mbData = build.db.motherboard.FirstOrDefault(m => m.id == newPart.id);

                    if (mbData != null)
                    {
                        if (cpu != null)
                        {
                            var cpuData = build.db.cpu.FirstOrDefault(c => c.id == cpu.Id);

                            if (cpuData != null && cpuData.socketid != mbData.socketid)
                            {
                                var cpuSocket = build.db.socket.FirstOrDefault(s => s.id == cpuData.socketid)?.name;
                                var mbSocket = build.db.socket.FirstOrDefault(s => s.id == mbData.socketid)?.name;

                                return new CompatibilityResult
                                {
                                    IsCompatible = false,
                                    Message = $"Сокет материнской платы ({mbSocket}) не совместим с сокетом процессора ({cpuSocket})!"
                                };
                            }
                        }

                        if (ram != null)
                        {
                            var ramData = build.db.ram.FirstOrDefault(r => r.id == ram.Id);

                            if (ramData != null && mbData.memorytypeid != ramData.memorytypeid)
                            {
                                var mbMemType = build.db.memorytype.FirstOrDefault(m => m.id == mbData.memorytypeid)?.name;
                                var ramMemType = build.db.memorytype.FirstOrDefault(m => m.id == ramData.memorytypeid)?.name;

                                return new CompatibilityResult
                                {
                                    IsCompatible = false,
                                    Message = $"Тип памяти материнской платы ({mbMemType}) не совместим с типом памяти ОЗУ ({ramMemType})!"
                                };
                            }
                        }

                        if (pcCase != null)
                        {
                            var caseData = build.db.case.FirstOrDefault(c => c.id == pcCase.Id);

                                if (caseData != null)
                                {
                                    var compatible = build.db.boardformfactorcase
                                        .Any(b => b.caseid == caseData.id && b.formfactorid == mbData.formfactorid);

                                    if (!compatible)
                                    {
                                        var mbFormFactor = build.db.formfactor.FirstOrDefault(f => f.id == mbData.formfactorid)?.name;

                                        return new CompatibilityResult
                                        {
                                            IsCompatible = false,
                                            Message = $"Форм-фактор материнской платы ({mbFormFactor}) не поддерживается корпусом!"
                                        };
                                    }
                                }
                            }
                        }
                    }

                    if (newPart.parttypeid == 3 && motherboard != null)
                    {
                        var mbData = build.db.motherboard.FirstOrDefault(m => m.id == motherboard.Id);
                        var ramData = build.db.ram.FirstOrDefault(r => r.id == newPart.id);

                        if (mbData != null && ramData != null && mbData.memorytypeid != ramData.memorytypeid)
                        {
                            var mbMemType = build.db.memorytype.FirstOrDefault(m => m.id == mbData.memorytypeid)?.name;
                            var ramMemType = build.db.memorytype.FirstOrDefault(m => m.id == ramData.memorytypeid)?.name;

                            return new CompatibilityResult
                            {
                                IsCompatible = false,
                                Message = $"Тип памяти ОЗУ ({ramMemType}) не совместим с типом памяти материнской платы ({mbMemType})!"
                            };
                        }
                    }

                    if (newPart.parttypeid == 5 && motherboard != null)
                    {
                        var mbData = build.db.motherboard.FirstOrDefault(m => m.id == motherboard.Id);
                        var caseData = build.db.case.FirstOrDefault(c => c.id == newPart.id);

                            if (mbData != null && caseData != null)
                            {
                                var compatible = build.db.boardformfactorcase
                                    .Any(b => b.caseid == caseData.id && b.formfactorid == mbData.formfactorid);

                                if (!compatible)
                                {
                                    var mbFormFactor = build.db.formfactor.FirstOrDefault(f => f.id == mbData.formfactorid)?.name;

                                    return new CompatibilityResult
                                    {
                                        IsCompatible = false,
                                        Message = $"Корпус не поддерживает форм-фактор материнской платы ({mbFormFactor})!"
                                    };
                                }
                            }
                        }

                        if (newPart.parttypeid == 2 && psu != null)
                        {
                            var gpuData = build.db.gpu.FirstOrDefault(g => g.id == newPart.id);
                            var psuData = build.db.powersupply.FirstOrDefault(p => p.id == psu.Id);

                            if (gpuData != null && psuData != null && gpuData.recommendpower.HasValue)
                            {
                                if (gpuData.recommendpower > psuData.power)
                                {
                                    return new CompatibilityResult
                                    {
                                        IsCompatible = false,
                                        Message = $"Мощность блока питания ({psuData.power} Вт) недостаточна для видеокарты! Требуется минимум {gpuData.recommendpower} Вт"
                                    };
                                }
                            }
                        }

                        if (newPart.parttypeid == 6 && gpu != null)
                        {
                            var gpuData = build.db.gpu.FirstOrDefault(g => g.id == gpu.Id);
                            var psuData = build.db.powersupply.FirstOrDefault(p => p.id == newPart.id);

                            if (gpuData != null && psuData != null && gpuData.recommendpower.HasValue)
                            {
                                if (gpuData.recommendpower > psuData.power)
                                {
                                    return new CompatibilityResult
                                    {
                                        IsCompatible = false,
                                        Message = $"Мощность блока питания ({psuData.power} Вт) недостаточна для видеокарты! Требуется минимум {gpuData.recommendpower} Вт"
                                    };
                                }
                            }
                        }

                        if (newPart.parttypeid == 7 && cpu != null)
                        {
                            var cpuData = build.db.cpu.FirstOrDefault(c => c.id == cpu.Id);
                            var coolerData = build.db.processorcooler.FirstOrDefault(p => p.id == newPart.id);

                            if (cpuData != null && coolerData != null)
                            {
                                var compatible = build.db.socketprocessorcooler
                                    .Any(s => s.processorcoolerid == coolerData.id && s.socketid == cpuData.socketid);

                                if (!compatible)
                                {
                                    var cpuSocket = build.db.socket.FirstOrDefault(s => s.id == cpuData.socketid)?.name;

                                    return new CompatibilityResult
                                    {
                                        IsCompatible = false,
                                        Message = $"Кулер не поддерживает сокет процессора ({cpuSocket})!"
                                    };
                                }
                            }
                        }

                        return new CompatibilityResult { IsCompatible = true };
                    }

        private void RemovePart_Click(object sender, RoutedEventArgs e)
            {
                var button = sender as Button;
                if (button != null)
                {
                    int partId = (int)button.Tag;
                    selectedParts.RemoveAll(p => p.Id == partId);
                    UpdateSelectedPartsList();
                }
            }

            private void UpdateSelectedPartsList()
            {
                icSelectedParts.ItemsSource = null;
                icSelectedParts.ItemsSource = selectedParts;

                decimal total = 0;
                foreach (var part in selectedParts)
                {
                    total += part.Price;
                }
                txtTotalPrice.Text = total.ToString("C");
            }

            private void btnSaveAssembly_Click(object sender, RoutedEventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtAssemblyName.Text))
                {
                    MessageBox.Show("Введите название сборки!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtAuthor.Text))
                {
                    MessageBox.Show("Введите имя автора!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (selectedParts.Count == 0)
                {
                    MessageBox.Show("Добавьте комплектующие в сборку!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    var assembly = new assembly
                    {
                        name = txtAssemblyName.Text,
                        author = txtAuthor.Text
                    };

                    build.db.assembly.Add(assembly);
                    build.db.SaveChanges();

                    foreach (var part in selectedParts)
                    {
                        build.db.partassembly.Add(new partassembly
                        {
                            partid = part.Id,
                            assemblyid = assembly.id
                        });
                    }

                    build.db.SaveChanges();

                    MessageBox.Show($"Сборка \"{assembly.name}\" успешно сохранена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    txtAssemblyName.Clear();
                    txtAuthor.Clear();
                    selectedParts.Clear();
                    UpdateSelectedPartsList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
}

