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
    /// Логика взаимодействия для Assembled.xaml
    /// </summary>
    public partial class Assembled : Page
    {
        public Assembled()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Берем данные напрямую из базы (AsNoTracking), чтобы всегда видеть новые сборки,
            // при этом НЕ ПЕРЕЗАПИСЫВАЯ глобальный App.db
            LvAssemblies.ItemsSource = App.db.assembly.ToList();
        }

        private void LvAssemblies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvAssemblies.SelectedItem is assembly selectedAssembly)
            {
                // Явно подгружаем связанные таблицы (комплектующие и их производителей)
                var parts = App.db.partassembly
                                  .Include("basepart")
                                  .Include("basepart.manufacturer")
                                  .Where(pa => pa.assemblyid == selectedAssembly.id)
                                  .Select(pa => pa.basepart)
                                  .ToList();

                LvAssemblyParts.ItemsSource = parts;
                TxtTotal.Text = $"Итоговая цена: {parts.Sum(p => p.price):N0} ₽";
            }
        }
    }
}
