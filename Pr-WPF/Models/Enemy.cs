using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Models
{
    internal abstract class Enemy : Entity
    {
        public virtual void PerformAttack(Player player)
        {
            int damage = BaseDamage;
            int finalDefense = player.TotalDefense;

            if (player.IsDefending)
            {
                if (Rng.NextDouble() < 0.4)
                {
                    Logger.Log($"{Name} промахнулся по игроку!");
                    return;
                }

                double blockMultiplier = Rng.NextDouble(0.7, 1.0);
                int blockedAmount = (int)(finalDefense * blockMultiplier);
                damage -= blockedAmount;
            }
            else
            {
                damage -= finalDefense;
            }

            if (damage < 0) damage = 0;
            player.TakeDamage(damage);
            Logger.Log($"{Name} наносит игроку {damage} урона.");
        }
    }
}
