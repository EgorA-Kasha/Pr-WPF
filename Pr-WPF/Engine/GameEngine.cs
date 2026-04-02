using Pr_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Engine
{
    internal class GameEngine
    {
        public Player Player { get; private set; }
        public int CurrentFloor { get; private set; }
        public List<Enemy> CurrentEnemies { get; private set; }
        public Item CurrentChestItem { get; private set; }
        public bool IsChestRoom { get; private set; }
        public bool IsGameOver { get; private set; }

        public void StartNewGame()
        {
            Player = new Player();
            CurrentFloor = 0;
            CurrentEnemies = new List<Enemy>();
            IsGameOver = false;
            Logger.Clear();
            NextFloor();
        }

        public void NextFloor()
        {
            CurrentFloor++;
            Player.IsFrozen = false;
            Player.IsDefending = false;
            CurrentEnemies.Clear();
            CurrentChestItem = null;

            Logger.Log($"--- Этаж {CurrentFloor} ---");

            if (CurrentFloor % 10 == 0)
            {
                GenerateBossRoom();
            }
            else
            {
                if (Rng.NextDouble() < 0.5) GenerateChestRoom();
                else GenerateEnemyRoom();
            }
        }

        private void GenerateEnemyRoom()
        {
            IsChestRoom = false;
            int enemyCount = Rng.Next(1, 4);
            for (int i = 0; i < enemyCount; i++)
            {
                int type = Rng.Next(0, 3);
                if (type == 0) CurrentEnemies.Add(new Goblin());
                else if (type == 1) CurrentEnemies.Add(new Skeleton());
                else CurrentEnemies.Add(new Mage());
            }
            Logger.Log($"Вы встретили врагов ({enemyCount} шт.)!");
            CheckPlayerFrozen();
        }

        private void GenerateBossRoom()
        {
            IsChestRoom = false;
            int bossType = Rng.Next(0, 4);
            if (bossType == 0) CurrentEnemies.Add(new VVG());
            else if (bossType == 1) CurrentEnemies.Add(new Kowalski());
            else if (bossType == 2) CurrentEnemies.Add(new ArchemageCPP());
            else CurrentEnemies.Add(new PestovCMM());

            Logger.Log("Битва с БОССОМ!");
            CheckPlayerFrozen();
        }

        private void GenerateChestRoom()
        {
            IsChestRoom = true;
            Logger.Log("Вы нашли сундук!");

            int itemType = Rng.Next(0, 3);
            if (itemType == 0) CurrentChestItem = new Potion();
            else if (itemType == 1) CurrentChestItem = new Weapon(CurrentFloor);
            else CurrentChestItem = new Armor(CurrentFloor);
        }

        public void PlayerAttack(Enemy target)
        {
            if (Player.IsFrozen)
            {
                Logger.Log("Вы заморожены и пропускаете ход!");
                Player.IsFrozen = false;
                EnemiesTurn();
                return;
            }

            Player.IsDefending = false;
            target.TakeDamage(Player.TotalDamage);
            Logger.Log($"Вы атаковали {target.Name} на {Player.TotalDamage} урона.");

            if (target.IsDead)
            {
                Logger.Log($"{target.Name} повержен!");
                CurrentEnemies.Remove(target);
            }

            if (CurrentEnemies.Count > 0)
            {
                EnemiesTurn();
            }
            else
            {
                NextFloor();
            }
        }

        public void PlayerDefend()
        {
            if (Player.IsFrozen)
            {
                Logger.Log("Вы заморожены и пропускаете ход!");
                Player.IsFrozen = false;
            }
            else
            {
                Player.IsDefending = true;
                Logger.Log("Вы ушли в защиту.");
            }
            EnemiesTurn();
        }

        private void EnemiesTurn()
        {
            foreach (var enemy in CurrentEnemies.ToList())
            {
                if (!Player.IsDead)
                {
                    enemy.PerformAttack(Player);
                }
            }

            if (Player.IsDead)
            {
                Logger.Log("Вы погибли...");
                IsGameOver = true;
            }
            else
            {
                CheckPlayerFrozen();
            }
        }

        private void CheckPlayerFrozen()
        {
            if (Player.IsFrozen)
            {
                Logger.Log("Вы заморожены и не можете действовать!");
                EnemiesTurn();
            }
        }

        public void UsePotion()
        {
            Player.Heal(Player.MaxHealth);
            Logger.Log("Здоровье полностью восстановлено.");
            NextFloor();
        }

        public void TakeItem()
        {
            if (CurrentChestItem is Weapon w) Player.EquippedWeapon = w;
            else if (CurrentChestItem is Armor a) Player.EquippedArmor = a;
            Logger.Log($"Вы экипировали {CurrentChestItem.Name}.");
            NextFloor();
        }

        public void DropItem()
        {
            Logger.Log($"Вы выбросили {CurrentChestItem.Name}.");
            NextFloor();
        }
    }
}
