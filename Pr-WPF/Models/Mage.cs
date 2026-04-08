using Pr_WPF.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Models
{
    internal class Mage : Enemy
    {
        public Mage()
        {
            Name = "Маг";
            MaxHealth = 25;
            CurrentHealth = 25;
            BaseDamage = 15;
            BaseDefense = 2;
            ImageName = "mage.jpg";
        }

        public override void PerformAttack(Player player)
        {
            base.PerformAttack(player);

            if (!player.IsDead && Rng.NextDouble() < 0.15)
            {
                player.IsFrozen = true;
                Logger.Log($"{Name} замораживает игрока!");
            }
        }
    }
}
