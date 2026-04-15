using Pr_WPF.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Models
{
    internal class Goblin : Enemy
    {
        protected double CritChance = 0.20;

        public Goblin()
        {
            Name = "goblin";
            MaxHealth = 30;
            CurrentHealth = 30;
            BaseDamage = 12;
            BaseDefense = 3;
            ImageName = "goblin.jpg";
        }

        public override void PerformAttack(Player player)
        {
            bool isCrit = Rng.NextDouble() < CritChance;
            int originalDamage = BaseDamage;

            if (isCrit)
            {
                BaseDamage *= 2;
                Logger.Log($"{Name} наносит критический удар!");
            }

            base.PerformAttack(player);
            BaseDamage = originalDamage;
        }
    }
}
