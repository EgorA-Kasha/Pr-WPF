using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Models
{
    internal class Entity
    {
        public string Name { get; protected set; }
        public int MaxHealth { get; protected set; }
        public int CurrentHealth { get; set; }
        public int BaseDamage { get; protected set; }
        public int BaseDefense { get; protected set; }
        public string ImageName { get; protected set; }

        public bool IsDead => CurrentHealth <= 0;

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth < 0) CurrentHealth = 0;
        }

        public void Heal(int amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
        }
    }
}
