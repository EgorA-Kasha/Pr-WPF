using Pr_WPF.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Models
{
    internal class VVG : Goblin
    {
        public VVG()
        {
            Name = "VVG";
            MaxHealth = (int)(30 * 2.0);
            CurrentHealth = MaxHealth;
            BaseDamage = (int)(12 * 1.5);
            BaseDefense = (int)(3 * 1.2);
            CritChance += 0.10;
            ImageName = "VVG.jpg";
        }
    }
}
