using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Models
{
    internal class Player : Entity
    {
        public Weapon EquippedWeapon { get; set; }
        public Armor EquippedArmor { get; set; }
        public bool IsFrozen { get; set; }
        public bool IsDefending { get; set; }

        public Player()
        {
            Name = "player";
            MaxHealth = 100;
            CurrentHealth = 100;
            BaseDamage = 5;
            BaseDefense = 2;
            ImageName = "player.jpg";
        }

        public int TotalDamage => BaseDamage + (EquippedWeapon?.DamageBonus ?? 0);
        public int TotalDefense => BaseDefense + (EquippedArmor?.DefenseBonus ?? 0);
    }
}
