using Pr_WPF.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Models
{
    internal class Skelet : Enemy
    {
        public Skelet()
        {
            Name = "skeleton";
            MaxHealth = 40;
            CurrentHealth = 40;
            BaseDamage = 10;
            BaseDefense = 5;
            ImageName = "skelet.jpg";
        }

        public override void PerformAttack(Player player)
        {
            Logger.Log($"{Name} игнорирует защиту!");
            int damage = BaseDamage;

            if (player.IsDefending)
            {
                if (Rng.NextDouble() < 0.4)
                {
                    Logger.Log($"{Name} промахнулся по игроку!");
                    return;
                }
            }

            player.TakeDamage(damage);
            Logger.Log($"{Name} наносит игроку {damage} урона.");
        }
    }
}
