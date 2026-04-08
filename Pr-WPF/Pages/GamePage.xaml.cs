using Pr_WPF.Engine;
using Pr_WPF.Models;
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
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private MainWindow _parent;
        private GameEngine _engine;

        public GamePage(MainWindow parent)
        {
            InitializeComponent();
            _parent = parent;
            _engine = new GameEngine();
            Logger.OnLog += UpdateLog;
            _engine.StartNewGame();
            UpdateUI();
        }

        private void UpdateLog(string message)
        {
            if (message == null)
            {
                TxtLog.Text = string.Empty;
            }
            else
            {
                TxtLog.Text += message + "\n";
                LogScroll.ScrollToEnd();
            }
        }

        private void UpdateUI()
        {
            if (_engine.IsGameOver)
            {
                _parent.NavigateTo(new GameOverPage(_parent));
                return;
            }

            TxtFloor.Text = $"Этаж № {_engine.CurrentFloor}";
            TxtHp.Text = $"Здоровье: {_engine.Player.CurrentHealth} / {_engine.Player.MaxHealth}";
            TxtWeapon.Text = $"Оружие: {(_engine.Player.EquippedWeapon != null ? _engine.Player.EquippedWeapon.Name : "Нет")} (Урон: {_engine.Player.TotalDamage})";
            TxtArmor.Text = $"Броня: {(_engine.Player.EquippedArmor != null ? _engine.Player.EquippedArmor.Name : "Нет")} (Защита: {_engine.Player.TotalDefense})";

            SetImage(ImgPlayer, _engine.Player.ImageName);

            BtnEnemy1.Visibility = Visibility.Hidden;
            BtnEnemy2.Visibility = Visibility.Hidden;
            BtnEnemy3.Visibility = Visibility.Hidden;
            PanelChest.Visibility = Visibility.Collapsed;
            BgImage.Source = null;

            BtnTake.Visibility = Visibility.Collapsed;
            BtnDrop.Visibility = Visibility.Collapsed;
            BtnContinue.Visibility = Visibility.Collapsed;
            BtnDefend.Visibility = Visibility.Collapsed;

            if (_engine.IsChestRoom)
            {
                PanelChest.Visibility = Visibility.Visible;
                SetImage(BgImage, "lootbox.jpg");
                SetImage(ImgChestItem, _engine.CurrentChestItem.ImageName);
                TxtChestItem.Text = _engine.CurrentChestItem.Name;

                if (_engine.CurrentChestItem is Potion)
                {
                    TxtChestStats.Text = "Восстанавливает все здоровье.";
                    BtnContinue.Visibility = Visibility.Visible;
                }
                else if (_engine.CurrentChestItem is Weapon w)
                {
                    TxtChestStats.Text = $"Урон: +{w.DamageBonus}\nТекущий урон от оружия: {(_engine.Player.EquippedWeapon?.DamageBonus ?? 0)}";
                    BtnTake.Visibility = Visibility.Visible;
                    BtnDrop.Visibility = Visibility.Visible;
                }
                else if (_engine.CurrentChestItem is Armor a)
                {
                    TxtChestStats.Text = $"Защита: +{a.DefenseBonus}\nТекущая защита от брони: {(_engine.Player.EquippedArmor?.DefenseBonus ?? 0)}";
                    BtnTake.Visibility = Visibility.Visible;
                    BtnDrop.Visibility = Visibility.Visible;
                }
            }
            else
            {
                BtnDefend.Visibility = Visibility.Visible;

                if (_engine.CurrentEnemies.Count > 0)
                {
                    BtnEnemy1.Visibility = Visibility.Visible;
                    SetImage(ImgEnemy1, _engine.CurrentEnemies[0].ImageName);
                    TxtEnemy1.Text = $"{_engine.CurrentEnemies[0].Name} ({_engine.CurrentEnemies[0].CurrentHealth} HP)";
                }
                if (_engine.CurrentEnemies.Count > 1)
                {
                    BtnEnemy2.Visibility = Visibility.Visible;
                    SetImage(ImgEnemy2, _engine.CurrentEnemies[1].ImageName);
                    TxtEnemy2.Text = $"{_engine.CurrentEnemies[1].Name} ({_engine.CurrentEnemies[1].CurrentHealth} HP)";
                }
                if (_engine.CurrentEnemies.Count > 2)
                {
                    BtnEnemy3.Visibility = Visibility.Visible;
                    SetImage(ImgEnemy3, _engine.CurrentEnemies[2].ImageName);
                    TxtEnemy3.Text = $"{_engine.CurrentEnemies[2].Name} ({_engine.CurrentEnemies[2].CurrentHealth} HP)";
                }
            }
        }

        private void SetImage(Image imgControl, string imageName)
        {
            try
            {
                imgControl.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/{imageName}"));
            }
            catch
            {
                imgControl.Source = null;
            }
        }

        private void BtnEnemy1_Click(object sender, RoutedEventArgs e)
        {
            _engine.PlayerAttack(_engine.CurrentEnemies[0]);
            UpdateUI();
        }

        private void BtnEnemy2_Click(object sender, RoutedEventArgs e)
        {
            _engine.PlayerAttack(_engine.CurrentEnemies[1]);
            UpdateUI();
        }

        private void BtnEnemy3_Click(object sender, RoutedEventArgs e)
        {
            _engine.PlayerAttack(_engine.CurrentEnemies[2]);
            UpdateUI();
        }

        private void BtnDefend_Click(object sender, RoutedEventArgs e)
        {
            _engine.PlayerDefend();
            UpdateUI();
        }

        private void BtnTake_Click(object sender, RoutedEventArgs e)
        {
            _engine.TakeItem();
            UpdateUI();
        }

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            _engine.UsePotion();
            UpdateUI();
        }

        private void BtnDrop_Click(object sender, RoutedEventArgs e)
        {
            _engine.DropItem();
            UpdateUI();
        }
    }
}
