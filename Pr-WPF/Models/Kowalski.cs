using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Models
{
    internal class Kowalski : Skelet
    {
        public Kowalski()
        {
            Name = "Kowalski";
            MaxHealth = (int)(40 * 2.5);
            CurrentHealth = MaxHealth;
            BaseDamage = (int)(10 * 1.3);
            BaseDefense = (int)(5 * 1.4);
            ImageName = "Kowalski.jpg";
        }
    }
}
