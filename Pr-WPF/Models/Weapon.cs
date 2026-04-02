using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pr_WPF.Models
{
    internal class Weapon : Item
    {
        public int DamageBonus { get; private set; }

        public Weapon(int level)
        {
            Name = $"Меч {level}";
            DamageBonus = level * 2;
            ImageName = "weapon.jpg";
        }
    }
}
