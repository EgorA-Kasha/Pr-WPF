using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Models
{
    internal class ArchemageCPP : Mage
    {
        public ArchemageCPP()
        {
            Name = "ArchemageCPP";
            MaxHealth = (int)(25 * 1.8);
            CurrentHealth = MaxHealth;
            BaseDamage = (int)(15 * 1.6);
            BaseDefense = (int)(2 * 1.1);
            FreezeChance += 0.10;
            ImageName = "ArchemageCPP.jpg";
        }
    }
}
