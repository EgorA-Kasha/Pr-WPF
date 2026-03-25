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

namespace Pr_WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для Assemblying.xaml
    /// </summary>
    public partial class Assemblying : Page
    {
        public static readonly DependencyProperty FilteredPartsProperty = DependencyProperty.Register(
            "FilteredParts", typeof(List<basepart>), typeof(Assemblying));

        public List<basepart> FilteredParts
        {
            get { return (List<basepart>)GetValue(FilteredPartsProperty); }
            set { SetValue(FilteredPartsProperty, value); }
        }

        private Dictionary<int, basepart> Cart = new Dictionary<int, basepart>();

        public Assemblying()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var manufacturers = App.db.manufacturer.ToList();
            manufacturers.Insert(0, new manufacturer { id = 0, name = "Все производители" });
            CmbManufacturer.ItemsSource = manufacturers;
            CmbManufacturer.SelectedIndex = 0;

            CategoriesTabControl.ItemsSource = App.db.parttype.ToList();
            if (CategoriesTabControl.Items.Count > 0)
                CategoriesTabControl.SelectedIndex = 0;
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            UpdateCurrentTabList();
        }

        private void CategoriesTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl) UpdateCurrentTabList();
        }

        private void UpdateCurrentTabList()
        {
            if (CategoriesTabControl.SelectedItem is parttype currentType)
            {
                int typeId = currentType.id;
                string searchText = TxtSearch.Text?.ToLower() ?? "";
                int mId = (CmbManufacturer.SelectedItem as manufacturer)?.id ?? 0;

                var query = App.db.basepart.Include("manufacturer").Where(p => p.parttypeid == typeId).AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                    query = query.Where(p => p.name.ToLower().Contains(searchText));

                if (mId != 0)
                    query = query.Where(p => p.manufacturerid == mId);

                FilteredParts = query.ToList();
            }
        }

        private void CatalogItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedItem is basepart selectedPart)
            {
                if (CheckCompatibility(selectedPart, out string errorMsg))
                {
                    Cart[selectedPart.parttypeid] = selectedPart;
                    UpdateCartUI();
                }
                else
                {
                    MessageBox.Show(errorMsg, "Ошибка совместимости", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void LvCart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LvCart.SelectedItem is basepart p)
            {
                Cart.Remove(p.parttypeid);
                UpdateCartUI();
            }
        }

        private void UpdateCartUI()
        {
            LvCart.ItemsSource = null;
            LvCart.ItemsSource = Cart.Values.ToList();
            TxtTotalPrice.Text = $"Итого: {Cart.Values.Sum(p => p.price):N0} ₽";
        }

        private bool CheckCompatibility(basepart newPart, out string errorMessage)
        {
            errorMessage = "";
            int typeId = newPart.parttypeid;

            if (typeId == 1)
            {
                var cpuSpec = App.db.cpu.FirstOrDefault(c => c.id == newPart.id);
                if (Cart.TryGetValue(4, out basepart mbPart))
                {
                    var mbSpec = App.db.motherboard.FirstOrDefault(m => m.id == mbPart.id);
                    if (cpuSpec.socketid != mbSpec.socketid) { errorMessage = "Сокет процессора не совпадает с материнской платой!"; return false; }
                }
                if (Cart.TryGetValue(7, out basepart coolerPart))
                {
                    bool coolerFits = App.db.socketprocessorcooler.Any(s => s.processorcoolerid == coolerPart.id && s.socketid == cpuSpec.socketid);
                    if (!coolerFits) { errorMessage = "Выбранный кулер не подходит к сокету этого процессора!"; return false; }
                }
            }
            else if (typeId == 4)
            {
                var mbSpec = App.db.motherboard.FirstOrDefault(m => m.id == newPart.id);
                if (Cart.TryGetValue(1, out basepart cpuPart))
                {
                    var cpuSpec = App.db.cpu.FirstOrDefault(c => c.id == cpuPart.id);
                    if (mbSpec.socketid != cpuSpec.socketid) { errorMessage = "Сокет материнской платы не совпадает с процессором!"; return false; }
                }
                if (Cart.TryGetValue(3, out basepart ramPart))
                {
                    var ramSpec = App.db.ram.FirstOrDefault(r => r.id == ramPart.id);
                    if (mbSpec.memorytypeid != ramSpec.memorytypeid) { errorMessage = "Тип оперативной памяти (DDR) не поддерживается этой материнской платой!"; return false; }
                }
                if (Cart.TryGetValue(5, out basepart casePart))
                {
                    bool fitsCase = App.db.boardformfactorcase.Any(b => b.caseid == casePart.id && b.formfactorid == mbSpec.formfactorid);
                    if (!fitsCase) { errorMessage = "Форм-фактор материнской платы не поддерживается данным корпусом!"; return false; }
                }
            }
            else if (typeId == 3)
            {
                var ramSpec = App.db.ram.FirstOrDefault(r => r.id == newPart.id);
                if (Cart.TryGetValue(4, out basepart mbPart))
                {
                    var mbSpec = App.db.motherboard.FirstOrDefault(m => m.id == mbPart.id);
                    if (ramSpec.memorytypeid != mbSpec.memorytypeid) { errorMessage = "Тип памяти не совпадает с материнской платой!"; return false; }
                }
            }
            else if (typeId == 5)
            {
                if (Cart.TryGetValue(4, out basepart mbPart))
                {
                    var mbSpec = App.db.motherboard.FirstOrDefault(m => m.id == mbPart.id);
                    bool fitsCase = App.db.boardformfactorcase.Any(b => b.caseid == newPart.id && b.formfactorid == mbSpec.formfactorid);
                    if (!fitsCase) { errorMessage = "В этот корпус не влезет выбранная материнская плата!"; return false; }
                }
            }
            else if (typeId == 2)
            {
                var gpuSpec = App.db.gpu.FirstOrDefault(g => g.id == newPart.id);
                if (gpuSpec.recommendpower != null && Cart.TryGetValue(6, out basepart psuPart))
                {
                    var psuSpec = App.db.powersupply.FirstOrDefault(p => p.id == psuPart.id);
                    if (psuSpec.power < gpuSpec.recommendpower) { errorMessage = $"Блок питания слишком слабый! Видеокарте нужно минимум {gpuSpec.recommendpower}W."; return false; }
                }
            }
            else if (typeId == 6)
            {
                var psuSpec = App.db.powersupply.FirstOrDefault(p => p.id == newPart.id);
                if (Cart.TryGetValue(2, out basepart gpuPart))
                {
                    var gpuSpec = App.db.gpu.FirstOrDefault(g => g.id == gpuPart.id);
                    if (gpuSpec.recommendpower != null && psuSpec.power < gpuSpec.recommendpower) { errorMessage = $"Этого БП не хватит для видеокарты! Нужно минимум {gpuSpec.recommendpower}W."; return false; }
                }
            }
            else if (typeId == 7)
            {
                if (Cart.TryGetValue(1, out basepart cpuPart))
                {
                    var cpuSpec = App.db.cpu.FirstOrDefault(c => c.id == cpuPart.id);
                    bool coolerFits = App.db.socketprocessorcooler.Any(s => s.processorcoolerid == newPart.id && s.socketid == cpuSpec.socketid);
                    if (!coolerFits) { errorMessage = "Этот кулер не подходит к сокету выбранного процессора!"; return false; }
                }
            }

            return true;
        }

        private void BtnSaveAssembly_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Count == 0) { MessageBox.Show("Сборка пуста!"); return; }
            if (string.IsNullOrWhiteSpace(TxtAssemblyName.Text) || TxtAssemblyName.Text == "Название сборки") { MessageBox.Show("Введите название!"); return; }
            if (string.IsNullOrWhiteSpace(TxtAssemblyAuthor.Text) || TxtAssemblyAuthor.Text == "Имя автора") { MessageBox.Show("Введите автора!"); return; }

            assembly newAssembly = new assembly
            {
                name = TxtAssemblyName.Text,
                author = TxtAssemblyAuthor.Text
            };
            App.db.assembly.Add(newAssembly);
            App.db.SaveChanges();

            foreach (var part in Cart.Values)
            {
                App.db.partassembly.Add(new partassembly
                {
                    assemblyid = newAssembly.id,
                    partid = part.id
                });
            }
            App.db.SaveChanges();

            MessageBox.Show("Сборка успешно сохранена!");
            Cart.Clear();
            UpdateCartUI();
            TxtAssemblyName.Text = "Название сборки";
            TxtAssemblyAuthor.Text = "Имя автора";
        }
    }
}
